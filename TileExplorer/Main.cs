//#define CHECK_TILES
//#define SHOW_TRACK_KM
//#define DUMMY_TILES

using Bluegrams.Application;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using P3tr0viCh;
using System;
using System.Collections;
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
using static TileExplorer.StatusStripPresenter;

namespace TileExplorer
{
    public interface IMainForm
    {
        Filter Filter { get; }

        void SelectById(object sender, long id);

        void ChangeById(object sender, long id);
    }

    public partial class Main : Form, IStatusStripView, IMainForm
    {
        private readonly Database DB;

        private readonly Filter filter = new Filter();
        public Filter Filter => filter;

        private readonly GMapOverlay tilesOverlay = new GMapOverlay("tiles");
        private readonly GMapOverlay tracksOverlay = new GMapOverlay("tracks");
        private readonly GMapOverlay markersOverlay = new GMapOverlay("markers");

        private readonly StatusStripPresenter statusStripPresenter;

        public ToolStripStatusLabel LabelZoom => slZoom;
        public ToolStripStatusLabel LabelTileId => slTileId;
        public ToolStripStatusLabel LabelPosition => slPosition;
        public ToolStripStatusLabel LabelMousePosition => slMousePosition;
        public ToolStripStatusLabel LabelStatus => slStatus;
        public ToolStripStatusLabel LabelTracksCount => slTracksCount;
        public ToolStripStatusLabel LabelTracksDistance => slTracksDistance;
        public ToolStripStatusLabel LabelTilesVisited => slTilesVisited;
        public ToolStripStatusLabel LabelTilesMaxCluster => slTilesMaxCluster;
        public ToolStripStatusLabel LabelTilesMaxSquare => slTilesMaxSquare;

        private readonly FrmFilter frmFilter;

        private readonly FrmTrackList frmTrackList;
        private readonly FrmMarkerList frmMarkerList;

        public Main()
        {
            InitializeComponent();

            PortableSettingsProvider.SettingsFileName = Files.SettingsFileName();
            PortableSettingsProviderBase.SettingsDirectory = Files.SettingsDirectory();
            Directory.CreateDirectory(PortableSettingsProviderBase.SettingsDirectory);
            PortableSettingsProvider.ApplyProvider(Settings.Default);
            PortableSettingsProviderBase.AllRoaming = true;

            DB = new Database(Files.DatabaseFileName());

            statusStripPresenter = new StatusStripPresenter(this);

            frmTrackList = new FrmTrackList(this);
            frmMarkerList = new FrmMarkerList(this);

            filter.Day = Settings.Default.FilterDay;
            filter.DateFrom = Settings.Default.FilterDateFrom;
            filter.DateTo = Settings.Default.FilterDateTo;
            filter.Years = SettingsExt.Default.LoadIntArray("FilterYears");

            frmFilter = new FrmFilter(this);

            filter.OnChanged += Filter_OnChanged;
        }

