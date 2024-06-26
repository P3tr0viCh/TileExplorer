﻿//#define CHECK_TILES
//#define SHOW_TRACK_KM
//#define DUMMY_TILES

using GMap.NET;
using GMap.NET.WindowsForms;
using P3tr0viCh.AppUpdate;
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

namespace TileExplorer
{
    public partial class Main : Form, IStatusStripView, IMainForm
    {
        private readonly GMapOverlay gridOverlay = new GMapOverlay("grid");
        private readonly GMapOverlay tilesOverlay = new GMapOverlay("tiles");
        private readonly GMapOverlay tracksOverlay = new GMapOverlay("tracks");
        private readonly GMapOverlay markersOverlay = new GMapOverlay("markers");

        private readonly StatusStripPresenter statusStripPresenter;

        private readonly ProgramStatus status = new ProgramStatus();
        public ProgramStatus ProgramStatus { get { return status; } }

        public Main()
        {
            InitializeComponent();

            statusStripPresenter = new StatusStripPresenter(this);

            AppSettings.Directory =
#if DEBUG
                 Files.ExecutableDirectory();
#else
                Files.AppDataDirectory();
#endif

            DebugWrite.Line("Settings: " + AppSettings.FilePath, "Main");
        }

        private void Filter_OnChanged()
        {
            var load = DataLoad.Tiles | DataLoad.TracksInfo | DataLoad.TrackList;

            if (miMainShowTracks.Checked) load |= DataLoad.Tracks;

            _ = UpdateDataAsync(load);
        }

        private bool AbnormalExit
        {
            get
            {
                return Tag != null && (bool)Tag;
            }
            set
            {
                Tag = value;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            AppSettingsLoad();

            GMapLoad();

            if (!SetDatabaseFileName())
            {
                WindowState = FormWindowState.Minimized;
                AbnormalExit = true;
                Application.Exit();
                return;
            }

            Database.Filter.Default.Day = AppSettings.Default.Filter.Day;
            Database.Filter.Default.DateFrom = AppSettings.Default.Filter.DateFrom;
            Database.Filter.Default.DateTo = AppSettings.Default.Filter.DateTo;
            Database.Filter.Default.Years = AppSettings.Default.Filter.Years;

            Database.Filter.Default.OnChanged += Filter_OnChanged;

            ProgramStatus.StatusChanged += ProgramStatus_StatusChanged;

            UpdateApp.Default.StatusChanged += UpdateApp_StatusChanged;

            AppSettings.LoadFormState(this, AppSettings.Default.FormStateMain);

#if DEBUG && DUMMY_TILES
            DummyTiles();
#endif

#if !DEBUG
            miMapTileAdd.Visible = false;
            miMapTileDelete.Visible = false;
#endif
            miMainShowGrid.Checked = AppSettings.Default.VisibleGrid;
            miMainShowTiles.Checked = AppSettings.Default.VisibleTiles;
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

            var starting = ProgramStatus.Start(Status.Starting);

            if (AppSettings.Default.VisibleResultYears)
            {
                ShowChildForm(ChildFormType.ResultYears, true);
            }
            if (AppSettings.Default.VisibleResultEquipments)
            {
                ShowChildForm(ChildFormType.ResultEquipments, true);
            }
            if (AppSettings.Default.VisibleTrackList)
            {
                ShowChildForm(ChildFormType.TrackList, true);
            }
            if (AppSettings.Default.VisibleMarkerList)
            {
                ShowChildForm(ChildFormType.MarkerList, true);
            }
            if (AppSettings.Default.VisibleEquipmentList)
            {
                ShowChildForm(ChildFormType.EquipmentList, true);
            }
            if (AppSettings.Default.VisibleFilter)
            {
                ShowChildForm(ChildFormType.Filter, true);
            }
            if (AppSettings.Default.VisibleTracksTree)
            {
                ShowChildForm(ChildFormType.TracksTree, true);
            }

            ProgramStatus.Stop(starting);

            UpdateData();
        }

        private async void UpdateData()
        {
            await Task.Run(() =>
            {
                this.InvokeIfNeeded(() => _ = UpdateDataAsync());
            });
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AbnormalExit) return;

            if (!UpdateApp.Default.CanClose())
            {
                if (!Msg.Question(Resources.AppUpdateQuestionInProgress))
                {
                    e.Cancel = true;

                    return;
                }
            }

            FullScreen = false;

            AppSettings.Default.FormStateMain = AppSettings.SaveFormState(this);

            AppSettings.Default.MapGrayScale = miMainGrayScale.Checked;

            AppSettings.Default.VisibleGrid = miMainShowGrid.Checked;
            AppSettings.Default.VisibleTiles = miMainShowTiles.Checked;
            AppSettings.Default.VisibleTracks = miMainShowTracks.Checked;
            AppSettings.Default.VisibleMarkers = miMainShowMarkers.Checked;

            AppSettings.Default.VisibleFilter = ChildFormMenuItemState(ChildFormType.Filter).Checked;

            AppSettings.Default.VisibleResultYears = ChildFormMenuItemState(ChildFormType.ResultYears).Checked;
            AppSettings.Default.VisibleResultEquipments = ChildFormMenuItemState(ChildFormType.ResultEquipments).Checked;

            AppSettings.Default.VisibleTrackList = ChildFormMenuItemState(ChildFormType.TrackList).Checked;
            AppSettings.Default.VisibleMarkerList = ChildFormMenuItemState(ChildFormType.MarkerList).Checked;
            AppSettings.Default.VisibleEquipmentList = ChildFormMenuItemState(ChildFormType.EquipmentList).Checked;

            AppSettings.Default.VisibleTracksTree = ChildFormMenuItemState(ChildFormType.TracksTree).Checked;

            AppSettings.Default.VisibleLeftPanel = miMainLeftPanel.Checked;

            AppSettings.Default.Filter.Day = Database.Filter.Default.Day;
            AppSettings.Default.Filter.DateFrom = Database.Filter.Default.DateFrom;
            AppSettings.Default.Filter.DateTo = Database.Filter.Default.DateTo;
            AppSettings.Default.Filter.Years = Database.Filter.Default.Years;

            AppSettingsSave();
        }

