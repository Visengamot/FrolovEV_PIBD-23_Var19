using ShoeStore.WinForms.Entities;

namespace ShoeStore.WinForms.Repositories;

public interface IShoeSizeRepository
{
    IReadOnlyList<ShoeSize> GetAll();

    ShoeSize? GetById(Guid id);

    void Add(ShoeSize entity);

    void Update(ShoeSize entity);

    void Delete(Guid id);
}
