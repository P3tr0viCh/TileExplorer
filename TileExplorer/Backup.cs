using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public class Backup
    {
        private const string fileNameExtXml = ".xml";
        private const string fileNameExtGpx = ".gpx";

        private const string fileNameMarkers = "markers";
        private const string fileNameEquipments = "equipments";

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

        private BackupSettings Settings => AppSettings.Local.Default.BackupSettings;

        public string Directory { get; private set; } = string.Empty;

        private string GetDirectory()
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

        private string GetFileName(string fileName, FileType fileType)
        {
            return Path.Combine(GetDirectory(), fileName + GetFileNameExt(fileType));
        }

        private void SaveMarkersAsGpx(List<Marker> markers)
        {
            DebugWrite.Line("start");

            var fileName = GetFileName(fileNameMarkers, FileType.Gpx);

            using (var xml = new XmlTextWriter(fileName, Encoding.UTF8))
            {
                xml.Formatting = Formatting.Indented;
                xml.Indentation = 2;

                var now = DateTime.UtcNow.ToString(Const.DateTimeFormatGpx);

                xml.WriteStartDocument(true);

                xml.WriteStartElement("gpx");
                {
                    xml.WriteAttributeString("version", "1.6");
                    xml.WriteAttributeString("creator", Utils.AssemblyNameAndVersion());
                    xml.WriteAttributeString("xmlns", "http://www.topografix.com/GPX/1/1");
                    xml.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    xml.WriteAttributeString("xsi:schemaLocation", "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd");
                }

                xml.WriteStartElement("metadata");
                {
                    xml.WriteElementString("name", "markers");
                    xml.WriteElementString("time", now);
                }
                xml.WriteEndElement();

                foreach (var marker in markers)
                {
                    xml.WriteStartElement("wpt");
                    {
                        xml.WriteAttributeString("lat", marker.Lat.ToString(CultureInfo.InvariantCulture));
                        xml.WriteAttributeString("lon", marker.Lng.ToString(CultureInfo.InvariantCulture));
                    }

                    xml.WriteElementString("name", marker.Text);

                    xml.WriteStartElement("extensions");
                    {
                        xml.WriteElementString("tileexplorer:textvisible", marker.IsTextVisible.ToString());
                        xml.WriteElementString("tileexplorer:offsetx", marker.OffsetX.ToString(CultureInfo.InvariantCulture));
                        xml.WriteElementString("tileexplorer:offsety", marker.OffsetY.ToString(CultureInfo.InvariantCulture));
                    }
                    xml.WriteEndElement();

                    xml.WriteEndElement();
                }

                xml.WriteEndDocument();

                xml.Close();
            }

            DebugWrite.Line("end");
        }
        private void SaveMarkersAsExcelXml(List<Marker> markers)
        {
            DebugWrite.Line("start");

            var fileName = GetFileName(fileNameMarkers, FileType.ExcelXml);

            var table = new DataTable()
            {
                TableName = "Markers"
            };

            table.Columns.Add(new DataColumn()
            {
                DataType = typeof(double),
                ColumnName = "Latitude",
            });
            table.Columns.Add(new DataColumn()
            {
                DataType = typeof(double),
                ColumnName = "Longitude",
            });
            table.Columns.Add(new DataColumn()
            {
                DataType = typeof(string),
                ColumnName = "Text",
            });
            table.Columns.Add(new DataColumn()
            {
                DataType = typeof(bool),
                ColumnName = "TextVisible",
            });
            table.Columns.Add(new DataColumn()
            {
                DataType = typeof(int),
                ColumnName = "OffsetX",
            });
            table.Columns.Add(new DataColumn()
            {
                DataType = typeof(int),
                ColumnName = "OffsetY",
            });

            foreach (var marker in markers)
            {
                var row = table.NewRow();

                row["Latitude"] = marker.Lat;
                row["Longitude"] = marker.Lng;
                row["Text"] = marker.Text;
                row["TextVisible"] = marker.IsTextVisible;
                row["OffsetX"] = marker.OffsetX;
                row["OffsetY"] = marker.OffsetY;

                table.Rows.Add(row);
            }

            new DataTableFile()
            {
                FileName = fileName,
                Table = table,
                Author = Utils.AssemblyNameAndVersion(),

            }.WriteToExcelXml();

            DebugWrite.Line("end");
        }

        private async Task SaveMarkersAsync()
        {
            if (Settings.Markers == default)
            {
                return;
            }

            var markers = await Database.Default.ListLoadAsync<Marker>();

            if (Settings.Markers.HasFlag(FileType.ExcelXml))
            {
                SaveMarkersAsExcelXml(markers);
            }
            if (Settings.Markers.HasFlag(FileType.Gpx))
            {
                SaveMarkersAsGpx(markers);
            }
        }

        private async Task SaveEquipmentsAsExcelXmlAsync()
        {
            DebugWrite.Line("start");

            var equipments = await Database.Default.ListLoadAsync<Equipment>();

            var fileName = GetFileName(fileNameEquipments, FileType.ExcelXml);

            var table = new DataTable()
            {
                TableName = "Equipments"
            };

            table.Columns.Add(new DataColumn()
            {
                DataType = typeof(string),
                ColumnName = "Text",
            });
            table.Columns.Add(new DataColumn()
            {
                DataType = typeof(string),
                ColumnName = "Brand",
            });
            table.Columns.Add(new DataColumn()
            {
                DataType = typeof(string),
                ColumnName = "Model",
            });

            foreach (var equipment in equipments)
            {
                var row = table.NewRow();

                row["Text"] = equipment.Text;
                row["Brand"] = equipment.Brand;
                row["Model"] = equipment.Model;

                table.Rows.Add(row);
            }

            new DataTableFile()
            {
                FileName = fileName,
                Table = table,
                Author = Utils.AssemblyNameAndVersion(),

            }.WriteToExcelXml();

            DebugWrite.Line("end");
        }

        private async Task SaveEquipmentsAsync()
        {
            if (Settings.Equipments == default)
            {
                return;
            }

            await SaveEquipmentsAsExcelXmlAsync();
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
                        Path.Combine(GetDirectory(), "Local." + Environment.MachineName + "." + Files.ExtConfig));
                }

                if (Settings.RoamingSettings)
                {
                    DebugWrite.Line("save roaming");

                    Utils.FileCopy(AppSettings.Roaming.FilePath,
                        Path.Combine(GetDirectory(), "Roaming." + Files.ExtConfig));
                }
            });

            DebugWrite.Line("end");
        }

        public async Task SaveAsync()
        {
            Directory = GetDirectory();

            Utils.DirectoryCreate(Directory);

            await SaveMarkersAsync();
            await SaveEquipmentsAsync();
            await SaveSettingsAsync();
        }
    }
}