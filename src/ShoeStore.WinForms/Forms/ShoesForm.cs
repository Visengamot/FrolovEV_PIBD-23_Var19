using ShoeStore.WinForms.Repositories;

namespace ShoeStore.WinForms.Forms;

public partial class ShoesForm : Form
{
    private readonly IShoeRepository _repository = null!;
    private readonly IManufacturerRepository _manufacturers = null!;

    public ShoesForm()
    {
        InitializeComponent();
    }

    public ShoesForm(IShoeRepository repository, IManufacturerRepository manufacturers) : this()
    {
        _repository = repository;
        _manufacturers = manufacturers;
        AttachRuntimeEvents();
    }

    private void Reload()
    {
        var manufacturers = _manufacturers.GetAll().ToDictionary(x => x.Id, x => x.Name);
        ItemsGrid.DataSource = _repository.GetAll()
            .Select(
            x => new
            {
                x.Id,
                x.Article,
                x.Name,
                Manufacturer = manufacturers.GetValueOrDefault(x.ManufacturerId, "—"),
                x.Price,
                Category = Ui.Category(x.Category),
                Seasons = Ui.Seasons(x.Seasons)
            }
        )
            .ToList();
    }

    private void AddItem() =>
        EditWith(new ShoeEditorForm(_repository, _manufacturers));

    private void EditItem() =>
        EditWith(
        new ShoeEditorForm(
        _repository,
        _manufacturers,
        _repository.GetById(SelectedId())
        ?? throw new InvalidOperationException("Запись не найдена.")
    )
    );

    private void DeleteItem(Guid id) =>
        _repository.Delete(id);
    // Only dependency-injected instances subscribe to runtime data operations.
    private void AttachRuntimeEvents()
    {
        Shown += CatalogShown;
        addButton.Click += AddButtonClick;
        editButton.Click += EditButtonClick;
        deleteButton.Click += DeleteButtonClick;
        refreshButton.Click += RefreshButtonClick;
        ItemsGrid.CellDoubleClick += ItemsGridCellDoubleClick;
    }

    private void CatalogShown(object? sender, EventArgs e) =>
        Ui.Run(this, Reload);

    private void AddButtonClick(object? sender, EventArgs e) =>
        Ui.Run(this, AddItem);

    private void EditButtonClick(object? sender, EventArgs e) =>
        Ui.Run(this, EditItem);

    private void DeleteButtonClick(object? sender, EventArgs e) =>
        Ui.Run(this, DeleteSelected);

    private void RefreshButtonClick(object? sender, EventArgs e) =>
        Ui.Run(this, Reload);

    private void ItemsGridCellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            Ui.Run(this, EditItem);
    }

    private Guid SelectedId()
    {
        if (ItemsGrid.CurrentRow?.Cells["IdColumn"].Value is Guid id)
            return id;
        throw new InvalidOperationException("Выберите запись в таблице.");
    }

    private void EditWith(Form editor)
    {
        using (editor)
        {
            if (editor.ShowDialog(this) == DialogResult.OK)
                Reload();
        }
    }

    private void DeleteSelected()
    {
        var id = SelectedId();
        if (MessageBox.Show(
            this,
            "Удалить выбранную запись?",
            "Подтверждение удаления",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2
        ) != DialogResult.Yes)
            return;
        DeleteItem(id);
        Reload();
    }
}
