namespace MySQL.ForExcel
{
  partial class ExportDataToTableDialog
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
      this.grpDataPreview = new System.Windows.Forms.GroupBox();
      this.chkUseFormatted = new System.Windows.Forms.CheckBox();
      this.btnRemove = new System.Windows.Forms.Button();
      this.chkFirstRowHeaders = new System.Windows.Forms.CheckBox();
      this.grdPreviewData = new System.Windows.Forms.DataGridView();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.columnPropertiesGrid = new System.Windows.Forms.PropertyGrid();
      this.txtTableName = new System.Windows.Forms.TextBox();
      this.lblTableName = new System.Windows.Forms.Label();
      this.grdColumnProperties = new System.Windows.Forms.DataGridView();
      this.columnNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dataTypeDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
      this.columnBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.btnExport = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.mySQLTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.grpDataPreview.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).BeginInit();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdColumnProperties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.columnBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.mySQLTableBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // grpDataPreview
      // 
      this.grpDataPreview.Controls.Add(this.chkUseFormatted);
      this.grpDataPreview.Controls.Add(this.btnRemove);
      this.grpDataPreview.Controls.Add(this.chkFirstRowHeaders);
      this.grpDataPreview.Controls.Add(this.grdPreviewData);
      this.grpDataPreview.Location = new System.Drawing.Point(12, 12);
      this.grpDataPreview.Name = "grpDataPreview";
      this.grpDataPreview.Size = new System.Drawing.Size(770, 200);
      this.grpDataPreview.TabIndex = 0;
      this.grpDataPreview.TabStop = false;
      this.grpDataPreview.Text = "Data Preview";
      // 
      // chkUseFormatted
      // 
      this.chkUseFormatted.AutoSize = true;
      this.chkUseFormatted.Location = new System.Drawing.Point(220, 19);
      this.chkUseFormatted.Name = "chkUseFormatted";
      this.chkUseFormatted.Size = new System.Drawing.Size(130, 17);
      this.chkUseFormatted.TabIndex = 1;
      this.chkUseFormatted.Text = "Use Formatted Values";
      this.chkUseFormatted.UseVisualStyleBackColor = true;
      this.chkUseFormatted.CheckedChanged += new System.EventHandler(this.chkUseFormatted_CheckedChanged);
      // 
      // btnRemove
      // 
      this.btnRemove.Location = new System.Drawing.Point(645, 15);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new System.Drawing.Size(119, 23);
      this.btnRemove.TabIndex = 2;
      this.btnRemove.Text = "Remove Column";
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
      // 
      // chkFirstRowHeaders
      // 
      this.chkFirstRowHeaders.AutoSize = true;
      this.chkFirstRowHeaders.Location = new System.Drawing.Point(6, 19);
      this.chkFirstRowHeaders.Name = "chkFirstRowHeaders";
      this.chkFirstRowHeaders.Size = new System.Drawing.Size(188, 17);
      this.chkFirstRowHeaders.TabIndex = 0;
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
      this.grdPreviewData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
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
      this.grdPreviewData.Location = new System.Drawing.Point(6, 42);
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
      this.grdPreviewData.Size = new System.Drawing.Size(758, 150);
      this.grdPreviewData.TabIndex = 3;
      this.grdPreviewData.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grdPreviewData_DataBindingComplete);
      this.grdPreviewData.SelectionChanged += new System.EventHandler(this.grdPreviewData_SelectionChanged);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.columnPropertiesGrid);
      this.groupBox1.Controls.Add(this.txtTableName);
      this.groupBox1.Controls.Add(this.lblTableName);
      this.groupBox1.Controls.Add(this.grdColumnProperties);
      this.groupBox1.Location = new System.Drawing.Point(12, 218);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(770, 321);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "New Table";
      // 
      // columnPropertiesGrid
      // 
      this.columnPropertiesGrid.Location = new System.Drawing.Point(395, 24);
      this.columnPropertiesGrid.Name = "columnPropertiesGrid";
      this.columnPropertiesGrid.Size = new System.Drawing.Size(369, 280);
      this.columnPropertiesGrid.TabIndex = 3;
      // 
      // txtTableName
      // 
      this.txtTableName.Location = new System.Drawing.Point(80, 24);
      this.txtTableName.Name = "txtTableName";
      this.txtTableName.Size = new System.Drawing.Size(309, 20);
      this.txtTableName.TabIndex = 1;
      this.txtTableName.Enter += new System.EventHandler(this.txtTableName_Enter);
      // 
      // lblTableName
      // 
      this.lblTableName.AutoSize = true;
      this.lblTableName.Location = new System.Drawing.Point(6, 27);
      this.lblTableName.Name = "lblTableName";
      this.lblTableName.Size = new System.Drawing.Size(68, 13);
      this.lblTableName.TabIndex = 0;
      this.lblTableName.Text = "Table Name:";
      // 
      // grdColumnProperties
      // 
      this.grdColumnProperties.AllowUserToAddRows = false;
      this.grdColumnProperties.AllowUserToDeleteRows = false;
      this.grdColumnProperties.AllowUserToResizeColumns = false;
      this.grdColumnProperties.AllowUserToResizeRows = false;
      this.grdColumnProperties.AutoGenerateColumns = false;
      this.grdColumnProperties.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.grdColumnProperties.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
      this.grdColumnProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdColumnProperties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnNameDataGridViewTextBoxColumn,
            this.dataTypeDataGridViewComboBoxColumn});
      this.grdColumnProperties.DataSource = this.columnBindingSource;
      dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdColumnProperties.DefaultCellStyle = dataGridViewCellStyle5;
      this.grdColumnProperties.Location = new System.Drawing.Point(9, 50);
      this.grdColumnProperties.MultiSelect = false;
      this.grdColumnProperties.Name = "grdColumnProperties";
      dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.grdColumnProperties.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
      this.grdColumnProperties.Size = new System.Drawing.Size(380, 254);
      this.grdColumnProperties.TabIndex = 2;
      this.grdColumnProperties.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdColumnProperties_CellValueChanged);
      this.grdColumnProperties.CurrentCellDirtyStateChanged += new System.EventHandler(this.grdColumnProperties_CurrentCellDirtyStateChanged);
      // 
      // columnNameDataGridViewTextBoxColumn
      // 
      this.columnNameDataGridViewTextBoxColumn.DataPropertyName = "ColumnName";
      this.columnNameDataGridViewTextBoxColumn.HeaderText = "ColumnName";
      this.columnNameDataGridViewTextBoxColumn.Name = "columnNameDataGridViewTextBoxColumn";
      // 
      // dataTypeDataGridViewComboBoxColumn
      // 
      this.dataTypeDataGridViewComboBoxColumn.DataPropertyName = "DataType";
      this.dataTypeDataGridViewComboBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
      this.dataTypeDataGridViewComboBoxColumn.HeaderText = "Data Type";
      this.dataTypeDataGridViewComboBoxColumn.Name = "dataTypeDataGridViewComboBoxColumn";
      this.dataTypeDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.dataTypeDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
      // 
      // columnBindingSource
      // 
      this.columnBindingSource.DataSource = typeof(MySQL.ForExcel.MySQLColumn);
      this.columnBindingSource.CurrentChanged += new System.EventHandler(this.columnBindingSource_CurrentChanged);
      // 
      // btnExport
      // 
      this.btnExport.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExport.Location = new System.Drawing.Point(626, 545);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new System.Drawing.Size(75, 23);
      this.btnExport.TabIndex = 2;
      this.btnExport.Text = "Export";
      this.btnExport.UseVisualStyleBackColor = true;
      this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(707, 545);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // mySQLTableBindingSource
      // 
      this.mySQLTableBindingSource.DataSource = typeof(MySQL.ForExcel.MySQLTable);
      // 
      // ExportDataToTableDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(794, 582);
      this.ControlBox = false;
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnExport);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.grpDataPreview);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "ExportDataToTableDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Export Data to Table";
      this.grpDataPreview.ResumeLayout(false);
      this.grpDataPreview.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdColumnProperties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.columnBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.mySQLTableBindingSource)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox grpDataPreview;
    private System.Windows.Forms.CheckBox chkFirstRowHeaders;
    private System.Windows.Forms.DataGridView grdPreviewData;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label lblTableName;
    private System.Windows.Forms.DataGridView grdColumnProperties;
    private System.Windows.Forms.PropertyGrid columnPropertiesGrid;
    private System.Windows.Forms.TextBox txtTableName;
    private System.Windows.Forms.Button btnExport;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.BindingSource columnBindingSource;
    private System.Windows.Forms.BindingSource mySQLTableBindingSource;
    private System.Windows.Forms.Button btnRemove;
    private System.Windows.Forms.CheckBox chkUseFormatted;
    private System.Windows.Forms.DataGridViewTextBoxColumn columnNameDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewComboBoxColumn dataTypeDataGridViewComboBoxColumn;
  }
}