using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public PresenterFrmListResultEquipments(IFrmList frmList) : base(frmList)
        {
            Grants = FrmListGrant.None;
        }

        protected override string FormTitle => Resources.TitleListResultEquipments;

        protected override void LoadFormState()
        {
            base.LoadFormState();

            StatusStrip.Visible = false;
        }

        protected override void UpdateColumns()
        {
            DataGridView.Columns[nameof(ResultEquipments.Text)].DisplayIndex = 0;

            DataGridView.Columns[nameof(ResultEquipments.Text)].Visible = true;

            DataGridView.Columns[nameof(ResultEquipments.DurationSum)].Visible = false;

            DataGridView.Columns[nameof(ResultEquipments.Text)].HeaderText = ResourcesColumnHeader.Name;

            columnFormattingIndex = new int[1];
            columnFormattingIndex[0] = DataGridView.Columns[nameof(ResultEquipments.Text)].Index;

            DataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(DataGridView_CellFormatting);
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
            DataGridView.Columns[nameof(ResultEquipments.Count)].DefaultCellStyle =
                DataGridViewCellStyles.Count;
            DataGridView.Columns[nameof(ResultEquipments.DistanceSum)].DefaultCellStyle =
                DataGridViewCellStyles.DistanceSum;
            DataGridView.Columns[nameof(ResultEquipments.DurationSumAsString)].DefaultCellStyle =
                DataGridViewCellStyles.DurationAsString;
        }
    }
}