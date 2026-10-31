using ShoeStore.Core.Enums;
using ShoeStore.Core.Models;

namespace ShoeStore.Core.Storage;

/// <summary>Общее хранилище данных на время работы приложения.</summary>
public sealed class InMemoryStore
{
    internal Dictionary<Guid, Manufacturer> Manufacturers { get; } = new();
    internal Dictionary<Guid, Shoe> Shoes { get; } = new();
    internal Dictionary<Guid, ShoeSize> Sizes { get; } = new();
    internal Dictionary<Guid, StockMovement> Movements { get; } = new();

    public InMemoryStore(bool seedData = true)
    {
        if (!seedData) return;
        var maker = Manufacturer.Create(Guid.NewGuid(), "Волжская обувная фабрика", "Россия");
        Manufacturers.Add(maker.Id, maker);
        var shoe = Shoe.Create(Guid.NewGuid(), "КР-001", "Кроссовки городские", maker.Id, 3900m, FootwearCategory.Sneakers, Season.Spring | Season.Autumn);
        var boots = Shoe.Create(Guid.NewGuid(), "БТ-002", "Ботинки зимние", maker.Id, 6500m, FootwearCategory.Boots, Season.Winter);
        Shoes.Add(shoe.Id, shoe);
        Shoes.Add(boots.Id, boots);
        var size = ShoeSize.Create(Guid.NewGuid(), 40m);
        var size2 = ShoeSize.Create(Guid.NewGuid(), 41m);
        Sizes.Add(size.Id, size);
        Sizes.Add(size2.Id, size2);
        var receipts = Guid.NewGuid();
        var receipt = StockMovement.Create(receipts, DateTime.Today.AddDays(-10), MovementType.Receipt, new[]
        {
            MovementItem.Create(Guid.NewGuid(), receipts, shoe.Id, size.Id, 12, 2500m),
            MovementItem.Create(Guid.NewGuid(), receipts, boots.Id, size2.Id, 8, 4200m)
        });
        Movements.Add(receipt.Id, receipt);
        var saleId = Guid.NewGuid();
        var sale = StockMovement.Create(saleId, DateTime.Today, MovementType.Sale, new[] { MovementItem.Create(Guid.NewGuid(), saleId, shoe.Id, size.Id, 2, shoe.Price) });
        Movements.Add(sale.Id, sale);
    }
}
