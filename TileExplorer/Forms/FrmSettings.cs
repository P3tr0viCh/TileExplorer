using P3tr0viCh.Utils;
using System;
using System.IO;
using TileExplorer.Properties;

namespace TileExplorer
{
    internal class FrmSettings : FrmSettingsBase
    {
        public FrmSettings(ISettingsBase settings) : base(settings)
        {
        }

        protected override void SaveFormState()
        {
            AppSettings.Local.Default.FormStateSettings = AppSettings.Local.SaveFormState(this);
        }

        protected override void LoadFormState()
        {
            AppSettings.Local.LoadFormState(this, AppSettings.Local.Default.FormStateSettings);
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

        private string GetFullPath(string path)
        {
            if (path.IsEmpty()) return string.Empty;

            return Path.GetFullPath(path);
        }

        private AppSettings AppSettings => Settings as AppSettings;

        private void GetFullPaths()
        {
            AppSettings.DirectoryDatabase = GetFullPath(AppSettings.DirectoryDatabase);

            AppSettings.DirectoryTracks = GetFullPath(AppSettings.DirectoryTracks);

            AppSettings.DirectoryBackups = GetFullPath(AppSettings.DirectoryBackups);

            AppSettings.DirectoryRoaming = GetFullPath(AppSettings.DirectoryRoaming);

            PropertyGrid.Refresh();
        }

        private void AssertDirectory(string path)
        {
            if (path.IsEmpty()) return;

            if (Directory.Exists(path)) return;

            throw new Exceptions.DirectoryNotExistsException(Resources.ErrorDirectoryNotExists, path);
        }

        private void AssertDirectories()
        {
            AssertDirectory(AppSettings.DirectoryDatabase);

            AssertDirectory(AppSettings.DirectoryTracks);

            AssertDirectory(AppSettings.DirectoryBackups);

            AssertDirectory(AppSettings.DirectoryRoaming);

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
            GetFullPaths();

            AssertDirectories();
        }
    }
}