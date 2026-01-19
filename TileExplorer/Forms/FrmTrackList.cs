using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Interfaces;
using static TileExplorer.ProgramStatus;

namespace TileExplorer
{
    public partial class FrmTrackList : Form
    {
        public IMainForm MainForm => Owner as IMainForm;

        private IEnumerable<Track> Tracks { get; set; }

        public FrmTrackList()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(Form owner, IEnumerable<Track> tracks)
        {
            using (var frm = new FrmTrackList()
            {
                Owner = owner,
            })
            {
                frm.Load += (sender, args) => frm.FrmTrackList_Load(tracks);

                return frm.ShowDialog(owner) == DialogResult.OK;
            }
        }

        private async void FrmTrackList_Load(IEnumerable<Track> tracks)
        {
            await LoadDataAsync();

            Tracks = tracks;
        }

        private async Task<bool> LoadDataAsync()
        {
            DebugWrite.Line("start");

            try
            {
                var status = ProgramStatus.Default.Start(Status.LoadData);

                try
                {
                    equipmentBindingSource.DataSource = await Database.Default.ListLoadAsync<Equipment>();
                }
                finally
                {
                    ProgramStatus.Default.Stop(status);
                }

                equipmentBindingSource.Insert(0, new Equipment());

                cboxEquipment.SelectedIndex = 0;

                return true;
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(Resources.MsgDatabaseLoadListEquipmentsFail, e.Message);

                return false;
            }
        }

        private bool UpdateData()
        {
            foreach (var track in Tracks)
            {
                track.Equipment = cboxEquipment.GetSelectedItem<Equipment>();
            }

            return true;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (UpdateData())
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}