using System;
using System.Collections.Generic;
using System.Linq;
using static TileExplorer.Database;

namespace TileExplorer
{
    internal static class Utils
    {

        private static void SetTileStatus(TileStatus[,] status, int[,] cluster, int x, int y, int clusterId)
        {
            if (status[x, y] != TileStatus.Cluster || cluster[x, y] != 0) return;

            cluster[x, y] = clusterId;

            SetTileStatus(status, cluster, x, y - 1, clusterId);
            SetTileStatus(status, cluster, x, y + 1, clusterId);
            SetTileStatus(status, cluster, x - 1, y, clusterId);
            SetTileStatus(status, cluster, x + 1, y, clusterId);
        }

        private static int CheckMaxSquare(TileStatus[,] status, int x, int y, int square)
        {
            if (status[x, y] == TileStatus.Unknown) return 0;

            square++;

            for (int i = x; i < x + square + 1; i++)
            {
                if (status[i, y + square] == TileStatus.Unknown)
                {
                    return square;
                }
            }
            for (int i = y; i < y + square + 1; i++)
            {
                if (status[x + square, i] == TileStatus.Unknown)
                {
                    return square;
                }
            }

            return CheckMaxSquare(status, x, y, square);
        }

        public struct CalcResult
        {
            public int Visited;
            public int MaxCluster;
            public int MaxSquare;
        }

        public static CalcResult CalcTiles(List<TileModel> tiles)
        {
            TileStatus[,] status = new TileStatus[Const.TILE_MAX, Const.TILE_MAX];

            int visited = 0;

            foreach (TileModel tile in tiles)
            {
                status[tile.X, tile.Y] = tile.Status;

                if (tile.Status == TileStatus.Visited) visited++;
            }

            if (visited == 0)
            {
                return new CalcResult { Visited = 0, MaxCluster = 0, MaxSquare = 0 };
            }

            foreach (TileModel tile in tiles)
            {
                if (tile.X == 0 || tile.X == Const.TILE_MAX - 1 || tile.Y == 0 || tile.Y == Const.TILE_MAX - 1) continue;

                if (tile.Status == TileStatus.Unknown) continue;

                if (status[tile.X, tile.Y - 1] == TileStatus.Unknown) continue;
                if (status[tile.X, tile.Y + 1] == TileStatus.Unknown) continue;
                if (status[tile.X - 1, tile.Y] == TileStatus.Unknown) continue;
                if (status[tile.X + 1, tile.Y] == TileStatus.Unknown) continue;

                tile.Status = TileStatus.Cluster;

                status[tile.X, tile.Y] = tile.Status;
            }

            int[,] cluster = new int[Const.TILE_MAX, Const.TILE_MAX];

            int clusterId = 0;

            foreach (TileModel tile in tiles)
            {
                if (status[tile.X, tile.Y] == TileStatus.Cluster) clusterId++;

                SetTileStatus(status, cluster, tile.X, tile.Y, clusterId);
            }

            int[] maxClusters = new int[clusterId + 1];

            foreach (TileModel tile in tiles)
            {
                if (status[tile.X, tile.Y] != TileStatus.Cluster) continue;

                maxClusters[cluster[tile.X, tile.Y]]++;
            }

            int maxCluster = 0;

            for (int i = 0; i < maxClusters.Length; i++)
            {
                if (maxClusters[i] > maxCluster)
                {
                    maxCluster = maxClusters[i];
                }
            }

            foreach (TileModel tile in tiles)
            {
                if (maxClusters[cluster[tile.X, tile.Y]] == maxCluster)
                {
                    tile.Status = TileStatus.MaxCluster;
                }
            }

            int maxSquare = 0;
            int maxSquareX = 0;
            int maxSquareY = 0;

            int checkMaxSquare;

            foreach (TileModel tile in tiles)
            {
                checkMaxSquare = CheckMaxSquare(status, tile.X, tile.Y, 0);

                if (checkMaxSquare > maxSquare)
                {
                    maxSquare = checkMaxSquare;
                    maxSquareX = tile.X;
                    maxSquareY = tile.Y;
                }
#if DEBUG
                if (checkMaxSquare > 1) tile.Text = checkMaxSquare.ToString();
#endif
            }

            if (maxSquare > 1)
            {
                for (int x = maxSquareX; x < maxSquareX + maxSquare; x++)
                {
                    for (int y = maxSquareY; y < maxSquareY + maxSquare; y++)
                    {
                        tiles.Where(tile => tile.X == x && tile.Y == y).First().Status = TileStatus.MaxSquare;
                    }
                }
            }
            
            return new CalcResult { Visited = visited, MaxCluster = maxCluster, MaxSquare = maxSquare };
        }
    }
}
