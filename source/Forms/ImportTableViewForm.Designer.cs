namespace MySQL.ForExcel
{
  partial class ImportTableViewForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportTableViewForm));
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      this.fromImageList = new System.Windows.Forms.ImageList(this.components);
      this.label1 = new System.Windows.Forms.Label();
      this.lblRowsCountSub = new System.Windows.Forms.Label();
      this.grpOptions = new System.Windows.Forms.GroupBox();
      this.btnSelectAll = new System.Windows.Forms.Button();
      this.numRowsToReturn = new System.Windows.Forms.NumericUpDown();
      this.lblRowsToReturn = new System.Windows.Forms.Label();
      this.numFromRow = new System.Windows.Forms.NumericUpDown();
      this.lblFromRow = new System.Windows.Forms.Label();
      this.chkLimitRows = new System.Windows.Forms.CheckBox();
      this.chkIncludeHeaders = new System.Windows.Forms.CheckBox();
      this.lblOptionsWarning = new System.Windows.Forms.Label();
      this.picOptionsWarning = new System.Windows.Forms.PictureBox();
      this.grdPreviewData = new System.Windows.Forms.DataGridView();
      this.lblPickColumnsSub = new System.Windows.Forms.Label();
      this.lblPickColumnsMain = new System.Windows.Forms.Label();
      this.picColumnOptions = new System.Windows.Forms.PictureBox();
      this.lblRowsCountMain = new System.Windows.Forms.Label();
      this.lblFromSub = new System.Windows.Forms.Label();
      this.lblFromMain = new System.Windows.Forms.Label();
      this.picFrom = new System.Windows.Forms.PictureBox();
      this.btnImport = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.contentAreaPanel.SuspendLayout();
      this.commandAreaPanel.SuspendLayout();
      this.grpOptions.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numRowsToReturn)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numFromRow)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picOptionsWarning)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColumnOptions)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picFrom)).BeginInit();
      this.SuspendLayout();
      // 
      // contentAreaPanel
      // 
      this.contentAreaPanel.Controls.Add(this.label1);
      this.contentAreaPanel.Controls.Add(this.lblRowsCountSub);
      this.contentAreaPanel.Controls.Add(this.grpOptions);
      this.contentAreaPanel.Controls.Add(this.grdPreviewData);
      this.contentAreaPanel.Controls.Add(this.lblPickColumnsSub);
      this.contentAreaPanel.Controls.Add(this.lblPickColumnsMain);
      this.contentAreaPanel.Controls.Add(this.picColumnOptions);
      this.contentAreaPanel.Controls.Add(this.lblRowsCountMain);
      this.contentAreaPanel.Controls.Add(this.lblFromSub);
      this.contentAreaPanel.Controls.Add(this.lblFromMain);
      this.contentAreaPanel.Controls.Add(this.picFrom);
      this.contentAreaPanel.Size = new System.Drawing.Size(844, 506);
      // 
      // commandAreaPanel
      // 
      this.commandAreaPanel.Controls.Add(this.btnImport);
      this.commandAreaPanel.Controls.Add(this.btnCancel);
      this.commandAreaPanel.Location = new System.Drawing.Point(0, 507);
      this.commandAreaPanel.Size = new System.Drawing.Size(844, 44);
      // 
      // fromImageList
      // 
      this.fromImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("fromImageList.ImageStream")));
      this.fromImageList.TransparentColor = System.Drawing.Color.Transparent;
      this.fromImageList.Images.SetKeyName(0, "db.Table.32x32.png");
      this.fromImageList.Images.SetKeyName(1, "db.View.32x32.png");
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.BackColor = System.Drawing.Color.Transparent;
      this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
      this.label1.Location = new System.Drawing.Point(79, 102);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(319, 15);
      this.label1.TabIndex = 6;
      this.label1.Text = "This is a small subset of the data for preview purposes only.";
      // 
      // lblRowsCountSub
      // 
      this.lblRowsCountSub.AutoSize = true;
      this.lblRowsCountSub.BackColor = System.Drawing.Color.Transparent;
      this.lblRowsCountSub.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblRowsCountSub.ForeColor = System.Drawing.Color.Navy;
      this.lblRowsCountSub.Location = new System.Drawing.Point(198, 71);
      this.lblRowsCountSub.Name = "lblRowsCountSub";
      this.lblRowsCountSub.Size = new System.Drawing.Size(15, 17);
      this.lblRowsCountSub.TabIndex = 3;
      this.lblRowsCountSub.Text = "0";
      // 
      // grpOptions
      // 
      this.grpOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grpOptions.BackColor = System.Drawing.Color.Transparent;
      this.grpOptions.Controls.Add(this.btnSelectAll);
      this.grpOptions.Controls.Add(this.numRowsToReturn);
      this.grpOptions.Controls.Add(this.lblRowsToReturn);
      this.grpOptions.Controls.Add(this.numFromRow);
      this.grpOptions.Controls.Add(this.lblFromRow);
      this.grpOptions.Controls.Add(this.chkLimitRows);
      this.grpOptions.Controls.Add(this.chkIncludeHeaders);
      this.grpOptions.Controls.Add(this.lblOptionsWarning);
      this.grpOptions.Controls.Add(this.picOptionsWarning);
      this.grpOptions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.grpOptions.Location = new System.Drawing.Point(82, 395);
      this.grpOptions.Name = "grpOptions";
      this.grpOptions.Size = new System.Drawing.Size(677, 100);
      this.grpOptions.TabIndex = 8;
      this.grpOptions.TabStop = false;
      this.grpOptions.Text = "Options";
      // 
      // btnSelectAll
      // 
      this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSelectAll.Location = new System.Drawing.Point(519, 29);
      this.btnSelectAll.Name = "btnSelectAll";
      this.btnSelectAll.Size = new System.Drawing.Size(150, 23);
      this.btnSelectAll.TabIndex = 7;
      this.btnSelectAll.Text = "Select All";
      this.btnSelectAll.UseVisualStyleBackColor = true;
      this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
      // 
      // numRowsToReturn
      // 
      this.numRowsToReturn.Enabled = false;
      this.numRowsToReturn.Location = new System.Drawing.Point(480, 63);
      this.numRowsToReturn.Name = "numRowsToReturn";
      this.numRowsToReturn.Size = new System.Drawing.Size(60, 23);
      this.numRowsToReturn.TabIndex = 6;
      // 
      // lblRowsToReturn
      // 
      this.lblRowsToReturn.AutoSize = true;
      this.lblRowsToReturn.Location = new System.Drawing.Point(323, 65);
      this.lblRowsToReturn.Name = "lblRowsToReturn";
      this.lblRowsToReturn.Size = new System.Drawing.Size(151, 15);
      this.lblRowsToReturn.TabIndex = 5;
      this.lblRowsToReturn.Text = "Number of Rows to Return:";
      // 
      // numFromRow
      // 
      this.numFromRow.Enabled = false;
      this.numFromRow.Location = new System.Drawing.Point(235, 63);
      this.numFromRow.Name = "numFromRow";
      this.numFromRow.Size = new System.Drawing.Size(60, 23);
      this.numFromRow.TabIndex = 4;
      this.numFromRow.ValueChanged += new System.EventHandler(this.numFromRow_ValueChanged);
      // 
      // lblFromRow
      // 
      this.lblFromRow.AutoSize = true;
      this.lblFromRow.Location = new System.Drawing.Point(165, 65);
      this.lblFromRow.Name = "lblFromRow";
      this.lblFromRow.Size = new System.Drawing.Size(64, 15);
      this.lblFromRow.TabIndex = 3;
      this.lblFromRow.Text = "From Row:";
      // 
      // chkLimitRows
      // 
      this.chkLimitRows.AutoSize = true;
      this.chkLimitRows.Location = new System.Drawing.Point(18, 64);
      this.chkLimitRows.Name = "chkLimitRows";
      this.chkLimitRows.Size = new System.Drawing.Size(84, 19);
      this.chkLimitRows.TabIndex = 2;
      this.chkLimitRows.Text = "Limit Rows";
      this.chkLimitRows.UseVisualStyleBackColor = true;
      this.chkLimitRows.CheckedChanged += new System.EventHandler(this.chkLimitRows_CheckedChanged);
      // 
      // chkIncludeHeaders
      // 
      this.chkIncludeHeaders.AutoSize = true;
      this.chkIncludeHeaders.Location = new System.Drawing.Point(18, 32);
      this.chkIncludeHeaders.Name = "chkIncludeHeaders";
      this.chkIncludeHeaders.Size = new System.Drawing.Size(211, 19);
      this.chkIncludeHeaders.TabIndex = 1;
      this.chkIncludeHeaders.Text = "Include Column Names as Headers";
      this.chkIncludeHeaders.UseVisualStyleBackColor = true;
      // 
      // lblOptionsWarning
      // 
      this.lblOptionsWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblOptionsWarning.AutoSize = true;
      this.lblOptionsWarning.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblOptionsWarning.ForeColor = System.Drawing.Color.Red;
      this.lblOptionsWarning.Location = new System.Drawing.Point(77, 0);
      this.lblOptionsWarning.Name = "lblOptionsWarning";
      this.lblOptionsWarning.Size = new System.Drawing.Size(76, 12);
      this.lblOptionsWarning.TabIndex = 0;
      this.lblOptionsWarning.Text = "Warning Message";
      this.lblOptionsWarning.Visible = false;
      // 
      // picOptionsWarning
      // 
      this.picOptionsWarning.Image = global::MySQL.ForExcel.Properties.Resources.Warning;
      this.picOptionsWarning.Location = new System.Drawing.Point(55, -1);
      this.picOptionsWarning.Name = "picOptionsWarning";
      this.picOptionsWarning.Size = new System.Drawing.Size(20, 20);
      this.picOptionsWarning.TabIndex = 24;
      this.picOptionsWarning.TabStop = false;
      this.picOptionsWarning.Visible = false;
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
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.grdPreviewData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
      this.grdPreviewData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdPreviewData.DefaultCellStyle = dataGridViewCellStyle4;
      this.grdPreviewData.Location = new System.Drawing.Point(82, 123);
      this.grdPreviewData.Name = "grdPreviewData";
      this.grdPreviewData.ReadOnly = true;
      this.grdPreviewData.RowHeadersVisible = false;
      this.grdPreviewData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.grdPreviewData.Size = new System.Drawing.Size(677, 265);
      this.grdPreviewData.TabIndex = 7;
      this.grdPreviewData.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grdPreviewData_DataBindingComplete);
      this.grdPreviewData.SelectionChanged += new System.EventHandler(this.grdPreviewData_SelectionChanged);
      // 
      // lblPickColumnsSub
      // 
      this.lblPickColumnsSub.AutoSize = true;
      this.lblPickColumnsSub.BackColor = System.Drawing.Color.Transparent;
      this.lblPickColumnsSub.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPickColumnsSub.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblPickColumnsSub.Location = new System.Drawing.Point(491, 73);
      this.lblPickColumnsSub.Name = "lblPickColumnsSub";
      this.lblPickColumnsSub.Size = new System.Drawing.Size(268, 15);
      this.lblPickColumnsSub.TabIndex = 5;
      this.lblPickColumnsSub.Text = "Click the header of a column to select/unselect it.";
      // 
      // lblPickColumnsMain
      // 
      this.lblPickColumnsMain.AutoSize = true;
      this.lblPickColumnsMain.BackColor = System.Drawing.Color.Transparent;
      this.lblPickColumnsMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPickColumnsMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblPickColumnsMain.Location = new System.Drawing.Point(491, 53);
      this.lblPickColumnsMain.Name = "lblPickColumnsMain";
      this.lblPickColumnsMain.Size = new System.Drawing.Size(192, 17);
      this.lblPickColumnsMain.TabIndex = 4;
      this.lblPickColumnsMain.Text = "Pick Columns to Import to Excel";
      // 
      // picColumnOptions
      // 
      this.picColumnOptions.BackColor = System.Drawing.Color.Transparent;
      this.picColumnOptions.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_ColumnOptions_32x32;
      this.picColumnOptions.Location = new System.Drawing.Point(453, 59);
      this.picColumnOptions.Name = "picColumnOptions";
      this.picColumnOptions.Size = new System.Drawing.Size(32, 32);
      this.picColumnOptions.TabIndex = 29;
      this.picColumnOptions.TabStop = false;
      // 
      // lblRowsCountMain
      // 
      this.lblRowsCountMain.AutoSize = true;
      this.lblRowsCountMain.BackColor = System.Drawing.Color.Transparent;
      this.lblRowsCountMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblRowsCountMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblRowsCountMain.Location = new System.Drawing.Point(79, 71);
      this.lblRowsCountMain.Name = "lblRowsCountMain";
      this.lblRowsCountMain.Size = new System.Drawing.Size(113, 17);
      this.lblRowsCountMain.TabIndex = 2;
      this.lblRowsCountMain.Text = "Total Rows Count:";
      // 
      // lblFromSub
      // 
      this.lblFromSub.AutoSize = true;
      this.lblFromSub.BackColor = System.Drawing.Color.Transparent;
      this.lblFromSub.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblFromSub.ForeColor = System.Drawing.Color.Navy;
      this.lblFromSub.Location = new System.Drawing.Point(162, 54);
      this.lblFromSub.Name = "lblFromSub";
      this.lblFromSub.Size = new System.Drawing.Size(43, 17);
      this.lblFromSub.TabIndex = 1;
      this.lblFromSub.Text = "Name";
      // 
      // lblFromMain
      // 
      this.lblFromMain.AutoSize = true;
      this.lblFromMain.BackColor = System.Drawing.Color.Transparent;
      this.lblFromMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblFromMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblFromMain.Location = new System.Drawing.Point(79, 54);
      this.lblFromMain.Name = "lblFromMain";
      this.lblFromMain.Size = new System.Drawing.Size(77, 17);
      this.lblFromMain.TabIndex = 0;
      this.lblFromMain.Text = "From Table:";
      // 
      // picFrom
      // 
      this.picFrom.BackColor = System.Drawing.Color.Transparent;
      this.picFrom.Location = new System.Drawing.Point(41, 60);
      this.picFrom.Name = "picFrom";
      this.picFrom.Size = new System.Drawing.Size(32, 32);
      this.picFrom.TabIndex = 20;
      this.picFrom.TabStop = false;
      // 
      // btnImport
      // 
      this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnImport.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnImport.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnImport.Location = new System.Drawing.Point(676, 10);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new System.Drawing.Size(75, 23);
      this.btnImport.TabIndex = 0;
      this.btnImport.Text = "Import";
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnCancel.Location = new System.Drawing.Point(757, 10);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // ImportTableViewForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(844, 552);
      this.CommandAreaHeight = 44;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MainInstruction = "Import Data from MySQL";
      this.MainInstructionLocation = new System.Drawing.Point(10, 14);
      this.MinimumSize = new System.Drawing.Size(860, 590);
      this.Name = "ImportTableViewForm";
      this.Text = "Import Data";
      this.contentAreaPanel.ResumeLayout(false);
      this.contentAreaPanel.PerformLayout();
      this.commandAreaPanel.ResumeLayout(false);
      this.grpOptions.ResumeLayout(false);
      this.grpOptions.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numRowsToReturn)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numFromRow)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picOptionsWarning)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColumnOptions)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picFrom)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ImageList fromImageList;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label lblRowsCountSub;
    private System.Windows.Forms.GroupBox grpOptions;
    private System.Windows.Forms.Button btnSelectAll;
    private System.Windows.Forms.NumericUpDown numRowsToReturn;
    private System.Windows.Forms.Label lblRowsToReturn;
    private System.Windows.Forms.NumericUpDown numFromRow;
    private System.Windows.Forms.Label lblFromRow;
    private System.Windows.Forms.CheckBox chkLimitRows;
    private System.Windows.Forms.CheckBox chkIncludeHeaders;
    private System.Windows.Forms.Label lblOptionsWarning;
    private System.Windows.Forms.PictureBox picOptionsWarning;
    private System.Windows.Forms.DataGridView grdPreviewData;
    private System.Windows.Forms.Label lblPickColumnsSub;
    private System.Windows.Forms.Label lblPickColumnsMain;
    private System.Windows.Forms.PictureBox picColumnOptions;
    private System.Windows.Forms.Label lblRowsCountMain;
    private System.Windows.Forms.Label lblFromSub;
    private System.Windows.Forms.Label lblFromMain;
    private System.Windows.Forms.PictureBox picFrom;
    private System.Windows.Forms.Button btnImport;
    private System.Windows.Forms.Button btnCancel;

  }
}