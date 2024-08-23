using P3tr0viCh.AppUpdate;
using P3tr0viCh.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;

namespace TileExplorer
{
    internal class UpdateApp
    {
        private const string GitHubOwner = "P3tr0viCh";
        private const string GitHubRepo = "TileExplorer";

        public class UpdateResult
        {
            public bool IsError = false;
            public bool CanRestart = false;
            public string Message;
        }

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

        private string GetProgramStartFileName()
        {
            return Path.Combine(
                Directory.GetParent(Application.ExecutablePath).Parent.FullName,
                Path.GetFileName(Application.ExecutablePath));
        }

        public void AppRestart()
        {
            var programStart = GetProgramStartFileName();

            DebugWrite.Line(programStart);

            if (File.Exists(programStart))
            {
                Process.Start(programStart, "");
            }
        }

        public async Task<UpdateResult> UpdateAsync()
        {
            var result = new UpdateResult();

            if (AppUpdate != null)
            {
                result.Message = Resources.AppUpdateInfoInProgress;

                return result;
            }

            AppUpdate = new AppUpdate();

            AppUpdate.Config.LocalFile = Application.ExecutablePath;

            AppUpdate.Config.Location = Location.GitHub;

            AppUpdate.Config.GitHub.Owner = GitHubOwner;
            AppUpdate.Config.GitHub.Repo = GitHubRepo;

            AppUpdate.Status.StatusChanged += StatusChanged;

            try
            {
                AppUpdate.Check();

                await AppUpdate.CheckLatestVersionAsync();

                if (AppUpdate.Versions.IsLatest())
                {
                    result.Message = Resources.AppUpdateInfoAlreadyLatest;

                    return result;

                }

                await AppUpdate.UpdateAsync();

                result.CanRestart = true;

                result.Message = Resources.AppUpdateQuestionUpdated;
            }
            catch (LocalFileWrongLocationException)
            {
                var path = Path.Combine(Directory.GetParent(AppUpdate.Config.LocalFile).FullName,
                    AppUpdate.Versions.Local.ToString(),
                    Path.GetFileName(AppUpdate.Config.LocalFile));

                result.IsError = true;
                result.Message = string.Format(Resources.AppUpdateErrorFileWrongLocation, path);
            }
            catch (HttpRequestException e)
            {
                DebugWrite.Error(e);

                result.IsError = true;
                result.Message = e.Message;
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                result.IsError = true;
                result.Message = e.Message;
            }
            finally
            {
                AppUpdate = null;
            }

            return result;
        }
    }
}