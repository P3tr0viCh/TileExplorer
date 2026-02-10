using P3tr0viCh.Utils.Extensions;
using System;
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
            equipment.Brand = tbBrand.GetTrimTextNullable();
            equipment.Model = tbModel.GetTrimTextNullable();

            return true;
        }

        private bool ApplyData()
        {
            return CheckData() && UpdateData();
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