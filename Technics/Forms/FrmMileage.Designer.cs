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
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTechs)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(326, 167);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 32);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(240, 167);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 32);
            this.btnOk.TabIndex = 12;
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
            this.cboxTech.Location = new System.Drawing.Point(12, 12);
            this.cboxTech.Name = "cboxTech";
            this.cboxTech.Size = new System.Drawing.Size(174, 25);
            this.cboxTech.TabIndex = 14;
            this.cboxTech.ValueMember = "Id";
            // 
            // dtpDateTime
            // 
            this.dtpDateTime.CustomFormat = "yyyy.MM.dd HH:mm";
            this.dtpDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateTime.Location = new System.Drawing.Point(202, 12);
            this.dtpDateTime.Name = "dtpDateTime";
            this.dtpDateTime.Size = new System.Drawing.Size(174, 25);
            this.dtpDateTime.TabIndex = 15;
            this.dtpDateTime.Value = new System.DateTime(1981, 3, 29, 1, 2, 0, 0);
            // 
            // tboxMileage
            // 
            this.tboxMileage.Location = new System.Drawing.Point(12, 64);
            this.tboxMileage.Name = "tboxMileage";
            this.tboxMileage.Size = new System.Drawing.Size(100, 25);
            this.tboxMileage.TabIndex = 16;
            this.tboxMileage.TextChanged += new System.EventHandler(this.TboxMileages_TextChanged);
            // 
            // tboxMileageCommon
            // 
            this.tboxMileageCommon.Location = new System.Drawing.Point(134, 64);
            this.tboxMileageCommon.Name = "tboxMileageCommon";
            this.tboxMileageCommon.Size = new System.Drawing.Size(100, 25);
            this.tboxMileageCommon.TabIndex = 17;
            this.tboxMileageCommon.TextChanged += new System.EventHandler(this.TboxMileages_TextChanged);
            // 
            // tboxDescription
            // 
            this.tboxDescription.Location = new System.Drawing.Point(12, 105);
            this.tboxDescription.Name = "tboxDescription";
            this.tboxDescription.Size = new System.Drawing.Size(389, 25);
            this.tboxDescription.TabIndex = 18;
            // 
            // FrmMileage
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(436, 220);
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
    }
}