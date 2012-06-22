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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
      this.ExportDataPanel = new System.Windows.Forms.Panel();
      this.lblPrimaryKeyWarning = new System.Windows.Forms.Label();
      this.picPrimaryKeyWarning = new System.Windows.Forms.PictureBox();
      this.grpColumnOptions = new System.Windows.Forms.GroupBox();
      this.cmbDatatype = new System.Windows.Forms.ComboBox();
      this.chkExcludeColumn = new System.Windows.Forms.CheckBox();
      this.chkAllowEmpty = new System.Windows.Forms.CheckBox();
      this.chkPrimaryKey = new System.Windows.Forms.CheckBox();
      this.chkUniqueIndex = new System.Windows.Forms.CheckBox();
      this.chkCreateIndex = new System.Windows.Forms.CheckBox();
      this.lblDatatype = new System.Windows.Forms.Label();
      this.txtColumnName = new System.Windows.Forms.TextBox();
      this.lblColumnName = new System.Windows.Forms.Label();
      this.chkFirstRowHeaders = new System.Windows.Forms.CheckBox();
      this.grdPreviewData = new System.Windows.Forms.DataGridView();
      this.lblColumnOptionsSub = new System.Windows.Forms.Label();
      this.lblColumnOptionsMain = new System.Windows.Forms.Label();
      this.picColumnOptions = new System.Windows.Forms.PictureBox();
      this.lblTableNameWarning = new System.Windows.Forms.Label();
      this.picTableNameWarning = new System.Windows.Forms.PictureBox();
      this.cmbPrimaryKeyColumns = new System.Windows.Forms.ComboBox();
      this.radUseExistingColumn = new System.Windows.Forms.RadioButton();
      this.txtAddPrimaryKey = new System.Windows.Forms.TextBox();
      this.radAddPrimaryKey = new System.Windows.Forms.RadioButton();
      this.txtTableNameInput = new System.Windows.Forms.TextBox();
      this.lblTableNameInput = new System.Windows.Forms.Label();
      this.lblPrimaryKeySub2 = new System.Windows.Forms.Label();
      this.lblPrimaryKeySub1 = new System.Windows.Forms.Label();
      this.lblPrimaryKeyMain = new System.Windows.Forms.Label();
      this.picPrimaryKey = new System.Windows.Forms.PictureBox();
      this.lblTableNameSub2 = new System.Windows.Forms.Label();
      this.lblTableNameSub1 = new System.Windows.Forms.Label();
      this.lblTableNameMain = new System.Windows.Forms.Label();
      this.picTable = new System.Windows.Forms.PictureBox();
      this.lblExportData = new System.Windows.Forms.Label();
      this.lblColumnOptionsWarning = new System.Windows.Forms.Label();
      this.picColumnOptionsWarning = new System.Windows.Forms.PictureBox();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnExport = new System.Windows.Forms.Button();
      this.btnAdvanced = new System.Windows.Forms.Button();
      this.btnCopySQL = new System.Windows.Forms.Button();
      this.timerTextChanged = new System.Windows.Forms.Timer(this.components);
      this.gridToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.columnBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.ExportDataPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picPrimaryKeyWarning)).BeginInit();
      this.grpColumnOptions.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColumnOptions)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picTableNameWarning)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPrimaryKey)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picTable)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColumnOptionsWarning)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.columnBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // ExportDataPanel
      // 
      this.ExportDataPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ExportDataPanel.BackColor = System.Drawing.SystemColors.Window;
      this.ExportDataPanel.Controls.Add(this.lblPrimaryKeyWarning);
      this.ExportDataPanel.Controls.Add(this.picPrimaryKeyWarning);
      this.ExportDataPanel.Controls.Add(this.grpColumnOptions);
      this.ExportDataPanel.Controls.Add(this.chkFirstRowHeaders);
      this.ExportDataPanel.Controls.Add(this.grdPreviewData);
      this.ExportDataPanel.Controls.Add(this.lblColumnOptionsSub);
      this.ExportDataPanel.Controls.Add(this.lblColumnOptionsMain);
      this.ExportDataPanel.Controls.Add(this.picColumnOptions);
      this.ExportDataPanel.Controls.Add(this.lblTableNameWarning);
      this.ExportDataPanel.Controls.Add(this.picTableNameWarning);
      this.ExportDataPanel.Controls.Add(this.cmbPrimaryKeyColumns);
      this.ExportDataPanel.Controls.Add(this.radUseExistingColumn);
      this.ExportDataPanel.Controls.Add(this.txtAddPrimaryKey);
      this.ExportDataPanel.Controls.Add(this.radAddPrimaryKey);
      this.ExportDataPanel.Controls.Add(this.txtTableNameInput);
      this.ExportDataPanel.Controls.Add(this.lblTableNameInput);
      this.ExportDataPanel.Controls.Add(this.lblPrimaryKeySub2);
      this.ExportDataPanel.Controls.Add(this.lblPrimaryKeySub1);
      this.ExportDataPanel.Controls.Add(this.lblPrimaryKeyMain);
      this.ExportDataPanel.Controls.Add(this.picPrimaryKey);
      this.ExportDataPanel.Controls.Add(this.lblTableNameSub2);
      this.ExportDataPanel.Controls.Add(this.lblTableNameSub1);
      this.ExportDataPanel.Controls.Add(this.lblTableNameMain);
      this.ExportDataPanel.Controls.Add(this.picTable);
      this.ExportDataPanel.Controls.Add(this.lblExportData);
      this.ExportDataPanel.Location = new System.Drawing.Point(-1, -2);
      this.ExportDataPanel.Name = "ExportDataPanel";
      this.ExportDataPanel.Size = new System.Drawing.Size(846, 559);
      this.ExportDataPanel.TabIndex = 0;
      // 
      // lblPrimaryKeyWarning
      // 
      this.lblPrimaryKeyWarning.AutoSize = true;
      this.lblPrimaryKeyWarning.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPrimaryKeyWarning.ForeColor = System.Drawing.Color.Red;
      this.lblPrimaryKeyWarning.Location = new System.Drawing.Point(486, 184);
      this.lblPrimaryKeyWarning.Name = "lblPrimaryKeyWarning";
      this.lblPrimaryKeyWarning.Size = new System.Drawing.Size(336, 12);
      this.lblPrimaryKeyWarning.TabIndex = 20;
      this.lblPrimaryKeyWarning.Text = "Primary Key column cannot be created because another column has the same name.";
      this.lblPrimaryKeyWarning.Visible = false;
      // 
      // picPrimaryKeyWarning
      // 
      this.picPrimaryKeyWarning.Image = global::MySQL.ForExcel.Properties.Resources.Warning;
      this.picPrimaryKeyWarning.Location = new System.Drawing.Point(463, 179);
      this.picPrimaryKeyWarning.Name = "picPrimaryKeyWarning";
      this.picPrimaryKeyWarning.Size = new System.Drawing.Size(20, 20);
      this.picPrimaryKeyWarning.TabIndex = 21;
      this.picPrimaryKeyWarning.TabStop = false;
      this.picPrimaryKeyWarning.Visible = false;
      // 
      // grpColumnOptions
      // 
      this.grpColumnOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grpColumnOptions.Controls.Add(this.cmbDatatype);
      this.grpColumnOptions.Controls.Add(this.chkExcludeColumn);
      this.grpColumnOptions.Controls.Add(this.chkAllowEmpty);
      this.grpColumnOptions.Controls.Add(this.chkPrimaryKey);
      this.grpColumnOptions.Controls.Add(this.chkUniqueIndex);
      this.grpColumnOptions.Controls.Add(this.chkCreateIndex);
      this.grpColumnOptions.Controls.Add(this.lblDatatype);
      this.grpColumnOptions.Controls.Add(this.txtColumnName);
      this.grpColumnOptions.Controls.Add(this.lblColumnName);
      this.grpColumnOptions.Controls.Add(this.lblColumnOptionsWarning);
      this.grpColumnOptions.Controls.Add(this.picColumnOptionsWarning);
      this.grpColumnOptions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.grpColumnOptions.Location = new System.Drawing.Point(83, 447);
      this.grpColumnOptions.Name = "grpColumnOptions";
      this.grpColumnOptions.Size = new System.Drawing.Size(677, 100);
      this.grpColumnOptions.TabIndex = 19;
      this.grpColumnOptions.TabStop = false;
      this.grpColumnOptions.Text = "Column Options";
      // 
      // cmbDatatype
      // 
      this.cmbDatatype.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cmbDatatype.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.columnBindingSource, "MySQLDataType", true));
      this.cmbDatatype.FormattingEnabled = true;
      this.cmbDatatype.Location = new System.Drawing.Point(122, 62);
      this.cmbDatatype.Name = "cmbDatatype";
      this.cmbDatatype.Size = new System.Drawing.Size(135, 23);
      this.cmbDatatype.TabIndex = 4;
      this.cmbDatatype.DropDown += new System.EventHandler(this.cmbDatatype_DropDown);
      this.cmbDatatype.SelectedIndexChanged += new System.EventHandler(this.cmbDatatype_SelectedIndexChanged);
      this.cmbDatatype.DropDownClosed += new System.EventHandler(this.cmbDatatype_DropDownClosed);
      this.cmbDatatype.Validating += new System.ComponentModel.CancelEventHandler(this.cmbDatatype_Validating);
      // 
      // chkExcludeColumn
      // 
      this.chkExcludeColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.chkExcludeColumn.AutoSize = true;
      this.chkExcludeColumn.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.columnBindingSource, "ExcludeColumn", true));
      this.chkExcludeColumn.Location = new System.Drawing.Point(529, 32);
      this.chkExcludeColumn.Name = "chkExcludeColumn";
      this.chkExcludeColumn.Size = new System.Drawing.Size(112, 19);
      this.chkExcludeColumn.TabIndex = 9;
      this.chkExcludeColumn.Text = "Exclude Column";
      this.chkExcludeColumn.UseVisualStyleBackColor = true;
      this.chkExcludeColumn.CheckedChanged += new System.EventHandler(this.chkExcludeColumn_CheckedChanged);
      this.chkExcludeColumn.Validated += new System.EventHandler(this.chkExcludeColumn_Validated);
      // 
      // chkAllowEmpty
      // 
      this.chkAllowEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.chkAllowEmpty.AutoSize = true;
      this.chkAllowEmpty.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.columnBindingSource, "AllowNull", true));
      this.chkAllowEmpty.Location = new System.Drawing.Point(407, 64);
      this.chkAllowEmpty.Name = "chkAllowEmpty";
      this.chkAllowEmpty.Size = new System.Drawing.Size(93, 19);
      this.chkAllowEmpty.TabIndex = 8;
      this.chkAllowEmpty.Text = "Allow Empty";
      this.chkAllowEmpty.UseVisualStyleBackColor = true;
      // 
      // chkPrimaryKey
      // 
      this.chkPrimaryKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.chkPrimaryKey.AutoSize = true;
      this.chkPrimaryKey.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.columnBindingSource, "PrimaryKey", true));
      this.chkPrimaryKey.Location = new System.Drawing.Point(407, 32);
      this.chkPrimaryKey.Name = "chkPrimaryKey";
      this.chkPrimaryKey.Size = new System.Drawing.Size(89, 19);
      this.chkPrimaryKey.TabIndex = 7;
      this.chkPrimaryKey.Text = "Primary Key";
      this.chkPrimaryKey.UseVisualStyleBackColor = true;
      this.chkPrimaryKey.CheckedChanged += new System.EventHandler(this.chkPrimaryKey_CheckedChanged);
      this.chkPrimaryKey.Validated += new System.EventHandler(this.chkPrimaryKey_Validated);
      // 
      // chkUniqueIndex
      // 
      this.chkUniqueIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.chkUniqueIndex.AutoSize = true;
      this.chkUniqueIndex.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.columnBindingSource, "UniqueKey", true));
      this.chkUniqueIndex.Location = new System.Drawing.Point(283, 64);
      this.chkUniqueIndex.Name = "chkUniqueIndex";
      this.chkUniqueIndex.Size = new System.Drawing.Size(95, 19);
      this.chkUniqueIndex.TabIndex = 6;
      this.chkUniqueIndex.Text = "Unique Index";
      this.chkUniqueIndex.UseVisualStyleBackColor = true;
      this.chkUniqueIndex.CheckedChanged += new System.EventHandler(this.chkUniqueIndex_CheckedChanged);
      // 
      // chkCreateIndex
      // 
      this.chkCreateIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.chkCreateIndex.AutoSize = true;
      this.chkCreateIndex.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.columnBindingSource, "CreateIndex", true));
      this.chkCreateIndex.Location = new System.Drawing.Point(283, 32);
      this.chkCreateIndex.Name = "chkCreateIndex";
      this.chkCreateIndex.Size = new System.Drawing.Size(91, 19);
      this.chkCreateIndex.TabIndex = 5;
      this.chkCreateIndex.Text = "Create Index";
      this.chkCreateIndex.UseVisualStyleBackColor = true;
      this.chkCreateIndex.CheckedChanged += new System.EventHandler(this.chkCreateIndex_CheckedChanged);
      // 
      // lblDatatype
      // 
      this.lblDatatype.AutoSize = true;
      this.lblDatatype.Location = new System.Drawing.Point(28, 65);
      this.lblDatatype.Name = "lblDatatype";
      this.lblDatatype.Size = new System.Drawing.Size(57, 15);
      this.lblDatatype.TabIndex = 3;
      this.lblDatatype.Text = "Datatype:";
      // 
      // txtColumnName
      // 
      this.txtColumnName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtColumnName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.columnBindingSource, "DisplayName", true));
      this.txtColumnName.Location = new System.Drawing.Point(122, 33);
      this.txtColumnName.Name = "txtColumnName";
      this.txtColumnName.Size = new System.Drawing.Size(135, 23);
      this.txtColumnName.TabIndex = 2;
      this.txtColumnName.TextChanged += new System.EventHandler(this.txtColumnName_TextChanged);
      this.txtColumnName.Validated += new System.EventHandler(this.txtColumnName_Validated);
      // 
      // lblColumnName
      // 
      this.lblColumnName.AutoSize = true;
      this.lblColumnName.Location = new System.Drawing.Point(28, 36);
      this.lblColumnName.Name = "lblColumnName";
      this.lblColumnName.Size = new System.Drawing.Size(88, 15);
      this.lblColumnName.TabIndex = 1;
      this.lblColumnName.Text = "Column Name:";
      // 
      // chkFirstRowHeaders
      // 
      this.chkFirstRowHeaders.AutoSize = true;
      this.chkFirstRowHeaders.Checked = true;
      this.chkFirstRowHeaders.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkFirstRowHeaders.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkFirstRowHeaders.Location = new System.Drawing.Point(83, 256);
      this.chkFirstRowHeaders.Name = "chkFirstRowHeaders";
      this.chkFirstRowHeaders.Size = new System.Drawing.Size(210, 19);
      this.chkFirstRowHeaders.TabIndex = 17;
      this.chkFirstRowHeaders.Text = "First Row Contains Column Names";
      this.chkFirstRowHeaders.UseVisualStyleBackColor = true;
      this.chkFirstRowHeaders.CheckedChanged += new System.EventHandler(this.chkFirstRowHeaders_CheckedChanged);
      // 
      // grdPreviewData
      // 
      this.grdPreviewData.AllowUserToAddRows = false;
      this.grdPreviewData.AllowUserToDeleteRows = false;
      this.grdPreviewData.AllowUserToResizeColumns = false;
      this.grdPreviewData.AllowUserToResizeRows = false;
      this.grdPreviewData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grdPreviewData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.grdPreviewData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
      this.grdPreviewData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdPreviewData.DefaultCellStyle = dataGridViewCellStyle11;
      this.grdPreviewData.Location = new System.Drawing.Point(83, 279);
      this.grdPreviewData.MultiSelect = false;
      this.grdPreviewData.Name = "grdPreviewData";
      this.grdPreviewData.ReadOnly = true;
      dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.grdPreviewData.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
      this.grdPreviewData.RowHeadersVisible = false;
      this.grdPreviewData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.grdPreviewData.Size = new System.Drawing.Size(677, 158);
      this.grdPreviewData.TabIndex = 18;
      this.gridToolTip.SetToolTip(this.grdPreviewData, "Mama");
      this.grdPreviewData.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grdPreviewData_DataBindingComplete);
      this.grdPreviewData.SelectionChanged += new System.EventHandler(this.grdPreviewData_SelectionChanged);
      this.grdPreviewData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdPreviewData_KeyDown);
      // 
      // lblColumnOptionsSub
      // 
      this.lblColumnOptionsSub.AutoSize = true;
      this.lblColumnOptionsSub.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblColumnOptionsSub.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblColumnOptionsSub.Location = new System.Drawing.Point(80, 226);
      this.lblColumnOptionsSub.Name = "lblColumnOptionsSub";
      this.lblColumnOptionsSub.Size = new System.Drawing.Size(438, 15);
      this.lblColumnOptionsSub.TabIndex = 16;
      this.lblColumnOptionsSub.Text = "Click the header of a column to specify options like column name and a datatype.";
      // 
      // lblColumnOptionsMain
      // 
      this.lblColumnOptionsMain.AutoSize = true;
      this.lblColumnOptionsMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblColumnOptionsMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblColumnOptionsMain.Location = new System.Drawing.Point(80, 206);
      this.lblColumnOptionsMain.Name = "lblColumnOptionsMain";
      this.lblColumnOptionsMain.Size = new System.Drawing.Size(161, 17);
      this.lblColumnOptionsMain.TabIndex = 15;
      this.lblColumnOptionsMain.Text = "3. Specify Column Options";
      // 
      // picColumnOptions
      // 
      this.picColumnOptions.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_ColumnOptions_32x32;
      this.picColumnOptions.Location = new System.Drawing.Point(42, 212);
      this.picColumnOptions.Name = "picColumnOptions";
      this.picColumnOptions.Size = new System.Drawing.Size(32, 32);
      this.picColumnOptions.TabIndex = 18;
      this.picColumnOptions.TabStop = false;
      // 
      // lblTableNameWarning
      // 
      this.lblTableNameWarning.AutoSize = true;
      this.lblTableNameWarning.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTableNameWarning.ForeColor = System.Drawing.Color.Red;
      this.lblTableNameWarning.Location = new System.Drawing.Point(151, 159);
      this.lblTableNameWarning.Name = "lblTableNameWarning";
      this.lblTableNameWarning.Size = new System.Drawing.Size(227, 12);
      this.lblTableNameWarning.TabIndex = 6;
      this.lblTableNameWarning.Text = "It is good practice to not use upper case letters or spaces.";
      this.lblTableNameWarning.Visible = false;
      // 
      // picTableNameWarning
      // 
      this.picTableNameWarning.Image = global::MySQL.ForExcel.Properties.Resources.Warning;
      this.picTableNameWarning.Location = new System.Drawing.Point(128, 154);
      this.picTableNameWarning.Name = "picTableNameWarning";
      this.picTableNameWarning.Size = new System.Drawing.Size(20, 20);
      this.picTableNameWarning.TabIndex = 15;
      this.picTableNameWarning.TabStop = false;
      this.picTableNameWarning.Visible = false;
      // 
      // cmbPrimaryKeyColumns
      // 
      this.cmbPrimaryKeyColumns.DisplayMember = "DisplayName";
      this.cmbPrimaryKeyColumns.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cmbPrimaryKeyColumns.FormattingEnabled = true;
      this.cmbPrimaryKeyColumns.Location = new System.Drawing.Point(639, 154);
      this.cmbPrimaryKeyColumns.Name = "cmbPrimaryKeyColumns";
      this.cmbPrimaryKeyColumns.Size = new System.Drawing.Size(121, 23);
      this.cmbPrimaryKeyColumns.TabIndex = 14;
      this.cmbPrimaryKeyColumns.ValueMember = "DisplayName";
      this.cmbPrimaryKeyColumns.SelectedIndexChanged += new System.EventHandler(this.cmbPrimaryKeyColumns_SelectedIndexChanged);
      // 
      // radUseExistingColumn
      // 
      this.radUseExistingColumn.AutoSize = true;
      this.radUseExistingColumn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.radUseExistingColumn.Location = new System.Drawing.Point(463, 154);
      this.radUseExistingColumn.Name = "radUseExistingColumn";
      this.radUseExistingColumn.Size = new System.Drawing.Size(134, 19);
      this.radUseExistingColumn.TabIndex = 13;
      this.radUseExistingColumn.TabStop = true;
      this.radUseExistingColumn.Text = "Use existing column:";
      this.radUseExistingColumn.UseVisualStyleBackColor = true;
      this.radUseExistingColumn.CheckedChanged += new System.EventHandler(this.radUseExistingColumn_CheckedChanged);
      // 
      // txtAddPrimaryKey
      // 
      this.txtAddPrimaryKey.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtAddPrimaryKey.Location = new System.Drawing.Point(638, 126);
      this.txtAddPrimaryKey.Name = "txtAddPrimaryKey";
      this.txtAddPrimaryKey.Size = new System.Drawing.Size(122, 22);
      this.txtAddPrimaryKey.TabIndex = 12;
      this.txtAddPrimaryKey.TextChanged += new System.EventHandler(this.txtAddPrimaryKey_TextChanged);
      // 
      // radAddPrimaryKey
      // 
      this.radAddPrimaryKey.AutoSize = true;
      this.radAddPrimaryKey.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.radAddPrimaryKey.Location = new System.Drawing.Point(463, 126);
      this.radAddPrimaryKey.Name = "radAddPrimaryKey";
      this.radAddPrimaryKey.Size = new System.Drawing.Size(169, 19);
      this.radAddPrimaryKey.TabIndex = 11;
      this.radAddPrimaryKey.TabStop = true;
      this.radAddPrimaryKey.Text = "Add a Primary Key column:";
      this.radAddPrimaryKey.UseVisualStyleBackColor = true;
      this.radAddPrimaryKey.CheckedChanged += new System.EventHandler(this.radAddPrimaryKey_CheckedChanged);
      // 
      // txtTableNameInput
      // 
      this.txtTableNameInput.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtTableNameInput.Location = new System.Drawing.Point(128, 126);
      this.txtTableNameInput.Name = "txtTableNameInput";
      this.txtTableNameInput.Size = new System.Drawing.Size(219, 22);
      this.txtTableNameInput.TabIndex = 5;
      this.txtTableNameInput.TextChanged += new System.EventHandler(this.txtTableNameInput_TextChanged);
      this.txtTableNameInput.Validating += new System.ComponentModel.CancelEventHandler(this.txtTableNameInput_Validating);
      // 
      // lblTableNameInput
      // 
      this.lblTableNameInput.AutoSize = true;
      this.lblTableNameInput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTableNameInput.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblTableNameInput.Location = new System.Drawing.Point(80, 128);
      this.lblTableNameInput.Name = "lblTableNameInput";
      this.lblTableNameInput.Size = new System.Drawing.Size(42, 15);
      this.lblTableNameInput.TabIndex = 4;
      this.lblTableNameInput.Text = "Name:";
      // 
      // lblPrimaryKeySub2
      // 
      this.lblPrimaryKeySub2.AutoSize = true;
      this.lblPrimaryKeySub2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPrimaryKeySub2.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblPrimaryKeySub2.Location = new System.Drawing.Point(460, 86);
      this.lblPrimaryKeySub2.Name = "lblPrimaryKeySub2";
      this.lblPrimaryKeySub2.Size = new System.Drawing.Size(170, 15);
      this.lblPrimaryKeySub2.TabIndex = 10;
      this.lblPrimaryKeySub2.Text = "that is used as the Primary Key.";
      // 
      // lblPrimaryKeySub1
      // 
      this.lblPrimaryKeySub1.AutoSize = true;
      this.lblPrimaryKeySub1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPrimaryKeySub1.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblPrimaryKeySub1.Location = new System.Drawing.Point(460, 71);
      this.lblPrimaryKeySub1.Name = "lblPrimaryKeySub1";
      this.lblPrimaryKeySub1.Size = new System.Drawing.Size(264, 15);
      this.lblPrimaryKeySub1.TabIndex = 9;
      this.lblPrimaryKeySub1.Text = "Each row of data needs to hold a unique number";
      // 
      // lblPrimaryKeyMain
      // 
      this.lblPrimaryKeyMain.AutoSize = true;
      this.lblPrimaryKeyMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPrimaryKeyMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblPrimaryKeyMain.Location = new System.Drawing.Point(460, 51);
      this.lblPrimaryKeyMain.Name = "lblPrimaryKeyMain";
      this.lblPrimaryKeyMain.Size = new System.Drawing.Size(128, 17);
      this.lblPrimaryKeyMain.TabIndex = 8;
      this.lblPrimaryKeyMain.Text = "2. Pick a Primary Key";
      // 
      // picPrimaryKey
      // 
      this.picPrimaryKey.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_PrimaryKey_32x32;
      this.picPrimaryKey.Location = new System.Drawing.Point(422, 66);
      this.picPrimaryKey.Name = "picPrimaryKey";
      this.picPrimaryKey.Size = new System.Drawing.Size(32, 32);
      this.picPrimaryKey.TabIndex = 5;
      this.picPrimaryKey.TabStop = false;
      // 
      // lblTableNameSub2
      // 
      this.lblTableNameSub2.AutoSize = true;
      this.lblTableNameSub2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTableNameSub2.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblTableNameSub2.Location = new System.Drawing.Point(80, 91);
      this.lblTableNameSub2.Name = "lblTableNameSub2";
      this.lblTableNameSub2.Size = new System.Drawing.Size(232, 15);
      this.lblTableNameSub2.TabIndex = 3;
      this.lblTableNameSub2.Text = "Please specify a unique name for the table.";
      // 
      // lblTableNameSub1
      // 
      this.lblTableNameSub1.AutoSize = true;
      this.lblTableNameSub1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTableNameSub1.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblTableNameSub1.Location = new System.Drawing.Point(80, 76);
      this.lblTableNameSub1.Name = "lblTableNameSub1";
      this.lblTableNameSub1.Size = new System.Drawing.Size(267, 15);
      this.lblTableNameSub1.TabIndex = 2;
      this.lblTableNameSub1.Text = "The selected data will be stored in a MySQL table.";
      // 
      // lblTableNameMain
      // 
      this.lblTableNameMain.AutoSize = true;
      this.lblTableNameMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTableNameMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblTableNameMain.Location = new System.Drawing.Point(80, 56);
      this.lblTableNameMain.Name = "lblTableNameMain";
      this.lblTableNameMain.Size = new System.Drawing.Size(126, 17);
      this.lblTableNameMain.TabIndex = 1;
      this.lblTableNameMain.Text = "1. Set a Table Name";
      // 
      // picTable
      // 
      this.picTable.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_TableName_32x32;
      this.picTable.Location = new System.Drawing.Point(42, 71);
      this.picTable.Name = "picTable";
      this.picTable.Size = new System.Drawing.Size(32, 32);
      this.picTable.TabIndex = 1;
      this.picTable.TabStop = false;
      // 
      // lblExportData
      // 
      this.lblExportData.AutoSize = true;
      this.lblExportData.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblExportData.ForeColor = System.Drawing.Color.Navy;
      this.lblExportData.Location = new System.Drawing.Point(18, 18);
      this.lblExportData.Name = "lblExportData";
      this.lblExportData.Size = new System.Drawing.Size(156, 20);
      this.lblExportData.TabIndex = 0;
      this.lblExportData.Text = "Export Data to MySQL";
      // 
      // lblColumnOptionsWarning
      // 
      this.lblColumnOptionsWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblColumnOptionsWarning.AutoSize = true;
      this.lblColumnOptionsWarning.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblColumnOptionsWarning.ForeColor = System.Drawing.Color.Red;
      this.lblColumnOptionsWarning.Location = new System.Drawing.Point(120, 0);
      this.lblColumnOptionsWarning.Name = "lblColumnOptionsWarning";
      this.lblColumnOptionsWarning.Size = new System.Drawing.Size(227, 12);
      this.lblColumnOptionsWarning.TabIndex = 0;
      this.lblColumnOptionsWarning.Text = "It is good practice to not use upper case letters or spaces.";
      this.lblColumnOptionsWarning.Visible = false;
      // 
      // picColumnOptionsWarning
      // 
      this.picColumnOptionsWarning.Image = global::MySQL.ForExcel.Properties.Resources.Warning;
      this.picColumnOptionsWarning.Location = new System.Drawing.Point(98, -1);
      this.picColumnOptionsWarning.Name = "picColumnOptionsWarning";
      this.picColumnOptionsWarning.Size = new System.Drawing.Size(20, 20);
      this.picColumnOptionsWarning.TabIndex = 24;
      this.picColumnOptionsWarning.TabStop = false;
      this.picColumnOptionsWarning.Visible = false;
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(757, 566);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnExport
      // 
      this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExport.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExport.Enabled = false;
      this.btnExport.Location = new System.Drawing.Point(676, 566);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new System.Drawing.Size(75, 23);
      this.btnExport.TabIndex = 3;
      this.btnExport.Text = "Export";
      this.btnExport.UseVisualStyleBackColor = true;
      this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
      // 
      // btnAdvanced
      // 
      this.btnAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnAdvanced.Location = new System.Drawing.Point(12, 566);
      this.btnAdvanced.Name = "btnAdvanced";
      this.btnAdvanced.Size = new System.Drawing.Size(131, 23);
      this.btnAdvanced.TabIndex = 1;
      this.btnAdvanced.Text = "Advanced Options...";
      this.btnAdvanced.UseVisualStyleBackColor = true;
      this.btnAdvanced.Click += new System.EventHandler(this.btnAdvanced_Click);
      // 
      // btnCopySQL
      // 
      this.btnCopySQL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCopySQL.Location = new System.Drawing.Point(595, 566);
      this.btnCopySQL.Name = "btnCopySQL";
      this.btnCopySQL.Size = new System.Drawing.Size(75, 23);
      this.btnCopySQL.TabIndex = 2;
      this.btnCopySQL.Text = "Copy SQL";
      this.btnCopySQL.UseVisualStyleBackColor = true;
      this.btnCopySQL.Visible = false;
      this.btnCopySQL.Click += new System.EventHandler(this.btnCopySQL_Click);
      // 
      // timerTextChanged
      // 
      this.timerTextChanged.Interval = 800;
      this.timerTextChanged.Tick += new System.EventHandler(this.timerTextChanged_Tick);
      // 
      // columnBindingSource
      // 
      this.columnBindingSource.DataSource = typeof(MySQL.ForExcel.MySQLDataColumn);
      // 
      // ExportDataForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(844, 602);
      this.Controls.Add(this.btnCopySQL);
      this.Controls.Add(this.btnAdvanced);
      this.Controls.Add(this.btnExport);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.ExportDataPanel);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(860, 640);
      this.Name = "ExportDataForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Export Data";
      this.ExportDataPanel.ResumeLayout(false);
      this.ExportDataPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picPrimaryKeyWarning)).EndInit();
      this.grpColumnOptions.ResumeLayout(false);
      this.grpColumnOptions.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColumnOptions)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picTableNameWarning)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPrimaryKey)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picTable)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColumnOptionsWarning)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.columnBindingSource)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel ExportDataPanel;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnExport;
    private System.Windows.Forms.Button btnAdvanced;
    private System.Windows.Forms.Label lblExportData;
    private System.Windows.Forms.Label lblPrimaryKeySub2;
    private System.Windows.Forms.Label lblPrimaryKeySub1;
    private System.Windows.Forms.Label lblPrimaryKeyMain;
    private System.Windows.Forms.PictureBox picPrimaryKey;
    private System.Windows.Forms.Label lblTableNameSub2;
    private System.Windows.Forms.Label lblTableNameSub1;
    private System.Windows.Forms.Label lblTableNameMain;
    private System.Windows.Forms.PictureBox picTable;
    private System.Windows.Forms.TextBox txtTableNameInput;
    private System.Windows.Forms.Label lblTableNameInput;
    private System.Windows.Forms.TextBox txtAddPrimaryKey;
    private System.Windows.Forms.RadioButton radAddPrimaryKey;
    private System.Windows.Forms.ComboBox cmbPrimaryKeyColumns;
    private System.Windows.Forms.RadioButton radUseExistingColumn;
    private System.Windows.Forms.Label lblTableNameWarning;
    private System.Windows.Forms.PictureBox picTableNameWarning;
    private System.Windows.Forms.DataGridView grdPreviewData;
    private System.Windows.Forms.Label lblColumnOptionsSub;
    private System.Windows.Forms.Label lblColumnOptionsMain;
    private System.Windows.Forms.PictureBox picColumnOptions;
    private System.Windows.Forms.GroupBox grpColumnOptions;
    private System.Windows.Forms.Label lblColumnOptionsWarning;
    private System.Windows.Forms.PictureBox picColumnOptionsWarning;
    private System.Windows.Forms.CheckBox chkFirstRowHeaders;
    private System.Windows.Forms.ComboBox cmbDatatype;
    private System.Windows.Forms.CheckBox chkExcludeColumn;
    private System.Windows.Forms.CheckBox chkAllowEmpty;
    private System.Windows.Forms.CheckBox chkPrimaryKey;
    private System.Windows.Forms.CheckBox chkUniqueIndex;
    private System.Windows.Forms.CheckBox chkCreateIndex;
    private System.Windows.Forms.Label lblDatatype;
    private System.Windows.Forms.TextBox txtColumnName;
    private System.Windows.Forms.Label lblColumnName;
    private System.Windows.Forms.Button btnCopySQL;
    private System.Windows.Forms.BindingSource columnBindingSource;
    private System.Windows.Forms.Timer timerTextChanged;
    private System.Windows.Forms.Label lblPrimaryKeyWarning;
    private System.Windows.Forms.PictureBox picPrimaryKeyWarning;
    private System.Windows.Forms.ToolTip gridToolTip;
  }
}