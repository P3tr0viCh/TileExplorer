using GMap.NET;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using TileExplorer.Properties;
using static TileExplorer.Database;

namespace TileExplorer
{
    public static partial class Utils
    {
        public static class Osm
        {
            public static int LatToTileY(double lat)
            {
                return P3tr0viCh.Utils.Osm.LatToTileY(lat, Const.TILE_ZOOM);
            }

            public static int LatToTileY(PointLatLng point)
            {
                return LatToTileY(point.Lat);
            }

            public static int LngToTileX(double lng)
            {
                return P3tr0viCh.Utils.Osm.LngToTileX(lng, Const.TILE_ZOOM);
            }

            public static int LngToTileX(PointLatLng point)
            {
                return LngToTileX(point.Lng);
            }

            public static double TileXToLng(int x)
            {
                return P3tr0viCh.Utils.Osm.TileXToLng(x, Const.TILE_ZOOM);
            }

            public static double TileYToLat(int y)
            {
                return P3tr0viCh.Utils.Osm.TileYToLat(y, Const.TILE_ZOOM);
            }
            private class OsmNode
            {
                public int Id;
                public PointLatLng Point;
            }

            private class OsmWay
            {
                public int Id;
                public int NodeId1;
                public int NodeId2;
            }

            public static void SaveTilesToFile(string fileName, List<Models.Tile> tiles)
            {
                if (tiles.Count == 0) return;

                var id = 0;

                using (var xml = new XmlTextWriter(fileName, null))
                {
                    xml.Formatting = Formatting.Indented;
                    xml.Indentation = 2;

                    xml.WriteStartDocument();

                    xml.WriteStartElement("osm");
                    {
                        xml.WriteAttributeString("version", null, "0.6");
                        xml.WriteAttributeString("generator", null, AssemblyNameAndVersion());
                    }

                    var osmNodes = new List<OsmNode>();
                    var osmWays = new List<OsmWay>();

                    OsmNode osmNode;
                    OsmWay osmWay;

                    var tileOsmNodesId = new List<int>();

                    foreach (var tile in tiles)
                    {
                        tileOsmNodesId.Clear();

                        foreach (var point in TilePoints(tile))
                        {
                            osmNode = osmNodes.Find(n => n.Point.Equals(point));

                            if (osmNode == null)
                            {
                                osmNode = new OsmNode()
                                {
                                    Id = ++id,
                                    Point = point
                                };
                            }

                            osmNodes.Add(osmNode);

                            tileOsmNodesId.Add(osmNode.Id);
                        }

                        osmWay = new OsmWay()
                        {
                            NodeId1 = tileOsmNodesId[0],
                            NodeId2 = tileOsmNodesId[1]
                        };

                        if (!osmWays.Exists(n => n.NodeId1 == osmWay.NodeId1 && n.NodeId2 == osmWay.NodeId2))
                        {
                            osmWay.Id = ++id;
                            osmWays.Add(osmWay);
                        }

                        osmWay = new OsmWay()
                        {
                            NodeId1 = tileOsmNodesId[0],
                            NodeId2 = tileOsmNodesId[3]
                        };

                        if (!osmWays.Exists(n => n.NodeId1 == osmWay.NodeId1 && n.NodeId2 == osmWay.NodeId2))
                        {
                            osmWay.Id = ++id;
                            osmWays.Add(osmWay);
                        }

                        osmWay = new OsmWay()
                        {
                            NodeId1 = tileOsmNodesId[1],
                            NodeId2 = tileOsmNodesId[2]
                        };

                        if (!osmWays.Exists(n => n.NodeId1 == osmWay.NodeId1 && n.NodeId2 == osmWay.NodeId2))
                        {
                            osmWay.Id = ++id;
                            osmWays.Add(osmWay);
                        }

                        osmWay = new OsmWay()
                        {
                            NodeId1 = tileOsmNodesId[3],
                            NodeId2 = tileOsmNodesId[2]
                        };

                        if (!osmWays.Exists(n => n.NodeId1 == osmWay.NodeId1 && n.NodeId2 == osmWay.NodeId2))
                        {
                            osmWay.Id = ++id;
                            osmWays.Add(osmWay);
                        }
                    }

                    // write nodes

                    foreach (var node in osmNodes)
                    {
                        xml.WriteStartElement("node");
                        {
                            xml.WriteAttributeString("id", null, "-" + node.Id);
                            xml.WriteAttributeString("action", null, "modify");
                            xml.WriteAttributeString("visible", null, "true");
                            xml.WriteAttributeString("lat", null, node.Point.Lat.ToString(CultureInfo.InvariantCulture));
                            xml.WriteAttributeString("lon", null, node.Point.Lng.ToString(CultureInfo.InvariantCulture));
                        }
                        xml.WriteEndElement();
                    }

                    // write ways

                    foreach (var way in osmWays)
                    {
                        xml.WriteStartElement("way");
                        {
                            xml.WriteAttributeString("id", null, "-" + way.Id);
                            xml.WriteAttributeString("action", null, "modify");
                            xml.WriteAttributeString("visible", null, "true");
                            {
                                xml.WriteStartElement("nd");
                                xml.WriteAttributeString("ref", null, "-" + way.NodeId1);
                                xml.WriteEndElement();

                                xml.WriteStartElement("nd");
                                xml.WriteAttributeString("ref", null, "-" + way.NodeId2);
                                xml.WriteEndElement();

                                if (!string.IsNullOrEmpty(AppSettings.Default.OsmTileKey))
                                {
                                    xml.WriteStartElement("tag");
                                    xml.WriteAttributeString("k", null, AppSettings.Default.OsmTileKey);
                                    xml.WriteAttributeString("v", null, AppSettings.Default.OsmTileValue);
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

                Debug.WriteLine("end write osm");
            }
        }
    }
}