using ShoeStore.Core.Enums;
using ShoeStore.Core.Validation;

namespace ShoeStore.Core.Models;

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
