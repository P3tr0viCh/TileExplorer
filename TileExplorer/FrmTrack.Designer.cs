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
            this.lblEquipment = new System.Windows.Forms.Label();
            this.cboxEquipment = new System.Windows.Forms.ComboBox();
            this.equipmentBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblEleAscent = new System.Windows.Forms.Label();
            this.tbEleAscent = new System.Windows.Forms.TextBox();
            this.btnOKToALL = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.equipmentBindingSource)).BeginInit();
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
            this.btnCancel.Location = new System.Drawing.Point(336, 128);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 32);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(248, 128);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 32);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // lblEquipment
            // 
            this.lblEquipment.AutoSize = true;
            this.lblEquipment.Location = new System.Drawing.Point(8, 64);
            this.lblEquipment.Name = "lblEquipment";
            this.lblEquipment.Size = new System.Drawing.Size(88, 19);
            this.lblEquipment.TabIndex = 2;
            this.lblEquipment.Text = "Снаряжение";
            // 
            // cboxEquipment
            // 
            this.cboxEquipment.DataSource = this.equipmentBindingSource;
            this.cboxEquipment.DisplayMember = "Name";
            this.cboxEquipment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxEquipment.FormattingEnabled = true;
            this.cboxEquipment.Location = new System.Drawing.Point(8, 86);
            this.cboxEquipment.Name = "cboxEquipment";
            this.cboxEquipment.Size = new System.Drawing.Size(160, 25);
            this.cboxEquipment.TabIndex = 3;
            this.cboxEquipment.ValueMember = "Id";
            // 
            // equipmentBindingSource
            // 
            this.equipmentBindingSource.DataSource = typeof(TileExplorer.Database.Models.Equipment);
            // 
            // lblEleAscent
            // 
            this.lblEleAscent.AutoSize = true;
            this.lblEleAscent.Location = new System.Drawing.Point(176, 64);
            this.lblEleAscent.Name = "lblEleAscent";
            this.lblEleAscent.Size = new System.Drawing.Size(60, 19);
            this.lblEleAscent.TabIndex = 4;
            this.lblEleAscent.Text = "Подъём";
            // 
            // tbEleAscent
            // 
            this.tbEleAscent.Location = new System.Drawing.Point(176, 86);
            this.tbEleAscent.Name = "tbEleAscent";
            this.tbEleAscent.Size = new System.Drawing.Size(80, 25);
            this.tbEleAscent.TabIndex = 5;
            // 
            // btnOKToALL
            // 
            this.btnOKToALL.Location = new System.Drawing.Point(8, 128);
            this.btnOKToALL.Name = "btnOKToALL";
            this.btnOKToALL.Size = new System.Drawing.Size(96, 32);
            this.btnOKToALL.TabIndex = 8;
            this.btnOKToALL.Text = "OK для всех";
            this.btnOKToALL.UseVisualStyleBackColor = true;
            this.btnOKToALL.Click += new System.EventHandler(this.BtnOKToALL_Click);
            // 
            // FrmTrack
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(424, 169);
            this.Controls.Add(this.btnOKToALL);
            this.Controls.Add(this.tbEleAscent);
            this.Controls.Add(this.lblEleAscent);
            this.Controls.Add(this.cboxEquipment);
            this.Controls.Add(this.lblEquipment);
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
            ((System.ComponentModel.ISupportInitialize)(this.equipmentBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblEquipment;
        private System.Windows.Forms.ComboBox cboxEquipment;
        private System.Windows.Forms.BindingSource equipmentBindingSource;
        private System.Windows.Forms.Label lblEleAscent;
        private System.Windows.Forms.TextBox tbEleAscent;
        private System.Windows.Forms.Button btnOKToALL;
    }
}