        private void Filter_OnChanged()
        {
            Debug.WriteLine("Filter_OnChanged");

            var load = DataLoad.Tiles | DataLoad.TracksInfo;

            if (frmTrackList.Visible || miMainShowTracks.Checked) load |= DataLoad.Tracks;

            _ = DataUpdateAsync(load);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            SettingsExt.Default.LoadFormBounds(this);

            SettingsExt.Default.LoadFormBounds(frmTrackList);
            SettingsExt.Default.LoadFormBounds(frmMarkerList);

            SettingsExt.Default.LoadFormBounds(frmFilter);

            SettingsExt.Default.LoadDataGridColumns(frmTrackList.DataGridView);
            SettingsExt.Default.LoadDataGridColumns(frmMarkerList.DataGridView);

#if DEBUG && DUMMY_TILES
            DummyTiles();
#endif

#if !DEBUG
            miMapTileAdd.Visible = false;
            miMapTileDelete.Visible = false;
#endif
            miMainShowTracks.Checked = Settings.Default.VisibleTracks;
            miMainShowMarkers.Checked = Settings.Default.VisibleMarkers;

            miMainGrayScale.Checked = Settings.Default.MapGrayScale;
            gMapControl.GrayScaleMode = miMainGrayScale.Checked;

            tracksOverlay.IsVisibile = miMainShowTracks.Checked;
            markersOverlay.IsVisibile = miMainShowMarkers.Checked;

            frmTrackList.Visible = Settings.Default.VisibleListTracks;
            frmMarkerList.Visible = Settings.Default.VisibleListMarkers;

            frmFilter.Visible = Settings.Default.VisibleFilter;

            var load = DataLoad.Tiles | DataLoad.TracksInfo;

            if (frmTrackList.Visible || miMainShowTracks.Checked) load |= DataLoad.Tracks;

            if (frmMarkerList.Visible || miMainShowMarkers.Checked) load |= DataLoad.Markers;

            _ = DataUpdateAsync(load);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            FullScreen = false;

            SettingsExt.Default.SaveFormBounds(this);

            SettingsExt.Default.SaveFormBounds(frmTrackList);
            SettingsExt.Default.SaveFormBounds(frmMarkerList);

            SettingsExt.Default.SaveFormBounds(frmFilter);

            SettingsExt.Default.SaveDataGridColumns(frmTrackList.DataGridView);
            SettingsExt.Default.SaveDataGridColumns(frmMarkerList.DataGridView);

            Settings.Default.VisibleTracks = miMainShowTracks.Checked;
            Settings.Default.VisibleMarkers = miMainShowMarkers.Checked;

            Settings.Default.MapGrayScale = miMainGrayScale.Checked;

            Settings.Default.VisibleListTracks = frmTrackList.Visible;
            Settings.Default.VisibleListMarkers = frmMarkerList.Visible;

            Settings.Default.VisibleFilter = frmFilter.Visible;

            Settings.Default.FilterDay = filter.Day;
            Settings.Default.FilterDateFrom = filter.DateFrom;
            Settings.Default.FilterDateTo = filter.DateTo;
            SettingsExt.Default.SaveIntArray("FilterYears", filter.Years);

            Settings.Default.Save();
        }

        private void GMapControl_Load(object sender, EventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            var assemblyName = Assembly.GetExecutingAssembly().GetName();

            GMap.NET.MapProviders.GMapProvider.UserAgent =
                string.Format("{0}/{1}.{2}", assemblyName.Name, assemblyName.Version.Major, assemblyName.Version.Minor);
#if DEBUG
            GMap.NET.MapProviders.GMapProvider.UserAgent += " (debug)";
#endif

            Debug.WriteLine(GMap.NET.MapProviders.GMapProvider.UserAgent);

            gMapControl.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            gMapControl.MapProvider.RefererUrl = Settings.Default.MapRefererUrl;

            gMapControl.ShowCenter = false;

            gMapControl.MinZoom = 2;
            gMapControl.MaxZoom = 16;

            HomeGoto();

            gMapControl.MouseWheelZoomType = MouseWheelZoomType.ViewCenter;

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
            SuspendLayout();
            toolStripContainer.ContentPanel.SuspendLayout();

            var savedBounds = gMapControl.Bounds;

            var imageSize = new Size(Screen.PrimaryScreen.Bounds.Width,
                Screen.PrimaryScreen.Bounds.Height);

            try
            {
                gMapControl.Dock = DockStyle.None;

                gMapControl.SetBounds(
                    (toolStripContainer.ContentPanel.Width - imageSize.Width) / 2,
                    (toolStripContainer.ContentPanel.Height - imageSize.Height) / 2,
                    imageSize.Width,
                    imageSize.Height);

                gMapControl.ToImage().Save(fileName, ImageFormat.Png);
            }
            finally
            {
                gMapControl.Bounds = savedBounds;

                gMapControl.Dock = DockStyle.Fill;

                toolStripContainer.ContentPanel.ResumeLayout(false);
                toolStripContainer.PerformLayout();
                ResumeLayout(false);
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

            var calcResult = await Task.Run(() =>
            {
                return Utils.CalcTiles(tiles);
            });

            statusStripPresenter.TilesVisited = calcResult.Visited;
            statusStripPresenter.TilesMaxCluster = calcResult.MaxCluster;
            statusStripPresenter.TilesMaxSquare = calcResult.MaxSquare;
        }

        private async Task LoadTilesAsync()
        {
            Debug.WriteLine("LoadTilesAsync: start");

            tilesOverlay.Clear();

            var tiles = await DB.LoadTilesAsync(filter);

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

            Debug.WriteLine("LoadTilesAsync: end");
        }

        private async Task LoadTracksAsync()
        {
            Debug.WriteLine("LoadTracksAsync: start");

            tracksOverlay.Clear();

            var tracks = await DB.LoadTracksAsync(filter);

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

            Debug.WriteLine("LoadTracksAsync: end");
        }

        private async Task LoadMarkersAsync()
        {
            Debug.WriteLine("LoadMarkersAsync: start");

            markersOverlay.Clear();

            var markers = await DB.LoadMarkersAsync();

            foreach (var marker in markers)
            {
                markersOverlay.Markers.Add(new MapMarker(marker));
            }

            frmMarkerList.List = markers;

            Debug.WriteLine("LoadMarkersAsync: end");
        }

        private async Task LoadTracksInfoAsync()
        {
            Debug.WriteLine("LoadTracksInfoAsync: start");

            var tracksInfo = await DB.LoadTracksInfoAsync(filter);

            statusStripPresenter.TracksCount = tracksInfo.Count;
            statusStripPresenter.TracksDistance = tracksInfo.Distance / 1000.0;

            Debug.WriteLine("LoadTracksInfoAsync: end");
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

                frmTrackList.Updating = value != ProgramStatus.Idle;
                frmMarkerList.Updating = value != ProgramStatus.Idle;

                frmFilter.Updating = value != ProgramStatus.Idle;
            }
        }

