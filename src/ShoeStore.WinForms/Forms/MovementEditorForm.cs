using ShoeStore.WinForms.Entities.Enums;
using ShoeStore.WinForms.Entities;
using ShoeStore.WinForms.Repositories;
using ShoeStore.WinForms.Forms;
using System.Globalization;

namespace ShoeStore.WinForms.Forms;

public sealed partial class MovementEditorForm : Form
{
    private readonly IStockMovementRepository _movements = null!;
    private readonly Dictionary<Guid, decimal> _prices = null!;

    public MovementEditorForm()
    {
        InitializeComponent();
    }

    public MovementEditorForm(
        IStockMovementRepository movements,
        IShoeRepository shoes,
        IShoeSizeRepository sizes
    ) : this()
    {
        _movements = movements;
        var shoeList = shoes.GetAll().Select(x => new
        {
            x.Id,
            Label = $"{x.Article} — {x.Name}",
            x.Price
        }).ToList();
        var sizeList = sizes.GetAll().OrderBy(x => x.Value).ToList();
        _prices = shoeList.ToDictionary(x => x.Id, x => x.Price);
        _date.MaxDate = DateTime.Today;
        _type.DataSource = Enum.GetValues<MovementType>()
            .Select(
            x => new
            {
                Value = x,
                Label = Ui.Movement(x)
            }
        )
            .ToList();
        ShoeColumn.DataSource = shoeList;
        SizeColumn.DataSource = sizeList;
        _lines.DataError += (_, e) =>
        {
            e.ThrowException = false;
            MessageBox.Show(
                this,
                "Некорректное значение. Количество — целое число, цена — число с не более чем двумя знаками после запятой.",
                "Ошибка ввода",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        };
        _lines.DefaultValuesNeeded += (_, e) =>
        {
            e.Row.Cells["QuantityColumn"].Value = 1;
            e.Row.Cells["UnitPriceColumn"].Value = 0m;
        };
        _lines.CurrentCellDirtyStateChanged += (_, _) =>
        {
            if (_lines.IsCurrentCellDirty && _lines.CurrentCell is DataGridViewComboBoxCell)
                _lines.CommitEdit(DataGridViewDataErrorContexts.Commit);
        };
        _lines.CellValueChanged += (_, e) =>
        {
            if (e.RowIndex >= 0
                && e.ColumnIndex == 0
                && _lines.Rows[e.RowIndex].Cells[0].Value is Guid id
                && _prices.TryGetValue(id, out var price))
                _lines.Rows[e.RowIndex].Cells["UnitPriceColumn"].Value = price;
            UpdateTotal();
        };
        _lines.RowsRemoved += (_, _) => UpdateTotal();
        cancelButton.Click += (_, _) =>
        {
            DialogResult = DialogResult.Cancel;
            Close();
        };
        saveMovementButton.Click += (_, _) => Ui.Run(this, SaveMovement);
        removeLineButton.Click += (_, _) => Ui.Run(this, () =>
        {
            if (_lines.CurrentRow is { IsNewRow: false } row)
                _lines.Rows.Remove(row);
        });
    }

    private void UpdateTotal()
    {
        decimal total = 0;
        foreach (DataGridViewRow row in _lines.Rows)
            if (!row.IsNewRow
                && decimal.TryParse(
                Convert.ToString(row.Cells["QuantityColumn"].Value),
                out var quantity
            )
                && decimal.TryParse(Convert.ToString(row.Cells["UnitPriceColumn"].Value), out var price))
                total += quantity * price;
        _total.Text = $"Итого: {total:N2} ₽";
    }

    private void SaveMovement()
    {
        if (!_lines.EndEdit())
            throw new ArgumentException("Исправьте значения в таблице.");
        var id = Guid.NewGuid();
        var items = new List<MovementItem>();
        foreach (DataGridViewRow row in _lines.Rows)
        {
            if (row.IsNewRow)
                continue;
            var prefix = $"Строка {row.Index + 1}: ";
            if (row.Cells["ShoeColumn"].Value is not Guid shoeId)
                throw new ArgumentException(prefix + "выберите обувь.");
            if (row.Cells["SizeColumn"].Value is not Guid sizeId)
                throw new ArgumentException(prefix + "выберите размер.");
            if (!int.TryParse(Convert.ToString(row.Cells["QuantityColumn"].Value), out var quantity) || quantity <= 0)
                throw new ArgumentException(prefix + "количество должно быть положительным целым числом.");
            if (!decimal.TryParse(
                Convert.ToString(row.Cells["UnitPriceColumn"].Value),
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out var price
            )
                || price <= 0
                || decimal.Round(price, 2) != price)
                throw new ArgumentException(
                    prefix + "цена должна быть положительной, не более двух знаков после запятой."
                );
            items.Add(MovementItem.Create(Guid.NewGuid(), id, shoeId, sizeId, quantity, price));
        }

        if (items.Count == 0)
            throw new ArgumentException("Добавьте хотя бы одну позицию товара.");
        _movements.Add(StockMovement.Create(id, _date.Value.Date, (MovementType)_type.SelectedValue!, items));
        DialogResult = DialogResult.OK;
        Close();
    }
}
