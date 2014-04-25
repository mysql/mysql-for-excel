// Copyright (c) 2012-2014, Oracle and/or its affiliates. All rights reserved.
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

using MySQL.ForExcel.Controls;

namespace MySQL.ForExcel.Panels
{
  /// <summary>
  /// First panel shown to users within the Add-In's <seealso cref="ExcelAddInPane"/>.
  /// </summary>
  sealed partial class WelcomePanel
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing"><c>true</c> if managed resources should be disposed; otherwise, <c>false</c>.</param>
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
      this.LargeImagesList = new System.Windows.Forms.ImageList(this.components);
      this.ManageConnectionsHotLabel = new MySQL.ForExcel.Controls.HotLabel();
      this.ConnectionsList = new MySQL.ForExcel.Controls.MySqlListView();
      this.ConnectionsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.DeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.EditConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.RefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.NewConnectionHotLabel = new MySQL.ForExcel.Controls.HotLabel();
      this.UpperPanel = new System.Windows.Forms.Panel();
      this.SeparatorImage = new MySQL.ForExcel.Controls.TransparentPictureBox();
      this.WelcomeTextPictureBox = new System.Windows.Forms.PictureBox();
      this.InstructionsLabel = new MySQL.ForExcel.Controls.TransparentLabel();
      this.MainLogoPictureBox = new System.Windows.Forms.PictureBox();
      this.OpenConnectionHotLabel = new MySQL.ForExcel.Controls.HotLabel();
      this.AboutHotLabel = new MySQL.ForExcel.Controls.HotLabel();
      this.ConnectionsContextMenuStrip.SuspendLayout();
      this.UpperPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.WelcomeTextPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.MainLogoPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // LargeImagesList
      // 
      this.LargeImagesList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("LargeImagesList.ImageStream")));
      this.LargeImagesList.TransparentColor = System.Drawing.Color.Transparent;
      this.LargeImagesList.Images.SetKeyName(0, "MySQLforExcel-WelcomePanel-ListItem-Connection-32x32.png");
      this.LargeImagesList.Images.SetKeyName(1, "MySQLforExcel-WelcomePanel-Connection-Disabled-24x24.png");
      // 
      // ManageConnectionsHotLabel
      // 
      this.ManageConnectionsHotLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ManageConnectionsHotLabel.Behavior = MySQL.ForExcel.Controls.HotLabel.BehaviorType.Button;
      this.ManageConnectionsHotLabel.CheckedImage = null;
      this.ManageConnectionsHotLabel.Description = "Launch MySQL Workbench";
      this.ManageConnectionsHotLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.ManageConnectionsHotLabel.DescriptionColorOpacity = 0.6D;
      this.ManageConnectionsHotLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ManageConnectionsHotLabel.DescriptionShadowOpacity = 0.4D;
      this.ManageConnectionsHotLabel.DescriptionShadowPixelsXOffset = 0;
      this.ManageConnectionsHotLabel.DescriptionShadowPixelsYOffset = 1;
      this.ManageConnectionsHotLabel.DisabledImage = null;
      this.ManageConnectionsHotLabel.DrawShadow = true;
      this.ManageConnectionsHotLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ManageConnectionsHotLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_WelcomePanel_ManageConnection_24x24;
      this.ManageConnectionsHotLabel.ImagePixelsXOffset = 0;
      this.ManageConnectionsHotLabel.ImagePixelsYOffset = 0;
      this.ManageConnectionsHotLabel.Location = new System.Drawing.Point(9, 558);
      this.ManageConnectionsHotLabel.Margin = new System.Windows.Forms.Padding(4);
      this.ManageConnectionsHotLabel.Name = "ManageConnectionsHotLabel";
      this.ManageConnectionsHotLabel.Size = new System.Drawing.Size(237, 28);
      this.ManageConnectionsHotLabel.TabIndex = 16;
      this.ManageConnectionsHotLabel.Title = "Manage Connections";
      this.ManageConnectionsHotLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.ManageConnectionsHotLabel.TitleColorOpacity = 0.95D;
      this.ManageConnectionsHotLabel.TitleDescriptionPixelsSpacing = 0;
      this.ManageConnectionsHotLabel.TitlePixelsXOffset = 3;
      this.ManageConnectionsHotLabel.TitlePixelsYOffset = -2;
      this.ManageConnectionsHotLabel.TitleShadowOpacity = 0.2D;
      this.ManageConnectionsHotLabel.TitleShadowPixelsXOffset = 0;
      this.ManageConnectionsHotLabel.TitleShadowPixelsYOffset = 1;
      this.ManageConnectionsHotLabel.Click += new System.EventHandler(this.ManageConnectionsHotLabel_Click);
      // 
      // ConnectionsList
      // 
      this.ConnectionsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ConnectionsList.CollapsedIcon = global::MySQL.ForExcel.Properties.Resources.ArrowRight;
      this.ConnectionsList.ContextMenuStrip = this.ConnectionsContextMenuStrip;
      this.ConnectionsList.DescriptionColor = System.Drawing.Color.Silver;
      this.ConnectionsList.DescriptionColorOpacity = 1D;
      this.ConnectionsList.DescriptionFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConnectionsList.DescriptionTextVerticalPixelsOffset = -3;
      this.ConnectionsList.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
      this.ConnectionsList.ExpandedIcon = global::MySQL.ForExcel.Properties.Resources.ArrowDown;
      this.ConnectionsList.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConnectionsList.ImageHorizontalPixelsOffset = 4;
      this.ConnectionsList.ImageToTextHorizontalPixelsOffset = 4;
      this.ConnectionsList.Indent = 18;
      this.ConnectionsList.ItemHeight = 20;
      this.ConnectionsList.Location = new System.Drawing.Point(9, 195);
      this.ConnectionsList.Name = "ConnectionsList";
      this.ConnectionsList.NodeHeightMultiple = 2;
      this.ConnectionsList.NodeImages = this.LargeImagesList;
      this.ConnectionsList.ShowNodeToolTips = true;
      this.ConnectionsList.Size = new System.Drawing.Size(242, 315);
      this.ConnectionsList.TabIndex = 22;
      this.ConnectionsList.TitleColorOpacity = 0.8D;
      this.ConnectionsList.TitleTextVerticalPixelsOffset = 2;
      this.ConnectionsList.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ConnectionsList_NodeMouseDoubleClick);
      // 
      // ConnectionsContextMenuStrip
      // 
      this.ConnectionsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteToolStripMenuItem,
            this.EditConnectionToolStripMenuItem,
            this.RefreshToolStripMenuItem});
      this.ConnectionsContextMenuStrip.Name = "contextMenuStripRefresh";
      this.ConnectionsContextMenuStrip.Size = new System.Drawing.Size(184, 70);
      this.ConnectionsContextMenuStrip.Text = "Refresh";
      this.ConnectionsContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ConnectionsContextMenuStrip_Opening);
      // 
      // DeleteToolStripMenuItem
      // 
      this.DeleteToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.DeleteHS;
      this.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem";
      this.DeleteToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
      this.DeleteToolStripMenuItem.Text = "Delete Connection";
      this.DeleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
      // 
      // EditConnectionToolStripMenuItem
      // 
      this.EditConnectionToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.edit;
      this.EditConnectionToolStripMenuItem.Name = "EditConnectionToolStripMenuItem";
      this.EditConnectionToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
      this.EditConnectionToolStripMenuItem.Text = "Edit Connection";
      this.EditConnectionToolStripMenuItem.Click += new System.EventHandler(this.EditConnectionToolStripMenuItem_Click);
      // 
      // RefreshToolStripMenuItem
      // 
      this.RefreshToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.refresh_sidebar;
      this.RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem";
      this.RefreshToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
      this.RefreshToolStripMenuItem.Text = "Refresh Connections";
      this.RefreshToolStripMenuItem.Click += new System.EventHandler(this.RefreshItem_Click);
      // 
      // NewConnectionHotLabel
      // 
      this.NewConnectionHotLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.NewConnectionHotLabel.Behavior = MySQL.ForExcel.Controls.HotLabel.BehaviorType.Button;
      this.NewConnectionHotLabel.CheckedImage = null;
      this.NewConnectionHotLabel.Description = "Add a new Database Connection";
      this.NewConnectionHotLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.NewConnectionHotLabel.DescriptionColorOpacity = 0.6D;
      this.NewConnectionHotLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.NewConnectionHotLabel.DescriptionShadowOpacity = 0.4D;
      this.NewConnectionHotLabel.DescriptionShadowPixelsXOffset = 0;
      this.NewConnectionHotLabel.DescriptionShadowPixelsYOffset = 1;
      this.NewConnectionHotLabel.DisabledImage = null;
      this.NewConnectionHotLabel.DrawShadow = true;
      this.NewConnectionHotLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.NewConnectionHotLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_WelcomePanel_NewConnection_24x24;
      this.NewConnectionHotLabel.ImagePixelsXOffset = 0;
      this.NewConnectionHotLabel.ImagePixelsYOffset = 0;
      this.NewConnectionHotLabel.Location = new System.Drawing.Point(9, 520);
      this.NewConnectionHotLabel.Margin = new System.Windows.Forms.Padding(4);
      this.NewConnectionHotLabel.Name = "NewConnectionHotLabel";
      this.NewConnectionHotLabel.Size = new System.Drawing.Size(237, 28);
      this.NewConnectionHotLabel.TabIndex = 15;
      this.NewConnectionHotLabel.Title = "New Connection";
      this.NewConnectionHotLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.NewConnectionHotLabel.TitleColorOpacity = 0.95D;
      this.NewConnectionHotLabel.TitleDescriptionPixelsSpacing = 0;
      this.NewConnectionHotLabel.TitlePixelsXOffset = 3;
      this.NewConnectionHotLabel.TitlePixelsYOffset = 0;
      this.NewConnectionHotLabel.TitleShadowOpacity = 0.2D;
      this.NewConnectionHotLabel.TitleShadowPixelsXOffset = 0;
      this.NewConnectionHotLabel.TitleShadowPixelsYOffset = 1;
      this.NewConnectionHotLabel.Click += new System.EventHandler(this.NewConnectionHotLabel_Click);
      // 
      // UpperPanel
      // 
      this.UpperPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.UpperPanel.Controls.Add(this.SeparatorImage);
      this.UpperPanel.Controls.Add(this.WelcomeTextPictureBox);
      this.UpperPanel.Controls.Add(this.InstructionsLabel);
      this.UpperPanel.Controls.Add(this.MainLogoPictureBox);
      this.UpperPanel.Controls.Add(this.OpenConnectionHotLabel);
      this.UpperPanel.Location = new System.Drawing.Point(0, 0);
      this.UpperPanel.Name = "UpperPanel";
      this.UpperPanel.Size = new System.Drawing.Size(260, 191);
      this.UpperPanel.TabIndex = 26;
      // 
      // SeparatorImage
      // 
      this.SeparatorImage.BackColor = System.Drawing.Color.Transparent;
      this.SeparatorImage.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Separator;
      this.SeparatorImage.Location = new System.Drawing.Point(9, 135);
      this.SeparatorImage.Name = "SeparatorImage";
      this.SeparatorImage.Opacity = 0.3F;
      this.SeparatorImage.Size = new System.Drawing.Size(242, 21);
      this.SeparatorImage.TabIndex = 30;
      // 
      // WelcomeTextPictureBox
      // 
      this.WelcomeTextPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_WelcomePanel_Title;
      this.WelcomeTextPictureBox.Location = new System.Drawing.Point(69, 21);
      this.WelcomeTextPictureBox.Name = "WelcomeTextPictureBox";
      this.WelcomeTextPictureBox.Size = new System.Drawing.Size(172, 36);
      this.WelcomeTextPictureBox.TabIndex = 29;
      this.WelcomeTextPictureBox.TabStop = false;
      // 
      // InstructionsLabel
      // 
      this.InstructionsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.InstructionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.InstructionsLabel.Location = new System.Drawing.Point(9, 72);
      this.InstructionsLabel.Name = "InstructionsLabel";
      this.InstructionsLabel.PixelsSpacingAdjustment = -3;
      this.InstructionsLabel.ShadowColor = System.Drawing.SystemColors.ControlText;
      this.InstructionsLabel.ShadowOpacity = 0.7D;
      this.InstructionsLabel.Size = new System.Drawing.Size(237, 54);
      this.InstructionsLabel.TabIndex = 26;
      this.InstructionsLabel.TextOpacity = 0.6D;
      this.InstructionsLabel.TransparentText = "MySQL for Excel allows you to work with the MySQL Database right from within the " +
    "MS Office Excel application. Excel is a powerful tool for data analysis and edit" +
    "ing.";
      // 
      // MainLogoPictureBox
      // 
      this.MainLogoPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Logo_64x64;
      this.MainLogoPictureBox.Location = new System.Drawing.Point(9, 11);
      this.MainLogoPictureBox.Name = "MainLogoPictureBox";
      this.MainLogoPictureBox.Size = new System.Drawing.Size(64, 64);
      this.MainLogoPictureBox.TabIndex = 27;
      this.MainLogoPictureBox.TabStop = false;
      // 
      // OpenConnectionHotLabel
      // 
      this.OpenConnectionHotLabel.Behavior = MySQL.ForExcel.Controls.HotLabel.BehaviorType.Label;
      this.OpenConnectionHotLabel.CheckedImage = null;
      this.OpenConnectionHotLabel.Description = "Double-Click a Connection to Start";
      this.OpenConnectionHotLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.OpenConnectionHotLabel.DescriptionColorOpacity = 0.6D;
      this.OpenConnectionHotLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OpenConnectionHotLabel.DescriptionShadowOpacity = 0.4D;
      this.OpenConnectionHotLabel.DescriptionShadowPixelsXOffset = 0;
      this.OpenConnectionHotLabel.DescriptionShadowPixelsYOffset = 1;
      this.OpenConnectionHotLabel.DisabledImage = null;
      this.OpenConnectionHotLabel.DrawShadow = true;
      this.OpenConnectionHotLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OpenConnectionHotLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_WelcomePanel_Connection_24x24;
      this.OpenConnectionHotLabel.ImagePixelsXOffset = 0;
      this.OpenConnectionHotLabel.ImagePixelsYOffset = -2;
      this.OpenConnectionHotLabel.Location = new System.Drawing.Point(9, 162);
      this.OpenConnectionHotLabel.Margin = new System.Windows.Forms.Padding(4);
      this.OpenConnectionHotLabel.Name = "OpenConnectionHotLabel";
      this.OpenConnectionHotLabel.Size = new System.Drawing.Size(242, 28);
      this.OpenConnectionHotLabel.TabIndex = 28;
      this.OpenConnectionHotLabel.Title = "Open a MySQL Connection";
      this.OpenConnectionHotLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.OpenConnectionHotLabel.TitleColorOpacity = 0.95D;
      this.OpenConnectionHotLabel.TitleDescriptionPixelsSpacing = 0;
      this.OpenConnectionHotLabel.TitlePixelsXOffset = 3;
      this.OpenConnectionHotLabel.TitlePixelsYOffset = -3;
      this.OpenConnectionHotLabel.TitleShadowOpacity = 0.2D;
      this.OpenConnectionHotLabel.TitleShadowPixelsXOffset = 0;
      this.OpenConnectionHotLabel.TitleShadowPixelsYOffset = 1;
      // 
      // AboutHotLabel
      // 
      this.AboutHotLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.AboutHotLabel.Behavior = MySQL.ForExcel.Controls.HotLabel.BehaviorType.Button;
      this.AboutHotLabel.CheckedImage = null;
      this.AboutHotLabel.Description = "";
      this.AboutHotLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.AboutHotLabel.DescriptionColorOpacity = 0.6D;
      this.AboutHotLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AboutHotLabel.DescriptionShadowOpacity = 0.4D;
      this.AboutHotLabel.DescriptionShadowPixelsXOffset = 0;
      this.AboutHotLabel.DescriptionShadowPixelsYOffset = 1;
      this.AboutHotLabel.DisabledImage = null;
      this.AboutHotLabel.DrawShadow = true;
      this.AboutHotLabel.Font = new System.Drawing.Font("Tahoma", 7F);
      this.AboutHotLabel.Image = null;
      this.AboutHotLabel.ImagePixelsXOffset = 0;
      this.AboutHotLabel.ImagePixelsYOffset = 0;
      this.AboutHotLabel.Location = new System.Drawing.Point(78, 599);
      this.AboutHotLabel.Margin = new System.Windows.Forms.Padding(4);
      this.AboutHotLabel.Name = "AboutHotLabel";
      this.AboutHotLabel.Size = new System.Drawing.Size(105, 22);
      this.AboutHotLabel.TabIndex = 27;
      this.AboutHotLabel.Title = "About MySQL For Excel";
      this.AboutHotLabel.TitleColor = System.Drawing.SystemColors.GrayText;
      this.AboutHotLabel.TitleColorOpacity = 0.95D;
      this.AboutHotLabel.TitleDescriptionPixelsSpacing = 0;
      this.AboutHotLabel.TitlePixelsXOffset = 3;
      this.AboutHotLabel.TitlePixelsYOffset = 0;
      this.AboutHotLabel.TitleShadowOpacity = 0.2D;
      this.AboutHotLabel.TitleShadowPixelsXOffset = 0;
      this.AboutHotLabel.TitleShadowPixelsYOffset = 1;
      this.AboutHotLabel.Click += new System.EventHandler(this.AboutHotLabel_Click);
      // 
      // WelcomePanel
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add(this.AboutHotLabel);
      this.Controls.Add(this.UpperPanel);
      this.Controls.Add(this.NewConnectionHotLabel);
      this.Controls.Add(this.ManageConnectionsHotLabel);
      this.Controls.Add(this.ConnectionsList);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "WelcomePanel";
      this.Size = new System.Drawing.Size(260, 625);
      this.ConnectionsContextMenuStrip.ResumeLayout(false);
      this.UpperPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.WelcomeTextPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.MainLogoPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ImageList LargeImagesList;
    private HotLabel NewConnectionHotLabel;
    private HotLabel ManageConnectionsHotLabel;
    private MySqlListView ConnectionsList;
    private System.Windows.Forms.ContextMenuStrip ConnectionsContextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem RefreshToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem DeleteToolStripMenuItem;
    private System.Windows.Forms.Panel UpperPanel;
    private TransparentPictureBox SeparatorImage;
    private System.Windows.Forms.PictureBox WelcomeTextPictureBox;
    private TransparentLabel InstructionsLabel;
    private System.Windows.Forms.PictureBox MainLogoPictureBox;
    private HotLabel OpenConnectionHotLabel;
    private HotLabel AboutHotLabel;
    private System.Windows.Forms.ToolStripMenuItem EditConnectionToolStripMenuItem;
  }
}
