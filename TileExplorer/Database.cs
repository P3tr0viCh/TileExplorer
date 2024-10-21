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

        public static Database Default => defaultInstance;

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
            return new SQLiteConnection(string.Format(ResourcesSql.ConnectionString, FileName));
        }

        private void CreateDatabase()
        {
            if (File.Exists(FileName)) return;

            SQLiteConnection.CreateFile(FileName);

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
                connection.Execute(ResourcesSql.CreateTriggerEquipmentsAD);
                connection.Execute(ResourcesSql.CreateTriggerTracksTilesAD);
            }
        }

        public async Task<TracksInfo> LoadTracksInfoAsync(Filter filter)
        {
            using (var connection = GetConnection())
            {
                var sql = string.Format(ResourcesSql.SelectTracksInfo, filter.ToSql());

#if SHOW_SQL
                DebugWrite.Line(sql);
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

        public async Task TilesSaveAsync(List<Tile> tiles)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    tiles.ForEach(async tile =>
                    {
                        tile.Id = await connection.InsertAsync(tile, transaction);
                    });

                    transaction.Commit();
                }
            }
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
        }

        public async Task TrackDeleteAsync(Track track)
        {
            using (var connection = GetConnection())
            {
                await connection.DeleteAsync(track);
            }
        }

        public async Task TrackSaveAsync(Track track)
        {
            DebugWrite.Line("start");

            using (var connection = GetConnection())
            {
                if (track.Id == Sql.NewId)
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        await connection.InsertAsync(track, transaction);

                        foreach (var trackPoint in track.TrackPoints)
                        {
                            trackPoint.TrackId = track.Id;
                        }

                        await connection.InsertAsync(track.TrackPoints, transaction);

                        transaction.Commit();
                    }
                }
                else
                {
                    await connection.UpdateAsync(track);
                }
            }

            DebugWrite.Line("end");
        }

        public async Task TracksTilesSaveAsync(List<TracksTiles> tracksTiles)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var tracksTile in tracksTiles)
                    {
                        await connection.InsertAsync(tracksTile, transaction);
                    }

                    transaction.Commit();
                }
            }
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

                        if (sql.IsEmpty())
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
                case nameof(TracksDistanceByMonth):
                    sql = ResourcesSql.SelectTracksDistanceByMonth;

                    param = filter;

                    break;
                default:
                    throw new NotImplementedException();
            }

#if SHOW_SQL
            DebugWrite.Line(sql);
#endif
        }

        public async Task<List<T>> ListLoadAsync<T>(object filter = null)
        {
            DebugWrite.Line(typeof(T).Name);

            GetQuery<T>(out string sql, out object param, filter);

            //await Task.Delay(3000);

            using (var connection = GetConnection())
            {
                var list = await connection.QueryAsync<T>(sql, param);

                return (List<T>)list;
            }
        }

        public async Task TrackSaveNewAsync(Track track)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    var trackId = await connection.InsertAsync(track, transaction);

                    track.TrackPoints.ForEach(trackPoint =>
                        trackPoint.TrackId = track.Id
                    );

                    await connection.InsertAsync(track.TrackPoints, transaction);

                    int id;

                    await Utils.Tracks.CalcTrackTilesAsync(track);

                    track.TrackTiles.ForEach(async tile =>
                    {
                        id = await Default.GetTileIdByXYAsync(tile);

                        if (id == Sql.NewId)
                        {
                            tile.Id = await connection.InsertAsync(tile, transaction);
                        }
                        else
                        {
                            tile.Id = id;
                        }
                    });

                    track.TrackTiles.ForEach(async tile =>
                    {
                        await connection.InsertAsync(new TracksTiles()
                        {
                            TrackId = track.Id,
                            TileId = tile.Id,
                        },
                        transaction);
                    });

                    transaction.Commit();
                }
            }
        }

        public async Task<List<int>> LoadYearsAsync()
        {
            DebugWrite.Line("years");

            var sql = ResourcesSql.SelectYears;

            using (var connection = GetConnection())
            {
                var list = await connection.QueryAsync<int>(sql);

                return (List<int>)list;
            }
        }
    }
}