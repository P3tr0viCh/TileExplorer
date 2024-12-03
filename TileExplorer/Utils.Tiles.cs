using GMap.NET;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;

namespace TileExplorer
{
    internal static partial class Utils
    {
        public static class Tiles
        {
            public struct CalcResult
            {
                public int Visited;
                public int MaxCluster;
                public int MaxSquare;
            }

            private static Tile FindTileByXY(List<Tile> tiles, int x, int y)
            {
                return tiles.Find(t => t.X == x && t.Y == y);
            }

            private static TileStatus GetTileStatus(List<Tile> tiles, int x, int y)
            {
                var tile = FindTileByXY(tiles, x, y);

                return tile != null ? tile.Status : TileStatus.Unknown;
            }

            public static CalcResult CalcTiles(List<Tile> tiles)
            {
                var result = new CalcResult();

                if (tiles.Count == 0) return result;

                result.Visited = tiles.Count;

                tiles.ForEach(tile => tile.Status = TileStatus.Visited);

                foreach (var tile in tiles)
                {
                    if (tile.X == 0 || tile.X == Const.TileMax - 1 || tile.Y == 0 || tile.Y == Const.TileMax - 1) continue;

                    if (GetTileStatus(tiles, tile.X, tile.Y - 1) == TileStatus.Unknown) continue;
                    if (GetTileStatus(tiles, tile.X, tile.Y + 1) == TileStatus.Unknown) continue;
                    if (GetTileStatus(tiles, tile.X - 1, tile.Y) == TileStatus.Unknown) continue;
                    if (GetTileStatus(tiles, tile.X + 1, tile.Y) == TileStatus.Unknown) continue;

                    tile.Status = TileStatus.Cluster;
                }

                var clusterId = 0;

                foreach (var tile in tiles)
                {
                    if (SetTileClusterId(tiles, tile.X, tile.Y, clusterId))
                    {
                        clusterId++;
                    }
                }

                var clusterCapacity = new int[clusterId];

                foreach (var tile in tiles)
                {
                    if (tile.Status != TileStatus.Cluster) continue;

                    clusterCapacity[tile.ClusterId]++;
                }

                var maxClusterId = -1;

                for (var i = 0; i < clusterCapacity.Length; i++)
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

                var maxSquare = 0;

                var maxSquareX = 0;
                var maxSquareY = 0;

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

                // Heatmap

                var minTrackCount = int.MaxValue;
                var maxTrackCount = 0;

                var trackCounts = new List<int>();

                foreach (var tile in tiles)
                {
                    if (tile.TrackCount < minTrackCount) minTrackCount = tile.TrackCount;
                    if (tile.TrackCount > maxTrackCount) maxTrackCount = tile.TrackCount;

                    if (!trackCounts.Exists(t => t == tile.TrackCount)) trackCounts.Add(tile.TrackCount);
                }

                trackCounts.Sort();

                DebugWrite.Line($"{minTrackCount} — {maxTrackCount}");
                DebugWrite.Line($"{trackCounts.Count}");

                trackCounts.ForEach(t => DebugWrite.Line(t.ToString()));

                foreach (var tile in tiles)
                {
                    if (tile.TrackCount == minTrackCount)
                    {
                        tile.HeatmapValue = AppSettings.Roaming.Default.ColorTileHeatmapMinAlpha;
                    }
                    else
                    {
                        if (tile.TrackCount == maxTrackCount)
                        {
                            tile.HeatmapValue = AppSettings.Roaming.Default.ColorTileHeatmapMaxAlpha;
                        }
                        else
                        {
                            var i = trackCounts.FindIndex(t => t == tile.TrackCount);

                            tile.HeatmapValue = Scale(i,
                                0, trackCounts.Count - 1,
                                AppSettings.Roaming.Default.ColorTileHeatmapMinAlpha, AppSettings.Roaming.Default.ColorTileHeatmapMaxAlpha);
                        }
                    }

                    DebugWrite.Line($"{tile.TrackCount}: {tile.HeatmapValue}");
                }

                return result;
            }

            public static List<PointLatLng> TilePoints(Tile tile)
            {
                var lat1 = Osm.TileYToLat(tile.Y);
                var lng1 = Osm.TileXToLng(tile.X);

                var lat2 = Osm.TileYToLat(tile.Y + 1);
                var lng2 = Osm.TileXToLng(tile.X + 1);

                return new List<PointLatLng> {
                    new PointLatLng(lat1, lng1),
                    new PointLatLng(lat1, lng2),
                    new PointLatLng(lat2, lng2),
                    new PointLatLng(lat2, lng1)};
            }

            private static int CheckMaxSquare(List<Tile> tiles, int x, int y, int square)
            {
                if (GetTileStatus(tiles, x, y) == TileStatus.Unknown) return 0;

                square++;

                for (var i = x; i < x + square + 1; i++)
                {
                    if (GetTileStatus(tiles, i, y + square) == TileStatus.Unknown)
                    {
                        return square;
                    }
                }
                for (var i = y; i < y + square + 1; i++)
                {
                    if (GetTileStatus(tiles, x + square, i) == TileStatus.Unknown)
                    {
                        return square;
                    }
                }

                return CheckMaxSquare(tiles, x, y, square);
            }


            private static bool SetTileClusterId(List<Tile> tiles, int x, int y, int clusterId)
            {
                var tile = FindTileByXY(tiles, x, y);

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

            private static void AddTileBoundary(List<Tile> tiles, int x, int y, List<Tile> boudaryTiles)
            {
                if (FindTileByXY(tiles, x, y) == null)
                {
                    if (!boudaryTiles.Exists(t => t.X == x && t.Y == y))
                    {
                        boudaryTiles.Add(new Tile(x, y));
                    }
                }
            }

            public static List<Tile> FindTilesBoundary(List<Tile> tiles)
            {
                var boudaryTiles = new List<Tile>();

                foreach (var tile in tiles)
                {
                    AddTileBoundary(tiles, tile.X - 1, tile.Y, boudaryTiles);
                    AddTileBoundary(tiles, tile.X + 1, tile.Y, boudaryTiles);
                    AddTileBoundary(tiles, tile.X, tile.Y - 1, boudaryTiles);
                    AddTileBoundary(tiles, tile.X, tile.Y + 1, boudaryTiles);
                    AddTileBoundary(tiles, tile.X - 1, tile.Y - 1, boudaryTiles);
                    AddTileBoundary(tiles, tile.X + 1, tile.Y - 1, boudaryTiles);
                    AddTileBoundary(tiles, tile.X - 1, tile.Y + 1, boudaryTiles);
                    AddTileBoundary(tiles, tile.X + 1, tile.Y + 1, boudaryTiles);
                }

                return boudaryTiles;
            }
        }
    }
}