using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
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

                    xml.WriteStartDocument();

                    xml.WriteStartElement("gpx");
                    {
                        xml.WriteAttributeString("version", "1.6");
                        xml.WriteAttributeString("creator", AssemblyNameAndVersion());
                        xml.WriteAttributeString("xmlns", "http://www.topografix.com/GPX/1/1");
                        xml.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                        xml.WriteAttributeString("xsi:schemaLocation", "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd");
                    }

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