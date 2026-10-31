using ShoeStore.Core.Models;

namespace ShoeStore.Core.Repositories.Interfaces;

public interface IStockMovementRepository
{
    IReadOnlyList<StockMovement> GetAll();
    StockMovement? GetById(Guid id);
    void Add(StockMovement entity);
}
