using Serilog;
using ShoeStore.WinForms.Entities.Enums;
using ShoeStore.WinForms.Reports.Builders;
using ShoeStore.WinForms.Repositories;

namespace ShoeStore.WinForms.Reports;

public sealed class MovementReport(
    IStockMovementRepository movements,
    IShoeRepository shoes,
    IShoeSizeRepository sizes,
    ILogger logger)
{
    public void Save(string path, DateTime from, DateTime to)
    {
        from = from.Date;
        to = to.Date;
        logger.Information("Отчёт Excel: период с {From:dd.MM.yyyy} по {To:dd.MM.yyyy}", from, to);

        try
        {
            if (from > to)
                throw new ArgumentException("Дата начала не может быть позже даты окончания.");

            var shoeById = shoes.GetAll().ToDictionary(x => x.Id);
            var sizeById = sizes.GetAll().ToDictionary(x => x.Id);
            var entries = movements.GetAll()
                .Where(x => x.Date.Date >= from && x.Date.Date <= to)
                .SelectMany(x => x.Items.Select(item => new { x.Type, Item = item }));

            var summary = entries.GroupBy(x => (x.Item.ShoeId, x.Item.ShoeSizeId))
                .Select(group =>
                {
                    var shoe = shoeById[group.Key.ShoeId];
                    var received = group.Where(x => x.Type == MovementType.Receipt).ToList();
                    var sold = group.Where(x => x.Type == MovementType.Sale).ToList();
                    return new
                    {
                        shoe.Article,
                        shoe.Name,
                        Size = sizeById[group.Key.ShoeSizeId].Value,
                        Received = received.Sum(x => (long)x.Item.Quantity),
                        ReceiptCost = received.Sum(x => x.Item.Total),
                        Sold = sold.Sum(x => (long)x.Item.Quantity),
                        Revenue = sold.Sum(x => x.Item.Total)
                    };
                })
                .OrderBy(x => x.Article)
                .ThenBy(x => x.Size)
                .ToList();

            var rows = summary.Select(x => (IReadOnlyList<object>)
                [x.Article, x.Name, x.Size, x.Received, x.ReceiptCost, x.Sold, x.Revenue]).ToList();
            var workbook = new ExcelBuilder();
            workbook.AddHeading("Поступления и продажи обуви");
            workbook.AddText($"Период: {from:dd.MM.yyyy} — {to:dd.MM.yyyy} (включительно)");
            if (rows.Count == 0)
                workbook.AddText("За этот период операций нет.");

            workbook.AddTable(
                ["Артикул", "Обувь", "Размер", "Поступило, пар", "Сумма поступлений, руб.",
                    "Продано, пар", "Выручка, руб."],
                rows,
                ["Итого", "", "", summary.Sum(x => x.Received), summary.Sum(x => x.ReceiptCost),
                    summary.Sum(x => x.Sold), summary.Sum(x => x.Revenue)]);
            workbook.Save(path);
            logger.Information("Отчёт Excel сохранён: {Path}, строк: {Rows}", path, rows.Count);
        }
        catch (Exception error)
        {
            logger.Error(error, "Не удалось сформировать отчёт Excel: {Path}", path);
            throw;
        }
    }
}
