using ShoeStore.Core.Models;
using ShoeStore.Core.Repositories;
using ShoeStore.Core.Services;

namespace ShoeStore.WinForms;

public sealed class MovementsForm : Form
{
    private readonly IStockMovementRepository _movements;
    private readonly IShoeRepository _shoes;
    private readonly IShoeSizeRepository _sizes;
    private readonly DataGridView _operations = Ui.Grid("movementsGrid");
    private readonly DataGridView _items = Ui.Grid("savedMovementItemsGrid");

    public MovementsForm(IStockMovementRepository movements, IShoeRepository shoes, IShoeSizeRepository sizes)
    {
        _movements = movements; _shoes = shoes; _sizes = sizes;
        Text = "Движение товара"; Name = nameof(MovementsForm); Size = new Size(1080, 700);
        MinimumSize = new Size(750, 450); StartPosition = FormStartPosition.CenterParent;
        Ui.Column(_operations, "Id", "Код"); _operations.Columns["IdColumn"].Visible = false;
        Ui.Column(_operations, "Date", "Дата", "d"); Ui.Column(_operations, "Type", "Вид операции");
        Ui.Column(_operations, "Lines", "Позиций"); Ui.Column(_operations, "Quantity", "Количество пар"); Ui.Column(_operations, "Total", "Сумма, ₽", "N2");
        Ui.Column(_items, "Article", "Артикул"); Ui.Column(_items, "Shoe", "Обувь"); Ui.Column(_items, "Size", "Размер", "0.#");
        Ui.Column(_items, "Quantity", "Количество"); Ui.Column(_items, "UnitPrice", "Цена, ₽", "N2"); Ui.Column(_items, "Total", "Сумма, ₽", "N2");
        var toolbar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 52, Padding = new Padding(10) };
        toolbar.Controls.Add(Ui.Button("addMovementButton", "Добавить", () => Ui.Run(this, () =>
        {
            using var editor = new MovementEditorForm(_movements, _shoes, _sizes);
            if (editor.ShowDialog(this) == DialogResult.OK) Reload();
        })));
        toolbar.Controls.Add(Ui.Button("refreshMovementsButton", "Обновить", () => Ui.Run(this, Reload)));
        toolbar.Controls.Add(new Label { Text = "Сохранённые операции доступны только для просмотра.", AutoSize = true, Padding = new Padding(12, 8, 0, 0) });
        var split = new SplitContainer { Name = "movementSplitContainer", Dock = DockStyle.Fill, Orientation = Orientation.Horizontal };
        split.Panel1.Controls.Add(_operations);
        var detailHeader = new Label { Text = "Позиции выбранной операции", Dock = DockStyle.Top, Height = 30, Padding = new Padding(8) };
        split.Panel2.Controls.Add(_items); split.Panel2.Controls.Add(detailHeader);
        Controls.Add(split); Controls.Add(toolbar);
        _operations.SelectionChanged += (_, _) => Ui.Run(this, LoadItems);
        Shown += (_, _) => { split.SplitterDistance = Math.Max(150, split.Height / 2); Ui.Run(this, Reload); };
    }

    private void Reload() => _operations.DataSource = _movements.GetAll().OrderByDescending(x => x.Date).Select(x =>
        new { x.Id, x.Date, Type = Ui.Movement(x.Type), Lines = x.Items.Count, Quantity = x.Items.Sum(i => i.Quantity), x.Total }).ToList();

    private void LoadItems()
    {
        if (_operations.CurrentRow?.Cells["IdColumn"].Value is not Guid id) { _items.DataSource = null; return; }
        var movement = _movements.GetById(id);
        var shoes = _shoes.GetAll().ToDictionary(x => x.Id);
        var sizes = _sizes.GetAll().ToDictionary(x => x.Id);
        _items.DataSource = movement?.Items.Select(x => new
        {
            Article = shoes.TryGetValue(x.ShoeId, out var shoe) ? shoe.Article : "—",
            Shoe = shoes.TryGetValue(x.ShoeId, out var name) ? name.Name : "—",
            Size = sizes.TryGetValue(x.ShoeSizeId, out var size) ? size.Value : 0,
            x.Quantity,
            x.UnitPrice,
            x.Total
        }).ToList();
    }
}

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
