//#define CHECK_TILES
//#define SHOW_TRACK_KM
//#define DUMMY_TILES

using GMap.NET;
using GMap.NET.WindowsForms;
using P3tr0viCh.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;
using static TileExplorer.StatusStripPresenter;

namespace TileExplorer
{
    public partial class Main : Form, IStatusStripView, IMainForm
    {
        private readonly GMapOverlay gridOverlay = new GMapOverlay("grid");
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

        private FrmFilter frmFilter;

        private FrmList frmResults;

        private FrmList frmTrackList;
        private FrmList frmMarkerList;

        public Main()
        {
            InitializeComponent();

            AppSettings.Directory =
#if DEBUG
                 Files.ExecutableDirectory();
#else
                Files.AppDataDirectory();
#endif

            Utils.WriteDebug("settings: " + AppSettings.FilePath);

            AppSettings.Default.Load();

            UpdateDatabaseFileName();

            statusStripPresenter = new StatusStripPresenter(this);

            Database.Filter.Default.Day = AppSettings.Default.Filter.Day;
            Database.Filter.Default.DateFrom = AppSettings.Default.Filter.DateFrom;
            Database.Filter.Default.DateTo = AppSettings.Default.Filter.DateTo;
            Database.Filter.Default.Years = AppSettings.Default.Filter.Years;

            Database.Filter.Default.OnChanged += Filter_OnChanged;
        }

