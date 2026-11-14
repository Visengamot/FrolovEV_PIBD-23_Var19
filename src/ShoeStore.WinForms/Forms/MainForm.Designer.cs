#nullable disable
namespace ShoeStore.WinForms.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private MenuStrip mainMenuStrip;
        private ToolStripMenuItem catalogsMenuItem;
        private ToolStripMenuItem manufacturersMenuItem;
        private ToolStripMenuItem shoesMenuItem;
        private ToolStripMenuItem sizesMenuItem;
        private ToolStripMenuItem operationsMenuItem;
        private ToolStripMenuItem movementsMenuItem;
        private ToolStripMenuItem inventoryMenuItem;
        private ToolStripMenuItem reportsMenuItem;
        private StatusStrip mainStatusStrip;
        private ToolStripStatusLabel storageStatusLabel;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            mainMenuStrip = new MenuStrip();
            catalogsMenuItem = new ToolStripMenuItem();
            manufacturersMenuItem = new ToolStripMenuItem();
            shoesMenuItem = new ToolStripMenuItem();
            sizesMenuItem = new ToolStripMenuItem();
            operationsMenuItem = new ToolStripMenuItem();
            movementsMenuItem = new ToolStripMenuItem();
            inventoryMenuItem = new ToolStripMenuItem();
            reportsMenuItem = new ToolStripMenuItem();
            mainStatusStrip = new StatusStrip();
            storageStatusLabel = new ToolStripStatusLabel();
            mainMenuStrip.SuspendLayout();
            mainStatusStrip.SuspendLayout();
            SuspendLayout();
            mainMenuStrip.Items.AddRange(new ToolStripItem[]
            {
                catalogsMenuItem,
                operationsMenuItem,
                reportsMenuItem
            });
            mainMenuStrip.Name = "mainMenuStrip";
            mainMenuStrip.Dock = DockStyle.Top;
            catalogsMenuItem.Name = "catalogsMenuItem";
            catalogsMenuItem.Text = "Справочники";
            catalogsMenuItem.DropDownItems.AddRange(
                new ToolStripItem[]
            {
                manufacturersMenuItem,
                shoesMenuItem,
                sizesMenuItem
            }
            );
            manufacturersMenuItem.Name = "manufacturersMenuItem";
            manufacturersMenuItem.Text = "Производители";
            shoesMenuItem.Name = "shoesMenuItem";
            shoesMenuItem.Text = "Ассортимент обуви";
            sizesMenuItem.Name = "sizesMenuItem";
            sizesMenuItem.Text = "Размеры";
            operationsMenuItem.Name = "operationsMenuItem";
            operationsMenuItem.Text = "Операции";
            operationsMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                movementsMenuItem,
                inventoryMenuItem
            });
            movementsMenuItem.Name = "movementsMenuItem";
            movementsMenuItem.Text = "Движение товара";
            inventoryMenuItem.Name = "inventoryMenuItem";
            inventoryMenuItem.Text = "Наличие товара";
            reportsMenuItem.Name = "reportsMenuItem";
            reportsMenuItem.Text = "Отчеты";
            mainStatusStrip.Name = "mainStatusStrip";
            mainStatusStrip.Items.AddRange(new ToolStripItem[]
            {
                storageStatusLabel
            });
            storageStatusLabel.Name = "storageStatusLabel";
            storageStatusLabel.Spring = true;
            storageStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
            storageStatusLabel.Text = "Учёт обуви • Данные хранятся в памяти до закрытия программы";
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 721);
            Controls.Add(mainMenuStrip);
            Controls.Add(mainStatusStrip);
            IsMdiContainer = true;
            MainMenuStrip = mainMenuStrip;
            MinimumSize = new Size(850, 550);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Магазин обуви — учёт товара";
            mainMenuStrip.ResumeLayout(false);
            mainMenuStrip.PerformLayout();
            mainStatusStrip.ResumeLayout(false);
            mainStatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
