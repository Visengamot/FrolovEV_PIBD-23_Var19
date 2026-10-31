using ShoeStore.Core.Models;

namespace ShoeStore.Core.Repositories.Interfaces;

public interface IShoeRepository
{
    IReadOnlyList<Shoe> GetAll();
    Shoe? GetById(Guid id);
    void Add(Shoe entity);
    void Update(Shoe entity);
    void Delete(Guid id);
}
