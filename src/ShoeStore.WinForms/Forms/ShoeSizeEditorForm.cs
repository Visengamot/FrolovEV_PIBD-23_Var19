using ShoeStore.WinForms.Entities;
using ShoeStore.WinForms.Repositories;

namespace ShoeStore.WinForms.Forms;

public partial class ShoeSizeEditorForm : Form
{
    private readonly IShoeSizeRepository? _repository;
    private readonly Guid _id;
    private readonly bool _editing;

    public ShoeSizeEditorForm()
    {
        InitializeComponent();
    }

    public ShoeSizeEditorForm(IShoeSizeRepository repository, ShoeSize? size = null)
        : this()
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
        _id = size?.Id ?? Guid.NewGuid();
        _editing = size is not null;
        Text = _editing ? "Изменение размера" : "Добавление размера";
        sizeNumericUpDown.Value = size?.Value ?? 36;
        saveButton.Click += SaveButton_Click;
    }

    private void SaveButton_Click(object? sender, EventArgs e)
    {
        Ui.Run(this, () =>
        {
            var item = ShoeSize.Create(_id, sizeNumericUpDown.Value);
            if (_editing) _repository!.Update(item); else _repository!.Add(item);
            DialogResult = DialogResult.OK;
            Close();
        });
    }
}
