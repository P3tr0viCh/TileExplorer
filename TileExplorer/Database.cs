using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static TileExplorer.Database;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public partial class Database
    {
        private const int DEFAULT_OFFSET_X = 20;
        private const int DEFAULT_OFFSET_Y = -10;

        public enum MarkerImageType
        {
            Default = 0,
            Image = 1,
            None
        }

        public enum TileStatus
        {
            Unknown = 0,
            Visited = 1,
            Cluster = 2,
            MaxCluster = 3,
            MaxSquare = 4,
        }

        private static readonly Database defaultInstance = new Database();

        public static Database Default
        {
            get
            {
                return defaultInstance;
            }
        }

        private string fileName;

        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;

                CreateDatabase();
            }
        }

        private SQLiteConnection GetConnection()
        {
            return new SQLiteConnection("data source=" + FileName + ";version=3;foreign keys=true;");
        }

        private void CreateDatabase()
        {
            if (!File.Exists(FileName))
            {
                SQLiteConnection.CreateFile(FileName);
#if DEBUG
            }
#endif
                using (var connection = GetConnection())
                {
                    /* tables */
                    connection.Execute("CREATE TABLE IF NOT EXISTS markers (" +
                        "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                        "lat REAL NOT NULL, lng REAL NOT NULL, " +
                        "text TEXT, istextvisible INTEGER, " +
                        "offsetx INTEGER, offsety INTEGER, image BLOB, imagetype INTEGER DEFAULT 0);");

                    connection.Execute("CREATE TABLE IF NOT EXISTS tiles (" +
                        "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                        "x INTEGER NOT NULL, y INTEGER NOT NULL, " +
                        "UNIQUE(x, y));");

                    connection.Execute("CREATE TABLE IF NOT EXISTS tracks (" +
                        "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                        "text TEXT, datetimestart TEXT, datetimefinish TEXT, distance REAL);");

                    connection.Execute("CREATE TABLE IF NOT EXISTS tracks_points (" +
                        "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                        "trackid INTEGER, num INTEGER, lat REAL NOT NULL, lng REAL NOT NULL, datetime TEXT, ele REAL, distance REAL, " +
                        "FOREIGN KEY (trackid) REFERENCES tracks (id) ON DELETE CASCADE ON UPDATE CASCADE);");

                    connection.Execute("CREATE TABLE IF NOT EXISTS tracks_tiles (" +
                        "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                        "trackid INTEGER, tileid INTEGER, " +
                        "FOREIGN KEY (trackid) REFERENCES tracks (id) ON DELETE CASCADE ON UPDATE CASCADE);");

                    /* indexes */
                    connection.Execute("CREATE INDEX IF NOT EXISTS tracks_points_index ON " +
                        "tracks_points (trackid);");

                    /* triggers */
                    connection.Execute("CREATE TRIGGER IF NOT EXISTS tracks_tiles_ad AFTER DELETE ON tracks_tiles " +
                        "WHEN " +
                            "(SELECT COUNT(*) FROM tracks_tiles WHERE tileid=OLD.tileid) = 0 " +
                        "BEGIN " +
                            "DELETE FROM tiles WHERE id=OLD.tileid; " +
                        "END;");
                }
#if !DEBUG
            }
