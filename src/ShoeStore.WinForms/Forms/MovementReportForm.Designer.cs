#nullable disable
namespace ShoeStore.WinForms.Forms;

partial class MovementReportForm
{
    private System.ComponentModel.IContainer components = null!;
    private Label descriptionLabel = null!;
    private Label fromDateLabel = null!;
    private Label toDateLabel = null!;
    private DateTimePicker fromDatePicker = null!;
    private DateTimePicker toDatePicker = null!;
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
        fromDateLabel = new Label();
        toDateLabel = new Label();
        fromDatePicker = new DateTimePicker();
        toDatePicker = new DateTimePicker();
        saveButton = new Button();
        cancelButton = new Button();
        SuspendLayout();
        // descriptionLabel
        descriptionLabel.AutoSize = true;
        descriptionLabel.Location = new Point(16, 17);
        descriptionLabel.Name = "descriptionLabel";
        descriptionLabel.Size = new Size(289, 15);
        descriptionLabel.TabIndex = 0;
        descriptionLabel.Text = "Выберите период отчёта (обе даты включены).";
        // fromDateLabel
        fromDateLabel.AutoSize = true;
        fromDateLabel.Location = new Point(16, 49);
        fromDateLabel.Name = "fromDateLabel";
        fromDateLabel.Size = new Size(102, 15);
        fromDateLabel.TabIndex = 1;
        fromDateLabel.Text = "Начало периода:";
        // fromDatePicker
        fromDatePicker.Format = DateTimePickerFormat.Short;
        fromDatePicker.Location = new Point(173, 45);
        fromDatePicker.Name = "fromDatePicker";
        fromDatePicker.Size = new Size(154, 23);
        fromDatePicker.TabIndex = 2;
        // toDateLabel
        toDateLabel.AutoSize = true;
        toDateLabel.Location = new Point(16, 82);
        toDateLabel.Name = "toDateLabel";
        toDateLabel.Size = new Size(97, 15);
        toDateLabel.TabIndex = 3;
        toDateLabel.Text = "Конец периода:";
        // toDatePicker
        toDatePicker.Format = DateTimePickerFormat.Short;
        toDatePicker.Location = new Point(173, 78);
        toDatePicker.Name = "toDatePicker";
        toDatePicker.Size = new Size(154, 23);
        toDatePicker.TabIndex = 4;
        // saveButton
        saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        saveButton.Location = new Point(160, 123);
        saveButton.Name = "saveButton";
        saveButton.Size = new Size(144, 32);
        saveButton.TabIndex = 5;
        saveButton.Text = "Сохранить в Excel…";
        saveButton.UseVisualStyleBackColor = true;
        // cancelButton
        cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        cancelButton.DialogResult = DialogResult.Cancel;
        cancelButton.Location = new Point(310, 123);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(104, 32);
        cancelButton.TabIndex = 6;
        cancelButton.Text = "Отмена";
        cancelButton.UseVisualStyleBackColor = true;
        // MovementReportForm
        AcceptButton = saveButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = cancelButton;
        ClientSize = new Size(430, 171);
        Controls.Add(descriptionLabel);
        Controls.Add(fromDateLabel);
        Controls.Add(fromDatePicker);
        Controls.Add(toDateLabel);
        Controls.Add(toDatePicker);
        Controls.Add(saveButton);
        Controls.Add(cancelButton);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "MovementReportForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Движение товаров — Excel";
        ResumeLayout(false);
        PerformLayout();
    }
}
