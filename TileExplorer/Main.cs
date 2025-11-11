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
    public partial class Main : Form, PresenterStatusStripMain.IPresenterStatusStripMain, IMainForm
    {
        private readonly GMapOverlay overlayGrid = new GMapOverlay("grid");
        private readonly GMapOverlay overlayTiles = new GMapOverlay("tiles");
        private readonly GMapOverlay overlayTracks = new GMapOverlay("tracks");
        private readonly GMapOverlay overlayMarkers = new GMapOverlay("markers");
        private readonly GMapOverlay overlayPositions = new GMapOverlay("position");

        private readonly MapZoomRuler mapZoomRuler;

        private readonly PresenterStatusStripMain statusStripPresenter;

        public ProgramStatus ProgramStatus { get; } = new ProgramStatus();

        public Main()
        {
            InitializeComponent();

            statusStripPresenter = new PresenterStatusStripMain(this);

            mapZoomRuler = new MapZoomRuler(gMapControl);
        }

        private async void Filter_OnChangedAsync()
        {
            Selected = null;

            await UpdateDataAsync(DataLoad.Tiles | DataLoad.Tracks);
        }

        private bool AbnormalExit
        {
            get => Tag != null && (bool)Tag;
            set => Tag = value;
        }

        private async void Main_Load(object sender, EventArgs e)
        {
#if DEBUG
            AppSettings.Local.Directory = Path.Combine(Files.ExecutableDirectory(), "local");

            Utils.DirectoryCreate(AppSettings.Local.Directory);
#else
            AppSettings.Local.Directory = Files.AppDataLocalDirectory();
#endif

            AppSettings.Load();

            GMapLoad();

            if (!SetDatabaseFileName())
            {
                WindowState = FormWindowState.Minimized;
                AbnormalExit = true;
                Application.Exit();
                return;
            }

            ProgramStatus.StatusChanged += ProgramStatus_StatusChanged;

            Database.Filter.Default.Day = AppSettings.Local.Default.Filter.Day;
            Database.Filter.Default.DateFrom = AppSettings.Local.Default.Filter.DateFrom;
            Database.Filter.Default.DateTo = AppSettings.Local.Default.Filter.DateTo;
            Database.Filter.Default.Years = AppSettings.Local.Default.Filter.Years;

            Database.Filter.Default.OnChanged += Filter_OnChangedAsync;

            UpdateApp.Default.StatusChanged += UpdateApp_StatusChanged;

#if DEBUG 
            miMapTileAdd.Click += MiMapTileAdd_Click;
            miMapTileDelete.Click += MiMapTileDelete_Click;

            miMapTileAdd.Visible = true;
            miMapTileDelete.Visible = true;
#endif

            AppSettings.LoadFormState(this, AppSettings.Local.Default.FormStateMain);

#if DEBUG && DUMMY_TILES
            DummyTiles();
#endif

            miMainShowGrid.Checked = AppSettings.Local.Default.VisibleGrid;
            miMainTilesHeatmap.Checked = AppSettings.Local.Default.MapTilesHeatmap;
            miMainShowTiles.Checked = AppSettings.Local.Default.VisibleTiles;
            miMainTilesHeatmap.Enabled = miMainShowTiles.Checked;
            miMainShowTracks.Checked = AppSettings.Local.Default.VisibleTracks;
            miMainShowMarkers.Checked = AppSettings.Local.Default.VisibleMarkers;

            miMainGrayScale.Checked = AppSettings.Local.Default.MapGrayScale;
            gMapControl.GrayScaleMode = miMainGrayScale.Checked;

            overlayGrid.IsVisibile = miMainShowGrid.Checked;

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

            ProgramStatus.Stop(starting);

            await UpdateDataAsync();

            await CheckDirectoryTracksAsync(false);

            BackupLoad();
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
            AppSettings.Local.Default.MapTilesHeatmap = miMainTilesHeatmap.Checked;

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

            AppSettings.Local.Default.VisibleLeftPanel = miMainLeftPanel.Checked;

            AppSettings.Local.Default.Filter.Day = Database.Filter.Default.Day;
            AppSettings.Local.Default.Filter.DateFrom = Database.Filter.Default.DateFrom;
            AppSettings.Local.Default.Filter.DateTo = Database.Filter.Default.DateTo;
            AppSettings.Local.Default.Filter.Years = Database.Filter.Default.Years;

            AppSettings.LocalSave();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            ctsTiles.Cancel();
            ctsTracks.Cancel();
            ctsMarkers.Cancel();
            ctsTracksInfo.Cancel();
            ctsCheckDirectoryTracks.Cancel();
        }

        private void GMapLoad()
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            GMap.NET.MapProviders.GMapProvider.UserAgent = Utils.AssemblyNameAndVersion();

            DebugWrite.Line($"useragent: {GMap.NET.MapProviders.GMapProvider.UserAgent}");

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

            gMapControl.Overlays.Add(overlayPositions);

            overlayPositions.Markers.Add(markerPosition);
            overlayPositions.Markers.Add(markerNewPosition);

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
            statusStripPresenter.Status = status;
        }

        private void UpdateApp_StatusChanged(object sender, UpdateStatus status)
        {
            statusStripPresenter.UpdateStatus = status;
        }

        ToolStripStatusLabel PresenterStatusStripMain.IPresenterStatusStripMain.GetLabel(PresenterStatusStripMain.StatusLabel label)
        {
            switch (label)
            {
                case PresenterStatusStripMain.StatusLabel.Zoom: return slZoom;
                case PresenterStatusStripMain.StatusLabel.TileId: return slTileId;
                case PresenterStatusStripMain.StatusLabel.Position: return slPosition;
                case PresenterStatusStripMain.StatusLabel.MousePosition: return slMousePosition;
                case PresenterStatusStripMain.StatusLabel.Status: return slStatus;
                case PresenterStatusStripMain.StatusLabel.UpdateStatus: return slUpdateStatus;
                case PresenterStatusStripMain.StatusLabel.TracksCount: return slTracksCount;
                case PresenterStatusStripMain.StatusLabel.TracksDistance: return slTracksDistance;
                case PresenterStatusStripMain.StatusLabel.TilesVisited: return slTilesVisited;
                case PresenterStatusStripMain.StatusLabel.TilesMaxCluster: return slTilesMaxCluster;
                case PresenterStatusStripMain.StatusLabel.TilesMaxSquare: return slTilesMaxSquare;
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

        private FormWindowState savedWindowState;

        private bool fullScreen;
        private bool FullScreen
        {
            get => fullScreen;
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

        private bool markerMoving;

        private IEnumerable<IMapItem> MouseOverMapItems()
        {
            IEnumerable<IMapItem> list = overlayMarkers.Markers.Cast<MapItemMarker>().Where(m => m.IsMouseOver);

            if (list?.Count() == 0)
            {
                list = overlayTracks.Routes.Cast<MapItemTrack>().Where(m => m.IsMouseOver);

                if (list?.Count() == 0) return null;
            }

            return list.OrderBy(i => ((IModelText)i.Model).Text);
        }


        private void ContextMenuShow(ContextMenuStrip menu, Point pos)
        {
            menu.Show(gMapControl, pos);
        }

        private void CreateMenuSelectedItems(IEnumerable<IMapItem> mapItems)
        {
            cmSelectedItems.Items.Clear();

            foreach (var mapItem in mapItems)
            {
                var item = cmSelectedItems.Items.Add(((IModelText)mapItem.Model).Text);

                item.Tag = mapItem;

                item.Click += MenuSelectedItemsItem_Click;
            }
        }

        private void MenuSelectedItemsItem_Click(object sender, EventArgs e)
        {
            Selected = (IMapItem)((ToolStripMenuItem)sender).Tag;
        }

        private async void GMapControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (markerMoving)
            {
                markerMoving = false;

                if (SelectedMarker != null)
                {
                    if (await Database.Actions.MarkerSaveAsync(SelectedMarker.Model))
                    {
                        await MarkerChangedAsync(SelectedMarker.Model);
                    }
                }

                return;
            }

            var items = MouseOverMapItems();

            DebugWrite.Line($"items count = {items?.Count()}");

            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (items == null)
                    {
                        break;
                    }

                    var item = items?.FirstOrDefault();

                    if (Selected == null)
                    {
                        Selected = item;
                        break;
                    }

                    if (items.Count() == 1)
                    {
                        Selected = item.Selected ? null : item;

                        break;
                    }

                    var list = items.ToList();

                    var pos = list.FindIndex(i => i.Model == Selected.Model);

                    DebugWrite.Line($"pos = {pos}");

                    if (pos == -1)
                    {
                        Selected = item;

                        break;
                    }

                    if (pos == list.Count - 1)
                    {
                        pos = 0;
                    }
                    else
                    {
                        pos++;
                    }

                    DebugWrite.Line($"next = {pos}");

                    Selected = list[pos];

                    break;
                case MouseButtons.Right:
                    if (items == null)
                    {
                        ContextMenuShow(cmMap, e.Location);

                        break;
                    }

                    if (items.Count() == 1)
                    {
                        Selected = items?.FirstOrDefault() ?? null;

                        if (Selected == null)
                        {
                            break;
                        }

                        if (Selected is MapItemMarker)
                        {
                            ContextMenuShow(cmMarker, e.Location);
                        }
                        else
                        {
                            if (Selected is MapItemTrack)
                            {
                                ContextMenuShow(cmTrack, e.Location);
                            }
                        }

                        break;
                    }

                    CreateMenuSelectedItems(items);

                    ContextMenuShow(cmSelectedItems, e.Location);

                    break;
            }
        }

        private async void GMapControl_MouseDoubleClick(object sender, MouseEventArgs e)
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
                            await MarkerChangeAsync(SelectedMarker.Model);
                            break;
                        case MapItemType.Track:
                            OpenChartTrackEle(SelectedTrack.Model);
                            break;
                    }
                }
            }
        }

        private async Task CheckAndLoadDataAsync(DataLoad load, bool visible)
        {
            switch (load)
            {
                case DataLoad.Tiles:
                    foreach (var polygon in overlayTiles.Polygons)
                    {
                        polygon.IsVisible = visible;
                    }

                    if (!tilesLoaded)
                    {
                        await LoadTilesAsync();
                    }

                    break;
                case DataLoad.Tracks:
                    foreach (var track in overlayTracks.Routes)
                    {
                        track.IsVisible = visible;
                    }

                    if (SelectedTrack != null)
                    {
                        SelectedTrack.IsVisible = true;
                    }

                    if (!tracksLoaded)
                    {
                        await LoadTracksAsync();
                    }

                    break;
                case DataLoad.Markers:
                    foreach (var marker in overlayMarkers.Markers)
                    {
                        marker.IsVisible = visible;
                    }

                    if (!markersLoaded)
                    {
                        await LoadMarkersAsync();
                    }

                    break;
            }
        }

        private async void MiMainShowTiles_Click(object sender, EventArgs e)
        {
            miMainTilesHeatmap.Enabled = miMainShowTiles.Checked;
            await CheckAndLoadDataAsync(DataLoad.Tiles, miMainShowTiles.Checked);
        }

        private async void MiMainShowMarkers_Click(object sender, EventArgs e)
        {
            await CheckAndLoadDataAsync(DataLoad.Markers, miMainShowMarkers.Checked);
        }

        private async void MiMainShowTracks_Click(object sender, EventArgs e)
        {
            await CheckAndLoadDataAsync(DataLoad.Tracks, miMainShowTracks.Checked);
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

        private async void MiMarkerChange_Click(object sender, EventArgs e)
        {
            await MarkerChangeAsync(SelectedMarker.Model);
        }

        private async void MiMarkerDelete_Click(object sender, EventArgs e)
        {
            await MarkerDeleteAsync(SelectedMarker.Model);
        }

        private async void MiTrackChange_Click(object sender, EventArgs e)
        {
            await TrackChangeAsync(SelectedTrack.Model);
        }

        private async void MiTrackDelete_Click(object sender, EventArgs e)
        {
            await TrackDeleteAsync(SelectedTrack.Model);
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

        private async void MiMainDataUpdate_Click(object sender, EventArgs e)
        {
            await UpdateDataAsync();
        }

#if DEBUG
        private async Task AddTileAsync()
        {
            var status = ProgramStatus.Start(Status.SaveData);

            try
            {
                var tile = new Tile(MenuPopupPointLatLng);

                var tileExists = await Database.Default.GetTileIdByXYAsync(tile) != Sql.NewId;

                if (tileExists)
                {
                    Msg.Error("tile exists");
                }
                else
                {
                    await Database.Default.TileSaveAsync(tile);

                    await UpdateDataAsync(DataLoad.Tiles);
                }
            }
            finally
            {
                ProgramStatus.Stop(status);
            }
        }

        private async void MiMapTileAdd_Click(object sender, EventArgs e)
        {
            await AddTileAsync();
        }

        private async Task DeleteTileAsync()
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

        private async void MiMapTileDelete_Click(object sender, EventArgs e)
        {
            await DeleteTileAsync();
        }
#endif

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

        private async void MiMainDataOpenTrack_Click(object sender, EventArgs e)
        {
            await OpenTracksAsync(null);
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
                Utils.Forms.GetChildForms<Form>(childFormType).ForEach(frm => frm.Close());
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

            return items.Cast<IMapItem>().FirstOrDefault(i => i.Model.Id == value.Id);
        }

        public async void ListItemAdd(object sender, BaseId value)
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
                await OpenTracksAsync(null);

                return;
            }

            if (value is Equipment equipment)
            {
                EquipmentAdd(equipment);

                return;
            }
        }

        public async Task ListItemChangeAsync(object sender, BaseId value)
        {
            if (value is Marker marker)
            {
                await MarkerChangeAsync(marker);
                return;
            }

            if (value is Track track)
            {
                await TrackChangeAsync(track);
                return;
            }

            if (value is Equipment equipment)
            {
                await EquipmentChangeAsync(equipment);
                return;
            }
        }

        public async Task ListItemDeleteAsync(object sender, BaseId value)
        {
            if (value is Marker marker)
            {
                await MarkerDeleteAsync(marker);
                return;
            }

            if (value is Track track)
            {
                await TrackDeleteAsync(track);
                return;
            }

            if (value is Equipment equipment)
            {
                await EquipmentDeleteAsync(equipment);
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
            var directoryDatabase = AppSettings.Local.Default.DirectoryDatabase;

            var defaultDirectoryDatabase =
#if DEBUG
                Files.ExecutableDirectory();
#else
                Files.AppDataRoamingDirectory();
#endif
            if (directoryDatabase.IsEmpty())
            {
                directoryDatabase = defaultDirectoryDatabase;
            }

            if (!Directory.Exists(directoryDatabase))
            {
                DebugWrite.Error($"database directory not exists: {directoryDatabase}");

                if (Msg.Question(Resources.ErrorDatabaseDirectoryNotExists, directoryDatabase, defaultDirectoryDatabase))
                {
                    AppSettings.Local.Default.DirectoryDatabase = string.Empty;

                    return SetDatabaseFileName();
                }

                return false;
            }

            var databaseFileName = Path.Combine(directoryDatabase, Files.DatabaseFileName());

            DebugWrite.Line($"database: {databaseFileName}");

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

                await UpdateDataAsync();
            }
        }

        private async void MiMainSettings_Click(object sender, EventArgs e)
        {
            await ShowSettingsAsync();
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

        private async Task ShowTileInfoAsync()
        {
            var tile = new Tile(MenuPopupPointLatLng);

            try
            {
                tile.Id = await Database.Default.GetTileIdByXYAsync(tile);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(Resources.MsgDatabaseLoadListTileInfoFail, e.Message);

                return;
            }

            if (tile.Id <= Sql.NewId)
            {
                Msg.Info(Resources.MsgTileNotVisited);

                return;
            }

            foreach (var frm in Utils.Forms.GetChildForms<FrmList>(ChildFormType.TileInfo))
            {
                if (((Tile)frm.Value).Id == tile.Id)
                {
                    frm.BringToFront();
                    return;
                }
            }
            ;

            FrmList.ShowFrm(this, ChildFormType.TileInfo, tile);
        }

        private async void MiMapShowTileInfo_Click(object sender, EventArgs e)
        {
            await ShowTileInfoAsync();
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

            if (result.HasError)
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

        private async void MiMainCheckUpdates_Click(object sender, EventArgs e)
        {
            await CheckUpdateAsync();
        }

        private async Task BackupSaveAsync()
        {
            if (ProgramStatus.Contains(Status.BackupSave) || ProgramStatus.Contains(Status.BackupLoad))
            {
                Msg.Info(Resources.BackupInfoInProgress);

                return;
            }

            if (!FrmBackup.ShowDlg(this))
            {
                return;
            }

            var status = ProgramStatus.Start(Status.BackupSave);

            bool result;
            string resultMessage;

            try
            {
                var backup = new Backup
                {
                    Settings = AppSettings.Local.Default.BackupSettings
                };

                backup.Settings.Name = DateTime.Now.ToString("yyyy-MM-dd");

                await backup.SaveAsync();

                result = true;
                resultMessage = string.Format(Resources.BackupSaveOk, backup.Settings.FullName);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                result = false;
                resultMessage = string.Format(Resources.BackupSaveFail, e.Message);
            }
            finally
            {
                ProgramStatus.Stop(status);
            }

            Utils.MsgResult(result, resultMessage);
        }

        private void BackupLoad()
        {
            if (ProgramStatus.Contains(Status.BackupSave) || ProgramStatus.Contains(Status.BackupLoad))
            {
                Msg.Info(Resources.BackupInfoInProgress);

                return;
            }

            folderBrowserDialog.SelectedPath = "d:\\docs\\Projects.exe\\TileExplorer\\Debug\\0.0.0.0\\backup\\2025-11-10\\";

            /*           folderBrowserDialog.SelectedPath = AppSettings.Local.Default.BackupSettings.Directory;

                       if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                       {
                           return;
                       }

                       if (!FrmBackup.ShowDlg(this))
                       {
                           return;
                       }*/

            var settings = new Backup.BackupSettings
            {
                FullName = folderBrowserDialog.SelectedPath,
                Markers = Backup.FileType.ExcelXml,
                Equipments = Backup.FileType.ExcelXml
            };

            var status = ProgramStatus.Start(Status.BackupLoad);

            bool result;
            string resultMessage;

            try
            {
                var backup = new Backup()
                {
                    Settings = settings
                };

                backup.Load();

                result = true;
                resultMessage = string.Format(Resources.BackupLoadOk, backup.Settings.FullName);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                result = false;
                resultMessage = string.Format(Resources.BackupLoadFail, e.Message);
            }
            finally
            {
                ProgramStatus.Stop(status);
            }

            Utils.MsgResult(result, resultMessage);
        }

        private async void MiMainDataBackupSave_Click(object sender, EventArgs e)
        {
            await BackupSaveAsync();
        }

        private void MiMainBackupLoad_Click(object sender, EventArgs e)
        {
            BackupLoad();
        }

        private void GMapControl_Paint(object sender, PaintEventArgs e)
        {
            mapZoomRuler.Paint(e.Graphics);
        }

        private void OpenChartTrackEle(Track value)
        {
            foreach (var frm in Utils.Forms.GetChildForms<FrmChartTrackEle>(ChildFormType.ChartTrackEle))
            {
                if (frm.Track.Id == value.Id)
                {
                    frm.BringToFront();
                    return;
                }
            }
            ;

            FrmChartTrackEle.ShowFrm(this, value);
        }

        public void ShowChartTrackEle(object sender, Track value)
        {
            OpenChartTrackEle(value);
        }

        public void ShowMarkerPosition(object sender, PointLatLng value)
        {
            markerPosition.Position = value;

            markerPosition.IsVisible = value != default;
        }

        private void MiTrackShowChartTrackEle_Click(object sender, EventArgs e)
        {
            OpenChartTrackEle(SelectedTrack.Model);
        }

        private async void MiMainDataCheckDirectoryTracks_Click(object sender, EventArgs e)
        {
            await CheckDirectoryTracksAsync(true);
        }

        private void TsbtnChartTracks_Click(object sender, EventArgs e)
        {
            miMainDataChartTracks.PerformClick();
        }

        private void MiMainDataChartTracks_Click(object sender, EventArgs e)
        {
            if (Years.Count == 0)
            {
                return;
            }

            var frmList = Utils.Forms.GetChildForms<FrmChartTracksByYear>(ChildFormType.ChartTracksByYear);

            var year = Years.Last();

            if (frmList != null && frmList.Count == 1 && frmList.First().Year == year)
            {
                frmList.First().Close();
                return;
            }

            Utils.Forms.OpenChartTracksByYear(this, year);
        }

        private void MiMainTilesHeatmap_Click(object sender, EventArgs e)
        {
            TilesSetHeatmap(miMainTilesHeatmap.Checked);
        }
    }
}