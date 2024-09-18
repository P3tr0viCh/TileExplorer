//#define CHECK_TILES
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
                this.InvokeIfNeeded(async () => await UpdateDataAsync(load))
            );
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

        private bool markerMoving;

        private IMapItem selected = null;

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

                        Utils.GetFrmList(ChildFormType.MarkerList)?.SetSelected(selected.Model);

                        break;
                    case MapItemType.Track:
                        SelectedTrackTiles = selected.Model as Track;

                        Utils.GetFrmList(ChildFormType.TrackList)?.SetSelected(selected.Model);

                        break;
                }
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

        private void GMapControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (markerMoving)
            {
                markerMoving = false;

                if (SelectedMarker != null)
                {
                    if (Database.Actions.MarkerSaveAsync(SelectedMarker.Model).Result)
                    {
                        MarkerChanged(SelectedMarker.Model);
                    }
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
                    MarkerAdd(new Marker(gMapControl.FromLocalToLatLng(e.X, e.Y)));
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

        private PointLatLng MenuPopupPointLatLng;

        private void CmMap_Opening(object sender, CancelEventArgs e)
        {
            var menuPopupPoint = gMapControl.PointToClient(MousePosition);

            MenuPopupPointLatLng = gMapControl.FromLocalToLatLng(menuPopupPoint.X, menuPopupPoint.Y);

            UpdateCopyCoords();
        }

        private void MiMapMarkerAdd_Click(object sender, EventArgs e)
        {
            MarkerAdd(new Marker(MenuPopupPointLatLng));
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

            markerMoving = true;

            MarkerMovingPrevPosition = SelectedMarker.Position;

            var point = gMapControl.FromLatLngToLocal(SelectedMarker.Position);

            Cursor.Position = gMapControl.PointToScreen(new Point((int)point.X, (int)point.Y));
        }

        private void GMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            var position = gMapControl.FromLocalToLatLng(e.X, e.Y);

            statusStripPresenter.TileId = position;
            statusStripPresenter.MousePosition = position;

            if (!markerMoving) return;

            if (SelectedMarker == null) return;

            SelectedMarker.Model.Lat = position.Lat;
            SelectedMarker.Model.Lng = position.Lng;
            SelectedMarker.NotifyModelChanged();
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (markerMoving)
                {
                    if (SelectedMarker != null)
                    {
                        SelectedMarker.Model.Lat = MarkerMovingPrevPosition.Lat;
                        SelectedMarker.Model.Lng = MarkerMovingPrevPosition.Lng;
                        SelectedMarker.NotifyModelChanged();
                    }

                    markerMoving = false;
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
                       DataLoad.TracksTree;
            }

            DebugWrite.Line($"Loading data {load}");

            if (load.HasFlag(DataLoad.Tiles))
            {
                if (miMainShowTiles.Checked)
                {
                    await LoadTilesAsync();
                }
            }

            if (load.HasFlag(DataLoad.Tracks))
            {
                await LoadTracksInfoAsync();

                if (miMainShowTracks.Checked)
                {
                    await LoadTracksAsync();
                }
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

            foreach (var frm in Utils.GetChildForms<IUpdateDataForm>(null))
            {
                switch (frm.FormType)
                {
                    case ChildFormType.TileInfo:
                    case ChildFormType.ResultYears:
                        updateData = load.HasFlag(DataLoad.Tracks);

                        break;
                    case ChildFormType.TrackList:
                    case ChildFormType.ResultEquipments:
                        updateData = load.HasFlag(DataLoad.Tracks);

                        break;
                    case ChildFormType.MarkerList:
                        updateData = load.HasFlag(DataLoad.Markers);

                        break;
                    case ChildFormType.EquipmentList:
                        updateData = load.HasFlag(DataLoad.Tracks);

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
                    frm.UpdateData();
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

#if DEBUG
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
#endif

        private void MiMapTileAdd_Click(object sender, EventArgs e)
        {
#if DEBUG
            AddTileAsync();
#endif
        }

#if DEBUG
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
#endif

        private void MiMapTileDelete_Click(object sender, EventArgs e)
        {
#if DEBUG
            DeleteTileAsync();
#endif
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
                foreach (var frm in Utils.GetChildForms<Form>(childFormType))
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

        public void ListItemAdd(object sender, BaseId value)
        {
            if (value is Marker marker)
            {
                marker.Lat = gMapControl.Position.Lat;
                marker.Lng = gMapControl.Position.Lng;

                MarkerAdd(marker);

                return;
            }

            if (value is Track)
            {
                OpenTracks();
                return;
            }

            if (value is Equipment equipment)
            {
                EquipmentAdd(equipment);
                return;
            }
        }

        public void ListItemChange(object sender, BaseId value)
        {
            if (value is Marker marker)
            {
                MarkerChange(marker);
                return;
            }

            if (value is Track track)
            {
                TrackChange(track);
                return;
            }

            if (value is Equipment equipment)
            {
                EquipmentChange(equipment);
                return;
            }
        }

        public void ListItemDelete(object sender, BaseId value)
        {
            if (value is Marker marker)
            {
                MarkerDelete(marker);
                return;
            }

            if (value is Track track)
            {
                TrackDelete(track);
                return;
            }

            if (value is Equipment equipment)
            {
                EquipmentDelete(equipment);
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

            ((SolidBrush)GMapToolTip.DefaultFill).Color = Color.FromArgb(
                   AppSettings.Roaming.Default.ColorMarkerTextFillAlpha,
                   AppSettings.Roaming.Default.ColorMarkerTextFill);

            ((SolidBrush)GMapToolTip.DefaultForeground).Color = Color.FromArgb(
                AppSettings.Roaming.Default.ColorMarkerTextAlpha,
                AppSettings.Roaming.Default.ColorMarkerText);

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
            SetChildFormMenuItemState(((IChildForm)sender).FormType, true);
        }

        public void ChildFormClosed(object sender)
        {
            Selected = null;

            SetChildFormMenuItemState(((IChildForm)sender).FormType, false);
        }

        private void MiMainOsmOpen_Click(object sender, EventArgs e)
        {
            Utils.Osm.StartUrlOpen((int)gMapControl.Zoom, gMapControl.Position);
        }

        private void MiMainOsmEdit_Click(object sender, EventArgs e)
        {
            Utils.Osm.StartUrlEdit((int)gMapControl.Zoom, gMapControl.Position);
        }

        private void MiMapOsmOpen_Click(object sender, EventArgs e)
        {
            Utils.Osm.StartUrlOpen((int)gMapControl.Zoom, MenuPopupPointLatLng);
        }

        private void MiMapOsmEdit_Click(object sender, EventArgs e)
        {
            Utils.Osm.StartUrlEdit((int)gMapControl.Zoom, MenuPopupPointLatLng);
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

            foreach (var form in Utils.GetChildForms<FrmList>(ChildFormType.TileInfo))
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
            Task.Run(() =>
                this.InvokeIfNeeded(async () => await ShowTileInfoAsync(MenuPopupPointLatLng))
            );
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

        private void CheckUpdate()
        {
            Task.Run(() =>
                this.InvokeIfNeeded(async () => await CheckUpdateAsync())
            );
        }

        private void MiMainCheckUpdates_Click(object sender, EventArgs e)
        {
            CheckUpdate();
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

        private void BackupSave()
        {
            Task.Run(() =>
                this.InvokeIfNeeded(async () => await BackupSaveAsync())
            );
        }

        private void MiMainDataBackupSave_Click(object sender, EventArgs e)
        {
            BackupSave();
        }

        private void GMapControl_Paint(object sender, PaintEventArgs e)
        {
            mapZoomRuler.Paint(e.Graphics);
        }

        private void OpenChartTrackEle(Track value)
        {
            foreach (var frm in Utils.GetChildForms<FrmChartTrackEle>(ChildFormType.ChartTrackEle))
            {
                if (frm.Track.Id == value.Id)
                {
                    frm.BringToFront();
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