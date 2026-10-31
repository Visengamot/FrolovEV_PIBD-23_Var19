using ShoeStore.WinForms.Helpers;

namespace ShoeStore.WinForms.Forms.Base;

public abstract class CatalogForm : Form
{
    protected readonly DataGridView ItemsGrid;
    protected CatalogForm(string title)
    {
        Text = title; Name = GetType().Name; StartPosition = FormStartPosition.CenterParent;
        Size = new Size(1050, 600); MinimumSize = new Size(700, 400);
        ItemsGrid = Ui.Grid("catalogGrid");
        var toolbar = new FlowLayoutPanel { Name = "catalogToolbar", Dock = DockStyle.Top, Height = 52, Padding = new Padding(10) };
        toolbar.Controls.Add(Ui.Button("addButton", "Добавить", () => Ui.Run(this, AddItem)));
        toolbar.Controls.Add(Ui.Button("editButton", "Изменить", () => Ui.Run(this, EditItem)));
        toolbar.Controls.Add(Ui.Button("deleteButton", "Удалить", () => Ui.Run(this, DeleteSelected)));
        toolbar.Controls.Add(Ui.Button("refreshButton", "Обновить", () => Ui.Run(this, Reload)));
        Controls.Add(ItemsGrid); Controls.Add(toolbar);
        Shown += (_, _) => Ui.Run(this, Reload);
        ItemsGrid.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) Ui.Run(this, EditItem); };
    }
    protected Guid SelectedId()
    {
        if (ItemsGrid.CurrentRow?.Cells["IdColumn"].Value is Guid id) return id;
        throw new InvalidOperationException("Выберите запись в таблице.");
    }
    protected void EditWith(Form editor)
    {
        using (editor) if (editor.ShowDialog(this) == DialogResult.OK) Reload();
    }
    private void DeleteSelected()
    {
        var id = SelectedId();
        if (MessageBox.Show(this, "Удалить выбранную запись?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        DeleteItem(id); Reload();
    }
    protected void IdColumn() { Ui.Column(ItemsGrid, "Id", "Код"); ItemsGrid.Columns["IdColumn"].Visible = false; }
    protected abstract void Reload();
    protected abstract void AddItem();
    protected abstract void EditItem();
    protected abstract void DeleteItem(Guid id);
}
