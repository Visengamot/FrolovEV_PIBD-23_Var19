#nullable disable
namespace ShoeStore.WinForms.Forms;

partial class SalesChartForm
{
    private System.ComponentModel.IContainer components = null!;
    private Label descriptionLabel = null!;
    private CheckBox periodCheckBox = null!;
    private Label salesDateLabel = null!;
    private Label endDateLabel = null!;
    private DateTimePicker endDatePicker = null!;
    private DateTimePicker salesDatePicker = null!;
    private Button saveButton = null!;
    private Button cancelButton = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        descriptionLabel = new Label();
        periodCheckBox = new CheckBox();
        salesDateLabel = new Label();
        salesDatePicker = new DateTimePicker();
        endDateLabel = new Label();
        endDatePicker = new DateTimePicker();
        saveButton = new Button();
        cancelButton = new Button();
        SuspendLayout();
        // descriptionLabel
        descriptionLabel.AutoSize = true;
        descriptionLabel.Location = new Point(16, 17);
        descriptionLabel.Name = "descriptionLabel";
        descriptionLabel.Size = new Size(315, 15);
        descriptionLabel.TabIndex = 0;
        descriptionLabel.Text = "Диаграмма покажет объём продаж по категориям.";
        // periodCheckBox
        periodCheckBox.AutoSize = true;
        periodCheckBox.Checked = true;
        periodCheckBox.Location = new Point(16, 46);
        periodCheckBox.Name = "periodCheckBox";
        periodCheckBox.TabIndex = 1;
        periodCheckBox.Text = "За период (снимите галочку, чтобы выбрать один день)";
        periodCheckBox.UseVisualStyleBackColor = true;
        // salesDateLabel
        salesDateLabel.AutoSize = true;
        salesDateLabel.Location = new Point(16, 80);
        salesDateLabel.Name = "salesDateLabel";
        salesDateLabel.TabIndex = 2;
        salesDateLabel.Text = "Дата начала:";
        // salesDatePicker
        salesDatePicker.Format = DateTimePickerFormat.Short;
        salesDatePicker.Location = new Point(173, 76);
        salesDatePicker.Name = "salesDatePicker";
        salesDatePicker.Size = new Size(154, 23);
        salesDatePicker.TabIndex = 3;
        // endDateLabel
        endDateLabel.AutoSize = true;
        endDateLabel.Location = new Point(16, 113);
        endDateLabel.Name = "endDateLabel";
        endDateLabel.TabIndex = 4;
        endDateLabel.Text = "Дата окончания:";
        // endDatePicker
        endDatePicker.Format = DateTimePickerFormat.Short;
        endDatePicker.Location = new Point(173, 109);
        endDatePicker.Name = "endDatePicker";
        endDatePicker.Size = new Size(154, 23);
        endDatePicker.TabIndex = 5;
        // saveButton
        saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        saveButton.Location = new Point(160, 151);
        saveButton.Name = "saveButton";
        saveButton.Size = new Size(144, 32);
        saveButton.TabIndex = 6;
        saveButton.Text = "Сохранить в PDF…";
        saveButton.UseVisualStyleBackColor = true;
        // cancelButton
        cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        cancelButton.DialogResult = DialogResult.Cancel;
        cancelButton.Location = new Point(310, 151);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(104, 32);
        cancelButton.TabIndex = 7;
        cancelButton.Text = "Отмена";
        cancelButton.UseVisualStyleBackColor = true;
        // SalesChartForm
        AcceptButton = saveButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = cancelButton;
        ClientSize = new Size(430, 199);
        Controls.Add(descriptionLabel);
        Controls.Add(periodCheckBox);
        Controls.Add(salesDateLabel);
        Controls.Add(salesDatePicker);
        Controls.Add(endDateLabel);
        Controls.Add(endDatePicker);
        Controls.Add(saveButton);
        Controls.Add(cancelButton);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "SalesChartForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Диаграмма продаж — PDF";
        ResumeLayout(false);
        PerformLayout();
    }
}
