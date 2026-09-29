using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Forms;
using P3tr0viCh.Utils.Storage;
using System;
using System.IO;
using System.Windows.Forms;
using TileExplorer.Properties;

namespace TileExplorer
{
    internal class FrmSettings : FrmSettingsBase
    {
        private AppSettings AppSettings => ObjectStorage as AppSettings;

        public FrmSettings(IObjectStorage objectStorage) : base(objectStorage)
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
            AppSettings.StorageLocal.SaveFormState(this, AppSettings.Local.FormStates);
        }

        protected override void LoadFormState()
        {
            AppSettings.StorageLocal.LoadFormState(this, AppSettings.Local.FormStates);
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
                if (File.Exists(AppSettings.StorageRoaming.FilePath))
                {
                    AppSettings.StorageRoaming.Load();
                }
            }
        }

        private void AssertDirectories()
        {
            if (!AppSettings.DirectoryRoaming.IsEmpty())
            {
                if (Files.PathEquals(AppSettings.DirectoryRoaming, AppSettings.StorageLocal.Directory))
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