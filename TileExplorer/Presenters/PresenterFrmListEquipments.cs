using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.EventArguments;
using System.Collections.Generic;
using System.Threading.Tasks;
using TileExplorer.Interfaces;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer.Presenters
{
    internal class PresenterFrmListEquipments : PresenterFrmListBase<Equipment>
    {
        public override FrmListType ListType => FrmListType.EquipmentList;

        public override ChildFormType FormType => ChildFormType.EquipmentList;

        public PresenterFrmListEquipments(IFrmListBase frmList) : base(frmList)
        {
            ItemChangeDialog += PresenterFrmListEquipments_ItemChangeDialog;

            ItemListDeleteDialog += PresenterFrmListEquipments_ItemListDeleteDialog;
        }

        protected override string FormTitle => Resources.TitleListEquipments;

        protected override void LoadFormState()
        {
            base.LoadFormState();

            PresenterDataGridView.SortColumn = nameof(Equipment.Text);
        }

        private void PresenterFrmListEquipments_ItemChangeDialog(object sender, ItemDialogEventArgs<Equipment> e)
        {
            e.Ok = FrmEquipment.ShowDlg(Form, e.Value);
        }

        private void PresenterFrmListEquipments_ItemListDeleteDialog(object sender, ItemListDialogEventArgs<Equipment> e)
        {
            e.Ok = Utils.ShowItemDeleteDialog(e.Values,
                Resources.QuestionEquipmentDelete, Resources.QuestionEquipmentListDelete);
        }

        protected override async Task ListItemSaveAsync(Equipment value)
        {
            await Task.CompletedTask;
        }

        protected override async Task ListItemDeleteAsync(IEnumerable<Equipment> list)
        {
            await Database.Actions.EquipmentDeleteAsync(list);
        }

        protected override void UpdateColumns()
        {
            base.UpdateColumns();

            FrmList.DataGridView.Columns[nameof(Equipment.Text)].DisplayIndex = 0;

            FrmList.DataGridView.Columns[nameof(Equipment.Text)].Visible = true;

            FrmList.DataGridView.Columns[nameof(Equipment.Text)].HeaderText = ResourcesColumnHeader.Name;
        }

        public override void UpdateSettings()
        {
        }

        public override int Compare(Equipment x, Equipment y, string dataPropertyName, ComparerSortOrder sortOrder)
        {
            var result = 0;

            switch (dataPropertyName)
            {
                case nameof(Equipment.Text):
                    result = EmptyStringComparer.Default.Compare(x.Text, y.Text, sortOrder);
                    break;
                case nameof(Equipment.Brand):
                    result = EmptyStringComparer.Default.Compare(x.Brand, y.Brand, sortOrder);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.Model, y.Model, ComparerSortOrder.Ascending);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.Text, y.Text, ComparerSortOrder.Ascending);
                    break;
                case nameof(Equipment.Model):
                    result = EmptyStringComparer.Default.Compare(x.Model, y.Model, sortOrder);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.Brand, y.Brand, ComparerSortOrder.Ascending);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.Text, y.Text, ComparerSortOrder.Ascending);
                    break;
            }

            return result;
        }
    }
}