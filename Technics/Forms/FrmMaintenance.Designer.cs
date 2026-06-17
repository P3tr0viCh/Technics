namespace Technics
{
    partial class FrmMaintenance
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
            this.lblDateTime = new System.Windows.Forms.Label();
            this.lblTech = new System.Windows.Forms.Label();
            this.dtpDateTime = new System.Windows.Forms.DateTimePicker();
            this.cboxTech = new System.Windows.Forms.ComboBox();
            this.bindingSourceTechs = new System.Windows.Forms.BindingSource(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblMt = new System.Windows.Forms.Label();
            this.cboxMt = new System.Windows.Forms.ComboBox();
            this.bindingSourceMts = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTechs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceMts)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDateTime
            // 
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.Location = new System.Drawing.Point(8, 120);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(94, 19);
            this.lblDateTime.TabIndex = 4;
            this.lblDateTime.Text = "Дата и время";
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
            // dtpDateTime
            // 
            this.dtpDateTime.CustomFormat = "yyyy.MM.dd HH:mm";
            this.dtpDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateTime.Location = new System.Drawing.Point(8, 144);
            this.dtpDateTime.Name = "dtpDateTime";
            this.dtpDateTime.Size = new System.Drawing.Size(176, 25);
            this.dtpDateTime.TabIndex = 5;
            this.dtpDateTime.Value = new System.DateTime(1981, 3, 29, 1, 2, 0, 0);
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
            this.cboxTech.SelectedIndexChanged += new System.EventHandler(this.CboxTech_SelectedIndexChanged);
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
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(200, 184);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 32);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // lblMt
            // 
            this.lblMt.AutoSize = true;
            this.lblMt.Location = new System.Drawing.Point(8, 64);
            this.lblMt.Name = "lblMt";
            this.lblMt.Size = new System.Drawing.Size(53, 19);
            this.lblMt.TabIndex = 2;
            this.lblMt.Text = "Деталь";
            // 
            // cboxMt
            // 
            this.cboxMt.DataSource = this.bindingSourceMts;
            this.cboxMt.DisplayMember = "Text";
            this.cboxMt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxMt.FormattingEnabled = true;
            this.cboxMt.Location = new System.Drawing.Point(8, 88);
            this.cboxMt.Name = "cboxMt";
            this.cboxMt.Size = new System.Drawing.Size(360, 25);
            this.cboxMt.TabIndex = 3;
            this.cboxMt.ValueMember = "Id";
            this.cboxMt.SelectedIndexChanged += new System.EventHandler(this.CboxMaintenance_SelectedIndexChanged);
            // 
            // bindingSourceMts
            // 
            this.bindingSourceMts.DataSource = typeof(Technics.Database.Models.MtModel);
            // 
            // FrmMaintenance
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(376, 225);
            this.Controls.Add(this.lblMt);
            this.Controls.Add(this.cboxMt);
            this.Controls.Add(this.lblDateTime);
            this.Controls.Add(this.lblTech);
            this.Controls.Add(this.dtpDateTime);
            this.Controls.Add(this.cboxTech);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMaintenance";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Обслуживание";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMaintenance_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTechs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceMts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Label lblTech;
        private System.Windows.Forms.DateTimePicker dtpDateTime;
        private System.Windows.Forms.ComboBox cboxTech;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.BindingSource bindingSourceTechs;
        private System.Windows.Forms.Label lblMt;
        private System.Windows.Forms.ComboBox cboxMt;
        private System.Windows.Forms.BindingSource bindingSourceMts;
    }
}