#if DEBUG
//#define SHOW_ALL_COLUMNS
#endif

using P3tr0viCh.Utils;
using System;
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

        public ChildFormType FormType { get; private set; }

        private object data;
        public object Value => data;

        internal readonly PresenterChildForm childFormPresenter;

        private int[] columnFormattingIndex;

        public FrmList()
        {
            InitializeComponent();

            childFormPresenter = new PresenterChildForm(this);
        }

        public static FrmList ShowFrm(Form owner, ChildFormType childFormType, object value = default)
        {
            var frm = new FrmList()
            {
                Owner = owner,
                FormType = childFormType,
                data = value,
            };

            DebugWrite.Line($"ListType = {childFormType}");

            frm.Show(owner);

            return frm;
        }

        private async void FrmListNew_Load(object sender, EventArgs e)
        {
            Type type;

            switch (FormType)
            {
                case ChildFormType.TrackList:
                    type = typeof(Track);
                    break;
                case ChildFormType.MarkerList:
                    type = typeof(Marker);
                    break;
                case ChildFormType.ResultYears:
                    type = typeof(ResultYears);
                    break;
                case ChildFormType.ResultEquipments:
                    type = typeof(ResultEquipments);
                    break;
                case ChildFormType.EquipmentList:
                    type = typeof(Equipment);
                    break;
                case ChildFormType.TileInfo:
                    type = typeof(Track);
                    break;
                default:
                    throw new NotImplementedException();
            }

            bindingSource.DataSource = type;

            dataGridView.DataSource = bindingSource;

            switch (FormType)
            {
                case ChildFormType.TrackList:
                    Text = Resources.TitleListTracks;

                    toolStripLeft.Visible = true;

                    toolStripSeparator1.Visible = true;
                    tsbtnChartTrackEle.Visible = true;

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
                    sortOrderDescending = true;

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

                    sortColumn = nameof(Marker.Text);
                    sortColumnIndex = dataGridView.Columns[sortColumn].Index;
                    sortOrderDescending = true;

                    break;
                case ChildFormType.ResultYears:
                    Text = Resources.TitleListResultYears;

                    toolStripLeft.Visible = false;

                    AppSettings.LoadFormState(this, AppSettings.Local.Default.FormStateResultYears);
                    AppSettings.LoadDataGridColumns(dataGridView, AppSettings.Local.Default.ColumnsResultYears);

                    dataGridView.Columns[nameof(ResultYears.DurationSum)].Visible = false;
                    dataGridView.Columns[nameof(ResultYears.DurationSumAsString)].Visible = true;
                    dataGridView.Columns[nameof(ResultYears.DistanceSum)].Visible = true;

                    dataGridView.Columns[nameof(ResultYears.Year)].DisplayIndex = 0;
                    dataGridView.Columns[nameof(ResultYears.Count)].DisplayIndex = 1;
                    dataGridView.Columns[nameof(ResultYears.DurationSumAsString)].DisplayIndex = 2;

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
                case ChildFormType.TileInfo:
                    AppSettings.LoadFormState(this, AppSettings.Local.Default.FormStateTileInfo);
                    AppSettings.LoadDataGridColumns(dataGridView, AppSettings.Local.Default.ColumnsTileInfo);

                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        column.Visible = false;
                    }

                    dataGridView.Columns[nameof(Track.DateTimeStart)].Visible = true;
                    dataGridView.Columns[nameof(Track.DateTimeStart)].DisplayIndex = 0;

                    dataGridView.Columns[nameof(Track.Text)].Visible = true;

                    toolStripLeft.Visible = false;

                    break;
                default:
                    throw new NotImplementedException();
            }

            dataGridView.Columns[nameof(BaseId.Id)].Visible = false;

            UpdateSettings();

            if (MainForm.ProgramStatus.Current != Status.Starting)
            {
                await UpdateDataAsync();
            }
        }

        private void FrmListNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (FormType)
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
                case ChildFormType.TileInfo:
                    AppSettings.Local.Default.FormStateTileInfo = AppSettings.SaveFormState(this);
                    AppSettings.Local.Default.ColumnsTileInfo = AppSettings.SaveDataGridColumns(dataGridView);

                    break;
                default:
                    throw new NotImplementedException();
            }

            AppSettings.LocalSave();
        }

        public void UpdateSettings()
        {
            switch (FormType)
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

                    dataGridView.Columns[nameof(ResultYears.DurationSumAsString)].DefaultCellStyle =
                        DataGridViewCellStyles.DurationAsString;

                    dataGridView.Columns[nameof(ResultYears.DistanceSum)].DefaultCellStyle =
                        DataGridViewCellStyles.DistanceSum;
                    dataGridView.Columns[nameof(ResultYears.DistanceStep0)].DefaultCellStyle =
                        DataGridViewCellStyles.DistanceStep;
                    dataGridView.Columns[nameof(ResultYears.DistanceStep1)].DefaultCellStyle =
                        DataGridViewCellStyles.DistanceStep;
                    dataGridView.Columns[nameof(ResultYears.DistanceStep2)].DefaultCellStyle =
                        DataGridViewCellStyles.DistanceStep;

                    dataGridView.Columns[nameof(ResultYears.EleAscentSum)].DefaultCellStyle =
                        DataGridViewCellStyles.EleAscentSum;
                    dataGridView.Columns[nameof(ResultYears.EleAscentStep0)].DefaultCellStyle =
                        DataGridViewCellStyles.EleAscentStep;
                    dataGridView.Columns[nameof(ResultYears.EleAscentStep1)].DefaultCellStyle =
                        DataGridViewCellStyles.EleAscentStep;
                    dataGridView.Columns[nameof(ResultYears.EleAscentStep2)].DefaultCellStyle =
                        DataGridViewCellStyles.EleAscentStep;

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
                case ChildFormType.TileInfo:
                    dataGridView.Columns[nameof(Track.DateTimeStart)].DefaultCellStyle =
                        DataGridViewCellStyles.DateTime;

                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private readonly WrapperCancellationTokenSource ctsList = new WrapperCancellationTokenSource();

        public async Task UpdateDataAsync()
        {
            DebugWrite.Line("start");

            ctsList.Start();

            var status = MainForm.ProgramStatus.Start(Status.LoadData);

            var errorMsg = string.Empty;

            try
            {
                // await Task.Delay(3000, ctsList.Token);

                switch (FormType)
                {
                    case ChildFormType.TrackList:
                        errorMsg = Resources.MsgDatabaseLoadListTrackFail;

                        bindingSource.DataSource = await Database.Default.ListLoadAsync<Track>();

                        break;
                    case ChildFormType.MarkerList:
                        errorMsg = Resources.MsgDatabaseLoadListMarkersFail;

                        bindingSource.DataSource = await Database.Default.ListLoadAsync<Marker>();

                        break;
                    case ChildFormType.ResultYears:
                        errorMsg = Resources.MsgDatabaseLoadListResultYearsFail;

                        bindingSource.DataSource = await Database.Default.ListLoadAsync<ResultYears>();

                        break;
                    case ChildFormType.ResultEquipments:
                        errorMsg = Resources.MsgDatabaseLoadListResultEquipmentsFail;

                        bindingSource.DataSource = await Database.Default.ListLoadAsync<ResultEquipments>();

                        break;
                    case ChildFormType.EquipmentList:
                        errorMsg = Resources.MsgDatabaseLoadListEquipmentsFail;

                        bindingSource.DataSource = await Database.Default.ListLoadAsync<Equipment>();

                        break;
                    case ChildFormType.TileInfo:
                        errorMsg = Resources.MsgDatabaseLoadListTileInfoFail;

                        var tile = (Tile)data;

                        Text = string.Format(Resources.StatusTileId, tile.X, tile.Y);

                        bindingSource.DataSource = await Database.Default.ListLoadAsync<Track>(tile);

                        break;
                    default:
                        throw new NotImplementedException();
                }

                Sort();
            }
            catch (TaskCanceledException e)
            {
                DebugWrite.Error(e);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(errorMsg, e.Message);
            }
            finally
            {
                ctsList.Finally();

                MainForm.ProgramStatus.Stop(status);
            }

            DebugWrite.Line("end");
        }

        public BaseId Find(BaseId value)
        {
            if (value == null) return null;

            return bindingSource.Cast<BaseId>().Where(item => item.Id == value.Id).FirstOrDefault();
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

        public int Count => bindingSource.Count;

        public void SetSelected(BaseId value)
        {
            Selected = value;
        }

        private void BindingSource_PositionChanged(object sender, EventArgs e)
        {
            switch (FormType)
            {
                case ChildFormType.TrackList:
                case ChildFormType.MarkerList:
                case ChildFormType.TileInfo:
                    if (MainForm.ProgramStatus.IsIdle)
                    {
                        MainForm.SelectMapItemAsync(this, Selected);
                    }
                    break;
            }
        }

        private void DataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            MainForm.ListItemChangeAsync(this, Selected);
        }

        private void TsbtnAdd_Click(object sender, EventArgs e)
        {
            BaseId value = null;

            switch (FormType)
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
            MainForm.ListItemChangeAsync(this, Selected);
        }

        private async void TsbtnDelete_Click(object sender, EventArgs e)
        {
            await MainForm.ListItemDeleteAsync(this, Selected);
        }

        private void ShowTrackEleChart()
        {
            if (Selected == null) return;

            MainForm.ShowChartTrackEle(this, Selected as Track);
        }

        private void TsbtnTrackEleChart_Click(object sender, EventArgs e)
        {
            ShowTrackEleChart();
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
            switch (FormType)
            {
                case ChildFormType.TileInfo:
                case ChildFormType.TrackList:
                case ChildFormType.MarkerList:
                    MainForm.SelectMapItemAsync(this, Selected);
                    break;
            }
        }

        private void DataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            switch (FormType)
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
                        sortOrderDescending = !sortOrderDescending;
                    }

                    sortColumnIndex = e.ColumnIndex;

                    Sort();

                    break;
            }
        }

        private void DataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (sortColumn == string.Empty || sortColumnIndex == -1)
            {
                return;
            }

            switch (FormType)
            {
                case ChildFormType.TrackList:
                    dataGridView.Columns[sortColumnIndex].HeaderCell.SortGlyphDirection =
                        sortOrderDescending ? SortOrder.Descending : SortOrder.Ascending;

                    break;
            }
        }

        private void DataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    dataGridView.CurrentCell = dataGridView[e.ColumnIndex, e.RowIndex];
                }
            }
        }

        private void FrmList_FormClosed(object sender, FormClosedEventArgs e)
        {
            ctsList.Cancel();
        }

        public void ListItemChange(BaseId value)
        {
            var item = Find(value);

            if (item == null)
            {
                bindingSource.Add(value);
            }
            else
            {
                var index = bindingSource.IndexOf(item);

                bindingSource.List[index] = value;

                bindingSource.ResetItem(index);
            }

            Sort();
        }

        public void ListItemDelete(BaseId value)
        {
            var item = Find(value);

            if (item == null) return;

            if (item == Selected)
            {
                Selected = null;
            }

            bindingSource.Remove(item);
        }
    }
}