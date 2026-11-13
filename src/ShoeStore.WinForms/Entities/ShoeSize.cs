using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShoeStore.WinForms.Entities;

namespace ShoeStore.WinForms.Entities;

public sealed class ShoeSize
{
    [Key]
    public Guid Id { get; private set; }
    [Column(TypeName = "numeric(4,1)")]
    public decimal Value { get; private set; }
    public ShoeSize() { }
    public static ShoeSize Create(Guid id, decimal value)
    {
        if (value < 10 || value > 60 || value * 2 != decimal.Truncate(value * 2)) throw new ArgumentOutOfRangeException(nameof(value), "Размер должен быть от 10 до 60 с шагом 0,5.");
        return new ShoeSize { Id = Guard.Id(id, nameof(id)), Value = value };
    }
}
