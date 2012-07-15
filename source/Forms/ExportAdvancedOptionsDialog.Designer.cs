namespace MySQL.ForExcel
{
  partial class ExportAdvancedOptionsDialog
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
      this.btnAccept = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.chkShowCopySQLButton = new System.Windows.Forms.CheckBox();
      this.lblOtherOptions = new System.Windows.Forms.Label();
      this.chkUseFormattedValues = new System.Windows.Forms.CheckBox();
      this.lblFieldDataOptions = new System.Windows.Forms.Label();
      this.chkAutoAllowEmptyNonIndexColumns = new System.Windows.Forms.CheckBox();
      this.chkAutoIndexIntColumns = new System.Windows.Forms.CheckBox();
      this.chkAddBufferToVarchar = new System.Windows.Forms.CheckBox();
      this.chkDetectDatatype = new System.Windows.Forms.CheckBox();
      this.lblColumnDatatypeOptions = new System.Windows.Forms.Label();
      this.lblAdvancedExportOptions = new System.Windows.Forms.Label();
      this.contentAreaPanel.SuspendLayout();
      this.commandAreaPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // contentAreaPanel
      // 
      this.contentAreaPanel.BackColor = System.Drawing.SystemColors.Window;
      this.contentAreaPanel.Controls.Add(this.lblAdvancedExportOptions);
      this.contentAreaPanel.Controls.Add(this.chkShowCopySQLButton);
      this.contentAreaPanel.Controls.Add(this.lblOtherOptions);
      this.contentAreaPanel.Controls.Add(this.chkUseFormattedValues);
      this.contentAreaPanel.Controls.Add(this.lblFieldDataOptions);
      this.contentAreaPanel.Controls.Add(this.chkAutoAllowEmptyNonIndexColumns);
      this.contentAreaPanel.Controls.Add(this.chkAutoIndexIntColumns);
      this.contentAreaPanel.Controls.Add(this.chkAddBufferToVarchar);
      this.contentAreaPanel.Controls.Add(this.chkDetectDatatype);
      this.contentAreaPanel.Controls.Add(this.lblColumnDatatypeOptions);
      this.contentAreaPanel.Size = new System.Drawing.Size(544, 269);
      // 
      // commandAreaPanel
      // 
      this.commandAreaPanel.Controls.Add(this.btnAccept);
      this.commandAreaPanel.Controls.Add(this.btnCancel);
      this.commandAreaPanel.Location = new System.Drawing.Point(0, 269);
      this.commandAreaPanel.Size = new System.Drawing.Size(544, 45);
      // 
      // btnAccept
      // 
      this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnAccept.Location = new System.Drawing.Point(366, 11);
      this.btnAccept.Name = "btnAccept";
      this.btnAccept.Size = new System.Drawing.Size(75, 23);
      this.btnAccept.TabIndex = 0;
      this.btnAccept.Text = "Accept";
      this.btnAccept.UseVisualStyleBackColor = true;
      this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(447, 11);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // chkShowCopySQLButton
      // 
      this.chkShowCopySQLButton.AutoSize = true;
      this.chkShowCopySQLButton.BackColor = System.Drawing.Color.Transparent;
      this.chkShowCopySQLButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkShowCopySQLButton.Location = new System.Drawing.Point(53, 294);
      this.chkShowCopySQLButton.Name = "chkShowCopySQLButton";
      this.chkShowCopySQLButton.Size = new System.Drawing.Size(152, 19);
      this.chkShowCopySQLButton.TabIndex = 9;
      this.chkShowCopySQLButton.Text = "Show Copy SQL Button";
      this.chkShowCopySQLButton.UseVisualStyleBackColor = false;
      this.chkShowCopySQLButton.Visible = false;
      // 
      // lblOtherOptions
      // 
      this.lblOtherOptions.AutoSize = true;
      this.lblOtherOptions.BackColor = System.Drawing.Color.Transparent;
      this.lblOtherOptions.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblOtherOptions.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblOtherOptions.Location = new System.Drawing.Point(24, 265);
      this.lblOtherOptions.Name = "lblOtherOptions";
      this.lblOtherOptions.Size = new System.Drawing.Size(91, 17);
      this.lblOtherOptions.TabIndex = 8;
      this.lblOtherOptions.Text = "Other Options";
      this.lblOtherOptions.Visible = false;
      // 
      // chkUseFormattedValues
      // 
      this.chkUseFormattedValues.AutoSize = true;
      this.chkUseFormattedValues.BackColor = System.Drawing.Color.Transparent;
      this.chkUseFormattedValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkUseFormattedValues.Location = new System.Drawing.Point(53, 228);
      this.chkUseFormattedValues.Name = "chkUseFormattedValues";
      this.chkUseFormattedValues.Size = new System.Drawing.Size(141, 19);
      this.chkUseFormattedValues.TabIndex = 7;
      this.chkUseFormattedValues.Text = "Use formatted values";
      this.chkUseFormattedValues.UseVisualStyleBackColor = false;
      // 
      // lblFieldDataOptions
      // 
      this.lblFieldDataOptions.AutoSize = true;
      this.lblFieldDataOptions.BackColor = System.Drawing.Color.Transparent;
      this.lblFieldDataOptions.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblFieldDataOptions.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblFieldDataOptions.Location = new System.Drawing.Point(24, 199);
      this.lblFieldDataOptions.Name = "lblFieldDataOptions";
      this.lblFieldDataOptions.Size = new System.Drawing.Size(116, 17);
      this.lblFieldDataOptions.TabIndex = 6;
      this.lblFieldDataOptions.Text = "Field Data Options";
      // 
      // chkAutoAllowEmptyNonIndexColumns
      // 
      this.chkAutoAllowEmptyNonIndexColumns.AutoSize = true;
      this.chkAutoAllowEmptyNonIndexColumns.BackColor = System.Drawing.Color.Transparent;
      this.chkAutoAllowEmptyNonIndexColumns.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkAutoAllowEmptyNonIndexColumns.Location = new System.Drawing.Point(53, 160);
      this.chkAutoAllowEmptyNonIndexColumns.Name = "chkAutoAllowEmptyNonIndexColumns";
      this.chkAutoAllowEmptyNonIndexColumns.Size = new System.Drawing.Size(436, 19);
      this.chkAutoAllowEmptyNonIndexColumns.TabIndex = 5;
      this.chkAutoAllowEmptyNonIndexColumns.Text = "Automatically check the Allow Empty checkbox for columns without an index";
      this.chkAutoAllowEmptyNonIndexColumns.UseVisualStyleBackColor = false;
      // 
      // chkAutoIndexIntColumns
      // 
      this.chkAutoIndexIntColumns.AutoSize = true;
      this.chkAutoIndexIntColumns.BackColor = System.Drawing.Color.Transparent;
      this.chkAutoIndexIntColumns.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkAutoIndexIntColumns.Location = new System.Drawing.Point(53, 135);
      this.chkAutoIndexIntColumns.Name = "chkAutoIndexIntColumns";
      this.chkAutoIndexIntColumns.Size = new System.Drawing.Size(349, 19);
      this.chkAutoIndexIntColumns.TabIndex = 4;
      this.chkAutoIndexIntColumns.Text = "Automatically check the Index checkbox for Integer columns";
      this.chkAutoIndexIntColumns.UseVisualStyleBackColor = false;
      // 
      // chkAddBufferToVarchar
      // 
      this.chkAddBufferToVarchar.AutoSize = true;
      this.chkAddBufferToVarchar.BackColor = System.Drawing.Color.Transparent;
      this.chkAddBufferToVarchar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkAddBufferToVarchar.Location = new System.Drawing.Point(73, 110);
      this.chkAddBufferToVarchar.Name = "chkAddBufferToVarchar";
      this.chkAddBufferToVarchar.Size = new System.Drawing.Size(431, 19);
      this.chkAddBufferToVarchar.TabIndex = 3;
      this.chkAddBufferToVarchar.Text = "Add additional buffer to VARCHAR length (round up to 12, 25, 45, 125, 255)";
      this.chkAddBufferToVarchar.UseVisualStyleBackColor = false;
      // 
      // chkDetectDatatype
      // 
      this.chkDetectDatatype.AutoSize = true;
      this.chkDetectDatatype.BackColor = System.Drawing.Color.Transparent;
      this.chkDetectDatatype.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkDetectDatatype.Location = new System.Drawing.Point(53, 85);
      this.chkDetectDatatype.Name = "chkDetectDatatype";
      this.chkDetectDatatype.Size = new System.Drawing.Size(418, 19);
      this.chkDetectDatatype.TabIndex = 2;
      this.chkDetectDatatype.Text = "Analyze and try to detect correct datatype based on column field contents";
      this.chkDetectDatatype.UseVisualStyleBackColor = false;
      this.chkDetectDatatype.CheckedChanged += new System.EventHandler(this.chkDetectDatatype_CheckedChanged);
      // 
      // lblColumnDatatypeOptions
      // 
      this.lblColumnDatatypeOptions.AutoSize = true;
      this.lblColumnDatatypeOptions.BackColor = System.Drawing.Color.Transparent;
      this.lblColumnDatatypeOptions.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblColumnDatatypeOptions.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblColumnDatatypeOptions.Location = new System.Drawing.Point(24, 56);
      this.lblColumnDatatypeOptions.Name = "lblColumnDatatypeOptions";
      this.lblColumnDatatypeOptions.Size = new System.Drawing.Size(158, 17);
      this.lblColumnDatatypeOptions.TabIndex = 1;
      this.lblColumnDatatypeOptions.Text = "Column Datatype Options";
      // 
      // lblAdvancedExportOptions
      // 
      this.lblAdvancedExportOptions.AutoSize = true;
      this.lblAdvancedExportOptions.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblAdvancedExportOptions.ForeColor = System.Drawing.Color.Navy;
      this.lblAdvancedExportOptions.Location = new System.Drawing.Point(17, 17);
      this.lblAdvancedExportOptions.Name = "lblAdvancedExportOptions";
      this.lblAdvancedExportOptions.Size = new System.Drawing.Size(178, 20);
      this.lblAdvancedExportOptions.TabIndex = 0;
      this.lblAdvancedExportOptions.Text = "Advanced Export Options";
      // 
      // ExportAdvancedOptionsDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(544, 316);
      this.CommandAreaHeight = 45;
      this.MainInstructionLocation = new System.Drawing.Point(13, 21);
      this.Name = "ExportAdvancedOptionsDialog";
      this.Text = "Advanced Options";
      this.contentAreaPanel.ResumeLayout(false);
      this.contentAreaPanel.PerformLayout();
      this.commandAreaPanel.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnAccept;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.CheckBox chkShowCopySQLButton;
    private System.Windows.Forms.Label lblOtherOptions;
    private System.Windows.Forms.CheckBox chkUseFormattedValues;
    private System.Windows.Forms.Label lblFieldDataOptions;
    private System.Windows.Forms.CheckBox chkAutoAllowEmptyNonIndexColumns;
    private System.Windows.Forms.CheckBox chkAutoIndexIntColumns;
    private System.Windows.Forms.CheckBox chkAddBufferToVarchar;
    private System.Windows.Forms.CheckBox chkDetectDatatype;
    private System.Windows.Forms.Label lblColumnDatatypeOptions;
    private System.Windows.Forms.Label lblAdvancedExportOptions;
  }
}