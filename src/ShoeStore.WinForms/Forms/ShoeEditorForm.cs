using ShoeStore.WinForms.Entities.Enums;
using ShoeStore.WinForms.Entities;
using ShoeStore.WinForms.Repositories;

namespace ShoeStore.WinForms.Forms;

public partial class ShoeEditorForm : Form
{
    private readonly IShoeRepository? _repository;
    private readonly Guid _id;
    private readonly bool _editing;
    private static readonly Season[] SeasonValues = [Season.Spring, Season.Summer, Season.Autumn, Season.Winter];

    public ShoeEditorForm()
    {
        InitializeComponent();
    }

    public ShoeEditorForm(IShoeRepository repository, IManufacturerRepository manufacturers, Shoe? shoe = null) : this()
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(manufacturers);
        _repository = repository;
        _id = shoe?.Id ?? Guid.NewGuid();
        _editing = shoe is not null;
        Text = _editing ? "Изменение обуви" : "Добавление обуви";
        articleTextBox.Text = shoe?.Article ?? "";
        shoeNameTextBox.Text = shoe?.Name ?? "";
        priceNumericUpDown.Value = shoe?.Price ?? 0;
        manufacturerComboBox.DataSource = manufacturers.GetAll().ToList();
        if (shoe is not null)
            manufacturerComboBox.SelectedValue = shoe.ManufacturerId;
        categoryComboBox.DataSource = Enum.GetValues<FootwearCategory>()
            .Select(
            x => new
            {
                Value = x,
                Label = Ui.Category(x)
            }
        )
            .ToList();
        if (shoe is not null)
            categoryComboBox.SelectedValue = shoe.Category;
        for (int i = 0; i < SeasonValues.Length; i++)
            seasonsCheckedListBox.SetItemChecked(i, (shoe?.Seasons ?? Season.All).HasFlag(SeasonValues[i]));
        saveButton.Click += SaveButton_Click;
    }

    private void SaveButton_Click(object? sender, EventArgs e)
    {
        Ui.Run(this, () =>
        {
            if (manufacturerComboBox.SelectedValue is not Guid manufacturerId)
                throw new ArgumentException("Выберите производителя. Сначала заполните справочник производителей.");
            var seasons = Season.None;
            foreach (int index in seasonsCheckedListBox.CheckedIndices)
                seasons |= SeasonValues[index];
            if (seasons == Season.None)
                throw new ArgumentException("Выберите хотя бы один сезон.");
            if (priceNumericUpDown.Value <= 0)
                throw new ArgumentException("Цена должна быть положительной.");
            if (categoryComboBox.SelectedValue is not FootwearCategory category)
                throw new ArgumentException("Выберите категорию.");
            var item = Shoe.Create(
                _id,
                Ui.Required(articleTextBox, "Артикул"),
                Ui.Required(shoeNameTextBox, "Название"),
                manufacturerId,
                priceNumericUpDown.Value,
                category,
                seasons
            );
            if (_editing)
                _repository!.Update(item);
            else
                _repository!.Add(item);
            DialogResult = DialogResult.OK;
            Close();
        });
    }
}
