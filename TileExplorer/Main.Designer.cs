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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.gMapControl = new GMap.NET.WindowsForms.GMapControl();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.slZoom = new System.Windows.Forms.ToolStripStatusLabel();
            this.slPosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.slTileId = new System.Windows.Forms.ToolStripStatusLabel();
            this.slTilesVisited = new System.Windows.Forms.ToolStripStatusLabel();
            this.slTilesMaxCluster = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.miMainFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainSaveToImage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainClose = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainMap = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainMarkers = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miMainFullScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.miMainImages = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // gMapControl
            // 
            this.gMapControl.Bearing = 0F;
            this.gMapControl.CanDragMap = true;
            this.gMapControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gMapControl.EmptyTileColor = System.Drawing.Color.Navy;
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
            this.gMapControl.Size = new System.Drawing.Size(584, 415);
            this.gMapControl.TabIndex = 0;
            this.gMapControl.Zoom = 0D;
            this.gMapControl.OnPositionChanged += new GMap.NET.PositionChanged(this.GMapControl_OnPositionChanged);
            this.gMapControl.OnMapZoomChanged += new GMap.NET.MapZoomChanged(this.GMapControl_OnMapZoomChanged);
            this.gMapControl.Load += new System.EventHandler(this.GMapControl_Load);
            this.gMapControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GMapControl_MouseClick);
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slZoom,
            this.slPosition,
            this.slTileId,
            this.slTilesVisited,
            this.slTilesMaxCluster});
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(584, 22);
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
            // menuStrip
            // 
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainFile,
            this.miMainMap,
            this.miMainHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(584, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
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
            this.miMainMarkers,
            this.miMainImages,
            this.toolStripSeparator2,
            this.miMainFullScreen});
            this.miMainMap.Name = "miMainMap";
            this.miMainMap.Size = new System.Drawing.Size(50, 20);
            this.miMainMap.Text = "Карта";
            // 
            // miMainMarkers
            // 
            this.miMainMarkers.Checked = true;
            this.miMainMarkers.CheckOnClick = true;
            this.miMainMarkers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miMainMarkers.Name = "miMainMarkers";
            this.miMainMarkers.Size = new System.Drawing.Size(232, 22);
            this.miMainMarkers.Text = "Подписи";
            this.miMainMarkers.Click += new System.EventHandler(this.MiMainMarkers_Click);
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
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(584, 415);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(584, 461);
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
            // miMainImages
            // 
            this.miMainImages.Checked = true;
            this.miMainImages.CheckOnClick = true;
            this.miMainImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miMainImages.Name = "miMainImages";
            this.miMainImages.Size = new System.Drawing.Size(232, 22);
            this.miMainImages.Text = "Изображения";
            this.miMainImages.Click += new System.EventHandler(this.MiMainImages_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.toolStripContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Main";
            this.Text = "TileExplorer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
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
        private System.Windows.Forms.ToolStripMenuItem miMainMarkers;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem miMainImages;
    }
}

