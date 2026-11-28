using ShoeStore.WinForms.Repositories;

namespace ShoeStore.WinForms.Forms;

public partial class ShoeSizesForm : Form
{
    private readonly IShoeSizeRepository _repository = null!;

    public ShoeSizesForm()
    {
        InitializeComponent();
    }

    public ShoeSizesForm(IShoeSizeRepository repository) : this()
    {
        _repository = repository;
        AttachRuntimeEvents();
    }

    private void Reload() =>
        ItemsGrid.DataSource = _repository.GetAll().OrderBy(x => x.Value).ToList();

    private void AddItem() =>
        EditWith(new ShoeSizeEditorForm(_repository));

    private void EditItem() =>
        EditWith(
        new ShoeSizeEditorForm(
        _repository,
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
