using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Forms;
using P3tr0viCh.Utils.Settings;
using System;
using System.IO;
using TileExplorer.Properties;

namespace TileExplorer
{
    internal class FrmSettings : FrmSettingsBase
    {
        private AppSettings AppSettings => Settings as AppSettings;

        public FrmSettings(ISettingsBase settings) : base(settings)
        {
        }

        protected override void SaveFormState()
        {
            AppSettings.Local.SaveFormState(this, AppSettings.Local.Default.FormStates);
        }

        protected override void LoadFormState()
        {
            AppSettings.Local.LoadFormState(this, AppSettings.Local.Default.FormStates);
        }

        private string prevDirectoryRoaming;

        protected override void BeforeOpen()
        {
            prevDirectoryRoaming = AppSettings.Local.Default.DirectoryRoaming;
        }

        protected override void BeforeSave()
        {
            AppSettings.UpdateDirectoryRoaming();

            if (AppSettings.Local.Default.DirectoryRoaming != prevDirectoryRoaming)
            {
                if (File.Exists(AppSettings.Roaming.FilePath))
                {
                    AppSettings.RoamingLoad();
                }
            }
        }

        private void AssertDirectories()
        {
            if (!AppSettings.DirectoryRoaming.IsEmpty())
            {
                if (Files.PathEquals(AppSettings.DirectoryRoaming, AppSettings.Local.Directory))
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