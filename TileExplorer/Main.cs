using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Newtonsoft.Json.Linq;
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

            Settings.Default.ImagesVisible = miMainImages.Checked;
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
                tilesOverlay.Polygons.Add(TileFromModel(tile));
            }
        }

        private enum MarkerType
        {
            Marker,
            Image
        }

        private abstract class MapMarkerBase : GMarkerGoogle
        {
            private readonly MarkerType markerType;

            public MapMarkerBase(MarkerType markerType) : base(new PointLatLng(), GMarkerGoogleType.blue)
            {
                this.markerType = markerType;
            }

            public MapMarkerBase(MarkerType markerType, Bitmap bitmap) : base(new PointLatLng(), bitmap)
            {
                this.markerType = markerType;
            }

            abstract public void NotifyModelUpdate();

            public MarkerType MarkerType { get { return markerType; } }
        }

        private class MapMarker : MapMarkerBase
        {
            public MapMarker(MarkerModel markerModel) : base(MarkerType.Marker)
            {
                ToolTipMode = MarkerTooltipMode.Always;

                ToolTip = new GMapToolTipSquare(this)
                {
                    Font = Settings.Default.FontMarker,

                    TextPadding = new Size(4, 4),

                    Foreground = new SolidBrush(Color.FromArgb(
                        Settings.Default.ColorMarkerTextAlpha, Settings.Default.ColorMarkerText)),
                    Stroke = new Pen(Color.FromArgb(
                        Settings.Default.ColorMarkerLineAlpha, Settings.Default.ColorMarkerLine)),
                    Fill = new SolidBrush(Color.FromArgb(
                        Settings.Default.ColorMarkerFillAlpha, Settings.Default.ColorMarkerFill))
                };

                ToolTip.Stroke.Width = 1f;

                ToolTip.Format.LineAlignment = StringAlignment.Center;
                ToolTip.Format.Alignment = StringAlignment.Center;

                MarkerModel = markerModel;
            }

            private readonly MarkerModel markerModel = new MarkerModel();

            public MarkerModel MarkerModel
            {
                get
                {
                    return markerModel;
                }
                set
                {
                    markerModel.Id = value.Id;

                    markerModel.Text = value.Text;

                    markerModel.Lat = value.Lat;
                    markerModel.Lng = value.Lng;

                    markerModel.OffsetX = value.OffsetX;
                    markerModel.OffsetY = value.OffsetY;

                    ToolTipText = markerModel.Text;

                    Position = new PointLatLng(markerModel.Lat, markerModel.Lng);

                    ToolTip.Offset.X = markerModel.OffsetX;
                    ToolTip.Offset.Y = markerModel.OffsetY;
                }
            }

            public override void NotifyModelUpdate()
            {
                markerModel.Lat = Position.Lat;
                markerModel.Lng = Position.Lng;

                markerModel.Text = ToolTipText;

                markerModel.OffsetX = ToolTip.Offset.X;
                markerModel.OffsetY = ToolTip.Offset.Y;
            }
        }

        private class MapMarkerImage : MapMarkerBase
        {
            public MapMarkerImage(ImageModel imageModel) : base(MarkerType.Image, imageModel.Image)
            {
                ImageModel = imageModel;
            }

            private readonly ImageModel imageModel = new ImageModel();

            public ImageModel ImageModel
            {
                get
                {
                    return imageModel;
                }
                set
                {
                    imageModel.Id = value.Id;

                    imageModel.Lat = value.Lat;
                    imageModel.Lng = value.Lng;

                    imageModel.Name = value.Name;

                    imageModel.Image = value.Image;

                    Position = new PointLatLng(imageModel.Lat, imageModel.Lng);
                }
            }

            public override void NotifyModelUpdate()
            {
                imageModel.Lat = Position.Lat;
                imageModel.Lng = Position.Lng;
            }
        }

        private void CreateMarkers()
        {
            List<MarkerModel> markers = DB.LoadMarkers();

            foreach (MarkerModel marker in markers)
            {
                markersOverlay.Markers.Add(new MapMarker(marker));
            }
        }

        private void CreateImages()
        {
            List<ImageModel> images = DB.LoadImages();

            foreach (ImageModel image in images)
            {
                imagesOverlay.Markers.Add(new MapMarkerImage(image));
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

        private void UpdateStatusTileId(PointLatLng point)
        {
            slTileId.Text = string.Format(Resources.StatusTileId,
                Osm.LngToTileX(point.Lng, TILE_ZOOM), Osm.LatToTileY(point.Lat, TILE_ZOOM));
        }

        private bool MarkerMoving;

        private MapMarkerBase SelectedMarker;

        private void GMapControl_MouseClick(object sender, MouseEventArgs e)
        {
            UpdateStatusTileId(gMapControl.FromLocalToLatLng(e.X, e.Y));

            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (MarkerMoving)
                    {
                        MarkerMoving = false;

                        if (SelectedMarker != null)
                        {
                            switch (SelectedMarker.MarkerType)
                            {
                                case MarkerType.Marker:
                                    MarkerMove((MapMarker)SelectedMarker);
                                    break;
                                case MarkerType.Image:
                                    MarkerImageMove((MapMarkerImage)SelectedMarker);
                                    break;
                            }

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

        private void MiMainImages_Click(object sender, EventArgs e)
        {
            imagesOverlay.IsVisibile = miMainImages.Checked;
        }

        private bool ShowFrmMarker(MarkerModel markerModel)
        {
            bool Result;

            using (var frmMarker = new FrmMarker())
            {
                frmMarker.MarkerText = markerModel.Text;

                frmMarker.MarkerLat = markerModel.Lat;
                frmMarker.MarkerLng = markerModel.Lng;

                frmMarker.OffsetX = markerModel.OffsetX;
                frmMarker.OffsetY = markerModel.OffsetY;

                Result = frmMarker.ShowDialog(this) == DialogResult.OK;

                if (Result)
                {
                    markerModel.Text = frmMarker.MarkerText;

                    markerModel.Lat = frmMarker.MarkerLat;
                    markerModel.Lng = frmMarker.MarkerLng;

                    markerModel.OffsetX = frmMarker.OffsetX;
                    markerModel.OffsetY = frmMarker.OffsetY;

                    DB.SaveMarker(markerModel);
                }
            }

            return Result;
        }

        private void MarkerAdd(PointLatLng point)
        {
            MarkerModel markerModel = new MarkerModel()
            {
                Lat = point.Lat,
                Lng = point.Lng,

#if DEBUG
                Text = DateTime.Now.ToString(),
#endif
            };

            GMapMarker markerTemp = new GMarkerGoogle(point, GMarkerGoogleType.red);

            markersOverlay.Markers.Add(markerTemp);

            if (ShowFrmMarker(markerModel))
            {
                markersOverlay.Markers.Remove(markerTemp);

                markersOverlay.Markers.Add(new MapMarker(markerModel));
            }
            else
            {
                markersOverlay.Markers.Remove(markerTemp);
            }
        }

        private void MarkerChange(MapMarker marker)
        {
            MarkerModel markerModel = marker.MarkerModel;

            if (ShowFrmMarker(markerModel))
            {
                marker.MarkerModel = markerModel;
            }
        }

        private void MarkerRemove(MapMarker marker)
        {
            DB.DeleteMarker(marker.MarkerModel);

            markersOverlay.Markers.Remove(marker);
        }
        
        private void ImageRemove(MapMarkerImage image)
        {
            DB.DeleteImage(image.ImageModel);

            imagesOverlay.Markers.Remove(image);
        }

        private void MarkerMove(MapMarker marker)
        {
            DB.SaveMarker(marker.MarkerModel);
        }

        private void MarkerImageMove(MapMarkerImage image)
        {
            DB.SaveImage(image.ImageModel);
        }

        private void GMapControl_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            MarkerMoving = false;

            SelectedMarker = (MapMarkerBase)item;

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

            if (SelectedMarker.MarkerType == MarkerType.Marker)
            {
                MarkerChange((MapMarker)SelectedMarker);
            }
        }

        private void MiMarkerDelete_Click(object sender, EventArgs e)
        {
            if (SelectedMarker == null) return;

            string Question = null;
            string Name = null;

            switch (SelectedMarker.MarkerType)
            {
                case MarkerType.Marker:
                    Question = Resources.QuestionMarkerDelete;
                    Name = ((MapMarker)SelectedMarker).MarkerModel.Text;
                    
                    break;
                case MarkerType.Image:
                    Question = Resources.QuestionImageDelete;
                    Name = ((MapMarkerImage)SelectedMarker).ImageModel.Name;

                    break;
            }
            
            if (Name == "")
            {
                Name = SelectedMarker.Position.Lat.ToString() + ":" + SelectedMarker.Position.Lng.ToString(); 
            }

            if (Msg.Question(string.Format(Question, Name)))
            {
                switch (SelectedMarker.MarkerType)
                {
                    case MarkerType.Marker:
                        MarkerRemove((MapMarker)SelectedMarker);

                        break;
                    case MarkerType.Image:
                        ImageRemove((MapMarkerImage)SelectedMarker);

                        break;
                }
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
            if (!MarkerMoving) return;

            if (SelectedMarker == null) return;

            SelectedMarker.Position = gMapControl.FromLocalToLatLng(e.X, e.Y);
            SelectedMarker.NotifyModelUpdate();
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (MarkerMoving)
                {
                    if (SelectedMarker != null)
                    {
                        SelectedMarker.Position = SelectedMarkerPosition;
                        SelectedMarker.NotifyModelUpdate();
                    }

                    MarkerMoving = false;
                }
                else
                {
                    if (miMainFullScreen.Checked)
                    {
                        FullScreen = false;
                    }
                }
            }
        }
    }
}