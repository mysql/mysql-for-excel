namespace MySQL.ExcelAddIn
{
  partial class OldExportDataToTableDialog
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
      this.lblExistingSchema = new System.Windows.Forms.Label();
      this.radExistingTable = new System.Windows.Forms.RadioButton();
      this.grpTargetTable = new System.Windows.Forms.GroupBox();
      this.lblEngine = new System.Windows.Forms.Label();
      this.cmbDBEngine = new System.Windows.Forms.ComboBox();
      this.txtNewTable = new System.Windows.Forms.TextBox();
      this.chkMakeSelectedTable = new System.Windows.Forms.CheckBox();
      this.chkMakeSelectedSchema = new System.Windows.Forms.CheckBox();
      this.cmbExistingTable = new System.Windows.Forms.ComboBox();
      this.cmbExistingSchema = new System.Windows.Forms.ComboBox();
      this.radNewTable = new System.Windows.Forms.RadioButton();
      this.grpColumnMapping = new System.Windows.Forms.GroupBox();
      this.btnMap = new System.Windows.Forms.Button();
      this.cmbColumnName = new System.Windows.Forms.ComboBox();
      this.columnsBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.chkColumnUniqueKey = new System.Windows.Forms.CheckBox();
      this.chkColumnPrimaryKey = new System.Windows.Forms.CheckBox();
      this.chkColumnAutoIncrement = new System.Windows.Forms.CheckBox();
      this.grpDataType = new System.Windows.Forms.GroupBox();
      this.chkColumnTypeBinary = new System.Windows.Forms.CheckBox();
      this.numColumnTypeDecimals = new System.Windows.Forms.NumericUpDown();
      this.lblColumnTypeDecimals = new System.Windows.Forms.Label();
      this.chkColumnTypeZeroFill = new System.Windows.Forms.CheckBox();
      this.chkColumnTypeUnsigned = new System.Windows.Forms.CheckBox();
      this.lblColumnTypeLength = new System.Windows.Forms.Label();
      this.cmbColumnType = new System.Windows.Forms.ComboBox();
      this.numColumnTypeLength = new System.Windows.Forms.NumericUpDown();
      this.lblColumnType = new System.Windows.Forms.Label();
      this.txtColumnDefaultValue = new System.Windows.Forms.TextBox();
      this.lblColumnDefaultValue = new System.Windows.Forms.Label();
      this.chkColumnNullable = new System.Windows.Forms.CheckBox();
      this.txtColumnName = new System.Windows.Forms.TextBox();
      this.lblColumnName = new System.Windows.Forms.Label();
      this.grpDataPreview = new System.Windows.Forms.GroupBox();
      this.lblMappedColumns = new System.Windows.Forms.Label();
      this.chkFirstRowHeaders = new System.Windows.Forms.CheckBox();
      this.btnUnmap = new System.Windows.Forms.Button();
      this.chkUseFormattedValues = new System.Windows.Forms.CheckBox();
      this.grdPreviewData = new System.Windows.Forms.DataGridView();
      this.btnExport = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.grpTargetTable.SuspendLayout();
      this.grpColumnMapping.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.columnsBindingSource)).BeginInit();
      this.grpDataType.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numColumnTypeDecimals)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numColumnTypeLength)).BeginInit();
      this.grpDataPreview.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).BeginInit();
      this.SuspendLayout();
      // 
      // lblExistingSchema
      // 
      this.lblExistingSchema.AutoSize = true;
      this.lblExistingSchema.Location = new System.Drawing.Point(21, 27);
      this.lblExistingSchema.Name = "lblExistingSchema";
      this.lblExistingSchema.Size = new System.Drawing.Size(88, 13);
      this.lblExistingSchema.TabIndex = 0;
      this.lblExistingSchema.Text = "Existing Schema:";
      // 
      // radExistingTable
      // 
      this.radExistingTable.AutoSize = true;
      this.radExistingTable.Location = new System.Drawing.Point(15, 51);
      this.radExistingTable.Name = "radExistingTable";
      this.radExistingTable.Size = new System.Drawing.Size(94, 17);
      this.radExistingTable.TabIndex = 3;
      this.radExistingTable.TabStop = true;
      this.radExistingTable.Text = "Existing Table:";
      this.radExistingTable.UseVisualStyleBackColor = true;
      this.radExistingTable.CheckedChanged += new System.EventHandler(this.radAnyTable_CheckedChanged);
      // 
      // grpTargetTable
      // 
      this.grpTargetTable.Controls.Add(this.lblEngine);
      this.grpTargetTable.Controls.Add(this.cmbDBEngine);
      this.grpTargetTable.Controls.Add(this.txtNewTable);
      this.grpTargetTable.Controls.Add(this.chkMakeSelectedTable);
      this.grpTargetTable.Controls.Add(this.chkMakeSelectedSchema);
      this.grpTargetTable.Controls.Add(this.cmbExistingTable);
      this.grpTargetTable.Controls.Add(this.cmbExistingSchema);
      this.grpTargetTable.Controls.Add(this.radNewTable);
      this.grpTargetTable.Controls.Add(this.lblExistingSchema);
      this.grpTargetTable.Controls.Add(this.radExistingTable);
      this.grpTargetTable.Location = new System.Drawing.Point(12, 12);
      this.grpTargetTable.Name = "grpTargetTable";
      this.grpTargetTable.Size = new System.Drawing.Size(553, 110);
      this.grpTargetTable.TabIndex = 0;
      this.grpTargetTable.TabStop = false;
      this.grpTargetTable.Text = "Target Table";
      // 
      // lblEngine
      // 
      this.lblEngine.AutoSize = true;
      this.lblEngine.Location = new System.Drawing.Point(375, 78);
      this.lblEngine.Name = "lblEngine";
      this.lblEngine.Size = new System.Drawing.Size(43, 13);
      this.lblEngine.TabIndex = 8;
      this.lblEngine.Text = "Engine:";
      // 
      // cmbDBEngine
      // 
      this.cmbDBEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbDBEngine.FormattingEnabled = true;
      this.cmbDBEngine.Location = new System.Drawing.Point(424, 75);
      this.cmbDBEngine.Name = "cmbDBEngine";
      this.cmbDBEngine.Size = new System.Drawing.Size(117, 21);
      this.cmbDBEngine.TabIndex = 9;
      // 
      // txtNewTable
      // 
      this.txtNewTable.Location = new System.Drawing.Point(115, 78);
      this.txtNewTable.Name = "txtNewTable";
      this.txtNewTable.Size = new System.Drawing.Size(213, 20);
      this.txtNewTable.TabIndex = 7;
      this.txtNewTable.Validating += new System.ComponentModel.CancelEventHandler(this.txtNewTable_Validating);
      // 
      // chkMakeSelectedTable
      // 
      this.chkMakeSelectedTable.AutoSize = true;
      this.chkMakeSelectedTable.Location = new System.Drawing.Point(347, 51);
      this.chkMakeSelectedTable.Name = "chkMakeSelectedTable";
      this.chkMakeSelectedTable.Size = new System.Drawing.Size(194, 17);
      this.chkMakeSelectedTable.TabIndex = 5;
      this.chkMakeSelectedTable.Text = "Make Currently Selected DB Object";
      this.chkMakeSelectedTable.UseVisualStyleBackColor = true;
      // 
      // chkMakeSelectedSchema
      // 
      this.chkMakeSelectedSchema.AutoSize = true;
      this.chkMakeSelectedSchema.Location = new System.Drawing.Point(347, 26);
      this.chkMakeSelectedSchema.Name = "chkMakeSelectedSchema";
      this.chkMakeSelectedSchema.Size = new System.Drawing.Size(184, 17);
      this.chkMakeSelectedSchema.TabIndex = 2;
      this.chkMakeSelectedSchema.Text = "Make Currently Selected Schema";
      this.chkMakeSelectedSchema.UseVisualStyleBackColor = true;
      // 
      // cmbExistingTable
      // 
      this.cmbExistingTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbExistingTable.FormattingEnabled = true;
      this.cmbExistingTable.Location = new System.Drawing.Point(115, 51);
      this.cmbExistingTable.Name = "cmbExistingTable";
      this.cmbExistingTable.Size = new System.Drawing.Size(213, 21);
      this.cmbExistingTable.TabIndex = 4;
      this.cmbExistingTable.SelectionChangeCommitted += new System.EventHandler(this.cmbExistingTable_SelectionChangeCommitted);
      // 
      // cmbExistingSchema
      // 
      this.cmbExistingSchema.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbExistingSchema.FormattingEnabled = true;
      this.cmbExistingSchema.Location = new System.Drawing.Point(115, 24);
      this.cmbExistingSchema.Name = "cmbExistingSchema";
      this.cmbExistingSchema.Size = new System.Drawing.Size(213, 21);
      this.cmbExistingSchema.TabIndex = 1;
      this.cmbExistingSchema.SelectionChangeCommitted += new System.EventHandler(this.cmbExistingSchema_SelectionChangeCommitted);
      // 
      // radNewTable
      // 
      this.radNewTable.AutoSize = true;
      this.radNewTable.Location = new System.Drawing.Point(15, 79);
      this.radNewTable.Name = "radNewTable";
      this.radNewTable.Size = new System.Drawing.Size(80, 17);
      this.radNewTable.TabIndex = 6;
      this.radNewTable.TabStop = true;
      this.radNewTable.Text = "New Table:";
      this.radNewTable.UseVisualStyleBackColor = true;
      this.radNewTable.CheckedChanged += new System.EventHandler(this.radAnyTable_CheckedChanged);
      // 
      // grpColumnMapping
      // 
      this.grpColumnMapping.Controls.Add(this.btnMap);
      this.grpColumnMapping.Controls.Add(this.cmbColumnName);
      this.grpColumnMapping.Controls.Add(this.chkColumnUniqueKey);
      this.grpColumnMapping.Controls.Add(this.chkColumnPrimaryKey);
      this.grpColumnMapping.Controls.Add(this.chkColumnAutoIncrement);
      this.grpColumnMapping.Controls.Add(this.grpDataType);
      this.grpColumnMapping.Controls.Add(this.txtColumnDefaultValue);
      this.grpColumnMapping.Controls.Add(this.lblColumnDefaultValue);
      this.grpColumnMapping.Controls.Add(this.chkColumnNullable);
      this.grpColumnMapping.Controls.Add(this.txtColumnName);
      this.grpColumnMapping.Controls.Add(this.lblColumnName);
      this.grpColumnMapping.Location = new System.Drawing.Point(12, 128);
      this.grpColumnMapping.Name = "grpColumnMapping";
      this.grpColumnMapping.Size = new System.Drawing.Size(553, 150);
      this.grpColumnMapping.TabIndex = 1;
      this.grpColumnMapping.TabStop = false;
      this.grpColumnMapping.Text = "Column Mapping";
      // 
      // btnMap
      // 
      this.btnMap.Location = new System.Drawing.Point(347, 19);
      this.btnMap.Name = "btnMap";
      this.btnMap.Size = new System.Drawing.Size(194, 23);
      this.btnMap.TabIndex = 9;
      this.btnMap.Text = "Map Column";
      this.btnMap.UseVisualStyleBackColor = true;
      this.btnMap.Click += new System.EventHandler(this.btnMap_Click);
      // 
      // cmbColumnName
      // 
      this.cmbColumnName.DataSource = this.columnsBindingSource;
      this.cmbColumnName.DisplayMember = "Name";
      this.cmbColumnName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbColumnName.FormattingEnabled = true;
      this.cmbColumnName.Location = new System.Drawing.Point(115, 21);
      this.cmbColumnName.Name = "cmbColumnName";
      this.cmbColumnName.Size = new System.Drawing.Size(213, 21);
      this.cmbColumnName.TabIndex = 1;
      this.cmbColumnName.ValueMember = "Name";
      this.cmbColumnName.SelectedIndexChanged += new System.EventHandler(this.cmbColumnName_SelectedIndexChanged);
      // 
      // columnsBindingSource
      // 
      this.columnsBindingSource.DataSource = typeof(MySQL.ExcelAddIn.TableSchemaInfo);
      // 
      // chkColumnUniqueKey
      // 
      this.chkColumnUniqueKey.AutoSize = true;
      this.chkColumnUniqueKey.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.columnsBindingSource, "UniqueKey", true));
      this.chkColumnUniqueKey.Location = new System.Drawing.Point(460, 120);
      this.chkColumnUniqueKey.Name = "chkColumnUniqueKey";
      this.chkColumnUniqueKey.Size = new System.Drawing.Size(81, 17);
      this.chkColumnUniqueKey.TabIndex = 8;
      this.chkColumnUniqueKey.Text = "Unique Key";
      this.chkColumnUniqueKey.UseVisualStyleBackColor = true;
      this.chkColumnUniqueKey.CheckedChanged += new System.EventHandler(this.chkColumnUniqueKey_CheckedChanged);
      // 
      // chkColumnPrimaryKey
      // 
      this.chkColumnPrimaryKey.AutoSize = true;
      this.chkColumnPrimaryKey.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.columnsBindingSource, "PrimaryKey", true));
      this.chkColumnPrimaryKey.Location = new System.Drawing.Point(347, 120);
      this.chkColumnPrimaryKey.Name = "chkColumnPrimaryKey";
      this.chkColumnPrimaryKey.Size = new System.Drawing.Size(81, 17);
      this.chkColumnPrimaryKey.TabIndex = 7;
      this.chkColumnPrimaryKey.Text = "Primary Key";
      this.chkColumnPrimaryKey.UseVisualStyleBackColor = true;
      this.chkColumnPrimaryKey.CheckedChanged += new System.EventHandler(this.chkColumnPrimaryKey_CheckedChanged);
      // 
      // chkColumnAutoIncrement
      // 
      this.chkColumnAutoIncrement.AutoSize = true;
      this.chkColumnAutoIncrement.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.columnsBindingSource, "AutoIncrement", true));
      this.chkColumnAutoIncrement.Location = new System.Drawing.Point(347, 97);
      this.chkColumnAutoIncrement.Name = "chkColumnAutoIncrement";
      this.chkColumnAutoIncrement.Size = new System.Drawing.Size(98, 17);
      this.chkColumnAutoIncrement.TabIndex = 5;
      this.chkColumnAutoIncrement.Text = "Auto-Increment";
      this.chkColumnAutoIncrement.UseVisualStyleBackColor = true;
      // 
      // grpDataType
      // 
      this.grpDataType.Controls.Add(this.chkColumnTypeBinary);
      this.grpDataType.Controls.Add(this.numColumnTypeDecimals);
      this.grpDataType.Controls.Add(this.lblColumnTypeDecimals);
      this.grpDataType.Controls.Add(this.chkColumnTypeZeroFill);
      this.grpDataType.Controls.Add(this.chkColumnTypeUnsigned);
      this.grpDataType.Controls.Add(this.lblColumnTypeLength);
      this.grpDataType.Controls.Add(this.cmbColumnType);
      this.grpDataType.Controls.Add(this.numColumnTypeLength);
      this.grpDataType.Controls.Add(this.lblColumnType);
      this.grpDataType.Location = new System.Drawing.Point(20, 48);
      this.grpDataType.Name = "grpDataType";
      this.grpDataType.Size = new System.Drawing.Size(318, 95);
      this.grpDataType.TabIndex = 2;
      this.grpDataType.TabStop = false;
      this.grpDataType.Text = "Data Type Options";
      // 
      // chkColumnTypeBinary
      // 
      this.chkColumnTypeBinary.AutoSize = true;
      this.chkColumnTypeBinary.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.columnsBindingSource, "Binary", true));
      this.chkColumnTypeBinary.Location = new System.Drawing.Point(253, 71);
      this.chkColumnTypeBinary.Name = "chkColumnTypeBinary";
      this.chkColumnTypeBinary.Size = new System.Drawing.Size(55, 17);
      this.chkColumnTypeBinary.TabIndex = 8;
      this.chkColumnTypeBinary.Text = "Binary";
      this.chkColumnTypeBinary.UseVisualStyleBackColor = true;
      // 
      // numColumnTypeDecimals
      // 
      this.numColumnTypeDecimals.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.columnsBindingSource, "Decimals", true));
      this.numColumnTypeDecimals.Location = new System.Drawing.Point(253, 45);
      this.numColumnTypeDecimals.Maximum = new decimal(new int[] {
            53,
            0,
            0,
            0});
      this.numColumnTypeDecimals.Name = "numColumnTypeDecimals";
      this.numColumnTypeDecimals.Size = new System.Drawing.Size(55, 20);
      this.numColumnTypeDecimals.TabIndex = 5;
      // 
      // lblColumnTypeDecimals
      // 
      this.lblColumnTypeDecimals.AutoSize = true;
      this.lblColumnTypeDecimals.Location = new System.Drawing.Point(194, 47);
      this.lblColumnTypeDecimals.Name = "lblColumnTypeDecimals";
      this.lblColumnTypeDecimals.Size = new System.Drawing.Size(53, 13);
      this.lblColumnTypeDecimals.TabIndex = 4;
      this.lblColumnTypeDecimals.Text = "Decimals:";
      // 
      // chkColumnTypeZeroFill
      // 
      this.chkColumnTypeZeroFill.AutoSize = true;
      this.chkColumnTypeZeroFill.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.columnsBindingSource, "ZeroFill", true));
      this.chkColumnTypeZeroFill.Location = new System.Drawing.Point(176, 71);
      this.chkColumnTypeZeroFill.Name = "chkColumnTypeZeroFill";
      this.chkColumnTypeZeroFill.Size = new System.Drawing.Size(63, 17);
      this.chkColumnTypeZeroFill.TabIndex = 7;
      this.chkColumnTypeZeroFill.Text = "Zero Fill";
      this.chkColumnTypeZeroFill.UseVisualStyleBackColor = true;
      // 
      // chkColumnTypeUnsigned
      // 
      this.chkColumnTypeUnsigned.AutoSize = true;
      this.chkColumnTypeUnsigned.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.columnsBindingSource, "Unsigned", true));
      this.chkColumnTypeUnsigned.Location = new System.Drawing.Point(95, 71);
      this.chkColumnTypeUnsigned.Name = "chkColumnTypeUnsigned";
      this.chkColumnTypeUnsigned.Size = new System.Drawing.Size(71, 17);
      this.chkColumnTypeUnsigned.TabIndex = 6;
      this.chkColumnTypeUnsigned.Text = "Unsigned";
      this.chkColumnTypeUnsigned.UseVisualStyleBackColor = true;
      // 
      // lblColumnTypeLength
      // 
      this.lblColumnTypeLength.AutoSize = true;
      this.lblColumnTypeLength.Location = new System.Drawing.Point(46, 47);
      this.lblColumnTypeLength.Name = "lblColumnTypeLength";
      this.lblColumnTypeLength.Size = new System.Drawing.Size(43, 13);
      this.lblColumnTypeLength.TabIndex = 2;
      this.lblColumnTypeLength.Text = "Length:";
      // 
      // cmbColumnType
      // 
      this.cmbColumnType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.cmbColumnType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.columnsBindingSource, "Type", true));
      this.cmbColumnType.FormattingEnabled = true;
      this.cmbColumnType.Location = new System.Drawing.Point(95, 18);
      this.cmbColumnType.Name = "cmbColumnType";
      this.cmbColumnType.Size = new System.Drawing.Size(213, 21);
      this.cmbColumnType.TabIndex = 1;
      this.cmbColumnType.SelectedIndexChanged += new System.EventHandler(this.cmbColumnType_SelectedIndexChanged);
      // 
      // numColumnTypeLength
      // 
      this.numColumnTypeLength.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.columnsBindingSource, "Length", true));
      this.numColumnTypeLength.Location = new System.Drawing.Point(97, 45);
      this.numColumnTypeLength.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
      this.numColumnTypeLength.Name = "numColumnTypeLength";
      this.numColumnTypeLength.Size = new System.Drawing.Size(55, 20);
      this.numColumnTypeLength.TabIndex = 3;
      // 
      // lblColumnType
      // 
      this.lblColumnType.AutoSize = true;
      this.lblColumnType.Location = new System.Drawing.Point(56, 21);
      this.lblColumnType.Name = "lblColumnType";
      this.lblColumnType.Size = new System.Drawing.Size(34, 13);
      this.lblColumnType.TabIndex = 0;
      this.lblColumnType.Text = "Type:";
      // 
      // txtColumnDefaultValue
      // 
      this.txtColumnDefaultValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.columnsBindingSource, "DefaultValue", true));
      this.txtColumnDefaultValue.Location = new System.Drawing.Point(424, 63);
      this.txtColumnDefaultValue.Name = "txtColumnDefaultValue";
      this.txtColumnDefaultValue.Size = new System.Drawing.Size(117, 20);
      this.txtColumnDefaultValue.TabIndex = 4;
      // 
      // lblColumnDefaultValue
      // 
      this.lblColumnDefaultValue.AutoSize = true;
      this.lblColumnDefaultValue.Location = new System.Drawing.Point(344, 66);
      this.lblColumnDefaultValue.Name = "lblColumnDefaultValue";
      this.lblColumnDefaultValue.Size = new System.Drawing.Size(74, 13);
      this.lblColumnDefaultValue.TabIndex = 3;
      this.lblColumnDefaultValue.Text = "Default Value:";
      // 
      // chkColumnNullable
      // 
      this.chkColumnNullable.AutoSize = true;
      this.chkColumnNullable.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.columnsBindingSource, "Nullable", true));
      this.chkColumnNullable.Location = new System.Drawing.Point(460, 95);
      this.chkColumnNullable.Name = "chkColumnNullable";
      this.chkColumnNullable.Size = new System.Drawing.Size(64, 17);
      this.chkColumnNullable.TabIndex = 6;
      this.chkColumnNullable.Text = "Nullable";
      this.chkColumnNullable.UseVisualStyleBackColor = true;
      // 
      // txtColumnName
      // 
      this.txtColumnName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.columnsBindingSource, "Name", true));
      this.txtColumnName.Location = new System.Drawing.Point(115, 22);
      this.txtColumnName.Name = "txtColumnName";
      this.txtColumnName.Size = new System.Drawing.Size(213, 20);
      this.txtColumnName.TabIndex = 1;
      this.txtColumnName.Validating += new System.ComponentModel.CancelEventHandler(this.txtColumnName_Validating);
      // 
      // lblColumnName
      // 
      this.lblColumnName.AutoSize = true;
      this.lblColumnName.Location = new System.Drawing.Point(71, 25);
      this.lblColumnName.Name = "lblColumnName";
      this.lblColumnName.Size = new System.Drawing.Size(38, 13);
      this.lblColumnName.TabIndex = 0;
      this.lblColumnName.Text = "Name:";
      // 
      // grpDataPreview
      // 
      this.grpDataPreview.Controls.Add(this.lblMappedColumns);
      this.grpDataPreview.Controls.Add(this.chkFirstRowHeaders);
      this.grpDataPreview.Controls.Add(this.btnUnmap);
      this.grpDataPreview.Controls.Add(this.chkUseFormattedValues);
      this.grpDataPreview.Controls.Add(this.grdPreviewData);
      this.grpDataPreview.Location = new System.Drawing.Point(12, 284);
      this.grpDataPreview.Name = "grpDataPreview";
      this.grpDataPreview.Size = new System.Drawing.Size(553, 221);
      this.grpDataPreview.TabIndex = 2;
      this.grpDataPreview.TabStop = false;
      this.grpDataPreview.Text = "Data Preview";
      // 
      // lblMappedColumns
      // 
      this.lblMappedColumns.AutoSize = true;
      this.lblMappedColumns.Location = new System.Drawing.Point(434, 16);
      this.lblMappedColumns.Name = "lblMappedColumns";
      this.lblMappedColumns.Size = new System.Drawing.Size(107, 13);
      this.lblMappedColumns.TabIndex = 2;
      this.lblMappedColumns.Text = "Mapped Columns: ??";
      this.lblMappedColumns.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // chkFirstRowHeaders
      // 
      this.chkFirstRowHeaders.AutoSize = true;
      this.chkFirstRowHeaders.Location = new System.Drawing.Point(15, 42);
      this.chkFirstRowHeaders.Name = "chkFirstRowHeaders";
      this.chkFirstRowHeaders.Size = new System.Drawing.Size(157, 17);
      this.chkFirstRowHeaders.TabIndex = 1;
      this.chkFirstRowHeaders.Text = "First Row Contains Headers";
      this.chkFirstRowHeaders.UseVisualStyleBackColor = true;
      this.chkFirstRowHeaders.CheckedChanged += new System.EventHandler(this.chkFirstRowHeaders_CheckedChanged);
      // 
      // btnUnmap
      // 
      this.btnUnmap.Location = new System.Drawing.Point(347, 36);
      this.btnUnmap.Name = "btnUnmap";
      this.btnUnmap.Size = new System.Drawing.Size(194, 23);
      this.btnUnmap.TabIndex = 3;
      this.btnUnmap.Text = "Unmap Column";
      this.btnUnmap.UseVisualStyleBackColor = true;
      this.btnUnmap.Click += new System.EventHandler(this.btnUnmap_Click);
      // 
      // chkUseFormattedValues
      // 
      this.chkUseFormattedValues.AutoSize = true;
      this.chkUseFormattedValues.Location = new System.Drawing.Point(15, 19);
      this.chkUseFormattedValues.Name = "chkUseFormattedValues";
      this.chkUseFormattedValues.Size = new System.Drawing.Size(159, 17);
      this.chkUseFormattedValues.TabIndex = 0;
      this.chkUseFormattedValues.Text = "Use Excel Formatted Values";
      this.chkUseFormattedValues.UseVisualStyleBackColor = true;
      this.chkUseFormattedValues.CheckedChanged += new System.EventHandler(this.chkUseFormattedValues_CheckedChanged);
      // 
      // grdPreviewData
      // 
      this.grdPreviewData.AllowUserToAddRows = false;
      this.grdPreviewData.AllowUserToDeleteRows = false;
      this.grdPreviewData.AllowUserToResizeColumns = false;
      this.grdPreviewData.AllowUserToResizeRows = false;
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
      this.grdPreviewData.Location = new System.Drawing.Point(15, 65);
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
      this.grdPreviewData.Size = new System.Drawing.Size(526, 150);
      this.grdPreviewData.TabIndex = 0;
      this.grdPreviewData.SelectionChanged += new System.EventHandler(this.grdPreviewData_SelectionChanged);
      // 
      // btnExport
      // 
      this.btnExport.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExport.Location = new System.Drawing.Point(397, 511);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new System.Drawing.Size(75, 23);
      this.btnExport.TabIndex = 3;
      this.btnExport.Text = "Export";
      this.btnExport.UseVisualStyleBackColor = true;
      this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(478, 511);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // OldExportDataToTableDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(577, 546);
      this.ControlBox = false;
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnExport);
      this.Controls.Add(this.grpDataPreview);
      this.Controls.Add(this.grpColumnMapping);
      this.Controls.Add(this.grpTargetTable);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.HelpButton = true;
      this.Name = "ExportDataToTableForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Export Data to Table";
      this.grpTargetTable.ResumeLayout(false);
      this.grpTargetTable.PerformLayout();
      this.grpColumnMapping.ResumeLayout(false);
      this.grpColumnMapping.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.columnsBindingSource)).EndInit();
      this.grpDataType.ResumeLayout(false);
      this.grpDataType.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numColumnTypeDecimals)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numColumnTypeLength)).EndInit();
      this.grpDataPreview.ResumeLayout(false);
      this.grpDataPreview.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lblExistingSchema;
    private System.Windows.Forms.RadioButton radExistingTable;
    private System.Windows.Forms.GroupBox grpTargetTable;
    private System.Windows.Forms.TextBox txtNewTable;
    private System.Windows.Forms.CheckBox chkMakeSelectedTable;
    private System.Windows.Forms.CheckBox chkMakeSelectedSchema;
    private System.Windows.Forms.ComboBox cmbExistingTable;
    private System.Windows.Forms.ComboBox cmbExistingSchema;
    private System.Windows.Forms.RadioButton radNewTable;
    private System.Windows.Forms.GroupBox grpColumnMapping;
    private System.Windows.Forms.ComboBox cmbColumnType;
    private System.Windows.Forms.Label lblColumnType;
    private System.Windows.Forms.TextBox txtColumnName;
    private System.Windows.Forms.Label lblColumnName;
    private System.Windows.Forms.GroupBox grpDataType;
    private System.Windows.Forms.NumericUpDown numColumnTypeLength;
    private System.Windows.Forms.TextBox txtColumnDefaultValue;
    private System.Windows.Forms.Label lblColumnDefaultValue;
    private System.Windows.Forms.CheckBox chkColumnNullable;
    private System.Windows.Forms.CheckBox chkColumnTypeBinary;
    private System.Windows.Forms.NumericUpDown numColumnTypeDecimals;
    private System.Windows.Forms.Label lblColumnTypeDecimals;
    private System.Windows.Forms.CheckBox chkColumnTypeZeroFill;
    private System.Windows.Forms.CheckBox chkColumnTypeUnsigned;
    private System.Windows.Forms.Label lblColumnTypeLength;
    private System.Windows.Forms.CheckBox chkColumnAutoIncrement;
    private System.Windows.Forms.ComboBox cmbDBEngine;
    private System.Windows.Forms.Label lblEngine;
    private System.Windows.Forms.GroupBox grpDataPreview;
    private System.Windows.Forms.DataGridView grdPreviewData;
    private System.Windows.Forms.Button btnExport;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.ComboBox cmbColumnName;
    private System.Windows.Forms.CheckBox chkColumnUniqueKey;
    private System.Windows.Forms.CheckBox chkColumnPrimaryKey;
    private System.Windows.Forms.Button btnMap;
    private System.Windows.Forms.BindingSource columnsBindingSource;
    private System.Windows.Forms.CheckBox chkUseFormattedValues;
    private System.Windows.Forms.Button btnUnmap;
    private System.Windows.Forms.Label lblMappedColumns;
    private System.Windows.Forms.CheckBox chkFirstRowHeaders;
  }
}