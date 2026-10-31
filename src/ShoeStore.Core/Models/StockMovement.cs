using ShoeStore.Core.Enums;
using ShoeStore.Core.Validation;

namespace ShoeStore.Core.Models;

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
