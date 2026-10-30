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
            this.slUpdateStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.slTracksCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.slTracksDistance = new System.Windows.Forms.ToolStripStatusLabel();
            this.slTilesVisited = new System.Windows.Forms.ToolStripStatusLabel();
            this.slTilesArea = new System.Windows.Forms.ToolStripStatusLabel();
            this.slTilesMaxCluster = new System.Windows.Forms.ToolStripStatusLabel();
            this.slTilesMaxSquare = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.miMainFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainSaveToImage = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainSaveTileBoundaryToFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainSaveTileStatusToFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainOsm = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainOsmOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainOsmEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainBackupSave = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainBackupLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainClose = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainMap = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainShowGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainShowTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainShowTiles = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainTilesHeatmap = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainShowMarkers = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainHome = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainHomeGoto = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainHomeSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainGrayScale = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainData = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainDataOpenTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainDataCheckDirectoryTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainDataTrackList = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainDataMarkerList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainDataTagList = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainDataEquipmentList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainDataChartTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator21 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainDataResults = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainDataResultYears = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainDataResultEquipments = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainDataFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainDataUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainView = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainLeftPanel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainFullScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainCheckUpdates = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbtnTrackList = new System.Windows.Forms.ToolStripButton();
            this.tsbtnMarkerList = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnChartTracks = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator20 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnResults = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnFilter = new System.Windows.Forms.ToolStripButton();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.cmMarker = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miMarkerChange = new System.Windows.Forms.ToolStripMenuItem();
            this.miMarkerMove = new System.Windows.Forms.ToolStripMenuItem();
            this.miMarkerDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMap = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miMapMarkerAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.miMapShowTileInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.miMapOsmOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.miMapOsmEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.miMapCopyCoords = new System.Windows.Forms.ToolStripMenuItem();
            this.miMapCopyCoordsFloat = new System.Windows.Forms.ToolStripMenuItem();
            this.miMapCopyCoordsFloatLat = new System.Windows.Forms.ToolStripMenuItem();
            this.miMapCopyCoordsFloatLng = new System.Windows.Forms.ToolStripMenuItem();
            this.miMapCopyCoordsFloat2 = new System.Windows.Forms.ToolStripMenuItem();
            this.miMapCopyCoordsFloat2Lat = new System.Windows.Forms.ToolStripMenuItem();
            this.miMapCopyCoordsFloat2Lng = new System.Windows.Forms.ToolStripMenuItem();
            this.miMapTileAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.miMapTileDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.cmTrack = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miTrackShowChartTrackEle = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
            this.miTrackChange = new System.Windows.Forms.ToolStripMenuItem();
            this.miTrackDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.timerMapChange = new System.Windows.Forms.Timer(this.components);
            this.cmSelectedItems = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.xxxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.LeftToolStripPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.cmMarker.SuspendLayout();
            this.cmMap.SuspendLayout();
            this.cmTrack.SuspendLayout();
            this.cmSelectedItems.SuspendLayout();
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
            this.gMapControl.Size = new System.Drawing.Size(807, 415);
            this.gMapControl.TabIndex = 0;
            this.gMapControl.Zoom = 0D;
            this.gMapControl.OnPositionChanged += new GMap.NET.PositionChanged(this.GMapControl_OnPositionChanged);
            this.gMapControl.OnMapZoomChanged += new GMap.NET.MapZoomChanged(this.GMapControl_OnMapZoomChanged);
            this.gMapControl.SizeChanged += new System.EventHandler(this.GMapControl_SizeChanged);
            this.gMapControl.Paint += new System.Windows.Forms.PaintEventHandler(this.GMapControl_Paint);
            this.gMapControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GMapControl_MouseClick);
            this.gMapControl.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.GMapControl_MouseDoubleClick);
            this.gMapControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GMapControl_MouseMove);
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slZoom,
            this.slPosition,
            this.slTileId,
            this.slMousePosition,
            this.slStatus,
            this.slUpdateStatus,
            this.slTracksCount,
            this.slTracksDistance,
            this.slTilesVisited,
            this.slTilesArea,
            this.slTilesMaxCluster,
            this.slTilesMaxSquare});
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(848, 22);
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
            this.slStatus.Size = new System.Drawing.Size(63, 17);
            this.slStatus.Spring = true;
            this.slStatus.Text = "status";
            // 
            // slUpdateStatus
            // 
            this.slUpdateStatus.Name = "slUpdateStatus";
            this.slUpdateStatus.Size = new System.Drawing.Size(63, 17);
            this.slUpdateStatus.Spring = true;
            this.slUpdateStatus.Text = "update";
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
            // slTilesArea
            // 
            this.slTilesArea.Name = "slTilesArea";
            this.slTilesArea.Size = new System.Drawing.Size(53, 17);
            this.slTilesArea.Text = "area: xxx";
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
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(16, 18);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainFile,
            this.miMainMap,
            this.miMainData,
            this.miMainView,
            this.miMainHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(848, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip";
            // 
            // miMainFile
            // 
            this.miMainFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainSaveToImage,
            this.miMainSaveTileBoundaryToFile,
            this.miMainSaveTileStatusToFile,
            this.toolStripSeparator11,
            this.miMainOsm,
            this.toolStripSeparator18,
            this.miMainBackup,
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
            this.miMainSaveToImage.Size = new System.Drawing.Size(299, 22);
            this.miMainSaveToImage.Text = "Сохранить изображение";
            this.miMainSaveToImage.Click += new System.EventHandler(this.MiMainSaveToImage_Click);
            // 
            // miMainSaveTileBoundaryToFile
            // 
            this.miMainSaveTileBoundaryToFile.Name = "miMainSaveTileBoundaryToFile";
            this.miMainSaveTileBoundaryToFile.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.miMainSaveTileBoundaryToFile.Size = new System.Drawing.Size(299, 22);
            this.miMainSaveTileBoundaryToFile.Text = "Сохранить сетку";
            this.miMainSaveTileBoundaryToFile.Click += new System.EventHandler(this.MiMainSaveTileBoundaryToFile_Click);
            // 
            // miMainSaveTileStatusToFile
            // 
            this.miMainSaveTileStatusToFile.Name = "miMainSaveTileStatusToFile";
            this.miMainSaveTileStatusToFile.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.miMainSaveTileStatusToFile.Size = new System.Drawing.Size(299, 22);
            this.miMainSaveTileStatusToFile.Text = "Сохранить закрытые плитки";
            this.miMainSaveTileStatusToFile.Click += new System.EventHandler(this.MiMainSaveTileStatusToFile_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(296, 6);
            // 
            // miMainOsm
            // 
            this.miMainOsm.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainOsmOpen,
            this.miMainOsmEdit});
            this.miMainOsm.Name = "miMainOsm";
            this.miMainOsm.Size = new System.Drawing.Size(299, 22);
            this.miMainOsm.Text = "ОпенСтритМап";
            // 
            // miMainOsmOpen
            // 
            this.miMainOsmOpen.Name = "miMainOsmOpen";
            this.miMainOsmOpen.Size = new System.Drawing.Size(154, 22);
            this.miMainOsmOpen.Text = "Открыть";
            this.miMainOsmOpen.Click += new System.EventHandler(this.MiMainOsmOpen_Click);
            // 
            // miMainOsmEdit
            // 
            this.miMainOsmEdit.Name = "miMainOsmEdit";
            this.miMainOsmEdit.Size = new System.Drawing.Size(154, 22);
            this.miMainOsmEdit.Text = "Редактировать";
            this.miMainOsmEdit.Click += new System.EventHandler(this.MiMainOsmEdit_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(296, 6);
            // 
            // miMainBackup
            // 
            this.miMainBackup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainBackupSave,
            this.miMainBackupLoad});
            this.miMainBackup.Name = "miMainBackup";
            this.miMainBackup.Size = new System.Drawing.Size(299, 22);
            this.miMainBackup.Text = "Архив";
            // 
            // miMainBackupSave
            // 
            this.miMainBackupSave.Name = "miMainBackupSave";
            this.miMainBackupSave.Size = new System.Drawing.Size(149, 22);
            this.miMainBackupSave.Text = "Сохранить";
            this.miMainBackupSave.Click += new System.EventHandler(this.MiMainDataBackupSave_Click);
            // 
            // miMainBackupLoad
            // 
            this.miMainBackupLoad.Name = "miMainBackupLoad";
            this.miMainBackupLoad.Size = new System.Drawing.Size(149, 22);
            this.miMainBackupLoad.Text = "Восстановить";
            this.miMainBackupLoad.Click += new System.EventHandler(this.MiMainBackupLoad_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(296, 6);
            // 
            // miMainSettings
            // 
            this.miMainSettings.Name = "miMainSettings";
            this.miMainSettings.Size = new System.Drawing.Size(299, 22);
            this.miMainSettings.Text = "Настройки";
            this.miMainSettings.Click += new System.EventHandler(this.MiMainSettings_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(296, 6);
            // 
            // miMainClose
            // 
            this.miMainClose.Name = "miMainClose";
            this.miMainClose.Size = new System.Drawing.Size(299, 22);
            this.miMainClose.Text = "Закрыть";
            this.miMainClose.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // miMainMap
            // 
            this.miMainMap.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainShowGrid,
            this.miMainShowTracks,
            this.miMainShowTiles,
            this.miMainTilesHeatmap,
            this.miMainShowMarkers,
            this.toolStripSeparator2,
            this.miMainHome,
            this.toolStripSeparator5,
            this.miMainGrayScale});
            this.miMainMap.Name = "miMainMap";
            this.miMainMap.Size = new System.Drawing.Size(50, 20);
            this.miMainMap.Text = "Карта";
            // 
            // miMainShowGrid
            // 
            this.miMainShowGrid.CheckOnClick = true;
            this.miMainShowGrid.Name = "miMainShowGrid";
            this.miMainShowGrid.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.miMainShowGrid.Size = new System.Drawing.Size(247, 22);
            this.miMainShowGrid.Text = "Сетка";
            this.miMainShowGrid.Click += new System.EventHandler(this.MiMainShowGrid_Click);
            // 
            // miMainShowTracks
            // 
            this.miMainShowTracks.CheckOnClick = true;
            this.miMainShowTracks.Name = "miMainShowTracks";
            this.miMainShowTracks.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.miMainShowTracks.Size = new System.Drawing.Size(247, 22);
            this.miMainShowTracks.Text = "Треки";
            this.miMainShowTracks.Click += new System.EventHandler(this.MiMainShowTracks_Click);
            // 
            // miMainShowTiles
            // 
            this.miMainShowTiles.CheckOnClick = true;
            this.miMainShowTiles.Name = "miMainShowTiles";
            this.miMainShowTiles.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.miMainShowTiles.Size = new System.Drawing.Size(247, 22);
            this.miMainShowTiles.Text = "Плитки";
            this.miMainShowTiles.Click += new System.EventHandler(this.MiMainShowTiles_Click);
            // 
            // miMainTilesHeatmap
            // 
            this.miMainTilesHeatmap.CheckOnClick = true;
            this.miMainTilesHeatmap.Name = "miMainTilesHeatmap";
            this.miMainTilesHeatmap.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.miMainTilesHeatmap.Size = new System.Drawing.Size(247, 22);
            this.miMainTilesHeatmap.Text = "Плитки: тепловая карта";
            this.miMainTilesHeatmap.Click += new System.EventHandler(this.MiMainTilesHeatmap_Click);
            // 
            // miMainShowMarkers
            // 
            this.miMainShowMarkers.CheckOnClick = true;
            this.miMainShowMarkers.Name = "miMainShowMarkers";
            this.miMainShowMarkers.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.miMainShowMarkers.Size = new System.Drawing.Size(247, 22);
            this.miMainShowMarkers.Text = "Маркеры";
            this.miMainShowMarkers.Click += new System.EventHandler(this.MiMainShowMarkers_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(244, 6);
            // 
            // miMainHome
            // 
            this.miMainHome.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainHomeGoto,
            this.miMainHomeSave});
            this.miMainHome.Name = "miMainHome";
            this.miMainHome.Size = new System.Drawing.Size(247, 22);
            this.miMainHome.Text = "Дом";
            // 
            // miMainHomeGoto
            // 
            this.miMainHomeGoto.Name = "miMainHomeGoto";
            this.miMainHomeGoto.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.miMainHomeGoto.Size = new System.Drawing.Size(179, 22);
            this.miMainHomeGoto.Text = "Перейти";
            this.miMainHomeGoto.Click += new System.EventHandler(this.MiMainHomeGoto_Click);
            // 
            // miMainHomeSave
            // 
            this.miMainHomeSave.Name = "miMainHomeSave";
            this.miMainHomeSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.miMainHomeSave.Size = new System.Drawing.Size(179, 22);
            this.miMainHomeSave.Text = "Сохранить";
            this.miMainHomeSave.Click += new System.EventHandler(this.MiMainHomeSave_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(244, 6);
            // 
            // miMainGrayScale
            // 
            this.miMainGrayScale.CheckOnClick = true;
            this.miMainGrayScale.Name = "miMainGrayScale";
            this.miMainGrayScale.Size = new System.Drawing.Size(247, 22);
            this.miMainGrayScale.Text = "Оттенки серого";
            this.miMainGrayScale.Click += new System.EventHandler(this.MiMainGrayScale_Click);
            // 
            // miMainData
            // 
            this.miMainData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainDataOpenTrack,
            this.miMainDataCheckDirectoryTracks,
            this.toolStripSeparator6,
            this.miMainDataTrackList,
            this.miMainDataMarkerList,
            this.toolStripSeparator9,
            this.miMainDataTagList,
            this.miMainDataEquipmentList,
            this.toolStripSeparator7,
            this.miMainDataChartTracks,
            this.toolStripSeparator21,
            this.miMainDataResults,
            this.toolStripSeparator14,
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
            this.miMainDataOpenTrack.Size = new System.Drawing.Size(262, 22);
            this.miMainDataOpenTrack.Text = "Добавить трек";
            this.miMainDataOpenTrack.Click += new System.EventHandler(this.MiMainDataOpenTrack_Click);
            // 
            // miMainDataCheckDirectoryTracks
            // 
            this.miMainDataCheckDirectoryTracks.Name = "miMainDataCheckDirectoryTracks";
            this.miMainDataCheckDirectoryTracks.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.miMainDataCheckDirectoryTracks.Size = new System.Drawing.Size(262, 22);
            this.miMainDataCheckDirectoryTracks.Text = "Поиск новых треков";
            this.miMainDataCheckDirectoryTracks.Click += new System.EventHandler(this.MiMainDataCheckDirectoryTracks_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(259, 6);
            // 
            // miMainDataTrackList
            // 
            this.miMainDataTrackList.Name = "miMainDataTrackList";
            this.miMainDataTrackList.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.T)));
            this.miMainDataTrackList.Size = new System.Drawing.Size(262, 22);
            this.miMainDataTrackList.Text = "Треки";
            this.miMainDataTrackList.Click += new System.EventHandler(this.MiMainDataTrackList_Click);
            // 
            // miMainDataMarkerList
            // 
            this.miMainDataMarkerList.Name = "miMainDataMarkerList";
            this.miMainDataMarkerList.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.M)));
            this.miMainDataMarkerList.Size = new System.Drawing.Size(262, 22);
            this.miMainDataMarkerList.Text = "Маркеры";
            this.miMainDataMarkerList.Click += new System.EventHandler(this.MiMainDataMarkerList_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(259, 6);
            // 
            // miMainDataTagList
            // 
            this.miMainDataTagList.Name = "miMainDataTagList";
            this.miMainDataTagList.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.G)));
            this.miMainDataTagList.Size = new System.Drawing.Size(262, 22);
            this.miMainDataTagList.Text = "Теги";
            this.miMainDataTagList.Click += new System.EventHandler(this.MiMainDataTagList_Click);
            // 
            // miMainDataEquipmentList
            // 
            this.miMainDataEquipmentList.Name = "miMainDataEquipmentList";
            this.miMainDataEquipmentList.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.E)));
            this.miMainDataEquipmentList.Size = new System.Drawing.Size(262, 22);
            this.miMainDataEquipmentList.Text = "Снаряжение";
            this.miMainDataEquipmentList.Click += new System.EventHandler(this.MiMainDataEquipmentList_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(259, 6);
            // 
            // miMainDataChartTracks
            // 
            this.miMainDataChartTracks.Name = "miMainDataChartTracks";
            this.miMainDataChartTracks.Size = new System.Drawing.Size(262, 22);
            this.miMainDataChartTracks.Text = "График треков";
            this.miMainDataChartTracks.Click += new System.EventHandler(this.MiMainDataChartTracks_Click);
            // 
            // toolStripSeparator21
            // 
            this.toolStripSeparator21.Name = "toolStripSeparator21";
            this.toolStripSeparator21.Size = new System.Drawing.Size(259, 6);
            // 
            // miMainDataResults
            // 
            this.miMainDataResults.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainDataResultYears,
            this.miMainDataResultEquipments});
            this.miMainDataResults.Name = "miMainDataResults";
            this.miMainDataResults.Size = new System.Drawing.Size(262, 22);
            this.miMainDataResults.Text = "Итоги";
            // 
            // miMainDataResultYears
            // 
            this.miMainDataResultYears.Name = "miMainDataResultYears";
            this.miMainDataResultYears.Size = new System.Drawing.Size(198, 22);
            this.miMainDataResultYears.Text = "Итоги по годам";
            this.miMainDataResultYears.Click += new System.EventHandler(this.MiMainDataResultYears_Click);
            // 
            // miMainDataResultEquipments
            // 
            this.miMainDataResultEquipments.Name = "miMainDataResultEquipments";
            this.miMainDataResultEquipments.Size = new System.Drawing.Size(198, 22);
            this.miMainDataResultEquipments.Text = "Итоги по снаряжению";
            this.miMainDataResultEquipments.Click += new System.EventHandler(this.MiMainDataResultEquipments_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(259, 6);
            // 
            // miMainDataFilter
            // 
            this.miMainDataFilter.Name = "miMainDataFilter";
            this.miMainDataFilter.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.miMainDataFilter.Size = new System.Drawing.Size(262, 22);
            this.miMainDataFilter.Text = "Фильтр";
            this.miMainDataFilter.Click += new System.EventHandler(this.MiMainDataFilter_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(259, 6);
            // 
            // miMainDataUpdate
            // 
            this.miMainDataUpdate.Name = "miMainDataUpdate";
            this.miMainDataUpdate.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.miMainDataUpdate.Size = new System.Drawing.Size(262, 22);
            this.miMainDataUpdate.Text = "Обновить";
            this.miMainDataUpdate.Click += new System.EventHandler(this.MiMainDataUpdate_Click);
            // 
            // miMainView
            // 
            this.miMainView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainLeftPanel,
            this.toolStripSeparator3,
            this.miMainFullScreen});
            this.miMainView.Name = "miMainView";
            this.miMainView.Size = new System.Drawing.Size(39, 20);
            this.miMainView.Text = "Вид";
            // 
            // miMainLeftPanel
            // 
            this.miMainLeftPanel.CheckOnClick = true;
            this.miMainLeftPanel.Name = "miMainLeftPanel";
            this.miMainLeftPanel.Size = new System.Drawing.Size(232, 22);
            this.miMainLeftPanel.Text = "Панель кнопок";
            this.miMainLeftPanel.Click += new System.EventHandler(this.MiMainLeftPanel_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(229, 6);
            // 
            // miMainFullScreen
            // 
            this.miMainFullScreen.Name = "miMainFullScreen";
            this.miMainFullScreen.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.miMainFullScreen.Size = new System.Drawing.Size(232, 22);
            this.miMainFullScreen.Text = "Полноэкранный режим";
            this.miMainFullScreen.Click += new System.EventHandler(this.MiMainFullScreen_Click);
            // 
            // miMainHelp
            // 
            this.miMainHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainCheckUpdates,
            this.toolStripSeparator17,
            this.miMainAbout});
            this.miMainHelp.Name = "miMainHelp";
            this.miMainHelp.Size = new System.Drawing.Size(65, 20);
            this.miMainHelp.Text = "Справка";
            // 
            // miMainCheckUpdates
            // 
            this.miMainCheckUpdates.Name = "miMainCheckUpdates";
            this.miMainCheckUpdates.Size = new System.Drawing.Size(204, 22);
            this.miMainCheckUpdates.Text = "Проверить обновления";
            this.miMainCheckUpdates.Click += new System.EventHandler(this.MiMainCheckUpdates_Click);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(201, 6);
            // 
            // miMainAbout
            // 
            this.miMainAbout.Name = "miMainAbout";
            this.miMainAbout.Size = new System.Drawing.Size(204, 22);
            this.miMainAbout.Text = "О программе";
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
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(807, 415);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // toolStripContainer.LeftToolStripPanel
            // 
            this.toolStripContainer.LeftToolStripPanel.Controls.Add(this.toolStrip);
            this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(848, 461);
            this.toolStripContainer.TabIndex = 3;
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.menuStrip);
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnTrackList,
            this.tsbtnMarkerList,
            this.toolStripSeparator15,
            this.tsbtnChartTracks,
            this.toolStripSeparator20,
            this.tsbtnResults,
            this.toolStripSeparator10,
            this.tsbtnFilter});
            this.toolStrip.Location = new System.Drawing.Point(0, 4);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(41, 191);
            this.toolStrip.TabIndex = 1;
            // 
            // tsbtnTrackList
            // 
            this.tsbtnTrackList.AutoSize = false;
            this.tsbtnTrackList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnTrackList.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnTrackList.Image")));
            this.tsbtnTrackList.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnTrackList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnTrackList.Name = "tsbtnTrackList";
            this.tsbtnTrackList.Size = new System.Drawing.Size(40, 32);
            this.tsbtnTrackList.Text = "Т";
            this.tsbtnTrackList.ToolTipText = "Треки";
            this.tsbtnTrackList.Click += new System.EventHandler(this.TsbtnTrackList_Click);
            // 
            // tsbtnMarkerList
            // 
            this.tsbtnMarkerList.AutoSize = false;
            this.tsbtnMarkerList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnMarkerList.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnMarkerList.Image")));
            this.tsbtnMarkerList.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnMarkerList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnMarkerList.Name = "tsbtnMarkerList";
            this.tsbtnMarkerList.Size = new System.Drawing.Size(40, 32);
            this.tsbtnMarkerList.Text = "М";
            this.tsbtnMarkerList.ToolTipText = "Маркеры";
            this.tsbtnMarkerList.Click += new System.EventHandler(this.TsbtnMarkerList_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(39, 6);
            // 
            // tsbtnChartTracks
            // 
            this.tsbtnChartTracks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnChartTracks.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnChartTracks.Image")));
            this.tsbtnChartTracks.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnChartTracks.Name = "tsbtnChartTracks";
            this.tsbtnChartTracks.Size = new System.Drawing.Size(39, 28);
            this.tsbtnChartTracks.Text = "toolStripButton1";
            this.tsbtnChartTracks.ToolTipText = "График треков";
            this.tsbtnChartTracks.Click += new System.EventHandler(this.TsbtnChartTracks_Click);
            // 
            // toolStripSeparator20
            // 
            this.toolStripSeparator20.Name = "toolStripSeparator20";
            this.toolStripSeparator20.Size = new System.Drawing.Size(39, 6);
            // 
            // tsbtnResults
            // 
            this.tsbtnResults.AutoSize = false;
            this.tsbtnResults.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnResults.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnResults.Image")));
            this.tsbtnResults.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnResults.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnResults.Name = "tsbtnResults";
            this.tsbtnResults.Size = new System.Drawing.Size(40, 32);
            this.tsbtnResults.Text = "И";
            this.tsbtnResults.ToolTipText = "Итоги";
            this.tsbtnResults.Click += new System.EventHandler(this.TsbtnResults_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(39, 6);
            // 
            // tsbtnFilter
            // 
            this.tsbtnFilter.AutoSize = false;
            this.tsbtnFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnFilter.Image")));
            this.tsbtnFilter.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnFilter.Name = "tsbtnFilter";
            this.tsbtnFilter.Size = new System.Drawing.Size(40, 32);
            this.tsbtnFilter.Text = "Ф";
            this.tsbtnFilter.ToolTipText = "Фильтр";
            this.tsbtnFilter.Click += new System.EventHandler(this.TsbtnFilter_Click);
            // 
            // cmMarker
            // 
            this.cmMarker.ImageScalingSize = new System.Drawing.Size(24, 24);
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
            this.cmMap.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmMap.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMapMarkerAdd,
            this.toolStripSeparator12,
            this.miMapShowTileInfo,
            this.toolStripSeparator13,
            this.miMapOsmOpen,
            this.miMapOsmEdit,
            this.toolStripSeparator16,
            this.miMapCopyCoords,
            this.miMapTileAdd,
            this.miMapTileDelete});
            this.cmMap.Name = "cmMap";
            this.cmMap.Size = new System.Drawing.Size(210, 176);
            this.cmMap.Opening += new System.ComponentModel.CancelEventHandler(this.CmMap_Opening);
            // 
            // miMapMarkerAdd
            // 
            this.miMapMarkerAdd.Name = "miMapMarkerAdd";
            this.miMapMarkerAdd.Size = new System.Drawing.Size(209, 22);
            this.miMapMarkerAdd.Text = "Добавить маркер...";
            this.miMapMarkerAdd.Click += new System.EventHandler(this.MiMapMarkerAdd_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(206, 6);
            // 
            // miMapShowTileInfo
            // 
            this.miMapShowTileInfo.Name = "miMapShowTileInfo";
            this.miMapShowTileInfo.Size = new System.Drawing.Size(209, 22);
            this.miMapShowTileInfo.Text = "Информация о плитке";
            this.miMapShowTileInfo.Click += new System.EventHandler(this.MiMapShowTileInfo_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(206, 6);
            // 
            // miMapOsmOpen
            // 
            this.miMapOsmOpen.Name = "miMapOsmOpen";
            this.miMapOsmOpen.Size = new System.Drawing.Size(209, 22);
            this.miMapOsmOpen.Text = "Открыть в ОСМ";
            this.miMapOsmOpen.Click += new System.EventHandler(this.MiMapOsmOpen_Click);
            // 
            // miMapOsmEdit
            // 
            this.miMapOsmEdit.Name = "miMapOsmEdit";
            this.miMapOsmEdit.Size = new System.Drawing.Size(209, 22);
            this.miMapOsmEdit.Text = "Редактировать в ОСМ";
            this.miMapOsmEdit.Click += new System.EventHandler(this.MiMapOsmEdit_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(206, 6);
            // 
            // miMapCopyCoords
            // 
            this.miMapCopyCoords.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMapCopyCoordsFloat,
            this.miMapCopyCoordsFloat2});
            this.miMapCopyCoords.Name = "miMapCopyCoords";
            this.miMapCopyCoords.Size = new System.Drawing.Size(209, 22);
            this.miMapCopyCoords.Text = "Копировать координаты";
            // 
            // miMapCopyCoordsFloat
            // 
            this.miMapCopyCoordsFloat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMapCopyCoordsFloatLat,
            this.miMapCopyCoordsFloatLng});
            this.miMapCopyCoordsFloat.Name = "miMapCopyCoordsFloat";
            this.miMapCopyCoordsFloat.Size = new System.Drawing.Size(182, 22);
            this.miMapCopyCoordsFloat.Text = "51,196369, 58,298527";
            this.miMapCopyCoordsFloat.Click += new System.EventHandler(this.MiMapCopyCoords_Click);
            // 
            // miMapCopyCoordsFloatLat
            // 
            this.miMapCopyCoordsFloatLat.Name = "miMapCopyCoordsFloatLat";
            this.miMapCopyCoordsFloatLat.Size = new System.Drawing.Size(125, 22);
            this.miMapCopyCoordsFloatLat.Text = "51,196369";
            this.miMapCopyCoordsFloatLat.Click += new System.EventHandler(this.MiMapCopyCoords_Click);
            // 
            // miMapCopyCoordsFloatLng
            // 
            this.miMapCopyCoordsFloatLng.Name = "miMapCopyCoordsFloatLng";
            this.miMapCopyCoordsFloatLng.Size = new System.Drawing.Size(125, 22);
            this.miMapCopyCoordsFloatLng.Text = "58,298527";
            this.miMapCopyCoordsFloatLng.Click += new System.EventHandler(this.MiMapCopyCoords_Click);
            // 
            // miMapCopyCoordsFloat2
            // 
            this.miMapCopyCoordsFloat2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMapCopyCoordsFloat2Lat,
            this.miMapCopyCoordsFloat2Lng});
            this.miMapCopyCoordsFloat2.Name = "miMapCopyCoordsFloat2";
            this.miMapCopyCoordsFloat2.Size = new System.Drawing.Size(182, 22);
            this.miMapCopyCoordsFloat2.Text = "51.196369 58.298527";
            this.miMapCopyCoordsFloat2.Click += new System.EventHandler(this.MiMapCopyCoords_Click);
            // 
            // miMapCopyCoordsFloat2Lat
            // 
            this.miMapCopyCoordsFloat2Lat.Name = "miMapCopyCoordsFloat2Lat";
            this.miMapCopyCoordsFloat2Lat.Size = new System.Drawing.Size(125, 22);
            this.miMapCopyCoordsFloat2Lat.Text = "51.196369";
            this.miMapCopyCoordsFloat2Lat.Click += new System.EventHandler(this.MiMapCopyCoords_Click);
            // 
            // miMapCopyCoordsFloat2Lng
            // 
            this.miMapCopyCoordsFloat2Lng.Name = "miMapCopyCoordsFloat2Lng";
            this.miMapCopyCoordsFloat2Lng.Size = new System.Drawing.Size(125, 22);
            this.miMapCopyCoordsFloat2Lng.Text = "58.298527";
            this.miMapCopyCoordsFloat2Lng.Click += new System.EventHandler(this.MiMapCopyCoords_Click);
            // 
            // miMapTileAdd
            // 
            this.miMapTileAdd.Name = "miMapTileAdd";
            this.miMapTileAdd.Size = new System.Drawing.Size(209, 22);
            this.miMapTileAdd.Text = "add tile";
            this.miMapTileAdd.Visible = false;
            // 
            // miMapTileDelete
            // 
            this.miMapTileDelete.Name = "miMapTileDelete";
            this.miMapTileDelete.Size = new System.Drawing.Size(209, 22);
            this.miMapTileDelete.Text = "delete tile";
            this.miMapTileDelete.Visible = false;
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "gpx";
            this.openFileDialog.Filter = "GPX|*.gpx|Все файлы|*.*";
            this.openFileDialog.Multiselect = true;
            // 
            // cmTrack
            // 
            this.cmTrack.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmTrack.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miTrackShowChartTrackEle,
            this.toolStripSeparator19,
            this.miTrackChange,
            this.miTrackDelete});
            this.cmTrack.Name = "cmTrack";
            this.cmTrack.Size = new System.Drawing.Size(161, 76);
            // 
            // miTrackShowChartTrackEle
            // 
            this.miTrackShowChartTrackEle.Name = "miTrackShowChartTrackEle";
            this.miTrackShowChartTrackEle.Size = new System.Drawing.Size(160, 22);
            this.miTrackShowChartTrackEle.Text = "График высоты";
            this.miTrackShowChartTrackEle.Click += new System.EventHandler(this.MiTrackShowChartTrackEle_Click);
            // 
            // toolStripSeparator19
            // 
            this.toolStripSeparator19.Name = "toolStripSeparator19";
            this.toolStripSeparator19.Size = new System.Drawing.Size(157, 6);
            // 
            // miTrackChange
            // 
            this.miTrackChange.Name = "miTrackChange";
            this.miTrackChange.Size = new System.Drawing.Size(160, 22);
            this.miTrackChange.Text = "Изменить...";
            this.miTrackChange.Click += new System.EventHandler(this.MiTrackChange_Click);
            // 
            // miTrackDelete
            // 
            this.miTrackDelete.Name = "miTrackDelete";
            this.miTrackDelete.Size = new System.Drawing.Size(160, 22);
            this.miTrackDelete.Text = "Удалить";
            this.miTrackDelete.Click += new System.EventHandler(this.MiTrackDelete_Click);
            // 
            // timerMapChange
            // 
            this.timerMapChange.Interval = 222;
            this.timerMapChange.Tick += new System.EventHandler(this.TimerMapChange_Tick);
            // 
            // cmSelectedItems
            // 
            this.cmSelectedItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xxxToolStripMenuItem});
            this.cmSelectedItems.Name = "cmSelectedItems";
            this.cmSelectedItems.Size = new System.Drawing.Size(93, 26);
            // 
            // xxxToolStripMenuItem
            // 
            this.xxxToolStripMenuItem.Name = "xxxToolStripMenuItem";
            this.xxxToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.xxxToolStripMenuItem.Text = "xxx";
            // 
            // Main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(848, 461);
            this.Controls.Add(this.toolStripContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Main";
            this.Text = "TileExplorer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.LeftToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.LeftToolStripPanel.PerformLayout();
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.cmMarker.ResumeLayout(false);
            this.cmMap.ResumeLayout(false);
            this.cmTrack.ResumeLayout(false);
            this.cmSelectedItems.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem miMainSaveTileBoundaryToFile;
        private System.Windows.Forms.Timer timerMapChange;
        private System.Windows.Forms.ToolStripMenuItem miMainShowGrid;
        private System.Windows.Forms.ToolStripMenuItem miMainSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem miMainDataResults;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsbtnTrackList;
        private System.Windows.Forms.ToolStripButton tsbtnMarkerList;
        private System.Windows.Forms.ToolStripMenuItem miMainView;
        private System.Windows.Forms.ToolStripMenuItem miMainFullScreen;
        private System.Windows.Forms.ToolStripMenuItem miMainLeftPanel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem miMainSaveTileStatusToFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripButton tsbtnFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem miMainOsm;
        private System.Windows.Forms.ToolStripMenuItem miMainOsmOpen;
        private System.Windows.Forms.ToolStripMenuItem miMainOsmEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem miMapOsmOpen;
        private System.Windows.Forms.ToolStripMenuItem miMapOsmEdit;
        private System.Windows.Forms.ToolStripMenuItem miMapShowTileInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem miMainDataEquipmentList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripMenuItem miMainDataResultYears;
        private System.Windows.Forms.ToolStripMenuItem miMainDataResultEquipments;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripButton tsbtnResults;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripMenuItem miMapCopyCoords;
        private System.Windows.Forms.ToolStripMenuItem miMapCopyCoordsFloat;
        private System.Windows.Forms.ToolStripMenuItem miMapCopyCoordsFloatLat;
        private System.Windows.Forms.ToolStripMenuItem miMapCopyCoordsFloatLng;
        private System.Windows.Forms.ToolStripMenuItem miMapCopyCoordsFloat2;
        private System.Windows.Forms.ToolStripMenuItem miMapCopyCoordsFloat2Lat;
        private System.Windows.Forms.ToolStripMenuItem miMapCopyCoordsFloat2Lng;
        private System.Windows.Forms.ToolStripMenuItem miMainCheckUpdates;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.ToolStripStatusLabel slUpdateStatus;
        private System.Windows.Forms.ToolStripMenuItem miMainShowTiles;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem miMainBackup;
        private System.Windows.Forms.ToolStripMenuItem miMainBackupSave;
        private System.Windows.Forms.ToolStripMenuItem miTrackShowChartTrackEle;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator19;
        private System.Windows.Forms.ContextMenuStrip cmSelectedItems;
        private System.Windows.Forms.ToolStripMenuItem xxxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miMainDataCheckDirectoryTracks;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator20;
        private System.Windows.Forms.ToolStripButton tsbtnChartTracks;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator21;
        private System.Windows.Forms.ToolStripMenuItem miMainDataChartTracks;
        private System.Windows.Forms.ToolStripMenuItem miMainTilesHeatmap;
        private System.Windows.Forms.ToolStripMenuItem miMainBackupLoad;
        private System.Windows.Forms.ToolStripStatusLabel slTilesArea;
        private System.Windows.Forms.ToolStripMenuItem miMainDataTagList;
    }
}

