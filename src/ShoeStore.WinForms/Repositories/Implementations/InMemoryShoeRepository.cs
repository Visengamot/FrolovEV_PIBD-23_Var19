using ShoeStore.WinForms.Entities;
using ShoeStore.WinForms.Repositories;
using ShoeStore.WinForms.Repositories.Implementations;

namespace ShoeStore.WinForms.Repositories.Implementations;

public sealed class InMemoryShoeRepository(InMemoryStore store) : IShoeRepository
{
    private readonly InMemoryStore _store = store ?? throw new ArgumentNullException(nameof(store));
    public IReadOnlyList<Shoe> GetAll() =>
        _store.Shoes.Values.OrderBy(x => x.Article).ToList();
    public Shoe? GetById(Guid id) =>
        _store.Shoes.GetValueOrDefault(id);
    public void Add(Shoe entity)
    {
        Validate(entity);
        if (!_store.Shoes.TryAdd(entity.Id, entity))
            throw new InvalidOperationException("Товар уже существует.");
    }

    public void Update(Shoe entity)
    {
        Validate(entity);
        if (!_store.Shoes.ContainsKey(entity.Id))
            throw new InvalidOperationException("Товар не найден.");
        _store.Shoes[entity.Id] = entity;
    }

    public void Delete(Guid id)
    {
        if (_store.Movements.Values.SelectMany(x => x.Items).Any(x => x.ShoeId == id))
            throw new InvalidOperationException("Нельзя удалить товар с историей движения.");
        if (!_store.Shoes.Remove(id))
            throw new InvalidOperationException("Товар не найден.");
    }

    private void Validate(Shoe entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (!_store.Manufacturers.ContainsKey(entity.ManufacturerId))
            throw new InvalidOperationException("Производитель не найден.");
        if (_store.Shoes.Values.Any(
            x => x.Id != entity.Id
            && string.Equals(x.Article, entity.Article, StringComparison.OrdinalIgnoreCase)
        ))
            throw new InvalidOperationException("Артикул должен быть уникальным.");
    }
}
