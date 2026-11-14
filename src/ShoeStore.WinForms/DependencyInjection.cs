using Autofac;
using ShoeStore.WinForms.Repositories.Implementations;
using ShoeStore.WinForms.Repositories;
using ShoeStore.WinForms.Forms;

namespace ShoeStore.WinForms;

public static class DependencyInjection
{
    public static IContainer CreateContainer() =>
        new ContainerBuilder().RegisterShoeStore().Build();
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
