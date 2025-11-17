namespace TileExplorer
{
    partial class FrmBackup
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
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.tbDirectory = new System.Windows.Forms.TextBox();
            this.btnDirectory = new System.Windows.Forms.Button();
            this.lblDirectory = new System.Windows.Forms.Label();
            this.gbMarkers = new System.Windows.Forms.GroupBox();
            this.cboxMarkersExcelXml = new System.Windows.Forms.CheckBox();
            this.cboxMarkersGpx = new System.Windows.Forms.CheckBox();
            this.gbEquipments = new System.Windows.Forms.GroupBox();
            this.cboxEquipmentsExcelXml = new System.Windows.Forms.CheckBox();
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.cboxLocalSettings = new System.Windows.Forms.CheckBox();
            this.cboxRoamingSettings = new System.Windows.Forms.CheckBox();
            this.lblName = new System.Windows.Forms.Label();
            this.btnName = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.cboxNameIsDate = new System.Windows.Forms.CheckBox();
            this.gdTracks = new System.Windows.Forms.GroupBox();
            this.cboxTrackExts = new System.Windows.Forms.CheckBox();
            this.gbMarkers.SuspendLayout();
            this.gbEquipments.SuspendLayout();
            this.gbSettings.SuspendLayout();
            this.gdTracks.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(304, 304);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 32);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(216, 304);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 32);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // tbDirectory
            // 
            this.tbDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDirectory.Location = new System.Drawing.Point(8, 32);
            this.tbDirectory.Name = "tbDirectory";
            this.tbDirectory.Size = new System.Drawing.Size(328, 25);
            this.tbDirectory.TabIndex = 1;
            // 
            // btnDirectory
            // 
            this.btnDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDirectory.Location = new System.Drawing.Point(344, 32);
            this.btnDirectory.Name = "btnDirectory";
            this.btnDirectory.Size = new System.Drawing.Size(40, 26);
            this.btnDirectory.TabIndex = 2;
            this.btnDirectory.Text = "...";
            this.btnDirectory.UseVisualStyleBackColor = true;
            this.btnDirectory.Click += new System.EventHandler(this.BtnDirectory_Click);
            // 
            // lblDirectory
            // 
            this.lblDirectory.AutoSize = true;
            this.lblDirectory.Location = new System.Drawing.Point(8, 8);
            this.lblDirectory.Name = "lblDirectory";
            this.lblDirectory.Size = new System.Drawing.Size(132, 19);
            this.lblDirectory.TabIndex = 0;
            this.lblDirectory.Text = "Каталог с архивами";
            // 
            // gbMarkers
            // 
            this.gbMarkers.Controls.Add(this.cboxMarkersExcelXml);
            this.gbMarkers.Controls.Add(this.cboxMarkersGpx);
            this.gbMarkers.Location = new System.Drawing.Point(8, 120);
            this.gbMarkers.Name = "gbMarkers";
            this.gbMarkers.Size = new System.Drawing.Size(184, 88);
            this.gbMarkers.TabIndex = 7;
            this.gbMarkers.TabStop = false;
            this.gbMarkers.Text = "Маркеры";
            // 
            // cboxMarkersExcelXml
            // 
            this.cboxMarkersExcelXml.AutoSize = true;
            this.cboxMarkersExcelXml.Location = new System.Drawing.Point(8, 24);
            this.cboxMarkersExcelXml.Name = "cboxMarkersExcelXml";
            this.cboxMarkersExcelXml.Size = new System.Drawing.Size(89, 23);
            this.cboxMarkersExcelXml.TabIndex = 0;
            this.cboxMarkersExcelXml.Text = "Excel XML";
            this.cboxMarkersExcelXml.UseVisualStyleBackColor = true;
            // 
            // cboxMarkersGpx
            // 
            this.cboxMarkersGpx.AutoSize = true;
            this.cboxMarkersGpx.Location = new System.Drawing.Point(8, 56);
            this.cboxMarkersGpx.Name = "cboxMarkersGpx";
            this.cboxMarkersGpx.Size = new System.Drawing.Size(54, 23);
            this.cboxMarkersGpx.TabIndex = 1;
            this.cboxMarkersGpx.Text = "GPX";
            this.cboxMarkersGpx.UseVisualStyleBackColor = true;
            // 
            // gbEquipments
            // 
            this.gbEquipments.Controls.Add(this.cboxEquipmentsExcelXml);
            this.gbEquipments.Location = new System.Drawing.Point(200, 120);
            this.gbEquipments.Name = "gbEquipments";
            this.gbEquipments.Size = new System.Drawing.Size(184, 88);
            this.gbEquipments.TabIndex = 8;
            this.gbEquipments.TabStop = false;
            this.gbEquipments.Text = "Снаряжение";
            // 
            // cboxEquipmentsExcelXml
            // 
            this.cboxEquipmentsExcelXml.AutoSize = true;
            this.cboxEquipmentsExcelXml.Location = new System.Drawing.Point(8, 24);
            this.cboxEquipmentsExcelXml.Name = "cboxEquipmentsExcelXml";
            this.cboxEquipmentsExcelXml.Size = new System.Drawing.Size(89, 23);
            this.cboxEquipmentsExcelXml.TabIndex = 0;
            this.cboxEquipmentsExcelXml.Text = "Excel XML";
            this.cboxEquipmentsExcelXml.UseVisualStyleBackColor = true;
            // 
            // gbSettings
            // 
            this.gbSettings.Controls.Add(this.cboxLocalSettings);
            this.gbSettings.Controls.Add(this.cboxRoamingSettings);
            this.gbSettings.Location = new System.Drawing.Point(8, 208);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(184, 88);
            this.gbSettings.TabIndex = 9;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "Настройки";
            // 
            // cboxLocalSettings
            // 
            this.cboxLocalSettings.AutoSize = true;
            this.cboxLocalSettings.Location = new System.Drawing.Point(8, 24);
            this.cboxLocalSettings.Name = "cboxLocalSettings";
            this.cboxLocalSettings.Size = new System.Drawing.Size(98, 23);
            this.cboxLocalSettings.TabIndex = 0;
            this.cboxLocalSettings.Text = "Локальные";
            this.cboxLocalSettings.UseVisualStyleBackColor = true;
            // 
            // cboxRoamingSettings
            // 
            this.cboxRoamingSettings.AutoSize = true;
            this.cboxRoamingSettings.Location = new System.Drawing.Point(8, 56);
            this.cboxRoamingSettings.Name = "cboxRoamingSettings";
            this.cboxRoamingSettings.Size = new System.Drawing.Size(74, 23);
            this.cboxRoamingSettings.TabIndex = 1;
            this.cboxRoamingSettings.Text = "Общие";
            this.cboxRoamingSettings.UseVisualStyleBackColor = true;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(8, 64);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(83, 19);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Имя архива";
            // 
            // btnName
            // 
            this.btnName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnName.Location = new System.Drawing.Point(344, 88);
            this.btnName.Name = "btnName";
            this.btnName.Size = new System.Drawing.Size(40, 26);
            this.btnName.TabIndex = 6;
            this.btnName.Text = "...";
            this.btnName.UseVisualStyleBackColor = true;
            this.btnName.Click += new System.EventHandler(this.BtnName_Click);
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.Location = new System.Drawing.Point(8, 88);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(328, 25);
            this.tbName.TabIndex = 5;
            // 
            // cboxNameIsDate
            // 
            this.cboxNameIsDate.AutoSize = true;
            this.cboxNameIsDate.Location = new System.Drawing.Point(96, 64);
            this.cboxNameIsDate.Name = "cboxNameIsDate";
            this.cboxNameIsDate.Size = new System.Drawing.Size(114, 23);
            this.cboxNameIsDate.TabIndex = 4;
            this.cboxNameIsDate.Text = "Текущая дата";
            this.cboxNameIsDate.UseVisualStyleBackColor = true;
            this.cboxNameIsDate.CheckedChanged += new System.EventHandler(this.CboxNameIsDate_CheckedChanged);
            // 
            // gdTracks
            // 
            this.gdTracks.Controls.Add(this.cboxTrackExts);
            this.gdTracks.Location = new System.Drawing.Point(200, 208);
            this.gdTracks.Name = "gdTracks";
            this.gdTracks.Size = new System.Drawing.Size(184, 88);
            this.gdTracks.TabIndex = 10;
            this.gdTracks.TabStop = false;
            this.gdTracks.Text = "Треки";
            // 
            // cboxTracksExt
            // 
            this.cboxTrackExts.AutoSize = true;
            this.cboxTrackExts.Location = new System.Drawing.Point(8, 24);
            this.cboxTrackExts.Name = "cboxTracksExt";
            this.cboxTrackExts.Size = new System.Drawing.Size(107, 23);
            this.cboxTrackExts.TabIndex = 0;
            this.cboxTrackExts.Text = "Снаряжение";
            this.cboxTrackExts.UseVisualStyleBackColor = true;
            // 
            // FrmBackup
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(392, 345);
            this.Controls.Add(this.gdTracks);
            this.Controls.Add(this.cboxNameIsDate);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnName);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.gbSettings);
            this.Controls.Add(this.gbEquipments);
            this.Controls.Add(this.gbMarkers);
            this.Controls.Add(this.lblDirectory);
            this.Controls.Add(this.btnDirectory);
            this.Controls.Add(this.tbDirectory);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(408, 384);
            this.Name = "FrmBackup";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Архив";
            this.gbMarkers.ResumeLayout(false);
            this.gbMarkers.PerformLayout();
            this.gbEquipments.ResumeLayout(false);
            this.gbEquipments.PerformLayout();
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            this.gdTracks.ResumeLayout(false);
            this.gdTracks.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox tbDirectory;
        private System.Windows.Forms.Button btnDirectory;
        private System.Windows.Forms.Label lblDirectory;
        private System.Windows.Forms.GroupBox gbMarkers;
        private System.Windows.Forms.CheckBox cboxMarkersExcelXml;
        private System.Windows.Forms.CheckBox cboxMarkersGpx;
        private System.Windows.Forms.GroupBox gbEquipments;
        private System.Windows.Forms.CheckBox cboxEquipmentsExcelXml;
        private System.Windows.Forms.GroupBox gbSettings;
        private System.Windows.Forms.CheckBox cboxLocalSettings;
        private System.Windows.Forms.CheckBox cboxRoamingSettings;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnName;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.CheckBox cboxNameIsDate;
        private System.Windows.Forms.GroupBox gdTracks;
        private System.Windows.Forms.CheckBox cboxTrackExts;
    }
}