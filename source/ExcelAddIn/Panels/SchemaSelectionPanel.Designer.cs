namespace MySQL.ExcelAddIn
{
  partial class SchemaSelectionPanel
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Schemas", System.Windows.Forms.HorizontalAlignment.Left);
      System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("System Schemas", System.Windows.Forms.HorizontalAlignment.Left);
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchemaSelectionPanel));
      this.lblConnectionName = new System.Windows.Forms.Label();
      this.lblUserIP = new System.Windows.Forms.Label();
      this.picAddInLogo = new System.Windows.Forms.PictureBox();
      this.lisDatabases = new System.Windows.Forms.ListView();
      this.schemasContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.selectDatabaseSchemaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.largeImages = new System.Windows.Forms.ImageList(this.components);
      this.smallImages = new System.Windows.Forms.ImageList(this.components);
      this.btnBack = new System.Windows.Forms.Button();
      this.btnNext = new System.Windows.Forms.Button();
      this.lblInstructions = new System.Windows.Forms.Label();
      this.infSelectDBSchema = new MySQL.ExcelAddIn.Controls.InfolLabel();
      this.infNewSchema = new MySQL.ExcelAddIn.Controls.InfolLabel();
      this.btnHelp = new System.Windows.Forms.Button();
      this.colSchemaName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.colSchemaInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogo)).BeginInit();
      this.schemasContextMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // lblConnectionName
      // 
      this.lblConnectionName.AutoSize = true;
      this.lblConnectionName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblConnectionName.Location = new System.Drawing.Point(84, 27);
      this.lblConnectionName.Name = "lblConnectionName";
      this.lblConnectionName.Size = new System.Drawing.Size(121, 16);
      this.lblConnectionName.TabIndex = 0;
      this.lblConnectionName.Text = "Connection Name";
      // 
      // lblUserIP
      // 
      this.lblUserIP.AutoSize = true;
      this.lblUserIP.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblUserIP.Location = new System.Drawing.Point(84, 53);
      this.lblUserIP.Name = "lblUserIP";
      this.lblUserIP.Size = new System.Drawing.Size(91, 15);
      this.lblUserIP.TabIndex = 1;
      this.lblUserIP.Text = "User: ??, IP: ??";
      // 
      // picAddInLogo
      // 
      this.picAddInLogo.Image = global::MySQL.ExcelAddIn.Properties.Resources.MainLogo;
      this.picAddInLogo.Location = new System.Drawing.Point(14, 12);
      this.picAddInLogo.Name = "picAddInLogo";
      this.picAddInLogo.Size = new System.Drawing.Size(64, 64);
      this.picAddInLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.picAddInLogo.TabIndex = 13;
      this.picAddInLogo.TabStop = false;
      // 
      // lisDatabases
      // 
      this.lisDatabases.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSchemaName,
            this.colSchemaInfo});
      this.lisDatabases.ContextMenuStrip = this.schemasContextMenu;
      this.lisDatabases.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lisDatabases.FullRowSelect = true;
      listViewGroup1.Header = "Schemas";
      listViewGroup1.Name = "grpUserSchemas";
      listViewGroup2.Header = "System Schemas";
      listViewGroup2.Name = "grpSystemSchemas";
      this.lisDatabases.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
      this.lisDatabases.HideSelection = false;
      this.lisDatabases.LargeImageList = this.largeImages;
      this.lisDatabases.Location = new System.Drawing.Point(14, 208);
      this.lisDatabases.MultiSelect = false;
      this.lisDatabases.Name = "lisDatabases";
      this.lisDatabases.Size = new System.Drawing.Size(256, 343);
      this.lisDatabases.SmallImageList = this.smallImages;
      this.lisDatabases.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.lisDatabases.TabIndex = 4;
      this.lisDatabases.UseCompatibleStateImageBehavior = false;
      this.lisDatabases.View = System.Windows.Forms.View.Tile;
      this.lisDatabases.ItemActivate += new System.EventHandler(this.lisDatabases_ItemActivate);
      this.lisDatabases.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lisDatabases_ItemSelectionChanged);
      // 
      // schemasContextMenu
      // 
      this.schemasContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectDatabaseSchemaToolStripMenuItem});
      this.schemasContextMenu.Name = "schemasContextMenu";
      this.schemasContextMenu.Size = new System.Drawing.Size(202, 26);
      this.schemasContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.schemasContextMenu_Opening);
      this.schemasContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.schemasContextMenu_ItemClicked);
      // 
      // selectDatabaseSchemaToolStripMenuItem
      // 
      this.selectDatabaseSchemaToolStripMenuItem.Image = global::MySQL.ExcelAddIn.Properties.Resources.db_Schema_16x16;
      this.selectDatabaseSchemaToolStripMenuItem.Name = "selectDatabaseSchemaToolStripMenuItem";
      this.selectDatabaseSchemaToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
      this.selectDatabaseSchemaToolStripMenuItem.Text = "Select Database Schema";
      // 
      // largeImages
      // 
      this.largeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("largeImages.ImageStream")));
      this.largeImages.TransparentColor = System.Drawing.Color.Transparent;
      this.largeImages.Images.SetKeyName(0, "db.Schema.32x32.png");
      // 
      // smallImages
      // 
      this.smallImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("smallImages.ImageStream")));
      this.smallImages.TransparentColor = System.Drawing.Color.Transparent;
      this.smallImages.Images.SetKeyName(0, "db.Schema.16x16.png");
      // 
      // btnBack
      // 
      this.btnBack.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnBack.Location = new System.Drawing.Point(114, 624);
      this.btnBack.Name = "btnBack";
      this.btnBack.Size = new System.Drawing.Size(75, 23);
      this.btnBack.TabIndex = 7;
      this.btnBack.Text = "< Back";
      this.btnBack.UseVisualStyleBackColor = true;
      this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
      // 
      // btnNext
      // 
      this.btnNext.Enabled = false;
      this.btnNext.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnNext.Location = new System.Drawing.Point(195, 624);
      this.btnNext.Name = "btnNext";
      this.btnNext.Size = new System.Drawing.Size(75, 23);
      this.btnNext.TabIndex = 8;
      this.btnNext.Text = "Next >";
      this.btnNext.UseVisualStyleBackColor = true;
      this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
      // 
      // lblInstructions
      // 
      this.lblInstructions.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblInstructions.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.lblInstructions.Location = new System.Drawing.Point(11, 83);
      this.lblInstructions.Name = "lblInstructions";
      this.lblInstructions.Size = new System.Drawing.Size(259, 83);
      this.lblInstructions.TabIndex = 2;
      this.lblInstructions.Text = "Please select the MySQL schema you want to work with. Each schema can hold a coll" +
    "ection of tables that store data, views that hold selected data and routines tha" +
    "t generate data.";
      // 
      // infSelectDBSchema
      // 
      this.infSelectDBSchema.Image = global::MySQL.ExcelAddIn.Properties.Resources.db_Schema_many_32x32;
      this.infSelectDBSchema.ImageSize = MySQL.ExcelAddIn.Controls.InfolLabel.PictureSize.W32H32;
      this.infSelectDBSchema.InfoText1 = "Then click the [Next>] button below";
      this.infSelectDBSchema.InfoText2 = "";
      this.infSelectDBSchema.Location = new System.Drawing.Point(14, 169);
      this.infSelectDBSchema.MainText = "Select a Database Schema";
      this.infSelectDBSchema.Name = "infSelectDBSchema";
      this.infSelectDBSchema.PictureAsButton = true;
      this.infSelectDBSchema.PictureEnabled = true;
      this.infSelectDBSchema.Size = new System.Drawing.Size(256, 38);
      this.infSelectDBSchema.TabIndex = 3;
      // 
      // infNewSchema
      // 
      this.infNewSchema.Image = global::MySQL.ExcelAddIn.Properties.Resources.db_Schema_add_32x32;
      this.infNewSchema.ImageSize = MySQL.ExcelAddIn.Controls.InfolLabel.PictureSize.W32H32;
      this.infNewSchema.InfoText1 = "Add a new Database Schema";
      this.infNewSchema.InfoText2 = "";
      this.infNewSchema.Location = new System.Drawing.Point(14, 557);
      this.infNewSchema.MainText = "Create New Schema";
      this.infNewSchema.Name = "infNewSchema";
      this.infNewSchema.PictureAsButton = true;
      this.infNewSchema.PictureEnabled = true;
      this.infNewSchema.Size = new System.Drawing.Size(256, 38);
      this.infNewSchema.TabIndex = 5;
      this.infNewSchema.PictureClick += new System.EventHandler(this.infNewSchema_PictureClick);
      // 
      // btnHelp
      // 
      this.btnHelp.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnHelp.Location = new System.Drawing.Point(14, 624);
      this.btnHelp.Name = "btnHelp";
      this.btnHelp.Size = new System.Drawing.Size(75, 23);
      this.btnHelp.TabIndex = 6;
      this.btnHelp.Text = "Help";
      this.btnHelp.UseVisualStyleBackColor = true;
      this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
      // 
      // colSchemaName
      // 
      this.colSchemaName.Text = "Schema Name";
      // 
      // colSchemaInfo
      // 
      this.colSchemaInfo.Text = "Info";
      // 
      // SchemaSelectionPanel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.btnHelp);
      this.Controls.Add(this.infNewSchema);
      this.Controls.Add(this.infSelectDBSchema);
      this.Controls.Add(this.lblInstructions);
      this.Controls.Add(this.btnNext);
      this.Controls.Add(this.btnBack);
      this.Controls.Add(this.lisDatabases);
      this.Controls.Add(this.lblConnectionName);
      this.Controls.Add(this.lblUserIP);
      this.Controls.Add(this.picAddInLogo);
      this.Name = "SchemaSelectionPanel";
      this.Size = new System.Drawing.Size(287, 650);
      this.VisibleChanged += new System.EventHandler(this.SchemaSelectionPanel_VisibleChanged);
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogo)).EndInit();
      this.schemasContextMenu.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblConnectionName;
    private System.Windows.Forms.Label lblUserIP;
    private System.Windows.Forms.PictureBox picAddInLogo;
    private System.Windows.Forms.ListView lisDatabases;
    public System.Windows.Forms.Button btnBack;
    public System.Windows.Forms.Button btnNext;
    private System.Windows.Forms.ImageList smallImages;
    private System.Windows.Forms.ImageList largeImages;
    private System.Windows.Forms.Label lblInstructions;
    private Controls.InfolLabel infSelectDBSchema;
    private Controls.InfolLabel infNewSchema;
    public System.Windows.Forms.Button btnHelp;
    private System.Windows.Forms.ContextMenuStrip schemasContextMenu;
    private System.Windows.Forms.ToolStripMenuItem selectDatabaseSchemaToolStripMenuItem;
    private System.Windows.Forms.ColumnHeader colSchemaName;
    private System.Windows.Forms.ColumnHeader colSchemaInfo;
  }
}
