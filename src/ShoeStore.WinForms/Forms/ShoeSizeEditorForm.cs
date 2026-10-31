using ShoeStore.WinForms.Entities;
using ShoeStore.WinForms.Repositories;
using ShoeStore.WinForms.Forms;

namespace ShoeStore.WinForms.Forms;

public sealed class ShoeSizeEditorForm : EditorForm
{
    private readonly IShoeSizeRepository _repository;
    private readonly Guid _id;
    private readonly bool _editing;
    private readonly NumericUpDown _size = Number("sizeNumericUpDown", 100, 1);

    public ShoeSizeEditorForm(IShoeSizeRepository repository, ShoeSize? size = null)
        : base(size is null ? "Добавление размера" : "Изменение размера")
    {
        _repository = repository; _id = size?.Id ?? Guid.NewGuid(); _editing = size is not null;
        _size.Minimum = 10; _size.Maximum = 60; _size.Value = size?.Value ?? 36; _size.Increment = 0.5m;
        Field("Размер", _size); ClientSize = new Size(460, 145);
    }

    protected override void Save()
    {
        var item = ShoeSize.Create(_id, _size.Value);
        if (_editing) _repository.Update(item); else _repository.Add(item);
    }
}
