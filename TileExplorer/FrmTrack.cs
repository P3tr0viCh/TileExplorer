using P3tr0viCh.Utils;
using System;
using System.Diagnostics;
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

        private readonly Track track = new Track();
        private Track Track
        {
            get
            {
                return track;
            }
            set
            {
                track.Assign(value);

                tbText.Text = track.Text;

                cboxEquipment.SelectedValue = track.EquipmentId;

                tbEleAscent.Text = track.EleAscent == 0 ? string.Empty : track.EleAscent.ToString("0.#");
            }
        }

        public FrmTrack()
        {
            InitializeComponent();
        }

        public static DialogResult ShowDlg(Form owner, Track track, bool canToAll)
        {
            using (var frm = new FrmTrack()
            {
                Owner = owner,
            })
            {
                if (!frm.LoadData()) return DialogResult.None;

                frm.Track = track;

                frm.btnOKToALL.Visible = canToAll;

                var result = frm.ShowDialog(owner);

                if (result == DialogResult.OK || result == DialogResult.Yes)
                {
                    track.Assign(frm.Track);
                }

                return result;
            }
        }

        public static bool ShowDlg(Form owner, Track track)
        {
            return ShowDlg(owner, track, false) == DialogResult.OK;
        }

        private bool LoadData()
        {
            var status = MainForm.ProgramStatus.Start(Status.LoadData);

            DebugWrite.Line("start");

            try
            {
                return Task.Run(() => LoadDataAsync()).Result;
            }
            finally
            {
                DebugWrite.Line("end");

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
                DebugWrite.Error(e);

                Msg.Error(Resources.MsgDatabaseLoadListEquipmentsFail, e.Message);

                return false;
            }
        }

        private bool CheckData()
        {
            if (!string.IsNullOrWhiteSpace(tbEleAscent.Text) && !Misc.FloatCheck(tbEleAscent.Text))
            {
                tbEleAscent.Focus();
                tbEleAscent.SelectAll();

                Msg.Error(Resources.ErrorNeedDigit);

                return false;
            }

            return true;
        }

        private bool UpdateData()
        {
            try
            {
                track.Text = tbText.Text;

                track.Equipment = cboxEquipment.SelectedItem as Equipment;

                track.EleAscent = Misc.FloatParse(tbEleAscent.Text);

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);

                Msg.Error(e.Message);

                return false;
            }
        }

        private bool SaveData()
        {
            try
            {
                Task.Run(() => Database.Default.SaveTrackAsync(Track)).Wait();

                MainForm.TrackChanged(Track);

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);

                var msg = e.InnerException != null ? e.InnerException.Message : e.Message;

                Msg.Error(msg);

                return false;
            }
        }

        private bool ApplyData()
        {
            return CheckData() && UpdateData() && SaveData();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (ApplyData())
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void BtnOKToALL_Click(object sender, EventArgs e)
        {
            if (!Msg.Question(Resources.QuestionTracksOKToAll))
            {
                return;
            }

            if (ApplyData())
            {
                DialogResult = DialogResult.Yes;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (btnOKToALL.Visible && ModifierKeys == Keys.Shift)
            {
                if (!Msg.Question(Resources.QuestionTracksCancelToAll))
                {
                    DialogResult = DialogResult.None;

                    return;
                }

                DialogResult = DialogResult.Abort;

                return;
            }

            DialogResult = DialogResult.Cancel;
        }
    }
}