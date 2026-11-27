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
    public void Save(string path, DateTime date)
    {
        date = date.Date;
        logger.Information("Отчёт PDF: дата продаж {Date:dd.MM.yyyy}", date);

        try
        {
            var categories = shoes.GetAll().ToDictionary(x => x.Id, x => x.Category);
            var sales = movements.GetAll()
                .Where(x => x.Date.Date == date && x.Type == MovementType.Sale)
                .SelectMany(x => x.Items)
                .GroupBy(x => categories[x.ShoeId])
                .Select(group => (Category: Ui.Category(group.Key), Quantity: group.Sum(x => (long)x.Quantity)))
                .OrderByDescending(x => x.Quantity)
                .ThenBy(x => x.Category)
                .ToList();

            var document = new PdfBuilder();
            document.AddHeading("Продажи обуви по категориям");
            document.AddText($"Дата продаж: {date:dd.MM.yyyy}");
            if (sales.Count == 0)
            {
                document.AddText("За выбранную дату продаж нет. Диаграмма не строится.");
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
