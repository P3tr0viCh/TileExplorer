using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TileExplorer
{
    public class Database
    {
        private readonly string FileName;

        public enum MarkerImageType
        {
            Default = 0,
            Image = 1,
            None
        }

        public class BaseModelId
        {
            [Key]
            public long Id { get; set; } = 0;
        }

        [Table("markers")]
        public class MarkerModel : BaseModelId
        {
            public double Lat { get; set; }
            public double Lng { get; set; }

            public string Text { get; set; }

            public bool IsTextVisible { get; set; } = true;

            public int OffsetX { get; set; }
            public int OffsetY { get; set; }

            public byte[] Image { get; set; }

            public MarkerImageType ImageType { get; set; } = MarkerImageType.Default;
        }

        public enum TileStatus
        {
            Unknown = 0,
            Visited = 1,
            Cluster = 2,
            MaxCluster = 3,
            MaxSquare = 4
        }

        [Table("tiles")]
        public class TileModel : BaseModelId
        {
            public int X { get; set; }
            public int Y { get; set; }

            [Write(false)]
            [Computed]
            public TileStatus Status { get; set; } = TileStatus.Unknown;

            [Write(false)]
            [Computed]
            public int ClusterId { get; set; } = -1;
        }

        [Table("tracks")]
        public class TrackModel : BaseModelId
        {
            public string Text { get; set; }

            public DateTime DateTime { get; set; }

            public int Distance { get; set; }

            [Write(false)]
            [Computed]
            public List<TrackPointModel> TrackPoints { get; set; }
        }

        [Table("tracks_points")]
        public class TrackPointModel : BaseModelId
        {
            public long TrackId { get; set; } = 0;

            public double Lat { get; set; }
            public double Lng { get; set; }

            public double Distance { get; set; }
        }

        [Table("tracks_tiles")]
        public class TracksTilesModel : BaseModelId
        {
            public long TrackId { get; set; } = 0;

            public long TileId { get; set; } = 0;
        }

        public class TracksInfoModel
        {
            public int Count { get; set; }

            public double Distance { get; set; }
        }

        public Database(string fileName)
        {
            FileName = fileName;
            Debug.WriteLine(FileName);
            CreateDatabase();
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
                        "text TEXT, datetime TEXT, distance INTEGER);");

                    connection.Execute("CREATE TABLE IF NOT EXISTS tracks_points (" +
                        "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                        "trackid INTEGER, lat REAL NOT NULL, lng REAL NOT NULL, distance REAL, " +
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

        public async Task<List<MarkerModel>> LoadMarkersAsync()
        {
            return await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    return connection.GetAll<MarkerModel>().OrderBy(m => m.Text).ToList();
                }
            });
        }

        public async Task<List<TileModel>> LoadTilesAsync()
        {
            return await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    return connection.GetAll<TileModel>().ToList();
                }
            });
        }

        public async Task<List<TrackModel>> LoadTracksAsync()
        {
            return await Task.Run(() =>
            {
                using (var connection = GetConnection())
                {
                    var tracks = connection.GetAll<TrackModel>().OrderBy(t => t.DateTime).ToList();

                    foreach (var track in tracks)
                    {
                        track.TrackPoints = connection.Query<TrackPointModel>(
                            "SELECT * FROM tracks_points WHERE trackid = :trackid;", new { trackid = track.Id }).ToList();
                    }

                    return tracks;
                }
            });
        }

        public async Task<TracksInfoModel> LoadTracksInfoAsync()
        {
            using (var connection = GetConnection())
            {
                return await connection.QueryFirstAsync<TracksInfoModel>
                    ("SELECT count(*) AS count, sum(distance) AS distance FROM tracks;");
            }
        }

        public async Task SaveMarkerAsync(MarkerModel marker)
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

        public async Task DeleteMarkerAsync(MarkerModel marker)
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

        public async Task SaveTilesAsync(List<TileModel> tiles)
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

        public async Task<int> SaveTileAsync(TileModel tile)
        {
            using (var connection = GetConnection())
            {
                return await connection.InsertAsync(tile);
            }
        }

        public async Task<int> DeleteTileAsync(TileModel tile)
        {
            using (var connection = GetConnection())
            {
                return await connection.ExecuteAsync("DELETE FROM tiles WHERE x = :x AND y = :y", new { x = tile.X, y = tile.Y });
            }
        }

        public async Task DeleteTrackAsync(TrackModel track)
        {
            using (var connection = GetConnection())
            {
                await connection.DeleteAsync(track);
            }
        }

        public async Task SaveTrackAsync(TrackModel track)
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
                        connection.Update<TrackModel>(track);
                    }
                }
            });
        }

        public async Task SaveTracksTilesAsync(List<TracksTilesModel> tracksTiles)
        {
            using (var connection = GetConnection())
            {
                await connection.InsertAsync(tracksTiles);
            }
        }
    }
}