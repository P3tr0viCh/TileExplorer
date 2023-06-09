﻿//#define CHECK_TILES
//#define SHOW_TRACK_KM
//#define DUMMY_TILES

using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using P3tr0viCh.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.StatusStripPresenter;

namespace TileExplorer
{
    public interface IMainForm
    {
        void SelectMapItemById(object sender, long id);

        void ChangeMapItemById(object sender, long id);
    }

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

        private readonly FrmFilter frmFilter;

        private readonly FrmResults frmResults;
        private readonly FrmTrackList frmTrackList;
        private readonly FrmMarkerList frmMarkerList;

        public Main()
        {
            InitializeComponent();

            AppSettings.Directory =
#if DEBUG
                 Files.ExecutableDirectory();
#else
                Files.AppDataDirectory();
#endif

            Debug.WriteLine("settings: " + AppSettings.FilePath);

            AppSettings.Default.Load();

            UpdateDatabaseFileName();

            statusStripPresenter = new StatusStripPresenter(this);

            frmResults = new FrmResults(this);
            frmTrackList = new FrmTrackList(this);
            frmMarkerList = new FrmMarkerList(this);

            Database.Filter.Default.Day = AppSettings.Default.Filter.Day;
            Database.Filter.Default.DateFrom = AppSettings.Default.Filter.DateFrom;
            Database.Filter.Default.DateTo = AppSettings.Default.Filter.DateTo;
            Database.Filter.Default.Years = AppSettings.Default.Filter.Years;

            frmFilter = new FrmFilter(this);

            Database.Filter.Default.OnChanged += Filter_OnChanged;
        }

