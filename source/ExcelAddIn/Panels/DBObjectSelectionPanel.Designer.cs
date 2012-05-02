namespace MySQL.ExcelAddIn
{
  partial class DBObjectSelectionPanel
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
      System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Tables", System.Windows.Forms.HorizontalAlignment.Left);
      System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Views", System.Windows.Forms.HorizontalAlignment.Left);
      System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Routines", System.Windows.Forms.HorizontalAlignment.Left);
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DBObjectSelectionPanel));
      this.lblConnectionName = new System.Windows.Forms.Label();
      this.lblUserIP = new System.Windows.Forms.Label();
      this.picAddInLogo = new System.Windows.Forms.PictureBox();
      this.lisDBObjects = new System.Windows.Forms.ListView();
      this.colDBObjectName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.colDBObjectInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.dbObjectsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.importDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.editDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.appendDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.largeImages = new System.Windows.Forms.ImageList(this.components);
      this.smallImages = new System.Windows.Forms.ImageList(this.components);
      this.btnClose = new System.Windows.Forms.Button();
      this.btnBack = new System.Windows.Forms.Button();
      this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
      this.linSeparator = new Microsoft.VisualBasic.PowerPacks.LineShape();
      this.btnHelp = new System.Windows.Forms.Button();
      this.appendData = new MySQL.ExcelAddIn.Controls.HotLabel();
      this.editData = new MySQL.ExcelAddIn.Controls.HotLabel();
      this.importData = new MySQL.ExcelAddIn.Controls.HotLabel();
      this.hotLabel2 = new MySQL.ExcelAddIn.Controls.HotLabel();
      this.exportToNewTable = new MySQL.ExcelAddIn.Controls.HotLabel();
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogo)).BeginInit();
      this.dbObjectsContextMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // lblConnectionName
      // 
      this.lblConnectionName.AutoSize = true;
      this.lblConnectionName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblConnectionName.Location = new System.Drawing.Point(98, 31);
      this.lblConnectionName.Name = "lblConnectionName";
      this.lblConnectionName.Size = new System.Drawing.Size(121, 16);
      this.lblConnectionName.TabIndex = 1;
      this.lblConnectionName.Text = "Connection Name";
      // 
      // lblUserIP
      // 
      this.lblUserIP.AutoSize = true;
      this.lblUserIP.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblUserIP.Location = new System.Drawing.Point(100, 51);
      this.lblUserIP.Name = "lblUserIP";
      this.lblUserIP.Size = new System.Drawing.Size(91, 15);
      this.lblUserIP.TabIndex = 2;
      this.lblUserIP.Text = "User: ??, IP: ??";
      // 
      // picAddInLogo
      // 
      this.picAddInLogo.Image = global::MySQL.ExcelAddIn.Properties.Resources.MainLogo;
      this.picAddInLogo.Location = new System.Drawing.Point(16, 14);
      this.picAddInLogo.Name = "picAddInLogo";
      this.picAddInLogo.Size = new System.Drawing.Size(75, 74);
      this.picAddInLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.picAddInLogo.TabIndex = 13;
      this.picAddInLogo.TabStop = false;
      // 
      // lisDBObjects
      // 
      this.lisDBObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lisDBObjects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDBObjectName,
            this.colDBObjectInfo});
      this.lisDBObjects.ContextMenuStrip = this.dbObjectsContextMenu;
      this.lisDBObjects.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lisDBObjects.FullRowSelect = true;
      listViewGroup1.Header = "Tables";
      listViewGroup1.Name = "grpTables";
      listViewGroup2.Header = "Views";
      listViewGroup2.Name = "grpViews";
      listViewGroup3.Header = "Routines";
      listViewGroup3.Name = "grpRoutines";
      this.lisDBObjects.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
      this.lisDBObjects.HideSelection = false;
      this.lisDBObjects.LargeImageList = this.largeImages;
      this.lisDBObjects.Location = new System.Drawing.Point(16, 214);
      this.lisDBObjects.MultiSelect = false;
      this.lisDBObjects.Name = "lisDBObjects";
      this.lisDBObjects.Size = new System.Drawing.Size(298, 262);
      this.lisDBObjects.SmallImageList = this.smallImages;
      this.lisDBObjects.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.lisDBObjects.TabIndex = 5;
      this.lisDBObjects.UseCompatibleStateImageBehavior = false;
      this.lisDBObjects.View = System.Windows.Forms.View.Tile;
      this.lisDBObjects.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lisDBObjects_ItemSelectionChanged);
      // 
      // colDBObjectName
      // 
      this.colDBObjectName.Text = "Name";
      // 
      // colDBObjectInfo
      // 
      this.colDBObjectInfo.Text = "Info";
      // 
      // dbObjectsContextMenu
      // 
      this.dbObjectsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importDataToolStripMenuItem,
            this.editDataToolStripMenuItem,
            this.appendDataToolStripMenuItem});
      this.dbObjectsContextMenu.Name = "dbObjectsContextMenu";
      this.dbObjectsContextMenu.Size = new System.Drawing.Size(219, 70);
      this.dbObjectsContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.dbObjectsContextMenu_Opening);
      this.dbObjectsContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.dbObjectsContextMenu_ItemClicked);
      // 
      // importDataToolStripMenuItem
      // 
      this.importDataToolStripMenuItem.Image = global::MySQL.ExcelAddIn.Properties.Resources.import_data_16x16;
      this.importDataToolStripMenuItem.Name = "importDataToolStripMenuItem";
      this.importDataToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
      this.importDataToolStripMenuItem.Text = "Import MySQL Data";
      // 
      // editDataToolStripMenuItem
      // 
      this.editDataToolStripMenuItem.Image = global::MySQL.ExcelAddIn.Properties.Resources.edit_data_16x16;
      this.editDataToolStripMenuItem.Name = "editDataToolStripMenuItem";
      this.editDataToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
      this.editDataToolStripMenuItem.Text = "Edit MySQL Data";
      this.editDataToolStripMenuItem.Visible = false;
      // 
      // appendDataToolStripMenuItem
      // 
      this.appendDataToolStripMenuItem.Image = global::MySQL.ExcelAddIn.Properties.Resources.export_excel_existing_table_16x16;
      this.appendDataToolStripMenuItem.Name = "appendDataToolStripMenuItem";
      this.appendDataToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
      this.appendDataToolStripMenuItem.Text = "Append Excel Data to Table";
      this.appendDataToolStripMenuItem.Visible = false;
      // 
      // largeImages
      // 
      this.largeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("largeImages.ImageStream")));
      this.largeImages.TransparentColor = System.Drawing.Color.Transparent;
      this.largeImages.Images.SetKeyName(0, "db.Table.32x32.png");
      this.largeImages.Images.SetKeyName(1, "db.View.32x32.png");
      this.largeImages.Images.SetKeyName(2, "db.Routine.32x32.png");
      // 
      // smallImages
      // 
      this.smallImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("smallImages.ImageStream")));
      this.smallImages.TransparentColor = System.Drawing.Color.Transparent;
      this.smallImages.Images.SetKeyName(0, "db.Table.16x16.png");
      this.smallImages.Images.SetKeyName(1, "db.View.16x16.png");
      this.smallImages.Images.SetKeyName(2, "db.Routine.16x16.png");
      // 
      // btnClose
      // 
      this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnClose.Location = new System.Drawing.Point(229, 651);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(87, 27);
      this.btnClose.TabIndex = 11;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnBack
      // 
      this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnBack.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnBack.Location = new System.Drawing.Point(136, 651);
      this.btnBack.Name = "btnBack";
      this.btnBack.Size = new System.Drawing.Size(87, 27);
      this.btnBack.TabIndex = 10;
      this.btnBack.Text = "< Back";
      this.btnBack.UseVisualStyleBackColor = true;
      this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
      // 
      // shapeContainer1
      // 
      this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
      this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
      this.shapeContainer1.Name = "shapeContainer1";
      this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.linSeparator});
      this.shapeContainer1.Size = new System.Drawing.Size(335, 697);
      this.shapeContainer1.TabIndex = 0;
      this.shapeContainer1.TabStop = false;
      // 
      // linSeparator
      // 
      this.linSeparator.BorderColor = System.Drawing.SystemColors.ControlDark;
      this.linSeparator.BorderWidth = 2;
      this.linSeparator.Name = "linSeparator";
      this.linSeparator.X1 = 16;
      this.linSeparator.X2 = 271;
      this.linSeparator.Y1 = 150;
      this.linSeparator.Y2 = 150;
      // 
      // btnHelp
      // 
      this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnHelp.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnHelp.Location = new System.Drawing.Point(17, 651);
      this.btnHelp.Name = "btnHelp";
      this.btnHelp.Size = new System.Drawing.Size(87, 27);
      this.btnHelp.TabIndex = 9;
      this.btnHelp.Text = "Help";
      this.btnHelp.UseVisualStyleBackColor = true;
      this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
      // 
      // appendData
      // 
      this.appendData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.appendData.Description = "Add data to an existing MySQL Table";
      this.appendData.DescriptionFont = new System.Drawing.Font("Arial", 8.25F);
      this.appendData.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.appendData.HotTracking = true;
      this.appendData.Image = global::MySQL.ExcelAddIn.Properties.Resources.export_excel_existing_table_32x32;
      this.appendData.ImageSize = new System.Drawing.Size(32, 32);
      this.appendData.Location = new System.Drawing.Point(17, 587);
      this.appendData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.appendData.Name = "appendData";
      this.appendData.Size = new System.Drawing.Size(258, 44);
      this.appendData.TabIndex = 18;
      this.appendData.Title = "Append Excel Data to Table";
      this.appendData.Click += new System.EventHandler(this.appendData_Click);
      // 
      // editData
      // 
      this.editData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.editData.Description = "Open a new sheet to edit object\'s data";
      this.editData.DescriptionFont = new System.Drawing.Font("Arial", 8.25F);
      this.editData.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.editData.HotTracking = true;
      this.editData.Image = global::MySQL.ExcelAddIn.Properties.Resources.edit_data_32x32;
      this.editData.ImageSize = new System.Drawing.Size(32, 32);
      this.editData.Location = new System.Drawing.Point(17, 535);
      this.editData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.editData.Name = "editData";
      this.editData.Size = new System.Drawing.Size(258, 44);
      this.editData.TabIndex = 17;
      this.editData.Title = "Edit MySQL Data";
      this.editData.Click += new System.EventHandler(this.editData_Click);
      // 
      // importData
      // 
      this.importData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.importData.Description = "Add object\'s data at a given range of cells";
      this.importData.DescriptionFont = new System.Drawing.Font("Arial", 8.25F);
      this.importData.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.importData.HotTracking = true;
      this.importData.Image = global::MySQL.ExcelAddIn.Properties.Resources.import_data_32x32;
      this.importData.ImageSize = new System.Drawing.Size(32, 32);
      this.importData.Location = new System.Drawing.Point(17, 483);
      this.importData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.importData.Name = "importData";
      this.importData.Size = new System.Drawing.Size(258, 44);
      this.importData.TabIndex = 16;
      this.importData.Title = "Import MySQL Data";
      this.importData.Click += new System.EventHandler(this.importData_Click);
      // 
      // hotLabel2
      // 
      this.hotLabel2.Description = "Then click on an action item below";
      this.hotLabel2.DescriptionFont = new System.Drawing.Font("Arial", 8.25F);
      this.hotLabel2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hotLabel2.HotTracking = false;
      this.hotLabel2.Image = global::MySQL.ExcelAddIn.Properties.Resources.db_Objects_32x32;
      this.hotLabel2.ImageSize = new System.Drawing.Size(32, 32);
      this.hotLabel2.Location = new System.Drawing.Point(16, 163);
      this.hotLabel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.hotLabel2.Name = "hotLabel2";
      this.hotLabel2.Size = new System.Drawing.Size(258, 44);
      this.hotLabel2.TabIndex = 15;
      this.hotLabel2.Title = "Select a Database Object";
      // 
      // exportToNewTable
      // 
      this.exportToNewTable.Description = "Create a new table and fill it with data";
      this.exportToNewTable.DescriptionFont = new System.Drawing.Font("Arial", 8.25F);
      this.exportToNewTable.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.exportToNewTable.HotTracking = true;
      this.exportToNewTable.Image = global::MySQL.ExcelAddIn.Properties.Resources.export_excel_new_table_32x32;
      this.exportToNewTable.ImageSize = new System.Drawing.Size(32, 32);
      this.exportToNewTable.Location = new System.Drawing.Point(17, 98);
      this.exportToNewTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.exportToNewTable.Name = "exportToNewTable";
      this.exportToNewTable.Size = new System.Drawing.Size(258, 44);
      this.exportToNewTable.TabIndex = 14;
      this.exportToNewTable.Title = "Export Excel Data to New Table";
      this.exportToNewTable.Click += new System.EventHandler(this.exportToNewTable_Click);
      // 
      // DBObjectSelectionPanel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.appendData);
      this.Controls.Add(this.editData);
      this.Controls.Add(this.importData);
      this.Controls.Add(this.hotLabel2);
      this.Controls.Add(this.exportToNewTable);
      this.Controls.Add(this.btnHelp);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.btnBack);
      this.Controls.Add(this.lisDBObjects);
      this.Controls.Add(this.lblConnectionName);
      this.Controls.Add(this.lblUserIP);
      this.Controls.Add(this.picAddInLogo);
      this.Controls.Add(this.shapeContainer1);
      this.Font = new System.Drawing.Font("Arial", 9F);
      this.Name = "DBObjectSelectionPanel";
      this.Size = new System.Drawing.Size(335, 697);
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogo)).EndInit();
      this.dbObjectsContextMenu.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblConnectionName;
    private System.Windows.Forms.Label lblUserIP;
    private System.Windows.Forms.PictureBox picAddInLogo;
    private System.Windows.Forms.ListView lisDBObjects;
    public System.Windows.Forms.Button btnClose;
    public System.Windows.Forms.Button btnBack;
    private System.Windows.Forms.ImageList smallImages;
    private System.Windows.Forms.ImageList largeImages;
    private System.Windows.Forms.ColumnHeader colDBObjectName;
    private System.Windows.Forms.ColumnHeader colDBObjectInfo;
    private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
    private Microsoft.VisualBasic.PowerPacks.LineShape linSeparator;
    public System.Windows.Forms.Button btnHelp;
    private System.Windows.Forms.ContextMenuStrip dbObjectsContextMenu;
    private System.Windows.Forms.ToolStripMenuItem importDataToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem editDataToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem appendDataToolStripMenuItem;
    private Controls.HotLabel exportToNewTable;
    private Controls.HotLabel hotLabel2;
    private Controls.HotLabel importData;
    private Controls.HotLabel editData;
    private Controls.HotLabel appendData;
  }
}
