// Copyright (c) 2012, 2019, Oracle and/or its affiliates. All rights reserved.
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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using MySql.Utility.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Properties;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Provides an interface to let users input a column mapping name that will be saved to file.
  /// </summary>
  public partial class AppendNewColumnMappingDialog : ValidatingBaseDialog
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="AppendNewColumnMappingDialog"/> class.
    /// </summary>
    /// <param name="mappings">A list of column mappings for the current user.</param>
    /// <param name="forMySqlTableName">The name of the MySQL table this mapping corresponds to.</param>
    public AppendNewColumnMappingDialog(List<MySqlColumnMapping> mappings, string forMySqlTableName)
    {
      ForMySqlTableName = forMySqlTableName;
      Mappings = mappings;
      InitializeComponent();
      ColumnMappingName = GetProposedNewMappingName();
      MappingNameTextBox.SelectAll();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppendNewColumnMappingDialog"/> class.
    /// </summary>
    public AppendNewColumnMappingDialog()
      : this(null, string.Empty)
    {
    }

    #region Properties

    /// <summary>
    /// Gets or sets the name of the column mapping that will be saved to file.
    /// </summary>
    public string ColumnMappingName
    {
      get => MappingNameTextBox.Text.Trim();
      set => MappingNameTextBox.Text = value;
    }

    /// <summary>
    /// Gets a list of column mappings for the current user.
    /// </summary>
    public List<MySqlColumnMapping> Mappings { get; private set; }

    /// <summary>
    /// Gets the name of the MySQL table this mapping corresponds to.
    /// </summary>
    public string ForMySqlTableName { get; private set; }

    #endregion Properties

    /// <summary>
    /// Returns a proposed mapping name that does not conflict with other saved mappings.
    /// </summary>
    /// <returns>A proposed mapping name that does not conflict with other saved mappings.</returns>
    public string GetProposedNewMappingName()
    {
      var mappingPiece = string.IsNullOrEmpty(ForMySqlTableName) ? "mapping" : "_mapping";
      var index = 1;
      string proposedMappingName;
      var separator = string.Empty;
      var indexSuffix = string.Empty;
      do
      {
        proposedMappingName = $"{ForMySqlTableName}{mappingPiece}{separator}{indexSuffix}";
        index++;
        separator = index > 1 ? "_" : string.Empty;
        indexSuffix = index > 1 ? index.ToString(CultureInfo.InvariantCulture) : string.Empty;
      }
      while (Mappings != null && Mappings.Any(mapping => mapping.Name == proposedMappingName));

      return proposedMappingName;
    }

    /// <summary>
    /// Handles the TextChanged event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected override void TextChangedHandler(object sender, EventArgs e)
    {
      // Looks like we could get rid of this empty override, but it is necessary to avoid an error of:
      // The method 'xxx' cannot be the method for an event because a class this class derives from already defines the method
      base.TextChangedHandler(sender, e);
    }

    /// <summary>
    /// Handles the TextValidated event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    /// <remarks>This event method is meant to be used with the <see cref="Control.Validated"/> event.</remarks>
    protected override void ValidatedHandler(object sender, EventArgs e)
    {
      // Looks like we could get rid of this empty override, but it is necessary to avoid an error of:
      // The method 'xxx' cannot be the method for an event because a class this class derives from already defines the method
      base.ValidatedHandler(sender, e);
    }

    /// <summary>
    /// Contains calls to methods that validate the given control's value.
    /// </summary>
    /// <returns>An error message or <c>null</c> / <see cref="string.Empty"/> if everything is valid.</returns>
    protected override string ValidateFields()
    {
      if (ErrorProviderControl == null)
      {
        return null;
      }

      string errorMessage = null;
      switch (ErrorProviderControl.Name)
      {
        case nameof(MappingNameTextBox):
          errorMessage = ValidateColumnMappingName(ColumnMappingName);
          break;
      }

      return errorMessage;
    }

    /// <summary>
    /// Validates a column mapping name is not already present in the collection of stored mappings.
    /// </summary>
    /// <param name="columnMappingName">A column mapping name.</param>
    /// <returns></returns>
    private string ValidateColumnMappingName(string columnMappingName)
    {
      if (string.IsNullOrEmpty(columnMappingName))
      {
        return Resources.AppendColumnMappingEmptyError;
      }

      if (Mappings != null && Mappings.Any(mapping => string.Equals(mapping.Name, columnMappingName, StringComparison.OrdinalIgnoreCase)))
      {
        return Resources.AppendColumnMappingExistsError;
      }

      return null;
    }
  }
}