#nullable disable
namespace ShoeStore.WinForms.Forms;

partial class CatalogReportForm
{
    private System.ComponentModel.IContainer components = null!;
    private Label descriptionLabel = null!;
    private CheckBox manufacturersCheckBox = null!;
    private CheckBox shoesCheckBox = null!;
    private CheckBox sizesCheckBox = null!;
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
        manufacturersCheckBox = new CheckBox();
        shoesCheckBox = new CheckBox();
        sizesCheckBox = new CheckBox();
        saveButton = new Button();
        cancelButton = new Button();
        SuspendLayout();
        // descriptionLabel
        descriptionLabel.AutoSize = true;
        descriptionLabel.Location = new Point(16, 17);
        descriptionLabel.Name = "descriptionLabel";
        descriptionLabel.Size = new Size(233, 15);
        descriptionLabel.TabIndex = 0;
        descriptionLabel.Text = "Выберите справочники для выгрузки:";
        // manufacturersCheckBox
        manufacturersCheckBox.AutoSize = true;
        manufacturersCheckBox.Checked = true;
        manufacturersCheckBox.CheckState = CheckState.Checked;
        manufacturersCheckBox.Location = new Point(16, 43);
        manufacturersCheckBox.Name = "manufacturersCheckBox";
        manufacturersCheckBox.Size = new Size(118, 19);
        manufacturersCheckBox.TabIndex = 1;
        manufacturersCheckBox.Text = "Производители";
        manufacturersCheckBox.UseVisualStyleBackColor = true;
        // shoesCheckBox
        shoesCheckBox.AutoSize = true;
        shoesCheckBox.Checked = true;
        shoesCheckBox.CheckState = CheckState.Checked;
        shoesCheckBox.Location = new Point(16, 68);
        shoesCheckBox.Name = "shoesCheckBox";
        shoesCheckBox.Size = new Size(62, 19);
        shoesCheckBox.TabIndex = 2;
        shoesCheckBox.Text = "Обувь";
        shoesCheckBox.UseVisualStyleBackColor = true;
        // sizesCheckBox
        sizesCheckBox.AutoSize = true;
        sizesCheckBox.Checked = true;
        sizesCheckBox.CheckState = CheckState.Checked;
        sizesCheckBox.Location = new Point(16, 93);
        sizesCheckBox.Name = "sizesCheckBox";
        sizesCheckBox.Size = new Size(111, 19);
        sizesCheckBox.TabIndex = 3;
        sizesCheckBox.Text = "Размеры обуви";
        sizesCheckBox.UseVisualStyleBackColor = true;
        // saveButton
        saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        saveButton.Location = new Point(160, 136);
        saveButton.Name = "saveButton";
        saveButton.Size = new Size(144, 32);
        saveButton.TabIndex = 4;
        saveButton.Text = "Сохранить в Word…";
        saveButton.UseVisualStyleBackColor = true;
        // cancelButton
        cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        cancelButton.DialogResult = DialogResult.Cancel;
        cancelButton.Location = new Point(310, 136);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(104, 32);
        cancelButton.TabIndex = 5;
        cancelButton.Text = "Отмена";
        cancelButton.UseVisualStyleBackColor = true;
        // CatalogReportForm
        AcceptButton = saveButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = cancelButton;
        ClientSize = new Size(430, 184);
        Controls.Add(descriptionLabel);
        Controls.Add(manufacturersCheckBox);
        Controls.Add(shoesCheckBox);
        Controls.Add(sizesCheckBox);
        Controls.Add(saveButton);
        Controls.Add(cancelButton);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "CatalogReportForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Справочники — Word";
        ResumeLayout(false);
        PerformLayout();
    }
}
