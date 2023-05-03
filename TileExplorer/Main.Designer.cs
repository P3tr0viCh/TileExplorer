namespace TileExplorer
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.gMapControl = new GMap.NET.WindowsForms.GMapControl();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.slZoom = new System.Windows.Forms.ToolStripStatusLabel();
            this.slPosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.slTileId = new System.Windows.Forms.ToolStripStatusLabel();
            this.slMousePosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.slStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.slTracksCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.slTracksDistance = new System.Windows.Forms.ToolStripStatusLabel();
            this.slTilesVisited = new System.Windows.Forms.ToolStripStatusLabel();
            this.slTilesMaxCluster = new System.Windows.Forms.ToolStripStatusLabel();
            this.slTilesMaxSquare = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.miMainFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainSaveToImage = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainSaveTileBoundaryToOsm = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainClose = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainMap = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainShowGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainShowTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainShowMarkers = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainFullScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainHome = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainHomeGoto = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainHomeSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainGrayScale = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainData = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainDataOpenTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainDataTrackList = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainDataMarkerList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainDataFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainDataUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.cmMarker = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miMarkerChange = new System.Windows.Forms.ToolStripMenuItem();
            this.miMarkerMove = new System.Windows.Forms.ToolStripMenuItem();
            this.miMarkerDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMap = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miMapMarkerAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.miMapTileAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.miMapTileDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.cmTrack = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miTrackChange = new System.Windows.Forms.ToolStripMenuItem();
            this.miTrackDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.timerMapMove = new System.Windows.Forms.Timer(this.components);
            this.statusStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.cmMarker.SuspendLayout();
            this.cmMap.SuspendLayout();
            this.cmTrack.SuspendLayout();
            this.SuspendLayout();
            // 
            // gMapControl
            // 
            this.gMapControl.BackColor = System.Drawing.SystemColors.Control;
            this.gMapControl.Bearing = 0F;
            this.gMapControl.CanDragMap = true;
            this.gMapControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.gMapControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gMapControl.EmptyTileColor = System.Drawing.Color.Navy;
            this.gMapControl.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gMapControl.GrayScaleMode = false;
            this.gMapControl.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMapControl.LevelsKeepInMemmory = 5;
            this.gMapControl.Location = new System.Drawing.Point(0, 0);
            this.gMapControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gMapControl.MarkersEnabled = true;
            this.gMapControl.MaxZoom = 2;
            this.gMapControl.MinZoom = 2;
            this.gMapControl.MouseWheelZoomEnabled = true;
            this.gMapControl.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMapControl.Name = "gMapControl";
            this.gMapControl.NegativeMode = false;
            this.gMapControl.PolygonsEnabled = true;
            this.gMapControl.RetryLoadTile = 0;
            this.gMapControl.RoutesEnabled = true;
            this.gMapControl.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMapControl.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMapControl.ShowTileGridLines = false;
            this.gMapControl.Size = new System.Drawing.Size(720, 415);
            this.gMapControl.TabIndex = 0;
            this.gMapControl.Zoom = 0D;
            this.gMapControl.OnPositionChanged += new GMap.NET.PositionChanged(this.GMapControl_OnPositionChanged);
            this.gMapControl.OnMapZoomChanged += new GMap.NET.MapZoomChanged(this.GMapControl_OnMapZoomChanged);
            this.gMapControl.Load += new System.EventHandler(this.GMapControl_Load);
            this.gMapControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GMapControl_MouseClick);
            this.gMapControl.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.GMapControl_MouseDoubleClick);
            this.gMapControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GMapControl_MouseMove);
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slZoom,
            this.slPosition,
            this.slTileId,
            this.slMousePosition,
            this.slStatus,
            this.slTracksCount,
            this.slTracksDistance,
            this.slTilesVisited,
            this.slTilesMaxCluster,
            this.slTilesMaxSquare});
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(720, 22);
            this.statusStrip.TabIndex = 1;
            // 
            // slZoom
            // 
            this.slZoom.Name = "slZoom";
            this.slZoom.Size = new System.Drawing.Size(61, 17);
            this.slZoom.Text = "zoom: xxx";
            // 
            // slPosition
            // 
            this.slPosition.Name = "slPosition";
            this.slPosition.Size = new System.Drawing.Size(62, 17);
            this.slPosition.Text = "pos: xx, yy";
            // 
            // slTileId
            // 
            this.slTileId.Name = "slTileId";
            this.slTileId.Size = new System.Drawing.Size(59, 17);
            this.slTileId.Text = "tile: xx, yy";
            // 
            // slMousePosition
            // 
            this.slMousePosition.Name = "slMousePosition";
            this.slMousePosition.Size = new System.Drawing.Size(79, 17);
            this.slMousePosition.Text = "mouse: xx, yy";
            // 
            // slStatus
            // 
            this.slStatus.Name = "slStatus";
            this.slStatus.Size = new System.Drawing.Size(52, 17);
            this.slStatus.Spring = true;
            this.slStatus.Text = "status";
            // 
            // slTracksCount
            // 
            this.slTracksCount.Name = "slTracksCount";
            this.slTracksCount.Size = new System.Drawing.Size(62, 17);
            this.slTracksCount.Text = "count: xxx";
            // 
            // slTracksDistance
            // 
            this.slTracksDistance.Name = "slTracksDistance";
            this.slTracksDistance.Size = new System.Drawing.Size(90, 17);
            this.slTracksDistance.Text = "distance: xxx.xx";
            // 
            // slTilesVisited
            // 
            this.slTilesVisited.Name = "slTilesVisited";
            this.slTilesVisited.Size = new System.Drawing.Size(52, 17);
            this.slTilesVisited.Text = "tiles: xxx";
            // 
            // slTilesMaxCluster
            // 
            this.slTilesMaxCluster.Name = "slTilesMaxCluster";
            this.slTilesMaxCluster.Size = new System.Drawing.Size(94, 17);
            this.slTilesMaxCluster.Text = "max_cluster: xxx";
            // 
            // slTilesMaxSquare
            // 
            this.slTilesMaxSquare.Name = "slTilesMaxSquare";
            this.slTilesMaxSquare.Size = new System.Drawing.Size(94, 17);
            this.slTilesMaxSquare.Text = "max_square: xxx";
            // 
            // menuStrip
            // 
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainFile,
            this.miMainMap,
            this.miMainData,
            this.miMainHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(720, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip";
            // 
            // miMainFile
            // 
            this.miMainFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainSaveToImage,
            this.miMainSaveTileBoundaryToOsm,
            this.toolStripSeparator1,
            this.miMainSettings,
            this.toolStripSeparator8,
            this.miMainClose});
            this.miMainFile.Name = "miMainFile";
            this.miMainFile.Size = new System.Drawing.Size(48, 20);
            this.miMainFile.Text = "Файл";
            // 
            // miMainSaveToImage
            // 
            this.miMainSaveToImage.Name = "miMainSaveToImage";
            this.miMainSaveToImage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.miMainSaveToImage.Size = new System.Drawing.Size(316, 22);
            this.miMainSaveToImage.Text = "Сохранить изображение...";
            this.miMainSaveToImage.Click += new System.EventHandler(this.MiMainSaveToImage_Click);
            // 
            // miMainSaveTileBoundaryToOsm
            // 
            this.miMainSaveTileBoundaryToOsm.Name = "miMainSaveTileBoundaryToOsm";
            this.miMainSaveTileBoundaryToOsm.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.miMainSaveTileBoundaryToOsm.Size = new System.Drawing.Size(316, 22);
            this.miMainSaveTileBoundaryToOsm.Text = "Сохранить сетку в файл OSM...";
            this.miMainSaveTileBoundaryToOsm.Click += new System.EventHandler(this.MiMainSaveTileBoundaryToOsm_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(313, 6);
            // 
            // miMainSettings
            // 
            this.miMainSettings.Name = "miMainSettings";
            this.miMainSettings.Size = new System.Drawing.Size(316, 22);
            this.miMainSettings.Text = "Настройки...";
            this.miMainSettings.Click += new System.EventHandler(this.MiMainSettings_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(313, 6);
            // 
            // miMainClose
            // 
            this.miMainClose.Name = "miMainClose";
            this.miMainClose.Size = new System.Drawing.Size(316, 22);
            this.miMainClose.Text = "Закрыть";
            this.miMainClose.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // miMainMap
            // 
            this.miMainMap.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainShowGrid,
            this.miMainShowTracks,
            this.miMainShowMarkers,
            this.toolStripSeparator2,
            this.miMainFullScreen,
            this.toolStripSeparator3,
            this.miMainHome,
            this.toolStripSeparator5,
            this.miMainGrayScale});
            this.miMainMap.Name = "miMainMap";
            this.miMainMap.Size = new System.Drawing.Size(50, 20);
            this.miMainMap.Text = "Карта";
            // 
            // miMainShowGrid
            // 
            this.miMainShowGrid.Checked = true;
            this.miMainShowGrid.CheckOnClick = true;
            this.miMainShowGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miMainShowGrid.Name = "miMainShowGrid";
            this.miMainShowGrid.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.miMainShowGrid.Size = new System.Drawing.Size(232, 22);
            this.miMainShowGrid.Text = "Сетка";
            this.miMainShowGrid.Click += new System.EventHandler(this.MiMainShowGrid_Click);
            // 
            // miMainShowTracks
            // 
            this.miMainShowTracks.Checked = true;
            this.miMainShowTracks.CheckOnClick = true;
            this.miMainShowTracks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miMainShowTracks.Name = "miMainShowTracks";
            this.miMainShowTracks.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.miMainShowTracks.Size = new System.Drawing.Size(232, 22);
            this.miMainShowTracks.Text = "Треки";
            this.miMainShowTracks.Click += new System.EventHandler(this.MiMainShowTracks_Click);
            // 
            // miMainShowMarkers
            // 
            this.miMainShowMarkers.Checked = true;
            this.miMainShowMarkers.CheckOnClick = true;
            this.miMainShowMarkers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miMainShowMarkers.Name = "miMainShowMarkers";
            this.miMainShowMarkers.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.miMainShowMarkers.Size = new System.Drawing.Size(232, 22);
            this.miMainShowMarkers.Text = "Маркеры";
            this.miMainShowMarkers.Click += new System.EventHandler(this.MiMainShowMarkers_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(229, 6);
            // 
            // miMainFullScreen
            // 
            this.miMainFullScreen.Name = "miMainFullScreen";
            this.miMainFullScreen.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.miMainFullScreen.Size = new System.Drawing.Size(232, 22);
            this.miMainFullScreen.Text = "Полноэкранный режим";
            this.miMainFullScreen.Click += new System.EventHandler(this.MiMainFullScreen_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(229, 6);
            // 
            // miMainHome
            // 
            this.miMainHome.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainHomeGoto,
            this.miMainHomeSave});
            this.miMainHome.Name = "miMainHome";
            this.miMainHome.Size = new System.Drawing.Size(232, 22);
            this.miMainHome.Text = "Дом";
            // 
            // miMainHomeGoto
            // 
            this.miMainHomeGoto.Name = "miMainHomeGoto";
            this.miMainHomeGoto.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.miMainHomeGoto.Size = new System.Drawing.Size(140, 22);
            this.miMainHomeGoto.Text = "Перейти";
            this.miMainHomeGoto.Click += new System.EventHandler(this.MiMainHomeGoto_Click);
            // 
            // miMainHomeSave
            // 
            this.miMainHomeSave.Name = "miMainHomeSave";
            this.miMainHomeSave.Size = new System.Drawing.Size(140, 22);
            this.miMainHomeSave.Text = "Сохранить";
            this.miMainHomeSave.Click += new System.EventHandler(this.MiMainHomeSave_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(229, 6);
            // 
            // miMainGrayScale
            // 
            this.miMainGrayScale.CheckOnClick = true;
            this.miMainGrayScale.Name = "miMainGrayScale";
            this.miMainGrayScale.Size = new System.Drawing.Size(232, 22);
            this.miMainGrayScale.Text = "Оттенки серого";
            this.miMainGrayScale.Click += new System.EventHandler(this.MiMainGrayScale_Click);
            // 
            // miMainData
            // 
            this.miMainData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainDataOpenTrack,
            this.toolStripSeparator6,
            this.miMainDataTrackList,
            this.miMainDataMarkerList,
            this.toolStripSeparator7,
            this.miMainDataFilter,
            this.toolStripSeparator4,
            this.miMainDataUpdate});
            this.miMainData.Name = "miMainData";
            this.miMainData.Size = new System.Drawing.Size(62, 20);
            this.miMainData.Text = "Данные";
            // 
            // miMainDataOpenTrack
            // 
            this.miMainDataOpenTrack.Name = "miMainDataOpenTrack";
            this.miMainDataOpenTrack.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.miMainDataOpenTrack.Size = new System.Drawing.Size(205, 22);
            this.miMainDataOpenTrack.Text = "Добавить трек...";
            this.miMainDataOpenTrack.Click += new System.EventHandler(this.MiMainDataOpenTrack_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(202, 6);
            // 
            // miMainDataTrackList
            // 
            this.miMainDataTrackList.Name = "miMainDataTrackList";
            this.miMainDataTrackList.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.T)));
            this.miMainDataTrackList.Size = new System.Drawing.Size(205, 22);
            this.miMainDataTrackList.Text = "Треки";
            this.miMainDataTrackList.Click += new System.EventHandler(this.MiMainDataTrackList_Click);
            // 
            // miMainDataMarkerList
            // 
            this.miMainDataMarkerList.Name = "miMainDataMarkerList";
            this.miMainDataMarkerList.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.M)));
            this.miMainDataMarkerList.Size = new System.Drawing.Size(205, 22);
            this.miMainDataMarkerList.Text = "Маркеры";
            this.miMainDataMarkerList.Click += new System.EventHandler(this.MiMainDataMarkerList_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(202, 6);
            // 
            // miMainDataFilter
            // 
            this.miMainDataFilter.Name = "miMainDataFilter";
            this.miMainDataFilter.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.miMainDataFilter.Size = new System.Drawing.Size(205, 22);
            this.miMainDataFilter.Text = "Фильтр";
            this.miMainDataFilter.Click += new System.EventHandler(this.MiMainDataFilter_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(202, 6);
            // 
            // miMainDataUpdate
            // 
            this.miMainDataUpdate.Name = "miMainDataUpdate";
            this.miMainDataUpdate.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.miMainDataUpdate.Size = new System.Drawing.Size(205, 22);
            this.miMainDataUpdate.Text = "Обновить";
            this.miMainDataUpdate.Click += new System.EventHandler(this.MiMainDataUpdate_Click);
            // 
            // miMainHelp
            // 
            this.miMainHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainAbout});
            this.miMainHelp.Name = "miMainHelp";
            this.miMainHelp.Size = new System.Drawing.Size(65, 20);
            this.miMainHelp.Text = "Справка";
            // 
            // miMainAbout
            // 
            this.miMainAbout.Name = "miMainAbout";
            this.miMainAbout.Size = new System.Drawing.Size(158, 22);
            this.miMainAbout.Text = "О программе...";
            this.miMainAbout.Click += new System.EventHandler(this.MiMainAbout_Click);
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.BottomToolStripPanel
            // 
            this.toolStripContainer.BottomToolStripPanel.Controls.Add(this.statusStrip);
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.AutoScroll = true;
            this.toolStripContainer.ContentPanel.Controls.Add(this.gMapControl);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(720, 415);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(720, 461);
            this.toolStripContainer.TabIndex = 3;
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.menuStrip);
            // 
            // cmMarker
            // 
            this.cmMarker.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMarkerChange,
            this.miMarkerMove,
            this.miMarkerDelete});
            this.cmMarker.Name = "cmMarker";
            this.cmMarker.Size = new System.Drawing.Size(147, 70);
            // 
            // miMarkerChange
            // 
            this.miMarkerChange.Name = "miMarkerChange";
            this.miMarkerChange.Size = new System.Drawing.Size(146, 22);
            this.miMarkerChange.Text = "Изменить...";
            this.miMarkerChange.Click += new System.EventHandler(this.MiMarkerChange_Click);
            // 
            // miMarkerMove
            // 
            this.miMarkerMove.Name = "miMarkerMove";
            this.miMarkerMove.Size = new System.Drawing.Size(146, 22);
            this.miMarkerMove.Text = "Переместить";
            this.miMarkerMove.Click += new System.EventHandler(this.MiMarkerMove_Click);
            // 
            // miMarkerDelete
            // 
            this.miMarkerDelete.Name = "miMarkerDelete";
            this.miMarkerDelete.Size = new System.Drawing.Size(146, 22);
            this.miMarkerDelete.Text = "Удалить";
            this.miMarkerDelete.Click += new System.EventHandler(this.MiMarkerDelete_Click);
            // 
            // cmMap
            // 
            this.cmMap.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMapMarkerAdd,
            this.miMapTileAdd,
            this.miMapTileDelete});
            this.cmMap.Name = "cmMap";
            this.cmMap.Size = new System.Drawing.Size(180, 70);
            this.cmMap.Opening += new System.ComponentModel.CancelEventHandler(this.CmMap_Opening);
            // 
            // miMapMarkerAdd
            // 
            this.miMapMarkerAdd.Name = "miMapMarkerAdd";
            this.miMapMarkerAdd.Size = new System.Drawing.Size(179, 22);
            this.miMapMarkerAdd.Text = "Добавить маркер...";
            this.miMapMarkerAdd.Click += new System.EventHandler(this.MiMapMarkerAdd_Click);
            // 
            // miMapTileAdd
            // 
            this.miMapTileAdd.Name = "miMapTileAdd";
            this.miMapTileAdd.Size = new System.Drawing.Size(179, 22);
            this.miMapTileAdd.Text = "add tile";
            this.miMapTileAdd.Click += new System.EventHandler(this.MiMapTileAdd_Click);
            // 
            // miMapTileDelete
            // 
            this.miMapTileDelete.Name = "miMapTileDelete";
            this.miMapTileDelete.Size = new System.Drawing.Size(179, 22);
            this.miMapTileDelete.Text = "delete tile";
            this.miMapTileDelete.Click += new System.EventHandler(this.MiMapTileDelete_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "gpx";
            this.openFileDialog.Filter = "GPX|*.gpx|Все файлы|*.*";
            this.openFileDialog.Multiselect = true;
            // 
            // cmTrack
            // 
            this.cmTrack.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miTrackChange,
            this.miTrackDelete});
            this.cmTrack.Name = "cmTrack";
            this.cmTrack.Size = new System.Drawing.Size(138, 48);
            // 
            // miTrackChange
            // 
            this.miTrackChange.Name = "miTrackChange";
            this.miTrackChange.Size = new System.Drawing.Size(137, 22);
            this.miTrackChange.Text = "Изменить...";
            this.miTrackChange.Click += new System.EventHandler(this.MiTrackChange_Click);
            // 
            // miTrackDelete
            // 
            this.miTrackDelete.Name = "miTrackDelete";
            this.miTrackDelete.Size = new System.Drawing.Size(137, 22);
            this.miTrackDelete.Text = "Удалить";
            this.miTrackDelete.Click += new System.EventHandler(this.MiTrackDelete_Click);
            // 
            // timerMapMove
            // 
            this.timerMapMove.Interval = 222;
            this.timerMapMove.Tick += new System.EventHandler(this.TimerMapMove_Tick);
            // 
            // Main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(720, 461);
            this.Controls.Add(this.toolStripContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Main";
            this.Text = "TileExplorer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.cmMarker.ResumeLayout(false);
            this.cmMap.ResumeLayout(false);
            this.cmTrack.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl gMapControl;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel slZoom;
        private System.Windows.Forms.ToolStripStatusLabel slPosition;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem miMainFile;
        private System.Windows.Forms.ToolStripMenuItem miMainClose;
        private System.Windows.Forms.ToolStripMenuItem miMainMap;
        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.ToolStripMenuItem miMainFullScreen;
        private System.Windows.Forms.ToolStripMenuItem miMainHelp;
        private System.Windows.Forms.ToolStripMenuItem miMainAbout;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripStatusLabel slTileId;
        private System.Windows.Forms.ToolStripStatusLabel slTilesVisited;
        private System.Windows.Forms.ToolStripStatusLabel slTilesMaxCluster;
        private System.Windows.Forms.ToolStripMenuItem miMainSaveToImage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miMainShowMarkers;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip cmMarker;
        private System.Windows.Forms.ToolStripMenuItem miMarkerDelete;
        private System.Windows.Forms.ContextMenuStrip cmMap;
        private System.Windows.Forms.ToolStripMenuItem miMapMarkerAdd;
        private System.Windows.Forms.ToolStripMenuItem miMarkerMove;
        private System.Windows.Forms.ToolStripMenuItem miMarkerChange;
        private System.Windows.Forms.ToolStripStatusLabel slMousePosition;
        private System.Windows.Forms.ToolStripStatusLabel slStatus;
        private System.Windows.Forms.ToolStripStatusLabel slTilesMaxSquare;
        private System.Windows.Forms.ToolStripMenuItem miMainData;
        private System.Windows.Forms.ToolStripMenuItem miMainDataUpdate;
        private System.Windows.Forms.ToolStripMenuItem miMapTileAdd;
        private System.Windows.Forms.ToolStripMenuItem miMapTileDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem miMainDataOpenTrack;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem miMainShowTracks;
        private System.Windows.Forms.ContextMenuStrip cmTrack;
        private System.Windows.Forms.ToolStripMenuItem miTrackDelete;
        private System.Windows.Forms.ToolStripMenuItem miMainDataMarkerList;
        private System.Windows.Forms.ToolStripMenuItem miMainDataTrackList;
        private System.Windows.Forms.ToolStripMenuItem miMainGrayScale;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem miMainHome;
        private System.Windows.Forms.ToolStripMenuItem miMainHomeGoto;
        private System.Windows.Forms.ToolStripMenuItem miMainHomeSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem miTrackChange;
        private System.Windows.Forms.ToolStripStatusLabel slTracksCount;
        private System.Windows.Forms.ToolStripStatusLabel slTracksDistance;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem miMainDataFilter;
        private System.Windows.Forms.ToolStripMenuItem miMainSaveTileBoundaryToOsm;
        private System.Windows.Forms.Timer timerMapMove;
        private System.Windows.Forms.ToolStripMenuItem miMainShowGrid;
        private System.Windows.Forms.ToolStripMenuItem miMainSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
    }
}

