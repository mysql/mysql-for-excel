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
using Excel = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel
{
  partial class EditDataDialog
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
      if (disposing)
      {
        if (EditingWorksheet != null)
        {
          EditingWorksheet.Change -= new Excel.DocEvents_ChangeEventHandler(EditingWorksheet_Change);
          EditingWorksheet.SelectionChange -= new Excel.DocEvents_SelectionChangeEventHandler(EditingWorksheet_SelectionChange);
        }
        if (components != null)
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
      this.RevertDataButton = new System.Windows.Forms.Button();
      this.CommitChangesButton = new System.Windows.Forms.Button();
      this.SakilaLogoPictureBox = new System.Windows.Forms.PictureBox();
      this.MySQLforExcelLabel = new System.Windows.Forms.Label();
      this.EditContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.exitEditModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.AutoCommitCheckBox = new System.Windows.Forms.CheckBox();
      this.DialogToolTip = new System.Windows.Forms.ToolTip(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.SakilaLogoPictureBox)).BeginInit();
      this.EditContextMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // RevertDataButton
      // 
      this.RevertDataButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.RevertDataButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RevertDataButton.Location = new System.Drawing.Point(16, 40);
      this.RevertDataButton.Name = "RevertDataButton";
      this.RevertDataButton.Size = new System.Drawing.Size(101, 25);
      this.RevertDataButton.TabIndex = 2;
      this.RevertDataButton.Text = "Revert Data";
      this.RevertDataButton.UseVisualStyleBackColor = true;
      this.RevertDataButton.Click += new System.EventHandler(this.RevertDataButton_Click);
      // 
      // CommitChangesButton
      // 
      this.CommitChangesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.CommitChangesButton.Enabled = false;
      this.CommitChangesButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CommitChangesButton.Location = new System.Drawing.Point(123, 40);
      this.CommitChangesButton.Name = "CommitChangesButton";
      this.CommitChangesButton.Size = new System.Drawing.Size(136, 25);
      this.CommitChangesButton.TabIndex = 3;
      this.CommitChangesButton.Text = "Commit Changes";
      this.CommitChangesButton.UseVisualStyleBackColor = true;
      this.CommitChangesButton.Click += new System.EventHandler(this.CommitChangesButton_Click);
      // 
      // SakilaLogoPictureBox
      // 
      this.SakilaLogoPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_EditDataDlg_Sakila_16x16;
      this.SakilaLogoPictureBox.Location = new System.Drawing.Point(6, 6);
      this.SakilaLogoPictureBox.Name = "SakilaLogoPictureBox";
      this.SakilaLogoPictureBox.Size = new System.Drawing.Size(16, 16);
      this.SakilaLogoPictureBox.TabIndex = 3;
      this.SakilaLogoPictureBox.TabStop = false;
      this.SakilaLogoPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GenericMouseDown);
      this.SakilaLogoPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GenericMouseMove);
      this.SakilaLogoPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GenericMouseUp);
      // 
      // MySQLforExcelLabel
      // 
      this.MySQLforExcelLabel.AutoSize = true;
      this.MySQLforExcelLabel.BackColor = System.Drawing.Color.Transparent;
      this.MySQLforExcelLabel.ContextMenuStrip = this.EditContextMenu;
      this.MySQLforExcelLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.MySQLforExcelLabel.ForeColor = System.Drawing.Color.White;
      this.MySQLforExcelLabel.Location = new System.Drawing.Point(28, 5);
      this.MySQLforExcelLabel.Name = "MySQLforExcelLabel";
      this.MySQLforExcelLabel.Size = new System.Drawing.Size(108, 17);
      this.MySQLforExcelLabel.TabIndex = 0;
      this.MySQLforExcelLabel.Text = "MySQL for Excel";
      this.MySQLforExcelLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GenericMouseDown);
      this.MySQLforExcelLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GenericMouseMove);
      this.MySQLforExcelLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GenericMouseUp);
      // 
      // EditContextMenu
      // 
      this.EditContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitEditModeToolStripMenuItem});
      this.EditContextMenu.Name = "contextMenu";
      this.EditContextMenu.Size = new System.Drawing.Size(153, 48);
      // 
      // exitEditModeToolStripMenuItem
      // 
      this.exitEditModeToolStripMenuItem.Name = "exitEditModeToolStripMenuItem";
      this.exitEditModeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.exitEditModeToolStripMenuItem.Text = "Exit Edit Mode";
      this.exitEditModeToolStripMenuItem.Click += new System.EventHandler(this.ExitEditModeToolStripMenuItem_Click);
      // 
      // AutoCommitCheckBox
      // 
      this.AutoCommitCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.AutoCommitCheckBox.AutoSize = true;
      this.AutoCommitCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.AutoCommitCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AutoCommitCheckBox.ForeColor = System.Drawing.Color.White;
      this.AutoCommitCheckBox.Location = new System.Drawing.Point(168, 6);
      this.AutoCommitCheckBox.Name = "AutoCommitCheckBox";
      this.AutoCommitCheckBox.Size = new System.Drawing.Size(101, 19);
      this.AutoCommitCheckBox.TabIndex = 1;
      this.AutoCommitCheckBox.Text = "Auto-Commit";
      this.AutoCommitCheckBox.UseVisualStyleBackColor = false;
      this.AutoCommitCheckBox.CheckedChanged += new System.EventHandler(this.AutoCommitCheckBox_CheckedChanged);
      // 
      // EditDataDialog
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.BackColor = System.Drawing.Color.Black;
      this.ClientSize = new System.Drawing.Size(275, 78);
      this.ContextMenuStrip = this.EditContextMenu;
      this.ControlBox = false;
      this.Controls.Add(this.AutoCommitCheckBox);
      this.Controls.Add(this.MySQLforExcelLabel);
      this.Controls.Add(this.SakilaLogoPictureBox);
      this.Controls.Add(this.CommitChangesButton);
      this.Controls.Add(this.RevertDataButton);
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.MaximizeBox = false;
      this.Name = "EditDataDialog";
      this.Padding = new System.Windows.Forms.Padding(3);
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.TopMost = true;
      this.Activated += new System.EventHandler(this.EditDataDialog_Activated);
      this.Shown += new System.EventHandler(this.EditDataDialog_Shown);
      ((System.ComponentModel.ISupportInitialize)(this.SakilaLogoPictureBox)).EndInit();
      this.EditContextMenu.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button RevertDataButton;
    private System.Windows.Forms.Button CommitChangesButton;
    private System.Windows.Forms.PictureBox SakilaLogoPictureBox;
    private System.Windows.Forms.Label MySQLforExcelLabel;
    private System.Windows.Forms.CheckBox AutoCommitCheckBox;
    private System.Windows.Forms.ContextMenuStrip EditContextMenu;
    private System.Windows.Forms.ToolStripMenuItem exitEditModeToolStripMenuItem;
    private System.Windows.Forms.ToolTip DialogToolTip;
  }
}