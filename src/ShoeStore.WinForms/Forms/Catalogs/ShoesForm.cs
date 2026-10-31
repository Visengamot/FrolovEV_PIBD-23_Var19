using ShoeStore.Core.Models;
using ShoeStore.Core.Repositories.Interfaces;
using ShoeStore.WinForms.Forms.Base;
using ShoeStore.WinForms.Helpers;

namespace ShoeStore.WinForms.Forms;

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
