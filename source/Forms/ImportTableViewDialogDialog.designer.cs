namespace MySQL.ExcelAddIn
{
  partial class ImportTableViewDialog
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
      this.grpPreview = new System.Windows.Forms.GroupBox();
      this.grdPreview = new System.Windows.Forms.DataGridView();
      this.lblFrom = new System.Windows.Forms.Label();
      this.grpOptions = new System.Windows.Forms.GroupBox();
      this.btnSelect = new System.Windows.Forms.Button();
      this.chkLimitRows = new System.Windows.Forms.CheckBox();
      this.chkIncludeHeaders = new System.Windows.Forms.CheckBox();
      this.numRowsCount = new System.Windows.Forms.NumericUpDown();
      this.lblToRow = new System.Windows.Forms.Label();
      this.numFromRow = new System.Windows.Forms.NumericUpDown();
      this.lblFromRow = new System.Windows.Forms.Label();
      this.btnImport = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.lblRowsCount = new System.Windows.Forms.Label();
      this.grpPreview.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreview)).BeginInit();
      this.grpOptions.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numRowsCount)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numFromRow)).BeginInit();
      this.SuspendLayout();
      // 
      // grpPreview
      // 
      this.grpPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grpPreview.Controls.Add(this.grdPreview);
      this.grpPreview.Location = new System.Drawing.Point(12, 25);
      this.grpPreview.Name = "grpPreview";
      this.grpPreview.Size = new System.Drawing.Size(704, 214);
      this.grpPreview.TabIndex = 1;
      this.grpPreview.TabStop = false;
      this.grpPreview.Text = "Data Preview";
      // 
      // grdPreview
      // 
      this.grdPreview.AllowUserToAddRows = false;
      this.grdPreview.AllowUserToDeleteRows = false;
      this.grdPreview.AllowUserToResizeColumns = false;
      this.grdPreview.AllowUserToResizeRows = false;
      this.grdPreview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.grdPreview.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.grdPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ScrollBar;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdPreview.DefaultCellStyle = dataGridViewCellStyle2;
      this.grdPreview.Dock = System.Windows.Forms.DockStyle.Fill;
      this.grdPreview.Location = new System.Drawing.Point(3, 16);
      this.grdPreview.Name = "grdPreview";
      this.grdPreview.ReadOnly = true;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.grdPreview.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
      this.grdPreview.RowHeadersVisible = false;
      this.grdPreview.Size = new System.Drawing.Size(698, 195);
      this.grdPreview.TabIndex = 0;
      this.grdPreview.ColumnStateChanged += new System.Windows.Forms.DataGridViewColumnStateChangedEventHandler(this.grdPreview_ColumnStateChanged);
      this.grdPreview.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grdPreview_DataBindingComplete);
      this.grdPreview.SelectionChanged += new System.EventHandler(this.grdPreview_SelectionChanged);
      // 
      // lblFrom
      // 
      this.lblFrom.AutoSize = true;
      this.lblFrom.Location = new System.Drawing.Point(12, 9);
      this.lblFrom.Name = "lblFrom";
      this.lblFrom.Size = new System.Drawing.Size(78, 13);
      this.lblFrom.TabIndex = 0;
      this.lblFrom.Text = "From Table: ??";
      // 
      // grpOptions
      // 
      this.grpOptions.Controls.Add(this.btnSelect);
      this.grpOptions.Controls.Add(this.chkLimitRows);
      this.grpOptions.Controls.Add(this.chkIncludeHeaders);
      this.grpOptions.Controls.Add(this.numRowsCount);
      this.grpOptions.Controls.Add(this.lblToRow);
      this.grpOptions.Controls.Add(this.numFromRow);
      this.grpOptions.Controls.Add(this.lblFromRow);
      this.grpOptions.Location = new System.Drawing.Point(12, 245);
      this.grpOptions.Name = "grpOptions";
      this.grpOptions.Size = new System.Drawing.Size(704, 70);
      this.grpOptions.TabIndex = 2;
      this.grpOptions.TabStop = false;
      this.grpOptions.Text = "Options";
      // 
      // btnSelect
      // 
      this.btnSelect.Location = new System.Drawing.Point(548, 15);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new System.Drawing.Size(150, 23);
      this.btnSelect.TabIndex = 1;
      this.btnSelect.Text = "Select All";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
      // 
      // chkLimitRows
      // 
      this.chkLimitRows.AutoSize = true;
      this.chkLimitRows.Location = new System.Drawing.Point(6, 42);
      this.chkLimitRows.Name = "chkLimitRows";
      this.chkLimitRows.Size = new System.Drawing.Size(77, 17);
      this.chkLimitRows.TabIndex = 2;
      this.chkLimitRows.Text = "Limit Rows";
      this.chkLimitRows.UseVisualStyleBackColor = true;
      this.chkLimitRows.CheckedChanged += new System.EventHandler(this.chkLimitRows_CheckedChanged);
      // 
      // chkIncludeHeaders
      // 
      this.chkIncludeHeaders.AutoSize = true;
      this.chkIncludeHeaders.Location = new System.Drawing.Point(6, 19);
      this.chkIncludeHeaders.Name = "chkIncludeHeaders";
      this.chkIncludeHeaders.Size = new System.Drawing.Size(192, 17);
      this.chkIncludeHeaders.TabIndex = 0;
      this.chkIncludeHeaders.Text = "Include Column Names as Headers";
      this.chkIncludeHeaders.UseVisualStyleBackColor = true;
      // 
      // numRowsCount
      // 
      this.numRowsCount.Enabled = false;
      this.numRowsCount.Location = new System.Drawing.Point(450, 41);
      this.numRowsCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
      this.numRowsCount.Name = "numRowsCount";
      this.numRowsCount.Size = new System.Drawing.Size(63, 20);
      this.numRowsCount.TabIndex = 6;
      // 
      // lblToRow
      // 
      this.lblToRow.AutoSize = true;
      this.lblToRow.Location = new System.Drawing.Point(308, 43);
      this.lblToRow.Name = "lblToRow";
      this.lblToRow.Size = new System.Drawing.Size(136, 13);
      this.lblToRow.TabIndex = 5;
      this.lblToRow.Text = "Number of Rows to Return:";
      // 
      // numFromRow
      // 
      this.numFromRow.Enabled = false;
      this.numFromRow.Location = new System.Drawing.Point(207, 41);
      this.numFromRow.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numFromRow.Name = "numFromRow";
      this.numFromRow.Size = new System.Drawing.Size(63, 20);
      this.numFromRow.TabIndex = 4;
      this.numFromRow.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // lblFromRow
      // 
      this.lblFromRow.AutoSize = true;
      this.lblFromRow.Location = new System.Drawing.Point(143, 43);
      this.lblFromRow.Name = "lblFromRow";
      this.lblFromRow.Size = new System.Drawing.Size(58, 13);
      this.lblFromRow.TabIndex = 3;
      this.lblFromRow.Text = "From Row:";
      // 
      // btnImport
      // 
      this.btnImport.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnImport.Location = new System.Drawing.Point(560, 321);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new System.Drawing.Size(75, 23);
      this.btnImport.TabIndex = 3;
      this.btnImport.Text = "Import";
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(641, 321);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // lblRowsCount
      // 
      this.lblRowsCount.AutoSize = true;
      this.lblRowsCount.Location = new System.Drawing.Point(583, 9);
      this.lblRowsCount.Name = "lblRowsCount";
      this.lblRowsCount.Size = new System.Drawing.Size(110, 13);
      this.lblRowsCount.TabIndex = 5;
      this.lblRowsCount.Text = "Total Rows Count: ??";
      this.lblRowsCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // ImportTableViewDialog
      // 
      this.AcceptButton = this.btnImport;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(728, 355);
      this.ControlBox = false;
      this.Controls.Add(this.lblRowsCount);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnImport);
      this.Controls.Add(this.grpOptions);
      this.Controls.Add(this.lblFrom);
      this.Controls.Add(this.grpPreview);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "ImportTableViewDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Import Data";
      this.grpPreview.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.grdPreview)).EndInit();
      this.grpOptions.ResumeLayout(false);
      this.grpOptions.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numRowsCount)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numFromRow)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox grpPreview;
    private System.Windows.Forms.Label lblFrom;
    private System.Windows.Forms.DataGridView grdPreview;
    private System.Windows.Forms.GroupBox grpOptions;
    private System.Windows.Forms.Label lblToRow;
    private System.Windows.Forms.NumericUpDown numFromRow;
    private System.Windows.Forms.Label lblFromRow;
    private System.Windows.Forms.CheckBox chkIncludeHeaders;
    private System.Windows.Forms.NumericUpDown numRowsCount;
    private System.Windows.Forms.Button btnImport;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.CheckBox chkLimitRows;
    private System.Windows.Forms.Button btnSelect;
    private System.Windows.Forms.Label lblRowsCount;
  }
}