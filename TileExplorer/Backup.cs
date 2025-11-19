using P3tr0viCh.Utils;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace TileExplorer
{
    public partial class Backup
    {
        private const string fileNameExtXml = ".xml";
        private const string fileNameExtGpx = ".gpx";

        [Flags]
        public enum FileName
        {
            MarkersGpx = 1,
            MarkersExcelXml = 2,
            EquipmentsExcelXml = 4,
            LocalSettings = 8,
            RoamingSettings = 16,
            TrackExts = 32,

            SaveAll = MarkersExcelXml | MarkersGpx |
                    EquipmentsExcelXml |
                    LocalSettings | RoamingSettings |
                    TrackExts,
            LoadAll = MarkersExcelXml | EquipmentsExcelXml | TrackExts,
        }

        public class BackupSettings
        {
            public string Name { get; set; }
            public string Directory { get; set; }

            public bool NameUseDate { get; set; } = true;

            public FileName FileNames { get; set; } = default;
        }

        public BackupSettings Settings { get; set; }

        public Backup(BackupSettings settings) => Settings = settings;


        private string fullPath = string.Empty;
        public string FullPath
        {
            get
            {
                if (fullPath.IsEmpty())
                {
                    if (Settings.Directory.IsEmpty())
                    {
                        throw new ArgumentNullException(nameof(Settings.Directory));
                    }

                    if (Settings.Name.IsEmpty())
                    {
                        throw new ArgumentNullException(nameof(Settings.Name));
                    }

                    fullPath = Path.Combine(Settings.Directory, Settings.Name);
                }

                return fullPath;
            }
        }

        private static string GetFileName(FileName fileName)
        {
            switch (fileName)
            {
                case FileName.MarkersGpx: return fileNameMarkers + fileNameExtGpx;
                case FileName.MarkersExcelXml: return fileNameMarkers + fileNameExtXml;
                case FileName.EquipmentsExcelXml: return fileNameEquipments + fileNameExtXml;
                case FileName.LocalSettings: return $"Local.{Environment.MachineName}.{Files.ExtConfig}";
                case FileName.RoamingSettings: return $"Roaming.{Files.ExtConfig}";
                case FileName.TrackExts: return fileNameTrackExts + fileNameExtXml;
                default: return string.Empty;
            }
        }

        public static string GetFullFileName(string directory, FileName fileName)
        {
            return Path.Combine(directory, GetFileName(fileName));
        }

        private string GetFullFileName(FileName fileName) => GetFullFileName(FullPath, fileName);

        private static DataTableFile CreateDataTableFile(DataTable table)
        {
            return new DataTableFile()
            {
                Table = table,
                Author = Utils.AssemblyNameAndVersion(),
            };
        }

        private async Task SaveSettingsAsync()
        {
            DebugWrite.Line("start");

            await Task.Factory.StartNew(() =>
            {
                if (Settings.FileNames.HasFlag(FileName.LocalSettings))
                {
                    DebugWrite.Line("save local");

                    Utils.FileCopy(AppSettings.Local.FilePath, GetFullFileName(FileName.LocalSettings));
                }

                if (Settings.FileNames.HasFlag(FileName.RoamingSettings))
                {
                    DebugWrite.Line("save roaming");

                    Utils.FileCopy(AppSettings.Roaming.FilePath, GetFullFileName(FileName.RoamingSettings));
                }
            });

            DebugWrite.Line("end");
        }

        public async Task SaveAsync()
        {
            Utils.DirectoryCreate(FullPath);

//            await Task.Delay(3000);

            await SaveMarkersAsync();
            await SaveEquipmentsAsync();
            await SaveSettingsAsync();
            await SaveTrackExtsAsync();
        }

        public async Task LoadAsync()
        {
            Files.CheckDirectoryExists(FullPath);

//            await Task.Delay(3000);

            await LoadMarkersAsync();
            await LoadEquipmentsAsync();
            await LoadTrackExtsAsync();
        }
    }
}