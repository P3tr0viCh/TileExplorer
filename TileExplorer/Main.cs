using P3tr0viCh;

using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TileExplorer.Properties;
using System.Reflection;

namespace TileExplorer
{
    public partial class Main : Form
    {
        GMapOverlay tiles;
        GMapOverlay images;
        GMapOverlay markers;

        public const int TILE_ZOOM = 14;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            var mainBounds = Settings.Default.MainBounds;

            if (mainBounds.X < 0) mainBounds.X = (Screen.FromControl(this).WorkingArea.Width - Width) / 2;
            if (mainBounds.Y < 0) mainBounds.Y = (Screen.FromControl(this).WorkingArea.Height - Height) / 2;
            if (mainBounds.Width < Width) mainBounds.Width = Width;
            if (mainBounds.Height < Height) mainBounds.Height = Height;

            Bounds = mainBounds;

            if (Settings.Default.MainMaximized)
            {
                WindowState = FormWindowState.Maximized;
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            SetFullScreen(false);

            if (WindowState == FormWindowState.Maximized)
            {
                Settings.Default.MainMaximized = true;
            }
            else
            {
                Settings.Default.MainMaximized = false;
                Settings.Default.MainBounds = Bounds;
            }

            Settings.Default.Save();
        }

        private void GMapControl_Load(object sender, EventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            //gMapControl.CacheLocation = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            GMap.NET.MapProviders.GMapProvider.UserAgent = "xxx/1.0";

            gMapControl.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            gMapControl.MapProvider.RefererUrl = "debug";

            gMapControl.ShowCenter = false;

            gMapControl.MinZoom = 2;
            gMapControl.MaxZoom = 16;
            gMapControl.Zoom = 13;
            GMapControl_OnMapZoomChanged();

            gMapControl.Position = new PointLatLng(51.1977, 58.2961);
            GMapControl_OnPositionChanged(gMapControl.Position);

            gMapControl.MouseWheelZoomType = MouseWheelZoomType.ViewCenter;

            gMapControl.CanDragMap = true;
            gMapControl.DragButton = MouseButtons.Left;

            tiles = new GMapOverlay("tiles");
            images = new GMapOverlay("images");
            markers = new GMapOverlay("markers");

            gMapControl.Overlays.Add(tiles);
            gMapControl.Overlays.Add(images);
            gMapControl.Overlays.Add(markers);

            CreateTiles();
            CreateImages();
            CreateMarkers();
        }

        private void GMapControl_OnMapZoomChanged()
        {
            slZoom.Text = string.Format(Resources.StatusZoom, gMapControl.Zoom);
        }

        private void GMapControl_OnPositionChanged(PointLatLng point)
        {
            slPosition.Text = string.Format(Resources.StatusPosition, point.Lat, point.Lng);
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveToImage(String fileName)
        {
            toolStripContainer.ContentPanel.SuspendLayout();

            gMapControl.Dock = DockStyle.None;

            var imageSize = new Size(Screen.PrimaryScreen.Bounds.Width,
                Screen.PrimaryScreen.Bounds.Height);

            try
            {
                gMapControl.SetBounds(
                    (toolStripContainer.ContentPanel.Width - imageSize.Width) / 2,
                    (toolStripContainer.ContentPanel.Height - imageSize.Height) / 2,
                    imageSize.Width,
                    imageSize.Height);

                gMapControl.ToImage().Save(fileName, ImageFormat.Png);
            }
            finally
            {
                gMapControl.SetBounds(0, 0,
                    toolStripContainer.ContentPanel.Width, toolStripContainer.ContentPanel.Height);

                gMapControl.Dock = DockStyle.Fill;

                toolStripContainer.ContentPanel.PerformLayout();
            }
        }

        private void SaveToImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveToImage(saveFileDialog.FileName);
            }
        }

        public enum TileType
        {
            None,
            Visited,
            Cluster,
            MaxSquare
        }

        private GMapPolygon CreateTile(TileType type, PointLatLng point1, PointLatLng point2)
        {
            Color colorFill;
            Color colorStroke;

            switch (type)
            {
                case TileType.Visited:
                    colorFill = Color.FromArgb(50, Color.Red);
                    colorStroke = Color.FromArgb(80, Color.Red);
                    break;
                case TileType.Cluster:
                    colorFill = Color.FromArgb(50, Color.Green);
                    colorStroke = Color.FromArgb(80, Color.Green);
                    break;
                case TileType.MaxSquare:
                    colorFill = Color.FromArgb(50, Color.Blue);
                    colorStroke = Color.FromArgb(80, Color.Blue);
                    break;
                default:
                    colorFill = Color.Empty;
                    colorStroke = Color.FromArgb(100, Color.Gray);
                    break;
            }

            return new GMapPolygon(new List<PointLatLng>
            {
                new PointLatLng(point1.Lat, point1.Lng),
                new PointLatLng(point1.Lat, point2.Lng),
                new PointLatLng(point2.Lat, point2.Lng),
                new PointLatLng(point2.Lat, point1.Lng)
            }, "")
            {
                Fill = new SolidBrush(colorFill),
                Stroke = new Pen(colorStroke, 1)
            };
        }

