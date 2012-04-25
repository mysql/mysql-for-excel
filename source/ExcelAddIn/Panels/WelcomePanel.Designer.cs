namespace MySQL.ExcelAddIn
{
  partial class WelcomePanel
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
      System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Local Connections", System.Windows.Forms.HorizontalAlignment.Left);
      System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Remote Connections", System.Windows.Forms.HorizontalAlignment.Left);
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomePanel));
      this.lblExcelAddIn = new System.Windows.Forms.Label();
      this.lblWelcome = new System.Windows.Forms.Label();
      this.picAddInLogo = new System.Windows.Forms.PictureBox();
      this.lisConnections = new System.Windows.Forms.ListView();
      this.colConnectionName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.colUserIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.connectionsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.openConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.largeImages = new System.Windows.Forms.ImageList(this.components);
      this.smallImages = new System.Windows.Forms.ImageList(this.components);
      this.lblInstructions = new System.Windows.Forms.Label();
      this.lblCopyright = new System.Windows.Forms.Label();
      this.lblAllRights = new System.Windows.Forms.Label();
      this.infOpenConnection = new MySQL.ExcelAddIn.Controls.InfolLabel();
      this.infNewConnection = new MySQL.ExcelAddIn.Controls.InfolLabel();
      this.infManageConnections = new MySQL.ExcelAddIn.Controls.InfolLabel();
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogo)).BeginInit();
      this.connectionsContextMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // lblExcelAddIn
      // 
      this.lblExcelAddIn.AutoSize = true;
      this.lblExcelAddIn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblExcelAddIn.Location = new System.Drawing.Point(98, 51);
      this.lblExcelAddIn.Name = "lblExcelAddIn";
      this.lblExcelAddIn.Size = new System.Drawing.Size(163, 19);
      this.lblExcelAddIn.TabIndex = 1;
      this.lblExcelAddIn.Text = "MySQL Excel Add-In";
      // 
      // lblWelcome
      // 
      this.lblWelcome.AutoSize = true;
      this.lblWelcome.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblWelcome.Location = new System.Drawing.Point(84, 32);
      this.lblWelcome.Name = "lblWelcome";
      this.lblWelcome.Size = new System.Drawing.Size(108, 16);
      this.lblWelcome.TabIndex = 0;
      this.lblWelcome.Text = "Welcome to the";
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
      // lisConnections
      // 
      this.lisConnections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colConnectionName,
            this.colUserIP});
      this.lisConnections.ContextMenuStrip = this.connectionsContextMenu;
      this.lisConnections.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lisConnections.FullRowSelect = true;
      listViewGroup1.Header = "Local Connections";
      listViewGroup1.Name = "grpLocalConnections";
      listViewGroup2.Header = "Remote Connections";
      listViewGroup2.Name = "grpRemoteConnections";
      this.lisConnections.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
      this.lisConnections.HideSelection = false;
      this.lisConnections.LargeImageList = this.largeImages;
      this.lisConnections.Location = new System.Drawing.Point(14, 208);
      this.lisConnections.MultiSelect = false;
      this.lisConnections.Name = "lisConnections";
      this.lisConnections.Size = new System.Drawing.Size(256, 300);
      this.lisConnections.SmallImageList = this.smallImages;
      this.lisConnections.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.lisConnections.StateImageList = this.largeImages;
      this.lisConnections.TabIndex = 4;
      this.lisConnections.UseCompatibleStateImageBehavior = false;
      this.lisConnections.View = System.Windows.Forms.View.Tile;
      this.lisConnections.ItemActivate += new System.EventHandler(this.lisConnections_ItemActivate);
      // 
      // colConnectionName
      // 
      this.colConnectionName.Text = "Connection Name";
      // 
      // colUserIP
      // 
      this.colUserIP.Text = "User & IP";
      // 
      // connectionsContextMenu
      // 
      this.connectionsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openConnectionToolStripMenuItem});
      this.connectionsContextMenu.Name = "connectionsContextMenu";
      this.connectionsContextMenu.Size = new System.Drawing.Size(169, 26);
      this.connectionsContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.connectionsContextMenu_Opening);
      this.connectionsContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.connectionsContextMenu_ItemClicked);
      // 
      // openConnectionToolStripMenuItem
      // 
      this.openConnectionToolStripMenuItem.Image = global::MySQL.ExcelAddIn.Properties.Resources.db_mgmt_Connection_16x16;
      this.openConnectionToolStripMenuItem.Name = "openConnectionToolStripMenuItem";
      this.openConnectionToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
      this.openConnectionToolStripMenuItem.Text = "Open Connection";
      // 
      // largeImages
      // 
      this.largeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("largeImages.ImageStream")));
      this.largeImages.TransparentColor = System.Drawing.Color.Transparent;
      this.largeImages.Images.SetKeyName(0, "db.mgmt.Connection.32x32.png");
      // 
      // smallImages
      // 
      this.smallImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("smallImages.ImageStream")));
      this.smallImages.TransparentColor = System.Drawing.Color.Transparent;
      this.smallImages.Images.SetKeyName(0, "db.mgmt.Connection.16x16.png");
      // 
      // lblInstructions
      // 
      this.lblInstructions.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblInstructions.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.lblInstructions.Location = new System.Drawing.Point(11, 83);
      this.lblInstructions.Name = "lblInstructions";
      this.lblInstructions.Size = new System.Drawing.Size(259, 83);
      this.lblInstructions.TabIndex = 2;
      this.lblInstructions.Text = "The MySQL Excel Add-In allows you to work with the MySQL Database right from with" +
    "in the MS Office Excel application. Excel is a powerful tool for data analysis a" +
    "nd editing.";
      // 
      // lblCopyright
      // 
      this.lblCopyright.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblCopyright.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.lblCopyright.Location = new System.Drawing.Point(12, 614);
      this.lblCopyright.Name = "lblCopyright";
      this.lblCopyright.Size = new System.Drawing.Size(258, 14);
      this.lblCopyright.TabIndex = 7;
      this.lblCopyright.Text = "Copyright © 2012 Oracle and/or its affiliates.";
      this.lblCopyright.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // lblAllRights
      // 
      this.lblAllRights.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblAllRights.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.lblAllRights.Location = new System.Drawing.Point(12, 628);
      this.lblAllRights.Name = "lblAllRights";
      this.lblAllRights.Size = new System.Drawing.Size(258, 14);
      this.lblAllRights.TabIndex = 8;
      this.lblAllRights.Text = "All rights reserved.";
      this.lblAllRights.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // infOpenConnection
      // 
      this.infOpenConnection.Image = global::MySQL.ExcelAddIn.Properties.Resources.lightning_32x32;
      this.infOpenConnection.ImageSize = MySQL.ExcelAddIn.Controls.InfolLabel.PictureSize.W32H32;
      this.infOpenConnection.InfoText1 = "Double-Click a Connection to Start";
      this.infOpenConnection.InfoText2 = "";
      this.infOpenConnection.Location = new System.Drawing.Point(14, 169);
      this.infOpenConnection.MainText = "Open a MySQL Connection";
      this.infOpenConnection.Name = "infOpenConnection";
      this.infOpenConnection.PictureAsButton = false;
      this.infOpenConnection.PictureEnabled = true;
      this.infOpenConnection.Size = new System.Drawing.Size(256, 38);
      this.infOpenConnection.TabIndex = 3;
      // 
      // infNewConnection
      // 
      this.infNewConnection.Image = global::MySQL.ExcelAddIn.Properties.Resources.new_connection_32x32;
      this.infNewConnection.ImageSize = MySQL.ExcelAddIn.Controls.InfolLabel.PictureSize.W32H32;
      this.infNewConnection.InfoText1 = "Add a new Database Connection";
      this.infNewConnection.InfoText2 = "";
      this.infNewConnection.Location = new System.Drawing.Point(14, 514);
      this.infNewConnection.MainText = "New Connection";
      this.infNewConnection.Name = "infNewConnection";
      this.infNewConnection.PictureAsButton = true;
      this.infNewConnection.PictureEnabled = true;
      this.infNewConnection.Size = new System.Drawing.Size(256, 38);
      this.infNewConnection.TabIndex = 5;
      this.infNewConnection.PictureClick += new System.EventHandler(this.infNewConnection_PictureClick);
      // 
      // infManageConnections
      // 
      this.infManageConnections.Image = global::MySQL.ExcelAddIn.Properties.Resources.db_mgmt_Connection_manage_32x32;
      this.infManageConnections.ImageSize = MySQL.ExcelAddIn.Controls.InfolLabel.PictureSize.W32H32;
      this.infManageConnections.InfoText1 = "Launch MySQL Workbench";
      this.infManageConnections.InfoText2 = "";
      this.infManageConnections.Location = new System.Drawing.Point(14, 558);
      this.infManageConnections.MainText = "Manage Connections";
      this.infManageConnections.Name = "infManageConnections";
      this.infManageConnections.PictureAsButton = true;
      this.infManageConnections.PictureEnabled = true;
      this.infManageConnections.Size = new System.Drawing.Size(256, 38);
      this.infManageConnections.TabIndex = 6;
      this.infManageConnections.PictureClick += new System.EventHandler(this.infManageConnections_PictureClick);
      // 
      // WelcomePanel
      // 
      this.Controls.Add(this.lblInstructions);
      this.Controls.Add(this.infOpenConnection);
      this.Controls.Add(this.infNewConnection);
      this.Controls.Add(this.infManageConnections);
      this.Controls.Add(this.lblCopyright);
      this.Controls.Add(this.lisConnections);
      this.Controls.Add(this.lblExcelAddIn);
      this.Controls.Add(this.lblWelcome);
      this.Controls.Add(this.picAddInLogo);
      this.Controls.Add(this.lblAllRights);
      this.Name = "WelcomePanel";
      this.Size = new System.Drawing.Size(287, 650);
      this.VisibleChanged += new System.EventHandler(this.WelcomePanel_VisibleChanged);
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogo)).EndInit();
      this.connectionsContextMenu.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblExcelAddIn;
    private System.Windows.Forms.Label lblWelcome;
    private System.Windows.Forms.PictureBox picAddInLogo;
    private System.Windows.Forms.ListView lisConnections;
    private System.Windows.Forms.ColumnHeader colConnectionName;
    private System.Windows.Forms.ColumnHeader colUserIP;
    private System.Windows.Forms.ImageList smallImages;
    private System.Windows.Forms.ImageList largeImages;
    private Controls.InfolLabel infOpenConnection;
    private System.Windows.Forms.Label lblInstructions;
    private System.Windows.Forms.ContextMenuStrip connectionsContextMenu;
    private System.Windows.Forms.ToolStripMenuItem openConnectionToolStripMenuItem;
    private Controls.InfolLabel infNewConnection;
    private Controls.InfolLabel infManageConnections;
    private System.Windows.Forms.Label lblCopyright;
    private System.Windows.Forms.Label lblAllRights;
  }
}
