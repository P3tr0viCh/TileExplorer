using P3tr0viCh.AppUpdate;
using P3tr0viCh.Utils;
using System;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using TileExplorer.Properties;

namespace TileExplorer
{
    internal class UpdateApp
    {
        private const string GitHubOwner = "P3tr0viCh";
        private const string GitHubRepo = "TileExplorer";
        private const string GitHubArchiveFile = "latest.zip";

        private static readonly UpdateApp instance = new UpdateApp();
        public static UpdateApp Default => instance;

        public event ProgramStatus<UpdateStatus>.StatusChangedEventHandler StatusChanged;

        private AppUpdate AppUpdate = null;

        public bool CanClose()
        {
            if (AppUpdate == null)
            {
                return true;
            }

            return AppUpdate.Status.IsIdle();
        }

        public async void Update()
        {
            if (AppUpdate != null)
            {
                Msg.Info(Resources.AppUpdateInfoInProgress);

                return;
            }

            AppUpdate = new AppUpdate();

            AppUpdate.Config.LocalFile = Application.ExecutablePath;

            AppUpdate.Config.Location = Location.GitHub;

            AppUpdate.Config.GitHub.Owner = GitHubOwner;
            AppUpdate.Config.GitHub.Repo = GitHubRepo;
            AppUpdate.Config.GitHub.ArchiveFile = GitHubArchiveFile;

            AppUpdate.Status.StatusChanged += StatusChanged;

            var errorMsg = string.Empty;

            try
            {
                AppUpdate.Check();

                await AppUpdate.CheckLatestVersionAsync();

                if (AppUpdate.Versions.IsLatest())
                {
                    Msg.Info(Resources.AppUpdateInfoAlreadyLatest);

                    return;
                }

                await AppUpdate.UpdateAsync();

                Msg.Info(Resources.AppUpdateInfoUpdated);
            }
            catch (LocalFileWrongLocationException)
            {
                var path = Path.Combine(Directory.GetParent(AppUpdate.Config.LocalFile).FullName,
                    AppUpdate.Versions.Local.ToString(),
                    Path.GetFileName(AppUpdate.Config.LocalFile));

                errorMsg = string.Format(Resources.AppUpdateErrorFileWrongLocation, path);
            }
            catch (HttpRequestException e)
            {
                DebugWrite.Error(e);

                errorMsg = e.Message;
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                errorMsg = e.Message;
            }
            finally
            {
                AppUpdate = null;
            }

            if (!errorMsg.IsEmpty())
            {
                Msg.Error(Resources.AppUpdateErrorInProgress, errorMsg);
            }
        }
    }
}