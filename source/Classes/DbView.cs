// Copyright (c) 2014, 2019, Oracle and/or its affiliates. All rights reserved.
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
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySQL.ForExcel.Properties;
using MySql.Utility.Classes;
using MySql.Utility.Classes.Logging;
using MySql.Utility.Classes.MySqlWorkbench;
using MySql.Utility.Forms;
using ExcelInterop = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a MySQL View that MySQL for Excel can interact with.
  /// </summary>
  public class DbView : DbObject
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DbView"/> class.
    /// </summary>
    /// <param name="connection">The MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="name">The name of the MySQL database object.</param>
    public DbView(MySqlWorkbenchConnection connection, string name)
      : base(connection, name)
    {
      ImportParameters = new ImportDataParams(name);
    }

    #region Properties

    /// <summary>
    /// Gets the parameters used on Import Data operations.
    /// </summary>
    public ImportDataParams ImportParameters { get; }

    #endregion Properties

    /// <summary>
    /// Gets a list of column names contained within this object.
    /// </summary>
    /// <returns>A list of column names contained within this object.</returns>
    public List<string> GetColumnNamesList()
    {
      if (Connection == null)
      {
        return null;
      }

      var columnsInfoTable = Connection.GetColumnsInformationTable(null, Name);
      if (columnsInfoTable == null)
      {
        return null;
      }

      var columnsList = new List<string>(columnsInfoTable.Rows.Count);
      columnsList.AddRange(from DataRow dr in columnsInfoTable.Rows select dr["Name"].ToString());
      return columnsList;
    }

    /// <summary>
    /// Fetches the data from the corresponding MySQL object and places it in a <see cref="DataTable"/> object.
    /// </summary>
    /// <returns><see cref="DataTable"/> containing the results of the query.</returns>
    public DataTable GetData()
    {
      if (Connection == null)
      {
        return null;
      }

      var queryString = GetSelectQuery();
      return string.IsNullOrEmpty(queryString) ? null : Connection.GetDataFromSelectQuery(queryString);
    }

    /// <summary>
    /// Gets a <see cref="MySqlDataTable"/> filled with data for this <see cref="DbObject"/>.
    /// </summary>
    /// <returns>A <see cref="MySqlDataTable"/> filled with data for this <see cref="DbObject"/>.</returns>
    public MySqlDataTable GetMySqlDataTable()
    {
      if (Connection == null)
      {
        return null;
      }

      var selectQuery = GetSelectQuery();
      var operationType = ImportParameters.ForEditDataOperation
        ? MySqlDataTable.DataOperationType.Edit
        : MySqlDataTable.DataOperationType.ImportTableOrView;
      return Connection.CreateImportMySqlTable(operationType, ImportParameters.DbObjectName, ImportParameters.IncludeColumnNames, selectQuery);
    }

    /// <summary>
    /// Gets the total number of rows contained in the corresponding MySQL object.
    /// </summary>
    /// <returns>The number of rows in a given table or view.</returns>
    public long GetRowsCount()
    {
      if (Connection == null)
      {
        return 0;
      }

      object objCount = null;
      try
      {
        var sql = $"SELECT COUNT(*) FROM `{Connection.Schema}`.`{Name}`";
        objCount = MySqlHelper.ExecuteScalar(Connection.GetConnectionStringBuilder().ConnectionString, sql);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, true, string.Format(Resources.UnableToRetrieveData, this is DbTable ? "table" : "view", Name));
      }

      return (long?)objCount ?? 0;
    }

    /// <summary>
    /// Creates a SELECT query against this database object.
    /// </summary>
    /// <returns>The SELECT query text.</returns>
    public string GetSelectQuery()
    {
      if (Connection == null)
      {
        return string.Empty;
      }

      const string BIG_ROW_COUNT_LIMIT = "18446744073709551615";
      var queryStringBuilder = new StringBuilder("SELECT ");
      if (ImportParameters.ColumnsNamesList == null || ImportParameters.ColumnsNamesList.Count == 0)
      {
        queryStringBuilder.Append("*");
      }
      else
      {
        foreach (var columnName in ImportParameters.ColumnsNamesList)
        {
          queryStringBuilder.AppendFormat("`{0}`,", columnName.Replace("`", "``"));
        }

        queryStringBuilder.Remove(queryStringBuilder.Length - 1, 1);
      }

      queryStringBuilder.AppendFormat(" FROM `{0}`.`{1}`", Connection.Schema, Name);
      if (ImportParameters.FirstRowIndex > 0)
      {
        var strCount = ImportParameters.RowsCount >= 0 ? ImportParameters.RowsCount.ToString(CultureInfo.InvariantCulture) : BIG_ROW_COUNT_LIMIT;
        queryStringBuilder.AppendFormat(" LIMIT {0},{1}", ImportParameters.FirstRowIndex, strCount);
      }
      else if (ImportParameters.RowsCount >= 0)
      {
        queryStringBuilder.AppendFormat(" LIMIT {0}", ImportParameters.RowsCount);
      }

      var returnString = queryStringBuilder.ToString();
      return returnString;
    }

    /// <summary>
    /// Imports the data of this <see cref="DbView"/> to a <see cref="ExcelInterop.Worksheet"/>.
    /// </summary>
    /// <returns>A <see cref="Tuple"/> containing a <see cref="MySqlDataTable"/> filled with data for this <see cref="DbView"/> and either a <see cref="ExcelInterop.ListObject"/> or a <see cref="ExcelInterop.Range"/> where the data was imported to.</returns>
    public Tuple<MySqlDataTable, object> ImportData()
    {
      Tuple<MySqlDataTable, object> retTuple;
      var activeWorkbook = Globals.ThisAddIn.ActiveWorkbook;
      try
      {
        // Create the MySqlDataTable that holds the data to be imported to Excel
        var mySqlTable = GetMySqlDataTable();
        object excelTableOrRange = null;
        if (mySqlTable == null)
        {
          return null;
        }

        if (!ImportParameters.ForEditDataOperation)
        {
          // Create a new Excel Worksheet and import the table/view data there
          if (ImportParameters.IntoNewWorksheet)
          {
            var currentWorksheet = activeWorkbook.CreateWorksheet(mySqlTable.TableName, true);
            if (currentWorksheet == null)
            {
              return null;
            }
          }
          else
          {
            // Check if the data being imported does not exceed the column available space
            var exceedColumnsLimit = ExcelUtilities.CheckIfColumnsExceedWorksheetLimit(mySqlTable.Columns.Count);
            var collides = DetectDataForImportPossibleCollisions(mySqlTable);
            if (exceedColumnsLimit || collides)
            {
              var infoProperties = InfoDialogProperties.GetYesNoDialogProperties(
                  InfoDialog.InfoType.Warning,
                  Resources.ImportOverWorksheetColumnsLimitErrorTitle,
                  Resources.ImportOverWorksheetColumnsLimitErrorDetail,
                  Resources.ImportOverWorksheetColumnsLimitErrorSubDetail);
              if (exceedColumnsLimit && InfoDialog.ShowDialog(infoProperties).DialogResult == DialogResult.No)
              {
                return null;
              }

              infoProperties.TitleText = Resources.ImportOverExcelObjectErrorTitle;
              infoProperties.DetailText = Resources.ImportOverExcelObjectErrorDetail;
              infoProperties.DetailSubText = Resources.ImportOverExcelObjectErrorSubDetail;
              if (collides && InfoDialog.ShowDialog(infoProperties).DialogResult == DialogResult.No)
              {
                return null;
              }

              var newWorkSheet = activeWorkbook.CreateWorksheet(mySqlTable.TableName, true);
              if (newWorkSheet == null)
              {
                return null;
              }
            }
          }

          excelTableOrRange = Settings.Default.ImportCreateExcelTable
            ? mySqlTable.ImportDataIntoExcelTable(ImportParameters.CreatePivotTable, ImportParameters.PivotTablePosition, ImportParameters.AddSummaryRow)
            : mySqlTable.ImportDataIntoExcelRange(ImportParameters.CreatePivotTable, ImportParameters.PivotTablePosition, ImportParameters.AddSummaryRow);
        }

        retTuple = new Tuple<MySqlDataTable, object>(mySqlTable, excelTableOrRange);
      }
      catch (Exception ex)
      {
        retTuple = null;
        Logger.LogException(ex, true, string.Format(Resources.UnableToRetrieveData, this is DbTable ? "table" : "view", Name));
      }

      return retTuple;
    }

    /// <summary>
    /// Checks if the <see cref="ExcelInterop.Range"/> where the data of this <see cref="DbObject"/> is imported would collide with another Excel object.
    /// </summary>
    /// <param name="mySqlTable">A <see cref="MySqlDataTable"/> filled with data for this <see cref="DbObject"/>.</param>
    /// <returns><c>true</c> if the <see cref="ExcelInterop.Range"/> where the data of this <see cref="DbObject"/> is imported would collide with another Excel object, <c>false</c> otherwise.</returns>
    private bool DetectDataForImportPossibleCollisions(MySqlDataTable mySqlTable)
    {
      if (mySqlTable == null)
      {
        return false;
      }

      var atCell = Globals.ThisAddIn.Application.ActiveCell;
      var ranges = mySqlTable.GetExcelRangesToOccupy(atCell, ImportParameters.AddSummaryRow, ImportParameters.CreatePivotTable);
      return ranges != null && ranges.Aggregate(false, (current, range) => current || range.IntersectsWithAnyExcelObject());
    }
  }
}