        public void AppSettingsLoad()
        {
            if (AppSettings.Load()) return;

            DebugWrite.Error(AppSettings.LastError);
        }

        public void AppSettingsSave()
        {
            if (AppSettings.Save()) return;

            DebugWrite.Error(AppSettings.LastError);
        }

        private void GMapLoad()
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            GMap.NET.MapProviders.GMapProvider.UserAgent = Utils.AssemblyNameAndVersion();

            DebugWrite.Line("useragent: " + GMap.NET.MapProviders.GMapProvider.UserAgent);

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

        private void ProgramStatus_StatusChanged(object sender, Status status)
        {
            statusStripPresenter.Status = status.Description();
        }

        private void UpdateApp_StatusChanged(object sender, UpdateStatus status)
        {
            DebugWrite.Line(status.ToString());

            switch (status)
            {
                case UpdateStatus.CheckLatest:
                    statusStripPresenter.UpdateStatus = Resources.AppUpdateInfoStatusCheckLatest;

                    break;
                case UpdateStatus.Download:
                    statusStripPresenter.UpdateStatus = Resources.AppUpdateInfoStatusDownload;

                    break;
                case UpdateStatus.ArchiveExtract:
                    statusStripPresenter.UpdateStatus = Resources.AppUpdateInfoStatusExtract;

                    break;
                case UpdateStatus.Check:
                case UpdateStatus.CheckLocal:
                case UpdateStatus.Update:
                case UpdateStatus.Idle:
                default:
                    statusStripPresenter.UpdateStatus = string.Empty;
                    break;
            }
        }

        public ToolStripStatusLabel GetLabel(StatusLabel label)
        {
            switch (label)
            {
                case StatusLabel.Zoom: return slZoom;
                case StatusLabel.TileId: return slTileId;
                case StatusLabel.Position: return slPosition;
                case StatusLabel.MousePosition: return slMousePosition;
                case StatusLabel.Status: return slStatus;
                case StatusLabel.UpdateStatus: return slUpdateStatus;
                case StatusLabel.TracksCount: return slTracksCount;
                case StatusLabel.TracksDistance: return slTracksDistance;
                case StatusLabel.TilesVisited: return slTilesVisited;
                case StatusLabel.TilesMaxCluster: return slTilesMaxCluster;
                case StatusLabel.TilesMaxSquare: return slTilesMaxSquare;
                default: throw new ArgumentOutOfRangeException();
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
            DebugWrite.Line("start");

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

            DebugWrite.Line("end");
        }

        private async Task LoadTracksAsync()
        {
            DebugWrite.Line("start");

            tracksOverlay.Clear();

            var tracks = await Database.Default.ListLoadAsync<Track>();

            foreach (var track in tracks)
            {
                track.TrackPoints = await Database.Default.ListLoadAsync<TrackPoint>(track);

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

                    DebugWrite.Line(Geo.Haversine(lat1, lng1, lat2, lng2));
                    
                    lat1 = lat2;
                    lng1 = lng2;
                }
#endif
            }

            DebugWrite.Line("end");
        }

