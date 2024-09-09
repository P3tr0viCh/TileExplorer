namespace TileExplorer
{
    partial class FrmTrackEleChart
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // chart
            // 
            chartArea3.AxisX.Interval = 10D;
            chartArea3.AxisX.TitleFont = new System.Drawing.Font("Segoe UI", 10F);
            chartArea3.AxisY.Interval = 50D;
            chartArea3.AxisY.TitleFont = new System.Drawing.Font("Segoe UI", 10F);
            chartArea3.CursorX.Interval = 0D;
            chartArea3.CursorX.IsUserEnabled = true;
            chartArea3.CursorY.Interval = 0D;
            chartArea3.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea3);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.chart.Legends.Add(legend3);
            this.chart.Location = new System.Drawing.Point(0, 0);
            this.chart.Name = "chart";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.IsVisibleInLegend = false;
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chart.Series.Add(series3);
            this.chart.Size = new System.Drawing.Size(584, 219);
            this.chart.TabIndex = 0;
            this.chart.Text = "chart";
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 219);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(584, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // FrmTrackEleChart
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
            this.Name = "FrmTrackEleChart";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FrmTrackEleChart";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmTrackEleChart_FormClosed);
            this.Load += new System.EventHandler(this.FrmTrackEleChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.StatusStrip statusStrip;
    }
}