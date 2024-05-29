using GMap.NET.Internals;
using P3tr0viCh.Utils;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public static partial class Utils
    {
        public static class Backup
        {
            private const string fileNameMarkers = "markers.gpx";
            private const string fileNameEquipments = "equipments.xml";

            private static async Task SaveMarkers(string dir)
            {
                DebugWrite.Line("start");

                var markers = await Database.Default.ListLoadAsync<Marker>();

                var fileName = Path.Combine(dir, fileNameMarkers);

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

            private static void XmlExcelWriteCellEquipments(XmlTextWriter xml, string value)
            {
                xml.WriteStartElement("Cell");
                {
                    xml.WriteStartElement("Data");
                    xml.WriteAttributeString("ss:Type", "String");
                    xml.WriteValue(value);
                    xml.WriteEndElement();
                }
                xml.WriteEndElement();
            }

            private static void XmlExcelWriteRowEquipments(XmlTextWriter xml, Equipment equipment)
            {
                xml.WriteStartElement("Row");
                {
                    XmlExcelWriteCellEquipments(xml, equipment.Text);
                    XmlExcelWriteCellEquipments(xml, equipment.Brand);
                    XmlExcelWriteCellEquipments(xml, equipment.Model);
                }
                xml.WriteEndElement();
            }

            private static async Task SaveEquipments(string dir)
            {
                DebugWrite.Line("start");

                var equipments = await Database.Default.ListLoadAsync<Equipment>();

                var fileName = Path.Combine(dir, fileNameEquipments);

                using (var xml = new XmlTextWriter(fileName, Encoding.UTF8))
                {
                    xml.Formatting = Formatting.Indented;
                    xml.Indentation = 2;

                    var now = DateTime.UtcNow.ToString(Const.DATETIME_FORMAT_GPX);

                    xml.WriteStartDocument();
                    xml.WriteProcessingInstruction("mso-application", "progid=\"Excel.Sheet\"");

                    xml.WriteStartElement("Workbook");
                    {
                        xml.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:spreadsheet");
                        xml.WriteAttributeString("xmlns:o", "urn:schemas-microsoft-com:office:office");
                        xml.WriteAttributeString("xmlns:x", "urn:schemas-microsoft-com:office:excel");
                        xml.WriteAttributeString("xmlns:ss", "urn:schemas-microsoft-com:office:spreadsheet");
                        xml.WriteAttributeString("xmlns:html", "http://www.w3.org/TR/REC-html40");
                    }

                    xml.WriteStartElement("DocumentProperties");
                    {
                        xml.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:office");

                        xml.WriteElementString("Author", AssemblyNameAndVersion());
                        xml.WriteElementString("Created", now);
                    }
                    xml.WriteEndElement();

                    xml.WriteStartElement("Worksheet");
                    {
                        xml.WriteAttributeString("ss:Name", "equipments");
                    }

                    xml.WriteStartElement("Table");

                    XmlExcelWriteRowEquipments(xml, new Equipment() { Text = "Text", Brand = "Brand", Model = "Model" });

                    foreach (var equipment in equipments)
                    {
                        XmlExcelWriteRowEquipments(xml, equipment);
                    }

                    xml.WriteEndElement();

                    xml.WriteEndElement();

                    xml.WriteEndElement();

                    xml.WriteEndDocument();

                    xml.Close();
                }

                DebugWrite.Line("end");
            }

            public static async Task SaveAsync(string dir)
            {
                await SaveMarkers(dir);
                await SaveEquipments(dir);
            }
        }
    }
}