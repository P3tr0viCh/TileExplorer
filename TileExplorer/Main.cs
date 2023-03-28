using GMap.NET;
using GMap.NET.Internals;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using P3tr0viCh;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database;

namespace TileExplorer
{
    public partial class Main : Form
    {
        readonly Database DB;

        readonly GMapOverlay tilesOverlay;
        readonly GMapOverlay imagesOverlay;
        readonly GMapOverlay markersOverlay;

        public const int TILE_ZOOM = 14;
        public const int TILE_MAX = 16384;// 2 ^ TILE_ZOOM;

        public Main()
        {
            InitializeComponent();

            DB = new Database(Path.ChangeExtension(Application.ExecutablePath, ".sqlite"));

            tilesOverlay = new GMapOverlay("tiles");
            imagesOverlay = new GMapOverlay("images");
            markersOverlay = new GMapOverlay("markers");
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

            CreateTiles();

            CreateImages();
            CreateMarkers();

            miMainImages.Checked = Settings.Default.ImagesVisible;
            miMainMarkers.Checked = Settings.Default.MarkersVisible;

            imagesOverlay.IsVisibile = miMainImages.Checked;
            markersOverlay.IsVisibile = miMainMarkers.Checked;
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

            Settings.Default.ImagesVisible = miMainImages.Checked;
            Settings.Default.MarkersVisible = miMainMarkers.Checked;

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

            gMapControl.Zoom = 11;
            gMapControl.Position = new PointLatLng(51.1977, 58.2961);

            gMapControl.MouseWheelZoomType = MouseWheelZoomType.ViewCenter;

            gMapControl.CanDragMap = true;
            gMapControl.DragButton = MouseButtons.Left;

            gMapControl.Overlays.Add(tilesOverlay);
            gMapControl.Overlays.Add(imagesOverlay);
            gMapControl.Overlays.Add(markersOverlay);

            GMapControl_OnMapZoomChanged();
            GMapControl_OnPositionChanged(gMapControl.Position);
            UpdateStatusTileId(gMapControl.Position);
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

        private void SaveToImage(string fileName)
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

        private GMapPolygon CreateTile(TileModel tile)
        {
            Color colorFill;
            Color colorStroke;

            switch (tile.Status)
            {
                case TileStatus.Visited:
                    colorFill = Color.FromArgb(25, Color.Red);
                    colorStroke = Color.FromArgb(25, Color.Red);
                    break;
                case TileStatus.Cluster:
                    colorFill = Color.FromArgb(40, Color.GreenYellow);
                    colorStroke = Color.FromArgb(80, Color.GreenYellow);
                    break;
                case TileStatus.MaxCluster:
                    colorFill = Color.FromArgb(40, Color.Green);
                    colorStroke = Color.FromArgb(80, Color.Green);
                    break;
                case TileStatus.MaxSquare:
                    colorFill = Color.FromArgb(25, Color.Blue);
                    colorStroke = Color.FromArgb(25, Color.Blue);
                    break;
                default:
                    colorFill = Color.Empty;
                    colorStroke = Color.FromArgb(100, Color.Gray);
                    break;
            }

            double lat1 = Osm.TileYToLat(tile.Y, TILE_ZOOM);
            double lng1 = Osm.TileXToLng(tile.X, TILE_ZOOM);

            double lat2 = Osm.TileYToLat(tile.Y + 1, TILE_ZOOM);
            double lng2 = Osm.TileXToLng(tile.X + 1, TILE_ZOOM);

            return new GMapPolygon(new List<PointLatLng>
            {
                new PointLatLng(lat1, lng1),
                new PointLatLng(lat1, lng2),
                new PointLatLng(lat2, lng2),
                new PointLatLng(lat2, lng1)
            }, "")
            {
                Fill = new SolidBrush(colorFill),
                Stroke = new Pen(colorStroke, 1)
            };
        }

        private void DummyTiles()
        {
            Random rand = new Random();

            var tiles = new List<TileModel>();

            for (int x = 10820; x < 10870; x++)
            {
                for (int y = 5456; y < 5484; y++)
                {
                    tiles.Add(new TileModel()
                    {
                        X = x,
                        Y = y,
                        Status = rand.Next(4) == 1 ? TileStatus.Unknown : TileStatus.Visited,
                    });
                }
            }

            DB.SaveTiles(tiles);
        }

        private void SetTileStatus(TileStatus[,] status, int[,] cluster, int x, int y, int clusterId)
        {
            if (status[x, y] != TileStatus.Cluster || cluster[x, y] != 0) return;

            cluster[x, y] = clusterId;

            SetTileStatus(status, cluster, x, y - 1, clusterId);
            SetTileStatus(status, cluster, x, y + 1, clusterId);
            SetTileStatus(status, cluster, x - 1, y, clusterId);
            SetTileStatus(status, cluster, x + 1, y, clusterId);
        }

        private void CalcTiles(List<TileModel> tiles)
        {
            int tilesCluster = 0;

            TileStatus[,] status = new TileStatus[TILE_MAX, TILE_MAX];

            foreach (TileModel tile in tiles)
            {
                status[tile.X, tile.Y] = tile.Status;
            }

            foreach (TileModel tile in tiles)
            {
                if (tile.X == 0 || tile.X == TILE_MAX - 1 || tile.Y == 0 || tile.Y == TILE_MAX - 1) continue;

                if (status[tile.X, tile.Y - 1] == TileStatus.Unknown) continue;
                if (status[tile.X, tile.Y + 1] == TileStatus.Unknown) continue;
                if (status[tile.X - 1, tile.Y] == TileStatus.Unknown) continue;
                if (status[tile.X + 1, tile.Y] == TileStatus.Unknown) continue;

                tile.Status = TileStatus.Cluster;

                tilesCluster++;

                status[tile.X, tile.Y] = tile.Status;
            }

            int[,] cluster = new int[TILE_MAX, TILE_MAX];

            int clusterId = 0;

            foreach (TileModel tile in tiles)
            {
                if (status[tile.X, tile.Y] == TileStatus.Cluster) clusterId++;

                SetTileStatus(status, cluster, tile.X, tile.Y, clusterId);
            }

            int[] maxClusters = new int[clusterId + 1];

            foreach (TileModel tile in tiles)
            {
                if (status[tile.X, tile.Y] != TileStatus.Cluster) continue;

                maxClusters[cluster[tile.X, tile.Y]]++;
            }

            int maxCluster = 0;

            for (int i = 0; i < maxClusters.Length; i++)
            {
                if (maxClusters[i] > maxCluster)
                {
                    maxCluster = maxClusters[i];
                }
            }

            foreach (TileModel tile in tiles)
            {
                if (maxClusters[cluster[tile.X, tile.Y]] == maxCluster)
                {
                    tile.Status = TileStatus.MaxCluster;
                }
            }

            foreach (TileModel tile in tiles)
            {
                break;
                if (status[tile.X, tile.Y] == TileStatus.Cluster)
                {
                    markersOverlay.Markers.Add(CreateMarker(
                        new MarkerModel()
                        {
                            Lat = Osm.TileYToLat(tile.Y, TILE_ZOOM),
                            Lng = Osm.TileXToLng(tile.X, TILE_ZOOM),
                            Text = cluster[tile.X, tile.Y].ToString() + "|" + maxClusters[cluster[tile.X, tile.Y]].ToString(),
                        }));
                }
            }

            slTilesVisited.Text = string.Format(Resources.StatusTilesVisited, tiles.Count);
            slTilesMaxCluster.Text = string.Format(Resources.StatusTilesMaxCluster, maxCluster);
        }

        private void CreateTiles()
        {
            DummyTiles();

            List<TileModel> tiles = DB.LoadTiles();

            CalcTiles(tiles);

            foreach (TileModel tile in tiles)
            {
                tilesOverlay.Polygons.Add(CreateTile(tile));
            }
        }

        private GMapMarker CreateMarker(MarkerModel marker)
        {
            GMapMarker gMapMarker = new GMarkerGoogle(new PointLatLng(marker.Lat, marker.Lng), GMarkerGoogleType.blue)
            {
                ToolTipText = marker.Text,
                ToolTipMode = MarkerTooltipMode.Always
            };

            if (gMapMarker.ToolTip != null)
            {
                gMapMarker.ToolTip.Offset.X += marker.OffsetX;
                gMapMarker.ToolTip.Offset.Y -= marker.OffsetY;
            }

            return gMapMarker;
        }

        private void CreateMarkers()
        {
            List<MarkerModel> markers = DB.LoadMarkers();

            foreach (MarkerModel marker in markers)
            {
                markersOverlay.Markers.Add(CreateMarker(marker));
            }
        }

        private GMapMarker CreateImage(ImageModel image)
        {
            return new GMarkerGoogle(new PointLatLng(image.Lat, image.Lng), image.Image);
        }

        private void CreateImages()
        {
            List<ImageModel> images = DB.LoadImages();

            foreach (ImageModel image in images)
            {
                imagesOverlay.Markers.Add(CreateImage(image));
            }
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

            Msg.Info(text);
        }

        private void UpdateStatusTileId(PointLatLng point)
        {
            slTileId.Text = string.Format(Resources.StatusTileId,
                Osm.LngToTileX(point.Lng, TILE_ZOOM), Osm.LatToTileY(point.Lat, TILE_ZOOM));
        }

        private void GMapControl_MouseClick(object sender, MouseEventArgs e)
        {
            UpdateStatusTileId(gMapControl.FromLocalToLatLng(e.X, e.Y));
        }

        private void MiMainMarkers_Click(object sender, EventArgs e)
        {
            markersOverlay.IsVisibile = miMainMarkers.Checked;
        }

        private void MiMainImages_Click(object sender, EventArgs e)
        {
            imagesOverlay.IsVisibile = miMainImages.Checked;
        }
    }
}