        private void Filter_OnChanged()
        {
            var load = DataLoad.Tiles | DataLoad.TracksInfo | DataLoad.TracksList;

            if (miMainShowTracks.Checked) load |= DataLoad.Tracks;

            _ = UpdateDataAsync(load);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            AppSettings.LoadFormState(this, AppSettings.Default.FormStateMain);

#if DEBUG && DUMMY_TILES
            DummyTiles();
#endif

#if !DEBUG
            miMapTileAdd.Visible = false;
            miMapTileDelete.Visible = false;
#endif
            miMainShowGrid.Checked = AppSettings.Default.VisibleGrid;
            miMainShowTracks.Checked = AppSettings.Default.VisibleTracks;
            miMainShowMarkers.Checked = AppSettings.Default.VisibleMarkers;

            miMainGrayScale.Checked = AppSettings.Default.MapGrayScale;
            gMapControl.GrayScaleMode = miMainGrayScale.Checked;

            gridOverlay.IsVisibile = miMainShowGrid.Checked;
            tracksOverlay.IsVisibile = miMainShowTracks.Checked;
            markersOverlay.IsVisibile = miMainShowMarkers.Checked;

            miMainLeftPanel.Checked = AppSettings.Default.VisibleLeftPanel;
            toolStripContainer.LeftToolStripPanelVisible = miMainLeftPanel.Checked;

            StartUpdateGrid();

            _ = UpdateDataAsync();

            if (AppSettings.Default.VisibleResults)
            {
                ShowChildForm(ChildFormType.Results, true);
            }
            if (AppSettings.Default.VisibleTrackList)
            {
                ShowChildForm(ChildFormType.Tracks, true);
            }
            if (AppSettings.Default.VisibleMarkerList)
            {
                ShowChildForm(ChildFormType.Markers, true);
            }
            if (AppSettings.Default.VisibleFilter)
            {
                ShowChildForm(ChildFormType.Filter, true);
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            FullScreen = false;

            AppSettings.Default.FormStateMain = AppSettings.SaveFormState(this);

            AppSettings.Default.MapGrayScale = miMainGrayScale.Checked;

            AppSettings.Default.VisibleGrid = miMainShowGrid.Checked;
            AppSettings.Default.VisibleTracks = miMainShowTracks.Checked;
            AppSettings.Default.VisibleMarkers = miMainShowMarkers.Checked;

            AppSettings.Default.VisibleFilter = Utils.IsChildFormExists(frmFilter);

            AppSettings.Default.VisibleResults = Utils.IsChildFormExists(frmResults);
            AppSettings.Default.VisibleTrackList = Utils.IsChildFormExists(frmTrackList);
            AppSettings.Default.VisibleMarkerList = Utils.IsChildFormExists(frmMarkerList);

            AppSettings.Default.VisibleLeftPanel = miMainLeftPanel.Checked;

            AppSettings.Default.Filter.Day = Database.Filter.Default.Day;
            AppSettings.Default.Filter.DateFrom = Database.Filter.Default.DateFrom;
            AppSettings.Default.Filter.DateTo = Database.Filter.Default.DateTo;
            AppSettings.Default.Filter.Years = Database.Filter.Default.Years;

            AppSettings.Default.Save();
        }

        private void GMapControl_Load(object sender, EventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            GMap.NET.MapProviders.GMapProvider.UserAgent = Utils.AssemblyNameAndVersion();

            Utils.WriteDebug("useragent: " + GMap.NET.MapProviders.GMapProvider.UserAgent);

            gMapControl.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            gMapControl.MapProvider.RefererUrl = AppSettings.Default.MapRefererUrl;

            gMapControl.ShowCenter = false;

            gMapControl.MinZoom = 2;
            gMapControl.MaxZoom = 16;

            HomeGoto();

            switch (AppSettings.Default.MouseWheelZoomType)
            {
                case 0:
                    gMapControl.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
                    break;
                case 1:
                default:
                    gMapControl.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;
                    break;
                case 2:
                    gMapControl.MouseWheelZoomType = MouseWheelZoomType.ViewCenter;
                    break;
            }

            gMapControl.CanDragMap = true;
            gMapControl.DragButton = MouseButtons.Left;

            gMapControl.Overlays.Add(gridOverlay);
            gMapControl.Overlays.Add(tilesOverlay);
            gMapControl.Overlays.Add(tracksOverlay);
            gMapControl.Overlays.Add(markersOverlay);

            statusStripPresenter.Zoom = gMapControl.Zoom;
            statusStripPresenter.Position = gMapControl.Position;
            statusStripPresenter.TileId = gMapControl.Position;
            statusStripPresenter.MousePosition = gMapControl.Position;
        }

        private void StartUpdateGrid()
        {
            if (!miMainShowGrid.Checked) return;

            timerMapMove.Stop();
            timerMapMove.Start();
        }

        public ProgramStatus Status { set => statusStripPresenter.Status = value.Description(); }

        private void GMapControl_OnMapZoomChanged()
        {
            statusStripPresenter.Zoom = gMapControl.Zoom;

            miMainSaveTileBoundaryToFile.Enabled = gMapControl.Zoom >= AppSettings.Default.SaveOsmTileMinZoom;

            StartUpdateGrid();
        }

        private void GMapControl_OnPositionChanged(PointLatLng point)
        {
            statusStripPresenter.Position = point;

            StartUpdateGrid();
        }

        private void GMapControl_SizeChanged(object sender, EventArgs e)
        {
            StartUpdateGrid();
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
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

        private async Task CalcTilesAsync(List<Tile> tiles)
        {
            foreach (var tile in tiles)
            {
                tile.Status = TileStatus.Visited;
            }

            var calcResult = await Task.Run(() =>
            {
                return Utils.Tiles.CalcTiles(tiles);
            });

            statusStripPresenter.TilesVisited = calcResult.Visited;
            statusStripPresenter.TilesMaxCluster = calcResult.MaxCluster;
            statusStripPresenter.TilesMaxSquare = calcResult.MaxSquare;
        }

        private async Task LoadTilesAsync()
        {
            Utils.WriteDebug("start");

            tilesOverlay.Clear();

            var tiles = await Database.Default.ListLoadAsync<Tile>();

            await CalcTilesAsync(tiles);

            foreach (var tile in tiles)
            {
                tilesOverlay.Polygons.Add(new MapItemTile(tile));

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

            Utils.WriteDebug("end");
        }

        private async Task LoadTracksAsync(bool loadPoints)
        {
            Utils.WriteDebug("start");

            tracksOverlay.Clear();

            var tracks = await Database.Default.ListLoadAsync<Track>();

            foreach (var track in tracks)
            {
                if (loadPoints)
                {
                    track.TrackPoints = await Database.Default.ListLoadAsync<TrackPoint>(track);
                }

                tracksOverlay.Routes.Add(new MapItemTrack(track));

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

                    Utils.WriteDebug(Geo.Haversine(lat1, lng1, lat2, lng2));
                    
                    lat1 = lat2;
                    lng1 = lng2;
                }
#endif
            }

            Utils.WriteDebug("end");
        }

        private async Task LoadMarkersAsync()
        {
            Utils.WriteDebug("start");

            markersOverlay.Clear();

            var markers = await Database.Default.ListLoadAsync<Marker>();

            foreach (var marker in markers)
            {
                markersOverlay.Markers.Add(new MapItemMarker(marker));
            }

            Utils.WriteDebug("end");
        }

        private async Task LoadTracksInfoAsync()
        {
            Utils.WriteDebug("start");

            var tracksInfo = await Database.Default.LoadTracksInfoAsync(Database.Filter.Default);

            statusStripPresenter.TracksCount = tracksInfo.Count;
            statusStripPresenter.TracksDistance = tracksInfo.Distance / 1000.0;

            Utils.WriteDebug("end");
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
                    toolStripContainer.TopToolStripPanelVisible = false;
                    toolStripContainer.BottomToolStripPanelVisible = false;
                    toolStripContainer.LeftToolStripPanelVisible = false;

                    savedWindowState = WindowState;

                    WindowState = FormWindowState.Normal;

                    FormBorderStyle = FormBorderStyle.None;

                    WindowState = FormWindowState.Maximized;
                }
                else
                {
                    FormBorderStyle = FormBorderStyle.Sizable;

                    WindowState = savedWindowState;

                    toolStripContainer.TopToolStripPanelVisible = true;
                    toolStripContainer.BottomToolStripPanelVisible = true;
                    toolStripContainer.LeftToolStripPanelVisible = miMainLeftPanel.Checked;
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
            FrmAbout.Show(new FrmAbout.Options() { Link = Resources.GitHubLink });
        }

        private async void UpdateSelectedTrackTiles(Track track)
        {
            foreach (var tile in tilesOverlay.Polygons.Cast<MapItemTile>())
            {
                tile.Selected = false;
            }

            if (track == null) return;

            var tiles = await Database.Default.ListLoadAsync<Tile>(track);

            foreach (var tile in from item in tilesOverlay.Polygons.Cast<MapItemTile>()
                                 from tile in tiles
                                 where item.Model.X == tile.X && item.Model.Y == tile.Y
                                 select item)
            {
                tile.Selected = true;
            }

            if (ActiveForm != this) gMapControl.Invalidate();
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
                            frmMarkerList?.SetSelected(selected.Model);

                            UpdateSelectedTrackTiles(null);

                            break;
                        case MapItemType.Track:
                            frmTrackList?.SetSelected(selected.Model);

                            UpdateSelectedTrackTiles((Track)selected.Model);

                            break;
                    }
                }
                else
                {
                    UpdateSelectedTrackTiles(null);
                }
            }
        }

