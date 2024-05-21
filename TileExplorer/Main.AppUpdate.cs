using P3tr0viCh.AppUpdate;
using P3tr0viCh.Utils;
using System;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using TileExplorer.Properties;
using static P3tr0viCh.AppUpdate.AppUpdate;

namespace TileExplorer
{
    public partial class Main
    {
        private class UpdateApp
        {
            private const string GitHubOwner = "P3tr0viCh";
            private const string GitHubRepo = "TileExplorer";
            private const string GitHubArchiveFile = "latest.zip";

            private static readonly UpdateApp instance = new UpdateApp();
            public static UpdateApp Default => instance;

            public event ProgramStatus<Status>.StatusChangedEventHandler StatusChanged;

            private Config Config = null;

            public bool CanClose()
            {
                if (Config == null)
                {
                    return true;
                }

                return Config.Status.IsIdle();
            }

            public async void Update()
            {
                if (Config != null)
                {
                    Msg.Info(Resources.AppUpdateInfoInProgress);

                    return;
                }

                Config = new Config()
                {
                    LocalFile = Application.ExecutablePath,
                };

                Config.GitHub.Owner = GitHubOwner;
                Config.GitHub.Repo = GitHubRepo;
                Config.GitHub.ArchiveFile = GitHubArchiveFile;

                Config.Status.StatusChanged += StatusChanged;


                var errorMsg = string.Empty;

                try
                {
                    Config.Check();

                    await Config.CheckLatestVersionAsync();

                    if (Config.IsLatestVersion())
                    {
                        Msg.Info(Resources.AppUpdateInfoAlreadyLatest);

                        return;
                    }

                    await Config.UpdateAsync();

                    Msg.Info(Resources.AppUpdateInfoUpdated);
                }
                catch (LocalFileWrongLocationException)
                {
                    var path = Path.Combine(Directory.GetParent(Config.LocalFile).FullName,
                        Config.LocalVersion.ToString(),
                        Path.GetFileName(Config.LocalFile));

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
                    Config = null;
                }

                if (!errorMsg.IsEmpty())
                {
                    Msg.Error(Resources.AppUpdateErrorInProgress, errorMsg);
                }
            }
        }
    }
}