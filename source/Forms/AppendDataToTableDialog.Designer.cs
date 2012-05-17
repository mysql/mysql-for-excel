namespace MySQL.ForExcel
{
  partial class AppendDataToTableDialog
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      this.grpExcelDataPreview = new System.Windows.Forms.GroupBox();
      this.lblMappedColumns = new System.Windows.Forms.Label();
      this.chkUseFormatted = new System.Windows.Forms.CheckBox();
      this.btnRemove = new System.Windows.Forms.Button();
      this.chkFirstRowHeaders = new System.Windows.Forms.CheckBox();
      this.grdPreviewData = new System.Windows.Forms.DataGridView();
      this.grpToTablePreview = new System.Windows.Forms.GroupBox();
      this.lblToTable = new System.Windows.Forms.Label();
      this.btnUnmap = new System.Windows.Forms.Button();
      this.lblRowsCount = new System.Windows.Forms.Label();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnAppend = new System.Windows.Forms.Button();
      this.lblInstructions = new System.Windows.Forms.Label();
      this.btnAutoMap = new System.Windows.Forms.Button();
      this.grdToTable = new MySQL.ForExcel.MultiHeaderDataGridView();
      this.grpExcelDataPreview.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).BeginInit();
      this.grpToTablePreview.SuspendLayout();
      this.SuspendLayout();
      // 
      // grpExcelDataPreview
      // 
      this.grpExcelDataPreview.Controls.Add(this.btnAutoMap);
      this.grpExcelDataPreview.Controls.Add(this.chkUseFormatted);
      this.grpExcelDataPreview.Controls.Add(this.btnRemove);
      this.grpExcelDataPreview.Controls.Add(this.chkFirstRowHeaders);
      this.grpExcelDataPreview.Controls.Add(this.grdPreviewData);
      this.grpExcelDataPreview.Location = new System.Drawing.Point(12, 247);
      this.grpExcelDataPreview.Name = "grpExcelDataPreview";
      this.grpExcelDataPreview.Size = new System.Drawing.Size(770, 200);
      this.grpExcelDataPreview.TabIndex = 2;
      this.grpExcelDataPreview.TabStop = false;
      this.grpExcelDataPreview.Text = "Excel Data Preview";
      // 
      // lblMappedColumns
      // 
      this.lblMappedColumns.AutoSize = true;
      this.lblMappedColumns.Location = new System.Drawing.Point(515, 24);
      this.lblMappedColumns.Name = "lblMappedColumns";
      this.lblMappedColumns.Size = new System.Drawing.Size(101, 13);
      this.lblMappedColumns.TabIndex = 2;
      this.lblMappedColumns.Text = "Mapped Columns: 0";
      // 
      // chkUseFormatted
      // 
      this.chkUseFormatted.AutoSize = true;
      this.chkUseFormatted.Location = new System.Drawing.Point(186, 19);
      this.chkUseFormatted.Name = "chkUseFormatted";
      this.chkUseFormatted.Size = new System.Drawing.Size(130, 17);
      this.chkUseFormatted.TabIndex = 1;
      this.chkUseFormatted.Text = "Use Formatted Values";
      this.chkUseFormatted.UseVisualStyleBackColor = true;
      this.chkUseFormatted.CheckedChanged += new System.EventHandler(this.chkUseFormatted_CheckedChanged);
      // 
      // btnRemove
      // 
      this.btnRemove.Location = new System.Drawing.Point(644, 15);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new System.Drawing.Size(120, 23);
      this.btnRemove.TabIndex = 3;
      this.btnRemove.Text = "Remove Column";
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
      // 
      // chkFirstRowHeaders
      // 
      this.chkFirstRowHeaders.AutoSize = true;
      this.chkFirstRowHeaders.Location = new System.Drawing.Point(6, 19);
      this.chkFirstRowHeaders.Name = "chkFirstRowHeaders";
      this.chkFirstRowHeaders.Size = new System.Drawing.Size(157, 17);
      this.chkFirstRowHeaders.TabIndex = 0;
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
      this.grdPreviewData.TabIndex = 4;
      this.grdPreviewData.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grdPreviewData_DataBindingComplete);
      this.grdPreviewData.SelectionChanged += new System.EventHandler(this.grdPreviewData_SelectionChanged);
      this.grdPreviewData.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.grdPreviewData_GiveFeedback);
      this.grdPreviewData.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.grdPreviewData_QueryContinueDrag);
      this.grdPreviewData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdPreviewData_MouseDown);
      this.grdPreviewData.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdPreviewData_MouseMove);
      this.grdPreviewData.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdPreviewData_MouseUp);
      // 
      // grpToTablePreview
      // 
      this.grpToTablePreview.Controls.Add(this.lblMappedColumns);
      this.grpToTablePreview.Controls.Add(this.lblToTable);
      this.grpToTablePreview.Controls.Add(this.btnUnmap);
      this.grpToTablePreview.Controls.Add(this.lblRowsCount);
      this.grpToTablePreview.Controls.Add(this.grdToTable);
      this.grpToTablePreview.Location = new System.Drawing.Point(12, 36);
      this.grpToTablePreview.Name = "grpToTablePreview";
      this.grpToTablePreview.Size = new System.Drawing.Size(770, 205);
      this.grpToTablePreview.TabIndex = 1;
      this.grpToTablePreview.TabStop = false;
      this.grpToTablePreview.Text = "Table Data Preview";
      // 
      // lblToTable
      // 
      this.lblToTable.AutoSize = true;
      this.lblToTable.Location = new System.Drawing.Point(6, 24);
      this.lblToTable.Name = "lblToTable";
      this.lblToTable.Size = new System.Drawing.Size(68, 13);
      this.lblToTable.TabIndex = 0;
      this.lblToTable.Text = "To Table: ??";
      // 
      // btnUnmap
      // 
      this.btnUnmap.Location = new System.Drawing.Point(644, 19);
      this.btnUnmap.Name = "btnUnmap";
      this.btnUnmap.Size = new System.Drawing.Size(120, 23);
      this.btnUnmap.TabIndex = 3;
      this.btnUnmap.Text = "Unmap Column";
      this.btnUnmap.UseVisualStyleBackColor = true;
      this.btnUnmap.Click += new System.EventHandler(this.btnUnmap_Click);
      // 
      // lblRowsCount
      // 
      this.lblRowsCount.AutoSize = true;
      this.lblRowsCount.Location = new System.Drawing.Point(183, 24);
      this.lblRowsCount.Name = "lblRowsCount";
      this.lblRowsCount.Size = new System.Drawing.Size(110, 13);
      this.lblRowsCount.TabIndex = 1;
      this.lblRowsCount.Text = "Total Rows Count: ??";
      this.lblRowsCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(707, 453);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnAppend
      // 
      this.btnAppend.Location = new System.Drawing.Point(626, 453);
      this.btnAppend.Name = "btnAppend";
      this.btnAppend.Size = new System.Drawing.Size(75, 23);
      this.btnAppend.TabIndex = 3;
      this.btnAppend.Text = "Append";
      this.btnAppend.UseVisualStyleBackColor = true;
      this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
      // 
      // lblInstructions
      // 
      this.lblInstructions.AutoSize = true;
      this.lblInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblInstructions.Location = new System.Drawing.Point(12, 9);
      this.lblInstructions.Name = "lblInstructions";
      this.lblInstructions.Size = new System.Drawing.Size(659, 13);
      this.lblInstructions.TabIndex = 0;
      this.lblInstructions.Text = "Drag columns from the Excel Data Preview grid to the Table Data Preview grid to m" +
    "ap columns for appending data.";
      // 
      // btnAutoMap
      // 
      this.btnAutoMap.Location = new System.Drawing.Point(518, 15);
      this.btnAutoMap.Name = "btnAutoMap";
      this.btnAutoMap.Size = new System.Drawing.Size(120, 23);
      this.btnAutoMap.TabIndex = 2;
      this.btnAutoMap.Text = "Auto-Map All";
      this.btnAutoMap.UseVisualStyleBackColor = true;
      this.btnAutoMap.Click += new System.EventHandler(this.btnAutoMap_Click);
      // 
      // grdToTable
      // 
      this.grdToTable.AllowUserToAddRows = false;
      this.grdToTable.AllowUserToDeleteRows = false;
      this.grdToTable.AllowUserToResizeColumns = false;
      this.grdToTable.AllowUserToResizeRows = false;
      this.grdToTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.grdToTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
      this.grdToTable.DataSource = null;
      this.grdToTable.GridAllowsDrop = true;
      this.grdToTable.Location = new System.Drawing.Point(9, 48);
      this.grdToTable.MultiSelect = false;
      this.grdToTable.Name = "grdToTable";
      this.grdToTable.ReadOnly = true;
      this.grdToTable.RowHeadersVisible = false;
      this.grdToTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
      this.grdToTable.Size = new System.Drawing.Size(755, 150);
      this.grdToTable.TabIndex = 4;
      this.grdToTable.SelectionChanged += new System.EventHandler(this.grdToTable_SelectionChanged);
      this.grdToTable.GridDragOver += new System.Windows.Forms.DragEventHandler(this.grdToTable_GridDragOver);
      this.grdToTable.GridDragDrop += new System.Windows.Forms.DragEventHandler(this.grdToTable_GridDragDrop);
      // 
      // AppendDataToTableDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(794, 488);
      this.ControlBox = false;
      this.Controls.Add(this.lblInstructions);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnAppend);
      this.Controls.Add(this.grpToTablePreview);
      this.Controls.Add(this.grpExcelDataPreview);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "AppendDataToTableDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Append Data to Table";
      this.grpExcelDataPreview.ResumeLayout(false);
      this.grpExcelDataPreview.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).EndInit();
      this.grpToTablePreview.ResumeLayout(false);
      this.grpToTablePreview.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox grpExcelDataPreview;
    private System.Windows.Forms.CheckBox chkUseFormatted;
    private System.Windows.Forms.Button btnRemove;
    private System.Windows.Forms.CheckBox chkFirstRowHeaders;
    private System.Windows.Forms.DataGridView grdPreviewData;
    private System.Windows.Forms.GroupBox grpToTablePreview;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnAppend;
    private System.Windows.Forms.Label lblToTable;
    private System.Windows.Forms.Label lblRowsCount;
    private MultiHeaderDataGridView grdToTable;
    private System.Windows.Forms.Label lblMappedColumns;
    private System.Windows.Forms.Button btnUnmap;
    private System.Windows.Forms.Label lblInstructions;
    private System.Windows.Forms.Button btnAutoMap;
  }
}