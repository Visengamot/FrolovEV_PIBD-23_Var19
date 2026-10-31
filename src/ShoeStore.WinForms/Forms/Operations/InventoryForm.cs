using ShoeStore.Core.Services;
using ShoeStore.WinForms.Helpers;

namespace ShoeStore.WinForms.Forms;

public sealed class InventoryForm : Form
{
    private readonly StockReportService _report;
    private readonly DataGridView _stock = Ui.Grid("inventoryGrid");
    private readonly Label _period = new() { AutoSize = true, Padding = new Padding(15, 8, 0, 0) };
    private readonly Label _totals = new() { Name = "inventoryTotalsLabel", Dock = DockStyle.Bottom, Height = 36, Padding = new Padding(12, 8, 0, 0) };

    public InventoryForm(StockReportService report)
    {
        _report = report; Text = "Наличие товара"; Name = nameof(InventoryForm);
        Size = new Size(1150, 650); MinimumSize = new Size(800, 430); StartPosition = FormStartPosition.CenterParent;
        Ui.Column(_stock, "Article", "Артикул"); Ui.Column(_stock, "ShoeName", "Обувь"); Ui.Column(_stock, "ManufacturerName", "Производитель");
        Ui.Column(_stock, "Size", "Размер", "0.#"); Ui.Column(_stock, "Balance", "Остаток, пар");
        Ui.Column(_stock, "CurrentMonthSold", "Продано за месяц, пар"); Ui.Column(_stock, "CurrentMonthRevenue", "Выручка за месяц, ₽", "N2");
        var toolbar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 52, Padding = new Padding(10) };
        toolbar.Controls.Add(Ui.Button("refreshInventoryButton", "Обновить", () => Ui.Run(this, Reload))); toolbar.Controls.Add(_period);
        Controls.Add(_stock); Controls.Add(_totals); Controls.Add(toolbar);
        Shown += (_, _) => Ui.Run(this, Reload);
    }

    private void Reload()
    {
        var today = DateTime.Today;
        var rows = _report.GetStockReport(today).ToList(); _stock.DataSource = rows;
        _period.Text = $"На {today:d}; продажи и выручка: {today:MMMM yyyy}";
        _totals.Text = $"Остаток: {rows.Sum(x => x.Balance)} пар   |   Продано за месяц: {rows.Sum(x => x.CurrentMonthSold)} пар   |   Выручка: {rows.Sum(x => x.CurrentMonthRevenue):N2} ₽";
    }
}
