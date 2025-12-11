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
            using (var frm = new FrmSettings())
            {
                AppSettings.Save();

                AppSettings.LoadFormState(frm, AppSettings.Local.Default.FormStateSettings);

                var prevDirectoryRoaming = AppSettings.Local.Default.DirectoryRoaming;

                frm.propertyGrid.SelectedObject = new AppSettings();

                if (frm.ShowDialog(owner) == DialogResult.OK)
                {
                    AppSettings.Local.Default.FormStateSettings = AppSettings.SaveFormState(frm);

                    AppSettings.UpdateDirectoryRoaming();

                    if (AppSettings.Local.Default.DirectoryRoaming != prevDirectoryRoaming)
                    {
                        if (File.Exists(AppSettings.Roaming.FilePath))
                        {
                            AppSettings.RoamingLoad();
                        }
                    }

                    AppSettings.Save();

                    return true;
                }
                else
                {
                    AppSettings.Load();

                    AppSettings.Local.Default.FormStateSettings = AppSettings.SaveFormState(frm);

                    return false;
                }
            }
        }

        private bool CheckDirectory(string path)
        {
            if (path.IsEmpty()) return true;

            if (Directory.Exists(path)) return true;

            Msg.Error(string.Format(Resources.ErrorDirectoryNotExists, path));

            return false;
        }

        private bool CheckData()
        {
            if (!CheckDirectory(AppSettings.Local.Default.DirectoryDatabase))
            {
                return false;
            }

            if (!CheckDirectory(AppSettings.Local.Default.DirectoryTracks))
            {
                return false;
            }

            if (!CheckDirectory(AppSettings.Local.Default.DirectoryRoaming))
            {
                return false;
            }

            if (!AppSettings.Local.Default.DirectoryRoaming.IsEmpty())
            {
                if (Files.PathEquals(AppSettings.Local.Default.DirectoryRoaming, AppSettings.Local.Directory))
                {
                    Msg.Error(string.Format(Resources.ErrorDirectoryWrongLocation, AppSettings.Local.Default.DirectoryRoaming));

                    return false;
                }
            }

            return true;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (CheckData())
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}