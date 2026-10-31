using ShoeStore.WinForms.Helpers;

namespace ShoeStore.WinForms.Forms.Base;

public abstract class EditorForm : Form
{
    protected readonly TableLayoutPanel Fields;
    private readonly FlowLayoutPanel _buttons;

    protected EditorForm(string title)
    {
        Text = title; Name = GetType().Name; StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false; MinimizeBox = false;
        AutoScaleMode = AutoScaleMode.Font; ClientSize = new Size(510, 380);
        Fields = new TableLayoutPanel { Name = "editorFields", Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(16), AutoScroll = true };
        Fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 155));
        Fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 58, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(10) };
        var cancel = Ui.Button("cancelButton", "Отмена", () => { DialogResult = DialogResult.Cancel; Close(); });
        var save = Ui.Button("saveButton", "Сохранить", () => Ui.Run(this, () => { Save(); DialogResult = DialogResult.OK; Close(); }));
        _buttons.Controls.Add(cancel); _buttons.Controls.Add(save);
        Controls.Add(Fields); Controls.Add(_buttons); AcceptButton = save; CancelButton = cancel;
    }

    protected void Field(string caption, Control control, int height = 42)
    {
        int row = Fields.RowCount++;
        Fields.RowStyles.Add(new RowStyle(SizeType.Absolute, height));
        Fields.Controls.Add(new Label { Text = caption, AutoSize = true, Anchor = AnchorStyles.Left }, 0, row);
        control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        Fields.Controls.Add(control, 1, row);
    }

    protected static NumericUpDown Number(string name, decimal max, int decimals = 0) => new()
    { Name = name, Minimum = 0, Maximum = max, DecimalPlaces = decimals, ThousandsSeparator = true };
    protected abstract void Save();
}
