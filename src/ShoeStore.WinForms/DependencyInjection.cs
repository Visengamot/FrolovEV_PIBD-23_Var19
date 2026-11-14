using Autofac;
using Serilog;
using ShoeStore.WinForms.Data;
using ShoeStore.WinForms.Forms;
using ShoeStore.WinForms.Repositories;
using ShoeStore.WinForms.Repositories.Implementations;

namespace ShoeStore.WinForms;

public static class DependencyInjection
{
    public static IContainer CreateContainer(Action<ContainerBuilder>? configure = null)
    {
        var builder = new ContainerBuilder();
        builder.RegisterShoeStore();
        configure?.Invoke(builder);
        return builder.Build();
    }

    public static ContainerBuilder RegisterShoeStore(this ContainerBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.RegisterType<ManufacturerRepository>().As<IManufacturerRepository>().InstancePerLifetimeScope();
        builder.RegisterType<ShoeRepository>().As<IShoeRepository>().InstancePerLifetimeScope();
        builder.RegisterType<ShoeSizeRepository>().As<IShoeSizeRepository>().InstancePerLifetimeScope();
        builder.RegisterType<StockMovementRepository>().As<IStockMovementRepository>().InstancePerLifetimeScope();
        builder.RegisterType<MovementItemRepository>().As<IMovementItemRepository>().InstancePerLifetimeScope();
        builder.RegisterType<StockReportService>().InstancePerLifetimeScope();
        builder.RegisterType<MainForm>();
        builder.RegisterType<ManufacturersForm>();
        builder.RegisterType<ShoesForm>();
        builder.RegisterType<ShoeSizesForm>();
        builder.RegisterType<MovementsForm>();
        builder.RegisterType<InventoryForm>();
        return builder;
    }
}
