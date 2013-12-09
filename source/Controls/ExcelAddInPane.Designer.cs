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

using System.Diagnostics;
using MySQL.ForExcel.Panels;
using MySQL.Utility.Classes;

namespace MySQL.ForExcel.Controls
{
  partial class ExcelAddInPane
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
        CloseConnection();
        if (ProtectedWorksheetPasskeys.Count > 0)
        {
          foreach (var dictEntry in ProtectedWorksheetPasskeys)
          {
            MySqlSourceTrace.WriteToLog(string.Format(Properties.Resources.WorkSheetInEditModeSavedLogWarning, dictEntry.Key, dictEntry.Value), SourceLevels.Warning);
          }

          ProtectedWorksheetPasskeys.Clear();
        }

        if (components != null)
        {
          components.Dispose();
        }
      }

      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.WelcomePanel1 = new WelcomePanel();
      this.DBObjectSelectionPanel3 = new DbObjectSelectionPanel();
      this.SchemaSelectionPanel2 = new SchemaSelectionPanel();
      this.SuspendLayout();
      // 
      // WelcomePanel1
      // 
      this.WelcomePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.WelcomePanel1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.WelcomePanel1.Location = new System.Drawing.Point(0, 0);
      this.WelcomePanel1.Name = "WelcomePanel1";
      this.WelcomePanel1.Size = new System.Drawing.Size(260, 625);
      this.WelcomePanel1.TabIndex = 0;
      // 
      // DBObjectSelectionPanel3
      // 
      this.DBObjectSelectionPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.DBObjectSelectionPanel3.Font = new System.Drawing.Font("Arial", 9F);
      this.DBObjectSelectionPanel3.Location = new System.Drawing.Point(0, 0);
      this.DBObjectSelectionPanel3.Name = "DBObjectSelectionPanel3";
      this.DBObjectSelectionPanel3.Size = new System.Drawing.Size(260, 625);
      this.DBObjectSelectionPanel3.TabIndex = 2;
      // 
      // SchemaSelectionPanel2
      // 
      this.SchemaSelectionPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.SchemaSelectionPanel2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SchemaSelectionPanel2.Location = new System.Drawing.Point(0, 0);
      this.SchemaSelectionPanel2.Name = "SchemaSelectionPanel2";
      this.SchemaSelectionPanel2.Size = new System.Drawing.Size(260, 625);
      this.SchemaSelectionPanel2.TabIndex = 1;
      // 
      // TaskPaneControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.WelcomePanel1);
      this.Controls.Add(this.DBObjectSelectionPanel3);
      this.Controls.Add(this.SchemaSelectionPanel2);
      this.Name = "TaskPaneControl";
      this.Size = new System.Drawing.Size(260, 625);
      this.ResumeLayout(false);

    }

    #endregion

    private WelcomePanel WelcomePanel1;
    private SchemaSelectionPanel SchemaSelectionPanel2;
    private DbObjectSelectionPanel DBObjectSelectionPanel3;

  }
}
