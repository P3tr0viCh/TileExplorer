//#define CHECK_TILES
//#define SHOW_TRACK_KM

using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using P3tr0viCh;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
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

            DB = new Database(Path.ChangeExtension(Application.ExecutablePath, ".sqlite"));

            statusStripPresenter = new StatusStripPresenter(this);

            frmTrackList = new FrmTrackList(this);
            frmMarkerList = new FrmMarkerList(this);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            SettingsExt.LoadFormBounds(this);

            SettingsExt.LoadFormBounds(frmTrackList);
            SettingsExt.LoadFormBounds(frmMarkerList);

#if DEBUG
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

            SettingsExt.SaveFormBounds(this);

            SettingsExt.SaveFormBounds(frmTrackList);
            SettingsExt.SaveFormBounds(frmMarkerList);

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

            DropTilesAsync();

            SaveTilesAsync(tiles);
        }
#endif

        private async void DropTilesAsync()
        {
            await DB.DropTilesAsync();
        }

        private async void SaveTilesAsync(List<TileModel> tiles)
        {
            await DB.SaveTilesAsync(tiles);
        }

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

        private async void SaveTileAsync(TileModel tile)
        {
            await DB.SaveTileAsync(tile);
        }

        private async void DeleteTileAsync(TileModel tile)
        {
            await DB.DeleteTileAsync(tile);
        }

        private async void DeleteMarkerAsync(MarkerModel marker)
        {
            await DB.DeleteMarkerAsync(marker);
        }

        private async void DeleteTrackAsync(TrackModel track)
        {
            await DB.DeleteTrackAsync(track);
        }

        private async void SaveMarkerAsync(MarkerModel marker)
        {
            await DB.SaveMarkerAsync(marker);
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

            Msg.Info(text);
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

                    frmTrackList.Selected = selectedTrack.TrackModel;
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

                    frmMarkerList.Selected = selectedMarker.MarkerModel;
                }
            }
        }

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
                            SaveMarkerAsync(SelectedMarker.MarkerModel);

                            frmMarkerList.Update(SelectedMarker.MarkerModel);
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
                        MarkerAdd(gMapControl.FromLocalToLatLng(e.X, e.Y));
                    }
                    else
                    {
                        var track = SelectedTrack.TrackModel;

                        Msg.Info(track.Text + "\n" + track.DateTime + "\n" +
                            track.TrackPoints.Count + "\n" + SelectedTrack.Points.Count + "\n" +
                            track.Distance / 1000.0 + "\n" + SelectedTrack.Distance);
                    }
                }
                else
                {
                    MarkerChange(SelectedMarker);
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

        private void MarkerAdd(PointLatLng point)
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
                SaveMarkerAsync(marker);

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

        private void MarkerChange(MapMarker marker)
        {
            ChildFormsTopMost = false;

            if (FrmMarker.ShowDlg(this, marker.MarkerModel))
            {
                SaveMarkerAsync(marker.MarkerModel);

                marker.NotifyModelChanged();

                frmMarkerList.Update(marker.MarkerModel);
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

            ChildFormsTopMost = false;

            if (Msg.Question(string.Format(Resources.QuestionMarkerDelete, Name)))
            {
                DeleteMarkerAsync(SelectedMarker.MarkerModel);

                markersOverlay.Markers.Remove(SelectedMarker);

                frmMarkerList.Delete(SelectedMarker.MarkerModel);
            }

            ChildFormsTopMost = true;
        }

        private void MiTrackDelete_Click(object sender, EventArgs e)
        {
            if (SelectedTrack == null) return;

            string Name = SelectedTrack.TrackModel.Text;

            if (Name == "")
            {
                Name = SelectedTrack.TrackModel.DateTime.ToString();
            }

            ChildFormsTopMost = false;

            if (Msg.Question(string.Format(Resources.QuestionTrackDelete, Name)))
            {
                DeleteTrackAsync(SelectedTrack.TrackModel);

                tracksOverlay.Routes.Remove(SelectedTrack);
            }

            ChildFormsTopMost = true;
        }

        PointLatLng SelectedMarkerPosition;

        private void MiMarkerMove_Click(object sender, EventArgs e)
        {
            if (SelectedMarker == null) return;

            MarkerMoving = true;

            SelectedMarkerPosition = SelectedMarker.Position;

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
                SaveTileAsync(new TileModel()
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

            DeleteTileAsync(new TileModel()
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

        private async void OpenTrackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChildFormsTopMost = false;

            bool open = openFileDialog.ShowDialog() == DialogResult.OK;

            ChildFormsTopMost = true;

            if (open)
            {
                Status = ProgramStatus.LoadGpx;

                foreach (var file in openFileDialog.FileNames)
                {
                    var track = await OpenTrackFromFileAsync(file);

                    await DB.SaveTrackAsync(track);

                    frmTrackList.Add(track);

                    tracksOverlay.Routes.Add(new MapTrack(track));
                }

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
        }

        public void SelectTrackById(long id)
        {
            foreach (var track in from MapTrack track in tracksOverlay.Routes
                                  where track.TrackModel.Id == id
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
                                   where marker.MarkerModel.Id == id
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