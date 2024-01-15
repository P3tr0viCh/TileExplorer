namespace TileExplorer
{
    partial class FrmTileInfo
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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.slCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.ColumnDateTimeStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trackBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slCount});
            this.statusStrip.Location = new System.Drawing.Point(0, 131);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(384, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip";
            // 
            // slCount
            // 
            this.slCount.Name = "slCount";
            this.slCount.Size = new System.Drawing.Size(62, 17);
            this.slCount.Text = "count: xxx";
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AutoGenerateColumns = false;
            this.dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnDateTimeStart,
            this.ColumnText});
            this.dataGridView.DataSource = this.trackBindingSource;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(384, 131);
            this.dataGridView.TabIndex = 2;
            // 
            // ColumnDateTimeStart
            // 
            this.ColumnDateTimeStart.DataPropertyName = "DateTimeStart";
            this.ColumnDateTimeStart.HeaderText = "Дата";
            this.ColumnDateTimeStart.Name = "ColumnDateTimeStart";
            this.ColumnDateTimeStart.ReadOnly = true;
            this.ColumnDateTimeStart.Width = 120;
            // 
            // ColumnText
            // 
            this.ColumnText.DataPropertyName = "Text";
            this.ColumnText.HeaderText = "Трек";
            this.ColumnText.Name = "ColumnText";
            this.ColumnText.ReadOnly = true;
            this.ColumnText.Width = 180;
            // 
            // trackBindingSource
            // 
            this.trackBindingSource.DataSource = typeof(TileExplorer.Database.Models.Track);
            // 
            // FrmTileInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 153);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTileInfo";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FrmTileInfo";
            this.Load += new System.EventHandler(this.FrmTileInfo_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmTileInfo_KeyDown);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel slCount;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.BindingSource trackBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDateTimeStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnText;
    }
}