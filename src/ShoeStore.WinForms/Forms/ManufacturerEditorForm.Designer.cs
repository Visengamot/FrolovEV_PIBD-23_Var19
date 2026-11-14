#nullable disable
namespace ShoeStore.WinForms.Forms;

partial class ManufacturerEditorForm
{
    private System.ComponentModel.IContainer components = null!;
    private Label nameLabel = null!;
    private Label countryLabel = null!;
    private TextBox manufacturerNameTextBox = null!;
    private TextBox countryTextBox = null!;
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
        nameLabel = new Label();
        countryLabel = new Label();
        manufacturerNameTextBox = new TextBox();
        countryTextBox = new TextBox();
        saveButton = new Button();
        cancelButton = new Button();
        SuspendLayout();
        // nameLabel
        nameLabel.AutoSize = true;
        nameLabel.Location = new Point(16, 25);
        nameLabel.Name = "nameLabel";
        nameLabel.Size = new Size(62, 15);
        nameLabel.TabIndex = 0;
        nameLabel.Text = "Название";
        // manufacturerNameTextBox
        manufacturerNameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        manufacturerNameTextBox.Location = new Point(174, 22);
        manufacturerNameTextBox.MaxLength = 200;
        manufacturerNameTextBox.Name = "manufacturerNameTextBox";
        manufacturerNameTextBox.Size = new Size(320, 23);
        manufacturerNameTextBox.TabIndex = 1;
        // countryLabel
        countryLabel.AutoSize = true;
        countryLabel.Location = new Point(16, 67);
        countryLabel.Name = "countryLabel";
        countryLabel.Size = new Size(46, 15);
        countryLabel.TabIndex = 2;
        countryLabel.Text = "Страна";
        // countryTextBox
        countryTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        countryTextBox.Location = new Point(174, 64);
        countryTextBox.MaxLength = 100;
        countryTextBox.Name = "countryTextBox";
        countryTextBox.Size = new Size(320, 23);
        countryTextBox.TabIndex = 3;
        // saveButton
        saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        saveButton.Location = new Point(280, 142);
        saveButton.Name = "saveButton";
        saveButton.Size = new Size(104, 32);
        saveButton.TabIndex = 4;
        saveButton.Text = "Сохранить";
        saveButton.UseVisualStyleBackColor = true;
        // cancelButton
        cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        cancelButton.DialogResult = DialogResult.Cancel;
        cancelButton.Location = new Point(390, 142);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(104, 32);
        cancelButton.TabIndex = 5;
        cancelButton.Text = "Отмена";
        cancelButton.UseVisualStyleBackColor = true;
        // ManufacturerEditorForm
        AcceptButton = saveButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = cancelButton;
        ClientSize = new Size(510, 190);
        Controls.Add(nameLabel);
        Controls.Add(manufacturerNameTextBox);
        Controls.Add(countryLabel);
        Controls.Add(countryTextBox);
        Controls.Add(saveButton);
        Controls.Add(cancelButton);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ManufacturerEditorForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Добавление производителя";
        ResumeLayout(false);
        PerformLayout();
    }
}
