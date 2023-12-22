using Dapper;
using Dapper.Contrib.Extensions;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TileExplorer.Properties;
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
            return new SQLiteConnection(
                string.Format(ResourcesSql.ConnectionString, FileName));
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
                connection.Execute(ResourcesSql.CreateTableMarkers);
                connection.Execute(ResourcesSql.CreateTableTiles);
                connection.Execute(ResourcesSql.CreateTableTracks);
                connection.Execute(ResourcesSql.CreateTableTracksPoints);
                connection.Execute(ResourcesSql.CreateTableTracksTiles);
                connection.Execute(ResourcesSql.CreateTableEquipments);

                /* indexes */
                connection.Execute(ResourcesSql.CreateIndexTracksDateTimeStart);
                connection.Execute(ResourcesSql.CreateIndexTracksPointsTrackId);
                connection.Execute(ResourcesSql.CreateIndexTracksTilesTileId);
                connection.Execute(ResourcesSql.CreateIndexTracksTilesTrackId);

                /* triggers */
                connection.Execute(ResourcesSql.CreateTriggerTracksTilesAD);
            }
#if !DEBUG
            }
#endif
        }

        public async Task<TracksInfo> LoadTracksInfoAsync(Filter filter)
        {
            using (var connection = GetConnection())
            {
                var sql = string.Format(ResourcesSql.SelectTracksInfo,
                    filter.ToSql());

                Utils.WriteDebug(sql);

                return await connection.QueryFirstAsync<TracksInfo>(sql);
            }
        }

        public async Task MarkerSaveAsync(Marker marker)
        {
            using (var connection = GetConnection())
            {
                if (marker.Id == Sql.NewId)
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
                if (equipment.Id == Sql.NewId)
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

        public async Task<bool> TileDeleteAsync(Tile tile)
        {
            using (var connection = GetConnection())
            {
                return await connection.DeleteAsync(tile);
            }
        }

        public async Task<int> GetTileIdByXYAsync(Tile tile)
        {
            return await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    return connection.ExecuteScalarAsync<int>(
                        ResourcesSql.SelectTileIdByXY,
                        new { x = tile.X, y = tile.Y });
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
                    if (track.Id == Sql.NewId)
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

        private void GetQuery<T>(out string sql, out object param, in object filter)
        {
            param = null;

            switch (typeof(T).Name)
            {
                case nameof(Results):
                    sql = ResourcesSql.SelectResults;
                    break;
                case nameof(Marker):
                    sql = ResourcesSql.SelectMarkers;
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
                            sql = ResourcesSql.SelectTiles;
                        else
                            sql = string.Format(
                                ResourcesSql.SelectTilesByTrackIds, sql);
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
                        sql = ResourcesSql.SelectTracksByTileId;
                        param = new { tileid = ((Tile)filter).Id };
                    }
                    break;
                case nameof(TrackPoint):
                    sql = ResourcesSql.SelectTrackPointsByTrackId;
                    param = new { trackid = ((Track)filter).Id };
                    break;
                case nameof(Equipment):
                    sql = ResourcesSql.SelectEquipments;
                    break;
                default:
                    throw new NotImplementedException();
            }

            Utils.WriteDebug(sql);
        }

        public async Task<List<T>> ListLoadAsync<T>(object filter = null)
        {
            Utils.WriteDebug(typeof(T).Name);

            return await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    GetQuery<T>(out string sql, out object param, filter);

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
    }
}