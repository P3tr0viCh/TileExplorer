using Newtonsoft.Json.Linq;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Interfaces;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer.Presenters
{
    internal class PresenterFrmListTracks : PresenterFrmListBase<Track>
    {
        public override FrmListType ListType => FrmListType.TrackList;

        public override ChildFormType FormType => ChildFormType.TrackList;

        public PresenterFrmListTracks(IFrmList frmList) : base(frmList)
        {
            Grants = Grants.AddFlag(FrmListGrant.MultiChange);

            OnPositionChanged += PresenterFrmListMarkers_OnPositionChanged;

            ItemChangeDialog += PresenterFrmListTracks_ItemChangeDialog;
            
            ItemListChangeDialog += PresenterFrmListTracks_ItemListChangeDialog;

            ItemListDeleteDialog += PresenterFrmListTracks_ItemListDeleteDialog;
        }

        protected override string FormTitle => Resources.TitleListTracks;

        protected override void LoadFormState()
        {
            base.LoadFormState();

            presenterDataGridView.SortColumn = nameof(Track.DateTimeStart);
            presenterDataGridView.SortOrder = ComparerSortOrder.Descending;
        }

        private async void PresenterFrmListTracks_ItemChangeDialog(object sender, ItemDialogEventArgs e)
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

        private async void PresenterFrmListTracks_ItemListChangeDialog(object sender, ItemListDialogEventArgs e)
        {
            e.Ok = await MainForm.ListItemChangeAsync(e.Values);
        }

        private void PresenterFrmListTracks_ItemListDeleteDialog(object sender, ItemListDialogEventArgs e)
        {
            e.Ok = Utils.ShowItemDeleteDialog(e.Values,
               Resources.QuestionTrackDelete, Resources.QuestionTrackListDelete);
        }

        protected override async Task ListItemSaveAsync(Track value)
        {
            await Task.CompletedTask;
        }

        protected override async Task ListItemSaveAsync(IEnumerable<Track> list)
        {
            await Task.CompletedTask;
        }

        protected override async Task ListItemDeleteAsync(IEnumerable<Track> list)
        {
            await Database.Actions.TrackDeleteAsync(list);
        }

        protected override void UpdateColumns()
        {
            DataGridView.Columns[nameof(Track.Text)].DisplayIndex = 0;

            DataGridView.Columns[nameof(Track.Text)].Visible = true;
            DataGridView.Columns[nameof(Track.DateTimeStart)].Visible = true;
            DataGridView.Columns[nameof(Track.DateTimeFinish)].Visible = true;
            DataGridView.Columns[nameof(Track.DurationAsString)].Visible = true;
            DataGridView.Columns[nameof(Track.DurationInMoveAsString)].Visible = true;
            DataGridView.Columns[nameof(Track.Distance)].Visible = true;
            DataGridView.Columns[nameof(Track.AverageSpeed)].Visible = true;
            DataGridView.Columns[nameof(Track.EleAscent)].Visible = true;
            DataGridView.Columns[nameof(Track.EleDescent)].Visible = true;
            DataGridView.Columns[nameof(Track.NewTilesCount)].Visible = true;
            DataGridView.Columns[nameof(Track.EquipmentText)].Visible = true;
            DataGridView.Columns[nameof(Track.TagsAsString)].Visible = true;

            DataGridView.Columns[nameof(Track.Equipment)].Visible = false;
            DataGridView.Columns[nameof(Track.Tags)].Visible = false;

            var visible =
#if SHOW_ALL_COLUMNS
                        true;
#else
                false;
#endif
            DataGridView.Columns[nameof(Track.Duration)].Visible = visible;
            DataGridView.Columns[nameof(Track.DurationInMove)].Visible = visible;
            DataGridView.Columns[nameof(Track.EquipmentId)].Visible = visible;
            DataGridView.Columns[nameof(Track.EquipmentBrand)].Visible = visible;
            DataGridView.Columns[nameof(Track.EquipmentModel)].Visible = visible;

            columnFormattingIndex = new int[2];

            columnFormattingIndex[0] = DataGridView.Columns[nameof(Track.Distance)].Index;
            columnFormattingIndex[1] = DataGridView.Columns[nameof(Track.AverageSpeed)].Index;

            DataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(DataGridView_CellFormatting);
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
            DataGridView.Columns[nameof(Track.DateTimeStart)].DefaultCellStyle = DataGridViewCellStyles.DateTime;
            DataGridView.Columns[nameof(Track.DateTimeFinish)].DefaultCellStyle = DataGridViewCellStyles.DateTime;

            DataGridView.Columns[nameof(Track.DurationAsString)].DefaultCellStyle =
                DataGridViewCellStyles.DurationAsString;
            DataGridView.Columns[nameof(Track.DurationInMoveAsString)].DefaultCellStyle =
                DataGridViewCellStyles.DurationAsString;

            DataGridView.Columns[nameof(Track.Distance)].DefaultCellStyle = DataGridViewCellStyles.Distance;

            DataGridView.Columns[nameof(Track.AverageSpeed)].DefaultCellStyle = DataGridViewCellStyles.Speed;

            DataGridView.Columns[nameof(Track.EleAscent)].DefaultCellStyle = DataGridViewCellStyles.EleAscent;
            DataGridView.Columns[nameof(Track.EleDescent)].DefaultCellStyle = DataGridViewCellStyles.EleAscent;

            DataGridView.Columns[nameof(Track.NewTilesCount)].DefaultCellStyle = DataGridViewCellStyles.Count;
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

        protected override async Task<IEnumerable<Track>> ListLoadAsync()
        {
            var tracks = await Database.Default.ListLoadAsync<Track>();

            foreach (var track in tracks)
            {
                track.Tags = await Database.Default.ListLoadAsync<TagModel>(track);
            }

            return tracks;
        }

        private void PresenterFrmListMarkers_OnPositionChanged()
        {
            MainForm.SelectMapItemAsync(this, Selected);
        }
    }
}