// Copyright (c) 2012, 2015, Oracle and/or its affiliates. All rights reserved.
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

namespace MySQL.ForExcel.Forms
{
  partial class ManageConnectionInfosDialog
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
      }

      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageConnectionInfosDialog));
      System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Workbook", System.Windows.Forms.HorizontalAlignment.Left);
      this.FromImageList = new System.Windows.Forms.ImageList(this.components);
      this.ChooseConnectionInfosSubLabel = new System.Windows.Forms.Label();
      this.ChooseConnectionInfosMainLabel = new System.Windows.Forms.Label();
      this.ChooseConnectionInfosPictureBox = new System.Windows.Forms.PictureBox();
      this.DialogAcceptButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.ManageConnectionInformationTitleLabel = new System.Windows.Forms.Label();
      this.ConnectionInfosListView = new System.Windows.Forms.ListView();
      this.SelectionCheckbox = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ConnectionInfoType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.Reference = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.LastAccess = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.TablesViewsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.SelectWorkbookNotFoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.SelectWorkbookConnectionInfosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.SelectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.SelectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.ColorMapMappedPictureBox = new System.Windows.Forms.PictureBox();
      this.ColorMapMappedLabel = new System.Windows.Forms.Label();
      this.ColorMapUnmappedPictureBox = new System.Windows.Forms.PictureBox();
      this.ColorMapUnmappedLabel = new System.Windows.Forms.Label();
      this.ExcelToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.DeleteOrphanedInfosCheckBox = new System.Windows.Forms.CheckBox();
      this.SelectConnectionInfosLinkLabel = new System.Windows.Forms.LinkLabel();
      this.SelectionOptionsGroupBox = new System.Windows.Forms.GroupBox();
      this.SelectConnectionInfosSub2Label = new System.Windows.Forms.Label();
      this.SelectConnectionInfosNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.SelectConnectionInfosSub1Label = new System.Windows.Forms.Label();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ChooseConnectionInfosPictureBox)).BeginInit();
      this.TablesViewsContextMenuStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ColorMapMappedPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ColorMapUnmappedPictureBox)).BeginInit();
      this.SelectionOptionsGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.SelectConnectionInfosNumericUpDown)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.SelectionOptionsGroupBox);
      this.ContentAreaPanel.Controls.Add(this.ColorMapMappedPictureBox);
      this.ContentAreaPanel.Controls.Add(this.ColorMapMappedLabel);
      this.ContentAreaPanel.Controls.Add(this.ColorMapUnmappedPictureBox);
      this.ContentAreaPanel.Controls.Add(this.ColorMapUnmappedLabel);
      this.ContentAreaPanel.Controls.Add(this.ConnectionInfosListView);
      this.ContentAreaPanel.Controls.Add(this.ManageConnectionInformationTitleLabel);
      this.ContentAreaPanel.Controls.Add(this.ChooseConnectionInfosSubLabel);
      this.ContentAreaPanel.Controls.Add(this.ChooseConnectionInfosMainLabel);
      this.ContentAreaPanel.Controls.Add(this.ChooseConnectionInfosPictureBox);
      this.ContentAreaPanel.Size = new System.Drawing.Size(629, 531);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.DialogAcceptButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 486);
      this.CommandAreaPanel.Size = new System.Drawing.Size(629, 45);
      // 
      // FromImageList
      // 
      this.FromImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("FromImageList.ImageStream")));
      this.FromImageList.TransparentColor = System.Drawing.Color.Transparent;
      this.FromImageList.Images.SetKeyName(0, "db.Table.32x32.png");
      this.FromImageList.Images.SetKeyName(1, "db.View.32x32.png");
      // 
      // ChooseConnectionInfosSubLabel
      // 
      this.ChooseConnectionInfosSubLabel.AutoSize = true;
      this.ChooseConnectionInfosSubLabel.BackColor = System.Drawing.Color.Transparent;
      this.ChooseConnectionInfosSubLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ChooseConnectionInfosSubLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ChooseConnectionInfosSubLabel.Location = new System.Drawing.Point(91, 57);
      this.ChooseConnectionInfosSubLabel.Name = "ChooseConnectionInfosSubLabel";
      this.ChooseConnectionInfosSubLabel.Size = new System.Drawing.Size(424, 15);
      this.ChooseConnectionInfosSubLabel.TabIndex = 3;
      this.ChooseConnectionInfosSubLabel.Text = "Select entries that no longer will be used, to be erased from storage definitivel" +
    "y.";
      // 
      // ChooseConnectionInfosMainLabel
      // 
      this.ChooseConnectionInfosMainLabel.AutoSize = true;
      this.ChooseConnectionInfosMainLabel.BackColor = System.Drawing.Color.Transparent;
      this.ChooseConnectionInfosMainLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ChooseConnectionInfosMainLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ChooseConnectionInfosMainLabel.Location = new System.Drawing.Point(91, 40);
      this.ChooseConnectionInfosMainLabel.Name = "ChooseConnectionInfosMainLabel";
      this.ChooseConnectionInfosMainLabel.Size = new System.Drawing.Size(343, 17);
      this.ChooseConnectionInfosMainLabel.TabIndex = 2;
      this.ChooseConnectionInfosMainLabel.Text = "Choose Edit/Import connection information to be deleted";
      // 
      // ChooseConnectionInfosPictureBox
      // 
      this.ChooseConnectionInfosPictureBox.BackColor = System.Drawing.Color.Transparent;
      this.ChooseConnectionInfosPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.select_all;
      this.ChooseConnectionInfosPictureBox.Location = new System.Drawing.Point(39, 40);
      this.ChooseConnectionInfosPictureBox.Name = "ChooseConnectionInfosPictureBox";
      this.ChooseConnectionInfosPictureBox.Size = new System.Drawing.Size(46, 48);
      this.ChooseConnectionInfosPictureBox.TabIndex = 29;
      this.ChooseConnectionInfosPictureBox.TabStop = false;
      // 
      // DialogAcceptButton
      // 
      this.DialogAcceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogAcceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DialogAcceptButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogAcceptButton.Location = new System.Drawing.Point(461, 11);
      this.DialogAcceptButton.Name = "DialogAcceptButton";
      this.DialogAcceptButton.Size = new System.Drawing.Size(75, 23);
      this.DialogAcceptButton.TabIndex = 1;
      this.DialogAcceptButton.Text = "Accept";
      this.DialogAcceptButton.UseVisualStyleBackColor = true;
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogCancelButton.Location = new System.Drawing.Point(542, 11);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 2;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // ManageConnectionInformationTitleLabel
      // 
      this.ManageConnectionInformationTitleLabel.AutoSize = true;
      this.ManageConnectionInformationTitleLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ManageConnectionInformationTitleLabel.ForeColor = System.Drawing.Color.Navy;
      this.ManageConnectionInformationTitleLabel.Location = new System.Drawing.Point(17, 17);
      this.ManageConnectionInformationTitleLabel.Name = "ManageConnectionInformationTitleLabel";
      this.ManageConnectionInformationTitleLabel.Size = new System.Drawing.Size(472, 20);
      this.ManageConnectionInformationTitleLabel.TabIndex = 1;
      this.ManageConnectionInformationTitleLabel.Text = "MySQL for Excel Import and Edit connection information maintenance";
      // 
      // ConnectionInfosListView
      // 
      this.ConnectionInfosListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ConnectionInfosListView.CheckBoxes = true;
      this.ConnectionInfosListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SelectionCheckbox,
            this.ConnectionInfoType,
            this.Reference,
            this.LastAccess});
      this.ConnectionInfosListView.ContextMenuStrip = this.TablesViewsContextMenuStrip;
      this.ConnectionInfosListView.FullRowSelect = true;
      listViewGroup2.Header = "Workbook";
      listViewGroup2.Name = "Workbook";
      this.ConnectionInfosListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup2});
      this.ConnectionInfosListView.HideSelection = false;
      this.ConnectionInfosListView.Location = new System.Drawing.Point(39, 91);
      this.ConnectionInfosListView.Name = "ConnectionInfosListView";
      this.ConnectionInfosListView.Size = new System.Drawing.Size(548, 259);
      this.ConnectionInfosListView.TabIndex = 30;
      this.ExcelToolTip.SetToolTip(this.ConnectionInfosListView, "Select Import and Edit connection information you want to delete by clicking thei" +
        "r checkboxes.");
      this.ConnectionInfosListView.UseCompatibleStateImageBehavior = false;
      this.ConnectionInfosListView.View = System.Windows.Forms.View.Details;
      // 
      // SelectionCheckbox
      // 
      this.SelectionCheckbox.Text = "";
      this.SelectionCheckbox.Width = 22;
      // 
      // ConnectionInfoType
      // 
      this.ConnectionInfoType.Text = "Type";
      this.ConnectionInfoType.Width = 55;
      // 
      // Reference
      // 
      this.Reference.Text = "Reference";
      this.Reference.Width = 333;
      // 
      // LastAccess
      // 
      this.LastAccess.Text = "Last Access";
      this.LastAccess.Width = 116;
      // 
      // TablesViewsContextMenuStrip
      // 
      this.TablesViewsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectWorkbookNotFoundToolStripMenuItem,
            this.SelectWorkbookConnectionInfosToolStripMenuItem,
            this.SelectAllToolStripMenuItem,
            this.SelectNoneToolStripMenuItem});
      this.TablesViewsContextMenuStrip.Name = "TablesViewsContextMenuStrip";
      this.TablesViewsContextMenuStrip.Size = new System.Drawing.Size(334, 92);
      this.TablesViewsContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.TablesViewsContextMenuStrip_Opening);
      // 
      // SelectWorkbookNotFoundToolStripMenuItem
      // 
      this.SelectWorkbookNotFoundToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.select_all;
      this.SelectWorkbookNotFoundToolStripMenuItem.Name = "SelectWorkbookNotFoundToolStripMenuItem";
      this.SelectWorkbookNotFoundToolStripMenuItem.Size = new System.Drawing.Size(333, 22);
      this.SelectWorkbookNotFoundToolStripMenuItem.Text = "Select all where Workbook is not found";
      this.SelectWorkbookNotFoundToolStripMenuItem.Click += new System.EventHandler(this.SelectWorkbookNotFoundToolStripMenuItem_Click);
      // 
      // SelectWorkbookConnectionInfosToolStripMenuItem
      // 
      this.SelectWorkbookConnectionInfosToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.select_all;
      this.SelectWorkbookConnectionInfosToolStripMenuItem.Name = "SelectWorkbookConnectionInfosToolStripMenuItem";
      this.SelectWorkbookConnectionInfosToolStripMenuItem.Size = new System.Drawing.Size(333, 22);
      this.SelectWorkbookConnectionInfosToolStripMenuItem.Text = "Select connections information within Workbook";
      this.SelectWorkbookConnectionInfosToolStripMenuItem.Click += new System.EventHandler(this.SelectWorkbookConnectionInfosToolStripMenuItem_Click);
      // 
      // SelectAllToolStripMenuItem
      // 
      this.SelectAllToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.select_all;
      this.SelectAllToolStripMenuItem.Name = "SelectAllToolStripMenuItem";
      this.SelectAllToolStripMenuItem.Size = new System.Drawing.Size(333, 22);
      this.SelectAllToolStripMenuItem.Text = "Select All";
      this.SelectAllToolStripMenuItem.Click += new System.EventHandler(this.SelectAllToolStripMenuItem_Click);
      // 
      // SelectNoneToolStripMenuItem
      // 
      this.SelectNoneToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.select_none;
      this.SelectNoneToolStripMenuItem.Name = "SelectNoneToolStripMenuItem";
      this.SelectNoneToolStripMenuItem.Size = new System.Drawing.Size(333, 22);
      this.SelectNoneToolStripMenuItem.Text = "Select None";
      this.SelectNoneToolStripMenuItem.Click += new System.EventHandler(this.SelectNoneToolStripMenuItem_Click);
      // 
      // ColorMapMappedPictureBox
      // 
      this.ColorMapMappedPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ColorMapMappedPictureBox.BackColor = System.Drawing.Color.Red;
      this.ColorMapMappedPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.ColorMapMappedPictureBox.Location = new System.Drawing.Point(186, 356);
      this.ColorMapMappedPictureBox.Name = "ColorMapMappedPictureBox";
      this.ColorMapMappedPictureBox.Size = new System.Drawing.Size(15, 15);
      this.ColorMapMappedPictureBox.TabIndex = 45;
      this.ColorMapMappedPictureBox.TabStop = false;
      // 
      // ColorMapMappedLabel
      // 
      this.ColorMapMappedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ColorMapMappedLabel.AutoSize = true;
      this.ColorMapMappedLabel.BackColor = System.Drawing.Color.Transparent;
      this.ColorMapMappedLabel.ForeColor = System.Drawing.Color.Red;
      this.ColorMapMappedLabel.Location = new System.Drawing.Point(201, 357);
      this.ColorMapMappedLabel.Name = "ColorMapMappedLabel";
      this.ColorMapMappedLabel.Size = new System.Drawing.Size(105, 13);
      this.ColorMapMappedLabel.TabIndex = 43;
      this.ColorMapMappedLabel.Text = "Workbook not found";
      this.ExcelToolTip.SetToolTip(this.ColorMapMappedLabel, "Import/Edit connection information which workbook was not found either because th" +
        "e file was moved or deleted, are marked in red.");
      // 
      // ColorMapUnmappedPictureBox
      // 
      this.ColorMapUnmappedPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ColorMapUnmappedPictureBox.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
      this.ColorMapUnmappedPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.ColorMapUnmappedPictureBox.Location = new System.Drawing.Point(39, 356);
      this.ColorMapUnmappedPictureBox.Name = "ColorMapUnmappedPictureBox";
      this.ColorMapUnmappedPictureBox.Size = new System.Drawing.Size(15, 15);
      this.ColorMapUnmappedPictureBox.TabIndex = 44;
      this.ColorMapUnmappedPictureBox.TabStop = false;
      // 
      // ColorMapUnmappedLabel
      // 
      this.ColorMapUnmappedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ColorMapUnmappedLabel.AutoSize = true;
      this.ColorMapUnmappedLabel.BackColor = System.Drawing.Color.Transparent;
      this.ColorMapUnmappedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ColorMapUnmappedLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
      this.ColorMapUnmappedLabel.Location = new System.Drawing.Point(54, 357);
      this.ColorMapUnmappedLabel.Name = "ColorMapUnmappedLabel";
      this.ColorMapUnmappedLabel.Size = new System.Drawing.Size(110, 13);
      this.ColorMapUnmappedLabel.TabIndex = 42;
      this.ColorMapUnmappedLabel.Text = "Current Workbook";
      this.ExcelToolTip.SetToolTip(this.ColorMapUnmappedLabel, "Import/Edit connection information that belong to the current workbook are marked" +
        " in bold. ");
      // 
      // ExcelToolTip
      // 
      this.ExcelToolTip.AutoPopDelay = 5000;
      this.ExcelToolTip.InitialDelay = 1000;
      this.ExcelToolTip.ReshowDelay = 100;
      // 
      // DeleteOrphanedInfosCheckBox
      // 
      this.DeleteOrphanedInfosCheckBox.AutoSize = true;
      this.DeleteOrphanedInfosCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DeleteOrphanedInfosCheckBox.Location = new System.Drawing.Point(6, 19);
      this.DeleteOrphanedInfosCheckBox.Name = "DeleteOrphanedInfosCheckBox";
      this.DeleteOrphanedInfosCheckBox.Size = new System.Drawing.Size(459, 19);
      this.DeleteOrphanedInfosCheckBox.TabIndex = 46;
      this.DeleteOrphanedInfosCheckBox.Text = "Delete automatically connection information where Workbook is no longer found.";
      this.ExcelToolTip.SetToolTip(this.DeleteOrphanedInfosCheckBox, "Deletes upon startup all connection information related to Excel Workbooks that c" +
        "an\'t be located anymore.");
      this.DeleteOrphanedInfosCheckBox.UseVisualStyleBackColor = true;
      // 
      // SelectConnectionInfosLinkLabel
      // 
      this.SelectConnectionInfosLinkLabel.AutoSize = true;
      this.SelectConnectionInfosLinkLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SelectConnectionInfosLinkLabel.Location = new System.Drawing.Point(6, 41);
      this.SelectConnectionInfosLinkLabel.Name = "SelectConnectionInfosLinkLabel";
      this.SelectConnectionInfosLinkLabel.Size = new System.Drawing.Size(205, 15);
      this.SelectConnectionInfosLinkLabel.TabIndex = 47;
      this.SelectConnectionInfosLinkLabel.TabStop = true;
      this.SelectConnectionInfosLinkLabel.Text = "Select connection information entries";
      this.ExcelToolTip.SetToolTip(this.SelectConnectionInfosLinkLabel, "Selects all saved connection information related to Excel Workbooks that have not" +
        " been accessed in the specified number of days.");
      this.SelectConnectionInfosLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SelectConnectionInfosLinkLabel_LinkClicked);
      // 
      // SelectionOptionsGroupBox
      // 
      this.SelectionOptionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.SelectionOptionsGroupBox.Controls.Add(this.SelectConnectionInfosSub2Label);
      this.SelectionOptionsGroupBox.Controls.Add(this.SelectConnectionInfosNumericUpDown);
      this.SelectionOptionsGroupBox.Controls.Add(this.SelectConnectionInfosSub1Label);
      this.SelectionOptionsGroupBox.Controls.Add(this.SelectConnectionInfosLinkLabel);
      this.SelectionOptionsGroupBox.Controls.Add(this.DeleteOrphanedInfosCheckBox);
      this.SelectionOptionsGroupBox.Location = new System.Drawing.Point(39, 377);
      this.SelectionOptionsGroupBox.Name = "SelectionOptionsGroupBox";
      this.SelectionOptionsGroupBox.Size = new System.Drawing.Size(548, 72);
      this.SelectionOptionsGroupBox.TabIndex = 47;
      this.SelectionOptionsGroupBox.TabStop = false;
      // 
      // SelectConnectionInfosSub2Label
      // 
      this.SelectConnectionInfosSub2Label.AutoSize = true;
      this.SelectConnectionInfosSub2Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SelectConnectionInfosSub2Label.Location = new System.Drawing.Point(463, 41);
      this.SelectConnectionInfosSub2Label.Name = "SelectConnectionInfosSub2Label";
      this.SelectConnectionInfosSub2Label.Size = new System.Drawing.Size(34, 15);
      this.SelectConnectionInfosSub2Label.TabIndex = 50;
      this.SelectConnectionInfosSub2Label.Text = "days.";
      // 
      // SelectConnectionInfosNumericUpDown
      // 
      this.SelectConnectionInfosNumericUpDown.Location = new System.Drawing.Point(421, 40);
      this.SelectConnectionInfosNumericUpDown.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
      this.SelectConnectionInfosNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.SelectConnectionInfosNumericUpDown.Name = "SelectConnectionInfosNumericUpDown";
      this.SelectConnectionInfosNumericUpDown.Size = new System.Drawing.Size(40, 20);
      this.SelectConnectionInfosNumericUpDown.TabIndex = 49;
      this.SelectConnectionInfosNumericUpDown.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
      // 
      // SelectConnectionInfosSub1Label
      // 
      this.SelectConnectionInfosSub1Label.AutoSize = true;
      this.SelectConnectionInfosSub1Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SelectConnectionInfosSub1Label.Location = new System.Drawing.Point(208, 41);
      this.SelectConnectionInfosSub1Label.Name = "SelectConnectionInfosSub1Label";
      this.SelectConnectionInfosSub1Label.Size = new System.Drawing.Size(210, 15);
      this.SelectConnectionInfosSub1Label.TabIndex = 48;
      this.SelectConnectionInfosSub1Label.Text = "that have not been accessed in the last";
      // 
      // ManageConnectionInfosDialog
      // 
      this.AcceptButton = this.DialogAcceptButton;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(629, 531);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MainInstructionLocation = new System.Drawing.Point(10, 14);
      this.MinimumSize = new System.Drawing.Size(645, 570);
      this.Name = "ManageConnectionInfosDialog";
      this.Text = "Manage Import/Edit Connections Information";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ManageConnectionInfosDialog_FormClosing);
      this.Shown += new System.EventHandler(this.ManageConnectionInfosDialog_Shown);
      this.Controls.SetChildIndex(this.FootnoteAreaPanel, 0);
      this.Controls.SetChildIndex(this.ContentAreaPanel, 0);
      this.Controls.SetChildIndex(this.CommandAreaPanel, 0);
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.ChooseConnectionInfosPictureBox)).EndInit();
      this.TablesViewsContextMenuStrip.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.ColorMapMappedPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ColorMapUnmappedPictureBox)).EndInit();
      this.SelectionOptionsGroupBox.ResumeLayout(false);
      this.SelectionOptionsGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.SelectConnectionInfosNumericUpDown)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ImageList FromImageList;
    private System.Windows.Forms.Label ChooseConnectionInfosSubLabel;
    private System.Windows.Forms.Label ChooseConnectionInfosMainLabel;
    private System.Windows.Forms.PictureBox ChooseConnectionInfosPictureBox;
    private System.Windows.Forms.Button DialogAcceptButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.Label ManageConnectionInformationTitleLabel;
    private System.Windows.Forms.ListView ConnectionInfosListView;
    private System.Windows.Forms.ColumnHeader ConnectionInfoType;
    private System.Windows.Forms.ColumnHeader LastAccess;
    private System.Windows.Forms.ColumnHeader SelectionCheckbox;
    private System.Windows.Forms.ColumnHeader Reference;
    private System.Windows.Forms.PictureBox ColorMapMappedPictureBox;
    private System.Windows.Forms.Label ColorMapMappedLabel;
    private System.Windows.Forms.PictureBox ColorMapUnmappedPictureBox;
    private System.Windows.Forms.Label ColorMapUnmappedLabel;
    private System.Windows.Forms.ToolTip ExcelToolTip;
    private System.Windows.Forms.ContextMenuStrip TablesViewsContextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem SelectWorkbookConnectionInfosToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem SelectNoneToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem SelectAllToolStripMenuItem;
    private System.Windows.Forms.CheckBox DeleteOrphanedInfosCheckBox;
    private System.Windows.Forms.ToolStripMenuItem SelectWorkbookNotFoundToolStripMenuItem;
    private System.Windows.Forms.GroupBox SelectionOptionsGroupBox;
    private System.Windows.Forms.Label SelectConnectionInfosSub2Label;
    private System.Windows.Forms.NumericUpDown SelectConnectionInfosNumericUpDown;
    private System.Windows.Forms.Label SelectConnectionInfosSub1Label;
    private System.Windows.Forms.LinkLabel SelectConnectionInfosLinkLabel;

  }
}