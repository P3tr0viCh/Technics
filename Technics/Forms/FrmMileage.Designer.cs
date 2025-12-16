namespace Technics
{
    partial class FrmMileage
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.bindingSourceTechs = new System.Windows.Forms.BindingSource(this.components);
            this.cboxTech = new System.Windows.Forms.ComboBox();
            this.dtpDateTime = new System.Windows.Forms.DateTimePicker();
            this.tboxMileage = new System.Windows.Forms.TextBox();
            this.tboxMileageCommon = new System.Windows.Forms.TextBox();
            this.tboxDescription = new System.Windows.Forms.TextBox();
            this.lblTech = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.lblMileage = new System.Windows.Forms.Label();
            this.lblMileageCommon = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTechs)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(288, 184);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 32);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(200, 184);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 32);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // bindingSourceTechs
            // 
            this.bindingSourceTechs.DataSource = typeof(Technics.Database.Models.TechModel);
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
            // dtpDateTime
            // 
            this.dtpDateTime.CustomFormat = "yyyy.MM.dd HH:mm";
            this.dtpDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateTime.Location = new System.Drawing.Point(192, 32);
            this.dtpDateTime.Name = "dtpDateTime";
            this.dtpDateTime.Size = new System.Drawing.Size(176, 25);
            this.dtpDateTime.TabIndex = 3;
            this.dtpDateTime.Value = new System.DateTime(1981, 3, 29, 1, 2, 0, 0);
            // 
            // tboxMileage
            // 
            this.tboxMileage.Location = new System.Drawing.Point(8, 88);
            this.tboxMileage.Name = "tboxMileage";
            this.tboxMileage.Size = new System.Drawing.Size(104, 25);
            this.tboxMileage.TabIndex = 5;
            this.tboxMileage.TextChanged += new System.EventHandler(this.TboxMileages_TextChanged);
            // 
            // tboxMileageCommon
            // 
            this.tboxMileageCommon.Location = new System.Drawing.Point(120, 88);
            this.tboxMileageCommon.Name = "tboxMileageCommon";
            this.tboxMileageCommon.Size = new System.Drawing.Size(104, 25);
            this.tboxMileageCommon.TabIndex = 7;
            this.tboxMileageCommon.TextChanged += new System.EventHandler(this.TboxMileages_TextChanged);
            // 
            // tboxDescription
            // 
            this.tboxDescription.Location = new System.Drawing.Point(8, 144);
            this.tboxDescription.Name = "tboxDescription";
            this.tboxDescription.Size = new System.Drawing.Size(360, 25);
            this.tboxDescription.TabIndex = 9;
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
            // lblDateTime
            // 
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.Location = new System.Drawing.Point(192, 8);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(94, 19);
            this.lblDateTime.TabIndex = 2;
            this.lblDateTime.Text = "Дата и время";
            // 
            // lblMileage
            // 
            this.lblMileage.AutoSize = true;
            this.lblMileage.Location = new System.Drawing.Point(8, 64);
            this.lblMileage.Name = "lblMileage";
            this.lblMileage.Size = new System.Drawing.Size(55, 19);
            this.lblMileage.TabIndex = 4;
            this.lblMileage.Text = "Пробег";
            // 
            // lblMileageCommon
            // 
            this.lblMileageCommon.AutoSize = true;
            this.lblMileageCommon.Location = new System.Drawing.Point(120, 64);
            this.lblMileageCommon.Name = "lblMileageCommon";
            this.lblMileageCommon.Size = new System.Drawing.Size(104, 19);
            this.lblMileageCommon.TabIndex = 6;
            this.lblMileageCommon.Text = "Общий пробег";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(8, 120);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(72, 19);
            this.lblDescription.TabIndex = 8;
            this.lblDescription.Text = "Описание";
            // 
            // FrmMileage
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(376, 225);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblMileageCommon);
            this.Controls.Add(this.lblMileage);
            this.Controls.Add(this.lblDateTime);
            this.Controls.Add(this.lblTech);
            this.Controls.Add(this.tboxDescription);
            this.Controls.Add(this.tboxMileageCommon);
            this.Controls.Add(this.tboxMileage);
            this.Controls.Add(this.dtpDateTime);
            this.Controls.Add(this.cboxTech);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMileage";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Пробег";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTechs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.BindingSource bindingSourceTechs;
        private System.Windows.Forms.ComboBox cboxTech;
        private System.Windows.Forms.DateTimePicker dtpDateTime;
        private System.Windows.Forms.TextBox tboxMileage;
        private System.Windows.Forms.TextBox tboxMileageCommon;
        private System.Windows.Forms.TextBox tboxDescription;
        private System.Windows.Forms.Label lblTech;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Label lblMileage;
        private System.Windows.Forms.Label lblMileageCommon;
        private System.Windows.Forms.Label lblDescription;
    }
}