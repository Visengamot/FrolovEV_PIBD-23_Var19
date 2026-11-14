#nullable disable

namespace ShoeStore.WinForms.Forms;

partial class InventoryForm
{
    private System.ComponentModel.IContainer components = null;
    private DataGridView _stock = null!;
    private FlowLayoutPanel inventoryToolbar = null!;
    private Button refreshInventoryButton = null!;
    private Label _period = null!;
    private Label _totals = null!;
    private DataGridViewTextBoxColumn ArticleColumn = null!;
    private DataGridViewTextBoxColumn ShoeNameColumn = null!;
    private DataGridViewTextBoxColumn ManufacturerNameColumn = null!;
    private DataGridViewTextBoxColumn SizeColumn = null!;
    private DataGridViewTextBoxColumn BalanceColumn = null!;
    private DataGridViewTextBoxColumn CurrentMonthSoldColumn = null!;
    private DataGridViewTextBoxColumn CurrentMonthRevenueColumn = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        _stock = new DataGridView();
        inventoryToolbar = new FlowLayoutPanel();
        refreshInventoryButton = new Button();
        _period = new Label();
        _totals = new Label();
        ArticleColumn = new DataGridViewTextBoxColumn();
        ShoeNameColumn = new DataGridViewTextBoxColumn();
        ManufacturerNameColumn = new DataGridViewTextBoxColumn();
        SizeColumn = new DataGridViewTextBoxColumn();
        BalanceColumn = new DataGridViewTextBoxColumn();
        CurrentMonthSoldColumn = new DataGridViewTextBoxColumn();
        CurrentMonthRevenueColumn = new DataGridViewTextBoxColumn();
        SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_stock).BeginInit();
        inventoryToolbar.SuspendLayout();
        _stock.Name = "inventoryGrid";
        _stock.Dock = DockStyle.Fill;
        _stock.AutoGenerateColumns = false;
        _stock.ReadOnly = true;
        _stock.AllowUserToAddRows = false;
        _stock.AllowUserToDeleteRows = false;
        _stock.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _stock.MultiSelect = false;
        _stock.RowHeadersVisible = false;
        _stock.BackgroundColor = Color.White;
        _stock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _stock.BorderStyle = BorderStyle.FixedSingle;
        ArticleColumn.Name = "ArticleColumn";
        ArticleColumn.DataPropertyName = "Article";
        ArticleColumn.HeaderText = "Артикул";
        _stock.Columns.Add(ArticleColumn);
        ShoeNameColumn.Name = "ShoeNameColumn";
        ShoeNameColumn.DataPropertyName = "ShoeName";
        ShoeNameColumn.HeaderText = "Обувь";
        _stock.Columns.Add(ShoeNameColumn);
        ManufacturerNameColumn.Name = "ManufacturerNameColumn";
        ManufacturerNameColumn.DataPropertyName = "ManufacturerName";
        ManufacturerNameColumn.HeaderText = "Производитель";
        _stock.Columns.Add(ManufacturerNameColumn);
        SizeColumn.Name = "SizeColumn";
        SizeColumn.DataPropertyName = "Size";
        SizeColumn.HeaderText = "Размер";
        SizeColumn.DefaultCellStyle.Format = "0.#";
        _stock.Columns.Add(SizeColumn);
        BalanceColumn.Name = "BalanceColumn";
        BalanceColumn.DataPropertyName = "Balance";
        BalanceColumn.HeaderText = "Остаток, пар";
        _stock.Columns.Add(BalanceColumn);
        CurrentMonthSoldColumn.Name = "CurrentMonthSoldColumn";
        CurrentMonthSoldColumn.DataPropertyName = "CurrentMonthSold";
        CurrentMonthSoldColumn.HeaderText = "Продано за месяц, пар";
        _stock.Columns.Add(CurrentMonthSoldColumn);
        CurrentMonthRevenueColumn.Name = "CurrentMonthRevenueColumn";
        CurrentMonthRevenueColumn.DataPropertyName = "CurrentMonthRevenue";
        CurrentMonthRevenueColumn.HeaderText = "Выручка за месяц, ₽";
        CurrentMonthRevenueColumn.DefaultCellStyle.Format = "N2";
        _stock.Columns.Add(CurrentMonthRevenueColumn);
        refreshInventoryButton.Name = "refreshInventoryButton";
        refreshInventoryButton.Text = "Обновить";
        refreshInventoryButton.AutoSize = true;
        refreshInventoryButton.Height = 32;
        refreshInventoryButton.MinimumSize = new Size(100, 32);
        inventoryToolbar.Name = "inventoryToolbar";
        inventoryToolbar.Dock = DockStyle.Top;
        inventoryToolbar.Height = 52;
        inventoryToolbar.Padding = new Padding(10);
        inventoryToolbar.Controls.Add(refreshInventoryButton);
        inventoryToolbar.Controls.Add(_period);
        _period.Name = "inventoryPeriodLabel";
        _period.Text = "Период отчёта";
        _period.AutoSize = true;
        _period.Padding = new Padding(15, 8, 0, 0);
        _totals.Name = "inventoryTotalsLabel";
        _totals.Text = "Остаток: 0 пар   |   Продано за месяц: 0 пар   |   Выручка: 0,00 ₽";
        _totals.Dock = DockStyle.Bottom;
        _totals.Height = 36;
        _totals.Padding = new Padding(12, 8, 0, 0);
        Controls.Add(_stock);
        Controls.Add(_totals);
        Controls.Add(inventoryToolbar);
        Text = "Наличие товара";
        Name = "InventoryForm";
        ClientSize = new Size(1134, 611);
        MinimumSize = new Size(800, 430);
        ((System.ComponentModel.ISupportInitialize)_stock).EndInit();
        inventoryToolbar.ResumeLayout(false);
        inventoryToolbar.PerformLayout();
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        StartPosition = FormStartPosition.CenterParent;
        ResumeLayout(false);
        PerformLayout();
    }
}
