using ShoeStore.Core.Validation;

namespace ShoeStore.Core.Models;

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
