namespace TileExplorer
{
    partial class FrmChartTracksByYear
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
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.cboxYear = new System.Windows.Forms.ToolStripComboBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miOpenChartTracksByMonth = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cboxYear});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(784, 27);
            this.toolStrip.TabIndex = 3;
            // 
            // cboxYear
            // 
            this.cboxYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxYear.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboxYear.Name = "cboxYear";
            this.cboxYear.Size = new System.Drawing.Size(120, 27);
            this.cboxYear.SelectedIndexChanged += new System.EventHandler(this.CboxYear_SelectedIndexChanged);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miOpenChartTracksByMonth});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(167, 26);
            // 
            // miOpenChartTracksByMonth
            // 
            this.miOpenChartTracksByMonth.Name = "miOpenChartTracksByMonth";
            this.miOpenChartTracksByMonth.Size = new System.Drawing.Size(166, 22);
            this.miOpenChartTracksByMonth.Text = "График за месяц";
            this.miOpenChartTracksByMonth.Click += new System.EventHandler(this.MiOpenChartTracksByMonth_Click);
            // 
            // FrmChartTracksByYear
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.toolStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "FrmChartTracksByYear";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FrmChartTracksByYear";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmChartTracksByYear_FormClosed);
            this.Load += new System.EventHandler(this.FrmChartTracksByYear_Load);
            this.ClientSizeChanged += new System.EventHandler(this.FrmChartTracksByYear_ClientSizeChanged);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripComboBox cboxYear;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem miOpenChartTracksByMonth;
    }
}