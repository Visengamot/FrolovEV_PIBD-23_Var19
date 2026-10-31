using ShoeStore.WinForms.Entities;

namespace ShoeStore.WinForms.Repositories;

public interface IShoeRepository
{
    IReadOnlyList<Shoe> GetAll();
    Shoe? GetById(Guid id);
    void Add(Shoe entity);
    void Update(Shoe entity);
    void Delete(Guid id);
}
