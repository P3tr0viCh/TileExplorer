using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.EventArguments;
using P3tr0viCh.Utils.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer.Presenters
{
    internal class PresenterFrmListEquipments : PresenterFrmList<Equipment>
    {
        public override ChildFormType FormType => ChildFormType.EquipmentList;

        public PresenterFrmListEquipments(IFrmList frmList) : base(frmList)
        {
            ItemsChangeDialog += PresenterFrmListEquipments_ItemsChangeDialog;
            ItemsDeleteDialog += PresenterFrmListEquipments_ItemsDeleteDialog;

            ItemsChanged += PresenterFrmListEquipments_ItemsChanged;
            ItemsDeleted += PresenterFrmListEquipments_ItemsDeleted;
        }

        protected override string FormTitle => Resources.TitleListEquipments;

        protected override void LoadFormState()
        {
            base.LoadFormState();

            PresenterDataGridView.SortColumn = nameof(Equipment.Text);
        }

        private async Task PresenterFrmListEquipments_ItemsChangeDialog(object sender, ItemsDialogEventArgs<Equipment> e)
        {
            e.Ok = FrmEquipment.ShowDlg(Form, e.Values.First());

            await Task.CompletedTask;
        }

        private async Task PresenterFrmListEquipments_ItemsDeleteDialog(object sender, ItemsDialogEventArgs<Equipment> e)
        {
            e.Ok = Utils.ShowItemDeleteDialog(e.Values, Resources.QuestionEquipmentDelete, Resources.QuestionEquipmentListDelete);

            await Task.CompletedTask;
        }

        private async void PresenterFrmListEquipments_ItemsChanged(object sender, ItemsEventArgs<Equipment> e)
        {
            Utils.Forms.ChildFormsListItemsChange(ChildFormType.TrackList, e.Values);

            await MainForm.UpdateDataAsync(DataLoad.None, ChildFormType.ResultEquipments | ChildFormType.Filter);
        }

        private async void PresenterFrmListEquipments_ItemsDeleted(object sender, ItemsEventArgs<Equipment> e)
        {
            Utils.Forms.ChildFormsListItemsDelete(ChildFormType.TrackList, e.Values);

            await MainForm.UpdateDataAsync(DataLoad.None, ChildFormType.ResultEquipments | ChildFormType.Filter);
        }

        protected override void UpdateColumns()
        {
            base.UpdateColumns();

            FrmList.DataGridView.Columns[nameof(Equipment.Text)].DisplayIndex = 0;

            FrmList.DataGridView.Columns[nameof(Equipment.Text)].Visible = true;

            FrmList.DataGridView.Columns[nameof(Equipment.Text)].HeaderText = ResourcesColumnHeader.Name;
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