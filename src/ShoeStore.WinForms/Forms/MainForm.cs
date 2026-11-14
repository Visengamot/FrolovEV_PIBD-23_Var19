using Autofac;

namespace ShoeStore.WinForms.Forms;

public partial class MainForm : Form
{
    private readonly ILifetimeScope? _scope;
    public MainForm()
    {
        InitializeComponent();
    }

    public MainForm(ILifetimeScope scope) : this()
    {
        _scope = scope ?? throw new ArgumentNullException(nameof(scope));
        manufacturersMenuItem.Click += (_, _) => OpenForm<ManufacturersForm>();
        shoesMenuItem.Click += (_, _) => OpenForm<ShoesForm>();
        sizesMenuItem.Click += (_, _) => OpenForm<ShoeSizesForm>();
        movementsMenuItem.Click += (_, _) => OpenForm<MovementsForm>();
        inventoryMenuItem.Click += (_, _) => OpenForm<InventoryForm>();
    }

    private void OpenForm<T>()
        where T : Form
    {
        if (_scope is null)
            return;
        Ui.Run(this, () =>
        {
            var existing = MdiChildren.OfType<T>().FirstOrDefault();
            if (existing is not null)
            {
                existing.Activate();
                return;
            }

            var childScope = _scope.BeginLifetimeScope();
            try
            {
                var form = childScope.Resolve<T>();
                form.MdiParent = this;
                form.FormClosed += (_, _) => childScope.Dispose();
                form.Show();
            }
            catch
            {
                childScope.Dispose();
                throw;
            }
        });
    }
}
