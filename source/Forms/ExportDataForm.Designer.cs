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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      this.columnBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.timerTextChanged = new System.Windows.Forms.Timer(this.components);
      this.btnCopySQL = new System.Windows.Forms.Button();
      this.btnAdvanced = new System.Windows.Forms.Button();
      this.btnExport = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
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
      this.lblColumnOptionsWarning = new System.Windows.Forms.Label();
      this.picColumnOptionsWarning = new System.Windows.Forms.PictureBox();
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
      this.contentAreaPanel.SuspendLayout();
      this.commandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.columnBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPrimaryKeyWarning)).BeginInit();
      this.grpColumnOptions.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picColumnOptionsWarning)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColumnOptions)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picTableNameWarning)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPrimaryKey)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picTable)).BeginInit();
      this.SuspendLayout();
      // 
      // contentAreaPanel
      // 
      this.contentAreaPanel.Controls.Add(this.lblPrimaryKeyWarning);
      this.contentAreaPanel.Controls.Add(this.picPrimaryKeyWarning);
      this.contentAreaPanel.Controls.Add(this.grpColumnOptions);
      this.contentAreaPanel.Controls.Add(this.chkFirstRowHeaders);
      this.contentAreaPanel.Controls.Add(this.grdPreviewData);
      this.contentAreaPanel.Controls.Add(this.lblColumnOptionsSub);
      this.contentAreaPanel.Controls.Add(this.lblColumnOptionsMain);
      this.contentAreaPanel.Controls.Add(this.picColumnOptions);
      this.contentAreaPanel.Controls.Add(this.lblTableNameWarning);
      this.contentAreaPanel.Controls.Add(this.picTableNameWarning);
      this.contentAreaPanel.Controls.Add(this.cmbPrimaryKeyColumns);
      this.contentAreaPanel.Controls.Add(this.radUseExistingColumn);
      this.contentAreaPanel.Controls.Add(this.txtAddPrimaryKey);
      this.contentAreaPanel.Controls.Add(this.radAddPrimaryKey);
      this.contentAreaPanel.Controls.Add(this.txtTableNameInput);
      this.contentAreaPanel.Controls.Add(this.lblTableNameInput);
      this.contentAreaPanel.Controls.Add(this.lblPrimaryKeySub2);
      this.contentAreaPanel.Controls.Add(this.lblPrimaryKeySub1);
      this.contentAreaPanel.Controls.Add(this.lblPrimaryKeyMain);
      this.contentAreaPanel.Controls.Add(this.picPrimaryKey);
      this.contentAreaPanel.Controls.Add(this.lblTableNameSub2);
      this.contentAreaPanel.Controls.Add(this.lblTableNameSub1);
      this.contentAreaPanel.Controls.Add(this.lblTableNameMain);
      this.contentAreaPanel.Controls.Add(this.picTable);
      this.contentAreaPanel.Size = new System.Drawing.Size(844, 556);
      // 
      // commandAreaPanel
      // 
      this.commandAreaPanel.Controls.Add(this.btnCopySQL);
      this.commandAreaPanel.Controls.Add(this.btnAdvanced);
      this.commandAreaPanel.Controls.Add(this.btnExport);
      this.commandAreaPanel.Controls.Add(this.btnCancel);
      this.commandAreaPanel.Location = new System.Drawing.Point(0, 557);
      this.commandAreaPanel.Size = new System.Drawing.Size(844, 44);
      // 
      // columnBindingSource
      // 
      this.columnBindingSource.DataSource = typeof(MySQL.ForExcel.MySQLDataColumn);
      // 
      // timerTextChanged
      // 
      this.timerTextChanged.Interval = 800;
      this.timerTextChanged.Tick += new System.EventHandler(this.timerTextChanged_Tick);
      // 
      // btnCopySQL
      // 
      this.btnCopySQL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCopySQL.Location = new System.Drawing.Point(595, 11);
      this.btnCopySQL.Name = "btnCopySQL";
      this.btnCopySQL.Size = new System.Drawing.Size(75, 23);
      this.btnCopySQL.TabIndex = 1;
      this.btnCopySQL.Text = "Copy SQL";
      this.btnCopySQL.UseVisualStyleBackColor = true;
      this.btnCopySQL.Visible = false;
      this.btnCopySQL.Click += new System.EventHandler(this.btnCopySQL_Click);
      // 
      // btnAdvanced
      // 
      this.btnAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnAdvanced.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnAdvanced.Location = new System.Drawing.Point(12, 11);
      this.btnAdvanced.Name = "btnAdvanced";
      this.btnAdvanced.Size = new System.Drawing.Size(131, 23);
      this.btnAdvanced.TabIndex = 0;
      this.btnAdvanced.Text = "Advanced Options...";
      this.btnAdvanced.UseVisualStyleBackColor = true;
      this.btnAdvanced.Click += new System.EventHandler(this.btnAdvanced_Click);
      // 
      // btnExport
      // 
      this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExport.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExport.Enabled = false;
      this.btnExport.Location = new System.Drawing.Point(676, 11);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new System.Drawing.Size(75, 23);
      this.btnExport.TabIndex = 2;
      this.btnExport.Text = "Export";
      this.btnExport.UseVisualStyleBackColor = true;
      this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(757, 11);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // lblPrimaryKeyWarning
      // 
      this.lblPrimaryKeyWarning.AutoSize = true;
      this.lblPrimaryKeyWarning.BackColor = System.Drawing.Color.Transparent;
      this.lblPrimaryKeyWarning.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPrimaryKeyWarning.ForeColor = System.Drawing.Color.Red;
      this.lblPrimaryKeyWarning.Location = new System.Drawing.Point(485, 182);
      this.lblPrimaryKeyWarning.Name = "lblPrimaryKeyWarning";
      this.lblPrimaryKeyWarning.Size = new System.Drawing.Size(336, 12);
      this.lblPrimaryKeyWarning.TabIndex = 13;
      this.lblPrimaryKeyWarning.Text = "Primary Key column cannot be created because another column has the same name.";
      this.lblPrimaryKeyWarning.Visible = false;
      // 
      // picPrimaryKeyWarning
      // 
      this.picPrimaryKeyWarning.BackColor = System.Drawing.Color.Transparent;
      this.picPrimaryKeyWarning.Image = global::MySQL.ForExcel.Properties.Resources.Warning;
      this.picPrimaryKeyWarning.Location = new System.Drawing.Point(462, 177);
      this.picPrimaryKeyWarning.Name = "picPrimaryKeyWarning";
      this.picPrimaryKeyWarning.Size = new System.Drawing.Size(20, 20);
      this.picPrimaryKeyWarning.TabIndex = 45;
      this.picPrimaryKeyWarning.TabStop = false;
      this.picPrimaryKeyWarning.Visible = false;
      // 
      // grpColumnOptions
      // 
      this.grpColumnOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.grpColumnOptions.BackColor = System.Drawing.Color.Transparent;
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
      this.grpColumnOptions.Location = new System.Drawing.Point(82, 445);
      this.grpColumnOptions.Name = "grpColumnOptions";
      this.grpColumnOptions.Size = new System.Drawing.Size(677, 100);
      this.grpColumnOptions.TabIndex = 18;
      this.grpColumnOptions.TabStop = false;
      this.grpColumnOptions.Text = "Column Options";
      // 
      // cmbDatatype
      // 
      this.cmbDatatype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.cmbDatatype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.cmbDatatype.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.columnBindingSource, "MySQLDataType", true));
      this.cmbDatatype.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.cmbDatatype.DropDownWidth = 400;
      this.cmbDatatype.FormattingEnabled = true;
      this.cmbDatatype.Location = new System.Drawing.Point(122, 62);
      this.cmbDatatype.Name = "cmbDatatype";
      this.cmbDatatype.Size = new System.Drawing.Size(135, 24);
      this.cmbDatatype.TabIndex = 4;
      this.cmbDatatype.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbDatatype_DrawItem);
      this.cmbDatatype.SelectedIndexChanged += new System.EventHandler(this.cmbDatatype_SelectedIndexChanged);
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
      // chkFirstRowHeaders
      // 
      this.chkFirstRowHeaders.AutoSize = true;
      this.chkFirstRowHeaders.BackColor = System.Drawing.Color.Transparent;
      this.chkFirstRowHeaders.Checked = true;
      this.chkFirstRowHeaders.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkFirstRowHeaders.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkFirstRowHeaders.Location = new System.Drawing.Point(82, 254);
      this.chkFirstRowHeaders.Name = "chkFirstRowHeaders";
      this.chkFirstRowHeaders.Size = new System.Drawing.Size(210, 19);
      this.chkFirstRowHeaders.TabIndex = 16;
      this.chkFirstRowHeaders.Text = "First Row Contains Column Names";
      this.chkFirstRowHeaders.UseVisualStyleBackColor = false;
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
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.grdPreviewData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.grdPreviewData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdPreviewData.DefaultCellStyle = dataGridViewCellStyle2;
      this.grdPreviewData.Location = new System.Drawing.Point(82, 277);
      this.grdPreviewData.MultiSelect = false;
      this.grdPreviewData.Name = "grdPreviewData";
      this.grdPreviewData.ReadOnly = true;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.grdPreviewData.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
      this.grdPreviewData.RowHeadersVisible = false;
      this.grdPreviewData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.grdPreviewData.ShowCellErrors = false;
      this.grdPreviewData.ShowEditingIcon = false;
      this.grdPreviewData.ShowRowErrors = false;
      this.grdPreviewData.Size = new System.Drawing.Size(677, 158);
      this.grdPreviewData.TabIndex = 17;
      this.grdPreviewData.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.grdPreviewData_CellToolTipTextNeeded);
      this.grdPreviewData.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grdPreviewData_DataBindingComplete);
      this.grdPreviewData.SelectionChanged += new System.EventHandler(this.grdPreviewData_SelectionChanged);
      this.grdPreviewData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdPreviewData_KeyDown);
      // 
      // lblColumnOptionsSub
      // 
      this.lblColumnOptionsSub.AutoSize = true;
      this.lblColumnOptionsSub.BackColor = System.Drawing.Color.Transparent;
      this.lblColumnOptionsSub.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblColumnOptionsSub.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblColumnOptionsSub.Location = new System.Drawing.Point(79, 224);
      this.lblColumnOptionsSub.Name = "lblColumnOptionsSub";
      this.lblColumnOptionsSub.Size = new System.Drawing.Size(438, 15);
      this.lblColumnOptionsSub.TabIndex = 15;
      this.lblColumnOptionsSub.Text = "Click the header of a column to specify options like column name and a datatype.";
      // 
      // lblColumnOptionsMain
      // 
      this.lblColumnOptionsMain.AutoSize = true;
      this.lblColumnOptionsMain.BackColor = System.Drawing.Color.Transparent;
      this.lblColumnOptionsMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblColumnOptionsMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblColumnOptionsMain.Location = new System.Drawing.Point(79, 204);
      this.lblColumnOptionsMain.Name = "lblColumnOptionsMain";
      this.lblColumnOptionsMain.Size = new System.Drawing.Size(161, 17);
      this.lblColumnOptionsMain.TabIndex = 14;
      this.lblColumnOptionsMain.Text = "3. Specify Column Options";
      // 
      // picColumnOptions
      // 
      this.picColumnOptions.BackColor = System.Drawing.Color.Transparent;
      this.picColumnOptions.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_ColumnOptions_32x32;
      this.picColumnOptions.Location = new System.Drawing.Point(41, 210);
      this.picColumnOptions.Name = "picColumnOptions";
      this.picColumnOptions.Size = new System.Drawing.Size(32, 32);
      this.picColumnOptions.TabIndex = 41;
      this.picColumnOptions.TabStop = false;
      // 
      // lblTableNameWarning
      // 
      this.lblTableNameWarning.AutoSize = true;
      this.lblTableNameWarning.BackColor = System.Drawing.Color.Transparent;
      this.lblTableNameWarning.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTableNameWarning.ForeColor = System.Drawing.Color.Red;
      this.lblTableNameWarning.Location = new System.Drawing.Point(150, 157);
      this.lblTableNameWarning.Name = "lblTableNameWarning";
      this.lblTableNameWarning.Size = new System.Drawing.Size(227, 12);
      this.lblTableNameWarning.TabIndex = 5;
      this.lblTableNameWarning.Text = "It is good practice to not use upper case letters or spaces.";
      this.lblTableNameWarning.Visible = false;
      // 
      // picTableNameWarning
      // 
      this.picTableNameWarning.BackColor = System.Drawing.Color.Transparent;
      this.picTableNameWarning.Image = global::MySQL.ForExcel.Properties.Resources.Warning;
      this.picTableNameWarning.Location = new System.Drawing.Point(127, 152);
      this.picTableNameWarning.Name = "picTableNameWarning";
      this.picTableNameWarning.Size = new System.Drawing.Size(20, 20);
      this.picTableNameWarning.TabIndex = 38;
      this.picTableNameWarning.TabStop = false;
      this.picTableNameWarning.Visible = false;
      // 
      // cmbPrimaryKeyColumns
      // 
      this.cmbPrimaryKeyColumns.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.cmbPrimaryKeyColumns.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.cmbPrimaryKeyColumns.DisplayMember = "DisplayName";
      this.cmbPrimaryKeyColumns.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cmbPrimaryKeyColumns.FormattingEnabled = true;
      this.cmbPrimaryKeyColumns.Location = new System.Drawing.Point(638, 152);
      this.cmbPrimaryKeyColumns.Name = "cmbPrimaryKeyColumns";
      this.cmbPrimaryKeyColumns.Size = new System.Drawing.Size(121, 23);
      this.cmbPrimaryKeyColumns.TabIndex = 12;
      this.cmbPrimaryKeyColumns.ValueMember = "DisplayName";
      this.cmbPrimaryKeyColumns.SelectedIndexChanged += new System.EventHandler(this.cmbPrimaryKeyColumns_SelectedIndexChanged);
      // 
      // radUseExistingColumn
      // 
      this.radUseExistingColumn.AutoSize = true;
      this.radUseExistingColumn.BackColor = System.Drawing.Color.Transparent;
      this.radUseExistingColumn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.radUseExistingColumn.Location = new System.Drawing.Point(462, 152);
      this.radUseExistingColumn.Name = "radUseExistingColumn";
      this.radUseExistingColumn.Size = new System.Drawing.Size(134, 19);
      this.radUseExistingColumn.TabIndex = 11;
      this.radUseExistingColumn.TabStop = true;
      this.radUseExistingColumn.Text = "Use existing column:";
      this.radUseExistingColumn.UseVisualStyleBackColor = false;
      this.radUseExistingColumn.CheckedChanged += new System.EventHandler(this.radUseExistingColumn_CheckedChanged);
      // 
      // txtAddPrimaryKey
      // 
      this.txtAddPrimaryKey.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtAddPrimaryKey.Location = new System.Drawing.Point(637, 124);
      this.txtAddPrimaryKey.Name = "txtAddPrimaryKey";
      this.txtAddPrimaryKey.Size = new System.Drawing.Size(122, 22);
      this.txtAddPrimaryKey.TabIndex = 10;
      this.txtAddPrimaryKey.TextChanged += new System.EventHandler(this.txtAddPrimaryKey_TextChanged);
      // 
      // radAddPrimaryKey
      // 
      this.radAddPrimaryKey.AutoSize = true;
      this.radAddPrimaryKey.BackColor = System.Drawing.Color.Transparent;
      this.radAddPrimaryKey.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.radAddPrimaryKey.Location = new System.Drawing.Point(462, 124);
      this.radAddPrimaryKey.Name = "radAddPrimaryKey";
      this.radAddPrimaryKey.Size = new System.Drawing.Size(169, 19);
      this.radAddPrimaryKey.TabIndex = 9;
      this.radAddPrimaryKey.TabStop = true;
      this.radAddPrimaryKey.Text = "Add a Primary Key column:";
      this.radAddPrimaryKey.UseVisualStyleBackColor = false;
      this.radAddPrimaryKey.CheckedChanged += new System.EventHandler(this.radAddPrimaryKey_CheckedChanged);
      // 
      // txtTableNameInput
      // 
      this.txtTableNameInput.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtTableNameInput.Location = new System.Drawing.Point(127, 124);
      this.txtTableNameInput.Name = "txtTableNameInput";
      this.txtTableNameInput.Size = new System.Drawing.Size(219, 22);
      this.txtTableNameInput.TabIndex = 4;
      this.txtTableNameInput.TextChanged += new System.EventHandler(this.txtTableNameInput_TextChanged);
      this.txtTableNameInput.Validating += new System.ComponentModel.CancelEventHandler(this.txtTableNameInput_Validating);
      // 
      // lblTableNameInput
      // 
      this.lblTableNameInput.AutoSize = true;
      this.lblTableNameInput.BackColor = System.Drawing.Color.Transparent;
      this.lblTableNameInput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTableNameInput.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblTableNameInput.Location = new System.Drawing.Point(79, 126);
      this.lblTableNameInput.Name = "lblTableNameInput";
      this.lblTableNameInput.Size = new System.Drawing.Size(42, 15);
      this.lblTableNameInput.TabIndex = 3;
      this.lblTableNameInput.Text = "Name:";
      // 
      // lblPrimaryKeySub2
      // 
      this.lblPrimaryKeySub2.AutoSize = true;
      this.lblPrimaryKeySub2.BackColor = System.Drawing.Color.Transparent;
      this.lblPrimaryKeySub2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPrimaryKeySub2.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblPrimaryKeySub2.Location = new System.Drawing.Point(459, 84);
      this.lblPrimaryKeySub2.Name = "lblPrimaryKeySub2";
      this.lblPrimaryKeySub2.Size = new System.Drawing.Size(170, 15);
      this.lblPrimaryKeySub2.TabIndex = 8;
      this.lblPrimaryKeySub2.Text = "that is used as the Primary Key.";
      // 
      // lblPrimaryKeySub1
      // 
      this.lblPrimaryKeySub1.AutoSize = true;
      this.lblPrimaryKeySub1.BackColor = System.Drawing.Color.Transparent;
      this.lblPrimaryKeySub1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPrimaryKeySub1.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblPrimaryKeySub1.Location = new System.Drawing.Point(459, 69);
      this.lblPrimaryKeySub1.Name = "lblPrimaryKeySub1";
      this.lblPrimaryKeySub1.Size = new System.Drawing.Size(264, 15);
      this.lblPrimaryKeySub1.TabIndex = 7;
      this.lblPrimaryKeySub1.Text = "Each row of data needs to hold a unique number";
      // 
      // lblPrimaryKeyMain
      // 
      this.lblPrimaryKeyMain.AutoSize = true;
      this.lblPrimaryKeyMain.BackColor = System.Drawing.Color.Transparent;
      this.lblPrimaryKeyMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPrimaryKeyMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblPrimaryKeyMain.Location = new System.Drawing.Point(459, 49);
      this.lblPrimaryKeyMain.Name = "lblPrimaryKeyMain";
      this.lblPrimaryKeyMain.Size = new System.Drawing.Size(128, 17);
      this.lblPrimaryKeyMain.TabIndex = 6;
      this.lblPrimaryKeyMain.Text = "2. Pick a Primary Key";
      // 
      // picPrimaryKey
      // 
      this.picPrimaryKey.BackColor = System.Drawing.Color.Transparent;
      this.picPrimaryKey.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_PrimaryKey_32x32;
      this.picPrimaryKey.Location = new System.Drawing.Point(421, 64);
      this.picPrimaryKey.Name = "picPrimaryKey";
      this.picPrimaryKey.Size = new System.Drawing.Size(32, 32);
      this.picPrimaryKey.TabIndex = 28;
      this.picPrimaryKey.TabStop = false;
      // 
      // lblTableNameSub2
      // 
      this.lblTableNameSub2.AutoSize = true;
      this.lblTableNameSub2.BackColor = System.Drawing.Color.Transparent;
      this.lblTableNameSub2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTableNameSub2.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblTableNameSub2.Location = new System.Drawing.Point(79, 89);
      this.lblTableNameSub2.Name = "lblTableNameSub2";
      this.lblTableNameSub2.Size = new System.Drawing.Size(232, 15);
      this.lblTableNameSub2.TabIndex = 2;
      this.lblTableNameSub2.Text = "Please specify a unique name for the table.";
      // 
      // lblTableNameSub1
      // 
      this.lblTableNameSub1.AutoSize = true;
      this.lblTableNameSub1.BackColor = System.Drawing.Color.Transparent;
      this.lblTableNameSub1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTableNameSub1.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblTableNameSub1.Location = new System.Drawing.Point(79, 74);
      this.lblTableNameSub1.Name = "lblTableNameSub1";
      this.lblTableNameSub1.Size = new System.Drawing.Size(267, 15);
      this.lblTableNameSub1.TabIndex = 1;
      this.lblTableNameSub1.Text = "The selected data will be stored in a MySQL table.";
      // 
      // lblTableNameMain
      // 
      this.lblTableNameMain.AutoSize = true;
      this.lblTableNameMain.BackColor = System.Drawing.Color.Transparent;
      this.lblTableNameMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTableNameMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblTableNameMain.Location = new System.Drawing.Point(79, 54);
      this.lblTableNameMain.Name = "lblTableNameMain";
      this.lblTableNameMain.Size = new System.Drawing.Size(126, 17);
      this.lblTableNameMain.TabIndex = 0;
      this.lblTableNameMain.Text = "1. Set a Table Name";
      // 
      // picTable
      // 
      this.picTable.BackColor = System.Drawing.Color.Transparent;
      this.picTable.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_TableName_32x32;
      this.picTable.Location = new System.Drawing.Point(41, 69);
      this.picTable.Name = "picTable";
      this.picTable.Size = new System.Drawing.Size(32, 32);
      this.picTable.TabIndex = 23;
      this.picTable.TabStop = false;
      // 
      // ExportDataForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(844, 602);
      this.CommandAreaHeight = 44;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MainInstruction = "Export Data to MySQL";
      this.MainInstructionLocation = new System.Drawing.Point(10, 15);
      this.MinimumSize = new System.Drawing.Size(860, 640);
      this.Name = "ExportDataForm";
      this.Text = "Export Data";
      this.contentAreaPanel.ResumeLayout(false);
      this.contentAreaPanel.PerformLayout();
      this.commandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.columnBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPrimaryKeyWarning)).EndInit();
      this.grpColumnOptions.ResumeLayout(false);
      this.grpColumnOptions.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picColumnOptionsWarning)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColumnOptions)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picTableNameWarning)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPrimaryKey)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picTable)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.BindingSource columnBindingSource;
    private System.Windows.Forms.Timer timerTextChanged;
    private System.Windows.Forms.Button btnCopySQL;
    private System.Windows.Forms.Button btnAdvanced;
    private System.Windows.Forms.Button btnExport;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label lblPrimaryKeyWarning;
    private System.Windows.Forms.PictureBox picPrimaryKeyWarning;
    private System.Windows.Forms.GroupBox grpColumnOptions;
    private System.Windows.Forms.ComboBox cmbDatatype;
    private System.Windows.Forms.CheckBox chkExcludeColumn;
    private System.Windows.Forms.CheckBox chkAllowEmpty;
    private System.Windows.Forms.CheckBox chkPrimaryKey;
    private System.Windows.Forms.CheckBox chkUniqueIndex;
    private System.Windows.Forms.CheckBox chkCreateIndex;
    private System.Windows.Forms.Label lblDatatype;
    private System.Windows.Forms.TextBox txtColumnName;
    private System.Windows.Forms.Label lblColumnName;
    private System.Windows.Forms.Label lblColumnOptionsWarning;
    private System.Windows.Forms.PictureBox picColumnOptionsWarning;
    private System.Windows.Forms.CheckBox chkFirstRowHeaders;
    private System.Windows.Forms.DataGridView grdPreviewData;
    private System.Windows.Forms.Label lblColumnOptionsSub;
    private System.Windows.Forms.Label lblColumnOptionsMain;
    private System.Windows.Forms.PictureBox picColumnOptions;
    private System.Windows.Forms.Label lblTableNameWarning;
    private System.Windows.Forms.PictureBox picTableNameWarning;
    private System.Windows.Forms.ComboBox cmbPrimaryKeyColumns;
    private System.Windows.Forms.RadioButton radUseExistingColumn;
    private System.Windows.Forms.TextBox txtAddPrimaryKey;
    private System.Windows.Forms.RadioButton radAddPrimaryKey;
    private System.Windows.Forms.TextBox txtTableNameInput;
    private System.Windows.Forms.Label lblTableNameInput;
    private System.Windows.Forms.Label lblPrimaryKeySub2;
    private System.Windows.Forms.Label lblPrimaryKeySub1;
    private System.Windows.Forms.Label lblPrimaryKeyMain;
    private System.Windows.Forms.PictureBox picPrimaryKey;
    private System.Windows.Forms.Label lblTableNameSub2;
    private System.Windows.Forms.Label lblTableNameSub1;
    private System.Windows.Forms.Label lblTableNameMain;
    private System.Windows.Forms.PictureBox picTable;
  }
}