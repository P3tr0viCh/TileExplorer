//#define CHECK_TILES

using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using P3tr0viCh;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database;
using static TileExplorer.StatusStripPresenter;

namespace TileExplorer
{
    public partial class Main : Form, IStatusStripView
    {
        readonly Database DB;

        readonly GMapOverlay tilesOverlay;
        readonly GMapOverlay markersOverlay;

        private readonly StatusStripPresenter statusStripPresenter;

        public ToolStripStatusLabel StatusLabelZoom => slZoom;
        public ToolStripStatusLabel StatusLabelTileId => slTileId;
        public ToolStripStatusLabel StatusLabelPosition => slPosition;
        public ToolStripStatusLabel StatusLabelMousePosition => slMousePosition;
        public ToolStripStatusLabel StatusLabelTilesVisited => slTilesVisited;
        public ToolStripStatusLabel StatusLabelTilesMaxCluster => slTilesMaxCluster;
        public ToolStripStatusLabel StatusLabelTilesMaxSquare => slTilesMaxSquare;

        public Main()
        {
            InitializeComponent();

            DB = new Database(Path.ChangeExtension(Application.ExecutablePath, ".sqlite"));

            tilesOverlay = new GMapOverlay("tiles");

            markersOverlay = new GMapOverlay("markers");

            statusStripPresenter = new StatusStripPresenter(this);
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

            slEmpty.Text = string.Empty;

            CreateTiles();

            CreateMarkers();

            miMainMarkers.Checked = Settings.Default.MarkersVisible;

            markersOverlay.IsVisibile = miMainMarkers.Checked;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            FullScreen = false;

            if (WindowState == FormWindowState.Maximized)
            {
                Settings.Default.MainMaximized = true;
            }
            else
            {
                Settings.Default.MainMaximized = false;
                Settings.Default.MainBounds = Bounds;
            }

            Settings.Default.MarkersVisible = miMainMarkers.Checked;

            Settings.Default.Save();
        }

        private void GMapControl_Load(object sender, EventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

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
            gMapControl.Overlays.Add(markersOverlay);

            GMapControl_OnMapZoomChanged();
            GMapControl_OnPositionChanged(gMapControl.Position);

            statusStripPresenter.TileId = gMapControl.Position;
            statusStripPresenter.MousePosition = gMapControl.Position;
        }

        private void GMapControl_OnMapZoomChanged()
        {
            statusStripPresenter.Zoom = gMapControl.Zoom;
        }

