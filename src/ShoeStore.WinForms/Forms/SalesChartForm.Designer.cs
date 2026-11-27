#nullable disable
namespace ShoeStore.WinForms.Forms;

partial class SalesChartForm
{
    private System.ComponentModel.IContainer components = null!;
    private Label descriptionLabel = null!;
    private Label salesDateLabel = null!;
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
        salesDateLabel = new Label();
        salesDatePicker = new DateTimePicker();
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
        // salesDateLabel
        salesDateLabel.AutoSize = true;
        salesDateLabel.Location = new Point(16, 49);
        salesDateLabel.Name = "salesDateLabel";
        salesDateLabel.Size = new Size(88, 15);
        salesDateLabel.TabIndex = 1;
        salesDateLabel.Text = "Дата продаж:";
        // salesDatePicker
        salesDatePicker.Format = DateTimePickerFormat.Short;
        salesDatePicker.Location = new Point(173, 45);
        salesDatePicker.Name = "salesDatePicker";
        salesDatePicker.Size = new Size(154, 23);
        salesDatePicker.TabIndex = 2;
        // saveButton
        saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        saveButton.Location = new Point(160, 90);
        saveButton.Name = "saveButton";
        saveButton.Size = new Size(144, 32);
        saveButton.TabIndex = 3;
        saveButton.Text = "Сохранить в PDF…";
        saveButton.UseVisualStyleBackColor = true;
        // cancelButton
        cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        cancelButton.DialogResult = DialogResult.Cancel;
        cancelButton.Location = new Point(310, 90);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(104, 32);
        cancelButton.TabIndex = 4;
        cancelButton.Text = "Отмена";
        cancelButton.UseVisualStyleBackColor = true;
        // SalesChartForm
        AcceptButton = saveButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = cancelButton;
        ClientSize = new Size(430, 138);
        Controls.Add(descriptionLabel);
        Controls.Add(salesDateLabel);
        Controls.Add(salesDatePicker);
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
