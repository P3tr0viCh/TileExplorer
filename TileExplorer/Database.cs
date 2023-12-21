using Dapper;
using Dapper.Contrib.Extensions;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
                    "offsetx INTEGER, offsety INTEGER);");

                connection.Execute("CREATE TABLE IF NOT EXISTS tiles (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    "x INTEGER NOT NULL, y INTEGER NOT NULL, " +
                    "UNIQUE(x, y));");

                connection.Execute("CREATE TABLE IF NOT EXISTS tracks (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    "text TEXT, datetimestart TEXT, datetimefinish TEXT, " +
                    "distance REAL, equipmentid INTEGER);");

                connection.Execute("CREATE TABLE IF NOT EXISTS tracks_points (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    "trackid INTEGER, num INTEGER, lat REAL NOT NULL, lng REAL NOT NULL, datetime TEXT, ele REAL, distance REAL, " +
                    "FOREIGN KEY (trackid) REFERENCES tracks (id) ON DELETE CASCADE ON UPDATE CASCADE);");

                connection.Execute("CREATE TABLE IF NOT EXISTS tracks_tiles (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    "trackid INTEGER, tileid INTEGER, " +
                    "FOREIGN KEY (trackid) REFERENCES tracks (id) ON DELETE CASCADE ON UPDATE CASCADE);");

                connection.Execute("CREATE TABLE IF NOT EXISTS equipments (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    "text TEXT, brand TEXT, model TEXT);");

                /* indexes */
                connection.Execute("CREATE INDEX IF NOT EXISTS tracks_datetimestart_idx ON " +
                    "tracks(datetimestart ASC)");

                connection.Execute("CREATE INDEX IF NOT EXISTS tracks_points_trackid_idx ON " +
                    "tracks_points (trackid);");

                connection.Execute("CREATE INDEX IF NOT EXISTS tracks_tiles_tileid_idx ON " +
                    "tracks_tiles(tileid);");

                connection.Execute("CREATE INDEX IF NOT EXISTS tracks_tiles_trackid_idx ON " +
                    "tracks_tiles(trackid);");

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

        public async Task<TracksInfo> LoadTracksInfoAsync(Filter filter)
        {
            using (var connection = GetConnection())
            {
                var sql = "SELECT count(*) AS count, sum(distance) AS distance FROM tracks" + filter.ToSql() + ";";

                return await connection.QueryFirstAsync<TracksInfo>(sql);
            }
        }

        public async Task MarkerSaveAsync(Marker marker)
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

        public async Task MarkerDeleteAsync(Marker marker)
        {
            using (var connection = GetConnection())
            {
                await connection.DeleteAsync(marker);
            }
        }

        public async Task EquipmentSaveAsync(Equipment equipment)
        {
            using (var connection = GetConnection())
            {
                if (equipment.Id == 0)
                {
                    await connection.InsertAsync(equipment);
                }
                else
                {
                    await connection.UpdateAsync(equipment);
                }
            }
        }

        public async Task EquipmentDeleteAsync(Equipment equipment)
        {
            using (var connection = GetConnection())
            {
                await connection.DeleteAsync(equipment);
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

        public async Task<int> TileSaveAsync(Tile tile)
        {
            using (var connection = GetConnection())
            {
                return await connection.InsertAsync(tile);
            }
        }

        public async Task<int> TileDeleteAsync(Tile tile)
        {
            using (var connection = GetConnection())
            {
                return await connection.ExecuteAsync("DELETE FROM tiles WHERE x = :x AND y = :y", new { x = tile.X, y = tile.Y });
            }
        }

        public async Task<int> TileExistsAsync(Tile tile)
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
            await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        foreach (var tracksTile in tracksTiles)
                        {
                            connection.Insert(tracksTile, transaction);
                        }

                        transaction.Commit();
                    }
                }
            });
        }

        public async Task<T> ListItemLoadAsync<T>(long itemId)
        {
            if (itemId <= Sql.NewId) return default;

            using (var connection = GetConnection())
            {
                var sql = new Sql.Query()
                {
                    table = Sql.TableName<T>(),
                    where = "id = :id"
                }.Select();

                Utils.WriteDebug(sql);

                return await connection.QueryFirstOrDefaultAsync<T>(sql, new { id = itemId });
            }
        }

        private Sql.Query GetQueryMarker()
        {
            return new Sql.Query()
            {
                table = Sql.TableName<Marker>(),
                order = "text"
            };
        }

        private Sql.Query GetQueryResults()
        {
            return new Sql.Query()
            {
                fields = "CAST(strftime('%Y', datetimestart) AS INTEGER) AS year, " +
                                    "COUNT(*) AS count, SUM(distance) / 1000.0 AS distancesum",
                table = Sql.TableName<Track>(),
                group = "year",
                order = "year"
            };
        }

        private Sql.Query GetQueryTrackPoint()
        {
            return new Sql.Query()
            {
                table = Sql.TableName<TrackPoint>(),
                where = "trackid = :trackid",
                order = "num"
            };
        }

        private Sql.Query GetQueryEquipment()
        {
            return new Sql.Query()
            {
                table = Sql.TableName<Equipment>(),
                order = "text"
            };
        }

        private Sql.Query GetQuery<T>()
        {
            switch (typeof(T).Name)
            {
                case nameof(Results):
                    return GetQueryResults();

                case nameof(Marker):
                    return GetQueryMarker();

                case nameof(TrackPoint):
                    return GetQueryTrackPoint();

                case nameof(Equipment):
                    return GetQueryEquipment();

                default:
                    throw new NotImplementedException();
            }
        }
        
        private object GetParam<T>(object filter)
        {
            switch (typeof(T).Name)
            {
                case nameof(TrackPoint):
                    return new { trackid = ((Track)filter).Id };

                default:
                    return null;
            }
        }

        private void GetSql<T>(out string sql, out object param, in object filter)
        {
            sql = GetQuery<T>().Select();

            param = GetParam<T>(filter);

            Utils.WriteDebug(sql);
        }

        public async Task<List<T>> ListLoadAsync<T>(object filter = null)
        {
            Utils.WriteDebug(typeof(T).Name);

            return await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    var sql = string.Empty;

                    object param = null;

                    switch (typeof(T).Name)
                    {
                        case nameof(Results):
                            GetSql<Results>(out sql, out param, filter);
                            break;
                        case nameof(Marker):
                            GetSql<Marker>(out sql, out param, filter);
                            break;
                        case nameof(Tile):
                            if (filter != null)
                            {
                                sql = new Sql.Query()
                                {
                                    table = Sql.TableName<Tile>(),
                                    where = "id IN (" +
                                        new Sql.Query()
                                        {
                                            fields = "tileid",
                                            table = Sql.TableName<TracksTiles>(),
                                            where = "trackid = :trackid"
                                        }.Select() +
                                            ")"
                                }.Select();

                                param = new { trackid = ((Track)filter).Id };
                            }
                            else
                            {
                                sql = Filter.Default.ToSql();

                                if (string.IsNullOrEmpty(sql))
                                    sql = "SELECT * FROM tiles;";
                                else
                                    sql = "SELECT * FROM tiles WHERE id IN (" +
                                            "SELECT tileid FROM tracks_tiles WHERE trackid IN (" +
                                                "SELECT id FROM tracks" + sql + "));";
                            }

                            break;
                        case nameof(Track):
                            if (filter == null)
                            {
                                sql = "SELECT id, text, " +
                                    "datetimestart, datetimefinish, " +
                                    "distance, equipmentid " +
                                    "FROM tracks " +
                                    Filter.Default.ToSql() + " " +
                                "ORDER BY datetimestart;";

                                /*                            sql = "SELECT id, text, " +
                                                            "dt AS datetimestart, datetimefinish, " +
                                                            "distance, " +
                                                            "equipmentid, " +
                                                            "SUM(CASE WHEN e = 0 THEN 1 ELSE 0 END) AS newtilescount " +
                                                            "FROM (" +
                                                                "SELECT *, EXISTS(" +
                                                                    "SELECT tileid, datetimestart " +
                                                                    "FROM tracks LEFT JOIN tracks_tiles ON tracks.id = tracks_tiles.trackid " +
                                                                    "WHERE tileid = tid AND datetimestart < dt) AS e " +
                                                                "FROM (" +
                                                                    "SELECT tracks.id AS id, text, " +
                                                                        "datetimestart AS dt, datetimefinish, " +
                                                                        "distance, equipmentid, tileid AS tid " +
                                                                    "FROM tracks " +
                                                                    "LEFT JOIN tracks_tiles ON tracks.id = tracks_tiles.trackid" +
                                                                     Filter.Default.ToSql() + " " +
                                                                ")" +
                                                            ") " +
                                                            "GROUP BY dt " +
                                                            "ORDER BY dt;";*/
                            }
                            else
                            {
                                sql = "SELECT * FROM tracks " +
                                "WHERE id IN (" +
                                    "SELECT trackid FROM tracks_tiles WHERE tileid IN (" +
                                        "SELECT id FROM tiles WHERE x = :x AND y = :y)) " +
                                "ORDER BY datetimestart;";

                                param = new { x = ((Tile)filter).X, y = ((Tile)filter).Y };
                            }

                            break;
                        case nameof(TrackPoint):
                            GetSql<TrackPoint>(out sql,out param, filter);

                            break;
                        case nameof(Equipment):
                            GetSql<Equipment>(out sql, out param, filter);

                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    Utils.WriteDebug(sql);

                    var list = connection.Query<T>(sql, param);

                    switch (typeof(T).Name)
                    {
                        case nameof(Track):
                            foreach (var item in list.Cast<Track>())
                            {
                                item.Equipment =
                                    ListItemLoadAsync<Equipment>(item.EquipmentId).Result;
                            }

                            break;
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
            });
        }

        public async Task LoadTileInfoAsync(Tile tile)
        {
            using (var connection = GetConnection())
            {
                tile.Tracks = await ListLoadAsync<Track>(tile);
            }
        }
    }
}