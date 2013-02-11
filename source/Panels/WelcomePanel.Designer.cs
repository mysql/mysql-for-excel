// 
// Copyright (c) 2012, Oracle and/or its affiliates. All rights reserved.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA
//

namespace MySQL.ForExcel
{
  /// <summary>
  /// First panel shown to users within the Add-In's <seealso cref="TaskPaneControl"/>.
  /// </summary>
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomePanel));
      System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Local Connections");
      System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Remote Connections");
      this.largeImages = new System.Windows.Forms.ImageList(this.components);
      this.manageConnectionsLabel = new MySQL.ForExcel.HotLabel();
      this.connectionList = new MySQL.ForExcel.MyTreeView();
      this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.newConnectionLabel = new MySQL.ForExcel.HotLabel();
      this.upperPanel = new System.Windows.Forms.Panel();
      this.imgSeparator = new MySQL.ForExcel.TransparentPictureBox();
      this.picAddInLogoText = new System.Windows.Forms.PictureBox();
      this.lblInstructions = new MySQL.ForExcel.TransparentLabel();
      this.picAddInLogo = new System.Windows.Forms.PictureBox();
      this.openConnectionLabel = new MySQL.ForExcel.HotLabel();
      this.aboutLabel = new MySQL.ForExcel.HotLabel();
      this.contextMenuStrip.SuspendLayout();
      this.upperPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogoText)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogo)).BeginInit();
      this.SuspendLayout();
      // 
      // largeImages
      // 
      this.largeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("largeImages.ImageStream")));
      this.largeImages.TransparentColor = System.Drawing.Color.Transparent;
      this.largeImages.Images.SetKeyName(0, "MySQLforExcel-WelcomePanel-ListItem-Connection-32x32.png");
      this.largeImages.Images.SetKeyName(1, "MySQLforExcel-WelcomePanel-Connection-Disabled-24x24.png");
      // 
      // manageConnectionsLabel
      // 
      this.manageConnectionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.manageConnectionsLabel.Description = "Launch MySQL Workbench";
      this.manageConnectionsLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.manageConnectionsLabel.DescriptionColorOpacity = 0.6D;
      this.manageConnectionsLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.manageConnectionsLabel.DescriptionShadowOpacity = 0.4D;
      this.manageConnectionsLabel.DescriptionShadowPixelsXOffset = 0;
      this.manageConnectionsLabel.DescriptionShadowPixelsYOffset = 1;
      this.manageConnectionsLabel.DisabledImage = null;
      this.manageConnectionsLabel.DrawShadow = true;
      this.manageConnectionsLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.manageConnectionsLabel.HotTracking = true;
      this.manageConnectionsLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_WelcomePanel_ManageConnection_24x24;
      this.manageConnectionsLabel.ImagePixelsXOffset = 0;
      this.manageConnectionsLabel.ImagePixelsYOffset = 0;
      this.manageConnectionsLabel.Location = new System.Drawing.Point(9, 558);
      this.manageConnectionsLabel.Margin = new System.Windows.Forms.Padding(4);
      this.manageConnectionsLabel.Name = "manageConnectionsLabel";
      this.manageConnectionsLabel.Size = new System.Drawing.Size(237, 28);
      this.manageConnectionsLabel.TabIndex = 16;
      this.manageConnectionsLabel.Title = "Manage Connections";
      this.manageConnectionsLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.manageConnectionsLabel.TitleColorOpacity = 0.95D;
      this.manageConnectionsLabel.TitleDescriptionPixelsSpacing = 0;
      this.manageConnectionsLabel.TitlePixelsXOffset = 3;
      this.manageConnectionsLabel.TitlePixelsYOffset = -2;
      this.manageConnectionsLabel.TitleShadowOpacity = 0.2D;
      this.manageConnectionsLabel.TitleShadowPixelsXOffset = 0;
      this.manageConnectionsLabel.TitleShadowPixelsYOffset = 1;
      this.manageConnectionsLabel.Click += new System.EventHandler(this.manageConnectionsLabel_Click);
      // 
      // connectionList
      // 
      this.connectionList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.connectionList.CollapsedIcon = global::MySQL.ForExcel.Properties.Resources.ArrowRight;
      this.connectionList.ContextMenuStrip = this.contextMenuStrip;
      this.connectionList.DescriptionColor = System.Drawing.Color.Silver;
      this.connectionList.DescriptionColorOpacity = 1D;
      this.connectionList.DescriptionFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.connectionList.DescriptionTextVerticalPixelsOffset = -3;
      this.connectionList.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
      this.connectionList.ExpandedIcon = global::MySQL.ForExcel.Properties.Resources.ArrowDown;
      this.connectionList.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.connectionList.ImageHorizontalPixelsOffset = 4;
      this.connectionList.ImageToTextHorizontalPixelsOffset = 4;
      this.connectionList.Indent = 18;
      this.connectionList.ItemHeight = 20;
      this.connectionList.Location = new System.Drawing.Point(9, 195);
      this.connectionList.Name = "connectionList";
      this.connectionList.NodeHeightMultiple = 2;
      this.connectionList.NodeImages = this.largeImages;
      treeNode1.BackColor = System.Drawing.SystemColors.ControlLight;
      treeNode1.ForeColor = System.Drawing.SystemColors.WindowText;
      treeNode1.Name = "LocalConnectionsNode";
      treeNode1.Text = "Local Connections";
      treeNode2.BackColor = System.Drawing.SystemColors.ControlLight;
      treeNode2.ForeColor = System.Drawing.SystemColors.WindowText;
      treeNode2.Name = "RemoteConnectionsNode";
      treeNode2.Text = "Remote Connections";
      this.connectionList.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
      this.connectionList.ShowNodeToolTips = true;
      this.connectionList.Size = new System.Drawing.Size(242, 315);
      this.connectionList.TabIndex = 22;
      this.connectionList.TitleColorOpacity = 0.8D;
      this.connectionList.TitleTextVerticalPixelsOffset = 2;
      this.connectionList.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.connectionList_NodeMouseDoubleClick);
      // 
      // contextMenuStrip
      // 
      this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem,
            this.refreshToolStripMenuItem});
      this.contextMenuStrip.Name = "contextMenuStripRefresh";
      this.contextMenuStrip.Size = new System.Drawing.Size(184, 70);
      this.contextMenuStrip.Text = "Refresh";
      this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
      // 
      // deleteToolStripMenuItem
      // 
      this.deleteToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.DeleteHS;
      this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
      this.deleteToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
      this.deleteToolStripMenuItem.Text = "Delete Connection";
      this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
      // 
      // refreshToolStripMenuItem
      // 
      this.refreshToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.refresh_sidebar;
      this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
      this.refreshToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
      this.refreshToolStripMenuItem.Text = "Refresh Connections";
      this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshItem_Click);
      // 
      // newConnectionLabel
      // 
      this.newConnectionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.newConnectionLabel.Description = "Add a new Database Connection";
      this.newConnectionLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.newConnectionLabel.DescriptionColorOpacity = 0.6D;
      this.newConnectionLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.newConnectionLabel.DescriptionShadowOpacity = 0.4D;
      this.newConnectionLabel.DescriptionShadowPixelsXOffset = 0;
      this.newConnectionLabel.DescriptionShadowPixelsYOffset = 1;
      this.newConnectionLabel.DisabledImage = null;
      this.newConnectionLabel.DrawShadow = true;
      this.newConnectionLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.newConnectionLabel.HotTracking = true;
      this.newConnectionLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_WelcomePanel_NewConnection_24x24;
      this.newConnectionLabel.ImagePixelsXOffset = 0;
      this.newConnectionLabel.ImagePixelsYOffset = 0;
      this.newConnectionLabel.Location = new System.Drawing.Point(9, 520);
      this.newConnectionLabel.Margin = new System.Windows.Forms.Padding(4);
      this.newConnectionLabel.Name = "newConnectionLabel";
      this.newConnectionLabel.Size = new System.Drawing.Size(237, 28);
      this.newConnectionLabel.TabIndex = 15;
      this.newConnectionLabel.Title = "New Connection";
      this.newConnectionLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.newConnectionLabel.TitleColorOpacity = 0.95D;
      this.newConnectionLabel.TitleDescriptionPixelsSpacing = 0;
      this.newConnectionLabel.TitlePixelsXOffset = 3;
      this.newConnectionLabel.TitlePixelsYOffset = 0;
      this.newConnectionLabel.TitleShadowOpacity = 0.2D;
      this.newConnectionLabel.TitleShadowPixelsXOffset = 0;
      this.newConnectionLabel.TitleShadowPixelsYOffset = 1;
      this.newConnectionLabel.Click += new System.EventHandler(this.newConnectionLabel_Click);
      // 
      // upperPanel
      // 
      this.upperPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.upperPanel.BackColor = System.Drawing.SystemColors.Control;
      this.upperPanel.Controls.Add(this.imgSeparator);
      this.upperPanel.Controls.Add(this.picAddInLogoText);
      this.upperPanel.Controls.Add(this.lblInstructions);
      this.upperPanel.Controls.Add(this.picAddInLogo);
      this.upperPanel.Controls.Add(this.openConnectionLabel);
      this.upperPanel.Location = new System.Drawing.Point(0, 0);
      this.upperPanel.Name = "upperPanel";
      this.upperPanel.Size = new System.Drawing.Size(260, 191);
      this.upperPanel.TabIndex = 26;
      // 
      // imgSeparator
      // 
      this.imgSeparator.BackColor = System.Drawing.Color.Transparent;
      this.imgSeparator.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Separator;
      this.imgSeparator.Location = new System.Drawing.Point(9, 135);
      this.imgSeparator.Name = "imgSeparator";
      this.imgSeparator.Opacity = 0.3F;
      this.imgSeparator.Size = new System.Drawing.Size(242, 21);
      this.imgSeparator.TabIndex = 30;
      // 
      // picAddInLogoText
      // 
      this.picAddInLogoText.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_WelcomePanel_Title;
      this.picAddInLogoText.Location = new System.Drawing.Point(69, 21);
      this.picAddInLogoText.Name = "picAddInLogoText";
      this.picAddInLogoText.Size = new System.Drawing.Size(172, 36);
      this.picAddInLogoText.TabIndex = 29;
      this.picAddInLogoText.TabStop = false;
      // 
      // lblInstructions
      // 
      this.lblInstructions.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblInstructions.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblInstructions.Location = new System.Drawing.Point(9, 72);
      this.lblInstructions.Name = "lblInstructions";
      this.lblInstructions.PixelsSpacingAdjustment = -3;
      this.lblInstructions.ShadowColor = System.Drawing.SystemColors.ControlText;
      this.lblInstructions.ShadowOpacity = 0.7D;
      this.lblInstructions.Size = new System.Drawing.Size(237, 54);
      this.lblInstructions.TabIndex = 26;
      this.lblInstructions.TextOpacity = 0.6D;
      this.lblInstructions.TransparentText = "MySQL for Excel allows you to work with the MySQL Database right from within the " +
    "MS Office Excel application. Excel is a powerful tool for data analysis and edit" +
    "ing.";
      // 
      // picAddInLogo
      // 
      this.picAddInLogo.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Logo_64x64;
      this.picAddInLogo.Location = new System.Drawing.Point(9, 11);
      this.picAddInLogo.Name = "picAddInLogo";
      this.picAddInLogo.Size = new System.Drawing.Size(64, 64);
      this.picAddInLogo.TabIndex = 27;
      this.picAddInLogo.TabStop = false;
      // 
      // openConnectionLabel
      // 
      this.openConnectionLabel.Description = "Double-Click a Connection to Start";
      this.openConnectionLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.openConnectionLabel.DescriptionColorOpacity = 0.6D;
      this.openConnectionLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.openConnectionLabel.DescriptionShadowOpacity = 0.4D;
      this.openConnectionLabel.DescriptionShadowPixelsXOffset = 0;
      this.openConnectionLabel.DescriptionShadowPixelsYOffset = 1;
      this.openConnectionLabel.DisabledImage = null;
      this.openConnectionLabel.DrawShadow = true;
      this.openConnectionLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.openConnectionLabel.HotTracking = false;
      this.openConnectionLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_WelcomePanel_Connection_24x24;
      this.openConnectionLabel.ImagePixelsXOffset = 0;
      this.openConnectionLabel.ImagePixelsYOffset = -2;
      this.openConnectionLabel.Location = new System.Drawing.Point(9, 162);
      this.openConnectionLabel.Margin = new System.Windows.Forms.Padding(4);
      this.openConnectionLabel.Name = "openConnectionLabel";
      this.openConnectionLabel.Size = new System.Drawing.Size(242, 28);
      this.openConnectionLabel.TabIndex = 28;
      this.openConnectionLabel.Title = "Open a MySQL Connection";
      this.openConnectionLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.openConnectionLabel.TitleColorOpacity = 0.95D;
      this.openConnectionLabel.TitleDescriptionPixelsSpacing = 0;
      this.openConnectionLabel.TitlePixelsXOffset = 3;
      this.openConnectionLabel.TitlePixelsYOffset = -3;
      this.openConnectionLabel.TitleShadowOpacity = 0.2D;
      this.openConnectionLabel.TitleShadowPixelsXOffset = 0;
      this.openConnectionLabel.TitleShadowPixelsYOffset = 1;
      // 
      // aboutLabel
      // 
      this.aboutLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.aboutLabel.Description = "";
      this.aboutLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.aboutLabel.DescriptionColorOpacity = 0.6D;
      this.aboutLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.aboutLabel.DescriptionShadowOpacity = 0.4D;
      this.aboutLabel.DescriptionShadowPixelsXOffset = 0;
      this.aboutLabel.DescriptionShadowPixelsYOffset = 1;
      this.aboutLabel.DisabledImage = null;
      this.aboutLabel.DrawShadow = true;
      this.aboutLabel.Font = new System.Drawing.Font("Tahoma", 7F);
      this.aboutLabel.HotTracking = true;
      this.aboutLabel.Image = null;
      this.aboutLabel.ImagePixelsXOffset = 0;
      this.aboutLabel.ImagePixelsYOffset = 0;
      this.aboutLabel.Location = new System.Drawing.Point(78, 599);
      this.aboutLabel.Margin = new System.Windows.Forms.Padding(4);
      this.aboutLabel.Name = "aboutLabel";
      this.aboutLabel.Size = new System.Drawing.Size(105, 22);
      this.aboutLabel.TabIndex = 27;
      this.aboutLabel.Title = "About MySQL For Excel";
      this.aboutLabel.TitleColor = System.Drawing.SystemColors.GrayText;
      this.aboutLabel.TitleColorOpacity = 0.95D;
      this.aboutLabel.TitleDescriptionPixelsSpacing = 0;
      this.aboutLabel.TitlePixelsXOffset = 3;
      this.aboutLabel.TitlePixelsYOffset = 0;
      this.aboutLabel.TitleShadowOpacity = 0.2D;
      this.aboutLabel.TitleShadowPixelsXOffset = 0;
      this.aboutLabel.TitleShadowPixelsYOffset = 1;
      this.aboutLabel.Click += new System.EventHandler(this.aboutLabel_Click);
      // 
      // WelcomePanel
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add(this.aboutLabel);
      this.Controls.Add(this.upperPanel);
      this.Controls.Add(this.newConnectionLabel);
      this.Controls.Add(this.manageConnectionsLabel);
      this.Controls.Add(this.connectionList);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "WelcomePanel";
      this.Size = new System.Drawing.Size(260, 625);
      this.contextMenuStrip.ResumeLayout(false);
      this.upperPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogoText)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picAddInLogo)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ImageList largeImages;
    private HotLabel newConnectionLabel;
    private HotLabel manageConnectionsLabel;
    private MyTreeView connectionList;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    private System.Windows.Forms.Panel upperPanel;
    private TransparentPictureBox imgSeparator;
    private System.Windows.Forms.PictureBox picAddInLogoText;
    private TransparentLabel lblInstructions;
    private System.Windows.Forms.PictureBox picAddInLogo;
    private HotLabel openConnectionLabel;
    private HotLabel aboutLabel;
  }
}
