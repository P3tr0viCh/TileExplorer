#if DEBUG
//#define SHOW_ALL_COLUMNS
#endif

using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Interfaces;
using P3tr0viCh.Utils.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TileExplorer.Interfaces;
using TileExplorer.Presenters;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Presenters.PresenterStatusStripList;

namespace TileExplorer
{
    public partial class FrmList : Form, IChildFormList, IChildForm,
        IUpdateSettings, IUpdateData, IUpdateDataList,
        PresenterStatusStrip<StatusLabel>.IPresenterStatusStrip
    {
        public IMainForm MainForm => Owner as IMainForm;

        public ChildFormType FormType => PresenterFrmList.FormType;

        public object Value => PresenterFrmList.Value;

        public DataGridView DataGridView => dataGridView;

        public ToolStrip ToolStrip => toolStrip;

        public StatusStrip StatusStrip => statusStrip;

        private IPresenterFrmList PresenterFrmList { get; set; }

        private readonly PresenterStatusStripList statusStripPresenter;

        public FrmList()
        {
            InitializeComponent();

            PresenterChildForm.LinkTo(this);

            statusStripPresenter = new PresenterStatusStripList(this);
        }

        public static FrmList ShowFrm(Form owner, ChildFormType frmType, object value = default)
        {
            var frm = new FrmList()
            {
                Owner = owner
            };

            DebugWrite.Line($"ChildFormType = {frmType}");

            frm.PresenterFrmList = PresenterFrmListFactory.PresenterFrmListInstance(frm, frmType);

            frm.PresenterFrmList.Value = value;

            frm.PresenterFrmList.ListChanged += frm.PresenterFrmList_ListChanged;

            frm.Show(owner);

            return frm;
        }

        private void PresenterFrmList_ListChanged(object sender, EventArgs e)
        {
            tsbtnChange.Enabled = tsbtnDelete.Enabled = tsbtnChartTrackEle.Enabled = !DataGridView.IsEmpty();

            statusStripPresenter.Count = DataGridView.Count();
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            statusStripPresenter.SelectedCount = DataGridView.SelectedCount();
        }

        private void FrmList_Load(object sender, EventArgs e)
        {
            lblLoading.Text = Resources.TextLoading;

            LabelLoadingUpdatePosition();

            switch (FormType)
            {
                case ChildFormType.TrackList:
                    toolStripSeparator1.Visible = true;
                    tsbtnChartTrackEle.Visible = true;

                    break;
            }
        }

        ToolStripStatusLabel PresenterStatusStrip<StatusLabel>.IPresenterStatusStrip.GetLabel(StatusLabel label)
        {
            switch (label)
            {
                case StatusLabel.Count: return slCount;
                case StatusLabel.SelectedCount: return slSelectedCount;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public void UpdateSettings() => PresenterFrmList.UpdateSettings();

        public async Task UpdateDataAsync() => await PresenterFrmList.UpdateDataAsync();

        public BaseId Selected
        {
            get => dataGridView.GetSelected<BaseId>();
            set
            {
                if (value?.Id == Selected?.Id) return;

                dataGridView.SetSelected(value);
            }
        }

        public IEnumerable<BaseId> SelectedList
        {
            get => dataGridView.GetSelectedList<BaseId>();
            set => dataGridView.SetSelectedList(value);
        }
        
        public bool Loading
        {
            set => lblLoading.Visible = value;
        }

        public void SetSelected(BaseId value) => Selected = value;

        public void SetSelected(IEnumerable<BaseId> values) => SelectedList = values;

        private async void TsbtnAdd_Click(object sender, EventArgs e)
        {
            await PresenterFrmList.AddNewItemAsync();
        }

        private async void TsbtnChange_Click(object sender, EventArgs e)
        {
            await PresenterFrmList.SelectedChangeAsync();
        }

        private async void TsbtnDelete_Click(object sender, EventArgs e)
        {
            await PresenterFrmList.SelectedDeleteAsync();
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

        private void TsbtnTrackEleChart_Click(object sender, EventArgs e)
        {
            ShowTrackEleChart();
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

        public void ListItemsChange(IEnumerable<IBaseId> list) => PresenterFrmList.ListItemsChange(list);

        public void ListItemsDelete(IEnumerable<IBaseId> list) => PresenterFrmList.ListItemsDelete(list);

        private void LabelLoadingUpdatePosition()
        {
            lblLoading.SetBounds((dataGridView.Width - lblLoading.Width) / 2, (dataGridView.Height - lblLoading.Height) / 2,
                lblLoading.Width, lblLoading.Height);
        }

        private void FrmList_Resize(object sender, EventArgs e)
        {
            LabelLoadingUpdatePosition();
        }
    }
}