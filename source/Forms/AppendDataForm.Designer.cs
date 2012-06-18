namespace MySQL.ForExcel
{
  partial class AppendDataForm
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.AppendDataPanel = new System.Windows.Forms.Panel();
      this.btnAutoMap = new System.Windows.Forms.Button();
      this.btnRemove = new System.Windows.Forms.Button();
      this.lblMappedColumnsCount = new System.Windows.Forms.Label();
      this.lblRowsCountNum = new System.Windows.Forms.Label();
      this.lblRowsCount = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.btnUnmap = new System.Windows.Forms.Button();
      this.lblMappedColumns = new System.Windows.Forms.Label();
      this.chkUseFormatted = new System.Windows.Forms.CheckBox();
      this.chkFirstRowHeaders = new System.Windows.Forms.CheckBox();
      this.grdPreviewData = new System.Windows.Forms.DataGridView();
      this.lblAppendFromSub = new System.Windows.Forms.Label();
      this.lblAppendFromMain = new System.Windows.Forms.Label();
      this.picAppendFrom = new System.Windows.Forms.PictureBox();
      this.lblToTableName = new System.Windows.Forms.Label();
      this.lblToTable = new System.Windows.Forms.Label();
      this.picToTable = new System.Windows.Forms.PictureBox();
      this.lblExportData = new System.Windows.Forms.Label();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnAppend = new System.Windows.Forms.Button();
      this.picColorMapMapped = new System.Windows.Forms.PictureBox();
      this.lblColorMapMapped = new System.Windows.Forms.Label();
      this.picColorMapUnmapped = new System.Windows.Forms.PictureBox();
      this.lblColorMapUnmapped = new System.Windows.Forms.Label();
      this.grdToTable = new MySQL.ForExcel.MultiHeaderDataGridView();
      this.AppendDataPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picAppendFrom)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picToTable)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColorMapMapped)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColorMapUnmapped)).BeginInit();
      this.SuspendLayout();
      // 
      // AppendDataPanel
      // 
      this.AppendDataPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.AppendDataPanel.BackColor = System.Drawing.SystemColors.Window;
      this.AppendDataPanel.Controls.Add(this.btnAutoMap);
      this.AppendDataPanel.Controls.Add(this.btnRemove);
      this.AppendDataPanel.Controls.Add(this.lblMappedColumnsCount);
      this.AppendDataPanel.Controls.Add(this.lblRowsCountNum);
      this.AppendDataPanel.Controls.Add(this.lblRowsCount);
      this.AppendDataPanel.Controls.Add(this.label2);
      this.AppendDataPanel.Controls.Add(this.btnUnmap);
      this.AppendDataPanel.Controls.Add(this.lblMappedColumns);
      this.AppendDataPanel.Controls.Add(this.chkUseFormatted);
      this.AppendDataPanel.Controls.Add(this.grdToTable);
      this.AppendDataPanel.Controls.Add(this.chkFirstRowHeaders);
      this.AppendDataPanel.Controls.Add(this.grdPreviewData);
      this.AppendDataPanel.Controls.Add(this.lblAppendFromSub);
      this.AppendDataPanel.Controls.Add(this.lblAppendFromMain);
      this.AppendDataPanel.Controls.Add(this.picAppendFrom);
      this.AppendDataPanel.Controls.Add(this.lblToTableName);
      this.AppendDataPanel.Controls.Add(this.lblToTable);
      this.AppendDataPanel.Controls.Add(this.picToTable);
      this.AppendDataPanel.Controls.Add(this.lblExportData);
      this.AppendDataPanel.Location = new System.Drawing.Point(-1, -2);
      this.AppendDataPanel.Name = "AppendDataPanel";
      this.AppendDataPanel.Size = new System.Drawing.Size(846, 519);
      this.AppendDataPanel.TabIndex = 0;
      // 
      // btnAutoMap
      // 
      this.btnAutoMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAutoMap.Location = new System.Drawing.Point(523, 252);
      this.btnAutoMap.Name = "btnAutoMap";
      this.btnAutoMap.Size = new System.Drawing.Size(120, 23);
      this.btnAutoMap.TabIndex = 26;
      this.btnAutoMap.Text = "Auto-Map All";
      this.btnAutoMap.UseVisualStyleBackColor = true;
      this.btnAutoMap.Click += new System.EventHandler(this.btnAutoMap_Click);
      // 
      // btnRemove
      // 
      this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnRemove.Location = new System.Drawing.Point(649, 252);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new System.Drawing.Size(120, 23);
      this.btnRemove.TabIndex = 27;
      this.btnRemove.Text = "Remove Column";
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
      // 
      // lblMappedColumnsCount
      // 
      this.lblMappedColumnsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.lblMappedColumnsCount.AutoSize = true;
      this.lblMappedColumnsCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblMappedColumnsCount.ForeColor = System.Drawing.Color.Navy;
      this.lblMappedColumnsCount.Location = new System.Drawing.Point(616, 490);
      this.lblMappedColumnsCount.Name = "lblMappedColumnsCount";
      this.lblMappedColumnsCount.Size = new System.Drawing.Size(13, 15);
      this.lblMappedColumnsCount.TabIndex = 25;
      this.lblMappedColumnsCount.Text = "0";
      // 
      // lblRowsCountNum
      // 
      this.lblRowsCountNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblRowsCountNum.AutoSize = true;
      this.lblRowsCountNum.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblRowsCountNum.ForeColor = System.Drawing.Color.Navy;
      this.lblRowsCountNum.Location = new System.Drawing.Point(186, 490);
      this.lblRowsCountNum.Name = "lblRowsCountNum";
      this.lblRowsCountNum.Size = new System.Drawing.Size(13, 15);
      this.lblRowsCountNum.TabIndex = 24;
      this.lblRowsCountNum.Text = "0";
      // 
      // lblRowsCount
      // 
      this.lblRowsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblRowsCount.AutoSize = true;
      this.lblRowsCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblRowsCount.Location = new System.Drawing.Point(80, 490);
      this.lblRowsCount.Name = "lblRowsCount";
      this.lblRowsCount.Size = new System.Drawing.Size(104, 15);
      this.lblRowsCount.TabIndex = 23;
      this.lblRowsCount.Text = "Total Rows Count:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
      this.label2.Location = new System.Drawing.Point(80, 309);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(373, 15);
      this.label2.TabIndex = 22;
      this.label2.Text = "Drop columns into the MySQL Table Data Preview grid below to map.";
      // 
      // btnUnmap
      // 
      this.btnUnmap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnUnmap.Location = new System.Drawing.Point(649, 486);
      this.btnUnmap.Name = "btnUnmap";
      this.btnUnmap.Size = new System.Drawing.Size(120, 23);
      this.btnUnmap.TabIndex = 21;
      this.btnUnmap.Text = "Unmap Column";
      this.btnUnmap.UseVisualStyleBackColor = true;
      this.btnUnmap.Click += new System.EventHandler(this.btnUnmap_Click);
      // 
      // lblMappedColumns
      // 
      this.lblMappedColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.lblMappedColumns.AutoSize = true;
      this.lblMappedColumns.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblMappedColumns.Location = new System.Drawing.Point(505, 490);
      this.lblMappedColumns.Name = "lblMappedColumns";
      this.lblMappedColumns.Size = new System.Drawing.Size(105, 15);
      this.lblMappedColumns.TabIndex = 20;
      this.lblMappedColumns.Text = "Mapped Columns:";
      // 
      // chkUseFormatted
      // 
      this.chkUseFormatted.AutoSize = true;
      this.chkUseFormatted.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkUseFormatted.Location = new System.Drawing.Point(259, 255);
      this.chkUseFormatted.Name = "chkUseFormatted";
      this.chkUseFormatted.Size = new System.Drawing.Size(140, 19);
      this.chkUseFormatted.TabIndex = 2;
      this.chkUseFormatted.Text = "Use Formatted Values";
      this.chkUseFormatted.UseVisualStyleBackColor = true;
      this.chkUseFormatted.CheckedChanged += new System.EventHandler(this.chkUseFormatted_CheckedChanged);
      // 
      // chkFirstRowHeaders
      // 
      this.chkFirstRowHeaders.AutoSize = true;
      this.chkFirstRowHeaders.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkFirstRowHeaders.Location = new System.Drawing.Point(83, 255);
      this.chkFirstRowHeaders.Name = "chkFirstRowHeaders";
      this.chkFirstRowHeaders.Size = new System.Drawing.Size(170, 19);
      this.chkFirstRowHeaders.TabIndex = 1;
      this.chkFirstRowHeaders.Text = "First Row Contains Headers";
      this.chkFirstRowHeaders.UseVisualStyleBackColor = true;
      this.chkFirstRowHeaders.CheckedChanged += new System.EventHandler(this.chkFirstRowHeaders_CheckedChanged);
      // 
      // grdPreviewData
      // 
      this.grdPreviewData.AllowUserToAddRows = false;
      this.grdPreviewData.AllowUserToDeleteRows = false;
      this.grdPreviewData.AllowUserToResizeColumns = false;
      this.grdPreviewData.AllowUserToResizeRows = false;
      this.grdPreviewData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grdPreviewData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.grdPreviewData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.grdPreviewData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdPreviewData.DefaultCellStyle = dataGridViewCellStyle2;
      this.grdPreviewData.Location = new System.Drawing.Point(83, 96);
      this.grdPreviewData.Name = "grdPreviewData";
      this.grdPreviewData.ReadOnly = true;
      this.grdPreviewData.RowHeadersVisible = false;
      this.grdPreviewData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.grdPreviewData.Size = new System.Drawing.Size(686, 150);
      this.grdPreviewData.TabIndex = 8;
      this.grdPreviewData.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grdPreviewData_DataBindingComplete);
      this.grdPreviewData.SelectionChanged += new System.EventHandler(this.grdPreviewData_SelectionChanged);
      this.grdPreviewData.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.grdPreviewData_GiveFeedback);
      this.grdPreviewData.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.grdPreviewData_QueryContinueDrag);
      this.grdPreviewData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdPreviewData_MouseDown);
      this.grdPreviewData.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdPreviewData_MouseMove);
      this.grdPreviewData.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdPreviewData_MouseUp);
      // 
      // lblAppendFromSub
      // 
      this.lblAppendFromSub.AutoSize = true;
      this.lblAppendFromSub.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblAppendFromSub.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblAppendFromSub.Location = new System.Drawing.Point(80, 75);
      this.lblAppendFromSub.Name = "lblAppendFromSub";
      this.lblAppendFromSub.Size = new System.Drawing.Size(292, 15);
      this.lblAppendFromSub.TabIndex = 6;
      this.lblAppendFromSub.Text = "Drag columns from the Excel Data Preview grid below.";
      // 
      // lblAppendFromMain
      // 
      this.lblAppendFromMain.AutoSize = true;
      this.lblAppendFromMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblAppendFromMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblAppendFromMain.Location = new System.Drawing.Point(80, 55);
      this.lblAppendFromMain.Name = "lblAppendFromMain";
      this.lblAppendFromMain.Size = new System.Drawing.Size(199, 17);
      this.lblAppendFromMain.TabIndex = 5;
      this.lblAppendFromMain.Text = "1. Map Columns from Excel Data";
      // 
      // picAppendFrom
      // 
      this.picAppendFrom.Image = global::MySQL.ForExcel.Properties.Resources.Chain_Link_32x32;
      this.picAppendFrom.Location = new System.Drawing.Point(42, 61);
      this.picAppendFrom.Name = "picAppendFrom";
      this.picAppendFrom.Size = new System.Drawing.Size(32, 32);
      this.picAppendFrom.TabIndex = 18;
      this.picAppendFrom.TabStop = false;
      // 
      // lblToTableName
      // 
      this.lblToTableName.AutoSize = true;
      this.lblToTableName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblToTableName.ForeColor = System.Drawing.Color.Navy;
      this.lblToTableName.Location = new System.Drawing.Point(235, 290);
      this.lblToTableName.Name = "lblToTableName";
      this.lblToTableName.Size = new System.Drawing.Size(43, 17);
      this.lblToTableName.TabIndex = 2;
      this.lblToTableName.Text = "Name";
      // 
      // lblToTable
      // 
      this.lblToTable.AutoSize = true;
      this.lblToTable.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblToTable.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblToTable.Location = new System.Drawing.Point(80, 290);
      this.lblToTable.Name = "lblToTable";
      this.lblToTable.Size = new System.Drawing.Size(149, 17);
      this.lblToTable.TabIndex = 1;
      this.lblToTable.Text = "2. Map to MySQL Table:";
      // 
      // picToTable
      // 
      this.picToTable.Image = global::MySQL.ForExcel.Properties.Resources.db_Table_32x32;
      this.picToTable.Location = new System.Drawing.Point(42, 296);
      this.picToTable.Name = "picToTable";
      this.picToTable.Size = new System.Drawing.Size(32, 32);
      this.picToTable.TabIndex = 1;
      this.picToTable.TabStop = false;
      // 
      // lblExportData
      // 
      this.lblExportData.AutoSize = true;
      this.lblExportData.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblExportData.ForeColor = System.Drawing.Color.Navy;
      this.lblExportData.Location = new System.Drawing.Point(18, 18);
      this.lblExportData.Name = "lblExportData";
      this.lblExportData.Size = new System.Drawing.Size(207, 20);
      this.lblExportData.TabIndex = 0;
      this.lblExportData.Text = "Append Data to MySQL Table";
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(757, 526);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnAppend
      // 
      this.btnAppend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAppend.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnAppend.Location = new System.Drawing.Point(676, 526);
      this.btnAppend.Name = "btnAppend";
      this.btnAppend.Size = new System.Drawing.Size(75, 23);
      this.btnAppend.TabIndex = 1;
      this.btnAppend.Text = "Append";
      this.btnAppend.UseVisualStyleBackColor = true;
      this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
      // 
      // picColorMapMapped
      // 
      this.picColorMapMapped.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.picColorMapMapped.BackColor = System.Drawing.Color.LightGreen;
      this.picColorMapMapped.Location = new System.Drawing.Point(156, 526);
      this.picColorMapMapped.Name = "picColorMapMapped";
      this.picColorMapMapped.Size = new System.Drawing.Size(23, 23);
      this.picColorMapMapped.TabIndex = 31;
      this.picColorMapMapped.TabStop = false;
      // 
      // lblColorMapMapped
      // 
      this.lblColorMapMapped.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblColorMapMapped.AutoSize = true;
      this.lblColorMapMapped.Location = new System.Drawing.Point(185, 531);
      this.lblColorMapMapped.Name = "lblColorMapMapped";
      this.lblColorMapMapped.Size = new System.Drawing.Size(93, 13);
      this.lblColorMapMapped.TabIndex = 30;
      this.lblColorMapMapped.Text = "Mapped Column";
      // 
      // picColorMapUnmapped
      // 
      this.picColorMapUnmapped.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.picColorMapUnmapped.BackColor = System.Drawing.Color.OrangeRed;
      this.picColorMapUnmapped.Location = new System.Drawing.Point(8, 526);
      this.picColorMapUnmapped.Name = "picColorMapUnmapped";
      this.picColorMapUnmapped.Size = new System.Drawing.Size(23, 23);
      this.picColorMapUnmapped.TabIndex = 29;
      this.picColorMapUnmapped.TabStop = false;
      // 
      // lblColorMapUnmapped
      // 
      this.lblColorMapUnmapped.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblColorMapUnmapped.AutoSize = true;
      this.lblColorMapUnmapped.Location = new System.Drawing.Point(37, 531);
      this.lblColorMapUnmapped.Name = "lblColorMapUnmapped";
      this.lblColorMapUnmapped.Size = new System.Drawing.Size(107, 13);
      this.lblColorMapUnmapped.TabIndex = 28;
      this.lblColorMapUnmapped.Text = "Unmapped Column";
      // 
      // grdToTable
      // 
      this.grdToTable.AllowUserToAddRows = false;
      this.grdToTable.AllowUserToDeleteRows = false;
      this.grdToTable.AllowUserToResizeColumns = false;
      this.grdToTable.AllowUserToResizeRows = false;
      this.grdToTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grdToTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.grdToTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
      this.grdToTable.DataSource = null;
      this.grdToTable.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.grdToTable.GridAllowsDrop = true;
      this.grdToTable.Location = new System.Drawing.Point(83, 330);
      this.grdToTable.MultiSelect = false;
      this.grdToTable.Name = "grdToTable";
      this.grdToTable.ReadOnly = true;
      this.grdToTable.RowHeadersVisible = false;
      this.grdToTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
      this.grdToTable.Size = new System.Drawing.Size(686, 150);
      this.grdToTable.TabIndex = 19;
      this.grdToTable.SelectionChanged += new System.EventHandler(this.grdToTable_SelectionChanged);
      this.grdToTable.GridDragOver += new System.Windows.Forms.DragEventHandler(this.grdToTable_GridDragOver);
      this.grdToTable.GridDragDrop += new System.Windows.Forms.DragEventHandler(this.grdToTable_GridDragDrop);
      // 
      // AppendDataForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(844, 562);
      this.Controls.Add(this.picColorMapMapped);
      this.Controls.Add(this.btnAppend);
      this.Controls.Add(this.lblColorMapMapped);
      this.Controls.Add(this.picColorMapUnmapped);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.lblColorMapUnmapped);
      this.Controls.Add(this.AppendDataPanel);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(860, 600);
      this.Name = "AppendDataForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Append Data";
      this.AppendDataPanel.ResumeLayout(false);
      this.AppendDataPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picAppendFrom)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picToTable)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColorMapMapped)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColorMapUnmapped)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel AppendDataPanel;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnAppend;
    private System.Windows.Forms.Label lblExportData;
    private System.Windows.Forms.Label lblToTableName;
    private System.Windows.Forms.Label lblToTable;
    private System.Windows.Forms.PictureBox picToTable;
    private System.Windows.Forms.DataGridView grdPreviewData;
    private System.Windows.Forms.Label lblAppendFromSub;
    private System.Windows.Forms.Label lblAppendFromMain;
    private System.Windows.Forms.PictureBox picAppendFrom;
    private System.Windows.Forms.CheckBox chkUseFormatted;
    private System.Windows.Forms.CheckBox chkFirstRowHeaders;
    private MultiHeaderDataGridView grdToTable;
    private System.Windows.Forms.Label lblMappedColumns;
    private System.Windows.Forms.Button btnUnmap;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label lblRowsCount;
    private System.Windows.Forms.Label lblRowsCountNum;
    private System.Windows.Forms.Label lblMappedColumnsCount;
    private System.Windows.Forms.Button btnAutoMap;
    private System.Windows.Forms.Button btnRemove;
    private System.Windows.Forms.PictureBox picColorMapMapped;
    private System.Windows.Forms.Label lblColorMapMapped;
    private System.Windows.Forms.PictureBox picColorMapUnmapped;
    private System.Windows.Forms.Label lblColorMapUnmapped;
  }
}