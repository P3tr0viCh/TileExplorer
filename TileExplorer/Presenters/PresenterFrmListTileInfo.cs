using GMap.NET.Internals;
using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.Forms;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Interfaces;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer.Presenters
{
    internal class PresenterFrmListTileInfo : PresenterFrmListBase<Track>
    {
        public override ChildFormType FormType => ChildFormType.TileInfo;

        public PresenterFrmListTileInfo(IFrmListBase frmList) : base(frmList)
        {
            Grants = FrmListGrant.Sort;

            BindingSource.PositionChanged += BindingSource_PositionChanged;
        }

        public Database.Models.Tile Tile => Value as Database.Models.Tile;

        protected override string FormTitle => string.Format(Resources.StatusTileId, Tile.X, Tile.Y);

        protected override void LoadFormState()
        {
            base.LoadFormState();

            PresenterDataGridView.SortColumn = nameof(Track.DateTimeStart);
            PresenterDataGridView.SortOrder = ComparerSortOrder.Descending;
        }

        protected override void UpdateColumns()
        {
            foreach (DataGridViewColumn column in FrmList.DataGridView.Columns)
            {
                column.Visible = false;
            }

            FrmList.DataGridView.Columns[nameof(Track.DateTimeStart)].Visible = true;
            FrmList.DataGridView.Columns[nameof(Track.DateTimeStart)].DisplayIndex = 0;

            FrmList.DataGridView.Columns[nameof(Track.Text)].Visible = true;
        }

        public override void UpdateSettings()
        {
            FrmList.DataGridView.Columns[nameof(Track.DateTimeStart)].DefaultCellStyle = DataGridViewCellStyles.DateTime;
        }

        public override int Compare(Track x, Track y, string dataPropertyName, ComparerSortOrder sortOrder)
        {
            var result = 0;

            switch (dataPropertyName)
            {
                case nameof(Track.Text):
                    result = EmptyStringComparer.Default.Compare(x.Text, y.Text, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTimeStart, y.DateTimeStart, ComparerSortOrder.Descending);
                    break;
                case nameof(Track.DateTimeStart):
                    result = SortOrderComparer.Default.Compare(x.DateTimeStart, y.DateTimeStart, sortOrder);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.Text, y.Text, ComparerSortOrder.Ascending);
                    break;
            }

            return result;
        }

        protected override async Task<IEnumerable<Track>> ListLoadAsync(CancellationToken token)
        {
            return await Database.Default.ListLoadAsync<Track>(Tile);
        }

        private void BindingSource_PositionChanged(object sender, EventArgs e)
        {
            MainForm.SelectMapItemAsync(this, Selected);
        }
    }
}