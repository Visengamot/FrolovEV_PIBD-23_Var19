namespace ShoeStore.Core.Models;

public enum FootwearCategory { Sneakers, Boots, Shoes, Sandals, Slippers }
[Flags]
public enum Season { None = 0, Spring = 1, Summer = 2, Autumn = 4, Winter = 8, All = 15 }
public enum MovementType { Receipt, Sale }

internal static class Guard
{
    public static Guid Id(Guid value, string name) => value != Guid.Empty ? value : throw new ArgumentException("Идентификатор не может быть пустым.", name);
    public static string Text(string? value, string name) => !string.IsNullOrWhiteSpace(value) ? value.Trim() : throw new ArgumentException("Обязательное поле не заполнено.", name);
    public static decimal Positive(decimal value, string name) => value > 0 ? value : throw new ArgumentOutOfRangeException(name, "Значение должно быть положительным.");
}

public sealed class Manufacturer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    private Manufacturer() { }
    public static Manufacturer Create(Guid id, string name, string country) => new()
    {
        Id = Guard.Id(id, nameof(id)),
        Name = Guard.Text(name, nameof(name)),
        Country = Guard.Text(country, nameof(country))
    };
}

public sealed class Shoe
{
    public Guid Id { get; private set; }
    public string Article { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public Guid ManufacturerId { get; private set; }
    public decimal Price { get; private set; }
    public FootwearCategory Category { get; private set; }
    public Season Seasons { get; private set; }
    private Shoe() { }
    public static Shoe Create(Guid id, string article, string name, Guid manufacturerId, decimal price, FootwearCategory category, Season seasons)
    {
        if (!Enum.IsDefined(category)) throw new ArgumentOutOfRangeException(nameof(category));
        if (seasons == Season.None || (seasons & ~Season.All) != 0) throw new ArgumentOutOfRangeException(nameof(seasons), "Выберите хотя бы один сезон.");
        return new Shoe { Id = Guard.Id(id, nameof(id)), Article = Guard.Text(article, nameof(article)), Name = Guard.Text(name, nameof(name)), ManufacturerId = Guard.Id(manufacturerId, nameof(manufacturerId)), Price = Guard.Positive(price, nameof(price)), Category = category, Seasons = seasons };
    }
}

public sealed class ShoeSize
{
    public Guid Id { get; private set; }
    public decimal Value { get; private set; }
    private ShoeSize() { }
    public static ShoeSize Create(Guid id, decimal value)
    {
        if (value < 10 || value > 60 || value * 2 != decimal.Truncate(value * 2)) throw new ArgumentOutOfRangeException(nameof(value), "Размер должен быть от 10 до 60 с шагом 0,5.");
        return new ShoeSize { Id = Guard.Id(id, nameof(id)), Value = value };
    }
}

public sealed class MovementItem
{
    public Guid Id { get; private set; }
    public Guid MovementId { get; private set; }
    public Guid ShoeId { get; private set; }
    public Guid ShoeSizeId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Total => Quantity * UnitPrice;
    private MovementItem() { }
    public static MovementItem Create(Guid id, Guid movementId, Guid shoeId, Guid shoeSizeId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Количество пар должно быть положительным.");
        return new MovementItem { Id = Guard.Id(id, nameof(id)), MovementId = Guard.Id(movementId, nameof(movementId)), ShoeId = Guard.Id(shoeId, nameof(shoeId)), ShoeSizeId = Guard.Id(shoeSizeId, nameof(shoeSizeId)), Quantity = quantity, UnitPrice = Guard.Positive(unitPrice, nameof(unitPrice)) };
    }
}

public sealed class StockMovement
{
    public Guid Id { get; private set; }
    public DateTime Date { get; private set; }
    public MovementType Type { get; private set; }
    public IReadOnlyList<MovementItem> Items { get; private set; } = Array.Empty<MovementItem>();
    public decimal Total => Items.Sum(x => x.Total);
    private StockMovement() { }
    public static StockMovement Create(Guid id, DateTime date, MovementType type, IEnumerable<MovementItem> items)
    {
        Guard.Id(id, nameof(id));
        ArgumentNullException.ThrowIfNull(items);
        if (!Enum.IsDefined(type)) throw new ArgumentOutOfRangeException(nameof(type));
        if (date == default || date.Date > DateTime.Today) throw new ArgumentOutOfRangeException(nameof(date), "Дата должна быть заполнена и не может быть в будущем.");
        var list = items.ToList();
        if (list.Count == 0 || list.Any(x => x is null)) throw new ArgumentException("Операция должна содержать позиции.", nameof(items));
        if (list.Any(x => x.MovementId != id)) throw new ArgumentException("Позиция относится к другой операции.", nameof(items));
        if (list.Select(x => x.Id).Distinct().Count() != list.Count || list.Select(x => (x.ShoeId, x.ShoeSizeId)).Distinct().Count() != list.Count) throw new ArgumentException("Повторяющиеся позиции операции.", nameof(items));
        return new StockMovement { Id = id, Date = date.Date, Type = type, Items = list.AsReadOnly() };
    }
}

public sealed record StockRow(Guid ShoeId, string Article, string ShoeName, string ManufacturerName, Guid ShoeSizeId, decimal Size, int Balance, int CurrentMonthSold, decimal CurrentMonthRevenue);
