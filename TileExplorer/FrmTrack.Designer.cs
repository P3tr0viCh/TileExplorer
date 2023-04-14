namespace TileExplorer
{
    partial class FrmTrack
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
            this.tbText = new System.Windows.Forms.TextBox();
            this.lblText = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.dtpDateTime = new System.Windows.Forms.DateTimePicker();
            this.cmDateTime = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miDateTimeAddTimeZone = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDateTime.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbText
            // 
            this.tbText.Location = new System.Drawing.Point(8, 30);
            this.tbText.Name = "tbText";
            this.tbText.Size = new System.Drawing.Size(408, 25);
            this.tbText.TabIndex = 1;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(8, 8);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(69, 19);
            this.lblText.TabIndex = 0;
            this.lblText.Text = "Название";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(336, 104);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 32);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(248, 104);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 32);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // dtpDateTime
            // 
            this.dtpDateTime.ContextMenuStrip = this.cmDateTime;
            this.dtpDateTime.CustomFormat = "";
            this.dtpDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateTime.Location = new System.Drawing.Point(8, 64);
            this.dtpDateTime.Name = "dtpDateTime";
            this.dtpDateTime.Size = new System.Drawing.Size(200, 25);
            this.dtpDateTime.TabIndex = 2;
            // 
            // cmDateTime
            // 
            this.cmDateTime.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDateTimeAddTimeZone});
            this.cmDateTime.Name = "cmDateTime";
            this.cmDateTime.Size = new System.Drawing.Size(274, 48);
            // 
            // miDateTimeAddTimeZone
            // 
            this.miDateTimeAddTimeZone.Name = "miDateTimeAddTimeZone";
            this.miDateTimeAddTimeZone.Size = new System.Drawing.Size(273, 22);
            this.miDateTimeAddTimeZone.Text = "Добавить сдвиг часового пояса ({0})";
            this.miDateTimeAddTimeZone.Click += new System.EventHandler(this.MiDateTimeAddTimeZone_Click);
            // 
            // FrmTrack
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(424, 145);
            this.Controls.Add(this.dtpDateTime);
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTrack";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Трек";
            this.Load += new System.EventHandler(this.FrmTrack_Load);
            this.cmDateTime.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.DateTimePicker dtpDateTime;
        private System.Windows.Forms.ContextMenuStrip cmDateTime;
        private System.Windows.Forms.ToolStripMenuItem miDateTimeAddTimeZone;
    }
}