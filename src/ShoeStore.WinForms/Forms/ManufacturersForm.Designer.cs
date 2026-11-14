#nullable disable
namespace ShoeStore.WinForms.Forms;

partial class ManufacturersForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        ItemsGrid = new DataGridView();
        catalogToolbar = new FlowLayoutPanel();
        addButton = new Button();
        editButton = new Button();
        deleteButton = new Button();
        refreshButton = new Button();
        IdColumn = new DataGridViewTextBoxColumn();
        NameColumn = new DataGridViewTextBoxColumn();
        CountryColumn = new DataGridViewTextBoxColumn();
        ((System.ComponentModel.ISupportInitialize)ItemsGrid).BeginInit();
        catalogToolbar.SuspendLayout();
        SuspendLayout();
        // ItemsGrid
        ItemsGrid.AllowUserToAddRows = false;
        ItemsGrid.AllowUserToDeleteRows = false;
        ItemsGrid.AutoGenerateColumns = false;
        ItemsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        ItemsGrid.BackgroundColor = Color.White;
        ItemsGrid.BorderStyle = BorderStyle.FixedSingle;
        ItemsGrid.Columns.Add(IdColumn);
        ItemsGrid.Columns.Add(NameColumn);
        ItemsGrid.Columns.Add(CountryColumn);
        ItemsGrid.Dock = DockStyle.Fill;
        ItemsGrid.Location = new Point(0, 52);
        ItemsGrid.MultiSelect = false;
        ItemsGrid.Name = "catalogGrid";
        ItemsGrid.ReadOnly = true;
        ItemsGrid.RowHeadersVisible = false;
        ItemsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        ItemsGrid.Size = new Size(1034, 509);
        ItemsGrid.TabIndex = 1;
        // catalogToolbar
        catalogToolbar.Controls.Add(addButton);
        catalogToolbar.Controls.Add(editButton);
        catalogToolbar.Controls.Add(deleteButton);
        catalogToolbar.Controls.Add(refreshButton);
        catalogToolbar.Dock = DockStyle.Top;
        catalogToolbar.Location = new Point(0, 0);
        catalogToolbar.Name = "catalogToolbar";
        catalogToolbar.Padding = new Padding(10);
        catalogToolbar.Size = new Size(1034, 52);
        catalogToolbar.TabIndex = 0;
        // addButton
        addButton.AutoSize = true;
        addButton.Location = new Point(13, 13);
        addButton.MinimumSize = new Size(100, 32);
        addButton.Name = "addButton";
        addButton.Size = new Size(100, 32);
        addButton.TabIndex = 0;
        addButton.Text = "Добавить";
        addButton.UseVisualStyleBackColor = true;
        // editButton
        editButton.AutoSize = true;
        editButton.Location = new Point(119, 13);
        editButton.MinimumSize = new Size(100, 32);
        editButton.Name = "editButton";
        editButton.Size = new Size(100, 32);
        editButton.TabIndex = 1;
        editButton.Text = "Изменить";
        editButton.UseVisualStyleBackColor = true;
        // deleteButton
        deleteButton.AutoSize = true;
        deleteButton.Location = new Point(225, 13);
        deleteButton.MinimumSize = new Size(100, 32);
        deleteButton.Name = "deleteButton";
        deleteButton.Size = new Size(100, 32);
        deleteButton.TabIndex = 2;
        deleteButton.Text = "Удалить";
        deleteButton.UseVisualStyleBackColor = true;
        // refreshButton
        refreshButton.AutoSize = true;
        refreshButton.Location = new Point(331, 13);
        refreshButton.MinimumSize = new Size(100, 32);
        refreshButton.Name = "refreshButton";
        refreshButton.Size = new Size(100, 32);
        refreshButton.TabIndex = 3;
        refreshButton.Text = "Обновить";
        refreshButton.UseVisualStyleBackColor = true;
        // IdColumn
        IdColumn.DataPropertyName = "Id";
        IdColumn.HeaderText = "Код";
        IdColumn.Name = "IdColumn";
        IdColumn.ReadOnly = true;
        IdColumn.Visible = false;
        // NameColumn
        NameColumn.DataPropertyName = "Name";
        NameColumn.HeaderText = "Название";
        NameColumn.Name = "NameColumn";
        NameColumn.ReadOnly = true;
        // CountryColumn
        CountryColumn.DataPropertyName = "Country";
        CountryColumn.HeaderText = "Страна";
        CountryColumn.Name = "CountryColumn";
        CountryColumn.ReadOnly = true;
        // ManufacturersForm
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1034, 561);
        Controls.Add(ItemsGrid);
        Controls.Add(catalogToolbar);
        MinimumSize = new Size(700, 400);
        Name = "ManufacturersForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Производители";
        ((System.ComponentModel.ISupportInitialize)ItemsGrid).EndInit();
        catalogToolbar.ResumeLayout(false);
        catalogToolbar.PerformLayout();
        ResumeLayout(false);
    }

    private DataGridView ItemsGrid;
    private FlowLayoutPanel catalogToolbar;
    private Button addButton;
    private Button editButton;
    private Button deleteButton;
    private Button refreshButton;
    private DataGridViewTextBoxColumn IdColumn;
    private DataGridViewTextBoxColumn NameColumn;
    private DataGridViewTextBoxColumn CountryColumn;
}
