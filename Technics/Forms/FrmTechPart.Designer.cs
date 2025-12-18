namespace Technics
{
    partial class FrmTechPart
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
            this.lblDateTimeInstall = new System.Windows.Forms.Label();
            this.lblTech = new System.Windows.Forms.Label();
            this.dtpDateTimeInstall = new System.Windows.Forms.DateTimePicker();
            this.cboxTech = new System.Windows.Forms.ComboBox();
            this.bindingSourceTechs = new System.Windows.Forms.BindingSource(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblDateTimeRemove = new System.Windows.Forms.Label();
            this.dtpDateTimeRemove = new System.Windows.Forms.DateTimePicker();
            this.lblPart = new System.Windows.Forms.Label();
            this.cboxPart = new System.Windows.Forms.ComboBox();
            this.bindingSourceParts = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTechs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceParts)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDateTimeInstall
            // 
            this.lblDateTimeInstall.AutoSize = true;
            this.lblDateTimeInstall.Location = new System.Drawing.Point(8, 120);
            this.lblDateTimeInstall.Name = "lblDateTimeInstall";
            this.lblDateTimeInstall.Size = new System.Drawing.Size(162, 19);
            this.lblDateTimeInstall.TabIndex = 4;
            this.lblDateTimeInstall.Text = "Дата и время установки";
            // 
            // lblTech
            // 
            this.lblTech.AutoSize = true;
            this.lblTech.Location = new System.Drawing.Point(8, 8);
            this.lblTech.Name = "lblTech";
            this.lblTech.Size = new System.Drawing.Size(59, 19);
            this.lblTech.TabIndex = 0;
            this.lblTech.Text = "Техника";
            // 
            // dtpDateTimeInstall
            // 
            this.dtpDateTimeInstall.CustomFormat = "yyyy.MM.dd HH:mm";
            this.dtpDateTimeInstall.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateTimeInstall.Location = new System.Drawing.Point(8, 144);
            this.dtpDateTimeInstall.Name = "dtpDateTimeInstall";
            this.dtpDateTimeInstall.Size = new System.Drawing.Size(176, 25);
            this.dtpDateTimeInstall.TabIndex = 5;
            this.dtpDateTimeInstall.Value = new System.DateTime(1981, 3, 29, 1, 2, 0, 0);
            // 
            // cboxTech
            // 
            this.cboxTech.DataSource = this.bindingSourceTechs;
            this.cboxTech.DisplayMember = "Text";
            this.cboxTech.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxTech.FormattingEnabled = true;
            this.cboxTech.Location = new System.Drawing.Point(8, 32);
            this.cboxTech.Name = "cboxTech";
            this.cboxTech.Size = new System.Drawing.Size(176, 25);
            this.cboxTech.TabIndex = 1;
            this.cboxTech.ValueMember = "Id";
            // 
            // bindingSourceTechs
            // 
            this.bindingSourceTechs.DataSource = typeof(Technics.Database.Models.TechModel);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(288, 184);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 32);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(200, 184);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 32);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // lblDateTimeRemove
            // 
            this.lblDateTimeRemove.AutoSize = true;
            this.lblDateTimeRemove.Location = new System.Drawing.Point(192, 120);
            this.lblDateTimeRemove.Name = "lblDateTimeRemove";
            this.lblDateTimeRemove.Size = new System.Drawing.Size(140, 19);
            this.lblDateTimeRemove.TabIndex = 6;
            this.lblDateTimeRemove.Text = "Дата и время снятия";
            // 
            // dtpDateTimeRemove
            // 
            this.dtpDateTimeRemove.CustomFormat = "yyyy.MM.dd HH:mm";
            this.dtpDateTimeRemove.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateTimeRemove.Location = new System.Drawing.Point(192, 144);
            this.dtpDateTimeRemove.Name = "dtpDateTimeRemove";
            this.dtpDateTimeRemove.ShowCheckBox = true;
            this.dtpDateTimeRemove.Size = new System.Drawing.Size(176, 25);
            this.dtpDateTimeRemove.TabIndex = 7;
            this.dtpDateTimeRemove.Value = new System.DateTime(1981, 3, 29, 1, 2, 0, 0);
            // 
            // lblPart
            // 
            this.lblPart.AutoSize = true;
            this.lblPart.Location = new System.Drawing.Point(8, 64);
            this.lblPart.Name = "lblPart";
            this.lblPart.Size = new System.Drawing.Size(53, 19);
            this.lblPart.TabIndex = 2;
            this.lblPart.Text = "Деталь";
            // 
            // cboxPart
            // 
            this.cboxPart.DataSource = this.bindingSourceParts;
            this.cboxPart.DisplayMember = "Text";
            this.cboxPart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxPart.FormattingEnabled = true;
            this.cboxPart.Location = new System.Drawing.Point(8, 88);
            this.cboxPart.Name = "cboxPart";
            this.cboxPart.Size = new System.Drawing.Size(360, 25);
            this.cboxPart.TabIndex = 3;
            this.cboxPart.ValueMember = "Id";
            // 
            // bindingSourceParts
            // 
            this.bindingSourceParts.DataSource = typeof(Technics.Database.Models.PartModel);
            // 
            // FrmTechPart
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(376, 225);
            this.Controls.Add(this.lblPart);
            this.Controls.Add(this.cboxPart);
            this.Controls.Add(this.lblDateTimeRemove);
            this.Controls.Add(this.dtpDateTimeRemove);
            this.Controls.Add(this.lblDateTimeInstall);
            this.Controls.Add(this.lblTech);
            this.Controls.Add(this.dtpDateTimeInstall);
            this.Controls.Add(this.cboxTech);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTechPart";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Установка и снятие";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTechs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceParts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDateTimeInstall;
        private System.Windows.Forms.Label lblTech;
        private System.Windows.Forms.DateTimePicker dtpDateTimeInstall;
        private System.Windows.Forms.ComboBox cboxTech;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.BindingSource bindingSourceTechs;
        private System.Windows.Forms.Label lblDateTimeRemove;
        private System.Windows.Forms.DateTimePicker dtpDateTimeRemove;
        private System.Windows.Forms.Label lblPart;
        private System.Windows.Forms.ComboBox cboxPart;
        private System.Windows.Forms.BindingSource bindingSourceParts;
    }
}