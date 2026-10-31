using ShoeStore.Core.Models;
using ShoeStore.Core.Repositories.Interfaces;
using ShoeStore.WinForms.Forms.Base;
using ShoeStore.WinForms.Helpers;

namespace ShoeStore.WinForms.Forms;

public sealed class ManufacturerEditorForm : EditorForm
{
    private readonly IManufacturerRepository _repository;
    private readonly Guid _id;
    private readonly bool _editing;
    private readonly TextBox _name = new() { Name = "manufacturerNameTextBox", MaxLength = 200 };
    private readonly TextBox _country = new() { Name = "countryTextBox", MaxLength = 100 };

    public ManufacturerEditorForm(IManufacturerRepository repository, Manufacturer? manufacturer = null)
        : base(manufacturer is null ? "Добавление производителя" : "Изменение производителя")
    {
        _repository = repository; _id = manufacturer?.Id ?? Guid.NewGuid(); _editing = manufacturer is not null;
        _name.Text = manufacturer?.Name ?? ""; _country.Text = manufacturer?.Country ?? "";
        Field("Название", _name); Field("Страна", _country); ClientSize = new Size(510, 190);
    }

    protected override void Save()
    {
        var item = Manufacturer.Create(_id, Ui.Required(_name, "Название"), Ui.Required(_country, "Страна"));
        if (_editing) _repository.Update(item); else _repository.Add(item);
    }
}
