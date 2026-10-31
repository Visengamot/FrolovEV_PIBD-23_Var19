using ShoeStore.WinForms.Entities;
using ShoeStore.WinForms.Repositories;
using ShoeStore.WinForms.Repositories.Implementations;

namespace ShoeStore.WinForms.Repositories.Implementations;

public sealed class InMemoryMovementItemRepository(InMemoryStore store) : IMovementItemRepository
{
    private readonly InMemoryStore _store = store ?? throw new ArgumentNullException(nameof(store));
    public IReadOnlyList<MovementItem> GetAll() => _store.Movements.Values.SelectMany(x => x.Items).ToList();
    public MovementItem? GetById(Guid id) => GetAll().FirstOrDefault(x => x.Id == id);
}
