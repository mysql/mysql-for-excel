// 
// Copyright (c) 2012-2013, Oracle and/or its affiliates. All rights reserved.
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
      this.FromImageList = new System.Windows.Forms.ImageList(this.components);
      this.SubSetOfDataLabel = new System.Windows.Forms.Label();
      this.RowsCountSubLabel = new System.Windows.Forms.Label();
      this.OptionsGroupBox = new System.Windows.Forms.GroupBox();
      this.FromRowNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.RowsToReturnLabel = new System.Windows.Forms.Label();
      this.RowsToReturnNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.LimitRowsCheckBox = new System.Windows.Forms.CheckBox();
      this.IncludeHeadersCheckBox = new System.Windows.Forms.CheckBox();
      this.OptionsWarningLabel = new System.Windows.Forms.Label();
      this.OptionsWarningPictureBox = new System.Windows.Forms.PictureBox();
      this.PreviewDataGridView = new MySQL.ForExcel.PreviewDataGridView();
      this.ContextMenuForGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.SelectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.SelectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.PickColumnsSubLabel = new System.Windows.Forms.Label();
      this.PickColumnsMainLabel = new System.Windows.Forms.Label();
      this.ColumnOptionsPictureBox = new System.Windows.Forms.PictureBox();
      this.RowsCountMainLabel = new System.Windows.Forms.Label();
      this.TableNameSubLabel = new System.Windows.Forms.Label();
      this.TableNameMainLabel = new System.Windows.Forms.Label();
      this.ImportButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.ExportDataLabel = new System.Windows.Forms.Label();
      this.AdvancedOptionsButton = new System.Windows.Forms.Button();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      this.OptionsGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.FromRowNumericUpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.RowsToReturnNumericUpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.OptionsWarningPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.PreviewDataGridView)).BeginInit();
      this.ContextMenuForGrid.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ColumnOptionsPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 517);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(849, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.OptionsWarningLabel);
      this.ContentAreaPanel.Controls.Add(this.OptionsWarningPictureBox);
      this.ContentAreaPanel.Controls.Add(this.ExportDataLabel);
      this.ContentAreaPanel.Controls.Add(this.SubSetOfDataLabel);
      this.ContentAreaPanel.Controls.Add(this.RowsCountSubLabel);
      this.ContentAreaPanel.Controls.Add(this.OptionsGroupBox);
      this.ContentAreaPanel.Controls.Add(this.PreviewDataGridView);
      this.ContentAreaPanel.Controls.Add(this.PickColumnsSubLabel);
      this.ContentAreaPanel.Controls.Add(this.PickColumnsMainLabel);
      this.ContentAreaPanel.Controls.Add(this.ColumnOptionsPictureBox);
      this.ContentAreaPanel.Controls.Add(this.RowsCountMainLabel);
      this.ContentAreaPanel.Controls.Add(this.TableNameSubLabel);
      this.ContentAreaPanel.Controls.Add(this.TableNameMainLabel);
      this.ContentAreaPanel.Size = new System.Drawing.Size(849, 596);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.AdvancedOptionsButton);
      this.CommandAreaPanel.Controls.Add(this.ImportButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 551);
      this.CommandAreaPanel.Size = new System.Drawing.Size(849, 45);
      // 
      // FromImageList
      // 
      this.FromImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("FromImageList.ImageStream")));
      this.FromImageList.TransparentColor = System.Drawing.Color.Transparent;
      this.FromImageList.Images.SetKeyName(0, "db.Table.32x32.png");
      this.FromImageList.Images.SetKeyName(1, "db.View.32x32.png");
      // 
      // SubSetOfDataLabel
      // 
      this.SubSetOfDataLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.SubSetOfDataLabel.AutoSize = true;
      this.SubSetOfDataLabel.BackColor = System.Drawing.Color.Transparent;
      this.SubSetOfDataLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SubSetOfDataLabel.ForeColor = System.Drawing.SystemColors.InactiveCaption;
      this.SubSetOfDataLabel.Location = new System.Drawing.Point(456, 142);
      this.SubSetOfDataLabel.Name = "SubSetOfDataLabel";
      this.SubSetOfDataLabel.Size = new System.Drawing.Size(319, 15);
      this.SubSetOfDataLabel.TabIndex = 6;
      this.SubSetOfDataLabel.Text = "This is a small subset of the data for preview purposes only.";
      // 
      // RowsCountSubLabel
      // 
      this.RowsCountSubLabel.AutoSize = true;
      this.RowsCountSubLabel.BackColor = System.Drawing.Color.Transparent;
      this.RowsCountSubLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RowsCountSubLabel.ForeColor = System.Drawing.Color.Navy;
      this.RowsCountSubLabel.Location = new System.Drawing.Point(169, 142);
      this.RowsCountSubLabel.Name = "RowsCountSubLabel";
      this.RowsCountSubLabel.Size = new System.Drawing.Size(13, 15);
      this.RowsCountSubLabel.TabIndex = 3;
      this.RowsCountSubLabel.Text = "0";
      // 
      // OptionsGroupBox
      // 
      this.OptionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.OptionsGroupBox.BackColor = System.Drawing.Color.Transparent;
      this.OptionsGroupBox.Controls.Add(this.FromRowNumericUpDown);
      this.OptionsGroupBox.Controls.Add(this.RowsToReturnLabel);
      this.OptionsGroupBox.Controls.Add(this.RowsToReturnNumericUpDown);
      this.OptionsGroupBox.Controls.Add(this.LimitRowsCheckBox);
      this.OptionsGroupBox.Controls.Add(this.IncludeHeadersCheckBox);
      this.OptionsGroupBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OptionsGroupBox.Location = new System.Drawing.Point(80, 442);
      this.OptionsGroupBox.Name = "OptionsGroupBox";
      this.OptionsGroupBox.Size = new System.Drawing.Size(695, 60);
      this.OptionsGroupBox.TabIndex = 8;
      this.OptionsGroupBox.TabStop = false;
      this.OptionsGroupBox.Text = "Options";
      // 
      // FromRowNumericUpDown
      // 
      this.FromRowNumericUpDown.Enabled = false;
      this.FromRowNumericUpDown.Location = new System.Drawing.Point(616, 21);
      this.FromRowNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.FromRowNumericUpDown.Name = "FromRowNumericUpDown";
      this.FromRowNumericUpDown.Size = new System.Drawing.Size(60, 23);
      this.FromRowNumericUpDown.TabIndex = 6;
      this.FromRowNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.FromRowNumericUpDown.ValueChanged += new System.EventHandler(this.FromRowNumericUpDown_ValueChanged);
      // 
      // RowsToReturnLabel
      // 
      this.RowsToReturnLabel.AutoSize = true;
      this.RowsToReturnLabel.Location = new System.Drawing.Point(473, 25);
      this.RowsToReturnLabel.Name = "RowsToReturnLabel";
      this.RowsToReturnLabel.Size = new System.Drawing.Size(137, 15);
      this.RowsToReturnLabel.TabIndex = 5;
      this.RowsToReturnLabel.Text = "Rows and Start with Row";
      // 
      // RowsToReturnNumericUpDown
      // 
      this.RowsToReturnNumericUpDown.Enabled = false;
      this.RowsToReturnNumericUpDown.Location = new System.Drawing.Point(407, 21);
      this.RowsToReturnNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.RowsToReturnNumericUpDown.Name = "RowsToReturnNumericUpDown";
      this.RowsToReturnNumericUpDown.Size = new System.Drawing.Size(60, 23);
      this.RowsToReturnNumericUpDown.TabIndex = 4;
      this.RowsToReturnNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // LimitRowsCheckBox
      // 
      this.LimitRowsCheckBox.AutoSize = true;
      this.LimitRowsCheckBox.Location = new System.Drawing.Point(334, 22);
      this.LimitRowsCheckBox.Name = "LimitRowsCheckBox";
      this.LimitRowsCheckBox.Size = new System.Drawing.Size(67, 19);
      this.LimitRowsCheckBox.TabIndex = 2;
      this.LimitRowsCheckBox.Text = "Limit to";
      this.LimitRowsCheckBox.UseVisualStyleBackColor = true;
      this.LimitRowsCheckBox.CheckedChanged += new System.EventHandler(this.LimitRowsCheckBox_CheckedChanged);
      // 
      // IncludeHeadersCheckBox
      // 
      this.IncludeHeadersCheckBox.AutoSize = true;
      this.IncludeHeadersCheckBox.Location = new System.Drawing.Point(18, 25);
      this.IncludeHeadersCheckBox.Name = "IncludeHeadersCheckBox";
      this.IncludeHeadersCheckBox.Size = new System.Drawing.Size(211, 19);
      this.IncludeHeadersCheckBox.TabIndex = 1;
      this.IncludeHeadersCheckBox.Text = "Include Column Names as Headers";
      this.IncludeHeadersCheckBox.UseVisualStyleBackColor = true;
      // 
      // OptionsWarningLabel
      // 
      this.OptionsWarningLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.OptionsWarningLabel.AutoSize = true;
      this.OptionsWarningLabel.BackColor = System.Drawing.SystemColors.Window;
      this.OptionsWarningLabel.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OptionsWarningLabel.ForeColor = System.Drawing.Color.Red;
      this.OptionsWarningLabel.Location = new System.Drawing.Point(160, 445);
      this.OptionsWarningLabel.Name = "OptionsWarningLabel";
      this.OptionsWarningLabel.Size = new System.Drawing.Size(76, 12);
      this.OptionsWarningLabel.TabIndex = 0;
      this.OptionsWarningLabel.Text = "Warning Message";
      this.OptionsWarningLabel.Visible = false;
      // 
      // OptionsWarningPictureBox
      // 
      this.OptionsWarningPictureBox.BackColor = System.Drawing.SystemColors.Window;
      this.OptionsWarningPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.Warning;
      this.OptionsWarningPictureBox.Location = new System.Drawing.Point(138, 440);
      this.OptionsWarningPictureBox.Name = "OptionsWarningPictureBox";
      this.OptionsWarningPictureBox.Size = new System.Drawing.Size(20, 20);
      this.OptionsWarningPictureBox.TabIndex = 24;
      this.OptionsWarningPictureBox.TabStop = false;
      this.OptionsWarningPictureBox.Visible = false;
      // 
      // PreviewDataGridView
      // 
      this.PreviewDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.PreviewDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.PreviewDataGridView.ColumnsMaximumWidth = 200;
      this.PreviewDataGridView.ContextMenuStrip = this.ContextMenuForGrid;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.PreviewDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
      this.PreviewDataGridView.Location = new System.Drawing.Point(80, 164);
      this.PreviewDataGridView.Name = "PreviewDataGridView";
      this.PreviewDataGridView.Size = new System.Drawing.Size(695, 265);
      this.PreviewDataGridView.TabIndex = 7;
      this.PreviewDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.PreviewDataGridView_DataBindingComplete);
      this.PreviewDataGridView.SelectionChanged += new System.EventHandler(this.PreviewDataGridView_SelectionChanged);
      // 
      // ContextMenuForGrid
      // 
      this.ContextMenuForGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectAllToolStripMenuItem,
            this.SelectNoneToolStripMenuItem});
      this.ContextMenuForGrid.Name = "contextMenuForGrid";
      this.ContextMenuForGrid.Size = new System.Drawing.Size(153, 70);
      this.ContextMenuForGrid.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuForGrid_Opening);
      // 
      // SelectAllToolStripMenuItem
      // 
      this.SelectAllToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_ColumnOptions_32x32;
      this.SelectAllToolStripMenuItem.Name = "SelectAllToolStripMenuItem";
      this.SelectAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.SelectAllToolStripMenuItem.Text = "Select All";
      this.SelectAllToolStripMenuItem.Click += new System.EventHandler(this.SelectAllToolStripMenuItem_Click);
      // 
      // SelectNoneToolStripMenuItem
      // 
      this.SelectNoneToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_ColumnOptions_32x32;
      this.SelectNoneToolStripMenuItem.Name = "SelectNoneToolStripMenuItem";
      this.SelectNoneToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.SelectNoneToolStripMenuItem.Text = "Select None";
      this.SelectNoneToolStripMenuItem.Click += new System.EventHandler(this.SelectNoneToolStripMenuItem_Click);
      // 
      // PickColumnsSubLabel
      // 
      this.PickColumnsSubLabel.AutoSize = true;
      this.PickColumnsSubLabel.BackColor = System.Drawing.Color.Transparent;
      this.PickColumnsSubLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PickColumnsSubLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.PickColumnsSubLabel.Location = new System.Drawing.Point(77, 73);
      this.PickColumnsSubLabel.Name = "PickColumnsSubLabel";
      this.PickColumnsSubLabel.Size = new System.Drawing.Size(302, 30);
      this.PickColumnsSubLabel.TabIndex = 5;
      this.PickColumnsSubLabel.Text = "Click on column headers to exclude/include them when\r\nimporting the MySQL table d" +
    "ata in Excel.";
      // 
      // PickColumnsMainLabel
      // 
      this.PickColumnsMainLabel.AutoSize = true;
      this.PickColumnsMainLabel.BackColor = System.Drawing.Color.Transparent;
      this.PickColumnsMainLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PickColumnsMainLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.PickColumnsMainLabel.Location = new System.Drawing.Point(77, 56);
      this.PickColumnsMainLabel.Name = "PickColumnsMainLabel";
      this.PickColumnsMainLabel.Size = new System.Drawing.Size(165, 17);
      this.PickColumnsMainLabel.TabIndex = 4;
      this.PickColumnsMainLabel.Text = "Choose Columns to Import";
      // 
      // ColumnOptionsPictureBox
      // 
      this.ColumnOptionsPictureBox.BackColor = System.Drawing.Color.Transparent;
      this.ColumnOptionsPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_ColumnOptions_32x32;
      this.ColumnOptionsPictureBox.Location = new System.Drawing.Point(39, 56);
      this.ColumnOptionsPictureBox.Name = "ColumnOptionsPictureBox";
      this.ColumnOptionsPictureBox.Size = new System.Drawing.Size(32, 32);
      this.ColumnOptionsPictureBox.TabIndex = 29;
      this.ColumnOptionsPictureBox.TabStop = false;
      // 
      // RowsCountMainLabel
      // 
      this.RowsCountMainLabel.AutoSize = true;
      this.RowsCountMainLabel.BackColor = System.Drawing.Color.Transparent;
      this.RowsCountMainLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RowsCountMainLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.RowsCountMainLabel.Location = new System.Drawing.Point(76, 142);
      this.RowsCountMainLabel.Name = "RowsCountMainLabel";
      this.RowsCountMainLabel.Size = new System.Drawing.Size(69, 15);
      this.RowsCountMainLabel.TabIndex = 2;
      this.RowsCountMainLabel.Text = "Row Count:";
      // 
      // TableNameSubLabel
      // 
      this.TableNameSubLabel.AutoSize = true;
      this.TableNameSubLabel.BackColor = System.Drawing.Color.Transparent;
      this.TableNameSubLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TableNameSubLabel.ForeColor = System.Drawing.Color.Navy;
      this.TableNameSubLabel.Location = new System.Drawing.Point(169, 127);
      this.TableNameSubLabel.Name = "TableNameSubLabel";
      this.TableNameSubLabel.Size = new System.Drawing.Size(39, 15);
      this.TableNameSubLabel.TabIndex = 1;
      this.TableNameSubLabel.Text = "Name";
      // 
      // TableNameMainLabel
      // 
      this.TableNameMainLabel.AutoSize = true;
      this.TableNameMainLabel.BackColor = System.Drawing.Color.Transparent;
      this.TableNameMainLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TableNameMainLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.TableNameMainLabel.Location = new System.Drawing.Point(76, 127);
      this.TableNameMainLabel.Name = "TableNameMainLabel";
      this.TableNameMainLabel.Size = new System.Drawing.Size(74, 15);
      this.TableNameMainLabel.TabIndex = 0;
      this.TableNameMainLabel.Text = "Table Name:";
      // 
      // ImportButton
      // 
      this.ImportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.ImportButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.ImportButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ImportButton.Location = new System.Drawing.Point(681, 11);
      this.ImportButton.Name = "ImportButton";
      this.ImportButton.Size = new System.Drawing.Size(75, 23);
      this.ImportButton.TabIndex = 1;
      this.ImportButton.Text = "Import";
      this.ImportButton.UseVisualStyleBackColor = true;
      this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogCancelButton.Location = new System.Drawing.Point(762, 11);
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
      this.ExportDataLabel.Size = new System.Drawing.Size(176, 20);
      this.ExportDataLabel.TabIndex = 30;
      this.ExportDataLabel.Text = "Import Data from MySQL";
      // 
      // AdvancedOptionsButton
      // 
      this.AdvancedOptionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.AdvancedOptionsButton.Location = new System.Drawing.Point(12, 11);
      this.AdvancedOptionsButton.Name = "AdvancedOptionsButton";
      this.AdvancedOptionsButton.Size = new System.Drawing.Size(131, 23);
      this.AdvancedOptionsButton.TabIndex = 0;
      this.AdvancedOptionsButton.Text = "Advanced Options...";
      this.AdvancedOptionsButton.UseVisualStyleBackColor = true;
      this.AdvancedOptionsButton.Click += new System.EventHandler(this.AdvancedOptionsButton_Click);
      // 
      // ImportTableViewForm
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.ClientSize = new System.Drawing.Size(849, 596);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MainInstructionLocation = new System.Drawing.Point(10, 14);
      this.MinimumSize = new System.Drawing.Size(865, 635);
      this.Name = "ImportTableViewForm";
      this.Text = "Import Data";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImportTableViewForm_FormClosing);
      this.Controls.SetChildIndex(this.FootnoteAreaPanel, 0);
      this.Controls.SetChildIndex(this.ContentAreaPanel, 0);
      this.Controls.SetChildIndex(this.CommandAreaPanel, 0);
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      this.OptionsGroupBox.ResumeLayout(false);
      this.OptionsGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.FromRowNumericUpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.RowsToReturnNumericUpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.OptionsWarningPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.PreviewDataGridView)).EndInit();
      this.ContextMenuForGrid.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.ColumnOptionsPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ImageList FromImageList;
    private System.Windows.Forms.Label SubSetOfDataLabel;
    private System.Windows.Forms.Label RowsCountSubLabel;
    private System.Windows.Forms.GroupBox OptionsGroupBox;
    private System.Windows.Forms.NumericUpDown FromRowNumericUpDown;
    private System.Windows.Forms.Label RowsToReturnLabel;
    private System.Windows.Forms.NumericUpDown RowsToReturnNumericUpDown;
    private System.Windows.Forms.CheckBox LimitRowsCheckBox;
    private System.Windows.Forms.CheckBox IncludeHeadersCheckBox;
    private System.Windows.Forms.Label OptionsWarningLabel;
    private System.Windows.Forms.PictureBox OptionsWarningPictureBox;
    private PreviewDataGridView PreviewDataGridView;
    private System.Windows.Forms.Label PickColumnsSubLabel;
    private System.Windows.Forms.Label PickColumnsMainLabel;
    private System.Windows.Forms.PictureBox ColumnOptionsPictureBox;
    private System.Windows.Forms.Label RowsCountMainLabel;
    private System.Windows.Forms.Label TableNameSubLabel;
    private System.Windows.Forms.Label TableNameMainLabel;
    private System.Windows.Forms.Button ImportButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.Label ExportDataLabel;
    private System.Windows.Forms.ContextMenuStrip ContextMenuForGrid;
    private System.Windows.Forms.ToolStripMenuItem SelectAllToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem SelectNoneToolStripMenuItem;
    private System.Windows.Forms.Button AdvancedOptionsButton;

  }
}