        [Flags]
        private enum DataLoad
        {
            Tiles = 1,
            Tracks = 2,
            Markers = 4,
            TracksInfo = 8
        }

        private async Task DataUpdateAsync(DataLoad load = default)
        {
            Status = ProgramStatus.LoadData;

            if (load == default)
            {
                load = DataLoad.Tiles | DataLoad.Tracks | DataLoad.Markers | DataLoad.TracksInfo;
            }

            if (load.HasFlag(DataLoad.Tracks))
            {
                load |= DataLoad.TracksInfo;
            }

            Debug.WriteLine($"Loading data {load}");

            if (load.HasFlag(DataLoad.Tiles)) await LoadTilesAsync();

            if (load.HasFlag(DataLoad.TracksInfo)) await LoadTracksInfoAsync();

            if (load.HasFlag(DataLoad.Tracks)) await LoadTracksAsync();

            if (load.HasFlag(DataLoad.Markers)) await LoadMarkersAsync();

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
            FrmAbout.Show();
        }

        private bool MarkerMoving;

        private IMapItem selected = null;

        public IMapItem Selected
        {
            get
            {
                return selected;
            }
            set
            {
                if (selected == value) return;

                if (selected != null)
                {
                    selected.Selected = false;
                }

                selected = value;

                if (selected != null)
                {
                    selected.Selected = true;

                    switch (selected.Type)
                    {
                        case MapItemType.Marker:
                            frmMarkerList.Selected = (MarkerModel)selected.Model;
                            break;
                        case MapItemType.Track:
                            frmTrackList.Selected = (TrackModel)selected.Model;
                            break;
                    }
                }
            }
        }

        public MapMarker SelectedMarker
        {
            get
            {
                if (Selected == null) return null;

                if (Selected.Type != MapItemType.Marker) return null;

                return (MapMarker)selected;
            }
        }

        public MapTrack SelectedTrack
        {
            get
            {
                if (Selected == null) return null;

                if (Selected.Type != MapItemType.Track) return null;

                return (MapTrack)selected;
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
                            await DB.SaveMarkerAsync(SelectedMarker.Model);

                            frmMarkerList.Update(SelectedMarker.Model);
                        }
                    }

                    break;
                case MouseButtons.Right:
                    cmMap.Show(gMapControl, e.Location);

