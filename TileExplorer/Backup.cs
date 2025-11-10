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

        [Flags]
        public enum FileType
        {
            ExcelXml = 1,
            Gpx = 2,
        }

        public class BackupSettings
        {
            public string Directory { get; set; } = string.Empty;

            public FileType Markers { get; set; } = default;
            public FileType Equipments { get; set; } = default;

            public bool LocalSettings { get; set; } = false;
            public bool RoamingSettings { get; set; } = false;
        }

        public BackupSettings Settings { get; set; }

        public string Directory { get; private set; } = string.Empty;

        private string GetSaveDirectory()
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd");

            return Path.Combine(Settings.Directory, date);
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

        private string GetSaveFileName(string fileName, FileType fileType)
        {
            return Path.Combine(GetSaveDirectory(), fileName + GetFileNameExt(fileType));
        }

        private string GetLoadFileName(string fileName, FileType fileType)
        {
            return Path.Combine(Settings.Directory, fileName + GetFileNameExt(fileType));
        }

        private async Task SaveSettingsAsync()
        {
            DebugWrite.Line("start");

            await Task.Factory.StartNew(() =>
            {
                if (Settings.LocalSettings)
                {
                    DebugWrite.Line("save local");

                    Utils.FileCopy(AppSettings.Local.FilePath,
                        Path.Combine(GetSaveDirectory(), "Local." + Environment.MachineName + "." + Files.ExtConfig));
                }

                if (Settings.RoamingSettings)
                {
                    DebugWrite.Line("save roaming");

                    Utils.FileCopy(AppSettings.Roaming.FilePath,
                        Path.Combine(GetSaveDirectory(), "Roaming." + Files.ExtConfig));
                }
            });

            DebugWrite.Line("end");
        }

        public async Task SaveAsync()
        {
            Directory = GetSaveDirectory();

            Utils.DirectoryCreate(Directory);

            await SaveMarkersAsync();
            await SaveEquipmentsAsync();
            await SaveSettingsAsync();
        }

        public void Load()
        {
            LoadEquipments();
        }
    }
}