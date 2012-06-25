﻿namespace MySQL.ForExcel
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DBObjectSelectionPanel));
      System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Tables");
      System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Views");
      System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Routines");
      this.lblConnectionName = new System.Windows.Forms.Label();
      this.lblUserIP = new System.Windows.Forms.Label();
      this.picAddInLogo = new System.Windows.Forms.PictureBox();
      this.dbObjectsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.importDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.editDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.appendDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.largeImages = new System.Windows.Forms.ImageList(this.components);
      this.smallImages = new System.Windows.Forms.ImageList(this.components);
      this.btnClose = new System.Windows.Forms.Button();
      this.btnBack = new System.Windows.Forms.Button();
      this.btnHelp = new System.Windows.Forms.Button();
      this.objectFilter = new MySQL.ForExcel.Controls.SearchEdit();
      this.objectList = new TreeViewTest.MyTreeView();
      this.appendData = new MySQL.ForExcel.Controls.HotLabel();
      this.editData = new MySQL.ForExcel.Controls.HotLabel();
      this.importData = new MySQL.ForExcel.Controls.HotLabel();
      this.hotLabel2 = new MySQL.ForExcel.Controls.HotLabel();
      this.exportToNewTable = new MySQL.ForExcel.Controls.HotLabel();
      this.picSeparator = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogo)).BeginInit();
      this.dbObjectsContextMenu.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picSeparator)).BeginInit();
      this.SuspendLayout();
      // 
      // lblConnectionName
      // 
      this.lblConnectionName.AutoSize = true;
      this.lblConnectionName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblConnectionName.Location = new System.Drawing.Point(79, 18);
      this.lblConnectionName.Name = "lblConnectionName";
      this.lblConnectionName.Size = new System.Drawing.Size(118, 17);
      this.lblConnectionName.TabIndex = 1;
      this.lblConnectionName.Text = "Connection Name";
      // 
      // lblUserIP
      // 
      this.lblUserIP.AutoSize = true;
      this.lblUserIP.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblUserIP.Location = new System.Drawing.Point(84, 35);
      this.lblUserIP.Name = "lblUserIP";
      this.lblUserIP.Size = new System.Drawing.Size(77, 13);
      this.lblUserIP.TabIndex = 2;
      this.lblUserIP.Text = "User: ??, IP: ??";
      // 
      // picAddInLogo
      // 
      this.picAddInLogo.Image = global::MySQL.ForExcel.Properties.Resources.mysql_header_img;
      this.picAddInLogo.Location = new System.Drawing.Point(9, 12);
      this.picAddInLogo.Name = "picAddInLogo";
      this.picAddInLogo.Size = new System.Drawing.Size(64, 64);
      this.picAddInLogo.TabIndex = 13;
      this.picAddInLogo.TabStop = false;
      // 
      // dbObjectsContextMenu
      // 
      this.dbObjectsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importDataToolStripMenuItem,
            this.editDataToolStripMenuItem,
            this.appendDataToolStripMenuItem});
      this.dbObjectsContextMenu.Name = "dbObjectsContextMenu";
      this.dbObjectsContextMenu.Size = new System.Drawing.Size(219, 70);
      // 
      // importDataToolStripMenuItem
      // 
      this.importDataToolStripMenuItem.Name = "importDataToolStripMenuItem";
      this.importDataToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
      this.importDataToolStripMenuItem.Text = "Import MySQL Data";
      // 
      // editDataToolStripMenuItem
      // 
      this.editDataToolStripMenuItem.Name = "editDataToolStripMenuItem";
      this.editDataToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
      this.editDataToolStripMenuItem.Text = "Edit MySQL Data";
      this.editDataToolStripMenuItem.Visible = false;
      // 
      // appendDataToolStripMenuItem
      // 
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
      this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnClose.Location = new System.Drawing.Point(169, 597);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(75, 25);
      this.btnClose.TabIndex = 11;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnBack
      // 
      this.btnBack.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnBack.Location = new System.Drawing.Point(90, 597);
      this.btnBack.Name = "btnBack";
      this.btnBack.Size = new System.Drawing.Size(75, 25);
      this.btnBack.TabIndex = 10;
      this.btnBack.Text = "< Back";
      this.btnBack.UseVisualStyleBackColor = true;
      this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
      // 
      // btnHelp
      // 
      this.btnHelp.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.btnHelp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnHelp.Location = new System.Drawing.Point(9, 597);
      this.btnHelp.Name = "btnHelp";
      this.btnHelp.Size = new System.Drawing.Size(75, 25);
      this.btnHelp.TabIndex = 9;
      this.btnHelp.Text = "Help";
      this.btnHelp.UseVisualStyleBackColor = true;
      this.btnHelp.Visible = false;
      this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
      // 
      // objectFilter
      // 
      this.objectFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.objectFilter.BackColor = System.Drawing.SystemColors.Window;
      this.objectFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.objectFilter.Location = new System.Drawing.Point(9, 191);
      this.objectFilter.Name = "objectFilter";
      this.objectFilter.NoTextLabel = "Filter Schema Objects";
      this.objectFilter.Size = new System.Drawing.Size(235, 26);
      this.objectFilter.TabIndex = 26;
      this.objectFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.objectFilter_KeyDown);
      // 
      // objectList
      // 
      this.objectList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.objectList.CollapsedIcon = global::MySQL.ForExcel.Properties.Resources.ArrowRight;
      this.objectList.DescriptionColor = System.Drawing.Color.Silver;
      this.objectList.DescriptionFont = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.objectList.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
      this.objectList.ExpandedIcon = global::MySQL.ForExcel.Properties.Resources.ArrowDown;
      this.objectList.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.objectList.Indent = 18;
      this.objectList.ItemHeight = 20;
      this.objectList.Location = new System.Drawing.Point(9, 223);
      this.objectList.Name = "objectList";
      this.objectList.NodeImages = this.largeImages;
      treeNode1.BackColor = System.Drawing.SystemColors.ControlDark;
      treeNode1.ForeColor = System.Drawing.SystemColors.WindowText;
      treeNode1.Name = "Node0";
      treeNode1.Text = "Tables";
      treeNode2.BackColor = System.Drawing.SystemColors.ControlDark;
      treeNode2.ForeColor = System.Drawing.SystemColors.WindowText;
      treeNode2.Name = "Node1";
      treeNode2.Text = "Views";
      treeNode3.BackColor = System.Drawing.SystemColors.ControlDark;
      treeNode3.ForeColor = System.Drawing.SystemColors.WindowText;
      treeNode3.Name = "Node0";
      treeNode3.Text = "Routines";
      this.objectList.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
      this.objectList.Size = new System.Drawing.Size(235, 233);
      this.objectList.TabIndex = 24;
      this.objectList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.objectList_AfterSelect);
      // 
      // appendData
      // 
      this.appendData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.appendData.Description = "Add data to an existing MySQL Table";
      this.appendData.DescriptionColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(151)))), ((int)(((byte)(194)))));
      this.appendData.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.appendData.DescriptionShadowOpacity = 0.3D;
      this.appendData.DrawShadow = true;
      this.appendData.Enabled = false;
      this.appendData.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.appendData.HotTracking = true;
      this.appendData.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_AppendData_32x32;
      this.appendData.ImageSize = new System.Drawing.Size(32, 32);
      this.appendData.Location = new System.Drawing.Point(5, 546);
      this.appendData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.appendData.Name = "appendData";
      this.appendData.Size = new System.Drawing.Size(241, 44);
      this.appendData.TabIndex = 18;
      this.appendData.Title = "Append Excel Data to Table";
      this.appendData.TitleColor = System.Drawing.SystemColors.WindowText;
      this.appendData.TitleShadowOpacity = 0.3D;
      this.appendData.Click += new System.EventHandler(this.appendData_Click);
      // 
      // editData
      // 
      this.editData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.editData.Description = "Open a new sheet to edit table data";
      this.editData.DescriptionColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(151)))), ((int)(((byte)(194)))));
      this.editData.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.editData.DescriptionShadowOpacity = 0.3D;
      this.editData.DrawShadow = true;
      this.editData.Enabled = false;
      this.editData.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.editData.HotTracking = true;
      this.editData.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_EditData_32x32;
      this.editData.ImageSize = new System.Drawing.Size(32, 32);
      this.editData.Location = new System.Drawing.Point(5, 505);
      this.editData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.editData.Name = "editData";
      this.editData.Size = new System.Drawing.Size(241, 44);
      this.editData.TabIndex = 17;
      this.editData.Title = "Edit MySQL Data";
      this.editData.TitleColor = System.Drawing.SystemColors.WindowText;
      this.editData.TitleShadowOpacity = 0.3D;
      this.editData.Click += new System.EventHandler(this.editData_Click);
      // 
      // importData
      // 
      this.importData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.importData.Description = "Add object\'s data at the current cell";
      this.importData.DescriptionColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(151)))), ((int)(((byte)(194)))));
      this.importData.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.importData.DescriptionShadowOpacity = 0.3D;
      this.importData.DrawShadow = true;
      this.importData.Enabled = false;
      this.importData.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.importData.HotTracking = true;
      this.importData.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_ImportData_32x32;
      this.importData.ImageSize = new System.Drawing.Size(32, 32);
      this.importData.Location = new System.Drawing.Point(6, 463);
      this.importData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.importData.Name = "importData";
      this.importData.Size = new System.Drawing.Size(241, 44);
      this.importData.TabIndex = 16;
      this.importData.Title = "Import MySQL Data";
      this.importData.TitleColor = System.Drawing.SystemColors.WindowText;
      this.importData.TitleShadowOpacity = 0.3D;
      this.importData.Click += new System.EventHandler(this.importData_Click);
      // 
      // hotLabel2
      // 
      this.hotLabel2.Description = "Then click on an action item below";
      this.hotLabel2.DescriptionColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(151)))), ((int)(((byte)(194)))));
      this.hotLabel2.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hotLabel2.DescriptionShadowOpacity = 0.3D;
      this.hotLabel2.DrawShadow = true;
      this.hotLabel2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hotLabel2.HotTracking = false;
      this.hotLabel2.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_SelectObject_32x32;
      this.hotLabel2.ImageSize = new System.Drawing.Size(32, 32);
      this.hotLabel2.Location = new System.Drawing.Point(5, 149);
      this.hotLabel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.hotLabel2.Name = "hotLabel2";
      this.hotLabel2.Size = new System.Drawing.Size(241, 44);
      this.hotLabel2.TabIndex = 15;
      this.hotLabel2.Title = "Select a Database Object";
      this.hotLabel2.TitleColor = System.Drawing.SystemColors.WindowText;
      this.hotLabel2.TitleShadowOpacity = 0.3D;
      // 
      // exportToNewTable
      // 
      this.exportToNewTable.Description = "Create a new table and fill it with data";
      this.exportToNewTable.DescriptionColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(151)))), ((int)(((byte)(194)))));
      this.exportToNewTable.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.exportToNewTable.DescriptionShadowOpacity = 0.3D;
      this.exportToNewTable.DrawShadow = true;
      this.exportToNewTable.Enabled = false;
      this.exportToNewTable.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.exportToNewTable.HotTracking = true;
      this.exportToNewTable.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_ExportToMySQL_32x32;
      this.exportToNewTable.ImageSize = new System.Drawing.Size(32, 32);
      this.exportToNewTable.Location = new System.Drawing.Point(5, 83);
      this.exportToNewTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.exportToNewTable.Name = "exportToNewTable";
      this.exportToNewTable.Size = new System.Drawing.Size(241, 44);
      this.exportToNewTable.TabIndex = 14;
      this.exportToNewTable.Title = "Export Excel Data to New Table";
      this.exportToNewTable.TitleColor = System.Drawing.SystemColors.WindowText;
      this.exportToNewTable.TitleShadowOpacity = 0.3D;
      this.exportToNewTable.Click += new System.EventHandler(this.exportToNewTable_Click);
      // 
      // picSeparator
      // 
      this.picSeparator.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_WelcomePanel_Separator_Blue;
      this.picSeparator.Location = new System.Drawing.Point(9, 124);
      this.picSeparator.Name = "picSeparator";
      this.picSeparator.Size = new System.Drawing.Size(235, 21);
      this.picSeparator.TabIndex = 27;
      this.picSeparator.TabStop = false;
      // 
      // DBObjectSelectionPanel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.picSeparator);
      this.Controls.Add(this.objectFilter);
      this.Controls.Add(this.appendData);
      this.Controls.Add(this.objectList);
      this.Controls.Add(this.importData);
      this.Controls.Add(this.hotLabel2);
      this.Controls.Add(this.btnHelp);
      this.Controls.Add(this.editData);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.btnBack);
      this.Controls.Add(this.lblConnectionName);
      this.Controls.Add(this.exportToNewTable);
      this.Controls.Add(this.lblUserIP);
      this.Controls.Add(this.picAddInLogo);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "DBObjectSelectionPanel";
      this.Size = new System.Drawing.Size(250, 625);
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogo)).EndInit();
      this.dbObjectsContextMenu.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.picSeparator)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblConnectionName;
    private System.Windows.Forms.Label lblUserIP;
    private System.Windows.Forms.PictureBox picAddInLogo;
    public System.Windows.Forms.Button btnClose;
    public System.Windows.Forms.Button btnBack;
    private System.Windows.Forms.ImageList smallImages;
    private System.Windows.Forms.ImageList largeImages;
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
    private TreeViewTest.MyTreeView objectList;
    private Controls.SearchEdit objectFilter;
    private System.Windows.Forms.PictureBox picSeparator;
  }
}
