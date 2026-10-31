using Autofac;
using ShoeStore.WinForms.Helpers;

namespace ShoeStore.WinForms.Forms;

public sealed class MainForm : Form
{
    private readonly ILifetimeScope _scope;

    public MainForm(ILifetimeScope scope)
    {
        _scope = scope;
        Name = nameof(MainForm); Text = "Магазин обуви — учёт товара";
        StartPosition = FormStartPosition.CenterScreen; Size = new Size(1200, 760);
        MinimumSize = new Size(850, 550); IsMdiContainer = true;
        var menu = new MenuStrip { Name = "mainMenuStrip" };
        var catalogs = new ToolStripMenuItem("Справочники") { Name = "catalogsMenuItem" };
        catalogs.DropDownItems.Add(Item<ManufacturersForm>("manufacturersMenuItem", "Производители"));
        catalogs.DropDownItems.Add(Item<ShoesForm>("shoesMenuItem", "Ассортимент обуви"));
        catalogs.DropDownItems.Add(Item<ShoeSizesForm>("sizesMenuItem", "Размеры"));
        var operations = new ToolStripMenuItem("Операции") { Name = "operationsMenuItem" };
        operations.DropDownItems.Add(Item<MovementsForm>("movementsMenuItem", "Движение товара"));
        operations.DropDownItems.Add(Item<InventoryForm>("inventoryMenuItem", "Наличие товара"));
        var reports = new ToolStripMenuItem("Отчеты") { Name = "reportsMenuItem" };
        menu.Items.AddRange([catalogs, operations, reports]);
        var status = new StatusStrip { Name = "mainStatusStrip" };
        status.Items.Add(new ToolStripStatusLabel("Учёт обуви • Данные хранятся в памяти до закрытия программы") { Spring = true, TextAlign = ContentAlignment.MiddleLeft });
        Controls.Add(menu); Controls.Add(status); MainMenuStrip = menu;
    }

    private ToolStripMenuItem Item<T>(string name, string label) where T : Form
    {
        var item = new ToolStripMenuItem(label) { Name = name };
        item.Click += (_, _) => Ui.Run(this, () =>
        {
            var existing = MdiChildren.OfType<T>().FirstOrDefault();
            if (existing is not null) { existing.Activate(); return; }
            var childScope = _scope.BeginLifetimeScope();
            try
            {
                var form = childScope.Resolve<T>();
                form.MdiParent = this; form.FormClosed += (_, _) => childScope.Dispose();
                form.Show();
            }
            catch { childScope.Dispose(); throw; }
        });
        return item;
    }
}
