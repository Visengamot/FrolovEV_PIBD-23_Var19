using ShoeStore.Core.Models;
using ShoeStore.Core.Repositories;

namespace ShoeStore.WinForms;

public abstract class CatalogForm : Form
{
    protected readonly DataGridView ItemsGrid;
    protected CatalogForm(string title)
    {
        Text = title; Name = GetType().Name; StartPosition = FormStartPosition.CenterParent;
        Size = new Size(1050, 600); MinimumSize = new Size(700, 400);
        ItemsGrid = Ui.Grid("catalogGrid");
        var toolbar = new FlowLayoutPanel { Name = "catalogToolbar", Dock = DockStyle.Top, Height = 52, Padding = new Padding(10) };
        toolbar.Controls.Add(Ui.Button("addButton", "Добавить", () => Ui.Run(this, AddItem)));
        toolbar.Controls.Add(Ui.Button("editButton", "Изменить", () => Ui.Run(this, EditItem)));
        toolbar.Controls.Add(Ui.Button("deleteButton", "Удалить", () => Ui.Run(this, DeleteSelected)));
        toolbar.Controls.Add(Ui.Button("refreshButton", "Обновить", () => Ui.Run(this, Reload)));
        Controls.Add(ItemsGrid); Controls.Add(toolbar);
        Shown += (_, _) => Ui.Run(this, Reload);
        ItemsGrid.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) Ui.Run(this, EditItem); };
    }
    protected Guid SelectedId()
    {
        if (ItemsGrid.CurrentRow?.Cells["IdColumn"].Value is Guid id) return id;
        throw new InvalidOperationException("Выберите запись в таблице.");
    }
    protected void EditWith(Form editor)
    {
        using (editor) if (editor.ShowDialog(this) == DialogResult.OK) Reload();
    }
    private void DeleteSelected()
    {
        var id = SelectedId();
        if (MessageBox.Show(this, "Удалить выбранную запись?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        DeleteItem(id); Reload();
    }
    protected void IdColumn() { Ui.Column(ItemsGrid, "Id", "Код"); ItemsGrid.Columns["IdColumn"].Visible = false; }
    protected abstract void Reload();
    protected abstract void AddItem();
    protected abstract void EditItem();
    protected abstract void DeleteItem(Guid id);
}

public sealed class ManufacturersForm : CatalogForm
{
    private readonly IManufacturerRepository _repository;
    public ManufacturersForm(IManufacturerRepository repository) : base("Производители")
    {
        _repository = repository; IdColumn(); Ui.Column(ItemsGrid, "Name", "Название"); Ui.Column(ItemsGrid, "Country", "Страна");
    }
    protected override void Reload() => ItemsGrid.DataSource = _repository.GetAll().ToList();
    protected override void AddItem() => EditWith(new ManufacturerEditorForm(_repository));
    protected override void EditItem() => EditWith(new ManufacturerEditorForm(_repository, _repository.GetById(SelectedId()) ?? throw new InvalidOperationException("Запись не найдена.")));
    protected override void DeleteItem(Guid id) => _repository.Delete(id);
}

public sealed class ShoeSizesForm : CatalogForm
{
    private readonly IShoeSizeRepository _repository;
    public ShoeSizesForm(IShoeSizeRepository repository) : base("Размеры")
    {
        _repository = repository; IdColumn(); Ui.Column(ItemsGrid, "Value", "Размер", "0.#");
    }
    protected override void Reload() => ItemsGrid.DataSource = _repository.GetAll().OrderBy(x => x.Value).ToList();
    protected override void AddItem() => EditWith(new ShoeSizeEditorForm(_repository));
    protected override void EditItem() => EditWith(new ShoeSizeEditorForm(_repository, _repository.GetById(SelectedId()) ?? throw new InvalidOperationException("Запись не найдена.")));
    protected override void DeleteItem(Guid id) => _repository.Delete(id);
}

public sealed class ShoesForm : CatalogForm
{
    private readonly IShoeRepository _repository;
    private readonly IManufacturerRepository _manufacturers;
    public ShoesForm(IShoeRepository repository, IManufacturerRepository manufacturers) : base("Ассортимент обуви")
    {
        _repository = repository; _manufacturers = manufacturers; IdColumn();
        Ui.Column(ItemsGrid, "Article", "Артикул"); Ui.Column(ItemsGrid, "Name", "Название");
        Ui.Column(ItemsGrid, "Manufacturer", "Производитель"); Ui.Column(ItemsGrid, "Price", "Цена, ₽", "N2");
        Ui.Column(ItemsGrid, "Category", "Категория"); Ui.Column(ItemsGrid, "Seasons", "Сезоны");
    }
    protected override void Reload()
    {
        var manufacturers = _manufacturers.GetAll().ToDictionary(x => x.Id, x => x.Name);
        ItemsGrid.DataSource = _repository.GetAll().Select(x => new
        {
            x.Id,
            x.Article,
            x.Name,
            Manufacturer = manufacturers.GetValueOrDefault(x.ManufacturerId, "—"),
            x.Price,
            Category = Ui.Category(x.Category),
            Seasons = Ui.Seasons(x.Seasons)
        }).ToList();
    }
    protected override void AddItem() => EditWith(new ShoeEditorForm(_repository, _manufacturers));
    protected override void EditItem() => EditWith(new ShoeEditorForm(_repository, _manufacturers, _repository.GetById(SelectedId()) ?? throw new InvalidOperationException("Запись не найдена.")));
    protected override void DeleteItem(Guid id) => _repository.Delete(id);
}
