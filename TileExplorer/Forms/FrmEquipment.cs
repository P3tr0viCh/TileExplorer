using P3tr0viCh.Utils.Extensions;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Interfaces;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public partial class FrmEquipment : Form
    {
        public IMainForm MainForm => Owner as IMainForm;

        private readonly Equipment equipment = new Equipment();

        private Equipment Equipment
        {
            get => equipment;
            set
            {
                equipment.Assign(value);

                tbText.SetText(equipment.Text);
                tbBrand.SetText(equipment.Brand);
                tbModel.SetText(equipment.Model);
            }
        }

        public FrmEquipment()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(Form owner, Equipment equipment)
        {
            using (var frm = new FrmEquipment()
            {
                Owner = owner,

                Equipment = equipment
            })
            {
                var result = frm.ShowDialog(owner);

                if (result == DialogResult.OK)
                {
                    equipment.Assign(frm.Equipment);
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
            equipment.Text = tbText.GetTrimText();
            equipment.Brand = tbBrand.GetTrimText();
            equipment.Model = tbModel.GetTrimText();

            return true;
        }

        private async Task<bool> SaveDataAsync()
        {
            return await Database.Actions.EquipmentSaveAsync(Equipment);
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
    }
}