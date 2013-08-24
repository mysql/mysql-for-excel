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
  partial class ImportProcedureForm
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.ImportButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.CallButton = new System.Windows.Forms.Button();
      this.ParametersPropertyGrid = new System.Windows.Forms.PropertyGrid();
      this.ProcedureParametersSubLabel = new System.Windows.Forms.Label();
      this.ImportResultsetsComboBox = new System.Windows.Forms.ComboBox();
      this.ImportResultsetsLabel = new System.Windows.Forms.Label();
      this.IncludeHeadersCheckBox = new System.Windows.Forms.CheckBox();
      this.ImportOptionsSubLabel = new System.Windows.Forms.Label();
      this.ImportOptionsMainLabel = new System.Windows.Forms.Label();
      this.ColumnOptionsPictureBox = new System.Windows.Forms.PictureBox();
      this.ProcedureNameLabel = new System.Windows.Forms.Label();
      this.ProcedureParametersMainLabel = new System.Windows.Forms.Label();
      this.ProcedureParametersPictureBox = new System.Windows.Forms.PictureBox();
      this.ImportDataLabel = new System.Windows.Forms.Label();
      this.ResultSetsTabControl = new System.Windows.Forms.TabControl();
      this.ResultSetsDataGridView = new MySQL.ForExcel.PreviewDataGridView();
      this.OptionsWarningLabel = new System.Windows.Forms.Label();
      this.OptionsWarningPictureBox = new System.Windows.Forms.PictureBox();
      this.SubSetOfDataLabel = new System.Windows.Forms.Label();
      this.AdvancedOptionsButton = new System.Windows.Forms.Button();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ColumnOptionsPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ProcedureParametersPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ResultSetsDataGridView)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.OptionsWarningPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.OptionsWarningLabel);
      this.ContentAreaPanel.Controls.Add(this.ResultSetsTabControl);
      this.ContentAreaPanel.Controls.Add(this.OptionsWarningPictureBox);
      this.ContentAreaPanel.Controls.Add(this.ImportResultsetsComboBox);
      this.ContentAreaPanel.Controls.Add(this.ImportResultsetsLabel);
      this.ContentAreaPanel.Controls.Add(this.ImportDataLabel);
      this.ContentAreaPanel.Controls.Add(this.CallButton);
      this.ContentAreaPanel.Controls.Add(this.IncludeHeadersCheckBox);
      this.ContentAreaPanel.Controls.Add(this.ParametersPropertyGrid);
      this.ContentAreaPanel.Controls.Add(this.ProcedureParametersSubLabel);
      this.ContentAreaPanel.Controls.Add(this.ImportOptionsSubLabel);
      this.ContentAreaPanel.Controls.Add(this.ImportOptionsMainLabel);
      this.ContentAreaPanel.Controls.Add(this.ColumnOptionsPictureBox);
      this.ContentAreaPanel.Controls.Add(this.ProcedureNameLabel);
      this.ContentAreaPanel.Controls.Add(this.ProcedureParametersMainLabel);
      this.ContentAreaPanel.Controls.Add(this.ProcedureParametersPictureBox);
      this.ContentAreaPanel.Controls.Add(this.ResultSetsDataGridView);
      this.ContentAreaPanel.Controls.Add(this.SubSetOfDataLabel);
      this.ContentAreaPanel.Size = new System.Drawing.Size(846, 601);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.AdvancedOptionsButton);
      this.CommandAreaPanel.Controls.Add(this.ImportButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 556);
      this.CommandAreaPanel.Size = new System.Drawing.Size(846, 45);
      // 
      // ImportButton
      // 
      this.ImportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.ImportButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.ImportButton.Enabled = false;
      this.ImportButton.Location = new System.Drawing.Point(678, 11);
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
      this.DialogCancelButton.Location = new System.Drawing.Point(759, 11);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 2;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // CallButton
      // 
      this.CallButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CallButton.Location = new System.Drawing.Point(666, 75);
      this.CallButton.Name = "CallButton";
      this.CallButton.Size = new System.Drawing.Size(87, 23);
      this.CallButton.TabIndex = 7;
      this.CallButton.Text = "Call";
      this.CallButton.UseVisualStyleBackColor = true;
      this.CallButton.Click += new System.EventHandler(this.CallButton_Click);
      // 
      // ParametersPropertyGrid
      // 
      this.ParametersPropertyGrid.HelpVisible = false;
      this.ParametersPropertyGrid.Location = new System.Drawing.Point(395, 75);
      this.ParametersPropertyGrid.Name = "ParametersPropertyGrid";
      this.ParametersPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
      this.ParametersPropertyGrid.Size = new System.Drawing.Size(265, 80);
      this.ParametersPropertyGrid.TabIndex = 6;
      this.ParametersPropertyGrid.ToolbarVisible = false;
      // 
      // ProcedureParametersSubLabel
      // 
      this.ProcedureParametersSubLabel.AutoSize = true;
      this.ProcedureParametersSubLabel.BackColor = System.Drawing.Color.Transparent;
      this.ProcedureParametersSubLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ProcedureParametersSubLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ProcedureParametersSubLabel.Location = new System.Drawing.Point(79, 73);
      this.ProcedureParametersSubLabel.Name = "ProcedureParametersSubLabel";
      this.ProcedureParametersSubLabel.Size = new System.Drawing.Size(285, 45);
      this.ProcedureParametersSubLabel.TabIndex = 2;
      this.ProcedureParametersSubLabel.Text = "A procedure might need parameters to be set. Please\r\nset a value for all paramete" +
    "rs. Then press the [Call]\r\nbutton to execute the procedure.";
      // 
      // ImportResultsetsComboBox
      // 
      this.ImportResultsetsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.ImportResultsetsComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ImportResultsetsComboBox.FormattingEnabled = true;
      this.ImportResultsetsComboBox.Location = new System.Drawing.Point(444, 192);
      this.ImportResultsetsComboBox.Name = "ImportResultsetsComboBox";
      this.ImportResultsetsComboBox.Size = new System.Drawing.Size(216, 23);
      this.ImportResultsetsComboBox.TabIndex = 2;
      // 
      // ImportResultsetsLabel
      // 
      this.ImportResultsetsLabel.AutoSize = true;
      this.ImportResultsetsLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ImportResultsetsLabel.Location = new System.Drawing.Point(392, 195);
      this.ImportResultsetsLabel.Name = "ImportResultsetsLabel";
      this.ImportResultsetsLabel.Size = new System.Drawing.Size(46, 15);
      this.ImportResultsetsLabel.TabIndex = 1;
      this.ImportResultsetsLabel.Text = "Import:";
      // 
      // IncludeHeadersCheckBox
      // 
      this.IncludeHeadersCheckBox.AutoSize = true;
      this.IncludeHeadersCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.IncludeHeadersCheckBox.Location = new System.Drawing.Point(395, 221);
      this.IncludeHeadersCheckBox.Name = "IncludeHeadersCheckBox";
      this.IncludeHeadersCheckBox.Size = new System.Drawing.Size(211, 19);
      this.IncludeHeadersCheckBox.TabIndex = 0;
      this.IncludeHeadersCheckBox.Text = "Include Column Names as Headers";
      this.IncludeHeadersCheckBox.UseVisualStyleBackColor = true;
      // 
      // ImportOptionsSubLabel
      // 
      this.ImportOptionsSubLabel.AutoSize = true;
      this.ImportOptionsSubLabel.BackColor = System.Drawing.Color.Transparent;
      this.ImportOptionsSubLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ImportOptionsSubLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ImportOptionsSubLabel.Location = new System.Drawing.Point(79, 195);
      this.ImportOptionsSubLabel.Name = "ImportOptionsSubLabel";
      this.ImportOptionsSubLabel.Size = new System.Drawing.Size(276, 45);
      this.ImportOptionsSubLabel.TabIndex = 5;
      this.ImportOptionsSubLabel.Text = "A procedure might return more than one result set.\r\nPlease choose which result se" +
    "t to import or how\r\nto import several result sets.";
      // 
      // ImportOptionsMainLabel
      // 
      this.ImportOptionsMainLabel.AutoSize = true;
      this.ImportOptionsMainLabel.BackColor = System.Drawing.Color.Transparent;
      this.ImportOptionsMainLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ImportOptionsMainLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ImportOptionsMainLabel.Location = new System.Drawing.Point(79, 175);
      this.ImportOptionsMainLabel.Name = "ImportOptionsMainLabel";
      this.ImportOptionsMainLabel.Size = new System.Drawing.Size(111, 17);
      this.ImportOptionsMainLabel.TabIndex = 4;
      this.ImportOptionsMainLabel.Text = "2. Import Options";
      // 
      // ColumnOptionsPictureBox
      // 
      this.ColumnOptionsPictureBox.BackColor = System.Drawing.Color.Transparent;
      this.ColumnOptionsPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ImportRoutineDlg_Options_32x32;
      this.ColumnOptionsPictureBox.Location = new System.Drawing.Point(41, 181);
      this.ColumnOptionsPictureBox.Name = "ColumnOptionsPictureBox";
      this.ColumnOptionsPictureBox.Size = new System.Drawing.Size(32, 32);
      this.ColumnOptionsPictureBox.TabIndex = 31;
      this.ColumnOptionsPictureBox.TabStop = false;
      // 
      // ProcedureNameLabel
      // 
      this.ProcedureNameLabel.AutoSize = true;
      this.ProcedureNameLabel.BackColor = System.Drawing.Color.Transparent;
      this.ProcedureNameLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ProcedureNameLabel.ForeColor = System.Drawing.Color.Navy;
      this.ProcedureNameLabel.Location = new System.Drawing.Point(392, 54);
      this.ProcedureNameLabel.Name = "ProcedureNameLabel";
      this.ProcedureNameLabel.Size = new System.Drawing.Size(43, 17);
      this.ProcedureNameLabel.TabIndex = 1;
      this.ProcedureNameLabel.Text = "Name";
      // 
      // ProcedureParametersMainLabel
      // 
      this.ProcedureParametersMainLabel.AutoSize = true;
      this.ProcedureParametersMainLabel.BackColor = System.Drawing.Color.Transparent;
      this.ProcedureParametersMainLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ProcedureParametersMainLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ProcedureParametersMainLabel.Location = new System.Drawing.Point(79, 54);
      this.ProcedureParametersMainLabel.Name = "ProcedureParametersMainLabel";
      this.ProcedureParametersMainLabel.Size = new System.Drawing.Size(174, 17);
      this.ProcedureParametersMainLabel.TabIndex = 0;
      this.ProcedureParametersMainLabel.Text = "1. Set Procedure Parameters";
      // 
      // ProcedureParametersPictureBox
      // 
      this.ProcedureParametersPictureBox.BackColor = System.Drawing.Color.Transparent;
      this.ProcedureParametersPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ImportRoutineDlg_Params_32x32;
      this.ProcedureParametersPictureBox.Location = new System.Drawing.Point(41, 60);
      this.ProcedureParametersPictureBox.Name = "ProcedureParametersPictureBox";
      this.ProcedureParametersPictureBox.Size = new System.Drawing.Size(32, 32);
      this.ProcedureParametersPictureBox.TabIndex = 20;
      this.ProcedureParametersPictureBox.TabStop = false;
      // 
      // ImportDataLabel
      // 
      this.ImportDataLabel.AutoSize = true;
      this.ImportDataLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ImportDataLabel.ForeColor = System.Drawing.Color.Navy;
      this.ImportDataLabel.Location = new System.Drawing.Point(17, 17);
      this.ImportDataLabel.Name = "ImportDataLabel";
      this.ImportDataLabel.Size = new System.Drawing.Size(176, 20);
      this.ImportDataLabel.TabIndex = 32;
      this.ImportDataLabel.Text = "Import Data from MySQL";
      // 
      // ResultSetsTabControl
      // 
      this.ResultSetsTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ResultSetsTabControl.Location = new System.Drawing.Point(82, 269);
      this.ResultSetsTabControl.Name = "ResultSetsTabControl";
      this.ResultSetsTabControl.SelectedIndex = 0;
      this.ResultSetsTabControl.Size = new System.Drawing.Size(676, 238);
      this.ResultSetsTabControl.TabIndex = 33;
      this.ResultSetsTabControl.SelectedIndexChanged += new System.EventHandler(this.ResultSetsTabControl_SelectedIndexChanged);
      // 
      // ResultSetsDataGridView
      // 
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.ResultSetsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.ResultSetsDataGridView.ColumnsMaximumWidth = 200;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.ResultSetsDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
      this.ResultSetsDataGridView.Location = new System.Drawing.Point(82, 269);
      this.ResultSetsDataGridView.Name = "ResultSetsDataGridView";
      this.ResultSetsDataGridView.Size = new System.Drawing.Size(676, 238);
      this.ResultSetsDataGridView.TabIndex = 9;
      // 
      // OptionsWarningLabel
      // 
      this.OptionsWarningLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.OptionsWarningLabel.AutoSize = true;
      this.OptionsWarningLabel.BackColor = System.Drawing.SystemColors.Window;
      this.OptionsWarningLabel.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OptionsWarningLabel.ForeColor = System.Drawing.Color.Red;
      this.OptionsWarningLabel.Location = new System.Drawing.Point(104, 510);
      this.OptionsWarningLabel.Name = "OptionsWarningLabel";
      this.OptionsWarningLabel.Size = new System.Drawing.Size(76, 12);
      this.OptionsWarningLabel.TabIndex = 25;
      this.OptionsWarningLabel.Text = "Warning Message";
      this.OptionsWarningLabel.Visible = false;
      // 
      // OptionsWarningPictureBox
      // 
      this.OptionsWarningPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.OptionsWarningPictureBox.BackColor = System.Drawing.SystemColors.Window;
      this.OptionsWarningPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.Warning;
      this.OptionsWarningPictureBox.Location = new System.Drawing.Point(82, 505);
      this.OptionsWarningPictureBox.Name = "OptionsWarningPictureBox";
      this.OptionsWarningPictureBox.Size = new System.Drawing.Size(20, 20);
      this.OptionsWarningPictureBox.TabIndex = 26;
      this.OptionsWarningPictureBox.TabStop = false;
      this.OptionsWarningPictureBox.Visible = false;
      // 
      // SubSetOfDataLabel
      // 
      this.SubSetOfDataLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.SubSetOfDataLabel.AutoSize = true;
      this.SubSetOfDataLabel.BackColor = System.Drawing.Color.Transparent;
      this.SubSetOfDataLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SubSetOfDataLabel.ForeColor = System.Drawing.SystemColors.InactiveCaption;
      this.SubSetOfDataLabel.Location = new System.Drawing.Point(434, 508);
      this.SubSetOfDataLabel.Name = "SubSetOfDataLabel";
      this.SubSetOfDataLabel.Size = new System.Drawing.Size(319, 15);
      this.SubSetOfDataLabel.TabIndex = 7;
      this.SubSetOfDataLabel.Text = "This is a small subset of the data for preview purposes only.";
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
      // ImportProcedureForm
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(846, 601);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MainInstructionLocation = new System.Drawing.Point(11, 15);
      this.MinimumSize = new System.Drawing.Size(862, 640);
      this.Name = "ImportProcedureForm";
      this.Text = "Import Data";
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.ColumnOptionsPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ProcedureParametersPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ResultSetsDataGridView)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.OptionsWarningPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button ImportButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.Button CallButton;
    private System.Windows.Forms.PropertyGrid ParametersPropertyGrid;
    private System.Windows.Forms.Label ProcedureParametersSubLabel;
    private System.Windows.Forms.ComboBox ImportResultsetsComboBox;
    private System.Windows.Forms.Label ImportResultsetsLabel;
    private System.Windows.Forms.CheckBox IncludeHeadersCheckBox;
    private System.Windows.Forms.Label ImportOptionsSubLabel;
    private System.Windows.Forms.Label ImportOptionsMainLabel;
    private System.Windows.Forms.PictureBox ColumnOptionsPictureBox;
    private System.Windows.Forms.Label ProcedureNameLabel;
    private System.Windows.Forms.Label ProcedureParametersMainLabel;
    private System.Windows.Forms.PictureBox ProcedureParametersPictureBox;
    private System.Windows.Forms.Label ImportDataLabel;
    private System.Windows.Forms.TabControl ResultSetsTabControl;
    private PreviewDataGridView ResultSetsDataGridView;
    private System.Windows.Forms.Label OptionsWarningLabel;
    private System.Windows.Forms.PictureBox OptionsWarningPictureBox;
    private System.Windows.Forms.Label SubSetOfDataLabel;
    private System.Windows.Forms.Button AdvancedOptionsButton;
  }
}