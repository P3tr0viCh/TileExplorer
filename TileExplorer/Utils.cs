using GMap.NET;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
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
            return DateTime.TryParseExact(str, Const.DATETIME_FORMAT_GPX, null, DateTimeStyles.AssumeLocal, out DateTime result) ?
                result : def;
        }

        public static double DoubleParse(string str, double def = 0.0)
        {
            return double.TryParse(str, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out double result) ?
                result : def;
        }

        public static string AssemblyNameAndVersion()
        {
            var assemblyDecorator = new Misc.AssemblyDecorator();

            return string.Format("{0}/{1}", assemblyDecorator.Assembly.GetName().Name, assemblyDecorator.VersionString());
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

        public static void WriteDebug(string s, [CallerMemberName] string memberName = "")
        {
            Debug.WriteLine(memberName + ": " + s);
        }

        public static void WriteError(Exception e, [CallerMemberName] string memberName = "")
        {
            if (e == null) return;

            WriteError(e.Message, memberName);

            WriteError(e.InnerException, memberName);
        }

        public static void WriteError(string err, [CallerMemberName] string memberName = "")
        {
            var error = string.Format("{0} fail: {1}", memberName, err);

            Debug.WriteLine(error);
        }

        public static bool IsChildFormExists(Form form)
        {
            return form != null;
        }
    }
}