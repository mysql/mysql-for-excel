namespace MySQL.ForExcel
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchemaSelectionPanel));
      System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Schemas");
      System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("System Schemas");
      this.lblConnectionName = new System.Windows.Forms.Label();
      this.lblUserIP = new System.Windows.Forms.Label();
      this.picAddInLogo = new System.Windows.Forms.PictureBox();
      this.schemasContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.selectDatabaseSchemaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.largeImages = new System.Windows.Forms.ImageList(this.components);
      this.smallImages = new System.Windows.Forms.ImageList(this.components);
      this.btnBack = new System.Windows.Forms.Button();
      this.btnNext = new System.Windows.Forms.Button();
      this.lblInstructions = new System.Windows.Forms.Label();
      this.btnHelp = new System.Windows.Forms.Button();
      this.databaseList = new TreeViewTest.MyTreeView();
      this.schemaFilter = new MySQL.ForExcel.Controls.SearchEdit();
      this.createNewSchema = new MySQL.ForExcel.Controls.HotLabel();
      this.hotLabel1 = new MySQL.ForExcel.Controls.HotLabel();
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogo)).BeginInit();
      this.schemasContextMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // lblConnectionName
      // 
      this.lblConnectionName.AutoSize = true;
      this.lblConnectionName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblConnectionName.Location = new System.Drawing.Point(84, 17);
      this.lblConnectionName.Name = "lblConnectionName";
      this.lblConnectionName.Size = new System.Drawing.Size(121, 16);
      this.lblConnectionName.TabIndex = 0;
      this.lblConnectionName.Text = "Connection Name";
      // 
      // lblUserIP
      // 
      this.lblUserIP.AutoSize = true;
      this.lblUserIP.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblUserIP.Location = new System.Drawing.Point(84, 38);
      this.lblUserIP.Name = "lblUserIP";
      this.lblUserIP.Size = new System.Drawing.Size(91, 15);
      this.lblUserIP.TabIndex = 1;
      this.lblUserIP.Text = "User: ??, IP: ??";
      // 
      // picAddInLogo
      // 
      this.picAddInLogo.Image = global::MySQL.ForExcel.Properties.Resources.mysql_header_img;
      this.picAddInLogo.Location = new System.Drawing.Point(14, 13);
      this.picAddInLogo.Name = "picAddInLogo";
      this.picAddInLogo.Size = new System.Drawing.Size(64, 64);
      this.picAddInLogo.TabIndex = 13;
      this.picAddInLogo.TabStop = false;
      // 
      // schemasContextMenu
      // 
      this.schemasContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectDatabaseSchemaToolStripMenuItem});
      this.schemasContextMenu.Name = "schemasContextMenu";
      this.schemasContextMenu.Size = new System.Drawing.Size(202, 26);
      // 
      // selectDatabaseSchemaToolStripMenuItem
      // 
      this.selectDatabaseSchemaToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.db_Schema_16x16;
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
      this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnBack.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnBack.Location = new System.Drawing.Point(123, 659);
      this.btnBack.Name = "btnBack";
      this.btnBack.Size = new System.Drawing.Size(75, 25);
      this.btnBack.TabIndex = 7;
      this.btnBack.Text = "< Back";
      this.btnBack.UseVisualStyleBackColor = true;
      this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
      // 
      // btnNext
      // 
      this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnNext.Enabled = false;
      this.btnNext.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnNext.Location = new System.Drawing.Point(204, 659);
      this.btnNext.Name = "btnNext";
      this.btnNext.Size = new System.Drawing.Size(75, 25);
      this.btnNext.TabIndex = 8;
      this.btnNext.Text = "Next >";
      this.btnNext.UseVisualStyleBackColor = true;
      this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
      // 
      // lblInstructions
      // 
      this.lblInstructions.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblInstructions.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.lblInstructions.Location = new System.Drawing.Point(11, 80);
      this.lblInstructions.Name = "lblInstructions";
      this.lblInstructions.Size = new System.Drawing.Size(259, 98);
      this.lblInstructions.TabIndex = 2;
      this.lblInstructions.Text = "Please select the MySQL schema you want to work with. Each schema can hold a coll" +
    "ection of tables that store data, views that hold selected data and routines tha" +
    "t generate data.";
      // 
      // btnHelp
      // 
      this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnHelp.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnHelp.Location = new System.Drawing.Point(14, 659);
      this.btnHelp.Name = "btnHelp";
      this.btnHelp.Size = new System.Drawing.Size(75, 25);
      this.btnHelp.TabIndex = 6;
      this.btnHelp.Text = "Help";
      this.btnHelp.UseVisualStyleBackColor = true;
      this.btnHelp.Visible = false;
      this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
      // 
      // databaseList
      // 
      this.databaseList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.databaseList.CollapsedIcon = global::MySQL.ForExcel.Properties.Resources.ArrowRight;
      this.databaseList.DescriptionColor = System.Drawing.Color.Silver;
      this.databaseList.DescriptionFont = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.databaseList.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
      this.databaseList.ExpandedIcon = global::MySQL.ForExcel.Properties.Resources.ArrowDown;
      this.databaseList.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.databaseList.Indent = 18;
      this.databaseList.ItemHeight = 20;
      this.databaseList.Location = new System.Drawing.Point(14, 254);
      this.databaseList.Name = "databaseList";
      this.databaseList.NodeImages = this.largeImages;
      treeNode1.BackColor = System.Drawing.SystemColors.ControlDark;
      treeNode1.ForeColor = System.Drawing.SystemColors.WindowText;
      treeNode1.Name = "Node0";
      treeNode1.Text = "Schemas";
      treeNode2.BackColor = System.Drawing.SystemColors.ControlDark;
      treeNode2.ForeColor = System.Drawing.SystemColors.WindowText;
      treeNode2.Name = "Node1";
      treeNode2.Text = "System Schemas";
      this.databaseList.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
      this.databaseList.Size = new System.Drawing.Size(265, 338);
      this.databaseList.TabIndex = 23;
      this.databaseList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.databaseList_AfterSelect);
      this.databaseList.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.databaseList_NodeMouseDoubleClick);
      // 
      // schemaFilter
      // 
      this.schemaFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.schemaFilter.BackColor = System.Drawing.SystemColors.Window;
      this.schemaFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.schemaFilter.Location = new System.Drawing.Point(14, 225);
      this.schemaFilter.Name = "schemaFilter";
      this.schemaFilter.NoTextLabel = "Filter Schemas";
      this.schemaFilter.Size = new System.Drawing.Size(265, 23);
      this.schemaFilter.TabIndex = 16;
      this.schemaFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.schemaFilter_KeyDown);
      // 
      // createNewSchema
      // 
      this.createNewSchema.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.createNewSchema.Description = "Add a new Database Schema";
      this.createNewSchema.DescriptionFont = new System.Drawing.Font("Arial", 8.25F);
      this.createNewSchema.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.createNewSchema.HotTracking = true;
      this.createNewSchema.Image = global::MySQL.ForExcel.Properties.Resources.db_Schema_add_32x32;
      this.createNewSchema.ImageSize = new System.Drawing.Size(32, 32);
      this.createNewSchema.Location = new System.Drawing.Point(14, 599);
      this.createNewSchema.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.createNewSchema.Name = "createNewSchema";
      this.createNewSchema.Size = new System.Drawing.Size(259, 45);
      this.createNewSchema.TabIndex = 15;
      this.createNewSchema.Title = "Create New Schema";
      this.createNewSchema.Click += new System.EventHandler(this.createNewSchema_Click);
      // 
      // hotLabel1
      // 
      this.hotLabel1.Description = "Then click the [Next>] button below";
      this.hotLabel1.DescriptionFont = new System.Drawing.Font("Arial", 8.25F);
      this.hotLabel1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hotLabel1.HotTracking = false;
      this.hotLabel1.Image = global::MySQL.ForExcel.Properties.Resources.db_Schema_many_32x32;
      this.hotLabel1.ImageSize = new System.Drawing.Size(32, 32);
      this.hotLabel1.Location = new System.Drawing.Point(14, 182);
      this.hotLabel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.hotLabel1.Name = "hotLabel1";
      this.hotLabel1.Size = new System.Drawing.Size(244, 44);
      this.hotLabel1.TabIndex = 14;
      this.hotLabel1.Title = "Select a Database Schema";
      // 
      // SchemaSelectionPanel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.databaseList);
      this.Controls.Add(this.schemaFilter);
      this.Controls.Add(this.createNewSchema);
      this.Controls.Add(this.hotLabel1);
      this.Controls.Add(this.btnHelp);
      this.Controls.Add(this.lblInstructions);
      this.Controls.Add(this.btnNext);
      this.Controls.Add(this.btnBack);
      this.Controls.Add(this.lblConnectionName);
      this.Controls.Add(this.lblUserIP);
      this.Controls.Add(this.picAddInLogo);
      this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "SchemaSelectionPanel";
      this.Size = new System.Drawing.Size(296, 700);
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogo)).EndInit();
      this.schemasContextMenu.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblConnectionName;
    private System.Windows.Forms.Label lblUserIP;
    private System.Windows.Forms.PictureBox picAddInLogo;
    public System.Windows.Forms.Button btnBack;
    public System.Windows.Forms.Button btnNext;
    private System.Windows.Forms.ImageList smallImages;
    private System.Windows.Forms.ImageList largeImages;
    private System.Windows.Forms.Label lblInstructions;
    public System.Windows.Forms.Button btnHelp;
    private System.Windows.Forms.ContextMenuStrip schemasContextMenu;
    private System.Windows.Forms.ToolStripMenuItem selectDatabaseSchemaToolStripMenuItem;
    private Controls.HotLabel hotLabel1;
    private Controls.HotLabel createNewSchema;
    private Controls.SearchEdit schemaFilter;
    private TreeViewTest.MyTreeView databaseList;
  }
}
