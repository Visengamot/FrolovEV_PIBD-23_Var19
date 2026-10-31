using ShoeStore.Core.Enums;
using ShoeStore.Core.Models;
using ShoeStore.Core.Repositories.Interfaces;

namespace ShoeStore.Core.Services;

public sealed class StockReportService
{
    private readonly IManufacturerRepository _manufacturers;
    private readonly IShoeRepository _shoes;
    private readonly IShoeSizeRepository _sizes;
    private readonly IStockMovementRepository _movements;

    public StockReportService(IManufacturerRepository manufacturers, IShoeRepository shoes, IShoeSizeRepository sizes, IStockMovementRepository movements)
    {
        _manufacturers = manufacturers ?? throw new ArgumentNullException(nameof(manufacturers));
        _shoes = shoes ?? throw new ArgumentNullException(nameof(shoes));
        _sizes = sizes ?? throw new ArgumentNullException(nameof(sizes));
        _movements = movements ?? throw new ArgumentNullException(nameof(movements));
    }

    public IReadOnlyList<StockRow> GetStockReport(DateTime? asOf = null)
    {
        var date = (asOf ?? DateTime.Today).Date;
        var monthStart = new DateTime(date.Year, date.Month, 1);
        var shoes = _shoes.GetAll().ToDictionary(x => x.Id);
        var sizes = _sizes.GetAll().ToDictionary(x => x.Id);
        var manufacturers = _manufacturers.GetAll().ToDictionary(x => x.Id);
        var entries = _movements.GetAll().Where(x => x.Date <= date)
            .SelectMany(m => m.Items.Select(item => new { Movement = m, Item = item }));
        return entries.GroupBy(x => (x.Item.ShoeId, x.Item.ShoeSizeId)).Select(group =>
        {
            var shoe = shoes[group.Key.ShoeId];
            var sales = group.Where(x => x.Movement.Type == MovementType.Sale && x.Movement.Date >= monthStart).ToList();
            return new StockRow(shoe.Id, shoe.Article, shoe.Name, manufacturers[shoe.ManufacturerId].Name,
                group.Key.ShoeSizeId, sizes[group.Key.ShoeSizeId].Value,
                group.Sum(x => x.Item.Quantity * (x.Movement.Type == MovementType.Receipt ? 1 : -1)),
                sales.Sum(x => x.Item.Quantity), sales.Sum(x => x.Item.Total));
        }).OrderBy(x => x.Article).ThenBy(x => x.Size).ToList();
    }
}
