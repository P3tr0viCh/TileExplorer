namespace TileExplorer
{
    partial class FrmChartTracksByMonth
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint8 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(29646D, 20000D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint9 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(29647D, 0D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint10 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(29648D, 50000D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint11 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(29649D, 75000D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint12 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(29650D, 0D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint13 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(29651D, 25000D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint14 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(29652D, 30000D);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmChartTracksByMonth));
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.cboxYear = new System.Windows.Forms.ToolStripComboBox();
            this.cboxMonth = new System.Windows.Forms.ToolStripComboBox();
            this.tbtnPrevMonth = new System.Windows.Forms.ToolStripButton();
            this.tbtnNextMonth = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart
            // 
            chartArea2.AxisX.InterlacedColor = System.Drawing.Color.White;
            chartArea2.AxisX.Interval = 1D;
            chartArea2.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea2.AxisX.IntervalOffset = 1D;
            chartArea2.AxisX.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Days;
            chartArea2.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Days;
            chartArea2.AxisX.IsStartedFromZero = false;
            chartArea2.AxisX.LabelStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            chartArea2.AxisX.LabelStyle.ForeColor = System.Drawing.Color.OrangeRed;
            chartArea2.AxisX.LabelStyle.Format = "d\\\\ndddd";
            chartArea2.AxisX.LineColor = System.Drawing.Color.Cyan;
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisX.MajorTickMark.LineColor = System.Drawing.Color.SandyBrown;
            chartArea2.AxisX.TitleFont = new System.Drawing.Font("Segoe UI", 10F);
            chartArea2.AxisX2.Interval = 50000D;
            chartArea2.AxisY.Interval = 50000D;
            chartArea2.AxisY.IsLabelAutoFit = false;
            chartArea2.AxisY.LabelStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            chartArea2.AxisY.MajorGrid.Enabled = false;
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.IndianRed;
            chartArea2.AxisY.TitleFont = new System.Drawing.Font("Segoe UI", 10F);
            chartArea2.CursorX.Interval = 0D;
            chartArea2.CursorX.LineColor = System.Drawing.Color.RosyBrown;
            chartArea2.CursorY.Interval = 0D;
            chartArea2.Name = "ChartArea";
            this.chart.ChartAreas.Add(chartArea2);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart.Location = new System.Drawing.Point(0, 31);
            this.chart.Name = "chart";
            this.chart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SemiTransparent;
            series2.ChartArea = "ChartArea";
            series2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            series2.Font = new System.Drawing.Font("Segoe UI", 10F);
            series2.IsValueShownAsLabel = true;
            series2.IsVisibleInLegend = false;
            series2.LabelForeColor = System.Drawing.Color.IndianRed;
            series2.Name = "TracksByMonth";
            dataPoint8.IsValueShownAsLabel = false;
            dataPoint8.LabelForeColor = System.Drawing.Color.Black;
            dataPoint9.IsEmpty = true;
            dataPoint12.IsEmpty = true;
            series2.Points.Add(dataPoint8);
            series2.Points.Add(dataPoint9);
            series2.Points.Add(dataPoint10);
            series2.Points.Add(dataPoint11);
            series2.Points.Add(dataPoint12);
            series2.Points.Add(dataPoint13);
            series2.Points.Add(dataPoint14);
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
            this.chart.Series.Add(series2);
            this.chart.Size = new System.Drawing.Size(584, 210);
            this.chart.TabIndex = 1;
            this.chart.Text = "chart";
            // 
            // toolStrip
            // 
            this.toolStrip.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cboxYear,
            this.cboxMonth,
            this.tbtnPrevMonth,
            this.tbtnNextMonth});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(584, 31);
            this.toolStrip.TabIndex = 2;
            // 
            // cboxYear
            // 
            this.cboxYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxYear.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboxYear.Name = "cboxYear";
            this.cboxYear.Size = new System.Drawing.Size(120, 31);
            this.cboxYear.SelectedIndexChanged += new System.EventHandler(this.CboxYear_SelectedIndexChanged);
            // 
            // cboxMonth
            // 
            this.cboxMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxMonth.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboxMonth.Name = "cboxMonth";
            this.cboxMonth.Size = new System.Drawing.Size(120, 31);
            this.cboxMonth.SelectedIndexChanged += new System.EventHandler(this.CboxMonth_SelectedIndexChanged);
            // 
            // tbtnPrevMonth
            // 
            this.tbtnPrevMonth.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbtnPrevMonth.Image = ((System.Drawing.Image)(resources.GetObject("tbtnPrevMonth.Image")));
            this.tbtnPrevMonth.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnPrevMonth.Name = "tbtnPrevMonth";
            this.tbtnPrevMonth.Size = new System.Drawing.Size(28, 28);
            this.tbtnPrevMonth.Text = "Предыдущий месяц";
            this.tbtnPrevMonth.Click += new System.EventHandler(this.TbtnPrevMonth_Click);
            // 
            // tbtnNextMonth
            // 
            this.tbtnNextMonth.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbtnNextMonth.Image = ((System.Drawing.Image)(resources.GetObject("tbtnNextMonth.Image")));
            this.tbtnNextMonth.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnNextMonth.Name = "tbtnNextMonth";
            this.tbtnNextMonth.Size = new System.Drawing.Size(28, 28);
            this.tbtnNextMonth.Text = "Следующий месяц";
            this.tbtnNextMonth.Click += new System.EventHandler(this.TbtnNextMonth_Click);
            // 
            // FrmChartTracksByMonth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 241);
            this.Controls.Add(this.chart);
            this.Controls.Add(this.toolStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "FrmChartTracksByMonth";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FrmChartTracksByMonth";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmChartTracksByMonth_FormClosed);
            this.Load += new System.EventHandler(this.FrmChartTracksByMonth_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripComboBox cboxYear;
        private System.Windows.Forms.ToolStripComboBox cboxMonth;
        private System.Windows.Forms.ToolStripButton tbtnPrevMonth;
        private System.Windows.Forms.ToolStripButton tbtnNextMonth;
    }
}