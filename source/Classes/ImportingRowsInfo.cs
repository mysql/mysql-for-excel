// Copyright (c) 2019, Oracle and/or its affiliates. All rights reserved.
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
using System.Linq;
using MySQL.ForExcel.Properties;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Contains information about the rows that can be imported from this <see cref="DbView"/>.
  /// </summary>
  public class ImportingRowsInfo
  {
    #region Properties

    /// <summary>
    /// The maximum number of rows that can be fit in the active worksheet from the active cell.
    /// </summary>
    public long MaximumExcelRowsThatFit;

    /// <summary>
    /// The total number of rows contained in this <see cref="DbView"/>.
    /// </summary>
    public long RowsCount;

    /// <summary>
    /// Flag indicating whether the rows count exceed the rows limit.
    /// </summary>
    public bool RowsCountExceedsRowsLimit;

    /// <summary>
    /// The number of rows that can be actually imported to the active worksheet depending on the current active cell, the workbook's maximum rows limit and other importing options.
    /// </summary>
    public long RowsLimit;

    /// <summary>
    /// The number of rows to be imported, may be different than the rows count if the starting importing row is not the first one.
    /// </summary>
    public long RowsToImport;

    #endregion Properties

    /// <summary>
    /// Gets information about importing rows.
    /// </summary>
    /// <param name="dataTable">A <see cref="DataTable"/> already filled with importing rows.</param>
    /// <param name="includeHeadersRow">Flag indicating whether a headers row with column names will be included in an import data operation.</param>
    /// <param name="includeSummaryRow">Flag indicating whether a summary row will be included at the end of the imported data.</param>
    /// <param name="startingRow">An optional starting row index, not 0-based but 1-based.</param>
    /// <returns>A <see cref="ImportingRowsInfo"/> struct with information about importing rows.</returns>
    public static ImportingRowsInfo FromDataTable(DataTable dataTable, bool includeHeadersRow, bool includeSummaryRow, long startingRow = 1)
    {
      return FromRowsCount(dataTable?.Rows.Count ?? 0, includeHeadersRow, includeSummaryRow, startingRow);
    }

    /// <summary>
    /// Gets information about importing rows.
    /// </summary>
    /// <param name="dataSet">A <see cref="DataSet"/> already filled with importing rows.</param>
    /// <param name="includeHeadersRow">Flag indicating whether a headers row with column names will be included in an import data operation.</param>
    /// <param name="includeSummaryRow">Flag indicating whether a summary row will be included at the end of the imported data.</param>
    /// <param name="startingRow">An optional starting row index, not 0-based but 1-based.</param>
    /// <returns>A list of <see cref="ImportingRowsInfo"/> struct with information about importing rows.</returns>
    public static List<ImportingRowsInfo> FromDataSet(DataSet dataSet, bool includeHeadersRow, bool includeSummaryRow, long startingRow = 1)
    {
      return dataSet?.Tables.Cast<DataTable>().Select(dt => FromDataTable(dt, includeHeadersRow, includeSummaryRow, startingRow)).ToList();
    }

    /// <summary>
    /// Gets information about importing rows.
    /// </summary>
    /// <param name="rowsCount">The total rows count to import.</param>
    /// <param name="includeHeadersRow">Flag indicating whether a headers row with column names will be included in an import data operation.</param>
    /// <param name="includeSummaryRow">Flag indicating whether a summary row will be included at the end of the imported data.</param>
    /// <param name="startingRow">An optional starting row index, not 0-based but 1-based.</param>
    /// <returns>A <see cref="ImportingRowsInfo"/> struct with information about importing rows.</returns>
    public static ImportingRowsInfo FromRowsCount(long rowsCount, bool includeHeadersRow, bool includeSummaryRow, long startingRow = 1)
    {
      var importingRowsInfo = new ImportingRowsInfo();

      // Calculate the maximum rows that can be fetched (total rows - starting row)
      importingRowsInfo.RowsCount = rowsCount;
      importingRowsInfo.RowsToImport = importingRowsInfo.RowsCount - startingRow + 1;

      // Calculate the maximum number of rows that can fit in the Worksheet, given the current Excel cell and the maximum rows in the Worksheet.
      // Note that an extra row has to be subtracted when using Excel tables (ListObjects) since that extra row could be used to add a summary row.
      var usingExcelTables = Settings.Default.ImportCreateExcelTable;
      var headerRowsCount = usingExcelTables || includeHeadersRow ? 1 : 0;
      var summaryRowsCount = includeSummaryRow ? 1 : 0;
      var extraRowIfUsingExcelTables = usingExcelTables ? 1 : 0;
      var activeWorkbookMaxRowNumber = Globals.ThisAddIn.ActiveWorkbook.GetWorkbookMaxRowNumber();
      var atCell = Globals.ThisAddIn.Application.ActiveCell;
      var atRow = atCell?.Row ?? 1;
      importingRowsInfo.MaximumExcelRowsThatFit = activeWorkbookMaxRowNumber - atRow - headerRowsCount - summaryRowsCount - extraRowIfUsingExcelTables + 1;

      // Get the minimum value between the rows that can fit in the Worksheet VS the rows that can be fetched from the table given the start row and the total rows.
      importingRowsInfo.RowsLimit = Math.Min(importingRowsInfo.RowsToImport, importingRowsInfo.MaximumExcelRowsThatFit);
      importingRowsInfo.RowsCountExceedsRowsLimit = importingRowsInfo.RowsLimit < importingRowsInfo.RowsCount;
      return importingRowsInfo;
    }
  }
}
