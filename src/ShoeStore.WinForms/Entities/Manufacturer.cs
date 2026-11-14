using ShoeStore.WinForms.Entities;

namespace ShoeStore.WinForms.Entities;

public sealed class Manufacturer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;

    private Manufacturer()
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
