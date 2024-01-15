namespace TileExplorer
{
    partial class FrmFilter
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
            this.panelFilter = new System.Windows.Forms.Panel();
            this.tbYears = new System.Windows.Forms.TextBox();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpDay = new System.Windows.Forms.DateTimePicker();
            this.rbtnFilterYears = new System.Windows.Forms.RadioButton();
            this.rbtnFilterPeriod = new System.Windows.Forms.RadioButton();
            this.rbtnFilterDay = new System.Windows.Forms.RadioButton();
            this.rbtnFilterNone = new System.Windows.Forms.RadioButton();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.panelFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFilter
            // 
            this.panelFilter.Controls.Add(this.tbYears);
            this.panelFilter.Controls.Add(this.dtpDateTo);
            this.panelFilter.Controls.Add(this.dtpDateFrom);
            this.panelFilter.Controls.Add(this.dtpDay);
            this.panelFilter.Controls.Add(this.rbtnFilterYears);
            this.panelFilter.Controls.Add(this.rbtnFilterPeriod);
            this.panelFilter.Controls.Add(this.rbtnFilterDay);
            this.panelFilter.Controls.Add(this.rbtnFilterNone);
            this.panelFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFilter.Location = new System.Drawing.Point(0, 0);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Size = new System.Drawing.Size(216, 241);
            this.panelFilter.TabIndex = 1;
            // 
            // tbYears
            // 
            this.tbYears.Location = new System.Drawing.Point(8, 208);
            this.tbYears.Name = "tbYears";
            this.tbYears.Size = new System.Drawing.Size(200, 25);
            this.tbYears.TabIndex = 8;
            this.tbYears.TextChanged += new System.EventHandler(this.YearsChanged);
            // 
            // dtpDateTo
            // 
            this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateTo.Location = new System.Drawing.Point(8, 152);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(200, 25);
            this.dtpDateTo.TabIndex = 7;
            this.dtpDateTo.Value = new System.DateTime(1981, 3, 29, 0, 0, 0, 0);
            this.dtpDateTo.ValueChanged += new System.EventHandler(this.FilterTypeChanged);
            // 
            // dtpDateFrom
            // 
            this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateFrom.Location = new System.Drawing.Point(8, 120);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(200, 25);
            this.dtpDateFrom.TabIndex = 6;
            this.dtpDateFrom.Value = new System.DateTime(1981, 3, 29, 0, 0, 0, 0);
            this.dtpDateFrom.ValueChanged += new System.EventHandler(this.FilterTypeChanged);
            // 
            // dtpDay
            // 
            this.dtpDay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDay.Location = new System.Drawing.Point(8, 64);
            this.dtpDay.Name = "dtpDay";
            this.dtpDay.Size = new System.Drawing.Size(200, 25);
            this.dtpDay.TabIndex = 5;
            this.dtpDay.Value = new System.DateTime(1981, 3, 29, 0, 0, 0, 0);
            this.dtpDay.ValueChanged += new System.EventHandler(this.DateTimeChanged);
            // 
            // rbtnFilterYears
            // 
            this.rbtnFilterYears.AutoSize = true;
            this.rbtnFilterYears.Location = new System.Drawing.Point(8, 184);
            this.rbtnFilterYears.Name = "rbtnFilterYears";
            this.rbtnFilterYears.Size = new System.Drawing.Size(60, 23);
            this.rbtnFilterYears.TabIndex = 4;
            this.rbtnFilterYears.TabStop = true;
            this.rbtnFilterYears.Text = "Годы";
            this.rbtnFilterYears.UseVisualStyleBackColor = true;
            this.rbtnFilterYears.CheckedChanged += new System.EventHandler(this.FilterTypeChanged);
            // 
            // rbtnFilterPeriod
            // 
            this.rbtnFilterPeriod.AutoSize = true;
            this.rbtnFilterPeriod.Location = new System.Drawing.Point(8, 96);
            this.rbtnFilterPeriod.Name = "rbtnFilterPeriod";
            this.rbtnFilterPeriod.Size = new System.Drawing.Size(76, 23);
            this.rbtnFilterPeriod.TabIndex = 3;
            this.rbtnFilterPeriod.TabStop = true;
            this.rbtnFilterPeriod.Text = "Период";
            this.rbtnFilterPeriod.UseVisualStyleBackColor = true;
            this.rbtnFilterPeriod.CheckedChanged += new System.EventHandler(this.FilterTypeChanged);
            // 
            // rbtnFilterDay
            // 
            this.rbtnFilterDay.AutoSize = true;
            this.rbtnFilterDay.Location = new System.Drawing.Point(8, 40);
            this.rbtnFilterDay.Name = "rbtnFilterDay";
            this.rbtnFilterDay.Size = new System.Drawing.Size(96, 23);
            this.rbtnFilterDay.TabIndex = 2;
            this.rbtnFilterDay.TabStop = true;
            this.rbtnFilterDay.Text = "Один день";
            this.rbtnFilterDay.UseVisualStyleBackColor = true;
            this.rbtnFilterDay.CheckedChanged += new System.EventHandler(this.FilterTypeChanged);
            // 
            // rbtnFilterNone
            // 
            this.rbtnFilterNone.AutoSize = true;
            this.rbtnFilterNone.Location = new System.Drawing.Point(8, 8);
            this.rbtnFilterNone.Name = "rbtnFilterNone";
            this.rbtnFilterNone.Size = new System.Drawing.Size(95, 23);
            this.rbtnFilterNone.TabIndex = 1;
            this.rbtnFilterNone.TabStop = true;
            this.rbtnFilterNone.Text = "Все записи";
            this.rbtnFilterNone.UseVisualStyleBackColor = true;
            this.rbtnFilterNone.CheckedChanged += new System.EventHandler(this.FilterTypeChanged);
            // 
            // timer
            // 
            this.timer.Interval = 444;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // FrmFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(216, 241);
            this.Controls.Add(this.panelFilter);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmFilter";
            this.ShowInTaskbar = false;
            this.Text = "Фильтр";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmFilter_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmFilter_FormClosed);
            this.Load += new System.EventHandler(this.FrmFilter_Load);
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.RadioButton rbtnFilterYears;
        private System.Windows.Forms.RadioButton rbtnFilterPeriod;
        private System.Windows.Forms.RadioButton rbtnFilterDay;
        private System.Windows.Forms.RadioButton rbtnFilterNone;
        private System.Windows.Forms.DateTimePicker dtpDay;
        private System.Windows.Forms.TextBox tbYears;
        private System.Windows.Forms.DateTimePicker dtpDateTo;
        private System.Windows.Forms.DateTimePicker dtpDateFrom;
        private System.Windows.Forms.Timer timer;
    }
}