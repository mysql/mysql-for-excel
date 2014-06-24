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
  partial class ManageSessionsDialog
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageSessionsDialog));
      System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Workbook", System.Windows.Forms.HorizontalAlignment.Left);
      this.FromImageList = new System.Windows.Forms.ImageList(this.components);
      this.ChooseSessionsSubLabel = new System.Windows.Forms.Label();
      this.ChooseSessionsMainLabel = new System.Windows.Forms.Label();
      this.ChooseSessionsPictureBox = new System.Windows.Forms.PictureBox();
      this.DeleteSelectedButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.ExportDataLabel = new System.Windows.Forms.Label();
      this.SessionsListView = new System.Windows.Forms.ListView();
      this.SelectionCheckbox = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.SessionType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.Reference = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.LastAccess = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.TablesViewsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.SelectWorkbookSessionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.SelectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.ColorMapMappedPictureBox = new System.Windows.Forms.PictureBox();
      this.ColorMapMappedLabel = new System.Windows.Forms.Label();
      this.ColorMapUnmappedPictureBox = new System.Windows.Forms.PictureBox();
      this.ColorMapUnmappedLabel = new System.Windows.Forms.Label();
      this.ExcelToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ChooseSessionsPictureBox)).BeginInit();
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
      this.ContentAreaPanel.Controls.Add(this.SessionsListView);
      this.ContentAreaPanel.Controls.Add(this.ExportDataLabel);
      this.ContentAreaPanel.Controls.Add(this.ChooseSessionsSubLabel);
      this.ContentAreaPanel.Controls.Add(this.ChooseSessionsMainLabel);
      this.ContentAreaPanel.Controls.Add(this.ChooseSessionsPictureBox);
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
      // ChooseSessionsSubLabel
      // 
      this.ChooseSessionsSubLabel.AutoSize = true;
      this.ChooseSessionsSubLabel.BackColor = System.Drawing.Color.Transparent;
      this.ChooseSessionsSubLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ChooseSessionsSubLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ChooseSessionsSubLabel.Location = new System.Drawing.Point(91, 57);
      this.ChooseSessionsSubLabel.Name = "ChooseSessionsSubLabel";
      this.ChooseSessionsSubLabel.Size = new System.Drawing.Size(446, 15);
      this.ChooseSessionsSubLabel.TabIndex = 3;
      this.ChooseSessionsSubLabel.Text = "Click on sessions that will no longer be used to be deleted from storage definiti" +
    "vely.";
      // 
      // ChooseSessionsMainLabel
      // 
      this.ChooseSessionsMainLabel.AutoSize = true;
      this.ChooseSessionsMainLabel.BackColor = System.Drawing.Color.Transparent;
      this.ChooseSessionsMainLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ChooseSessionsMainLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ChooseSessionsMainLabel.Location = new System.Drawing.Point(91, 40);
      this.ChooseSessionsMainLabel.Name = "ChooseSessionsMainLabel";
      this.ChooseSessionsMainLabel.Size = new System.Drawing.Size(188, 17);
      this.ChooseSessionsMainLabel.TabIndex = 2;
      this.ChooseSessionsMainLabel.Text = "Choose sessions to be deleted";
      // 
      // ChooseSessionsPictureBox
      // 
      this.ChooseSessionsPictureBox.BackColor = System.Drawing.Color.Transparent;
      this.ChooseSessionsPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.select_all;
      this.ChooseSessionsPictureBox.Location = new System.Drawing.Point(39, 40);
      this.ChooseSessionsPictureBox.Name = "ChooseSessionsPictureBox";
      this.ChooseSessionsPictureBox.Size = new System.Drawing.Size(46, 48);
      this.ChooseSessionsPictureBox.TabIndex = 29;
      this.ChooseSessionsPictureBox.TabStop = false;
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
      // ExportDataLabel
      // 
      this.ExportDataLabel.AutoSize = true;
      this.ExportDataLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExportDataLabel.ForeColor = System.Drawing.Color.Navy;
      this.ExportDataLabel.Location = new System.Drawing.Point(17, 17);
      this.ExportDataLabel.Name = "ExportDataLabel";
      this.ExportDataLabel.Size = new System.Drawing.Size(372, 20);
      this.ExportDataLabel.TabIndex = 1;
      this.ExportDataLabel.Text = "MySQL for Excel Import and Edit Sessions maintenance";
      // 
      // SessionsListView
      // 
      this.SessionsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.SessionsListView.CheckBoxes = true;
      this.SessionsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SelectionCheckbox,
            this.SessionType,
            this.Reference,
            this.LastAccess});
      this.SessionsListView.ContextMenuStrip = this.TablesViewsContextMenuStrip;
      this.SessionsListView.FullRowSelect = true;
      listViewGroup1.Header = "Workbook";
      listViewGroup1.Name = "Workbook";
      this.SessionsListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
      this.SessionsListView.HideSelection = false;
      this.SessionsListView.Location = new System.Drawing.Point(39, 91);
      this.SessionsListView.Name = "SessionsListView";
      this.SessionsListView.Size = new System.Drawing.Size(548, 259);
      this.SessionsListView.TabIndex = 30;
      this.ExcelToolTip.SetToolTip(this.SessionsListView, "Select Import and Edit Sessions  you want to delete by clicking their checkboxes." +
        "");
      this.SessionsListView.UseCompatibleStateImageBehavior = false;
      this.SessionsListView.View = System.Windows.Forms.View.Details;
      // 
      // SelectionCheckbox
      // 
      this.SelectionCheckbox.Text = "";
      this.SelectionCheckbox.Width = 22;
      // 
      // SessionType
      // 
      this.SessionType.Text = "Type";
      this.SessionType.Width = 55;
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
            this.SelectWorkbookSessionsToolStripMenuItem,
            this.SelectNoneToolStripMenuItem});
      this.TablesViewsContextMenuStrip.Name = "TablesViewsContextMenuStrip";
      this.TablesViewsContextMenuStrip.Size = new System.Drawing.Size(211, 70);
      // 
      // SelectWorkbookSessionsToolStripMenuItem
      // 
      this.SelectWorkbookSessionsToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.select_all;
      this.SelectWorkbookSessionsToolStripMenuItem.Name = "SelectWorkbookSessionsToolStripMenuItem";
      this.SelectWorkbookSessionsToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
      this.SelectWorkbookSessionsToolStripMenuItem.Text = "Select Workbook Sessions";
      this.SelectWorkbookSessionsToolStripMenuItem.Click += new System.EventHandler(this.SelectWorkbookSessionsToolStripMenuItem_Click);
      // 
      // SelectNoneToolStripMenuItem
      // 
      this.SelectNoneToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.select_none;
      this.SelectNoneToolStripMenuItem.Name = "SelectNoneToolStripMenuItem";
      this.SelectNoneToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
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
      this.ExcelToolTip.SetToolTip(this.ColorMapMappedLabel, "Sessions which workbook was not found either because the file was moved or delete" +
        "d, are marked in red.");
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
      this.ExcelToolTip.SetToolTip(this.ColorMapUnmappedLabel, "Sessions that belong to the current workbook are marked in bold. ");
      // 
      // ExcelToolTip
      // 
      this.ExcelToolTip.AutoPopDelay = 5000;
      this.ExcelToolTip.InitialDelay = 1000;
      this.ExcelToolTip.ReshowDelay = 100;
      // 
      // ManageSessionsDialog
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
      this.Name = "ManageSessionsDialog";
      this.Text = "Manage Sessions";
      this.Controls.SetChildIndex(this.FootnoteAreaPanel, 0);
      this.Controls.SetChildIndex(this.ContentAreaPanel, 0);
      this.Controls.SetChildIndex(this.CommandAreaPanel, 0);
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.ChooseSessionsPictureBox)).EndInit();
      this.TablesViewsContextMenuStrip.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.ColorMapMappedPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ColorMapUnmappedPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ImageList FromImageList;
    private System.Windows.Forms.Label ChooseSessionsSubLabel;
    private System.Windows.Forms.Label ChooseSessionsMainLabel;
    private System.Windows.Forms.PictureBox ChooseSessionsPictureBox;
    private System.Windows.Forms.Button DeleteSelectedButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.Label ExportDataLabel;
    private System.Windows.Forms.ListView SessionsListView;
    private System.Windows.Forms.ColumnHeader SessionType;
    private System.Windows.Forms.ColumnHeader LastAccess;
    private System.Windows.Forms.ColumnHeader SelectionCheckbox;
    private System.Windows.Forms.ColumnHeader Reference;
    private System.Windows.Forms.PictureBox ColorMapMappedPictureBox;
    private System.Windows.Forms.Label ColorMapMappedLabel;
    private System.Windows.Forms.PictureBox ColorMapUnmappedPictureBox;
    private System.Windows.Forms.Label ColorMapUnmappedLabel;
    private System.Windows.Forms.ToolTip ExcelToolTip;
    private System.Windows.Forms.ContextMenuStrip TablesViewsContextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem SelectWorkbookSessionsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem SelectNoneToolStripMenuItem;

  }
}