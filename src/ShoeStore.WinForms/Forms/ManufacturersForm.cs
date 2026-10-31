using ShoeStore.WinForms.Repositories;
using ShoeStore.WinForms.Forms;

namespace ShoeStore.WinForms.Forms;

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
