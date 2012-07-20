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
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppendDataForm));
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.btnAppend = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.lblManuallyAdjustMappingMainSub = new System.Windows.Forms.Label();
      this.chkFirstRowHeaders = new System.Windows.Forms.CheckBox();
      this.grdFromExcelData = new MySQL.ForExcel.PreviewDataGridView();
      this.lblChooseColumnMappingMainSub = new System.Windows.Forms.Label();
      this.lblChooseColumnMappingMain = new System.Windows.Forms.Label();
      this.picChooseColumnMapping = new System.Windows.Forms.PictureBox();
      this.lblManuallyAdjustMappingMain = new System.Windows.Forms.Label();
      this.picManuallyAdjustMapping = new System.Windows.Forms.PictureBox();
      this.lblExportData = new System.Windows.Forms.Label();
      this.picColorMapMapped = new System.Windows.Forms.PictureBox();
      this.lblColorMapMapped = new System.Windows.Forms.Label();
      this.picColorMapUnmapped = new System.Windows.Forms.PictureBox();
      this.lblColorMapUnmapped = new System.Windows.Forms.Label();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.lblMappingMethod = new System.Windows.Forms.Label();
      this.cmbMappingMethod = new System.Windows.Forms.ComboBox();
      this.btnAdvanced = new System.Windows.Forms.Button();
      this.btnStoreMapping = new System.Windows.Forms.Button();
      this.grdToMySQLTable = new MySQL.ForExcel.MultiHeaderDataGridView();
      this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.removeColumnMappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.clearAllMappingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.contentAreaPanel.SuspendLayout();
      this.commandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdFromExcelData)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picChooseColumnMapping)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picManuallyAdjustMapping)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColorMapMapped)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColorMapUnmapped)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.grdToMySQLTable)).BeginInit();
      this.contextMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // contentAreaPanel
      // 
      this.contentAreaPanel.AllowDrop = true;
      this.contentAreaPanel.BackColor = System.Drawing.SystemColors.Window;
      this.contentAreaPanel.Controls.Add(this.grdToMySQLTable);
      this.contentAreaPanel.Controls.Add(this.cmbMappingMethod);
      this.contentAreaPanel.Controls.Add(this.lblMappingMethod);
      this.contentAreaPanel.Controls.Add(this.pictureBox1);
      this.contentAreaPanel.Controls.Add(this.picColorMapMapped);
      this.contentAreaPanel.Controls.Add(this.lblColorMapMapped);
      this.contentAreaPanel.Controls.Add(this.picColorMapUnmapped);
      this.contentAreaPanel.Controls.Add(this.lblColorMapUnmapped);
      this.contentAreaPanel.Controls.Add(this.lblExportData);
      this.contentAreaPanel.Controls.Add(this.lblManuallyAdjustMappingMainSub);
      this.contentAreaPanel.Controls.Add(this.chkFirstRowHeaders);
      this.contentAreaPanel.Controls.Add(this.grdFromExcelData);
      this.contentAreaPanel.Controls.Add(this.lblChooseColumnMappingMainSub);
      this.contentAreaPanel.Controls.Add(this.lblChooseColumnMappingMain);
      this.contentAreaPanel.Controls.Add(this.picChooseColumnMapping);
      this.contentAreaPanel.Controls.Add(this.lblManuallyAdjustMappingMain);
      this.contentAreaPanel.Controls.Add(this.picManuallyAdjustMapping);
      this.contentAreaPanel.Size = new System.Drawing.Size(844, 550);
      this.contentAreaPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.contentAreaPanel_DragDrop);
      this.contentAreaPanel.DragOver += new System.Windows.Forms.DragEventHandler(this.contentAreaPanel_DragOver);
      this.contentAreaPanel.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.contentAreaPanel_QueryContinueDrag);
      // 
      // commandAreaPanel
      // 
      this.commandAreaPanel.Controls.Add(this.btnStoreMapping);
      this.commandAreaPanel.Controls.Add(this.btnAdvanced);
      this.commandAreaPanel.Controls.Add(this.btnAppend);
      this.commandAreaPanel.Controls.Add(this.btnCancel);
      this.commandAreaPanel.Location = new System.Drawing.Point(0, 550);
      this.commandAreaPanel.Size = new System.Drawing.Size(844, 45);
      // 
      // btnAppend
      // 
      this.btnAppend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAppend.Location = new System.Drawing.Point(678, 12);
      this.btnAppend.Name = "btnAppend";
      this.btnAppend.Size = new System.Drawing.Size(75, 23);
      this.btnAppend.TabIndex = 2;
      this.btnAppend.Text = "Append";
      this.btnAppend.UseVisualStyleBackColor = true;
      this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(759, 12);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // lblManuallyAdjustMappingMainSub
      // 
      this.lblManuallyAdjustMappingMainSub.AutoSize = true;
      this.lblManuallyAdjustMappingMainSub.BackColor = System.Drawing.Color.Transparent;
      this.lblManuallyAdjustMappingMainSub.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblManuallyAdjustMappingMainSub.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblManuallyAdjustMappingMainSub.Location = new System.Drawing.Point(470, 73);
      this.lblManuallyAdjustMappingMainSub.Name = "lblManuallyAdjustMappingMainSub";
      this.lblManuallyAdjustMappingMainSub.Size = new System.Drawing.Size(298, 45);
      this.lblManuallyAdjustMappingMainSub.TabIndex = 6;
      this.lblManuallyAdjustMappingMainSub.Text = "Manually change the column mapping if needed. Click\r\na column in the upper table " +
    "with the mouse and drag it\r\nonto a column in the lower table.";
      // 
      // chkFirstRowHeaders
      // 
      this.chkFirstRowHeaders.AutoSize = true;
      this.chkFirstRowHeaders.BackColor = System.Drawing.Color.Transparent;
      this.chkFirstRowHeaders.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkFirstRowHeaders.Location = new System.Drawing.Point(82, 157);
      this.chkFirstRowHeaders.Name = "chkFirstRowHeaders";
      this.chkFirstRowHeaders.Size = new System.Drawing.Size(210, 19);
      this.chkFirstRowHeaders.TabIndex = 7;
      this.chkFirstRowHeaders.Text = "First Row Contains Column Names";
      this.chkFirstRowHeaders.UseVisualStyleBackColor = false;
      this.chkFirstRowHeaders.CheckedChanged += new System.EventHandler(this.chkFirstRowHeaders_CheckedChanged);
      // 
      // grdFromExcelData
      // 
      this.grdFromExcelData.AllowUserToAddRows = false;
      this.grdFromExcelData.AllowUserToDeleteRows = false;
      this.grdFromExcelData.AllowUserToResizeColumns = false;
      this.grdFromExcelData.AllowUserToResizeRows = false;
      this.grdFromExcelData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grdFromExcelData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdFromExcelData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
      this.grdFromExcelData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdFromExcelData.ColumnsMaximumWidth = 200;
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdFromExcelData.DefaultCellStyle = dataGridViewCellStyle4;
      this.grdFromExcelData.Location = new System.Drawing.Point(82, 182);
      this.grdFromExcelData.Name = "grdFromExcelData";
      this.grdFromExcelData.ReadOnly = true;
      this.grdFromExcelData.RowHeadersVisible = false;
      this.grdFromExcelData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.grdFromExcelData.ShowCellErrors = false;
      this.grdFromExcelData.ShowEditingIcon = false;
      this.grdFromExcelData.ShowRowErrors = false;
      this.grdFromExcelData.Size = new System.Drawing.Size(686, 150);
      this.grdFromExcelData.TabIndex = 8;
      this.grdFromExcelData.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grdFromExcelData_DataBindingComplete);
      this.grdFromExcelData.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.grdGiveFeedback);
      this.grdFromExcelData.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.grdQueryContinueDrag);
      this.grdFromExcelData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdMouseDown);
      this.grdFromExcelData.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdMouseMove);
      this.grdFromExcelData.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdMouseUp);
      // 
      // lblChooseColumnMappingMainSub
      // 
      this.lblChooseColumnMappingMainSub.AutoSize = true;
      this.lblChooseColumnMappingMainSub.BackColor = System.Drawing.Color.Transparent;
      this.lblChooseColumnMappingMainSub.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblChooseColumnMappingMainSub.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblChooseColumnMappingMainSub.Location = new System.Drawing.Point(79, 73);
      this.lblChooseColumnMappingMainSub.Name = "lblChooseColumnMappingMainSub";
      this.lblChooseColumnMappingMainSub.Size = new System.Drawing.Size(298, 30);
      this.lblChooseColumnMappingMainSub.TabIndex = 2;
      this.lblChooseColumnMappingMainSub.Text = "Select how the Excel columns should be mapped to the\r\nMySQL table columns.";
      // 
      // lblChooseColumnMappingMain
      // 
      this.lblChooseColumnMappingMain.AutoSize = true;
      this.lblChooseColumnMappingMain.BackColor = System.Drawing.Color.Transparent;
      this.lblChooseColumnMappingMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblChooseColumnMappingMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblChooseColumnMappingMain.Location = new System.Drawing.Point(79, 53);
      this.lblChooseColumnMappingMain.Name = "lblChooseColumnMappingMain";
      this.lblChooseColumnMappingMain.Size = new System.Drawing.Size(221, 17);
      this.lblChooseColumnMappingMain.TabIndex = 1;
      this.lblChooseColumnMappingMain.Text = "1. Choose Column Mapping Method";
      // 
      // picChooseColumnMapping
      // 
      this.picChooseColumnMapping.BackColor = System.Drawing.Color.Transparent;
      this.picChooseColumnMapping.Image = ((System.Drawing.Image)(resources.GetObject("picChooseColumnMapping.Image")));
      this.picChooseColumnMapping.Location = new System.Drawing.Point(41, 59);
      this.picChooseColumnMapping.Name = "picChooseColumnMapping";
      this.picChooseColumnMapping.Size = new System.Drawing.Size(32, 32);
      this.picChooseColumnMapping.TabIndex = 36;
      this.picChooseColumnMapping.TabStop = false;
      // 
      // lblManuallyAdjustMappingMain
      // 
      this.lblManuallyAdjustMappingMain.AutoSize = true;
      this.lblManuallyAdjustMappingMain.BackColor = System.Drawing.Color.Transparent;
      this.lblManuallyAdjustMappingMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblManuallyAdjustMappingMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblManuallyAdjustMappingMain.Location = new System.Drawing.Point(470, 54);
      this.lblManuallyAdjustMappingMain.Name = "lblManuallyAdjustMappingMain";
      this.lblManuallyAdjustMappingMain.Size = new System.Drawing.Size(219, 17);
      this.lblManuallyAdjustMappingMain.TabIndex = 5;
      this.lblManuallyAdjustMappingMain.Text = "2. Manually Adjust Column Mapping";
      // 
      // picManuallyAdjustMapping
      // 
      this.picManuallyAdjustMapping.BackColor = System.Drawing.Color.Transparent;
      this.picManuallyAdjustMapping.Image = ((System.Drawing.Image)(resources.GetObject("picManuallyAdjustMapping.Image")));
      this.picManuallyAdjustMapping.Location = new System.Drawing.Point(432, 60);
      this.picManuallyAdjustMapping.Name = "picManuallyAdjustMapping";
      this.picManuallyAdjustMapping.Size = new System.Drawing.Size(32, 32);
      this.picManuallyAdjustMapping.TabIndex = 30;
      this.picManuallyAdjustMapping.TabStop = false;
      // 
      // lblExportData
      // 
      this.lblExportData.AutoSize = true;
      this.lblExportData.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblExportData.ForeColor = System.Drawing.Color.Navy;
      this.lblExportData.Location = new System.Drawing.Point(17, 17);
      this.lblExportData.Name = "lblExportData";
      this.lblExportData.Size = new System.Drawing.Size(207, 20);
      this.lblExportData.TabIndex = 0;
      this.lblExportData.Text = "Append Data to MySQL Table";
      // 
      // picColorMapMapped
      // 
      this.picColorMapMapped.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.picColorMapMapped.BackColor = System.Drawing.Color.LightGreen;
      this.picColorMapMapped.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.picColorMapMapped.Location = new System.Drawing.Point(229, 516);
      this.picColorMapMapped.Name = "picColorMapMapped";
      this.picColorMapMapped.Size = new System.Drawing.Size(15, 15);
      this.picColorMapMapped.TabIndex = 41;
      this.picColorMapMapped.TabStop = false;
      // 
      // lblColorMapMapped
      // 
      this.lblColorMapMapped.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblColorMapMapped.AutoSize = true;
      this.lblColorMapMapped.BackColor = System.Drawing.Color.Transparent;
      this.lblColorMapMapped.Location = new System.Drawing.Point(244, 516);
      this.lblColorMapMapped.Name = "lblColorMapMapped";
      this.lblColorMapMapped.Size = new System.Drawing.Size(89, 13);
      this.lblColorMapMapped.TabIndex = 11;
      this.lblColorMapMapped.Text = "Mapped Columns";
      // 
      // picColorMapUnmapped
      // 
      this.picColorMapUnmapped.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.picColorMapUnmapped.BackColor = System.Drawing.Color.OrangeRed;
      this.picColorMapUnmapped.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.picColorMapUnmapped.Location = new System.Drawing.Point(82, 516);
      this.picColorMapUnmapped.Name = "picColorMapUnmapped";
      this.picColorMapUnmapped.Size = new System.Drawing.Size(15, 15);
      this.picColorMapUnmapped.TabIndex = 40;
      this.picColorMapUnmapped.TabStop = false;
      // 
      // lblColorMapUnmapped
      // 
      this.lblColorMapUnmapped.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblColorMapUnmapped.AutoSize = true;
      this.lblColorMapUnmapped.BackColor = System.Drawing.Color.Transparent;
      this.lblColorMapUnmapped.Location = new System.Drawing.Point(97, 516);
      this.lblColorMapUnmapped.Name = "lblColorMapUnmapped";
      this.lblColorMapUnmapped.Size = new System.Drawing.Size(102, 13);
      this.lblColorMapUnmapped.TabIndex = 10;
      this.lblColorMapUnmapped.Text = "Unmapped Columns";
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(414, 340);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(17, 11);
      this.pictureBox1.TabIndex = 42;
      this.pictureBox1.TabStop = false;
      // 
      // lblMappingMethod
      // 
      this.lblMappingMethod.AutoSize = true;
      this.lblMappingMethod.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblMappingMethod.Location = new System.Drawing.Point(79, 115);
      this.lblMappingMethod.Name = "lblMappingMethod";
      this.lblMappingMethod.Size = new System.Drawing.Size(103, 15);
      this.lblMappingMethod.TabIndex = 3;
      this.lblMappingMethod.Text = "Mapping Method:";
      // 
      // cmbMappingMethod
      // 
      this.cmbMappingMethod.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.cmbMappingMethod.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.cmbMappingMethod.DropDownWidth = 243;
      this.cmbMappingMethod.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cmbMappingMethod.FormattingEnabled = true;
      this.cmbMappingMethod.Location = new System.Drawing.Point(188, 112);
      this.cmbMappingMethod.Name = "cmbMappingMethod";
      this.cmbMappingMethod.Size = new System.Drawing.Size(189, 23);
      this.cmbMappingMethod.TabIndex = 4;
      this.cmbMappingMethod.SelectedIndexChanged += new System.EventHandler(this.cmbMappingMethod_SelectedIndexChanged);
      // 
      // btnAdvanced
      // 
      this.btnAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnAdvanced.Location = new System.Drawing.Point(12, 12);
      this.btnAdvanced.Name = "btnAdvanced";
      this.btnAdvanced.Size = new System.Drawing.Size(131, 23);
      this.btnAdvanced.TabIndex = 0;
      this.btnAdvanced.Text = "Advanced Options...";
      this.btnAdvanced.UseVisualStyleBackColor = true;
      this.btnAdvanced.Click += new System.EventHandler(this.btnAdvanced_Click);
      // 
      // btnStoreMapping
      // 
      this.btnStoreMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnStoreMapping.Enabled = false;
      this.btnStoreMapping.Location = new System.Drawing.Point(572, 12);
      this.btnStoreMapping.Name = "btnStoreMapping";
      this.btnStoreMapping.Size = new System.Drawing.Size(100, 23);
      this.btnStoreMapping.TabIndex = 1;
      this.btnStoreMapping.Text = "Store Mapping";
      this.btnStoreMapping.UseVisualStyleBackColor = true;
      this.btnStoreMapping.Click += new System.EventHandler(this.btnStoreMapping_Click);
      // 
      // grdToMySQLTable
      // 
      this.grdToMySQLTable.AllowDrop = true;
      this.grdToMySQLTable.AllowUserToAddRows = false;
      this.grdToMySQLTable.AllowUserToDeleteRows = false;
      this.grdToMySQLTable.AllowUserToResizeColumns = false;
      this.grdToMySQLTable.AllowUserToResizeRows = false;
      this.grdToMySQLTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grdToMySQLTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdToMySQLTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.grdToMySQLTable.ColumnHeadersHeight = 46;
      this.grdToMySQLTable.ColumnsMaximumWidth = 200;
      this.grdToMySQLTable.ContextMenuStrip = this.contextMenu;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdToMySQLTable.DefaultCellStyle = dataGridViewCellStyle2;
      this.grdToMySQLTable.Location = new System.Drawing.Point(82, 360);
      this.grdToMySQLTable.MultiSelect = false;
      this.grdToMySQLTable.Name = "grdToMySQLTable";
      this.grdToMySQLTable.ReadOnly = true;
      this.grdToMySQLTable.RowHeadersVisible = false;
      this.grdToMySQLTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.grdToMySQLTable.ShowCellErrors = false;
      this.grdToMySQLTable.ShowEditingIcon = false;
      this.grdToMySQLTable.ShowRowErrors = false;
      this.grdToMySQLTable.Size = new System.Drawing.Size(686, 150);
      this.grdToMySQLTable.TabIndex = 9;
      this.grdToMySQLTable.DragDrop += new System.Windows.Forms.DragEventHandler(this.grdToMySQLTable_DragDrop);
      this.grdToMySQLTable.DragOver += new System.Windows.Forms.DragEventHandler(this.grdToMySQLTable_DragOver);
      this.grdToMySQLTable.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.grdGiveFeedback);
      this.grdToMySQLTable.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.grdQueryContinueDrag);
      this.grdToMySQLTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdMouseDown);
      this.grdToMySQLTable.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdMouseMove);
      this.grdToMySQLTable.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdMouseUp);
      // 
      // contextMenu
      // 
      this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeColumnMappingToolStripMenuItem,
            this.clearAllMappingsToolStripMenuItem});
      this.contextMenu.Name = "contextMenu";
      this.contextMenu.Size = new System.Drawing.Size(215, 70);
      this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_Opening);
      // 
      // removeColumnMappingToolStripMenuItem
      // 
      this.removeColumnMappingToolStripMenuItem.Name = "removeColumnMappingToolStripMenuItem";
      this.removeColumnMappingToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
      this.removeColumnMappingToolStripMenuItem.Text = "Remove Column Mapping";
      this.removeColumnMappingToolStripMenuItem.Click += new System.EventHandler(this.removeColumnMappingToolStripMenuItem_Click);
      // 
      // clearAllMappingsToolStripMenuItem
      // 
      this.clearAllMappingsToolStripMenuItem.Name = "clearAllMappingsToolStripMenuItem";
      this.clearAllMappingsToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
      this.clearAllMappingsToolStripMenuItem.Text = "Clear All Mappings";
      this.clearAllMappingsToolStripMenuItem.Click += new System.EventHandler(this.clearAllMappingsToolStripMenuItem_Click);
      // 
      // AppendDataForm
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(844, 597);
      this.CommandAreaHeight = 45;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MainInstructionLocation = new System.Drawing.Point(11, 16);
      this.MinimumSize = new System.Drawing.Size(860, 635);
      this.Name = "AppendDataForm";
      this.Text = "Append Data";
      this.Controls.SetChildIndex(this.contentAreaPanel, 0);
      this.Controls.SetChildIndex(this.commandAreaPanel, 0);
      this.contentAreaPanel.ResumeLayout(false);
      this.contentAreaPanel.PerformLayout();
      this.commandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.grdFromExcelData)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picChooseColumnMapping)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picManuallyAdjustMapping)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColorMapMapped)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColorMapUnmapped)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.grdToMySQLTable)).EndInit();
      this.contextMenu.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnAppend;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label lblManuallyAdjustMappingMainSub;
    private System.Windows.Forms.CheckBox chkFirstRowHeaders;
    private PreviewDataGridView grdFromExcelData;
    private System.Windows.Forms.Label lblChooseColumnMappingMainSub;
    private System.Windows.Forms.Label lblChooseColumnMappingMain;
    private System.Windows.Forms.PictureBox picChooseColumnMapping;
    private System.Windows.Forms.Label lblManuallyAdjustMappingMain;
    private System.Windows.Forms.PictureBox picManuallyAdjustMapping;
    private System.Windows.Forms.Label lblExportData;
    private System.Windows.Forms.PictureBox picColorMapMapped;
    private System.Windows.Forms.Label lblColorMapMapped;
    private System.Windows.Forms.PictureBox picColorMapUnmapped;
    private System.Windows.Forms.Label lblColorMapUnmapped;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.ComboBox cmbMappingMethod;
    private System.Windows.Forms.Label lblMappingMethod;
    private System.Windows.Forms.Button btnAdvanced;
    private System.Windows.Forms.Button btnStoreMapping;
    private MultiHeaderDataGridView grdToMySQLTable;
    private System.Windows.Forms.ContextMenuStrip contextMenu;
    private System.Windows.Forms.ToolStripMenuItem removeColumnMappingToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem clearAllMappingsToolStripMenuItem;
  }
}