        private void GMapControl_OnPositionChanged(PointLatLng point)
        {
            statusStripPresenter.Position = point;
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

        private GMapPolygon TileFromModel(TileModel tile)
        {
            Color colorFill;
            Color colorStroke;

            switch (tile.Status)
            {
                case TileStatus.Visited:
                    colorFill = Color.FromArgb(
                        Settings.Default.ColorTileVisitedAlpha, Settings.Default.ColorTileVisited);
                    colorStroke = Color.FromArgb(
                        Settings.Default.ColorTileVisitedLineAlpha, Settings.Default.ColorTileVisited);
                    break;
                case TileStatus.Cluster:
                    colorFill = Color.FromArgb(
                        Settings.Default.ColorTileClusterAlpha, Settings.Default.ColorTileCluster);
                    colorStroke = Color.FromArgb(
                        Settings.Default.ColorTileClusterLineAlpha, Settings.Default.ColorTileCluster);
                    break;
                case TileStatus.MaxCluster:
                    colorFill = Color.FromArgb(
                        Settings.Default.ColorTileMaxClusterAlpha, Settings.Default.ColorTileMaxCluster);
                    colorStroke = Color.FromArgb(
                        Settings.Default.ColorTileMaxClusterLineAlpha, Settings.Default.ColorTileMaxCluster);
                    break;
                case TileStatus.MaxSquare:
                    colorFill = Color.FromArgb(
                        Settings.Default.ColorTileMaxSquareAlpha, Settings.Default.ColorTileMaxSquare);
                    colorStroke = Color.FromArgb(
                        Settings.Default.ColorTileMaxSquareLineAlpha, Settings.Default.ColorTileMaxSquare);
                    break;
                default:
                    colorFill = Color.Empty;
                    colorStroke = Color.FromArgb(100, Color.Gray);
                    break;
            }

            double lat1 = Osm.TileYToLat(tile.Y, Const.TILE_ZOOM);
            double lng1 = Osm.TileXToLng(tile.X, Const.TILE_ZOOM);

            double lat2 = Osm.TileYToLat(tile.Y + 1, Const.TILE_ZOOM);
            double lng2 = Osm.TileXToLng(tile.X + 1, Const.TILE_ZOOM);

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

#if DEBUG
        private List<TileModel> DummyTiles()
        {
            Random rand = new Random();

            var tiles = new List<TileModel>();

            for (int x = 10820; x < 10870; x++)
            {
                for (int y = 5456; y < 5485; y++)
                {
                    if (rand.Next(12) > 1)
                    {
                        tiles.Add(new TileModel()
                        {
                            X = x,
                            Y = y,
                            Status = TileStatus.Visited,
                        });
                    }
                }
            }

            DB.DropTiles();

            DB.SaveTiles(tiles);

            return tiles;
        }
#endif

        private void CreateTiles()
        {
            List<TileModel> tiles =
#if DEBUG
            DummyTiles();
#else
            DB.LoadTiles();
            foreach (var tile in tiles)
            {
                tile.Status = TileStatus.Visited;
            }
#endif

            Utils.CalcResult calcResult = Utils.CalcTiles(tiles);

            statusStripPresenter.TilesVisited = calcResult.Visited;
            statusStripPresenter.TilesMaxCluster = calcResult.MaxCluster;
            statusStripPresenter.TilesMaxSquare = calcResult.MaxSquare;

            foreach (TileModel tile in tiles)
            {
                tilesOverlay.Polygons.Add(TileFromModel(tile));

#if DEBUG && CHECK_TILES
                if (tile.Status != TileStatus.Unknown && tile.Text != string.Empty)
                {
                    markersOverlay.Markers.Add(new MapMarker(new MarkerModel()
                    {
                        Lat = Osm.TileYToLat(tile.Y, Const.TILE_ZOOM),
                        Lng = Osm.TileXToLng(tile.X, Const.TILE_ZOOM),
                        Text = tile.Text,
                        IsTextVisible = true
                    }));
                }
#endif
            }
        }

        private void CreateMarkers()
        {
            var markers = DB.LoadMarkers();

            foreach (var marker in markers)
            {
                markersOverlay.Markers.Add(new MapMarker(marker));
            }
        }

        private Rectangle savedScreenBounds;

        private bool fullScreen;

        private bool FullScreen
        {
            get
            {
                return fullScreen;
            }
            set
            {
                if (fullScreen == value) return;

                fullScreen = value;

                TopMost = value;

                gMapControl.Visible = false;

                if (value)
                {
                    savedScreenBounds = Bounds;

                    FormBorderStyle = FormBorderStyle.None;

                    toolStripContainer.TopToolStripPanel.Visible = false;
                    toolStripContainer.BottomToolStripPanel.Visible = false;

                    SetBounds(0, 0, Screen.FromControl(this).Bounds.Width, Screen.FromControl(this).Bounds.Height);
                }
                else
                {
                    FormBorderStyle = FormBorderStyle.Sizable;

                    toolStripContainer.TopToolStripPanel.Visible = true;
                    toolStripContainer.BottomToolStripPanel.Visible = true;

                    Bounds = savedScreenBounds;
                }

                gMapControl.Visible = true;
            }
        }

        private void MiMainFullScreen_Click(object sender, EventArgs e)
        {
            FullScreen = !FullScreen;
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

        private bool MarkerMoving;

        private MapMarker SelectedMarker;

        private void GMapControl_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (MarkerMoving)
                    {
                        MarkerMoving = false;

                        if (SelectedMarker != null)
                        {
                            MarkerMove(SelectedMarker);

                            SelectedMarker = null;
                        }
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

        private void MarkerAdd(PointLatLng point)
        {
            bool prevMarkersVisible = markersOverlay.IsVisibile;

            markersOverlay.IsVisibile = true;

            MarkerModel markerModel = new MarkerModel()
            {
                Lat = point.Lat,
                Lng = point.Lng,

#if DEBUG
                Text = DateTime.Now.ToString(),
#endif
            };

            GMapMarker markerTemp = new GMarkerCross(point)
            {
                Pen = new Pen(Brushes.Red, 2f)
            };

            markersOverlay.Markers.Add(markerTemp);

            if (FrmMarker.Show(this, markerModel))
            {
                DB.SaveMarker(markerModel);

                markersOverlay.Markers.Remove(markerTemp);

                markersOverlay.Markers.Add(new MapMarker(markerModel));
            }
            else
            {
                markersOverlay.Markers.Remove(markerTemp);
            }

            markersOverlay.IsVisibile = prevMarkersVisible;
        }

        private void MarkerChange(MapMarker marker)
        {
            if (FrmMarker.Show(this, marker.MarkerModel))
            {
                DB.SaveMarker(marker.MarkerModel);

                marker.NotifyModelChanged();
            }
        }

        private void MarkerRemove(MapMarker marker)
        {
            DB.DeleteMarker(marker.MarkerModel);

            markersOverlay.Markers.Remove(marker);
        }

        private void MarkerMove(MapMarker marker)
        {
            DB.SaveMarker(marker.MarkerModel);
        }

        private void GMapControl_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            MarkerMoving = false;

            SelectedMarker = (MapMarker)item;

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

        private void MiMarkerChange_Click(object sender, EventArgs e)
        {
            if (SelectedMarker == null) return;

            MarkerChange(SelectedMarker);
        }

        private void MiMarkerDelete_Click(object sender, EventArgs e)
        {
            if (SelectedMarker == null) return;

            string Name = SelectedMarker.MarkerModel.Text;

            if (Name == "")
            {
                Name = SelectedMarker.Position.Lat.ToString() + ":" + SelectedMarker.Position.Lng.ToString();
            }

            if (Msg.Question(string.Format(Resources.QuestionMarkerDelete, Name)))
            {
                MarkerRemove(SelectedMarker);
            }
        }

        PointLatLng SelectedMarkerPosition;

        private void MiMarkerMove_Click(object sender, EventArgs e)
        {
            if (SelectedMarker == null) return;

            MarkerMoving = true;

            SelectedMarkerPosition = SelectedMarker.Position;

            GPoint gPoint = gMapControl.FromLatLngToLocal(SelectedMarker.Position);

            Cursor.Position = gMapControl.PointToScreen(new Point((int)gPoint.X, (int)gPoint.Y));
        }

        private void GMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            PointLatLng position = gMapControl.FromLocalToLatLng(e.X, e.Y);

            statusStripPresenter.TileId = position;
            statusStripPresenter.MousePosition = position;

            if (!MarkerMoving) return;

            if (SelectedMarker == null) return;

            SelectedMarker.MarkerModel.Lat = position.Lat;
            SelectedMarker.MarkerModel.Lng = position.Lng;
            SelectedMarker.NotifyModelChanged();
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (MarkerMoving)
                {
                    if (SelectedMarker != null)
                    {
                        SelectedMarker.MarkerModel.Lat = SelectedMarkerPosition.Lat;
                        SelectedMarker.MarkerModel.Lng = SelectedMarkerPosition.Lng;
                        SelectedMarker.NotifyModelChanged();
                    }

                    MarkerMoving = false;
                }
                else
                {
                    if (FullScreen)
                    {
                        FullScreen = false;
                    }
                }
            }
        }
    }
}