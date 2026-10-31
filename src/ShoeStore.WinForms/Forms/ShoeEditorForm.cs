using ShoeStore.WinForms.Entities.Enums;
using ShoeStore.WinForms.Entities;
using ShoeStore.WinForms.Repositories;
using ShoeStore.WinForms.Forms;

namespace ShoeStore.WinForms.Forms;

public sealed class ShoeEditorForm : EditorForm
{
    private readonly IShoeRepository _repository;
    private readonly Guid _id;
    private readonly bool _editing;
    private readonly TextBox _article = new() { Name = "articleTextBox", MaxLength = 100 };
    private readonly TextBox _name = new() { Name = "shoeNameTextBox", MaxLength = 200 };
    private readonly ComboBox _manufacturer = new() { Name = "manufacturerComboBox", DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "Name", ValueMember = "Id" };
    private readonly ComboBox _category = new() { Name = "categoryComboBox", DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "Label", ValueMember = "Value" };
    private readonly NumericUpDown _price = Number("priceNumericUpDown", 100000000, 2);
    private readonly CheckedListBox _seasons = new() { Name = "seasonsCheckedListBox", CheckOnClick = true, Height = 88 };
    private static readonly Season[] SeasonValues = [Season.Spring, Season.Summer, Season.Autumn, Season.Winter];

    public ShoeEditorForm(IShoeRepository repository, IManufacturerRepository manufacturers, Shoe? shoe = null)
        : base(shoe is null ? "Добавление обуви" : "Изменение обуви")
    {
        _repository = repository; _id = shoe?.Id ?? Guid.NewGuid(); _editing = shoe is not null;
        _article.Text = shoe?.Article ?? ""; _name.Text = shoe?.Name ?? ""; _price.Value = shoe?.Price ?? 0;
        _manufacturer.DataSource = manufacturers.GetAll().ToList();
        if (shoe is not null) _manufacturer.SelectedValue = shoe.ManufacturerId;
        _category.DataSource = Enum.GetValues<FootwearCategory>().Select(x => new { Value = x, Label = Ui.Category(x) }).ToList();
        if (shoe is not null) _category.SelectedValue = shoe.Category;
        _seasons.Items.AddRange(["Весна", "Лето", "Осень", "Зима"]);
        for (int i = 0; i < SeasonValues.Length; i++) _seasons.SetItemChecked(i, (shoe?.Seasons ?? Season.All).HasFlag(SeasonValues[i]));
        Field("Артикул", _article); Field("Название", _name); Field("Производитель", _manufacturer);
        Field("Цена, ₽", _price); Field("Категория", _category); Field("Сезоны", _seasons, 105);
        ClientSize = new Size(550, 410);
    }

    protected override void Save()
    {
        if (_manufacturer.SelectedValue is not Guid manufacturerId) throw new ArgumentException("Выберите производителя. Сначала заполните справочник производителей.");
        var seasons = Season.None;
        foreach (int index in _seasons.CheckedIndices) seasons |= SeasonValues[index];
        if (seasons == Season.None) throw new ArgumentException("Выберите хотя бы один сезон.");
        var item = Shoe.Create(_id, Ui.Required(_article, "Артикул"), Ui.Required(_name, "Название"), manufacturerId,
            _price.Value, (FootwearCategory)_category.SelectedValue!, seasons);
        if (_editing) _repository.Update(item); else _repository.Add(item);
    }
}