#endif
        }

        public async Task<List<Tile>> LoadTilesAsync(Filter filter)
        {
            return await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    var sql = filter.ToSql();

                    if (string.IsNullOrEmpty(sql))
                        sql = "SELECT * FROM tiles;";
                    else
                        sql = "SELECT * FROM tiles WHERE id IN (" +
                                "SELECT tileid FROM tracks_tiles WHERE trackid IN (" +
                                    "SELECT id FROM tracks " + filter.ToSql() + "));";

                    Debug.WriteLine(sql);

                    return connection.Query<Tile>(sql).ToList();
                }
            });
        }

        public async Task<List<Track>> LoadTracksAsync(Filter filter)
        {
            return await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    var sql = "SELECT * FROM tracks" + filter.ToSql() + " ORDER BY datetimestart;";

                    Debug.WriteLine(sql);

                    return connection.Query<Track>(sql).ToList();
                }
            });
        }

        public async Task LoadTrackPointsAsync(Track track)
        {
            await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    var sql = "SELECT * FROM tracks_points WHERE trackid = :trackid ORDER BY num;";

                    Debug.WriteLine(sql);

                    track.TrackPoints = connection.Query<TrackPoint>(sql, new { trackid = track.Id }).ToList();
                }
            });
        }

        public async Task<List<Tile>> LoadTrackTilesAsync(Track track)
        {
            return await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    var sql = "SELECT * FROM tiles WHERE id IN (" +
                            "SELECT tileid FROM tracks_tiles WHERE trackid = :trackid);";

                    Debug.WriteLine(sql);

                    return connection.Query<Tile>(sql, new { trackid = track.Id }).ToList();
                }
            });
        }

        public async Task<TracksInfo> LoadTracksInfoAsync(Filter filter)
        {
            using (var connection = GetConnection())
            {
                var sql = "SELECT count(*) AS count, sum(distance) AS distance FROM tracks" + filter.ToSql() + ";";

                return await connection.QueryFirstAsync<TracksInfo>(sql);
            }
        }

        public async Task SaveMarkerAsync(MapMarker marker)
        {
            using (var connection = GetConnection())
            {
                if (marker.Id == 0)
                {
                    await connection.InsertAsync(marker);
                }
                else
                {
                    await connection.UpdateAsync(marker);
                }
            }
        }

        public async Task DeleteMarkerAsync(MapMarker marker)
        {
            using (var connection = GetConnection())
            {
                await connection.DeleteAsync(marker);
            }
        }

        public async Task SaveTilesAsync(List<Tile> tiles)
        {
            await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        foreach (var tile in tiles)
                        {
                            tile.Id = connection.Insert(tile, transaction);
                        }

                        transaction.Commit();
                    }
                }
            });
        }

        public async Task<int> SaveTileAsync(Tile tile)
        {
            using (var connection = GetConnection())
            {
                return await connection.InsertAsync(tile);
            }
        }

        public async Task<int> DeleteTileAsync(Tile tile)
        {
            using (var connection = GetConnection())
            {
                return await connection.ExecuteAsync("DELETE FROM tiles WHERE x = :x AND y = :y", new { x = tile.X, y = tile.Y });
            }
        }

        public async Task<int> ExistsTileAsync(Tile tile)
        {
            return await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    return connection.ExecuteScalarAsync<int>("SELECT id FROM tiles WHERE x = :x AND y = :y", new { x = tile.X, y = tile.Y });
                }
            });
        }

        public async Task DeleteTrackAsync(Track track)
        {
            using (var connection = GetConnection())
            {
                await connection.DeleteAsync(track);
            }
        }

        public async Task SaveTrackAsync(Track track)
        {
            await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    if (track.Id == 0)
                    {
                        connection.Open();

                        using (var transaction = connection.BeginTransaction())
                        {
                            connection.Insert(track, transaction);

                            foreach (var trackPoint in track.TrackPoints)
                            {
                                trackPoint.TrackId = track.Id;
                            }

                            connection.Insert(track.TrackPoints, transaction);

                            transaction.Commit();
                        }
                    }
                    else
                    {
                        connection.Update<Track>(track);
                    }
                }
            });
        }

        public async Task SaveTracksTilesAsync(List<TracksTiles> tracksTiles)
        {
            using (var connection = GetConnection())
            {
                await connection.InsertAsync(tracksTiles);
            }
        }

        public async Task<List<T>> ListLoadAsync<T>()
        {
            Utils.WriteDebug(typeof(T).Name);

            using (var connection = GetConnection())
            {
                var sql = string.Empty;

                switch (typeof(T).Name)
                {
                    case nameof(Results):
                        sql = "SELECT " +
                                "CAST(strftime('%Y', datetimestart) AS INTEGER) AS year, " +
                                "COUNT(*) AS count, SUM(distance) / 1000.0 AS distancesum " +
                            "FROM tracks " +
                            "GROUP BY year " +
                            "ORDER BY year;";

                        break;
                    case nameof(Track):
                        sql = "SELECT " +
                                "id, text, " +
                                "datetimestart, datetimefinish, " +
                                "distance / 1000.0 AS distance " +
                                "FROM tracks" + Filter.Default.ToSql() + " ORDER BY datetimestart;";
                        
                        break;
                    case nameof(MapMarker):
                        sql = "SELECT * FROM markers ORDER BY text;";

                        break;
                    default:
                        throw new NotImplementedException();
                }

                Utils.WriteDebug(sql);

                var list = await connection.QueryAsync<T>(sql);

                switch (typeof(T).Name)
                {
                    case nameof(Results):
                        var results = list.Cast<Results>();

                        var resultSum = new Results { Count = 0, DistanceSum = 0.0 };

                        foreach (var item in results)
                        {
                            resultSum.Count += item.Count;
                            resultSum.DistanceSum += item.DistanceSum;
                        }

                        results.AsList().Add(resultSum);

                        break;
                }

                return (List<T>)list;
            }
        }
    }
}