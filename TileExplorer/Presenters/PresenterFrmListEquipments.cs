using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Interfaces;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer.Presenters
{
    internal class PresenterFrmListEquipments : PresenterFrmListBase<Equipment>
    {
        public override FrmListType ListType => FrmListType.EquipmentList;

        public override ChildFormType FormType => ChildFormType.EquipmentList;

        public PresenterFrmListEquipments(IFrmList frmList) : base(frmList)
        {
        }

        protected override string FormTitle => Resources.TitleListEquipments;

        protected override void LoadFormState()
        {
            base.LoadFormState();

            presenterDataGridView.SortColumn = nameof(Equipment.Text);
        }

        protected override bool ShowItemChangeDialog(Equipment value)
        {
            return FrmEquipment.ShowDlg(Form, value);
        }

        protected override bool ShowItemDeleteDialog(IEnumerable<Equipment> list)
        {
            var count = list?.Count();

            if (count == 0) return false;

            var first = list.FirstOrDefault();

            var text = first.Text;

            var question = count == 1 ? Resources.QuestionEquipmentDelete : Resources.QuestionEquipmentListDelete;

            return Msg.Question(question, text, count - 1);
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
            DataGridView.Columns[nameof(Equipment.Text)].DisplayIndex = 0;

            DataGridView.Columns[nameof(Equipment.Text)].Visible = true;

            DataGridView.Columns[nameof(Equipment.Text)].HeaderText = ResourcesColumnHeader.Name;
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