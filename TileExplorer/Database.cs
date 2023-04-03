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
            public int Id { get; set; }

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
            public int Id { get; set; }

            public int X { get; set; }
            public int Y { get; set; }

            [Write(false)]
            [Computed]
            public TileStatus Status { get; set; } = TileStatus.Unknown;

#if DEBUG
            [Write(false)]
            [Computed]
            public string Text { get; set; } = string.Empty;
#endif
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
                    "lat REAL NOT NULL, lng REAL NOT NULL, " +
                    "text TEXT, istextvisible INTEGER, " +
                    "offsetx INTEGER, offsety INTEGER, image BLOB, imagetype INTEGER DEFAULT 0);");

                Connection.Execute("CREATE TABLE IF NOT EXISTS tiles (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    "x INTEGER NOT NULL, y INTEGER NOT NULL, status INTEGER DEFAULT 0, " +
                    "UNIQUE(x, y));");
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
            }
            else
            {
                Connection.Update<MarkerModel>(marker);
            }
        }

        public void DeleteMarker(MarkerModel marker)
        {
            Connection.Delete(marker);
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

        public List<TileModel> LoadTiles()
        {
            return Connection.GetAll<TileModel>().ToList();
        }

        public void DropTiles()
        {
            Connection.Open();

            using (var transaction = Connection.BeginTransaction())
            {
                Connection.DeleteAll<TileModel>(transaction);

                transaction.Commit();
            }

            Connection.Close();
        }

        public void SaveTiles(List<TileModel> tiles)
        {
            Connection.Open();

            using (var transaction = Connection.BeginTransaction())
            {
                Connection.Insert(tiles, transaction);

                transaction.Commit();
            }

            Connection.Close();
        }
    }
}