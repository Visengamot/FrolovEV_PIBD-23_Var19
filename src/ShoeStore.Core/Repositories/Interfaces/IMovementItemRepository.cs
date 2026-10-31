using ShoeStore.Core.Models;

namespace ShoeStore.Core.Repositories.Interfaces;

public interface IMovementItemRepository
{
    IReadOnlyList<MovementItem> GetAll();
    MovementItem? GetById(Guid id);
}
