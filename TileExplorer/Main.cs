//#define CHECK_TILES
//#define SHOW_TRACK_KM
//#define DUMMY_TILES

using Bluegrams.Application;
using GMap.NET;
using GMap.NET.Internals;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using P3tr0viCh;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database;
using static TileExplorer.Main;
using static TileExplorer.StatusStripPresenter;

namespace TileExplorer
{
    public partial class Main : Form, IStatusStripView, IMainForm
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

        private readonly FrmTrackList frmTrackList;
        private readonly FrmMarkerList frmMarkerList;

        public Main()
        {
            InitializeComponent();

            PortableSettingsProvider.SettingsFileName = Files.SettingsFileName();
            PortableSettingsProviderBase.SettingsDirectory = Files.SettingsDirectory();
            Directory.CreateDirectory(PortableSettingsProviderBase.SettingsDirectory);
            PortableSettingsProvider.ApplyProvider(Settings.Default);

            DB = new Database(Files.DatabaseFileName());

            statusStripPresenter = new StatusStripPresenter(this);
            
            frmTrackList = new FrmTrackList(this);
            frmMarkerList = new FrmMarkerList(this);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            SettingsExt.Default.LoadFormBounds(this);

            SettingsExt.Default.LoadFormBounds(frmTrackList);
            SettingsExt.Default.LoadFormBounds(frmMarkerList);

#if DEBUG && DUMMY_TILES
            DummyTiles();
#endif           

            DataUpdateAsync();

            miMainShowTracks.Checked = Settings.Default.VisibleTracks;
            miMainShowMarkers.Checked = Settings.Default.VisibleMarkers;

            miMainGrayScale.Checked = Settings.Default.MapGrayScale;
            gMapControl.GrayScaleMode = miMainGrayScale.Checked;

            tracksOverlay.IsVisibile = miMainShowTracks.Checked;
            markersOverlay.IsVisibile = miMainShowMarkers.Checked;

            frmTrackList.Visible = Settings.Default.VisibleListTracks;
            frmMarkerList.Visible = Settings.Default.VisibleListMarkers;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            FullScreen = false;

            SettingsExt.Default.SaveFormBounds(this);

            SettingsExt.Default.SaveFormBounds(frmTrackList);
            SettingsExt.Default.SaveFormBounds(frmMarkerList);

            Settings.Default.VisibleTracks = miMainShowTracks.Checked;
            Settings.Default.VisibleMarkers = miMainShowMarkers.Checked;

            Settings.Default.MapGrayScale = miMainGrayScale.Checked;

            Settings.Default.VisibleListTracks = frmTrackList.Visible;
            Settings.Default.VisibleListMarkers = frmMarkerList.Visible;

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

            HomeGoto();

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

#if DEBUG && DUMMY_TILES
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

            DropTilesAsync();

            SaveTilesAsync(tiles);
        }
#endif

#if DEBUG && DUMMY_TILES
       private async void DropTilesAsync()
        {
            await DB.DropTilesAsync();
        }
#endif

#if DEBUG && DUMMY_TILES
        private async Task SaveTilesAsync(List<TileModel> tiles)
        {
            await DB.SaveTilesAsync(tiles);
        }
#endif

        private async Task CalcTilesAsync(List<TileModel> tiles)
        {
            foreach (var tile in tiles)
            {
                tile.Status = TileStatus.Visited;
            }

            Utils.CalcResult calcResult = await Task.Run(() =>
            {
                return Utils.CalcTiles(tiles);
            });

            statusStripPresenter.TilesVisited = calcResult.Visited;
            statusStripPresenter.TilesMaxCluster = calcResult.MaxCluster;
            statusStripPresenter.TilesMaxSquare = calcResult.MaxSquare;
        }

        private async Task LoadTilesAsync()
        {
            tilesOverlay.Clear();

            var tiles = await DB.LoadTilesAsync();

            await CalcTilesAsync(tiles);

            foreach (var tile in tiles)
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

            frmTrackList.List = tracks;
        }

