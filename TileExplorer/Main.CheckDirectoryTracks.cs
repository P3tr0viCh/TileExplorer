#if DEBUG
//#define SHOW_FILES
#endif

using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Enums;

namespace TileExplorer
{
    public partial class Main
    {
        internal class GpxFiles : SettingsBase<GpxFiles>
        {
            public List<string> Files { get; set; }
        }

        private readonly WrapperCancellationTokenSource ctsCheckDirectoryTracks = new WrapperCancellationTokenSource();

        private void FindFiles(string directory, List<string> files)
        {
            foreach (var dir in Directory.EnumerateDirectories(directory))
            {
                if (ctsCheckDirectoryTracks.IsCancellationRequested) return;

                FindFiles(dir, files);
            }

            foreach (var file in Directory.EnumerateFiles(directory, "*.gpx"))
            {
                if (ctsCheckDirectoryTracks.IsCancellationRequested) return;

                // check *.gpx_2
                if (!Path.GetExtension(file).ToLower().Equals(".gpx")) continue;

                files.Add(file);
            }
        }

        private async Task FindFilesAsync(string directory, List<string> files)
        {
            DebugWrite.Line("start");

            await Task.Factory.StartNew(() =>
            {
                FindFiles(directory, files);
            }, ctsCheckDirectoryTracks.Token);

#if SHOW_FILES
            await Task.Factory.StartNew(() =>
            {
                foreach (var file in files)
                {
                    if (ctsCheckDirectoryTracks.IsCancellationRequested) return;

                    DebugWrite.Line(file);
                }
            }, ctsCheckDirectoryTracks.Token);
#endif

            DebugWrite.Line("done");
        }

        private bool FileContains(string file)
        {
            foreach (var oldFile in GpxFiles.Default.Files)
            {
                if (Files.PathEquals(file, oldFile))
                {
                    return true;
                }
            }

            return false;
        }

        private void GetNewFiles(List<string> files, List<string> newFiles)
        {
            GpxFiles.Directory = AppSettings.Local.Default.DirectoryDatabase;

            GpxFiles.FileName = "GpxFiles.config";

            DebugWrite.Line($"GpxFiles: {GpxFiles.FilePath}");

            GpxFiles.Load();

            if (GpxFiles.Default.Files is null)
            {
                GpxFiles.Default.Files = new List<string>();
            }

            foreach (var file in files)
            {
                if (ctsCheckDirectoryTracks.IsCancellationRequested) return;

                if (FileContains(file)) continue;

                newFiles.Add(file);
            }

            GpxFiles.Default.Files = files;

            GpxFiles.Save();

#if SHOW_FILES
            DebugWrite.Line($"new files count: {newFiles.Count()}");

            foreach (var file in newFiles)
            {
                DebugWrite.Line(file);
            }
#endif
        }

        private async Task GetNewFilesAsync(List<string> files, List<string> newFiles)
        {
            DebugWrite.Line("start");

            await Task.Factory.StartNew(() =>
            {
                GetNewFiles(files, newFiles);
            }, ctsCheckDirectoryTracks.Token);

            DebugWrite.Line("done");
        }

        private bool QuestionAddTracks(List<string> files)
        {
            var question = string.Empty;

            if (files.Count == 1)
            {
                question = string.Format(Resources.QuestionNewTrackFinded1, files[0]);
            }
            else
            {
                var questionFiles = string.Empty;

                var maxShow = 5;

                for (var i = 0; i < maxShow && i < files.Count; i++)
                {
                    questionFiles = questionFiles.JoinExcludeEmpty(Str.Eol, files[i]);
                }

                if (files.Count <= maxShow)
                {
                    question = string.Format(Resources.QuestionNewTrackFinded2, questionFiles);
                }
                else
                {
                    question = string.Format(Resources.QuestionNewTrackFinded3, questionFiles, files.Count - maxShow);
                }
            }

            return Msg.Question(question);
        }

        private async Task CheckDirectoryTracksAsync(bool showMessage)
        {
            var directoryTracks = AppSettings.Local.Default.DirectoryTracks;

            if (directoryTracks.IsEmpty())
            {
                DebugWrite.Line("DirectoryTracks empty");

                if (showMessage)
                {
                    Msg.Error(Resources.ErrorDirectoryTracksEmpty);
                }

                return;
            }

            if (!Directory.Exists(directoryTracks))
            {
                DebugWrite.Line("DirectoryTracks not exists");

                if (showMessage)
                {
                    Msg.Error(Resources.ErrorDirectoryNotExists, directoryTracks);
                }

                return;
            }

            DebugWrite.Line("start");

            ctsCheckDirectoryTracks.Start();

            try
            {
                var files = new List<string>();
                var newFiles = new List<string>();

                var status = ProgramStatus.Start(Status.CheckDirectoryTracks);

                try
                {
                    await FindFilesAsync(directoryTracks, files);

                    await GetNewFilesAsync(files, newFiles);
                }
                finally
                {
                    ProgramStatus.Stop(status);
                }

                if (ctsCheckDirectoryTracks.IsCancellationRequested) return;

                if (newFiles.Count == 0)
                {
                    DebugWrite.Line("no new files");

                    if (showMessage)
                    {
                        Msg.Info(Resources.MsgDirectoryTracksNoNewFiles, directoryTracks);
                    }

                    return;
                }

                if (ctsCheckDirectoryTracks.IsCancellationRequested) return;

                if (!QuestionAddTracks(newFiles))
                {
                    DebugWrite.Line("add tracks canceled");

                    return;
                }

                if (ctsCheckDirectoryTracks.IsCancellationRequested) return;

                await OpenTracksAsync(newFiles.ToArray());
            }
            catch (TaskCanceledException e)
            {
                DebugWrite.Error(e);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                BeginInvoke((MethodInvoker)delegate
                {
                    Msg.Error(Resources.MsgCheckDirectoryTracksFail, e.Message);
                });
            }
            finally
            {
                ctsCheckDirectoryTracks.Finally();

                DebugWrite.Line("end");
            }
        }
    }
}