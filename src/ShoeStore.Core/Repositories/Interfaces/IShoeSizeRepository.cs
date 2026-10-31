using ShoeStore.Core.Models;

namespace ShoeStore.Core.Repositories.Interfaces;

public interface IShoeSizeRepository
{
    IReadOnlyList<ShoeSize> GetAll();
    ShoeSize? GetById(Guid id);
    void Add(ShoeSize entity);
    void Update(ShoeSize entity);
    void Delete(Guid id);
}
