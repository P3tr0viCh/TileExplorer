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
        private readonly GMapOverlay overlayGrid = new GMapOverlay("grid");
        private readonly GMapOverlay overlayTiles = new GMapOverlay("tiles");
        private readonly GMapOverlay overlayTracks = new GMapOverlay("tracks");
        private readonly GMapOverlay overlayMarkers = new GMapOverlay("markers");
        private readonly GMapOverlay overlayPosition = new GMapOverlay("position");

        private readonly MapItemMarkerCircle markerPosition = new MapItemMarkerCircle(default);

        private readonly MapZoomRuler mapZoomRuler;

        private readonly PresenterStatusStrip statusStripPresenter;

        private readonly ProgramStatus status = new ProgramStatus();
        public ProgramStatus ProgramStatus => status;

        public Main()
        {
            InitializeComponent();

            statusStripPresenter = new PresenterStatusStrip(this);

            mapZoomRuler = new MapZoomRuler(gMapControl);

#if DEBUG
            AppSettings.Local.Directory = Files.ExecutableDirectory();
            AppSettings.Local.FileName = Path.Combine(AppSettings.Local.Directory,
                 Path.ChangeExtension(Files.ExecutableName(), ".Local.config"));

            AppSettings.Roaming.Directory = Files.ExecutableDirectory();
            AppSettings.Roaming.FileName = Path.Combine(AppSettings.Roaming.Directory,
                Path.ChangeExtension(Files.ExecutableName(), ".Roaming.config"));
#else
            AppSettings.Local.Directory = Files.AppDataLocalDirectory();
            AppSettings.Roaming.Directory = Files.AppDataRoamingDirectory();
#endif

            DebugWrite.Line("Settings Local: " + AppSettings.Local.FilePath, "Main");
            DebugWrite.Line("Settings Roaming: " + AppSettings.Roaming.FilePath, "Main");
        }

        private void Filter_OnChanged()
        {
            var load = DataLoad.Tiles | DataLoad.Tracks;

            UpdateData(load);
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
            AppSettings.Load();

            GMapLoad();

            if (!SetDatabaseFileName())
            {
                WindowState = FormWindowState.Minimized;
                AbnormalExit = true;
                Application.Exit();
                return;
            }

            Database.Filter.Default.Day = AppSettings.Local.Default.Filter.Day;
            Database.Filter.Default.DateFrom = AppSettings.Local.Default.Filter.DateFrom;
            Database.Filter.Default.DateTo = AppSettings.Local.Default.Filter.DateTo;
            Database.Filter.Default.Years = AppSettings.Local.Default.Filter.Years;

            Database.Filter.Default.OnChanged += Filter_OnChanged;

            ProgramStatus.StatusChanged += ProgramStatus_StatusChanged;

            UpdateApp.Default.StatusChanged += UpdateApp_StatusChanged;

            AppSettings.LoadFormState(this, AppSettings.Local.Default.FormStateMain);

#if DEBUG && DUMMY_TILES
            DummyTiles();
#endif

#if !DEBUG
            miMapTileAdd.Visible = false;
            miMapTileDelete.Visible = false;
#endif
            miMainShowGrid.Checked = AppSettings.Local.Default.VisibleGrid;
            miMainShowTiles.Checked = AppSettings.Local.Default.VisibleTiles;
            miMainShowTracks.Checked = AppSettings.Local.Default.VisibleTracks;
            miMainShowMarkers.Checked = AppSettings.Local.Default.VisibleMarkers;

            miMainGrayScale.Checked = AppSettings.Local.Default.MapGrayScale;
            gMapControl.GrayScaleMode = miMainGrayScale.Checked;

            overlayGrid.IsVisibile = miMainShowGrid.Checked;
            overlayTracks.IsVisibile = miMainShowTracks.Checked;
            overlayMarkers.IsVisibile = miMainShowMarkers.Checked;

            miMainLeftPanel.Checked = AppSettings.Local.Default.VisibleLeftPanel;
            toolStripContainer.LeftToolStripPanelVisible = miMainLeftPanel.Checked;

            UpdateSettings();

            StartUpdateGrid();

            var starting = ProgramStatus.Start(Status.Starting);

            if (AppSettings.Local.Default.VisibleResultYears)
            {
                ShowChildForm(ChildFormType.ResultYears, true);
            }
            if (AppSettings.Local.Default.VisibleResultEquipments)
            {
                ShowChildForm(ChildFormType.ResultEquipments, true);
            }
            if (AppSettings.Local.Default.VisibleTrackList)
            {
                ShowChildForm(ChildFormType.TrackList, true);
            }
            if (AppSettings.Local.Default.VisibleMarkerList)
            {
                ShowChildForm(ChildFormType.MarkerList, true);
            }
            if (AppSettings.Local.Default.VisibleEquipmentList)
            {
                ShowChildForm(ChildFormType.EquipmentList, true);
            }
            if (AppSettings.Local.Default.VisibleFilter)
            {
                ShowChildForm(ChildFormType.Filter, true);
            }
            if (AppSettings.Local.Default.VisibleTracksTree)
            {
                ShowChildForm(ChildFormType.TracksTree, true);
            }

            ProgramStatus.Stop(starting);

            UpdateData();
        }

        private void UpdateData(DataLoad load = default)
        {
            Task.Run(() =>
            {
                this.InvokeIfNeeded(async () => await UpdateDataAsync(load));
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

            AppSettings.Local.Default.FormStateMain = AppSettings.SaveFormState(this);

            AppSettings.Local.Default.MapGrayScale = miMainGrayScale.Checked;

            AppSettings.Local.Default.VisibleGrid = miMainShowGrid.Checked;
            AppSettings.Local.Default.VisibleTiles = miMainShowTiles.Checked;
            AppSettings.Local.Default.VisibleTracks = miMainShowTracks.Checked;
            AppSettings.Local.Default.VisibleMarkers = miMainShowMarkers.Checked;

            AppSettings.Local.Default.VisibleFilter = GetChildFormMenuItemState(ChildFormType.Filter);

            AppSettings.Local.Default.VisibleResultYears = GetChildFormMenuItemState(ChildFormType.ResultYears);
            AppSettings.Local.Default.VisibleResultEquipments = GetChildFormMenuItemState(ChildFormType.ResultEquipments);

            AppSettings.Local.Default.VisibleTrackList = GetChildFormMenuItemState(ChildFormType.TrackList);
            AppSettings.Local.Default.VisibleMarkerList = GetChildFormMenuItemState(ChildFormType.MarkerList);
            AppSettings.Local.Default.VisibleEquipmentList = GetChildFormMenuItemState(ChildFormType.EquipmentList);

            AppSettings.Local.Default.VisibleTracksTree = GetChildFormMenuItemState(ChildFormType.TracksTree);

            AppSettings.Local.Default.VisibleLeftPanel = miMainLeftPanel.Checked;

            AppSettings.Local.Default.Filter.Day = Database.Filter.Default.Day;
            AppSettings.Local.Default.Filter.DateFrom = Database.Filter.Default.DateFrom;
            AppSettings.Local.Default.Filter.DateTo = Database.Filter.Default.DateTo;
            AppSettings.Local.Default.Filter.Years = Database.Filter.Default.Years;

            AppSettings.LocalSave();
        }

        private void GMapLoad()
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            GMap.NET.MapProviders.GMapProvider.UserAgent = Utils.AssemblyNameAndVersion();

            DebugWrite.Line("useragent: " + GMap.NET.MapProviders.GMapProvider.UserAgent);

            gMapControl.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            gMapControl.MapProvider.RefererUrl = Const.MapRefererUrl;

            gMapControl.ShowCenter = false;

            gMapControl.MinZoom = 2;
            gMapControl.MaxZoom = 16;

            HomeGoto();

            gMapControl.MouseWheelZoomType = AppSettings.Roaming.Default.MouseWheelZoomType;

            gMapControl.CanDragMap = true;
            gMapControl.DragButton = MouseButtons.Left;

            gMapControl.Overlays.Add(overlayGrid);

            gMapControl.Overlays.Add(overlayTiles);
            gMapControl.Overlays.Add(overlayTracks);
            gMapControl.Overlays.Add(overlayMarkers);

            gMapControl.Overlays.Add(overlayPosition);

            overlayPosition.Markers.Add(markerPosition);

            statusStripPresenter.Zoom = gMapControl.Zoom;
            statusStripPresenter.Position = gMapControl.Position;
            statusStripPresenter.TileId = gMapControl.Position;
            statusStripPresenter.MousePosition = gMapControl.Position;
        }

        private void StartUpdateGrid()
        {
            timerMapChange.Stop();
            timerMapChange.Start();
        }

        private void TimerMapChange_Tick(object sender, EventArgs e)
        {
            timerMapChange.Stop();

            if (miMainShowGrid.Checked)
            {
                UpdateGrid();
            }
        }

        private void GMapControl_OnMapZoomChanged()
        {
            statusStripPresenter.Zoom = gMapControl.Zoom;

            miMainSaveTileBoundaryToFile.Enabled = gMapControl.Zoom >= AppSettings.Roaming.Default.SaveOsmTileMinZoom;

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
            this.InvokeIfNeeded(() => statusStripPresenter.Status = status.Description());
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

            this.InvokeIfNeeded(() =>
            {
                statusStripPresenter.TilesVisited = calcResult.Visited;
                statusStripPresenter.TilesMaxCluster = calcResult.MaxCluster;
                statusStripPresenter.TilesMaxSquare = calcResult.MaxSquare;
            });
        }

        private async Task LoadTilesAsync()
        {
            DebugWrite.Line("start");

            overlayTiles.Clear();

            var tiles = await Database.Default.ListLoadAsync<Tile>();

            await CalcTilesAsync(tiles);

            foreach (var tile in tiles)
            {
                overlayTiles.Polygons.Add(new MapItemTile(tile));

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

            overlayTracks.Clear();

            var tracks = await Database.Default.ListLoadAsync<Track>();

            foreach (var track in tracks)
            {
                track.TrackPoints = await Database.Default.ListLoadAsync<TrackPoint>(track);

                overlayTracks.Routes.Add(new MapItemTrack(track));

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

        private MapItemMarker NewMapItemMarker(Marker marker)
        {
            var item = new MapItemMarker(marker);

            ((SolidBrush)item.ToolTip.Fill).Color = Color.FromArgb(
                   AppSettings.Roaming.Default.ColorMarkerTextFillAlpha,
                   AppSettings.Roaming.Default.ColorMarkerTextFill);

            ((SolidBrush)item.ToolTip.Foreground).Color = Color.FromArgb(
                AppSettings.Roaming.Default.ColorMarkerTextAlpha,
                AppSettings.Roaming.Default.ColorMarkerText);

            item.ToolTip.Font = AppSettings.Roaming.Default.FontMarker;

            return item;
        }

        private async Task LoadMarkersAsync()
        {
            DebugWrite.Line("start");

            overlayMarkers.Clear();

            var markers = await Database.Default.ListLoadAsync<Marker>();

            foreach (var marker in markers)
            {
                overlayMarkers.Markers.Add(NewMapItemMarker(marker));
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
                foreach (var tile in overlayTiles.Polygons.Cast<MapItemTile>())
                {
                    tile.Selected = false;
                }

                gMapControl.Invalidate();

                if (track == null) return;

                var tiles = Task.Run(() =>
                {
                    return Database.Default.ListLoadAsync<Tile>(track);
                }).Result;

                foreach (var tile in from item in overlayTiles.Polygons.Cast<MapItemTile>()
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
            GetChildForms<FrmList>(childFormType).FirstOrDefault()?.SetSelected(baseId);
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

        private IEnumerable<IMapItem> MouseOverMapItems()
        {
            if (gMapControl.IsMouseOverMarker)
            {
                return overlayMarkers.Markers.Cast<MapItemMarker>().Where(m => m.IsMouseOver);
            }

            if (gMapControl.IsMouseOverRoute)
            {
                return overlayTracks.Routes.Cast<MapItemTrack>().Where(m => m.IsMouseOver);
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

                    await UpdateDataAsync(DataLoad.Markers);
                }

                return;
            }

            var items = MouseOverMapItems();

            var item = items?.FirstOrDefault();

            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (item != null)
                    {
                        Selected = item.Selected ? null : item;
                    }

                    break;
                case MouseButtons.Right:
                    Selected = item ?? null;

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
            var items = MouseOverMapItems();

            var item = items?.FirstOrDefault();

            Selected = item ?? null;

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
                            OpenChartTrackEle(SelectedTrack.Model);
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
                    if (overlayTracks.Routes.Count == 0)
                    {
                        UpdateData(DataLoad.Tracks);
                    }
                    break;
                case DataLoad.Markers:
                    if (overlayMarkers.Markers.Count == 0)
                    {
                        UpdateData(DataLoad.Markers);
                    }
                    break;
            }
        }

        private void MiMainShowMarkers_Click(object sender, EventArgs e)
        {
            overlayMarkers.IsVisibile = miMainShowMarkers.Checked;

            CheckAndLoadData(DataLoad.Markers);
        }

        private void MiMainShowTracks_Click(object sender, EventArgs e)
        {
            overlayTracks.IsVisibile = miMainShowTracks.Checked;

            CheckAndLoadData(DataLoad.Tracks);
        }

        private void MiMainShowTiles_Click(object sender, EventArgs e)
        {
            overlayTiles.IsVisibile = miMainShowTiles.Checked;
        }

        private void HomeGoto()
        {
            gMapControl.Zoom = AppSettings.Local.Default.HomeZoom;
            gMapControl.Position = new PointLatLng(AppSettings.Local.Default.HomeLat, AppSettings.Local.Default.HomeLng);
        }

        private void HomeSave()
        {
            AppSettings.Local.Default.HomeZoom = (int)gMapControl.Zoom;
            AppSettings.Local.Default.HomeLat = gMapControl.Position.Lat;
            AppSettings.Local.Default.HomeLng = gMapControl.Position.Lng;
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
            bool prevMarkersVisible = overlayMarkers.IsVisibile;

            overlayMarkers.IsVisibile = true;

            var marker = new Marker()
            {
                Lat = point.Lat,
                Lng = point.Lng,

#if DEBUG
                Text = DateTime.Now.ToString(),
#endif
            };

            markerTemp = new MapItemMarkerCross(point);

            overlayMarkers.Markers.Add(markerTemp);

            if (FrmMarker.ShowDlg(this, marker))
            {
                UpdateData(DataLoad.Markers);
            }

            overlayMarkers.Markers.Remove(markerTemp);

            overlayMarkers.IsVisibile = prevMarkersVisible;
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
                overlayMarkers.Markers.Remove(markerTemp);

                overlayMarkers.Markers.Add(NewMapItemMarker(marker));
            }
            else
            {
                mapItem.Model = marker;

                mapItem.NotifyModelChanged();
            }

            GetChildForms<FrmList>(ChildFormType.MarkerList).FirstOrDefault()?.UpdateData();

            SelectMapItem(this, marker);
        }

        private void MarkerDelete(Marker marker)
        {
            if (marker == null) return;

            var name = marker.Text;

            if (name.IsEmpty())
            {
                name = marker.Lat.ToString() + ":" + marker.Lng.ToString();
            }

            if (!Msg.Question(string.Format(Resources.QuestionMarkerDelete, name))) return;

            Task.Run(async () =>
            {
                await Database.Default.MarkerDeleteAsync(marker);

                var mapItem = FindMapItem(marker);

                if (mapItem != null)
                {
                    overlayMarkers.Markers.Remove(mapItem as MapItemMarker);
                }

                Selected = null;

                ChildFormsUpdateData(DataLoad.Markers);
            });
        }

        private void TrackChange(Track track)
        {
            if (track == null) return;

            FrmTrack.ShowDlg(this, track);

            SelectMapItem(this, track);
        }

        public void TrackChanged(Track track)
        {
            UpdateData(DataLoad.Tracks);
        }

        private void TrackDelete(Track track)
        {
            if (track == null) return;

            var name = track.Text;

            if (name.IsEmpty())
            {
                name = track.DateTimeStart.ToString();
            }

            if (!Msg.Question(string.Format(Resources.QuestionTrackDelete, name))) return;

            Task.Run(async () =>
            {
                await Database.Default.TrackDeleteAsync(track);

                await UpdateDataAsync(DataLoad.Tiles);

                var mapItem = FindMapItem(track);

                if (mapItem != null)
                {
                    overlayTracks.Routes.Remove(mapItem as MapItemTrack);
                }

                Selected = null;

                ChildFormsUpdateData(DataLoad.Tracks | DataLoad.TracksTree);
            });
        }

        private PointLatLng MenuPopupPointLatLng;

        private void CmMap_Opening(object sender, CancelEventArgs e)
        {
            var menuPopupPoint = gMapControl.PointToClient(MousePosition);

            MenuPopupPointLatLng = gMapControl.FromLocalToLatLng(menuPopupPoint.X, menuPopupPoint.Y);

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
            MarkerDelete(SelectedMarker.Model);
        }

        private void MiTrackChange_Click(object sender, EventArgs e)
        {
            TrackChange(SelectedTrack.Model);
        }

        private void MiTrackDelete_Click(object sender, EventArgs e)
        {
            TrackDelete(SelectedTrack.Model);
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
                       DataLoad.Tracks |
                       DataLoad.Markers |
                       DataLoad.Equipments |
                       DataLoad.TracksTree;
            }

            DebugWrite.Line($"Loading data {load}");

            if (load.HasFlag(DataLoad.Tiles))
            {
                if (miMainShowTiles.Checked) await LoadTilesAsync();
            }

            if (load.HasFlag(DataLoad.Tracks))
            {
                await LoadTracksInfoAsync();

                if (miMainShowTracks.Checked) await LoadTracksAsync();
            }

            if (load.HasFlag(DataLoad.Markers))
            {
                if (miMainShowMarkers.Checked)
                {
                    await LoadMarkersAsync();
                }
            }

            ProgramStatus.Stop(status);

            ChildFormsUpdateData(load);
        }

        private void ChildFormsUpdateData(DataLoad load)
        {
            bool updateData;

            foreach (var frm in Application.OpenForms)
            {
                if (frm is IChildForm frmChild && frm is IUpdateDataForm frmUpdateData)
                {
                    switch (frmChild.ChildFormType)
                    {
                        case ChildFormType.TileInfo:
                        case ChildFormType.ResultYears:
                            updateData = load.HasFlag(DataLoad.Tracks);

                            break;
                        case ChildFormType.TrackList:
                        case ChildFormType.ResultEquipments:
                            updateData = load.HasFlag(DataLoad.Tracks) ||
                                         load.HasFlag(DataLoad.Equipments);

                            break;
                        case ChildFormType.MarkerList:
                            updateData = load.HasFlag(DataLoad.Markers);

                            break;
                        case ChildFormType.EquipmentList:
                            updateData = load.HasFlag(DataLoad.Equipments);

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

        private async Task<bool> OpenTracksAsync(string[] files)
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
                    track = Utils.Tracks.OpenTrackFromFile(file);

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

                        await Database.Default.TrackSaveAsync(track);
                    }

                    if (exitForEach)
                    {
                        break;
                    }

                    var mapTrack = new MapItemTrack(track);

                    overlayTracks.Routes.Add(mapTrack);

                    Selected = mapTrack;

                    trackTiles = Utils.Tiles.GetTilesFromTrack(track);

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

                    await Database.Default.TilesSaveAsync(saveTiles);

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

                    await Database.Default.TracksTilesSaveAsync(tracksTiles);

                    DebugWrite.Line("SaveTracksTilesAsync done");
                }

                return true;
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                var msg = e.InnerException != null ? e.InnerException.Message : e.Message;

                this.InvokeIfNeeded(() =>
                {
                    Msg.Error(msg);
                });

                return false;
            }
            finally
            {
                ProgramStatus.Stop(status);
            }
        }

        private void OpenTracks()
        {
            openFileDialog.FileName = string.Empty;

            openFileDialog.InitialDirectory = AppSettings.Local.Default.DirectoryTracks;

            if (openFileDialog.ShowDialog(this) != DialogResult.OK) return;

            AppSettings.Local.Default.DirectoryTracks = Directory.GetParent(openFileDialog.FileName).FullName;

            Task.Run(() =>
            {
                this.InvokeIfNeeded(async () =>
                {
                    if (await OpenTracksAsync(openFileDialog.FileNames))
                    {
                        await UpdateDataAsync(DataLoad.Tiles);

                        ChildFormsUpdateData(DataLoad.Tracks | DataLoad.TracksTree);
                    }
                });
            });
        }

        private void MiMainDataOpenTrack_Click(object sender, EventArgs e)
        {
            OpenTracks();
        }

        private void ShowChildForm(ChildFormType childFormType, bool show)
        {
            if (show)
            {
                if (!GetChildFormMenuItemState(childFormType))
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
                foreach (var frm in GetChildForms<Form>(childFormType))
                {
                    frm.Close();
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
                items = overlayMarkers.Markers;
            }
            else
            {
                if (value is Track)
                {
                    items = overlayTracks.Routes;
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

        private void EquipmentChange(Equipment equipment)
        {
            if (FrmEquipment.ShowDlg(this, equipment))
            {
                UpdateData(DataLoad.Equipments);
            }
        }

        private void EquipmentDelete(Equipment equipment)
        {
            if (equipment == null) return;

            var name = equipment.Name;

            if (Msg.Question(string.Format(Resources.QuestionEquipmentDelete, name)))
            {
                Task.Run(async () =>
                {
                    await Database.Default.EquipmentDeleteAsync(equipment);

                    await UpdateDataAsync(DataLoad.Equipments);
                });
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
                OpenTracks();
                return;
            }

            if (value is Equipment)
            {
                EquipmentChange(value as Equipment);
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
                EquipmentChange(value as Equipment);
                return;
            }
        }

        public void ListItemDelete(object sender, BaseId value)
        {
            if (value is Track)
            {
                TrackDelete(value as Track);
                return;
            }

            if (value is Marker)
            {
                MarkerDelete(value as Marker);
                return;
            }

            if (value is Equipment)
            {
                EquipmentDelete(value as Equipment);
                return;
            }
        }

        public void UpdateGrid()
        {
            if (gMapControl.Zoom < AppSettings.Roaming.Default.SaveOsmTileMinZoom || !miMainShowGrid.Checked)
            {
                overlayGrid.IsVisibile = false;

                return;
            }
            else
            {
                overlayGrid.IsVisibile = true;
            }

            var pointLeftTop = gMapControl.FromLocalToLatLng(0, 0);
            var pointRightBottom = gMapControl.FromLocalToLatLng(gMapControl.Width, gMapControl.Height);

            var leftTopX = Utils.Osm.LngToTileX(pointLeftTop);
            var leftTopY = Utils.Osm.LatToTileY(pointLeftTop);

            var rightBottomX = Utils.Osm.LngToTileX(pointRightBottom);
            var rightBottomY = Utils.Osm.LatToTileY(pointRightBottom);

            overlayGrid.Polygons.Clear();

            for (var x = leftTopX; x <= rightBottomX; x++)
                for (var y = leftTopY; y <= rightBottomY; y++)
                {
                    overlayGrid.Polygons.Add(new MapItemTile(new Tile(x, y)));
                }
        }

        private void MiMainShowGrid_Click(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        private bool SetDatabaseFileName()
        {
            var databaseHome = AppSettings.Local.Default.DatabaseHome;

            var defaultDatabaseHome =
#if DEBUG
                Files.ExecutableDirectory();
#else
                Files.AppDataRoamingDirectory();
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
                    AppSettings.Local.Default.DatabaseHome = string.Empty;

                    return SetDatabaseFileName();
                }

                return false;
            }

            var databaseFileName = Path.Combine(databaseHome, Files.DatabaseFileName());

            DebugWrite.Line("database: " + databaseFileName);

            Database.Default.FileName = databaseFileName;

            return true;
        }

        private void UpdateSettings()
        {
            MapItemMarkerCircle.DefaultFill.Color = AppSettings.Roaming.Default.ColorMarkerPosition;

            MapItemMarker.DefaultFill.Color = Color.FromArgb(
                AppSettings.Roaming.Default.ColorMarkerFillAlpha,
                AppSettings.Roaming.Default.ColorMarkerFill);
            MapItemMarker.DefaultSelectedFill.Color = Color.FromArgb(
                AppSettings.Roaming.Default.ColorMarkerSelectedFillAlpha,
                AppSettings.Roaming.Default.ColorMarkerSelectedFill);

            MapItemMarker.DefaultStroke.Color = Color.FromArgb(
                AppSettings.Roaming.Default.ColorMarkerLineAlpha,
                AppSettings.Roaming.Default.ColorMarkerLine);
            MapItemMarker.DefaultStroke.Width = AppSettings.Roaming.Default.WidthMarkerLine;

            MapItemMarker.DefaultSelectedStroke.Color = Color.FromArgb(
                AppSettings.Roaming.Default.ColorMarkerSelectedLineAlpha,
                AppSettings.Roaming.Default.ColorMarkerSelectedLine);
            MapItemMarker.DefaultSelectedStroke.Width = AppSettings.Roaming.Default.WidthMarkerLineSelected;

            GMapRoute.DefaultStroke.Color = Color.FromArgb(
                AppSettings.Roaming.Default.ColorTrackAlpha,
                AppSettings.Roaming.Default.ColorTrack);
            GMapRoute.DefaultStroke.Width = AppSettings.Roaming.Default.WidthTrack;

            MapItemTrack.DefaultSelectedStroke.Color = Color.FromArgb(
                AppSettings.Roaming.Default.ColorTrackSelectedAlpha,
                AppSettings.Roaming.Default.ColorTrackSelected);
            MapItemTrack.DefaultSelectedStroke.Width = AppSettings.Roaming.Default.WidthTrackSelected;

            DataGridViewCellStyles.UpdateSettings();
        }

        private async Task ShowSettingsAsync()
        {
            var TrackMinDistancePoint = AppSettings.Roaming.Default.TrackMinDistancePoint;

            if (FrmSettings.ShowDlg(this))
            {
                if (!SetDatabaseFileName())
                {
                    AbnormalExit = true;
                    Application.Exit();
                    return;
                }

                UpdateSettings();

                foreach (var frm in Application.OpenForms)
                {
                    if (frm is IChildForm form)
                    {
                        form.UpdateSettings();
                    }
                }

                if (TrackMinDistancePoint != AppSettings.Roaming.Default.TrackMinDistancePoint)
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
            _ = ShowSettingsAsync();
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

        private ToolStripMenuItem GetChildFormMenuItem(ChildFormType childFormType)
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
                    return null;
            }
        }

        private void SetChildFormMenuItemState(ChildFormType childFormType, bool state)
        {
            var item = GetChildFormMenuItem(childFormType);

            if (item != null)
            {
                item.Checked = state;
            }
        }

        private bool GetChildFormMenuItemState(ChildFormType childFormType)
        {
            var item = GetChildFormMenuItem(childFormType);

            if (item != null)
            {
                return item.Checked;
            }

            throw new NotImplementedException();
        }

        public void ChildFormOpened(object sender)
        {
            SetChildFormMenuItemState(((IChildForm)sender).ChildFormType, true);
        }

        public void ChildFormClosed(object sender)
        {
            Selected = null;

            SetChildFormMenuItemState(((IChildForm)sender).ChildFormType, false);
        }

        private void OsmAction(bool open)
        {
            var zoom = gMapControl.Zoom;

            if (!open && zoom < Const.OsmEditMinZoom)
            {
                if (Msg.Question(Resources.QuestionOsmSetEditZoom))
                {
                    zoom = Const.OsmEditMinZoom;
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

        private async Task ShowTileInfoAsync(PointLatLng position)
        {
            var tile = new Tile(position);

            tile.Id = await Database.Default.GetTileIdByXYAsync(tile);

            if (tile.Id <= Sql.NewId)
            {
                Msg.Info(Resources.MsgTileNotVisited);

                return;
            }

            foreach (var form in GetChildForms<FrmList>(ChildFormType.TileInfo))
            {
                if (((Tile)form.Data).Id == tile.Id)
                {
                    form.BringToFront();
                    return;
                }
            }

            FrmList.ShowFrm(this, ChildFormType.TileInfo, tile);
        }

        private void ShowTileInfo()
        {
            _ = ShowTileInfoAsync(MenuPopupPointLatLng);
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
            if (GetChildFormMenuItemState(ChildFormType.ResultYears) &&
                GetChildFormMenuItemState(ChildFormType.ResultEquipments))
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
                MenuPopupPointLatLng.Lat.ToString(AppSettings.Roaming.Default.FormatLatLng);
            miMapCopyCoordsFloatLng.Text =
                MenuPopupPointLatLng.Lng.ToString(AppSettings.Roaming.Default.FormatLatLng);
            miMapCopyCoordsFloat.Text = miMapCopyCoordsFloatLat.Text + ", " +
                miMapCopyCoordsFloatLng.Text;

            miMapCopyCoordsFloat2Lat.Text =
                MenuPopupPointLatLng.Lat.ToString(AppSettings.Roaming.Default.FormatLatLng, CultureInfo.InvariantCulture);
            miMapCopyCoordsFloat2Lng.Text =
                MenuPopupPointLatLng.Lng.ToString(AppSettings.Roaming.Default.FormatLatLng, CultureInfo.InvariantCulture);
            miMapCopyCoordsFloat2.Text = miMapCopyCoordsFloat2Lat.Text + " " +
                miMapCopyCoordsFloat2Lng.Text;
        }

        private void MiMapCopyCoords_Click(object sender, EventArgs e)
        {
            cmMap.Hide();

            Clipboard.SetText(((ToolStripMenuItem)sender).Text);
        }

        private async Task CheckUpdateAsync()
        {
            var result = await UpdateApp.Default.UpdateAsync();

            if (result.IsError)
            {
                Msg.Error(Resources.AppUpdateErrorInProgress, result.Message);
            }
            else
            {
                if (result.CanRestart)
                {
                    if (Msg.Question(result.Message))
                    {
                        Close();

                        UpdateApp.Default.AppRestart();
                    }
                }
                else
                {
                    Msg.Info(result.Message);
                }
            }
        }

        private async void MiMainCheckUpdates_ClickAsync(object sender, EventArgs e)
        {
            await CheckUpdateAsync();
        }

        private async Task BackupSaveAsync()
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
                var backup = new Backup();

                await backup.SaveAsync();

                Msg.Info(Resources.BackupSaveOk, AppSettings.Local.Default.BackupSettings.Directory);
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

        private async void MiMainDataBackupSave_ClickAsync(object sender, EventArgs e)
        {
            await BackupSaveAsync();
        }

        private void GMapControl_Paint(object sender, PaintEventArgs e)
        {
            mapZoomRuler.Paint(e.Graphics);
        }

        private ICollection<T> GetChildForms<T>(ChildFormType type) where T : Form
        {
            var forms = new List<T>();

            foreach (var frm in Application.OpenForms)
            {
                if (frm is IChildForm childFrm && frm is T childFrmT &&
                    childFrm.ChildFormType == type)
                {
                    forms.Add(childFrmT);
                }
            }

            return forms;
        }

        private void OpenChartTrackEle(Track value)
        {
            foreach (var form in GetChildForms<FrmChartTrackEle>(ChildFormType.ChartTrackEle))
            {
                if (form.Track.Id == value.Id)
                {
                    form.BringToFront();
                    return;
                }
            }

            FrmChartTrackEle.ShowFrm(this, value);
        }

        public void ShowChartTrackEle(object sender, Track value)
        {
            OpenChartTrackEle(value);
        }

        public void ShowMarkerPosition(object sender, PointLatLng value)
        {
            markerPosition.Position = value;

            overlayPosition.IsVisibile = value != default;
        }

        private void MiTrackShowChartTrackEle_Click(object sender, EventArgs e)
        {
            OpenChartTrackEle(SelectedTrack.Model);
        }
    }
}