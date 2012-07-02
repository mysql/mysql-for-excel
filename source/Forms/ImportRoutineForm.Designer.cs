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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      this.btnImport = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
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
      this.contentAreaPanel.SuspendLayout();
      this.commandAreaPanel.SuspendLayout();
      this.grpOptions.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColumnOptions)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picFrom)).BeginInit();
      this.SuspendLayout();
      // 
      // contentAreaPanel
      // 
      this.contentAreaPanel.BackColor = System.Drawing.SystemColors.Window;
      this.contentAreaPanel.Controls.Add(this.lblExportData);
      this.contentAreaPanel.Controls.Add(this.btnCall);
      this.contentAreaPanel.Controls.Add(this.lblFromRoutineSub2);
      this.contentAreaPanel.Controls.Add(this.lisResultSets);
      this.contentAreaPanel.Controls.Add(this.parametersGrid);
      this.contentAreaPanel.Controls.Add(this.lblFromRoutineSub1);
      this.contentAreaPanel.Controls.Add(this.grpOptions);
      this.contentAreaPanel.Controls.Add(this.grdPreviewData);
      this.contentAreaPanel.Controls.Add(this.lblPickColumnsSub);
      this.contentAreaPanel.Controls.Add(this.lblPickColumnsMain);
      this.contentAreaPanel.Controls.Add(this.picColumnOptions);
      this.contentAreaPanel.Controls.Add(this.lblFromRoutineName);
      this.contentAreaPanel.Controls.Add(this.lblFromRoutineMain);
      this.contentAreaPanel.Controls.Add(this.picFrom);
      this.contentAreaPanel.Size = new System.Drawing.Size(944, 415);
      // 
      // commandAreaPanel
      // 
      this.commandAreaPanel.Controls.Add(this.btnImport);
      this.commandAreaPanel.Controls.Add(this.btnCancel);
      this.commandAreaPanel.Location = new System.Drawing.Point(0, 416);
      this.commandAreaPanel.Size = new System.Drawing.Size(944, 45);
      // 
      // btnImport
      // 
      this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnImport.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnImport.Enabled = false;
      this.btnImport.Location = new System.Drawing.Point(776, 11);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new System.Drawing.Size(75, 23);
      this.btnImport.TabIndex = 0;
      this.btnImport.Text = "Import";
      this.btnImport.UseVisualStyleBackColor = true;
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(857, 11);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnCall
      // 
      this.btnCall.Location = new System.Drawing.Point(84, 352);
      this.btnCall.Name = "btnCall";
      this.btnCall.Size = new System.Drawing.Size(75, 23);
      this.btnCall.TabIndex = 7;
      this.btnCall.Text = "Call";
      this.btnCall.UseVisualStyleBackColor = true;
      this.btnCall.Click += new System.EventHandler(this.btnCall_Click);
      // 
      // lblFromRoutineSub2
      // 
      this.lblFromRoutineSub2.AutoSize = true;
      this.lblFromRoutineSub2.BackColor = System.Drawing.Color.Transparent;
      this.lblFromRoutineSub2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblFromRoutineSub2.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblFromRoutineSub2.Location = new System.Drawing.Point(80, 88);
      this.lblFromRoutineSub2.Name = "lblFromRoutineSub2";
      this.lblFromRoutineSub2.Size = new System.Drawing.Size(190, 15);
      this.lblFromRoutineSub2.TabIndex = 3;
      this.lblFromRoutineSub2.Text = "parameters to the selected routine.";
      // 
      // lisResultSets
      // 
      this.lisResultSets.FormattingEnabled = true;
      this.lisResultSets.Location = new System.Drawing.Point(280, 119);
      this.lisResultSets.Name = "lisResultSets";
      this.lisResultSets.Size = new System.Drawing.Size(120, 212);
      this.lisResultSets.TabIndex = 8;
      this.lisResultSets.SelectedIndexChanged += new System.EventHandler(this.lisResultSets_SelectedIndexChanged);
      // 
      // parametersGrid
      // 
      this.parametersGrid.Location = new System.Drawing.Point(84, 116);
      this.parametersGrid.Name = "parametersGrid";
      this.parametersGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
      this.parametersGrid.Size = new System.Drawing.Size(186, 215);
      this.parametersGrid.TabIndex = 6;
      this.parametersGrid.ToolbarVisible = false;
      // 
      // lblFromRoutineSub1
      // 
      this.lblFromRoutineSub1.AutoSize = true;
      this.lblFromRoutineSub1.BackColor = System.Drawing.Color.Transparent;
      this.lblFromRoutineSub1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblFromRoutineSub1.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblFromRoutineSub1.Location = new System.Drawing.Point(79, 73);
      this.lblFromRoutineSub1.Name = "lblFromRoutineSub1";
      this.lblFromRoutineSub1.Size = new System.Drawing.Size(167, 15);
      this.lblFromRoutineSub1.TabIndex = 2;
      this.lblFromRoutineSub1.Text = "Below you can pass all needed";
      // 
      // grpOptions
      // 
      this.grpOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grpOptions.BackColor = System.Drawing.Color.Transparent;
      this.grpOptions.Controls.Add(this.cmbMultipleResultSets);
      this.grpOptions.Controls.Add(this.lblMultipleResultSets);
      this.grpOptions.Controls.Add(this.chkIncludeHeaders);
      this.grpOptions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.grpOptions.Location = new System.Drawing.Point(280, 337);
      this.grpOptions.Name = "grpOptions";
      this.grpOptions.Size = new System.Drawing.Size(588, 56);
      this.grpOptions.TabIndex = 10;
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
      this.grdPreviewData.Location = new System.Drawing.Point(406, 119);
      this.grdPreviewData.Name = "grdPreviewData";
      this.grdPreviewData.ReadOnly = true;
      this.grdPreviewData.RowHeadersVisible = false;
      this.grdPreviewData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.grdPreviewData.Size = new System.Drawing.Size(462, 212);
      this.grdPreviewData.TabIndex = 9;
      // 
      // lblPickColumnsSub
      // 
      this.lblPickColumnsSub.AutoSize = true;
      this.lblPickColumnsSub.BackColor = System.Drawing.Color.Transparent;
      this.lblPickColumnsSub.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPickColumnsSub.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblPickColumnsSub.Location = new System.Drawing.Point(444, 74);
      this.lblPickColumnsSub.Name = "lblPickColumnsSub";
      this.lblPickColumnsSub.Size = new System.Drawing.Size(219, 15);
      this.lblPickColumnsSub.TabIndex = 5;
      this.lblPickColumnsSub.Text = "Select how to import Result Sets to Excel";
      // 
      // lblPickColumnsMain
      // 
      this.lblPickColumnsMain.AutoSize = true;
      this.lblPickColumnsMain.BackColor = System.Drawing.Color.Transparent;
      this.lblPickColumnsMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPickColumnsMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblPickColumnsMain.Location = new System.Drawing.Point(444, 54);
      this.lblPickColumnsMain.Name = "lblPickColumnsMain";
      this.lblPickColumnsMain.Size = new System.Drawing.Size(129, 17);
      this.lblPickColumnsMain.TabIndex = 4;
      this.lblPickColumnsMain.Text = "2. Review Result Sets";
      // 
      // picColumnOptions
      // 
      this.picColumnOptions.BackColor = System.Drawing.Color.Transparent;
      this.picColumnOptions.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ExportDlg_ColumnOptions_32x32;
      this.picColumnOptions.Location = new System.Drawing.Point(406, 60);
      this.picColumnOptions.Name = "picColumnOptions";
      this.picColumnOptions.Size = new System.Drawing.Size(32, 32);
      this.picColumnOptions.TabIndex = 31;
      this.picColumnOptions.TabStop = false;
      // 
      // lblFromRoutineName
      // 
      this.lblFromRoutineName.AutoSize = true;
      this.lblFromRoutineName.BackColor = System.Drawing.Color.Transparent;
      this.lblFromRoutineName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblFromRoutineName.ForeColor = System.Drawing.Color.Navy;
      this.lblFromRoutineName.Location = new System.Drawing.Point(277, 54);
      this.lblFromRoutineName.Name = "lblFromRoutineName";
      this.lblFromRoutineName.Size = new System.Drawing.Size(43, 17);
      this.lblFromRoutineName.TabIndex = 1;
      this.lblFromRoutineName.Text = "Name";
      // 
      // lblFromRoutineMain
      // 
      this.lblFromRoutineMain.AutoSize = true;
      this.lblFromRoutineMain.BackColor = System.Drawing.Color.Transparent;
      this.lblFromRoutineMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblFromRoutineMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblFromRoutineMain.Location = new System.Drawing.Point(79, 54);
      this.lblFromRoutineMain.Name = "lblFromRoutineMain";
      this.lblFromRoutineMain.Size = new System.Drawing.Size(192, 17);
      this.lblFromRoutineMain.TabIndex = 0;
      this.lblFromRoutineMain.Text = "1. Fill Parameters From Routine:";
      // 
      // picFrom
      // 
      this.picFrom.BackColor = System.Drawing.Color.Transparent;
      this.picFrom.Image = global::MySQL.ForExcel.Properties.Resources.db_Routine_32x32;
      this.picFrom.Location = new System.Drawing.Point(41, 60);
      this.picFrom.Name = "picFrom";
      this.picFrom.Size = new System.Drawing.Size(32, 32);
      this.picFrom.TabIndex = 20;
      this.picFrom.TabStop = false;
      // 
      // lblExportData
      // 
      this.lblExportData.AutoSize = true;
      this.lblExportData.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblExportData.ForeColor = System.Drawing.Color.Navy;
      this.lblExportData.Location = new System.Drawing.Point(17, 17);
      this.lblExportData.Name = "lblExportData";
      this.lblExportData.Size = new System.Drawing.Size(176, 20);
      this.lblExportData.TabIndex = 32;
      this.lblExportData.Text = "Import Data from MySQL";
      // 
      // ImportRoutineForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(944, 462);
      this.CommandAreaHeight = 45;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MainInstructionLocation = new System.Drawing.Point(11, 15);
      this.MinimumSize = new System.Drawing.Size(960, 500);
      this.Name = "ImportRoutineForm";
      this.Text = "Import Data";
      this.contentAreaPanel.ResumeLayout(false);
      this.contentAreaPanel.PerformLayout();
      this.commandAreaPanel.ResumeLayout(false);
      this.grpOptions.ResumeLayout(false);
      this.grpOptions.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreviewData)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picColumnOptions)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picFrom)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnImport;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnCall;
    private System.Windows.Forms.Label lblFromRoutineSub2;
    private System.Windows.Forms.ListBox lisResultSets;
    private System.Windows.Forms.PropertyGrid parametersGrid;
    private System.Windows.Forms.Label lblFromRoutineSub1;
    private System.Windows.Forms.GroupBox grpOptions;
    private System.Windows.Forms.ComboBox cmbMultipleResultSets;
    private System.Windows.Forms.Label lblMultipleResultSets;
    private System.Windows.Forms.CheckBox chkIncludeHeaders;
    private System.Windows.Forms.DataGridView grdPreviewData;
    private System.Windows.Forms.Label lblPickColumnsSub;
    private System.Windows.Forms.Label lblPickColumnsMain;
    private System.Windows.Forms.PictureBox picColumnOptions;
    private System.Windows.Forms.Label lblFromRoutineName;
    private System.Windows.Forms.Label lblFromRoutineMain;
    private System.Windows.Forms.PictureBox picFrom;
    private System.Windows.Forms.Label lblExportData;
  }
}