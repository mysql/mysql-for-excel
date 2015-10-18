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
  partial class AppendDataForm
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
        
        if (_droppableCursor != null)
        {
          _droppableCursor.Dispose();
        }

        if (_draggingCursor != null)
        {
          _draggingCursor.Dispose();
        }

        if (_sourceMySqlPreviewDataTable != null)
        {
          _sourceMySqlPreviewDataTable.Dispose();
        }

        if (_targetMySqlPreviewDataTable != null)
        {
          _targetMySqlPreviewDataTable.Dispose();
        }

        if (_trashCursor != null)
        {
          _trashCursor.Dispose();
        }

        // Set variables to null so this object does not hold references to them and the GC disposes of them sooner.
        _appendDataRange = null;
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppendDataForm));
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
      this.AppendButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.ManuallyAdjustMappingMainSubLabel = new System.Windows.Forms.Label();
      this.FirstRowHeadersCheckBox = new System.Windows.Forms.CheckBox();
      this.AppendContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.RemoveColumnMappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.ClearAllMappingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.ChooseColumnMappingMainSubLabel = new System.Windows.Forms.Label();
      this.ChooseColumnMappingMainLabel = new System.Windows.Forms.Label();
      this.ChooseColumnMappingPictureBox = new System.Windows.Forms.PictureBox();
      this.ManuallyAdjustMappingMainLabel = new System.Windows.Forms.Label();
      this.ManuallyAdjustMappingPictureBox = new System.Windows.Forms.PictureBox();
      this.ExportDataLabel = new System.Windows.Forms.Label();
      this.ColorMapMappedPictureBox = new System.Windows.Forms.PictureBox();
      this.ColorMapMappedLabel = new System.Windows.Forms.Label();
      this.ColorMapUnmappedPictureBox = new System.Windows.Forms.PictureBox();
      this.ColorMapUnmappedLabel = new System.Windows.Forms.Label();
      this.DownArrowPictureBox = new System.Windows.Forms.PictureBox();
      this.MappingMethodLabel = new System.Windows.Forms.Label();
      this.MappingMethodComboBox = new System.Windows.Forms.ComboBox();
      this.AdvancedOptionsButton = new System.Windows.Forms.Button();
      this.StoreMappingButton = new System.Windows.Forms.Button();
      this.SubSetOfDataLabel = new System.Windows.Forms.Label();
      this.ColumnWarningLabel = new System.Windows.Forms.Label();
      this.ColumnWarningPictureBox = new System.Windows.Forms.PictureBox();
      this.TargetMySqlTableDataGridView = new MySQL.ForExcel.Controls.MultiHeaderDataGridView();
      this.SourceExcelDataDataGridView = new MySQL.ForExcel.Controls.MultiHeaderDataGridView();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      this.AppendContextMenu.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ChooseColumnMappingPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ManuallyAdjustMappingPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ColorMapMappedPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ColorMapUnmappedPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.DownArrowPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ColumnWarningPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.TargetMySqlTableDataGridView)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.SourceExcelDataDataGridView)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.AllowDrop = true;
      this.ContentAreaPanel.Controls.Add(this.ColumnWarningLabel);
      this.ContentAreaPanel.Controls.Add(this.SubSetOfDataLabel);
      this.ContentAreaPanel.Controls.Add(this.ColumnWarningPictureBox);
      this.ContentAreaPanel.Controls.Add(this.TargetMySqlTableDataGridView);
      this.ContentAreaPanel.Controls.Add(this.MappingMethodComboBox);
      this.ContentAreaPanel.Controls.Add(this.MappingMethodLabel);
      this.ContentAreaPanel.Controls.Add(this.DownArrowPictureBox);
      this.ContentAreaPanel.Controls.Add(this.ColorMapMappedPictureBox);
      this.ContentAreaPanel.Controls.Add(this.ColorMapMappedLabel);
      this.ContentAreaPanel.Controls.Add(this.ColorMapUnmappedPictureBox);
      this.ContentAreaPanel.Controls.Add(this.ColorMapUnmappedLabel);
      this.ContentAreaPanel.Controls.Add(this.ExportDataLabel);
      this.ContentAreaPanel.Controls.Add(this.ManuallyAdjustMappingMainSubLabel);
      this.ContentAreaPanel.Controls.Add(this.FirstRowHeadersCheckBox);
      this.ContentAreaPanel.Controls.Add(this.SourceExcelDataDataGridView);
      this.ContentAreaPanel.Controls.Add(this.ChooseColumnMappingMainSubLabel);
      this.ContentAreaPanel.Controls.Add(this.ChooseColumnMappingMainLabel);
      this.ContentAreaPanel.Controls.Add(this.ChooseColumnMappingPictureBox);
      this.ContentAreaPanel.Controls.Add(this.ManuallyAdjustMappingMainLabel);
      this.ContentAreaPanel.Controls.Add(this.ManuallyAdjustMappingPictureBox);
      this.ContentAreaPanel.Size = new System.Drawing.Size(844, 597);
      this.ContentAreaPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.ContentAreaPanel_DragDrop);
      this.ContentAreaPanel.DragOver += new System.Windows.Forms.DragEventHandler(this.ContentAreaPanel_DragOver);
      this.ContentAreaPanel.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.ContentAreaPanel_QueryContinueDrag);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.StoreMappingButton);
      this.CommandAreaPanel.Controls.Add(this.AdvancedOptionsButton);
      this.CommandAreaPanel.Controls.Add(this.AppendButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 552);
      this.CommandAreaPanel.Size = new System.Drawing.Size(844, 45);
      // 
      // AppendButton
      // 
      this.AppendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.AppendButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.AppendButton.Location = new System.Drawing.Point(678, 12);
      this.AppendButton.Name = "AppendButton";
      this.AppendButton.Size = new System.Drawing.Size(75, 23);
      this.AppendButton.TabIndex = 2;
      this.AppendButton.Text = "Append";
      this.AppendButton.UseVisualStyleBackColor = true;
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Location = new System.Drawing.Point(759, 12);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 3;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // ManuallyAdjustMappingMainSubLabel
      // 
      this.ManuallyAdjustMappingMainSubLabel.AutoSize = true;
      this.ManuallyAdjustMappingMainSubLabel.BackColor = System.Drawing.Color.Transparent;
      this.ManuallyAdjustMappingMainSubLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ManuallyAdjustMappingMainSubLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ManuallyAdjustMappingMainSubLabel.Location = new System.Drawing.Point(470, 73);
      this.ManuallyAdjustMappingMainSubLabel.Name = "ManuallyAdjustMappingMainSubLabel";
      this.ManuallyAdjustMappingMainSubLabel.Size = new System.Drawing.Size(298, 45);
      this.ManuallyAdjustMappingMainSubLabel.TabIndex = 6;
      this.ManuallyAdjustMappingMainSubLabel.Text = "Manually change the column mapping if needed. Click\r\na column in the upper table " +
    "with the mouse and drag it\r\nonto a column in the lower table.";
      // 
      // FirstRowHeadersCheckBox
      // 
      this.FirstRowHeadersCheckBox.AutoSize = true;
      this.FirstRowHeadersCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.FirstRowHeadersCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FirstRowHeadersCheckBox.Location = new System.Drawing.Point(82, 158);
      this.FirstRowHeadersCheckBox.Name = "FirstRowHeadersCheckBox";
      this.FirstRowHeadersCheckBox.Size = new System.Drawing.Size(210, 19);
      this.FirstRowHeadersCheckBox.TabIndex = 7;
      this.FirstRowHeadersCheckBox.Text = "First Row Contains Column Names";
      this.FirstRowHeadersCheckBox.UseVisualStyleBackColor = false;
      this.FirstRowHeadersCheckBox.CheckedChanged += new System.EventHandler(this.FirstRowHeadersCheckBox_CheckedChanged);
      // 
      // AppendContextMenu
      // 
      this.AppendContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RemoveColumnMappingToolStripMenuItem,
            this.ClearAllMappingsToolStripMenuItem});
      this.AppendContextMenu.Name = "contextMenu";
      this.AppendContextMenu.Size = new System.Drawing.Size(215, 48);
      this.AppendContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.AppendContextMenu_Opening);
      // 
      // RemoveColumnMappingToolStripMenuItem
      // 
      this.RemoveColumnMappingToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.remove_col_mapping;
      this.RemoveColumnMappingToolStripMenuItem.Name = "RemoveColumnMappingToolStripMenuItem";
      this.RemoveColumnMappingToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
      this.RemoveColumnMappingToolStripMenuItem.Text = "Remove Column Mapping";
      this.RemoveColumnMappingToolStripMenuItem.Click += new System.EventHandler(this.RemoveColumnMappingToolStripMenuItem_Click);
      // 
      // ClearAllMappingsToolStripMenuItem
      // 
      this.ClearAllMappingsToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.clear_output;
      this.ClearAllMappingsToolStripMenuItem.Name = "ClearAllMappingsToolStripMenuItem";
      this.ClearAllMappingsToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
      this.ClearAllMappingsToolStripMenuItem.Text = "Clear All Mappings";
      this.ClearAllMappingsToolStripMenuItem.Click += new System.EventHandler(this.ClearAllMappingsToolStripMenuItem_Click);
      // 
      // ChooseColumnMappingMainSubLabel
      // 
      this.ChooseColumnMappingMainSubLabel.AutoSize = true;
      this.ChooseColumnMappingMainSubLabel.BackColor = System.Drawing.Color.Transparent;
      this.ChooseColumnMappingMainSubLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ChooseColumnMappingMainSubLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ChooseColumnMappingMainSubLabel.Location = new System.Drawing.Point(79, 73);
      this.ChooseColumnMappingMainSubLabel.Name = "ChooseColumnMappingMainSubLabel";
      this.ChooseColumnMappingMainSubLabel.Size = new System.Drawing.Size(298, 30);
      this.ChooseColumnMappingMainSubLabel.TabIndex = 2;
      this.ChooseColumnMappingMainSubLabel.Text = "Select how the Excel columns should be mapped to the\r\nMySQL table columns.";
      // 
      // ChooseColumnMappingMainLabel
      // 
      this.ChooseColumnMappingMainLabel.AutoSize = true;
      this.ChooseColumnMappingMainLabel.BackColor = System.Drawing.Color.Transparent;
      this.ChooseColumnMappingMainLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ChooseColumnMappingMainLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ChooseColumnMappingMainLabel.Location = new System.Drawing.Point(79, 53);
      this.ChooseColumnMappingMainLabel.Name = "ChooseColumnMappingMainLabel";
      this.ChooseColumnMappingMainLabel.Size = new System.Drawing.Size(221, 17);
      this.ChooseColumnMappingMainLabel.TabIndex = 1;
      this.ChooseColumnMappingMainLabel.Text = "1. Choose Column Mapping Method";
      // 
      // ChooseColumnMappingPictureBox
      // 
      this.ChooseColumnMappingPictureBox.BackColor = System.Drawing.Color.Transparent;
      this.ChooseColumnMappingPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("ChooseColumnMappingPictureBox.Image")));
      this.ChooseColumnMappingPictureBox.Location = new System.Drawing.Point(41, 59);
      this.ChooseColumnMappingPictureBox.Name = "ChooseColumnMappingPictureBox";
      this.ChooseColumnMappingPictureBox.Size = new System.Drawing.Size(32, 32);
      this.ChooseColumnMappingPictureBox.TabIndex = 36;
      this.ChooseColumnMappingPictureBox.TabStop = false;
      // 
      // ManuallyAdjustMappingMainLabel
      // 
      this.ManuallyAdjustMappingMainLabel.AutoSize = true;
      this.ManuallyAdjustMappingMainLabel.BackColor = System.Drawing.Color.Transparent;
      this.ManuallyAdjustMappingMainLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ManuallyAdjustMappingMainLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ManuallyAdjustMappingMainLabel.Location = new System.Drawing.Point(470, 54);
      this.ManuallyAdjustMappingMainLabel.Name = "ManuallyAdjustMappingMainLabel";
      this.ManuallyAdjustMappingMainLabel.Size = new System.Drawing.Size(219, 17);
      this.ManuallyAdjustMappingMainLabel.TabIndex = 5;
      this.ManuallyAdjustMappingMainLabel.Text = "2. Manually Adjust Column Mapping";
      // 
      // ManuallyAdjustMappingPictureBox
      // 
      this.ManuallyAdjustMappingPictureBox.BackColor = System.Drawing.Color.Transparent;
      this.ManuallyAdjustMappingPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("ManuallyAdjustMappingPictureBox.Image")));
      this.ManuallyAdjustMappingPictureBox.Location = new System.Drawing.Point(432, 60);
      this.ManuallyAdjustMappingPictureBox.Name = "ManuallyAdjustMappingPictureBox";
      this.ManuallyAdjustMappingPictureBox.Size = new System.Drawing.Size(32, 32);
      this.ManuallyAdjustMappingPictureBox.TabIndex = 30;
      this.ManuallyAdjustMappingPictureBox.TabStop = false;
      // 
      // ExportDataLabel
      // 
      this.ExportDataLabel.AutoSize = true;
      this.ExportDataLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExportDataLabel.ForeColor = System.Drawing.Color.Navy;
      this.ExportDataLabel.Location = new System.Drawing.Point(17, 17);
      this.ExportDataLabel.Name = "ExportDataLabel";
      this.ExportDataLabel.Size = new System.Drawing.Size(207, 20);
      this.ExportDataLabel.TabIndex = 0;
      this.ExportDataLabel.Text = "Append Data to MySQL Table";
      // 
      // ColorMapMappedPictureBox
      // 
      this.ColorMapMappedPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ColorMapMappedPictureBox.BackColor = System.Drawing.Color.LightGreen;
      this.ColorMapMappedPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.ColorMapMappedPictureBox.Location = new System.Drawing.Point(229, 516);
      this.ColorMapMappedPictureBox.Name = "ColorMapMappedPictureBox";
      this.ColorMapMappedPictureBox.Size = new System.Drawing.Size(15, 15);
      this.ColorMapMappedPictureBox.TabIndex = 41;
      this.ColorMapMappedPictureBox.TabStop = false;
      // 
      // ColorMapMappedLabel
      // 
      this.ColorMapMappedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ColorMapMappedLabel.AutoSize = true;
      this.ColorMapMappedLabel.BackColor = System.Drawing.Color.Transparent;
      this.ColorMapMappedLabel.Location = new System.Drawing.Point(244, 516);
      this.ColorMapMappedLabel.Name = "ColorMapMappedLabel";
      this.ColorMapMappedLabel.Size = new System.Drawing.Size(89, 13);
      this.ColorMapMappedLabel.TabIndex = 12;
      this.ColorMapMappedLabel.Text = "Mapped Columns";
      // 
      // ColorMapUnmappedPictureBox
      // 
      this.ColorMapUnmappedPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ColorMapUnmappedPictureBox.BackColor = System.Drawing.Color.OrangeRed;
      this.ColorMapUnmappedPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.ColorMapUnmappedPictureBox.Location = new System.Drawing.Point(82, 516);
      this.ColorMapUnmappedPictureBox.Name = "ColorMapUnmappedPictureBox";
      this.ColorMapUnmappedPictureBox.Size = new System.Drawing.Size(15, 15);
      this.ColorMapUnmappedPictureBox.TabIndex = 40;
      this.ColorMapUnmappedPictureBox.TabStop = false;
      // 
      // ColorMapUnmappedLabel
      // 
      this.ColorMapUnmappedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ColorMapUnmappedLabel.AutoSize = true;
      this.ColorMapUnmappedLabel.BackColor = System.Drawing.Color.Transparent;
      this.ColorMapUnmappedLabel.Location = new System.Drawing.Point(97, 516);
      this.ColorMapUnmappedLabel.Name = "ColorMapUnmappedLabel";
      this.ColorMapUnmappedLabel.Size = new System.Drawing.Size(102, 13);
      this.ColorMapUnmappedLabel.TabIndex = 11;
      this.ColorMapUnmappedLabel.Text = "Unmapped Columns";
      // 
      // DownArrowPictureBox
      // 
      this.DownArrowPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("DownArrowPictureBox.Image")));
      this.DownArrowPictureBox.Location = new System.Drawing.Point(414, 340);
      this.DownArrowPictureBox.Name = "DownArrowPictureBox";
      this.DownArrowPictureBox.Size = new System.Drawing.Size(17, 11);
      this.DownArrowPictureBox.TabIndex = 42;
      this.DownArrowPictureBox.TabStop = false;
      // 
      // MappingMethodLabel
      // 
      this.MappingMethodLabel.AutoSize = true;
      this.MappingMethodLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.MappingMethodLabel.Location = new System.Drawing.Point(79, 115);
      this.MappingMethodLabel.Name = "MappingMethodLabel";
      this.MappingMethodLabel.Size = new System.Drawing.Size(103, 15);
      this.MappingMethodLabel.TabIndex = 3;
      this.MappingMethodLabel.Text = "Mapping Method:";
      // 
      // MappingMethodComboBox
      // 
      this.MappingMethodComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.MappingMethodComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.MappingMethodComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.MappingMethodComboBox.DropDownWidth = 243;
      this.MappingMethodComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.MappingMethodComboBox.FormattingEnabled = true;
      this.MappingMethodComboBox.Location = new System.Drawing.Point(188, 112);
      this.MappingMethodComboBox.Name = "MappingMethodComboBox";
      this.MappingMethodComboBox.Size = new System.Drawing.Size(189, 23);
      this.MappingMethodComboBox.TabIndex = 4;
      this.MappingMethodComboBox.SelectedIndexChanged += new System.EventHandler(this.MappingMethodComboBox_SelectedIndexChanged);
      // 
      // AdvancedOptionsButton
      // 
      this.AdvancedOptionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.AdvancedOptionsButton.Location = new System.Drawing.Point(12, 12);
      this.AdvancedOptionsButton.Name = "AdvancedOptionsButton";
      this.AdvancedOptionsButton.Size = new System.Drawing.Size(131, 23);
      this.AdvancedOptionsButton.TabIndex = 0;
      this.AdvancedOptionsButton.Text = "Advanced Options...";
      this.AdvancedOptionsButton.UseVisualStyleBackColor = true;
      this.AdvancedOptionsButton.Click += new System.EventHandler(this.AdvancedOptionsButton_Click);
      // 
      // StoreMappingButton
      // 
      this.StoreMappingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.StoreMappingButton.Enabled = false;
      this.StoreMappingButton.Location = new System.Drawing.Point(572, 12);
      this.StoreMappingButton.Name = "StoreMappingButton";
      this.StoreMappingButton.Size = new System.Drawing.Size(100, 23);
      this.StoreMappingButton.TabIndex = 1;
      this.StoreMappingButton.Text = "Store Mapping";
      this.StoreMappingButton.UseVisualStyleBackColor = true;
      this.StoreMappingButton.Click += new System.EventHandler(this.StoreMappingButton_Click);
      // 
      // SubSetOfDataLabel
      // 
      this.SubSetOfDataLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.SubSetOfDataLabel.AutoSize = true;
      this.SubSetOfDataLabel.BackColor = System.Drawing.Color.Transparent;
      this.SubSetOfDataLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SubSetOfDataLabel.ForeColor = System.Drawing.SystemColors.InactiveCaption;
      this.SubSetOfDataLabel.Location = new System.Drawing.Point(449, 159);
      this.SubSetOfDataLabel.Name = "SubSetOfDataLabel";
      this.SubSetOfDataLabel.Size = new System.Drawing.Size(319, 15);
      this.SubSetOfDataLabel.TabIndex = 8;
      this.SubSetOfDataLabel.Text = "This is a small subset of the data for preview purposes only.";
      // 
      // ColumnWarningLabel
      // 
      this.ColumnWarningLabel.AutoSize = true;
      this.ColumnWarningLabel.BackColor = System.Drawing.SystemColors.Window;
      this.ColumnWarningLabel.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ColumnWarningLabel.ForeColor = System.Drawing.Color.Red;
      this.ColumnWarningLabel.Location = new System.Drawing.Point(477, 519);
      this.ColumnWarningLabel.Name = "ColumnWarningLabel";
      this.ColumnWarningLabel.Size = new System.Drawing.Size(291, 12);
      this.ColumnWarningLabel.TabIndex = 25;
      this.ColumnWarningLabel.Text = "Appending data is not suitable for the mapped target column\'s data type.";
      this.ColumnWarningLabel.Visible = false;
      // 
      // ColumnWarningPictureBox
      // 
      this.ColumnWarningPictureBox.BackColor = System.Drawing.SystemColors.Window;
      this.ColumnWarningPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.Warning;
      this.ColumnWarningPictureBox.Location = new System.Drawing.Point(455, 515);
      this.ColumnWarningPictureBox.Name = "ColumnWarningPictureBox";
      this.ColumnWarningPictureBox.Size = new System.Drawing.Size(20, 20);
      this.ColumnWarningPictureBox.TabIndex = 26;
      this.ColumnWarningPictureBox.TabStop = false;
      this.ColumnWarningPictureBox.Visible = false;
      // 
      // TargetMySqlTableDataGridView
      // 
      this.TargetMySqlTableDataGridView.AllowChangingHeaderCellsColors = true;
      this.TargetMySqlTableDataGridView.AllowDrop = true;
      this.TargetMySqlTableDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TargetMySqlTableDataGridView.AutoSizeColumnsBasedOnAdditionalHeadersContent = true;
      this.TargetMySqlTableDataGridView.BaseColumnHeadersTextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
      dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3, 3, 1, 1);
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.TargetMySqlTableDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.TargetMySqlTableDataGridView.ColumnHeadersSeparatorColor = System.Drawing.SystemColors.ControlDark;
      this.TargetMySqlTableDataGridView.ColumnHeadersSeparatorWidth = 1;
      this.TargetMySqlTableDataGridView.ColumnsMaximumWidth = 200;
      this.TargetMySqlTableDataGridView.ColumnsMinimumWidth = 50;
      this.TargetMySqlTableDataGridView.ContextMenuStrip = this.AppendContextMenu;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.TargetMySqlTableDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
      this.TargetMySqlTableDataGridView.FixedColumnHeadersHeight = 23;
      this.TargetMySqlTableDataGridView.Location = new System.Drawing.Point(82, 360);
      this.TargetMySqlTableDataGridView.MultiSelect = false;
      this.TargetMySqlTableDataGridView.Name = "TargetMySqlTableDataGridView";
      this.TargetMySqlTableDataGridView.ReverseMultiHeaderRowOrder = false;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.TargetMySqlTableDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
      this.TargetMySqlTableDataGridView.Size = new System.Drawing.Size(686, 150);
      this.TargetMySqlTableDataGridView.TabIndex = 10;
      this.TargetMySqlTableDataGridView.UseColumnPaddings = true;
      this.TargetMySqlTableDataGridView.UseFixedColumnHeadersHeight = false;
      this.TargetMySqlTableDataGridView.SelectionChanged += new System.EventHandler(this.TargetMySQLTableDataGridView_SelectionChanged);
      this.TargetMySqlTableDataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.TargetMySQLTableDataGridView_DragDrop);
      this.TargetMySqlTableDataGridView.DragOver += new System.Windows.Forms.DragEventHandler(this.TargetMySQLTableDataGridView_DragOver);
      this.TargetMySqlTableDataGridView.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.DataGridView_GiveFeedback);
      this.TargetMySqlTableDataGridView.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.DataGridView_QueryContinueDrag);
      this.TargetMySqlTableDataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DataGridView_MouseDown);
      this.TargetMySqlTableDataGridView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DataGridView_MouseMove);
      this.TargetMySqlTableDataGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DataGridView_MouseUp);
      // 
      // SourceExcelDataDataGridView
      // 
      this.SourceExcelDataDataGridView.AllowChangingHeaderCellsColors = true;
      this.SourceExcelDataDataGridView.AllowDrop = true;
      this.SourceExcelDataDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.SourceExcelDataDataGridView.AutoSizeColumnsBasedOnAdditionalHeadersContent = true;
      this.SourceExcelDataDataGridView.BaseColumnHeadersTextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
      dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
      dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(3, 3, 1, 1);
      dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.SourceExcelDataDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
      this.SourceExcelDataDataGridView.ColumnHeadersSeparatorColor = System.Drawing.SystemColors.ControlDark;
      this.SourceExcelDataDataGridView.ColumnHeadersSeparatorWidth = 1;
      this.SourceExcelDataDataGridView.ColumnsMaximumWidth = 200;
      this.SourceExcelDataDataGridView.ColumnsMinimumWidth = 50;
      this.SourceExcelDataDataGridView.ContextMenuStrip = this.AppendContextMenu;
      dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.SourceExcelDataDataGridView.DefaultCellStyle = dataGridViewCellStyle5;
      this.SourceExcelDataDataGridView.FixedColumnHeadersHeight = 23;
      this.SourceExcelDataDataGridView.Location = new System.Drawing.Point(82, 182);
      this.SourceExcelDataDataGridView.MultiSelect = false;
      this.SourceExcelDataDataGridView.Name = "SourceExcelDataDataGridView";
      this.SourceExcelDataDataGridView.ReverseMultiHeaderRowOrder = false;
      dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.SourceExcelDataDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
      this.SourceExcelDataDataGridView.Size = new System.Drawing.Size(686, 150);
      this.SourceExcelDataDataGridView.TabIndex = 9;
      this.SourceExcelDataDataGridView.UseColumnPaddings = true;
      this.SourceExcelDataDataGridView.UseFixedColumnHeadersHeight = false;
      this.SourceExcelDataDataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.SourceExcelDataDataGridView_DragDrop);
      this.SourceExcelDataDataGridView.DragOver += new System.Windows.Forms.DragEventHandler(this.SourceExcelDataDataGridView_DragOver);
      this.SourceExcelDataDataGridView.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.DataGridView_GiveFeedback);
      this.SourceExcelDataDataGridView.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.DataGridView_QueryContinueDrag);
      this.SourceExcelDataDataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DataGridView_MouseDown);
      this.SourceExcelDataDataGridView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DataGridView_MouseMove);
      this.SourceExcelDataDataGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DataGridView_MouseUp);
      // 
      // AppendDataForm
      // 
      this.AcceptButton = this.AppendButton;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(844, 597);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MainInstructionLocation = new System.Drawing.Point(11, 16);
      this.MinimumSize = new System.Drawing.Size(860, 635);
      this.Name = "AppendDataForm";
      this.Text = "Append Data";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AppendDataForm_FormClosing);
      this.Controls.SetChildIndex(this.FootnoteAreaPanel, 0);
      this.Controls.SetChildIndex(this.ContentAreaPanel, 0);
      this.Controls.SetChildIndex(this.CommandAreaPanel, 0);
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      this.AppendContextMenu.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.ChooseColumnMappingPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ManuallyAdjustMappingPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ColorMapMappedPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ColorMapUnmappedPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.DownArrowPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ColumnWarningPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.TargetMySqlTableDataGridView)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.SourceExcelDataDataGridView)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button AppendButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.Label ManuallyAdjustMappingMainSubLabel;
    private System.Windows.Forms.CheckBox FirstRowHeadersCheckBox;
    private MultiHeaderDataGridView SourceExcelDataDataGridView;
    private System.Windows.Forms.Label ChooseColumnMappingMainSubLabel;
    private System.Windows.Forms.Label ChooseColumnMappingMainLabel;
    private System.Windows.Forms.PictureBox ChooseColumnMappingPictureBox;
    private System.Windows.Forms.Label ManuallyAdjustMappingMainLabel;
    private System.Windows.Forms.PictureBox ManuallyAdjustMappingPictureBox;
    private System.Windows.Forms.Label ExportDataLabel;
    private System.Windows.Forms.PictureBox ColorMapMappedPictureBox;
    private System.Windows.Forms.Label ColorMapMappedLabel;
    private System.Windows.Forms.PictureBox ColorMapUnmappedPictureBox;
    private System.Windows.Forms.Label ColorMapUnmappedLabel;
    private System.Windows.Forms.PictureBox DownArrowPictureBox;
    private System.Windows.Forms.ComboBox MappingMethodComboBox;
    private System.Windows.Forms.Label MappingMethodLabel;
    private System.Windows.Forms.Button AdvancedOptionsButton;
    private System.Windows.Forms.Button StoreMappingButton;
    private MultiHeaderDataGridView TargetMySqlTableDataGridView;
    private System.Windows.Forms.ContextMenuStrip AppendContextMenu;
    private System.Windows.Forms.ToolStripMenuItem RemoveColumnMappingToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem ClearAllMappingsToolStripMenuItem;
    private System.Windows.Forms.Label SubSetOfDataLabel;
    private System.Windows.Forms.Label ColumnWarningLabel;
    private System.Windows.Forms.PictureBox ColumnWarningPictureBox;
  }
}