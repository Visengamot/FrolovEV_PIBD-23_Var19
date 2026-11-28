using Serilog;
using ShoeStore.WinForms.Entities.Enums;
using ShoeStore.WinForms.Forms;
using ShoeStore.WinForms.Reports.Builders;
using ShoeStore.WinForms.Repositories;

namespace ShoeStore.WinForms.Reports;

public sealed class SalesChartReport(
    IStockMovementRepository movements,
    IShoeRepository shoes,
    ILogger logger)
{
    public void Save(string path, DateTime date) => Save(path, date, date);

    public void Save(string path, DateTime from, DateTime to)
    {
        from = from.Date;
        to = to.Date;
        logger.Information("Отчёт PDF: период с {From:dd.MM.yyyy} по {To:dd.MM.yyyy}", from, to);

        try
        {
            if (from > to)
                throw new ArgumentException("Начало периода не может быть позже конца.");

            var categories = shoes.GetAll().ToDictionary(x => x.Id, x => x.Category);
            var sales = movements.GetAll()
                .Where(x => x.Type == MovementType.Sale && x.Date.Date >= from && x.Date.Date <= to)
                .SelectMany(x => x.Items)
                .GroupBy(x => categories[x.ShoeId])
                .Select(group => (Category: Ui.Category(group.Key), Quantity: group.Sum(x => (long)x.Quantity)))
                .OrderByDescending(x => x.Quantity)
                .ThenBy(x => x.Category)
                .ToList();

            var document = new PdfBuilder();
            document.AddHeading("Продажи обуви по категориям");
            document.AddText(from == to
                ? $"Дата продаж: {from:dd.MM.yyyy}"
                : $"Период продаж: {from:dd.MM.yyyy} — {to:dd.MM.yyyy} (включительно)");
            if (sales.Count == 0)
            {
                document.AddText(from == to
                    ? "За выбранную дату продаж нет. Диаграмма не строится."
                    : "За выбранный период продаж нет. Диаграмма не строится.");
            }
            else
            {
                var total = sales.Sum(x => x.Quantity);
                document.AddText($"Всего продано пар: {total}");
                document.AddPieChart(
                    sales.Select(x => x.Category).ToArray(),
                    sales.Select(x => (double)x.Quantity).ToArray());
                foreach (var sale in sales)
                    document.AddText($"{sale.Category}: {sale.Quantity} пар ({(double)sale.Quantity / total:P1})");
            }

            document.Save(path);
            logger.Information("Отчёт PDF сохранён: {Path}, категорий: {Count}", path, sales.Count);
        }
        catch (Exception error)
        {
            logger.Error(error, "Не удалось сформировать отчёт PDF: {Path}", path);
            throw;
        }
    }
}
