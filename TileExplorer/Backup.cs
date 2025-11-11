using P3tr0viCh.Utils;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TileExplorer
{
    public partial class Backup
    {
        private const string fileNameExtXml = ".xml";
        private const string fileNameExtGpx = ".gpx";

        private enum FileName
        {
            Markers,
            Equipments,
            LocalSettings,
            RoamingSettings,
        }

        [Flags]
        public enum FileType
        {
            ExcelXml = 1,
            Gpx = 2,
        }

        public class BackupSettings
        {
            private static string name = string.Empty;
            public string Name
            {
                get => name;
                set
                {
                    name = value;
                    fullName = string.Empty;
                }
            }

            private static string directory = string.Empty;
            public string Directory
            {
                get => directory;
                set
                {
                    directory = value;
                    fullName = string.Empty;
                }
            }

            private static string fullName = string.Empty;
            public string FullName
            {
                get
                {
                    if (fullName.IsEmpty())
                    {
                        if (directory.IsEmpty())
                        {
                            directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                Files.ExecutableName() + "Backup");

                        }

                        if (name.IsEmpty())
                        {
                            name = DateTime.Now.ToString("yyyy-MM-dd");
                        }

                        fullName = Path.Combine(directory, name);
                    }

                    return fullName;
                }
                set
                {
                    fullName = value.TrimEnd('\\');
                    name = Path.GetFileName(fullName);
                    directory = Path.GetDirectoryName(fullName);
                }
            }

            public FileType Markers { get; set; } = default;
            public FileType Equipments { get; set; } = default;

            public bool LocalSettings { get; set; } = false;
            public bool RoamingSettings { get; set; } = false;
        }

        public BackupSettings Settings { get; set; }

        private string GetFileName(FileName fileName)
        {
            switch (fileName)
            {
                case FileName.Markers: return fileNameMarkers;
                case FileName.Equipments: return fileNameEquipments;
                case FileName.LocalSettings: return $"Local.{Environment.MachineName}.{Files.ExtConfig}";
                case FileName.RoamingSettings: return $"Roaming.{Files.ExtConfig}";
                default: return string.Empty;
            }
        }

        private string GetFileNameExt(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.ExcelXml: return fileNameExtXml;
                case FileType.Gpx: return fileNameExtGpx;
                default: return string.Empty;
            }
        }

        private string GetFullFileName(FileName fileName, FileType fileType)
        {
            return Path.Combine(Settings.FullName, GetFileName(fileName) + GetFileNameExt(fileType));
        }

        private async Task SaveSettingsAsync()
        {
            DebugWrite.Line("start");

            await Task.Factory.StartNew(() =>
            {
                if (Settings.LocalSettings)
                {
                    DebugWrite.Line("save local");

                    Utils.FileCopy(AppSettings.Local.FilePath, GetFullFileName(FileName.LocalSettings, default));
                }

                if (Settings.RoamingSettings)
                {
                    DebugWrite.Line("save roaming");

                    Utils.FileCopy(AppSettings.Roaming.FilePath, GetFullFileName(FileName.RoamingSettings, default));
                }
            });

            DebugWrite.Line("end");
        }

        public async Task SaveAsync()
        {
            Utils.DirectoryCreate(Settings.FullName);

            await SaveMarkersAsync();
            await SaveEquipmentsAsync();
            await SaveSettingsAsync();
        }

        public void Load()
        {
            LoadMarkers();
            LoadEquipments();
        }
    }
}