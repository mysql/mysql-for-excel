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
      this.lblConnectionName = new System.Windows.Forms.Label();
      this.lblUserIP = new System.Windows.Forms.Label();
      this.picAddInLogo = new System.Windows.Forms.PictureBox();
      this.largeImages = new System.Windows.Forms.ImageList(this.components);
      this.btnClose = new System.Windows.Forms.Button();
      this.btnBack = new System.Windows.Forms.Button();
      this.btnHelp = new System.Windows.Forms.Button();
      this.objectFilter = new MySQL.ForExcel.SearchEdit();
      this.objectList = new MySQL.ForExcel.MyTreeView();
      this.appendDataLabel = new MySQL.ForExcel.HotLabel();
      this.editDataLabel = new MySQL.ForExcel.HotLabel();
      this.importDataLabel = new MySQL.ForExcel.HotLabel();
      this.selectDatabaseObjectLabel = new MySQL.ForExcel.HotLabel();
      this.exportToNewTableLabel = new MySQL.ForExcel.HotLabel();
      this.imgSeparator = new MySQL.ForExcel.TransparentPictureBox();
      this.labelsToolTip = new System.Windows.Forms.ToolTip(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogo)).BeginInit();
      this.SuspendLayout();
      // 
      // lblConnectionName
      // 
      this.lblConnectionName.AutoEllipsis = true;
      this.lblConnectionName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblConnectionName.Location = new System.Drawing.Point(61, 18);
      this.lblConnectionName.Name = "lblConnectionName";
      this.lblConnectionName.Size = new System.Drawing.Size(190, 18);
      this.lblConnectionName.TabIndex = 1;
      this.lblConnectionName.Text = "Connection Name";
      this.lblConnectionName.Paint += new System.Windows.Forms.PaintEventHandler(this.label_Paint);
      // 
      // lblUserIP
      // 
      this.lblUserIP.AutoEllipsis = true;
      this.lblUserIP.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblUserIP.Location = new System.Drawing.Point(61, 36);
      this.lblUserIP.Name = "lblUserIP";
      this.lblUserIP.Size = new System.Drawing.Size(190, 18);
      this.lblUserIP.TabIndex = 2;
      this.lblUserIP.Text = "User: ??, IP: ??";
      this.lblUserIP.Paint += new System.Windows.Forms.PaintEventHandler(this.label_Paint);
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
      this.btnClose.Location = new System.Drawing.Point(176, 597);
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
      this.btnBack.Location = new System.Drawing.Point(95, 597);
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
      this.btnHelp.Location = new System.Drawing.Point(14, 597);
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
      this.objectFilter.Size = new System.Drawing.Size(242, 21);
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
      this.objectList.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.objectList.ImageHorizontalPixelsOffset = 14;
      this.objectList.ImageToTextHorizontalPixelsOffset = 3;
      this.objectList.Indent = 18;
      this.objectList.ItemHeight = 10;
      this.objectList.Location = new System.Drawing.Point(9, 198);
      this.objectList.Name = "objectList";
      this.objectList.NodeHeightMultiple = 3;
      this.objectList.NodeImages = this.largeImages;
      this.objectList.ShowNodeToolTips = true;
      this.objectList.Size = new System.Drawing.Size(242, 275);
      this.objectList.TabIndex = 24;
      this.objectList.TitleColorOpacity = 0.8D;
      this.objectList.TitleTextVerticalPixelsOffset = 0;
      this.objectList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.objectList_AfterSelect);
      // 
      // appendDataLabel
      // 
      this.appendDataLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.appendDataLabel.Description = "Add data to an existing MySQL Table";
      this.appendDataLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.appendDataLabel.DescriptionColorOpacity = 0.6D;
      this.appendDataLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.appendDataLabel.DescriptionShadowOpacity = 0.4D;
      this.appendDataLabel.DescriptionShadowPixelsXOffset = 0;
      this.appendDataLabel.DescriptionShadowPixelsYOffset = 1;
      this.appendDataLabel.DisabledImage = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_AppendData_Disabled_24x24;
      this.appendDataLabel.DrawShadow = true;
      this.appendDataLabel.Enabled = false;
      this.appendDataLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.appendDataLabel.HotTracking = true;
      this.appendDataLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_AppendData_24x24;
      this.appendDataLabel.ImagePixelsXOffset = 0;
      this.appendDataLabel.ImagePixelsYOffset = 1;
      this.appendDataLabel.Location = new System.Drawing.Point(9, 556);
      this.appendDataLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.appendDataLabel.Name = "appendDataLabel";
      this.appendDataLabel.Size = new System.Drawing.Size(237, 28);
      this.appendDataLabel.TabIndex = 18;
      this.appendDataLabel.Title = "Append Excel Data to Table";
      this.appendDataLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.appendDataLabel.TitleColorOpacity = 0.95D;
      this.appendDataLabel.TitleDescriptionPixelsSpacing = 0;
      this.appendDataLabel.TitlePixelsXOffset = 3;
      this.appendDataLabel.TitlePixelsYOffset = 0;
      this.appendDataLabel.TitleShadowOpacity = 0.2D;
      this.appendDataLabel.TitleShadowPixelsXOffset = 0;
      this.appendDataLabel.TitleShadowPixelsYOffset = 1;
      this.appendDataLabel.Click += new System.EventHandler(this.appendData_Click);
      // 
      // editDataLabel
      // 
      this.editDataLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.editDataLabel.Description = "Open a new sheet to edit table data";
      this.editDataLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.editDataLabel.DescriptionColorOpacity = 0.6D;
      this.editDataLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.editDataLabel.DescriptionShadowOpacity = 0.4D;
      this.editDataLabel.DescriptionShadowPixelsXOffset = 0;
      this.editDataLabel.DescriptionShadowPixelsYOffset = 1;
      this.editDataLabel.DisabledImage = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_EditData_Disabled_24x24;
      this.editDataLabel.DrawShadow = true;
      this.editDataLabel.Enabled = false;
      this.editDataLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.editDataLabel.HotTracking = true;
      this.editDataLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_EditData_24x24;
      this.editDataLabel.ImagePixelsXOffset = 0;
      this.editDataLabel.ImagePixelsYOffset = 1;
      this.editDataLabel.Location = new System.Drawing.Point(9, 518);
      this.editDataLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.editDataLabel.Name = "editDataLabel";
      this.editDataLabel.Size = new System.Drawing.Size(237, 28);
      this.editDataLabel.TabIndex = 17;
      this.editDataLabel.Title = "Edit MySQL Data";
      this.editDataLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.editDataLabel.TitleColorOpacity = 0.95D;
      this.editDataLabel.TitleDescriptionPixelsSpacing = 0;
      this.editDataLabel.TitlePixelsXOffset = 3;
      this.editDataLabel.TitlePixelsYOffset = 0;
      this.editDataLabel.TitleShadowOpacity = 0.2D;
      this.editDataLabel.TitleShadowPixelsXOffset = 0;
      this.editDataLabel.TitleShadowPixelsYOffset = 1;
      this.editDataLabel.Click += new System.EventHandler(this.editData_Click);
      // 
      // importDataLabel
      // 
      this.importDataLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.importDataLabel.Description = "Add object\'s data at the current cell";
      this.importDataLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.importDataLabel.DescriptionColorOpacity = 0.6D;
      this.importDataLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.importDataLabel.DescriptionShadowOpacity = 0.4D;
      this.importDataLabel.DescriptionShadowPixelsXOffset = 0;
      this.importDataLabel.DescriptionShadowPixelsYOffset = 1;
      this.importDataLabel.DisabledImage = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_ImportData_Disabled_24x24;
      this.importDataLabel.DrawShadow = true;
      this.importDataLabel.Enabled = false;
      this.importDataLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.importDataLabel.HotTracking = true;
      this.importDataLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_ImportData_24x24;
      this.importDataLabel.ImagePixelsXOffset = 0;
      this.importDataLabel.ImagePixelsYOffset = 2;
      this.importDataLabel.Location = new System.Drawing.Point(9, 480);
      this.importDataLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.importDataLabel.Name = "importDataLabel";
      this.importDataLabel.Size = new System.Drawing.Size(237, 28);
      this.importDataLabel.TabIndex = 16;
      this.importDataLabel.Title = "Import MySQL Data";
      this.importDataLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.importDataLabel.TitleColorOpacity = 0.95D;
      this.importDataLabel.TitleDescriptionPixelsSpacing = 0;
      this.importDataLabel.TitlePixelsXOffset = 3;
      this.importDataLabel.TitlePixelsYOffset = 0;
      this.importDataLabel.TitleShadowOpacity = 0.2D;
      this.importDataLabel.TitleShadowPixelsXOffset = 0;
      this.importDataLabel.TitleShadowPixelsYOffset = 1;
      this.importDataLabel.Click += new System.EventHandler(this.importData_Click);
      // 
      // selectDatabaseObjectLabel
      // 
      this.selectDatabaseObjectLabel.Description = "Then click on an action item below";
      this.selectDatabaseObjectLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.selectDatabaseObjectLabel.DescriptionColorOpacity = 0.6D;
      this.selectDatabaseObjectLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.selectDatabaseObjectLabel.DescriptionShadowOpacity = 0.4D;
      this.selectDatabaseObjectLabel.DescriptionShadowPixelsXOffset = 0;
      this.selectDatabaseObjectLabel.DescriptionShadowPixelsYOffset = 1;
      this.selectDatabaseObjectLabel.DisabledImage = null;
      this.selectDatabaseObjectLabel.DrawShadow = true;
      this.selectDatabaseObjectLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.selectDatabaseObjectLabel.HotTracking = false;
      this.selectDatabaseObjectLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_SelectObject_24x24;
      this.selectDatabaseObjectLabel.ImagePixelsXOffset = 0;
      this.selectDatabaseObjectLabel.ImagePixelsYOffset = 2;
      this.selectDatabaseObjectLabel.Location = new System.Drawing.Point(9, 136);
      this.selectDatabaseObjectLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.selectDatabaseObjectLabel.Name = "selectDatabaseObjectLabel";
      this.selectDatabaseObjectLabel.Size = new System.Drawing.Size(237, 28);
      this.selectDatabaseObjectLabel.TabIndex = 15;
      this.selectDatabaseObjectLabel.Title = "Select a Database Object";
      this.selectDatabaseObjectLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.selectDatabaseObjectLabel.TitleColorOpacity = 0.95D;
      this.selectDatabaseObjectLabel.TitleDescriptionPixelsSpacing = 0;
      this.selectDatabaseObjectLabel.TitlePixelsXOffset = 3;
      this.selectDatabaseObjectLabel.TitlePixelsYOffset = 0;
      this.selectDatabaseObjectLabel.TitleShadowOpacity = 0.2D;
      this.selectDatabaseObjectLabel.TitleShadowPixelsXOffset = 0;
      this.selectDatabaseObjectLabel.TitleShadowPixelsYOffset = 1;
      // 
      // exportToNewTableLabel
      // 
      this.exportToNewTableLabel.Description = "Create a new table and fill it with data";
      this.exportToNewTableLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.exportToNewTableLabel.DescriptionColorOpacity = 0.6D;
      this.exportToNewTableLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.exportToNewTableLabel.DescriptionShadowOpacity = 0.4D;
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
      this.exportToNewTableLabel.Size = new System.Drawing.Size(237, 28);
      this.exportToNewTableLabel.TabIndex = 14;
      this.exportToNewTableLabel.Title = "Export Excel Data to New Table";
      this.exportToNewTableLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.exportToNewTableLabel.TitleColorOpacity = 0.95D;
      this.exportToNewTableLabel.TitleDescriptionPixelsSpacing = 0;
      this.exportToNewTableLabel.TitlePixelsXOffset = 3;
      this.exportToNewTableLabel.TitlePixelsYOffset = 0;
      this.exportToNewTableLabel.TitleShadowOpacity = 0.2D;
      this.exportToNewTableLabel.TitleShadowPixelsXOffset = 0;
      this.exportToNewTableLabel.TitleShadowPixelsYOffset = 1;
      this.exportToNewTableLabel.Click += new System.EventHandler(this.exportToNewTable_Click);
      // 
      // imgSeparator
      // 
      this.imgSeparator.BackColor = System.Drawing.Color.Transparent;
      this.imgSeparator.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Separator;
      this.imgSeparator.Location = new System.Drawing.Point(9, 108);
      this.imgSeparator.Name = "imgSeparator";
      this.imgSeparator.Opacity = 0.3F;
      this.imgSeparator.Size = new System.Drawing.Size(237, 22);
      this.imgSeparator.TabIndex = 27;
      // 
      // DBObjectSelectionPanel
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add(this.imgSeparator);
      this.Controls.Add(this.lblUserIP);
      this.Controls.Add(this.objectFilter);
      this.Controls.Add(this.appendDataLabel);
      this.Controls.Add(this.objectList);
      this.Controls.Add(this.importDataLabel);
      this.Controls.Add(this.selectDatabaseObjectLabel);
      this.Controls.Add(this.btnHelp);
      this.Controls.Add(this.editDataLabel);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.btnBack);
      this.Controls.Add(this.lblConnectionName);
      this.Controls.Add(this.exportToNewTableLabel);
      this.Controls.Add(this.picAddInLogo);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.InheritSystemFontToControls = false;
      this.Name = "DBObjectSelectionPanel";
      this.Size = new System.Drawing.Size(260, 625);
      this.UseSystemFont = false;
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogo)).EndInit();
      this.ResumeLayout(false);

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
    private HotLabel selectDatabaseObjectLabel;
    private HotLabel importDataLabel;
    private HotLabel editDataLabel;
    private HotLabel appendDataLabel;
    private MyTreeView objectList;
    private SearchEdit objectFilter;
    private TransparentPictureBox imgSeparator;
    private System.Windows.Forms.ToolTip labelsToolTip;
  }
}
