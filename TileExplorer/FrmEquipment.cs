using P3tr0viCh.Utils;
using System;
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
            get
            {
                return equipment;
            }
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
                return frm.ShowDialog(owner) == DialogResult.OK;
            }
        }
        
        private bool CheckData()
        {
            tbText.Text = tbText.Text.Trim();
            tbBrand.Text = tbBrand.Text.Trim();
            tbModel.Text = tbModel.Text.Trim();

            if (tbText.Text.IsEmpty() &&
                tbBrand.Text.IsEmpty() &&
                tbModel.Text.IsEmpty())
            {
                tbText.Focus();

                Msg.Error(Resources.ErrorNeedText);

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

        private bool SaveData()
        {
            _ = Database.Default.EquipmentSaveAsync(Equipment);

            return true;
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
    }
}