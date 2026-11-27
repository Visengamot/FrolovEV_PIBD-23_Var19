using ShoeStore.WinForms.Reports;

namespace ShoeStore.WinForms.Forms;

public partial class SalesChartForm : Form
{
    private readonly SalesChartReport _report = null!;

    public SalesChartForm()
    {
        InitializeComponent();
    }

    public SalesChartForm(SalesChartReport report) : this()
    {
        ArgumentNullException.ThrowIfNull(report);
        _report = report;
        salesDatePicker.Value = DateTime.Today;
        saveButton.Click += SaveButton_Click;
    }

    private void SaveButton_Click(object? sender, EventArgs e) =>
        Ui.Run(this, () =>
        {
            var date = salesDatePicker.Value.Date;
            using var dialog = new SaveFileDialog
            {
                Title = "Сохранение диаграммы продаж в PDF",
                Filter = "Документ PDF (*.pdf)|*.pdf",
                DefaultExt = "pdf",
                AddExtension = true,
                FileName = "Диаграмма продаж.pdf",
                OverwritePrompt = true,
                RestoreDirectory = true
            };
            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;
            if (!string.Equals(Path.GetExtension(dialog.FileName), ".pdf", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Имя файла должно иметь расширение .pdf.");

            _report.Save(dialog.FileName, date);
            MessageBox.Show(this, $"Отчёт сохранён:\n{dialog.FileName}", "Готово",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        });
}
