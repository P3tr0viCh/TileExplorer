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
            this.btnCancelToALL = new System.Windows.Forms.Button();
            this.tbEleDescent = new System.Windows.Forms.TextBox();
            this.lblEleDescent = new System.Windows.Forms.Label();
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
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(336, 128);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 32);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(248, 128);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 32);
            this.btnOk.TabIndex = 10;
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
            this.cboxEquipment.DisplayMember = "Text";
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
            // btnCancelToALL
            // 
            this.btnCancelToALL.Location = new System.Drawing.Point(112, 128);
            this.btnCancelToALL.Name = "btnCancelToALL";
            this.btnCancelToALL.Size = new System.Drawing.Size(126, 32);
            this.btnCancelToALL.TabIndex = 9;
            this.btnCancelToALL.Text = "Отмена для всех";
            this.btnCancelToALL.UseVisualStyleBackColor = true;
            this.btnCancelToALL.Click += new System.EventHandler(this.BtnCancelToALL_Click);
            // 
            // tbEleDescent
            // 
            this.tbEleDescent.Location = new System.Drawing.Point(264, 86);
            this.tbEleDescent.Name = "tbEleDescent";
            this.tbEleDescent.Size = new System.Drawing.Size(80, 25);
            this.tbEleDescent.TabIndex = 7;
            // 
            // lblEleDescent
            // 
            this.lblEleDescent.AutoSize = true;
            this.lblEleDescent.Location = new System.Drawing.Point(264, 64);
            this.lblEleDescent.Name = "lblEleDescent";
            this.lblEleDescent.Size = new System.Drawing.Size(46, 19);
            this.lblEleDescent.TabIndex = 6;
            this.lblEleDescent.Text = "Спуск";
            // 
            // FrmTrack
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(424, 169);
            this.Controls.Add(this.tbEleDescent);
            this.Controls.Add(this.lblEleDescent);
            this.Controls.Add(this.btnCancelToALL);
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
            this.Load += new System.EventHandler(this.FrmTrack_Load);
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
        private System.Windows.Forms.Button btnCancelToALL;
        private System.Windows.Forms.TextBox tbEleDescent;
        private System.Windows.Forms.Label lblEleDescent;
    }
}