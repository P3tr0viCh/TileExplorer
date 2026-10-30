using Newtonsoft.Json;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Storage;
using System;
using System.Diagnostics;
using System.IO;
using TileExplorer.Properties;

namespace TileExplorer
{
    internal static partial class Utils
    {
        public static class Obsidian
        {
            private const string SettingsFolderName = ".obsidian";

            private const string CorePluginsFileName = "core-plugins.json";

            private const string DailyNotesFileName = "daily-notes.json";

            private const string FileExtMd = ".md";

            private const string Uri = "obsidian://open?vault={0}&file={1}";

            private static string GetCorePluginsFilePath()
            {
                return Path.Combine(AppSettings.Default.DirectoryObsidianVault, SettingsFolderName, CorePluginsFileName);
            }

            private static string GetDailyNotesFilePath()
            {
                return Path.Combine(AppSettings.Default.DirectoryObsidianVault, SettingsFolderName, DailyNotesFileName);
            }

            private class CorePlugins : ObjectPersistenceBase
            {
                [JsonProperty("daily-notes")]
                public bool DailyNotes { get; set; } = false;
            }

            private class CorePluginsStorage : ObjectStorage<CorePlugins>
            {
                public CorePluginsStorage()
                {
                    FileName = GetCorePluginsFilePath();
                }
            }

            private class DailyNotes : ObjectPersistenceBase
            {
                [JsonProperty("folder")]
                public string Folder { get; set; } = string.Empty;

                [JsonProperty("format")]
                public string Format { get; set; } = "YYYY-MM-DD";
            }

            private class DailyNotesStorage : ObjectStorage<DailyNotes>
            {
                public DailyNotesStorage()
                {
                    FileName = GetDailyNotesFilePath();
                }
            }

            public static void AssertVault()
            {
                var corePluginsFilePath = GetCorePluginsFilePath();

                if (File.Exists(corePluginsFilePath)) return;

                throw new ArgumentException(
                    string.Format(Resources.ErrorObsidianVaultWrongLocation, AppSettings.Default.DirectoryObsidianVault));
            }

            private static void AssertDailyNotes()
            {
                var corePluginsStorage = new CorePluginsStorage();

                if (!corePluginsStorage.Load())
                {
                    throw new Exception(corePluginsStorage.LastError.Message);
                }

                if (!corePluginsStorage.Data.DailyNotes)
                {
                    throw new Exception(
                        string.Format(Resources.ErrorObsidianVaultDailyNotesDisabled,
                            AppSettings.Default.DirectoryObsidianVault));
                }
            }

            private static DailyNotes LoadDailyNotesSettings()
            {
                var dailyNotesStorage = new DailyNotesStorage();

                if (!dailyNotesStorage.Load())
                {
                    throw new Exception(dailyNotesStorage.LastError.Message);
                }

                return dailyNotesStorage.Data;
            }

            public static void OpenDailyNote(DateTime date)
            {
                try
                {
                    AssertVault();

                    AssertDailyNotes();

                    var dailyNotesSettings = LoadDailyNotesSettings();

                    var format = dailyNotesSettings.Format;

                    format = format.Replace('Y', 'y').Replace('D', 'd')
                        .Replace("/", "\\" + Path.DirectorySeparatorChar);

                    var dailyNote = date.ToString(format);

                    var dailyNotePath = Path.Combine(dailyNotesSettings.Folder, dailyNote);

                    var dailyNoteFileName = Path.Combine(
                        AppSettings.Default.DirectoryObsidianVault, dailyNotePath) + FileExtMd;

                    DebugWrite.Line(dailyNoteFileName);

                    if (File.Exists(dailyNoteFileName))
                    {
                        var vaultName = Path.GetFileName(AppSettings.Default.DirectoryObsidianVault);

                        var uri = new Uri(string.Format(Uri, vaultName, dailyNotePath));

                        DebugWrite.Line(uri);
                        DebugWrite.Line(uri.AbsoluteUri);

                        Process.Start(uri.AbsoluteUri);
                    }
                    else
                    {
                        Msg.Error(Resources.ErrorObsidianVaultDailyNoteNotExits,
                            AppSettings.Default.DirectoryObsidianVault, DateToString(date));
                    }
                }
                catch (Exception e)
                {
                    Msg.Error(Resources.ErrorObsidianVaultOpenDailyNote,
                        AppSettings.Default.DirectoryObsidianVault, e.Message);
                }
            }
        }
    }
}