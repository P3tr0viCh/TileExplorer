using GMap.NET;
using P3tr0viCh;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using TileExplorer.Properties;
using static TileExplorer.Database;

namespace TileExplorer
{
    public static class Utils
    {
        private static TileModel GetTileByXY(List<TileModel> tiles, int x, int y)
        {
            return tiles.Find(t => t.X == x && t.Y == y);
        }

        private static TileStatus GetTileStatus(List<TileModel> tiles, int x, int y)
        {
            TileModel tile = GetTileByXY(tiles, x, y);

            return tile != null ? tile.Status : TileStatus.Unknown;
        }

        private static bool SetTileClusterId(List<TileModel> tiles, int x, int y, int clusterId)
        {
            TileModel tile = GetTileByXY(tiles, x, y);

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

        private static int CheckMaxSquare(List<TileModel> tiles, int x, int y, int square)
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

        public static CalcResult CalcTiles(List<TileModel> tiles)
        {
            CalcResult result = new CalcResult();

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

        private static string XmlGetText(XmlNode node)
        {
            return node != null ? node.InnerText : string.Empty;
        }

        public static TrackModel OpenTrackFromFile(string path)
        {
            var trackXml = new XmlDocument();

            var track = new TrackModel();

            try
            {
                Debug.WriteLine(path);

                trackXml.Load(path);

                Debug.WriteLine("xml loaded");

                track.TrackPoints = new List<TrackPointModel>();

                var trkptList = trackXml.GetElementsByTagName("trkpt");

                Debug.WriteLine("trkptList count: " + trkptList.Count);

                foreach (XmlNode trkpt in trkptList)
                {
                    if (trkpt.Attributes["lat"] != null && trkpt.Attributes["lon"] != null)
                    {
                        track.TrackPoints.Add(new TrackPointModel()
                        {
                            Lat = double.Parse(trkpt.Attributes["lat"].Value, CultureInfo.InvariantCulture),
                            Lng = double.Parse(trkpt.Attributes["lon"].Value, CultureInfo.InvariantCulture)
                        });
                    }
                }

                if (track.TrackPoints.Count == 0)
                {
                    throw new Exception("empty track");
                }

                double lat, lng, latPrev = 0, lngPrev = 0;

                foreach (var point in track.TrackPoints)
                {
                    lat = point.Lat;
                    lng = point.Lng;

                    point.Distance = Geo.Haversine(latPrev, lngPrev, lat, lng);

                    latPrev = lat;
                    lngPrev = lng;
                }

                track.TrackPoints[0].Distance = 0;

                track.Distance += (int)track.TrackPoints.Sum(p => p.Distance);

                string trkname = XmlGetText(trackXml.DocumentElement["trk"]?["name"]);

                if (trkname == string.Empty)
                {
                    trkname = XmlGetText(trackXml.DocumentElement["metadata"]?["name"]);
                }
                if (trkname == string.Empty)
                {
                    trkname = Path.GetFileNameWithoutExtension(path);
                }

                track.Text = trkname;

                string trktime = XmlGetText(trackXml.DocumentElement["metadata"]?["time"]);

                Debug.WriteLine("trktime: " + trktime);

                if (trktime != string.Empty)
                {
                    try
                    {
                        track.DateTime = DateTimeOffset.ParseExact(trktime, Const.DATETIME_FORMAT_GPX, null).DateTime;
                    }
                    catch (Exception)
                    {
                        track.DateTime = DateTime.Now;
                    }
                }
                else
                {
                    track.DateTime = DateTime.Now;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("error: " + e.Message);

                Msg.Error(e.Message);
            }

            Debug.WriteLine("end open xml");

            return track;
        }

        public static List<TileModel> GetTilesFromTrack(TrackModel track)
        {
            var tiles = new List<TileModel>();

            int x, y;

            foreach (var point in track.TrackPoints)
            {
                x = LngToTileX(point.Lng);
                y = LatToTileY(point.Lat);

                if (tiles.FindIndex(tile => tile.X == x && tile.Y == y) == -1)
                {
                    tiles.Add(new TileModel() { X = x, Y = y });
                }
            }

            return tiles;
        }

        public static PointLatLng TrackPointToPointLatLng(TrackPointModel trackPointModel)
        {
            return new PointLatLng(trackPointModel.Lat, trackPointModel.Lng);
        }

        public static int LngToTileX(double lng)
        {
            return Osm.LngToTileX(lng, Const.TILE_ZOOM);
        }
        public static int LngToTileX(PointLatLng point)
        {
            return LngToTileX(point.Lng);
        }

        public static int LatToTileY(double lat)
        {
            return Osm.LatToTileY(lat, Const.TILE_ZOOM);
        }

        public static int LatToTileY(PointLatLng point)
        {
            return LatToTileY(point.Lat);
        }
        public static double TileYToLat(int y)
        {
            return Osm.TileYToLat(y, Const.TILE_ZOOM);
        }

        public static double TileXToLng(int x)
        {
            return Osm.TileXToLng(x, Const.TILE_ZOOM);
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

        public static List<PointLatLng> TilePoints(TileModel tile)
        {
            double lat1 = TileYToLat(tile.Y);
            double lng1 = TileXToLng(tile.X);

            double lat2 = TileYToLat(tile.Y + 1);
            double lng2 = TileXToLng(tile.X + 1);

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

        public static void SaveTilesToOsm(string fileName, List<TileModel> tiles)
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

                            if (!string.IsNullOrEmpty(Settings.Default.OsmTileKey))
                            {
                                xml.WriteStartElement("tag");
                                xml.WriteAttributeString("k", null, Settings.Default.OsmTileKey);
                                xml.WriteAttributeString("v", null, Settings.Default.OsmTileValue);
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