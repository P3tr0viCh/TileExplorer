namespace TileExplorer
{
    partial class FrmList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmList));
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.toolStripLeft = new System.Windows.Forms.ToolStrip();
            this.tsbtnAdd = new System.Windows.Forms.ToolStripButton();
            this.tsbtnChange = new System.Windows.Forms.ToolStripButton();
            this.tsbtnDelete = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.LeftToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.toolStripLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // bindingSource
            // 
            this.bindingSource.PositionChanged += new System.EventHandler(this.BindingSource_PositionChanged);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersWidth = 32;
            this.dataGridView.Size = new System.Drawing.Size(343, 240);
            this.dataGridView.TabIndex = 1;
            this.dataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridView_CellMouseDoubleClick);
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.dataGridView);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(343, 240);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // toolStripContainer.LeftToolStripPanel
            // 
            this.toolStripContainer.LeftToolStripPanel.Controls.Add(this.toolStripLeft);
            this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(384, 265);
            this.toolStripContainer.TabIndex = 2;
            this.toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripLeft
            // 
            this.toolStripLeft.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripLeft.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripLeft.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripLeft.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnAdd,
            this.tsbtnChange,
            this.tsbtnDelete});
            this.toolStripLeft.Location = new System.Drawing.Point(0, 4);
            this.toolStripLeft.Name = "toolStripLeft";
            this.toolStripLeft.Size = new System.Drawing.Size(41, 126);
            this.toolStripLeft.TabIndex = 0;
            this.toolStripLeft.MouseEnter += new System.EventHandler(this.ToolStripLeft_MouseEnter);
            // 
            // tsbtnAdd
            // 
            this.tsbtnAdd.AutoSize = false;
            this.tsbtnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnAdd.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnAdd.Image")));
            this.tsbtnAdd.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnAdd.Name = "tsbtnAdd";
            this.tsbtnAdd.Size = new System.Drawing.Size(40, 32);
            this.tsbtnAdd.Text = "+";
            this.tsbtnAdd.ToolTipText = "Добавить";
            this.tsbtnAdd.Click += new System.EventHandler(this.TsbtnAdd_Click);
            // 
            // tsbtnChange
            // 
            this.tsbtnChange.AutoSize = false;
            this.tsbtnChange.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnChange.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnChange.Image")));
            this.tsbtnChange.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnChange.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnChange.Name = "tsbtnChange";
            this.tsbtnChange.Size = new System.Drawing.Size(40, 32);
            this.tsbtnChange.Text = "~";
            this.tsbtnChange.ToolTipText = "Изменить";
            this.tsbtnChange.Click += new System.EventHandler(this.TsbtnChange_Click);
            // 
            // tsbtnDelete
            // 
            this.tsbtnDelete.AutoSize = false;
            this.tsbtnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnDelete.Image")));
            this.tsbtnDelete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnDelete.Name = "tsbtnDelete";
            this.tsbtnDelete.Size = new System.Drawing.Size(40, 32);
            this.tsbtnDelete.Text = "-";
            this.tsbtnDelete.ToolTipText = "Удалить";
            this.tsbtnDelete.Click += new System.EventHandler(this.TsbtnDelete_Click);
            // 
            // FrmList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 265);
            this.Controls.Add(this.toolStripContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 100);
            this.Name = "FrmList";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FrmList";
            this.Activated += new System.EventHandler(this.FrmList_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmListNew_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmListNew_FormClosed);
            this.Load += new System.EventHandler(this.FrmListNew_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.LeftToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.LeftToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.toolStripLeft.ResumeLayout(false);
            this.toolStripLeft.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.ToolStrip toolStripLeft;
        private System.Windows.Forms.ToolStripButton tsbtnAdd;
        private System.Windows.Forms.ToolStripButton tsbtnChange;
        private System.Windows.Forms.ToolStripButton tsbtnDelete;
    }
}