using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShoeStore.WinForms.Entities.Enums;
using ShoeStore.WinForms.Entities;

namespace ShoeStore.WinForms.Entities;

public sealed class Shoe
{
    [Key]
    public Guid Id { get; private set; }
    [Required, MaxLength(100)]
    public string Article { get; private set; } = string.Empty;
    [Required, MaxLength(200)]
    public string Name { get; private set; } = string.Empty;
    public Guid ManufacturerId { get; private set; }
    [Column(TypeName = "numeric(18,2)")]
    public decimal Price { get; private set; }
    [ForeignKey(nameof(ManufacturerId))]
    public Manufacturer Manufacturer { get; private set; } = null!;
    public FootwearCategory Category { get; private set; }
    public Season Seasons { get; private set; }
    public Shoe() { }
    public static Shoe Create(Guid id, string article, string name, Guid manufacturerId, decimal price, FootwearCategory category, Season seasons)
    {
        if (!Enum.IsDefined(category)) throw new ArgumentOutOfRangeException(nameof(category));
        if (seasons == Season.None || (seasons & ~Season.All) != 0) throw new ArgumentOutOfRangeException(nameof(seasons), "Выберите хотя бы один сезон.");
        return new Shoe { Id = Guard.Id(id, nameof(id)), Article = Guard.Text(article, nameof(article)), Name = Guard.Text(name, nameof(name)), ManufacturerId = Guard.Id(manufacturerId, nameof(manufacturerId)), Price = Guard.Positive(price, nameof(price)), Category = category, Seasons = seasons };
    }
}
