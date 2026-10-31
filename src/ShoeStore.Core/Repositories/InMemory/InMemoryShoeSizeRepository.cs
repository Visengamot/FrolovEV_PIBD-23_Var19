using ShoeStore.Core.Models;
using ShoeStore.Core.Repositories.Interfaces;
using ShoeStore.Core.Storage;

namespace ShoeStore.Core.Repositories.InMemory;

public sealed class InMemoryShoeSizeRepository(InMemoryStore store) : IShoeSizeRepository
{
    private readonly InMemoryStore _store = store ?? throw new ArgumentNullException(nameof(store));
    public IReadOnlyList<ShoeSize> GetAll() => _store.Sizes.Values.OrderBy(x => x.Value).ToList();
    public ShoeSize? GetById(Guid id) => _store.Sizes.GetValueOrDefault(id);
    public void Add(ShoeSize entity) { Validate(entity); if (!_store.Sizes.TryAdd(entity.Id, entity)) throw new InvalidOperationException("Размер уже существует."); }
    public void Update(ShoeSize entity) { Validate(entity); if (!_store.Sizes.ContainsKey(entity.Id)) throw new InvalidOperationException("Размер не найден."); _store.Sizes[entity.Id] = entity; }
    public void Delete(Guid id)
    {
        if (_store.Movements.Values.SelectMany(x => x.Items).Any(x => x.ShoeSizeId == id)) throw new InvalidOperationException("Нельзя удалить размер с историей движения.");
        if (!_store.Sizes.Remove(id)) throw new InvalidOperationException("Размер не найден.");
    }
    private void Validate(ShoeSize entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (_store.Sizes.Values.Any(x => x.Id != entity.Id && x.Value == entity.Value)) throw new InvalidOperationException("Такой размер уже существует.");
        if (_store.Sizes.TryGetValue(entity.Id, out var existing) && existing.Value != entity.Value && _store.Movements.Values.SelectMany(x => x.Items).Any(x => x.ShoeSizeId == entity.Id)) throw new InvalidOperationException("Нельзя менять размер, используемый в истории движения.");
    }
}
