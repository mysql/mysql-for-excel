namespace MySQL.ForExcel
{
  partial class ImportRoutineForm
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
      this.ImportRoutinePanel = new System.Windows.Forms.Panel();
      this.btnCall = new System.Windows.Forms.Button();
      this.lblFromRoutineSub2 = new System.Windows.Forms.Label();
      this.lisResultSets = new System.Windows.Forms.ListBox();
      this.parametersGrid = new System.Windows.Forms.PropertyGrid();
      this.lblFromRoutineSub1 = new System.Windows.Forms.Label();
      this.grpOptions = new System.Windows.Forms.GroupBox();
      this.cmbMultipleResultSets = new System.Windows.Forms.ComboBox();
      this.lblMultipleResultSets = new System.Windows.Forms.Label();
      this.chkIncludeHeaders = new System.Windows.Forms.CheckBox();
      this.grdPreviewData = new System.Windows.Forms.DataGridView();
      this.lblPickColumnsSub = new System.Windows.Forms.Label();
      this.lblPickColumnsMain = new System.Windows.Forms.Label();
      this.picColumnOptions = new System.Windows.Forms.PictureBox();
      this.lblFromRoutineName = new System.Windows.Forms.Label();
      this.lblFromRoutineMain = new System.Windows.Forms.Label();
      this.picFrom = new System.Windows.Forms.PictureBox();
      this.lblExportData = new System.Windows.Forms.Label();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnImport = new System.Windows.Forms.Button();
      this.ImportRoutinePanel.SuspendLayout();
      this.grpOptions.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColumnOptions)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picFrom)).BeginInit();
      this.SuspendLayout();
      // 
      // ImportRoutinePanel
      // 
      this.ImportRoutinePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ImportRoutinePanel.BackColor = System.Drawing.SystemColors.Window;
      this.ImportRoutinePanel.Controls.Add(this.btnCall);
      this.ImportRoutinePanel.Controls.Add(this.lblFromRoutineSub2);
      this.ImportRoutinePanel.Controls.Add(this.lisResultSets);
      this.ImportRoutinePanel.Controls.Add(this.parametersGrid);
      this.ImportRoutinePanel.Controls.Add(this.lblFromRoutineSub1);
      this.ImportRoutinePanel.Controls.Add(this.grpOptions);
      this.ImportRoutinePanel.Controls.Add(this.grdPreviewData);
      this.ImportRoutinePanel.Controls.Add(this.lblPickColumnsSub);
      this.ImportRoutinePanel.Controls.Add(this.lblPickColumnsMain);
      this.ImportRoutinePanel.Controls.Add(this.picColumnOptions);
      this.ImportRoutinePanel.Controls.Add(this.lblFromRoutineName);
      this.ImportRoutinePanel.Controls.Add(this.lblFromRoutineMain);
      this.ImportRoutinePanel.Controls.Add(this.picFrom);
      this.ImportRoutinePanel.Controls.Add(this.lblExportData);
      this.ImportRoutinePanel.Location = new System.Drawing.Point(-1, -2);
      this.ImportRoutinePanel.Name = "ImportRoutinePanel";
      this.ImportRoutinePanel.Size = new System.Drawing.Size(946, 419);
      this.ImportRoutinePanel.TabIndex = 0;
      // 
      // btnCall
      // 
      this.btnCall.Location = new System.Drawing.Point(85, 354);
      this.btnCall.Name = "btnCall";
      this.btnCall.Size = new System.Drawing.Size(75, 23);
      this.btnCall.TabIndex = 8;
      this.btnCall.Text = "Call";
      this.btnCall.UseVisualStyleBackColor = true;
      this.btnCall.Click += new System.EventHandler(this.btnCall_Click);
      // 
      // lblFromRoutineSub2
      // 
      this.lblFromRoutineSub2.AutoSize = true;
      this.lblFromRoutineSub2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblFromRoutineSub2.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblFromRoutineSub2.Location = new System.Drawing.Point(81, 90);
      this.lblFromRoutineSub2.Name = "lblFromRoutineSub2";
      this.lblFromRoutineSub2.Size = new System.Drawing.Size(190, 15);
      this.lblFromRoutineSub2.TabIndex = 4;
      this.lblFromRoutineSub2.Text = "parameters to the selected routine.";
      // 
      // lisResultSets
      // 
      this.lisResultSets.FormattingEnabled = true;
      this.lisResultSets.Location = new System.Drawing.Point(281, 121);
      this.lisResultSets.Name = "lisResultSets";
      this.lisResultSets.Size = new System.Drawing.Size(120, 212);
      this.lisResultSets.TabIndex = 9;
      this.lisResultSets.SelectedIndexChanged += new System.EventHandler(this.lisResultSets_SelectedIndexChanged);
      // 
      // parametersGrid
      // 
      this.parametersGrid.Location = new System.Drawing.Point(85, 118);
      this.parametersGrid.Name = "parametersGrid";
      this.parametersGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
      this.parametersGrid.Size = new System.Drawing.Size(186, 215);
      this.parametersGrid.TabIndex = 7;
      this.parametersGrid.ToolbarVisible = false;
      // 
      // lblFromRoutineSub1
      // 
      this.lblFromRoutineSub1.AutoSize = true;
      this.lblFromRoutineSub1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblFromRoutineSub1.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblFromRoutineSub1.Location = new System.Drawing.Point(80, 75);
      this.lblFromRoutineSub1.Name = "lblFromRoutineSub1";
      this.lblFromRoutineSub1.Size = new System.Drawing.Size(167, 15);
      this.lblFromRoutineSub1.TabIndex = 3;
      this.lblFromRoutineSub1.Text = "Below you can pass all needed";
      // 
      // grpOptions
      // 
      this.grpOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grpOptions.Controls.Add(this.cmbMultipleResultSets);
      this.grpOptions.Controls.Add(this.lblMultipleResultSets);
      this.grpOptions.Controls.Add(this.chkIncludeHeaders);
      this.grpOptions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.grpOptions.Location = new System.Drawing.Point(281, 339);
      this.grpOptions.Name = "grpOptions";
      this.grpOptions.Size = new System.Drawing.Size(588, 56);
      this.grpOptions.TabIndex = 11;
      this.grpOptions.TabStop = false;
      this.grpOptions.Text = "Options";
      // 
      // cmbMultipleResultSets
      // 
      this.cmbMultipleResultSets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cmbMultipleResultSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbMultipleResultSets.FormattingEnabled = true;
      this.cmbMultipleResultSets.Location = new System.Drawing.Point(399, 20);
      this.cmbMultipleResultSets.Name = "cmbMultipleResultSets";
      this.cmbMultipleResultSets.Size = new System.Drawing.Size(183, 23);
      this.cmbMultipleResultSets.TabIndex = 2;
      // 
      // lblMultipleResultSets
      // 
      this.lblMultipleResultSets.AutoSize = true;
      this.lblMultipleResultSets.Location = new System.Drawing.Point(232, 23);
      this.lblMultipleResultSets.Name = "lblMultipleResultSets";
      this.lblMultipleResultSets.Size = new System.Drawing.Size(161, 15);
      this.lblMultipleResultSets.TabIndex = 1;
      this.lblMultipleResultSets.Text = "Return Multiple ResultSets in:";
      // 
      // chkIncludeHeaders
      // 
      this.chkIncludeHeaders.AutoSize = true;
      this.chkIncludeHeaders.Location = new System.Drawing.Point(6, 22);
      this.chkIncludeHeaders.Name = "chkIncludeHeaders";
      this.chkIncludeHeaders.Size = new System.Drawing.Size(211, 19);
      this.chkIncludeHeaders.TabIndex = 0;
      this.chkIncludeHeaders.Text = "Include Column Names as Headers";
      this.chkIncludeHeaders.UseVisualStyleBackColor = true;
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
      dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
      dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.grdPreviewData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
      this.grdPreviewData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdPreviewData.DefaultCellStyle = dataGridViewCellStyle11;
      this.grdPreviewData.Location = new System.Drawing.Point(407, 121);
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
      this.grdPreviewData.Size = new System.Drawing.Size(462, 212);
      this.grdPreviewData.TabIndex = 10;
      // 
      // lblPickColumnsSub
      // 
      this.lblPickColumnsSub.AutoSize = true;
      this.lblPickColumnsSub.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPickColumnsSub.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblPickColumnsSub.Location = new System.Drawing.Point(445, 76);
      this.lblPickColumnsSub.Name = "lblPickColumnsSub";
      this.lblPickColumnsSub.Size = new System.Drawing.Size(219, 15);
      this.lblPickColumnsSub.TabIndex = 6;
      this.lblPickColumnsSub.Text = "Select how to import Result Sets to Excel";
      // 
      // lblPickColumnsMain
      // 
      this.lblPickColumnsMain.AutoSize = true;
      this.lblPickColumnsMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPickColumnsMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblPickColumnsMain.Location = new System.Drawing.Point(445, 56);
      this.lblPickColumnsMain.Name = "lblPickColumnsMain";
      this.lblPickColumnsMain.Size = new System.Drawing.Size(129, 17);
      this.lblPickColumnsMain.TabIndex = 5;
      this.lblPickColumnsMain.Text = "2. Review Result Sets";
      // 
      // picColumnOptions
      // 
      this.picColumnOptions.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_ColumnOptions_32x32;
      this.picColumnOptions.Location = new System.Drawing.Point(407, 62);
      this.picColumnOptions.Name = "picColumnOptions";
      this.picColumnOptions.Size = new System.Drawing.Size(32, 32);
      this.picColumnOptions.TabIndex = 18;
      this.picColumnOptions.TabStop = false;
      // 
      // lblFromRoutineName
      // 
      this.lblFromRoutineName.AutoSize = true;
      this.lblFromRoutineName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblFromRoutineName.ForeColor = System.Drawing.Color.Navy;
      this.lblFromRoutineName.Location = new System.Drawing.Point(278, 56);
      this.lblFromRoutineName.Name = "lblFromRoutineName";
      this.lblFromRoutineName.Size = new System.Drawing.Size(43, 17);
      this.lblFromRoutineName.TabIndex = 2;
      this.lblFromRoutineName.Text = "Name";
      // 
      // lblFromRoutineMain
      // 
      this.lblFromRoutineMain.AutoSize = true;
      this.lblFromRoutineMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblFromRoutineMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblFromRoutineMain.Location = new System.Drawing.Point(80, 56);
      this.lblFromRoutineMain.Name = "lblFromRoutineMain";
      this.lblFromRoutineMain.Size = new System.Drawing.Size(192, 17);
      this.lblFromRoutineMain.TabIndex = 1;
      this.lblFromRoutineMain.Text = "1. Fill Parameters From Routine:";
      // 
      // picFrom
      // 
      this.picFrom.Image = global::MySQL.ForExcel.Properties.Resources.db_Routine_32x32;
      this.picFrom.Location = new System.Drawing.Point(42, 62);
      this.picFrom.Name = "picFrom";
      this.picFrom.Size = new System.Drawing.Size(32, 32);
      this.picFrom.TabIndex = 1;
      this.picFrom.TabStop = false;
      // 
      // lblExportData
      // 
      this.lblExportData.AutoSize = true;
      this.lblExportData.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblExportData.ForeColor = System.Drawing.Color.Navy;
      this.lblExportData.Location = new System.Drawing.Point(18, 18);
      this.lblExportData.Name = "lblExportData";
      this.lblExportData.Size = new System.Drawing.Size(176, 20);
      this.lblExportData.TabIndex = 0;
      this.lblExportData.Text = "Import Data from MySQL";
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(857, 426);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnImport
      // 
      this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnImport.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnImport.Enabled = false;
      this.btnImport.Location = new System.Drawing.Point(776, 426);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new System.Drawing.Size(75, 23);
      this.btnImport.TabIndex = 1;
      this.btnImport.Text = "Import";
      this.btnImport.UseVisualStyleBackColor = true;
      // 
      // ImportRoutineForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(944, 462);
      this.Controls.Add(this.btnImport);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.ImportRoutinePanel);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(960, 500);
      this.Name = "ImportRoutineForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Import Data";
      this.ImportRoutinePanel.ResumeLayout(false);
      this.ImportRoutinePanel.PerformLayout();
      this.grpOptions.ResumeLayout(false);
      this.grpOptions.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColumnOptions)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picFrom)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel ImportRoutinePanel;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnImport;
    private System.Windows.Forms.Label lblExportData;
    private System.Windows.Forms.Label lblFromRoutineName;
    private System.Windows.Forms.Label lblFromRoutineMain;
    private System.Windows.Forms.PictureBox picFrom;
    private System.Windows.Forms.DataGridView grdPreviewData;
    private System.Windows.Forms.Label lblPickColumnsSub;
    private System.Windows.Forms.Label lblPickColumnsMain;
    private System.Windows.Forms.PictureBox picColumnOptions;
    private System.Windows.Forms.GroupBox grpOptions;
    private System.Windows.Forms.CheckBox chkIncludeHeaders;
    private System.Windows.Forms.Label lblMultipleResultSets;
    private System.Windows.Forms.Label lblFromRoutineSub1;
    private System.Windows.Forms.PropertyGrid parametersGrid;
    private System.Windows.Forms.ListBox lisResultSets;
    private System.Windows.Forms.Label lblFromRoutineSub2;
    private System.Windows.Forms.ComboBox cmbMultipleResultSets;
    private System.Windows.Forms.Button btnCall;
  }
}