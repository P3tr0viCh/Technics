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
            this.splitterMileages = new System.Windows.Forms.Splitter();
            this.panelTop = new System.Windows.Forms.Panel();
            this.splitterTechs = new System.Windows.Forms.Splitter();
            this.panelTechs = new System.Windows.Forms.Panel();
            this.tvTechs = new System.Windows.Forms.TreeView();
            this.menuTechs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miTechAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.miTechAddFolder2 = new System.Windows.Forms.ToolStripMenuItem();
            this.miTechAddTech2 = new System.Windows.Forms.ToolStripMenuItem();
            this.miTechChange = new System.Windows.Forms.ToolStripMenuItem();
            this.miTechDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList16 = new System.Windows.Forms.ImageList(this.components);
            this.toolStripTechs = new System.Windows.Forms.ToolStrip();
            this.tsbtnTechAdd = new System.Windows.Forms.ToolStripDropDownButton();
            this.miTechAddFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.miTechAddTech = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtnTechChange = new System.Windows.Forms.ToolStripButton();
            this.tsbtnTechDelete = new System.Windows.Forms.ToolStripButton();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.dgvMileages = new System.Windows.Forms.DataGridView();
            this.MileagesTechText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MileagesDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MileagesMileage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MileagesMileageCommon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MileagesDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSourceMileages = new System.Windows.Forms.BindingSource(this.components);
            this.toolStripMileages = new System.Windows.Forms.ToolStrip();
            this.tsbtnMileagesAdd = new System.Windows.Forms.ToolStripDropDownButton();
            this.miMileagesAddMileage = new System.Windows.Forms.ToolStripMenuItem();
            this.miMileagesAddMilleageTotal = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtnMileagesChange = new System.Windows.Forms.ToolStripButton();
            this.tsbtnMileagesDelete = new System.Windows.Forms.ToolStripButton();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.miMainFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelTechs.SuspendLayout();
            this.menuTechs.SuspendLayout();
            this.toolStripTechs.SuspendLayout();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMileages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceMileages)).BeginInit();
            this.toolStripMileages.SuspendLayout();
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
            this.toolStripContainer.ContentPanel.Controls.Add(this.splitterMileages);
            this.toolStripContainer.ContentPanel.Controls.Add(this.panelTop);
            this.toolStripContainer.ContentPanel.Controls.Add(this.panelBottom);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(688, 382);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(688, 433);
            this.toolStripContainer.TabIndex = 0;
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.menuStrip);
            // 
            // statusStrip
            // 
            this.statusStrip.AutoSize = false;
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LabelStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(688, 24);
            this.statusStrip.TabIndex = 0;
            // 
            // LabelStatus
            // 
            this.LabelStatus.Name = "LabelStatus";
            this.LabelStatus.Size = new System.Drawing.Size(47, 19);
            this.LabelStatus.Text = "Status";
            // 
            // splitterMileages
            // 
            this.splitterMileages.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitterMileages.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterMileages.Location = new System.Drawing.Point(0, 178);
            this.splitterMileages.MinSize = 160;
            this.splitterMileages.Name = "splitterMileages";
            this.splitterMileages.Size = new System.Drawing.Size(688, 8);
            this.splitterMileages.TabIndex = 2;
            this.splitterMileages.TabStop = false;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.splitterTechs);
            this.panelTop.Controls.Add(this.panelTechs);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(688, 186);
            this.panelTop.TabIndex = 1;
            // 
            // splitterTechs
            // 
            this.splitterTechs.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitterTechs.Location = new System.Drawing.Point(260, 0);
            this.splitterTechs.Name = "splitterTechs";
            this.splitterTechs.Size = new System.Drawing.Size(8, 186);
            this.splitterTechs.TabIndex = 3;
            this.splitterTechs.TabStop = false;
            // 
            // panelTechs
            // 
            this.panelTechs.Controls.Add(this.tvTechs);
            this.panelTechs.Controls.Add(this.toolStripTechs);
            this.panelTechs.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelTechs.Location = new System.Drawing.Point(0, 0);
            this.panelTechs.Name = "panelTechs";
            this.panelTechs.Size = new System.Drawing.Size(260, 186);
            this.panelTechs.TabIndex = 4;
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
            this.tvTechs.Size = new System.Drawing.Size(260, 136);
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
            this.miTechChange.Click += new System.EventHandler(this.MiTechChange_Click);
            // 
            // miTechDelete
            // 
            this.miTechDelete.Name = "miTechDelete";
            this.miTechDelete.Size = new System.Drawing.Size(128, 22);
            this.miTechDelete.Text = "Удалить";
            this.miTechDelete.Click += new System.EventHandler(this.MiTechDelete_Click);
            // 
            // imageList16
            // 
            this.imageList16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList16.ImageStream")));
            this.imageList16.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList16.Images.SetKeyName(0, "folder-custom-16.png");
            this.imageList16.Images.SetKeyName(1, "circle-double-custom-16.png");
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
            this.toolStripTechs.ShowItemToolTips = false;
            this.toolStripTechs.Size = new System.Drawing.Size(260, 50);
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
            this.miTechAddFolder.Size = new System.Drawing.Size(128, 24);
            this.miTechAddFolder.Text = "Папка";
            this.miTechAddFolder.Click += new System.EventHandler(this.MiTechAddFolder_Click);
            // 
            // miTechAddTech
            // 
            this.miTechAddTech.Name = "miTechAddTech";
            this.miTechAddTech.Size = new System.Drawing.Size(128, 24);
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
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.dgvMileages);
            this.panelBottom.Controls.Add(this.toolStripMileages);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 186);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(688, 196);
            this.panelBottom.TabIndex = 2;
            // 
            // dgvMileages
            // 
            this.dgvMileages.AllowUserToAddRows = false;
            this.dgvMileages.AllowUserToDeleteRows = false;
            this.dgvMileages.AllowUserToResizeRows = false;
            this.dgvMileages.AutoGenerateColumns = false;
            this.dgvMileages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMileages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMileages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MileagesTechText,
            this.MileagesDateTime,
            this.MileagesMileage,
            this.MileagesMileageCommon,
            this.MileagesDescription});
            this.dgvMileages.DataSource = this.bindingSourceMileages;
            this.dgvMileages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMileages.Location = new System.Drawing.Point(0, 50);
            this.dgvMileages.Name = "dgvMileages";
            this.dgvMileages.ReadOnly = true;
            this.dgvMileages.Size = new System.Drawing.Size(688, 146);
            this.dgvMileages.TabIndex = 3;
            this.dgvMileages.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvMileages_CellMouseDown);
            // 
            // MileagesTechText
            // 
            this.MileagesTechText.DataPropertyName = "TechText";
            this.MileagesTechText.HeaderText = "TechText";
            this.MileagesTechText.Name = "MileagesTechText";
            this.MileagesTechText.ReadOnly = true;
            // 
            // MileagesDateTime
            // 
            this.MileagesDateTime.DataPropertyName = "DateTime";
            this.MileagesDateTime.HeaderText = "DateTime";
            this.MileagesDateTime.Name = "MileagesDateTime";
            this.MileagesDateTime.ReadOnly = true;
            // 
            // MileagesMileage
            // 
            this.MileagesMileage.DataPropertyName = "Mileage";
            this.MileagesMileage.HeaderText = "Mileage";
            this.MileagesMileage.Name = "MileagesMileage";
            this.MileagesMileage.ReadOnly = true;
            // 
            // MileagesMileageCommon
            // 
            this.MileagesMileageCommon.DataPropertyName = "MileageCommon";
            this.MileagesMileageCommon.HeaderText = "MileageCommon";
            this.MileagesMileageCommon.Name = "MileagesMileageCommon";
            this.MileagesMileageCommon.ReadOnly = true;
            // 
            // MileagesDescription
            // 
            this.MileagesDescription.DataPropertyName = "Description";
            this.MileagesDescription.HeaderText = "Description";
            this.MileagesDescription.Name = "MileagesDescription";
            this.MileagesDescription.ReadOnly = true;
            // 
            // bindingSourceMileages
            // 
            this.bindingSourceMileages.DataSource = typeof(Technics.Database.Models.MileageModel);
            this.bindingSourceMileages.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.BindingSourceMileages_ListChanged);
            // 
            // toolStripMileages
            // 
            this.toolStripMileages.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.toolStripMileages.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMileages.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMileages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnMileagesAdd,
            this.tsbtnMileagesChange,
            this.tsbtnMileagesDelete});
            this.toolStripMileages.Location = new System.Drawing.Point(0, 0);
            this.toolStripMileages.Name = "toolStripMileages";
            this.toolStripMileages.ShowItemToolTips = false;
            this.toolStripMileages.Size = new System.Drawing.Size(688, 50);
            this.toolStripMileages.TabIndex = 2;
            // 
            // tsbtnMileagesAdd
            // 
            this.tsbtnMileagesAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMileagesAddMileage,
            this.miMileagesAddMilleageTotal});
            this.tsbtnMileagesAdd.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnMileagesAdd.Image")));
            this.tsbtnMileagesAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnMileagesAdd.Name = "tsbtnMileagesAdd";
            this.tsbtnMileagesAdd.Size = new System.Drawing.Size(83, 47);
            this.tsbtnMileagesAdd.Text = "Добавить";
            this.tsbtnMileagesAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // miMileagesAddMileage
            // 
            this.miMileagesAddMileage.Name = "miMileagesAddMileage";
            this.miMileagesAddMileage.Size = new System.Drawing.Size(180, 24);
            this.miMileagesAddMileage.Text = "Пробег";
            this.miMileagesAddMileage.Click += new System.EventHandler(this.MiMileagesAddMileage_Click);
            // 
            // miMileagesAddMilleageTotal
            // 
            this.miMileagesAddMilleageTotal.Name = "miMileagesAddMilleageTotal";
            this.miMileagesAddMilleageTotal.Size = new System.Drawing.Size(180, 24);
            this.miMileagesAddMilleageTotal.Text = "Общий пробег";
            this.miMileagesAddMilleageTotal.Click += new System.EventHandler(this.MiMileagesAddMilleageTotal_Click);
            // 
            // tsbtnMileagesChange
            // 
            this.tsbtnMileagesChange.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnMileagesChange.Image")));
            this.tsbtnMileagesChange.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnMileagesChange.Name = "tsbtnMileagesChange";
            this.tsbtnMileagesChange.Size = new System.Drawing.Size(75, 47);
            this.tsbtnMileagesChange.Text = "Изменить";
            this.tsbtnMileagesChange.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // tsbtnMileagesDelete
            // 
            this.tsbtnMileagesDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnMileagesDelete.Image")));
            this.tsbtnMileagesDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnMileagesDelete.Name = "tsbtnMileagesDelete";
            this.tsbtnMileagesDelete.Size = new System.Drawing.Size(64, 47);
            this.tsbtnMileagesDelete.Text = "Удалить";
            this.tsbtnMileagesDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbtnMileagesDelete.Click += new System.EventHandler(this.TsbtnMileagesDelete_Click);
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
            this.menuStrip.Size = new System.Drawing.Size(688, 27);
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
            // Main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(688, 433);
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
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTechs.ResumeLayout(false);
            this.panelTechs.PerformLayout();
            this.menuTechs.ResumeLayout(false);
            this.toolStripTechs.ResumeLayout(false);
            this.toolStripTechs.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMileages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceMileages)).EndInit();
            this.toolStripMileages.ResumeLayout(false);
            this.toolStripMileages.PerformLayout();
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
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.TreeView tvTechs;
        private System.Windows.Forms.ToolStrip toolStripTechs;
        private System.Windows.Forms.ToolStripDropDownButton tsbtnTechAdd;
        private System.Windows.Forms.ToolStripMenuItem miTechAddFolder;
        private System.Windows.Forms.Splitter splitterMileages;
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
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Splitter splitterTechs;
        private System.Windows.Forms.ToolStrip toolStripMileages;
        private System.Windows.Forms.ToolStripDropDownButton tsbtnMileagesAdd;
        private System.Windows.Forms.ToolStripMenuItem miMileagesAddMileage;
        private System.Windows.Forms.ToolStripMenuItem miMileagesAddMilleageTotal;
        private System.Windows.Forms.ToolStripButton tsbtnMileagesChange;
        private System.Windows.Forms.ToolStripButton tsbtnMileagesDelete;
        private System.Windows.Forms.DataGridView dgvMileages;
        private System.Windows.Forms.BindingSource bindingSourceMileages;
        private System.Windows.Forms.Panel panelTechs;
        private System.Windows.Forms.DataGridViewTextBoxColumn MileagesTechText;
        private System.Windows.Forms.DataGridViewTextBoxColumn MileagesDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn MileagesMileage;
        private System.Windows.Forms.DataGridViewTextBoxColumn MileagesMileageCommon;
        private System.Windows.Forms.DataGridViewTextBoxColumn MileagesDescription;
    }
}

