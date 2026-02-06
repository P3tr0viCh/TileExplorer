using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Forms;
using System.Windows.Forms;
using TileExplorer.Interfaces;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer.Presenters
{
    internal class PresenterFrmListResultEquipments : PresenterFrmListBase<ResultEquipments>
    {
        public override FrmListType ListType => FrmListType.ResultEquipments;

        public override ChildFormType FormType => ChildFormType.ResultEquipments;

        public PresenterFrmListResultEquipments(IFrmListBase frmList) : base(frmList)
        {
            Grants = FrmListGrant.None;
        }

        protected override string FormTitle => Resources.TitleListResultEquipments;

        protected override void LoadFormState()
        {
            base.LoadFormState();

            FrmList.StatusStrip.Visible = false;
        }

        protected override void UpdateColumns()
        {
            FrmList.DataGridView.Columns[nameof(ResultEquipments.Text)].DisplayIndex = 0;

            FrmList.DataGridView.Columns[nameof(ResultEquipments.Text)].Visible = true;

            FrmList.DataGridView.Columns[nameof(ResultEquipments.DurationSum)].Visible = false;

            FrmList.DataGridView.Columns[nameof(ResultEquipments.Text)].HeaderText = ResourcesColumnHeader.Name;

            columnFormattingIndex = new int[1];
            columnFormattingIndex[0] = FrmList.DataGridView.Columns[nameof(ResultEquipments.Text)].Index;

            FrmList.DataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(DataGridView_CellFormatting);
        }

        private int[] columnFormattingIndex;

        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == columnFormattingIndex[0])
            {
                if ((e.Value as string).IsEmpty()) e.Value = Resources.TextOther;
            }
        }

        public override int Compare(ResultEquipments x, ResultEquipments y, string dataPropertyName, ComparerSortOrder sortOrder)
        {
            return 0;
        }

        public override void UpdateSettings()
        {
            FrmList.DataGridView.Columns[nameof(ResultEquipments.Count)].DefaultCellStyle =
                DataGridViewCellStyles.Count;
            FrmList.DataGridView.Columns[nameof(ResultEquipments.DistanceSum)].DefaultCellStyle =
                DataGridViewCellStyles.DistanceSum;
            FrmList.DataGridView.Columns[nameof(ResultEquipments.DurationSumAsString)].DefaultCellStyle =
                DataGridViewCellStyles.DurationAsString;
        }
    }
}