using GMap.NET.Internals;
using P3tr0viCh.Utils.Comparers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Interfaces;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer.Presenters
{
    internal class PresenterFrmListTileInfo : PresenterFrmListBase<Track>
    {
        public override FrmListType ListType => FrmListType.TileInfo;

        public override ChildFormType FormType => ChildFormType.TileInfo;

        public PresenterFrmListTileInfo(IFrmList frmList) : base(frmList)
        {
            Grants = FrmListGrant.Sort;

            OnPositionChanged += PresenterFrmListMarkers_OnPositionChanged;
        }

        public Database.Models.Tile Tile => Value as Database.Models.Tile;

        protected override string FormTitle => string.Format(Resources.StatusTileId, Tile.X, Tile.Y);

        protected override void LoadFormState()
        {
            base.LoadFormState();

            presenterDataGridView.SortColumn = nameof(Track.DateTimeStart);
            presenterDataGridView.SortOrder = ComparerSortOrder.Descending;
        }

        protected override void UpdateColumns()
        {
            foreach (DataGridViewColumn column in DataGridView.Columns)
            {
                column.Visible = false;
            }

            DataGridView.Columns[nameof(Track.DateTimeStart)].Visible = true;
            DataGridView.Columns[nameof(Track.DateTimeStart)].DisplayIndex = 0;

            DataGridView.Columns[nameof(Track.Text)].Visible = true;
        }

        public override void UpdateSettings()
        {
            DataGridView.Columns[nameof(Track.DateTimeStart)].DefaultCellStyle = DataGridViewCellStyles.DateTime;
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

        protected override bool ShowItemChangeDialog(Track value)
        {
            throw new NotImplementedException();
        }

        protected override bool ShowItemDeleteDialog(IEnumerable<Track> list)
        {
            throw new NotImplementedException();
        }

        protected override async Task<IEnumerable<Track>> ListLoadAsync()
        {
            return await Database.Default.ListLoadAsync<Track>(Tile);
        }

        private void PresenterFrmListMarkers_OnPositionChanged()
        {
            MainForm.SelectMapItemAsync(this, Selected);
        }
    }
}