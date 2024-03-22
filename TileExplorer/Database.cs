#if DEBUG
//#define SHOW_SQL
#endif

using Dapper;
using Dapper.Contrib.Extensions;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public partial class Database
    {
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

#if SHOW_SQL
                Utils.WriteDebug(sql);
#endif

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
            using (var connection = GetConnection())
            {
                return await connection.ExecuteScalarAsync<int>(
                    ResourcesSql.SelectTileIdByXY,
                    new { x = tile.X, y = tile.Y });
            }
            //            });
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

        private void GetQuery<T>(out string sql, out object param, in object filter)
        {
            param = null;

            switch (typeof(T).Name)
            {
                case nameof(ResultYears):
                    sql = ResourcesSql.SelectResultYears;
                    break;
                case nameof(ResultEquipments):
                    sql = ResourcesSql.SelectResultEquipments;
                    break;
                case nameof(Marker):
                    sql = ResourcesSql.SelectMarkers;
                    break;
                case nameof(Tile):
                    if (filter != null)
                    {
                        sql = ResourcesSql.SelectTilesByTrackId;
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
                        sql = Filter.Default.ToSql();

                        sql = string.Format(ResourcesSql.SelectTracks, sql);
                    }
                    else
                    {
                        if (filter.GetType() == typeof(Tile))
                        {
                            sql = ResourcesSql.SelectTracksByTileId;

                            param = new { tileid = ((Tile)filter).Id };
                        }
                        else
                        {
                            sql = ResourcesSql.SelectTracksOnly;
                        }
                    }
                    break;
                case nameof(TrackPoint):
                    if (filter?.GetType() == typeof(Track))
                    {
                        sql = ResourcesSql.SelectTrackPointsByTrackId;

                        param = new { trackid = ((Track)filter).Id };
                    }
                    else
                    {
                        sql = ResourcesSql.SelectTrackPointsByTrackIdFull;

                        dynamic f = filter;

                        Track track = f.track;

                        param = new { trackid = track.Id };
                    }

                    break;
                case nameof(Equipment):
                    sql = ResourcesSql.SelectEquipments;
                    break;
                case nameof(TracksTree):
                    sql = ResourcesSql.SelectTracksTree;
                    break;
                default:
                    throw new NotImplementedException();
            }

#if SHOW_SQL
            Utils.WriteDebug(sql);
#endif
        }

        public async Task<List<T>> ListLoadAsync<T>(object filter = null)
        {
            Utils.WriteDebug(typeof(T).Name);

            GetQuery<T>(out string sql, out object param, filter);

            //await Task.Delay(3000);

            return await Task.Run(async () =>
            {
                using (var connection = GetConnection())
                {
                    var list = await connection.QueryAsync<T>(sql, param);

                    return (List<T>)list;
                }
            });
        }

        public async Task UpdateTrackMinDistancePointAsync(Track track)
        {
            await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        foreach (var point in track.TrackPoints)
                        {
                            connection.UpdateAsync(point, transaction);
                        }

                        transaction.Commit();
                    }
                }
            });
        }
    }
}