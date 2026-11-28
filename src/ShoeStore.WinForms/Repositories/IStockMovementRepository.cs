using ShoeStore.WinForms.Entities;

namespace ShoeStore.WinForms.Repositories;

public interface IStockMovementRepository
{
    IReadOnlyList<StockMovement> GetAll();

    StockMovement? GetById(Guid id);

    void Add(StockMovement entity);
}
