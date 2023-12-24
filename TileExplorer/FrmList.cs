#if DEBUG
//#define SHOW_ALL_COLUMNS
#endif

using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private int columnDistanceIndex = -1;

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

            Debug.WriteLine("ListType = " + childFormType);

            frm.Show(owner);

            return frm;
        }

        private void FrmListNew_Load(object sender, EventArgs e)
        {
            switch (ChildFormType)
            {
                case ChildFormType.TrackList:
                    bindingSource.DataSource = typeof(Track);
                    break;
                case ChildFormType.MarkerList:
                    bindingSource.DataSource = typeof(Marker);
                    break;
                case ChildFormType.Results:
                    bindingSource.DataSource = typeof(Results);
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

                    AppSettings.LoadFormState(this, AppSettings.Default.FormStateTrackList);
                    AppSettings.LoadDataGridColumns(dataGridView, AppSettings.Default.ColumnsTrackList);

                    columnDistanceIndex = dataGridView.Columns[nameof(Track.Distance)].Index;
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
                    dataGridView.Columns[nameof(Track.EquipmentId)].Visible = visible;
                    dataGridView.Columns[nameof(Track.EquipmentText)].Visible = visible;
                    dataGridView.Columns[nameof(Track.EquipmentBrand)].Visible = visible;
                    dataGridView.Columns[nameof(Track.EquipmentModel)].Visible = visible;

                    break;
                case ChildFormType.MarkerList:
                    Text = Resources.TitleListMarkers;

                    toolStripLeft.Visible = true;

                    AppSettings.LoadFormState(this, AppSettings.Default.FormStateMarkerList);
                    AppSettings.LoadDataGridColumns(dataGridView, AppSettings.Default.ColumnsMarkerList);

                    dataGridView.Columns[nameof(Marker.Text)].DisplayIndex = 0;

                    dataGridView.Columns[nameof(Marker.IsTextVisible)].Visible = false;
                    dataGridView.Columns[nameof(Marker.OffsetX)].Visible = false;
                    dataGridView.Columns[nameof(Marker.OffsetY)].Visible = false;

                    break;
                case ChildFormType.Results:
                    Text = Resources.TitleListResults;

                    toolStripLeft.Visible = false;

                    AppSettings.LoadFormState(this, AppSettings.Default.FormStateResults);
                    AppSettings.LoadDataGridColumns(dataGridView, AppSettings.Default.ColumnsResults);

                    break;
                case ChildFormType.EquipmentList:
                    Text = Resources.TitleListEquipments;

                    toolStripLeft.Visible = true;

                    AppSettings.LoadFormState(this, AppSettings.Default.FormStateEquipmentList);
                    AppSettings.LoadDataGridColumns(dataGridView, AppSettings.Default.ColumnsEquipmentList);

                    dataGridView.Columns[nameof(Equipment.Name)].Visible = false;

                    break;
                default:
                    throw new NotImplementedException();
            }

            dataGridView.Columns[nameof(BaseId.Id)].Visible = false;

            UpdateSettings();

            _ = UpdateDataAsync();

            MainForm.ChildFormOpened(this);
        }

        private void FrmListNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (ChildFormType)
            {
                case ChildFormType.TrackList:
                    AppSettings.Default.FormStateTrackList = AppSettings.SaveFormState(this);
                    AppSettings.Default.ColumnsTrackList = AppSettings.SaveDataGridColumns(dataGridView);

                    break;
                case ChildFormType.MarkerList:
                    AppSettings.Default.FormStateMarkerList = AppSettings.SaveFormState(this);
                    AppSettings.Default.ColumnsMarkerList = AppSettings.SaveDataGridColumns(dataGridView);

                    break;
                case ChildFormType.Results:
                    AppSettings.Default.FormStateResults = AppSettings.SaveFormState(this);
                    AppSettings.Default.ColumnsResults = AppSettings.SaveDataGridColumns(dataGridView);

                    break;
                case ChildFormType.EquipmentList:
                    AppSettings.Default.FormStateEquipmentList = AppSettings.SaveFormState(this);
                    AppSettings.Default.ColumnsEquipmentList = AppSettings.SaveDataGridColumns(dataGridView);

                    break;
                default:
                    throw new NotImplementedException();
            }

            AppSettings.Default.Save();
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
                    dataGridView.Columns[nameof(Track.Duration)].DefaultCellStyle =
                        DataGridViewCellStyles.Duration;
                    dataGridView.Columns[nameof(Track.Distance)].DefaultCellStyle =
                        DataGridViewCellStyles.Distance;
                    dataGridView.Columns[nameof(Track.NewTilesCount)].DefaultCellStyle =
                        DataGridViewCellStyles.Count;

                    break;
                case ChildFormType.MarkerList:
                    dataGridView.Columns[nameof(Marker.Lat)].DefaultCellStyle =
                        DataGridViewCellStyles.LatLng;
                    dataGridView.Columns[nameof(Marker.Lng)].DefaultCellStyle =
                        DataGridViewCellStyles.LatLng;

                    break;
                case ChildFormType.Results:
                    dataGridView.Columns[nameof(Results.Year)].DefaultCellStyle =
                        DataGridViewCellStyles.Year;
                    dataGridView.Columns[nameof(Results.Count)].DefaultCellStyle =
                        DataGridViewCellStyles.Count;
                    dataGridView.Columns[nameof(Results.DistanceSum)].DefaultCellStyle =
                        DataGridViewCellStyles.DistanceSum;

                    break;
                case ChildFormType.EquipmentList:
                    dataGridView.Columns[nameof(Equipment.Count)].DefaultCellStyle =
                        DataGridViewCellStyles.Count;
                    dataGridView.Columns[nameof(Equipment.DistanceSum)].DefaultCellStyle =
                        DataGridViewCellStyles.DistanceSum;

                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public async Task UpdateDataAsync()
        {
            MainForm.Status = ProgramStatus.LoadData;

            var errorMsg = string.Empty;

            try
            {
                switch (ChildFormType)
                {
                    case ChildFormType.TrackList:
                        errorMsg = Resources.MsgDatabaseLoadListTrackFail;

                        bindingSource.DataSource = await ListLoadAsync<Track>();

                        break;
                    case ChildFormType.MarkerList:
                        errorMsg = Resources.MsgDatabaseLoadListMarkersFail;

                        bindingSource.DataSource = await ListLoadAsync<Marker>();

                        break;
                    case ChildFormType.Results:
                        errorMsg = Resources.MsgDatabaseLoadListResultsFail;

                        bindingSource.DataSource = await ListLoadAsync<Results>();

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
                Utils.WriteError(e);

                Msg.Error(errorMsg, e.Message);
            }
            finally
            {
                MainForm.Status = ProgramStatus.Idle;
            }
        }

        private async Task<List<T>> ListLoadAsync<T>()
        {
            Utils.WriteDebug("start");

            try
            {
                return await Database.Default.ListLoadAsync<T>();
            }
            finally
            {
                Utils.WriteDebug("end");
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
            if (e.ColumnIndex == columnDistanceIndex)
            {
                e.Value = (double)e.Value / 1000;
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
    }
}