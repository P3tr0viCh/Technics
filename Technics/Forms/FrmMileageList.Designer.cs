namespace Technics
{
    partial class FrmMileageList
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
            this.lblTech = new System.Windows.Forms.Label();
            this.cboxTech = new System.Windows.Forms.ComboBox();
            this.bindingSourceTechs = new System.Windows.Forms.BindingSource(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTechs)).BeginInit();
            this.SuspendLayout();
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
            this.btnCancel.Location = new System.Drawing.Point(104, 72);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 32);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(16, 72);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 32);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // FrmMileageList
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(192, 113);
            this.Controls.Add(this.lblTech);
            this.Controls.Add(this.cboxTech);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMileageList";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Пробеги";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTechs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTech;
        private System.Windows.Forms.ComboBox cboxTech;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.BindingSource bindingSourceTechs;
    }
}