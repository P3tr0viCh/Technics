namespace Technics
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.splitterLeft = new System.Windows.Forms.Splitter();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.tvTechs = new System.Windows.Forms.TreeView();
            this.toolStripTechs = new System.Windows.Forms.ToolStrip();
            this.tsbtnTechAdd = new System.Windows.Forms.ToolStripDropDownButton();
            this.miTechAddFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.miTechAddItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.miMainFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.LabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.toolStripTechs.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.BottomToolStripPanel
            // 
            this.toolStripContainer.BottomToolStripPanel.Controls.Add(this.statusStrip);
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.splitterLeft);
            this.toolStripContainer.ContentPanel.Controls.Add(this.panelLeft);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(650, 291);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(650, 342);
            this.toolStripContainer.TabIndex = 0;
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.menuStrip);
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LabelStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(650, 24);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // splitterLeft
            // 
            this.splitterLeft.Location = new System.Drawing.Point(200, 0);
            this.splitterLeft.Name = "splitterLeft";
            this.splitterLeft.Size = new System.Drawing.Size(8, 291);
            this.splitterLeft.TabIndex = 2;
            this.splitterLeft.TabStop = false;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.tvTechs);
            this.panelLeft.Controls.Add(this.toolStripTechs);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(200, 291);
            this.panelLeft.TabIndex = 1;
            // 
            // tvTechs
            // 
            this.tvTechs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTechs.Location = new System.Drawing.Point(0, 50);
            this.tvTechs.Name = "tvTechs";
            this.tvTechs.ShowRootLines = false;
            this.tvTechs.Size = new System.Drawing.Size(200, 241);
            this.tvTechs.TabIndex = 0;
            // 
            // toolStripTechs
            // 
            this.toolStripTechs.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.toolStripTechs.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTechs.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripTechs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnTechAdd});
            this.toolStripTechs.Location = new System.Drawing.Point(0, 0);
            this.toolStripTechs.Name = "toolStripTechs";
            this.toolStripTechs.Size = new System.Drawing.Size(200, 50);
            this.toolStripTechs.TabIndex = 1;
            // 
            // tsbtnTechAdd
            // 
            this.tsbtnTechAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miTechAddFolder,
            this.miTechAddItem});
            this.tsbtnTechAdd.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnTechAdd.Image")));
            this.tsbtnTechAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnTechAdd.Name = "tsbtnTechAdd";
            this.tsbtnTechAdd.Size = new System.Drawing.Size(83, 47);
            this.tsbtnTechAdd.Text = "Добавить";
            this.tsbtnTechAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // miTechAddFolder
            // 
            this.miTechAddFolder.Name = "miTechAddFolder";
            this.miTechAddFolder.Size = new System.Drawing.Size(128, 24);
            this.miTechAddFolder.Text = "Папка";
            this.miTechAddFolder.Click += new System.EventHandler(this.MiTechAddFolder_Click);
            // 
            // miTechAddItem
            // 
            this.miTechAddItem.Name = "miTechAddItem";
            this.miTechAddItem.Size = new System.Drawing.Size(128, 24);
            this.miTechAddItem.Text = "Техника";
            this.miTechAddItem.Click += new System.EventHandler(this.MiTechAddItem_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainFile});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(650, 27);
            this.menuStrip.TabIndex = 0;
            // 
            // miMainFile
            // 
            this.miMainFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFileClose});
            this.miMainFile.Name = "miMainFile";
            this.miMainFile.Size = new System.Drawing.Size(53, 23);
            this.miMainFile.Text = "Файл";
            // 
            // miFileClose
            // 
            this.miFileClose.Name = "miFileClose";
            this.miFileClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.miFileClose.Size = new System.Drawing.Size(169, 24);
            this.miFileClose.Text = "Выход";
            this.miFileClose.Click += new System.EventHandler(this.MiFileClose_Click);
            // 
            // LabelStatus
            // 
            this.LabelStatus.Name = "LabelStatus";
            this.LabelStatus.Size = new System.Drawing.Size(47, 19);
            this.LabelStatus.Text = "Status";
            // 
            // Main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(650, 342);
            this.Controls.Add(this.toolStripContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main";
            this.Text = "Technics";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.panelLeft.PerformLayout();
            this.toolStripTechs.ResumeLayout(false);
            this.toolStripTechs.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem miMainFile;
        private System.Windows.Forms.ToolStripMenuItem miFileClose;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.TreeView tvTechs;
        private System.Windows.Forms.ToolStrip toolStripTechs;
        private System.Windows.Forms.ToolStripDropDownButton tsbtnTechAdd;
        private System.Windows.Forms.ToolStripMenuItem miTechAddFolder;
        private System.Windows.Forms.Splitter splitterLeft;
        private System.Windows.Forms.ToolStripMenuItem miTechAddItem;
        private System.Windows.Forms.ToolStripStatusLabel LabelStatus;
    }
}

