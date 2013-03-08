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

namespace MySQL.ForExcel
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
      this.TextChangedTimer = new System.Windows.Forms.Timer(this.components);
      this.CopySQLButton = new System.Windows.Forms.Button();
      this.AdvancedOptionsButton = new System.Windows.Forms.Button();
      this.ExportButton = new System.Windows.Forms.Button();
      this.CancelButton = new System.Windows.Forms.Button();
      this.PrimaryKeyWarningLabel = new System.Windows.Forms.Label();
      this.PrimaryKeyWarningPicture = new System.Windows.Forms.PictureBox();
      this.ColumnOptionsGroupBox = new System.Windows.Forms.GroupBox();
      this.DataTypeComboBox = new System.Windows.Forms.ComboBox();
      this.ExcludeColumnCheckBox = new System.Windows.Forms.CheckBox();
      this.AllowEmptyCheckBox = new System.Windows.Forms.CheckBox();
      this.PrimaryKeyCheckBox = new System.Windows.Forms.CheckBox();
      this.UniqueIndexCheckBox = new System.Windows.Forms.CheckBox();
      this.CreateIndexCheckBox = new System.Windows.Forms.CheckBox();
      this.DatatypeLabel = new System.Windows.Forms.Label();
      this.ColumnNameTextBox = new System.Windows.Forms.TextBox();
      this.ColumnNameLabel = new System.Windows.Forms.Label();
      this.ColumnOptionsWarningLabel = new System.Windows.Forms.Label();
      this.ColumnOptionsWarningPicture = new System.Windows.Forms.PictureBox();
      this.FirstRowHeadersCheckBox = new System.Windows.Forms.CheckBox();
      this.PreviewDataGrid = new MySQL.ForExcel.PreviewDataGridView();
      this.ColumnOptionsSubLabel = new System.Windows.Forms.Label();
      this.ColumnOptionsMainLabel = new System.Windows.Forms.Label();
      this.ColumnOptionsPicture = new System.Windows.Forms.PictureBox();
      this.TableNameWarningLabel = new System.Windows.Forms.Label();
      this.TableNameWarningPicture = new System.Windows.Forms.PictureBox();
      this.PrimaryKeyColumnsComboBox = new System.Windows.Forms.ComboBox();
      this.UseExistingColumnRadioButton = new System.Windows.Forms.RadioButton();
      this.AddPrimaryKeyTextBox = new System.Windows.Forms.TextBox();
      this.AddPrimaryKeyRadioButton = new System.Windows.Forms.RadioButton();
      this.TableNameInputTextBox = new System.Windows.Forms.TextBox();
      this.TableNameInputLabel = new System.Windows.Forms.Label();
      this.PrimaryKeySubLabel = new System.Windows.Forms.Label();
      this.PrimaryKeyMainLabel = new System.Windows.Forms.Label();
      this.PrimaryKeyPicture = new System.Windows.Forms.PictureBox();
      this.TableNameSubLabel = new System.Windows.Forms.Label();
      this.TableNameMainLabel = new System.Windows.Forms.Label();
      this.TablePicture = new System.Windows.Forms.PictureBox();
      this.ExportDataLabel = new System.Windows.Forms.Label();
      this.SubSetOfDataLabel = new System.Windows.Forms.Label();
      this.contentAreaPanel.SuspendLayout();
      this.commandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.PrimaryKeyWarningPicture)).BeginInit();
      this.ColumnOptionsGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ColumnOptionsWarningPicture)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.PreviewDataGrid)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ColumnOptionsPicture)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.TableNameWarningPicture)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.PrimaryKeyPicture)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.TablePicture)).BeginInit();
      this.SuspendLayout();
      // 
      // contentAreaPanel
      // 
      this.contentAreaPanel.BackColor = System.Drawing.SystemColors.Window;
      this.contentAreaPanel.Controls.Add(this.SubSetOfDataLabel);
      this.contentAreaPanel.Controls.Add(this.ColumnOptionsWarningLabel);
      this.contentAreaPanel.Controls.Add(this.ColumnOptionsWarningPicture);
      this.contentAreaPanel.Controls.Add(this.ExportDataLabel);
      this.contentAreaPanel.Controls.Add(this.PrimaryKeyWarningLabel);
      this.contentAreaPanel.Controls.Add(this.PrimaryKeyWarningPicture);
      this.contentAreaPanel.Controls.Add(this.ColumnOptionsGroupBox);
      this.contentAreaPanel.Controls.Add(this.FirstRowHeadersCheckBox);
      this.contentAreaPanel.Controls.Add(this.PreviewDataGrid);
      this.contentAreaPanel.Controls.Add(this.ColumnOptionsSubLabel);
      this.contentAreaPanel.Controls.Add(this.ColumnOptionsMainLabel);
      this.contentAreaPanel.Controls.Add(this.ColumnOptionsPicture);
      this.contentAreaPanel.Controls.Add(this.TableNameWarningLabel);
      this.contentAreaPanel.Controls.Add(this.TableNameWarningPicture);
      this.contentAreaPanel.Controls.Add(this.PrimaryKeyColumnsComboBox);
      this.contentAreaPanel.Controls.Add(this.UseExistingColumnRadioButton);
      this.contentAreaPanel.Controls.Add(this.AddPrimaryKeyTextBox);
      this.contentAreaPanel.Controls.Add(this.AddPrimaryKeyRadioButton);
      this.contentAreaPanel.Controls.Add(this.TableNameInputTextBox);
      this.contentAreaPanel.Controls.Add(this.TableNameInputLabel);
      this.contentAreaPanel.Controls.Add(this.PrimaryKeySubLabel);
      this.contentAreaPanel.Controls.Add(this.PrimaryKeyMainLabel);
      this.contentAreaPanel.Controls.Add(this.PrimaryKeyPicture);
      this.contentAreaPanel.Controls.Add(this.TableNameSubLabel);
      this.contentAreaPanel.Controls.Add(this.TableNameMainLabel);
      this.contentAreaPanel.Controls.Add(this.TablePicture);
      this.contentAreaPanel.Size = new System.Drawing.Size(844, 555);
      // 
      // commandAreaPanel
      // 
      this.commandAreaPanel.Controls.Add(this.CopySQLButton);
      this.commandAreaPanel.Controls.Add(this.AdvancedOptionsButton);
      this.commandAreaPanel.Controls.Add(this.ExportButton);
      this.commandAreaPanel.Controls.Add(this.CancelButton);
      this.commandAreaPanel.Location = new System.Drawing.Point(0, 555);
      this.commandAreaPanel.Size = new System.Drawing.Size(844, 45);
      // 
      // TextChangedTimer
      // 
      this.TextChangedTimer.Interval = 800;
      this.TextChangedTimer.Tick += new System.EventHandler(this.TextChangedTimerTick);
      // 
      // CopySQLButton
      // 
      this.CopySQLButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.CopySQLButton.Location = new System.Drawing.Point(595, 12);
      this.CopySQLButton.Name = "CopySQLButton";
      this.CopySQLButton.Size = new System.Drawing.Size(75, 23);
      this.CopySQLButton.TabIndex = 1;
      this.CopySQLButton.Text = "Copy SQL";
      this.CopySQLButton.UseVisualStyleBackColor = true;
      this.CopySQLButton.Visible = false;
      this.CopySQLButton.Click += new System.EventHandler(this.CopySQLButton_Click);
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
      // ExportButton
      // 
      this.ExportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.ExportButton.Enabled = false;
      this.ExportButton.Location = new System.Drawing.Point(676, 12);
      this.ExportButton.Name = "ExportButton";
      this.ExportButton.Size = new System.Drawing.Size(75, 23);
      this.ExportButton.TabIndex = 2;
      this.ExportButton.Text = "Export";
      this.ExportButton.UseVisualStyleBackColor = true;
      this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
      // 
      // CancelButton
      // 
      this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.CancelButton.Location = new System.Drawing.Point(757, 12);
      this.CancelButton.Name = "CancelButton";
      this.CancelButton.Size = new System.Drawing.Size(75, 23);
      this.CancelButton.TabIndex = 3;
      this.CancelButton.Text = "Cancel";
      this.CancelButton.UseVisualStyleBackColor = true;
      // 
      // PrimaryKeyWarningLabel
      // 
      this.PrimaryKeyWarningLabel.AutoSize = true;
      this.PrimaryKeyWarningLabel.BackColor = System.Drawing.Color.Transparent;
      this.PrimaryKeyWarningLabel.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PrimaryKeyWarningLabel.ForeColor = System.Drawing.Color.Red;
      this.PrimaryKeyWarningLabel.Location = new System.Drawing.Point(485, 171);
      this.PrimaryKeyWarningLabel.Name = "PrimaryKeyWarningLabel";
      this.PrimaryKeyWarningLabel.Size = new System.Drawing.Size(336, 12);
      this.PrimaryKeyWarningLabel.TabIndex = 13;
      this.PrimaryKeyWarningLabel.Text = "Primary Key column cannot be created because another column has the same name.";
      this.PrimaryKeyWarningLabel.Visible = false;
      // 
      // PrimaryKeyWarningPicture
      // 
      this.PrimaryKeyWarningPicture.BackColor = System.Drawing.Color.Transparent;
      this.PrimaryKeyWarningPicture.Image = global::MySQL.ForExcel.Properties.Resources.Warning;
      this.PrimaryKeyWarningPicture.Location = new System.Drawing.Point(462, 166);
      this.PrimaryKeyWarningPicture.Name = "PrimaryKeyWarningPicture";
      this.PrimaryKeyWarningPicture.Size = new System.Drawing.Size(20, 20);
      this.PrimaryKeyWarningPicture.TabIndex = 45;
      this.PrimaryKeyWarningPicture.TabStop = false;
      this.PrimaryKeyWarningPicture.Visible = false;
      // 
      // ColumnOptionsGroupBox
      // 
      this.ColumnOptionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ColumnOptionsGroupBox.BackColor = System.Drawing.Color.Transparent;
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
      this.ColumnOptionsGroupBox.Size = new System.Drawing.Size(677, 89);
      this.ColumnOptionsGroupBox.TabIndex = 19;
      this.ColumnOptionsGroupBox.TabStop = false;
      this.ColumnOptionsGroupBox.Text = "Column Options";
      // 
      // DataTypeComboBox
      // 
      this.DataTypeComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.DataTypeComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.DataTypeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.DataTypeComboBox.DropDownWidth = 400;
      this.DataTypeComboBox.FormattingEnabled = true;
      this.DataTypeComboBox.Location = new System.Drawing.Point(122, 51);
      this.DataTypeComboBox.Name = "DataTypeComboBox";
      this.DataTypeComboBox.Size = new System.Drawing.Size(135, 24);
      this.DataTypeComboBox.TabIndex = 4;
      this.DataTypeComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.DataTypeComboBoxDrawItem);
      this.DataTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.DataTypeComboBox_SelectedIndexChanged);
      this.DataTypeComboBox.TextChanged += new System.EventHandler(this.DataTypeComboBox_TextChanged);
      this.DataTypeComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.DataTypeComboBox_Validating);
      // 
      // ExcludeColumnCheckBox
      // 
      this.ExcludeColumnCheckBox.AutoSize = true;
      this.ExcludeColumnCheckBox.Location = new System.Drawing.Point(529, 21);
      this.ExcludeColumnCheckBox.Name = "ExcludeColumnCheckBox";
      this.ExcludeColumnCheckBox.Size = new System.Drawing.Size(112, 19);
      this.ExcludeColumnCheckBox.TabIndex = 9;
      this.ExcludeColumnCheckBox.Text = "Exclude Column";
      this.ExcludeColumnCheckBox.UseVisualStyleBackColor = true;
      this.ExcludeColumnCheckBox.CheckedChanged += new System.EventHandler(this.ExcludeCheckBox_CheckedChanged);
      // 
      // AllowEmptyCheckBox
      // 
      this.AllowEmptyCheckBox.AutoSize = true;
      this.AllowEmptyCheckBox.Location = new System.Drawing.Point(407, 53);
      this.AllowEmptyCheckBox.Name = "AllowEmptyCheckBox";
      this.AllowEmptyCheckBox.Size = new System.Drawing.Size(93, 19);
      this.AllowEmptyCheckBox.TabIndex = 8;
      this.AllowEmptyCheckBox.Text = "Allow Empty";
      this.AllowEmptyCheckBox.UseVisualStyleBackColor = true;
      this.AllowEmptyCheckBox.CheckedChanged += new System.EventHandler(this.AllowEmptyCheckBox_CheckedChanged);
      // 
      // PrimaryKeyCheckBox
      // 
      this.PrimaryKeyCheckBox.AutoSize = true;
      this.PrimaryKeyCheckBox.Location = new System.Drawing.Point(407, 21);
      this.PrimaryKeyCheckBox.Name = "PrimaryKeyCheckBox";
      this.PrimaryKeyCheckBox.Size = new System.Drawing.Size(89, 19);
      this.PrimaryKeyCheckBox.TabIndex = 7;
      this.PrimaryKeyCheckBox.Text = "Primary Key";
      this.PrimaryKeyCheckBox.UseVisualStyleBackColor = true;
      this.PrimaryKeyCheckBox.CheckedChanged += new System.EventHandler(this.PrimaryKeyCheckBox_CheckedChanged);
      // 
      // UniqueIndexCheckBox
      // 
      this.UniqueIndexCheckBox.AutoSize = true;
      this.UniqueIndexCheckBox.Location = new System.Drawing.Point(283, 53);
      this.UniqueIndexCheckBox.Name = "UniqueIndexCheckBox";
      this.UniqueIndexCheckBox.Size = new System.Drawing.Size(95, 19);
      this.UniqueIndexCheckBox.TabIndex = 6;
      this.UniqueIndexCheckBox.Text = "Unique Index";
      this.UniqueIndexCheckBox.UseVisualStyleBackColor = true;
      this.UniqueIndexCheckBox.CheckedChanged += new System.EventHandler(this.UniqueIndexCheckBox_CheckedChanged);
      // 
      // CreateIndexCheckBox
      // 
      this.CreateIndexCheckBox.AutoSize = true;
      this.CreateIndexCheckBox.Location = new System.Drawing.Point(283, 21);
      this.CreateIndexCheckBox.Name = "CreateIndexCheckBox";
      this.CreateIndexCheckBox.Size = new System.Drawing.Size(91, 19);
      this.CreateIndexCheckBox.TabIndex = 5;
      this.CreateIndexCheckBox.Text = "Create Index";
      this.CreateIndexCheckBox.UseVisualStyleBackColor = true;
      this.CreateIndexCheckBox.CheckedChanged += new System.EventHandler(this.CreateIndexCheckBox_CheckedChanged);
      // 
      // DatatypeLabel
      // 
      this.DatatypeLabel.AutoSize = true;
      this.DatatypeLabel.Location = new System.Drawing.Point(28, 54);
      this.DatatypeLabel.Name = "DatatypeLabel";
      this.DatatypeLabel.Size = new System.Drawing.Size(57, 15);
      this.DatatypeLabel.TabIndex = 3;
      this.DatatypeLabel.Text = "Datatype:";
      // 
      // ColumnNameTextBox
      // 
      this.ColumnNameTextBox.Location = new System.Drawing.Point(122, 22);
      this.ColumnNameTextBox.Name = "ColumnNameTextBox";
      this.ColumnNameTextBox.Size = new System.Drawing.Size(135, 23);
      this.ColumnNameTextBox.TabIndex = 2;
      this.ColumnNameTextBox.TextChanged += new System.EventHandler(this.ColumnNameTextBox_TextChanged);
      this.ColumnNameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ColumnNameTextBox_Validating);
      // 
      // ColumnNameLabel
      // 
      this.ColumnNameLabel.AutoSize = true;
      this.ColumnNameLabel.Location = new System.Drawing.Point(28, 25);
      this.ColumnNameLabel.Name = "ColumnNameLabel";
      this.ColumnNameLabel.Size = new System.Drawing.Size(88, 15);
      this.ColumnNameLabel.TabIndex = 1;
      this.ColumnNameLabel.Text = "Column Name:";
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
      this.ColumnOptionsWarningLabel.TabIndex = 0;
      this.ColumnOptionsWarningLabel.Text = "It is good practice to not use upper case letters or spaces.";
      this.ColumnOptionsWarningLabel.Visible = false;
      // 
      // ColumnOptionsWarningPicture
      // 
      this.ColumnOptionsWarningPicture.BackColor = System.Drawing.SystemColors.Window;
      this.ColumnOptionsWarningPicture.Image = global::MySQL.ForExcel.Properties.Resources.Warning;
      this.ColumnOptionsWarningPicture.Location = new System.Drawing.Point(186, 442);
      this.ColumnOptionsWarningPicture.Name = "ColumnOptionsWarningPicture";
      this.ColumnOptionsWarningPicture.Size = new System.Drawing.Size(20, 20);
      this.ColumnOptionsWarningPicture.TabIndex = 24;
      this.ColumnOptionsWarningPicture.TabStop = false;
      this.ColumnOptionsWarningPicture.Visible = false;
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
      this.FirstRowHeadersCheckBox.TabIndex = 16;
      this.FirstRowHeadersCheckBox.Text = "First Row Contains Column Names";
      this.FirstRowHeadersCheckBox.UseVisualStyleBackColor = false;
      this.FirstRowHeadersCheckBox.CheckedChanged += new System.EventHandler(this.FirstRowHeadersCheckBox_CheckedChanged);
      // 
      // PreviewDataGrid
      // 
      this.PreviewDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.PreviewDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
      this.PreviewDataGrid.ColumnsMaximumWidth = 200;
      dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.PreviewDataGrid.DefaultCellStyle = dataGridViewCellStyle14;
      this.PreviewDataGrid.Location = new System.Drawing.Point(82, 277);
      this.PreviewDataGrid.MultiSelect = false;
      this.PreviewDataGrid.Name = "PreviewDataGrid";
      dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.PreviewDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
      this.PreviewDataGrid.Size = new System.Drawing.Size(677, 157);
      this.PreviewDataGrid.TabIndex = 18;
      this.PreviewDataGrid.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.PreviewDataGrid_CellToolTipTextNeeded);
      this.PreviewDataGrid.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.PreviewDataGrid_DataBindingComplete);
      this.PreviewDataGrid.SelectionChanged += new System.EventHandler(this.PreviewDataGrid_SelectionChanged);
      this.PreviewDataGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PreviewDataGrid_KeyDown);
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
      this.ColumnOptionsSubLabel.TabIndex = 15;
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
      this.ColumnOptionsMainLabel.TabIndex = 14;
      this.ColumnOptionsMainLabel.Text = "3. Specify Column Options";
      // 
      // ColumnOptionsPicture
      // 
      this.ColumnOptionsPicture.BackColor = System.Drawing.Color.Transparent;
      this.ColumnOptionsPicture.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_ColumnOptions_32x32;
      this.ColumnOptionsPicture.Location = new System.Drawing.Point(41, 207);
      this.ColumnOptionsPicture.Name = "ColumnOptionsPicture";
      this.ColumnOptionsPicture.Size = new System.Drawing.Size(32, 32);
      this.ColumnOptionsPicture.TabIndex = 41;
      this.ColumnOptionsPicture.TabStop = false;
      // 
      // TableNameWarningLabel
      // 
      this.TableNameWarningLabel.AutoSize = true;
      this.TableNameWarningLabel.BackColor = System.Drawing.Color.Transparent;
      this.TableNameWarningLabel.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TableNameWarningLabel.ForeColor = System.Drawing.Color.Red;
      this.TableNameWarningLabel.Location = new System.Drawing.Point(150, 146);
      this.TableNameWarningLabel.Name = "TableNameWarningLabel";
      this.TableNameWarningLabel.Size = new System.Drawing.Size(227, 12);
      this.TableNameWarningLabel.TabIndex = 5;
      this.TableNameWarningLabel.Text = "It is good practice to not use upper case letters or spaces.";
      this.TableNameWarningLabel.Visible = false;
      // 
      // TableNameWarningPicture
      // 
      this.TableNameWarningPicture.BackColor = System.Drawing.Color.Transparent;
      this.TableNameWarningPicture.Image = global::MySQL.ForExcel.Properties.Resources.Warning;
      this.TableNameWarningPicture.Location = new System.Drawing.Point(127, 141);
      this.TableNameWarningPicture.Name = "TableNameWarningPicture";
      this.TableNameWarningPicture.Size = new System.Drawing.Size(20, 20);
      this.TableNameWarningPicture.TabIndex = 38;
      this.TableNameWarningPicture.TabStop = false;
      this.TableNameWarningPicture.Visible = false;
      // 
      // PrimaryKeyColumnsComboBox
      // 
      this.PrimaryKeyColumnsComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.PrimaryKeyColumnsComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.PrimaryKeyColumnsComboBox.DisplayMember = "DisplayName";
      this.PrimaryKeyColumnsComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PrimaryKeyColumnsComboBox.FormattingEnabled = true;
      this.PrimaryKeyColumnsComboBox.Location = new System.Drawing.Point(638, 144);
      this.PrimaryKeyColumnsComboBox.Name = "PrimaryKeyColumnsComboBox";
      this.PrimaryKeyColumnsComboBox.Size = new System.Drawing.Size(121, 23);
      this.PrimaryKeyColumnsComboBox.TabIndex = 12;
      this.PrimaryKeyColumnsComboBox.ValueMember = "DisplayName";
      this.PrimaryKeyColumnsComboBox.SelectedIndexChanged += new System.EventHandler(this.PrimaryKeyColumnsComboBox_SelectedIndexChanged);
      // 
      // UseExistingColumnRadioButton
      // 
      this.UseExistingColumnRadioButton.AutoSize = true;
      this.UseExistingColumnRadioButton.BackColor = System.Drawing.Color.Transparent;
      this.UseExistingColumnRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UseExistingColumnRadioButton.Location = new System.Drawing.Point(462, 144);
      this.UseExistingColumnRadioButton.Name = "UseExistingColumnRadioButton";
      this.UseExistingColumnRadioButton.Size = new System.Drawing.Size(134, 19);
      this.UseExistingColumnRadioButton.TabIndex = 11;
      this.UseExistingColumnRadioButton.TabStop = true;
      this.UseExistingColumnRadioButton.Text = "Use existing column:";
      this.UseExistingColumnRadioButton.UseVisualStyleBackColor = false;
      this.UseExistingColumnRadioButton.CheckedChanged += new System.EventHandler(this.UseExistingColumnRadioButton_CheckedChanged);
      // 
      // AddPrimaryKeyTextBox
      // 
      this.AddPrimaryKeyTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AddPrimaryKeyTextBox.Location = new System.Drawing.Point(637, 116);
      this.AddPrimaryKeyTextBox.Name = "AddPrimaryKeyTextBox";
      this.AddPrimaryKeyTextBox.Size = new System.Drawing.Size(122, 22);
      this.AddPrimaryKeyTextBox.TabIndex = 10;
      this.AddPrimaryKeyTextBox.TextChanged += new System.EventHandler(this.AddPrimaryKeyTextBox_TextChanged);
      this.AddPrimaryKeyTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.AddPrimaryKeyTextBox_Validating);
      // 
      // AddPrimaryKeyRadioButton
      // 
      this.AddPrimaryKeyRadioButton.AutoSize = true;
      this.AddPrimaryKeyRadioButton.BackColor = System.Drawing.Color.Transparent;
      this.AddPrimaryKeyRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AddPrimaryKeyRadioButton.Location = new System.Drawing.Point(462, 116);
      this.AddPrimaryKeyRadioButton.Name = "AddPrimaryKeyRadioButton";
      this.AddPrimaryKeyRadioButton.Size = new System.Drawing.Size(169, 19);
      this.AddPrimaryKeyRadioButton.TabIndex = 9;
      this.AddPrimaryKeyRadioButton.TabStop = true;
      this.AddPrimaryKeyRadioButton.Text = "Add a Primary Key column:";
      this.AddPrimaryKeyRadioButton.UseVisualStyleBackColor = false;
      this.AddPrimaryKeyRadioButton.CheckedChanged += new System.EventHandler(this.AddPrimaryKeyRadioButton_CheckedChanged);
      // 
      // TableNameInputTextBox
      // 
      this.TableNameInputTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TableNameInputTextBox.Location = new System.Drawing.Point(127, 118);
      this.TableNameInputTextBox.Name = "TableNameInputTextBox";
      this.TableNameInputTextBox.Size = new System.Drawing.Size(219, 22);
      this.TableNameInputTextBox.TabIndex = 4;
      this.TableNameInputTextBox.TextChanged += new System.EventHandler(this.TableNameInputTextBox_TextChanged);
      this.TableNameInputTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.TableNameInputTextBox_Validating);
      // 
      // TableNameInputLabel
      // 
      this.TableNameInputLabel.AutoSize = true;
      this.TableNameInputLabel.BackColor = System.Drawing.Color.Transparent;
      this.TableNameInputLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TableNameInputLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.TableNameInputLabel.Location = new System.Drawing.Point(79, 120);
      this.TableNameInputLabel.Name = "TableNameInputLabel";
      this.TableNameInputLabel.Size = new System.Drawing.Size(42, 15);
      this.TableNameInputLabel.TabIndex = 3;
      this.TableNameInputLabel.Text = "Name:";
      // 
      // PrimaryKeySubLabel
      // 
      this.PrimaryKeySubLabel.AutoSize = true;
      this.PrimaryKeySubLabel.BackColor = System.Drawing.Color.Transparent;
      this.PrimaryKeySubLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PrimaryKeySubLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.PrimaryKeySubLabel.Location = new System.Drawing.Point(459, 71);
      this.PrimaryKeySubLabel.Name = "PrimaryKeySubLabel";
      this.PrimaryKeySubLabel.Size = new System.Drawing.Size(264, 30);
      this.PrimaryKeySubLabel.TabIndex = 7;
      this.PrimaryKeySubLabel.Text = "Each row of data needs to hold a unique number\r\nthat is used as the Primary Key.";
      // 
      // PrimaryKeyMainLabel
      // 
      this.PrimaryKeyMainLabel.AutoSize = true;
      this.PrimaryKeyMainLabel.BackColor = System.Drawing.Color.Transparent;
      this.PrimaryKeyMainLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PrimaryKeyMainLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.PrimaryKeyMainLabel.Location = new System.Drawing.Point(459, 54);
      this.PrimaryKeyMainLabel.Name = "PrimaryKeyMainLabel";
      this.PrimaryKeyMainLabel.Size = new System.Drawing.Size(128, 17);
      this.PrimaryKeyMainLabel.TabIndex = 6;
      this.PrimaryKeyMainLabel.Text = "2. Pick a Primary Key";
      // 
      // PrimaryKeyPicture
      // 
      this.PrimaryKeyPicture.BackColor = System.Drawing.Color.Transparent;
      this.PrimaryKeyPicture.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_PrimaryKey_32x32;
      this.PrimaryKeyPicture.Location = new System.Drawing.Point(421, 57);
      this.PrimaryKeyPicture.Name = "PrimaryKeyPicture";
      this.PrimaryKeyPicture.Size = new System.Drawing.Size(32, 32);
      this.PrimaryKeyPicture.TabIndex = 28;
      this.PrimaryKeyPicture.TabStop = false;
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
      this.TableNameSubLabel.TabIndex = 1;
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
      this.TableNameMainLabel.TabIndex = 0;
      this.TableNameMainLabel.Text = "1. Set a Table Name";
      // 
      // TablePicture
      // 
      this.TablePicture.BackColor = System.Drawing.Color.Transparent;
      this.TablePicture.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_TableName_32x32;
      this.TablePicture.Location = new System.Drawing.Point(41, 57);
      this.TablePicture.Name = "TablePicture";
      this.TablePicture.Size = new System.Drawing.Size(32, 32);
      this.TablePicture.TabIndex = 23;
      this.TablePicture.TabStop = false;
      // 
      // ExportDataLabel
      // 
      this.ExportDataLabel.AutoSize = true;
      this.ExportDataLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExportDataLabel.ForeColor = System.Drawing.Color.Navy;
      this.ExportDataLabel.Location = new System.Drawing.Point(17, 17);
      this.ExportDataLabel.Name = "ExportDataLabel";
      this.ExportDataLabel.Size = new System.Drawing.Size(156, 20);
      this.ExportDataLabel.TabIndex = 46;
      this.ExportDataLabel.Text = "Export Data to MySQL";
      // 
      // SubSetOfDataLabel
      // 
      this.SubSetOfDataLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.SubSetOfDataLabel.AutoSize = true;
      this.SubSetOfDataLabel.BackColor = System.Drawing.Color.Transparent;
      this.SubSetOfDataLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SubSetOfDataLabel.ForeColor = System.Drawing.SystemColors.InactiveCaption;
      this.SubSetOfDataLabel.Location = new System.Drawing.Point(440, 255);
      this.SubSetOfDataLabel.Name = "SubSetOfDataLabel";
      this.SubSetOfDataLabel.Size = new System.Drawing.Size(319, 15);
      this.SubSetOfDataLabel.TabIndex = 17;
      this.SubSetOfDataLabel.Text = "This is a small subset of the data for preview purposes only.";
      // 
      // ExportDataForm
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.ClientSize = new System.Drawing.Size(844, 602);
      this.CommandAreaHeight = 45;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MainInstructionLocation = new System.Drawing.Point(10, 15);
      this.MinimumSize = new System.Drawing.Size(860, 640);
      this.Name = "ExportDataForm";
      this.Text = "Export Data";
      this.Load += new System.EventHandler(this.ExportDataForm_Load);
      this.contentAreaPanel.ResumeLayout(false);
      this.contentAreaPanel.PerformLayout();
      this.commandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.PrimaryKeyWarningPicture)).EndInit();
      this.ColumnOptionsGroupBox.ResumeLayout(false);
      this.ColumnOptionsGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ColumnOptionsWarningPicture)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.PreviewDataGrid)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ColumnOptionsPicture)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.TableNameWarningPicture)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.PrimaryKeyPicture)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.TablePicture)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Timer TextChangedTimer;
    private System.Windows.Forms.Button CopySQLButton;
    private System.Windows.Forms.Button AdvancedOptionsButton;
    private System.Windows.Forms.Button ExportButton;
    private System.Windows.Forms.Button CancelButton;
    private System.Windows.Forms.Label PrimaryKeyWarningLabel;
    private System.Windows.Forms.PictureBox PrimaryKeyWarningPicture;
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
    private System.Windows.Forms.PictureBox ColumnOptionsWarningPicture;
    private System.Windows.Forms.CheckBox FirstRowHeadersCheckBox;
    private PreviewDataGridView PreviewDataGrid;
    private System.Windows.Forms.Label ColumnOptionsSubLabel;
    private System.Windows.Forms.Label ColumnOptionsMainLabel;
    private System.Windows.Forms.PictureBox ColumnOptionsPicture;
    private System.Windows.Forms.Label TableNameWarningLabel;
    private System.Windows.Forms.PictureBox TableNameWarningPicture;
    private System.Windows.Forms.ComboBox PrimaryKeyColumnsComboBox;
    private System.Windows.Forms.RadioButton UseExistingColumnRadioButton;
    private System.Windows.Forms.TextBox AddPrimaryKeyTextBox;
    private System.Windows.Forms.RadioButton AddPrimaryKeyRadioButton;
    private System.Windows.Forms.TextBox TableNameInputTextBox;
    private System.Windows.Forms.Label TableNameInputLabel;
    private System.Windows.Forms.Label PrimaryKeySubLabel;
    private System.Windows.Forms.Label PrimaryKeyMainLabel;
    private System.Windows.Forms.PictureBox PrimaryKeyPicture;
    private System.Windows.Forms.Label TableNameSubLabel;
    private System.Windows.Forms.Label TableNameMainLabel;
    private System.Windows.Forms.PictureBox TablePicture;
    private System.Windows.Forms.Label ExportDataLabel;
    private System.Windows.Forms.Label SubSetOfDataLabel;
  }
}