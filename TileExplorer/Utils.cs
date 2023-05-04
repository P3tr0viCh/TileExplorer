using GMap.NET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using static TileExplorer.Database;

namespace TileExplorer
{
    public static partial class Utils
    {
        private static Models.Tile GetTileByXY(List<Models.Tile> tiles, int x, int y)
        {
            return tiles.Find(t => t.X == x && t.Y == y);
        }

        private static TileStatus GetTileStatus(List<Models.Tile> tiles, int x, int y)
        {
            Models.Tile tile = GetTileByXY(tiles, x, y);

            return tile != null ? tile.Status : TileStatus.Unknown;
        }

        private static bool SetTileClusterId(List<Models.Tile> tiles, int x, int y, int clusterId)
        {
            var tile = GetTileByXY(tiles, x, y);

            if (tile == null) return false;

            if (tile.Status != TileStatus.Cluster) return false;

            if (tile.ClusterId >= 0) return false;

            tile.ClusterId = clusterId;

            SetTileClusterId(tiles, x, y - 1, clusterId);
            SetTileClusterId(tiles, x, y + 1, clusterId);
            SetTileClusterId(tiles, x - 1, y, clusterId);
            SetTileClusterId(tiles, x + 1, y, clusterId);

            return true;
        }

        private static int CheckMaxSquare(List<Models.Tile> tiles, int x, int y, int square)
        {
            if (GetTileStatus(tiles, x, y) == TileStatus.Unknown) return 0;

            square++;

            for (int i = x; i < x + square + 1; i++)
            {
                if (GetTileStatus(tiles, i, y + square) == TileStatus.Unknown)
                {
                    return square;
                }
            }
            for (int i = y; i < y + square + 1; i++)
            {
                if (GetTileStatus(tiles, x + square, i) == TileStatus.Unknown)
                {
                    return square;
                }
            }

            return CheckMaxSquare(tiles, x, y, square);
        }

        public struct CalcResult
        {
            public int Visited;
            public int MaxCluster;
            public int MaxSquare;
        }

        public static CalcResult CalcTiles(List<Models.Tile> tiles)
        {
            var result = new CalcResult();

            foreach (var tile in tiles)
            {
                if (tile.Status == TileStatus.Visited) result.Visited++;
            }

            if (result.Visited == 0)
            {
                return result;
            }

            foreach (var tile in tiles)
            {
                if (tile.X == 0 || tile.X == Const.TILE_MAX - 1 || tile.Y == 0 || tile.Y == Const.TILE_MAX - 1) continue;

                if (tile.Status == TileStatus.Unknown) continue;

                if (GetTileStatus(tiles, tile.X, tile.Y - 1) == TileStatus.Unknown) continue;
                if (GetTileStatus(tiles, tile.X, tile.Y + 1) == TileStatus.Unknown) continue;
                if (GetTileStatus(tiles, tile.X - 1, tile.Y) == TileStatus.Unknown) continue;
                if (GetTileStatus(tiles, tile.X + 1, tile.Y) == TileStatus.Unknown) continue;

                tile.Status = TileStatus.Cluster;
            }

            int clusterId = 0;

            foreach (var tile in tiles)
            {
                if (SetTileClusterId(tiles, tile.X, tile.Y, clusterId))
                {
                    clusterId++;
                }
            }

            int[] clusterCapacity = new int[clusterId];

            foreach (var tile in tiles)
            {
                if (tile.Status != TileStatus.Cluster) continue;

                clusterCapacity[tile.ClusterId]++;
            }

            int maxClusterId = -1;

            for (int i = 0; i < clusterCapacity.Length; i++)
            {
                if (clusterCapacity[i] > result.MaxCluster)
                {
                    result.MaxCluster = clusterCapacity[i];
                    maxClusterId = i;
                }
            }

            if (maxClusterId >= 0)
            {
                foreach (var tile in tiles)
                {
                    if (tile.ClusterId == maxClusterId)
                    {
                        tile.Status = TileStatus.MaxCluster;
                    }
                }
            }

            int maxSquare;

            int maxSquareX = 0;
            int maxSquareY = 0;

            foreach (var tile in tiles)
            {
                maxSquare = CheckMaxSquare(tiles, tile.X, tile.Y, 0);

                if (maxSquare > result.MaxSquare)
                {
                    result.MaxSquare = maxSquare;

                    maxSquareX = tile.X;
                    maxSquareY = tile.Y;
                }
            }

            if (result.MaxSquare > 1)
            {
                foreach (var tile in tiles.Where(tile =>
                    tile.X >= maxSquareX && tile.X < maxSquareX + result.MaxSquare &&
                    tile.Y >= maxSquareY && tile.Y < maxSquareY + result.MaxSquare))
                {
                    tile.Status = TileStatus.MaxSquare;
                }
            }

            return result;
        }

        public static List<Models.Tile> GetTilesFromTrack(Models.Track track)
        {
            var tiles = new List<Models.Tile>();

            int x, y;

            foreach (var point in track.TrackPoints)
            {
                x = Osm.LngToTileX(point.Lng);
                y = Osm.LatToTileY(point.Lat);

                if (tiles.FindIndex(tile => tile.X == x && tile.Y == y) == -1)
                {
                    tiles.Add(new Models.Tile() { X = x, Y = y });
                }
            }

            return tiles;
        }

        public static PointLatLng TrackPointToPointLatLng(Models.TrackPoint trackPointModel)
        {
            return new PointLatLng(trackPointModel.Lat, trackPointModel.Lng);
        }

        public static DateTime DateTimeParse(string str, DateTime def = default)
        {
            return DateTimeOffset.TryParseExact(str, Const.DATETIME_FORMAT_GPX, null, DateTimeStyles.None, out DateTimeOffset result) ?
                result.DateTime : def;
        }

        public static double DoubleParse(string str, double def = 0.0)
        {
            return double.TryParse(str, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out double result) ?
                result : def;
        }

        public static string AssemblyNameAndVersion()
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();

            var result = string.Format("{0}/{1}.{2}", assemblyName.Name, assemblyName.Version.Major, assemblyName.Version.Minor);
#if DEBUG
            result += ".debug";
#endif
            return result;
        }

        public static List<PointLatLng> TilePoints(Models.Tile tile)
        {
            var lat1 = Osm.TileYToLat(tile.Y);
            var lng1 = Osm.TileXToLng(tile.X);

            var lat2 = Osm.TileYToLat(tile.Y + 1);
            var lng2 = Osm.TileXToLng(tile.X + 1);

            return new List<PointLatLng>
            {
                new PointLatLng(lat1, lng1),
                new PointLatLng(lat1, lng2),
                new PointLatLng(lat2, lng2),
                new PointLatLng(lat2, lng1)
            };
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

        public static void SaveTilesToOsm(string fileName, List<Models.Tile> tiles)
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

                            if (!string.IsNullOrEmpty(Properties.Settings.Default.OsmTileKey))
                            {
                                xml.WriteStartElement("tag");
                                xml.WriteAttributeString("k", null, Properties.Settings.Default.OsmTileKey);
                                xml.WriteAttributeString("v", null, Properties.Settings.Default.OsmTileValue);
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