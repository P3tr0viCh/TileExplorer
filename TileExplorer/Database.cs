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

        private readonly SQLiteConnection Connection;

        public enum MarkerImageType
        {
            Default = 0,
            Image = 1,
            None
        }

        [Table("markers")]
        public class MarkerModel
        {
            [Key]
            public long Id { get; set; }

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
        public class TileModel
        {
            [Key]
            public long Id { get; set; }

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
        public class TrackModel
        {
            [Key]
            public long Id { get; set; }

            public string Text { get; set; }

            public DateTime DateTime { get; set; }

            public int Distance { get; set; }

            [Write(false)]
            [Computed]
            public List<TrackPointModel> TrackPoints { get; set; }
        }

        [Table("tracks_points")]
        public class TrackPointModel
        {
            [Key]
            public long Id { get; set; }
            public long TrackId { get; set; }

            public double Lat { get; set; }
            public double Lng { get; set; }

            public bool IsUsedForDraw { get; set; } = false;
        }

        public Database(string fileName)
        {
            FileName = fileName;

            Connection = new SQLiteConnection("Data Source=" + FileName + ";Version=3;");

            CreateDatabase();
        }

        private void CreateDatabase()
        {
            if (!File.Exists(FileName))
            {
                SQLiteConnection.CreateFile(FileName);

                Connection.Execute("CREATE TABLE markers (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    "lat REAL NOT NULL, lng REAL NOT NULL, " +
                    "text TEXT, istextvisible INTEGER, " +
                    "offsetx INTEGER, offsety INTEGER, image BLOB, imagetype INTEGER DEFAULT 0);");

                Connection.Execute("CREATE TABLE tiles (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    "x INTEGER NOT NULL, y INTEGER NOT NULL, " +
                    "UNIQUE(x, y));");
            }

            Connection.Execute("CREATE TABLE IF NOT EXISTS tracks (" +
                "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "text TEXT, datetime TEXT, distance INTEGER);");

            Connection.Execute("CREATE TABLE IF NOT EXISTS tracks_points (" +
                "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "trackid INTEGER, lat REAL NOT NULL, lng REAL NOT NULL, isusedfordraw INTEGER);");

            Connection.Execute("CREATE INDEX IF NOT EXISTS tracks_points_index ON " +
                "tracks_points (trackid);");

            Connection.Execute("CREATE TRIGGER IF NOT EXISTS tracks_delete " +
                "BEFORE DELETE ON tracks " +
                    "FOR EACH ROW BEGIN DELETE FROM tracks_points WHERE tracks_points.trackid = OLD.id; " +
                "END;");
        }

        public async Task<List<MarkerModel>> LoadMarkersAsync()
        {
            return await Task.Run(() => Connection.GetAll<MarkerModel>().ToList());
        }

        public async Task<List<TileModel>> LoadTilesAsync()
        {
            return await Task.Run(() => Connection.GetAll<TileModel>().ToList());
        }

        public async Task<List<TrackModel>> LoadTracksAsync()
        {
            return await Task.Run(() =>
            {
                var tracks = Connection.GetAll<TrackModel>().ToList();

                foreach (var track in tracks)
                {
                    track.TrackPoints = Connection.Query<TrackPointModel>(
                        "SELECT * FROM tracks_points WHERE trackid = :trackid", new { trackid = track.Id }).ToList();
                }

                return tracks;
            });
        }

        public async Task SaveMarkerAsync(MarkerModel marker)
        {
            if (marker.Id == 0)
            {
                await Connection.InsertAsync(marker);
            }
            else
            {
                await Connection.UpdateAsync(marker);
            }
        }

        public async Task DeleteMarkerAsync(MarkerModel marker)
        {
            await Connection.DeleteAsync(marker);
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

        public async Task DropTilesAsync()
        {
            Connection.Open();

            using (var transaction = Connection.BeginTransaction())
            {
                await Connection.DeleteAllAsync<TileModel>(transaction);

                transaction.Commit();
            }

            Connection.Close();
        }

        public async Task SaveTilesAsync(List<TileModel> tiles)
        {
            Connection.Open();

            using (var transaction = Connection.BeginTransaction())
            {
                await Connection.InsertAsync(tiles, transaction);

                transaction.Commit();
            }

            Connection.Close();
        }

        public async Task<int> SaveTileAsync(TileModel tile)
        {
            return await Connection.InsertAsync(tile);
        }

        public async Task<int> DeleteTileAsync(TileModel tile)
        {
            return await Connection.ExecuteAsync("DELETE FROM tiles WHERE x = :x AND y = :y", new { x = tile.X, y = tile.Y });
        }

        public void DropTracks()
        {
            Connection.Open();

            using (var transaction = Connection.BeginTransaction())
            {
                Connection.DeleteAll<TrackModel>(transaction);

                transaction.Commit();
            }

            Connection.Close();
        }

        public async Task DeleteTrackAsync(TrackModel track)
        {
            await Connection.DeleteAsync(track);
        }

        public async Task SaveTrackAsync(TrackModel track)
        {
            await Task.Run(() =>
            {
                Connection.Open();

                using (var transaction = Connection.BeginTransaction())
                {
                    var id = Connection.Insert(track, transaction);

                    foreach (var trackPoint in track.TrackPoints)
                    {
                        trackPoint.TrackId = id;
                    }

                    Connection.Insert(track.TrackPoints, transaction);

                    transaction.Commit();
                }

                Connection.Close();
            });
        }
    }
}