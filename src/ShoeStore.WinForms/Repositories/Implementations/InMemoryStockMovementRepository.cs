using ShoeStore.WinForms.Entities.Enums;
using ShoeStore.WinForms.Entities;
using ShoeStore.WinForms.Repositories;
using ShoeStore.WinForms.Repositories.Implementations;

namespace ShoeStore.WinForms.Repositories.Implementations;

public sealed class InMemoryStockMovementRepository(InMemoryStore store) : IStockMovementRepository
{
    private readonly InMemoryStore _store = store ?? throw new ArgumentNullException(nameof(store));
    public IReadOnlyList<StockMovement> GetAll() =>
        _store.Movements.Values.OrderByDescending(x => x.Date).ToList();
    public StockMovement? GetById(Guid id) =>
        _store.Movements.GetValueOrDefault(id);
    public void Add(StockMovement entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (_store.Movements.ContainsKey(entity.Id))
            throw new InvalidOperationException("Операция уже существует.");
        var existingIds = _store.Movements.Values.SelectMany(x => x.Items).Select(x => x.Id).ToHashSet();
        foreach (var item in entity.Items)
        {
            if (!_store.Shoes.ContainsKey(item.ShoeId) || !_store.Sizes.ContainsKey(item.ShoeSizeId))
                throw new InvalidOperationException("Товар или размер не найден.");
            if (existingIds.Contains(item.Id))
                throw new InvalidOperationException("Идентификатор позиции уже используется.");
            // Revalidate the complete chronological ledger, including backdated entries.
            long balance = 0;
            foreach (var movement in _store.Movements.Values.Append(entity).OrderBy(x => x.Date).ThenBy(x => x.Type))
            {
                balance += movement.Items.Where(
                    x => x.ShoeId == item.ShoeId
                    && x.ShoeSizeId == item.ShoeSizeId
                )
                    .Sum(
                    x => (long)x.Quantity
                ) * (movement.Type == MovementType.Receipt
                    ? 1
                    : -1);
                if (balance < 0)
                    throw new InvalidOperationException(
                        "Недостаточно товара: продажа не может предшествовать поступлению или создавать отрицательный остаток."
                    );
                if (balance > int.MaxValue)
                    throw new InvalidOperationException("Превышен допустимый складской остаток.");
            }
        }

        _store.Movements.Add(entity.Id, entity);
    }
}
