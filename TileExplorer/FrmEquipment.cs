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

                tbText.Text = equipment.Text;
                tbBrand.Text = equipment.Brand;
                tbModel.Text = equipment.Model;
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
            tbText.Text = tbText.Text.Trim();
            tbBrand.Text = tbBrand.Text.Trim();
            tbModel.Text = tbModel.Text.Trim();

            if (Utils.Forms.TextBoxIsEmpty(tbText, Resources.ErrorNeedText))
            {
                return false;
            }

            return true;
        }

        private bool UpdateData()
        {
            equipment.Text = tbText.Text;
            equipment.Brand = tbBrand.Text;
            equipment.Model = tbModel.Text;

            return true;
        }

        private async Task<bool> SaveDataAsync()
        {
            var result = false;

            var status = MainForm.ProgramStatus.Start(Status.SaveData);

            try
            {
                result = await Database.Actions.EquipmentSaveAsync(Equipment);
            }
            finally
            {
                MainForm.ProgramStatus.Stop(status);
            }

            return result;
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