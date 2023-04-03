namespace TileExplorer
{
    partial class FrmMarker
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblText = new System.Windows.Forms.Label();
            this.tbText = new System.Windows.Forms.TextBox();
            this.lblPointLat = new System.Windows.Forms.Label();
            this.lblPointLng = new System.Windows.Forms.Label();
            this.udPointLat = new System.Windows.Forms.NumericUpDown();
            this.udPointLng = new System.Windows.Forms.NumericUpDown();
            this.udOffsetY = new System.Windows.Forms.NumericUpDown();
            this.udOffsetX = new System.Windows.Forms.NumericUpDown();
            this.lblOffsetY = new System.Windows.Forms.Label();
            this.lblOffsetX = new System.Windows.Forms.Label();
            this.cboxTextVisible = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.udPointLat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPointLng)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udOffsetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udOffsetX)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(136, 224);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 32);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(224, 224);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 32);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(8, 8);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(42, 19);
            this.lblText.TabIndex = 0;
            this.lblText.Text = "Текст";
            // 
            // tbText
            // 
            this.tbText.Location = new System.Drawing.Point(8, 30);
            this.tbText.Name = "tbText";
            this.tbText.Size = new System.Drawing.Size(296, 25);
            this.tbText.TabIndex = 1;
            // 
            // lblPointLat
            // 
            this.lblPointLat.AutoSize = true;
            this.lblPointLat.Location = new System.Drawing.Point(8, 66);
            this.lblPointLat.Name = "lblPointLat";
            this.lblPointLat.Size = new System.Drawing.Size(59, 19);
            this.lblPointLat.TabIndex = 2;
            this.lblPointLat.Text = "Широта";
            // 
            // lblPointLng
            // 
            this.lblPointLng.AutoSize = true;
            this.lblPointLng.Location = new System.Drawing.Point(160, 66);
            this.lblPointLng.Name = "lblPointLng";
            this.lblPointLng.Size = new System.Drawing.Size(60, 19);
            this.lblPointLng.TabIndex = 4;
            this.lblPointLng.Text = "Долгота";
            // 
            // udPointLat
            // 
            this.udPointLat.DecimalPlaces = 8;
            this.udPointLat.Location = new System.Drawing.Point(8, 88);
            this.udPointLat.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.udPointLat.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.udPointLat.Name = "udPointLat";
            this.udPointLat.Size = new System.Drawing.Size(144, 25);
            this.udPointLat.TabIndex = 3;
            // 
            // udPointLng
            // 
            this.udPointLng.DecimalPlaces = 8;
            this.udPointLng.Location = new System.Drawing.Point(160, 88);
            this.udPointLng.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.udPointLng.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.udPointLng.Name = "udPointLng";
            this.udPointLng.Size = new System.Drawing.Size(144, 25);
            this.udPointLng.TabIndex = 5;
            // 
            // udOffsetY
            // 
            this.udOffsetY.Location = new System.Drawing.Point(160, 144);
            this.udOffsetY.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.udOffsetY.Name = "udOffsetY";
            this.udOffsetY.Size = new System.Drawing.Size(144, 25);
            this.udOffsetY.TabIndex = 9;
            // 
            // udOffsetX
            // 
            this.udOffsetX.Location = new System.Drawing.Point(8, 144);
            this.udOffsetX.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.udOffsetX.Name = "udOffsetX";
            this.udOffsetX.Size = new System.Drawing.Size(144, 25);
            this.udOffsetX.TabIndex = 7;
            // 
            // lblOffsetY
            // 
            this.lblOffsetY.AutoSize = true;
            this.lblOffsetY.Location = new System.Drawing.Point(160, 122);
            this.lblOffsetY.Name = "lblOffsetY";
            this.lblOffsetY.Size = new System.Drawing.Size(132, 19);
            this.lblOffsetY.TabIndex = 8;
            this.lblOffsetY.Text = "Смещение текста Y";
            // 
            // lblOffsetX
            // 
            this.lblOffsetX.AutoSize = true;
            this.lblOffsetX.Location = new System.Drawing.Point(8, 122);
            this.lblOffsetX.Name = "lblOffsetX";
            this.lblOffsetX.Size = new System.Drawing.Size(132, 19);
            this.lblOffsetX.TabIndex = 6;
            this.lblOffsetX.Text = "Смещение текста X";
            // 
            // cboxTextVisible
            // 
            this.cboxTextVisible.AutoSize = true;
            this.cboxTextVisible.Checked = true;
            this.cboxTextVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxTextVisible.Location = new System.Drawing.Point(8, 184);
            this.cboxTextVisible.Name = "cboxTextVisible";
            this.cboxTextVisible.Size = new System.Drawing.Size(139, 23);
            this.cboxTextVisible.TabIndex = 10;
            this.cboxTextVisible.Text = "Показывать текст";
            this.cboxTextVisible.UseVisualStyleBackColor = true;
            // 
            // FrmMarker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 265);
            this.Controls.Add(this.cboxTextVisible);
            this.Controls.Add(this.udOffsetY);
            this.Controls.Add(this.udOffsetX);
            this.Controls.Add(this.lblOffsetY);
            this.Controls.Add(this.lblOffsetX);
            this.Controls.Add(this.udPointLng);
            this.Controls.Add(this.udPointLat);
            this.Controls.Add(this.lblPointLng);
            this.Controls.Add(this.lblPointLat);
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmMarker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Маркер";
            ((System.ComponentModel.ISupportInitialize)(this.udPointLat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPointLng)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udOffsetY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udOffsetX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.Label lblPointLat;
        private System.Windows.Forms.Label lblPointLng;
        private System.Windows.Forms.NumericUpDown udPointLat;
        private System.Windows.Forms.NumericUpDown udPointLng;
        private System.Windows.Forms.NumericUpDown udOffsetY;
        private System.Windows.Forms.NumericUpDown udOffsetX;
        private System.Windows.Forms.Label lblOffsetY;
        private System.Windows.Forms.Label lblOffsetX;
        private System.Windows.Forms.CheckBox cboxTextVisible;
    }
}