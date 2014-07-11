// Copyright (c) 2014, Oracle and/or its affiliates. All rights reserved.
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
  partial class PreviewTableViewDialog
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

        if (_previewDataTable != null)
        {
          _previewDataTable.Dispose();
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.RowsCountSubLabel = new System.Windows.Forms.Label();
      this.PreviewDataGridView = new MySQL.ForExcel.Controls.PreviewDataGridView();
      this.PreviewPictureBox = new System.Windows.Forms.PictureBox();
      this.RowsCountMainLabel = new System.Windows.Forms.Label();
      this.TableNameSubLabel = new System.Windows.Forms.Label();
      this.TableNameMainLabel = new System.Windows.Forms.Label();
      this.OkButton = new System.Windows.Forms.Button();
      this.ExportDataLabel = new System.Windows.Forms.Label();
      this.SubSetOfDataLabel = new System.Windows.Forms.Label();
      this.RowsLabel = new System.Windows.Forms.Label();
      this.RowsNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.PreviewLabel = new System.Windows.Forms.Label();
      this.RefreshButton = new System.Windows.Forms.Button();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.PreviewDataGridView)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.PreviewPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.RowsNumericUpDown)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.RefreshButton);
      this.ContentAreaPanel.Controls.Add(this.PreviewLabel);
      this.ContentAreaPanel.Controls.Add(this.RowsLabel);
      this.ContentAreaPanel.Controls.Add(this.RowsNumericUpDown);
      this.ContentAreaPanel.Controls.Add(this.SubSetOfDataLabel);
      this.ContentAreaPanel.Controls.Add(this.ExportDataLabel);
      this.ContentAreaPanel.Controls.Add(this.RowsCountSubLabel);
      this.ContentAreaPanel.Controls.Add(this.PreviewDataGridView);
      this.ContentAreaPanel.Controls.Add(this.PreviewPictureBox);
      this.ContentAreaPanel.Controls.Add(this.RowsCountMainLabel);
      this.ContentAreaPanel.Controls.Add(this.TableNameSubLabel);
      this.ContentAreaPanel.Controls.Add(this.TableNameMainLabel);
      this.ContentAreaPanel.Size = new System.Drawing.Size(749, 461);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.OkButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 416);
      this.CommandAreaPanel.Size = new System.Drawing.Size(749, 45);
      // 
      // RowsCountSubLabel
      // 
      this.RowsCountSubLabel.AutoSize = true;
      this.RowsCountSubLabel.BackColor = System.Drawing.Color.Transparent;
      this.RowsCountSubLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RowsCountSubLabel.ForeColor = System.Drawing.Color.Navy;
      this.RowsCountSubLabel.Location = new System.Drawing.Point(157, 73);
      this.RowsCountSubLabel.Name = "RowsCountSubLabel";
      this.RowsCountSubLabel.Size = new System.Drawing.Size(13, 15);
      this.RowsCountSubLabel.TabIndex = 4;
      this.RowsCountSubLabel.Text = "0";
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
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.PreviewDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
      this.PreviewDataGridView.Enabled = false;
      this.PreviewDataGridView.Location = new System.Drawing.Point(39, 106);
      this.PreviewDataGridView.Name = "PreviewDataGridView";
      this.PreviewDataGridView.Size = new System.Drawing.Size(668, 265);
      this.PreviewDataGridView.TabIndex = 6;
      this.PreviewDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.PreviewDataGridView_DataBindingComplete);
      this.PreviewDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.PreviewDataGridView_DataError);
      // 
      // PreviewPictureBox
      // 
      this.PreviewPictureBox.BackColor = System.Drawing.Color.Transparent;
      this.PreviewPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_ColumnOptions_32x32;
      this.PreviewPictureBox.Location = new System.Drawing.Point(39, 56);
      this.PreviewPictureBox.Name = "PreviewPictureBox";
      this.PreviewPictureBox.Size = new System.Drawing.Size(32, 32);
      this.PreviewPictureBox.TabIndex = 29;
      this.PreviewPictureBox.TabStop = false;
      // 
      // RowsCountMainLabel
      // 
      this.RowsCountMainLabel.AutoSize = true;
      this.RowsCountMainLabel.BackColor = System.Drawing.Color.Transparent;
      this.RowsCountMainLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RowsCountMainLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.RowsCountMainLabel.Location = new System.Drawing.Point(77, 73);
      this.RowsCountMainLabel.Name = "RowsCountMainLabel";
      this.RowsCountMainLabel.Size = new System.Drawing.Size(69, 15);
      this.RowsCountMainLabel.TabIndex = 3;
      this.RowsCountMainLabel.Text = "Row Count:";
      // 
      // TableNameSubLabel
      // 
      this.TableNameSubLabel.AutoSize = true;
      this.TableNameSubLabel.BackColor = System.Drawing.Color.Transparent;
      this.TableNameSubLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TableNameSubLabel.ForeColor = System.Drawing.Color.Navy;
      this.TableNameSubLabel.Location = new System.Drawing.Point(157, 56);
      this.TableNameSubLabel.Name = "TableNameSubLabel";
      this.TableNameSubLabel.Size = new System.Drawing.Size(39, 15);
      this.TableNameSubLabel.TabIndex = 2;
      this.TableNameSubLabel.Text = "Name";
      // 
      // TableNameMainLabel
      // 
      this.TableNameMainLabel.AutoSize = true;
      this.TableNameMainLabel.BackColor = System.Drawing.Color.Transparent;
      this.TableNameMainLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TableNameMainLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.TableNameMainLabel.Location = new System.Drawing.Point(77, 56);
      this.TableNameMainLabel.Name = "TableNameMainLabel";
      this.TableNameMainLabel.Size = new System.Drawing.Size(74, 15);
      this.TableNameMainLabel.TabIndex = 1;
      this.TableNameMainLabel.Text = "Table Name:";
      // 
      // OkButton
      // 
      this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.OkButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OkButton.Location = new System.Drawing.Point(662, 10);
      this.OkButton.Name = "OkButton";
      this.OkButton.Size = new System.Drawing.Size(75, 23);
      this.OkButton.TabIndex = 0;
      this.OkButton.Text = "OK";
      this.OkButton.UseVisualStyleBackColor = true;
      // 
      // ExportDataLabel
      // 
      this.ExportDataLabel.AutoSize = true;
      this.ExportDataLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExportDataLabel.ForeColor = System.Drawing.Color.Navy;
      this.ExportDataLabel.Location = new System.Drawing.Point(17, 17);
      this.ExportDataLabel.Name = "ExportDataLabel";
      this.ExportDataLabel.Size = new System.Drawing.Size(146, 20);
      this.ExportDataLabel.TabIndex = 0;
      this.ExportDataLabel.Text = "Preview MySQL Data";
      // 
      // SubSetOfDataLabel
      // 
      this.SubSetOfDataLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.SubSetOfDataLabel.AutoSize = true;
      this.SubSetOfDataLabel.BackColor = System.Drawing.Color.Transparent;
      this.SubSetOfDataLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SubSetOfDataLabel.ForeColor = System.Drawing.SystemColors.InactiveCaption;
      this.SubSetOfDataLabel.Location = new System.Drawing.Point(419, 88);
      this.SubSetOfDataLabel.Name = "SubSetOfDataLabel";
      this.SubSetOfDataLabel.Size = new System.Drawing.Size(288, 15);
      this.SubSetOfDataLabel.TabIndex = 5;
      this.SubSetOfDataLabel.Text = "This is a subset of the data for preview purposes only.";
      // 
      // RowsLabel
      // 
      this.RowsLabel.AutoSize = true;
      this.RowsLabel.Location = new System.Drawing.Point(157, 380);
      this.RowsLabel.Name = "RowsLabel";
      this.RowsLabel.Size = new System.Drawing.Size(32, 13);
      this.RowsLabel.TabIndex = 9;
      this.RowsLabel.Text = "rows.";
      // 
      // RowsNumericUpDown
      // 
      this.RowsNumericUpDown.Location = new System.Drawing.Point(91, 377);
      this.RowsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.RowsNumericUpDown.Name = "RowsNumericUpDown";
      this.RowsNumericUpDown.Size = new System.Drawing.Size(60, 20);
      this.RowsNumericUpDown.TabIndex = 8;
      this.RowsNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
      // 
      // PreviewLabel
      // 
      this.PreviewLabel.AutoSize = true;
      this.PreviewLabel.Location = new System.Drawing.Point(40, 380);
      this.PreviewLabel.Name = "PreviewLabel";
      this.PreviewLabel.Size = new System.Drawing.Size(45, 13);
      this.PreviewLabel.TabIndex = 7;
      this.PreviewLabel.Text = "Preview";
      // 
      // RefreshButton
      // 
      this.RefreshButton.Location = new System.Drawing.Point(195, 375);
      this.RefreshButton.Name = "RefreshButton";
      this.RefreshButton.Size = new System.Drawing.Size(75, 23);
      this.RefreshButton.TabIndex = 10;
      this.RefreshButton.Text = "Refresh";
      this.RefreshButton.UseVisualStyleBackColor = true;
      this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
      // 
      // PreviewTableViewDialog
      // 
      this.AcceptButton = this.OkButton;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.OkButton;
      this.ClientSize = new System.Drawing.Size(749, 461);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MainInstructionLocation = new System.Drawing.Point(10, 14);
      this.MinimumSize = new System.Drawing.Size(565, 335);
      this.Name = "PreviewTableViewDialog";
      this.Text = "Preview Data";
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.PreviewDataGridView)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.PreviewPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.RowsNumericUpDown)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label RowsCountSubLabel;
    private PreviewDataGridView PreviewDataGridView;
    private System.Windows.Forms.PictureBox PreviewPictureBox;
    private System.Windows.Forms.Label RowsCountMainLabel;
    private System.Windows.Forms.Label TableNameSubLabel;
    private System.Windows.Forms.Label TableNameMainLabel;
    private System.Windows.Forms.Button OkButton;
    private System.Windows.Forms.Label ExportDataLabel;
    private System.Windows.Forms.Label SubSetOfDataLabel;
    private System.Windows.Forms.Label PreviewLabel;
    private System.Windows.Forms.Label RowsLabel;
    private System.Windows.Forms.NumericUpDown RowsNumericUpDown;
    private System.Windows.Forms.Button RefreshButton;

  }
}