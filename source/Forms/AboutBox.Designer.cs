// Copyright (c) 2013, 2016, Oracle and/or its affiliates. All rights reserved.
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

namespace MySQL.ForExcel.Forms
{
  partial class AboutBox
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
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
      this.InstallerVersionLabel = new System.Windows.Forms.Label();
      this.ExcelVersionLabel = new System.Windows.Forms.Label();
      this.ForExcelLabel = new System.Windows.Forms.Label();
      this.VersionLabel = new System.Windows.Forms.Label();
      this.CopyrightLabel = new System.Windows.Forms.Label();
      this.TrademarkLabel = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // InstallerVersionLabel
      // 
      this.InstallerVersionLabel.AutoSize = true;
      this.InstallerVersionLabel.BackColor = System.Drawing.Color.Transparent;
      this.InstallerVersionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.InstallerVersionLabel.ForeColor = System.Drawing.Color.White;
      this.InstallerVersionLabel.Location = new System.Drawing.Point(102, 136);
      this.InstallerVersionLabel.Name = "InstallerVersionLabel";
      this.InstallerVersionLabel.Size = new System.Drawing.Size(114, 15);
      this.InstallerVersionLabel.TabIndex = 5;
      this.InstallerVersionLabel.Text = "MySQL Installer 1.4";
      this.InstallerVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // ExcelVersionLabel
      // 
      this.ExcelVersionLabel.AutoSize = true;
      this.ExcelVersionLabel.BackColor = System.Drawing.Color.Transparent;
      this.ExcelVersionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExcelVersionLabel.ForeColor = System.Drawing.Color.White;
      this.ExcelVersionLabel.Location = new System.Drawing.Point(102, 117);
      this.ExcelVersionLabel.Name = "ExcelVersionLabel";
      this.ExcelVersionLabel.Size = new System.Drawing.Size(128, 15);
      this.ExcelVersionLabel.TabIndex = 4;
      this.ExcelVersionLabel.Text = "MySQL for Excel 1.1.x";
      this.ExcelVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // ForExcelLabel
      // 
      this.ForExcelLabel.AutoSize = true;
      this.ForExcelLabel.BackColor = System.Drawing.Color.Transparent;
      this.ForExcelLabel.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ForExcelLabel.ForeColor = System.Drawing.Color.White;
      this.ForExcelLabel.Location = new System.Drawing.Point(96, 56);
      this.ForExcelLabel.Name = "ForExcelLabel";
      this.ForExcelLabel.Size = new System.Drawing.Size(171, 54);
      this.ForExcelLabel.TabIndex = 6;
      this.ForExcelLabel.Text = "for Excel";
      // 
      // VersionLabel
      // 
      this.VersionLabel.AutoSize = true;
      this.VersionLabel.BackColor = System.Drawing.Color.Transparent;
      this.VersionLabel.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.VersionLabel.ForeColor = System.Drawing.Color.White;
      this.VersionLabel.Location = new System.Drawing.Point(257, 56);
      this.VersionLabel.Name = "VersionLabel";
      this.VersionLabel.Size = new System.Drawing.Size(76, 54);
      this.VersionLabel.TabIndex = 7;
      this.VersionLabel.Text = "1.3";
      // 
      // CopyrightLabel
      // 
      this.CopyrightLabel.AutoSize = true;
      this.CopyrightLabel.BackColor = System.Drawing.Color.Transparent;
      this.CopyrightLabel.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CopyrightLabel.ForeColor = System.Drawing.Color.DarkGray;
      this.CopyrightLabel.Location = new System.Drawing.Point(103, 223);
      this.CopyrightLabel.Name = "CopyrightLabel";
      this.CopyrightLabel.Size = new System.Drawing.Size(304, 11);
      this.CopyrightLabel.TabIndex = 8;
      this.CopyrightLabel.Text = "Copyright © 2008, 2016 Oracle and/or its affiliates. All Rights Reserved.";
      // 
      // TrademarkLabel
      // 
      this.TrademarkLabel.AutoSize = true;
      this.TrademarkLabel.BackColor = System.Drawing.Color.Transparent;
      this.TrademarkLabel.Font = new System.Drawing.Font("Tahoma", 6.75F);
      this.TrademarkLabel.ForeColor = System.Drawing.Color.DarkGray;
      this.TrademarkLabel.Location = new System.Drawing.Point(103, 243);
      this.TrademarkLabel.Name = "TrademarkLabel";
      this.TrademarkLabel.Size = new System.Drawing.Size(313, 22);
      this.TrademarkLabel.TabIndex = 9;
      this.TrademarkLabel.Text = "Oracle is a registered trademark of Oracle Corporation and/or its affiliates. \r\nO" +
    "ther names may be trademarks of their respective owners.";
      // 
      // AboutBox
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackgroundImage = global::MySQL.ForExcel.Properties.Resources.SplashScreen;
      this.ClientSize = new System.Drawing.Size(560, 322);
      this.Controls.Add(this.TrademarkLabel);
      this.Controls.Add(this.CopyrightLabel);
      this.Controls.Add(this.VersionLabel);
      this.Controls.Add(this.ForExcelLabel);
      this.Controls.Add(this.InstallerVersionLabel);
      this.Controls.Add(this.ExcelVersionLabel);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutBox";
      this.Padding = new System.Windows.Forms.Padding(9);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "AboutBox";
      this.Click += new System.EventHandler(this.AboutBox_Click);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AboutBox_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label InstallerVersionLabel;
    private System.Windows.Forms.Label ExcelVersionLabel;
    private System.Windows.Forms.Label ForExcelLabel;
    private System.Windows.Forms.Label VersionLabel;
    private System.Windows.Forms.Label CopyrightLabel;
    private System.Windows.Forms.Label TrademarkLabel;
  }
}
