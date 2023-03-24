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

        public struct TileModel
        {
            public int Id;

            public int X;
            public int Y;

            public double Lat;
            public double Lng;
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

            commandText = "CREATE TABLE IF NOT EXISTS tiles (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, x INTEGER NOT NULL, y INTEGER NOT NULL, lat REAL NOT NULL, lng REAL NOT NULL)";
            command = new SQLiteCommand(commandText, Connection);
            command.ExecuteNonQuery();

            commandText = "CREATE INDEX IF NOT EXISTS index_tiles ON tiles (x, y)";
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

        public TileModel LoadTile(int x, int y)
        {
            var tile = new TileModel
            {
                Id = -1,
                X = x,
                Y = y
            };

            Connection.Open();

            string commandText = "SELECT * FROM tiles WHERE x = :x AND y = :y;";

            SQLiteCommand command = new SQLiteCommand(commandText, Connection);

            command.Parameters.AddWithValue("x", x);
            command.Parameters.AddWithValue("y", y);

            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);

            DataTable table = new DataTable();

            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                tile.Id = Convert.ToInt32(table.Rows[0]["id"]);

                tile.Lat = Convert.ToDouble(table.Rows[0]["lat"]);
                tile.Lng = Convert.ToDouble(table.Rows[0]["lng"]);
            }

            Connection.Close();

            return tile;
        }

        public void SaveTile(TileModel tile)
        {
            Connection.Open();

            string commandText = "INSERT INTO tiles(x, y, lat, lng) VALUES (:x, :y, :lat, :lng);";

            SQLiteCommand command = new SQLiteCommand(commandText, Connection);

            command.Parameters.AddWithValue("x", tile.X);
            command.Parameters.AddWithValue("y", tile.Y);
            command.Parameters.AddWithValue("lat", tile.Lat);
            command.Parameters.AddWithValue("lng", tile.Lng);

            command.ExecuteNonQuery();

            Connection.Close();
        }
    }
}