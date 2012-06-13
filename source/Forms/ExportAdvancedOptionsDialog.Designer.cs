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
      this.AdvancedExportOptionsPanel = new System.Windows.Forms.Panel();
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
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnAccept = new System.Windows.Forms.Button();
      this.AdvancedExportOptionsPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // AdvancedExportOptionsPanel
      // 
      this.AdvancedExportOptionsPanel.BackColor = System.Drawing.SystemColors.Window;
      this.AdvancedExportOptionsPanel.Controls.Add(this.chkShowCopySQLButton);
      this.AdvancedExportOptionsPanel.Controls.Add(this.lblOtherOptions);
      this.AdvancedExportOptionsPanel.Controls.Add(this.chkUseFormattedValues);
      this.AdvancedExportOptionsPanel.Controls.Add(this.lblFieldDataOptions);
      this.AdvancedExportOptionsPanel.Controls.Add(this.chkAutoAllowEmptyNonIndexColumns);
      this.AdvancedExportOptionsPanel.Controls.Add(this.chkAutoIndexIntColumns);
      this.AdvancedExportOptionsPanel.Controls.Add(this.chkAddBufferToVarchar);
      this.AdvancedExportOptionsPanel.Controls.Add(this.chkDetectDatatype);
      this.AdvancedExportOptionsPanel.Controls.Add(this.lblColumnDatatypeOptions);
      this.AdvancedExportOptionsPanel.Controls.Add(this.lblAdvancedExportOptions);
      this.AdvancedExportOptionsPanel.Location = new System.Drawing.Point(0, 0);
      this.AdvancedExportOptionsPanel.Name = "AdvancedExportOptionsPanel";
      this.AdvancedExportOptionsPanel.Size = new System.Drawing.Size(525, 330);
      this.AdvancedExportOptionsPanel.TabIndex = 0;
      // 
      // chkShowCopySQLButton
      // 
      this.chkShowCopySQLButton.AutoSize = true;
      this.chkShowCopySQLButton.Location = new System.Drawing.Point(48, 285);
      this.chkShowCopySQLButton.Name = "chkShowCopySQLButton";
      this.chkShowCopySQLButton.Size = new System.Drawing.Size(138, 17);
      this.chkShowCopySQLButton.TabIndex = 15;
      this.chkShowCopySQLButton.Text = "Show Copy SQL Button";
      this.chkShowCopySQLButton.UseVisualStyleBackColor = true;
      // 
      // lblOtherOptions
      // 
      this.lblOtherOptions.AutoSize = true;
      this.lblOtherOptions.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblOtherOptions.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblOtherOptions.Location = new System.Drawing.Point(19, 256);
      this.lblOtherOptions.Name = "lblOtherOptions";
      this.lblOtherOptions.Size = new System.Drawing.Size(91, 17);
      this.lblOtherOptions.TabIndex = 14;
      this.lblOtherOptions.Text = "Other Options";
      // 
      // chkUseFormattedValues
      // 
      this.chkUseFormattedValues.AutoSize = true;
      this.chkUseFormattedValues.Location = new System.Drawing.Point(48, 218);
      this.chkUseFormattedValues.Name = "chkUseFormattedValues";
      this.chkUseFormattedValues.Size = new System.Drawing.Size(126, 17);
      this.chkUseFormattedValues.TabIndex = 11;
      this.chkUseFormattedValues.Text = "Use formatted values";
      this.chkUseFormattedValues.UseVisualStyleBackColor = true;
      // 
      // lblFieldDataOptions
      // 
      this.lblFieldDataOptions.AutoSize = true;
      this.lblFieldDataOptions.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblFieldDataOptions.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblFieldDataOptions.Location = new System.Drawing.Point(19, 189);
      this.lblFieldDataOptions.Name = "lblFieldDataOptions";
      this.lblFieldDataOptions.Size = new System.Drawing.Size(116, 17);
      this.lblFieldDataOptions.TabIndex = 10;
      this.lblFieldDataOptions.Text = "Field Data Options";
      // 
      // chkAutoAllowEmptyNonIndexColumns
      // 
      this.chkAutoAllowEmptyNonIndexColumns.AutoSize = true;
      this.chkAutoAllowEmptyNonIndexColumns.Location = new System.Drawing.Point(48, 150);
      this.chkAutoAllowEmptyNonIndexColumns.Name = "chkAutoAllowEmptyNonIndexColumns";
      this.chkAutoAllowEmptyNonIndexColumns.Size = new System.Drawing.Size(386, 17);
      this.chkAutoAllowEmptyNonIndexColumns.TabIndex = 9;
      this.chkAutoAllowEmptyNonIndexColumns.Text = "Automatically check the Allow Empty checkbox for columns without an index";
      this.chkAutoAllowEmptyNonIndexColumns.UseVisualStyleBackColor = true;
      // 
      // chkAutoIndexIntColumns
      // 
      this.chkAutoIndexIntColumns.AutoSize = true;
      this.chkAutoIndexIntColumns.Location = new System.Drawing.Point(48, 127);
      this.chkAutoIndexIntColumns.Name = "chkAutoIndexIntColumns";
      this.chkAutoIndexIntColumns.Size = new System.Drawing.Size(311, 17);
      this.chkAutoIndexIntColumns.TabIndex = 8;
      this.chkAutoIndexIntColumns.Text = "Automatically check the Index checkbox for Integer columns";
      this.chkAutoIndexIntColumns.UseVisualStyleBackColor = true;
      // 
      // chkAddBufferToVarchar
      // 
      this.chkAddBufferToVarchar.AutoSize = true;
      this.chkAddBufferToVarchar.Location = new System.Drawing.Point(48, 104);
      this.chkAddBufferToVarchar.Name = "chkAddBufferToVarchar";
      this.chkAddBufferToVarchar.Size = new System.Drawing.Size(384, 17);
      this.chkAddBufferToVarchar.TabIndex = 7;
      this.chkAddBufferToVarchar.Text = "Add additional buffer to VARCHAR length (round up to 12, 25, 45, 125, 255)";
      this.chkAddBufferToVarchar.UseVisualStyleBackColor = true;
      // 
      // chkDetectDatatype
      // 
      this.chkDetectDatatype.AutoSize = true;
      this.chkDetectDatatype.Location = new System.Drawing.Point(48, 81);
      this.chkDetectDatatype.Name = "chkDetectDatatype";
      this.chkDetectDatatype.Size = new System.Drawing.Size(373, 17);
      this.chkDetectDatatype.TabIndex = 6;
      this.chkDetectDatatype.Text = "Analyze and try to detect correct datatype based on column field contents";
      this.chkDetectDatatype.UseVisualStyleBackColor = true;
      // 
      // lblColumnDatatypeOptions
      // 
      this.lblColumnDatatypeOptions.AutoSize = true;
      this.lblColumnDatatypeOptions.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblColumnDatatypeOptions.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblColumnDatatypeOptions.Location = new System.Drawing.Point(19, 52);
      this.lblColumnDatatypeOptions.Name = "lblColumnDatatypeOptions";
      this.lblColumnDatatypeOptions.Size = new System.Drawing.Size(158, 17);
      this.lblColumnDatatypeOptions.TabIndex = 2;
      this.lblColumnDatatypeOptions.Text = "Column Datatype Options";
      // 
      // lblAdvancedExportOptions
      // 
      this.lblAdvancedExportOptions.AutoSize = true;
      this.lblAdvancedExportOptions.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblAdvancedExportOptions.ForeColor = System.Drawing.Color.Navy;
      this.lblAdvancedExportOptions.Location = new System.Drawing.Point(18, 18);
      this.lblAdvancedExportOptions.Name = "lblAdvancedExportOptions";
      this.lblAdvancedExportOptions.Size = new System.Drawing.Size(178, 20);
      this.lblAdvancedExportOptions.TabIndex = 1;
      this.lblAdvancedExportOptions.Text = "Advanced Export Options";
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(437, 341);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnAccept
      // 
      this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnAccept.Location = new System.Drawing.Point(356, 341);
      this.btnAccept.Name = "btnAccept";
      this.btnAccept.Size = new System.Drawing.Size(75, 23);
      this.btnAccept.TabIndex = 2;
      this.btnAccept.Text = "Accept";
      this.btnAccept.UseVisualStyleBackColor = true;
      this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
      // 
      // ExportAdvancedOptionsDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(524, 376);
      this.Controls.Add(this.btnAccept);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.AdvancedExportOptionsPanel);
      this.MaximizeBox = false;
      this.MaximumSize = new System.Drawing.Size(540, 414);
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(540, 414);
      this.Name = "ExportAdvancedOptionsDialog";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Advanced Options";
      this.AdvancedExportOptionsPanel.ResumeLayout(false);
      this.AdvancedExportOptionsPanel.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel AdvancedExportOptionsPanel;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnAccept;
    private System.Windows.Forms.Label lblAdvancedExportOptions;
    private System.Windows.Forms.Label lblColumnDatatypeOptions;
    private System.Windows.Forms.CheckBox chkUseFormattedValues;
    private System.Windows.Forms.Label lblFieldDataOptions;
    private System.Windows.Forms.CheckBox chkAutoAllowEmptyNonIndexColumns;
    private System.Windows.Forms.CheckBox chkAutoIndexIntColumns;
    private System.Windows.Forms.CheckBox chkAddBufferToVarchar;
    private System.Windows.Forms.CheckBox chkDetectDatatype;
    private System.Windows.Forms.CheckBox chkShowCopySQLButton;
    private System.Windows.Forms.Label lblOtherOptions;
  }
}