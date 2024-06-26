﻿using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static TileExplorer.Database.Models;
using static TileExplorer.Utils.Backup;

namespace TileExplorer
{
    public static partial class Utils
    {
        public class BackupSettings
        {
            public string Directory { get; set; } = string.Empty;

            public FileType FileTypeMarkers { get; set; } = default;
            public FileType FileTypeEquipments { get; set; } = default;

            public void Clear()
            {
                Directory = string.Empty;

                FileTypeMarkers = default;
                FileTypeEquipments = default;
            }

            public void Assign(BackupSettings source)
            {
                if (source == null)
                {
                    Clear();

                    return;
                }

                Directory = source.Directory;

                FileTypeMarkers = source.FileTypeMarkers;
                FileTypeEquipments = source.FileTypeEquipments;
            }
        }

        public class Backup
        {
            [Flags]
            public enum FileType
            {
                ExcelXml = 1,
                Gpx = 2,
            }

            private const string fileNameExtXml = ".xml";
            private const string fileNameExtGpx = ".gpx";

            private const string fileNameMarkers = "markers";
            private const string fileNameEquipments = "equipments";

            private readonly BackupSettings settings = new BackupSettings();
            public BackupSettings Settings
            {
                get
                {
                    return settings;
                }
                set
                {
                    settings.Assign(value);
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

            private string GetFileName(string fileName, FileType fileType)
            {
                return Path.Combine(Settings.Directory, fileName + GetFileNameExt(fileType));
            }

            private void SaveMarkersAsGpx(List<Marker> markers)
            {
                DebugWrite.Line("start");

                var fileName = GetFileName(fileNameMarkers, FileType.Gpx);

                using (var xml = new XmlTextWriter(fileName, Encoding.UTF8))
                {
                    xml.Formatting = Formatting.Indented;
                    xml.Indentation = 2;

                    var now = DateTime.UtcNow.ToString(Const.DATETIME_FORMAT_GPX);

                    xml.WriteStartDocument(true);

                    xml.WriteStartElement("gpx");
                    {
                        xml.WriteAttributeString("version", "1.6");
                        xml.WriteAttributeString("creator", AssemblyNameAndVersion());
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
                    Author = AssemblyNameAndVersion(),

                }.WriteToExcelXml();

                DebugWrite.Line("end");
            }

            private async Task SaveMarkers()
            {
                if (Settings.FileTypeMarkers == default)
                {
                    return;
                }

                var markers = await Database.Default.ListLoadAsync<Marker>();

                if (Settings.FileTypeMarkers.HasFlag(FileType.ExcelXml))
                {
                    SaveMarkersAsExcelXml(markers);
                }
                if (Settings.FileTypeMarkers.HasFlag(FileType.Gpx))
                {
                    SaveMarkersAsGpx(markers);
                }
            }

            private async Task SaveEquipmentsAsExcelXml()
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
                    Author = AssemblyNameAndVersion(),

                }.WriteToExcelXml();

                DebugWrite.Line("end");
            }

            private async Task SaveEquipments()
            {
                if (Settings.FileTypeEquipments == default)
                {
                    return;
                }

                await SaveEquipmentsAsExcelXml();
            }

            public async Task SaveAsync()
            {
                await SaveMarkers();
                await SaveEquipments();
            }
        }
    }
}