namespace MySQL.ExcelAddIn
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
      this.grpDataPreview = new System.Windows.Forms.GroupBox();
      this.chkFirstRowHeaders = new System.Windows.Forms.CheckBox();
      this.grdPreviewData = new System.Windows.Forms.DataGridView();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.grdColumnProperties = new System.Windows.Forms.DataGridView();
      this.lblTableName = new System.Windows.Forms.Label();
      this.txtTableName = new System.Windows.Forms.TextBox();
      this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
      this.btnExport = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.grpDataPreview.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).BeginInit();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdColumnProperties)).BeginInit();
      this.SuspendLayout();
      // 
      // grpDataPreview
      // 
      this.grpDataPreview.Controls.Add(this.chkFirstRowHeaders);
      this.grpDataPreview.Controls.Add(this.grdPreviewData);
      this.grpDataPreview.Location = new System.Drawing.Point(12, 12);
      this.grpDataPreview.Name = "grpDataPreview";
      this.grpDataPreview.Size = new System.Drawing.Size(770, 200);
      this.grpDataPreview.TabIndex = 3;
      this.grpDataPreview.TabStop = false;
      this.grpDataPreview.Text = "Data Preview";
      // 
      // chkFirstRowHeaders
      // 
      this.chkFirstRowHeaders.AutoSize = true;
      this.chkFirstRowHeaders.Location = new System.Drawing.Point(6, 19);
      this.chkFirstRowHeaders.Name = "chkFirstRowHeaders";
      this.chkFirstRowHeaders.Size = new System.Drawing.Size(157, 17);
      this.chkFirstRowHeaders.TabIndex = 1;
      this.chkFirstRowHeaders.Text = "First Row Contains Headers";
      this.chkFirstRowHeaders.UseVisualStyleBackColor = true;
      // 
      // grdPreviewData
      // 
      this.grdPreviewData.AllowUserToAddRows = false;
      this.grdPreviewData.AllowUserToDeleteRows = false;
      this.grdPreviewData.AllowUserToResizeColumns = false;
      this.grdPreviewData.AllowUserToResizeRows = false;
      this.grdPreviewData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.grdPreviewData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
      this.grdPreviewData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdPreviewData.DefaultCellStyle = dataGridViewCellStyle8;
      this.grdPreviewData.Location = new System.Drawing.Point(6, 42);
      this.grdPreviewData.MultiSelect = false;
      this.grdPreviewData.Name = "grdPreviewData";
      this.grdPreviewData.ReadOnly = true;
      dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.grdPreviewData.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
      this.grdPreviewData.RowHeadersVisible = false;
      this.grdPreviewData.Size = new System.Drawing.Size(758, 150);
      this.grdPreviewData.TabIndex = 0;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.propertyGrid1);
      this.groupBox1.Controls.Add(this.txtTableName);
      this.groupBox1.Controls.Add(this.lblTableName);
      this.groupBox1.Controls.Add(this.grdColumnProperties);
      this.groupBox1.Location = new System.Drawing.Point(12, 218);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(770, 199);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "New Table";
      // 
      // grdColumnProperties
      // 
      this.grdColumnProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdColumnProperties.Location = new System.Drawing.Point(9, 50);
      this.grdColumnProperties.Name = "grdColumnProperties";
      this.grdColumnProperties.Size = new System.Drawing.Size(380, 143);
      this.grdColumnProperties.TabIndex = 0;
      // 
      // lblTableName
      // 
      this.lblTableName.AutoSize = true;
      this.lblTableName.Location = new System.Drawing.Point(6, 27);
      this.lblTableName.Name = "lblTableName";
      this.lblTableName.Size = new System.Drawing.Size(68, 13);
      this.lblTableName.TabIndex = 1;
      this.lblTableName.Text = "Table Name:";
      // 
      // txtTableName
      // 
      this.txtTableName.Location = new System.Drawing.Point(80, 24);
      this.txtTableName.Name = "txtTableName";
      this.txtTableName.Size = new System.Drawing.Size(309, 20);
      this.txtTableName.TabIndex = 2;
      // 
      // propertyGrid1
      // 
      this.propertyGrid1.HelpVisible = false;
      this.propertyGrid1.Location = new System.Drawing.Point(395, 24);
      this.propertyGrid1.Name = "propertyGrid1";
      this.propertyGrid1.Size = new System.Drawing.Size(369, 169);
      this.propertyGrid1.TabIndex = 3;
      // 
      // btnExport
      // 
      this.btnExport.Location = new System.Drawing.Point(626, 423);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new System.Drawing.Size(75, 23);
      this.btnExport.TabIndex = 5;
      this.btnExport.Text = "Export";
      this.btnExport.UseVisualStyleBackColor = true;
      // 
      // btnCancel
      // 
      this.btnCancel.Location = new System.Drawing.Point(707, 423);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // ExportDataToTableDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(794, 457);
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
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox grpDataPreview;
    private System.Windows.Forms.CheckBox chkFirstRowHeaders;
    private System.Windows.Forms.DataGridView grdPreviewData;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label lblTableName;
    private System.Windows.Forms.DataGridView grdColumnProperties;
    private System.Windows.Forms.PropertyGrid propertyGrid1;
    private System.Windows.Forms.TextBox txtTableName;
    private System.Windows.Forms.Button btnExport;
    private System.Windows.Forms.Button btnCancel;
  }
}