        private PointLatLng TileToPoint(int tileX, int tileY)
        {
            return new PointLatLng(Osm.TileYToLat(tileY, TILE_ZOOM), Osm.TileXToLng(tileX, TILE_ZOOM));
        }

        private void CreateTiles()
        {
            int tileX;
            int tileY;

            tileX = 10844;
            tileY = 5470;
            tiles.Polygons.Add(CreateTile(TileType.None, TileToPoint(tileX, tileY), TileToPoint(tileX + 1, tileY + 1)));

            tileX = 10845;
            tileY = 5470;
            tiles.Polygons.Add(CreateTile(TileType.Visited, TileToPoint(tileX, tileY), TileToPoint(tileX + 1, tileY + 1)));

            tileX = 10844;
            tileY = 5471;
            tiles.Polygons.Add(CreateTile(TileType.Cluster, TileToPoint(tileX, tileY), TileToPoint(tileX + 1, tileY + 1)));

            tileX = 10843;
            tileY = 5471;
            tiles.Polygons.Add(CreateTile(TileType.MaxSquare, TileToPoint(tileX, tileY), TileToPoint(tileX + 1, tileY + 1)));
        }

        private GMapMarker CreateMarker(PointLatLng point, String Text, Point offset)
        {
            GMapMarker gMapMarker = new GMarkerGoogle(point, GMarkerGoogleType.blue)
            {
                ToolTipText = Text,
                ToolTipMode = MarkerTooltipMode.Always
            };

            gMapMarker.ToolTip.Offset.X += offset.X;
            gMapMarker.ToolTip.Offset.Y -= offset.Y;

            return gMapMarker;
        }

        private void CreateMarkers()
        {
            markers.Markers.Add(CreateMarker(new PointLatLng(51.1977, 58.2961), "Яяяя", new Point(0, 0)));
            markers.Markers.Add(CreateMarker(new PointLatLng(51.1977, 58.2981), "XXX", new Point(50, 10)));
        }

        private GMapMarker CreateImage(PointLatLng point, String fileName)
        {
            return new GMarkerGoogle(point, new Bitmap(fileName));
        }

        private void CreateImages()
        {
            images.Markers.Add(CreateImage(new PointLatLng(51.2169, 58.2446), $@"d:\!Temp\logo.png"));
            images.Markers.Add(CreateImage(new PointLatLng(51.2132, 58.3297), $@"d:\!Temp\bic.png"));
        }

        Rectangle FullScreenBounds;

        private void SetFullScreen(bool value)
        {
            if (miMainFullScreen.Checked == value) return;

            miMainFullScreen.Checked = value;

            TopMost = value;

            if (value)
            {
                FormBorderStyle = FormBorderStyle.None;

                toolStripContainer.TopToolStripPanel.Visible = false;
                toolStripContainer.BottomToolStripPanel.Visible = false;

                FullScreenBounds = Bounds;

                SetBounds(0, 0, Screen.FromControl(this).Bounds.Width, Screen.FromControl(this).Bounds.Height);
            }
            else
            {
                FormBorderStyle = FormBorderStyle.Sizable;

                toolStripContainer.TopToolStripPanel.Visible = true;
                toolStripContainer.BottomToolStripPanel.Visible = true;

                Bounds = FullScreenBounds;
            }
        }

        private void MiMainFullScreen_Click(object sender, EventArgs e)
        {
            SetFullScreen(!miMainFullScreen.Checked);
        }

        private void MiMainAbout_Click(object sender, EventArgs e)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var assemblyTitle = (AssemblyTitleAttribute)assembly.GetCustomAttribute(typeof(AssemblyTitleAttribute));
            var assemblyVersion = (AssemblyFileVersionAttribute)assembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute));
            var assemblyCopyright = (AssemblyCopyrightAttribute)assembly.GetCustomAttribute(typeof(AssemblyCopyrightAttribute));

            var text = assemblyTitle.Title + Environment.NewLine +
                assemblyVersion.Version + Environment.NewLine +
                assemblyCopyright.Copyright;

            MessageBox.Show(text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void gMapControl_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            MessageBox.Show(item.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}