using ShoeStore.WinForms.Reports;

namespace ShoeStore.WinForms.Forms;

public partial class MovementReportForm : Form
{
    private readonly MovementReport _report = null!;

    public MovementReportForm()
    {
        InitializeComponent();
    }

    public MovementReportForm(MovementReport report) : this()
    {
        ArgumentNullException.ThrowIfNull(report);
        _report = report;
        var today = DateTime.Today;
        fromDatePicker.Value = new DateTime(today.Year, today.Month, 1);
        toDatePicker.Value = today;
        saveButton.Click += SaveButton_Click;
    }

    private void SaveButton_Click(object? sender, EventArgs e) =>
        Ui.Run(this, () =>
        {
            var from = fromDatePicker.Value.Date;
            var to = toDatePicker.Value.Date;
            if (from > to)
            {
                fromDatePicker.Focus();
                throw new ArgumentException("Начало периода не может быть позже конца.");
            }

            using var dialog = new SaveFileDialog
            {
                Title = "Сохранение движения товаров в Excel",
                Filter = "Книга Excel (*.xlsx)|*.xlsx",
                DefaultExt = "xlsx",
                AddExtension = true,
                FileName = "Движение товаров.xlsx",
                OverwritePrompt = true,
                RestoreDirectory = true
            };
            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;
            if (!string.Equals(Path.GetExtension(dialog.FileName), ".xlsx", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Имя файла должно иметь расширение .xlsx.");

            _report.Save(dialog.FileName, from, to);
            MessageBox.Show(this, $"Отчёт сохранён:\n{dialog.FileName}", "Готово",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        });
}