        public MapItemMarker SelectedMarker
        {
            get
            {
                if (Selected == null) return null;

                if (Selected.Type != MapItemType.Marker) return null;

                return (MapItemMarker)selected;
            }
        }

        public MapItemTrack SelectedTrack
        {
            get
            {
                if (Selected == null) return null;

                if (Selected.Type != MapItemType.Track) return null;

                return (MapItemTrack)selected;
            }
        }

        private IMapItem MouseOverMapItem()
        {
            if (gMapControl.IsMouseOverMarker)
            {
                foreach (var marker in markersOverlay.Markers)
                {
                    if (marker.IsMouseOver)
                    {
                        return (MapItemMarker)marker;
                    }
                }
            }

            if (gMapControl.IsMouseOverRoute)
            {
                foreach (var track in tracksOverlay.Routes)
                {
                    if (track.IsMouseOver)
                    {
                        return (MapItemTrack)track;
                    }
                }
            }

            return null;
        }

        private async void GMapControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (MarkerMoving)
            {
                MarkerMoving = false;

                if (SelectedMarker != null)
                {
                    await Database.Default.SaveMarkerAsync(SelectedMarker.Model);

                    await UpdateDataAsync(DataLoad.MarkersList);
                }

                return;
            }

            var mapItem = MouseOverMapItem();

            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (mapItem != null)
                    {
                        Selected = mapItem.Selected ? null : mapItem;
                    }

                    break;
                case MouseButtons.Right:
                    Selected = mapItem ?? null;

                    ContextMenuStrip contextMenu;

