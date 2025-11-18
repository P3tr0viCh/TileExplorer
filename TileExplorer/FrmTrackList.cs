using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public partial class FrmTrackList : Form
    {
        public IMainForm MainForm => Owner as IMainForm;

        private List<Track> Tracks { get; set; }

        public FrmTrackList()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(Form owner, List<Track> tracks)
        {
            using (var frm = new FrmTrackList()
            {
                Owner = owner,
            })
            {
                frm.Tracks = tracks;

                return frm.ShowDialog(owner) == DialogResult.OK;
            }
        }

        private async void FrmTrackList_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private async Task<bool> LoadDataAsync()
        {
            DebugWrite.Line("start");

            try
            {
                equipmentBindingSource.DataSource = await Database.Default.ListLoadAsync<Equipment>();

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
            Tracks.ForEach(track => track.Equipment = cboxEquipment.SelectedItem as Equipment);

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