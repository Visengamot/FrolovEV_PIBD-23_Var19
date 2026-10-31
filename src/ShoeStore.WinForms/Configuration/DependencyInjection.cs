using Autofac;
using ShoeStore.Core.Repositories.InMemory;
using ShoeStore.Core.Repositories.Interfaces;
using ShoeStore.Core.Services;
using ShoeStore.Core.Storage;
using ShoeStore.WinForms.Forms;

namespace ShoeStore.WinForms.Configuration;

public static class DependencyInjection
{
    public static IContainer CreateContainer() => new ContainerBuilder().RegisterShoeStore().Build();

    /// <summary>Registers one shared session store and repositories; forms are resolved per request.</summary>
    public static ContainerBuilder RegisterShoeStore(this ContainerBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Register(_ => new InMemoryStore()).SingleInstance();
        builder.RegisterType<InMemoryManufacturerRepository>().As<IManufacturerRepository>().SingleInstance();
        builder.RegisterType<InMemoryShoeRepository>().As<IShoeRepository>().SingleInstance();
        builder.RegisterType<InMemoryShoeSizeRepository>().As<IShoeSizeRepository>().SingleInstance();
        builder.RegisterType<InMemoryStockMovementRepository>().As<IStockMovementRepository>().SingleInstance();
        builder.RegisterType<InMemoryMovementItemRepository>().As<IMovementItemRepository>().SingleInstance();
        builder.RegisterType<StockReportService>().SingleInstance();
        builder.RegisterType<MainForm>();
        builder.RegisterType<ManufacturersForm>();
        builder.RegisterType<ShoesForm>();
        builder.RegisterType<ShoeSizesForm>();
        builder.RegisterType<MovementsForm>();
        builder.RegisterType<InventoryForm>();
        return builder;
    }
}
