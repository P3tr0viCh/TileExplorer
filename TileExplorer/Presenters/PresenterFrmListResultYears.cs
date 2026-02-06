using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.Forms;
using System.Windows.Forms;
using TileExplorer.Interfaces;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer.Presenters
{
    internal class PresenterFrmListResultYears : PresenterFrmListBase<ResultYears>
    {
        public override FrmListType ListType => FrmListType.ResultYears;

        public override ChildFormType FormType => ChildFormType.ResultYears;

        public PresenterFrmListResultYears(IFrmListBase frmList) : base(frmList)
        {
            Grants = FrmListGrant.None;
        }

        protected override string FormTitle => Resources.TitleListResultYears;

        protected override void LoadFormState()
        {
            base.LoadFormState();

            FrmList.StatusStrip.Visible = false;
        }

        protected override void UpdateColumns()
        {
            FrmList.DataGridView.Columns[nameof(ResultYears.DurationSum)].Visible = false;
            FrmList.DataGridView.Columns[nameof(ResultYears.DurationSumAsString)].Visible = true;
            FrmList.DataGridView.Columns[nameof(ResultYears.DistanceSum)].Visible = true;

            FrmList.DataGridView.Columns[nameof(ResultYears.Year)].DisplayIndex = 0;
            FrmList.DataGridView.Columns[nameof(ResultYears.Count)].DisplayIndex = 1;
            FrmList.DataGridView.Columns[nameof(ResultYears.DurationSumAsString)].DisplayIndex = 2;

            columnFormattingIndex = new int[1];
            columnFormattingIndex[0] = FrmList.DataGridView.Columns[nameof(ResultYears.Year)].Index;

            FrmList.DataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(DataGridView_CellFormatting);
        }

        private int[] columnFormattingIndex;

        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == columnFormattingIndex[0])
            {
                if ((int)e.Value == 0) e.Value = Resources.TextTotal;
            }
        }

        public override int Compare(ResultYears x, ResultYears y, string dataPropertyName, ComparerSortOrder sortOrder)
        {
            return 0;
        }

        public override void UpdateSettings()
        {
            FrmList.DataGridView.Columns[nameof(ResultYears.Year)].DefaultCellStyle = DataGridViewCellStyles.Year;
            FrmList.DataGridView.Columns[nameof(ResultYears.Count)].DefaultCellStyle = DataGridViewCellStyles.Count;

            FrmList.DataGridView.Columns[nameof(ResultYears.DurationSumAsString)].DefaultCellStyle =
                DataGridViewCellStyles.DurationAsString;

            FrmList.DataGridView.Columns[nameof(ResultYears.DistanceSum)].DefaultCellStyle =
                DataGridViewCellStyles.DistanceSum;
            FrmList.DataGridView.Columns[nameof(ResultYears.DistanceStep0)].DefaultCellStyle =
                DataGridViewCellStyles.DistanceStep;
            FrmList.DataGridView.Columns[nameof(ResultYears.DistanceStep1)].DefaultCellStyle =
                DataGridViewCellStyles.DistanceStep;
            FrmList.DataGridView.Columns[nameof(ResultYears.DistanceStep2)].DefaultCellStyle =
                DataGridViewCellStyles.DistanceStep;

            FrmList.DataGridView.Columns[nameof(ResultYears.EleAscentSum)].DefaultCellStyle =
                DataGridViewCellStyles.EleAscentSum;
            FrmList.DataGridView.Columns[nameof(ResultYears.EleAscentStep0)].DefaultCellStyle =
                DataGridViewCellStyles.EleAscentStep;
            FrmList.DataGridView.Columns[nameof(ResultYears.EleAscentStep1)].DefaultCellStyle =
                DataGridViewCellStyles.EleAscentStep;
            FrmList.DataGridView.Columns[nameof(ResultYears.EleAscentStep2)].DefaultCellStyle =
                DataGridViewCellStyles.EleAscentStep;
        }
    }
}