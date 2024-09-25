using Newtonsoft.Json.Linq;
using P3tr0viCh.Utils;
using System;
using System.Reflection;
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
                Task.Run(async () => { await LoadDataAsync(); }).GetAwaiter().GetResult();

                return true;
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(Resources.MsgDatabaseLoadListEquipmentsFail, e.Message);

                return false;
            }
        }

        private async Task LoadDataAsync()
        {
            equipmentBindingSource.DataSource = await Database.Default.ListLoadAsync<Equipment>();

            equipmentBindingSource.Insert(0, new Equipment());
        }

        private bool CheckData()
        {
            if (!tbEleAscent.Text.IsEmpty() && !Misc.FloatCheck(tbEleAscent.Text))
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
                DebugWrite.Error(e);

                Msg.Error(e.Message);

                return false;
            }
        }

        private async Task<bool> SaveDataAsync()
        {
            try
            {
                await Database.Default.TrackSaveAsync(Track);

                await MainForm.TrackChangedAsync(Track);

                return true;
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(e.Message);

                return false;
            }
        }

        private async Task<bool> ApplyDataAsync()
        {
            return CheckData() && UpdateData() && await SaveDataAsync();
        }

        private async void BtnOk_Click(object sender, EventArgs e)
        {
            if (await ApplyDataAsync())
            {
                DialogResult = DialogResult.OK;
            }
        }

        private async void BtnOKToALL_Click(object sender, EventArgs e)
        {
            if (!Msg.Question(Resources.QuestionTracksOKToAll))
            {
                return;
            }

            if (await ApplyDataAsync())
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