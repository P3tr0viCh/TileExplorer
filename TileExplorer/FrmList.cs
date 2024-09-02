#if DEBUG
//#define SHOW_ALL_COLUMNS
#endif

using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public partial class FrmList : Form, IChildForm, IUpdateDataForm, IListForm
    {
        public IMainForm MainForm => Owner as IMainForm;

        public ChildFormType ChildFormType { get; set; }

        private int[] columnFormattingIndex;

        private string sortColumn = string.Empty;
        private int sortColumnIndex = -1;
        private SortOrder sortOrder = SortOrder.None;

        public FrmList()
        {
            InitializeComponent();
        }

        public static FrmList ShowFrm(Form owner, ChildFormType childFormType)
        {
            var frm = new FrmList()
            {
                Owner = owner,
                ChildFormType = childFormType,
            };

            DebugWrite.Line($"ListType = {childFormType}");

            frm.Show(owner);

            return frm;
        }

        private void FrmListNew_Load(object sender, EventArgs e)
        {
            MainForm.ChildFormOpened(this);

            switch (ChildFormType)
            {
                case ChildFormType.TrackList:
                    bindingSource.DataSource = typeof(Track);
                    break;
                case ChildFormType.MarkerList:
                    bindingSource.DataSource = typeof(Marker);
                    break;
                case ChildFormType.ResultYears:
                    bindingSource.DataSource = typeof(ResultYears);
                    break;
                case ChildFormType.ResultEquipments:
                    bindingSource.DataSource = typeof(ResultEquipments);
                    break;
                case ChildFormType.EquipmentList:
                    bindingSource.DataSource = typeof(Equipment);
                    break;
                default:
                    throw new NotImplementedException();
            }

            dataGridView.DataSource = bindingSource;

            switch (ChildFormType)
            {
                case ChildFormType.TrackList:
                    Text = Resources.TitleListTracks;

                    toolStripLeft.Visible = true;

                    AppSettings.LoadFormState(this, AppSettings.Local.Default.FormStateTrackList);
                    AppSettings.LoadDataGridColumns(dataGridView, AppSettings.Local.Default.ColumnsTrackList);

                    columnFormattingIndex = new int[2];

                    columnFormattingIndex[0] = dataGridView.Columns[nameof(Track.Distance)].Index;
                    columnFormattingIndex[1] = dataGridView.Columns[nameof(Track.AverageSpeed)].Index;

                    dataGridView.CellFormatting +=
                        new DataGridViewCellFormattingEventHandler(DataGridView_CellFormattingTrackList);

                    dataGridView.Columns[nameof(Track.Text)].DisplayIndex = 0;

                    dataGridView.Columns[nameof(Track.Equipment)].Visible = false;

                    var visible =
#if SHOW_ALL_COLUMNS
                        true;
#else
                        false;
#endif
                    dataGridView.Columns[nameof(Track.Gpx)].Visible = visible;
                    dataGridView.Columns[nameof(Track.Duration)].Visible = visible;
                    dataGridView.Columns[nameof(Track.DurationInMove)].Visible = visible;
                    dataGridView.Columns[nameof(Track.EquipmentId)].Visible = visible;
                    dataGridView.Columns[nameof(Track.EquipmentText)].Visible = visible;
                    dataGridView.Columns[nameof(Track.EquipmentBrand)].Visible = visible;
                    dataGridView.Columns[nameof(Track.EquipmentModel)].Visible = visible;

                    sortColumn = nameof(Track.DateTimeStart);
                    sortColumnIndex = dataGridView.Columns[sortColumn].Index;
                    sortOrder = SortOrder.Descending;

                    break;
                case ChildFormType.MarkerList:
                    Text = Resources.TitleListMarkers;

                    toolStripLeft.Visible = true;

                    AppSettings.LoadFormState(this, AppSettings.Local.Default.FormStateMarkerList);
                    AppSettings.LoadDataGridColumns(dataGridView, AppSettings.Local.Default.ColumnsMarkerList);

                    dataGridView.Columns[nameof(Marker.Text)].DisplayIndex = 0;

                    dataGridView.Columns[nameof(Marker.IsTextVisible)].Visible = false;
                    dataGridView.Columns[nameof(Marker.OffsetX)].Visible = false;
                    dataGridView.Columns[nameof(Marker.OffsetY)].Visible = false;

                    break;
                case ChildFormType.ResultYears:
                    Text = Resources.TitleListResultYears;

                    toolStripLeft.Visible = false;

                    AppSettings.LoadFormState(this, AppSettings.Local.Default.FormStateResultYears);
                    AppSettings.LoadDataGridColumns(dataGridView, AppSettings.Local.Default.ColumnsResultYears);

                    dataGridView.Columns[nameof(ResultYears.DurationSum)].Visible = false;

                    columnFormattingIndex = new int[1];
                    columnFormattingIndex[0] = dataGridView.Columns[nameof(ResultYears.Year)].Index;

                    dataGridView.CellFormatting +=
                        new DataGridViewCellFormattingEventHandler(DataGridView_CellFormattingResultYears);

                    break;
                case ChildFormType.ResultEquipments:
                    Text = Resources.TitleListResultEquipments;

                    toolStripLeft.Visible = false;

                    AppSettings.LoadFormState(this, AppSettings.Local.Default.FormStateResultEquipments);
                    AppSettings.LoadDataGridColumns(dataGridView, AppSettings.Local.Default.ColumnsResultEquipments);

                    dataGridView.Columns[nameof(ResultEquipments.DurationSum)].Visible = false;

                    columnFormattingIndex = new int[1];
                    columnFormattingIndex[0] = dataGridView.Columns[nameof(ResultEquipments.Text)].Index;

                    dataGridView.CellFormatting +=
                        new DataGridViewCellFormattingEventHandler(DataGridView_CellFormattingResultEquipments);

                    break;
                case ChildFormType.EquipmentList:
                    Text = Resources.TitleListEquipments;

                    toolStripLeft.Visible = true;

                    AppSettings.LoadFormState(this, AppSettings.Local.Default.FormStateEquipmentList);
                    AppSettings.LoadDataGridColumns(dataGridView, AppSettings.Local.Default.ColumnsEquipmentList);

                    dataGridView.Columns[nameof(Equipment.Name)].Visible = false;

                    break;
                default:
                    throw new NotImplementedException();
            }

            dataGridView.Columns[nameof(BaseId.Id)].Visible = false;

            UpdateSettings();

            if (MainForm.ProgramStatus.Current != Status.Starting)
            {
                UpdateData();
            }
        }

        public async void UpdateData()
        {
            await Task.Run(() =>
            {
                ((Form)MainForm).InvokeIfNeeded(() => _ = UpdateDataAsync());
            });
        }

        private void FrmListNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (ChildFormType)
            {
                case ChildFormType.TrackList:
                    AppSettings.Local.Default.FormStateTrackList = AppSettings.SaveFormState(this);
                    AppSettings.Local.Default.ColumnsTrackList = AppSettings.SaveDataGridColumns(dataGridView);

                    break;
                case ChildFormType.MarkerList:
                    AppSettings.Local.Default.FormStateMarkerList = AppSettings.SaveFormState(this);
                    AppSettings.Local.Default.ColumnsMarkerList = AppSettings.SaveDataGridColumns(dataGridView);

                    break;
                case ChildFormType.ResultYears:
                    AppSettings.Local.Default.FormStateResultYears = AppSettings.SaveFormState(this);
                    AppSettings.Local.Default.ColumnsResultYears = AppSettings.SaveDataGridColumns(dataGridView);

                    break;
                case ChildFormType.ResultEquipments:
                    AppSettings.Local.Default.FormStateResultEquipments = AppSettings.SaveFormState(this);
                    AppSettings.Local.Default.ColumnsResultEquipments = AppSettings.SaveDataGridColumns(dataGridView);

                    break;
                case ChildFormType.EquipmentList:
                    AppSettings.Local.Default.FormStateEquipmentList = AppSettings.SaveFormState(this);
                    AppSettings.Local.Default.ColumnsEquipmentList = AppSettings.SaveDataGridColumns(dataGridView);

                    break;
                default:
                    throw new NotImplementedException();
            }

            AppSettings.LocalSave();
        }

        private void FrmListNew_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm.ChildFormClosed(this);
        }

        public void UpdateSettings()
        {
            switch (ChildFormType)
            {
                case ChildFormType.TrackList:
                    dataGridView.Columns[nameof(Track.DateTimeStart)].DefaultCellStyle =
                        DataGridViewCellStyles.DateTime;
                    dataGridView.Columns[nameof(Track.DateTimeFinish)].DefaultCellStyle =
                        DataGridViewCellStyles.DateTime;

                    dataGridView.Columns[nameof(Track.DurationAsString)].DefaultCellStyle =
                        DataGridViewCellStyles.DurationAsString;
                    dataGridView.Columns[nameof(Track.DurationInMoveAsString)].DefaultCellStyle =
                        DataGridViewCellStyles.DurationAsString;

                    dataGridView.Columns[nameof(Track.Distance)].DefaultCellStyle =
                        DataGridViewCellStyles.Distance;

                    dataGridView.Columns[nameof(Track.AverageSpeed)].DefaultCellStyle =
                        DataGridViewCellStyles.Speed;

                    dataGridView.Columns[nameof(Track.EleAscent)].DefaultCellStyle =
                        DataGridViewCellStyles.EleAscent;

                    dataGridView.Columns[nameof(Track.NewTilesCount)].DefaultCellStyle =
                        DataGridViewCellStyles.Count;

                    break;
                case ChildFormType.MarkerList:
                    dataGridView.Columns[nameof(Marker.Lat)].DefaultCellStyle =
                        DataGridViewCellStyles.LatLng;
                    dataGridView.Columns[nameof(Marker.Lng)].DefaultCellStyle =
                        DataGridViewCellStyles.LatLng;

                    break;
                case ChildFormType.ResultYears:
                    dataGridView.Columns[nameof(ResultYears.Year)].DefaultCellStyle =
                        DataGridViewCellStyles.Year;
                    dataGridView.Columns[nameof(ResultYears.Count)].DefaultCellStyle =
                        DataGridViewCellStyles.Count;
                    dataGridView.Columns[nameof(ResultYears.DistanceSum)].DefaultCellStyle =
                        DataGridViewCellStyles.DistanceSum;
                    dataGridView.Columns[nameof(ResultYears.DurationSumAsString)].DefaultCellStyle =
                        DataGridViewCellStyles.DurationAsString;
                    dataGridView.Columns[nameof(ResultYears.DistanceStep0)].DefaultCellStyle =
                        DataGridViewCellStyles.DistanceStep;
                    dataGridView.Columns[nameof(ResultYears.DistanceStep1)].DefaultCellStyle =
                        DataGridViewCellStyles.DistanceStep;
                    dataGridView.Columns[nameof(ResultYears.DistanceStep2)].DefaultCellStyle =
                        DataGridViewCellStyles.DistanceStep;

                    break;
                case ChildFormType.ResultEquipments:
                    dataGridView.Columns[nameof(ResultEquipments.Count)].DefaultCellStyle =
                        DataGridViewCellStyles.Count;
                    dataGridView.Columns[nameof(ResultEquipments.DistanceSum)].DefaultCellStyle =
                        DataGridViewCellStyles.DistanceSum;
                    dataGridView.Columns[nameof(ResultEquipments.DurationSumAsString)].DefaultCellStyle =
                        DataGridViewCellStyles.DurationAsString;

                    break;
                case ChildFormType.EquipmentList:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private async Task UpdateDataAsync()
        {
            DebugWrite.Line("start");

            var status = MainForm.ProgramStatus.Start(Status.LoadData);

            var errorMsg = string.Empty;

            try
            {
                switch (ChildFormType)
                {
                    case ChildFormType.TrackList:
                        errorMsg = Resources.MsgDatabaseLoadListTrackFail;

                        bindingSource.DataSource = await ListLoadAsync<Track>(new { sortColumn, sortOrder });

                        break;
                    case ChildFormType.MarkerList:
                        errorMsg = Resources.MsgDatabaseLoadListMarkersFail;

                        bindingSource.DataSource = await ListLoadAsync<Marker>();

                        break;
                    case ChildFormType.ResultYears:
                        errorMsg = Resources.MsgDatabaseLoadListResultYearsFail;

                        bindingSource.DataSource = await ListLoadAsync<ResultYears>();

                        break;
                    case ChildFormType.ResultEquipments:
                        errorMsg = Resources.MsgDatabaseLoadListResultEquipmentsFail;

                        bindingSource.DataSource = await ListLoadAsync<ResultEquipments>();

                        break;
                    case ChildFormType.EquipmentList:
                        errorMsg = Resources.MsgDatabaseLoadListEquipmentsFail;

                        bindingSource.DataSource = await ListLoadAsync<Equipment>();

                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(errorMsg, e.Message);
            }
            finally
            {
                MainForm.ProgramStatus.Stop(status);
            }

            DebugWrite.Line("end");
        }

        private async Task<List<T>> ListLoadAsync<T>(object orderBy = null)
        {
            DebugWrite.Line("start");

            try
            {
                return await Database.Default.ListLoadAsync<T>(null, orderBy);
            }
            finally
            {
                DebugWrite.Line("end");
            }
        }

        public BaseId Find(BaseId value)
        {
            return bindingSource.List.OfType<BaseId>().ToList().Find(i => i.Id == value.Id);
        }

        public BaseId Selected
        {
            get
            {
                return ((BindingSource)dataGridView.DataSource).Current as BaseId;
            }
            set
            {
                bindingSource.Position = bindingSource.IndexOf(Find(value));
            }
        }

        public void SetSelected(BaseId value)
        {
            Selected = value;
        }

        private void BindingSource_PositionChanged(object sender, EventArgs e)
        {
            switch (ChildFormType)
            {
                case ChildFormType.TrackList:
                case ChildFormType.MarkerList:
                    MainForm.SelectMapItem(this, Selected);
                    break;
            }
        }

        private void DataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            MainForm.ListItemChange(this, Selected);
        }

        private void TsbtnAdd_Click(object sender, EventArgs e)
        {
            BaseId value = null;

            switch (ChildFormType)
            {
                case ChildFormType.TrackList:
                    value = new Track();
                    break;
                case ChildFormType.MarkerList:
                    value = new Marker();
                    break;
                case ChildFormType.EquipmentList:
                    value = new Equipment();
                    break;
            }

            MainForm.ListItemAdd(this, value);
        }

        private void TsbtnChange_Click(object sender, EventArgs e)
        {
            MainForm.ListItemChange(this, Selected);
        }

        private void TsbtnDelete_Click(object sender, EventArgs e)
        {
            MainForm.ListItemDelete(this, Selected);
        }

        private void DataGridView_CellFormattingTrackList(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == columnFormattingIndex[0])
            {
                e.Value = (double)e.Value / 1000;
            }
            else
            {
                if (e.ColumnIndex == columnFormattingIndex[1])
                {
                    e.Value = (float)e.Value * 3.6;
                }
            }
        }

        private void DataGridView_CellFormattingResultYears(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == columnFormattingIndex[0])
            {
                if ((int)e.Value == 0) e.Value = Resources.TextTotal;
            }
        }

        private void DataGridView_CellFormattingResultEquipments(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == columnFormattingIndex[0])
            {
                if ((e.Value as string).IsEmpty()) e.Value = Resources.TextOther;
            }
        }

        private void ToolStripLeft_MouseEnter(object sender, EventArgs e)
        {
            Activate();
        }

        private void FrmList_Activated(object sender, EventArgs e)
        {
            switch (ChildFormType)
            {
                case ChildFormType.TrackList:
                case ChildFormType.MarkerList:
                    MainForm.SelectMapItem(this, Selected);
                    break;
            }
        }

        private void DataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            switch (ChildFormType)
            {
                case ChildFormType.TrackList:
                    if (e.ColumnIndex == dataGridView.Columns[nameof(Track.DurationAsString)].Index)
                    {
                        sortColumn = nameof(Track.Duration);
                    }
                    else
                    {
                        if (e.ColumnIndex == dataGridView.Columns[nameof(Track.DurationInMoveAsString)].Index)
                        {
                            sortColumn = nameof(Track.DurationInMove);
                        }
                        else
                        {
                            sortColumn = dataGridView.Columns[e.ColumnIndex].Name;
                        }
                    }

                    if (sortColumnIndex == e.ColumnIndex)
                    {
                        sortOrder = sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
                    }

                    sortColumnIndex = e.ColumnIndex;

                    UpdateData();

                    break;
            }
        }

        private void DataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (sortColumn == string.Empty || sortColumnIndex == -1)
            {
                return;
            }

            switch (ChildFormType)
            {
                case ChildFormType.TrackList:
                    dataGridView.Columns[sortColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;

                    break;
            }
        }
    }
}