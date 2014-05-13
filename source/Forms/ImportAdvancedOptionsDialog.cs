// Copyright (c) 2013-2014, Oracle and/or its affiliates. All rights reserved.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA

using System;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Forms;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Advanced options dialog for the operations performed by the <see cref="ExportDataForm"/>.
  /// </summary>
  public partial class ImportAdvancedOptionsDialog : AutoStyleableBaseDialog
  {
    /// <summary>
    /// Gets or sets a value indicating whether the data in the parent form needs to be reloaded on the grids.
    /// </summary>
    /// <value>
    ///   <c>true</c> if requires refreshing; otherwise, <c>false</c>.
    /// </value>
    public bool ParentFormRequiresRefresh { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportAdvancedOptionsDialog"/> class.
    /// </summary>
    public ImportAdvancedOptionsDialog()
    {
      ParentFormRequiresRefresh = false;
      InitializeComponent();
      RefreshControlValues();
      SetExcelTableControlsAvailability();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="CreateExcelTableCheckbox"/> checkbox is checked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void CreateExcelTableCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      SetExcelTableControlsAvailability();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ImportAdvancedOptionsDialog"/> is being closed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportAdvancedOptionsDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.Cancel)
      {
        return;
      }

      var previewRowsQuantity = (int)PreviewRowsQuantityNumericUpDown.Value;
      ParentFormRequiresRefresh = Settings.Default.ImportPreviewRowsQuantity != previewRowsQuantity;

      Settings.Default.ImportPreviewRowsQuantity = previewRowsQuantity;
      Settings.Default.ImportEscapeFormulaTextValues = EscapeFormulaValuesCheckBox.Checked;
      Settings.Default.ImportCreateExcelTable = CreateExcelTableCheckbox.Checked;
      Settings.Default.ImportExcelTableStyleName = UseStyleComboBox.Text;
      Settings.Default.ImportPrefixExcelTable = PrefixExcelTablesCheckBox.Checked;
      Settings.Default.ImportPrefixExcelTableText = PrefixExcelTablesTextBox.Text;
      MiscUtilities.SaveSettings();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PrefixExcelTablesCheckBox"/> checkbox is checked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PrefixExcelTablesCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      PrefixExcelTablesTextBox.ReadOnly = !(CreateExcelTableCheckbox.Checked && PrefixExcelTablesCheckBox.Checked);
    }

    /// <summary>
    /// Refreshes the dialog controls' values.
    /// </summary>
    /// <param name="useDefaultValues">Controls are set to their default values if <c>true</c>. Current stored values in application settings are used otherwise.</param>
    private void RefreshControlValues(bool useDefaultValues = false)
    {
      if (useDefaultValues)
      {
        var settings = Settings.Default;
        PreviewRowsQuantityNumericUpDown.Value = settings.GetPropertyDefaultValueByName<int>("ImportPreviewRowsQuantity");
        EscapeFormulaValuesCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("ImportEscapeFormulaTextValues");
        CreateExcelTableCheckbox.Checked = settings.GetPropertyDefaultValueByName<bool>("ImportCreateExcelTable");
        UseStyleComboBox.DataSource = Globals.ThisAddIn.Application.ActiveWorkbook.ListTableStyles();
        UseStyleComboBox.Text = settings.GetPropertyDefaultValueByName<string>("ImportExcelTableStyleName");
        PrefixExcelTablesCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("ImportPrefixExcelTable");
        PrefixExcelTablesTextBox.Text = settings.GetPropertyDefaultValueByName<string>("ImportPrefixExcelTableText");
      }
      else
      {
        PreviewRowsQuantityNumericUpDown.Value = Math.Min(PreviewRowsQuantityNumericUpDown.Maximum, Settings.Default.ImportPreviewRowsQuantity);
        EscapeFormulaValuesCheckBox.Checked = Settings.Default.ImportEscapeFormulaTextValues;
        CreateExcelTableCheckbox.Checked = Settings.Default.ImportCreateExcelTable;
        UseStyleComboBox.DataSource = Globals.ThisAddIn.Application.ActiveWorkbook.ListTableStyles();
        UseStyleComboBox.Text = Settings.Default.ImportExcelTableStyleName;
        PrefixExcelTablesCheckBox.Checked = Settings.Default.ImportPrefixExcelTable;
        PrefixExcelTablesTextBox.Text = Settings.Default.ImportPrefixExcelTableText;
      }
    }

    /// <summary>
    /// Handles the Click event of the ResetToDefaultsButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void ResetToDefaultsButton_Click(object sender, EventArgs e)
    {
      RefreshControlValues(true);
      Refresh();
    }

    /// <summary>
    /// Set sthe availability of the Excel table creation controls.
    /// </summary>
    private void SetExcelTableControlsAvailability()
    {
      UseStyle1Label.Enabled = CreateExcelTableCheckbox.Checked;
      UseStyle2Label.Enabled = CreateExcelTableCheckbox.Checked;
      UseStyleComboBox.Enabled = CreateExcelTableCheckbox.Checked;
      PrefixExcelTablesCheckBox.Enabled = CreateExcelTableCheckbox.Checked;
      PrefixExcelTablesTextBox.ReadOnly = !(CreateExcelTableCheckbox.Checked && PrefixExcelTablesCheckBox.Checked);
      if (!CreateExcelTableCheckbox.Checked)
      {
        UseStyleComboBox.Text = ExcelUtilities.DEFAULT_MYSQL_STYLE_NAME;
      }
    }
  }
}