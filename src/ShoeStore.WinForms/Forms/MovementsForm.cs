using ShoeStore.WinForms.Repositories;

namespace ShoeStore.WinForms.Forms;

public sealed partial class MovementsForm : Form
{
    private readonly IStockMovementRepository _movements = null!;
    private readonly IShoeRepository _shoes = null!;
    private readonly IShoeSizeRepository _sizes = null!;

    public MovementsForm()
    {
        InitializeComponent();
    }

    public MovementsForm(IStockMovementRepository movements, IShoeRepository shoes, IShoeSizeRepository sizes) : this()
    {
        _movements = movements;
        _shoes = shoes;
        _sizes = sizes;
        addMovementButton.Click += AddMovement;
        refreshMovementsButton.Click += RefreshMovements;
        _operations.SelectionChanged += SelectionChanged;
        Shown += FormShown;
    }

    private void AddMovement(object? sender, EventArgs e) =>
        Ui.Run(this, () =>
    {
        using var editor = new MovementEditorForm(_movements, _shoes, _sizes);
        if (editor.ShowDialog(this) == DialogResult.OK)
            Reload();
    });

    private void RefreshMovements(object? sender, EventArgs e) =>
        Ui.Run(this, Reload);

    private void SelectionChanged(object? sender, EventArgs e) =>
        Ui.Run(this, LoadItems);

    private void FormShown(object? sender, EventArgs e)
    {
        movementSplitContainer.SplitterDistance = Math.Max(150, movementSplitContainer.Height / 2);
        Ui.Run(this, Reload);
    }

    private void Reload() =>
        _operations.DataSource = _movements.GetAll()
        .OrderByDescending(x => x.Date)
        .Select(
        x => new
        {
            x.Id,
            x.Date,
            Type = Ui.Movement(x.Type),
            Lines = x.Items.Count,
            Quantity = x.Items.Sum(i => i.Quantity),
            x.Total
        }
    )
        .ToList();

    private void LoadItems()
    {
        if (_operations.CurrentRow?.Cells["IdColumn"].Value is not Guid id)
        {
            _items.DataSource = null;
            return;
        }

        var movement = _movements.GetById(id);
        var shoes = _shoes.GetAll().ToDictionary(x => x.Id);
        var sizes = _sizes.GetAll().ToDictionary(x => x.Id);
        _items.DataSource = movement?.Items.Select(
            x => new
            {
                Article = shoes.TryGetValue(x.ShoeId, out var shoe)
            ? shoe.Article
            : "—",
                Shoe = shoes.TryGetValue(x.ShoeId, out var name)
            ? name.Name
            : "—",
                Size = sizes.TryGetValue(x.ShoeSizeId, out var size)
            ? size.Value
            : 0,
                x.Quantity,
                x.UnitPrice,
                x.Total
            }
        )
            .ToList();
    }
}
