using ShoeStore.Core.Models;

namespace ShoeStore.Core.Repositories;

public interface IManufacturerRepository
{
    IReadOnlyList<Manufacturer> GetAll();
    Manufacturer? GetById(Guid id);
    void Add(Manufacturer entity);
    void Update(Manufacturer entity);
    void Delete(Guid id);
}

public interface IShoeRepository
{
    IReadOnlyList<Shoe> GetAll();
    Shoe? GetById(Guid id);
    void Add(Shoe entity);
    void Update(Shoe entity);
    void Delete(Guid id);
}

public interface IShoeSizeRepository
{
    IReadOnlyList<ShoeSize> GetAll();
    ShoeSize? GetById(Guid id);
    void Add(ShoeSize entity);
    void Update(ShoeSize entity);
    void Delete(Guid id);
}

public interface IStockMovementRepository
{
    IReadOnlyList<StockMovement> GetAll();
    StockMovement? GetById(Guid id);
    void Add(StockMovement entity);
}

public interface IMovementItemRepository
{
    IReadOnlyList<MovementItem> GetAll();
    MovementItem? GetById(Guid id);
}