        private void Filter_OnChanged()
        {
            var load = DataLoad.Tiles | DataLoad.TracksInfo;

            if (frmTrackList.Visible) load |= DataLoad.TrackList;

            if (miMainShowTracks.Checked) load |= DataLoad.Tracks;

            _ = DataUpdateAsync(load);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            AppSettings.LoadFormState(this, AppSettings.Default.FormStateMain);

            AppSettings.LoadFormState(frmResults, AppSettings.Default.FormStateResults);

            AppSettings.LoadFormState(frmTrackList, AppSettings.Default.FormStateTrackList);
            AppSettings.LoadFormState(frmMarkerList, AppSettings.Default.FormStateMarkerList);

            AppSettings.LoadFormState(frmFilter, AppSettings.Default.FormStateFilter);

            AppSettings.LoadDataGridColumns(frmResults.DataGridView, AppSettings.Default.ResultsColumns);
            AppSettings.LoadDataGridColumns(frmTrackList.DataGridView, AppSettings.Default.TrackListColumns);
            AppSettings.LoadDataGridColumns(frmMarkerList.DataGridView, AppSettings.Default.MarkerListColumns);

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

            frmResults.Visible = AppSettings.Default.VisibleResults;

            frmTrackList.Visible = AppSettings.Default.VisibleTrackList;
            frmMarkerList.Visible = AppSettings.Default.VisibleMarkerList;

            frmFilter.Visible = AppSettings.Default.VisibleFilter;

            StartUpdateGrid();

            _ = DataUpdateAsync();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            FullScreen = false;

            AppSettings.Default.FormStateMain = AppSettings.SaveFormState(this);

            AppSettings.Default.FormStateResults = AppSettings.SaveFormState(frmResults);

            AppSettings.Default.FormStateTrackList = AppSettings.SaveFormState(frmTrackList);
            AppSettings.Default.FormStateMarkerList = AppSettings.SaveFormState(frmMarkerList);

            AppSettings.Default.FormStateFilter = AppSettings.SaveFormState(frmFilter);

            AppSettings.Default.ResultsColumns = AppSettings.SaveDataGridColumns(frmResults.DataGridView);
            AppSettings.Default.TrackListColumns = AppSettings.SaveDataGridColumns(frmTrackList.DataGridView);
            AppSettings.Default.MarkerListColumns = AppSettings.SaveDataGridColumns(frmMarkerList.DataGridView);

            AppSettings.Default.MapGrayScale = miMainGrayScale.Checked;

            AppSettings.Default.VisibleGrid = miMainShowGrid.Checked;
            AppSettings.Default.VisibleTracks = miMainShowTracks.Checked;
            AppSettings.Default.VisibleMarkers = miMainShowMarkers.Checked;

            AppSettings.Default.VisibleFilter = frmFilter.Visible;

            AppSettings.Default.VisibleResults = frmResults.Visible;

            AppSettings.Default.VisibleTrackList = frmTrackList.Visible;
            AppSettings.Default.VisibleMarkerList = frmMarkerList.Visible;

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

            Debug.WriteLine("useragent: " + GMap.NET.MapProviders.GMapProvider.UserAgent);

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

            miMainSaveTileBoundaryToOsm.Enabled = gMapControl.Zoom >= AppSettings.Default.SaveOsmTileMinZoom;

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

        private async Task CalcTilesAsync(List<Database.Models.Tile> tiles)
        {
            foreach (var tile in tiles)
            {
                tile.Status = Database.TileStatus.Visited;
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

            var tiles = await Database.Default.LoadTilesAsync(Database.Filter.Default);

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

        private async Task LoadTracksAsync(bool loadPoints)
        {
            Debug.WriteLine("LoadTracksAsync: start");

            tracksOverlay.Clear();

            var tracks = await Database.Default.LoadTracksAsync(Database.Filter.Default);

            foreach (var track in tracks)
            {
                if (loadPoints)
                {
                    await Database.Default.LoadTrackPointsAsync(track);
                }

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

            var markers = await Database.Default.LoadMarkersAsync();

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

            var tracksInfo = await Database.Default.LoadTracksInfoAsync(Database.Filter.Default);

            statusStripPresenter.TracksCount = tracksInfo.Count;
            statusStripPresenter.TracksDistance = tracksInfo.Distance / 1000.0;

            Debug.WriteLine("LoadTracksInfoAsync: end");
        }

        private async Task LoadResultsAsync()
        {
            Debug.WriteLine("LoadResultsAsync: start");

            var results = await Database.Default.LoadResultsAsync();

            var resultSum = new Database.Models.Results { Count = 0, DistanceSum = 0.0 };

            foreach (var result in results)
            {
                resultSum.Count += result.Count;
                resultSum.DistanceSum += result.DistanceSum;
            }

            results.Add(resultSum);

            frmResults.List = results;

            Debug.WriteLine("LoadResultsAsync: end");
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

                frmResults.Updating = value != ProgramStatus.Idle;

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
            TrackList = 8,
            TracksInfo = 16,
            Results = 32,
        }

        private async Task DataUpdateAsync(DataLoad load = default)
        {
            Status = ProgramStatus.LoadData;

            if (load == default)
            {
                load = DataLoad.Tiles | DataLoad.TracksInfo;

                if (frmTrackList.Visible) load |= DataLoad.TrackList;

                if (miMainShowTracks.Checked) load |= DataLoad.Tracks;

                if (frmMarkerList.Visible || miMainShowMarkers.Checked) load |= DataLoad.Markers;

                if (frmResults.Visible) load |= DataLoad.Results;
            }

            if (load.HasFlag(DataLoad.Tracks))
            {
                load |= DataLoad.TracksInfo;

                load ^= DataLoad.TrackList;
            }

            Debug.WriteLine($"Loading data {load}");

            Selected = null;

            if (load.HasFlag(DataLoad.Tiles)) await LoadTilesAsync();

            if (load.HasFlag(DataLoad.TracksInfo)) await LoadTracksInfoAsync();

            if (load.HasFlag(DataLoad.TrackList)) await LoadTracksAsync(false);

            if (load.HasFlag(DataLoad.Tracks)) await LoadTracksAsync(true);

            if (load.HasFlag(DataLoad.Markers)) await LoadMarkersAsync();

            if (load.HasFlag(DataLoad.Results)) await LoadResultsAsync();

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
            FrmAbout.Show(new FrmAbout.Options() { Link = Resources.GitHubLink });
        }

        private async void UpdateSelectedTrackTiles(Database.Models.Track track)
        {
            foreach (var tile in tilesOverlay.Polygons.Cast<MapTile>())
            {
                tile.Selected = false;
            }

            if (track == null) return;

            var tiles = await Database.Default.LoadTrackTilesAsync(track);

            foreach (var tile in from item in tilesOverlay.Polygons.Cast<MapTile>()
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
                            frmMarkerList.Selected = (Database.Models.Marker)selected.Model;

                            UpdateSelectedTrackTiles(null);

                            break;
                        case MapItemType.Track:
                            frmTrackList.Selected = (Database.Models.Track)selected.Model;

                            UpdateSelectedTrackTiles((Database.Models.Track)selected.Model);

                            break;
                    }
                }
                else
                {
                    UpdateSelectedTrackTiles(null);
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

        private IMapItem MouseOverMapItem()
        {
            if (gMapControl.IsMouseOverMarker)
            {
                foreach (var marker in markersOverlay.Markers)
                {
                    if (marker.IsMouseOver)
                    {
                        return (MapMarker)marker;
                    }
                }
            }

            if (gMapControl.IsMouseOverRoute)
            {
                foreach (var track in tracksOverlay.Routes)
                {
                    if (track.IsMouseOver)
                    {
                        return (MapTrack)track;
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

                    frmMarkerList.Update(SelectedMarker.Model);
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
                    MarkerAddAsync(gMapControl.FromLocalToLatLng(e.X, e.Y));
                }
                else
                {
                    switch (Selected.Type)
                    {
                        case MapItemType.Marker:
                            MarkerChangeAsync(SelectedMarker);
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
                        _ = DataUpdateAsync(DataLoad.Tracks);
                    }
                    break;
                case DataLoad.Markers:
                    if (markersOverlay.Markers.Count == 0)
                    {
                        _ = DataUpdateAsync(DataLoad.Markers);
                    }
                    break;
                case DataLoad.Results:
                    if (frmResults.DataGridView.Rows.Count == 0)
                    {
                        _ = DataUpdateAsync(DataLoad.Results);
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
            HomeSave();
        }

        private async void MarkerAddAsync(PointLatLng point)
        {
            bool prevMarkersVisible = markersOverlay.IsVisibile;

            markersOverlay.IsVisibile = true;

            var marker = new Database.Models.Marker()
            {
                Lat = point.Lat,
                Lng = point.Lng,

#if DEBUG
                Text = DateTime.Now.ToString(),
#endif
            };

            var markerTemp = new GMarkerCross(point)
            {
                Pen = new Pen(Brushes.Red, 2f)
            };

            markersOverlay.Markers.Add(markerTemp);

            if (FrmMarker.ShowDlg(this, marker))
            {
                await Database.Default.SaveMarkerAsync(marker);

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
                await Database.Default.SaveMarkerAsync(marker.Model);

                marker.NotifyModelChanged();

                frmMarkerList.Update(marker.Model);

                if (ActiveForm != this) gMapControl.Invalidate();
            }
        }

        private async void MarkerDeleteAsync(MapMarker marker)
        {
            if (marker == null) return;

            var Name = marker.Model.Text;

            if (string.IsNullOrEmpty(Name))
            {
                Name = marker.Position.Lat.ToString() + ":" + marker.Position.Lng.ToString();
            }

            if (Msg.Question(string.Format(Resources.QuestionMarkerDelete, Name)))
            {
                await Database.Default.DeleteMarkerAsync(marker.Model);

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
                await Database.Default.SaveTrackAsync(track.Model);

                track.NotifyModelChanged();

                frmTrackList.Update(track.Model);

                if (ActiveForm != this) gMapControl.Invalidate();
            }
        }

        private async void TrackDeleteAsync(MapTrack track)
        {
            if (track == null) return;

            var Name = track.Model.Text;

            if (string.IsNullOrEmpty(Name))
            {
                Name = track.Model.DateTimeStart.ToString();
            }

            if (Msg.Question(string.Format(Resources.QuestionTrackDelete, Name)))
            {
                await Database.Default.DeleteTrackAsync(track.Model);

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
                    FullScreen = false;

                    Selected = null;

                    UpdateSelectedTrackTiles(null);

                    gMapControl.Invalidate();
                }
            }
        }

        private void MiMainDataUpdate_Click(object sender, EventArgs e)
        {
            _ = DataUpdateAsync();
        }

        private async void AddTileAsync(Database.Models.Tile tile)
        {
            Status = ProgramStatus.SaveData;

            await Database.Default.SaveTileAsync(tile);

            await DataUpdateAsync(DataLoad.Tiles);
        }

        private void MiMapTileAdd_Click(object sender, EventArgs e)
        {
            PointLatLng position = gMapControl.FromLocalToLatLng(MenuPopupPoint.X, MenuPopupPoint.Y);

            try
            {
                AddTileAsync(new Database.Models.Tile()
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

        private async void DeleteTileAsync(Database.Models.Tile tile)
        {
            Status = ProgramStatus.SaveData;

            await Database.Default.DeleteTileAsync(tile);

            await DataUpdateAsync(DataLoad.Tiles);
        }

        private void MiMapTileDelete_Click(object sender, EventArgs e)
        {
            var position = gMapControl.FromLocalToLatLng(MenuPopupPoint.X, MenuPopupPoint.Y);

            DeleteTileAsync(new Database.Models.Tile()
            {
                X = Utils.Osm.LngToTileX(position),
                Y = Utils.Osm.LatToTileY(position)
            });
        }

#if !DEBUG
        private enum SaveFileDialogType
        {
            Png, Osm
        }

        private bool ShowSaveFileDialog(SaveFileDialogType type)
        {
            switch (type)
            {
                case SaveFileDialogType.Png:
                    saveFileDialog.DefaultExt = Resources.FileSaveDefaultExtPng;
                    saveFileDialog.Filter = Resources.FileSaveFilterPng;
                    break;
                case SaveFileDialogType.Osm:
                    saveFileDialog.DefaultExt = Resources.FileSaveDefaultExtOsm;
                    saveFileDialog.Filter = Resources.FileSaveFilterOsm;
                    break;
            }

            return saveFileDialog.ShowDialog(this) == DialogResult.OK;
        }
#endif

        private void MiMainSaveToImage_Click(object sender, EventArgs e)
        {
            try
            {
#if DEBUG
                SaveToImage(Files.TempFileName("xxx.png"));
#else
                if (ShowSaveFileDialog(SaveFileDialogType.Png))
                {
                    SaveToImage(saveFileDialog.FileName);
                }
#endif
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                Msg.Error(ex.Message);
            }
        }

        private void MiMainSaveTileBoundaryToOsm_Click(object sender, EventArgs e)
        {
            var pointFrom = gMapControl.FromLocalToLatLng(0, 0);
            var pointTo = gMapControl.FromLocalToLatLng(gMapControl.Width, gMapControl.Height);

            var tiles = new List<Database.Models.Tile>();

            for (var x = Utils.Osm.LngToTileX(pointFrom); x <= Utils.Osm.LngToTileX(pointTo); x++)
                for (var y = Utils.Osm.LatToTileY(pointFrom); y <= Utils.Osm.LatToTileY(pointTo); y++)
                {
                    tiles.Add(new Database.Models.Tile()
                    {
                        X = x,
                        Y = y,
                    });
                }

            try
            {
#if DEBUG
                Utils.Osm.SaveTilesToFile(Files.TempFileName("xxx.osm"), tiles);
#else
                if (ShowSaveFileDialog(SaveFileDialogType.Osm))
                {
                    Utils.Osm.SaveTilesToFile(saveFileDialog.FileName, tiles);
                }
#endif
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                Msg.Error(ex.Message);
            }
        }

        private async Task<Database.Models.Track> OpenTrackFromFileAsync(string path)
        {
            return await Task.Run(() => { return Utils.Tracks.OpenTrackFromFile(path); });
        }

        private async Task<List<Database.Models.Tile>> GetTilesFromTrackAsync(Database.Models.Track track)
        {
            return await Task.Run(() => { return Utils.GetTilesFromTrack(track); });
        }

        private async Task OpenTracksAsync(string[] files)
        {
            Status = ProgramStatus.LoadGpx;

            Database.Models.Track track;

            List<Database.Models.Tile> trackTiles;

            foreach (var file in files)
            {
                track = await OpenTrackFromFileAsync(file);

                await Database.Default.SaveTrackAsync(track);

                frmTrackList.Add(track);

                var mapTrack = new MapTrack(track);

                tracksOverlay.Routes.Add(mapTrack);

                Selected = mapTrack;

                trackTiles = await GetTilesFromTrackAsync(track);

                var saveTiles = new List<Database.Models.Tile>();

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

                Debug.WriteLine("saveTiles count: " + saveTiles.Count);

                await Database.Default.SaveTilesAsync(saveTiles);

                var tracksTiles = new List<Database.Models.TracksTiles>();

                foreach (var tile in trackTiles)
                {
                    tracksTiles.Add(new Database.Models.TracksTiles()
                    {
                        TrackId = track.Id,
                        TileId = tile.Id,
                    });
                }

                await Database.Default.SaveTracksTilesAsync(tracksTiles);
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

                await DataUpdateAsync(DataLoad.Tiles | DataLoad.TracksInfo | DataLoad.Results);
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

        private void MiMainDataResults_Click(object sender, EventArgs e)
        {
            frmResults.Visible = !frmResults.Visible;

            CheckAndLoadData(DataLoad.Results);
        }

        private void MiMainGrayScale_Click(object sender, EventArgs e)
        {
            gMapControl.GrayScaleMode = miMainGrayScale.Checked;
        }

        public void SelectMapItemById(object sender, long id)
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

        public void ChangeMapItemById(object sender, long id)
        {
            SelectMapItemById(sender, id);

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

        private readonly Database.Models.Tile TileLeftTop = new Database.Models.Tile();
        private readonly Database.Models.Tile TileRightBottom = new Database.Models.Tile();

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

            foreach (var tile in gridOverlay.Polygons.Cast<MapTile>().
                Where(t => t.Model.X < TileLeftTop.X || t.Model.Y < TileLeftTop.Y ||
                           t.Model.X > TileRightBottom.X || t.Model.Y > TileRightBottom.Y).ToList())
            {
                gridOverlay.Polygons.Remove(tile);
            }

            for (var x = leftTopX; x <= rightBottomX; x++)
                for (var y = leftTopY; y <= rightBottomY; y++)
                {
                    if (x >= TileLeftTop.X && x <= TileRightBottom.X && y >= TileLeftTop.Y && y <= TileRightBottom.Y) continue;

                    if (tilesOverlay.Polygons.Cast<MapTile>().ToList().
                        Exists(t => t.Model.X == x && t.Model.Y == y)) continue;

                    gridOverlay.Polygons.Add(new MapTile(new Database.Models.Tile()
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

            Debug.WriteLine("database: " + databaseFileName);

            Database.Default.FileName = databaseFileName;
        }

        private void MiMainSettings_Click(object sender, EventArgs e)
        {
            if (FrmSettings.ShowDlg(this))
            {
                UpdateDatabaseFileName();

                _ = DataUpdateAsync();
            }
        }
    }
}