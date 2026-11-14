#nullable disable

namespace ShoeStore.WinForms.Forms;

partial class ShoeEditorForm
{
    private System.ComponentModel.IContainer components = null!;
    private Label articleLabel = null!;
    private Label nameLabel = null!;
    private Label manufacturerLabel = null!;
    private Label priceLabel = null!;
    private Label categoryLabel = null!;
    private Label seasonsLabel = null!;
    private TextBox articleTextBox = null!;
    private TextBox shoeNameTextBox = null!;
    private ComboBox manufacturerComboBox = null!;
    private NumericUpDown priceNumericUpDown = null!;
    private ComboBox categoryComboBox = null!;
    private CheckedListBox seasonsCheckedListBox = null!;
    private Button saveButton = null!;
    private Button cancelButton = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        articleLabel = new Label();
        nameLabel = new Label();
        manufacturerLabel = new Label();
        priceLabel = new Label();
        categoryLabel = new Label();
        seasonsLabel = new Label();
        articleTextBox = new TextBox();
        shoeNameTextBox = new TextBox();
        manufacturerComboBox = new ComboBox();
        priceNumericUpDown = new NumericUpDown();
        categoryComboBox = new ComboBox();
        seasonsCheckedListBox = new CheckedListBox();
        saveButton = new Button();
        cancelButton = new Button();
        ((System.ComponentModel.ISupportInitialize)priceNumericUpDown).BeginInit();
        SuspendLayout();
        // articleLabel
        articleLabel.AutoSize = true;
        articleLabel.Location = new Point(16, 25);
        articleLabel.Name = "articleLabel";
        articleLabel.Size = new Size(54, 15);
        articleLabel.TabIndex = 0;
        articleLabel.Text = "Артикул";
        // articleTextBox
        articleTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        articleTextBox.Location = new Point(174, 22);
        articleTextBox.MaxLength = 100;
        articleTextBox.Name = "articleTextBox";
        articleTextBox.Size = new Size(360, 23);
        articleTextBox.TabIndex = 1;
        // nameLabel
        nameLabel.AutoSize = true;
        nameLabel.Location = new Point(16, 67);
        nameLabel.Name = "nameLabel";
        nameLabel.Size = new Size(62, 15);
        nameLabel.TabIndex = 2;
        nameLabel.Text = "Название";
        // shoeNameTextBox
        shoeNameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        shoeNameTextBox.Location = new Point(174, 64);
        shoeNameTextBox.MaxLength = 200;
        shoeNameTextBox.Name = "shoeNameTextBox";
        shoeNameTextBox.Size = new Size(360, 23);
        shoeNameTextBox.TabIndex = 3;
        // manufacturerLabel
        manufacturerLabel.AutoSize = true;
        manufacturerLabel.Location = new Point(16, 109);
        manufacturerLabel.Name = "manufacturerLabel";
        manufacturerLabel.Size = new Size(98, 15);
        manufacturerLabel.TabIndex = 4;
        manufacturerLabel.Text = "Производитель";
        // manufacturerComboBox
        manufacturerComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        manufacturerComboBox.DisplayMember = "Name";
        manufacturerComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        manufacturerComboBox.FormattingEnabled = true;
        manufacturerComboBox.Location = new Point(174, 106);
        manufacturerComboBox.Name = "manufacturerComboBox";
        manufacturerComboBox.Size = new Size(360, 23);
        manufacturerComboBox.TabIndex = 5;
        manufacturerComboBox.ValueMember = "Id";
        // priceLabel
        priceLabel.AutoSize = true;
        priceLabel.Location = new Point(16, 151);
        priceLabel.Name = "priceLabel";
        priceLabel.Size = new Size(50, 15);
        priceLabel.TabIndex = 6;
        priceLabel.Text = "Цена, ₽";
        // priceNumericUpDown
        priceNumericUpDown.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        priceNumericUpDown.DecimalPlaces = 2;
        priceNumericUpDown.Location = new Point(174, 148);
        priceNumericUpDown.Maximum = 100000000m;
        priceNumericUpDown.Name = "priceNumericUpDown";
        priceNumericUpDown.Size = new Size(360, 23);
        priceNumericUpDown.TabIndex = 7;
        priceNumericUpDown.ThousandsSeparator = true;
        // categoryLabel
        categoryLabel.AutoSize = true;
        categoryLabel.Location = new Point(16, 193);
        categoryLabel.Name = "categoryLabel";
        categoryLabel.Size = new Size(66, 15);
        categoryLabel.TabIndex = 8;
        categoryLabel.Text = "Категория";
        // categoryComboBox
        categoryComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        categoryComboBox.DisplayMember = "Label";
        categoryComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        categoryComboBox.FormattingEnabled = true;
        categoryComboBox.Location = new Point(174, 190);
        categoryComboBox.Name = "categoryComboBox";
        categoryComboBox.Size = new Size(360, 23);
        categoryComboBox.TabIndex = 9;
        categoryComboBox.ValueMember = "Value";
        // seasonsLabel
        seasonsLabel.AutoSize = true;
        seasonsLabel.Location = new Point(16, 235);
        seasonsLabel.Name = "seasonsLabel";
        seasonsLabel.Size = new Size(49, 15);
        seasonsLabel.TabIndex = 10;
        seasonsLabel.Text = "Сезоны";
        // seasonsCheckedListBox
        seasonsCheckedListBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        seasonsCheckedListBox.CheckOnClick = true;
        seasonsCheckedListBox.FormattingEnabled = true;
        seasonsCheckedListBox.Items.AddRange(new object[] { "Весна", "Лето", "Осень", "Зима" });
        seasonsCheckedListBox.Location = new Point(174, 232);
        seasonsCheckedListBox.Name = "seasonsCheckedListBox";
        seasonsCheckedListBox.Size = new Size(360, 94);
        seasonsCheckedListBox.TabIndex = 11;
        // saveButton
        saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        saveButton.Location = new Point(320, 362);
        saveButton.Name = "saveButton";
        saveButton.Size = new Size(104, 32);
        saveButton.TabIndex = 12;
        saveButton.Text = "Сохранить";
        saveButton.UseVisualStyleBackColor = true;
        // cancelButton
        cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        cancelButton.DialogResult = DialogResult.Cancel;
        cancelButton.Location = new Point(430, 362);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(104, 32);
        cancelButton.TabIndex = 13;
        cancelButton.Text = "Отмена";
        cancelButton.UseVisualStyleBackColor = true;
        // ShoeEditorForm
        AcceptButton = saveButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = cancelButton;
        ClientSize = new Size(550, 410);
        Controls.Add(articleLabel);
        Controls.Add(articleTextBox);
        Controls.Add(nameLabel);
        Controls.Add(shoeNameTextBox);
        Controls.Add(manufacturerLabel);
        Controls.Add(manufacturerComboBox);
        Controls.Add(priceLabel);
        Controls.Add(priceNumericUpDown);
        Controls.Add(categoryLabel);
        Controls.Add(categoryComboBox);
        Controls.Add(seasonsLabel);
        Controls.Add(seasonsCheckedListBox);
        Controls.Add(saveButton);
        Controls.Add(cancelButton);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ShoeEditorForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Добавление обуви";
        ((System.ComponentModel.ISupportInitialize)priceNumericUpDown).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
