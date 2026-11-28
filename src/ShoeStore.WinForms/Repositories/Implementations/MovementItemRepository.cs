using Microsoft.EntityFrameworkCore;
using ShoeStore.WinForms.Data;
using ShoeStore.WinForms.Entities;

namespace ShoeStore.WinForms.Repositories.Implementations;

public sealed class MovementItemRepository(DataContext context, Serilog.ILogger logger) : IMovementItemRepository
{
    public IReadOnlyList<MovementItem> GetAll() =>
        RepositoryOperation.Read(
        logger,
        "MovementItems.GetAll",
        () => (IReadOnlyList<MovementItem>)context.MovementItems.AsNoTracking()
        .OrderBy(x => x.Id)
        .ToList()
    );

    public MovementItem? GetById(
        Guid id
    ) =>
        RepositoryOperation.Read(
        logger,
        $"MovementItems.GetById({id})",
        () => context.MovementItems.AsNoTracking()
        .SingleOrDefault(x => x.Id == id)
    );
}
