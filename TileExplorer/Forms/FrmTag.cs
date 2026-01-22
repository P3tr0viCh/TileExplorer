using P3tr0viCh.Utils.Extensions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Interfaces;
using static TileExplorer.ProgramStatus;

namespace TileExplorer
{
    public partial class FrmTag : Form
    {
        public IMainForm MainForm => Owner as IMainForm;

        private readonly TagModel tagModel = new TagModel();

        private TagModel TagModel
        {
            get => tagModel;
            set
            {
                tagModel.Assign(value);

                tbText.SetText(tagModel.Text);
            }
        }

        public FrmTag()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(Form owner, TagModel tag)
        {
            using (var frm = new FrmTag()
            {
                Owner = owner,

                TagModel = tag
            })
            {
                var result = frm.ShowDialog(owner);

                if (result == DialogResult.OK)
                {
                    tag.Assign(frm.TagModel);
                }

                return result == DialogResult.OK;
            }
        }

        private bool CheckData()
        {
            if (Utils.Forms.TextBoxIsEmpty(tbText, Resources.ErrorNeedText))
            {
                return false;
            }

            return true;
        }

        private bool UpdateData()
        {
            tagModel.Text = tbText.GetTrimText();

            return true;
        }

        private async Task<bool> SaveDataAsync()
        {
            var result = await Database.Actions.TagSaveAsync(TagModel);

            if (result)
            {
                await MainForm.TagChangedAsync(TagModel);
            }

            return result;
        }

        private async Task<bool> ApplyDataAsync()
        {
            return CheckData() && UpdateData() && await SaveDataAsync();
        }

        private async void BtnOk_Click(object sender, System.EventArgs e)
        {
            if (await ApplyDataAsync())
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}