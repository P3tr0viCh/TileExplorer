using GMap.NET;
using GMap.NET.WindowsForms;
using P3tr0viCh.Utils;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Xml;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    internal static partial class Utils
    {
        public static class Osm
        {
            public static int LatToTileY(double lat)
            {
                return P3tr0viCh.Utils.Osm.LatToTileY(lat, Const.TileZoom);
            }

            public static int LatToTileY(PointLatLng point)
            {
                return LatToTileY(point.Lat);
            }

            public static int LngToTileX(double lng)
            {
                return P3tr0viCh.Utils.Osm.LngToTileX(lng, Const.TileZoom);
            }

            public static int LngToTileX(PointLatLng point)
            {
                return LngToTileX(point.Lng);
            }

            public static double TileXToLng(int x)
            {
                return P3tr0viCh.Utils.Osm.TileXToLng(x, Const.TileZoom);
            }

            public static double TileYToLat(int y)
            {
                return P3tr0viCh.Utils.Osm.TileYToLat(y, Const.TileZoom);
            }

            private class OsmNode
            {
                public int Id;
                public PointLatLng Point;
            }

            private class OsmWay
            {
                public int NodeId1;
                public int NodeId2;
            }

            public static void SaveTilesToFile(string fileName, List<Tile> tiles)
            {
                if (tiles.Count == 0) return;

                var id = 0;

                using (var xml = new XmlTextWriter(fileName, Encoding.UTF8))
                {
                    xml.Formatting = Formatting.Indented;
                    xml.Indentation = 2;

                    xml.WriteStartDocument(true);

                    xml.WriteStartElement("osm");
                    {
                        xml.WriteAttributeString("version", "0.6");
                        xml.WriteAttributeString("generator", AssemblyNameAndVersion());
                    }

                    var osmNodes = new List<OsmNode>();
                    var osmWays = new List<OsmWay>();

                    OsmNode osmNode;
                    OsmWay osmWay;

                    var tileOsmNodesId = new List<int>();

                    foreach (var tile in tiles)
                    {
                        tileOsmNodesId.Clear();

                        foreach (var point in Tiles.TilePoints(tile))
                        {
                            osmNode = osmNodes.Find(n => n.Point.Equals(point));

                            if (osmNode == null)
                            {
                                osmNode = new OsmNode()
                                {
                                    Id = ++id,
                                    Point = point
                                };

                                osmNodes.Add(osmNode);
                            }

                            tileOsmNodesId.Add(osmNode.Id);
                        }

                        osmWay = new OsmWay()
                        {
                            NodeId1 = tileOsmNodesId[0],
                            NodeId2 = tileOsmNodesId[1]
                        };

                        if (!osmWays.Exists(n => n.NodeId1 == osmWay.NodeId1 && n.NodeId2 == osmWay.NodeId2))
                        {
                            osmWays.Add(osmWay);
                        }

                        osmWay = new OsmWay()
                        {
                            NodeId1 = tileOsmNodesId[0],
                            NodeId2 = tileOsmNodesId[3]
                        };

                        if (!osmWays.Exists(n => n.NodeId1 == osmWay.NodeId1 && n.NodeId2 == osmWay.NodeId2))
                        {
                            osmWays.Add(osmWay);
                        }

                        osmWay = new OsmWay()
                        {
                            NodeId1 = tileOsmNodesId[1],
                            NodeId2 = tileOsmNodesId[2]
                        };

                        if (!osmWays.Exists(n => n.NodeId1 == osmWay.NodeId1 && n.NodeId2 == osmWay.NodeId2))
                        {
                            osmWays.Add(osmWay);
                        }

                        osmWay = new OsmWay()
                        {
                            NodeId1 = tileOsmNodesId[3],
                            NodeId2 = tileOsmNodesId[2]
                        };

                        if (!osmWays.Exists(n => n.NodeId1 == osmWay.NodeId1 && n.NodeId2 == osmWay.NodeId2))
                        {
                            osmWays.Add(osmWay);
                        }
                    }

                    // write nodes

                    foreach (var node in osmNodes)
                    {
                        xml.WriteStartElement("node");
                        {
                            xml.WriteAttributeString("id", "-" + node.Id);
                            xml.WriteAttributeString("action", "modify");
                            xml.WriteAttributeString("visible", "true");
                            xml.WriteAttributeString("lat", node.Point.Lat.ToString(CultureInfo.InvariantCulture));
                            xml.WriteAttributeString("lon", node.Point.Lng.ToString(CultureInfo.InvariantCulture));
                        }
                        xml.WriteEndElement();
                    }

                    // write ways

                    foreach (var way in osmWays)
                    {
                        xml.WriteStartElement("way");
                        {
                            xml.WriteAttributeString("id", "-" + ++id);
                            xml.WriteAttributeString("action", "modify");
                            xml.WriteAttributeString("visible", "true");
                            {
                                xml.WriteStartElement("nd");
                                xml.WriteAttributeString("ref", "-" + way.NodeId1);
                                xml.WriteEndElement();

                                xml.WriteStartElement("nd");
                                xml.WriteAttributeString("ref", "-" + way.NodeId2);
                                xml.WriteEndElement();

                                if (!AppSettings.Roaming.Default.OsmTileKey.IsEmpty())
                                {
                                    xml.WriteStartElement("tag");
                                    xml.WriteAttributeString("k", AppSettings.Roaming.Default.OsmTileKey);
                                    xml.WriteAttributeString("v", AppSettings.Roaming.Default.OsmTileValue);
                                    xml.WriteEndElement();
                                }
                            }
                        }
                        xml.WriteEndElement();
                    }

                    xml.WriteEndElement();

                    xml.WriteEndDocument();

                    xml.Close();
                }

                DebugWrite.Line("end write osm");
            }

            private static void StartUrl(bool open, int zoom, PointLatLng point)
            {
                if (!open && zoom < Const.OsmEditMinZoom)
                {
                    if (Msg.Question(Resources.QuestionOsmSetEditZoom))
                    {
                        zoom = Const.OsmEditMinZoom;
                    }
                }

                var url = string.Format(open ? Resources.OsmUrlOpen : Resources.OsmUrlEdit,
                    zoom,
                    point.Lat.ToString(CultureInfo.InvariantCulture),
                    point.Lng.ToString(CultureInfo.InvariantCulture));

                Process.Start(url);
            }

            public static void StartUrlOpen(int zoom, PointLatLng point)
            {
                StartUrl(true, zoom, point);
            }

            public static void StartUrlEdit(int zoom, PointLatLng point)
            {
                StartUrl(false, zoom, point);
            }
        }
    }
}