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
            this.slTilesVisited = new System.Windows.Forms.ToolStripStatusLabel();
            this.slTilesMaxCluster = new System.Windows.Forms.ToolStripStatusLabel();
            this.slTilesMaxSquare = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.miMainFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainSaveToImage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainClose = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainMap = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainShowTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainShowMarkers = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainFullScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainMapDesign = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainData = new System.Windows.Forms.ToolStripMenuItem();
            this.openTrackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainDataTrackList = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainDataMarkerList = new System.Windows.Forms.ToolStripMenuItem();
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
            this.addTileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteTileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.cmTrack = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miTrackDelete = new System.Windows.Forms.ToolStripMenuItem();
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
            this.gMapControl.OnMarkerClick += new GMap.NET.WindowsForms.MarkerClick(this.GMapControl_OnMarkerClick);
            this.gMapControl.OnRouteClick += new GMap.NET.WindowsForms.RouteClick(this.GMapControl_OnRouteClick);
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
            this.slStatus.Size = new System.Drawing.Size(204, 17);
            this.slStatus.Spring = true;
            this.slStatus.Text = "status";
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
            this.toolStripSeparator1,
            this.miMainClose});
            this.miMainFile.Name = "miMainFile";
            this.miMainFile.Size = new System.Drawing.Size(48, 20);
            this.miMainFile.Text = "Файл";
            // 
            // miMainSaveToImage
            // 
            this.miMainSaveToImage.Name = "miMainSaveToImage";
            this.miMainSaveToImage.Size = new System.Drawing.Size(219, 22);
            this.miMainSaveToImage.Text = "Сохранить изображение...";
            this.miMainSaveToImage.Click += new System.EventHandler(this.MiMainSaveToImage_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(216, 6);
            // 
            // miMainClose
            // 
            this.miMainClose.Name = "miMainClose";
            this.miMainClose.Size = new System.Drawing.Size(219, 22);
            this.miMainClose.Text = "Закрыть";
            this.miMainClose.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // miMainMap
            // 
            this.miMainMap.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainShowTracks,
            this.miMainShowMarkers,
            this.toolStripSeparator2,
            this.miMainFullScreen,
            this.toolStripSeparator3,
            this.miMainMapDesign});
            this.miMainMap.Name = "miMainMap";
            this.miMainMap.Size = new System.Drawing.Size(50, 20);
            this.miMainMap.Text = "Карта";
            // 
            // miMainShowTracks
            // 
            this.miMainShowTracks.Checked = true;
            this.miMainShowTracks.CheckOnClick = true;
            this.miMainShowTracks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miMainShowTracks.Name = "miMainShowTracks";
            this.miMainShowTracks.Size = new System.Drawing.Size(232, 22);
            this.miMainShowTracks.Text = "Отображать треки";
            this.miMainShowTracks.Click += new System.EventHandler(this.MiMainShowTracks_Click);
            // 
            // miMainShowMarkers
            // 
            this.miMainShowMarkers.Checked = true;
            this.miMainShowMarkers.CheckOnClick = true;
            this.miMainShowMarkers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miMainShowMarkers.Name = "miMainShowMarkers";
            this.miMainShowMarkers.Size = new System.Drawing.Size(232, 22);
            this.miMainShowMarkers.Text = "Отображать маркеры";
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
            // miMainMapDesign
            // 
            this.miMainMapDesign.Name = "miMainMapDesign";
            this.miMainMapDesign.Size = new System.Drawing.Size(232, 22);
            this.miMainMapDesign.Text = "Оформление...";
            this.miMainMapDesign.Click += new System.EventHandler(this.MiMainMapDesign_Click);
            // 
            // miMainData
            // 
            this.miMainData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openTrackToolStripMenuItem,
            this.miMainDataTrackList,
            this.miMainDataMarkerList,
            this.toolStripSeparator4,
            this.miMainDataUpdate});
            this.miMainData.Name = "miMainData";
            this.miMainData.Size = new System.Drawing.Size(62, 20);
            this.miMainData.Text = "Данные";
            // 
            // openTrackToolStripMenuItem
            // 
            this.openTrackToolStripMenuItem.Name = "openTrackToolStripMenuItem";
            this.openTrackToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openTrackToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.openTrackToolStripMenuItem.Text = "open track...";
            this.openTrackToolStripMenuItem.Click += new System.EventHandler(this.OpenTrackToolStripMenuItem_Click);
            // 
            // miMainDataTrackList
            // 
            this.miMainDataTrackList.Name = "miMainDataTrackList";
            this.miMainDataTrackList.Size = new System.Drawing.Size(182, 22);
            this.miMainDataTrackList.Text = "Треки";
            this.miMainDataTrackList.Click += new System.EventHandler(this.MiMainDataTrackList_Click);
            // 
            // miMainDataMarkerList
            // 
            this.miMainDataMarkerList.Name = "miMainDataMarkerList";
            this.miMainDataMarkerList.Size = new System.Drawing.Size(182, 22);
            this.miMainDataMarkerList.Text = "Маркеры";
            this.miMainDataMarkerList.Click += new System.EventHandler(this.MiMainDataMarkerList_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(179, 6);
            // 
            // miMainDataUpdate
            // 
            this.miMainDataUpdate.Name = "miMainDataUpdate";
            this.miMainDataUpdate.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.miMainDataUpdate.Size = new System.Drawing.Size(182, 22);
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
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "png";
            this.saveFileDialog.Filter = "PNG|*.png|Все файлы|*.*";
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
            this.addTileToolStripMenuItem,
            this.deleteTileToolStripMenuItem});
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
            // addTileToolStripMenuItem
            // 
            this.addTileToolStripMenuItem.Name = "addTileToolStripMenuItem";
            this.addTileToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.addTileToolStripMenuItem.Text = "add tile";
            this.addTileToolStripMenuItem.Click += new System.EventHandler(this.AddTileToolStripMenuItem_Click);
            // 
            // deleteTileToolStripMenuItem
            // 
            this.deleteTileToolStripMenuItem.Name = "deleteTileToolStripMenuItem";
            this.deleteTileToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.deleteTileToolStripMenuItem.Text = "delete tile";
            this.deleteTileToolStripMenuItem.Click += new System.EventHandler(this.DeleteTileToolStripMenuItem_Click);
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
            this.miTrackDelete});
            this.cmTrack.Name = "cmTrack";
            this.cmTrack.Size = new System.Drawing.Size(119, 26);
            // 
            // miTrackDelete
            // 
            this.miTrackDelete.Name = "miTrackDelete";
            this.miTrackDelete.Size = new System.Drawing.Size(118, 22);
            this.miTrackDelete.Text = "Удалить";
            this.miTrackDelete.Click += new System.EventHandler(this.MiTrackDelete_Click);
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
            this.SizeChanged += new System.EventHandler(this.Main_SizeChanged);
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
        private System.Windows.Forms.ToolStripMenuItem addTileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteTileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem miMainMapDesign;
        private System.Windows.Forms.ToolStripMenuItem openTrackToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem miMainShowTracks;
        private System.Windows.Forms.ContextMenuStrip cmTrack;
        private System.Windows.Forms.ToolStripMenuItem miTrackDelete;
        private System.Windows.Forms.ToolStripMenuItem miMainDataMarkerList;
        private System.Windows.Forms.ToolStripMenuItem miMainDataTrackList;
    }
}

