//#define CHECK_TILES
//#define SHOW_TRACK_KM

using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using P3tr0viCh;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database;
using static TileExplorer.StatusStripPresenter;

namespace TileExplorer
{
    public partial class Main : Form, IStatusStripView
    {
        private readonly Database DB;

        private readonly GMapOverlay tilesOverlay = new GMapOverlay("tiles");
        private readonly GMapOverlay tracksOverlay = new GMapOverlay("tracks");
        private readonly GMapOverlay markersOverlay = new GMapOverlay("markers");

        private readonly StatusStripPresenter statusStripPresenter;

        public ToolStripStatusLabel StatusLabelZoom => slZoom;
        public ToolStripStatusLabel StatusLabelTileId => slTileId;
        public ToolStripStatusLabel StatusLabelPosition => slPosition;
        public ToolStripStatusLabel StatusLabelMousePosition => slMousePosition;
        public ToolStripStatusLabel StatusLabelStatus => slStatus;
        public ToolStripStatusLabel StatusLabelTilesVisited => slTilesVisited;
        public ToolStripStatusLabel StatusLabelTilesMaxCluster => slTilesMaxCluster;
        public ToolStripStatusLabel StatusLabelTilesMaxSquare => slTilesMaxSquare;

        public Main()
        {
            InitializeComponent();

            DB = new Database(Path.ChangeExtension(Application.ExecutablePath, ".sqlite"));

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

#if DEBUG
            DummyTiles();
#endif           

            DataUpdateAsync();

            miMainShowTracks.Checked = Settings.Default.MapTrackVisible;
            miMainShowMarkers.Checked = Settings.Default.MapMarkersVisible;

            tracksOverlay.IsVisibile = miMainShowTracks.Checked;
            markersOverlay.IsVisibile = miMainShowMarkers.Checked;
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

            Settings.Default.MapTrackVisible = miMainShowTracks.Checked;
            Settings.Default.MapMarkersVisible = miMainShowMarkers.Checked;

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

            gMapControl.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;

            gMapControl.CanDragMap = true;
            gMapControl.DragButton = MouseButtons.Left;

            gMapControl.Overlays.Add(tilesOverlay);
            gMapControl.Overlays.Add(tracksOverlay);
            gMapControl.Overlays.Add(markersOverlay);

            statusStripPresenter.Zoom = gMapControl.Zoom;
            statusStripPresenter.Position = gMapControl.Position;
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

#if DEBUG
        private void DummyTiles()
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
                        });
                    }
                }
            }

            DB.DropTiles();

            DB.SaveTiles(tiles);
        }
