using P3tr0viCh.Utils;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public partial class FrmTileInfo : Form, IChildForm, IUpdateDataForm
    {
        public IMainForm MainForm => Owner as IMainForm;

        public ChildFormType ChildFormType => ChildFormType.TileInfo;

        private readonly CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

        private Tile Tile { get; set; } = null;

        public FrmTileInfo()
        {
            InitializeComponent();
        }

        public static FrmTileInfo ShowFrm(Form owner, Tile tile)
        {
            var frm = new FrmTileInfo()
            {
                Owner = owner,
                Text = string.Format(Resources.StatusTileId, tile.X, tile.Y),
                Tile = tile
            };

            frm.Location = new Point(owner.Location.X + owner.Width / 2 - frm.Width / 2,
                                     owner.Location.Y + owner.Height / 2 - frm.Height / 2);

            frm.Show(owner);

            return frm;
        }

        private void FrmTileInfo_Load(object sender, EventArgs e)
        {
            UpdateSettings();

            UpdateData();
        }

        private void FrmTileInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        public void UpdateSettings()
        {
            ColumnDateTimeStart.DefaultCellStyle = DataGridViewCellStyles.Date;

            dataGridView.Refresh();
        }

        public async Task UpdateDataAsync()
        {
            var status = MainForm.ProgramStatus.Start(Status.LoadData);

            try
            {
                Tile.Tracks = await Database.Default.ListLoadAsync<Track>(Tile);

                slCount.Text = string.Format(Resources.StatusTracksCount, Tile.Tracks.Count);

                trackBindingSource.DataSource = Tile.Tracks;
            }
            catch (TaskCanceledException e)
            {
                DebugWrite.Error(e);
            }
            catch (Exception e)
            {
                slCount.Text = string.Empty;

                DebugWrite.Error(e);

                Msg.Error(Resources.MsgDatabaseLoadTileInfoFail, e.Message);
            }
            finally
            {
                MainForm.ProgramStatus.Stop(status);
            }
        }

        public void UpdateData()
        {
            Task.Run(() =>
            {
                this.InvokeIfNeeded(async () => await UpdateDataAsync());
            }, cancelTokenSource.Token);
        }

        private void FrmTileInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            cancelTokenSource.Cancel();
        }
    }
}