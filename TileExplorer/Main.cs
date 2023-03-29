using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using P3tr0viCh;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        public class GMapToolTipSquare : GMapToolTip
        {
            private const int MARKER_OFFSET_X = 20;
            private const int MARKER_OFFSET_Y = -30;

            public GMapToolTipSquare(GMapMarker marker) : base(marker)
            {
            }

            public override void OnRender(Graphics g)
            {
                Size size = g.MeasureString(Marker.ToolTipText, Font).ToSize();

                checked
                {
                    Rectangle rectangle = new Rectangle(Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y - size.Height,
                        size.Width + TextPadding.Width, size.Height + TextPadding.Height);

                    rectangle.Offset(MARKER_OFFSET_X + Offset.X, MARKER_OFFSET_Y - Offset.Y);

                    g.DrawLine(Stroke, Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y,
                        rectangle.X, rectangle.Y + unchecked(rectangle.Height / 2));

                    g.FillRectangle(Fill, rectangle);

                    g.DrawRectangle(Stroke, rectangle);

                    g.DrawString(Marker.ToolTipText, Font, Foreground, rectangle, Format);
                }
            }
        }

        private GMapMarker MarkerFromModel(MarkerModel markerModel)
        {
            GMapMarker mapMarker = new GMarkerGoogle(new PointLatLng(), GMarkerGoogleType.blue)
            {
                Tag = markerModel.Id,

                Position = new PointLatLng(markerModel.Lat, markerModel.Lng),

                ToolTipText = markerModel.Text,
                ToolTipMode = MarkerTooltipMode.Always
            };

            if (mapMarker.ToolTipText.Length != 0)
            {
                mapMarker.ToolTip = new GMapToolTipSquare(mapMarker)
                {
                    Font = gMapControl.Font,

                    TextPadding = new Size(4, 4),

                    Foreground = new SolidBrush(Color.Red),
                    Stroke = new Pen(Color.FromArgb(140, Color.Green)),
                    Fill = new SolidBrush(Color.FromArgb(222, Color.Yellow))
                };

                mapMarker.ToolTip.Stroke.Width = 1f;

                mapMarker.ToolTip.Format.LineAlignment = StringAlignment.Center;
                mapMarker.ToolTip.Format.Alignment = StringAlignment.Center;

                mapMarker.ToolTip.Offset.X = markerModel.OffsetX;
                mapMarker.ToolTip.Offset.Y = markerModel.OffsetY;
            }

            return mapMarker;
        }

        private MarkerModel MarkerToModel(GMapMarker mapMarker)
        {
            MarkerModel markerModel = new MarkerModel()
            {
                Id = (long)mapMarker.Tag,

                Lat = mapMarker.Position.Lat,
                Lng = mapMarker.Position.Lng,

                Text = mapMarker.ToolTipText
            };

            if (mapMarker.ToolTip != null)
            {
                markerModel.OffsetX = mapMarker.ToolTip.Offset.X;
                markerModel.OffsetY = mapMarker.ToolTip.Offset.Y;
            }
            else
            {
                markerModel.OffsetX = 0;
                markerModel.OffsetY = 0;
            }

            return markerModel;
        }

        private void CreateMarkers()
        {
            List<MarkerModel> markers = DB.LoadMarkers();

            foreach (MarkerModel marker in markers)
            {
                markersOverlay.Markers.Add(MarkerFromModel(marker));
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

        private bool MarkerMoving;

        private void GMapControl_MouseClick(object sender, MouseEventArgs e)
        {
            UpdateStatusTileId(gMapControl.FromLocalToLatLng(e.X, e.Y));

            switch (e.Button)
            {
                case MouseButtons.Left:
                    MarkerMoving = false;

                    if (SelectedMarker != null)
                    {
                        MarkerMove(SelectedMarker);

                        SelectedMarker = null;
                    }

                    break;
                case MouseButtons.Right:
                    cmMap.Show(gMapControl, e.Location);

                    break;
            }
        }

        private void MiMainMarkers_Click(object sender, EventArgs e)
        {
            markersOverlay.IsVisibile = miMainMarkers.Checked;
        }

        private void MiMainImages_Click(object sender, EventArgs e)
        {
            imagesOverlay.IsVisibile = miMainImages.Checked;
        }

        GMapMarker SelectedMarker;

        private void MarkerAdd(PointLatLng point)
        {
            MarkerModel markerModel = new MarkerModel()
            {
                Lat = point.Lat,
                Lng = point.Lng,
            };

            GMapMarker markerTemp = MarkerFromModel(markerModel);

            markersOverlay.Markers.Add(markerTemp);

            if (Msg.Question("add marker?"))
            {
                markerModel.Text = DateTime.Now.ToString();

                DB.SaveMarker(markerModel);

                markersOverlay.Markers.Remove(markerTemp);

                markersOverlay.Markers.Add(MarkerFromModel(markerModel));
            }
            else
            {
                markersOverlay.Markers.Remove(markerTemp);
            }
        }

        private void MarkerRemove(GMapMarker marker)
        {
            DB.DeleteMarker(MarkerToModel(marker));

            markersOverlay.Markers.Remove(marker);
        }

        private void MarkerMove(GMapMarker marker)
        {
            DB.SaveMarker(MarkerToModel(marker));
        }

        private void GMapControl_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            MarkerMoving = false;

            SelectedMarker = item;

            if (e.Button == MouseButtons.Right)
            {
                cmMarker.Show(gMapControl, e.Location);
            }
        }

        Point MenuPopupPoint;

        private void CmMap_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MenuPopupPoint = gMapControl.PointToClient(MousePosition);
        }

        private void MiMapMarkerAdd_Click(object sender, EventArgs e)
        {
            MarkerAdd(gMapControl.FromLocalToLatLng(MenuPopupPoint.X, MenuPopupPoint.Y));
        }

        private void MiMarkerDelete_Click(object sender, EventArgs e)
        {
            if (SelectedMarker == null)
            {
                return;
            }

            if (Msg.Question(string.Format(Resources.QuestionMarkerDelete, SelectedMarker.ToolTipText)))
            {
                MarkerRemove(SelectedMarker);
            }
        }

        PointLatLng SelectedMarkerPosistion;

        private void MiMarkerMove_Click(object sender, EventArgs e)
        {
            if (SelectedMarker == null) return;

            MarkerMoving = true;

            SelectedMarkerPosistion = SelectedMarker.Position;

            GPoint gPoint = gMapControl.FromLatLngToLocal(SelectedMarker.Position);

            Cursor.Position = gMapControl.PointToScreen(new Point((int)gPoint.X, (int)gPoint.Y));
        }

        private void GMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!MarkerMoving) return;

            if (SelectedMarker == null) return;

            SelectedMarker.Position = gMapControl.FromLocalToLatLng(e.X, e.Y);
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (MarkerMoving)
                {
                    if (SelectedMarker != null)
                    {
                        SelectedMarker.Position = SelectedMarkerPosistion;
                    }

                    MarkerMoving = false;

                }
                else
                {
                    if (miMainFullScreen.Checked)
                    {
                        SetFullScreen(false);
                    }
                }
            }
        }
    }
}