using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public partial class FrmTracksTree : Form, IChildForm, IUpdateDataForm
    {
        public IMainForm MainForm => Owner as IMainForm;

        public ChildFormType ChildFormType => ChildFormType.TracksTree;

        private bool selfChange = false;

        public FrmTracksTree()
        {
            InitializeComponent();
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

        private void FrmTracksTree_Load(object sender, System.EventArgs e)
        {
            MainForm.ChildFormOpened(this);

            AppSettings.LoadFormState(this, AppSettings.Default.FormStateTracksTree);

            UpdateSettings();

            selfChange = true;

            if (MainForm.ProgramStatus.Current != Status.Starting)
            {
                UpdateData();
            }

            selfChange = false;
        }

        private void FrmTracksTree_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppSettings.Default.FormStateTracksTree = AppSettings.SaveFormState(this);

            AppSettings.Default.Save();
        }

        private void FrmTracksTree_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm.ChildFormClosed(this);
        }

        public void UpdateSettings()
        {

        }

        private async Task UpdateDataAsync()
        {
            var status = MainForm.ProgramStatus.Start(Status.LoadData);

            try
            {
                var list = await Database.Default.ListLoadAsync<TracksTree>();

                CreateTracksTree(list);
            }
            catch (Exception e)
            {
                Utils.WriteError(e);

                Msg.Error(Resources.MsgDatabaseLoadTracksTreeFail, e.Message);
            }
            finally
            {
                MainForm.ProgramStatus.Stop(status);
            }
        }

        public async void UpdateData()
        {
            await Task.Run(() =>
            {
                this.InvokeIfNeeded(() => _ = UpdateDataAsync());
            });
        }

        private string MonthTostring(int month)
        {
            var date = new DateTime(1981, month, 1);

            return date.ToString("MMMM");
        }

        private void CreateTracksTree(List<TracksTree> list)
        {
            list.ForEach(t => { Utils.WriteDebug($"year: {t.Year}, month: {t.Month}"); });

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
                    Text = MonthTostring(item.Month),
                    Tag = item.Month,
                });
            }

            treeView.ExpandAll();

            selfChange = true;

            treeView.SelectedNode = root;

            selfChange = false;
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
                Utils.WriteDebug("all records");

                Filter.Default.Type = Filter.FilterType.None;
            }
            else
            {
                if (node.Parent.Tag == null)
                {
                    Utils.WriteDebug($"year: {node.Tag}");

                    Filter.Default.Years = new int[] { (int)node.Tag };

                    Filter.Default.Type = Filter.FilterType.Years;
                }
                else
                {
                    Utils.WriteDebug($"year: {node.Parent.Tag}, month: {node.Tag}");

                    Filter.Default.SetDates(
                        new DateTime((int)node.Parent.Tag, (int)node.Tag, 1),
                        new DateTime((int)node.Parent.Tag, (int)node.Tag,
                            DateTime.DaysInMonth((int)node.Parent.Tag, (int)node.Tag)));

                    Filter.Default.Type = Filter.FilterType.Period;
                }
            }
        }
    }
}