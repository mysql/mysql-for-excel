// Copyright (c) 2014, 2018, Oracle and/or its affiliates. All rights reserved.
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
using System.Linq;
using MySQL.ForExcel.Forms;
using MySQL.ForExcel.Properties;
using ExcelInterop = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a relationship between 2 MySQL tables based on a single column on both database objects.
  /// </summary>
  public class MySqlDataRelationship
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataRelationship"/> class.
    /// </summary>
    /// <param name="direction">The relationship direction dictated by what table defined the foreign key constraint.</param>
    /// <param name="mySqlForeignKeyName">The name of the foreign key constraint from which the relationship was created from.</param>
    /// <param name="tableName">The name of the table defining the relationship to a foreign one.</param>
    /// <param name="relatedTableName">The name of the related foreign table.</param>
    /// <param name="columnName">The name of the column defining the relationship to a foreign one.</param>
    /// <param name="relatedColumnName">The name of the related foreign column.</param>
    public MySqlDataRelationship(DirectionType direction, string mySqlForeignKeyName, string tableName, string relatedTableName, string columnName, string relatedColumnName)
    {
      Direction = direction;
      MySqlForeignKeyName = mySqlForeignKeyName;
      if (string.IsNullOrEmpty(tableName))
      {
        throw new ArgumentNullException(nameof(tableName));
      }

      if (string.IsNullOrEmpty(relatedTableName))
      {
        throw new ArgumentNullException(nameof(relatedTableName));
      }

      if (string.IsNullOrEmpty(columnName))
      {
        throw new ArgumentNullException(nameof(columnName));
      }

      if (string.IsNullOrEmpty(relatedColumnName))
      {
        throw new ArgumentNullException(nameof(relatedColumnName));
      }

      TableName = tableName;
      RelatedTableName = relatedTableName;
      ColumnName = columnName;
      RelatedColumnName = relatedColumnName;
    }

    #region Enumerations

    /// <summary>
    /// Specifies identifiers to indicate the resulting status of a <see cref="ExcelInterop.ModelRelationship"/> creation.
    /// </summary>
    public enum CreationStatus
    {
      /// <summary>
      /// Model Relationships are not supported in the current Excel version.
      /// </summary>
      ModelRelationshipsNotSupported,

      /// <summary>
      /// A <see cref="ExcelInterop.ModelTableColumn"/> defining the <see cref="MySqlDataRelationship"/> was not found in one or both <see cref="ExcelInterop.ModelTable"/> objects.
      /// </summary>
      ModelTableColumnsNotFound,

      /// <summary>
      /// A <see cref="ExcelInterop.ModelTable"/> was not found for one or both tables in the <see cref="MySqlDataRelationship"/>.
      /// </summary>
      ModelTablesNotFound,

      /// <summary>
      /// A possible circular reference among tables already related in the Excel Model may be created so Excel can't create the <see cref="ExcelInterop.ModelRelationship"/>.
      /// </summary>
      PossibleCircularReference,

      /// <summary>
      /// The <see cref="ExcelInterop.ModelRelationship"/> was created successfully.
      /// </summary>
      Success
    }

    /// <summary>
    /// Specifies identifiers to indicate the direction of the relationship.
    /// </summary>
    public enum DirectionType
    {
      /// <summary>
      /// The foreign key is declared on the table with <see cref="TableName"/> and the <see cref="RelatedTableName"/> is the foreign table.
      /// </summary>
      Normal,

      /// <summary>
      /// The foreign key is declared on the table with <see cref="RelatedTableName"/> and the <see cref="TableName"/> is the foreign table.
      /// </summary>
      Reverse
    }

    #endregion Enumerations

    #region Properties

    /// <summary>
    /// Gets the name of the column defining the relationship to a foreign one.
    /// </summary>
    public string ColumnName { get; private set; }

    /// <summary>
    /// Gets the relationship direction dictated by what table defined the foreign key constraint.
    /// </summary>
    public DirectionType Direction { get; private set; }

    /// <summary>
    /// Gets the name of the foreign key constraint from which the relationship was created from.
    /// If <c>null</c> it means it was created by a user.
    /// </summary>
    public string MySqlForeignKeyName { get; private set; }

    /// <summary>
    /// Gets the name of the related foreign column.
    /// </summary>
    public string RelatedColumnName { get; private set; }

    /// <summary>
    /// Gets the name of the related foreign table.
    /// </summary>
    public string RelatedTableName { get; private set; }

    /// <summary>
    /// Gets the name of the table defining the relationsip to a foreign one.
    /// </summary>
    public string TableName { get; private set; }

    #endregion Properties

    /// <summary>
    /// Gets an error message corresponding to the given <see cref="CreationStatus"/>.
    /// </summary>
    /// <param name="creationStatus">A <see cref="CreationStatus"/> value.</param>
    /// <returns>An error message corresponding to the given <see cref="CreationStatus"/>.</returns>
    public static string GetCreationStatusErrorMessage(CreationStatus creationStatus)
    {
      switch (creationStatus)
      {
        case CreationStatus.ModelRelationshipsNotSupported:
          return Resources.ModelRelationshipsNotSupportedError;

        case CreationStatus.ModelTableColumnsNotFound:
          return Resources.ModelTableColumnsNotFoundError;

        case CreationStatus.ModelTablesNotFound:
          return Resources.ModelTablesNotFoundError;

        case CreationStatus.PossibleCircularReference:
          return Resources.PossibleCircularReferenceError;
      }

      return string.Empty;
    }

    /// <summary>
    /// Gets an error message corresponding to the given <see cref="CreationStatus"/>.
    /// </summary>
    /// <param name="creationStatus">A <see cref="CreationStatus"/> value.</param>
    /// <returns>An error message corresponding to the given <see cref="CreationStatus"/>.</returns>
    public string GetCreationErrorMessage(CreationStatus creationStatus)
    {
      var statusError = GetCreationStatusErrorMessage(creationStatus);
      return string.IsNullOrEmpty(statusError)
        ? string.Empty
        : ToString() + ": " + Environment.NewLine + statusError;
    }

    /// <summary>
    /// Creates a <see cref="ExcelInterop.ModelRelationship"/> based on the information of this object.
    /// </summary>
    /// <param name="modelTableName">The name of the <see cref="ExcelInterop.ModelTable"/> (or <see cref="ExcelInterop.ListObject"/>) defining the relationship to a foreign one.</param>
    /// <param name="relatedModelTableName">The name of the <see cref="ExcelInterop.ModelTable"/> (or <see cref="ExcelInterop.ListObject"/>) defining the related foreign table.</param>
    /// <returns>A <see cref="CreationStatus"/> reflecting the result of the <see cref="ExcelInterop.ModelRelationship"/> creation.</returns>
    public CreationStatus CreateExcelRelationship(string modelTableName, string relatedModelTableName)
    {
      if (ImportMultipleDialog.Excel2010OrLower)
      {
        return CreationStatus.ModelRelationshipsNotSupported;
      }

      if (string.IsNullOrEmpty(modelTableName) || string.IsNullOrEmpty(relatedModelTableName))
      {
        return CreationStatus.ModelTablesNotFound;
      }

      try
      {
        // Create the Workbook connections that trigger the Model Tables creation
        CreateExcelModelConnection(modelTableName);
        CreateExcelModelConnection(relatedModelTableName);

        // Get the ModelColumnName objects needed to define the relationship
        modelTableName = modelTableName.Replace(".", " ");
        relatedModelTableName = relatedModelTableName.Replace(".", " ");
        var activeWorkbook = Globals.ThisAddIn.ActiveWorkbook;
        var table = activeWorkbook.Model.ModelTables.Cast<ExcelInterop.ModelTable>().FirstOrDefault(mt => string.Equals(mt.Name, modelTableName, StringComparison.InvariantCulture));
        var relatedTable = activeWorkbook.Model.ModelTables.Cast<ExcelInterop.ModelTable>().FirstOrDefault(mt => string.Equals(mt.Name, relatedModelTableName, StringComparison.InvariantCulture));
        if (table == null || relatedTable == null)
        {
          return CreationStatus.ModelTablesNotFound;
        }

        var column = table.ModelTableColumns.Cast<ExcelInterop.ModelTableColumn>().FirstOrDefault(col => string.Equals(col.Name, ColumnName, StringComparison.InvariantCulture));
        var relatedColumn = relatedTable.ModelTableColumns.Cast<ExcelInterop.ModelTableColumn>().FirstOrDefault(col => string.Equals(col.Name, RelatedColumnName, StringComparison.InvariantCulture));
        if (column == null || relatedColumn == null)
        {
          return CreationStatus.ModelTableColumnsNotFound;
        }

        activeWorkbook.Model.ModelRelationships.Add(column, relatedColumn);
      }
      catch (Exception)
      {
        return CreationStatus.PossibleCircularReference;
      }

      return CreationStatus.Success;
    }

    /// <summary>
    /// Checks whether this relationship can exist among tables in the given list.
    /// </summary>
    /// <param name="tableNames">A list of table names.</param>
    /// <returns><c>true</c> if this relationship can exist among tables in the given list, <c>false</c> otherwise.</returns>
    public bool ExistsAmongTablesInList(List<string> tableNames)
    {
      return tableNames != null
             && Direction == DirectionType.Normal
             && tableNames.Any(table => string.Equals(table, TableName, StringComparison.InvariantCultureIgnoreCase))
             && tableNames.Any(table => string.Equals(table, RelatedTableName, StringComparison.InvariantCultureIgnoreCase));
    }

    /// <summary>
    /// Returns a string describing the current relationship.
    /// </summary>
    /// <returns>A string describing the current relationship.</returns>
    public override string ToString()
    {
      return $"`{TableName}` (`{ColumnName}`) {(Direction == DirectionType.Normal ? ">--" : "--<")} `{RelatedTableName}` (`{RelatedColumnName}`)";
    }

    /// <summary>
    /// Creates a <see cref="ExcelInterop.WorkbookConnection"/> for each of the tables in the relationship, needed so Excel automatically creates their corresponding <see cref="ExcelInterop.ModelTable"/> objects.
    /// </summary>
    /// <param name="modelTableName">The name of the <see cref="ExcelInterop.ModelTable"/> (or <see cref="ExcelInterop.ListObject"/>).</param>
    private void CreateExcelModelConnection(string modelTableName)
    {
      if (ImportMultipleDialog.Excel2010OrLower)
      {
        return;
      }

      var activeWorkbook = Globals.ThisAddIn.ActiveWorkbook;
      var commandText = $"{activeWorkbook.Name}!{modelTableName}";
      var connectionName = "ModelConnection_For_" + commandText;
      var connectionStringForCmdExcel = "WORKSHEET;" + activeWorkbook.Name;
      var workbookConnection = activeWorkbook.Connections.Add2(connectionName, string.Empty, connectionStringForCmdExcel, commandText, ExcelInterop.XlCmdType.xlCmdExcel, true, false);
      workbookConnection.Description = Resources.WorkbookConnectionForExcelModelDescription;
    }
  }
}