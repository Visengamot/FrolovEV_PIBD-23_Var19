using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShoeStore.WinForms.Entities;

namespace ShoeStore.WinForms.Entities;

public sealed class Manufacturer
{
    [Key]
    public Guid Id { get; private set; }

    [Required, MaxLength(200)]
    public string Name { get; private set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Country { get; private set; } = string.Empty;

    public Manufacturer()
    {
    }

    public static Manufacturer Create(Guid id, string name, string country) =>
        new()
        {
            Id = Guard.Id(id, nameof(id)),
            Name = Guard.Text(name, nameof(name)),
            Country = Guard.Text(country, nameof(country))
        };
}
