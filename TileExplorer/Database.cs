using Dapper;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<List<Models.Marker>> LoadMarkersAsync()
        {
            return await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    return connection.GetAll<Models.Marker>().OrderBy(m => m.Text).ToList();
                }
            });
        }

        public async Task<List<Models.Tile>> LoadTilesAsync(Filter filter)
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

                    return connection.Query<Models.Tile>(sql).ToList();
                }
            });
        }

        public async Task<List<Models.Track>> LoadTracksAsync(Filter filter)
        {
            return await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    var sql = "SELECT * FROM tracks" + filter.ToSql() + " ORDER BY datetimestart;";

                    Debug.WriteLine(sql);

                    return connection.Query<Models.Track>(sql).ToList();
                }
            });
        }

        public async Task LoadTrackPointsAsync(Models.Track track)
        {
            await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    var sql = "SELECT * FROM tracks_points WHERE trackid = :trackid ORDER BY num;";

                    Debug.WriteLine(sql);

                    track.TrackPoints = connection.Query<Models.TrackPoint>(sql, new { trackid = track.Id }).ToList();
                }
            });
        }

        public async Task<List<Models.Tile>> LoadTrackTilesAsync(Models.Track track)
        {
            return await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    var sql = "SELECT * FROM tiles WHERE id IN (" +
                            "SELECT tileid FROM tracks_tiles WHERE trackid = :trackid);";

                    Debug.WriteLine(sql);

                    return connection.Query<Models.Tile>(sql, new { trackid = track.Id }).ToList();
                }
            });
        }

        public async Task<Models.TracksInfo> LoadTracksInfoAsync(Filter filter)
        {
            using (var connection = GetConnection())
            {
                var sql = "SELECT count(*) AS count, sum(distance) AS distance FROM tracks" + filter.ToSql() + ";";

                return await connection.QueryFirstAsync<Models.TracksInfo>(sql);
            }
        }

        public async Task<List<Models.Results>> LoadResultsAsync()
        {
            return await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    var sql = "SELECT CAST(strftime('%Y', datetimestart) AS INTEGER) AS year, count(*) as count, sum(distance) as distancesum FROM tracks GROUP BY year ORDER BY year;";

                    return connection.Query<Models.Results>(sql).ToList();
                }
            });
        }

        public async Task SaveMarkerAsync(Models.Marker marker)
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

        public async Task DeleteMarkerAsync(Models.Marker marker)
        {
            using (var connection = GetConnection())
            {
                await connection.DeleteAsync(marker);
            }
        }

        /*        public void SaveImage(ImageModel image)
                {
                    Connection.Open();

                    string commandText;

                    if (image.Id == 0)
                    {
                        commandText = "INSERT INTO images(lat, lng, name, image)" +
                            " VALUES (:lat, :lng, :name, :image);";
                    }
                    else
                    {
                        commandText = "INSERT OR REPLACE INTO images(id, lat, lng, name, image)" +
                            " VALUES (:id, :lat, :lng, :name, :image);";
                    }

                    using (var command = new SQLiteCommand(commandText, Connection))
                    {
                        if (image.Id != 0)
                        {
                            command.Parameters.AddWithValue("id", image.Id);
                        }

                        command.Parameters.AddWithValue("lat", image.Lat);
                        command.Parameters.AddWithValue("lng", image.Lng);

                        command.Parameters.AddWithValue("name", image.Name);

                        MemoryStream ms = new MemoryStream();

                        image.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                        byte[] data = ms.ToArray();

                        command.Parameters.AddWithValue("image", data);

                        command.ExecuteNonQuery();

                        if (image.Id == 0)
                        {
                            image.Id = Connection.LastInsertRowId;
                        }
                    }

                    Connection.Close();
                }

                public void DeleteImage(ImageModel image)
                {
                    Connection.Open();

                    string commandText = "DELETE FROM images WHERE id = :id;";

                    using (var command = new SQLiteCommand(commandText, Connection))
                    {
                        command.Parameters.AddWithValue("id", image.Id);

                        command.ExecuteNonQuery();
                    }

                    Connection.Close();
                }

                public List<ImageModel> LoadImages()
                {
                    //return Connection.Query<ImageModel>("SELECT * FROM images;").ToList();
                    var list = new List<ImageModel>();

                    Connection.Open();

                    string commandText = "SELECT * FROM images;";

                    using (var adapter = new SQLiteDataAdapter(commandText, Connection))
                    using (var table = new DataTable())
                    {
                        adapter.Fill(table);

                        foreach (DataRow row in table.Rows)
                        {
                            list.Add(new ImageModel
                            {
                                Id = Convert.ToInt32(row["id"]),

                                Lat = Convert.ToDouble(row["lat"]),
                                Lng = Convert.ToDouble(row["lng"]),

                                Name = row["name"].GetType() != typeof(DBNull) ? Convert.ToString(row["name"]) : "",

                                Image = new Bitmap(new MemoryStream((byte[])row["image"]))
                            });
                        }
                    }

                    Connection.Close();

                    return list;
                }
        */

        public async Task SaveTilesAsync(List<Models.Tile> tiles)
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

        public async Task<int> SaveTileAsync(Models.Tile tile)
        {
            using (var connection = GetConnection())
            {
                return await connection.InsertAsync(tile);
            }
        }

        public async Task<int> DeleteTileAsync(Models.Tile tile)
        {
            using (var connection = GetConnection())
            {
                return await connection.ExecuteAsync("DELETE FROM tiles WHERE x = :x AND y = :y", new { x = tile.X, y = tile.Y });
            }
        }

        public async Task<int> ExistsTileAsync(Models.Tile tile)
        {
            return await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    return connection.ExecuteScalarAsync<int>("SELECT id FROM tiles WHERE x = :x AND y = :y", new { x = tile.X, y = tile.Y });
                }
            });
        }

        public async Task DeleteTrackAsync(Models.Track track)
        {
            using (var connection = GetConnection())
            {
                await connection.DeleteAsync(track);
            }
        }

        public async Task SaveTrackAsync(Models.Track track)
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
                        connection.Update<Models.Track>(track);
                    }
                }
            });
        }

        public async Task SaveTracksTilesAsync(List<Models.TracksTiles> tracksTiles)
        {
            using (var connection = GetConnection())
            {
                await connection.InsertAsync(tracksTiles);
            }
        }
    }
}