                    break;
            }

            Selected = null;
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
                        TrackChangeAsync(SelectedTrack);
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

            Selected = (MapMarker)item;

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

            Selected = (MapTrack)item;

            switch (e.Button)
            {
                case MouseButtons.Left:

                    break;
                case MouseButtons.Right:
                    cmTrack.Show(gMapControl, e.Location);

                    break;
            }
        }

        private void CheckAndLoadData(DataLoad load)
        {
            switch (load)
            {
                case DataLoad.Tracks:
                    if (tracksOverlay.Routes.Count == 0)
                    {
                        _ = DataUpdateAsync(DataLoad.Tracks);
                    }
                    break;
                case DataLoad.Markers:
                    if (markersOverlay.Markers.Count == 0)
                    {
                        _ = DataUpdateAsync(DataLoad.Markers);
                    }
                    break;
            }
        }

        private void MiMainShowMarkers_Click(object sender, EventArgs e)
        {
            markersOverlay.IsVisibile = miMainShowMarkers.Checked;

            CheckAndLoadData(DataLoad.Markers);
        }

        private void MiMainShowTracks_Click(object sender, EventArgs e)
        {
            tracksOverlay.IsVisibile = miMainShowTracks.Checked;

            CheckAndLoadData(DataLoad.Tracks);
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
        }

        private async void MarkerChangeAsync(MapMarker marker)
        {
            if (marker == null) return;

            if (FrmMarker.ShowDlg(this, marker.Model))
            {
                await DB.SaveMarkerAsync(marker.Model);

                marker.NotifyModelChanged();

                frmMarkerList.Update(marker.Model);

                if (ActiveForm != this) gMapControl.Invalidate();
            }
        }

        private async void MarkerDeleteAsync(MapMarker marker)
        {
            if (marker == null) return;

            string Name = marker.Model.Text;

            if (Name == "")
            {
                Name = marker.Position.Lat.ToString() + ":" + marker.Position.Lng.ToString();
            }

            if (Msg.Question(string.Format(Resources.QuestionMarkerDelete, Name)))
            {
                await DB.DeleteMarkerAsync(marker.Model);

                markersOverlay.Markers.Remove(marker);

                frmMarkerList.Delete(marker.Model);

                if (ActiveForm != this) gMapControl.Invalidate();
            }
        }

        private async void TrackChangeAsync(MapTrack track)
        {
            if (track == null) return;

            if (FrmTrack.ShowDlg(this, track.Model))
            {
                await DB.SaveTrackAsync(track.Model);

                track.NotifyModelChanged();

                frmTrackList.Update(track.Model);

                if (ActiveForm != this) gMapControl.Invalidate();
            }
        }

        private async void TrackDeleteAsync(MapTrack track)
        {
            if (track == null) return;

            string Name = track.Model.Text;

            if (Name == "")
            {
                Name = track.Model.DateTime.ToString();
            }

            if (Msg.Question(string.Format(Resources.QuestionTrackDelete, Name)))
            {
                await DB.DeleteTrackAsync(track.Model);

                tracksOverlay.Routes.Remove(track);

                frmTrackList.Delete(track.Model);

                await DataUpdateAsync(DataLoad.Tiles | DataLoad.TracksInfo);
            }
        }

        Point MenuPopupPoint;

        private void CmMap_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MenuPopupPoint = gMapControl.PointToClient(Control.MousePosition);
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

        private void MiTrackChange_Click(object sender, EventArgs e)
        {
            TrackChangeAsync(SelectedTrack);
        }

        private void MiTrackDelete_Click(object sender, EventArgs e)
        {
            TrackDeleteAsync(SelectedTrack);
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

            SelectedMarker.Model.Lat = position.Lat;
            SelectedMarker.Model.Lng = position.Lng;
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
                        SelectedMarker.Model.Lat = MarkerMovingPrevPosition.Lat;
                        SelectedMarker.Model.Lng = MarkerMovingPrevPosition.Lng;
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
            _ = DataUpdateAsync();
        }

        private async void AddTileAsync(TileModel tile)
        {
            Status = ProgramStatus.SaveData;

            await DB.SaveTileAsync(tile);

            await DataUpdateAsync(DataLoad.Tiles);
        }

        private void MiMapTileAdd_Click(object sender, EventArgs e)
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
                Msg.Error("tile exists");
            }
        }

        private async void DeleteTileAsync(TileModel tile)
        {
            Status = ProgramStatus.SaveData;

            await DB.DeleteTileAsync(tile);

            await DataUpdateAsync(DataLoad.Tiles);
        }

        private void MiMapTileDelete_Click(object sender, EventArgs e)
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
#if DEBUG
            SaveToImage(Path.Combine(Path.GetTempPath(), "xxx.png"));
