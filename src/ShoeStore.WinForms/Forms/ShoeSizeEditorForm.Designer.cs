#nullable disable
namespace ShoeStore.WinForms.Forms;

partial class ShoeSizeEditorForm
{
    private System.ComponentModel.IContainer components = null!;
    private Label sizeLabel = null!;
    private NumericUpDown sizeNumericUpDown = null!;
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
        sizeLabel = new Label();
        sizeNumericUpDown = new NumericUpDown();
        saveButton = new Button();
        cancelButton = new Button();
        ((System.ComponentModel.ISupportInitialize)sizeNumericUpDown).BeginInit();
        SuspendLayout();
        // sizeLabel
        sizeLabel.AutoSize = true;
        sizeLabel.Location = new Point(16, 25);
        sizeLabel.Name = "sizeLabel";
        sizeLabel.Size = new Size(49, 15);
        sizeLabel.TabIndex = 0;
        sizeLabel.Text = "Размер";
        // sizeNumericUpDown
        sizeNumericUpDown.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        sizeNumericUpDown.DecimalPlaces = 1;
        sizeNumericUpDown.Increment = 0.5m;
        sizeNumericUpDown.Location = new Point(174, 22);
        sizeNumericUpDown.Maximum = 60m;
        sizeNumericUpDown.Minimum = 10m;
        sizeNumericUpDown.Name = "sizeNumericUpDown";
        sizeNumericUpDown.Size = new Size(270, 23);
        sizeNumericUpDown.TabIndex = 1;
        sizeNumericUpDown.ThousandsSeparator = true;
        sizeNumericUpDown.Value = 36m;
        // saveButton
        saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        saveButton.Location = new Point(230, 97);
        saveButton.Name = "saveButton";
        saveButton.Size = new Size(104, 32);
        saveButton.TabIndex = 2;
        saveButton.Text = "Сохранить";
        saveButton.UseVisualStyleBackColor = true;
        // cancelButton
        cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        cancelButton.DialogResult = DialogResult.Cancel;
        cancelButton.Location = new Point(340, 97);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(104, 32);
        cancelButton.TabIndex = 3;
        cancelButton.Text = "Отмена";
        cancelButton.UseVisualStyleBackColor = true;
        // ShoeSizeEditorForm
        AcceptButton = saveButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = cancelButton;
        ClientSize = new Size(460, 145);
        Controls.Add(sizeLabel);
        Controls.Add(sizeNumericUpDown);
        Controls.Add(saveButton);
        Controls.Add(cancelButton);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ShoeSizeEditorForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Добавление размера";
        ((System.ComponentModel.ISupportInitialize)sizeNumericUpDown).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
