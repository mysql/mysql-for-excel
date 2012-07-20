namespace MySQL.ForExcel
{
  partial class ImportProcedureForm
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
      this.btnImport = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnCall = new System.Windows.Forms.Button();
      this.parametersGrid = new System.Windows.Forms.PropertyGrid();
      this.lblProcedureParametersSub = new System.Windows.Forms.Label();
      this.cmbImportResultsets = new System.Windows.Forms.ComboBox();
      this.lblImportResultsets = new System.Windows.Forms.Label();
      this.chkIncludeHeaders = new System.Windows.Forms.CheckBox();
      this.lblImportOptionsSub = new System.Windows.Forms.Label();
      this.lblImportOptionsMain = new System.Windows.Forms.Label();
      this.picColumnOptions = new System.Windows.Forms.PictureBox();
      this.lblFromProcedureName = new System.Windows.Forms.Label();
      this.lblProcedureParametersMain = new System.Windows.Forms.Label();
      this.picFrom = new System.Windows.Forms.PictureBox();
      this.lblImportData = new System.Windows.Forms.Label();
      this.tabResultSets = new System.Windows.Forms.TabControl();
      this.grdResultSet = new MySQL.ForExcel.PreviewDataGridView();
      this.contentAreaPanel.SuspendLayout();
      this.commandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picColumnOptions)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picFrom)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.grdResultSet)).BeginInit();
      this.SuspendLayout();
      // 
      // contentAreaPanel
      // 
      this.contentAreaPanel.BackColor = System.Drawing.SystemColors.Window;
      this.contentAreaPanel.Controls.Add(this.tabResultSets);
      this.contentAreaPanel.Controls.Add(this.cmbImportResultsets);
      this.contentAreaPanel.Controls.Add(this.lblImportResultsets);
      this.contentAreaPanel.Controls.Add(this.lblImportData);
      this.contentAreaPanel.Controls.Add(this.btnCall);
      this.contentAreaPanel.Controls.Add(this.chkIncludeHeaders);
      this.contentAreaPanel.Controls.Add(this.parametersGrid);
      this.contentAreaPanel.Controls.Add(this.lblProcedureParametersSub);
      this.contentAreaPanel.Controls.Add(this.lblImportOptionsSub);
      this.contentAreaPanel.Controls.Add(this.lblImportOptionsMain);
      this.contentAreaPanel.Controls.Add(this.picColumnOptions);
      this.contentAreaPanel.Controls.Add(this.lblFromProcedureName);
      this.contentAreaPanel.Controls.Add(this.lblProcedureParametersMain);
      this.contentAreaPanel.Controls.Add(this.picFrom);
      this.contentAreaPanel.Controls.Add(this.grdResultSet);
      this.contentAreaPanel.Size = new System.Drawing.Size(846, 547);
      // 
      // commandAreaPanel
      // 
      this.commandAreaPanel.Controls.Add(this.btnImport);
      this.commandAreaPanel.Controls.Add(this.btnCancel);
      this.commandAreaPanel.Location = new System.Drawing.Point(0, 547);
      this.commandAreaPanel.Size = new System.Drawing.Size(846, 45);
      // 
      // btnImport
      // 
      this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnImport.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnImport.Enabled = false;
      this.btnImport.Location = new System.Drawing.Point(678, 11);
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
      this.btnCancel.Location = new System.Drawing.Point(759, 11);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnCall
      // 
      this.btnCall.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnCall.Location = new System.Drawing.Point(666, 75);
      this.btnCall.Name = "btnCall";
      this.btnCall.Size = new System.Drawing.Size(87, 23);
      this.btnCall.TabIndex = 7;
      this.btnCall.Text = "Call";
      this.btnCall.UseVisualStyleBackColor = true;
      this.btnCall.Click += new System.EventHandler(this.btnCall_Click);
      // 
      // parametersGrid
      // 
      this.parametersGrid.HelpVisible = false;
      this.parametersGrid.Location = new System.Drawing.Point(395, 75);
      this.parametersGrid.Name = "parametersGrid";
      this.parametersGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
      this.parametersGrid.Size = new System.Drawing.Size(265, 80);
      this.parametersGrid.TabIndex = 6;
      this.parametersGrid.ToolbarVisible = false;
      // 
      // lblProcedureParametersSub
      // 
      this.lblProcedureParametersSub.AutoSize = true;
      this.lblProcedureParametersSub.BackColor = System.Drawing.Color.Transparent;
      this.lblProcedureParametersSub.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblProcedureParametersSub.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblProcedureParametersSub.Location = new System.Drawing.Point(79, 73);
      this.lblProcedureParametersSub.Name = "lblProcedureParametersSub";
      this.lblProcedureParametersSub.Size = new System.Drawing.Size(285, 45);
      this.lblProcedureParametersSub.TabIndex = 2;
      this.lblProcedureParametersSub.Text = "A procedure might need parameters to be set. Please\r\nset a value for all paramete" +
    "rs. Then press the [Call]\r\nbutton to execute the procedure.";
      // 
      // cmbImportResultsets
      // 
      this.cmbImportResultsets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbImportResultsets.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cmbImportResultsets.FormattingEnabled = true;
      this.cmbImportResultsets.Location = new System.Drawing.Point(444, 192);
      this.cmbImportResultsets.Name = "cmbImportResultsets";
      this.cmbImportResultsets.Size = new System.Drawing.Size(216, 23);
      this.cmbImportResultsets.TabIndex = 2;
      // 
      // lblImportResultsets
      // 
      this.lblImportResultsets.AutoSize = true;
      this.lblImportResultsets.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblImportResultsets.Location = new System.Drawing.Point(392, 195);
      this.lblImportResultsets.Name = "lblImportResultsets";
      this.lblImportResultsets.Size = new System.Drawing.Size(46, 15);
      this.lblImportResultsets.TabIndex = 1;
      this.lblImportResultsets.Text = "Import:";
      // 
      // chkIncludeHeaders
      // 
      this.chkIncludeHeaders.AutoSize = true;
      this.chkIncludeHeaders.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkIncludeHeaders.Location = new System.Drawing.Point(395, 221);
      this.chkIncludeHeaders.Name = "chkIncludeHeaders";
      this.chkIncludeHeaders.Size = new System.Drawing.Size(211, 19);
      this.chkIncludeHeaders.TabIndex = 0;
      this.chkIncludeHeaders.Text = "Include Column Names as Headers";
      this.chkIncludeHeaders.UseVisualStyleBackColor = true;
      // 
      // lblImportOptionsSub
      // 
      this.lblImportOptionsSub.AutoSize = true;
      this.lblImportOptionsSub.BackColor = System.Drawing.Color.Transparent;
      this.lblImportOptionsSub.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblImportOptionsSub.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblImportOptionsSub.Location = new System.Drawing.Point(79, 195);
      this.lblImportOptionsSub.Name = "lblImportOptionsSub";
      this.lblImportOptionsSub.Size = new System.Drawing.Size(276, 45);
      this.lblImportOptionsSub.TabIndex = 5;
      this.lblImportOptionsSub.Text = "A procedure might return more than one result set.\r\nPlease choose which result se" +
    "t to import or how\r\nto import several result sets.";
      // 
      // lblImportOptionsMain
      // 
      this.lblImportOptionsMain.AutoSize = true;
      this.lblImportOptionsMain.BackColor = System.Drawing.Color.Transparent;
      this.lblImportOptionsMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblImportOptionsMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblImportOptionsMain.Location = new System.Drawing.Point(79, 175);
      this.lblImportOptionsMain.Name = "lblImportOptionsMain";
      this.lblImportOptionsMain.Size = new System.Drawing.Size(111, 17);
      this.lblImportOptionsMain.TabIndex = 4;
      this.lblImportOptionsMain.Text = "2. Import Options";
      // 
      // picColumnOptions
      // 
      this.picColumnOptions.BackColor = System.Drawing.Color.Transparent;
      this.picColumnOptions.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ImportRoutineDlg_Options_32x32;
      this.picColumnOptions.Location = new System.Drawing.Point(41, 181);
      this.picColumnOptions.Name = "picColumnOptions";
      this.picColumnOptions.Size = new System.Drawing.Size(32, 32);
      this.picColumnOptions.TabIndex = 31;
      this.picColumnOptions.TabStop = false;
      // 
      // lblFromProcedureName
      // 
      this.lblFromProcedureName.AutoSize = true;
      this.lblFromProcedureName.BackColor = System.Drawing.Color.Transparent;
      this.lblFromProcedureName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblFromProcedureName.ForeColor = System.Drawing.Color.Navy;
      this.lblFromProcedureName.Location = new System.Drawing.Point(392, 54);
      this.lblFromProcedureName.Name = "lblFromProcedureName";
      this.lblFromProcedureName.Size = new System.Drawing.Size(43, 17);
      this.lblFromProcedureName.TabIndex = 1;
      this.lblFromProcedureName.Text = "Name";
      // 
      // lblProcedureParametersMain
      // 
      this.lblProcedureParametersMain.AutoSize = true;
      this.lblProcedureParametersMain.BackColor = System.Drawing.Color.Transparent;
      this.lblProcedureParametersMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblProcedureParametersMain.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblProcedureParametersMain.Location = new System.Drawing.Point(79, 54);
      this.lblProcedureParametersMain.Name = "lblProcedureParametersMain";
      this.lblProcedureParametersMain.Size = new System.Drawing.Size(174, 17);
      this.lblProcedureParametersMain.TabIndex = 0;
      this.lblProcedureParametersMain.Text = "1. Set Procedure Parameters";
      // 
      // picFrom
      // 
      this.picFrom.BackColor = System.Drawing.Color.Transparent;
      this.picFrom.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_ImportRoutineDlg_Params_32x32;
      this.picFrom.Location = new System.Drawing.Point(41, 60);
      this.picFrom.Name = "picFrom";
      this.picFrom.Size = new System.Drawing.Size(32, 32);
      this.picFrom.TabIndex = 20;
      this.picFrom.TabStop = false;
      // 
      // lblImportData
      // 
      this.lblImportData.AutoSize = true;
      this.lblImportData.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblImportData.ForeColor = System.Drawing.Color.Navy;
      this.lblImportData.Location = new System.Drawing.Point(17, 17);
      this.lblImportData.Name = "lblImportData";
      this.lblImportData.Size = new System.Drawing.Size(176, 20);
      this.lblImportData.TabIndex = 32;
      this.lblImportData.Text = "Import Data from MySQL";
      // 
      // tabResultSets
      // 
      this.tabResultSets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabResultSets.Location = new System.Drawing.Point(82, 269);
      this.tabResultSets.Name = "tabResultSets";
      this.tabResultSets.SelectedIndex = 0;
      this.tabResultSets.Size = new System.Drawing.Size(676, 238);
      this.tabResultSets.TabIndex = 33;
      this.tabResultSets.SelectedIndexChanged += new System.EventHandler(this.tabResultSets_SelectedIndexChanged);
      // 
      // grdResultSet
      // 
      this.grdResultSet.AllowUserToAddRows = false;
      this.grdResultSet.AllowUserToDeleteRows = false;
      this.grdResultSet.AllowUserToResizeColumns = false;
      this.grdResultSet.AllowUserToResizeRows = false;
      this.grdResultSet.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdResultSet.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.grdResultSet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdResultSet.ColumnsMaximumWidth = 200;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.InactiveCaption;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdResultSet.DefaultCellStyle = dataGridViewCellStyle2;
      this.grdResultSet.Location = new System.Drawing.Point(82, 269);
      this.grdResultSet.Name = "grdResultSet";
      this.grdResultSet.ReadOnly = true;
      this.grdResultSet.RowHeadersVisible = false;
      this.grdResultSet.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.grdResultSet.ShowCellErrors = false;
      this.grdResultSet.ShowEditingIcon = false;
      this.grdResultSet.ShowRowErrors = false;
      this.grdResultSet.Size = new System.Drawing.Size(676, 238);
      this.grdResultSet.TabIndex = 9;
      // 
      // ImportProcedureForm
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(846, 594);
      this.CommandAreaHeight = 45;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MainInstructionLocation = new System.Drawing.Point(11, 15);
      this.MinimumSize = new System.Drawing.Size(862, 632);
      this.Name = "ImportProcedureForm";
      this.Text = "Import Data";
      this.contentAreaPanel.ResumeLayout(false);
      this.contentAreaPanel.PerformLayout();
      this.commandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.picColumnOptions)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picFrom)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.grdResultSet)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnImport;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnCall;
    private System.Windows.Forms.PropertyGrid parametersGrid;
    private System.Windows.Forms.Label lblProcedureParametersSub;
    private System.Windows.Forms.ComboBox cmbImportResultsets;
    private System.Windows.Forms.Label lblImportResultsets;
    private System.Windows.Forms.CheckBox chkIncludeHeaders;
    private System.Windows.Forms.Label lblImportOptionsSub;
    private System.Windows.Forms.Label lblImportOptionsMain;
    private System.Windows.Forms.PictureBox picColumnOptions;
    private System.Windows.Forms.Label lblFromProcedureName;
    private System.Windows.Forms.Label lblProcedureParametersMain;
    private System.Windows.Forms.PictureBox picFrom;
    private System.Windows.Forms.Label lblImportData;
    private System.Windows.Forms.TabControl tabResultSets;
    private PreviewDataGridView grdResultSet;
  }
}