#else
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                SaveToImage(saveFileDialog.FileName);
            }
#endif
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
            Status = ProgramStatus.LoadGpx;

            TrackModel track;

            List<TileModel> trackTiles;

            foreach (var file in files)
            {
                track = await OpenTrackFromFileAsync(file);

                await DB.SaveTrackAsync(track);

                frmTrackList.Add(track);

                var mapTrack = new MapTrack(track);

                tracksOverlay.Routes.Add(mapTrack);

                Selected = mapTrack;

                trackTiles = await GetTilesFromTrackAsync(track);

                var saveTiles = new List<TileModel>();

                int id;

                foreach (var trackTile in trackTiles)
                {
                    id = await DB.ExistsTileAsync(trackTile);

                    Debug.WriteLine(id);

                    if (id == 0)
                    {
                        saveTiles.Add(trackTile);
                    }
                    else
                    {
                        trackTile.Id = id;
                    }
                }

                Debug.WriteLine("saveTiles count: " + saveTiles.Count);

                await DB.SaveTilesAsync(saveTiles);

                var tracksTiles = new List<TracksTilesModel>();

                foreach (var tile in trackTiles)
                {
                    tracksTiles.Add(new TracksTilesModel()
                    {
                        TrackId = track.Id,
                        TileId = tile.Id,
                    });
                }

                await DB.SaveTracksTilesAsync(tracksTiles);
            }

            frmTrackList.Sort();

            Status = ProgramStatus.Idle;
        }

        private async void MiMainDataOpenTrack_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = string.Empty;

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                await OpenTracksAsync(openFileDialog.FileNames);

                await DataUpdateAsync(DataLoad.Tiles | DataLoad.TracksInfo);
            }
        }

        private void MiMainDataMarkerList_Click(object sender, EventArgs e)
        {
            frmMarkerList.Visible = !frmMarkerList.Visible;

            CheckAndLoadData(DataLoad.Markers);
        }

        private void MiMainDataTrackList_Click(object sender, EventArgs e)
        {
            frmTrackList.Visible = !frmTrackList.Visible;

            CheckAndLoadData(DataLoad.Tracks);
        }

        private void MiMainDataFilter_Click(object sender, EventArgs e)
        {
            frmFilter.Visible = !frmFilter.Visible;
        }

        private void MiMainGrayScale_Click(object sender, EventArgs e)
        {
            gMapControl.GrayScaleMode = miMainGrayScale.Checked;
        }

        public void SelectById(object sender, long id)
        {
            IEnumerable items = null;

            switch (((IFrmChild)sender).Type)
            {
                case FrmListType.Markers:
                    items = markersOverlay.Markers;
                    break;
                case FrmListType.Tracks:
                    items = tracksOverlay.Routes;
                    break;
                default:
                    return;
            }

            foreach (var item in from IMapItem item in items
                                 where item.Model.Id == id
                                 select item)
            {
                Selected = item;

                if (ActiveForm != this) gMapControl.Invalidate();

                break;
            }
        }

        public void ChangeById(object sender, long id)
        {
            SelectById(sender, id);

            switch (((IFrmChild)sender).Type)
            {
                case FrmListType.Markers:
                    MarkerChangeAsync(SelectedMarker);
                    break;
                case FrmListType.Tracks:
                    TrackChangeAsync(SelectedTrack);
                    break;
            }
        }
    }
}