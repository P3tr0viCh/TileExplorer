using P3tr0viCh.Utils;
using System;
using System.IO;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

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
            using (var frm = new FrmSettings())
            {
                AppSettings.Save();

                AppSettings.LoadFormState(frm, AppSettings.Default.FormStateSettings);

                frm.propertyGrid.SelectedObject = AppSettings.Default;

                if (frm.ShowDialog(owner) == DialogResult.OK)
                {
                    AppSettings.Default.FormStateSettings = AppSettings.SaveFormState(frm);

                    AppSettings.Save();

                    return true;
                }
                else
                {
                    AppSettings.Load();

                    AppSettings.Default.FormStateSettings = AppSettings.SaveFormState(frm);

                    return false;
                }
            }
        }

        private bool CheckDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) return true;

            if (Directory.Exists(path)) return true;

            Msg.Error(string.Format(Resources.ErrorDirectoryNotExists, path));

            return false;
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