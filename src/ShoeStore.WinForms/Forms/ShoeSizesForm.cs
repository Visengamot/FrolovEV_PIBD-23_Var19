using ShoeStore.WinForms.Repositories;
using ShoeStore.WinForms.Forms;

namespace ShoeStore.WinForms.Forms;

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
