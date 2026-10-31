using ShoeStore.WinForms.Entities;
using ShoeStore.WinForms.Repositories;
using ShoeStore.WinForms.Repositories.Implementations;

namespace ShoeStore.WinForms.Repositories.Implementations;

public sealed class InMemoryManufacturerRepository(InMemoryStore store) : IManufacturerRepository
{
    private readonly InMemoryStore _store = store ?? throw new ArgumentNullException(nameof(store));
    public IReadOnlyList<Manufacturer> GetAll() => _store.Manufacturers.Values.OrderBy(x => x.Name).ToList();
    public Manufacturer? GetById(Guid id) => _store.Manufacturers.GetValueOrDefault(id);
    public void Add(Manufacturer entity) { Validate(entity); if (!_store.Manufacturers.TryAdd(entity.Id, entity)) throw new InvalidOperationException("Производитель уже существует."); }
    public void Update(Manufacturer entity) { Validate(entity); if (!_store.Manufacturers.ContainsKey(entity.Id)) throw new InvalidOperationException("Производитель не найден."); _store.Manufacturers[entity.Id] = entity; }
    public void Delete(Guid id)
    {
        if (_store.Shoes.Values.Any(x => x.ManufacturerId == id)) throw new InvalidOperationException("Нельзя удалить производителя, используемого в ассортименте.");
        if (!_store.Manufacturers.Remove(id)) throw new InvalidOperationException("Производитель не найден.");
    }
    private void Validate(Manufacturer entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (_store.Manufacturers.Values.Any(x => x.Id != entity.Id && string.Equals(x.Name, entity.Name, StringComparison.OrdinalIgnoreCase))) throw new InvalidOperationException("Производитель с таким названием уже существует.");
    }
}
