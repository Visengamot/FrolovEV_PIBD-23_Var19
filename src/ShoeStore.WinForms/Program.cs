using Autofac;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ShoeStore.WinForms.Data;
using ShoeStore.WinForms.Forms;
using System.Globalization;

namespace ShoeStore.WinForms;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("ru-RU");
        ApplicationConfiguration.Initialize();
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) =>
        {
            Log.Error(e.Exception, "Ошибка формы");
            MessageBox.Show(
                e.Exception.GetBaseException().Message,
                "Ошибка приложения",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        };
        try
        {
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(DataContext.LoadConfiguration())
                .CreateLogger();
            using var container = DependencyInjection.CreateContainer(builder =>
            {
                builder.RegisterType<DataContext>().InstancePerLifetimeScope();
                builder.RegisterInstance(Log.Logger).As<ILogger>().ExternallyOwned();
            });
            using (var scope = container.BeginLifetimeScope())
            {
                scope.Resolve<DataContext>().Database.Migrate();
            }

            Log.Information("Приложение запущено, миграции проверены");
            Application.Run(container.Resolve<MainForm>());
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Ошибка запуска");
            MessageBox.Show(
                "Не удалось запустить приложение. Проверьте запуск PostgreSQL и локальные настройки подключения.\n\n" + exception.GetBaseException().Message,
                "Ошибка запуска",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
