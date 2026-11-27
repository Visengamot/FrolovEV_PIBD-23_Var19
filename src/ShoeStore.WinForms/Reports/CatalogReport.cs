using Serilog;
using ShoeStore.WinForms.Forms;
using ShoeStore.WinForms.Reports.Builders;
using ShoeStore.WinForms.Repositories;

namespace ShoeStore.WinForms.Reports;

public sealed class CatalogReport(
    IManufacturerRepository manufacturers,
    IShoeRepository shoes,
    IShoeSizeRepository sizes,
    ILogger logger)
{
    public void Save(string path, bool includeManufacturers, bool includeShoes, bool includeSizes)
    {
        logger.Information(
            "Отчёт Word: производители={Manufacturers}, обувь={Shoes}, размеры={Sizes}",
            includeManufacturers, includeShoes, includeSizes);

        try
        {
            if (!includeManufacturers && !includeShoes && !includeSizes)
                throw new ArgumentException("Выберите хотя бы один справочник.");

            var document = new WordBuilder();
            document.AddHeading("Справочники магазина обуви");
            document.AddText($"Дата формирования: {DateTime.Today:dd.MM.yyyy}");

            var manufacturerList = includeManufacturers || includeShoes ? manufacturers.GetAll() : [];
            if (includeManufacturers)
            {
                document.AddHeading("Производители");
                var rows = manufacturerList.OrderBy(x => x.Name)
                    .Select(x => (IReadOnlyList<string>)[x.Name, x.Country]);
                document.AddTable(WithHeader(["Название", "Страна"], rows), [5600, 3800]);
            }

            if (includeShoes)
            {
                document.AddHeading("Ассортимент обуви");
                var names = manufacturerList.ToDictionary(x => x.Id, x => x.Name);
                var rows = shoes.GetAll().OrderBy(x => x.Article)
                    .Select(x => (IReadOnlyList<string>)
                    [
                        x.Article, x.Name, names.GetValueOrDefault(x.ManufacturerId, "—"),
                        Ui.Category(x.Category), Ui.Seasons(x.Seasons), $"{x.Price:N2}"
                    ]);
                var headers = new[] { "Артикул", "Название", "Производитель", "Категория", "Сезоны", "Цена, руб." };
                document.AddTable(WithHeader(headers, rows), [1100, 2200, 1900, 1400, 1500, 1300]);
            }

            if (includeSizes)
            {
                document.AddHeading("Размеры");
                var rows = sizes.GetAll().OrderBy(x => x.Value)
                    .Select(x => (IReadOnlyList<string>)[$"{x.Value:0.#}"]);
                document.AddTable(WithHeader(["Размер"], rows), [2200]);
            }

            document.Save(path);
            logger.Information("Отчёт Word сохранён: {Path}", path);
        }
        catch (Exception error)
        {
            logger.Error(error, "Не удалось сформировать отчёт Word: {Path}", path);
            throw;
        }
    }

    private static IReadOnlyList<IReadOnlyList<string>> WithHeader(
        IReadOnlyList<string> header, IEnumerable<IReadOnlyList<string>> rows) =>
        rows.Prepend(header).ToList();
}
