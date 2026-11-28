using ShoeStore.WinForms.Entities;

namespace ShoeStore.WinForms.Repositories;

public interface IManufacturerRepository
{
    IReadOnlyList<Manufacturer> GetAll();

    Manufacturer? GetById(Guid id);

    void Add(Manufacturer entity);

    void Update(Manufacturer entity);

    void Delete(Guid id);
}
