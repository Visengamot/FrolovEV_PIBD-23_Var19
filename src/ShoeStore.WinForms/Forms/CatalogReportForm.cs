using ShoeStore.WinForms.Reports;

namespace ShoeStore.WinForms.Forms;

public partial class CatalogReportForm : Form
{
    private readonly CatalogReport _report = null!;

    public CatalogReportForm()
    {
        InitializeComponent();
    }

    public CatalogReportForm(CatalogReport report) : this()
    {
        ArgumentNullException.ThrowIfNull(report);
        _report = report;
        saveButton.Click += SaveButton_Click;
    }

    private void SaveButton_Click(object? sender, EventArgs e) =>
        Ui.Run(this, () =>
        {
            var manufacturers = manufacturersCheckBox.Checked;
            var shoes = shoesCheckBox.Checked;
            var sizes = sizesCheckBox.Checked;
            if (!manufacturers && !shoes && !sizes)
            {
                manufacturersCheckBox.Focus();
                throw new InvalidOperationException("Выберите хотя бы один справочник.");
            }

            using var dialog = new SaveFileDialog
            {
                Title = "Сохранение справочников в Word",
                Filter = "Документ Word (*.docx)|*.docx",
                DefaultExt = "docx",
                AddExtension = true,
                InitialDirectory = Ui.ReportDirectory(),
                FileName = "Справочники.docx",
                OverwritePrompt = true,
                RestoreDirectory = true
            };
            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;
            if (!string.Equals(Path.GetExtension(dialog.FileName), ".docx", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Имя файла должно иметь расширение .docx.");

            _report.Save(dialog.FileName, manufacturers, shoes, sizes);
            MessageBox.Show(this, $"Отчёт сохранён:\n{dialog.FileName}", "Готово",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        });
}
