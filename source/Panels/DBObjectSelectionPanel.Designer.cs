namespace MySQL.ForExcel
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
      this.largeImages = new System.Windows.Forms.ImageList(this.components);
      this.btnClose = new System.Windows.Forms.Button();
      this.btnBack = new System.Windows.Forms.Button();
      this.btnHelp = new System.Windows.Forms.Button();
      this.objectFilter = new MySQL.ForExcel.SearchEdit();
      this.objectList = new MySQL.ForExcel.MyTreeView();
      this.appendData = new MySQL.ForExcel.HotLabel();
      this.editData = new MySQL.ForExcel.HotLabel();
      this.importData = new MySQL.ForExcel.HotLabel();
      this.hotLabel2 = new MySQL.ForExcel.HotLabel();
      this.exportToNewTableLabel = new MySQL.ForExcel.HotLabel();
      this.picSeparator = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogo)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picSeparator)).BeginInit();
      this.SuspendLayout();
      // 
      // lblConnectionName
      // 
      this.lblConnectionName.AutoSize = true;
      this.lblConnectionName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblConnectionName.Location = new System.Drawing.Point(61, 18);
      this.lblConnectionName.Name = "lblConnectionName";
      this.lblConnectionName.Size = new System.Drawing.Size(118, 17);
      this.lblConnectionName.TabIndex = 1;
      this.lblConnectionName.Text = "Connection Name";
      // 
      // lblUserIP
      // 
      this.lblUserIP.AutoSize = true;
      this.lblUserIP.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblUserIP.Location = new System.Drawing.Point(61, 36);
      this.lblUserIP.Name = "lblUserIP";
      this.lblUserIP.Size = new System.Drawing.Size(77, 13);
      this.lblUserIP.TabIndex = 2;
      this.lblUserIP.Text = "User: ??, IP: ??";
      // 
      // picAddInLogo
      // 
      this.picAddInLogo.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Logo_64x64;
      this.picAddInLogo.Location = new System.Drawing.Point(9, 11);
      this.picAddInLogo.Name = "picAddInLogo";
      this.picAddInLogo.Size = new System.Drawing.Size(64, 64);
      this.picAddInLogo.TabIndex = 13;
      this.picAddInLogo.TabStop = false;
      // 
      // largeImages
      // 
      this.largeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("largeImages.ImageStream")));
      this.largeImages.TransparentColor = System.Drawing.Color.Transparent;
      this.largeImages.Images.SetKeyName(0, "MySQLforExcel-ObjectPanel-ListItem-Table-24x24.png");
      this.largeImages.Images.SetKeyName(1, "MySQLforExcel-ObjectPanel-ListItem-View-24x24.png");
      this.largeImages.Images.SetKeyName(2, "MySQLforExcel-ObjectPanel-ListItem-Routine-24x24.png");
      // 
      // btnClose
      // 
      this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnClose.Location = new System.Drawing.Point(166, 597);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(75, 23);
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
      this.btnBack.Size = new System.Drawing.Size(75, 23);
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
      this.btnHelp.Size = new System.Drawing.Size(75, 23);
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
      this.objectFilter.Location = new System.Drawing.Point(9, 171);
      this.objectFilter.Name = "objectFilter";
      this.objectFilter.NoTextLabel = "Filter Schema Objects";
      this.objectFilter.Size = new System.Drawing.Size(232, 21);
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
      this.objectList.DescriptionColorOpacity = 1D;
      this.objectList.DescriptionFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.objectList.DescriptionTextVerticalPixelsOffset = 0;
      this.objectList.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
      this.objectList.ExpandedIcon = global::MySQL.ForExcel.Properties.Resources.ArrowDown;
      this.objectList.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.objectList.ImageHorizontalPixelsOffset = 14;
      this.objectList.ImageToTextHorizontalPixelsOffset = 3;
      this.objectList.Indent = 18;
      this.objectList.ItemHeight = 20;
      this.objectList.Location = new System.Drawing.Point(9, 198);
      this.objectList.Name = "objectList";
      this.objectList.NodeHeightMultiple = 2;
      this.objectList.NodeImages = this.largeImages;
      treeNode1.BackColor = System.Drawing.SystemColors.ControlLight;
      treeNode1.ForeColor = System.Drawing.SystemColors.WindowText;
      treeNode1.Name = "TablesNode";
      treeNode1.Text = "Tables";
      treeNode2.BackColor = System.Drawing.SystemColors.ControlLight;
      treeNode2.ForeColor = System.Drawing.SystemColors.WindowText;
      treeNode2.Name = "ViewsNode";
      treeNode2.Text = "Views";
      treeNode3.BackColor = System.Drawing.SystemColors.ControlLight;
      treeNode3.ForeColor = System.Drawing.SystemColors.WindowText;
      treeNode3.Name = "RoutinesNode";
      treeNode3.Text = "Routines";
      this.objectList.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
      this.objectList.Size = new System.Drawing.Size(232, 275);
      this.objectList.TabIndex = 24;
      this.objectList.TitleColorOpacity = 0.8D;
      this.objectList.TitleTextVerticalPixelsOffset = 0;
      this.objectList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.objectList_AfterSelect);
      // 
      // appendData
      // 
      this.appendData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.appendData.Description = "Add data to an existing MySQL Table";
      this.appendData.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.appendData.DescriptionColorOpacity = 0.6D;
      this.appendData.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.appendData.DescriptionShadowOpacity = 0.8D;
      this.appendData.DescriptionShadowPixelsXOffset = 0;
      this.appendData.DescriptionShadowPixelsYOffset = 1;
      this.appendData.DisabledImage = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_AppendData_Disabled_24x24;
      this.appendData.DrawShadow = true;
      this.appendData.Enabled = false;
      this.appendData.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.appendData.HotTracking = true;
      this.appendData.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_AppendData_24x24;
      this.appendData.ImagePixelsXOffset = 0;
      this.appendData.ImagePixelsYOffset = 1;
      this.appendData.Location = new System.Drawing.Point(9, 556);
      this.appendData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.appendData.Name = "appendData";
      this.appendData.Size = new System.Drawing.Size(232, 28);
      this.appendData.TabIndex = 18;
      this.appendData.Title = "Append Excel Data to Table";
      this.appendData.TitleColor = System.Drawing.SystemColors.WindowText;
      this.appendData.TitleColorOpacity = 0.8D;
      this.appendData.TitleDescriptionPixelsSpacing = 0;
      this.appendData.TitlePixelsXOffset = 0;
      this.appendData.TitlePixelsYOffset = 0;
      this.appendData.TitleShadowOpacity = 0.3D;
      this.appendData.TitleShadowPixelsXOffset = 0;
      this.appendData.TitleShadowPixelsYOffset = 1;
      this.appendData.Click += new System.EventHandler(this.appendData_Click);
      // 
      // editData
      // 
      this.editData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.editData.Description = "Open a new sheet to edit table data";
      this.editData.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.editData.DescriptionColorOpacity = 0.6D;
      this.editData.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.editData.DescriptionShadowOpacity = 0.8D;
      this.editData.DescriptionShadowPixelsXOffset = 0;
      this.editData.DescriptionShadowPixelsYOffset = 1;
      this.editData.DisabledImage = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_EditData_Disabled_24x24;
      this.editData.DrawShadow = true;
      this.editData.Enabled = false;
      this.editData.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.editData.HotTracking = true;
      this.editData.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_EditData_24x24;
      this.editData.ImagePixelsXOffset = 0;
      this.editData.ImagePixelsYOffset = 1;
      this.editData.Location = new System.Drawing.Point(9, 518);
      this.editData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.editData.Name = "editData";
      this.editData.Size = new System.Drawing.Size(232, 28);
      this.editData.TabIndex = 17;
      this.editData.Title = "Edit MySQL Data";
      this.editData.TitleColor = System.Drawing.SystemColors.WindowText;
      this.editData.TitleColorOpacity = 0.8D;
      this.editData.TitleDescriptionPixelsSpacing = 0;
      this.editData.TitlePixelsXOffset = 0;
      this.editData.TitlePixelsYOffset = 0;
      this.editData.TitleShadowOpacity = 0.3D;
      this.editData.TitleShadowPixelsXOffset = 0;
      this.editData.TitleShadowPixelsYOffset = 1;
      this.editData.Click += new System.EventHandler(this.editData_Click);
      // 
      // importData
      // 
      this.importData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.importData.Description = "Add object\'s data at the current cell";
      this.importData.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.importData.DescriptionColorOpacity = 0.6D;
      this.importData.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.importData.DescriptionShadowOpacity = 0.8D;
      this.importData.DescriptionShadowPixelsXOffset = 0;
      this.importData.DescriptionShadowPixelsYOffset = 1;
      this.importData.DisabledImage = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_ImportData_Disabled_24x24;
      this.importData.DrawShadow = true;
      this.importData.Enabled = false;
      this.importData.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.importData.HotTracking = true;
      this.importData.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_ImportData_24x24;
      this.importData.ImagePixelsXOffset = 0;
      this.importData.ImagePixelsYOffset = 2;
      this.importData.Location = new System.Drawing.Point(9, 480);
      this.importData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.importData.Name = "importData";
      this.importData.Size = new System.Drawing.Size(232, 28);
      this.importData.TabIndex = 16;
      this.importData.Title = "Import MySQL Data";
      this.importData.TitleColor = System.Drawing.SystemColors.WindowText;
      this.importData.TitleColorOpacity = 0.8D;
      this.importData.TitleDescriptionPixelsSpacing = 0;
      this.importData.TitlePixelsXOffset = 0;
      this.importData.TitlePixelsYOffset = 0;
      this.importData.TitleShadowOpacity = 0.3D;
      this.importData.TitleShadowPixelsXOffset = 0;
      this.importData.TitleShadowPixelsYOffset = 1;
      this.importData.Click += new System.EventHandler(this.importData_Click);
      // 
      // hotLabel2
      // 
      this.hotLabel2.Description = "Then click on an action item below";
      this.hotLabel2.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.hotLabel2.DescriptionColorOpacity = 0.6D;
      this.hotLabel2.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hotLabel2.DescriptionShadowOpacity = 0.8D;
      this.hotLabel2.DescriptionShadowPixelsXOffset = 0;
      this.hotLabel2.DescriptionShadowPixelsYOffset = 1;
      this.hotLabel2.DisabledImage = null;
      this.hotLabel2.DrawShadow = true;
      this.hotLabel2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hotLabel2.HotTracking = false;
      this.hotLabel2.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_SelectObject_24x24;
      this.hotLabel2.ImagePixelsXOffset = 0;
      this.hotLabel2.ImagePixelsYOffset = 2;
      this.hotLabel2.Location = new System.Drawing.Point(9, 136);
      this.hotLabel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.hotLabel2.Name = "hotLabel2";
      this.hotLabel2.Size = new System.Drawing.Size(232, 28);
      this.hotLabel2.TabIndex = 15;
      this.hotLabel2.Title = "Select a Database Object";
      this.hotLabel2.TitleColor = System.Drawing.SystemColors.WindowText;
      this.hotLabel2.TitleColorOpacity = 0.8D;
      this.hotLabel2.TitleDescriptionPixelsSpacing = 0;
      this.hotLabel2.TitlePixelsXOffset = 0;
      this.hotLabel2.TitlePixelsYOffset = 0;
      this.hotLabel2.TitleShadowOpacity = 0.3D;
      this.hotLabel2.TitleShadowPixelsXOffset = 0;
      this.hotLabel2.TitleShadowPixelsYOffset = 1;
      // 
      // exportToNewTableLabel
      // 
      this.exportToNewTableLabel.Description = "Create a new table and fill it with data";
      this.exportToNewTableLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.exportToNewTableLabel.DescriptionColorOpacity = 0.6D;
      this.exportToNewTableLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.exportToNewTableLabel.DescriptionShadowOpacity = 0.8D;
      this.exportToNewTableLabel.DescriptionShadowPixelsXOffset = 0;
      this.exportToNewTableLabel.DescriptionShadowPixelsYOffset = 1;
      this.exportToNewTableLabel.DisabledImage = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_ExportToMySQL_Disabled_24x24;
      this.exportToNewTableLabel.DrawShadow = true;
      this.exportToNewTableLabel.Enabled = false;
      this.exportToNewTableLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.exportToNewTableLabel.HotTracking = true;
      this.exportToNewTableLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_ExportToMySQL_24x24;
      this.exportToNewTableLabel.ImagePixelsXOffset = 0;
      this.exportToNewTableLabel.ImagePixelsYOffset = 0;
      this.exportToNewTableLabel.Location = new System.Drawing.Point(9, 73);
      this.exportToNewTableLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.exportToNewTableLabel.Name = "exportToNewTableLabel";
      this.exportToNewTableLabel.Size = new System.Drawing.Size(232, 28);
      this.exportToNewTableLabel.TabIndex = 14;
      this.exportToNewTableLabel.Title = "Export Excel Data to New Table";
      this.exportToNewTableLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.exportToNewTableLabel.TitleColorOpacity = 0.8D;
      this.exportToNewTableLabel.TitleDescriptionPixelsSpacing = 0;
      this.exportToNewTableLabel.TitlePixelsXOffset = 0;
      this.exportToNewTableLabel.TitlePixelsYOffset = 0;
      this.exportToNewTableLabel.TitleShadowOpacity = 0.3D;
      this.exportToNewTableLabel.TitleShadowPixelsXOffset = 0;
      this.exportToNewTableLabel.TitleShadowPixelsYOffset = 1;
      this.exportToNewTableLabel.Click += new System.EventHandler(this.exportToNewTable_Click);
      // 
      // picSeparator
      // 
      this.picSeparator.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Separator;
      this.picSeparator.Location = new System.Drawing.Point(9, 108);
      this.picSeparator.Name = "picSeparator";
      this.picSeparator.Size = new System.Drawing.Size(232, 21);
      this.picSeparator.TabIndex = 27;
      this.picSeparator.TabStop = false;
      // 
      // DBObjectSelectionPanel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.lblUserIP);
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
      this.Controls.Add(this.exportToNewTableLabel);
      this.Controls.Add(this.picAddInLogo);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "DBObjectSelectionPanel";
      this.Size = new System.Drawing.Size(250, 625);
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogo)).EndInit();
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
    private System.Windows.Forms.ImageList largeImages;
    public System.Windows.Forms.Button btnHelp;
    private HotLabel exportToNewTableLabel;
    private HotLabel hotLabel2;
    private HotLabel importData;
    private HotLabel editData;
    private HotLabel appendData;
    private MyTreeView objectList;
    private SearchEdit objectFilter;
    private System.Windows.Forms.PictureBox picSeparator;
  }
}
