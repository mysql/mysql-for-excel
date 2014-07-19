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
      System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Workbook", System.Windows.Forms.HorizontalAlignment.Left);
      this.FromImageList = new System.Windows.Forms.ImageList(this.components);
      this.ChooseConnectionInfosSubLabel = new System.Windows.Forms.Label();
      this.ChooseConnectionInfosMainLabel = new System.Windows.Forms.Label();
      this.ChooseConnectionInfosPictureBox = new System.Windows.Forms.PictureBox();
      this.DeleteSelectedButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.ManageConnectionInformationTitleLabel = new System.Windows.Forms.Label();
      this.ConnectionInfosListView = new System.Windows.Forms.ListView();
      this.SelectionCheckbox = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ConnectionInfoType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.Reference = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.LastAccess = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.TablesViewsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.SelectWorkbookConnectionInfosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.SelectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.ColorMapMappedPictureBox = new System.Windows.Forms.PictureBox();
      this.ColorMapMappedLabel = new System.Windows.Forms.Label();
      this.ColorMapUnmappedPictureBox = new System.Windows.Forms.PictureBox();
      this.ColorMapUnmappedLabel = new System.Windows.Forms.Label();
      this.ExcelToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ChooseConnectionInfosPictureBox)).BeginInit();
      this.TablesViewsContextMenuStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ColorMapMappedPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ColorMapUnmappedPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.ColorMapMappedPictureBox);
      this.ContentAreaPanel.Controls.Add(this.ColorMapMappedLabel);
      this.ContentAreaPanel.Controls.Add(this.ColorMapUnmappedPictureBox);
      this.ContentAreaPanel.Controls.Add(this.ColorMapUnmappedLabel);
      this.ContentAreaPanel.Controls.Add(this.ConnectionInfosListView);
      this.ContentAreaPanel.Controls.Add(this.ManageConnectionInformationTitleLabel);
      this.ContentAreaPanel.Controls.Add(this.ChooseConnectionInfosSubLabel);
      this.ContentAreaPanel.Controls.Add(this.ChooseConnectionInfosMainLabel);
      this.ContentAreaPanel.Controls.Add(this.ChooseConnectionInfosPictureBox);
      this.ContentAreaPanel.Size = new System.Drawing.Size(629, 461);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.DeleteSelectedButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 416);
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
      // DeleteSelectedButton
      // 
      this.DeleteSelectedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DeleteSelectedButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DeleteSelectedButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DeleteSelectedButton.Location = new System.Drawing.Point(412, 11);
      this.DeleteSelectedButton.Name = "DeleteSelectedButton";
      this.DeleteSelectedButton.Size = new System.Drawing.Size(124, 23);
      this.DeleteSelectedButton.TabIndex = 1;
      this.DeleteSelectedButton.Text = "Delete Selected";
      this.DeleteSelectedButton.UseVisualStyleBackColor = true;
      this.DeleteSelectedButton.Click += new System.EventHandler(this.DeleteSelectedButton_Click);
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
      listViewGroup1.Header = "Workbook";
      listViewGroup1.Name = "Workbook";
      this.ConnectionInfosListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
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
            this.SelectWorkbookConnectionInfosToolStripMenuItem,
            this.SelectNoneToolStripMenuItem});
      this.TablesViewsContextMenuStrip.Name = "TablesViewsContextMenuStrip";
      this.TablesViewsContextMenuStrip.Size = new System.Drawing.Size(357, 48);
      // 
      // SelectWorkbookConnectionInfosToolStripMenuItem
      // 
      this.SelectWorkbookConnectionInfosToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.select_all;
      this.SelectWorkbookConnectionInfosToolStripMenuItem.Name = "SelectWorkbookConnectionInfosToolStripMenuItem";
      this.SelectWorkbookConnectionInfosToolStripMenuItem.Size = new System.Drawing.Size(356, 22);
      this.SelectWorkbookConnectionInfosToolStripMenuItem.Text = "Select Workbook Import/Edit connection information";
      this.SelectWorkbookConnectionInfosToolStripMenuItem.Click += new System.EventHandler(this.SelectWorkbookConnectionInfosToolStripMenuItem_Click);
      // 
      // SelectNoneToolStripMenuItem
      // 
      this.SelectNoneToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.select_none;
      this.SelectNoneToolStripMenuItem.Name = "SelectNoneToolStripMenuItem";
      this.SelectNoneToolStripMenuItem.Size = new System.Drawing.Size(356, 22);
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
      this.ColorMapMappedLabel.Location = new System.Drawing.Point(201, 356);
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
      this.ColorMapUnmappedLabel.Location = new System.Drawing.Point(54, 356);
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
      // ManageConnectionInfosDialog
      // 
      this.AcceptButton = this.DeleteSelectedButton;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(629, 461);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MainInstructionLocation = new System.Drawing.Point(10, 14);
      this.MinimumSize = new System.Drawing.Size(645, 500);
      this.Name = "ManageConnectionInfosDialog";
      this.Text = "Manage Import/Edit Connections Information";
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
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ImageList FromImageList;
    private System.Windows.Forms.Label ChooseConnectionInfosSubLabel;
    private System.Windows.Forms.Label ChooseConnectionInfosMainLabel;
    private System.Windows.Forms.PictureBox ChooseConnectionInfosPictureBox;
    private System.Windows.Forms.Button DeleteSelectedButton;
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

  }
}