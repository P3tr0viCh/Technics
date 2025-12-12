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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.LabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitterLeft = new System.Windows.Forms.Splitter();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.tvTechs = new System.Windows.Forms.TreeView();
            this.menuTechs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miTechAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.miTechAddFolder2 = new System.Windows.Forms.ToolStripMenuItem();
            this.miTechAddTech2 = new System.Windows.Forms.ToolStripMenuItem();
            this.miTechChange = new System.Windows.Forms.ToolStripMenuItem();
            this.miTechDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTechs = new System.Windows.Forms.ToolStrip();
            this.tsbtnTechAdd = new System.Windows.Forms.ToolStripDropDownButton();
            this.miTechAddFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.miTechAddTech = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtnTechChange = new System.Windows.Forms.ToolStripButton();
            this.tsbtnTechDelete = new System.Windows.Forms.ToolStripButton();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.miMainFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList16 = new System.Windows.Forms.ImageList(this.components);
            this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.menuTechs.SuspendLayout();
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
            // LabelStatus
            // 
            this.LabelStatus.Name = "LabelStatus";
            this.LabelStatus.Size = new System.Drawing.Size(47, 19);
            this.LabelStatus.Text = "Status";
            // 
            // splitterLeft
            // 
            this.splitterLeft.Location = new System.Drawing.Point(259, 0);
            this.splitterLeft.MinSize = 160;
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
            this.panelLeft.Size = new System.Drawing.Size(259, 291);
            this.panelLeft.TabIndex = 1;
            // 
            // tvTechs
            // 
            this.tvTechs.ContextMenuStrip = this.menuTechs;
            this.tvTechs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTechs.HideSelection = false;
            this.tvTechs.ImageIndex = 0;
            this.tvTechs.ImageList = this.imageList16;
            this.tvTechs.Location = new System.Drawing.Point(0, 50);
            this.tvTechs.Name = "tvTechs";
            this.tvTechs.SelectedImageIndex = 0;
            this.tvTechs.ShowRootLines = false;
            this.tvTechs.Size = new System.Drawing.Size(259, 241);
            this.tvTechs.TabIndex = 0;
            this.tvTechs.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TvTechs_AfterSelect);
            this.tvTechs.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TvTechs_NodeMouseClick);
            // 
            // menuTechs
            // 
            this.menuTechs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miTechAdd,
            this.miTechChange,
            this.miTechDelete});
            this.menuTechs.Name = "menuTechs";
            this.menuTechs.Size = new System.Drawing.Size(129, 70);
            // 
            // miTechAdd
            // 
            this.miTechAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miTechAddFolder2,
            this.miTechAddTech2});
            this.miTechAdd.Name = "miTechAdd";
            this.miTechAdd.Size = new System.Drawing.Size(128, 22);
            this.miTechAdd.Text = "Добавить";
            // 
            // miTechAddFolder2
            // 
            this.miTechAddFolder2.Name = "miTechAddFolder2";
            this.miTechAddFolder2.Size = new System.Drawing.Size(118, 22);
            this.miTechAddFolder2.Text = "Папка";
            this.miTechAddFolder2.Click += new System.EventHandler(this.MiTechAddFolder2_Click);
            // 
            // miTechAddTech2
            // 
            this.miTechAddTech2.Name = "miTechAddTech2";
            this.miTechAddTech2.Size = new System.Drawing.Size(118, 22);
            this.miTechAddTech2.Text = "Техника";
            this.miTechAddTech2.Click += new System.EventHandler(this.MiTechAddTech2_Click);
            // 
            // miTechChange
            // 
            this.miTechChange.Name = "miTechChange";
            this.miTechChange.Size = new System.Drawing.Size(128, 22);
            this.miTechChange.Text = "Изменить";
            this.miTechChange.Click += new System.EventHandler(this.miTechChange_Click);
            // 
            // miTechDelete
            // 
            this.miTechDelete.Name = "miTechDelete";
            this.miTechDelete.Size = new System.Drawing.Size(128, 22);
            this.miTechDelete.Text = "Удалить";
            this.miTechDelete.Click += new System.EventHandler(this.MiTechDelete_Click);
            // 
            // toolStripTechs
            // 
            this.toolStripTechs.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.toolStripTechs.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTechs.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripTechs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnTechAdd,
            this.tsbtnTechChange,
            this.tsbtnTechDelete});
            this.toolStripTechs.Location = new System.Drawing.Point(0, 0);
            this.toolStripTechs.Name = "toolStripTechs";
            this.toolStripTechs.Size = new System.Drawing.Size(259, 50);
            this.toolStripTechs.TabIndex = 1;
            // 
            // tsbtnTechAdd
            // 
            this.tsbtnTechAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miTechAddFolder,
            this.miTechAddTech});
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
            this.miTechAddFolder.Size = new System.Drawing.Size(180, 24);
            this.miTechAddFolder.Text = "Папка";
            this.miTechAddFolder.Click += new System.EventHandler(this.MiTechAddFolder_Click);
            // 
            // miTechAddTech
            // 
            this.miTechAddTech.Name = "miTechAddTech";
            this.miTechAddTech.Size = new System.Drawing.Size(180, 24);
            this.miTechAddTech.Text = "Техника";
            this.miTechAddTech.Click += new System.EventHandler(this.MiTechAddItem_Click);
            // 
            // tsbtnTechChange
            // 
            this.tsbtnTechChange.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnTechChange.Image")));
            this.tsbtnTechChange.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnTechChange.Name = "tsbtnTechChange";
            this.tsbtnTechChange.Size = new System.Drawing.Size(75, 47);
            this.tsbtnTechChange.Text = "Изменить";
            this.tsbtnTechChange.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbtnTechChange.Click += new System.EventHandler(this.TsbtnTechChange_Click);
            // 
            // tsbtnTechDelete
            // 
            this.tsbtnTechDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnTechDelete.Image")));
            this.tsbtnTechDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnTechDelete.Name = "tsbtnTechDelete";
            this.tsbtnTechDelete.Size = new System.Drawing.Size(64, 47);
            this.tsbtnTechDelete.Text = "Удалить";
            this.tsbtnTechDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbtnTechDelete.Click += new System.EventHandler(this.TsbtnTechDelete_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainFile,
            this.miMainHelp});
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
            // miMainHelp
            // 
            this.miMainHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miHelpAbout});
            this.miMainHelp.Name = "miMainHelp";
            this.miMainHelp.Size = new System.Drawing.Size(74, 23);
            this.miMainHelp.Text = "Справка";
            // 
            // miHelpAbout
            // 
            this.miHelpAbout.Name = "miHelpAbout";
            this.miHelpAbout.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.miHelpAbout.Size = new System.Drawing.Size(188, 24);
            this.miHelpAbout.Text = "О программе";
            this.miHelpAbout.Click += new System.EventHandler(this.MiHelpAbout_Click);
            // 
            // imageList16
            // 
            this.imageList16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList16.ImageStream")));
            this.imageList16.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList16.Images.SetKeyName(0, "folder-custom-16.png");
            this.imageList16.Images.SetKeyName(1, "circle-double-custom-16.png");
            // 
            // Main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(650, 342);
            this.Controls.Add(this.toolStripContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
            this.menuTechs.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem miTechAddTech;
        private System.Windows.Forms.ToolStripStatusLabel LabelStatus;
        private System.Windows.Forms.ToolStripButton tsbtnTechChange;
        private System.Windows.Forms.ToolStripButton tsbtnTechDelete;
        private System.Windows.Forms.ContextMenuStrip menuTechs;
        private System.Windows.Forms.ToolStripMenuItem miTechAdd;
        private System.Windows.Forms.ToolStripMenuItem miTechAddFolder2;
        private System.Windows.Forms.ToolStripMenuItem miTechAddTech2;
        private System.Windows.Forms.ToolStripMenuItem miTechChange;
        private System.Windows.Forms.ToolStripMenuItem miTechDelete;
        private System.Windows.Forms.ToolStripMenuItem miMainHelp;
        private System.Windows.Forms.ToolStripMenuItem miHelpAbout;
        private System.Windows.Forms.ImageList imageList16;
    }
}

