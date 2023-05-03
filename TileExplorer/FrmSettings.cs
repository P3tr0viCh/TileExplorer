using P3tr0viCh;
using P3tr0viCh.Utils;
using System;
using System.IO;
using System.Windows.Forms;
using TileExplorer.Properties;

namespace TileExplorer
{
    public partial class FrmSettings : Form
    {
        private readonly AppSettings AppSettings = new AppSettings();

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
                    Settings.Default.DatabaseHome = frm.AppSettings.DatabaseHome;

                    Settings.Default.ColorMarkerFill = frm.AppSettings.ColorMarkerFill;
                    Settings.Default.ColorMarkerFillAlpha = frm.AppSettings.ColorMarkerFillAlpha;
                    Settings.Default.ColorMarkerText = frm.AppSettings.ColorMarkerText;
                    Settings.Default.ColorMarkerTextAlpha = frm.AppSettings.ColorMarkerTextAlpha;

                    Settings.Default.FontMarker = frm.AppSettings.FontMarker;

                    Settings.Default.Save();
                }
            }

            return Result;
        }

        private void FrmMapDesign_Load(object sender, EventArgs e)
        {
            SettingsExt.Default.LoadFormBounds(this);

            AppSettings.DatabaseHome = Settings.Default.DatabaseHome;

            AppSettings.ColorMarkerFill = Settings.Default.ColorMarkerFill;
            AppSettings.ColorMarkerFillAlpha = Settings.Default.ColorMarkerFillAlpha;
            AppSettings.ColorMarkerText = Settings.Default.ColorMarkerText;
            AppSettings.ColorMarkerTextAlpha = Settings.Default.ColorMarkerTextAlpha;

            AppSettings.FontMarker = Settings.Default.FontMarker;

            propertyGrid.SelectedObject = AppSettings;
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
            if (e.ChangedItem.PropertyDescriptor.PropertyType == AppSettings.DatabaseHome.GetType())
            {
                CheckDirectory((string)e.ChangedItem.Value);
                return;
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (CheckDirectory(AppSettings.DatabaseHome))
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void FrmSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            SettingsExt.Default.SaveFormBounds(this);
            Settings.Default.Save();
        }
    }
}