using ShoeStore.WinForms.Repositories.Implementations;

namespace ShoeStore.WinForms.Forms;

public sealed partial class InventoryForm : Form
{
    private readonly StockReportService _report = null!;
    public InventoryForm()
    {
        InitializeComponent();
    }

    public InventoryForm(StockReportService report) : this()
    {
        _report = report;
        refreshInventoryButton.Click += RefreshInventory;
        Shown += RefreshInventory;
    }

    private void RefreshInventory(object? sender, EventArgs e) =>
        Ui.Run(this, Reload);
    private void Reload()
    {
        var today = DateTime.Today;
        var rows = _report.GetStockReport(today).ToList();
        _stock.DataSource = rows;
        _period.Text = $"На {today:d}; продажи и выручка: {today:MMMM yyyy}";
        _totals.Text = $"Остаток: {rows.Sum(
            x => x.Balance
        )} пар   |   Продано за месяц: {rows.Sum(
            x => x.CurrentMonthSold
        )} пар   |   Выручка: {rows.Sum(
            x => x.CurrentMonthRevenue
        ):N2} ₽";
    }
}
