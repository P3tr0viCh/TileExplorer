using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public partial class Backup
    {
        private const string fileNameMarkers = "markers";

        private DataTableFile dtfMarkers = CreateDataTableFileMarkers();

        private static DataTableFile CreateDataTableFileMarkers()
        {
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

            return new DataTableFile()
            {
                Table = table,
                Author = Utils.AssemblyNameAndVersion(),
            };
        }

        private void SaveMarkersAsGpx(List<Marker> markers)
        {
            DebugWrite.Line("start");

            var fileName = GetFullFileName(FileName.MarkersGpx);

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

            foreach (var marker in markers)
            {
                var row = dtfMarkers.Table.NewRow();

                row["Latitude"] = marker.Lat;
                row["Longitude"] = marker.Lng;
                row["Text"] = marker.Text;
                row["TextVisible"] = marker.IsTextVisible;
                row["OffsetX"] = marker.OffsetX;
                row["OffsetY"] = marker.OffsetY;

                dtfMarkers.Table.Rows.Add(row);
            }

            dtfMarkers.FileName = GetFullFileName(FileName.MarkersExcelXml);

            dtfMarkers.WriteToExcelXml();

            DebugWrite.Line("end");
        }

        private async Task SaveMarkersAsync()
        {
            if (!(Settings.FileNames.HasFlag(FileName.MarkersGpx) ||
                  Settings.FileNames.HasFlag(FileName.MarkersExcelXml)))
            {
                return;
            }

            var markers = await Database.Default.ListLoadAsync<Marker>();

            if (Settings.FileNames.HasFlag(FileName.MarkersExcelXml))
            {
                SaveMarkersAsExcelXml(markers);
            }
            if (Settings.FileNames.HasFlag(FileName.MarkersGpx))
            {
                SaveMarkersAsGpx(markers);
            }
        }

        private async Task LoadMarkersAsync()
        {
            if (!Settings.FileNames.HasFlag(FileName.MarkersExcelXml))
            {
                return;
            }

            DebugWrite.Line("start");

            dtfMarkers.FileName = GetFullFileName(FileName.MarkersExcelXml);

            dtfMarkers.ReadFromExcelXml();

            var markers = new List<Marker>();

            foreach (DataRow row in dtfMarkers.Table.Rows)
            {
                DebugWrite.Line($"{row["Text"]}: {row["Latitude"]} — {row["Longitude"]}");

                markers.Add(new Marker()
                {
                    Lat = Convert.ToDouble(row["Latitude"]),
                    Lng = Convert.ToDouble(row["Longitude"]),
                    Text = Convert.ToString(row["Text"]),
                    IsTextVisible = Convert.ToBoolean(row["TextVisible"]),
                    OffsetX = Convert.ToInt32(row["OffsetX"]),
                    OffsetY = Convert.ToInt32(row["OffsetY"])
                });
            }

            await Database.Default.MarkersReplaceAsync(markers);

            DebugWrite.Line("end");
        }
    }
}