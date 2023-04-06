namespace TileExplorer
{
    partial class FrmMapDesign
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.gbMarker = new System.Windows.Forms.GroupBox();
            this.btnColorMarkerText = new System.Windows.Forms.Button();
            this.tbColorMarkerText = new System.Windows.Forms.TextBox();
            this.btnFontMarker = new System.Windows.Forms.Button();
            this.tbFontMarker = new System.Windows.Forms.TextBox();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.gbMarker.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(359, 279);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 32);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(273, 279);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 32);
            this.btnOk.TabIndex = 13;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // fontDialog
            // 
            this.fontDialog.FontMustExist = true;
            this.fontDialog.ShowEffects = false;
            // 
            // gbMarker
            // 
            this.gbMarker.Controls.Add(this.btnColorMarkerText);
            this.gbMarker.Controls.Add(this.tbColorMarkerText);
            this.gbMarker.Controls.Add(this.btnFontMarker);
            this.gbMarker.Controls.Add(this.tbFontMarker);
            this.gbMarker.Location = new System.Drawing.Point(8, 8);
            this.gbMarker.Name = "gbMarker";
            this.gbMarker.Size = new System.Drawing.Size(410, 118);
            this.gbMarker.TabIndex = 0;
            this.gbMarker.TabStop = false;
            this.gbMarker.Text = "Маркер";
            // 
            // btnColorMarkerText
            // 
            this.btnColorMarkerText.Location = new System.Drawing.Point(241, 60);
            this.btnColorMarkerText.Name = "btnColorMarkerText";
            this.btnColorMarkerText.Size = new System.Drawing.Size(56, 26);
            this.btnColorMarkerText.TabIndex = 20;
            this.btnColorMarkerText.Text = "...";
            this.btnColorMarkerText.UseVisualStyleBackColor = true;
            this.btnColorMarkerText.Click += new System.EventHandler(this.BtnFontColorMarker_Click);
            // 
            // tbColorMarkerText
            // 
            this.tbColorMarkerText.Location = new System.Drawing.Point(135, 59);
            this.tbColorMarkerText.Name = "tbColorMarkerText";
            this.tbColorMarkerText.Size = new System.Drawing.Size(100, 27);
            this.tbColorMarkerText.TabIndex = 19;
            // 
            // btnFontMarker
            // 
            this.btnFontMarker.Location = new System.Drawing.Point(241, 26);
            this.btnFontMarker.Name = "btnFontMarker";
            this.btnFontMarker.Size = new System.Drawing.Size(56, 26);
            this.btnFontMarker.TabIndex = 18;
            this.btnFontMarker.Text = "...";
            this.btnFontMarker.UseVisualStyleBackColor = true;
            this.btnFontMarker.Click += new System.EventHandler(this.BtnFontMarker_Click);
            // 
            // tbFontMarker
            // 
            this.tbFontMarker.Location = new System.Drawing.Point(8, 26);
            this.tbFontMarker.Name = "tbFontMarker";
            this.tbFontMarker.ReadOnly = true;
            this.tbFontMarker.Size = new System.Drawing.Size(227, 27);
            this.tbFontMarker.TabIndex = 17;
            // 
            // FrmMapDesign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 321);
            this.Controls.Add(this.gbMarker);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FrmMapDesign";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Оформление";
            this.gbMarker.ResumeLayout(false);
            this.gbMarker.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.GroupBox gbMarker;
        private System.Windows.Forms.TextBox tbColorMarkerText;
        private System.Windows.Forms.Button btnFontMarker;
        private System.Windows.Forms.TextBox tbFontMarker;
        private System.Windows.Forms.Button btnColorMarkerText;
        private System.Windows.Forms.ColorDialog colorDialog;
    }
}