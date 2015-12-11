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
  partial class ExportDataForm
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

        if (_exportDataTable != null)
        {
          _exportDataTable.Dispose();
        }

        if (_previewDataTable != null)
        {
          _previewDataTable.Dispose();
        }

        // Set variables to null so this object does not hold references to them and the GC disposes of them sooner.
        _exportDataRange = null;
        _wbConnection = null;
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportDataForm));
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      this.TextChangedTimer = new System.Windows.Forms.Timer(this.components);
      this.AdvancedOptionsButton = new System.Windows.Forms.Button();
      this.ExportButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.PrimaryKeyWarningLabel = new System.Windows.Forms.Label();
      this.PrimaryKeyWarningPictureBox = new System.Windows.Forms.PictureBox();
      this.ColumnOptionsGroupBox = new System.Windows.Forms.GroupBox();
      this.DefaultValuePictureBox = new System.Windows.Forms.PictureBox();
      this.AutoIncrementCheckBox = new System.Windows.Forms.CheckBox();
      this.DefaultValueTextBox = new System.Windows.Forms.TextBox();
      this.DefaultValueLabel = new System.Windows.Forms.Label();
      this.DataTypeComboBox = new System.Windows.Forms.ComboBox();
      this.DataTypeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.UnsignedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.ZeroFillToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.ExcludeColumnCheckBox = new System.Windows.Forms.CheckBox();
      this.AllowEmptyCheckBox = new System.Windows.Forms.CheckBox();
      this.PrimaryKeyCheckBox = new System.Windows.Forms.CheckBox();
      this.UniqueIndexCheckBox = new System.Windows.Forms.CheckBox();
      this.CreateIndexCheckBox = new System.Windows.Forms.CheckBox();
      this.DatatypeLabel = new System.Windows.Forms.Label();
      this.ColumnNameTextBox = new System.Windows.Forms.TextBox();
      this.ColumnNameLabel = new System.Windows.Forms.Label();
      this.MoreInfoLinkLabel = new System.Windows.Forms.LinkLabel();
      this.ColumnOptionsWarningLabel = new System.Windows.Forms.Label();
      this.ColumnOptionsWarningPictureBox = new System.Windows.Forms.PictureBox();
      this.FirstRowHeadersCheckBox = new System.Windows.Forms.CheckBox();
      this.ColumnOptionsSubLabel = new System.Windows.Forms.Label();
      this.ColumnOptionsMainLabel = new System.Windows.Forms.Label();
      this.ColumnOptionsPictureBox = new System.Windows.Forms.PictureBox();
      this.TableNameWarningLabel = new System.Windows.Forms.Label();
      this.TableNameWarningPictureBox = new System.Windows.Forms.PictureBox();
      this.PrimaryKeyColumnsComboBox = new System.Windows.Forms.ComboBox();
      this.UseExistingColumnRadioButton = new System.Windows.Forms.RadioButton();
      this.AddPrimaryKeyTextBox = new System.Windows.Forms.TextBox();
      this.AddPrimaryKeyRadioButton = new System.Windows.Forms.RadioButton();
      this.TableNameInputTextBox = new System.Windows.Forms.TextBox();
      this.TableNameInputLabel = new System.Windows.Forms.Label();
      this.PrimaryKeySubLabel = new System.Windows.Forms.Label();
      this.PrimaryKeyMainLabel = new System.Windows.Forms.Label();
      this.PrimaryKeyPictureBox = new System.Windows.Forms.PictureBox();
      this.TableNameSubLabel = new System.Windows.Forms.Label();
      this.TableNameMainLabel = new System.Windows.Forms.Label();
      this.TablePictureBox = new System.Windows.Forms.PictureBox();
      this.ExportDataLabel = new System.Windows.Forms.Label();
      this.SubSetOfDataLabel = new System.Windows.Forms.Label();
      this.DropDownButton = new System.Windows.Forms.Button();
      this.ExportContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.ExportDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.CreateTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.CollationComboBox = new System.Windows.Forms.ComboBox();
      this.CollationLabel = new System.Windows.Forms.Label();
      this.DataTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.HelpToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.PreviewDataGridView = new MySQL.ForExcel.Controls.PreviewDataGridView();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.PrimaryKeyWarningPictureBox)).BeginInit();
      this.ColumnOptionsGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DefaultValuePictureBox)).BeginInit();
      this.DataTypeContextMenuStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ColumnOptionsWarningPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ColumnOptionsPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.TableNameWarningPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.PrimaryKeyPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.TablePictureBox)).BeginInit();
      this.ExportContextMenuStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DataTypeBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.PreviewDataGridView)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.MoreInfoLinkLabel);
      this.ContentAreaPanel.Controls.Add(this.CollationComboBox);
      this.ContentAreaPanel.Controls.Add(this.CollationLabel);
      this.ContentAreaPanel.Controls.Add(this.SubSetOfDataLabel);
      this.ContentAreaPanel.Controls.Add(this.ColumnOptionsWarningLabel);
      this.ContentAreaPanel.Controls.Add(this.ColumnOptionsWarningPictureBox);
      this.ContentAreaPanel.Controls.Add(this.ExportDataLabel);
      this.ContentAreaPanel.Controls.Add(this.PrimaryKeyWarningLabel);
      this.ContentAreaPanel.Controls.Add(this.PrimaryKeyWarningPictureBox);
      this.ContentAreaPanel.Controls.Add(this.ColumnOptionsGroupBox);
      this.ContentAreaPanel.Controls.Add(this.FirstRowHeadersCheckBox);
      this.ContentAreaPanel.Controls.Add(this.PreviewDataGridView);
      this.ContentAreaPanel.Controls.Add(this.ColumnOptionsSubLabel);
      this.ContentAreaPanel.Controls.Add(this.ColumnOptionsMainLabel);
      this.ContentAreaPanel.Controls.Add(this.ColumnOptionsPictureBox);
      this.ContentAreaPanel.Controls.Add(this.TableNameWarningLabel);
      this.ContentAreaPanel.Controls.Add(this.TableNameWarningPictureBox);
      this.ContentAreaPanel.Controls.Add(this.PrimaryKeyColumnsComboBox);
      this.ContentAreaPanel.Controls.Add(this.UseExistingColumnRadioButton);
      this.ContentAreaPanel.Controls.Add(this.AddPrimaryKeyTextBox);
      this.ContentAreaPanel.Controls.Add(this.AddPrimaryKeyRadioButton);
      this.ContentAreaPanel.Controls.Add(this.TableNameInputTextBox);
      this.ContentAreaPanel.Controls.Add(this.TableNameInputLabel);
      this.ContentAreaPanel.Controls.Add(this.PrimaryKeySubLabel);
      this.ContentAreaPanel.Controls.Add(this.PrimaryKeyMainLabel);
      this.ContentAreaPanel.Controls.Add(this.PrimaryKeyPictureBox);
      this.ContentAreaPanel.Controls.Add(this.TableNameSubLabel);
      this.ContentAreaPanel.Controls.Add(this.TableNameMainLabel);
      this.ContentAreaPanel.Controls.Add(this.TablePictureBox);
      this.ContentAreaPanel.Size = new System.Drawing.Size(884, 625);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.DropDownButton);
      this.CommandAreaPanel.Controls.Add(this.AdvancedOptionsButton);
      this.CommandAreaPanel.Controls.Add(this.ExportButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 580);
      this.CommandAreaPanel.Size = new System.Drawing.Size(884, 45);
      // 
      // TextChangedTimer
      // 
      this.TextChangedTimer.Interval = 800;
      this.TextChangedTimer.Tick += new System.EventHandler(this.TextChangedTimerTick);
      // 
      // AdvancedOptionsButton
      // 
      this.AdvancedOptionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.AdvancedOptionsButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AdvancedOptionsButton.Location = new System.Drawing.Point(12, 12);
      this.AdvancedOptionsButton.Name = "AdvancedOptionsButton";
      this.AdvancedOptionsButton.Size = new System.Drawing.Size(140, 23);
      this.AdvancedOptionsButton.TabIndex = 0;
      this.AdvancedOptionsButton.Text = "Advanced Options...";
      this.AdvancedOptionsButton.UseVisualStyleBackColor = true;
      this.AdvancedOptionsButton.Click += new System.EventHandler(this.AdvancedOptionsButton_Click);
      // 
      // ExportButton
      // 
      this.ExportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.ExportButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.ExportButton.Enabled = false;
      this.ExportButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExportButton.Location = new System.Drawing.Point(689, 12);
      this.ExportButton.Name = "ExportButton";
      this.ExportButton.Size = new System.Drawing.Size(102, 23);
      this.ExportButton.TabIndex = 1;
      this.ExportButton.Text = "Export Data  ";
      this.ExportButton.UseVisualStyleBackColor = true;
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogCancelButton.Location = new System.Drawing.Point(797, 12);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 3;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // PrimaryKeyWarningLabel
      // 
      this.PrimaryKeyWarningLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.PrimaryKeyWarningLabel.BackColor = System.Drawing.Color.Transparent;
      this.PrimaryKeyWarningLabel.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PrimaryKeyWarningLabel.ForeColor = System.Drawing.Color.Red;
      this.PrimaryKeyWarningLabel.Location = new System.Drawing.Point(528, 172);
      this.PrimaryKeyWarningLabel.Name = "PrimaryKeyWarningLabel";
      this.PrimaryKeyWarningLabel.Size = new System.Drawing.Size(274, 26);
      this.PrimaryKeyWarningLabel.TabIndex = 14;
      this.PrimaryKeyWarningLabel.Text = "Primary Key column cannot be created because another column has the same name.";
      this.PrimaryKeyWarningLabel.Visible = false;
      // 
      // PrimaryKeyWarningPictureBox
      // 
      this.PrimaryKeyWarningPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.PrimaryKeyWarningPictureBox.BackColor = System.Drawing.Color.Transparent;
      this.PrimaryKeyWarningPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.Warning;
      this.PrimaryKeyWarningPictureBox.Location = new System.Drawing.Point(505, 173);
      this.PrimaryKeyWarningPictureBox.Name = "PrimaryKeyWarningPictureBox";
      this.PrimaryKeyWarningPictureBox.Size = new System.Drawing.Size(20, 20);
      this.PrimaryKeyWarningPictureBox.TabIndex = 45;
      this.PrimaryKeyWarningPictureBox.TabStop = false;
      this.PrimaryKeyWarningPictureBox.Visible = false;
      // 
      // ColumnOptionsGroupBox
      // 
      this.ColumnOptionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ColumnOptionsGroupBox.BackColor = System.Drawing.Color.Transparent;
      this.ColumnOptionsGroupBox.Controls.Add(this.DefaultValuePictureBox);
      this.ColumnOptionsGroupBox.Controls.Add(this.AutoIncrementCheckBox);
      this.ColumnOptionsGroupBox.Controls.Add(this.DefaultValueTextBox);
      this.ColumnOptionsGroupBox.Controls.Add(this.DefaultValueLabel);
      this.ColumnOptionsGroupBox.Controls.Add(this.DataTypeComboBox);
      this.ColumnOptionsGroupBox.Controls.Add(this.ExcludeColumnCheckBox);
      this.ColumnOptionsGroupBox.Controls.Add(this.AllowEmptyCheckBox);
      this.ColumnOptionsGroupBox.Controls.Add(this.PrimaryKeyCheckBox);
      this.ColumnOptionsGroupBox.Controls.Add(this.UniqueIndexCheckBox);
      this.ColumnOptionsGroupBox.Controls.Add(this.CreateIndexCheckBox);
      this.ColumnOptionsGroupBox.Controls.Add(this.DatatypeLabel);
      this.ColumnOptionsGroupBox.Controls.Add(this.ColumnNameTextBox);
      this.ColumnOptionsGroupBox.Controls.Add(this.ColumnNameLabel);
      this.ColumnOptionsGroupBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ColumnOptionsGroupBox.Location = new System.Drawing.Point(82, 444);
      this.ColumnOptionsGroupBox.Name = "ColumnOptionsGroupBox";
      this.ColumnOptionsGroupBox.Size = new System.Drawing.Size(720, 113);
      this.ColumnOptionsGroupBox.TabIndex = 20;
      this.ColumnOptionsGroupBox.TabStop = false;
      this.ColumnOptionsGroupBox.Text = "Column Options";
      // 
      // DefaultValuePictureBox
      // 
      this.DefaultValuePictureBox.Image = global::MySQL.ForExcel.Properties.Resources.help;
      this.DefaultValuePictureBox.Location = new System.Drawing.Point(297, 84);
      this.DefaultValuePictureBox.Name = "DefaultValuePictureBox";
      this.DefaultValuePictureBox.Size = new System.Drawing.Size(16, 16);
      this.DefaultValuePictureBox.TabIndex = 12;
      this.DefaultValuePictureBox.TabStop = false;
      this.HelpToolTip.SetToolTip(this.DefaultValuePictureBox, "Click here to assign CURRENT_TIMESTAMP as the default value for DateTime or TimeS" +
        "tamp type.");
      this.DefaultValuePictureBox.Visible = false;
      this.DefaultValuePictureBox.Click += new System.EventHandler(this.DefaultValuePictureBox_Click);
      // 
      // AutoIncrementCheckBox
      // 
      this.AutoIncrementCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.AutoIncrementCheckBox.AutoSize = true;
      this.AutoIncrementCheckBox.Location = new System.Drawing.Point(589, 53);
      this.AutoIncrementCheckBox.Name = "AutoIncrementCheckBox";
      this.AutoIncrementCheckBox.Size = new System.Drawing.Size(109, 19);
      this.AutoIncrementCheckBox.TabIndex = 11;
      this.AutoIncrementCheckBox.Text = "Auto Increment";
      this.HelpToolTip.SetToolTip(this.AutoIncrementCheckBox, resources.GetString("AutoIncrementCheckBox.ToolTip"));
      this.AutoIncrementCheckBox.UseVisualStyleBackColor = true;
      this.AutoIncrementCheckBox.CheckedChanged += new System.EventHandler(this.AutoIncrementCheckBox_CheckedChanged);
      // 
      // DefaultValueTextBox
      // 
      this.DefaultValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.DefaultValueTextBox.Location = new System.Drawing.Point(118, 81);
      this.DefaultValueTextBox.Name = "DefaultValueTextBox";
      this.DefaultValueTextBox.Size = new System.Drawing.Size(173, 23);
      this.DefaultValueTextBox.TabIndex = 5;
      this.HelpToolTip.SetToolTip(this.DefaultValueTextBox, "A default value can\'t be applied to Blob, Text, Geometry and JSON data types.\r\nIf" +
        " a default value is set, the column can\'t be set to Auto Increment its values.");
      this.DefaultValueTextBox.TextChanged += new System.EventHandler(this.DefaultValueTextBox_TextChanged);
      this.DefaultValueTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.DefaultValueTextBox_Validating);
      // 
      // DefaultValueLabel
      // 
      this.DefaultValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.DefaultValueLabel.AutoSize = true;
      this.DefaultValueLabel.Location = new System.Drawing.Point(32, 85);
      this.DefaultValueLabel.Name = "DefaultValueLabel";
      this.DefaultValueLabel.Size = new System.Drawing.Size(80, 15);
      this.DefaultValueLabel.TabIndex = 4;
      this.DefaultValueLabel.Text = "Default Value:";
      // 
      // DataTypeComboBox
      // 
      this.DataTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.DataTypeComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.DataTypeComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.DataTypeComboBox.ContextMenuStrip = this.DataTypeContextMenuStrip;
      this.DataTypeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.DataTypeComboBox.DropDownWidth = 400;
      this.DataTypeComboBox.FormattingEnabled = true;
      this.DataTypeComboBox.Location = new System.Drawing.Point(118, 51);
      this.DataTypeComboBox.Name = "DataTypeComboBox";
      this.DataTypeComboBox.Size = new System.Drawing.Size(173, 24);
      this.DataTypeComboBox.TabIndex = 3;
      this.HelpToolTip.SetToolTip(this.DataTypeComboBox, resources.GetString("DataTypeComboBox.ToolTip"));
      this.DataTypeComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.DataTypeComboBoxDrawItem);
      this.DataTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.DataTypeComboBox_SelectedIndexChanged);
      this.DataTypeComboBox.TextChanged += new System.EventHandler(this.DataTypeComboBox_TextChanged);
      this.DataTypeComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.DataTypeComboBox_Validating);
      // 
      // DataTypeContextMenuStrip
      // 
      this.DataTypeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UnsignedToolStripMenuItem,
            this.ZeroFillToolStripMenuItem});
      this.DataTypeContextMenuStrip.Name = "DataTypeContextMenuStrip";
      this.DataTypeContextMenuStrip.Size = new System.Drawing.Size(125, 48);
      this.DataTypeContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.DataTypeContextMenuStrip_Opening);
      // 
      // UnsignedToolStripMenuItem
      // 
      this.UnsignedToolStripMenuItem.CheckOnClick = true;
      this.UnsignedToolStripMenuItem.Name = "UnsignedToolStripMenuItem";
      this.UnsignedToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
      this.UnsignedToolStripMenuItem.Text = "Unsigned";
      this.UnsignedToolStripMenuItem.CheckedChanged += new System.EventHandler(this.UnsignedToolStripMenuItem_CheckedChanged);
      // 
      // ZeroFillToolStripMenuItem
      // 
      this.ZeroFillToolStripMenuItem.CheckOnClick = true;
      this.ZeroFillToolStripMenuItem.Name = "ZeroFillToolStripMenuItem";
      this.ZeroFillToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
      this.ZeroFillToolStripMenuItem.Text = "Zero Fill";
      this.ZeroFillToolStripMenuItem.CheckedChanged += new System.EventHandler(this.ZeroFillToolStripMenuItem_CheckedChanged);
      // 
      // ExcludeColumnCheckBox
      // 
      this.ExcludeColumnCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ExcludeColumnCheckBox.AutoSize = true;
      this.ExcludeColumnCheckBox.Location = new System.Drawing.Point(589, 24);
      this.ExcludeColumnCheckBox.Name = "ExcludeColumnCheckBox";
      this.ExcludeColumnCheckBox.Size = new System.Drawing.Size(112, 19);
      this.ExcludeColumnCheckBox.TabIndex = 10;
      this.ExcludeColumnCheckBox.Text = "Exclude Column";
      this.HelpToolTip.SetToolTip(this.ExcludeColumnCheckBox, "When checked, the column is excluded from the table creation and Export Data oper" +
        "ation.");
      this.ExcludeColumnCheckBox.UseVisualStyleBackColor = true;
      this.ExcludeColumnCheckBox.CheckedChanged += new System.EventHandler(this.ExcludeCheckBox_CheckedChanged);
      // 
      // AllowEmptyCheckBox
      // 
      this.AllowEmptyCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.AllowEmptyCheckBox.AutoSize = true;
      this.AllowEmptyCheckBox.Location = new System.Drawing.Point(464, 53);
      this.AllowEmptyCheckBox.Name = "AllowEmptyCheckBox";
      this.AllowEmptyCheckBox.Size = new System.Drawing.Size(93, 19);
      this.AllowEmptyCheckBox.TabIndex = 9;
      this.AllowEmptyCheckBox.Text = "Allow Empty";
      this.HelpToolTip.SetToolTip(this.AllowEmptyCheckBox, resources.GetString("AllowEmptyCheckBox.ToolTip"));
      this.AllowEmptyCheckBox.UseVisualStyleBackColor = true;
      this.AllowEmptyCheckBox.CheckedChanged += new System.EventHandler(this.AllowEmptyCheckBox_CheckedChanged);
      // 
      // PrimaryKeyCheckBox
      // 
      this.PrimaryKeyCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.PrimaryKeyCheckBox.AutoSize = true;
      this.PrimaryKeyCheckBox.Location = new System.Drawing.Point(331, 24);
      this.PrimaryKeyCheckBox.Name = "PrimaryKeyCheckBox";
      this.PrimaryKeyCheckBox.Size = new System.Drawing.Size(89, 19);
      this.PrimaryKeyCheckBox.TabIndex = 6;
      this.PrimaryKeyCheckBox.Text = "Primary Key";
      this.HelpToolTip.SetToolTip(this.PrimaryKeyCheckBox, "A Primary Key can be a multiple-column index, and it represents a unique index wh" +
        "ere all key columns must not allow empty (null) values.\r\n");
      this.PrimaryKeyCheckBox.UseVisualStyleBackColor = true;
      this.PrimaryKeyCheckBox.CheckedChanged += new System.EventHandler(this.PrimaryKeyCheckBox_CheckedChanged);
      // 
      // UniqueIndexCheckBox
      // 
      this.UniqueIndexCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.UniqueIndexCheckBox.AutoSize = true;
      this.UniqueIndexCheckBox.Location = new System.Drawing.Point(331, 53);
      this.UniqueIndexCheckBox.Name = "UniqueIndexCheckBox";
      this.UniqueIndexCheckBox.Size = new System.Drawing.Size(95, 19);
      this.UniqueIndexCheckBox.TabIndex = 7;
      this.UniqueIndexCheckBox.Text = "Unique Index";
      this.HelpToolTip.SetToolTip(this.UniqueIndexCheckBox, "A Unique Index creates a constraint such that all values in the index must be dis" +
        "tinct.\r\nAn error occurs if you try to add a new row with a key value that matche" +
        "s an existing row. ");
      this.UniqueIndexCheckBox.UseVisualStyleBackColor = true;
      this.UniqueIndexCheckBox.CheckedChanged += new System.EventHandler(this.UniqueIndexCheckBox_CheckedChanged);
      // 
      // CreateIndexCheckBox
      // 
      this.CreateIndexCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.CreateIndexCheckBox.AutoSize = true;
      this.CreateIndexCheckBox.Location = new System.Drawing.Point(464, 24);
      this.CreateIndexCheckBox.Name = "CreateIndexCheckBox";
      this.CreateIndexCheckBox.Size = new System.Drawing.Size(91, 19);
      this.CreateIndexCheckBox.TabIndex = 8;
      this.CreateIndexCheckBox.Text = "Create Index";
      this.HelpToolTip.SetToolTip(this.CreateIndexCheckBox, resources.GetString("CreateIndexCheckBox.ToolTip"));
      this.CreateIndexCheckBox.UseVisualStyleBackColor = true;
      this.CreateIndexCheckBox.CheckedChanged += new System.EventHandler(this.CreateIndexCheckBox_CheckedChanged);
      // 
      // DatatypeLabel
      // 
      this.DatatypeLabel.AutoSize = true;
      this.DatatypeLabel.Location = new System.Drawing.Point(49, 54);
      this.DatatypeLabel.Name = "DatatypeLabel";
      this.DatatypeLabel.Size = new System.Drawing.Size(63, 15);
      this.DatatypeLabel.TabIndex = 2;
      this.DatatypeLabel.Text = "Data Type:";
      // 
      // ColumnNameTextBox
      // 
      this.ColumnNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ColumnNameTextBox.Location = new System.Drawing.Point(118, 22);
      this.ColumnNameTextBox.Name = "ColumnNameTextBox";
      this.ColumnNameTextBox.Size = new System.Drawing.Size(173, 23);
      this.ColumnNameTextBox.TabIndex = 1;
      this.HelpToolTip.SetToolTip(this.ColumnNameTextBox, "The name of the column.");
      this.ColumnNameTextBox.TextChanged += new System.EventHandler(this.ColumnNameTextBox_TextChanged);
      this.ColumnNameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ColumnNameTextBox_Validating);
      // 
      // ColumnNameLabel
      // 
      this.ColumnNameLabel.AutoSize = true;
      this.ColumnNameLabel.Location = new System.Drawing.Point(24, 25);
      this.ColumnNameLabel.Name = "ColumnNameLabel";
      this.ColumnNameLabel.Size = new System.Drawing.Size(88, 15);
      this.ColumnNameLabel.TabIndex = 0;
      this.ColumnNameLabel.Text = "Column Name:";
      // 
      // MoreInfoLinkLabel
      // 
      this.MoreInfoLinkLabel.AutoSize = true;
      this.MoreInfoLinkLabel.BackColor = System.Drawing.SystemColors.Window;
      this.MoreInfoLinkLabel.Font = new System.Drawing.Font("Segoe UI", 6.75F);
      this.MoreInfoLinkLabel.Location = new System.Drawing.Point(434, 446);
      this.MoreInfoLinkLabel.Name = "MoreInfoLinkLabel";
      this.MoreInfoLinkLabel.Size = new System.Drawing.Size(84, 12);
      this.MoreInfoLinkLabel.TabIndex = 9;
      this.MoreInfoLinkLabel.TabStop = true;
      this.MoreInfoLinkLabel.Text = " (More Information) ";
      this.MoreInfoLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.MoreInfoLinkLabel_LinkClicked);
      // 
      // ColumnOptionsWarningLabel
      // 
      this.ColumnOptionsWarningLabel.AutoSize = true;
      this.ColumnOptionsWarningLabel.BackColor = System.Drawing.SystemColors.Window;
      this.ColumnOptionsWarningLabel.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ColumnOptionsWarningLabel.ForeColor = System.Drawing.Color.Red;
      this.ColumnOptionsWarningLabel.Location = new System.Drawing.Point(208, 446);
      this.ColumnOptionsWarningLabel.Name = "ColumnOptionsWarningLabel";
      this.ColumnOptionsWarningLabel.Size = new System.Drawing.Size(227, 12);
      this.ColumnOptionsWarningLabel.TabIndex = 21;
      this.ColumnOptionsWarningLabel.Text = "It is good practice to not use upper case letters or spaces.";
      this.ColumnOptionsWarningLabel.Visible = false;
      // 
      // ColumnOptionsWarningPictureBox
      // 
      this.ColumnOptionsWarningPictureBox.BackColor = System.Drawing.SystemColors.Window;
      this.ColumnOptionsWarningPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.Warning;
      this.ColumnOptionsWarningPictureBox.Location = new System.Drawing.Point(186, 442);
      this.ColumnOptionsWarningPictureBox.Name = "ColumnOptionsWarningPictureBox";
      this.ColumnOptionsWarningPictureBox.Size = new System.Drawing.Size(20, 20);
      this.ColumnOptionsWarningPictureBox.TabIndex = 24;
      this.ColumnOptionsWarningPictureBox.TabStop = false;
      this.ColumnOptionsWarningPictureBox.Visible = false;
      // 
      // FirstRowHeadersCheckBox
      // 
      this.FirstRowHeadersCheckBox.AutoSize = true;
      this.FirstRowHeadersCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.FirstRowHeadersCheckBox.Checked = true;
      this.FirstRowHeadersCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.FirstRowHeadersCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FirstRowHeadersCheckBox.Location = new System.Drawing.Point(82, 254);
      this.FirstRowHeadersCheckBox.Name = "FirstRowHeadersCheckBox";
      this.FirstRowHeadersCheckBox.Size = new System.Drawing.Size(210, 19);
      this.FirstRowHeadersCheckBox.TabIndex = 17;
      this.FirstRowHeadersCheckBox.Text = "First Row Contains Column Names";
      this.HelpToolTip.SetToolTip(this.FirstRowHeadersCheckBox, "When checked, the first data row is used to set the names of the columns and the " +
        "data rows start on the second column.");
      this.FirstRowHeadersCheckBox.UseVisualStyleBackColor = false;
      this.FirstRowHeadersCheckBox.CheckedChanged += new System.EventHandler(this.FirstRowHeadersCheckBox_CheckedChanged);
      // 
      // ColumnOptionsSubLabel
      // 
      this.ColumnOptionsSubLabel.AutoSize = true;
      this.ColumnOptionsSubLabel.BackColor = System.Drawing.Color.Transparent;
      this.ColumnOptionsSubLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ColumnOptionsSubLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ColumnOptionsSubLabel.Location = new System.Drawing.Point(79, 224);
      this.ColumnOptionsSubLabel.Name = "ColumnOptionsSubLabel";
      this.ColumnOptionsSubLabel.Size = new System.Drawing.Size(438, 15);
      this.ColumnOptionsSubLabel.TabIndex = 16;
      this.ColumnOptionsSubLabel.Text = "Click the header of a column to specify options like column name and a datatype.";
      // 
      // ColumnOptionsMainLabel
      // 
      this.ColumnOptionsMainLabel.AutoSize = true;
      this.ColumnOptionsMainLabel.BackColor = System.Drawing.Color.Transparent;
      this.ColumnOptionsMainLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ColumnOptionsMainLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ColumnOptionsMainLabel.Location = new System.Drawing.Point(79, 204);
      this.ColumnOptionsMainLabel.Name = "ColumnOptionsMainLabel";
      this.ColumnOptionsMainLabel.Size = new System.Drawing.Size(161, 17);
      this.ColumnOptionsMainLabel.TabIndex = 15;
      this.ColumnOptionsMainLabel.Text = "3. Specify Column Options";
      // 
      // ColumnOptionsPictureBox
      // 
      this.ColumnOptionsPictureBox.BackColor = System.Drawing.Color.Transparent;
      this.ColumnOptionsPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_ColumnOptions_32x32;
      this.ColumnOptionsPictureBox.Location = new System.Drawing.Point(41, 207);
      this.ColumnOptionsPictureBox.Name = "ColumnOptionsPictureBox";
      this.ColumnOptionsPictureBox.Size = new System.Drawing.Size(32, 32);
      this.ColumnOptionsPictureBox.TabIndex = 41;
      this.ColumnOptionsPictureBox.TabStop = false;
      // 
      // TableNameWarningLabel
      // 
      this.TableNameWarningLabel.AutoSize = true;
      this.TableNameWarningLabel.BackColor = System.Drawing.Color.Transparent;
      this.TableNameWarningLabel.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TableNameWarningLabel.ForeColor = System.Drawing.Color.Red;
      this.TableNameWarningLabel.Location = new System.Drawing.Point(150, 176);
      this.TableNameWarningLabel.Name = "TableNameWarningLabel";
      this.TableNameWarningLabel.Size = new System.Drawing.Size(227, 12);
      this.TableNameWarningLabel.TabIndex = 7;
      this.TableNameWarningLabel.Text = "It is good practice to not use upper case letters or spaces.";
      this.TableNameWarningLabel.Visible = false;
      // 
      // TableNameWarningPictureBox
      // 
      this.TableNameWarningPictureBox.BackColor = System.Drawing.Color.Transparent;
      this.TableNameWarningPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.Warning;
      this.TableNameWarningPictureBox.Location = new System.Drawing.Point(127, 171);
      this.TableNameWarningPictureBox.Name = "TableNameWarningPictureBox";
      this.TableNameWarningPictureBox.Size = new System.Drawing.Size(20, 20);
      this.TableNameWarningPictureBox.TabIndex = 38;
      this.TableNameWarningPictureBox.TabStop = false;
      this.TableNameWarningPictureBox.Visible = false;
      // 
      // PrimaryKeyColumnsComboBox
      // 
      this.PrimaryKeyColumnsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.PrimaryKeyColumnsComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.PrimaryKeyColumnsComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.PrimaryKeyColumnsComboBox.DisplayMember = "DisplayName";
      this.PrimaryKeyColumnsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.PrimaryKeyColumnsComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PrimaryKeyColumnsComboBox.FormattingEnabled = true;
      this.PrimaryKeyColumnsComboBox.Location = new System.Drawing.Point(681, 146);
      this.PrimaryKeyColumnsComboBox.Name = "PrimaryKeyColumnsComboBox";
      this.PrimaryKeyColumnsComboBox.Size = new System.Drawing.Size(121, 23);
      this.PrimaryKeyColumnsComboBox.TabIndex = 13;
      this.HelpToolTip.SetToolTip(this.PrimaryKeyColumnsComboBox, "Sets one of the data columns being exported as the table\'s Primary Key column.");
      this.PrimaryKeyColumnsComboBox.ValueMember = "DisplayName";
      this.PrimaryKeyColumnsComboBox.SelectedIndexChanged += new System.EventHandler(this.PrimaryKeyColumnsComboBox_SelectedIndexChanged);
      // 
      // UseExistingColumnRadioButton
      // 
      this.UseExistingColumnRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.UseExistingColumnRadioButton.AutoSize = true;
      this.UseExistingColumnRadioButton.BackColor = System.Drawing.Color.Transparent;
      this.UseExistingColumnRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UseExistingColumnRadioButton.Location = new System.Drawing.Point(505, 146);
      this.UseExistingColumnRadioButton.Name = "UseExistingColumnRadioButton";
      this.UseExistingColumnRadioButton.Size = new System.Drawing.Size(134, 19);
      this.UseExistingColumnRadioButton.TabIndex = 12;
      this.UseExistingColumnRadioButton.TabStop = true;
      this.UseExistingColumnRadioButton.Text = "Use existing column:";
      this.UseExistingColumnRadioButton.UseVisualStyleBackColor = false;
      this.UseExistingColumnRadioButton.CheckedChanged += new System.EventHandler(this.UseExistingColumnRadioButton_CheckedChanged);
      // 
      // AddPrimaryKeyTextBox
      // 
      this.AddPrimaryKeyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.AddPrimaryKeyTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AddPrimaryKeyTextBox.Location = new System.Drawing.Point(680, 118);
      this.AddPrimaryKeyTextBox.Name = "AddPrimaryKeyTextBox";
      this.AddPrimaryKeyTextBox.Size = new System.Drawing.Size(122, 22);
      this.AddPrimaryKeyTextBox.TabIndex = 11;
      this.HelpToolTip.SetToolTip(this.AddPrimaryKeyTextBox, "Prepends an integer Primary Key column with the given name.");
      this.AddPrimaryKeyTextBox.TextChanged += new System.EventHandler(this.AddPrimaryKeyTextBox_TextChanged);
      this.AddPrimaryKeyTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.AddPrimaryKeyTextBox_Validating);
      // 
      // AddPrimaryKeyRadioButton
      // 
      this.AddPrimaryKeyRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.AddPrimaryKeyRadioButton.AutoSize = true;
      this.AddPrimaryKeyRadioButton.BackColor = System.Drawing.Color.Transparent;
      this.AddPrimaryKeyRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AddPrimaryKeyRadioButton.Location = new System.Drawing.Point(505, 118);
      this.AddPrimaryKeyRadioButton.Name = "AddPrimaryKeyRadioButton";
      this.AddPrimaryKeyRadioButton.Size = new System.Drawing.Size(169, 19);
      this.AddPrimaryKeyRadioButton.TabIndex = 10;
      this.AddPrimaryKeyRadioButton.TabStop = true;
      this.AddPrimaryKeyRadioButton.Text = "Add a Primary Key column:";
      this.AddPrimaryKeyRadioButton.UseVisualStyleBackColor = false;
      this.AddPrimaryKeyRadioButton.CheckedChanged += new System.EventHandler(this.AddPrimaryKeyRadioButton_CheckedChanged);
      // 
      // TableNameInputTextBox
      // 
      this.TableNameInputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TableNameInputTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TableNameInputTextBox.Location = new System.Drawing.Point(127, 118);
      this.TableNameInputTextBox.Name = "TableNameInputTextBox";
      this.TableNameInputTextBox.Size = new System.Drawing.Size(250, 23);
      this.TableNameInputTextBox.TabIndex = 4;
      this.HelpToolTip.SetToolTip(this.TableNameInputTextBox, "Mandatory field.\r\nIt is good practice to not use upper case letters or spaces.");
      this.TableNameInputTextBox.TextChanged += new System.EventHandler(this.TableNameInputTextBox_TextChanged);
      this.TableNameInputTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.TableNameInputTextBox_Validating);
      // 
      // TableNameInputLabel
      // 
      this.TableNameInputLabel.AutoSize = true;
      this.TableNameInputLabel.BackColor = System.Drawing.Color.Transparent;
      this.TableNameInputLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TableNameInputLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.TableNameInputLabel.Location = new System.Drawing.Point(79, 122);
      this.TableNameInputLabel.Name = "TableNameInputLabel";
      this.TableNameInputLabel.Size = new System.Drawing.Size(42, 15);
      this.TableNameInputLabel.TabIndex = 3;
      this.TableNameInputLabel.Text = "Name:";
      // 
      // PrimaryKeySubLabel
      // 
      this.PrimaryKeySubLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.PrimaryKeySubLabel.BackColor = System.Drawing.Color.Transparent;
      this.PrimaryKeySubLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PrimaryKeySubLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.PrimaryKeySubLabel.Location = new System.Drawing.Point(502, 73);
      this.PrimaryKeySubLabel.Name = "PrimaryKeySubLabel";
      this.PrimaryKeySubLabel.Size = new System.Drawing.Size(300, 30);
      this.PrimaryKeySubLabel.TabIndex = 9;
      this.PrimaryKeySubLabel.Text = "Each row of data needs to hold a unique number that is used as the Primary Key.\r\n" +
    "";
      // 
      // PrimaryKeyMainLabel
      // 
      this.PrimaryKeyMainLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.PrimaryKeyMainLabel.AutoSize = true;
      this.PrimaryKeyMainLabel.BackColor = System.Drawing.Color.Transparent;
      this.PrimaryKeyMainLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PrimaryKeyMainLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.PrimaryKeyMainLabel.Location = new System.Drawing.Point(502, 56);
      this.PrimaryKeyMainLabel.Name = "PrimaryKeyMainLabel";
      this.PrimaryKeyMainLabel.Size = new System.Drawing.Size(128, 17);
      this.PrimaryKeyMainLabel.TabIndex = 8;
      this.PrimaryKeyMainLabel.Text = "2. Pick a Primary Key";
      // 
      // PrimaryKeyPictureBox
      // 
      this.PrimaryKeyPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.PrimaryKeyPictureBox.BackColor = System.Drawing.Color.Transparent;
      this.PrimaryKeyPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_PrimaryKey_32x32;
      this.PrimaryKeyPictureBox.Location = new System.Drawing.Point(464, 59);
      this.PrimaryKeyPictureBox.Name = "PrimaryKeyPictureBox";
      this.PrimaryKeyPictureBox.Size = new System.Drawing.Size(32, 32);
      this.PrimaryKeyPictureBox.TabIndex = 28;
      this.PrimaryKeyPictureBox.TabStop = false;
      // 
      // TableNameSubLabel
      // 
      this.TableNameSubLabel.AutoSize = true;
      this.TableNameSubLabel.BackColor = System.Drawing.Color.Transparent;
      this.TableNameSubLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TableNameSubLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.TableNameSubLabel.Location = new System.Drawing.Point(79, 71);
      this.TableNameSubLabel.Name = "TableNameSubLabel";
      this.TableNameSubLabel.Size = new System.Drawing.Size(267, 30);
      this.TableNameSubLabel.TabIndex = 2;
      this.TableNameSubLabel.Text = "The selected data will be stored in a MySQL table.\r\nPlease specify a unique name " +
    "for the table.";
      // 
      // TableNameMainLabel
      // 
      this.TableNameMainLabel.AutoSize = true;
      this.TableNameMainLabel.BackColor = System.Drawing.Color.Transparent;
      this.TableNameMainLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TableNameMainLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.TableNameMainLabel.Location = new System.Drawing.Point(79, 54);
      this.TableNameMainLabel.Name = "TableNameMainLabel";
      this.TableNameMainLabel.Size = new System.Drawing.Size(126, 17);
      this.TableNameMainLabel.TabIndex = 1;
      this.TableNameMainLabel.Text = "1. Set a Table Name";
      // 
      // TablePictureBox
      // 
      this.TablePictureBox.BackColor = System.Drawing.Color.Transparent;
      this.TablePictureBox.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_TableName_32x32;
      this.TablePictureBox.Location = new System.Drawing.Point(41, 57);
      this.TablePictureBox.Name = "TablePictureBox";
      this.TablePictureBox.Size = new System.Drawing.Size(32, 32);
      this.TablePictureBox.TabIndex = 23;
      this.TablePictureBox.TabStop = false;
      // 
      // ExportDataLabel
      // 
      this.ExportDataLabel.AutoSize = true;
      this.ExportDataLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExportDataLabel.ForeColor = System.Drawing.Color.Navy;
      this.ExportDataLabel.Location = new System.Drawing.Point(17, 17);
      this.ExportDataLabel.Name = "ExportDataLabel";
      this.ExportDataLabel.Size = new System.Drawing.Size(156, 20);
      this.ExportDataLabel.TabIndex = 0;
      this.ExportDataLabel.Text = "Export Data to MySQL";
      // 
      // SubSetOfDataLabel
      // 
      this.SubSetOfDataLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.SubSetOfDataLabel.AutoSize = true;
      this.SubSetOfDataLabel.BackColor = System.Drawing.Color.Transparent;
      this.SubSetOfDataLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SubSetOfDataLabel.ForeColor = System.Drawing.SystemColors.InactiveCaption;
      this.SubSetOfDataLabel.Location = new System.Drawing.Point(483, 255);
      this.SubSetOfDataLabel.Name = "SubSetOfDataLabel";
      this.SubSetOfDataLabel.Size = new System.Drawing.Size(319, 15);
      this.SubSetOfDataLabel.TabIndex = 18;
      this.SubSetOfDataLabel.Text = "This is a small subset of the data for preview purposes only.";
      // 
      // DropDownButton
      // 
      this.DropDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DropDownButton.FlatAppearance.BorderSize = 0;
      this.DropDownButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.DropDownButton.Font = new System.Drawing.Font("Segoe UI", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DropDownButton.Location = new System.Drawing.Point(777, 16);
      this.DropDownButton.Margin = new System.Windows.Forms.Padding(0);
      this.DropDownButton.Name = "DropDownButton";
      this.DropDownButton.Size = new System.Drawing.Size(12, 15);
      this.DropDownButton.TabIndex = 2;
      this.DropDownButton.Text = "▼";
      this.HelpToolTip.SetToolTip(this.DropDownButton, "Click here to select between Export Data and Create Table operations.");
      this.DropDownButton.UseVisualStyleBackColor = true;
      this.DropDownButton.Click += new System.EventHandler(this.DropDownButton_Click);
      // 
      // ExportContextMenuStrip
      // 
      this.ExportContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExportDataToolStripMenuItem,
            this.CreateTableToolStripMenuItem});
      this.ExportContextMenuStrip.Name = "ExportContextMenuStrip";
      this.ExportContextMenuStrip.ShowCheckMargin = true;
      this.ExportContextMenuStrip.ShowImageMargin = false;
      this.ExportContextMenuStrip.Size = new System.Drawing.Size(141, 48);
      // 
      // ExportDataToolStripMenuItem
      // 
      this.ExportDataToolStripMenuItem.Checked = true;
      this.ExportDataToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ExportDataToolStripMenuItem.Name = "ExportDataToolStripMenuItem";
      this.ExportDataToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
      this.ExportDataToolStripMenuItem.Text = "Export Data";
      this.ExportDataToolStripMenuItem.Click += new System.EventHandler(this.ExportDataToolStripMenuItem_Click);
      // 
      // CreateTableToolStripMenuItem
      // 
      this.CreateTableToolStripMenuItem.Name = "CreateTableToolStripMenuItem";
      this.CreateTableToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
      this.CreateTableToolStripMenuItem.Text = "Create Table";
      this.CreateTableToolStripMenuItem.Click += new System.EventHandler(this.CreateTableToolStripMenuItem_Click);
      // 
      // CollationComboBox
      // 
      this.CollationComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.CollationComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.CollationComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.CollationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.CollationComboBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.CollationComboBox.FormattingEnabled = true;
      this.CollationComboBox.Location = new System.Drawing.Point(127, 146);
      this.CollationComboBox.Name = "CollationComboBox";
      this.CollationComboBox.Size = new System.Drawing.Size(250, 23);
      this.CollationComboBox.TabIndex = 6;
      this.HelpToolTip.SetToolTip(this.CollationComboBox, "Specifies a default collation for the table.\r\nBy default the table will inherit t" +
        "he collation from its parent schema.");
      // 
      // CollationLabel
      // 
      this.CollationLabel.AutoSize = true;
      this.CollationLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CollationLabel.Location = new System.Drawing.Point(63, 150);
      this.CollationLabel.Name = "CollationLabel";
      this.CollationLabel.Size = new System.Drawing.Size(58, 15);
      this.CollationLabel.TabIndex = 5;
      this.CollationLabel.Text = "Collation:";
      // 
      // HelpToolTip
      // 
      this.HelpToolTip.AutoPopDelay = 5000;
      this.HelpToolTip.InitialDelay = 1000;
      this.HelpToolTip.ReshowDelay = 100;
      // 
      // PreviewDataGridView
      // 
      this.PreviewDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.PreviewDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.PreviewDataGridView.ColumnsMaximumWidth = 200;
      this.PreviewDataGridView.ColumnsMinimumWidth = 50;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.PreviewDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
      this.PreviewDataGridView.Location = new System.Drawing.Point(82, 277);
      this.PreviewDataGridView.MultiSelect = false;
      this.PreviewDataGridView.Name = "PreviewDataGridView";
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.PreviewDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
      this.PreviewDataGridView.Size = new System.Drawing.Size(720, 159);
      this.PreviewDataGridView.TabIndex = 19;
      this.PreviewDataGridView.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.PreviewDataGridView_CellToolTipTextNeeded);
      this.PreviewDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.PreviewDataGridView_DataBindingComplete);
      this.PreviewDataGridView.SelectionChanged += new System.EventHandler(this.PreviewDataGridView_SelectionChanged);
      this.PreviewDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PreviewDataGridView_KeyDown);
      // 
      // ExportDataForm
      // 
      this.AcceptButton = this.ExportButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(884, 625);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MainInstructionLocation = new System.Drawing.Point(10, 15);
      this.MinimumSize = new System.Drawing.Size(900, 664);
      this.Name = "ExportDataForm";
      this.Text = "Export Data";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExportDataForm_FormClosing);
      this.Load += new System.EventHandler(this.ExportDataForm_Load);
      this.Controls.SetChildIndex(this.FootnoteAreaPanel, 0);
      this.Controls.SetChildIndex(this.ContentAreaPanel, 0);
      this.Controls.SetChildIndex(this.CommandAreaPanel, 0);
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.PrimaryKeyWarningPictureBox)).EndInit();
      this.ColumnOptionsGroupBox.ResumeLayout(false);
      this.ColumnOptionsGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DefaultValuePictureBox)).EndInit();
      this.DataTypeContextMenuStrip.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.ColumnOptionsWarningPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ColumnOptionsPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.TableNameWarningPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.PrimaryKeyPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.TablePictureBox)).EndInit();
      this.ExportContextMenuStrip.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.DataTypeBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.PreviewDataGridView)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Timer TextChangedTimer;
    private System.Windows.Forms.Button AdvancedOptionsButton;
    private System.Windows.Forms.Button ExportButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.Label PrimaryKeyWarningLabel;
    private System.Windows.Forms.PictureBox PrimaryKeyWarningPictureBox;
    private System.Windows.Forms.GroupBox ColumnOptionsGroupBox;
    private System.Windows.Forms.ComboBox DataTypeComboBox;
    private System.Windows.Forms.CheckBox ExcludeColumnCheckBox;
    private System.Windows.Forms.CheckBox AllowEmptyCheckBox;
    private System.Windows.Forms.CheckBox PrimaryKeyCheckBox;
    private System.Windows.Forms.CheckBox UniqueIndexCheckBox;
    private System.Windows.Forms.CheckBox CreateIndexCheckBox;
    private System.Windows.Forms.Label DatatypeLabel;
    private System.Windows.Forms.TextBox ColumnNameTextBox;
    private System.Windows.Forms.Label ColumnNameLabel;
    private System.Windows.Forms.Label ColumnOptionsWarningLabel;
    private System.Windows.Forms.PictureBox ColumnOptionsWarningPictureBox;
    private System.Windows.Forms.CheckBox FirstRowHeadersCheckBox;
    private PreviewDataGridView PreviewDataGridView;
    private System.Windows.Forms.Label ColumnOptionsSubLabel;
    private System.Windows.Forms.Label ColumnOptionsMainLabel;
    private System.Windows.Forms.PictureBox ColumnOptionsPictureBox;
    private System.Windows.Forms.Label TableNameWarningLabel;
    private System.Windows.Forms.PictureBox TableNameWarningPictureBox;
    private System.Windows.Forms.ComboBox PrimaryKeyColumnsComboBox;
    private System.Windows.Forms.RadioButton UseExistingColumnRadioButton;
    private System.Windows.Forms.TextBox AddPrimaryKeyTextBox;
    private System.Windows.Forms.RadioButton AddPrimaryKeyRadioButton;
    private System.Windows.Forms.TextBox TableNameInputTextBox;
    private System.Windows.Forms.Label TableNameInputLabel;
    private System.Windows.Forms.Label PrimaryKeySubLabel;
    private System.Windows.Forms.Label PrimaryKeyMainLabel;
    private System.Windows.Forms.PictureBox PrimaryKeyPictureBox;
    private System.Windows.Forms.Label TableNameSubLabel;
    private System.Windows.Forms.Label TableNameMainLabel;
    private System.Windows.Forms.PictureBox TablePictureBox;
    private System.Windows.Forms.Label ExportDataLabel;
    private System.Windows.Forms.Label SubSetOfDataLabel;
    private System.Windows.Forms.Button DropDownButton;
    private System.Windows.Forms.ContextMenuStrip ExportContextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem ExportDataToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem CreateTableToolStripMenuItem;
    private System.Windows.Forms.ComboBox CollationComboBox;
    private System.Windows.Forms.Label CollationLabel;
    private System.Windows.Forms.BindingSource DataTypeBindingSource;
    private System.Windows.Forms.LinkLabel MoreInfoLinkLabel;
    private System.Windows.Forms.ContextMenuStrip DataTypeContextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem UnsignedToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem ZeroFillToolStripMenuItem;
    private System.Windows.Forms.TextBox DefaultValueTextBox;
    private System.Windows.Forms.Label DefaultValueLabel;
    private System.Windows.Forms.ToolTip HelpToolTip;
    private System.Windows.Forms.CheckBox AutoIncrementCheckBox;
    private System.Windows.Forms.PictureBox DefaultValuePictureBox;
  }
}