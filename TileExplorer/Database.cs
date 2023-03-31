using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace TileExplorer
{
    internal class Database
    {
        private readonly string FileName;

        private readonly SQLiteConnection Connection;

        [Table("markers")]
        public class MarkerModel
        {
            [Key]
            public int Id { get; set; }

            public double Lat { get; set; }
            public double Lng { get; set; }

            public string Text { get; set; }

            public int OffsetX { get; set; }
            public int OffsetY { get; set; }
        }

        public class ImageModel
        {
            public long Id;

            public double Lat;
            public double Lng;

            public string Name;

            public Bitmap Image;
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
            public int X { get; set; }
            [Key]
            public int Y { get; set; }

            public TileStatus Status { get; set; }
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

                Connection.Execute("CREATE TABLE IF NOT EXISTS markers (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    "lat REAL NOT NULL, lng REAL NOT NULL, text TEXT, " +
                    "offsetx INTEGER, offsety INTEGER);");
                Connection.Execute("CREATE TABLE IF NOT EXISTS images (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    "lat REAL NOT NULL, lng REAL NOT NULL, name TEXT, image BLOB NOT NULL);");
                Connection.Execute("CREATE TABLE IF NOT EXISTS tiles (" +
                    "x INTEGER NOT NULL, y INTEGER NOT NULL, status INTEGER DEFAULT 0, PRIMARY KEY(x, y));");
            }
        }

        public List<MarkerModel> LoadMarkers()
        {
            return Connection.GetAll<MarkerModel>().ToList();
        }

        public void SaveMarker(MarkerModel marker)
        {
            if (marker.Id == 0)
            {
                Connection.Insert(marker);
            } else
            {
                Connection.Update<MarkerModel>(marker);
            }
        }

        public void DeleteMarker(MarkerModel marker)
        {
            Connection.Delete(marker);
        }

        public void SaveImage(ImageModel image)
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

        public bool LoadTile(TileModel tile)
        {
            bool result = false;

            Connection.Open();

            string commandText = "SELECT * FROM tiles WHERE x = :x AND y = :y;";

            using (var command = new SQLiteCommand(commandText, Connection))
            {
                command.Parameters.AddWithValue("x", tile.X);
                command.Parameters.AddWithValue("y", tile.Y);

                using (var adapter = new SQLiteDataAdapter(command))
                using (var table = new DataTable())
                {
                    adapter.Fill(table);

                    if (table.Rows.Count > 0)
                    {
                        tile.Status = (TileStatus)Convert.ToInt32(table.Rows[0]["status"]);

                        result = true;
                    }
                }
            }

            Connection.Close();

            return result;
        }

        public List<TileModel> LoadTiles()
        {
            return Connection.Query<TileModel>("SELECT * FROM tiles WHERE STATUS > 0 ORDER BY x, y;").ToList();
        }

        public void SaveTile(TileModel tile)
        {
            Connection.Open();

            string commandText = "INSERT OR REPLACE INTO tiles(x, y, status) VALUES (:x, :y, :status);";

            using (var command = new SQLiteCommand(commandText, Connection))
            {
                command.Parameters.AddWithValue("x", tile.X);
                command.Parameters.AddWithValue("y", tile.Y);

                command.Parameters.AddWithValue("status", (int)tile.Status);

                command.ExecuteNonQuery();
            }

            Connection.Close();
        }

        public void SaveTiles(List<TileModel> tiles)
        {
            Connection.Open();

            using (var transaction = Connection.BeginTransaction())
            {
                Connection.Execute("INSERT OR REPLACE INTO tiles(x, y, status) VALUES (:x, :y, :status);",
                    tiles, transaction);

                transaction.Commit();
            }

            Connection.Close();
        }
    }
}