using ShoeStore.Core.Models;

namespace ShoeStore.Core.Repositories;

/// <summary>Session-local shared data store; register as a singleton in the IoC container.</summary>
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

public sealed class InMemoryManufacturerRepository(InMemoryStore store) : IManufacturerRepository
{
    private readonly InMemoryStore _store = store ?? throw new ArgumentNullException(nameof(store));
    public IReadOnlyList<Manufacturer> GetAll() => _store.Manufacturers.Values.OrderBy(x => x.Name).ToList();
    public Manufacturer? GetById(Guid id) => _store.Manufacturers.GetValueOrDefault(id);
    public void Add(Manufacturer entity) { Validate(entity); if (!_store.Manufacturers.TryAdd(entity.Id, entity)) throw new InvalidOperationException("Производитель уже существует."); }
    public void Update(Manufacturer entity) { Validate(entity); if (!_store.Manufacturers.ContainsKey(entity.Id)) throw new InvalidOperationException("Производитель не найден."); _store.Manufacturers[entity.Id] = entity; }
    public void Delete(Guid id)
    {
        if (_store.Shoes.Values.Any(x => x.ManufacturerId == id)) throw new InvalidOperationException("Нельзя удалить производителя, используемого в ассортименте.");
        if (!_store.Manufacturers.Remove(id)) throw new InvalidOperationException("Производитель не найден.");
    }
    private void Validate(Manufacturer entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (_store.Manufacturers.Values.Any(x => x.Id != entity.Id && string.Equals(x.Name, entity.Name, StringComparison.OrdinalIgnoreCase))) throw new InvalidOperationException("Производитель с таким названием уже существует.");
    }
}

public sealed class InMemoryShoeRepository(InMemoryStore store) : IShoeRepository
{
    private readonly InMemoryStore _store = store ?? throw new ArgumentNullException(nameof(store));
    public IReadOnlyList<Shoe> GetAll() => _store.Shoes.Values.OrderBy(x => x.Article).ToList();
    public Shoe? GetById(Guid id) => _store.Shoes.GetValueOrDefault(id);
    public void Add(Shoe entity) { Validate(entity); if (!_store.Shoes.TryAdd(entity.Id, entity)) throw new InvalidOperationException("Товар уже существует."); }
    public void Update(Shoe entity) { Validate(entity); if (!_store.Shoes.ContainsKey(entity.Id)) throw new InvalidOperationException("Товар не найден."); _store.Shoes[entity.Id] = entity; }
    public void Delete(Guid id)
    {
        if (_store.Movements.Values.SelectMany(x => x.Items).Any(x => x.ShoeId == id)) throw new InvalidOperationException("Нельзя удалить товар с историей движения.");
        if (!_store.Shoes.Remove(id)) throw new InvalidOperationException("Товар не найден.");
    }
    private void Validate(Shoe entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (!_store.Manufacturers.ContainsKey(entity.ManufacturerId)) throw new InvalidOperationException("Производитель не найден.");
        if (_store.Shoes.Values.Any(x => x.Id != entity.Id && string.Equals(x.Article, entity.Article, StringComparison.OrdinalIgnoreCase))) throw new InvalidOperationException("Артикул должен быть уникальным.");
    }
}

public sealed class InMemoryShoeSizeRepository(InMemoryStore store) : IShoeSizeRepository
{
    private readonly InMemoryStore _store = store ?? throw new ArgumentNullException(nameof(store));
    public IReadOnlyList<ShoeSize> GetAll() => _store.Sizes.Values.OrderBy(x => x.Value).ToList();
    public ShoeSize? GetById(Guid id) => _store.Sizes.GetValueOrDefault(id);
    public void Add(ShoeSize entity) { Validate(entity); if (!_store.Sizes.TryAdd(entity.Id, entity)) throw new InvalidOperationException("Размер уже существует."); }
    public void Update(ShoeSize entity) { Validate(entity); if (!_store.Sizes.ContainsKey(entity.Id)) throw new InvalidOperationException("Размер не найден."); _store.Sizes[entity.Id] = entity; }
    public void Delete(Guid id)
    {
        if (_store.Movements.Values.SelectMany(x => x.Items).Any(x => x.ShoeSizeId == id)) throw new InvalidOperationException("Нельзя удалить размер с историей движения.");
        if (!_store.Sizes.Remove(id)) throw new InvalidOperationException("Размер не найден.");
    }
    private void Validate(ShoeSize entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (_store.Sizes.Values.Any(x => x.Id != entity.Id && x.Value == entity.Value)) throw new InvalidOperationException("Такой размер уже существует.");
        if (_store.Sizes.TryGetValue(entity.Id, out var existing) && existing.Value != entity.Value && _store.Movements.Values.SelectMany(x => x.Items).Any(x => x.ShoeSizeId == entity.Id)) throw new InvalidOperationException("Нельзя менять размер, используемый в истории движения.");
    }
}

public sealed class InMemoryStockMovementRepository(InMemoryStore store) : IStockMovementRepository
{
    private readonly InMemoryStore _store = store ?? throw new ArgumentNullException(nameof(store));
    public IReadOnlyList<StockMovement> GetAll() => _store.Movements.Values.OrderByDescending(x => x.Date).ToList();
    public StockMovement? GetById(Guid id) => _store.Movements.GetValueOrDefault(id);
    public void Add(StockMovement entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (_store.Movements.ContainsKey(entity.Id)) throw new InvalidOperationException("Операция уже существует.");
        var existingIds = _store.Movements.Values.SelectMany(x => x.Items).Select(x => x.Id).ToHashSet();
        foreach (var item in entity.Items)
        {
            if (!_store.Shoes.ContainsKey(item.ShoeId) || !_store.Sizes.ContainsKey(item.ShoeSizeId)) throw new InvalidOperationException("Товар или размер не найден.");
            if (existingIds.Contains(item.Id)) throw new InvalidOperationException("Идентификатор позиции уже используется.");
            // Revalidate the complete chronological ledger, including backdated entries.
            long balance = 0;
            foreach (var movement in _store.Movements.Values.Append(entity).OrderBy(x => x.Date).ThenBy(x => x.Type))
            {
                balance += movement.Items.Where(x => x.ShoeId == item.ShoeId && x.ShoeSizeId == item.ShoeSizeId).Sum(x => (long)x.Quantity) * (movement.Type == MovementType.Receipt ? 1 : -1);
                if (balance < 0) throw new InvalidOperationException("Недостаточно товара: продажа не может предшествовать поступлению или создавать отрицательный остаток.");
                if (balance > int.MaxValue) throw new InvalidOperationException("Превышен допустимый складской остаток.");
            }
        }
        _store.Movements.Add(entity.Id, entity);
    }
}

public sealed class InMemoryMovementItemRepository(InMemoryStore store) : IMovementItemRepository
{
    private readonly InMemoryStore _store = store ?? throw new ArgumentNullException(nameof(store));
    public IReadOnlyList<MovementItem> GetAll() => _store.Movements.Values.SelectMany(x => x.Items).ToList();
    public MovementItem? GetById(Guid id) => GetAll().FirstOrDefault(x => x.Id == id);
}
