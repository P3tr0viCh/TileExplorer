using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Interfaces;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
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
            AppSettings.Local.LoadFormState(this, AppSettings.Local.Default.FormStates);

            await LoadDataAsync();

            Tracks = tracks;

            var equipmentIds = tracks.Select(track => track.EquipmentId).Distinct();

            if (equipmentIds.Count() == 1)
            {
                cboxEquipment.SelectedValue = equipmentIds.First();
            }
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

        private void FrmTrackList_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppSettings.Local.SaveFormState(this, AppSettings.Local.Default.FormStates);

            AppSettings.LocalSave();
        }
    }
}