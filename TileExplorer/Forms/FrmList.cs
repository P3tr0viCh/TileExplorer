#if DEBUG
//#define SHOW_ALL_COLUMNS
#endif

using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Interfaces;
using TileExplorer.Presenters;
using static TileExplorer.Database.Models;
using static TileExplorer.Presenters.PresenterStatusStripList;

namespace TileExplorer
{
    public partial class FrmList : Form, IFrmListBase, PresenterStatusStrip<StatusLabel>.IPresenterStatusStrip
    {
        public IMainForm MainForm => Owner as IMainForm;

        public ChildFormType FormType => PresenterFrmList.FormType;

        public DataGridView DataGridView => dataGridView;

        public ToolStrip ToolStrip => toolStrip;

        public StatusStrip StatusStrip => statusStrip;

        private IPresenterFrmListBase PresenterFrmList { get; set; }

        private readonly PresenterStatusStripList statusStripPresenter;

        public object Value => PresenterFrmList.Value;

        public FrmList()
        {
            InitializeComponent();

            PresenterChildForm.LinkTo(this);

            statusStripPresenter = new PresenterStatusStripList(this);
        }

        public static FrmList ShowFrm(Form owner, ChildFormType childFormType, object value = default)
        {
            var frm = new FrmList()
            {
                Owner = owner
            };

            DebugWrite.Line($"ChildFormType = {childFormType}");

            var listType = FrmListType.None;

            switch (childFormType)
            {
                case ChildFormType.TagList:
                    listType = FrmListType.TagList;
                    break;
                case ChildFormType.TrackList:
                    listType = FrmListType.TrackList;
                    break;
                case ChildFormType.MarkerList:
                    listType = FrmListType.MarkerList;
                    break;
                case ChildFormType.EquipmentList:
                    listType = FrmListType.EquipmentList;
                    break;
                case ChildFormType.TileInfo:
                    listType = FrmListType.TileInfo;
                    break;
                case ChildFormType.ResultYears:
                    listType = FrmListType.ResultYears;
                    break;
                case ChildFormType.ResultEquipments:
                    listType = FrmListType.ResultEquipments;
                    break;
            }

            DebugWrite.Line($"ListType = {listType}");

            frm.PresenterFrmList = PresenterFrmListFactory.PresenterFrmListInstance(frm, listType);

            frm.PresenterFrmList.Value = value;

            frm.PresenterFrmList.FrmListChanged += frm.PresenterFrmList_FrmListChanged;

            frm.Show(owner);

            return frm;
        }

        private void PresenterFrmList_FrmListChanged(object sender)
        {
            tsbtnChange.Enabled = tsbtnDelete.Enabled = tsbtnChartTrackEle.Enabled = !DataGridView.IsEmpty();

            statusStripPresenter.Count = DataGridView.Count();
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            statusStripPresenter.SelectedCount = DataGridView.SelectedCount();
        }

        private void FrmListNew_Load(object sender, EventArgs e)
        {
            switch (FormType)
            {
                case ChildFormType.TrackList:
                    toolStripSeparator1.Visible = true;
                    tsbtnChartTrackEle.Visible = true;

                    return;
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
            set => dataGridView.SetSelected(value);
        }

        public IEnumerable<BaseId> SelectedList
        {
            get => dataGridView.GetSelectedList<BaseId>();
            set => dataGridView.SetSelectedList(value);
        }

        public int Count => dataGridView.Count();

        public void SetSelected(BaseId value) => Selected = value;

        public void SetSelected(IEnumerable<BaseId> values) => SelectedList = values;

        private async void TsbtnAdd_Click(object sender, EventArgs e)
        {
            await PresenterFrmList.ListItemAddNewAsync();
        }

        private async void TsbtnChange_Click(object sender, EventArgs e)
        {
            await PresenterFrmList.ListItemChangeSelectedAsync();
        }

        private async void TsbtnDelete_Click(object sender, EventArgs e)
        {
            await PresenterFrmList.ListItemDeleteSelectedAsync();
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

        public void ListItemChange(IBaseId value) => PresenterFrmList.ListItemChange(value);

        public void ListItemDelete(IBaseId value) => PresenterFrmList.ListItemDelete(value);
    }
}