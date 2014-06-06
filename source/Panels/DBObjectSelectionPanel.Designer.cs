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
  partial class DbObjectSelectionPanel
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbObjectSelectionPanel));
      this.LargeImagesList = new System.Windows.Forms.ImageList(this.components);
      this.CloseButton = new System.Windows.Forms.Button();
      this.BackButton = new System.Windows.Forms.Button();
      this.OptionsButton = new System.Windows.Forms.Button();
      this.DBObjectList = new MySQL.ForExcel.Controls.MySqlListView();
      this.DBObjectsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.ImportRelatedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.RefreshDatabaseObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.AppendDataHotLabel = new MySQL.ForExcel.Controls.HotLabel();
      this.EditDataHotLabel = new MySQL.ForExcel.Controls.HotLabel();
      this.ImportDataHotLabel = new MySQL.ForExcel.Controls.HotLabel();
      this.UpperPanel = new System.Windows.Forms.Panel();
      this.SchemaLabel = new System.Windows.Forms.Label();
      this.SeparatorImage = new MySQL.ForExcel.Controls.TransparentPictureBox();
      this.UserIPLabel = new System.Windows.Forms.Label();
      this.DBObjectsFilter = new MySQL.ForExcel.Controls.SearchEdit();
      this.SelectDatabaseObjectHotLabel = new MySQL.ForExcel.Controls.HotLabel();
      this.ConnectionNameLabel = new System.Windows.Forms.Label();
      this.ExportToNewTableHotLabel = new MySQL.ForExcel.Controls.HotLabel();
      this.MainLogoPictureBox = new System.Windows.Forms.PictureBox();
      this.ImportMultiHotLabel = new MySQL.ForExcel.Controls.HotLabel();
      this.DBObjectsContextMenuStrip.SuspendLayout();
      this.UpperPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.MainLogoPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // LargeImagesList
      // 
      this.LargeImagesList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("LargeImagesList.ImageStream")));
      this.LargeImagesList.TransparentColor = System.Drawing.Color.Transparent;
      this.LargeImagesList.Images.SetKeyName(0, "MySQLforExcel-ObjectPanel-ListItem-Table-24x24.png");
      this.LargeImagesList.Images.SetKeyName(1, "MySQLforExcel-ObjectPanel-ListItem-View-24x24.png");
      this.LargeImagesList.Images.SetKeyName(2, "MySQLforExcel-ObjectPanel-ListItem-Routine-24x24.png");
      // 
      // CloseButton
      // 
      this.CloseButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.CloseButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CloseButton.Location = new System.Drawing.Point(176, 597);
      this.CloseButton.Name = "CloseButton";
      this.CloseButton.Size = new System.Drawing.Size(75, 23);
      this.CloseButton.TabIndex = 11;
      this.CloseButton.Text = "Close";
      this.CloseButton.UseVisualStyleBackColor = true;
      this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
      // 
      // BackButton
      // 
      this.BackButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.BackButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.BackButton.Location = new System.Drawing.Point(95, 597);
      this.BackButton.Name = "BackButton";
      this.BackButton.Size = new System.Drawing.Size(75, 23);
      this.BackButton.TabIndex = 10;
      this.BackButton.Text = "< Back";
      this.BackButton.UseVisualStyleBackColor = true;
      this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
      // 
      // OptionsButton
      // 
      this.OptionsButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.OptionsButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OptionsButton.Location = new System.Drawing.Point(9, 597);
      this.OptionsButton.Name = "OptionsButton";
      this.OptionsButton.Size = new System.Drawing.Size(75, 23);
      this.OptionsButton.TabIndex = 9;
      this.OptionsButton.Text = "Options";
      this.OptionsButton.UseVisualStyleBackColor = true;
      this.OptionsButton.Click += new System.EventHandler(this.OptionsButton_Click);
      // 
      // DBObjectList
      // 
      this.DBObjectList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.DBObjectList.CollapsedIcon = global::MySQL.ForExcel.Properties.Resources.ArrowRight;
      this.DBObjectList.ContextMenuStrip = this.DBObjectsContextMenuStrip;
      this.DBObjectList.DescriptionColor = System.Drawing.Color.Silver;
      this.DBObjectList.DescriptionColorOpacity = 1D;
      this.DBObjectList.DescriptionFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DBObjectList.DescriptionTextVerticalPixelsOffset = 0;
      this.DBObjectList.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
      this.DBObjectList.ExpandedIcon = global::MySQL.ForExcel.Properties.Resources.ArrowDown;
      this.DBObjectList.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DBObjectList.ImageHorizontalPixelsOffset = 14;
      this.DBObjectList.ImageToTextHorizontalPixelsOffset = 3;
      this.DBObjectList.Indent = 18;
      this.DBObjectList.ItemHeight = 10;
      this.DBObjectList.Location = new System.Drawing.Point(9, 198);
      this.DBObjectList.MultiSelect = true;
      this.DBObjectList.Name = "DBObjectList";
      this.DBObjectList.NodeHeightMultiple = 3;
      this.DBObjectList.NodeImages = this.LargeImagesList;
      this.DBObjectList.ShowNodeToolTips = true;
      this.DBObjectList.Size = new System.Drawing.Size(242, 275);
      this.DBObjectList.TabIndex = 24;
      this.DBObjectList.TitleColorOpacity = 0.8D;
      this.DBObjectList.TitleTextVerticalPixelsOffset = 0;
      this.DBObjectList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.DBObjectList_AfterSelect);
      // 
      // DBObjectsContextMenuStrip
      // 
      this.DBObjectsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ImportRelatedToolStripMenuItem,
            this.RefreshDatabaseObjectsToolStripMenuItem});
      this.DBObjectsContextMenuStrip.Name = "contextMenuStrip";
      this.DBObjectsContextMenuStrip.Size = new System.Drawing.Size(284, 48);
      this.DBObjectsContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.DBObjectsContextMenuStrip_Opening);
      // 
      // ImportRelatedToolStripMenuItem
      // 
      this.ImportRelatedToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_ImportMany_24x24;
      this.ImportRelatedToolStripMenuItem.Name = "ImportRelatedToolStripMenuItem";
      this.ImportRelatedToolStripMenuItem.Size = new System.Drawing.Size(283, 22);
      this.ImportRelatedToolStripMenuItem.Text = "Import Selected and Related DB Objects";
      this.ImportRelatedToolStripMenuItem.Click += new System.EventHandler(this.ImportRelatedToolStripMenuItem_Click);
      // 
      // RefreshDatabaseObjectsToolStripMenuItem
      // 
      this.RefreshDatabaseObjectsToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.refresh_sidebar;
      this.RefreshDatabaseObjectsToolStripMenuItem.Name = "RefreshDatabaseObjectsToolStripMenuItem";
      this.RefreshDatabaseObjectsToolStripMenuItem.Size = new System.Drawing.Size(283, 22);
      this.RefreshDatabaseObjectsToolStripMenuItem.Text = "Refresh Database Objects";
      this.RefreshDatabaseObjectsToolStripMenuItem.Click += new System.EventHandler(this.RefreshDatabaseObjectsToolStripMenuItem_Click);
      // 
      // AppendDataHotLabel
      // 
      this.AppendDataHotLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.AppendDataHotLabel.Behavior = MySQL.ForExcel.Controls.HotLabel.BehaviorType.Button;
      this.AppendDataHotLabel.CheckedImage = null;
      this.AppendDataHotLabel.Description = "Add data to an existing MySQL Table";
      this.AppendDataHotLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.AppendDataHotLabel.DescriptionColorOpacity = 0.6D;
      this.AppendDataHotLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AppendDataHotLabel.DescriptionShadowOpacity = 0.4D;
      this.AppendDataHotLabel.DescriptionShadowPixelsXOffset = 0;
      this.AppendDataHotLabel.DescriptionShadowPixelsYOffset = 1;
      this.AppendDataHotLabel.DisabledImage = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_AppendData_Disabled_24x24;
      this.AppendDataHotLabel.DrawShadow = true;
      this.AppendDataHotLabel.Enabled = false;
      this.AppendDataHotLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AppendDataHotLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_AppendData_24x24;
      this.AppendDataHotLabel.ImagePixelsXOffset = 0;
      this.AppendDataHotLabel.ImagePixelsYOffset = 1;
      this.AppendDataHotLabel.Location = new System.Drawing.Point(9, 556);
      this.AppendDataHotLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.AppendDataHotLabel.Name = "AppendDataHotLabel";
      this.AppendDataHotLabel.Size = new System.Drawing.Size(237, 28);
      this.AppendDataHotLabel.TabIndex = 18;
      this.AppendDataHotLabel.Title = "Append Excel Data to Table";
      this.AppendDataHotLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.AppendDataHotLabel.TitleColorOpacity = 0.95D;
      this.AppendDataHotLabel.TitleDescriptionPixelsSpacing = 0;
      this.AppendDataHotLabel.TitlePixelsXOffset = 3;
      this.AppendDataHotLabel.TitlePixelsYOffset = 0;
      this.AppendDataHotLabel.TitleShadowOpacity = 0.2D;
      this.AppendDataHotLabel.TitleShadowPixelsXOffset = 0;
      this.AppendDataHotLabel.TitleShadowPixelsYOffset = 1;
      this.AppendDataHotLabel.Click += new System.EventHandler(this.AppendDataHotLabel_Click);
      // 
      // EditDataHotLabel
      // 
      this.EditDataHotLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.EditDataHotLabel.Behavior = MySQL.ForExcel.Controls.HotLabel.BehaviorType.Button;
      this.EditDataHotLabel.CheckedImage = null;
      this.EditDataHotLabel.Description = "Open a new sheet to edit table data";
      this.EditDataHotLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.EditDataHotLabel.DescriptionColorOpacity = 0.6D;
      this.EditDataHotLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.EditDataHotLabel.DescriptionShadowOpacity = 0.4D;
      this.EditDataHotLabel.DescriptionShadowPixelsXOffset = 0;
      this.EditDataHotLabel.DescriptionShadowPixelsYOffset = 1;
      this.EditDataHotLabel.DisabledImage = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_EditData_Disabled_24x24;
      this.EditDataHotLabel.DrawShadow = true;
      this.EditDataHotLabel.Enabled = false;
      this.EditDataHotLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.EditDataHotLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_EditData_24x24;
      this.EditDataHotLabel.ImagePixelsXOffset = 0;
      this.EditDataHotLabel.ImagePixelsYOffset = 1;
      this.EditDataHotLabel.Location = new System.Drawing.Point(9, 518);
      this.EditDataHotLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.EditDataHotLabel.Name = "EditDataHotLabel";
      this.EditDataHotLabel.Size = new System.Drawing.Size(237, 28);
      this.EditDataHotLabel.TabIndex = 17;
      this.EditDataHotLabel.Title = "Edit MySQL Data";
      this.EditDataHotLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.EditDataHotLabel.TitleColorOpacity = 0.95D;
      this.EditDataHotLabel.TitleDescriptionPixelsSpacing = 0;
      this.EditDataHotLabel.TitlePixelsXOffset = 3;
      this.EditDataHotLabel.TitlePixelsYOffset = 0;
      this.EditDataHotLabel.TitleShadowOpacity = 0.2D;
      this.EditDataHotLabel.TitleShadowPixelsXOffset = 0;
      this.EditDataHotLabel.TitleShadowPixelsYOffset = 1;
      this.EditDataHotLabel.Click += new System.EventHandler(this.EditDataHotLabel_Click);
      // 
      // ImportDataHotLabel
      // 
      this.ImportDataHotLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ImportDataHotLabel.Behavior = MySQL.ForExcel.Controls.HotLabel.BehaviorType.Button;
      this.ImportDataHotLabel.CheckedImage = null;
      this.ImportDataHotLabel.Description = "Add object\'s data at the current cell";
      this.ImportDataHotLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.ImportDataHotLabel.DescriptionColorOpacity = 0.6D;
      this.ImportDataHotLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ImportDataHotLabel.DescriptionShadowOpacity = 0.4D;
      this.ImportDataHotLabel.DescriptionShadowPixelsXOffset = 0;
      this.ImportDataHotLabel.DescriptionShadowPixelsYOffset = 1;
      this.ImportDataHotLabel.DisabledImage = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_ImportData_Disabled_24x24;
      this.ImportDataHotLabel.DrawShadow = true;
      this.ImportDataHotLabel.Enabled = false;
      this.ImportDataHotLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ImportDataHotLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_ImportData_24x24;
      this.ImportDataHotLabel.ImagePixelsXOffset = 0;
      this.ImportDataHotLabel.ImagePixelsYOffset = 2;
      this.ImportDataHotLabel.Location = new System.Drawing.Point(9, 480);
      this.ImportDataHotLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.ImportDataHotLabel.Name = "ImportDataHotLabel";
      this.ImportDataHotLabel.Size = new System.Drawing.Size(237, 28);
      this.ImportDataHotLabel.TabIndex = 16;
      this.ImportDataHotLabel.Title = "Import MySQL Data";
      this.ImportDataHotLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.ImportDataHotLabel.TitleColorOpacity = 0.95D;
      this.ImportDataHotLabel.TitleDescriptionPixelsSpacing = 0;
      this.ImportDataHotLabel.TitlePixelsXOffset = 3;
      this.ImportDataHotLabel.TitlePixelsYOffset = 0;
      this.ImportDataHotLabel.TitleShadowOpacity = 0.2D;
      this.ImportDataHotLabel.TitleShadowPixelsXOffset = 0;
      this.ImportDataHotLabel.TitleShadowPixelsYOffset = 1;
      this.ImportDataHotLabel.Click += new System.EventHandler(this.ImportDataHotLabel_Click);
      // 
      // UpperPanel
      // 
      this.UpperPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.UpperPanel.Controls.Add(this.SchemaLabel);
      this.UpperPanel.Controls.Add(this.SeparatorImage);
      this.UpperPanel.Controls.Add(this.UserIPLabel);
      this.UpperPanel.Controls.Add(this.DBObjectsFilter);
      this.UpperPanel.Controls.Add(this.SelectDatabaseObjectHotLabel);
      this.UpperPanel.Controls.Add(this.ConnectionNameLabel);
      this.UpperPanel.Controls.Add(this.ExportToNewTableHotLabel);
      this.UpperPanel.Controls.Add(this.MainLogoPictureBox);
      this.UpperPanel.Location = new System.Drawing.Point(0, 0);
      this.UpperPanel.Name = "UpperPanel";
      this.UpperPanel.Size = new System.Drawing.Size(260, 195);
      this.UpperPanel.TabIndex = 28;
      // 
      // SchemaLabel
      // 
      this.SchemaLabel.AutoEllipsis = true;
      this.SchemaLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SchemaLabel.Location = new System.Drawing.Point(61, 45);
      this.SchemaLabel.Name = "SchemaLabel";
      this.SchemaLabel.Size = new System.Drawing.Size(190, 18);
      this.SchemaLabel.TabIndex = 35;
      this.SchemaLabel.Text = "Schema: ??";
      // 
      // SeparatorImage
      // 
      this.SeparatorImage.BackColor = System.Drawing.Color.Transparent;
      this.SeparatorImage.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Separator;
      this.SeparatorImage.Location = new System.Drawing.Point(9, 108);
      this.SeparatorImage.Name = "SeparatorImage";
      this.SeparatorImage.Opacity = 0.3F;
      this.SeparatorImage.Size = new System.Drawing.Size(242, 22);
      this.SeparatorImage.TabIndex = 34;
      // 
      // UserIPLabel
      // 
      this.UserIPLabel.AutoEllipsis = true;
      this.UserIPLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UserIPLabel.Location = new System.Drawing.Point(61, 28);
      this.UserIPLabel.Name = "UserIPLabel";
      this.UserIPLabel.Size = new System.Drawing.Size(190, 18);
      this.UserIPLabel.TabIndex = 29;
      this.UserIPLabel.Text = "User: ??, IP: ??";
      // 
      // DBObjectsFilter
      // 
      this.DBObjectsFilter.BackColor = System.Drawing.SystemColors.Window;
      this.DBObjectsFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.DBObjectsFilter.Location = new System.Drawing.Point(9, 171);
      this.DBObjectsFilter.Name = "DBObjectsFilter";
      this.DBObjectsFilter.NoTextLabel = "Filter Schema Objects";
      this.DBObjectsFilter.SearchImage = global::MySQL.ForExcel.Properties.Resources.ExcelAddinFilter;
      this.DBObjectsFilter.Size = new System.Drawing.Size(242, 21);
      this.DBObjectsFilter.TabIndex = 33;
      this.DBObjectsFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DBObjectsFilter_KeyDown);
      // 
      // SelectDatabaseObjectHotLabel
      // 
      this.SelectDatabaseObjectHotLabel.Behavior = MySQL.ForExcel.Controls.HotLabel.BehaviorType.Label;
      this.SelectDatabaseObjectHotLabel.CheckedImage = null;
      this.SelectDatabaseObjectHotLabel.Description = "Use CTRL or SHIFT for multiple selection.";
      this.SelectDatabaseObjectHotLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.SelectDatabaseObjectHotLabel.DescriptionColorOpacity = 0.6D;
      this.SelectDatabaseObjectHotLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SelectDatabaseObjectHotLabel.DescriptionShadowOpacity = 0.4D;
      this.SelectDatabaseObjectHotLabel.DescriptionShadowPixelsXOffset = 0;
      this.SelectDatabaseObjectHotLabel.DescriptionShadowPixelsYOffset = 1;
      this.SelectDatabaseObjectHotLabel.DisabledImage = null;
      this.SelectDatabaseObjectHotLabel.DrawShadow = true;
      this.SelectDatabaseObjectHotLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SelectDatabaseObjectHotLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_SelectObject_24x24;
      this.SelectDatabaseObjectHotLabel.ImagePixelsXOffset = 0;
      this.SelectDatabaseObjectHotLabel.ImagePixelsYOffset = 2;
      this.SelectDatabaseObjectHotLabel.Location = new System.Drawing.Point(9, 136);
      this.SelectDatabaseObjectHotLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.SelectDatabaseObjectHotLabel.Name = "SelectDatabaseObjectHotLabel";
      this.SelectDatabaseObjectHotLabel.Size = new System.Drawing.Size(242, 30);
      this.SelectDatabaseObjectHotLabel.TabIndex = 32;
      this.SelectDatabaseObjectHotLabel.Title = "Select Database Objects";
      this.SelectDatabaseObjectHotLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.SelectDatabaseObjectHotLabel.TitleColorOpacity = 0.95D;
      this.SelectDatabaseObjectHotLabel.TitleDescriptionPixelsSpacing = 0;
      this.SelectDatabaseObjectHotLabel.TitlePixelsXOffset = 3;
      this.SelectDatabaseObjectHotLabel.TitlePixelsYOffset = 0;
      this.SelectDatabaseObjectHotLabel.TitleShadowOpacity = 0.2D;
      this.SelectDatabaseObjectHotLabel.TitleShadowPixelsXOffset = 0;
      this.SelectDatabaseObjectHotLabel.TitleShadowPixelsYOffset = 1;
      // 
      // ConnectionNameLabel
      // 
      this.ConnectionNameLabel.AutoEllipsis = true;
      this.ConnectionNameLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConnectionNameLabel.Location = new System.Drawing.Point(61, 8);
      this.ConnectionNameLabel.Name = "ConnectionNameLabel";
      this.ConnectionNameLabel.Size = new System.Drawing.Size(190, 18);
      this.ConnectionNameLabel.TabIndex = 28;
      this.ConnectionNameLabel.Text = "Connection Name";
      // 
      // ExportToNewTableHotLabel
      // 
      this.ExportToNewTableHotLabel.Behavior = MySQL.ForExcel.Controls.HotLabel.BehaviorType.Button;
      this.ExportToNewTableHotLabel.CheckedImage = null;
      this.ExportToNewTableHotLabel.Description = "Create a new table and fill it with data";
      this.ExportToNewTableHotLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.ExportToNewTableHotLabel.DescriptionColorOpacity = 0.6D;
      this.ExportToNewTableHotLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExportToNewTableHotLabel.DescriptionShadowOpacity = 0.4D;
      this.ExportToNewTableHotLabel.DescriptionShadowPixelsXOffset = 0;
      this.ExportToNewTableHotLabel.DescriptionShadowPixelsYOffset = 1;
      this.ExportToNewTableHotLabel.DisabledImage = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_ExportToMySQL_Disabled_24x24;
      this.ExportToNewTableHotLabel.DrawShadow = true;
      this.ExportToNewTableHotLabel.Enabled = false;
      this.ExportToNewTableHotLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExportToNewTableHotLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_ExportToMySQL_24x24;
      this.ExportToNewTableHotLabel.ImagePixelsXOffset = 0;
      this.ExportToNewTableHotLabel.ImagePixelsYOffset = 0;
      this.ExportToNewTableHotLabel.Location = new System.Drawing.Point(9, 73);
      this.ExportToNewTableHotLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.ExportToNewTableHotLabel.Name = "ExportToNewTableHotLabel";
      this.ExportToNewTableHotLabel.Size = new System.Drawing.Size(242, 28);
      this.ExportToNewTableHotLabel.TabIndex = 31;
      this.ExportToNewTableHotLabel.Title = "Export Excel Data to New Table";
      this.ExportToNewTableHotLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.ExportToNewTableHotLabel.TitleColorOpacity = 0.95D;
      this.ExportToNewTableHotLabel.TitleDescriptionPixelsSpacing = 0;
      this.ExportToNewTableHotLabel.TitlePixelsXOffset = 3;
      this.ExportToNewTableHotLabel.TitlePixelsYOffset = 0;
      this.ExportToNewTableHotLabel.TitleShadowOpacity = 0.2D;
      this.ExportToNewTableHotLabel.TitleShadowPixelsXOffset = 0;
      this.ExportToNewTableHotLabel.TitleShadowPixelsYOffset = 1;
      this.ExportToNewTableHotLabel.Click += new System.EventHandler(this.ExportToNewTableHotLabel_Click);
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
      // ImportMultiHotLabel
      // 
      this.ImportMultiHotLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ImportMultiHotLabel.Behavior = MySQL.ForExcel.Controls.HotLabel.BehaviorType.Button;
      this.ImportMultiHotLabel.CheckedImage = null;
      this.ImportMultiHotLabel.Description = "Add each object\'s data to new sheets";
      this.ImportMultiHotLabel.DescriptionColor = System.Drawing.SystemColors.WindowText;
      this.ImportMultiHotLabel.DescriptionColorOpacity = 0.6D;
      this.ImportMultiHotLabel.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ImportMultiHotLabel.DescriptionShadowOpacity = 0.4D;
      this.ImportMultiHotLabel.DescriptionShadowPixelsXOffset = 0;
      this.ImportMultiHotLabel.DescriptionShadowPixelsYOffset = 1;
      this.ImportMultiHotLabel.DisabledImage = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_ImportManyDisabled_24x24;
      this.ImportMultiHotLabel.DrawShadow = true;
      this.ImportMultiHotLabel.Enabled = false;
      this.ImportMultiHotLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ImportMultiHotLabel.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ObjectPanel_ImportMany_24x24;
      this.ImportMultiHotLabel.ImagePixelsXOffset = 0;
      this.ImportMultiHotLabel.ImagePixelsYOffset = 2;
      this.ImportMultiHotLabel.Location = new System.Drawing.Point(9, 480);
      this.ImportMultiHotLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.ImportMultiHotLabel.Name = "ImportMultiHotLabel";
      this.ImportMultiHotLabel.Size = new System.Drawing.Size(237, 28);
      this.ImportMultiHotLabel.TabIndex = 29;
      this.ImportMultiHotLabel.Title = "Import Multiple Tables and Views";
      this.ImportMultiHotLabel.TitleColor = System.Drawing.SystemColors.WindowText;
      this.ImportMultiHotLabel.TitleColorOpacity = 0.95D;
      this.ImportMultiHotLabel.TitleDescriptionPixelsSpacing = 0;
      this.ImportMultiHotLabel.TitlePixelsXOffset = 3;
      this.ImportMultiHotLabel.TitlePixelsYOffset = 0;
      this.ImportMultiHotLabel.TitleShadowOpacity = 0.2D;
      this.ImportMultiHotLabel.TitleShadowPixelsXOffset = 0;
      this.ImportMultiHotLabel.TitleShadowPixelsYOffset = 1;
      this.ImportMultiHotLabel.Visible = false;
      this.ImportMultiHotLabel.Click += new System.EventHandler(this.ImportMultiHotLabel_Click);
      // 
      // DbObjectSelectionPanel
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add(this.UpperPanel);
      this.Controls.Add(this.AppendDataHotLabel);
      this.Controls.Add(this.DBObjectList);
      this.Controls.Add(this.ImportDataHotLabel);
      this.Controls.Add(this.OptionsButton);
      this.Controls.Add(this.EditDataHotLabel);
      this.Controls.Add(this.CloseButton);
      this.Controls.Add(this.BackButton);
      this.Controls.Add(this.ImportMultiHotLabel);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "DbObjectSelectionPanel";
      this.Size = new System.Drawing.Size(260, 625);
      this.DBObjectsContextMenuStrip.ResumeLayout(false);
      this.UpperPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.MainLogoPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    public System.Windows.Forms.Button CloseButton;
    public System.Windows.Forms.Button BackButton;
    private System.Windows.Forms.ImageList LargeImagesList;
    public System.Windows.Forms.Button OptionsButton;
    private HotLabel ImportDataHotLabel;
    private HotLabel EditDataHotLabel;
    private HotLabel AppendDataHotLabel;
    public MySqlListView DBObjectList;
    private System.Windows.Forms.ContextMenuStrip DBObjectsContextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem RefreshDatabaseObjectsToolStripMenuItem;
    private System.Windows.Forms.Panel UpperPanel;
    private TransparentPictureBox SeparatorImage;
    private System.Windows.Forms.Label UserIPLabel;
    private SearchEdit DBObjectsFilter;
    private HotLabel SelectDatabaseObjectHotLabel;
    private System.Windows.Forms.Label ConnectionNameLabel;
    private HotLabel ExportToNewTableHotLabel;
    private System.Windows.Forms.PictureBox MainLogoPictureBox;
    private System.Windows.Forms.Label SchemaLabel;
    private HotLabel ImportMultiHotLabel;
    private System.Windows.Forms.ToolStripMenuItem ImportRelatedToolStripMenuItem;
  }
}
