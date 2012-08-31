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
        if (dataAdapter != null)
          dataAdapter.Dispose();
        if (connection != null)
        {
          connection.Close();
          connection.Dispose();
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
      this.btnRevert = new System.Windows.Forms.Button();
      this.btnCommit = new System.Windows.Forms.Button();
      this.picSakilaLogo = new System.Windows.Forms.PictureBox();
      this.lblMySQLforExcel = new System.Windows.Forms.Label();
      this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.exitEditModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.chkAutoCommit = new System.Windows.Forms.CheckBox();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.picSakilaLogo)).BeginInit();
      this.contextMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnRevert
      // 
      this.btnRevert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnRevert.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnRevert.Location = new System.Drawing.Point(16, 40);
      this.btnRevert.Name = "btnRevert";
      this.btnRevert.Size = new System.Drawing.Size(101, 25);
      this.btnRevert.TabIndex = 1;
      this.btnRevert.Text = "Revert Data";
      this.btnRevert.UseVisualStyleBackColor = true;
      this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
      // 
      // btnCommit
      // 
      this.btnCommit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCommit.Enabled = false;
      this.btnCommit.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnCommit.Location = new System.Drawing.Point(123, 40);
      this.btnCommit.Name = "btnCommit";
      this.btnCommit.Size = new System.Drawing.Size(136, 25);
      this.btnCommit.TabIndex = 2;
      this.btnCommit.Text = "Commit Changes";
      this.btnCommit.UseVisualStyleBackColor = true;
      this.btnCommit.Click += new System.EventHandler(this.btnCommit_Click);
      // 
      // picSakilaLogo
      // 
      this.picSakilaLogo.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_EditDataDlg_Sakila_16x16;
      this.picSakilaLogo.Location = new System.Drawing.Point(6, 6);
      this.picSakilaLogo.Name = "picSakilaLogo";
      this.picSakilaLogo.Size = new System.Drawing.Size(16, 16);
      this.picSakilaLogo.TabIndex = 3;
      this.picSakilaLogo.TabStop = false;
      this.picSakilaLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GenericMouseDown);
      this.picSakilaLogo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GenericMouseMove);
      this.picSakilaLogo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GenericMouseUp);
      // 
      // lblMySQLforExcel
      // 
      this.lblMySQLforExcel.AutoSize = true;
      this.lblMySQLforExcel.BackColor = System.Drawing.Color.Transparent;
      this.lblMySQLforExcel.ContextMenuStrip = this.contextMenu;
      this.lblMySQLforExcel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblMySQLforExcel.ForeColor = System.Drawing.Color.White;
      this.lblMySQLforExcel.Location = new System.Drawing.Point(28, 5);
      this.lblMySQLforExcel.Name = "lblMySQLforExcel";
      this.lblMySQLforExcel.Size = new System.Drawing.Size(108, 17);
      this.lblMySQLforExcel.TabIndex = 4;
      this.lblMySQLforExcel.Text = "MySQL for Excel";
      this.lblMySQLforExcel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GenericMouseDown);
      this.lblMySQLforExcel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GenericMouseMove);
      this.lblMySQLforExcel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GenericMouseUp);
      // 
      // contextMenu
      // 
      this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitEditModeToolStripMenuItem});
      this.contextMenu.Name = "contextMenu";
      this.contextMenu.Size = new System.Drawing.Size(150, 26);
      // 
      // exitEditModeToolStripMenuItem
      // 
      this.exitEditModeToolStripMenuItem.Name = "exitEditModeToolStripMenuItem";
      this.exitEditModeToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
      this.exitEditModeToolStripMenuItem.Text = "Exit Edit Mode";
      this.exitEditModeToolStripMenuItem.Click += new System.EventHandler(this.exitEditModeToolStripMenuItem_Click);
      // 
      // chkAutoCommit
      // 
      this.chkAutoCommit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.chkAutoCommit.AutoSize = true;
      this.chkAutoCommit.BackColor = System.Drawing.Color.Transparent;
      this.chkAutoCommit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkAutoCommit.ForeColor = System.Drawing.Color.White;
      this.chkAutoCommit.Location = new System.Drawing.Point(168, 6);
      this.chkAutoCommit.Name = "chkAutoCommit";
      this.chkAutoCommit.Size = new System.Drawing.Size(101, 19);
      this.chkAutoCommit.TabIndex = 5;
      this.chkAutoCommit.Text = "Auto-Commit";
      this.chkAutoCommit.UseVisualStyleBackColor = false;
      this.chkAutoCommit.CheckedChanged += new System.EventHandler(this.chkAutoCommit_CheckedChanged);
      // 
      // EditDataDialog
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.BackColor = System.Drawing.Color.Black;
      this.ClientSize = new System.Drawing.Size(275, 78);
      this.ContextMenuStrip = this.contextMenu;
      this.ControlBox = false;
      this.Controls.Add(this.chkAutoCommit);
      this.Controls.Add(this.lblMySQLforExcel);
      this.Controls.Add(this.picSakilaLogo);
      this.Controls.Add(this.btnCommit);
      this.Controls.Add(this.btnRevert);
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.MaximizeBox = false;
      this.Name = "EditDataDialog";
      this.Padding = new System.Windows.Forms.Padding(3);
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.TopMost = true;
      ((System.ComponentModel.ISupportInitialize)(this.picSakilaLogo)).EndInit();
      this.contextMenu.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnRevert;
    private System.Windows.Forms.Button btnCommit;
    private System.Windows.Forms.PictureBox picSakilaLogo;
    private System.Windows.Forms.Label lblMySQLforExcel;
    private System.Windows.Forms.CheckBox chkAutoCommit;
    private System.Windows.Forms.ContextMenuStrip contextMenu;
    private System.Windows.Forms.ToolStripMenuItem exitEditModeToolStripMenuItem;
    private System.Windows.Forms.ToolTip toolTip;
  }
}