using Microsoft.EntityFrameworkCore;
using ShoeStore.WinForms.Data;
using ShoeStore.WinForms.Entities;
using ShoeStore.WinForms.Entities.Enums;

namespace ShoeStore.WinForms.Repositories.Implementations;

public sealed class StockMovementRepository(DataContext context, Serilog.ILogger logger) : IStockMovementRepository
{
    public IReadOnlyList<StockMovement> GetAll() =>
        RepositoryOperation.Read(
        logger,
        "StockMovements.GetAll",
        () => (IReadOnlyList<StockMovement>)context.StockMovements.AsNoTracking()
        .Include(x => x.Items)
        .OrderByDescending(x => x.Date)
        .ToList()
    );

    public StockMovement? GetById(
        Guid id
    ) =>
        RepositoryOperation.Read(
        logger,
        $"StockMovements.GetById({id})",
        () => context.StockMovements.AsNoTracking()
        .Include(x => x.Items)
        .SingleOrDefault(x => x.Id == id)
    );

    public void Add(StockMovement entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        RepositoryOperation.Write(context, logger, "StockMovements.Add", entity, () =>
        {
            if (context.StockMovements.Any(x => x.Id == entity.Id))
                throw new InvalidOperationException("Операция уже существует.");
            var ledger = context.StockMovements.AsNoTracking().Include(x => x.Items).ToList();
            var existingIds = ledger.SelectMany(x => x.Items).Select(x => x.Id).ToHashSet();
            foreach (var item in entity.Items)
            {
                if (!context.Shoes.Any(x => x.Id == item.ShoeId)
                    || !context.ShoeSizes.Any(x => x.Id == item.ShoeSizeId))
                    throw new InvalidOperationException("Товар или размер не найден.");
                if (existingIds.Contains(item.Id))
                    throw new InvalidOperationException("Идентификатор позиции уже используется.");
                long balance = 0;
                foreach (var movement in ledger.Append(entity).OrderBy(x => x.Date).ThenBy(x => x.Type))
                {
                    balance += movement.Items.Where(x => x.ShoeId == item.ShoeId && x.ShoeSizeId == item.ShoeSizeId)
                        .Sum(x => (long)x.Quantity) * (movement.Type == MovementType.Receipt
                        ? 1
                        : -1);
                    if (balance < 0)
                        throw new InvalidOperationException(
                            "Недостаточно товара: продажа не может предшествовать поступлению или создавать отрицательный остаток."
                        );
                    if (balance > int.MaxValue)
                        throw new InvalidOperationException("Превышен допустимый складской остаток.");
                }
            }

            context.StockMovements.Add(entity);
            context.SaveChanges();
        });
    }
}
