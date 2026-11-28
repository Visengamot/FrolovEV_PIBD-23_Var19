using ShoeStore.WinForms.Entities;
using ShoeStore.WinForms.Repositories;

namespace ShoeStore.WinForms.Forms;

public partial class ManufacturerEditorForm : Form
{
    private readonly IManufacturerRepository? _repository;
    private readonly Guid _id;
    private readonly bool _editing;

    public ManufacturerEditorForm()
    {
        InitializeComponent();
    }

    public ManufacturerEditorForm(IManufacturerRepository repository, Manufacturer? manufacturer = null) : this()
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
        _id = manufacturer?.Id ?? Guid.NewGuid();
        _editing = manufacturer is not null;
        Text = _editing ? "Изменение производителя" : "Добавление производителя";
        manufacturerNameTextBox.Text = manufacturer?.Name ?? "";
        countryTextBox.Text = manufacturer?.Country ?? "";
        saveButton.Click += SaveButton_Click;
    }

    private void SaveButton_Click(object? sender, EventArgs e)
    {
        Ui.Run(this, () =>
        {
            var item = Manufacturer.Create(
                _id,
                Ui.Required(manufacturerNameTextBox, "Название"),
                Ui.Required(countryTextBox, "Страна")
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
