using GMap.NET.Internals;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace TileExplorer
{
    internal class Database
    {
        private readonly string FileName;

        private readonly SQLiteConnection Connection;

        public class MarkerModel
        {
            public long Id;

            public double Lat;
            public double Lng;

            public string Text;

            public int OffsetX;
            public int OffsetY;
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

        public class TileModel
        {
            public int X;
            public int Y;

            public TileStatus Status;
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
            }

            Connection.Open();

            using (var command = new SQLiteCommand(Connection))
            {
                string commandText;

                commandText = "CREATE TABLE IF NOT EXISTS markers (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, lat REAL NOT NULL, lng REAL NOT NULL, text TEXT, offset_x INTEGER, offset_y INTEGER)";
                command.CommandText = commandText;
                command.ExecuteNonQuery();

                commandText = "CREATE TABLE IF NOT EXISTS images (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, lat REAL NOT NULL, lng REAL NOT NULL, name TEXT, image BLOB NOT NULL)";
                command.CommandText = commandText;
                command.ExecuteNonQuery();

                commandText = "CREATE TABLE IF NOT EXISTS tiles (x INTEGER NOT NULL, y INTEGER NOT NULL, status INTEGER DEFAULT 0, PRIMARY KEY(x, y))";
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }

            Connection.Close();
        }

        public List<MarkerModel> LoadMarkers()
        {
            var list = new List<MarkerModel>();

            Connection.Open();

            string commandText = "SELECT * FROM markers;";

            using (var adapter = new SQLiteDataAdapter(commandText, Connection))
            using (var table = new DataTable())
            {
                adapter.Fill(table);

                foreach (DataRow row in table.Rows)
                {
                    list.Add(new MarkerModel
                    {
                        Id = Convert.ToInt32(row["id"]),

                        Lat = Convert.ToDouble(row["lat"]),
                        Lng = Convert.ToDouble(row["lng"]),

                        Text = row["text"].GetType() != typeof(DBNull) ? Convert.ToString(row["text"]) : "",

                        OffsetX = row["offset_x"].GetType() != typeof(DBNull) ? Convert.ToInt32(row["offset_x"]) : 0,
                        OffsetY = row["offset_y"].GetType() != typeof(DBNull) ? Convert.ToInt32(row["offset_y"]) : 0
                    });
                }
            }

            Connection.Close();

            return list;
        }

        public void SaveMarker(MarkerModel marker)
        {
            Connection.Open();

            string commandText;

            if (marker.Id == 0)
            {
                commandText = "INSERT INTO markers(lat, lng, text, offset_x, offset_y)" +
                    " VALUES (:lat, :lng, :text, :offset_x, :offset_y);";
            }
            else
            {
                commandText = "INSERT OR REPLACE INTO markers(id, lat, lng, text, offset_x, offset_y)" +
                    " VALUES (:id, :lat, :lng, :text, :offset_x, :offset_y);";
            }

            using (var command = new SQLiteCommand(commandText, Connection))
            {
                if (marker.Id != 0)
                {
                    command.Parameters.AddWithValue("id", marker.Id);
                }

                command.Parameters.AddWithValue("lat", marker.Lat);
                command.Parameters.AddWithValue("lng", marker.Lng);

                command.Parameters.AddWithValue("text", marker.Text);

                command.Parameters.AddWithValue("offset_x", marker.OffsetX);
                command.Parameters.AddWithValue("offset_y", marker.OffsetY);

                command.ExecuteNonQuery();

                if (marker.Id == 0)
                {
                    marker.Id = Connection.LastInsertRowId;
                }
            }

            Connection.Close();
        }

        public void DeleteMarker(MarkerModel marker)
        {
            Connection.Open();

            string commandText = "DELETE FROM markers WHERE id = :id;";

            using (var command = new SQLiteCommand(commandText, Connection))
            {
                command.Parameters.AddWithValue("id", marker.Id);

                command.ExecuteNonQuery();
            }

            Connection.Close();
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
            var tiles = new List<TileModel>();

            Connection.Open();
            string commandText = "SELECT x, y, status FROM tiles WHERE STATUS > 0 ORDER BY x, y;";

            using (var adapter = new SQLiteDataAdapter(commandText, Connection))
            using (var table = new DataTable())
            {
                adapter.Fill(table);

                foreach (DataRow row in table.Rows)
                {
                    tiles.Add(new TileModel
                    {
                        X = Convert.ToInt32(row["x"]),
                        Y = Convert.ToInt32(row["y"]),

                        Status = (TileStatus)Convert.ToInt32(table.Rows[0]["status"])
                    });
                }
            }

            Connection.Close();

            return tiles;
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
                string commandText = "INSERT OR REPLACE INTO tiles(x, y, status) VALUES (:x, :y, :status);";

                using (var command = new SQLiteCommand(commandText, Connection))
                {
                    foreach (TileModel tile in tiles)
                    {
                        command.Parameters.AddWithValue("x", tile.X);
                        command.Parameters.AddWithValue("y", tile.Y);

                        command.Parameters.AddWithValue("status", (int)tile.Status);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }

            Connection.Close();
        }
    }
}