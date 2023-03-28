using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using GMap.NET;
using System.Drawing;
using System.Data.Entity.Infrastructure;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using P3tr0viCh;
using GMap.NET.Internals;

namespace TileExplorer
{
    internal class Database
    {
        private readonly string FileName;

        private readonly SQLiteConnection Connection;

        public struct MarkerModel
        {
            public int Id;

            public double Lat;
            public double Lng;

            public string Text;

            public int OffsetX;
            public int OffsetY;
        }

        public struct ImageModel
        {
            public int Id;

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

            string commandText;
            SQLiteCommand command;

            Connection.Open();

            commandText = "CREATE TABLE IF NOT EXISTS markers (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, lat REAL NOT NULL, lng REAL NOT NULL, text TEXT, offset_x INTEGER, offset_y INTEGER)";
            command = new SQLiteCommand(commandText, Connection);
            command.ExecuteNonQuery();

            commandText = "CREATE TABLE IF NOT EXISTS images (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, lat REAL NOT NULL, lng REAL NOT NULL, name TEXT, image BLOB NOT NULL)";
            command = new SQLiteCommand(commandText, Connection);
            command.ExecuteNonQuery();

            commandText = "CREATE TABLE IF NOT EXISTS tiles (x INTEGER NOT NULL, y INTEGER NOT NULL, lat REAL NOT NULL, lng REAL NOT NULL, status INTEGER DEFAULT 0, PRIMARY KEY(x, y))";
            command = new SQLiteCommand(commandText, Connection);
            command.ExecuteNonQuery();

            Connection.Close();
        }

        public List<MarkerModel> LoadMarkers()
        {
            var list = new List<MarkerModel>();

            Connection.Open();

            string commandText = "SELECT * FROM markers;";

            SQLiteDataAdapter adapter = new SQLiteDataAdapter(commandText, Connection);

            DataTable table = new DataTable();

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

            Connection.Close();

            return list;
        }

        public List<ImageModel> LoadImages()
        {
            var list = new List<ImageModel>();

            Connection.Open();

            string commandText = "SELECT * FROM images;";

            SQLiteDataAdapter adapter = new SQLiteDataAdapter(commandText, Connection);

            DataTable table = new DataTable();

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

            Connection.Close();

            return list;
        }

        public bool LoadTile(TileModel tile)
        {
            bool result = false;

            Connection.Open();

            string commandText = "SELECT * FROM tiles WHERE x = :x AND y = :y;";

            SQLiteCommand command = new SQLiteCommand(commandText, Connection);

            command.Parameters.AddWithValue("x", tile.X);
            command.Parameters.AddWithValue("y", tile.Y);

            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);

            DataTable table = new DataTable();

            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                tile.Status = (TileStatus)Convert.ToInt32(table.Rows[0]["status"]);

                result = true;
            }

            Connection.Close();

            return result;
        }

        public List<TileModel> LoadTiles()
        {
            var tiles = new List<TileModel>();

            Connection.Open();
            string commandText = "SELECT x, y, status FROM tiles WHERE STATUS > 0 ORDER BY x, y;";

            SQLiteDataAdapter adapter = new SQLiteDataAdapter(commandText, Connection);

            DataTable table = new DataTable();

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

            Connection.Close();

            return tiles;
        }

        public void SaveTile(TileModel tile)
        {
            Connection.Open();

            string commandText = "INSERT OR REPLACE INTO tiles(x, y, status) VALUES (:x, :y, :status);";

            SQLiteCommand command = new SQLiteCommand(commandText, Connection);

            command.Parameters.AddWithValue("x", tile.X);
            command.Parameters.AddWithValue("y", tile.Y);

            command.Parameters.AddWithValue("status", (int)tile.Status);

            command.ExecuteNonQuery();

            Connection.Close();
        }

        public void SaveTiles(List<TileModel> tiles)
        {
            Connection.Open();

            using (var transaction = Connection.BeginTransaction())
            {
                string commandText = "INSERT OR REPLACE INTO tiles(x, y, status) VALUES (:x, :y, :status);";

                SQLiteCommand command = new SQLiteCommand(commandText, Connection);

                foreach (TileModel tile in tiles)
                {
                    command.Parameters.AddWithValue("x", tile.X);
                    command.Parameters.AddWithValue("y", tile.Y);

                    command.Parameters.AddWithValue("status", (int)tile.Status);

                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }

            Connection.Close();
        }
    }
}