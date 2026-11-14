using Microsoft.EntityFrameworkCore;
using ShoeStore.WinForms.Data;
using ShoeStore.WinForms.Entities;

namespace ShoeStore.WinForms.Repositories.Implementations;

public sealed class ShoeRepository(DataContext context, Serilog.ILogger logger) : IShoeRepository
{
    public IReadOnlyList<Shoe> GetAll() =>
        RepositoryOperation.Read(
        logger,
        "Shoes.GetAll",
        () => (IReadOnlyList<Shoe>)context.Shoes.AsNoTracking()
        .OrderBy(x => x.Article)
        .ToList()
    );

    public Shoe? GetById(
        Guid id
    ) =>
        RepositoryOperation.Read(
        logger,
        $"Shoes.GetById({id})",
        () => context.Shoes.AsNoTracking()
        .SingleOrDefault(x => x.Id == id)
    );

    public void Add(Shoe entity) =>
        Save(entity, false);

    public void Update(Shoe entity) =>
        Save(entity, true);

    private void Save(Shoe entity, bool update)
    {
        ArgumentNullException.ThrowIfNull(entity);
        RepositoryOperation.Write(context, logger, update ? "Shoes.Update" : "Shoes.Add", entity, () =>
        {
            if (!context.Manufacturers.Any(x => x.Id == entity.ManufacturerId))
                throw new InvalidOperationException("Производитель не найден.");
            if (context.Shoes.Any(x => x.Id != entity.Id && x.Article.ToUpper() == entity.Article.ToUpper()))
                throw new InvalidOperationException("Артикул должен быть уникальным.");
            var existing = context.Shoes.SingleOrDefault(x => x.Id == entity.Id);
            if (update)
            {
                if (existing is null)
                    throw new InvalidOperationException("Товар не найден.");
                context.Entry(existing).CurrentValues.SetValues(entity);
            }
            else
            {
                if (existing is not null)
                    throw new InvalidOperationException("Товар уже существует.");
                context.Shoes.Add(entity);
            }

            context.SaveChanges();
        });
    }

    public void Delete(Guid id) =>
        RepositoryOperation.Write(context, logger, "Shoes.Delete", new
        {
            Id = id
        }, () =>
    {
        if (context.MovementItems.Any(x => x.ShoeId == id))
            throw new InvalidOperationException("Нельзя удалить товар с историей движения.");
        var existing = context.Shoes.SingleOrDefault(x => x.Id == id)
            ?? throw new InvalidOperationException("Товар не найден.");
        context.Shoes.Remove(existing);
        context.SaveChanges();
    });
}
