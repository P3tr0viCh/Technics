namespace Technics.Forms
{
    partial class FrmPart
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
            this.lblDescription = new System.Windows.Forms.Label();
            this.tboxDescription = new System.Windows.Forms.TextBox();
            this.lblText = new System.Windows.Forms.Label();
            this.tboxText = new System.Windows.Forms.TextBox();
            this.lblFolder = new System.Windows.Forms.Label();
            this.cboxFolder = new System.Windows.Forms.ComboBox();
            this.bindingSourceFolders = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceFolders)).BeginInit();
            this.SuspendLayout();
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
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(8, 120);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(72, 19);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Описание";
            // 
            // tboxDescription
            // 
            this.tboxDescription.Location = new System.Drawing.Point(8, 144);
            this.tboxDescription.Name = "tboxDescription";
            this.tboxDescription.Size = new System.Drawing.Size(360, 25);
            this.tboxDescription.TabIndex = 5;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(8, 64);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(42, 19);
            this.lblText.TabIndex = 2;
            this.lblText.Text = "Текст";
            // 
            // tboxText
            // 
            this.tboxText.Location = new System.Drawing.Point(8, 88);
            this.tboxText.Name = "tboxText";
            this.tboxText.Size = new System.Drawing.Size(360, 25);
            this.tboxText.TabIndex = 3;
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.Location = new System.Drawing.Point(8, 8);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(48, 19);
            this.lblFolder.TabIndex = 0;
            this.lblFolder.Text = "Папка";
            // 
            // cboxFolder
            // 
            this.cboxFolder.DataSource = this.bindingSourceFolders;
            this.cboxFolder.DisplayMember = "Text";
            this.cboxFolder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxFolder.FormattingEnabled = true;
            this.cboxFolder.Location = new System.Drawing.Point(8, 32);
            this.cboxFolder.Name = "cboxFolder";
            this.cboxFolder.Size = new System.Drawing.Size(176, 25);
            this.cboxFolder.TabIndex = 1;
            this.cboxFolder.ValueMember = "Id";
            // 
            // bindingSourceFolders
            // 
            this.bindingSourceFolders.DataSource = typeof(Technics.Database.Models.FolderModel);
            // 
            // FrmPart
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(376, 225);
            this.Controls.Add(this.lblFolder);
            this.Controls.Add(this.cboxFolder);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.tboxText);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.tboxDescription);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPart";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Деталь";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceFolders)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tboxDescription;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.TextBox tboxText;
        private System.Windows.Forms.Label lblFolder;
        private System.Windows.Forms.ComboBox cboxFolder;
        private System.Windows.Forms.BindingSource bindingSourceFolders;
    }
}