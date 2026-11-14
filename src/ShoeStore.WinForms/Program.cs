using Autofac;
using ShoeStore.WinForms;
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
        Application.ThreadException += (
            _,
            e
        ) => MessageBox.Show(
            e.Exception.GetBaseException().Message,
            "Ошибка приложения",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        );
        try
        {
            using var container = DependencyInjection.CreateContainer();
            Application.Run(container.Resolve<MainForm>());
        }
        catch (Exception exception)
        {
            MessageBox.Show(
                exception.GetBaseException().Message,
                "Ошибка запуска",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }
}