        private async Task LoadMarkersAsync()
        {
            markersOverlay.Clear();

            var markers = await DB.LoadMarkersAsync();

            foreach (var marker in markers)
            {
                markersOverlay.Markers.Add(new MapMarker(marker));
            }

            frmMarkerList.List = markers;
        }

        public enum ProgramStatus
        {
            Idle,
            LoadData,
            LoadGpx,
            SaveData
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
                    case ProgramStatus.SaveData:
                        statusStripPresenter.Status = Resources.ProgramStatusSaveData;
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

        private bool childFormsTopMost = true;
        public bool ChildFormsTopMost
        {
            get
            {
                return childFormsTopMost;
            }
            set
            {
                if (childFormsTopMost == value) return;

                childFormsTopMost = value;

                frmTrackList.TopMost = value;
                frmMarkerList.TopMost = value;
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

            UtilsFiles.Info(text);
        }

        private bool MarkerMoving;

        private MapTrack selectedTrack = null;

        public MapTrack SelectedTrack
        {
            get
            {
                return selectedTrack;
            }
            set
            {
                if (selectedTrack == value) return;

                if (selectedTrack != null)
                {
                    selectedTrack.Selected = false;
                }

                selectedTrack = value;

                if (selectedTrack != null)
                {
                    selectedTrack.Selected = true;

                    frmTrackList.Selected = selectedTrack.Track;
                }
            }
        }

        private MapMarker selectedMarker = null;

        public MapMarker SelectedMarker
        {
            get
            {
                return selectedMarker;
            }
            set
            {
                if (selectedMarker == value) return;

                if (selectedMarker != null)
                {
                    selectedMarker.Selected = false;
                }

                selectedMarker = value;

                if (selectedMarker != null)
                {
                    selectedMarker.Selected = true;

                    frmMarkerList.Selected = selectedMarker.Marker;
                }
            }
        }

        private async void GMapControl_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (MarkerMoving)
                    {
                        MarkerMoving = false;

                        if (SelectedMarker != null)
                        {
                            await DB.SaveMarkerAsync(SelectedMarker.Marker);

                            frmMarkerList.Update(SelectedMarker.Marker);
                        }
                    }

                    break;
                case MouseButtons.Right:
                    cmMap.Show(gMapControl, e.Location);

                    break;
            }

            SelectedTrack = null;
            SelectedMarker = null;
        }

