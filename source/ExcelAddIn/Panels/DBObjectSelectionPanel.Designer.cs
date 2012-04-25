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
      this.infSelectDBObject = new MySQL.ExcelAddIn.Controls.InfolLabel();
      this.infExportDataNewTable = new MySQL.ExcelAddIn.Controls.InfolLabel();
      this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
      this.linSeparator = new Microsoft.VisualBasic.PowerPacks.LineShape();
      this.btnHelp = new System.Windows.Forms.Button();
      this.infImportData = new MySQL.ExcelAddIn.Controls.InfolLabel();
      this.infEditData = new MySQL.ExcelAddIn.Controls.InfolLabel();
      this.infAppendData = new MySQL.ExcelAddIn.Controls.InfolLabel();
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogo)).BeginInit();
      this.dbObjectsContextMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // lblConnectionName
      // 
      this.lblConnectionName.AutoSize = true;
      this.lblConnectionName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblConnectionName.Location = new System.Drawing.Point(84, 27);
      this.lblConnectionName.Name = "lblConnectionName";
      this.lblConnectionName.Size = new System.Drawing.Size(121, 16);
      this.lblConnectionName.TabIndex = 1;
      this.lblConnectionName.Text = "Connection Name";
      // 
      // lblUserIP
      // 
      this.lblUserIP.AutoSize = true;
      this.lblUserIP.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblUserIP.Location = new System.Drawing.Point(84, 53);
      this.lblUserIP.Name = "lblUserIP";
      this.lblUserIP.Size = new System.Drawing.Size(91, 15);
      this.lblUserIP.TabIndex = 2;
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
      // lisDBObjects
      // 
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
      this.lisDBObjects.Location = new System.Drawing.Point(14, 185);
      this.lisDBObjects.MultiSelect = false;
      this.lisDBObjects.Name = "lisDBObjects";
      this.lisDBObjects.Size = new System.Drawing.Size(256, 300);
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
      this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnClose.Location = new System.Drawing.Point(195, 624);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(75, 23);
      this.btnClose.TabIndex = 11;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnBack
      // 
      this.btnBack.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnBack.Location = new System.Drawing.Point(114, 624);
      this.btnBack.Name = "btnBack";
      this.btnBack.Size = new System.Drawing.Size(75, 23);
      this.btnBack.TabIndex = 10;
      this.btnBack.Text = "< Back";
      this.btnBack.UseVisualStyleBackColor = true;
      this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
      // 
      // infSelectDBObject
      // 
      this.infSelectDBObject.Image = global::MySQL.ExcelAddIn.Properties.Resources.db_Objects_32x32;
      this.infSelectDBObject.ImageSize = MySQL.ExcelAddIn.Controls.InfolLabel.PictureSize.W32H32;
      this.infSelectDBObject.InfoText1 = "Then click on an action item below";
      this.infSelectDBObject.InfoText2 = "";
      this.infSelectDBObject.Location = new System.Drawing.Point(14, 141);
      this.infSelectDBObject.MainText = "Select a Database Object";
      this.infSelectDBObject.Name = "infSelectDBObject";
      this.infSelectDBObject.PictureAsButton = true;
      this.infSelectDBObject.PictureEnabled = true;
      this.infSelectDBObject.Size = new System.Drawing.Size(256, 38);
      this.infSelectDBObject.TabIndex = 4;
      // 
      // infExportDataNewTable
      // 
      this.infExportDataNewTable.Image = global::MySQL.ExcelAddIn.Properties.Resources.export_excel_new_table_32x32;
      this.infExportDataNewTable.ImageSize = MySQL.ExcelAddIn.Controls.InfolLabel.PictureSize.W32H32;
      this.infExportDataNewTable.InfoText1 = "Create a new table and fill it with data";
      this.infExportDataNewTable.InfoText2 = "";
      this.infExportDataNewTable.Location = new System.Drawing.Point(14, 82);
      this.infExportDataNewTable.MainText = "Export Excel Data to New Table";
      this.infExportDataNewTable.Name = "infExportDataNewTable";
      this.infExportDataNewTable.PictureAsButton = true;
      this.infExportDataNewTable.PictureEnabled = true;
      this.infExportDataNewTable.Size = new System.Drawing.Size(256, 38);
      this.infExportDataNewTable.TabIndex = 3;
      this.infExportDataNewTable.PictureClick += new System.EventHandler(this.infExportDataNewTable_PictureClick);
      // 
      // shapeContainer1
      // 
      this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
      this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
      this.shapeContainer1.Name = "shapeContainer1";
      this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.linSeparator});
      this.shapeContainer1.Size = new System.Drawing.Size(287, 650);
      this.shapeContainer1.TabIndex = 0;
      this.shapeContainer1.TabStop = false;
      // 
      // linSeparator
      // 
      this.linSeparator.BorderColor = System.Drawing.SystemColors.ControlDark;
      this.linSeparator.BorderWidth = 2;
      this.linSeparator.Name = "linSeparator";
      this.linSeparator.X1 = 15;
      this.linSeparator.X2 = 270;
      this.linSeparator.Y1 = 131;
      this.linSeparator.Y2 = 131;
      // 
      // btnHelp
      // 
      this.btnHelp.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnHelp.Location = new System.Drawing.Point(14, 624);
      this.btnHelp.Name = "btnHelp";
      this.btnHelp.Size = new System.Drawing.Size(75, 23);
      this.btnHelp.TabIndex = 9;
      this.btnHelp.Text = "Help";
      this.btnHelp.UseVisualStyleBackColor = true;
      this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
      // 
      // infImportData
      // 
      this.infImportData.Image = global::MySQL.ExcelAddIn.Properties.Resources.import_data_32x32;
      this.infImportData.ImageSize = MySQL.ExcelAddIn.Controls.InfolLabel.PictureSize.W32H32;
      this.infImportData.InfoText1 = "Add object\'s data at a given range of cells";
      this.infImportData.InfoText2 = "";
      this.infImportData.Location = new System.Drawing.Point(14, 491);
      this.infImportData.MainText = "Import MySQL Data";
      this.infImportData.Name = "infImportData";
      this.infImportData.PictureAsButton = true;
      this.infImportData.PictureEnabled = true;
      this.infImportData.Size = new System.Drawing.Size(256, 38);
      this.infImportData.TabIndex = 6;
      this.infImportData.PictureClick += new System.EventHandler(this.infImportData_PictureClick);
      // 
      // infEditData
      // 
      this.infEditData.Image = global::MySQL.ExcelAddIn.Properties.Resources.edit_data_32x32;
      this.infEditData.ImageSize = MySQL.ExcelAddIn.Controls.InfolLabel.PictureSize.W32H32;
      this.infEditData.InfoText1 = "Open a new sheet to edit object\'s data";
      this.infEditData.InfoText2 = "";
      this.infEditData.Location = new System.Drawing.Point(14, 535);
      this.infEditData.MainText = "Edit MySQL Data";
      this.infEditData.Name = "infEditData";
      this.infEditData.PictureAsButton = true;
      this.infEditData.PictureEnabled = true;
      this.infEditData.Size = new System.Drawing.Size(256, 38);
      this.infEditData.TabIndex = 7;
      this.infEditData.PictureClick += new System.EventHandler(this.infEditData_PictureClick);
      // 
      // infAppendData
      // 
      this.infAppendData.Image = global::MySQL.ExcelAddIn.Properties.Resources.export_excel_existing_table_32x32;
      this.infAppendData.ImageSize = MySQL.ExcelAddIn.Controls.InfolLabel.PictureSize.W32H32;
      this.infAppendData.InfoText1 = "Add data to an existing MySQL Table";
      this.infAppendData.InfoText2 = "";
      this.infAppendData.Location = new System.Drawing.Point(15, 579);
      this.infAppendData.MainText = "Append Excel Data to Table";
      this.infAppendData.Name = "infAppendData";
      this.infAppendData.PictureAsButton = true;
      this.infAppendData.PictureEnabled = true;
      this.infAppendData.Size = new System.Drawing.Size(256, 38);
      this.infAppendData.TabIndex = 8;
      this.infAppendData.PictureClick += new System.EventHandler(this.infAppendData_PictureClick);
      // 
      // DBObjectSelectionPanel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.infAppendData);
      this.Controls.Add(this.infEditData);
      this.Controls.Add(this.infImportData);
      this.Controls.Add(this.btnHelp);
      this.Controls.Add(this.infExportDataNewTable);
      this.Controls.Add(this.infSelectDBObject);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.btnBack);
      this.Controls.Add(this.lisDBObjects);
      this.Controls.Add(this.lblConnectionName);
      this.Controls.Add(this.lblUserIP);
      this.Controls.Add(this.picAddInLogo);
      this.Controls.Add(this.shapeContainer1);
      this.Name = "DBObjectSelectionPanel";
      this.Size = new System.Drawing.Size(287, 650);
      this.VisibleChanged += new System.EventHandler(this.DBObjectSelectionPanel_VisibleChanged);
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
    private Controls.InfolLabel infSelectDBObject;
    private Controls.InfolLabel infExportDataNewTable;
    private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
    private Microsoft.VisualBasic.PowerPacks.LineShape linSeparator;
    public System.Windows.Forms.Button btnHelp;
    private Controls.InfolLabel infImportData;
    private Controls.InfolLabel infEditData;
    private Controls.InfolLabel infAppendData;
    private System.Windows.Forms.ContextMenuStrip dbObjectsContextMenu;
    private System.Windows.Forms.ToolStripMenuItem importDataToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem editDataToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem appendDataToolStripMenuItem;
  }
}
