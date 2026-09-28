using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Forms;
using P3tr0viCh.Utils.Settings;
using System;
using System.IO;
using System.Windows.Forms;
using TileExplorer.Properties;

namespace TileExplorer
{
    internal class FrmSettings : FrmSettingsBase
    {
        private AppSettings AppSettings => Settings as AppSettings;

        public FrmSettings(ISettingsStore settings) : base(settings)
        {
        }
        
        public static bool ShowDlg(Form owner)
        {
            using (var frm = new FrmSettings(new AppSettings()))
            {
                return frm.ShowDialog(owner);
            }
        }

        protected override void SaveFormState()
        {
            AppSettings.StoreLocal.SaveFormState(this, AppSettings.Local.FormStates);
        }

        protected override void LoadFormState()
        {
            AppSettings.StoreLocal.LoadFormState(this, AppSettings.Local.FormStates);
        }

        private string prevDirectoryRoaming;

        protected override void BeforeOpen()
        {
            prevDirectoryRoaming = AppSettings.Local.DirectoryRoaming;
        }

        protected override void BeforeSave()
        {
            AppSettings.UpdateDirectoryRoaming();

            if (AppSettings.Local.DirectoryRoaming != prevDirectoryRoaming)
            {
                if (File.Exists(AppSettings.StoreRoaming.FilePath))
                {
                    AppSettings.StoreRoaming.Load();
                }
            }
        }

        private void AssertDirectories()
        {
            if (!AppSettings.DirectoryRoaming.IsEmpty())
            {
                if (Files.PathEquals(AppSettings.DirectoryRoaming, AppSettings.StoreLocal.Directory))
                {
                    throw new ArgumentException(
                        string.Format(Resources.ErrorDirectoryRoamingWrongLocation,
                        AppSettings.DirectoryRoaming));
                }
            }
        }

        protected override void CheckSettings()
        {
            AssertDirectories();
        }
    }
}