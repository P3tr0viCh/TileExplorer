using P3tr0viCh.Utils;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public partial class FrmTrack : Form
    {
        public IMainForm MainForm => Owner as IMainForm;

        public FrmTrack()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(Form owner, Track track)
        {
            using (var frm = new FrmTrack()
            {
                Owner = owner,
            })
            {
                if (!frm.LoadData()) return false;

                frm.tbText.Text = track.Text;

                frm.cboxEquipment.SelectedValue = track.EquipmentId;

                if (frm.ShowDialog(owner) != DialogResult.OK) return false;

                track.Text = frm.tbText.Text;

                track.Equipment = frm.cboxEquipment.SelectedItem as Equipment;

                return true;
            }
        }

        private bool LoadData()
        {
            var status = MainForm.ProgramStatus.Start(Status.LoadData);

            Utils.WriteDebug("start");

            try
            {
                return Task.Run(() => LoadDataAsync()).Result;
            }
            finally
            {
                Utils.WriteDebug("end");

                MainForm.ProgramStatus.Stop(status);
            }
        }

        private async Task<bool> LoadDataAsync()
        {
            try
            {
                equipmentBindingSource.DataSource =
                    await Database.Default.ListLoadAsync<Equipment>();
                
                Utils.ComboBoxInsertItem(equipmentBindingSource, 0, new Equipment());

                return true;
            }
            catch (Exception e)
            {
                Utils.WriteError(e);

                Msg.Error(Resources.MsgDatabaseLoadListEquipmentsFail, e.Message);

                return false;
            }
        }
    }
}