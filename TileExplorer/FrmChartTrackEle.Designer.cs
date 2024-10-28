namespace TileExplorer
{
    partial class FrmChartTrackEle
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 20D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(5000D, 25D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint3 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(10000D, 50D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint4 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(15000D, 75D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint5 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(25000D, 50D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint6 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(31000D, 25D);
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.slDistance = new System.Windows.Forms.ToolStripStatusLabel();
            this.slEle = new System.Windows.Forms.ToolStripStatusLabel();
            this.slDateTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.slDateTimeSpan = new System.Windows.Forms.ToolStripStatusLabel();
            this.slIsSelection = new System.Windows.Forms.ToolStripStatusLabel();
            this.slSelectedDistance = new System.Windows.Forms.ToolStripStatusLabel();
            this.slSelectedEle = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart
            // 
            chartArea1.AxisX.Interval = 10000D;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            chartArea1.AxisX.Maximum = 31000D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Segoe UI", 10F);
            chartArea1.AxisY.Interval = 50D;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.IndianRed;
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Segoe UI", 10F);
            chartArea1.CursorX.Interval = 0D;
            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.CursorX.LineColor = System.Drawing.Color.RosyBrown;
            chartArea1.CursorY.Interval = 0D;
            chartArea1.Name = "AreaTrackEle";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart.Location = new System.Drawing.Point(0, 0);
            this.chart.Name = "chart";
            this.chart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SemiTransparent;
            series1.ChartArea = "AreaTrackEle";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            series1.IsVisibleInLegend = false;
            series1.Name = "TrackEle";
            series1.Points.Add(dataPoint1);
            series1.Points.Add(dataPoint2);
            series1.Points.Add(dataPoint3);
            series1.Points.Add(dataPoint4);
            series1.Points.Add(dataPoint5);
            series1.Points.Add(dataPoint6);
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(584, 219);
            this.chart.TabIndex = 0;
            this.chart.Text = "chart";
            this.chart.SelectionRangeChanging += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.CursorEventArgs>(this.Chart_SelectionRangeChanging);
            this.chart.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Chart_MouseClick);
            this.chart.MouseEnter += new System.EventHandler(this.Chart_MouseEnter);
            this.chart.MouseLeave += new System.EventHandler(this.Chart_MouseLeave);
            this.chart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Chart_MouseMove);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slDistance,
            this.slEle,
            this.slDateTime,
            this.slDateTimeSpan,
            this.slIsSelection,
            this.slSelectedDistance,
            this.slSelectedEle});
            this.statusStrip.Location = new System.Drawing.Point(0, 219);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(584, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // slDistance
            // 
            this.slDistance.Name = "slDistance";
            this.slDistance.Size = new System.Drawing.Size(45, 17);
            this.slDistance.Text = "Dist: 42";
            // 
            // slEle
            // 
            this.slEle.Name = "slEle";
            this.slEle.Size = new System.Drawing.Size(49, 17);
            this.slEle.Text = " Ele: 666";
            // 
            // slDateTime
            // 
            this.slDateTime.Name = "slDateTime";
            this.slDateTime.Size = new System.Drawing.Size(50, 17);
            this.slDateTime.Text = "DT: 1981";
            // 
            // slDateTimeSpan
            // 
            this.slDateTimeSpan.Name = "slDateTimeSpan";
            this.slDateTimeSpan.Size = new System.Drawing.Size(51, 17);
            this.slDateTimeSpan.Text = "DT: span";
            // 
            // slIsSelection
            // 
            this.slIsSelection.Name = "slIsSelection";
            this.slIsSelection.Size = new System.Drawing.Size(55, 17);
            this.slIsSelection.Text = "Selection";
            // 
            // slSelectedDistance
            // 
            this.slSelectedDistance.Name = "slSelectedDistance";
            this.slSelectedDistance.Size = new System.Drawing.Size(89, 17);
            this.slSelectedDistance.Text = "SelectedDist: 42";
            // 
            // slSelectedEle
            // 
            this.slSelectedEle.Name = "slSelectedEle";
            this.slSelectedEle.Size = new System.Drawing.Size(93, 17);
            this.slSelectedEle.Text = "Selected Ele: 666";
            // 
            // FrmChartTrackEle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 241);
            this.Controls.Add(this.chart);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "FrmChartTrackEle";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FrmTrackEleChart";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmChartTrackEle_FormClosed);
            this.Load += new System.EventHandler(this.FrmTrackEleChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel slDistance;
        private System.Windows.Forms.ToolStripStatusLabel slEle;
        private System.Windows.Forms.ToolStripStatusLabel slDateTime;
        private System.Windows.Forms.ToolStripStatusLabel slDateTimeSpan;
        private System.Windows.Forms.ToolStripStatusLabel slIsSelection;
        private System.Windows.Forms.ToolStripStatusLabel slSelectedDistance;
        private System.Windows.Forms.ToolStripStatusLabel slSelectedEle;
    }
}