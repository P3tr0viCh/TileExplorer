using P3tr0viCh.Utils;
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
    internal class PresenterFrmListTracks : PresenterFrmList<Track>
    {
        private ToolStripSeparator tssepChartTrackEle;
        private ToolStripButton tsbtnChartTrackEle;
        private ToolStripSeparator tssepOpenDailyNote;
        private ToolStripButton tsbtnOpenDailyNote;

        public override ChildFormType FormType => ChildFormType.TrackList;

        protected override string FormTitle => Resources.TitleListTracks;

        public PresenterFrmListTracks(IChildFormList frmList) : base(frmList)
        {
            Grants = Grants.AddFlag(FrmListGrant.MultiChange);

            BindingSource.PositionChanged += BindingSource_PositionChanged;

            ItemsChangeDialog += PresenterFrmListTracks_ItemsChangeDialog;
            ItemsDeleteDialog += PresenterFrmListTracks_ItemsDeleteDialog;

            ItemsChanged += PresenterFrmListTracks_ItemsChanged;
            ItemsDeleted += PresenterFrmListTracks_ItemsDeleted;

            ListChanged += PresenterFrmListTracks_ListChanged;

            CreateToolStripButtons();
        }

        private void PresenterFrmListTracks_ListChanged(object sender, EventArgs e)
        {
            tsbtnChartTrackEle.Enabled =
            tsbtnOpenDailyNote.Enabled =
                !FrmList.DataGridView.IsEmpty();
        }

        private void CreateToolStripButtons()
        {
            tssepChartTrackEle = new ToolStripSeparator();

            tsbtnChartTrackEle = new ToolStripButton
            {
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Image = ResourcesImages.ChartTrackEle,
                ImageTransparentColor = System.Drawing.Color.Magenta,
                Name = "tsbtnChartTrackEle",
                Size = new System.Drawing.Size(39, 28),
                ToolTipText = Resources.TextButtonChartTrackEleToolTip
            };
            tsbtnChartTrackEle.Click += new EventHandler(TsbtnChartTrackEle_Click);

            tssepOpenDailyNote = new ToolStripSeparator();

            tsbtnOpenDailyNote = new ToolStripButton
            {
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Image = ResourcesImages.OpenDailyNote,
                ImageTransparentColor = System.Drawing.Color.Magenta,
                Name = "tsbtnOpenDailyNote",
                Size = new System.Drawing.Size(39, 28),
                ToolTipText = Resources.TextButtonOpenDailyNoteToolTip
            };
            tsbtnOpenDailyNote.Click += new EventHandler(TsbtnOpenDailyNote_Click);

            FrmList.ToolStrip.SuspendLayout();

            FrmList.ToolStrip.Items.AddRange(new ToolStripItem[] {
                tssepChartTrackEle, tsbtnChartTrackEle,
                tssepOpenDailyNote, tsbtnOpenDailyNote
            });

            FrmList.ToolStrip.ResumeLayout(false);
            FrmList.ToolStrip.PerformLayout();
        }

        protected override void LoadFormState()
        {
            base.LoadFormState();

            PresenterDataGridView.SortColumn = nameof(Track.DateTimeStart);
            PresenterDataGridView.SortOrder = ComparerSortOrder.Descending;
        }

        private async Task PresenterFrmListTracks_ItemsChangeDialog(object sender, ItemsDialogEventArgs<Track> e)
        {
            e.Ok = await MainForm.TrackChangeAsync(e.Values);
        }

        private async Task PresenterFrmListTracks_ItemsDeleteDialog(object sender, ItemsDialogEventArgs<Track> e)
        {
            e.Ok = Utils.ShowItemDeleteDialog(e.Values, Resources.QuestionTrackDelete, Resources.QuestionTrackListDelete);

            await Task.CompletedTask;
        }

        private async void PresenterFrmListTracks_ItemsChanged(object sender, ItemsEventArgs<Track> e)
        {
            await MainForm.TrackChangedAsync(e.Values);
        }

        private async void PresenterFrmListTracks_ItemsDeleted(object sender, ItemsEventArgs<Track> e)
        {
            await MainForm.TracksDeletedAsync(e.Values);
        }

        protected override async Task DatabaseListItemsSaveAsync(IEnumerable<Track> list)
        {
            await Task.CompletedTask;
        }

        protected override void UpdateColumns()
        {
            base.UpdateColumns();

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
            tssepOpenDailyNote.Visible =
            tsbtnOpenDailyNote.Visible =
                !AppSettings.Default.DirectoryObsidianVault.IsEmpty();

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
                case nameof(Track.DurationAsString):
                    result = SortOrderComparer.Default.Compare(x.Duration, y.Duration, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTimeStart, y.DateTimeStart, ComparerSortOrder.Descending);
                    break;
                case nameof(Track.DurationInMoveAsString):
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
            return await Database.Default.LoadTracksWithTagsAsync(token);
        }

        private void BindingSource_PositionChanged(object sender, EventArgs e)
        {
            MainForm.SelectMapItemAsync(this, Selected);
        }

        private void EquipmentsChange(IEnumerable<Equipment> equipments, bool delete)
        {
            for (var i = 0; i < List.Count; i++)
            {
                var equipment = equipments.WhereId(List[i].EquipmentId).FirstOrDefault();

                if (equipment == default) continue;

                List[i].Equipment = delete ? null : equipment;

                BindingSource.ResetItem(i);
            }
        }

        private void TagsChange(IEnumerable<TagModel> tags, bool delete)
        {
            for (var i = 0; i < List.Count; i++)
            {
                foreach (var tag in tags)
                {
                    var exists = List[i].Tags.WhereId(tag.Id).FirstOrDefault();

                    if (exists == default) continue;

                    if (delete)
                    {
                        var newTags = List[i].Tags.ToList();

                        newTags.Remove(exists);

                        List[i].Tags = newTags;
                    }
                    else
                    {
                        exists.Assign(tag);
                    }

                    BindingSource.ResetItem(i);

                    break;
                }
            }
        }

        public override void ListItemsChange(IEnumerable<IBaseId> list)
        {
            if (list is IEnumerable<Track> tracks)
            {
                base.ListItemsChange(tracks);

                return;
            }

            if (list is IEnumerable<Equipment> equipments)
            {
                EquipmentsChange(equipments, false);

                return;
            }

            if (list is IEnumerable<TagModel> tags)
            {
                TagsChange(tags, false);

                return;
            }
        }

        public override void ListItemsDelete(IEnumerable<IBaseId> list)
        {
            if (list is IEnumerable<Track> tracks)
            {
                base.ListItemsDelete(tracks);

                return;
            }

            if (list is IEnumerable<Equipment> equipments)
            {
                EquipmentsChange(equipments, true);

                return;
            }

            if (list is IEnumerable<TagModel> tags)
            {
                TagsChange(tags, true);

                return;
            }
        }

        private void ShowTrackEleChart()
        {
            var selectedCount = SelectedList.Count();

            if (selectedCount == 0) return;

            if (selectedCount > 5)
            {
                if (!Msg.Question(Resources.QuestionOpenManyForms, selectedCount)) return;
            }

            foreach (var track in SelectedList.Cast<Track>())
            {
                FrmChartTrackEle.OpenFrm(MainForm as Form, track);
            }
        }

        private void TsbtnChartTrackEle_Click(object sender, EventArgs e)
        {
            ShowTrackEleChart();
        }

        private void OpenDailyNote()
        {
            var selectedCount = SelectedList.Count();

            if (selectedCount == 0) return;

            var track = Selected;

            if (selectedCount > 1)
            {
                FrmList.DataGridView.ClearSelection();

                FrmList.DataGridView.SetSelectedRows(track);
            }

            Utils.Obsidian.OpenDailyNote(track.DateTimeStart);
        }

        private void TsbtnOpenDailyNote_Click(object sender, EventArgs e)
        {
            OpenDailyNote();
        }
    }
}