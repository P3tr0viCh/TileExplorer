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
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlUpdating = new System.Windows.Forms.Panel();
            this.lblUpdating = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.pnlUpdating.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(5);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersWidth = 32;
            this.dataGridView.Size = new System.Drawing.Size(384, 265);
            this.dataGridView.TabIndex = 2;
            // 
            // colId
            // 
            this.colId.HeaderText = "Id";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.Width = 64;
            // 
            // pnlUpdating
            // 
            this.pnlUpdating.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUpdating.Controls.Add(this.lblUpdating);
            this.pnlUpdating.Location = new System.Drawing.Point(146, 99);
            this.pnlUpdating.Name = "pnlUpdating";
            this.pnlUpdating.Size = new System.Drawing.Size(116, 57);
            this.pnlUpdating.TabIndex = 3;
            // 
            // lblUpdating
            // 
            this.lblUpdating.AutoSize = true;
            this.lblUpdating.Location = new System.Drawing.Point(16, 16);
            this.lblUpdating.Name = "lblUpdating";
            this.lblUpdating.Size = new System.Drawing.Size(75, 19);
            this.lblUpdating.TabIndex = 0;
            this.lblUpdating.Text = "Updating...";
            // 
            // FrmList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 265);
            this.Controls.Add(this.pnlUpdating);
            this.Controls.Add(this.dataGridView);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(200, 100);
            this.Name = "FrmList";
            this.ShowInTaskbar = false;
            this.Text = "FrmList";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmList_FormClosing);
            this.SizeChanged += new System.EventHandler(this.FrmList_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.pnlUpdating.ResumeLayout(false);
            this.pnlUpdating.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Panel pnlUpdating;
        private System.Windows.Forms.Label lblUpdating;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
    }
}