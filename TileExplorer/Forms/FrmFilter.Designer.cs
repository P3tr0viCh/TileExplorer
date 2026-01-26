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
            this.clbYears = new System.Windows.Forms.CheckedListBox();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpDay = new System.Windows.Forms.DateTimePicker();
            this.rbtnFilterYears = new System.Windows.Forms.RadioButton();
            this.rbtnFilterPeriod = new System.Windows.Forms.RadioButton();
            this.rbtnFilterDay = new System.Windows.Forms.RadioButton();
            this.rbtnFilterAllDate = new System.Windows.Forms.RadioButton();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.clbEquipments = new System.Windows.Forms.CheckedListBox();
            this.cboxUseEquipments = new System.Windows.Forms.CheckBox();
            this.cboxUseTags = new System.Windows.Forms.CheckBox();
            this.clbTags = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // clbYears
            // 
            this.clbYears.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.clbYears.CheckOnClick = true;
            this.clbYears.IntegralHeight = false;
            this.clbYears.Location = new System.Drawing.Point(8, 208);
            this.clbYears.MultiColumn = true;
            this.clbYears.Name = "clbYears";
            this.clbYears.Size = new System.Drawing.Size(200, 96);
            this.clbYears.TabIndex = 7;
            this.clbYears.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckedListBox_ItemCheck);
            // 
            // dtpDateTo
            // 
            this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateTo.Location = new System.Drawing.Point(8, 152);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(200, 25);
            this.dtpDateTo.TabIndex = 5;
            this.dtpDateTo.Value = new System.DateTime(1981, 3, 29, 0, 0, 0, 0);
            this.dtpDateTo.ValueChanged += new System.EventHandler(this.FilterTypeChanged);
            // 
            // dtpDateFrom
            // 
            this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateFrom.Location = new System.Drawing.Point(8, 120);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(200, 25);
            this.dtpDateFrom.TabIndex = 4;
            this.dtpDateFrom.Value = new System.DateTime(1981, 3, 29, 0, 0, 0, 0);
            this.dtpDateFrom.ValueChanged += new System.EventHandler(this.FilterTypeChanged);
            // 
            // dtpDay
            // 
            this.dtpDay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDay.Location = new System.Drawing.Point(8, 64);
            this.dtpDay.Name = "dtpDay";
            this.dtpDay.Size = new System.Drawing.Size(200, 25);
            this.dtpDay.TabIndex = 2;
            this.dtpDay.Value = new System.DateTime(1981, 3, 29, 0, 0, 0, 0);
            this.dtpDay.ValueChanged += new System.EventHandler(this.DateTimeChanged);
            // 
            // rbtnFilterYears
            // 
            this.rbtnFilterYears.AutoSize = true;
            this.rbtnFilterYears.Location = new System.Drawing.Point(8, 184);
            this.rbtnFilterYears.Name = "rbtnFilterYears";
            this.rbtnFilterYears.Size = new System.Drawing.Size(60, 23);
            this.rbtnFilterYears.TabIndex = 6;
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
            this.rbtnFilterDay.TabIndex = 1;
            this.rbtnFilterDay.TabStop = true;
            this.rbtnFilterDay.Text = "Один день";
            this.rbtnFilterDay.UseVisualStyleBackColor = true;
            this.rbtnFilterDay.CheckedChanged += new System.EventHandler(this.FilterTypeChanged);
            // 
            // rbtnFilterAllDate
            // 
            this.rbtnFilterAllDate.AutoSize = true;
            this.rbtnFilterAllDate.Location = new System.Drawing.Point(8, 8);
            this.rbtnFilterAllDate.Name = "rbtnFilterAllDate";
            this.rbtnFilterAllDate.Size = new System.Drawing.Size(101, 23);
            this.rbtnFilterAllDate.TabIndex = 0;
            this.rbtnFilterAllDate.TabStop = true;
            this.rbtnFilterAllDate.Text = "Любая дата";
            this.rbtnFilterAllDate.UseVisualStyleBackColor = true;
            this.rbtnFilterAllDate.CheckedChanged += new System.EventHandler(this.FilterTypeChanged);
            // 
            // timer
            // 
            this.timer.Interval = 444;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // clbEquipments
            // 
            this.clbEquipments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clbEquipments.CheckOnClick = true;
            this.clbEquipments.ColumnWidth = 160;
            this.clbEquipments.IntegralHeight = false;
            this.clbEquipments.Location = new System.Drawing.Point(216, 32);
            this.clbEquipments.MultiColumn = true;
            this.clbEquipments.Name = "clbEquipments";
            this.clbEquipments.Size = new System.Drawing.Size(200, 96);
            this.clbEquipments.TabIndex = 9;
            this.clbEquipments.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckedListBox_ItemCheck);
            // 
            // cboxUseEquipments
            // 
            this.cboxUseEquipments.AutoSize = true;
            this.cboxUseEquipments.Location = new System.Drawing.Point(216, 8);
            this.cboxUseEquipments.Name = "cboxUseEquipments";
            this.cboxUseEquipments.Size = new System.Drawing.Size(107, 23);
            this.cboxUseEquipments.TabIndex = 8;
            this.cboxUseEquipments.Text = "Снаряжение";
            this.cboxUseEquipments.UseVisualStyleBackColor = true;
            this.cboxUseEquipments.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // cboxUseTags
            // 
            this.cboxUseTags.AutoSize = true;
            this.cboxUseTags.Location = new System.Drawing.Point(216, 136);
            this.cboxUseTags.Name = "cboxUseTags";
            this.cboxUseTags.Size = new System.Drawing.Size(55, 23);
            this.cboxUseTags.TabIndex = 10;
            this.cboxUseTags.Text = "Теги";
            this.cboxUseTags.UseVisualStyleBackColor = true;
            this.cboxUseTags.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // clbTags
            // 
            this.clbTags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clbTags.CheckOnClick = true;
            this.clbTags.ColumnWidth = 160;
            this.clbTags.IntegralHeight = false;
            this.clbTags.Location = new System.Drawing.Point(216, 160);
            this.clbTags.MultiColumn = true;
            this.clbTags.Name = "clbTags";
            this.clbTags.Size = new System.Drawing.Size(200, 144);
            this.clbTags.TabIndex = 11;
            this.clbTags.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckedListBox_ItemCheck);
            // 
            // FrmFilter
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(424, 313);
            this.Controls.Add(this.cboxUseTags);
            this.Controls.Add(this.clbTags);
            this.Controls.Add(this.cboxUseEquipments);
            this.Controls.Add(this.clbEquipments);
            this.Controls.Add(this.rbtnFilterAllDate);
            this.Controls.Add(this.clbYears);
            this.Controls.Add(this.rbtnFilterDay);
            this.Controls.Add(this.dtpDateTo);
            this.Controls.Add(this.rbtnFilterPeriod);
            this.Controls.Add(this.dtpDateFrom);
            this.Controls.Add(this.rbtnFilterYears);
            this.Controls.Add(this.dtpDay);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(440, 352);
            this.Name = "FrmFilter";
            this.ShowInTaskbar = false;
            this.Text = "Фильтр";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmFilter_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmFilter_FormClosed);
            this.Load += new System.EventHandler(this.FrmFilter_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmFilter_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton rbtnFilterYears;
        private System.Windows.Forms.RadioButton rbtnFilterPeriod;
        private System.Windows.Forms.RadioButton rbtnFilterDay;
        private System.Windows.Forms.RadioButton rbtnFilterAllDate;
        private System.Windows.Forms.DateTimePicker dtpDay;
        private System.Windows.Forms.DateTimePicker dtpDateTo;
        private System.Windows.Forms.DateTimePicker dtpDateFrom;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.CheckedListBox clbYears;
        private System.Windows.Forms.CheckedListBox clbEquipments;
        private System.Windows.Forms.CheckBox cboxUseEquipments;
        private System.Windows.Forms.CheckBox cboxUseTags;
        private System.Windows.Forms.CheckedListBox clbTags;
    }
}