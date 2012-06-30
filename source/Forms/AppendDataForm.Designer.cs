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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
      this.picColorMapMapped = new System.Windows.Forms.PictureBox();
      this.btnAppend = new System.Windows.Forms.Button();
      this.lblColorMapMapped = new System.Windows.Forms.Label();
      this.picColorMapUnmapped = new System.Windows.Forms.PictureBox();
      this.btnCancel = new System.Windows.Forms.Button();
      this.lblColorMapUnmapped = new System.Windows.Forms.Label();
      this.btnAutoMap = new System.Windows.Forms.Button();
      this.btnRemove = new System.Windows.Forms.Button();
      this.lblMappedColumnsCount = new System.Windows.Forms.Label();
      this.lblRowsCountNum = new System.Windows.Forms.Label();
      this.lblRowsCount = new System.Windows.Forms.Label();
      this.lblToTableSub = new System.Windows.Forms.Label();
      this.btnUnmap = new System.Windows.Forms.Button();
      this.lblMappedColumns = new System.Windows.Forms.Label();
      this.chkUseFormatted = new System.Windows.Forms.CheckBox();
      this.grdToTable = new MySQL.ForExcel.MultiHeaderDataGridView();
      this.chkFirstRowHeaders = new System.Windows.Forms.CheckBox();
      this.grdPreviewData = new System.Windows.Forms.DataGridView();
      this.lblAppendFromSub = new System.Windows.Forms.Label();
      this.lblAppendFromMain = new System.Windows.Forms.Label();
      this.picAppendFrom = new System.Windows.Forms.PictureBox();
      this.lblToTableName = new System.Windows.Forms.Label();
      this.lblToTable = new System.Windows.Forms.Label();
      this.picToTable = new System.Windows.Forms.PictureBox();
      this.contentAreaPanel.SuspendLayout();
      this.commandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picColorMapMapped)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColorMapUnmapped)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picAppendFrom)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picToTable)).BeginInit();
      this.SuspendLayout();
      // 
      // contentAreaPanel
      // 
      this.contentAreaPanel.Controls.Add(this.btnAutoMap);
      this.contentAreaPanel.Controls.Add(this.btnRemove);
      this.contentAreaPanel.Controls.Add(this.lblMappedColumnsCount);
      this.contentAreaPanel.Controls.Add(this.lblRowsCountNum);
      this.contentAreaPanel.Controls.Add(this.lblRowsCount);
      this.contentAreaPanel.Controls.Add(this.lblToTableSub);
      this.contentAreaPanel.Controls.Add(this.btnUnmap);
      this.contentAreaPanel.Controls.Add(this.lblMappedColumns);
      this.contentAreaPanel.Controls.Add(this.chkUseFormatted);
      this.contentAreaPanel.Controls.Add(this.grdToTable);
      this.contentAreaPanel.Controls.Add(this.chkFirstRowHeaders);
      this.contentAreaPanel.Controls.Add(this.grdPreviewData);
      this.contentAreaPanel.Controls.Add(this.lblAppendFromSub);
      this.contentAreaPanel.Controls.Add(this.lblAppendFromMain);
      this.contentAreaPanel.Controls.Add(this.picAppendFrom);
      this.contentAreaPanel.Controls.Add(this.lblToTableName);
      this.contentAreaPanel.Controls.Add(this.lblToTable);
      this.contentAreaPanel.Controls.Add(this.picToTable);
      this.contentAreaPanel.Size = new System.Drawing.Size(844, 516);
      // 
      // commandAreaPanel
      // 
      this.commandAreaPanel.Controls.Add(this.picColorMapMapped);
      this.commandAreaPanel.Controls.Add(this.btnAppend);
      this.commandAreaPanel.Controls.Add(this.lblColorMapMapped);
      this.commandAreaPanel.Controls.Add(this.picColorMapUnmapped);
      this.commandAreaPanel.Controls.Add(this.btnCancel);
      this.commandAreaPanel.Controls.Add(this.lblColorMapUnmapped);
      this.commandAreaPanel.Location = new System.Drawing.Point(0, 517);
      this.commandAreaPanel.Size = new System.Drawing.Size(844, 44);
      // 
      // picColorMapMapped
      // 
      this.picColorMapMapped.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.picColorMapMapped.BackColor = System.Drawing.Color.LightGreen;
      this.picColorMapMapped.Location = new System.Drawing.Point(158, 11);
      this.picColorMapMapped.Name = "picColorMapMapped";
      this.picColorMapMapped.Size = new System.Drawing.Size(23, 23);
      this.picColorMapMapped.TabIndex = 37;
      this.picColorMapMapped.TabStop = false;
      // 
      // btnAppend
      // 
      this.btnAppend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAppend.Location = new System.Drawing.Point(678, 11);
      this.btnAppend.Name = "btnAppend";
      this.btnAppend.Size = new System.Drawing.Size(75, 23);
      this.btnAppend.TabIndex = 2;
      this.btnAppend.Text = "Append";
      this.btnAppend.UseVisualStyleBackColor = true;
      this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
      // 
      // lblColorMapMapped
      // 
      this.lblColorMapMapped.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblColorMapMapped.AutoSize = true;
      this.lblColorMapMapped.BackColor = System.Drawing.Color.Transparent;
      this.lblColorMapMapped.Location = new System.Drawing.Point(187, 16);
      this.lblColorMapMapped.Name = "lblColorMapMapped";
      this.lblColorMapMapped.Size = new System.Drawing.Size(84, 13);
      this.lblColorMapMapped.TabIndex = 1;
      this.lblColorMapMapped.Text = "Mapped Column";
      // 
      // picColorMapUnmapped
      // 
      this.picColorMapUnmapped.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.picColorMapUnmapped.BackColor = System.Drawing.Color.OrangeRed;
      this.picColorMapUnmapped.Location = new System.Drawing.Point(10, 11);
      this.picColorMapUnmapped.Name = "picColorMapUnmapped";
      this.picColorMapUnmapped.Size = new System.Drawing.Size(23, 23);
      this.picColorMapUnmapped.TabIndex = 35;
      this.picColorMapUnmapped.TabStop = false;
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(759, 11);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // lblColorMapUnmapped
      // 
      this.lblColorMapUnmapped.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblColorMapUnmapped.AutoSize = true;
      this.lblColorMapUnmapped.BackColor = System.Drawing.Color.Transparent;
      this.lblColorMapUnmapped.Location = new System.Drawing.Point(39, 16);
      this.lblColorMapUnmapped.Name = "lblColorMapUnmapped";
      this.lblColorMapUnmapped.Size = new System.Drawing.Size(97, 13);
      this.lblColorMapUnmapped.TabIndex = 0;
      this.lblColorMapUnmapped.Text = "Unmapped Column";
      // 
      // btnAutoMap
      // 
      this.btnAutoMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAutoMap.Location = new System.Drawing.Point(522, 250);
      this.btnAutoMap.Name = "btnAutoMap";
      this.btnAutoMap.Size = new System.Drawing.Size(120, 23);
      this.btnAutoMap.TabIndex = 5;
      this.btnAutoMap.Text = "Auto-Map All";
      this.btnAutoMap.UseVisualStyleBackColor = true;
      this.btnAutoMap.Click += new System.EventHandler(this.btnAutoMap_Click);
      // 
      // btnRemove
      // 
      this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnRemove.Location = new System.Drawing.Point(648, 250);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new System.Drawing.Size(120, 23);
      this.btnRemove.TabIndex = 6;
      this.btnRemove.Text = "Remove Column";
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
      // 
      // lblMappedColumnsCount
      // 
      this.lblMappedColumnsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.lblMappedColumnsCount.AutoSize = true;
      this.lblMappedColumnsCount.BackColor = System.Drawing.Color.Transparent;
      this.lblMappedColumnsCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblMappedColumnsCount.ForeColor = System.Drawing.Color.Navy;
      this.lblMappedColumnsCount.Location = new System.Drawing.Point(615, 488);
      this.lblMappedColumnsCount.Name = "lblMappedColumnsCount";
      this.lblMappedColumnsCount.Size = new System.Drawing.Size(13, 15);
      this.lblMappedColumnsCount.TabIndex = 14;
      this.lblMappedColumnsCount.Text = "0";
      // 
      // lblRowsCountNum
      // 
      this.lblRowsCountNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblRowsCountNum.AutoSize = true;
      this.lblRowsCountNum.BackColor = System.Drawing.Color.Transparent;
      this.lblRowsCountNum.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblRowsCountNum.ForeColor = System.Drawing.Color.Navy;
      this.lblRowsCountNum.Location = new System.Drawing.Point(185, 488);
      this.lblRowsCountNum.Name = "lblRowsCountNum";
      this.lblRowsCountNum.Size = new System.Drawing.Size(13, 15);
      this.lblRowsCountNum.TabIndex = 12;
      this.lblRowsCountNum.Text = "0";
      // 
      // lblRowsCount
      // 
      this.lblRowsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblRowsCount.AutoSize = true;
      this.lblRowsCount.BackColor = System.Drawing.Color.Transparent;
      this.lblRowsCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblRowsCount.Location = new System.Drawing.Point(79, 488);
      this.lblRowsCount.Name = "lblRowsCount";
      this.lblRowsCount.Size = new System.Drawing.Size(104, 15);
      this.lblRowsCount.TabIndex = 11;
      this.lblRowsCount.Text = "Total Rows Count:";
      // 
      // lblToTableSub
      // 
      this.lblToTableSub.AutoSize = true;
      this.lblToTableSub.BackColor = System.Drawing.Color.Transparent;
      this.lblToTableSub.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblToTableSub.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblToTableSub.Location = new System.Drawing.Point(79, 307);
      this.lblToTableSub.Name = "lblToTableSub";
      this.lblToTableSub.Size = new System.Drawing.Size(373, 15);
      this.lblToTableSub.TabIndex = 9;
      this.lblToTableSub.Text = "Drop columns into the MySQL Table Data Preview grid below to map.";
      // 
      // btnUnmap
      // 
      this.btnUnmap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnUnmap.Location = new System.Drawing.Point(648, 484);
      this.btnUnmap.Name = "btnUnmap";
      this.btnUnmap.Size = new System.Drawing.Size(120, 23);
      this.btnUnmap.TabIndex = 15;
      this.btnUnmap.Text = "Unmap Column";
      this.btnUnmap.UseVisualStyleBackColor = true;
      this.btnUnmap.Click += new System.EventHandler(this.btnUnmap_Click);
      // 
      // lblMappedColumns
      // 
      this.lblMappedColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.lblMappedColumns.AutoSize = true;
      this.lblMappedColumns.BackColor = System.Drawing.Color.Transparent;
      this.lblMappedColumns.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblMappedColumns.Location = new System.Drawing.Point(504, 488);
      this.lblMappedColumns.Name = "lblMappedColumns";
      this.lblMappedColumns.Size = new System.Drawing.Size(105, 15);
      this.lblMappedColumns.TabIndex = 13;
      this.lblMappedColumns.Text = "Mapped Columns:";
      // 
      // chkUseFormatted
      // 
      this.chkUseFormatted.AutoSize = true;
      this.chkUseFormatted.BackColor = System.Drawing.Color.Transparent;
      this.chkUseFormatted.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkUseFormatted.Location = new System.Drawing.Point(258, 253);
      this.chkUseFormatted.Name = "chkUseFormatted";
      this.chkUseFormatted.Size = new System.Drawing.Size(140, 19);
      this.chkUseFormatted.TabIndex = 4;
      this.chkUseFormatted.Text = "Use Formatted Values";
      this.chkUseFormatted.UseVisualStyleBackColor = false;
      this.chkUseFormatted.CheckedChanged += new System.EventHandler(this.chkUseFormatted_CheckedChanged);
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
      this.grdToTable.Location = new System.Drawing.Point(82, 328);
      this.grdToTable.MultiSelect = false;
      this.grdToTable.Name = "grdToTable";
      this.grdToTable.ReadOnly = true;
      this.grdToTable.RowHeadersVisible = false;
      this.grdToTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
      this.grdToTable.Size = new System.Drawing.Size(686, 150);
      this.grdToTable.TabIndex = 10;
      this.grdToTable.SelectionChanged += new System.EventHandler(this.grdToTable_SelectionChanged);
      this.grdToTable.GridDragOver += new System.Windows.Forms.DragEventHandler(this.grdToTable_GridDragOver);
      this.grdToTable.GridDragDrop += new System.Windows.Forms.DragEventHandler(this.grdToTable_GridDragDrop);
      // 
      // chkFirstRowHeaders
      // 
      this.chkFirstRowHeaders.AutoSize = true;
      this.chkFirstRowHeaders.BackColor = System.Drawing.Color.Transparent;
      this.chkFirstRowHeaders.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkFirstRowHeaders.Location = new System.Drawing.Point(82, 253);
      this.chkFirstRowHeaders.Name = "chkFirstRowHeaders";
      this.chkFirstRowHeaders.Size = new System.Drawing.Size(170, 19);
      this.chkFirstRowHeaders.TabIndex = 3;
      this.chkFirstRowHeaders.Text = "First Row Contains Headers";
      this.chkFirstRowHeaders.UseVisualStyleBackColor = false;
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
      dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
      dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.grdPreviewData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
      this.grdPreviewData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdPreviewData.DefaultCellStyle = dataGridViewCellStyle8;
      this.grdPreviewData.Location = new System.Drawing.Point(82, 94);
      this.grdPreviewData.Name = "grdPreviewData";
      this.grdPreviewData.ReadOnly = true;
      this.grdPreviewData.RowHeadersVisible = false;
      this.grdPreviewData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.grdPreviewData.Size = new System.Drawing.Size(686, 150);
      this.grdPreviewData.TabIndex = 2;
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
      this.lblAppendFromSub.BackColor = System.Drawing.Color.Transparent;
      this.lblAppendFromSub.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblAppendFromSub.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblAppendFromSub.Location = new System.Drawing.Point(79, 73);
      this.lblAppendFromSub.Name = "lblAppendFromSub";
      this.lblAppendFromSub.Size = new System.Drawing.Size(292, 15);
      this.lblAppendFromSub.TabIndex = 1;
      this.lblAppendFromSub.Text = "Drag columns from the Excel Data Preview grid below.";
      // 
      // lblAppendFromMain
      // 
      this.lblAppendFromMain.AutoSize = true;
      this.lblAppendFromMain.BackColor = System.Drawing.Color.Transparent;
      this.lblAppendFromMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblAppendFromMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblAppendFromMain.Location = new System.Drawing.Point(79, 53);
      this.lblAppendFromMain.Name = "lblAppendFromMain";
      this.lblAppendFromMain.Size = new System.Drawing.Size(199, 17);
      this.lblAppendFromMain.TabIndex = 0;
      this.lblAppendFromMain.Text = "1. Map Columns from Excel Data";
      // 
      // picAppendFrom
      // 
      this.picAppendFrom.BackColor = System.Drawing.Color.Transparent;
      this.picAppendFrom.Image = global::MySQL.ForExcel.Properties.Resources.Chain_Link_32x32;
      this.picAppendFrom.Location = new System.Drawing.Point(41, 59);
      this.picAppendFrom.Name = "picAppendFrom";
      this.picAppendFrom.Size = new System.Drawing.Size(32, 32);
      this.picAppendFrom.TabIndex = 36;
      this.picAppendFrom.TabStop = false;
      // 
      // lblToTableName
      // 
      this.lblToTableName.AutoSize = true;
      this.lblToTableName.BackColor = System.Drawing.Color.Transparent;
      this.lblToTableName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblToTableName.ForeColor = System.Drawing.Color.Navy;
      this.lblToTableName.Location = new System.Drawing.Point(234, 288);
      this.lblToTableName.Name = "lblToTableName";
      this.lblToTableName.Size = new System.Drawing.Size(43, 17);
      this.lblToTableName.TabIndex = 8;
      this.lblToTableName.Text = "Name";
      // 
      // lblToTable
      // 
      this.lblToTable.AutoSize = true;
      this.lblToTable.BackColor = System.Drawing.Color.Transparent;
      this.lblToTable.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblToTable.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblToTable.Location = new System.Drawing.Point(79, 288);
      this.lblToTable.Name = "lblToTable";
      this.lblToTable.Size = new System.Drawing.Size(149, 17);
      this.lblToTable.TabIndex = 7;
      this.lblToTable.Text = "2. Map to MySQL Table:";
      // 
      // picToTable
      // 
      this.picToTable.BackColor = System.Drawing.Color.Transparent;
      this.picToTable.Image = global::MySQL.ForExcel.Properties.Resources.db_Table_32x32;
      this.picToTable.Location = new System.Drawing.Point(41, 294);
      this.picToTable.Name = "picToTable";
      this.picToTable.Size = new System.Drawing.Size(32, 32);
      this.picToTable.TabIndex = 30;
      this.picToTable.TabStop = false;
      // 
      // AppendDataForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(844, 562);
      this.CommandAreaHeight = 44;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MainInstruction = "Append Data to MySQL Table";
      this.MainInstructionLocation = new System.Drawing.Point(11, 16);
      this.MinimumSize = new System.Drawing.Size(860, 600);
      this.Name = "AppendDataForm";
      this.Text = "Append Data";
      this.contentAreaPanel.ResumeLayout(false);
      this.contentAreaPanel.PerformLayout();
      this.commandAreaPanel.ResumeLayout(false);
      this.commandAreaPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picColorMapMapped)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColorMapUnmapped)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picAppendFrom)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picToTable)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox picColorMapMapped;
    private System.Windows.Forms.Button btnAppend;
    private System.Windows.Forms.Label lblColorMapMapped;
    private System.Windows.Forms.PictureBox picColorMapUnmapped;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label lblColorMapUnmapped;
    private System.Windows.Forms.Button btnAutoMap;
    private System.Windows.Forms.Button btnRemove;
    private System.Windows.Forms.Label lblMappedColumnsCount;
    private System.Windows.Forms.Label lblRowsCountNum;
    private System.Windows.Forms.Label lblRowsCount;
    private System.Windows.Forms.Label lblToTableSub;
    private System.Windows.Forms.Button btnUnmap;
    private System.Windows.Forms.Label lblMappedColumns;
    private System.Windows.Forms.CheckBox chkUseFormatted;
    private MultiHeaderDataGridView grdToTable;
    private System.Windows.Forms.CheckBox chkFirstRowHeaders;
    private System.Windows.Forms.DataGridView grdPreviewData;
    private System.Windows.Forms.Label lblAppendFromSub;
    private System.Windows.Forms.Label lblAppendFromMain;
    private System.Windows.Forms.PictureBox picAppendFrom;
    private System.Windows.Forms.Label lblToTableName;
    private System.Windows.Forms.Label lblToTable;
    private System.Windows.Forms.PictureBox picToTable;
  }
}