using Microsoft.EntityFrameworkCore;
using ShoeStore.WinForms.Data;
using ShoeStore.WinForms.Entities;

namespace ShoeStore.WinForms.Repositories.Implementations;

public sealed class ManufacturerRepository(DataContext context, Serilog.ILogger logger) : IManufacturerRepository
{
    public IReadOnlyList<Manufacturer> GetAll() => RepositoryOperation.Read(logger, "Manufacturers.GetAll", () => (IReadOnlyList<Manufacturer>)context.Manufacturers.AsNoTracking().OrderBy(x => x.Name).ToList());
    public Manufacturer? GetById(Guid id) => RepositoryOperation.Read(logger, $"Manufacturers.GetById({id})", () => context.Manufacturers.AsNoTracking().SingleOrDefault(x => x.Id == id));
    public void Add(Manufacturer entity) => Save(entity, false);
    public void Update(Manufacturer entity) => Save(entity, true);
    private void Save(Manufacturer entity, bool update)
    {
        ArgumentNullException.ThrowIfNull(entity);
        RepositoryOperation.Write(context, logger, update ? "Manufacturers.Update" : "Manufacturers.Add", entity, () =>
        {
            if (context.Manufacturers.Any(x => x.Id != entity.Id && x.Name.ToUpper() == entity.Name.ToUpper())) throw new InvalidOperationException("Производитель с таким названием уже существует.");
            var existing = context.Manufacturers.SingleOrDefault(x => x.Id == entity.Id);
            if (update)
            {
                if (existing is null) throw new InvalidOperationException("Производитель не найден.");
                context.Entry(existing).CurrentValues.SetValues(entity);
            }
            else
            {
                if (existing is not null) throw new InvalidOperationException("Производитель уже существует.");
                context.Manufacturers.Add(entity);
            }
            context.SaveChanges();
        });
    }
    public void Delete(Guid id) => RepositoryOperation.Write(context, logger, "Manufacturers.Delete", new { Id = id }, () =>
    {
        if (context.Shoes.Any(x => x.ManufacturerId == id)) throw new InvalidOperationException("Нельзя удалить производителя, используемого в ассортименте.");
        var existing = context.Manufacturers.SingleOrDefault(x => x.Id == id) ?? throw new InvalidOperationException("Производитель не найден.");
        context.Manufacturers.Remove(existing); context.SaveChanges();
    });
}
