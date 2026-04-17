namespace Technics.Forms
{
    partial class FrmTech
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
            this.cboxState = new System.Windows.Forms.CheckBox();
            this.lblText = new System.Windows.Forms.Label();
            this.tboxText = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tboxDescription = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cboxState
            // 
            this.cboxState.AutoSize = true;
            this.cboxState.Location = new System.Drawing.Point(8, 120);
            this.cboxState.Margin = new System.Windows.Forms.Padding(4);
            this.cboxState.Name = "cboxState";
            this.cboxState.Size = new System.Drawing.Size(359, 23);
            this.cboxState.TabIndex = 4;
            this.cboxState.Text = "Архив (больше не используется или утилизировано)";
            this.cboxState.UseVisualStyleBackColor = true;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(8, 8);
            this.lblText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(42, 19);
            this.lblText.TabIndex = 0;
            this.lblText.Text = "Текст";
            // 
            // tboxText
            // 
            this.tboxText.Location = new System.Drawing.Point(8, 32);
            this.tboxText.Margin = new System.Windows.Forms.Padding(4);
            this.tboxText.Name = "tboxText";
            this.tboxText.Size = new System.Drawing.Size(360, 25);
            this.tboxText.TabIndex = 1;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(8, 64);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(72, 19);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "Описание";
            // 
            // tboxDescription
            // 
            this.tboxDescription.Location = new System.Drawing.Point(8, 88);
            this.tboxDescription.Margin = new System.Windows.Forms.Padding(4);
            this.tboxDescription.Name = "tboxDescription";
            this.tboxDescription.Size = new System.Drawing.Size(360, 25);
            this.tboxDescription.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(288, 152);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 32);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(200, 152);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 32);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // FrmTech
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(376, 193);
            this.Controls.Add(this.cboxState);
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
            this.Name = "FrmTech";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Техника";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cboxState;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.TextBox tboxText;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tboxDescription;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
    }
}