#endif

        private async Task LoadTilesAsync()
        {
            tilesOverlay.Clear();

            var tiles = await DB.LoadTilesAsync();

            foreach (var tile in tiles)
            {
                tile.Status = TileStatus.Visited;
            }

            Utils.CalcResult calcResult = Utils.CalcTiles(tiles);

            foreach (TileModel tile in tiles)
            {
                tilesOverlay.Polygons.Add(new MapTile(tile));

#if DEBUG && CHECK_TILES
                if (tile.Status > TileStatus.Visited)
                {
                    markersOverlay.Markers.Add(new MapMarker(new MarkerModel()
                    {
                        Lat = Osm.TileYToLat(tile.Y, Const.TILE_ZOOM),
                        Lng = Osm.TileXToLng(tile.X, Const.TILE_ZOOM),
                        Text = tile.ClusterId.ToString(),
                        IsTextVisible = true
                    }));
                }
#endif
            }

            statusStripPresenter.TilesVisited = calcResult.Visited;
            statusStripPresenter.TilesMaxCluster = calcResult.MaxCluster;
            statusStripPresenter.TilesMaxSquare = calcResult.MaxSquare;
        }

        private async Task LoadTracksAsync()
        {
            tracksOverlay.Clear();

            var tracks = await DB.LoadTracksAsync();

            foreach (var track in tracks)
            {
                tracksOverlay.Routes.Add(new MapTrack(track));

#if DEBUG && SHOW_TRACK_KM
                double lat1 = 0, lng1 = 0, lat2 = 0, lng2 = 0;

                int i = 0;

                foreach (var trackPoint in track.TrackPoints)
                {
                    lat2 = trackPoint.Lat;
                    lng2 = trackPoint.Lng;

                    if (Geo.Haversine(lat1, lng1, lat2, lng2) < 1000) continue;

                    tracksOverlay.Markers.Add(new MapMarker(new MarkerModel()
                    {
                        Lat = trackPoint.Lat,
                        Lng = trackPoint.Lng,
                        Text = i.ToString(),
                        IsTextVisible = true
                    }));

                    i++;

                    Debug.WriteLine(Geo.Haversine(lat1, lng1, lat2, lng2));
                    
                    lat1 = lat2;
                    lng1 = lng2;
                }
#endif
            }
        }

        private async Task LoadMarkersAsync()
        {
            markersOverlay.Clear();

            var markers = await DB.LoadMarkersAsync();

            foreach (var marker in markers)
            {
                markersOverlay.Markers.Add(new MapMarker(marker));
            }
        }

        public enum ProgramStatus
        {
            Idle,
            LoadData,
            LoadGpx
        }

        public ProgramStatus Status
        {
            set
            {
                switch (value)
                {
                    case ProgramStatus.Idle:
                        statusStripPresenter.Status = string.Empty;
                        break;
                    case ProgramStatus.LoadData:
                        statusStripPresenter.Status = Resources.ProgramStatusLoadData;
                        break;
                    case ProgramStatus.LoadGpx:
                        statusStripPresenter.Status = Resources.ProgramStatusLoadGpx;
                        break;
                }
            }
        }

        private async void DataUpdateAsync()
        {
            Status = ProgramStatus.LoadData;

            await LoadTilesAsync();

            await LoadTracksAsync();

            await LoadMarkersAsync();

            Status = ProgramStatus.Idle;
        }

        private FormWindowState savedWindowState;

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
                    toolStripContainer.TopToolStripPanel.Visible = false;
                    toolStripContainer.BottomToolStripPanel.Visible = false;

                    savedWindowState = WindowState;

                    WindowState = FormWindowState.Normal;
                    
                    FormBorderStyle = FormBorderStyle.None;
                    
                    WindowState = FormWindowState.Maximized;
                }
                else
                {
                    FormBorderStyle = FormBorderStyle.Sizable;

                    WindowState = savedWindowState;

                    toolStripContainer.TopToolStripPanel.Visible = true;
                    toolStripContainer.BottomToolStripPanel.Visible = true;
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
                        }
                    }

                    break;
                case MouseButtons.Right:
                    cmMap.Show(gMapControl, e.Location);

                    break;
            }

            SelectedMarker = null;
        }

        private void MiMainShowMarkers_Click(object sender, EventArgs e)
        {
            markersOverlay.IsVisibile = miMainShowMarkers.Checked;
        }

        private void MiMainShowTracks_Click(object sender, EventArgs e)
        {
            tracksOverlay.IsVisibile = miMainShowTracks.Checked;
        }

        private async void MarkerAdd(PointLatLng point)
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

            if (FrmMarker.ShowDlg(this, markerModel))
            {
                await DB.SaveMarkerAsync(markerModel);

                markersOverlay.Markers.Remove(markerTemp);

                markersOverlay.Markers.Add(new MapMarker(markerModel));
            }
            else
            {
                markersOverlay.Markers.Remove(markerTemp);
            }

            markersOverlay.IsVisibile = prevMarkersVisible;
        }

        private async void MarkerChange(MapMarker marker)
        {
            if (FrmMarker.ShowDlg(this, marker.MarkerModel))
            {
                await DB.SaveMarkerAsync(marker.MarkerModel);

                marker.NotifyModelChanged();
            }
        }

        private async void MarkerRemove(MapMarker marker)
        {
            await DB.DeleteMarkerAsync(marker.MarkerModel);

            markersOverlay.Markers.Remove(marker);
        }

        private async void MarkerMove(MapMarker marker)
        {
            await DB.SaveMarkerAsync(marker.MarkerModel);
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
                    FullScreen = false;
                }
            }
        }

        private void MiMainDataUpdate_Click(object sender, EventArgs e)
        {
            DataUpdateAsync();
        }

        private void AddTileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PointLatLng position = gMapControl.FromLocalToLatLng(MenuPopupPoint.X, MenuPopupPoint.Y);

            try
            {
                DB.SaveTile(new TileModel()
                {
                    X = Osm.LngToTileX(position.Lng, Const.TILE_ZOOM),
                    Y = Osm.LatToTileY(position.Lat, Const.TILE_ZOOM)
                });

                DataUpdateAsync();
            }
            catch (Exception)
            {
                Msg.Error("tile exists");
            }
        }

        private void DeleteTileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PointLatLng position = gMapControl.FromLocalToLatLng(MenuPopupPoint.X, MenuPopupPoint.Y);

            DB.DeleteTile(new TileModel()
            {
                X = Osm.LngToTileX(position.Lng, Const.TILE_ZOOM),
                Y = Osm.LatToTileY(position.Lat, Const.TILE_ZOOM)
            });

            DataUpdateAsync();
        }

        private void MiMainMapDesign_Click(object sender, EventArgs e)
        {
            if (FrmMapDesign.ShowDlg(this))
            {
                markersOverlay.IsVisibile = false;

                foreach (var marker in markersOverlay.Markers)
                {
                    marker.ToolTip.Font = Settings.Default.FontMarker;
                    marker.ToolTip.Foreground = new SolidBrush(Color.FromArgb(
                        Settings.Default.ColorMarkerTextAlpha, Settings.Default.ColorMarkerText));
                }

                markersOverlay.IsVisibile = true;
            }
        }

        private void GMapControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (SelectedMarker == null)
                {
                    MarkerAdd(gMapControl.FromLocalToLatLng(e.X, e.Y));
                }
                else
                {
                    MarkerChange(SelectedMarker);
                }
            }
        }

        private void GMapControl_OnRouteClick(GMapRoute item, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SelectedMarker = null;

                var track = ((MapTrack)item).TrackModel;

                Msg.Info(track.Text + "\n" + track.DateTime + "\n" +
                    track.TrackPoints.Count + "\n" + item.Points.Count);
            }
        }

        private async Task<TrackModel> OpenTrackFromFileAsync(string path)
        {
            return await Task.Run(() => { return Utils.OpenTrackFromFile(path); });
        }

        private async void OpenTrackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Status = ProgramStatus.LoadGpx;

                foreach (var file in openFileDialog.FileNames)
                {
                    var track = await OpenTrackFromFileAsync(file);

                    await DB.SaveTrackAsync(track);
                }

                Status = ProgramStatus.Idle;

                DataUpdateAsync();
            }
        }
    }
}