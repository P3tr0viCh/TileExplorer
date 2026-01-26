using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Interfaces;
using static TileExplorer.ProgramStatus;

namespace TileExplorer
{
    public partial class FrmTrack : Form
    {
        public IMainForm MainForm => Owner as IMainForm;

        private readonly Track track = new Track();
        private Track Track
        {
            get => track;
            set
            {
                track.Assign(value);

                tbText.SetText(track.Text);

                cboxEquipment.SelectedValue = track.EquipmentId;

                tbEleAscent.SetDouble(track.EleAscent, AppSettings.Roaming.Default.FormatEleAscent);
                tbEleDescent.SetDouble(track.EleDescent, AppSettings.Roaming.Default.FormatEleAscent);

                CheckedListBoxSetChecked(clbTags, track.Tags);
            }
        }

        private bool saveOnClose;

        public FrmTrack()
        {
            InitializeComponent();
        }

        public static DialogResult ShowDlg(Form owner, Track track, bool canToAll, bool saveOnClose)
        {
            using (var frm = new FrmTrack()
            {
                Owner = owner,
            })
            {
                frm.btnOKToALL.Visible = canToAll;
                frm.btnCancelToALL.Visible = canToAll;

                frm.saveOnClose = saveOnClose;

                frm.Load += (sender, args) => frm.FrmTrack_Load(track);

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
            return ShowDlg(owner, track, false, true) == DialogResult.OK;
        }

        private async void FrmTrack_Load(Track track)
        {
            AppSettings.Local.LoadFormState(this, AppSettings.Local.Default.FormStates);

            await LoadDataAsync();

            Track = track;
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

                    await LoadDataTags();
                }
                finally
                {
                    ProgramStatus.Default.Stop(status);
                }

                equipmentBindingSource.Insert(0, new Equipment());

                return true;
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(Resources.MsgDatabaseLoadListEquipmentsFail, e.Message);

                return false;
            }
        }

        private class TagItem: TagModel
        {
            public TagItem(TagModel tag)
            {
                Assign(tag);
            }

            public override string ToString() => Text;
        }

        private async Task LoadDataTags()
        {
            clbTags.Items.Clear();

            var tags = await Database.Default.ListLoadAsync<TagModel>();

            foreach (var tag in tags)
            {
                clbTags.Items.Add(new TagItem(tag));
            }
        }

        private long CheckedListBoxGetItemId(CheckedListBox checkedListBox, int index)
        {
            if (checkedListBox.Items.Count == 0) return -1;

            var item = checkedListBox.Items[index] as TagItem;

            return item.Id;
        }

        private bool EnumerableContainsId<T>(IEnumerable<T> items, long id) where T : IBaseId
        {
            foreach (var item in items)
            {
                if (item.Id == id) return true;
            }

            return false;
        }

        private void CheckedListBoxSetChecked<T>(CheckedListBox checkedListBox, IEnumerable<T> items) where T : IBaseId
        {
            if (items is null) return;

            for (var i = 0; i < clbTags.Items.Count; i++)
            {
                var id = CheckedListBoxGetItemId(checkedListBox, i);

                var exists = EnumerableContainsId(items, id);

                clbTags.SetItemChecked(i, exists);
            }
        }

        private IEnumerable<T> CheckedListBoxGetChecked<T>(CheckedListBox checkedListBox) where T : IBaseId
        {
            var checkedItems = clbTags.CheckedItems;

            return checkedItems.Cast<T>();  
        }

        private bool CheckData()
        {
            return Utils.Forms.TextBoxIsWrongFloat(tbEleAscent) && Utils.Forms.TextBoxIsWrongFloat(tbEleDescent);
        }

        private bool UpdateData()
        {
            try
            {
                track.Text = tbText.GetTrimText();

                track.Equipment = cboxEquipment.GetSelectedItem<Equipment>();

                track.EleAscent = tbEleAscent.GetDouble();
                track.EleDescent = tbEleDescent.GetDouble();

                track.Tags = CheckedListBoxGetChecked<TagModel>(clbTags);

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
            if (!CheckData() || !UpdateData())
            {
                return false;
            }

            if (saveOnClose)
            {
                return await SaveDataAsync();
            }
            else
            {
                return true;
            }
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
            if (!Msg.Question(Resources.QuestionTrackListOKToAll))
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
            DialogResult = DialogResult.Cancel;
        }

        private void BtnCancelToALL_Click(object sender, EventArgs e)
        {
            if (!Msg.Question(Resources.QuestionTracksCancelToAll))
            {
                DialogResult = DialogResult.None;

                return;
            }

            DialogResult = DialogResult.Abort;
        }

        private void FrmTrack_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppSettings.Local.SaveFormState(this, AppSettings.Local.Default.FormStates);

            AppSettings.LocalSave();
        }
    }
}