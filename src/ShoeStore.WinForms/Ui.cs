using ShoeStore.Core.Models;

namespace ShoeStore.WinForms;

internal static class Ui
{
    public static void Error(IWin32Window owner, Exception exception) => MessageBox.Show(owner,
        exception.GetBaseException().Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);

    public static void Run(IWin32Window owner, Action action)
    {
        try { action(); }
        catch (Exception exception) { Error(owner, exception); }
    }

    public static DataGridView Grid(string name, bool readOnly = true) => new()
    {
        Name = name,
        Dock = DockStyle.Fill,
        AutoGenerateColumns = false,
        ReadOnly = readOnly,
        AllowUserToAddRows = !readOnly,
        AllowUserToDeleteRows = !readOnly,
        SelectionMode = readOnly ? DataGridViewSelectionMode.FullRowSelect : DataGridViewSelectionMode.CellSelect,
        MultiSelect = false,
        RowHeadersVisible = false,
        BackgroundColor = Color.White,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        BorderStyle = BorderStyle.FixedSingle
    };

    public static void Column(DataGridView grid, string property, string title, string? format = null)
    {
        var column = new DataGridViewTextBoxColumn { Name = property + "Column", DataPropertyName = property, HeaderText = title };
        if (format is not null) column.DefaultCellStyle.Format = format;
        grid.Columns.Add(column);
    }

    public static Button Button(string name, string text, Action click)
    {
        var button = new Button { Name = name, Text = text, AutoSize = true, Height = 32, MinimumSize = new Size(100, 32) };
        button.Click += (_, _) => click();
        return button;
    }

    public static string Required(TextBox input, string label)
    {
        if (string.IsNullOrWhiteSpace(input.Text))
        {
            input.Focus();
            throw new ArgumentException($"Заполните поле «{label}».");
        }
        return input.Text.Trim();
    }

    public static string Category(FootwearCategory category) => category switch
    {
        FootwearCategory.Sneakers => "Кроссовки",
        FootwearCategory.Boots => "Сапоги / ботинки",
        FootwearCategory.Shoes => "Туфли",
        FootwearCategory.Sandals => "Сандалии",
        FootwearCategory.Slippers => "Тапочки",
        _ => category.ToString()
    };

    public static string Seasons(Season seasons)
    {
        if (seasons == Season.All) return "Все сезоны";
        return string.Join(", ", new[] { (Season.Spring, "Весна"), (Season.Summer, "Лето"), (Season.Autumn, "Осень"), (Season.Winter, "Зима") }
            .Where(x => seasons.HasFlag(x.Item1)).Select(x => x.Item2));
    }

    public static string Movement(MovementType type) => type == MovementType.Receipt ? "Приход" : "Продажа";
}

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
