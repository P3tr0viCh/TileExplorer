using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.EventArguments;
using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Interfaces;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer.Presenters
{
    internal class PresenterFrmListTracks : PresenterFrmListBase<Track>
    {
        public override ChildFormType FormType => ChildFormType.TrackList;

        public PresenterFrmListTracks(IFrmListBase frmList) : base(frmList)
        {
            Grants = Grants.AddFlag(FrmListGrant.MultiChange);

            BindingSource.PositionChanged += BindingSource_PositionChanged;

            ItemChangeDialog += PresenterFrmListTracks_ItemChangeDialog;
            ItemListChangeDialog += PresenterFrmListTracks_ItemListChangeDialog;

            ItemDeleteDialog += PresenterFrmListTracks_ItemDeleteDialog;
            ItemListDeleteDialog += PresenterFrmListTracks_ItemListDeleteDialog;
        }

        protected override string FormTitle => Resources.TitleListTracks;

        protected override void LoadFormState()
        {
            base.LoadFormState();

            PresenterDataGridView.SortColumn = nameof(Track.DateTimeStart);
            PresenterDataGridView.SortOrder = ComparerSortOrder.Descending;
        }

        private async void PresenterFrmListTracks_ItemChangeDialog(object sender, ItemDialogEventArgs<Track> e)
        {
            if (e.Value.IsNew)
            {
                e.Ok = await MainForm.ListItemAddAsync(e.Value);
            }
            else
            {
                e.Ok = await MainForm.ListItemChangeAsync(new List<Track>() { e.Value });
            }
        }

        private async void PresenterFrmListTracks_ItemListChangeDialog(object sender, ItemListDialogEventArgs<Track> e)
        {
            e.Ok = await MainForm.ListItemChangeAsync(e.Values);
        }

        private void PresenterFrmListTracks_ItemDeleteDialog(object sender, ItemDialogEventArgs<Track> e)
        {
            e.Ok = Utils.ShowItemDeleteDialog(e.Value, Resources.QuestionTrackDelete);
        }

        private void PresenterFrmListTracks_ItemListDeleteDialog(object sender, ItemListDialogEventArgs<Track> e)
        {
            e.Ok = Utils.ShowItemDeleteDialog(e.Values, Resources.QuestionTrackListDelete);
        }

        protected override async Task DatabaseListItemSaveAsync(Track value)
        {
            await Task.CompletedTask;
        }

        protected override async Task DatabaseListItemSaveAsync(IEnumerable<Track> list)
        {
            await Task.CompletedTask;
        }

        protected override async Task DatabaseListItemDeleteAsync(IEnumerable<Track> list)
        {
            await Database.Actions.TrackDeleteAsync(list);
        }

        protected override void UpdateColumns()
        {
            FrmList.DataGridView.Columns[nameof(Track.Text)].DisplayIndex = 0;

            FrmList.DataGridView.Columns[nameof(Track.Text)].Visible = true;
            FrmList.DataGridView.Columns[nameof(Track.DateTimeStart)].Visible = true;
            FrmList.DataGridView.Columns[nameof(Track.DateTimeFinish)].Visible = true;
            FrmList.DataGridView.Columns[nameof(Track.DurationAsString)].Visible = true;
            FrmList.DataGridView.Columns[nameof(Track.DurationInMoveAsString)].Visible = true;
            FrmList.DataGridView.Columns[nameof(Track.Distance)].Visible = true;
            FrmList.DataGridView.Columns[nameof(Track.AverageSpeed)].Visible = true;
            FrmList.DataGridView.Columns[nameof(Track.EleAscent)].Visible = true;
            FrmList.DataGridView.Columns[nameof(Track.EleDescent)].Visible = true;
            FrmList.DataGridView.Columns[nameof(Track.NewTilesCount)].Visible = true;
            FrmList.DataGridView.Columns[nameof(Track.EquipmentText)].Visible = true;
            FrmList.DataGridView.Columns[nameof(Track.TagsAsString)].Visible = true;

            FrmList.DataGridView.Columns[nameof(Track.Equipment)].Visible = false;
            FrmList.DataGridView.Columns[nameof(Track.Tags)].Visible = false;

            var visible =
#if SHOW_ALL_COLUMNS
                        true;
#else
                false;
#endif
            FrmList.DataGridView.Columns[nameof(Track.Duration)].Visible = visible;
            FrmList.DataGridView.Columns[nameof(Track.DurationInMove)].Visible = visible;
            FrmList.DataGridView.Columns[nameof(Track.EquipmentId)].Visible = visible;
            FrmList.DataGridView.Columns[nameof(Track.EquipmentBrand)].Visible = visible;
            FrmList.DataGridView.Columns[nameof(Track.EquipmentModel)].Visible = visible;

            columnFormattingIndex = new int[2];

            columnFormattingIndex[0] = FrmList.DataGridView.Columns[nameof(Track.Distance)].Index;
            columnFormattingIndex[1] = FrmList.DataGridView.Columns[nameof(Track.AverageSpeed)].Index;

            FrmList.DataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(DataGridView_CellFormatting);
        }

        private int[] columnFormattingIndex;

        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == columnFormattingIndex[0])
            {
                e.Value = (double)e.Value / 1000;
            }
            else
            {
                if (e.ColumnIndex == columnFormattingIndex[1])
                {
                    e.Value = (double)e.Value * 3.6;
                }
            }
        }

        public override void UpdateSettings()
        {
            FrmList.DataGridView.Columns[nameof(Track.DateTimeStart)].DefaultCellStyle = DataGridViewCellStyles.DateTime;
            FrmList.DataGridView.Columns[nameof(Track.DateTimeFinish)].DefaultCellStyle = DataGridViewCellStyles.DateTime;

            FrmList.DataGridView.Columns[nameof(Track.DurationAsString)].DefaultCellStyle =
                DataGridViewCellStyles.DurationAsString;
            FrmList.DataGridView.Columns[nameof(Track.DurationInMoveAsString)].DefaultCellStyle =
                DataGridViewCellStyles.DurationAsString;

            FrmList.DataGridView.Columns[nameof(Track.Distance)].DefaultCellStyle = DataGridViewCellStyles.Distance;

            FrmList.DataGridView.Columns[nameof(Track.AverageSpeed)].DefaultCellStyle = DataGridViewCellStyles.Speed;

            FrmList.DataGridView.Columns[nameof(Track.EleAscent)].DefaultCellStyle = DataGridViewCellStyles.EleAscent;
            FrmList.DataGridView.Columns[nameof(Track.EleDescent)].DefaultCellStyle = DataGridViewCellStyles.EleAscent;

            FrmList.DataGridView.Columns[nameof(Track.NewTilesCount)].DefaultCellStyle = DataGridViewCellStyles.Count;
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
                case nameof(Track.DateTimeFinish):
                    result = SortOrderComparer.Default.Compare(x.DateTimeFinish, y.DateTimeFinish, sortOrder);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.Text, y.Text, ComparerSortOrder.Ascending);
                    break;
                case nameof(Track.Duration):
                    result = SortOrderComparer.Default.Compare(x.Duration, y.Duration, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTimeStart, y.DateTimeStart, ComparerSortOrder.Descending);
                    break;
                case nameof(Track.DurationInMove):
                    result = SortOrderComparer.Default.Compare(x.DurationInMove, y.DurationInMove, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTimeStart, y.DateTimeStart, ComparerSortOrder.Descending);
                    break;
                case nameof(Track.Distance):
                    result = SortOrderComparer.Default.Compare(x.Distance, y.Distance, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTimeStart, y.DateTimeStart, ComparerSortOrder.Descending);
                    break;
                case nameof(Track.AverageSpeed):
                    result = SortOrderComparer.Default.Compare(x.AverageSpeed, y.AverageSpeed, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTimeStart, y.DateTimeStart, ComparerSortOrder.Descending);
                    break;
                case nameof(Track.EleAscent):
                    result = SortOrderComparer.Default.Compare(x.EleAscent, y.EleAscent, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTimeStart, y.DateTimeStart, ComparerSortOrder.Descending);
                    break;
                case nameof(Track.EleDescent):
                    result = SortOrderComparer.Default.Compare(x.EleDescent, y.EleDescent, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTimeStart, y.DateTimeStart, ComparerSortOrder.Descending);
                    break;
                case nameof(Track.NewTilesCount):
                    result = SortOrderComparer.Default.Compare(x.NewTilesCount, y.NewTilesCount, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTimeStart, y.DateTimeStart, ComparerSortOrder.Descending);
                    break;
                case nameof(Track.EquipmentText):
                    result = EmptyStringComparer.Default.Compare(x.EquipmentText, y.EquipmentText, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTimeStart, y.DateTimeStart, ComparerSortOrder.Descending);
                    break;
                case nameof(Track.TagsAsString):
                    result = EmptyStringComparer.Default.Compare(x.TagsAsString, y.TagsAsString, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTimeStart, y.DateTimeStart, ComparerSortOrder.Descending);
                    break;
            }

            return result;
        }

        protected override async Task<IEnumerable<Track>> DatabaseListLoadAsync(CancellationToken token)
        {
            var tracks = await Database.Default.ListLoadAsync<Track>();

            if (token.IsCancellationRequested) return Enumerable.Empty<Track>();

            foreach (var track in tracks)
            {
                if (token.IsCancellationRequested) return Enumerable.Empty<Track>();

                track.Tags = await Database.Default.ListLoadAsync<TagModel>(track);
            }

            return tracks;
        }

        private void BindingSource_PositionChanged(object sender, EventArgs e)
        {
            MainForm.SelectMapItemAsync(this, Selected);
        }
    }
}