        private void GMapControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (SelectedMarker == null)
                {
                    if (SelectedTrack == null)
                    {
                        MarkerAddAsync(gMapControl.FromLocalToLatLng(e.X, e.Y));
                    }
                    else
                    {
                        var track = SelectedTrack.Track;

                        UtilsFiles.Info(track.Text + "\n" + track.DateTime + "\n" +
                            track.TrackPoints.Count + "\n" + SelectedTrack.Points.Count + "\n" +
                            track.Distance / 1000.0 + "\n" + SelectedTrack.Distance);
                    }
                }
                else
                {
                    MarkerChangeAsync(SelectedMarker);
                }
            }
        }

        private void GMapControl_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            MarkerMoving = false;

            SelectedTrack = null;

            SelectedMarker = (MapMarker)item;

            switch (e.Button)
            {
                case MouseButtons.Left:

                    break;
                case MouseButtons.Right:
                    cmMarker.Show(gMapControl, e.Location);

                    break;
            }
        }

        private void GMapControl_OnRouteClick(GMapRoute item, MouseEventArgs e)
        {
            MarkerMoving = false;

            SelectedMarker = null;

            SelectedTrack = (MapTrack)item;

            switch (e.Button)
            {
                case MouseButtons.Left:

                    break;
                case MouseButtons.Right:
                    cmTrack.Show(gMapControl, e.Location);

                    break;
            }
        }

        private void MiMainShowMarkers_Click(object sender, EventArgs e)
        {
            markersOverlay.IsVisibile = miMainShowMarkers.Checked;
        }

        private void MiMainShowTracks_Click(object sender, EventArgs e)
        {
            tracksOverlay.IsVisibile = miMainShowTracks.Checked;
        }

        private void HomeGoto()
        {
            gMapControl.Zoom = Settings.Default.HomeZoom;
            gMapControl.Position = new PointLatLng(Settings.Default.HomeLat, Settings.Default.HomeLng);
        }

        private void HomeSave()
        {
            Settings.Default.HomeZoom = (int)gMapControl.Zoom;
            Settings.Default.HomeLat = gMapControl.Position.Lat;
            Settings.Default.HomeLng = gMapControl.Position.Lng;
        }

        private void MiMainHomeGoto_Click(object sender, EventArgs e)
        {
            HomeGoto();
        }

        private void MiMainHomeSave_Click(object sender, EventArgs e)
        {
            HomeSave();
        }

        private async void MarkerAddAsync(PointLatLng point)
        {
            ChildFormsTopMost = false;

            bool prevMarkersVisible = markersOverlay.IsVisibile;

            markersOverlay.IsVisibile = true;

            MarkerModel marker = new MarkerModel()
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

            if (FrmMarker.ShowDlg(this, marker))
            {
                await DB.SaveMarkerAsync(marker);

                markersOverlay.Markers.Remove(markerTemp);

                markersOverlay.Markers.Add(new MapMarker(marker));

                frmMarkerList.Add(marker);
            }
            else
            {
                markersOverlay.Markers.Remove(markerTemp);
            }

            markersOverlay.IsVisibile = prevMarkersVisible;

            ChildFormsTopMost = true;
        }

        private async void MarkerChangeAsync(MapMarker marker)
        {
            if (marker == null) return;

            ChildFormsTopMost = false;

            if (FrmMarker.ShowDlg(this, marker.Marker))
            {
                await DB.SaveMarkerAsync(marker.Marker);

                marker.NotifyModelChanged();

                frmMarkerList.Update(marker.Marker);

                if (ActiveForm != this) gMapControl.Invalidate();
            }

            ChildFormsTopMost = true;
        }

        private async void MarkerDeleteAsync(MapMarker marker)
        {
            if (marker == null) return;

            string Name = marker.Marker.Text;

            if (Name == "")
            {
                Name = marker.Position.Lat.ToString() + ":" + marker.Position.Lng.ToString();
            }

            ChildFormsTopMost = false;

            if (UtilsFiles.Question(string.Format(Resources.QuestionMarkerDelete, Name)))
            {
                await DB.DeleteMarkerAsync(marker.Marker);

                markersOverlay.Markers.Remove(marker);

                frmMarkerList.Delete(marker.Marker);

                if (ActiveForm != this) gMapControl.Invalidate();
            }

            ChildFormsTopMost = true;
        }

        private async void TrackChangeAsync(MapTrack track)
        {
            if (track == null) return;

            ChildFormsTopMost = false;

            if (FrmTrack.ShowDlg(this, track.Track))
            {
                //await DB.SaveMarkerAsync(marker.MarkerModel);

                track.NotifyModelChanged();

                frmTrackList.Update(track.Track);

                if (ActiveForm != this) gMapControl.Invalidate();
            }

            ChildFormsTopMost = true;
        }

        Point MenuPopupPoint;

        private void CmMap_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MenuPopupPoint = gMapControl.PointToClient(MousePosition);
        }

        private void MiMapMarkerAdd_Click(object sender, EventArgs e)
        {
            MarkerAddAsync(gMapControl.FromLocalToLatLng(MenuPopupPoint.X, MenuPopupPoint.Y));
        }

        private void MiMarkerChange_Click(object sender, EventArgs e)
        {
            MarkerChangeAsync(SelectedMarker);
        }

        private void MiMarkerDelete_Click(object sender, EventArgs e)
        {
            MarkerDeleteAsync(SelectedMarker);
        }

        private async void MiTrackDelete_Click(object sender, EventArgs e)
        {
            if (SelectedTrack == null) return;

            string Name = SelectedTrack.Track.Text;

            if (Name == "")
            {
                Name = SelectedTrack.Track.DateTime.ToString();
            }

            ChildFormsTopMost = false;

            if (UtilsFiles.Question(string.Format(Resources.QuestionTrackDelete, Name)))
            {
                await DB.DeleteTrackAsync(SelectedTrack.Track);

                tracksOverlay.Routes.Remove(SelectedTrack);

                frmTrackList.Delete(SelectedTrack.Track);

                await LoadTilesAsync();
            }

            ChildFormsTopMost = true;
        }

        PointLatLng MarkerMovingPrevPosition;

        private void MiMarkerMove_Click(object sender, EventArgs e)
        {
            if (SelectedMarker == null) return;

            MarkerMoving = true;

            MarkerMovingPrevPosition = SelectedMarker.Position;

            GPoint point = gMapControl.FromLatLngToLocal(SelectedMarker.Position);

            Cursor.Position = gMapControl.PointToScreen(new Point((int)point.X, (int)point.Y));
        }

        private void GMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            PointLatLng position = gMapControl.FromLocalToLatLng(e.X, e.Y);

            statusStripPresenter.TileId = position;
            statusStripPresenter.MousePosition = position;

            if (!MarkerMoving) return;

            if (SelectedMarker == null) return;

            SelectedMarker.Marker.Lat = position.Lat;
            SelectedMarker.Marker.Lng = position.Lng;
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
                        SelectedMarker.Marker.Lat = MarkerMovingPrevPosition.Lat;
                        SelectedMarker.Marker.Lng = MarkerMovingPrevPosition.Lng;
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

        private async void AddTileAsync(TileModel tile)
        {
            try
            {
                Status = ProgramStatus.SaveData;

                await DB.SaveTileAsync(tile);

                Status = ProgramStatus.LoadData;

                await LoadTilesAsync();
            }
            finally
            {
                Status = ProgramStatus.Idle;
            }
        }

        private void AddTileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PointLatLng position = gMapControl.FromLocalToLatLng(MenuPopupPoint.X, MenuPopupPoint.Y);

            try
            {
                AddTileAsync(new TileModel()
                {
                    X = Osm.LngToTileX(position.Lng, Const.TILE_ZOOM),
                    Y = Osm.LatToTileY(position.Lat, Const.TILE_ZOOM)
                });
            }
            catch (Exception)
            {
                UtilsFiles.Error("tile exists");
            }
        }

        private async void DeleteTileAsync(TileModel tile)
        {
            try
            {
                Status = ProgramStatus.SaveData;

                await DB.DeleteTileAsync(tile);

                Status = ProgramStatus.LoadData;

                await LoadTilesAsync();
            }
            finally
            {
                Status = ProgramStatus.Idle;
            }
        }

        private void DeleteTileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PointLatLng position = gMapControl.FromLocalToLatLng(MenuPopupPoint.X, MenuPopupPoint.Y);

            DeleteTileAsync(new TileModel()
            {
                X = Osm.LngToTileX(position.Lng, Const.TILE_ZOOM),
                Y = Osm.LatToTileY(position.Lat, Const.TILE_ZOOM)
            });
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

        private void MiMainSaveToImage_Click(object sender, EventArgs e)
        {
            ChildFormsTopMost = false;

            bool save = saveFileDialog.ShowDialog() == DialogResult.OK;

            ChildFormsTopMost = true;

            if (save)
            {
                SaveToImage(saveFileDialog.FileName);
            }
        }

        private async Task<TrackModel> OpenTrackFromFileAsync(string path)
        {
            return await Task.Run(() => { return Utils.OpenTrackFromFile(path); });
        }

        private async Task<List<TileModel>> GetTilesFromTrackAsync(TrackModel track)
        {
            return await Task.Run(() => { return Utils.GetTilesFromTrack(track); });
        }

        private async Task OpenTracksAsync(string[] files)
        {
            TrackModel track;

            List<TileModel> trackTiles;

            var tiles = await DB.LoadTilesAsync();

            foreach (var file in files)
            {
                track = await OpenTrackFromFileAsync(file);

                await DB.SaveTrackAsync(track);

                frmTrackList.Add(track);

                tracksOverlay.Routes.Add(new MapTrack(track));

                trackTiles = await GetTilesFromTrackAsync(track);

                var saveTiles = new List<TileModel>();

                int index;

                foreach (var trackTile in trackTiles)
                {
                    index = tiles.FindIndex(tile => tile.X == trackTile.X && tile.Y == trackTile.Y);

                    if (index == -1)
                    {
                        tiles.Add(trackTile);

                        saveTiles.Add(trackTile);
                    }
                    else
                    {
                        trackTile.Id = tiles[index].Id;
                    }
                }

                await DB.SaveTilesAsync(saveTiles);

                List<TracksTilesModel> tracksTiles = new List<TracksTilesModel>();

                foreach (var tile in trackTiles)
                {
                    Debug.WriteLine(tile.Id);
                    tracksTiles.Add(new TracksTilesModel()
                    {
                        TrackId = track.Id,
                        TileId = tile.Id,
                    });
                }

                await DB.SaveTracksTilesAsync(tracksTiles);
            }

            await LoadTilesAsync();
        }

        private async void MiMainDataOpenTrack_Click(object sender, EventArgs e)
        {
            ChildFormsTopMost = false;

            bool open = openFileDialog.ShowDialog() == DialogResult.OK;

            ChildFormsTopMost = true;

            if (open)
            {
                Status = ProgramStatus.LoadGpx;

                await OpenTracksAsync(openFileDialog.FileNames);

                Status = ProgramStatus.Idle;
            }
        }

        private void MiMainDataMarkerList_Click(object sender, EventArgs e)
        {
            frmMarkerList.Visible = !frmMarkerList.Visible;
        }

        private void MiMainDataTrackList_Click(object sender, EventArgs e)
        {
            frmTrackList.Visible = !frmTrackList.Visible;
        }

        private void MiMainGrayScale_Click(object sender, EventArgs e)
        {
            gMapControl.GrayScaleMode = miMainGrayScale.Checked;
        }

        public interface IMainForm
        {
            void SelectById(object sender, long id);

            void ChangeSelected(object sender);
        }

        public void SelectTrackById(long id)
        {
            foreach (var track in from MapTrack track in tracksOverlay.Routes
                                  where track.Track.Id == id
                                  select track)
            {
                SelectedTrack = track;

                if (ActiveForm != this) gMapControl.Invalidate();

                break;
            }
        }

        public void SelectMarkerById(long id)
        {
            foreach (var marker in from MapMarker marker in markersOverlay.Markers
                                   where marker.Marker.Id == id
                                   select marker)
            {
                SelectedMarker = marker;

                if (ActiveForm != this) gMapControl.Invalidate();

                break;
            }
        }

        public void SelectById(object sender, long id)
        {
            if (sender == frmTrackList) SelectTrackById(id);
            else if (sender == frmMarkerList) SelectMarkerById(id);
        }

        public void ChangeSelected(object sender)
        {
            if (sender == frmTrackList) TrackChangeAsync(SelectedTrack);
            else if (sender == frmMarkerList) MarkerChangeAsync(SelectedMarker);
        }

        private void UpdateChildFormState(Form childForm)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                childForm.TopMost = false;
                if (childForm.Visible) childForm.WindowState = FormWindowState.Minimized;
            }
            else
            {
                if (childForm.Visible) childForm.WindowState = FormWindowState.Normal;
                childForm.TopMost = true;
            }
        }

        private void Main_SizeChanged(object sender, EventArgs e)
        {
            UpdateChildFormState(frmTrackList);
            UpdateChildFormState(frmMarkerList);
        }
    }
}