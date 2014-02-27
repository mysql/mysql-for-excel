// Copyright (c) 2013, Oracle and/or its affiliates. All rights reserved.
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
  partial class GlobalOptionsDialog
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlobalOptionsDialog));
      this.DialogAcceptButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.ConnectionOptionsLabel = new System.Windows.Forms.Label();
      this.GlobalOptionsLabel = new System.Windows.Forms.Label();
      this.ConnectionTimeout1Label = new System.Windows.Forms.Label();
      this.ConnectionTimeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.ConnectionTimeout2Label = new System.Windows.Forms.Label();
      this.QueryTimeout2Label = new System.Windows.Forms.Label();
      this.QueryTimeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.QueryTimeout1Label = new System.Windows.Forms.Label();
      this.SqlQueriesLabel = new System.Windows.Forms.Label();
      this.RestoreSavedEditSessionsCheckBox = new System.Windows.Forms.CheckBox();
      this.EditSessionOptionsLabel = new System.Windows.Forms.Label();
      this.UseOptimisticUpdatesCheckBox = new System.Windows.Forms.CheckBox();
      this.ShowQueriesOptionsPanel = new System.Windows.Forms.Panel();
      this.ShowExecutedSqlQueryRadioButton = new System.Windows.Forms.RadioButton();
      this.PreviewSqlQueriesRadioButton = new System.Windows.Forms.RadioButton();
      this.NoSqlStatementsRadioButton = new System.Windows.Forms.RadioButton();
      this.panel1 = new System.Windows.Forms.Panel();
      this.CreateNewWorksheetsRadioButton = new System.Windows.Forms.RadioButton();
      this.ReuseWorksheetsRadioButton = new System.Windows.Forms.RadioButton();
      this.ExcelToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionTimeoutNumericUpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.QueryTimeoutNumericUpDown)).BeginInit();
      this.ShowQueriesOptionsPanel.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.panel1);
      this.ContentAreaPanel.Controls.Add(this.UseOptimisticUpdatesCheckBox);
      this.ContentAreaPanel.Controls.Add(this.ShowQueriesOptionsPanel);
      this.ContentAreaPanel.Controls.Add(this.RestoreSavedEditSessionsCheckBox);
      this.ContentAreaPanel.Controls.Add(this.EditSessionOptionsLabel);
      this.ContentAreaPanel.Controls.Add(this.SqlQueriesLabel);
      this.ContentAreaPanel.Controls.Add(this.QueryTimeout2Label);
      this.ContentAreaPanel.Controls.Add(this.QueryTimeoutNumericUpDown);
      this.ContentAreaPanel.Controls.Add(this.QueryTimeout1Label);
      this.ContentAreaPanel.Controls.Add(this.ConnectionTimeout2Label);
      this.ContentAreaPanel.Controls.Add(this.ConnectionTimeoutNumericUpDown);
      this.ContentAreaPanel.Controls.Add(this.ConnectionTimeout1Label);
      this.ContentAreaPanel.Controls.Add(this.GlobalOptionsLabel);
      this.ContentAreaPanel.Controls.Add(this.ConnectionOptionsLabel);
      this.ContentAreaPanel.Size = new System.Drawing.Size(504, 451);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.DialogAcceptButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 406);
      this.CommandAreaPanel.Size = new System.Drawing.Size(504, 45);
      // 
      // DialogAcceptButton
      // 
      this.DialogAcceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogAcceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DialogAcceptButton.Location = new System.Drawing.Point(326, 11);
      this.DialogAcceptButton.Name = "DialogAcceptButton";
      this.DialogAcceptButton.Size = new System.Drawing.Size(75, 23);
      this.DialogAcceptButton.TabIndex = 0;
      this.DialogAcceptButton.Text = "Accept";
      this.DialogAcceptButton.UseVisualStyleBackColor = true;
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Location = new System.Drawing.Point(407, 11);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 1;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // ConnectionOptionsLabel
      // 
      this.ConnectionOptionsLabel.AutoSize = true;
      this.ConnectionOptionsLabel.BackColor = System.Drawing.Color.Transparent;
      this.ConnectionOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConnectionOptionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ConnectionOptionsLabel.Location = new System.Drawing.Point(24, 56);
      this.ConnectionOptionsLabel.Name = "ConnectionOptionsLabel";
      this.ConnectionOptionsLabel.Size = new System.Drawing.Size(123, 17);
      this.ConnectionOptionsLabel.TabIndex = 1;
      this.ConnectionOptionsLabel.Text = "Connection Options";
      // 
      // GlobalOptionsLabel
      // 
      this.GlobalOptionsLabel.AutoSize = true;
      this.GlobalOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.GlobalOptionsLabel.ForeColor = System.Drawing.Color.Navy;
      this.GlobalOptionsLabel.Location = new System.Drawing.Point(17, 17);
      this.GlobalOptionsLabel.Name = "GlobalOptionsLabel";
      this.GlobalOptionsLabel.Size = new System.Drawing.Size(109, 20);
      this.GlobalOptionsLabel.TabIndex = 0;
      this.GlobalOptionsLabel.Text = "Global Options";
      // 
      // ConnectionTimeout1Label
      // 
      this.ConnectionTimeout1Label.AutoSize = true;
      this.ConnectionTimeout1Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConnectionTimeout1Label.Location = new System.Drawing.Point(50, 85);
      this.ConnectionTimeout1Label.Name = "ConnectionTimeout1Label";
      this.ConnectionTimeout1Label.Size = new System.Drawing.Size(34, 15);
      this.ConnectionTimeout1Label.TabIndex = 2;
      this.ConnectionTimeout1Label.Text = "Wait ";
      // 
      // ConnectionTimeoutNumericUpDown
      // 
      this.ConnectionTimeoutNumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConnectionTimeoutNumericUpDown.Location = new System.Drawing.Point(90, 83);
      this.ConnectionTimeoutNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      this.ConnectionTimeoutNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.ConnectionTimeoutNumericUpDown.Name = "ConnectionTimeoutNumericUpDown";
      this.ConnectionTimeoutNumericUpDown.Size = new System.Drawing.Size(52, 21);
      this.ConnectionTimeoutNumericUpDown.TabIndex = 3;
      this.ExcelToolTip.SetToolTip(this.ConnectionTimeoutNumericUpDown, "Number of seconds to wait before a connection to a MySQL server times out.\r\nOn ve" +
        "ry slow connections it is advised to raise this value.");
      this.ConnectionTimeoutNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // ConnectionTimeout2Label
      // 
      this.ConnectionTimeout2Label.AutoSize = true;
      this.ConnectionTimeout2Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConnectionTimeout2Label.Location = new System.Drawing.Point(148, 85);
      this.ConnectionTimeout2Label.Name = "ConnectionTimeout2Label";
      this.ConnectionTimeout2Label.Size = new System.Drawing.Size(310, 15);
      this.ConnectionTimeout2Label.TabIndex = 4;
      this.ConnectionTimeout2Label.Text = "seconds for a connection to the server before timing out.";
      // 
      // QueryTimeout2Label
      // 
      this.QueryTimeout2Label.AutoSize = true;
      this.QueryTimeout2Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.QueryTimeout2Label.Location = new System.Drawing.Point(148, 112);
      this.QueryTimeout2Label.Name = "QueryTimeout2Label";
      this.QueryTimeout2Label.Size = new System.Drawing.Size(324, 15);
      this.QueryTimeout2Label.TabIndex = 7;
      this.QueryTimeout2Label.Text = "seconds for a database query to execute before timing out.";
      // 
      // QueryTimeoutNumericUpDown
      // 
      this.QueryTimeoutNumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.QueryTimeoutNumericUpDown.Location = new System.Drawing.Point(90, 110);
      this.QueryTimeoutNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      this.QueryTimeoutNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.QueryTimeoutNumericUpDown.Name = "QueryTimeoutNumericUpDown";
      this.QueryTimeoutNumericUpDown.Size = new System.Drawing.Size(52, 21);
      this.QueryTimeoutNumericUpDown.TabIndex = 6;
      this.ExcelToolTip.SetToolTip(this.QueryTimeoutNumericUpDown, "Number of seconds to wait before a query sent to a MySQL server times out.\r\nOn ve" +
        "ry slow connections or when connecting to a slow computer it is advised to raise" +
        " this value.");
      this.QueryTimeoutNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // QueryTimeout1Label
      // 
      this.QueryTimeout1Label.AutoSize = true;
      this.QueryTimeout1Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.QueryTimeout1Label.Location = new System.Drawing.Point(50, 112);
      this.QueryTimeout1Label.Name = "QueryTimeout1Label";
      this.QueryTimeout1Label.Size = new System.Drawing.Size(34, 15);
      this.QueryTimeout1Label.TabIndex = 5;
      this.QueryTimeout1Label.Text = "Wait ";
      // 
      // SqlQueriesLabel
      // 
      this.SqlQueriesLabel.AutoSize = true;
      this.SqlQueriesLabel.BackColor = System.Drawing.Color.Transparent;
      this.SqlQueriesLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SqlQueriesLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.SqlQueriesLabel.Location = new System.Drawing.Point(24, 151);
      this.SqlQueriesLabel.Name = "SqlQueriesLabel";
      this.SqlQueriesLabel.Size = new System.Drawing.Size(130, 17);
      this.SqlQueriesLabel.TabIndex = 8;
      this.SqlQueriesLabel.Text = "SQL Queries Options";
      // 
      // RestoreSavedEditSessionsCheckBox
      // 
      this.RestoreSavedEditSessionsCheckBox.AutoSize = true;
      this.RestoreSavedEditSessionsCheckBox.Location = new System.Drawing.Point(53, 315);
      this.RestoreSavedEditSessionsCheckBox.Name = "RestoreSavedEditSessionsCheckBox";
      this.RestoreSavedEditSessionsCheckBox.Size = new System.Drawing.Size(323, 17);
      this.RestoreSavedEditSessionsCheckBox.TabIndex = 12;
      this.RestoreSavedEditSessionsCheckBox.Text = "Restore saved Edit sessions when opening an Excel workbook";
      this.ExcelToolTip.SetToolTip(this.RestoreSavedEditSessionsCheckBox, "When checked Edit Data sessions that were active when an Excel workbook was saved" +
        ", are restored when the workbook is opened again.");
      this.RestoreSavedEditSessionsCheckBox.UseVisualStyleBackColor = true;
      this.RestoreSavedEditSessionsCheckBox.CheckedChanged += new System.EventHandler(this.RestoreSavedEditSessionsCheckBox_CheckedChanged);
      // 
      // EditSessionOptionsLabel
      // 
      this.EditSessionOptionsLabel.AutoSize = true;
      this.EditSessionOptionsLabel.BackColor = System.Drawing.Color.Transparent;
      this.EditSessionOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.EditSessionOptionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.EditSessionOptionsLabel.Location = new System.Drawing.Point(24, 286);
      this.EditSessionOptionsLabel.Name = "EditSessionOptionsLabel";
      this.EditSessionOptionsLabel.Size = new System.Drawing.Size(134, 17);
      this.EditSessionOptionsLabel.TabIndex = 11;
      this.EditSessionOptionsLabel.Text = "Edit Sesssion Options";
      // 
      // UseOptimisticUpdatesCheckBox
      // 
      this.UseOptimisticUpdatesCheckBox.AutoSize = true;
      this.UseOptimisticUpdatesCheckBox.Location = new System.Drawing.Point(53, 180);
      this.UseOptimisticUpdatesCheckBox.Name = "UseOptimisticUpdatesCheckBox";
      this.UseOptimisticUpdatesCheckBox.Size = new System.Drawing.Size(250, 17);
      this.UseOptimisticUpdatesCheckBox.TabIndex = 9;
      this.UseOptimisticUpdatesCheckBox.Text = "Use optimistic updates on all Edit Data sessions";
      this.ExcelToolTip.SetToolTip(this.UseOptimisticUpdatesCheckBox, resources.GetString("UseOptimisticUpdatesCheckBox.ToolTip"));
      this.UseOptimisticUpdatesCheckBox.UseVisualStyleBackColor = true;
      // 
      // ShowQueriesOptionsPanel
      // 
      this.ShowQueriesOptionsPanel.Controls.Add(this.ShowExecutedSqlQueryRadioButton);
      this.ShowQueriesOptionsPanel.Controls.Add(this.PreviewSqlQueriesRadioButton);
      this.ShowQueriesOptionsPanel.Controls.Add(this.NoSqlStatementsRadioButton);
      this.ShowQueriesOptionsPanel.Location = new System.Drawing.Point(21, 191);
      this.ShowQueriesOptionsPanel.Name = "ShowQueriesOptionsPanel";
      this.ShowQueriesOptionsPanel.Size = new System.Drawing.Size(461, 79);
      this.ShowQueriesOptionsPanel.TabIndex = 10;
      // 
      // ShowExecutedSqlQueryRadioButton
      // 
      this.ShowExecutedSqlQueryRadioButton.AutoSize = true;
      this.ShowExecutedSqlQueryRadioButton.Location = new System.Drawing.Point(32, 56);
      this.ShowExecutedSqlQueryRadioButton.Name = "ShowExecutedSqlQueryRadioButton";
      this.ShowExecutedSqlQueryRadioButton.Size = new System.Drawing.Size(284, 17);
      this.ShowExecutedSqlQueryRadioButton.TabIndex = 2;
      this.ShowExecutedSqlQueryRadioButton.TabStop = true;
      this.ShowExecutedSqlQueryRadioButton.Text = "Show executed SQL statements along with their results";
      this.ExcelToolTip.SetToolTip(this.ShowExecutedSqlQueryRadioButton, "SQL statements and their execution results are shown after they are sent to the s" +
        "erver.");
      this.ShowExecutedSqlQueryRadioButton.UseVisualStyleBackColor = true;
      // 
      // PreviewSqlQueriesRadioButton
      // 
      this.PreviewSqlQueriesRadioButton.AutoSize = true;
      this.PreviewSqlQueriesRadioButton.Location = new System.Drawing.Point(32, 33);
      this.PreviewSqlQueriesRadioButton.Name = "PreviewSqlQueriesRadioButton";
      this.PreviewSqlQueriesRadioButton.Size = new System.Drawing.Size(300, 17);
      this.PreviewSqlQueriesRadioButton.TabIndex = 1;
      this.PreviewSqlQueriesRadioButton.TabStop = true;
      this.PreviewSqlQueriesRadioButton.Text = "Preview SQL statements before they are sent to the server";
      this.ExcelToolTip.SetToolTip(this.PreviewSqlQueriesRadioButton, "SQL statements are shown, and can be modified, before they are sent to the server" +
        ".");
      this.PreviewSqlQueriesRadioButton.UseVisualStyleBackColor = true;
      // 
      // NoSqlStatementsRadioButton
      // 
      this.NoSqlStatementsRadioButton.AutoSize = true;
      this.NoSqlStatementsRadioButton.Location = new System.Drawing.Point(32, 10);
      this.NoSqlStatementsRadioButton.Name = "NoSqlStatementsRadioButton";
      this.NoSqlStatementsRadioButton.Size = new System.Drawing.Size(248, 17);
      this.NoSqlStatementsRadioButton.TabIndex = 0;
      this.NoSqlStatementsRadioButton.TabStop = true;
      this.NoSqlStatementsRadioButton.Text = "Do not show SQL statements sent to the server";
      this.ExcelToolTip.SetToolTip(this.NoSqlStatementsRadioButton, "SQL statements are never shown, only the results of the executed queries are.");
      this.NoSqlStatementsRadioButton.UseVisualStyleBackColor = true;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.CreateNewWorksheetsRadioButton);
      this.panel1.Controls.Add(this.ReuseWorksheetsRadioButton);
      this.panel1.Location = new System.Drawing.Point(21, 333);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(461, 51);
      this.panel1.TabIndex = 13;
      // 
      // CreateNewWorksheetsRadioButton
      // 
      this.CreateNewWorksheetsRadioButton.AutoSize = true;
      this.CreateNewWorksheetsRadioButton.Location = new System.Drawing.Point(52, 28);
      this.CreateNewWorksheetsRadioButton.Name = "CreateNewWorksheetsRadioButton";
      this.CreateNewWorksheetsRadioButton.Size = new System.Drawing.Size(303, 17);
      this.CreateNewWorksheetsRadioButton.TabIndex = 1;
      this.CreateNewWorksheetsRadioButton.TabStop = true;
      this.CreateNewWorksheetsRadioButton.Text = "Create new Excel worksheets for the restored Edit sessions";
      this.ExcelToolTip.SetToolTip(this.CreateNewWorksheetsRadioButton, "When restoring Edit Data sessions the data will be imported on new worksheets.");
      this.CreateNewWorksheetsRadioButton.UseVisualStyleBackColor = true;
      // 
      // ReuseWorksheetsRadioButton
      // 
      this.ReuseWorksheetsRadioButton.AutoSize = true;
      this.ReuseWorksheetsRadioButton.Location = new System.Drawing.Point(52, 5);
      this.ReuseWorksheetsRadioButton.Name = "ReuseWorksheetsRadioButton";
      this.ReuseWorksheetsRadioButton.Size = new System.Drawing.Size(383, 17);
      this.ReuseWorksheetsRadioButton.TabIndex = 0;
      this.ReuseWorksheetsRadioButton.TabStop = true;
      this.ReuseWorksheetsRadioButton.Text = "Reuse Excel worksheets matching their names with the session table names";
      this.ExcelToolTip.SetToolTip(this.ReuseWorksheetsRadioButton, "When restoring Edit Data sessions the data will be imported on worksheets that ha" +
        "ve the same name as the MySQL table being edited.");
      this.ReuseWorksheetsRadioButton.UseVisualStyleBackColor = true;
      // 
      // ExcelToolTip
      // 
      this.ExcelToolTip.AutoPopDelay = 5000;
      this.ExcelToolTip.InitialDelay = 1000;
      this.ExcelToolTip.ReshowDelay = 100;
      // 
      // GlobalOptionsDialog
      // 
      this.AcceptButton = this.DialogAcceptButton;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(504, 451);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.MainInstructionLocation = new System.Drawing.Point(13, 21);
      this.Name = "GlobalOptionsDialog";
      this.Text = "MySQL for Excel Options";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GlobalOptionsDialog_FormClosing);
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionTimeoutNumericUpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.QueryTimeoutNumericUpDown)).EndInit();
      this.ShowQueriesOptionsPanel.ResumeLayout(false);
      this.ShowQueriesOptionsPanel.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button DialogAcceptButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.Label ConnectionOptionsLabel;
    private System.Windows.Forms.Label GlobalOptionsLabel;
    private System.Windows.Forms.NumericUpDown ConnectionTimeoutNumericUpDown;
    private System.Windows.Forms.Label ConnectionTimeout1Label;
    private System.Windows.Forms.Label QueryTimeout2Label;
    private System.Windows.Forms.NumericUpDown QueryTimeoutNumericUpDown;
    private System.Windows.Forms.Label QueryTimeout1Label;
    private System.Windows.Forms.Label ConnectionTimeout2Label;
    private System.Windows.Forms.Label SqlQueriesLabel;
    private System.Windows.Forms.CheckBox RestoreSavedEditSessionsCheckBox;
    private System.Windows.Forms.Label EditSessionOptionsLabel;
    private System.Windows.Forms.CheckBox UseOptimisticUpdatesCheckBox;
    private System.Windows.Forms.Panel ShowQueriesOptionsPanel;
    private System.Windows.Forms.RadioButton ShowExecutedSqlQueryRadioButton;
    private System.Windows.Forms.RadioButton PreviewSqlQueriesRadioButton;
    private System.Windows.Forms.RadioButton NoSqlStatementsRadioButton;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.RadioButton CreateNewWorksheetsRadioButton;
    private System.Windows.Forms.RadioButton ReuseWorksheetsRadioButton;
    private System.Windows.Forms.ToolTip ExcelToolTip;
  }
}