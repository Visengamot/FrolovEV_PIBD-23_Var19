using ShoeStore.WinForms.Entities;

namespace ShoeStore.WinForms.Repositories;

public interface IMovementItemRepository
{
    IReadOnlyList<MovementItem> GetAll();

    MovementItem? GetById(Guid id);
}