                    if (gMapControl.IsMouseOverMarker)
                    {
                        contextMenu = cmMarker;
                    }
                    else
                    {
                        if (gMapControl.IsMouseOverRoute)
                        {
                            contextMenu = cmTrack;
                        }
                        else
                        {
                            contextMenu = cmMap;
                        }
                    }

                    contextMenu.Show(gMapControl, e.Location);

                    break;
            }
        }

        private void GMapControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var mapItem = MouseOverMapItem();

            Selected = mapItem ?? null;

            if (e.Button == MouseButtons.Left)
            {
                if (Selected == null)
                {
                    MarkerAdd(gMapControl.FromLocalToLatLng(e.X, e.Y));
                }
                else
                {
                    switch (Selected.Type)
                    {
                        case MapItemType.Marker:
                            MarkerChange(SelectedMarker);
                            break;
                        case MapItemType.Track:
                            TrackChangeAsync(SelectedTrack);
                            break;
                    }
                }
            }
        }

        private void CheckAndLoadData(DataLoad load)
        {
            switch (load)
            {
                case DataLoad.Tracks:
                    if (tracksOverlay.Routes.Count == 0)
                    {
                        _ = UpdateDataAsync(DataLoad.Tracks);
                    }
                    break;
                case DataLoad.Markers:
                    if (markersOverlay.Markers.Count == 0)
                    {
                        _ = UpdateDataAsync(DataLoad.Markers);
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
            gMapControl.Zoom = AppSettings.Default.HomeZoom;
            gMapControl.Position = new PointLatLng(AppSettings.Default.HomeLat, AppSettings.Default.HomeLng);
        }

        private void HomeSave()
        {
            AppSettings.Default.HomeZoom = (int)gMapControl.Zoom;
            AppSettings.Default.HomeLat = gMapControl.Position.Lat;
            AppSettings.Default.HomeLng = gMapControl.Position.Lng;
        }

        private void MiMainHomeGoto_Click(object sender, EventArgs e)
        {
            HomeGoto();
        }

        private void MiMainHomeSave_Click(object sender, EventArgs e)
        {
            if (Msg.Question(Resources.QuestionHomeSave))
            {
                HomeSave();
            }
        }

        private GMapMarker markerTemp;

        private void MarkerAdd(PointLatLng point)
        {
            bool prevMarkersVisible = markersOverlay.IsVisibile;

            markersOverlay.IsVisibile = true;

            var marker = new Marker()
            {
                Lat = point.Lat,
                Lng = point.Lng,

#if DEBUG
                Text = DateTime.Now.ToString(),
#endif
            };

            markerTemp = new MapItemMarkerCross(point);

            markersOverlay.Markers.Add(markerTemp);

            if (FrmMarker.ShowDlg(this, marker))
            {
                _ = UpdateDataAsync(DataLoad.MarkersList);
            }

            markersOverlay.Markers.Remove(markerTemp);

            markersOverlay.IsVisibile = prevMarkersVisible;
        }

        private void MarkerChange(MapItemMarker marker)
        {
            if (marker == null) return;

            if (FrmMarker.ShowDlg(this, marker.Model))
            {
                _ = UpdateDataAsync(DataLoad.MarkersList);
            }
        }

        public void MarkerChanged(Marker marker)
        {
            var mapMarker = FindMapItem(marker);

            if (mapMarker == null)
            {
                markersOverlay.Markers.Remove(markerTemp);

                markersOverlay.Markers.Add(new MapItemMarker(marker));

                _ = UpdateDataAsync(DataLoad.MarkersList);
            }
            else
            {
                mapMarker.Model = marker;

                mapMarker.NotifyModelChanged();

                _ = UpdateDataAsync(DataLoad.MarkersList);

                if (ActiveForm != this) gMapControl.Invalidate();
            }
        }

        private async void MarkerDeleteAsync(MapItemMarker marker)
        {
            if (marker == null) return;

            var name = marker.Model.Text;

            if (string.IsNullOrEmpty(name))
            {
                name = marker.Position.Lat.ToString() + ":" + marker.Position.Lng.ToString();
            }

            if (Msg.Question(string.Format(Resources.QuestionMarkerDelete, name)))
            {
                await Database.Default.DeleteMarkerAsync(marker.Model);

                markersOverlay.Markers.Remove(marker);

                await UpdateDataAsync(DataLoad.MarkersList);

                if (ActiveForm != this) gMapControl.Invalidate();
            }
        }

        private async void TrackChangeAsync(MapItemTrack track)
        {
            if (track == null) return;

            if (FrmTrack.ShowDlg(this, track.Model))
            {
                await Database.Default.SaveTrackAsync(track.Model);

                track.NotifyModelChanged();

                await UpdateDataAsync(DataLoad.TracksList);

                if (ActiveForm != this) gMapControl.Invalidate();
            }
        }

        private async void TrackDeleteAsync(MapItemTrack track)
        {
            if (track == null) return;

            var name = track.Model.Text;

            if (string.IsNullOrEmpty(name))
            {
                name = track.Model.DateTimeStart.ToString();
            }

            if (Msg.Question(string.Format(Resources.QuestionTrackDelete, name)))
            {
                await Database.Default.DeleteTrackAsync(track.Model);

                tracksOverlay.Routes.Remove(track);

                await UpdateDataAsync(DataLoad.Tiles | DataLoad.TracksInfo | DataLoad.TracksList);
            }
        }

        private Point MenuPopupPoint;

        private void CmMap_Opening(object sender, CancelEventArgs e)
        {
            MenuPopupPoint = gMapControl.PointToClient(MousePosition);
        }

        private void MiMapMarkerAdd_Click(object sender, EventArgs e)
        {
            MarkerAdd(gMapControl.FromLocalToLatLng(MenuPopupPoint.X, MenuPopupPoint.Y));
        }

        private void MiMarkerChange_Click(object sender, EventArgs e)
        {
            MarkerChange(SelectedMarker);
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

        private PointLatLng MarkerMovingPrevPosition;

        private void MiMarkerMove_Click(object sender, EventArgs e)
        {
            if (SelectedMarker == null) return;

            MarkerMoving = true;

            MarkerMovingPrevPosition = SelectedMarker.Position;

            var point = gMapControl.FromLatLngToLocal(SelectedMarker.Position);

            Cursor.Position = gMapControl.PointToScreen(new Point((int)point.X, (int)point.Y));
        }

        private void GMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            var position = gMapControl.FromLocalToLatLng(e.X, e.Y);

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
                    if (Selected != null)
                    {
                        Selected = null;

                        UpdateSelectedTrackTiles(null);

                        gMapControl.Invalidate();
                    }
                    else
                    {
                        FullScreen = false;
                    }
                }
            }
        }

        private async Task UpdateChildFormAsync(FrmList frm)
        {
            if (Utils.IsChildFormExists(frm)) await frm.UpdateDataAsync();
        }

        private async Task UpdateDataAsync(DataLoad load = default)
        {
            Status = ProgramStatus.LoadData;

            Selected = null;

            if (load == default)
            {
                load = DataLoad.Tiles | DataLoad.TracksInfo | DataLoad.TracksList | DataLoad.MarkersList;

                if (miMainShowTracks.Checked) load |= DataLoad.Tracks;

                if (miMainShowMarkers.Checked) load |= DataLoad.Markers;
            }

            Utils.WriteDebug($"Loading data {load}");

            if (load.HasFlag(DataLoad.Tiles)) await LoadTilesAsync();

            if (load.HasFlag(DataLoad.TracksInfo)) await LoadTracksInfoAsync();

            if (load.HasFlag(DataLoad.Tracks)) await LoadTracksAsync(true);

            if (load.HasFlag(DataLoad.Markers)) await LoadMarkersAsync();

            Status = ProgramStatus.Idle;

            if (load.HasFlag(DataLoad.Tracks) ||
                load.HasFlag(DataLoad.TracksList) ||
                load.HasFlag(DataLoad.TracksInfo)) await UpdateChildFormAsync(frmResults);

            if (load.HasFlag(DataLoad.TracksList)) await UpdateChildFormAsync(frmTrackList);

            if (load.HasFlag(DataLoad.MarkersList)) await UpdateChildFormAsync(frmMarkerList);
        }

        private void MiMainDataUpdate_Click(object sender, EventArgs e)
        {
            Status = ProgramStatus.LoadData;

            _ = UpdateDataAsync();

            Status = ProgramStatus.Idle;
        }

        private async void AddTileAsync(Tile tile)
        {
            Status = ProgramStatus.SaveData;

            await Database.Default.SaveTileAsync(tile);

            await UpdateDataAsync(DataLoad.Tiles);
        }

        private void MiMapTileAdd_Click(object sender, EventArgs e)
        {
            var position = gMapControl.FromLocalToLatLng(MenuPopupPoint.X, MenuPopupPoint.Y);

            try
            {
                AddTileAsync(new Tile()
                {
                    X = Utils.Osm.LngToTileX(position),
                    Y = Utils.Osm.LatToTileY(position)
                });
            }
            catch (Exception)
            {
                Msg.Error("tile exists");
            }
        }

        private async void DeleteTileAsync(Tile tile)
        {
            Status = ProgramStatus.SaveData;

            await Database.Default.DeleteTileAsync(tile);

            await UpdateDataAsync(DataLoad.Tiles);
        }

        private void MiMapTileDelete_Click(object sender, EventArgs e)
        {
            var position = gMapControl.FromLocalToLatLng(MenuPopupPoint.X, MenuPopupPoint.Y);

            DeleteTileAsync(new Tile()
            {
                X = Utils.Osm.LngToTileX(position),
                Y = Utils.Osm.LatToTileY(position)
            });
        }

        private void MiMainSaveToImage_Click(object sender, EventArgs e)
        {
            SaveMapToImage();
        }

        private void MiMainSaveTileBoundaryToFile_Click(object sender, EventArgs e)
        {
            SaveTileBoundaryToFile();
        }

        private void MiMainSaveTileStatusToFile_Click(object sender, EventArgs e)
        {
            SaveTileStatusToFile();
        }

        private async Task<Track> OpenTrackFromFileAsync(string path)
        {
            return await Task.Run(() => { return Utils.Tracks.OpenTrackFromFile(path); });
        }

        private async Task<List<Tile>> GetTilesFromTrackAsync(Track track)
        {
            return await Task.Run(() =>
            {
                return Utils.Tiles.GetTilesFromTrack(track);
            });
        }

        private async Task OpenTracksAsync(string[] files)
        {
            Status = ProgramStatus.LoadGpx;

            Track track;

            List<Tile> trackTiles;

            foreach (var file in files)
            {
                track = await OpenTrackFromFileAsync(file);

                Utils.WriteDebug("OpenTrackFromFileAsync done");

                await Database.Default.SaveTrackAsync(track);

                Utils.WriteDebug("SaveTrackAsync done");

                var mapTrack = new MapItemTrack(track);

                tracksOverlay.Routes.Add(mapTrack);

                Selected = mapTrack;

                trackTiles = await GetTilesFromTrackAsync(track);

                Utils.WriteDebug("GetTilesFromTrackAsync done");

                var saveTiles = new List<Tile>();

                int id;

                foreach (var trackTile in trackTiles)
                {
                    id = await Database.Default.ExistsTileAsync(trackTile);

                    if (id == 0)
                    {
                        saveTiles.Add(trackTile);
                    }
                    else
                    {
                        trackTile.Id = id;
                    }
                }

                Utils.WriteDebug("saveTiles count: " + saveTiles.Count);

                await Database.Default.SaveTilesAsync(saveTiles);

                Utils.WriteDebug("SaveTilesAsync done");

                var tracksTiles = new List<TracksTiles>();

                foreach (var tile in trackTiles)
                {
                    tracksTiles.Add(new TracksTiles()
                    {
                        TrackId = track.Id,
                        TileId = tile.Id,
                    });
                }

                Utils.WriteDebug("tracksTiles count: " + tracksTiles.Count);

                await Database.Default.SaveTracksTilesAsync(tracksTiles);

                Utils.WriteDebug("SaveTracksTilesAsync done");
            }

            Status = ProgramStatus.Idle;
        }

        private async Task OpenTracks()
        {
            openFileDialog.FileName = string.Empty;

            if (openFileDialog.ShowDialog(this) != DialogResult.OK) return;

            await OpenTracksAsync(openFileDialog.FileNames);

            await UpdateDataAsync(DataLoad.Tiles | DataLoad.Tracks | DataLoad.TracksInfo | DataLoad.TracksList);
        }

        private void MiMainDataOpenTrack_Click(object sender, EventArgs e)
        {
            _ = OpenTracks();
        }

        private void ShowChildForm(ChildFormType listType, bool show)
        {
            Form frm;

            switch (listType)
            {
                case ChildFormType.Filter:
                    frm = frmFilter;
                    break;
                case ChildFormType.Tracks:
                    frm = frmTrackList;
                    break;
                case ChildFormType.Markers:
                    frm = frmMarkerList;
                    break;
                case ChildFormType.Results:
                    frm = frmResults;
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (show)
            {
                if (!Utils.IsChildFormExists(frm))
                {
                    switch (listType)
                    {
                        case ChildFormType.Filter:
                            FrmFilter.ShowFrm(this);

                            break;
                        case ChildFormType.Tracks:
                        case ChildFormType.Markers:
                        case ChildFormType.Results:
                            FrmList.ShowFrm(this, listType);

                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
            else
            {
                if (Utils.IsChildFormExists(frm))
                {
                    frm.Close();
                }
            }
        }

        private void MiMainDataTrackList_Click(object sender, EventArgs e)
        {
            ShowChildForm(ChildFormType.Tracks, !miMainDataTrackList.Checked);
        }

        private void MiMainDataMarkerList_Click(object sender, EventArgs e)
        {
            ShowChildForm(ChildFormType.Markers, !miMainDataMarkerList.Checked);
        }

        private void MiMainDataResults_Click(object sender, EventArgs e)
        {
            ShowChildForm(ChildFormType.Results, !miMainDataResults.Checked);
        }

        private void MiMainDataFilter_Click(object sender, EventArgs e)
        {
            ShowChildForm(ChildFormType.Filter, !miMainDataFilter.Checked);
        }

        private void MiMainGrayScale_Click(object sender, EventArgs e)
        {
            gMapControl.GrayScaleMode = miMainGrayScale.Checked;
        }

        private IMapItem FindMapItem(BaseId value)
        {
            if (value == null) return null;

            IEnumerable items;

            if (value is Marker)
            {
                items = markersOverlay.Markers;
            }
            else
            {
                if (value is Track)
                {
                    items = tracksOverlay.Routes;
                }
                else
                {
                    return null;
                }
            }

            return items.Cast<IMapItem>().Where(i => i.Model.Id == value.Id).FirstOrDefault();
        }

        public void SelectMapItem(object sender, BaseId value)
        {
            var item = FindMapItem(value);

            if (item != null)
            {
                Selected = item;

                if (ActiveForm != this) gMapControl.Invalidate();
            }
        }

        public void ChangeMapItem(object sender, BaseId value)
        {
            SelectMapItem(sender, value);

            if (value is Marker)
            {
                MarkerChange(SelectedMarker);
            }
            else
            {
                if (value is Track)
                {
                    TrackChangeAsync(SelectedTrack);
                }
            }
        }

        private readonly Tile TileLeftTop = new Tile();
        private readonly Tile TileRightBottom = new Tile();

        public void UpdateGrid()
        {
            timerMapMove.Stop();

            if (gMapControl.Zoom < AppSettings.Default.SaveOsmTileMinZoom || !miMainShowGrid.Checked)
            {
                gridOverlay.IsVisibile = false;

                return;
            }
            else
            {
                gridOverlay.IsVisibile = true;
            }

            var pointLeftTop = gMapControl.FromLocalToLatLng(0, 0);
            var pointRightBottom = gMapControl.FromLocalToLatLng(gMapControl.Width, gMapControl.Height);

            var leftTopX = Utils.Osm.LngToTileX(pointLeftTop);
            var leftTopY = Utils.Osm.LatToTileY(pointLeftTop);

            var rightBottomX = Utils.Osm.LngToTileX(pointRightBottom);
            var rightBottomY = Utils.Osm.LatToTileY(pointRightBottom);

            foreach (var tile in gridOverlay.Polygons.Cast<MapItemTile>().
                Where(t => t.Model.X < TileLeftTop.X || t.Model.Y < TileLeftTop.Y ||
                           t.Model.X > TileRightBottom.X || t.Model.Y > TileRightBottom.Y).ToList())
            {
                gridOverlay.Polygons.Remove(tile);
            }

            for (var x = leftTopX; x <= rightBottomX; x++)
                for (var y = leftTopY; y <= rightBottomY; y++)
                {
                    if (x >= TileLeftTop.X && x <= TileRightBottom.X && y >= TileLeftTop.Y && y <= TileRightBottom.Y) continue;

                    if (tilesOverlay.Polygons.Cast<MapItemTile>().ToList().
                        Exists(t => t.Model.X == x && t.Model.Y == y)) continue;

                    gridOverlay.Polygons.Add(new MapItemTile(new Tile()
                    {
                        X = x,
                        Y = y,
                    }));
                }

            TileLeftTop.X = leftTopX;
            TileLeftTop.Y = leftTopY;

            TileRightBottom.X = rightBottomX;
            TileRightBottom.Y = rightBottomY;
        }

        private void TimerMapMove_Tick(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        private void MiMainShowGrid_Click(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        private void UpdateDatabaseFileName()
        {
            var databaseHome = AppSettings.Default.DatabaseHome;

            if (string.IsNullOrEmpty(databaseHome))
            {
                databaseHome =
#if DEBUG
                    Files.ExecutableDirectory();
#else
                    Files.AppDataDirectory();
#endif
            }

            var databaseFileName = Path.Combine(databaseHome, Files.DatabaseFileName());

            Utils.WriteDebug("database: " + databaseFileName);

            Database.Default.FileName = databaseFileName;
        }

        private void MiMainSettings_Click(object sender, EventArgs e)
        {
            if (FrmSettings.ShowDlg(this))
            {
                UpdateDatabaseFileName();

                frmResults?.UpdateSettings();
                frmTrackList?.UpdateSettings();
                frmMarkerList?.UpdateSettings();

                _ = UpdateDataAsync();
            }
        }

        private void TsbtnResults_Click(object sender, EventArgs e)
        {
            miMainDataResults.PerformClick();
        }

        private void TsbtnTrackList_Click(object sender, EventArgs e)
        {
            miMainDataTrackList.PerformClick();
        }

        private void TsbtnMarkerList_Click(object sender, EventArgs e)
        {
            miMainDataMarkerList.PerformClick();
        }

        private void TsbtnFilter_Click(object sender, EventArgs e)
        {
            miMainDataFilter.PerformClick();
        }

        private void MiMainLeftPanel_Click(object sender, EventArgs e)
        {
            toolStripContainer.LeftToolStripPanelVisible = miMainLeftPanel.Checked;
        }

        public void ChildFormOpened(object sender)
        {
            switch (((IChildForm)sender).ListType)
            {
                case ChildFormType.Filter:
                    frmFilter = (FrmFilter)sender;
                    miMainDataFilter.Checked = true;
                    break;
                case ChildFormType.Tracks:
                    frmTrackList = (FrmList)sender;
                    miMainDataTrackList.Checked = true;
                    break;
                case ChildFormType.Markers:
                    frmMarkerList = (FrmList)sender;
                    miMainDataMarkerList.Checked = true;
                    break;
                case ChildFormType.Results:
                    frmResults = (FrmList)sender;
                    miMainDataResults.Checked = true;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void ChildFormClosed(object sender)
        {
            switch (((IChildForm)sender).ListType)
            {
                case ChildFormType.Filter:
                    frmFilter = null;
                    miMainDataFilter.Checked = false;
                    break;
                case ChildFormType.Tracks:
                    frmTrackList = null;
                    miMainDataTrackList.Checked = false;
                    break;
                case ChildFormType.Markers:
                    frmMarkerList = null;
                    miMainDataMarkerList.Checked = false;
                    break;
                case ChildFormType.Results:
                    frmResults = null;
                    miMainDataResults.Checked = false;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void OsmAction(bool open)
        {
            var zoom = gMapControl.Zoom;

            if (!open && zoom < Const.OSM_EDIT_MIN_ZOOM)
            {
                if (Msg.Question(Resources.QuestionOsmSetEditZoom))
                {
                    zoom = Const.OSM_EDIT_MIN_ZOOM;
                }
            }

            var url = string.Format(open ? Resources.OsmUrlOpen : Resources.OsmUrlEdit,
                zoom,
                gMapControl.Position.Lat.ToString(CultureInfo.InvariantCulture),
                gMapControl.Position.Lng.ToString(CultureInfo.InvariantCulture));

            Process.Start(url);
        }

        private void MiMainOsmOpen_Click(object sender, EventArgs e)
        {
            OsmAction(true);
        }

        private void MiMainOsmEdit_Click(object sender, EventArgs e)
        {
            OsmAction(false);
        }
    }
}