using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public static partial class Utils
    {
        public static class Gpx
        {
            public static void SaveTileStatusToFile(string fileName, List<Tile> tiles)
            {
                if (tiles.Count == 0) return;

                using (var xml = new XmlTextWriter(fileName, null))
                {
                    xml.Formatting = Formatting.Indented;
                    xml.Indentation = 2;

                    var now = DateTime.Now.ToString(Const.DATETIME_FORMAT_GPX);

                    xml.WriteStartDocument();

                    xml.WriteStartElement("gpx");
                    {
                        xml.WriteAttributeString("version", "1.6");
                        xml.WriteAttributeString("creator", AssemblyNameAndVersion());
                        xml.WriteAttributeString("xmlns", "http://www.topografix.com/GPX/1/1");
                        if (AppSettings.Default.TileStatusFileUseOsmand)
                        {
                            xml.WriteAttributeString("xmlns:osmand", "https://osmand.net");
                        }
                        xml.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                        xml.WriteAttributeString("xsi:schemaLocation", "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd");
                    }

                    xml.WriteStartElement("metadata");
                    {
                        xml.WriteElementString("name", Path.GetFileNameWithoutExtension(fileName));
                        xml.WriteElementString("time", now);
                    }
                    xml.WriteEndElement();

                    double lat, lon;

                    foreach (var tile in tiles)
                    {
                        xml.WriteStartElement("wpt");
                        {
                            lat = Osm.TileYToLat(tile.Y);
                            lat += (Osm.TileYToLat(tile.Y + 1) - lat) / 2.0;
                            lon = Osm.TileXToLng(tile.X);
                            lon += (Osm.TileXToLng(tile.X + 1) - lon) / 2.0;

                            xml.WriteAttributeString("lat", lat.ToString(CultureInfo.InvariantCulture));
                            xml.WriteAttributeString("lon", lon.ToString(CultureInfo.InvariantCulture));
                        }

                        xml.WriteElementString("name", tile.X + ":" + tile.Y);

                        xml.WriteElementString("time", now);

                        if (!string.IsNullOrEmpty(AppSettings.Default.TileStatusFileWptType))
                        {
                            xml.WriteElementString("type", AppSettings.Default.TileStatusFileWptType);
                        }

                        if (AppSettings.Default.TileStatusFileUseOsmand)
                        {
                            xml.WriteStartElement("extensions");
                            {
                                xml.WriteElementString("osmand:icon", AppSettings.Default.TileStatusFileOsmandIcon);
                                xml.WriteElementString("osmand:background", AppSettings.Default.TileStatusFileOsmandIconBackground.ToString().ToLower());
                                xml.WriteElementString("osmand:color", AppSettings.Default.TileStatusFileOsmandIconColor.ToHexString());
                            }
                            xml.WriteEndElement();
                        }

                        xml.WriteEndElement();
                    }

                    if (AppSettings.Default.TileStatusFileUseOsmand)
                    {
                        xml.WriteStartElement("extensions");
                        xml.WriteStartElement("osmand:points_groups");
                        xml.WriteStartElement("group");
                        {
                            xml.WriteAttributeString("name", AppSettings.Default.TileStatusFileWptType);
                            xml.WriteAttributeString("icon", AppSettings.Default.TileStatusFileOsmandIcon);
                            xml.WriteAttributeString("background", AppSettings.Default.TileStatusFileOsmandIconBackground.ToString().ToLower());
                            xml.WriteAttributeString("color", AppSettings.Default.TileStatusFileOsmandIconColor.ToHexString());
                        }
                        xml.WriteEndElement();
                        xml.WriteEndElement();
                        xml.WriteEndElement();
                    }

                    xml.WriteEndElement();

                    xml.WriteEndDocument();

                    xml.Close();
                }

                WriteDebug("end write gpx");
            }
        }
    }
}