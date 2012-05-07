namespace MySQL.ExcelAddIn
{
  partial class ImportRoutineDialog
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
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnImport = new System.Windows.Forms.Button();
      this.grpOptions = new System.Windows.Forms.GroupBox();
      this.cmbMultipleResultSets = new System.Windows.Forms.ComboBox();
      this.lblMultipleResultSets = new System.Windows.Forms.Label();
      this.chkIncludeHeaders = new System.Windows.Forms.CheckBox();
      this.lblFrom = new System.Windows.Forms.Label();
      this.grpPreview = new System.Windows.Forms.GroupBox();
      this.lisResultSets = new System.Windows.Forms.ListBox();
      this.grdPreview = new System.Windows.Forms.DataGridView();
      this.parametersGrid = new System.Windows.Forms.PropertyGrid();
      this.grpParameters = new System.Windows.Forms.GroupBox();
      this.btnCall = new System.Windows.Forms.Button();
      this.grpOptions.SuspendLayout();
      this.grpPreview.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreview)).BeginInit();
      this.grpParameters.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(707, 321);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnImport
      // 
      this.btnImport.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnImport.Location = new System.Drawing.Point(626, 321);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new System.Drawing.Size(75, 23);
      this.btnImport.TabIndex = 8;
      this.btnImport.Text = "Import";
      this.btnImport.UseVisualStyleBackColor = true;
      // 
      // grpOptions
      // 
      this.grpOptions.Controls.Add(this.cmbMultipleResultSets);
      this.grpOptions.Controls.Add(this.lblMultipleResultSets);
      this.grpOptions.Controls.Add(this.chkIncludeHeaders);
      this.grpOptions.Location = new System.Drawing.Point(215, 270);
      this.grpOptions.Name = "grpOptions";
      this.grpOptions.Size = new System.Drawing.Size(567, 45);
      this.grpOptions.TabIndex = 7;
      this.grpOptions.TabStop = false;
      this.grpOptions.Text = "Options";
      // 
      // cmbMultipleResultSets
      // 
      this.cmbMultipleResultSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbMultipleResultSets.FormattingEnabled = true;
      this.cmbMultipleResultSets.Location = new System.Drawing.Point(385, 17);
      this.cmbMultipleResultSets.Name = "cmbMultipleResultSets";
      this.cmbMultipleResultSets.Size = new System.Drawing.Size(176, 21);
      this.cmbMultipleResultSets.TabIndex = 6;
      // 
      // lblMultipleResultSets
      // 
      this.lblMultipleResultSets.AutoSize = true;
      this.lblMultipleResultSets.Location = new System.Drawing.Point(233, 20);
      this.lblMultipleResultSets.Name = "lblMultipleResultSets";
      this.lblMultipleResultSets.Size = new System.Drawing.Size(146, 13);
      this.lblMultipleResultSets.TabIndex = 5;
      this.lblMultipleResultSets.Text = "Return Multiple ResultSets in:";
      // 
      // chkIncludeHeaders
      // 
      this.chkIncludeHeaders.AutoSize = true;
      this.chkIncludeHeaders.Location = new System.Drawing.Point(6, 19);
      this.chkIncludeHeaders.Name = "chkIncludeHeaders";
      this.chkIncludeHeaders.Size = new System.Drawing.Size(192, 17);
      this.chkIncludeHeaders.TabIndex = 4;
      this.chkIncludeHeaders.Text = "Include Column Names as Headers";
      this.chkIncludeHeaders.UseVisualStyleBackColor = true;
      // 
      // lblFrom
      // 
      this.lblFrom.AutoSize = true;
      this.lblFrom.Location = new System.Drawing.Point(12, 9);
      this.lblFrom.Name = "lblFrom";
      this.lblFrom.Size = new System.Drawing.Size(88, 13);
      this.lblFrom.TabIndex = 6;
      this.lblFrom.Text = "From Routine: ??";
      // 
      // grpPreview
      // 
      this.grpPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grpPreview.Controls.Add(this.lisResultSets);
      this.grpPreview.Controls.Add(this.grdPreview);
      this.grpPreview.Location = new System.Drawing.Point(212, 25);
      this.grpPreview.Name = "grpPreview";
      this.grpPreview.Size = new System.Drawing.Size(570, 239);
      this.grpPreview.TabIndex = 5;
      this.grpPreview.TabStop = false;
      this.grpPreview.Text = "Data Preview";
      // 
      // lisResultSets
      // 
      this.lisResultSets.FormattingEnabled = true;
      this.lisResultSets.Location = new System.Drawing.Point(6, 16);
      this.lisResultSets.Name = "lisResultSets";
      this.lisResultSets.Size = new System.Drawing.Size(80, 212);
      this.lisResultSets.TabIndex = 1;
      this.lisResultSets.SelectedIndexChanged += new System.EventHandler(this.lisResultSets_SelectedIndexChanged);
      // 
      // grdPreview
      // 
      this.grdPreview.AllowUserToAddRows = false;
      this.grdPreview.AllowUserToDeleteRows = false;
      this.grdPreview.AllowUserToResizeColumns = false;
      this.grdPreview.AllowUserToResizeRows = false;
      this.grdPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.grdPreview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.grdPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdPreview.Location = new System.Drawing.Point(92, 16);
      this.grdPreview.Name = "grdPreview";
      this.grdPreview.ReadOnly = true;
      this.grdPreview.RowHeadersVisible = false;
      this.grdPreview.Size = new System.Drawing.Size(475, 212);
      this.grdPreview.TabIndex = 0;
      // 
      // parametersGrid
      // 
      this.parametersGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.parametersGrid.Location = new System.Drawing.Point(3, 16);
      this.parametersGrid.Name = "parametersGrid";
      this.parametersGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
      this.parametersGrid.Size = new System.Drawing.Size(188, 239);
      this.parametersGrid.TabIndex = 10;
      this.parametersGrid.ToolbarVisible = false;
      // 
      // grpParameters
      // 
      this.grpParameters.Controls.Add(this.btnCall);
      this.grpParameters.Controls.Add(this.parametersGrid);
      this.grpParameters.Location = new System.Drawing.Point(12, 25);
      this.grpParameters.Name = "grpParameters";
      this.grpParameters.Size = new System.Drawing.Size(194, 290);
      this.grpParameters.TabIndex = 11;
      this.grpParameters.TabStop = false;
      this.grpParameters.Text = "Parameters";
      // 
      // btnCall
      // 
      this.btnCall.Location = new System.Drawing.Point(113, 261);
      this.btnCall.Name = "btnCall";
      this.btnCall.Size = new System.Drawing.Size(75, 23);
      this.btnCall.TabIndex = 1;
      this.btnCall.Text = "Call";
      this.btnCall.UseVisualStyleBackColor = true;
      this.btnCall.Click += new System.EventHandler(this.btnCall_Click);
      // 
      // ImportRoutineDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(794, 356);
      this.ControlBox = false;
      this.Controls.Add(this.grpParameters);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnImport);
      this.Controls.Add(this.grpOptions);
      this.Controls.Add(this.lblFrom);
      this.Controls.Add(this.grpPreview);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "ImportRoutineDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Import Data";
      this.grpOptions.ResumeLayout(false);
      this.grpOptions.PerformLayout();
      this.grpPreview.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.grdPreview)).EndInit();
      this.grpParameters.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnImport;
    private System.Windows.Forms.GroupBox grpOptions;
    private System.Windows.Forms.CheckBox chkIncludeHeaders;
    private System.Windows.Forms.Label lblFrom;
    private System.Windows.Forms.GroupBox grpPreview;
    private System.Windows.Forms.DataGridView grdPreview;
    private System.Windows.Forms.PropertyGrid parametersGrid;
    private System.Windows.Forms.GroupBox grpParameters;
    private System.Windows.Forms.Button btnCall;
    private System.Windows.Forms.ListBox lisResultSets;
    private System.Windows.Forms.ComboBox cmbMultipleResultSets;
    private System.Windows.Forms.Label lblMultipleResultSets;
  }
}