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
  partial class ImportTableViewForm
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportTableViewForm));
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.fromImageList = new System.Windows.Forms.ImageList(this.components);
      this.lblSubSetOfData = new System.Windows.Forms.Label();
      this.lblRowsCountSub = new System.Windows.Forms.Label();
      this.grpOptions = new System.Windows.Forms.GroupBox();
      this.numFromRow = new System.Windows.Forms.NumericUpDown();
      this.lblRowsToReturn = new System.Windows.Forms.Label();
      this.numRowsToReturn = new System.Windows.Forms.NumericUpDown();
      this.chkLimitRows = new System.Windows.Forms.CheckBox();
      this.chkIncludeHeaders = new System.Windows.Forms.CheckBox();
      this.lblOptionsWarning = new System.Windows.Forms.Label();
      this.picOptionsWarning = new System.Windows.Forms.PictureBox();
      this.grdPreviewData = new MySQL.ForExcel.PreviewDataGridView();
      this.contextMenuForGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.lblPickColumnsSub = new System.Windows.Forms.Label();
      this.lblPickColumnsMain = new System.Windows.Forms.Label();
      this.picColumnOptions = new System.Windows.Forms.PictureBox();
      this.lblRowsCountMain = new System.Windows.Forms.Label();
      this.lblTableNameSub = new System.Windows.Forms.Label();
      this.lblTableNameMain = new System.Windows.Forms.Label();
      this.btnImport = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.lblExportData = new System.Windows.Forms.Label();
      this.contentAreaPanel.SuspendLayout();
      this.commandAreaPanel.SuspendLayout();
      this.grpOptions.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numFromRow)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numRowsToReturn)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picOptionsWarning)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).BeginInit();
      this.contextMenuForGrid.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picColumnOptions)).BeginInit();
      this.SuspendLayout();
      // 
      // contentAreaPanel
      // 
      this.contentAreaPanel.BackColor = System.Drawing.SystemColors.Window;
      this.contentAreaPanel.Controls.Add(this.lblOptionsWarning);
      this.contentAreaPanel.Controls.Add(this.picOptionsWarning);
      this.contentAreaPanel.Controls.Add(this.lblExportData);
      this.contentAreaPanel.Controls.Add(this.lblSubSetOfData);
      this.contentAreaPanel.Controls.Add(this.lblRowsCountSub);
      this.contentAreaPanel.Controls.Add(this.grpOptions);
      this.contentAreaPanel.Controls.Add(this.grdPreviewData);
      this.contentAreaPanel.Controls.Add(this.lblPickColumnsSub);
      this.contentAreaPanel.Controls.Add(this.lblPickColumnsMain);
      this.contentAreaPanel.Controls.Add(this.picColumnOptions);
      this.contentAreaPanel.Controls.Add(this.lblRowsCountMain);
      this.contentAreaPanel.Controls.Add(this.lblTableNameSub);
      this.contentAreaPanel.Controls.Add(this.lblTableNameMain);
      this.contentAreaPanel.Size = new System.Drawing.Size(849, 550);
      // 
      // commandAreaPanel
      // 
      this.commandAreaPanel.Controls.Add(this.btnImport);
      this.commandAreaPanel.Controls.Add(this.btnCancel);
      this.commandAreaPanel.Location = new System.Drawing.Point(0, 550);
      this.commandAreaPanel.Size = new System.Drawing.Size(849, 45);
      // 
      // fromImageList
      // 
      this.fromImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("fromImageList.ImageStream")));
      this.fromImageList.TransparentColor = System.Drawing.Color.Transparent;
      this.fromImageList.Images.SetKeyName(0, "db.Table.32x32.png");
      this.fromImageList.Images.SetKeyName(1, "db.View.32x32.png");
      // 
      // lblSubSetOfData
      // 
      this.lblSubSetOfData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lblSubSetOfData.AutoSize = true;
      this.lblSubSetOfData.BackColor = System.Drawing.Color.Transparent;
      this.lblSubSetOfData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblSubSetOfData.ForeColor = System.Drawing.SystemColors.InactiveCaption;
      this.lblSubSetOfData.Location = new System.Drawing.Point(456, 142);
      this.lblSubSetOfData.Name = "lblSubSetOfData";
      this.lblSubSetOfData.Size = new System.Drawing.Size(319, 15);
      this.lblSubSetOfData.TabIndex = 6;
      this.lblSubSetOfData.Text = "This is a small subset of the data for preview purposes only.";
      // 
      // lblRowsCountSub
      // 
      this.lblRowsCountSub.AutoSize = true;
      this.lblRowsCountSub.BackColor = System.Drawing.Color.Transparent;
      this.lblRowsCountSub.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblRowsCountSub.ForeColor = System.Drawing.Color.Navy;
      this.lblRowsCountSub.Location = new System.Drawing.Point(169, 142);
      this.lblRowsCountSub.Name = "lblRowsCountSub";
      this.lblRowsCountSub.Size = new System.Drawing.Size(13, 15);
      this.lblRowsCountSub.TabIndex = 3;
      this.lblRowsCountSub.Text = "0";
      // 
      // grpOptions
      // 
      this.grpOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.grpOptions.BackColor = System.Drawing.Color.Transparent;
      this.grpOptions.Controls.Add(this.numFromRow);
      this.grpOptions.Controls.Add(this.lblRowsToReturn);
      this.grpOptions.Controls.Add(this.numRowsToReturn);
      this.grpOptions.Controls.Add(this.chkLimitRows);
      this.grpOptions.Controls.Add(this.chkIncludeHeaders);
      this.grpOptions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.grpOptions.Location = new System.Drawing.Point(80, 442);
      this.grpOptions.Name = "grpOptions";
      this.grpOptions.Size = new System.Drawing.Size(695, 60);
      this.grpOptions.TabIndex = 8;
      this.grpOptions.TabStop = false;
      this.grpOptions.Text = "Options";
      // 
      // numFromRow
      // 
      this.numFromRow.Enabled = false;
      this.numFromRow.Location = new System.Drawing.Point(616, 21);
      this.numFromRow.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numFromRow.Name = "numFromRow";
      this.numFromRow.Size = new System.Drawing.Size(60, 23);
      this.numFromRow.TabIndex = 6;
      this.numFromRow.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numFromRow.ValueChanged += new System.EventHandler(this.numFromRow_ValueChanged);
      // 
      // lblRowsToReturn
      // 
      this.lblRowsToReturn.AutoSize = true;
      this.lblRowsToReturn.Location = new System.Drawing.Point(473, 25);
      this.lblRowsToReturn.Name = "lblRowsToReturn";
      this.lblRowsToReturn.Size = new System.Drawing.Size(137, 15);
      this.lblRowsToReturn.TabIndex = 5;
      this.lblRowsToReturn.Text = "Rows and Start with Row";
      // 
      // numRowsToReturn
      // 
      this.numRowsToReturn.Enabled = false;
      this.numRowsToReturn.Location = new System.Drawing.Point(407, 21);
      this.numRowsToReturn.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numRowsToReturn.Name = "numRowsToReturn";
      this.numRowsToReturn.Size = new System.Drawing.Size(60, 23);
      this.numRowsToReturn.TabIndex = 4;
      this.numRowsToReturn.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // chkLimitRows
      // 
      this.chkLimitRows.AutoSize = true;
      this.chkLimitRows.Location = new System.Drawing.Point(334, 22);
      this.chkLimitRows.Name = "chkLimitRows";
      this.chkLimitRows.Size = new System.Drawing.Size(67, 19);
      this.chkLimitRows.TabIndex = 2;
      this.chkLimitRows.Text = "Limit to";
      this.chkLimitRows.UseVisualStyleBackColor = true;
      this.chkLimitRows.CheckedChanged += new System.EventHandler(this.chkLimitRows_CheckedChanged);
      // 
      // chkIncludeHeaders
      // 
      this.chkIncludeHeaders.AutoSize = true;
      this.chkIncludeHeaders.Location = new System.Drawing.Point(18, 25);
      this.chkIncludeHeaders.Name = "chkIncludeHeaders";
      this.chkIncludeHeaders.Size = new System.Drawing.Size(211, 19);
      this.chkIncludeHeaders.TabIndex = 1;
      this.chkIncludeHeaders.Text = "Include Column Names as Headers";
      this.chkIncludeHeaders.UseVisualStyleBackColor = true;
      // 
      // lblOptionsWarning
      // 
      this.lblOptionsWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblOptionsWarning.AutoSize = true;
      this.lblOptionsWarning.BackColor = System.Drawing.SystemColors.Window;
      this.lblOptionsWarning.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblOptionsWarning.ForeColor = System.Drawing.Color.Red;
      this.lblOptionsWarning.Location = new System.Drawing.Point(160, 445);
      this.lblOptionsWarning.Name = "lblOptionsWarning";
      this.lblOptionsWarning.Size = new System.Drawing.Size(76, 12);
      this.lblOptionsWarning.TabIndex = 0;
      this.lblOptionsWarning.Text = "Warning Message";
      this.lblOptionsWarning.Visible = false;
      // 
      // picOptionsWarning
      // 
      this.picOptionsWarning.BackColor = System.Drawing.SystemColors.Window;
      this.picOptionsWarning.Image = global::MySQL.ForExcel.Properties.Resources.Warning;
      this.picOptionsWarning.Location = new System.Drawing.Point(138, 440);
      this.picOptionsWarning.Name = "picOptionsWarning";
      this.picOptionsWarning.Size = new System.Drawing.Size(20, 20);
      this.picOptionsWarning.TabIndex = 24;
      this.picOptionsWarning.TabStop = false;
      this.picOptionsWarning.Visible = false;
      // 
      // grdPreviewData
      // 
      this.grdPreviewData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdPreviewData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.grdPreviewData.ColumnsMaximumWidth = 200;
      this.grdPreviewData.ContextMenuStrip = this.contextMenuForGrid;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdPreviewData.DefaultCellStyle = dataGridViewCellStyle2;
      this.grdPreviewData.Location = new System.Drawing.Point(80, 164);
      this.grdPreviewData.Name = "grdPreviewData";
      this.grdPreviewData.Size = new System.Drawing.Size(695, 265);
      this.grdPreviewData.TabIndex = 7;
      this.grdPreviewData.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grdPreviewData_DataBindingComplete);
      this.grdPreviewData.SelectionChanged += new System.EventHandler(this.grdPreviewData_SelectionChanged);
      // 
      // contextMenuForGrid
      // 
      this.contextMenuForGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem});
      this.contextMenuForGrid.Name = "contextMenuForGrid";
      this.contextMenuForGrid.Size = new System.Drawing.Size(123, 26);
      // 
      // selectAllToolStripMenuItem
      // 
      this.selectAllToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_ColumnOptions_32x32;
      this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
      this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
      this.selectAllToolStripMenuItem.Text = "Select All";
      this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
      // 
      // lblPickColumnsSub
      // 
      this.lblPickColumnsSub.AutoSize = true;
      this.lblPickColumnsSub.BackColor = System.Drawing.Color.Transparent;
      this.lblPickColumnsSub.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPickColumnsSub.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblPickColumnsSub.Location = new System.Drawing.Point(77, 73);
      this.lblPickColumnsSub.Name = "lblPickColumnsSub";
      this.lblPickColumnsSub.Size = new System.Drawing.Size(302, 30);
      this.lblPickColumnsSub.TabIndex = 5;
      this.lblPickColumnsSub.Text = "Click on column headers to exclude/include them when\r\nimporting the MySQL table d" +
    "ata in Excel.";
      // 
      // lblPickColumnsMain
      // 
      this.lblPickColumnsMain.AutoSize = true;
      this.lblPickColumnsMain.BackColor = System.Drawing.Color.Transparent;
      this.lblPickColumnsMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPickColumnsMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblPickColumnsMain.Location = new System.Drawing.Point(77, 56);
      this.lblPickColumnsMain.Name = "lblPickColumnsMain";
      this.lblPickColumnsMain.Size = new System.Drawing.Size(165, 17);
      this.lblPickColumnsMain.TabIndex = 4;
      this.lblPickColumnsMain.Text = "Choose Columns to Import";
      // 
      // picColumnOptions
      // 
      this.picColumnOptions.BackColor = System.Drawing.Color.Transparent;
      this.picColumnOptions.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_ColumnOptions_32x32;
      this.picColumnOptions.Location = new System.Drawing.Point(39, 56);
      this.picColumnOptions.Name = "picColumnOptions";
      this.picColumnOptions.Size = new System.Drawing.Size(32, 32);
      this.picColumnOptions.TabIndex = 29;
      this.picColumnOptions.TabStop = false;
      // 
      // lblRowsCountMain
      // 
      this.lblRowsCountMain.AutoSize = true;
      this.lblRowsCountMain.BackColor = System.Drawing.Color.Transparent;
      this.lblRowsCountMain.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblRowsCountMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblRowsCountMain.Location = new System.Drawing.Point(76, 142);
      this.lblRowsCountMain.Name = "lblRowsCountMain";
      this.lblRowsCountMain.Size = new System.Drawing.Size(69, 15);
      this.lblRowsCountMain.TabIndex = 2;
      this.lblRowsCountMain.Text = "Row Count:";
      // 
      // lblTableNameSub
      // 
      this.lblTableNameSub.AutoSize = true;
      this.lblTableNameSub.BackColor = System.Drawing.Color.Transparent;
      this.lblTableNameSub.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTableNameSub.ForeColor = System.Drawing.Color.Navy;
      this.lblTableNameSub.Location = new System.Drawing.Point(169, 127);
      this.lblTableNameSub.Name = "lblTableNameSub";
      this.lblTableNameSub.Size = new System.Drawing.Size(39, 15);
      this.lblTableNameSub.TabIndex = 1;
      this.lblTableNameSub.Text = "Name";
      // 
      // lblTableNameMain
      // 
      this.lblTableNameMain.AutoSize = true;
      this.lblTableNameMain.BackColor = System.Drawing.Color.Transparent;
      this.lblTableNameMain.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTableNameMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblTableNameMain.Location = new System.Drawing.Point(76, 127);
      this.lblTableNameMain.Name = "lblTableNameMain";
      this.lblTableNameMain.Size = new System.Drawing.Size(74, 15);
      this.lblTableNameMain.TabIndex = 0;
      this.lblTableNameMain.Text = "Table Name:";
      // 
      // btnImport
      // 
      this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnImport.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnImport.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnImport.Location = new System.Drawing.Point(681, 11);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new System.Drawing.Size(75, 23);
      this.btnImport.TabIndex = 0;
      this.btnImport.Text = "Import";
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnCancel.Location = new System.Drawing.Point(762, 11);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // lblExportData
      // 
      this.lblExportData.AutoSize = true;
      this.lblExportData.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblExportData.ForeColor = System.Drawing.Color.Navy;
      this.lblExportData.Location = new System.Drawing.Point(17, 17);
      this.lblExportData.Name = "lblExportData";
      this.lblExportData.Size = new System.Drawing.Size(176, 20);
      this.lblExportData.TabIndex = 30;
      this.lblExportData.Text = "Import Data from MySQL";
      // 
      // ImportTableViewForm
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(849, 597);
      this.CommandAreaHeight = 45;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MainInstructionLocation = new System.Drawing.Point(10, 14);
      this.MinimumSize = new System.Drawing.Size(865, 635);
      this.Name = "ImportTableViewForm";
      this.Text = "Import Data";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImportTableViewForm_FormClosing);
      this.Controls.SetChildIndex(this.contentAreaPanel, 0);
      this.Controls.SetChildIndex(this.commandAreaPanel, 0);
      this.contentAreaPanel.ResumeLayout(false);
      this.contentAreaPanel.PerformLayout();
      this.commandAreaPanel.ResumeLayout(false);
      this.grpOptions.ResumeLayout(false);
      this.grpOptions.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numFromRow)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numRowsToReturn)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picOptionsWarning)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).EndInit();
      this.contextMenuForGrid.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.picColumnOptions)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ImageList fromImageList;
    private System.Windows.Forms.Label lblSubSetOfData;
    private System.Windows.Forms.Label lblRowsCountSub;
    private System.Windows.Forms.GroupBox grpOptions;
    private System.Windows.Forms.NumericUpDown numFromRow;
    private System.Windows.Forms.Label lblRowsToReturn;
    private System.Windows.Forms.NumericUpDown numRowsToReturn;
    private System.Windows.Forms.CheckBox chkLimitRows;
    private System.Windows.Forms.CheckBox chkIncludeHeaders;
    private System.Windows.Forms.Label lblOptionsWarning;
    private System.Windows.Forms.PictureBox picOptionsWarning;
    private PreviewDataGridView grdPreviewData;
    private System.Windows.Forms.Label lblPickColumnsSub;
    private System.Windows.Forms.Label lblPickColumnsMain;
    private System.Windows.Forms.PictureBox picColumnOptions;
    private System.Windows.Forms.Label lblRowsCountMain;
    private System.Windows.Forms.Label lblTableNameSub;
    private System.Windows.Forms.Label lblTableNameMain;
    private System.Windows.Forms.Button btnImport;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label lblExportData;
    private System.Windows.Forms.ContextMenuStrip contextMenuForGrid;
    private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;

  }
}