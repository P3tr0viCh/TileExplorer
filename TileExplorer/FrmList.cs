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
    public partial class FrmList : Form, IChildForm
    {
        public IMainForm MainForm => Owner as IMainForm;

        public ChildFormType ListType { get; set; }

        public FrmList()
        {
            InitializeComponent();
        }

        public static FrmList ShowFrm(Form owner, ChildFormType listType)
        {
            var frm = new FrmList()
            {
                Owner = owner,
                ListType = listType,
            };

            Debug.WriteLine("ListType = " + listType);

            frm.Show(owner);

            return frm;
        }

        private void FrmListNew_Load(object sender, EventArgs e)
        {
            switch (ListType)
            {
                case ChildFormType.Results:
                    bindingSource.DataSource = typeof(Results);
                    break;
                case ChildFormType.Tracks:
                    bindingSource.DataSource = typeof(Track);
                    break;
                case ChildFormType.Markers:
                    bindingSource.DataSource = typeof(Marker);
                    break;
                default:
                    throw new NotImplementedException();
            }

            dataGridView.DataSource = bindingSource;

            dataGridView.Columns[nameof(BaseId.Id)].Visible = false;

            switch (ListType)
            {
                case ChildFormType.Results:
                    Text = Resources.TitleListResults;

                    AppSettings.LoadFormState(this, AppSettings.Default.FormStateResults);
                    AppSettings.LoadDataGridColumns(dataGridView, AppSettings.Default.ColumnsResults);

                    break;
                case ChildFormType.Tracks:
                    Text = Resources.TitleListTracks;

                    dataGridView.Columns[nameof(Track.Text)].DisplayIndex = 0;

                    AppSettings.LoadFormState(this, AppSettings.Default.FormStateTrackList);
                    AppSettings.LoadDataGridColumns(dataGridView, AppSettings.Default.ColumnsTrackList);

                    break;
                case ChildFormType.Markers:
                    Text = Resources.TitleListMarkers;

                    dataGridView.Columns[nameof(Marker.Text)].DisplayIndex = 0;

                    dataGridView.Columns[nameof(Marker.IsTextVisible)].Visible = false;
                    dataGridView.Columns[nameof(Marker.OffsetX)].Visible = false;
                    dataGridView.Columns[nameof(Marker.OffsetY)].Visible = false;

                    AppSettings.LoadFormState(this, AppSettings.Default.FormStateMarkerList);
                    AppSettings.LoadDataGridColumns(dataGridView, AppSettings.Default.ColumnsMarkerList);

                    break;
                default:
                    throw new NotImplementedException();
            }

            UpdateSettings();

            _ = UpdateDataAsync();

            MainForm.ChildFormOpened(this);
        }

        private void FrmListNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (ListType)
            {
                case ChildFormType.Results:
                    AppSettings.Default.FormStateResults = AppSettings.SaveFormState(this);
                    AppSettings.Default.ColumnsResults = AppSettings.SaveDataGridColumns(dataGridView);
                    break;
                case ChildFormType.Tracks:
                    AppSettings.Default.FormStateTrackList = AppSettings.SaveFormState(this);
                    AppSettings.Default.ColumnsTrackList = AppSettings.SaveDataGridColumns(dataGridView);
                    break;
                case ChildFormType.Markers:
                    AppSettings.Default.FormStateMarkerList = AppSettings.SaveFormState(this);
                    AppSettings.Default.ColumnsMarkerList = AppSettings.SaveDataGridColumns(dataGridView);
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
            switch (ListType)
            {
                case ChildFormType.Results:
                    dataGridView.Columns[nameof(Results.Year)].DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        Format = "####"
                    };
                    dataGridView.Columns[nameof(Results.Count)].DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.TopRight
                    };
                    dataGridView.Columns[nameof(Results.DistanceSum)].DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.TopRight,
                        Format = AppSettings.Default.FormatDistance2
                    };

                    break;
                case ChildFormType.Tracks:
                    dataGridView.Columns[nameof(Track.DateTimeStart)].DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        Format = AppSettings.Default.FormatDateTime
                    };
                    dataGridView.Columns[nameof(Track.DateTimeFinish)].DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        Format = AppSettings.Default.FormatDateTime
                    };
                    dataGridView.Columns[nameof(Track.Duration)].DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.TopRight,
                        Format = "hh\\:mm"
                    };
                    dataGridView.Columns[nameof(Track.Distance)].DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.TopRight,
                        Format = AppSettings.Default.FormatDistance
                    };
                    dataGridView.Columns[nameof(Track.NewTilesCount)].DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.TopRight
                    };

                    break;
                case ChildFormType.Markers:
                    dataGridView.Columns[nameof(Marker.Lat)].DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.TopRight,
                        Format = AppSettings.Default.FormatLatLng
                    };
                    dataGridView.Columns[nameof(Marker.Lng)].DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.TopRight,
                        Format = AppSettings.Default.FormatLatLng
                    };

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
                switch (ListType)
                {
                    case ChildFormType.Results:
                        errorMsg = Resources.MsgDatabaseLoadListResultsFail;

                        bindingSource.DataSource = await ListLoadAsync<Results>();

                        break;
                    case ChildFormType.Tracks:
                        errorMsg = Resources.MsgDatabaseLoadListTrackFail;

                        bindingSource.DataSource = await ListLoadAsync<Track>();

                        break;
                    case ChildFormType.Markers:
                        errorMsg = Resources.MsgDatabaseLoadListMarkersFail;

                        bindingSource.DataSource = await ListLoadAsync<Marker>();

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
            MainForm.SelectMapItem(this, Selected);
        }

        private void DataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            MainForm.ChangeMapItem(this, Selected);
        }
    }
}