using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public partial class FrmTracksTree : Form, IChildForm, IUpdateDataForm
    {
        public IMainForm MainForm => Owner as IMainForm;

        public ChildFormType FormType => ChildFormType.TracksTree;

        internal readonly PresenterChildForm childFormPresenter;

        public FrmTracksTree()
        {
            InitializeComponent();

            childFormPresenter = new PresenterChildForm(this);
        }

        public static FrmTracksTree ShowFrm(Form owner)
        {
            var frm = new FrmTracksTree()
            {
                Owner = owner,
                Text = Resources.TitleTracksTree,
            };

            frm.Show(owner);

            return frm;
        }

        private void FrmTracksTree_Load(object sender, EventArgs e)
        {
            AppSettings.LoadFormState(this, AppSettings.Local.Default.FormStateTracksTree);

            UpdateSettings();

            if (MainForm.ProgramStatus.Current != Status.Starting)
            {
                UpdateData();
            }
        }

        private void FrmTracksTree_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppSettings.Local.Default.FormStateTracksTree = AppSettings.SaveFormState(this);

            AppSettings.LocalSave();
        }

        public void UpdateSettings()
        {
        }

        private readonly WrapperCancellationTokenSource ctsTracsTree = new WrapperCancellationTokenSource();

        private async Task UpdateDataAsync()
        {
            var status = MainForm.ProgramStatus.Start(Status.LoadData);

            try
            {
                var list = await Database.Actions.ListLoadAsync<TracksTree>();

                if (list != null)
                {
                    CreateTracksTree(list);
                }
            }
            finally
            {
                ctsTracsTree.Finally();

                MainForm.ProgramStatus.Stop(status);
            }
        }

        public void UpdateData()
        {
            ctsTracsTree.Start();

            Task.Run(() => this.InvokeIfNeeded(async () => await UpdateDataAsync()), ctsTracsTree.Token);
        }

        private string MonthToString(int month)
        {
            var date = new DateTime(1981, month, 1);

            return date.ToString("MMMM");
        }

        private void CreateTracksTree(List<TracksTree> list)
        {
            list.ForEach(t => { DebugWrite.Line($"year: {t.Year}, month: {t.Month}"); });

            treeView.Nodes.Clear();

            var root = new TreeNode()
            {
                Text = Resources.TextTracksTreeAll
            };

            treeView.Nodes.Add(root);

            var year = 0;

            TreeNode treeNodeYear = null;

            foreach (var item in list)
            {
                if (year != item.Year)
                {
                    year = item.Year;

                    treeNodeYear = new TreeNode()
                    {
                        Text = $"{item.Year}",
                        Tag = item.Year,
                    };

                    root.Nodes.Add(treeNodeYear);
                }

                treeNodeYear.Nodes.Add(new TreeNode()
                {
                    Text = MonthToString(item.Month),
                    Tag = item.Month,
                });
            }

            treeView.ExpandAll();

            treeView.SelectedNode = root;
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            AfterSelect(e.Node);
        }

        private void AfterSelect(TreeNode node)
        {
            if (node == null) return;

            if (node.Parent == null)
            {
                DebugWrite.Line("all records");

                Database.Filter.Default.Type = Database.Filter.FilterType.None;
            }
            else
            {
                if (node.Parent.Tag == null)
                {
                    DebugWrite.Line($"year: {node.Tag}");

                    Database.Filter.Default.Years = new int[] { (int)node.Tag };

                    Database.Filter.Default.Type = Database.Filter.FilterType.Years;
                }
                else
                {
                    DebugWrite.Line($"year: {node.Parent.Tag}, month: {node.Tag}");

                    Database.Filter.Default.SetDates(
                        new DateTime((int)node.Parent.Tag, (int)node.Tag, 1),
                        new DateTime((int)node.Parent.Tag, (int)node.Tag,
                            DateTime.DaysInMonth((int)node.Parent.Tag, (int)node.Tag)));

                    Database.Filter.Default.Type = Database.Filter.FilterType.Period;
                }
            }
        }

        private void FrmTracksTree_FormClosed(object sender, FormClosedEventArgs e)
        {
            ctsTracsTree.Cancel();
        }
    }
}