        private async Task LoadMarkersAsync()
        {
            DebugWrite.Line("start");

            markersOverlay.Clear();

            var markers = await Database.Default.ListLoadAsync<Marker>();

            foreach (var marker in markers)
            {
                markersOverlay.Markers.Add(new MapItemMarker(marker));
            }

            DebugWrite.Line("end");
        }

        private async Task LoadTracksInfoAsync()
        {
            DebugWrite.Line("start");

            var tracksInfo = await Database.Default.LoadTracksInfoAsync(Database.Filter.Default);

            statusStripPresenter.TracksCount = tracksInfo.Count;
            statusStripPresenter.TracksDistance = tracksInfo.Distance / 1000.0;

            DebugWrite.Line("end");
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

        private void UpdateSelectedTrackTiles(Track track)
        {
            var status = ProgramStatus.Start(Status.LoadData);

            try
            {
                foreach (var tile in tilesOverlay.Polygons.Cast<MapItemTile>())
                {
                    tile.Selected = false;
                }

                gMapControl.Invalidate();

                if (track == null) return;

                var tiles = Task.Run(() =>
                {
                    return Database.Default.ListLoadAsync<Tile>(track);
                }).Result;

                foreach (var tile in from item in tilesOverlay.Polygons.Cast<MapItemTile>()
                                     from tile in tiles
                                     where item.Model.X == tile.X && item.Model.Y == tile.Y
                                     select item)
                {
                    tile.Selected = true;
                }

                if (ActiveForm != this) gMapControl.Invalidate();
            }
            finally
            {
                ProgramStatus.Stop(status);
            }
        }

        private bool MarkerMoving;

        private IMapItem selected = null;

        private void ChildFormSetSelected(ChildFormType childFormType, BaseId baseId)
        {
            foreach (var frm in Application.OpenForms)
            {
                if (frm is IChildForm form && form.ChildFormType == childFormType)
                {
                    ((IListForm)form).SetSelected(baseId);
                }
            }
        }

        public IMapItem Selected
        {
            get
            {
                return selected;
            }
            set
            {
                if (selected == null)
                {
                    SelectedTrackTiles = null;
                }

                if (selected == value) return;

                if (selected != null)
                {
                    selected.Selected = false;
                }

                selected = value;

                if (selected == null)
                {
                    gMapControl.Invalidate();
                    return;
                }

                selected.Selected = true;

                switch (selected.Type)
                {
                    case MapItemType.Marker:
                        SelectedTrackTiles = null;

                        ChildFormSetSelected(ChildFormType.MarkerList,
                            selected.Model);

                        break;
                    case MapItemType.Track:
                        SelectedTrackTiles = selected.Model as Track;

                        ChildFormSetSelected(ChildFormType.TrackList,
                            selected.Model);

                        break;
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

        public Track SelectedTrackTiles
        {
            set
            {
                UpdateSelectedTrackTiles(value);
            }
        }

        private IMapItem MouseOverMapItem()
        {
            if (gMapControl.IsMouseOverMarker)
            {
                return markersOverlay.Markers.Cast<MapItemMarker>()
                .Where(m => m.IsMouseOver).FirstOrDefault();
            }

            if (gMapControl.IsMouseOverRoute)
            {
                return tracksOverlay.Routes.Cast<MapItemTrack>()
                .Where(m => m.IsMouseOver).FirstOrDefault();
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
                    await Database.Default.MarkerSaveAsync(SelectedMarker.Model);

                    await UpdateDataAsync(DataLoad.MarkerList);
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
                            MarkerChange(SelectedMarker.Model);
                            break;
                        case MapItemType.Track:
                            TrackChange(SelectedTrack.Model);
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

        private void MiMainShowTiles_Click(object sender, EventArgs e)
        {
            tilesOverlay.IsVisibile = miMainShowTiles.Checked;
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
                _ = UpdateDataAsync(DataLoad.MarkerList);
            }

            markersOverlay.Markers.Remove(markerTemp);

            markersOverlay.IsVisibile = prevMarkersVisible;
        }

        private void MarkerChange(Marker marker)
        {
            if (marker == null) return;

            FrmMarker.ShowDlg(this, marker);

            SelectMapItem(this, marker);
        }

        public void MarkerChanged(Marker marker)
        {
            var mapItem = FindMapItem(marker);

            if (mapItem == null)
            {
                markersOverlay.Markers.Remove(markerTemp);

                markersOverlay.Markers.Add(new MapItemMarker(marker));

                _ = UpdateDataAsync(DataLoad.MarkerList);
            }
            else
            {
                mapItem.Model = marker;

                mapItem.NotifyModelChanged();

                _ = UpdateDataAsync(DataLoad.MarkerList);

                if (ActiveForm != this) gMapControl.Invalidate();
            }

            SelectMapItem(this, marker);
        }

        private async void MarkerDeleteAsync(Marker marker)
        {
            if (marker == null) return;

            var name = marker.Text;

            if (name.IsEmpty())
            {
                name = marker.Lat.ToString() + ":" + marker.Lng.ToString();
            }

            if (Msg.Question(string.Format(Resources.QuestionMarkerDelete, name)))
            {
                await Database.Default.MarkerDeleteAsync(marker);

                markersOverlay.Markers.Remove(
                    markersOverlay.Markers.Cast<MapItemMarker>()
                    .Where(t => t.Model.Id == marker.Id).SingleOrDefault()
                );

                await UpdateDataAsync(DataLoad.MarkerList);

                if (ActiveForm != this) gMapControl.Invalidate();
            }
        }

        private void TrackChange(Track track)
        {
            if (track == null) return;

            FrmTrack.ShowDlg(this, track);

            SelectMapItem(this, track);
        }

        public void TrackChanged(Track track)
        {
            _ = UpdateDataAsync(DataLoad.TrackList);

            if (ActiveForm != this) gMapControl.Invalidate();
        }

        private async void TrackDeleteAsync(Track track)
        {
            if (track == null) return;

            var name = track.Text;

            if (name.IsEmpty())
            {
                name = track.DateTimeStart.ToString();
            }

            if (Msg.Question(string.Format(Resources.QuestionTrackDelete, name)))
            {
                await Database.Default.DeleteTrackAsync(track);

                tracksOverlay.Routes.Remove(
                    tracksOverlay.Routes.Cast<MapItemTrack>()
                    .Where(t => t.Model.Id == track.Id).SingleOrDefault()
                );

                await UpdateDataAsync(DataLoad.Tiles | DataLoad.TracksInfo | DataLoad.TrackList);
            }
        }

        private PointLatLng MenuPopupPointLatLng;

        private void CmMap_Opening(object sender, CancelEventArgs e)
        {
            var MenuPopupPoint = gMapControl.PointToClient(MousePosition);
            MenuPopupPointLatLng = gMapControl.FromLocalToLatLng(MenuPopupPoint.X, MenuPopupPoint.Y);

            UpdateCopyCoords();
        }

        private void MiMapMarkerAdd_Click(object sender, EventArgs e)
        {
            MarkerAdd(MenuPopupPointLatLng);
        }

        private void MiMarkerChange_Click(object sender, EventArgs e)
        {
            MarkerChange(SelectedMarker.Model);
        }

        private void MiMarkerDelete_Click(object sender, EventArgs e)
        {
            MarkerDeleteAsync(SelectedMarker.Model);
        }

        private void MiTrackChange_Click(object sender, EventArgs e)
        {
            TrackChange(SelectedTrack.Model);
        }

        private void MiTrackDelete_Click(object sender, EventArgs e)
        {
            TrackDeleteAsync(SelectedTrack.Model);
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
                    SelectedTrackTiles = null;

                    if (Selected != null)
                    {
                        Selected = null;

                        gMapControl.Invalidate();
                    }
                    else
                    {
                        FullScreen = false;
                    }
                }
            }
        }

        private async Task UpdateDataAsync(DataLoad load = default)
        {
            var status = ProgramStatus.Start(Status.LoadData);

            Selected = null;

            if (load == default)
            {
                load = DataLoad.Tiles |
                       DataLoad.TracksInfo |
                       DataLoad.TrackList |
                       DataLoad.MarkerList |
                       DataLoad.EquipmentList |
                       DataLoad.TracksTree;

                if (miMainShowTracks.Checked) load |= DataLoad.Tracks;

                if (miMainShowMarkers.Checked) load |= DataLoad.Markers;
            }

            DebugWrite.Line($"Loading data {load}");

            if (load.HasFlag(DataLoad.Tiles)) await LoadTilesAsync();

            if (load.HasFlag(DataLoad.TracksInfo)) await LoadTracksInfoAsync();

            if (load.HasFlag(DataLoad.Tracks)) await LoadTracksAsync();

            if (load.HasFlag(DataLoad.Markers)) await LoadMarkersAsync();

            ProgramStatus.Stop(status);

            bool updateData;

            foreach (var frm in Application.OpenForms)
            {
                if (frm is IChildForm frmChild && frm is IUpdateDataForm frmUpdateData)
                {
                    switch (frmChild.ChildFormType)
                    {
                        case ChildFormType.ResultYears:
                        case ChildFormType.ResultEquipments:
                        case ChildFormType.TileInfo:
                            updateData = load.HasFlag(DataLoad.Tracks) ||
                                         load.HasFlag(DataLoad.TrackList) ||
                                         load.HasFlag(DataLoad.TracksInfo);

                            break;
                        case ChildFormType.TrackList:
                            updateData = load.HasFlag(DataLoad.TrackList) ||
                                         load.HasFlag(DataLoad.EquipmentList);

                            break;
                        case ChildFormType.MarkerList:
                            updateData = load.HasFlag(DataLoad.MarkerList);

                            break;
                        case ChildFormType.EquipmentList:
                            updateData = load.HasFlag(DataLoad.EquipmentList) ||
                                         load.HasFlag(DataLoad.TrackList);

                            break;
                        case ChildFormType.TracksTree:
                            updateData = load.HasFlag(DataLoad.TracksTree);

                            break;
                        default:
                            updateData = false;

                            break;
                    }

                    if (updateData)
                    {
                        frmUpdateData.UpdateData();
                    }
                }
            }
        }

        private async Task UpdateUpdateTrackMinDistancePointAsync()
        {
            var status = ProgramStatus.Start(Status.SaveData);

            try
            {
                DebugWrite.Line("start");

                var tracks = await Database.Default.ListLoadAsync<Track>(new { onlyTrack = true });

                foreach (var track in tracks)
                {
                    track.TrackPoints = await Database.Default.ListLoadAsync<TrackPoint>(new { track, full = true });

                    Utils.Tracks.UpdateTrackMinDistancePoint(track);

                    await Database.Default.UpdateTrackMinDistancePointAsync(track);
                }

                DebugWrite.Line("end");
            }
            finally
            {
                ProgramStatus.Stop(status);
            }
        }

        private void MiMainDataUpdate_Click(object sender, EventArgs e)
        {
            if (ProgramStatus.Current != Status.Idle) return;

            UpdateData();
        }

        private async void AddTileAsync()
        {
            var status = ProgramStatus.Start(Status.SaveData);

            try
            {
                var tile = new Tile(MenuPopupPointLatLng);

                if (await Database.Default.GetTileIdByXYAsync(tile) == 0)
                {
                    await Database.Default.TileSaveAsync(tile);

                    await UpdateDataAsync(DataLoad.Tiles);
                }
                else
                {
                    Msg.Error("tile exists");
                }
            }
            finally
            {
                ProgramStatus.Stop(status);
            }
        }

        private void MiMapTileAdd_Click(object sender, EventArgs e)
        {
            AddTileAsync();
        }

        private async void DeleteTileAsync()
        {
            var status = ProgramStatus.Start(Status.SaveData);

            try
            {
                var tile = new Tile(MenuPopupPointLatLng);

                tile.Id = await Database.Default.GetTileIdByXYAsync(tile);

                if (tile.Id <= Sql.NewId) return;

                await Database.Default.TileDeleteAsync(tile);

                await UpdateDataAsync(DataLoad.Tiles);
            }
            finally
            {
                ProgramStatus.Stop(status);
            }
        }

        private void MiMapTileDelete_Click(object sender, EventArgs e)
        {
            DeleteTileAsync();
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
            var status = ProgramStatus.Start(Status.LoadGpx);

            Track track;

            List<Tile> trackTiles;

            try
            {
                var showDlg = true;

                var canToAll = files.Count() > 1;

                Equipment equipmentToAll = null;

                var exitForEach = false;

                foreach (var file in files)
                {
                    track = await OpenTrackFromFileAsync(file);

                    DebugWrite.Line("OpenTrackFromFileAsync done");

                    if (showDlg)
                    {
                        switch (FrmTrack.ShowDlg(this, track, canToAll))
                        {
                            case DialogResult.Cancel:
                                continue;
                            case DialogResult.Abort:
                                exitForEach = true;

                                break;
                            case DialogResult.Yes:
                                showDlg = false;

                                equipmentToAll = track.Equipment;

                                break;
                        }
                    }
                    else
                    {
                        track.Equipment = equipmentToAll;

                        Task.Run(() => Database.Default.SaveTrackAsync(track)).Wait();

                        TrackChanged(track);
                    }

                    if (exitForEach)
                    {
                        break;
                    }

                    var mapTrack = new MapItemTrack(track);

                    tracksOverlay.Routes.Add(mapTrack);

                    Selected = mapTrack;

                    trackTiles = await GetTilesFromTrackAsync(track);

                    DebugWrite.Line("GetTilesFromTrackAsync done");

                    var saveTiles = new List<Tile>();

                    int id;

                    foreach (var trackTile in trackTiles)
                    {
                        id = await Database.Default.GetTileIdByXYAsync(trackTile);

                        if (id == 0)
                        {
                            saveTiles.Add(trackTile);
                        }
                        else
                        {
                            trackTile.Id = id;
                        }
                    }

                    DebugWrite.Line("saveTiles count: " + saveTiles.Count);

                    await Database.Default.SaveTilesAsync(saveTiles);

                    DebugWrite.Line("SaveTilesAsync done");

                    var tracksTiles = new List<TracksTiles>();

                    foreach (var tile in trackTiles)
                    {
                        tracksTiles.Add(new TracksTiles()
                        {
                            TrackId = track.Id,
                            TileId = tile.Id,
                        });
                    }

                    DebugWrite.Line("tracksTiles count: " + tracksTiles.Count);

                    await Database.Default.SaveTracksTilesAsync(tracksTiles);

                    DebugWrite.Line("SaveTracksTilesAsync done");
                }
            }
            finally
            {
                ProgramStatus.Stop(status);
            }
        }

        private async Task OpenTracksAsync()
        {
            openFileDialog.FileName = string.Empty;

            if (openFileDialog.ShowDialog(this) != DialogResult.OK) return;

            await OpenTracksAsync(openFileDialog.FileNames);

            await UpdateDataAsync(DataLoad.Tiles | DataLoad.Tracks |
                DataLoad.TracksInfo | DataLoad.TrackList | DataLoad.TracksTree);
        }

        private void MiMainDataOpenTrack_Click(object sender, EventArgs e)
        {
            _ = OpenTracksAsync();
        }

        private void ShowChildForm(ChildFormType childFormType, bool show)
        {
            if (show)
            {
                if (!ChildFormMenuItemState(childFormType).Checked)
                {
                    switch (childFormType)
                    {
                        case ChildFormType.Filter:
                            FrmFilter.ShowFrm(this);

                            break;
                        case ChildFormType.TracksTree:
                            FrmTracksTree.ShowFrm(this);

                            break;
                        case ChildFormType.TrackList:
                        case ChildFormType.MarkerList:
                        case ChildFormType.ResultYears:
                        case ChildFormType.ResultEquipments:
                        case ChildFormType.EquipmentList:
                            FrmList.ShowFrm(this, childFormType);

                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
            else
            {
                for (var i = Application.OpenForms.Count - 1; i > 0; i--)
                {
                    if (Application.OpenForms[i] is IChildForm form &&
                        form.ChildFormType == childFormType)
                    {
                        Application.OpenForms[i].Close();
                    }
                }
            }
        }

        private void MiMainDataTrackList_Click(object sender, EventArgs e)
        {
            ShowChildForm(ChildFormType.TrackList, !miMainDataTrackList.Checked);
        }

        private void MiMainDataMarkerList_Click(object sender, EventArgs e)
        {
            ShowChildForm(ChildFormType.MarkerList, !miMainDataMarkerList.Checked);
        }

        private void MiMainDataEquipmentList_Click(object sender, EventArgs e)
        {
            ShowChildForm(ChildFormType.EquipmentList, !miMainDataEquipmentList.Checked);
        }

        private void MiMainDataResults_Click(object sender, EventArgs e)
        {

        }

        private void MiMainDataFilter_Click(object sender, EventArgs e)
        {
            ShowChildForm(ChildFormType.Filter, !miMainDataFilter.Checked);
        }

        private void MiMainDataTracksTree_Click(object sender, EventArgs e)
        {
            ShowChildForm(ChildFormType.TracksTree, !miMainDataTracksTree.Checked);
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
            else
            {
                Selected = null;

                SelectedTrackTiles = value is Track ? value as Track : null;
            }
        }

        private async void EquipmentChangeAsync(Equipment equipment)
        {
            if (FrmEquipment.ShowDlg(this, equipment))
            {
                await UpdateDataAsync(DataLoad.EquipmentList);
            }
        }

        private async void EquipmentDeleteAsync(Equipment equipment)
        {
            if (equipment == null) return;

            var name = equipment.Name;

            if (Msg.Question(string.Format(Resources.QuestionEquipmentDelete, name)))
            {
                await Database.Default.EquipmentDeleteAsync(equipment);

                await UpdateDataAsync(DataLoad.EquipmentList);
            }
        }

        public void ListItemAdd(object sender, BaseId value)
        {
            if (value is Marker)
            {
                MarkerAdd(gMapControl.Position);
                return;
            }

            if (value is Track)
            {
                _ = OpenTracksAsync();
                return;
            }

            if (value is Equipment)
            {
                EquipmentChangeAsync(value as Equipment);
                return;
            }
        }

        public void ListItemChange(object sender, BaseId value)
        {
            if (value is Marker)
            {
                MarkerChange(value as Marker);
                return;
            }

            if (value is Track)
            {
                TrackChange(value as Track);
                return;
            }

            if (value is Equipment)
            {
                EquipmentChangeAsync(value as Equipment);
                return;
            }
        }

        public void ListItemDelete(object sender, BaseId value)
        {
            if (value is Track)
            {
                TrackDeleteAsync(value as Track);
                return;
            }

            if (value is Marker)
            {
                MarkerDeleteAsync(value as Marker);
                return;
            }

            if (value is Equipment)
            {
                EquipmentDeleteAsync(value as Equipment);
                return;
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

                    gridOverlay.Polygons.Add(new MapItemTile(new Tile(x, y)));
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

        private bool SetDatabaseFileName()
        {
            var databaseHome = AppSettings.Default.DatabaseHome;

            var defaultDatabaseHome =
#if DEBUG
                Files.ExecutableDirectory();
#else
                Files.AppDataDirectory();
#endif
            if (databaseHome.IsEmpty())
            {
                databaseHome = defaultDatabaseHome;
            }

            if (!Directory.Exists(databaseHome))
            {
                DebugWrite.Error("database directory not exists: " + databaseHome);

                if (Msg.Question(Resources.ErrorDatabaseDirectoryNotExists, databaseHome, defaultDatabaseHome))
                {
                    AppSettings.Default.DatabaseHome = string.Empty;
                    return SetDatabaseFileName();
                }

                return false;
            }

            var databaseFileName = Path.Combine(databaseHome, Files.DatabaseFileName());

            DebugWrite.Line("database: " + databaseFileName);

            Database.Default.FileName = databaseFileName;

            return true;
        }

        private async void ShowSettingsAsync()
        {
            int TrackMinDistancePoint = AppSettings.Default.TrackMinDistancePoint;

            if (FrmSettings.ShowDlg(this))
            {
                if (!SetDatabaseFileName())
                {
                    Application.Exit();
                    return;
                }

                DataGridViewCellStyles.UpdateSettings();

                foreach (var frm in Application.OpenForms)
                {
                    if (frm is IChildForm form)
                    {
                        form.UpdateSettings();
                    }
                }

                if (TrackMinDistancePoint != AppSettings.Default.TrackMinDistancePoint)
                {
                    if (Msg.Question(Resources.QuestionUpdateTrackMinDistancePoint))
                    {
                        await UpdateUpdateTrackMinDistancePointAsync();
                    }
                }

                await UpdateDataAsync();
            }
        }

        private void MiMainSettings_Click(object sender, EventArgs e)
        {
            ShowSettingsAsync();
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

        private void TsbtnTracksTree_Click(object sender, EventArgs e)
        {
            miMainDataTracksTree.PerformClick();
        }

        private void MiMainLeftPanel_Click(object sender, EventArgs e)
        {
            toolStripContainer.LeftToolStripPanelVisible = miMainLeftPanel.Checked;
        }

        public ToolStripMenuItem ChildFormMenuItemState(ChildFormType childFormType)
        {
            switch (childFormType)
            {
                case ChildFormType.Filter:
                    return miMainDataFilter;
                case ChildFormType.TrackList:
                    return miMainDataTrackList;
                case ChildFormType.MarkerList:
                    return miMainDataMarkerList;
                case ChildFormType.ResultYears:
                    return miMainDataResultYears;
                case ChildFormType.ResultEquipments:
                    return miMainDataResultEquipments;
                case ChildFormType.EquipmentList:
                    return miMainDataEquipmentList;
                case ChildFormType.TracksTree:
                    return miMainDataTracksTree;
                default:
                    throw new NotImplementedException();
            }
        }

        public void ChildFormOpened(object sender)
        {
            ChildFormMenuItemState(((IChildForm)sender).ChildFormType).Checked = true;
        }

        public void ChildFormClosed(object sender)
        {
            Selected = null;
            ChildFormMenuItemState(((IChildForm)sender).ChildFormType).Checked = false;
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

        private async void ShowTileInfoAsync(PointLatLng position)
        {
            var tile = new Tile(position);

            tile.Id = await Database.Default.GetTileIdByXYAsync(tile);

            if (tile.Id <= Sql.NewId)
            {
                Msg.Info(Resources.MsgTileNotVisited);

                return;
            }

            FrmTileInfo.ShowFrm(this, tile);
        }

        private void ShowTileInfo()
        {
            ShowTileInfoAsync(MenuPopupPointLatLng);
        }

        private void MiMapShowTileInfo_Click(object sender, EventArgs e)
        {
            ShowTileInfo();
        }

        private void MiMainDataResultYears_Click(object sender, EventArgs e)
        {
            ShowChildForm(ChildFormType.ResultYears, !miMainDataResultYears.Checked);
        }

        private void MiMainDataResultEquipments_Click(object sender, EventArgs e)
        {
            ShowChildForm(ChildFormType.ResultEquipments, !miMainDataResultEquipments.Checked);
        }

        private void TsbtnResults_Click(object sender, EventArgs e)
        {
            if (ChildFormMenuItemState(ChildFormType.ResultYears).Checked &&
                ChildFormMenuItemState(ChildFormType.ResultEquipments).Checked)
            {
                ShowChildForm(ChildFormType.ResultYears, false);
                ShowChildForm(ChildFormType.ResultEquipments, false);
            }
            else
            {
                ShowChildForm(ChildFormType.ResultYears, true);
                ShowChildForm(ChildFormType.ResultEquipments, true);
            }
        }

        private void UpdateCopyCoords()
        {
            miMapCopyCoordsFloatLat.Text =
                MenuPopupPointLatLng.Lat.ToString(AppSettings.Default.FormatLatLng);
            miMapCopyCoordsFloatLng.Text =
                MenuPopupPointLatLng.Lng.ToString(AppSettings.Default.FormatLatLng);
            miMapCopyCoordsFloat.Text = miMapCopyCoordsFloatLat.Text + ", " +
                miMapCopyCoordsFloatLng.Text;

            miMapCopyCoordsFloat2Lat.Text =
                MenuPopupPointLatLng.Lat.ToString(AppSettings.Default.FormatLatLng, CultureInfo.InvariantCulture);
            miMapCopyCoordsFloat2Lng.Text =
                MenuPopupPointLatLng.Lng.ToString(AppSettings.Default.FormatLatLng, CultureInfo.InvariantCulture);
            miMapCopyCoordsFloat2.Text = miMapCopyCoordsFloat2Lat.Text + " " +
                miMapCopyCoordsFloat2Lng.Text;
        }

        private void MiMapCopyCoords_Click(object sender, EventArgs e)
        {
            cmMap.Hide();

            Clipboard.SetText(((ToolStripMenuItem)sender).Text);
        }

        private void MiMainCheckUpdates_Click(object sender, EventArgs e)
        {
            UpdateApp.Default.Update();
        }

        private async void BackupSave()
        {
            if (ProgramStatus.Contains(Status.BackupSave))
            {
                Msg.Info(Resources.BackupInfoInProgress);

                return;
            }

            if (!FrmBackup.ShowDlg(this))
            {
                return;
            }

            var status = ProgramStatus.Start(Status.BackupSave);

            try
            {
                var backup = new Utils.Backup
                {
                    Settings = AppSettings.Default.BackupSettings
                };

                await backup.SaveAsync();

                Msg.Info(Resources.BackupSaveOk, backup.Settings.Directory);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(Resources.BackupSaveFail, e.Message);
            }
            finally
            {
                ProgramStatus.Stop(status);
            }
        }

        private void MiMainDataBackupSave_Click(object sender, EventArgs e)
        {
            BackupSave();
        }
    }
}