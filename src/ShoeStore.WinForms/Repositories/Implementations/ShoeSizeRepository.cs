using Microsoft.EntityFrameworkCore;
using ShoeStore.WinForms.Data;
using ShoeStore.WinForms.Entities;

namespace ShoeStore.WinForms.Repositories.Implementations;

public sealed class ShoeSizeRepository(DataContext context, Serilog.ILogger logger) : IShoeSizeRepository
{
    public IReadOnlyList<ShoeSize> GetAll() =>
        RepositoryOperation.Read(
        logger,
        "ShoeSizes.GetAll",
        () => (IReadOnlyList<ShoeSize>)context.ShoeSizes.AsNoTracking()
        .OrderBy(x => x.Value)
        .ToList()
    );

    public ShoeSize? GetById(
        Guid id
    ) =>
        RepositoryOperation.Read(
        logger,
        $"ShoeSizes.GetById({id})",
        () => context.ShoeSizes.AsNoTracking()
        .SingleOrDefault(x => x.Id == id)
    );

    public void Add(ShoeSize entity) =>
        Save(entity, false);

    public void Update(ShoeSize entity) =>
        Save(entity, true);

    private void Save(ShoeSize entity, bool update)
    {
        ArgumentNullException.ThrowIfNull(entity);
        RepositoryOperation.Write(context, logger, update ? "ShoeSizes.Update" : "ShoeSizes.Add", entity, () =>
        {
            if (context.ShoeSizes.Any(x => x.Id != entity.Id && x.Value == entity.Value))
                throw new InvalidOperationException("Такой размер уже существует.");
            var existing = context.ShoeSizes.SingleOrDefault(x => x.Id == entity.Id);
            if (update)
            {
                if (existing is null)
                    throw new InvalidOperationException("Размер не найден.");
                if (existing.Value != entity.Value && context.MovementItems.Any(x => x.ShoeSizeId == entity.Id))
                    throw new InvalidOperationException("Нельзя менять размер, используемый в истории движения.");
                context.Entry(existing).CurrentValues.SetValues(entity);
            }
            else
            {
                if (existing is not null)
                    throw new InvalidOperationException("Размер уже существует.");
                context.ShoeSizes.Add(entity);
            }

            context.SaveChanges();
        });
    }

    public void Delete(Guid id) =>
        RepositoryOperation.Write(context, logger, "ShoeSizes.Delete", new
        {
            Id = id
        }, () =>
    {
        if (context.MovementItems.Any(x => x.ShoeSizeId == id))
            throw new InvalidOperationException("Нельзя удалить размер с историей движения.");
        var existing = context.ShoeSizes.SingleOrDefault(x => x.Id == id)
            ?? throw new InvalidOperationException("Размер не найден.");
        context.ShoeSizes.Remove(existing);
        context.SaveChanges();
    });
}
