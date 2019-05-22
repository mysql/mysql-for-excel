// Copyright (c) 2012, 2019, Oracle and/or its affiliates. All rights reserved.
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
  partial class SchemaSelectionPanel
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
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }

        _wbConnection = null;
        if (LoadedSchemas != null)
        {
          LoadedSchemas.ForEach(dbo => dbo.Dispose());
          LoadedSchemas.Clear();
        }
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
      this.LargeImagesList = new System.Windows.Forms.ImageList(this.components);
      this.BackButton = new System.Windows.Forms.Button();
      this.NextButton = new System.Windows.Forms.Button();
      this.OptionsButton = new System.Windows.Forms.Button();
      this.SchemasList = new MySQL.ForExcel.Controls.MySqlListView();
      this.SchemasContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.DisplaySchemaCollationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.RefreshSchemasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.CreateNewSchemaHotLabel = new MySQL.ForExcel.Controls.HotLabel();
      this.UpperPanel = new System.Windows.Forms.Panel();
      this.ConnectionInfoLabel = new System.Windows.Forms.Label();
      this.ConnectionPictureBox = new System.Windows.Forms.PictureBox();
      this.UserPictureBox = new System.Windows.Forms.PictureBox();
      this.SeparatorImage = new MySQL.ForExcel.Controls.TransparentPictureBox();
      this.UserLabel = new System.Windows.Forms.Label();
      this.SchemaFilter = new MySQL.ForExcel.Controls.SearchEdit();
      this.SelectSchemaHotLabel = new MySQL.ForExcel.Controls.HotLabel();
      this.InstructionsLabel = new MySQL.ForExcel.Controls.TransparentLabel();
      this.ConnectionNameLabel = new System.Windows.Forms.Label();
      this.MainLogoPictureBox = new System.Windows.Forms.PictureBox();
      this.SchemasContextMenuStrip.SuspendLayout();
      this.UpperPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.UserPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.MainLogoPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // LargeImagesList
      // 
      this.LargeImagesList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("LargeImagesList.ImageStream")));
      this.LargeImagesList.TransparentColor = System.Drawing.Color.Transparent;
      this.LargeImagesList.Images.SetKeyName(0, "MySQLforExcel-SchemaPanel-ListItem-Schema-24x24.png");
      this.LargeImagesList.Images.SetKeyName(1, "MySQLforExcel-SchemaPanel-ListItem-Schema-32x32.png");
      // 
      // BackButton
      // 
      this.BackButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.BackButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.BackButton.Location = new System.Drawing.Point(95, 599);
      this.BackButton.Name = "BackButton";
      this.BackButton.Size = new System.Drawing.Size(75, 23);
      this.BackButton.TabIndex = 4;
      this.BackButton.Text = "< Back";
      this.BackButton.UseVisualStyleBackColor = true;
      this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
      // 
      // NextButton
      // 
      this.NextButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.NextButton.Enabled = false;
      this.NextButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.NextButton.Location = new System.Drawing.Point(176, 599);
      this.NextButton.Name = "NextButton";
      this.NextButton.Size = new System.Drawing.Size(75, 23);
      this.NextButton.TabIndex = 5;
      this.NextButton.Text = "Next >";
      this.NextButton.UseVisualStyleBackColor = true;
      this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
      // 
      // OptionsButton
      // 
      this.OptionsButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.OptionsButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OptionsButton.Location = new System.Drawing.Point(9, 599);
      this.OptionsButton.Name = "OptionsButton";
      this.OptionsButton.Size = new System.Drawing.Size(75, 23);
      this.OptionsButton.TabIndex = 3;
      this.OptionsButton.Text = "Options";
      this.OptionsButton.UseVisualStyleBackColor = true;
      this.OptionsButton.Click += new System.EventHandler(this.OptionsButton_Click);
      // 
      // SchemasList
      // 
      this.SchemasList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.SchemasList.CollapsedIcon = global::MySQL.ForExcel.Properties.Resources.ArrowRight;
      this.SchemasList.ContextMenuStrip = this.SchemasContextMenuStrip;
      this.SchemasList.DescriptionColor = System.Drawing.Color.Silver;
      this.SchemasList.DescriptionColorOpacity = 1D;
      this.SchemasList.DescriptionFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SchemasList.DescriptionTextVerticalOffset = 0;
      this.SchemasList.DisplayImagesOfDisabledNodesInGrayScale = true;
      this.SchemasList.ExpandedIcon = global::MySQL.ForExcel.Properties.Resources.ArrowDown;
      this.SchemasList.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SchemasList.ImageHorizontalOffset = 14;
      this.SchemasList.Indent = 18;
      this.SchemasList.ItemHeight = 10;
      this.SchemasList.Location = new System.Drawing.Point(9, 226);
      this.SchemasList.MultiSelect = false;
      this.SchemasList.Name = "SchemasList";
      this.SchemasList.NodeHeightMultiple = 3;
      this.SchemasList.NodeImages = this.LargeImagesList;
      this.SchemasList.ScaledImagesVerticalSpacing = 1;
      this.SchemasList.ScaleImages = false;
      this.SchemasList.ShowNodeToolTips = true;
      this.SchemasList.Size = new System.Drawing.Size(242, 325);
      this.SchemasList.TabIndex = 1;
      this.SchemasList.TextHorizontalOffset = 3;
      this.SchemasList.TitleColorOpacity = 0.8D;
      this.SchemasList.TitleTextVerticalOffset = 0;
      this.SchemasList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SchemasList_AfterSelect);
      this.SchemasList.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.SchemasList_NodeMouseDoubleClick);
      // 
      // SchemasContextMenuStrip
      // 
      this.SchemasContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DisplaySchemaCollationsToolStripMenuItem,
            this.RefreshSchemasToolStripMenuItem});
      this.SchemasContextMenuStrip.Name = "contextMenuStrip";
      this.SchemasContextMenuStrip.Size = new System.Drawing.Size(214, 48);
      // 
      // DisplaySchemaCollationsToolStripMenuItem
      // 
      this.DisplaySchemaCollationsToolStripMenuItem.CheckOnClick = true;
      this.DisplaySchemaCollationsToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_SchemaPanel_SchemaInspect_32x32;
      this.DisplaySchemaCollationsToolStripMenuItem.Name = "DisplaySchemaCollationsToolStripMenuItem";
      this.DisplaySchemaCollationsToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
      this.DisplaySchemaCollationsToolStripMenuItem.Text = "Display Schema Collations";
      this.DisplaySchemaCollationsToolStripMenuItem.Click += new System.EventHandler(this.DisplaySchemaCollationsToolStripMenuItem_Click);
      // 
      // RefreshSchemasToolStripMenuItem
      // 
      this.RefreshSchemasToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.refresh_sidebar;
      this.RefreshSchemasToolStripMenuItem.Name = "RefreshSchemasToolStripMenuItem";
      this.RefreshSchemasToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
      this.RefreshSchemasToolStripMenuItem.Text = "Refresh Schemas";
      this.RefreshSchemasToolStripMenuItem.Click += new System.EventHandler(this.RefreshSchemasToolStripMenuItem_Click);
      // 
      // CreateNewSchemaHotLabel
      // 
      this.CreateNewSchemaHotLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.CreateNewSchemaHotLabel.Behavior = MySQL.ForExcel.Controls.HotLabel.BehaviorType.Button;
      this.CreateNewSchemaHotLabel.CheckedImage = null;
      this.CreateNewSchemaHotLabel.Description = "Add a new Database Schema";
      this.CreateNewSchemaHotLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.CreateNewSchemaHotLabel.DescriptionColorOpacity = 0.6D;
      this.CreateNewSchemaHotLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CreateNewSchemaHotLabel.DescriptionShadowOpacity = 0.4D;
      this.CreateNewSchemaHotLabel.DescriptionShadowXOffset = 0;
      this.CreateNewSchemaHotLabel.DescriptionShadowYOffset = 1;
      this.CreateNewSchemaHotLabel.DisabledImage = null;
      this.CreateNewSchemaHotLabel.DrawShadow = true;
      this.CreateNewSchemaHotLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CreateNewSchemaHotLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_SchemaPanel_NewSchema_24x24;
      this.CreateNewSchemaHotLabel.ImagePixelsXOffset = 0;
      this.CreateNewSchemaHotLabel.ImagePixelsYOffset = 0;
      this.CreateNewSchemaHotLabel.Location = new System.Drawing.Point(9, 558);
      this.CreateNewSchemaHotLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.CreateNewSchemaHotLabel.Name = "CreateNewSchemaHotLabel";
      this.CreateNewSchemaHotLabel.Size = new System.Drawing.Size(237, 28);
      this.CreateNewSchemaHotLabel.TabIndex = 2;
      this.CreateNewSchemaHotLabel.Title = "Create New Schema";
      this.CreateNewSchemaHotLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.CreateNewSchemaHotLabel.TitleColorOpacity = 0.95D;
      this.CreateNewSchemaHotLabel.TitleDescriptionPixelsSpacing = 0;
      this.CreateNewSchemaHotLabel.TitleShadowOpacity = 0.2D;
      this.CreateNewSchemaHotLabel.TitleShadowXOffset = 0;
      this.CreateNewSchemaHotLabel.TitleShadowYOffset = 1;
      this.CreateNewSchemaHotLabel.TitleXOffset = 3;
      this.CreateNewSchemaHotLabel.TitleYOffset = 0;
      this.CreateNewSchemaHotLabel.Click += new System.EventHandler(this.CreateNewSchemaHotLabel_Click);
      // 
      // UpperPanel
      // 
      this.UpperPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.UpperPanel.Controls.Add(this.ConnectionInfoLabel);
      this.UpperPanel.Controls.Add(this.ConnectionPictureBox);
      this.UpperPanel.Controls.Add(this.UserPictureBox);
      this.UpperPanel.Controls.Add(this.SeparatorImage);
      this.UpperPanel.Controls.Add(this.UserLabel);
      this.UpperPanel.Controls.Add(this.SchemaFilter);
      this.UpperPanel.Controls.Add(this.SelectSchemaHotLabel);
      this.UpperPanel.Controls.Add(this.InstructionsLabel);
      this.UpperPanel.Controls.Add(this.ConnectionNameLabel);
      this.UpperPanel.Controls.Add(this.MainLogoPictureBox);
      this.UpperPanel.Location = new System.Drawing.Point(0, 0);
      this.UpperPanel.Name = "UpperPanel";
      this.UpperPanel.Size = new System.Drawing.Size(260, 222);
      this.UpperPanel.TabIndex = 0;
      // 
      // ConnectionInfoLabel
      // 
      this.ConnectionInfoLabel.AutoEllipsis = true;
      this.ConnectionInfoLabel.AutoSize = true;
      this.ConnectionInfoLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConnectionInfoLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.ConnectionInfoLabel.Location = new System.Drawing.Point(83, 44);
      this.ConnectionInfoLabel.Name = "ConnectionInfoLabel";
      this.ConnectionInfoLabel.Size = new System.Drawing.Size(91, 13);
      this.ConnectionInfoLabel.TabIndex = 2;
      this.ConnectionInfoLabel.Text = "Connection info";
      // 
      // ConnectionPictureBox
      // 
      this.ConnectionPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.network_light;
      this.ConnectionPictureBox.Location = new System.Drawing.Point(64, 46);
      this.ConnectionPictureBox.Name = "ConnectionPictureBox";
      this.ConnectionPictureBox.Size = new System.Drawing.Size(13, 11);
      this.ConnectionPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.ConnectionPictureBox.TabIndex = 35;
      this.ConnectionPictureBox.TabStop = false;
      // 
      // UserPictureBox
      // 
      this.UserPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.user_light;
      this.UserPictureBox.Location = new System.Drawing.Point(64, 29);
      this.UserPictureBox.Name = "UserPictureBox";
      this.UserPictureBox.Size = new System.Drawing.Size(13, 11);
      this.UserPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.UserPictureBox.TabIndex = 34;
      this.UserPictureBox.TabStop = false;
      // 
      // SeparatorImage
      // 
      this.SeparatorImage.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.SeparatorImage.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Separator;
      this.SeparatorImage.Location = new System.Drawing.Point(9, 136);
      this.SeparatorImage.MaintainAspectRatio = true;
      this.SeparatorImage.Name = "SeparatorImage";
      this.SeparatorImage.Opacity = 0.3F;
      this.SeparatorImage.ScaleImage = false;
      this.SeparatorImage.Size = new System.Drawing.Size(242, 21);
      this.SeparatorImage.TabIndex = 4;
      // 
      // UserLabel
      // 
      this.UserLabel.AutoEllipsis = true;
      this.UserLabel.AutoSize = true;
      this.UserLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UserLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.UserLabel.Location = new System.Drawing.Point(83, 27);
      this.UserLabel.Name = "UserLabel";
      this.UserLabel.Size = new System.Drawing.Size(61, 13);
      this.UserLabel.TabIndex = 1;
      this.UserLabel.Text = "User name";
      // 
      // SchemaFilter
      // 
      this.SchemaFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.SchemaFilter.BackColor = System.Drawing.SystemColors.Window;
      this.SchemaFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.SchemaFilter.ImageXOffset = 3;
      this.SchemaFilter.Location = new System.Drawing.Point(9, 199);
      this.SchemaFilter.Name = "SchemaFilter";
      this.SchemaFilter.NoTextLabel = "Filter Schemas";
      this.SchemaFilter.NoTextLabelColor = System.Drawing.SystemColors.InactiveCaption;
      this.SchemaFilter.ScaleImage = false;
      this.SchemaFilter.SearchFiredOnLeave = false;
      this.SchemaFilter.SearchImage = global::MySQL.ForExcel.Properties.Resources.ExcelAddinFilter;
      this.SchemaFilter.Size = new System.Drawing.Size(242, 21);
      this.SchemaFilter.TabIndex = 6;
      this.SchemaFilter.TextColor = System.Drawing.SystemColors.ControlText;
      this.SchemaFilter.SearchFired += new System.EventHandler(this.SchemaFilter_SearchFired);
      // 
      // SelectSchemaHotLabel
      // 
      this.SelectSchemaHotLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.SelectSchemaHotLabel.Behavior = MySQL.ForExcel.Controls.HotLabel.BehaviorType.Label;
      this.SelectSchemaHotLabel.CheckedImage = null;
      this.SelectSchemaHotLabel.Description = "Then click the [Next>] button below";
      this.SelectSchemaHotLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.SelectSchemaHotLabel.DescriptionColorOpacity = 0.6D;
      this.SelectSchemaHotLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SelectSchemaHotLabel.DescriptionShadowOpacity = 0.4D;
      this.SelectSchemaHotLabel.DescriptionShadowXOffset = 0;
      this.SelectSchemaHotLabel.DescriptionShadowYOffset = 1;
      this.SelectSchemaHotLabel.DisabledImage = null;
      this.SelectSchemaHotLabel.DrawShadow = true;
      this.SelectSchemaHotLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SelectSchemaHotLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_SchemaPanel_Schemas_24x24;
      this.SelectSchemaHotLabel.ImagePixelsXOffset = 0;
      this.SelectSchemaHotLabel.ImagePixelsYOffset = 2;
      this.SelectSchemaHotLabel.Location = new System.Drawing.Point(10, 164);
      this.SelectSchemaHotLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.SelectSchemaHotLabel.Name = "SelectSchemaHotLabel";
      this.SelectSchemaHotLabel.Size = new System.Drawing.Size(237, 28);
      this.SelectSchemaHotLabel.TabIndex = 5;
      this.SelectSchemaHotLabel.Title = "Select a Database Schema";
      this.SelectSchemaHotLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.SelectSchemaHotLabel.TitleColorOpacity = 0.95D;
      this.SelectSchemaHotLabel.TitleDescriptionPixelsSpacing = 0;
      this.SelectSchemaHotLabel.TitleShadowOpacity = 0.2D;
      this.SelectSchemaHotLabel.TitleShadowXOffset = 0;
      this.SelectSchemaHotLabel.TitleShadowYOffset = 1;
      this.SelectSchemaHotLabel.TitleXOffset = 3;
      this.SelectSchemaHotLabel.TitleYOffset = 0;
      // 
      // InstructionsLabel
      // 
      this.InstructionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.InstructionsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.InstructionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.InstructionsLabel.Location = new System.Drawing.Point(10, 73);
      this.InstructionsLabel.Name = "InstructionsLabel";
      this.InstructionsLabel.PixelsSpacingAdjustment = -3;
      this.InstructionsLabel.ShadowColor = System.Drawing.SystemColors.ControlText;
      this.InstructionsLabel.ShadowOpacity = 0.7D;
      this.InstructionsLabel.ShadowPixelsXOffset = 0;
      this.InstructionsLabel.Size = new System.Drawing.Size(241, 53);
      this.InstructionsLabel.TabIndex = 3;
      this.InstructionsLabel.TextOpacity = 0.6D;
      this.InstructionsLabel.TransparentText = "Please select the MySQL schema you want to work with. Each schema can hold a coll" +
    "ection of tables that store data, views that hold selected data and routines tha" +
    "t generate data.";
      // 
      // ConnectionNameLabel
      // 
      this.ConnectionNameLabel.AutoEllipsis = true;
      this.ConnectionNameLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConnectionNameLabel.Location = new System.Drawing.Point(61, 8);
      this.ConnectionNameLabel.Name = "ConnectionNameLabel";
      this.ConnectionNameLabel.Size = new System.Drawing.Size(190, 18);
      this.ConnectionNameLabel.TabIndex = 0;
      this.ConnectionNameLabel.Text = "Connection Name";
      // 
      // MainLogoPictureBox
      // 
      this.MainLogoPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Logo_64x64;
      this.MainLogoPictureBox.Location = new System.Drawing.Point(9, 11);
      this.MainLogoPictureBox.Name = "MainLogoPictureBox";
      this.MainLogoPictureBox.Size = new System.Drawing.Size(64, 64);
      this.MainLogoPictureBox.TabIndex = 30;
      this.MainLogoPictureBox.TabStop = false;
      // 
      // SchemaSelectionPanel
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.Controls.Add(this.UpperPanel);
      this.Controls.Add(this.SchemasList);
      this.Controls.Add(this.CreateNewSchemaHotLabel);
      this.Controls.Add(this.OptionsButton);
      this.Controls.Add(this.NextButton);
      this.Controls.Add(this.BackButton);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "SchemaSelectionPanel";
      this.Size = new System.Drawing.Size(260, 625);
      this.SchemasContextMenuStrip.ResumeLayout(false);
      this.UpperPanel.ResumeLayout(false);
      this.UpperPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.UserPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.MainLogoPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    public System.Windows.Forms.Button BackButton;
    public System.Windows.Forms.Button NextButton;
    private System.Windows.Forms.ImageList LargeImagesList;
    public System.Windows.Forms.Button OptionsButton;
    private HotLabel CreateNewSchemaHotLabel;
    public MySqlListView SchemasList;
    private System.Windows.Forms.ContextMenuStrip SchemasContextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem RefreshSchemasToolStripMenuItem;
    private System.Windows.Forms.Panel UpperPanel;
    private TransparentPictureBox SeparatorImage;
    private System.Windows.Forms.Label UserLabel;
    private SearchEdit SchemaFilter;
    private HotLabel SelectSchemaHotLabel;
    private TransparentLabel InstructionsLabel;
    private System.Windows.Forms.Label ConnectionNameLabel;
    private System.Windows.Forms.PictureBox MainLogoPictureBox;
    private System.Windows.Forms.ToolStripMenuItem DisplaySchemaCollationsToolStripMenuItem;
    private System.Windows.Forms.Label ConnectionInfoLabel;
    private System.Windows.Forms.PictureBox ConnectionPictureBox;
    private System.Windows.Forms.PictureBox UserPictureBox;
  }
}
