using P3tr0viCh.Utils;
using System;
using System.IO;
using System.Windows.Forms;
using TileExplorer.Properties;

namespace TileExplorer
{
    public partial class FrmSettings : Form
    {
        public FrmSettings()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(IWin32Window owner)
        {
            bool Result;

            using (var frm = new FrmSettings())
            {
                Result = frm.ShowDialog(owner) == DialogResult.OK;

                if (Result)
                {
                    AppSettings.Default.Save();
                }
            }

            return Result;
        }

        private void FrmSettings_Load(object sender, EventArgs e)
        {
            AppSettings.LoadFormState(this, AppSettings.Default.FormStateSettings);

            propertyGrid.SelectedObject = AppSettings.Default;
        }

        private void FrmSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            AppSettings.Default.FormStateSettings = AppSettings.SaveFormState(this);
        }

        private bool CheckDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) return true;

            if (Directory.Exists(path)) return true;

            Msg.Error(string.Format(Resources.ErrorDirectoryNotExists, path));

            return false;
        }

        private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.PropertyDescriptor.PropertyType == AppSettings.Default.DatabaseHome.GetType())
            {
                CheckDirectory((string)e.ChangedItem.Value);
                return;
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (CheckDirectory(AppSettings.Default.DatabaseHome))
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}