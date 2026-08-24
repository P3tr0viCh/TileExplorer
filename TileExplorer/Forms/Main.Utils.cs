using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.IO;
using System.Windows.Forms;
using TileExplorer.Properties;

namespace TileExplorer
{
    public partial class Main
    {
        private bool AbnormalExit
        {
            get => Tag != null && (bool)Tag;
            set => Tag = value;
        }

        private bool CreateProgramDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return false;
            }

            Utils.DirectoryCreate(path);

            return true;
        }

        private bool SetDirectories()
        {
            try
            {
                var appDataLocalDirectory =
#if DEBUG
                    Path.Combine(Files.ExecutableDirectory(), "local");
#else
                    Files.AppDataLocalDirectory();
#endif
                var appDataRoamingDirectory =
#if DEBUG
                    Path.Combine(Files.ExecutableDirectory(), "roaming");
#else
                    Files.AppDataRoamingDirectory();
#endif

                CreateProgramDirectory(appDataLocalDirectory);
                CreateProgramDirectory(appDataRoamingDirectory);

                AppSettings.Local.Directory = appDataLocalDirectory;
                AppSettings.Roaming.Directory = appDataRoamingDirectory;

                return true;
            }
            catch (Exception e)
            {
                Msg.Error(Resources.ErrorDirectoryCreateFail, e.Message);

                WindowState = FormWindowState.Minimized;

                AbnormalExit = true;

                Application.Exit();

                return false;
            }
        }

        private void SetDatabase()
        {
            var directoryDatabase = AppSettings.Local.Default.DirectoryDatabase;

            var defaultDirectoryDatabase =
#if DEBUG
                Files.ExecutableDirectory();
#else
                Files.AppDataRoamingDirectory();
#endif

            if (directoryDatabase.IsEmpty())
            {
                directoryDatabase = defaultDirectoryDatabase;
            }
            else
            {
                if (!Directory.Exists(directoryDatabase))
                {
                    DebugWrite.Error($"database directory not exists: {directoryDatabase}");

                    Msg.Error(Resources.ErrorDatabaseDirectoryNotExists, directoryDatabase, defaultDirectoryDatabase);

                    AppSettings.Local.Default.DirectoryDatabase = string.Empty;

                    directoryDatabase = defaultDirectoryDatabase;
                }
            }

            var databaseFileName = Path.Combine(directoryDatabase, Files.DatabaseFileName());

            DebugWrite.Line($"database: {databaseFileName}");

            Database.Default.FileName = databaseFileName;
        }

        private void CancelTokens()
        {
            ctsTiles.Cancel();
            ctsTracks.Cancel();
            ctsMarkers.Cancel();
            ctsTracksInfo.Cancel();
            ctsCheckDirectoryTracks.Cancel();
        }
    }
}