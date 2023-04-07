﻿using GMap.NET.Internals;
using P3tr0viCh;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using TileExplorer.Properties;
using static TileExplorer.Database;

namespace TileExplorer
{
    internal static class Utils
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

            Debug.WriteLine("clusters: " + clusterId + ", maxClusterId: " + maxClusterId);

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

                var trkseg = trackXml.DocumentElement["trk"]?["trkseg"];

                if (trkseg != null)
                {
                    Debug.WriteLine("trkseg count: " + trkseg.ChildNodes.Count);

                    foreach (XmlNode trkpt in trkseg)
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

                    double latPrev = track.TrackPoints[0].Lat;
                    double lngPrev = track.TrackPoints[0].Lng;

                    track.TrackPoints[0].IsUsedForDraw = true;

                    double lat, lng;
                    
                    double distance = 0, pointsDistance;

                    for (int i = 1; i < track.TrackPoints.Count; i++)
                    {
                        lat = track.TrackPoints[i].Lat;
                        lng = track.TrackPoints[i].Lng;

                        distance += Geo.Haversine(track.TrackPoints[i - 1].Lat, track.TrackPoints[i - 1].Lng, lat, lng);

                        pointsDistance = Geo.Haversine(latPrev, lngPrev, lat, lng);

                        if (pointsDistance >= Settings.Default.TrackMinDistancePoint)
                        {
                            latPrev = lat;
                            lngPrev = lng;

                            track.TrackPoints[i].IsUsedForDraw = true;
                        }
                    }

                    track.Distance = (int)distance;
                }
                else
                {
                    throw new Exception("trkseg is null");
                }

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

                Msg.Error("error: " + e.Message);
            }

            Debug.WriteLine("end open xml");

            return track;
        }
    }
}