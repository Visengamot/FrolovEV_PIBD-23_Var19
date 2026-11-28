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
        var today = DateTime.Today;
        salesDatePicker.Value = new DateTime(today.Year, today.Month, 1);
        endDatePicker.Value = today;
        periodCheckBox.CheckedChanged += (_, _) => UpdateDateFields();
        UpdateDateFields();
        saveButton.Click += SaveButton_Click;
    }

    private void UpdateDateFields()
    {
        salesDateLabel.Text = periodCheckBox.Checked ? "Дата начала:" : "Дата продаж:";
        endDateLabel.Enabled = periodCheckBox.Checked;
        endDatePicker.Enabled = periodCheckBox.Checked;
    }

    private void SaveButton_Click(object? sender, EventArgs e) =>
        Ui.Run(this, () =>
        {
            var from = salesDatePicker.Value.Date;
            var to = periodCheckBox.Checked ? endDatePicker.Value.Date : from;
            if (from > to)
            {
                endDatePicker.Focus();
                throw new ArgumentException("Начало периода не может быть позже конца.");
            }

            using var dialog = new SaveFileDialog
            {
                Title = "Сохранение диаграммы продаж в PDF",
                Filter = "Документ PDF (*.pdf)|*.pdf",
                DefaultExt = "pdf",
                AddExtension = true,
                InitialDirectory = Ui.ReportDirectory(),
                FileName = "Диаграмма продаж.pdf",
                OverwritePrompt = true,
                RestoreDirectory = true
            };
            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;
            if (!string.Equals(Path.GetExtension(dialog.FileName), ".pdf", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Имя файла должно иметь расширение .pdf.");

            _report.Save(dialog.FileName, from, to);
            MessageBox.Show(this, $"Отчёт сохранён:\n{dialog.FileName}", "Готово",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        });
}
