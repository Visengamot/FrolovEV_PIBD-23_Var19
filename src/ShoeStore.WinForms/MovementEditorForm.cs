using System.Globalization;
using ShoeStore.Core.Models;
using ShoeStore.Core.Repositories;

namespace ShoeStore.WinForms;

/// <summary>Only creates operations. Saved operations have no editing UI.</summary>
public sealed class MovementEditorForm : Form
{
    private readonly IStockMovementRepository _movements;
    private readonly DateTimePicker _date = new() { Name = "movementDatePicker", Format = DateTimePickerFormat.Short, Width = 130 };
    private readonly ComboBox _type = new() { Name = "movementTypeComboBox", DropDownStyle = ComboBoxStyle.DropDownList, Width = 170, DisplayMember = "Label", ValueMember = "Value" };
    private readonly DataGridView _lines = Ui.Grid("movementLinesGrid", false);
    private readonly Label _total = new() { Name = "totalLabel", AutoSize = true, Padding = new Padding(12), Text = "Итого: 0,00 ₽" };
    private readonly Dictionary<Guid, decimal> _prices;

    public MovementEditorForm(IStockMovementRepository movements, IShoeRepository shoes, IShoeSizeRepository sizes)
    {
        _movements = movements; Text = "Добавление движения товара"; Name = nameof(MovementEditorForm);
        StartPosition = FormStartPosition.CenterParent; Size = new Size(1000, 600); MinimumSize = new Size(760, 420);
        var shoeList = shoes.GetAll().Select(x => new { x.Id, Label = $"{x.Article} — {x.Name}", x.Price }).ToList();
        var sizeList = sizes.GetAll().OrderBy(x => x.Value).ToList();
        _prices = shoeList.ToDictionary(x => x.Id, x => x.Price);
        _date.MaxDate = DateTime.Today;
        _type.DataSource = Enum.GetValues<MovementType>().Select(x => new { Value = x, Label = Ui.Movement(x) }).ToList();
        var header = new FlowLayoutPanel { Name = "movementHeader", Dock = DockStyle.Top, Height = 58, Padding = new Padding(12) };
        header.Controls.Add(new Label { Text = "Дата", AutoSize = true, Padding = new Padding(0, 5, 5, 0) }); header.Controls.Add(_date);
        header.Controls.Add(new Label { Text = "Вид операции", AutoSize = true, Padding = new Padding(20, 5, 5, 0) }); header.Controls.Add(_type);
        _lines.Columns.Add(new DataGridViewComboBoxColumn { Name = "ShoeColumn", HeaderText = "Обувь", DataSource = shoeList, DisplayMember = "Label", ValueMember = "Id", FillWeight = 230 });
        _lines.Columns.Add(new DataGridViewComboBoxColumn { Name = "SizeColumn", HeaderText = "Размер", DataSource = sizeList, DisplayMember = "Value", ValueMember = "Id", FillWeight = 70 });
        _lines.Columns.Add(new DataGridViewTextBoxColumn { Name = "QuantityColumn", HeaderText = "Количество", ValueType = typeof(int), FillWeight = 80 });
        _lines.Columns.Add(new DataGridViewTextBoxColumn { Name = "UnitPriceColumn", HeaderText = "Цена за единицу, ₽", ValueType = typeof(decimal), DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" } });
        _lines.DataError += (_, e) => { e.ThrowException = false; MessageBox.Show(this, "Некорректное значение. Количество — целое число, цена — число с не более чем двумя знаками после запятой.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning); };
        _lines.DefaultValuesNeeded += (_, e) => { e.Row.Cells["QuantityColumn"].Value = 1; e.Row.Cells["UnitPriceColumn"].Value = 0m; };
        _lines.CurrentCellDirtyStateChanged += (_, _) => { if (_lines.IsCurrentCellDirty && _lines.CurrentCell is DataGridViewComboBoxCell) _lines.CommitEdit(DataGridViewDataErrorContexts.Commit); };
        _lines.CellValueChanged += (_, e) =>
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0 && _lines.Rows[e.RowIndex].Cells[0].Value is Guid id && _prices.TryGetValue(id, out var price))
                _lines.Rows[e.RowIndex].Cells["UnitPriceColumn"].Value = price;
            UpdateTotal();
        };
        _lines.RowsRemoved += (_, _) => UpdateTotal();
        var footer = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 60, Padding = new Padding(10), FlowDirection = FlowDirection.RightToLeft };
        var cancel = Ui.Button("cancelButton", "Отмена", () => { DialogResult = DialogResult.Cancel; Close(); });
        footer.Controls.Add(cancel);
        footer.Controls.Add(Ui.Button("saveMovementButton", "Сохранить", () => Ui.Run(this, SaveMovement)));
        footer.Controls.Add(Ui.Button("removeLineButton", "Удалить строку", () => Ui.Run(this, () => { if (_lines.CurrentRow is { IsNewRow: false } row) _lines.Rows.Remove(row); })));
        footer.Controls.Add(_total);
        Controls.Add(_lines); Controls.Add(header); Controls.Add(footer); CancelButton = cancel;
        var hint = new Label { Text = "Добавляйте позиции в пустой строке таблицы. Сохранённые операции не изменяются.", Dock = DockStyle.Bottom, Height = 28, Padding = new Padding(10, 3, 0, 0) };
        Controls.Add(hint); hint.BringToFront();
    }

    private void UpdateTotal()
    {
        decimal total = 0;
        foreach (DataGridViewRow row in _lines.Rows)
            if (!row.IsNewRow && decimal.TryParse(Convert.ToString(row.Cells["QuantityColumn"].Value), out var quantity)
                && decimal.TryParse(Convert.ToString(row.Cells["UnitPriceColumn"].Value), out var price)) total += quantity * price;
        _total.Text = $"Итого: {total:N2} ₽";
    }

    private void SaveMovement()
    {
        if (!_lines.EndEdit()) throw new ArgumentException("Исправьте значения в таблице.");
        var id = Guid.NewGuid(); var items = new List<MovementItem>();
        foreach (DataGridViewRow row in _lines.Rows)
        {
            if (row.IsNewRow) continue;
            var prefix = $"Строка {row.Index + 1}: ";
            if (row.Cells["ShoeColumn"].Value is not Guid shoeId) throw new ArgumentException(prefix + "выберите обувь.");
            if (row.Cells["SizeColumn"].Value is not Guid sizeId) throw new ArgumentException(prefix + "выберите размер.");
            if (!int.TryParse(Convert.ToString(row.Cells["QuantityColumn"].Value), out var quantity) || quantity <= 0)
                throw new ArgumentException(prefix + "количество должно быть положительным целым числом.");
            if (!decimal.TryParse(Convert.ToString(row.Cells["UnitPriceColumn"].Value), NumberStyles.Number, CultureInfo.CurrentCulture, out var price) || price <= 0 || decimal.Round(price, 2) != price)
                throw new ArgumentException(prefix + "цена должна быть положительной, не более двух знаков после запятой.");
            items.Add(MovementItem.Create(Guid.NewGuid(), id, shoeId, sizeId, quantity, price));
        }
        if (items.Count == 0) throw new ArgumentException("Добавьте хотя бы одну позицию товара.");
        _movements.Add(StockMovement.Create(id, _date.Value.Date, (MovementType)_type.SelectedValue!, items));
        DialogResult = DialogResult.OK; Close();
    }
}
