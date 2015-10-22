// Copyright (c) 2013, 2015, Oracle and/or its affiliates. All rights reserved.
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
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Office.Core;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Forms;
using ExcelInterop = Microsoft.Office.Interop.Excel;
using ExcelTools = Microsoft.Office.Tools.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Provides extension methods and other static methods to leverage the work with Excel objects.
  /// </summary>
  public static class ExcelUtilities
  {
    #region Constants

    /// <summary>
    /// The default interior color for Excel cells committed to the MySQL server during an Edit Data operation.
    /// </summary>
    /// <remarks>Green-ish.</remarks>
    public const string DEFAULT_COMMITED_CELLS_HTML_COLOR = "#7CC576";

    /// <summary>
    /// The default interior color for Excel cells that caused errors during a commit of an Edit Data operation.
    /// </summary>
    /// <remarks>Red-ish.</remarks>
    public const string DEFAULT_ERRORED_CELLS_HTML_COLOR = "#FF8282";

    /// <summary>
    /// The default interior color for Excel cells locked during an Edit Data operation (like the headers containing column names)..
    /// </summary>
    /// <remarks>Gray-ish</remarks>
    public const string DEFAULT_LOCKED_CELLS_HTML_COLOR = "#D7D7D7";

    /// <summary>
    /// The default name for the default MySQL style used for Excel tables.
    /// </summary>
    public const string DEFAULT_MYSQL_STYLE_NAME = "MySqlDefault";

    /// <summary>
    /// The default interior color for Excel cells accepting data from users to create a new row in the table during an Edit Data operation.
    /// </summary>
    /// <remarks>Yellow-ish.</remarks>
    public const string DEFAULT_NEW_ROW_CELLS_HTML_COLOR = "#FFFCC7";

    /// <summary>
    /// The default interior color for Excel cells containing values that have been changed by the user but not yet committed during an Edit Data operation.
    /// </summary>
    /// <remarks>Blue-ish.</remarks>
    public const string DEFAULT_UNCOMMITTED_CELLS_HTML_COLOR = "#B8E5F7";

    /// <summary>
    /// The default interior color for Excel cells containing values that caused concurrency warnings during an Edit Data operation using optimistic updates.
    /// </summary>
    /// <remarks>Green-ish.</remarks>
    public const string DEFAULT_WARNING_CELLS_HTML_COLOR = "#FCC451";

    /// <summary>
    /// The interior color used to revert Excel cells to their original background color.
    /// </summary>
    public const int EMPTY_CELLS_OLE_COLOR = 0;

    /// <summary>
    /// The en-us locale code.
    /// </summary>
    public const int EN_US_LOCALE_CODE = 1033;

    /// <summary>
    /// The universal long time format which includes hours minutes and seconds.
    /// </summary>
    public const string LONG_TIME_FORMAT = "hh:mm:ss";

    /// <summary>
    /// The maximum number of columns that can exist in 2003 and older versions of Excel;
    /// </summary>
    public const int MAXIMUM_WORKSHEET_COLUMNS_IN_COMPATIBILITY_MODE = 256;

    /// <summary>
    /// The maximum number of columns that can exist in 2007 and newer versions of Excel;
    /// </summary>
    public const int MAXIMUM_WORKSHEET_COLUMNS_IN_LATEST_VERSION = 16384;

    /// <summary>
    /// The maximum number of rows that can exist in 2007 and newer versions of Excel;
    /// </summary>
    public const int MAXIMUM_WORKSHEET_ROWS_IN_LATEST_VERSION = 1048576;

    /// <summary>
    /// The maximum number of rows that can exist in 2003 and older versions of Excel;
    /// </summary>
    public const int MAXIMUM_WORKSHEET_ROWS_IN_COMPATIBILITY_MODE = UInt16.MaxValue + 1;

    /// <summary>
    /// The default number of columns a <see cref="ExcelInterop.PivotTable"/> placeholder occupies.
    /// </summary>
    public const int PIVOT_TABLES_PLACEHOLDER_DEFAULT_COLUMNS_SIZE = 3;

    /// <summary>
    /// The default number of rows a <see cref="ExcelInterop.PivotTable"/> placeholder occupies.
    /// </summary>
    public const int PIVOT_TABLES_PLACEHOLDER_DEFAULT_ROWS_SIZE = 18;

    /// <summary>
    /// The name of the <see cref="ExcelInterop.WorkbookConnection"/> created for the whole Excel data model in the active <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    public const string WORKBOOK_DATA_MODEL_CONNECTION_NAME = "ThisWorkbookDataModel";

    #endregion Constants

    /// <summary>
    /// Initializes the <see cref="ExcelUtilities"/> class.
    /// </summary>
    static ExcelUtilities()
    {
      CommitedCellsHtmlColor = DEFAULT_COMMITED_CELLS_HTML_COLOR;
      ErroredCellsHtmlColor = DEFAULT_ERRORED_CELLS_HTML_COLOR;
      LockedCellsHtmlColor = DEFAULT_LOCKED_CELLS_HTML_COLOR;
      NewRowCellsHtmlColor = DEFAULT_NEW_ROW_CELLS_HTML_COLOR;
      UncommittedCellsHtmlColor = DEFAULT_UNCOMMITTED_CELLS_HTML_COLOR;
      WarningCellsHtmlColor = DEFAULT_WARNING_CELLS_HTML_COLOR;
    }

    #region Properties

    /// <summary>
    /// Gets or sets the interior color for Excel cells committed to the MySQL server during an Edit Data operation.
    /// </summary>
    public static string CommitedCellsHtmlColor
    {
      get
      {
        return ColorTranslator.ToHtml(ColorTranslator.FromOle(CommitedCellsOleColor));
      }

      set
      {
        CommitedCellsOleColor = ColorTranslator.ToOle(ColorTranslator.FromHtml(value));
      }
    }

    /// <summary>
    /// Gets the interior color for Excel cells committed to the MySQL server during an Edit Data operation.
    /// </summary>
    public static int CommitedCellsOleColor { get; private set; }

    /// <summary>
    /// Gets or sets the interior color for Excel cells that caused errors during a commit of an Edit Data operation.
    /// </summary>
    public static string ErroredCellsHtmlColor
    {
      get
      {
        return ColorTranslator.ToHtml(ColorTranslator.FromOle(ErroredCellsOleColor));
      }

      set
      {
        ErroredCellsOleColor = ColorTranslator.ToOle(ColorTranslator.FromHtml(value));
      }
    }

    /// <summary>
    /// Gets the interior color for Excel cells that caused errors during a commit of an Edit Data operation.
    /// </summary>
    public static int ErroredCellsOleColor { get; private set; }

    /// <summary>
    /// Gets or sets the interior color for Excel cells locked during an Edit Data operation (like the headers containing column names).
    /// </summary>
    public static string LockedCellsHtmlColor
    {
      get
      {
        return ColorTranslator.ToHtml(ColorTranslator.FromOle(LockedCellsOleColor));
      }

      set
      {
        LockedCellsOleColor = ColorTranslator.ToOle(ColorTranslator.FromHtml(value));
      }
    }

    /// <summary>
    /// Gets the default interior color for Excel cells locked during an Edit Data operation (like the headers containing column names).
    /// </summary>
    public static int LockedCellsOleColor { get; private set; }

    /// <summary>
    /// Gets or sets the interior color for Excel cells accepting data from users to create a new row in the table during an Edit Data operation.
    /// </summary>
    public static string NewRowCellsHtmlColor
    {
      get
      {
        return ColorTranslator.ToHtml(ColorTranslator.FromOle(NewRowCellsOleColor));
      }

      set
      {
        NewRowCellsOleColor = ColorTranslator.ToOle(ColorTranslator.FromHtml(value));
      }
    }

    /// <summary>
    /// Gets the interior color for Excel cells accepting data from users to create a new row in the table during an Edit Data operation.
    /// </summary>
    public static int NewRowCellsOleColor { get; private set; }

    /// <summary>
    /// Gets or sets the interior color for Excel cells containing values that have been changed by the user but not yet committed during an Edit Data operation.
    /// </summary>
    public static string UncommittedCellsHtmlColor
    {
      get
      {
        return ColorTranslator.ToHtml(ColorTranslator.FromOle(UncommittedCellsOleColor));
      }

      set
      {
        UncommittedCellsOleColor = ColorTranslator.ToOle(ColorTranslator.FromHtml(value));
      }
    }

    /// <summary>
    /// Gets the interior color for Excel cells containing values that have been changed by the user but not yet committed during an Edit Data operation.
    /// </summary>
    public static int UncommittedCellsOleColor { get; private set; }

    /// <summary>
    /// Gets or sets the interior color for Excel cells containing values that caused concurrency warnings during an Edit Data operation using optimistic updates.
    /// </summary>
    public static string WarningCellsHtmlColor
    {
      get
      {
        return ColorTranslator.ToHtml(ColorTranslator.FromOle(WarningCellsOleColor));
      }

      set
      {
        WarningCellsOleColor = ColorTranslator.ToOle(ColorTranslator.FromHtml(value));
      }
    }

    /// <summary>
    /// Gets the interior color for Excel cells containing values that caused concurrency warnings during an Edit Data operation using optimistic updates.
    /// </summary>
    public static int WarningCellsOleColor { get; private set; }

    #endregion Properties

    /// <summary>
    /// Specifies identifiers to indicate the position where new <see cref="ExcelInterop.PivotTable"/> objects are placed relative to imported table's data.
    /// </summary>
    public enum PivotTablePosition
    {
      /// <summary>
      /// The <see cref="ExcelInterop.PivotTable"/> is placed below the imported data skipping one Excel row.
      /// </summary>
      Below,

      /// <summary>
      /// The <see cref="ExcelInterop.PivotTable"/> is placed to the right of the imported data skipping one Excel column.
      /// </summary>
      Right
    }

    /// <summary>
    /// Adds a new row at the bottom of the given Excel range.
    /// </summary>
    /// <param name="range">The Excel range to add a new row to the end of it.</param>
    /// <param name="clearLastRowColoring">Flag indicating whether the previous row that was placeholder for new rows is cleared of its formatting.</param>
    /// <param name="newRowRange">An Excel range containing just the newly added row if <see cref="clearLastRowColoring"/> is <c>true</c>, or containing the row above the newly added one otherwise.</param>
    /// <returns>The original Excel range with the newly added row at the end of it.</returns>
    public static ExcelInterop.Range AddNewRow(this ExcelInterop.Range range, bool clearLastRowColoring, out ExcelInterop.Range newRowRange)
    {
      newRowRange = null;
      if (range == null)
      {
        return null;
      }

      range = range.SafeResize(range.Rows.Count + 1, range.Columns.Count);
      newRowRange = range.Rows[range.Rows.Count] as ExcelInterop.Range;
      if (newRowRange != null)
      {
        newRowRange.Interior.Color = NewRowCellsOleColor;
      }

      if (!clearLastRowColoring || range.Rows.Count <= 0)
      {
        return range;
      }

      newRowRange = range.Rows[range.Rows.Count - 1] as ExcelInterop.Range;
      if (newRowRange != null)
      {
        newRowRange.Interior.ColorIndex = ExcelInterop.XlColorIndex.xlColorIndexNone;
      }

      return range;
    }

    /// <summary>
    /// Adds a summary row below the data of the given <see cref="ExcelInterop.ListObject"/>.
    /// </summary>
    /// <param name="excelTable">A <see cref="ExcelInterop.ListObject"/> object.</param>
    public static void AddSummaryRow(this ExcelInterop.ListObject excelTable)
    {
      if (excelTable == null)
      {
        return;
      }

      excelTable.ShowTotals = true;
      excelTable.TotalsRowRange.Font.Bold = true;

      // Reset all of the other borders first
      excelTable.TotalsRowRange.Borders[ExcelInterop.XlBordersIndex.xlDiagonalDown].LineStyle = ExcelInterop.XlLineStyle.xlLineStyleNone;
      excelTable.TotalsRowRange.Borders[ExcelInterop.XlBordersIndex.xlDiagonalUp].LineStyle = ExcelInterop.XlLineStyle.xlLineStyleNone;
      excelTable.TotalsRowRange.Borders[ExcelInterop.XlBordersIndex.xlEdgeLeft].LineStyle = ExcelInterop.XlLineStyle.xlLineStyleNone;
      excelTable.TotalsRowRange.Borders[ExcelInterop.XlBordersIndex.xlEdgeBottom].LineStyle = ExcelInterop.XlLineStyle.xlLineStyleNone;
      excelTable.TotalsRowRange.Borders[ExcelInterop.XlBordersIndex.xlEdgeTop].LineStyle = ExcelInterop.XlLineStyle.xlLineStyleNone;
      excelTable.TotalsRowRange.Borders[ExcelInterop.XlBordersIndex.xlEdgeRight].LineStyle = ExcelInterop.XlLineStyle.xlLineStyleNone;
      excelTable.TotalsRowRange.Borders[ExcelInterop.XlBordersIndex.xlInsideVertical].LineStyle = ExcelInterop.XlLineStyle.xlLineStyleNone;
      excelTable.TotalsRowRange.Borders[ExcelInterop.XlBordersIndex.xlInsideHorizontal].LineStyle = ExcelInterop.XlLineStyle.xlLineStyleNone;

      // Set the only border we care about.
      var topBorder = excelTable.TotalsRowRange.Borders[ExcelInterop.XlBordersIndex.xlEdgeTop];
      topBorder.LineStyle = ExcelInterop.XlLineStyle.xlContinuous;
      topBorder.ColorIndex = 0;
      topBorder.TintAndShade = 0;
      topBorder.Weight = XlBorderWeight.xlMedium;
    }

    /// <summary>
    /// Checks if the given <see cref="ExcelInterop.Range"/> contains data in any of its cells.
    /// </summary>
    /// <param name="range">A <see cref="ExcelInterop.Range"/> object.</param>
    /// <returns><c>true</c> if the given range is not empty, <c>false</c> otherwise.</returns>
    public static bool ContainsAnyData(this ExcelInterop.Range range)
    {
      if (range == null || range.CountLarge < 1)
      {
        return false;
      }

      return Globals.ThisAddIn.Application.WorksheetFunction.CountA(range).CompareTo(0) != 0;
    }

    /// <summary>
    /// Indicates whether a number of columns relative to the actual position exceeds the <see cref="ExcelInterop.Worksheet"/> columns limit.
    /// </summary>
    /// <param name="columns">The number of columns to the right of the actual position to evaluate.</param>
    /// <returns><c>true</c> if number of columns to the right of the actual position exceeds the <see cref="ExcelInterop.Worksheet"/> columns limit, <c>false</c> otherwise.</returns>
    public static bool CheckIfColumnsExceedWorksheetLimit(int columns)
    {
      var atCell = Globals.ThisAddIn.Application.ActiveCell;
      if (atCell == null)
      {
        return false;
      }

      var currentColumn = atCell.Column;
      var activeWorkbook = atCell.Worksheet.Parent as ExcelInterop.Workbook;
      if (activeWorkbook == null)
      {
        return false;
      }

      var maxColumnNumber = activeWorkbook.GetWorkbookMaxColumnNumber();
      var totalColumns = Math.Min(columns, (maxColumnNumber - currentColumn) + 1);
      return columns > totalColumns;
    }

    /// <summary>
    /// Creates a default <see cref="ExcelInterop.TableStyle"/> for MySQL imported data.
    /// </summary>
    /// <param name="workbook">The workbook where the new <see cref="ExcelInterop.Style"/> is added to.</param>
    /// <returns>A new <see cref="ExcelInterop.TableStyle"/> for MySQL imported data.</returns>
    public static ExcelInterop.TableStyle CreateMySqlTableStyle(this ExcelInterop.Workbook workbook)
    {
      if (workbook == null || workbook.TableStyles.Cast<ExcelInterop.TableStyle>().Any(style => style.Name == DEFAULT_MYSQL_STYLE_NAME))
      {
        return null;
      }

      var mySqlTableStyle = workbook.TableStyles.Add(DEFAULT_MYSQL_STYLE_NAME);
      mySqlTableStyle.ShowAsAvailableTableStyle = false;
      mySqlTableStyle.TableStyleElements[ExcelInterop.XlTableStyleElementType.xlWholeTable].SetAsMySqlStyle();
      mySqlTableStyle.TableStyleElements[ExcelInterop.XlTableStyleElementType.xlHeaderRow].SetAsMySqlStyle(LockedCellsOleColor, true);
      return mySqlTableStyle;
    }

    /// <summary>
    /// Creates a <see cref="ExcelInterop.PivotTable"/> starting at the given cell containing the data in the given <see cref="ExcelInterop.Range"/>.
    /// </summary>
    /// <param name="fromExcelRange">A <see cref="ExcelInterop.Range"/> containing the data from which to create the <see cref="ExcelInterop.PivotTable"/>.</param>
    /// <param name="pivotPosition">The position where new <see cref="ExcelInterop.PivotTable"/> objects are placed relative to imported table's data.</param>
    /// <param name="proposedName">The proposed name for the new <see cref="ExcelInterop.PivotTable"/>.</param>
    /// <returns>The newly created <see cref="ExcelInterop.PivotTable"/>.</returns>
    public static ExcelInterop.PivotTable CreatePivotTable(ExcelInterop.Range fromExcelRange, PivotTablePosition pivotPosition, string proposedName)
    {
      if (fromExcelRange == null)
      {
        return null;
      }

      var atCell = fromExcelRange.GetPivotTableTopLeftCell(pivotPosition);
      return atCell == null ? null : CreatePivotTable(fromExcelRange, atCell, proposedName);
    }

    /// <summary>
    /// Creates a <see cref="ExcelInterop.PivotTable"/> starting at the given cell containing the data in the given <see cref="ExcelInterop.Range"/>.
    /// </summary>
    /// <param name="fromExcelTable">A <see cref="ExcelInterop.ListObject"/> containing the data from which to create the <see cref="ExcelInterop.PivotTable"/>.</param>
    /// <param name="pivotPosition">The position where new <see cref="ExcelInterop.PivotTable"/> objects are placed relative to imported table's data.</param>
    /// <param name="proposedName">The proposed name for the new <see cref="ExcelInterop.PivotTable"/>.</param>
    /// <returns>The newly created <see cref="ExcelInterop.PivotTable"/>.</returns>
    public static ExcelInterop.PivotTable CreatePivotTable(ExcelInterop.ListObject fromExcelTable, PivotTablePosition pivotPosition, string proposedName)
    {
      return fromExcelTable == null ? null : CreatePivotTable(fromExcelTable.Range, pivotPosition, proposedName);
    }

    /// <summary>
    /// Creates a <see cref="ExcelInterop.PivotTable"/> starting at the given cell containing the data in the given <see cref="ExcelInterop.Range"/>.
    /// </summary>
    /// <param name="fromExcelTable">A <see cref="ExcelInterop.ListObject"/> containing the data from which to create the <see cref="ExcelInterop.PivotTable"/>.</param>
    /// <param name="atCell">The top left Excel cell of the new <see cref="ExcelInterop.PivotTable"/>.</param>
    /// <param name="proposedName">The proposed name for the new <see cref="ExcelInterop.PivotTable"/>.</param>
    /// <returns>The newly created <see cref="ExcelInterop.PivotTable"/>.</returns>
    public static ExcelInterop.PivotTable CreatePivotTable(ExcelInterop.ListObject fromExcelTable, ExcelInterop.Range atCell, string proposedName)
    {
      return fromExcelTable == null ? null : CreatePivotTable(fromExcelTable.Range, atCell, proposedName);
    }

    /// <summary>
    /// Creates a <see cref="ExcelInterop.PivotTable"/> starting at the given cell containing the data in the given <see cref="ExcelInterop.Range"/>.
    /// </summary>
    /// <param name="fromExcelRange">A <see cref="ExcelInterop.Range"/> containing the data from which to create the <see cref="ExcelInterop.PivotTable"/>.</param>
    /// <param name="atCell">The top left Excel cell of the new <see cref="ExcelInterop.PivotTable"/>.</param>
    /// <param name="proposedName">The proposed name for the new <see cref="ExcelInterop.PivotTable"/>.</param>
    /// <returns>The newly created <see cref="ExcelInterop.PivotTable"/>.</returns>
    public static ExcelInterop.PivotTable CreatePivotTable(ExcelInterop.Range fromExcelRange, ExcelInterop.Range atCell, string proposedName)
    {
      if (atCell == null || fromExcelRange == null)
      {
        return null;
      }

      if (string.IsNullOrEmpty(proposedName))
      {
        proposedName = "PivotTable";
      }

      var workbook = atCell.Worksheet.Parent as ExcelInterop.Workbook;
      if (workbook == null)
      {
        return null;
      }

      ExcelInterop.PivotTable pivotTable = null;
      try
      {
        string pivotSource = fromExcelRange.Address[true, true, ExcelInterop.XlReferenceStyle.xlR1C1, true];
        proposedName = proposedName.GetPivotTableNameAvoidingDuplicates();
        var pivotTableVersion = Globals.ThisAddIn.ExcelPivotTableVersion;
        var pivotCache = workbook.PivotCaches().Create(ExcelInterop.XlPivotTableSourceType.xlDatabase, pivotSource, pivotTableVersion);
        string tableDestination = atCell.Address[true, true, ExcelInterop.XlReferenceStyle.xlR1C1, true];
        pivotTable = pivotCache.CreatePivotTable(tableDestination, proposedName, true, pivotTableVersion);
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(string.Format(Resources.PivotTableCreationError, proposedName), ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }

      return pivotTable;
    }

    /// <summary>
    /// Creates a <see cref="ExcelInterop.PivotTable"/> starting at the given cell where the data is taken from sources in the given <see cref="ExcelInterop.WorkbookConnection"/>.
    /// </summary>
    /// <param name="fromWorkbookConnection">A <see cref="ExcelInterop.WorkbookConnection"/> pointing to the data from which to create the <see cref="ExcelInterop.PivotTable"/>.</param>
    /// <param name="atCell">The top left Excel cell of the new <see cref="ExcelInterop.PivotTable"/>.</param>
    /// <param name="proposedName">The proposed name for the new <see cref="ExcelInterop.PivotTable"/>.</param>
    /// <returns>The newly created <see cref="ExcelInterop.PivotTable"/>.</returns>
    public static ExcelInterop.PivotTable CreatePivotTable(ExcelInterop.WorkbookConnection fromWorkbookConnection, ExcelInterop.Range atCell, string proposedName)
    {
      if (atCell == null || fromWorkbookConnection == null)
      {
        return null;
      }

      if (string.IsNullOrEmpty(proposedName))
      {
        proposedName = "PivotTable";
      }

      var workbook = atCell.Worksheet.Parent as ExcelInterop.Workbook;
      if (workbook == null)
      {
        return null;
      }

      ExcelInterop.PivotTable pivotTable = null;
      try
      {
        proposedName = proposedName.GetPivotTableNameAvoidingDuplicates();
        var pivotTableVersion = Globals.ThisAddIn.ExcelPivotTableVersion;
        var pivotCache = workbook.PivotCaches().Create(ExcelInterop.XlPivotTableSourceType.xlExternal, fromWorkbookConnection, pivotTableVersion);
        string tableDestination = atCell.Address[true, true, ExcelInterop.XlReferenceStyle.xlR1C1, true];
        pivotTable = pivotCache.CreatePivotTable(tableDestination, proposedName, true, pivotTableVersion);
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(string.Format(Resources.PivotTableCreationError, proposedName), ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }

      return pivotTable;
    }

    /// <summary>
    /// Gets a <see cref="ExcelInterop.Worksheet"/> with a given name existing in the given <see cref="ExcelInterop.Workbook"/> or creates a new one.
    /// </summary>
    /// <param name="workBook">The <see cref="ExcelInterop.Workbook"/> to look for a <see cref="ExcelInterop.Worksheet"/>.</param>
    /// <param name="workSheetName">The name of the new <see cref="ExcelInterop.Worksheet"/>.</param>
    /// <param name="selectTopLeftCell">Flag indicating whether the cell A1 receives focus.</param>
    /// <returns>The existing or new <see cref="ExcelInterop.Worksheet"/> object.</returns>
    public static ExcelInterop.Worksheet CreateWorksheet(this ExcelInterop.Workbook workBook, string workSheetName, bool selectTopLeftCell)
    {
      if (workBook == null)
      {
        return null;
      }

      ExcelInterop.Worksheet newWorksheet = null;
      try
      {
        newWorksheet = workBook.Worksheets.Add(Type.Missing, workBook.ActiveSheet, Type.Missing, Type.Missing);
        newWorksheet.Name = workBook.GetWorksheetNameAvoidingDuplicates(workSheetName);

        if (selectTopLeftCell)
        {
          newWorksheet.SelectTopLeftCell();
        }
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.WorksheetCreationErrorText, ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }

      return newWorksheet;
    }

    /// <summary>
    /// Attempts to delete a <see cref="ExcelInterop.ListObject"/> trapping any COM exception.
    /// </summary>
    /// <param name="excelTable">A <see cref="ExcelInterop.ListObject"/> object.</param>
    /// <param name="logException">Flag indicating whether any exception is written to the application log.</param>
    /// <returns><c>true</c> if the deletion happened without errors, <c>false</c> otherwise.</returns>
    public static bool DeleteSafely(this ExcelInterop.ListObject excelTable, bool logException)
    {
      if (excelTable == null)
      {
        return false;
      }

      bool success = true;
      try
      {
        excelTable.Delete();
      }
      catch (Exception ex)
      {
        success = false;
        if (logException)
        {
          MySqlSourceTrace.WriteAppErrorToLog(ex);
        }
      }

      return success;
    }

    /// <summary>
    /// Attempts to delete a <see cref="ExcelTools.ListObject"/> trapping any COM exception.
    /// </summary>
    /// <param name="toolsExcelTable">A <see cref="ExcelTools.ListObject"/> object.</param>
    /// <param name="logException">Flag indicating whether any exception is written to the application log.</param>
    /// <returns><c>true</c> if the deletion happened without errors, <c>false</c> otherwise.</returns>
    public static bool DeleteSafely(this ExcelTools.ListObject toolsExcelTable, bool logException)
    {
      if (toolsExcelTable == null)
      {
        return false;
      }

      bool success = true;
      try
      {
        var interopExcelTable = toolsExcelTable.InnerObject;
        toolsExcelTable.Delete();
        toolsExcelTable.Dispose();
        if (interopExcelTable != null)
        {
          interopExcelTable.Delete();
        }
      }
      catch (Exception ex)
      {
        success = false;
        if (logException)
        {
          MySqlSourceTrace.WriteAppErrorToLog(ex);
        }
      }

      return success;
    }

    /// <summary>
    /// Attempts to disconnect and delete a <see cref="ExcelTools.ListObject"/> trapping any COM exception.
    /// </summary>
    /// <param name="toolsExcelTable">A <see cref="ExcelTools.ListObject"/> object.</param>
    /// <param name="logException">Flag indicating whether any exception is written to the application log.</param>
    /// <returns><c>true</c> if the deletion happened without errors, <c>false</c> otherwise.</returns>
    public static bool DisconnectAndDelete(this ExcelTools.ListObject toolsExcelTable, bool logException = false)
    {
      if (toolsExcelTable == null)
      {
        return false;
      }

      if (toolsExcelTable.IsBinding)
      {
        toolsExcelTable.Disconnect();
      }

      if (toolsExcelTable.DataSource is MySqlDataTable)
      {
        var boundTable = toolsExcelTable.DataSource as MySqlDataTable;
        boundTable.Dispose();
      }

      return toolsExcelTable.DeleteSafely(logException);
    }

    /// <summary>
    /// Gets the value of an Excel cell with the correct .NET type packed in an <see cref="object"/>.
    /// </summary>
    /// <param name="excelCell">A single Excel cell.</param>
    /// <param name="useFormattedValue">Flag indicating whether the data is formatted (numbers, dates, text) or not (numbers and text).</param>
    /// <returns>The value of an Excel cell with the correct .NET type packed in an <see cref="object"/>.</returns>
    public static object GetCellPackedValue(this ExcelInterop.Range excelCell, bool useFormattedValue)
    {
      if (excelCell == null)
      {
        return null;
      }

      if (!useFormattedValue)
      {
        return excelCell.Value2;
      }

      object packedValue = excelCell.Value;
      if (!(packedValue is double))
      {
        return packedValue;
      }

      // If the Excel value is a number then check if it is formatted as a Time, since Excel does not return it automatically as a TimeSpan object.
      string cellNumberFormat = excelCell.NumberFormat;
      if (string.IsNullOrEmpty(cellNumberFormat) || !cellNumberFormat.ToLowerInvariant().Contains("h:mm"))
      {
        return packedValue;
      }

      return TimeSpan.FromDays((double)packedValue);
    }

    /// <summary>
    /// Gets a collection of <see cref="ExcelInterop.PivotTable"/> objects in the given <see cref="ExcelInterop.Worksheet"/>.
    /// This is used instead of the <see cref="ExcelInterop.Worksheet.ChartObjects"/> method since it can return either a <see cref="ExcelInterop.ChartObject"/> or a <see cref="ExcelInterop.ChartObjects"/> object.
    /// </summary>
    /// <param name="worksheet">A <see cref="ExcelInterop.Worksheet"/>.</param>
    /// <returns>a collection of <see cref="ExcelInterop.ChartObject"/> objects in the given <see cref="ExcelInterop.Worksheet"/>.</returns>
    public static IEnumerable<ExcelInterop.ChartObject> GetChartObjects(this ExcelInterop.Worksheet worksheet)
    {
      if (worksheet == null)
      {
        return null;
      }

      // Since the PivotTables method of an Excel Worksheet can return either a collection of PivotTable objects or
      // a single PivotTable instance, we need to test the type of the returned object first.
      object chartObjects = worksheet.ChartObjects();
      if (chartObjects is ExcelInterop.ChartObjects)
      {
        var chartObjectsCollection = chartObjects as ExcelInterop.ChartObjects;
        return chartObjectsCollection.Cast<ExcelInterop.ChartObject>();
      }

      var chartObject = chartObjects as ExcelInterop.ChartObject;
      return chartObject != null ? new List<ExcelInterop.ChartObject>(1) { chartObject } : null;
    }

    /// <summary>
    /// Returns an Excel range with the first row cells corresponding to the column names.
    /// </summary>
    /// <param name="mysqlDataRange">If <c>null</c> the whole first row is returned, otherwise only the column cells within the editing range.</param>
    /// <returns>The Excel range with the first row cells corresponding to the column names</returns>
    public static ExcelInterop.Range GetColumnNamesRange(this ExcelInterop.Range mysqlDataRange)
    {
      return mysqlDataRange == null ? null : mysqlDataRange.SafeResize(1, mysqlDataRange.Columns.Count);
    }

    /// <summary>
    /// Returns a <see cref="ExcelInterop.ListObject"/> contained in the given <see cref="ExcelInterop.Worksheet"/> with the given name.
    /// </summary>
    /// <param name="worksheet">A <see cref="ExcelInterop.Worksheet"/> object.</param>
    /// <param name="excelTableName">The name of the <see cref="ExcelInterop.ListObject"/> to find.</param>
    /// <returns>A <see cref="ExcelInterop.ListObject"/> contained in an <see cref="ExcelInterop.Worksheet"/> with the given names.</returns>
    public static ExcelInterop.ListObject GetExcelTableByName(this ExcelInterop.Worksheet worksheet, string excelTableName)
    {
      return worksheet == null ? null : worksheet.ListObjects.Cast<ExcelInterop.ListObject>().FirstOrDefault(lo => string.Equals(lo.Name, excelTableName, StringComparison.InvariantCultureIgnoreCase));
    }

    /// <summary>
    /// Returns a <see cref="ExcelInterop.ListObject"/> contained in a <see cref="ExcelInterop.Worksheet"/> with the given names.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> object.</param>
    /// <param name="worksheetName">The name of the <see cref="ExcelInterop.Worksheet"/> containing the <see cref="ExcelInterop.ListObject"/>.</param>
    /// <param name="excelTableName">The name of the <see cref="ExcelInterop.ListObject"/> to find.</param>
    /// <returns>A <see cref="ExcelInterop.ListObject"/> contained in an <see cref="ExcelInterop.Worksheet"/> with the given names.</returns>
    public static ExcelInterop.ListObject GetExcelTableByName(this ExcelInterop.Workbook workbook, string worksheetName, string excelTableName)
    {
      if (workbook == null)
      {
        return null;
      }

      var worksheet = workbook.Worksheets.Cast<ExcelInterop.Worksheet>().FirstOrDefault(ws => string.Equals(ws.Name, worksheetName, StringComparison.InvariantCultureIgnoreCase));
      return worksheet.GetExcelTableByName(excelTableName);
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="ExcelInterop.ListObject"/> that avoids duplicates with existing ones in the current <see cref="ExcelTools.Worksheet"/>.
    /// </summary>
    /// <param name="worksheet">A <see cref="ExcelTools.Workbook"/> object.</param>
    /// <param name="excelTableName">The proposed name for a <see cref="ExcelInterop.ListObject"/>.</param>
    /// <returns>A <see cref="ExcelInterop.ListObject"/> valid name.</returns>
    public static string GetExcelTableNameAvoidingDuplicates(this ExcelTools.Worksheet worksheet, string excelTableName)
    {
      var newName = excelTableName;
      if (worksheet == null)
      {
        return newName;
      }

      int copyIndex = 1;
      do
      {
        // Prepare Excel table name and dummy connection
        newName = excelTableName.GetExcelTableNameAvoidingDuplicates(ref copyIndex);

        // Check first if there is an orphaned Tools Excel table (leftover from a deleted Interop Excel table) and if so then attempt to free resources.
        if (!worksheet.Controls.Contains(newName))
        {
          break;
        }

        var toolsExcelTable = worksheet.Controls[newName] as ExcelTools.ListObject;
        toolsExcelTable.DisconnectAndDelete();

        // At this point a new name is needed since for some reason or bug the Globals.Factory throws an error
        // trying to check if there is a Tools Excel table already for the existing name, so continue in the loop.
      } while (true);
      return newName;
    }

    /// <summary>
    /// Gets a <see cref="ImportConnectionInfo"/> object related to the given <see cref="ExcelInterop.ListObject"/>.
    /// </summary>
    /// <param name="excelTable">A <see cref="ExcelInterop.ListObject"/>.</param>
    /// <returns>A <see cref="ImportConnectionInfo"/> object related to the given <see cref="ExcelInterop.ListObject"/>.</returns>
    public static ImportConnectionInfo GetImportConnectionInfo(this ExcelInterop.ListObject excelTable)
    {
      if (excelTable == null || !excelTable.Comment.IsGuid())
      {
        return null;
      }

      ImportConnectionInfo importConnectionInfo = null;
      var invalidConnectionInfos = new List<ImportConnectionInfo>();
      foreach (var workbookConnectionInfo in Globals.ThisAddIn.ActiveWorkbookImportConnectionInfos)
      {
        try
        {
          // Compare the comment values since they contain the GUID that identify the connection information.
          if (!string.Equals(workbookConnectionInfo.ExcelTable.Comment, excelTable.Comment, StringComparison.InvariantCultureIgnoreCase))
          {
            continue;
          }

          importConnectionInfo = workbookConnectionInfo;
          break;
        }
        catch (COMException)
        {
          // The ListObject was moved to another worksheet or when its columns had been deleted or the reference to it no longer exists.
          invalidConnectionInfos.Add(workbookConnectionInfo);
        }
      }

      // Dispose of invalid ImportConnectionInfo objects.
      if (invalidConnectionInfos.Count > 0)
      {
        invalidConnectionInfos.ForEach(invalidConnectionInfo => invalidConnectionInfo.ExcelTable.DeleteSafely(false));
        invalidConnectionInfos.ForEach(invalidConnectionInfo => Globals.ThisAddIn.StoredImportConnectionInfos.Remove(invalidConnectionInfo));
      }

      return importConnectionInfo;
    }

    /// <summary>
    /// Checks if a given <see cref="ExcelInterop.Range"/> intersects with an Excel object in its containing <see cref="ExcelInterop.Worksheet"/>.
    /// </summary>
    /// <param name="range">A <see cref="ExcelInterop.Range"/> object.</param>
    /// <param name="checkListObjects">Flag indicating whether intersection with <see cref="ExcelInterop.ListObject"/> objects is performed.</param>
    /// <param name="checkPivotTables">Flag indicating whether intersection with <see cref="ExcelInterop.PivotTable"/> objects is performed.</param>
    /// <param name="checkCharts">Flag indicating whether intersection with <see cref="ExcelInterop.ChartObject"/> objects is performed.</param>
    /// <param name="skipListObjectsWithGuid">A GUID of a <see cref="ExcelInterop.ListObject"/> to skip it from the intersection check.</param>
    /// <returns>A <see cref="ExcelInterop.Range"/> if an intersection was found, <c>null</c> otherwise.</returns>
    public static ExcelInterop.Range GetIntersectingRangeWithAnyExcelObject(this ExcelInterop.Range range, bool checkListObjects = true, bool checkPivotTables = true, bool checkCharts = true, string skipListObjectsWithGuid = null)
    {
      ExcelInterop.Range intersectingRange = null;

      if (checkListObjects)
      {
        foreach (ExcelInterop.ListObject listObject in range.Worksheet.ListObjects)
        {
          if (listObject.Comment.Length > 0 && listObject.Comment == skipListObjectsWithGuid)
          {
            continue;
          }

          intersectingRange = listObject.Range.IntersectWith(range);
          if (intersectingRange != null && intersectingRange.CountLarge != 0)
          {
            break;
          }
        }

        if (intersectingRange != null)
        {
          return intersectingRange;
        }
      }


      if (checkPivotTables)
      {
        foreach (var pivotTable in range.Worksheet.GetPivotTables())
        {
          intersectingRange = pivotTable.TableRange1.IntersectWith(range);
          if (intersectingRange == null || intersectingRange.CountLarge == 0)
          {
            continue;
          }

          intersectingRange = pivotTable.TableRange2.IntersectWith(range);
          if (intersectingRange == null || intersectingRange.CountLarge == 0)
          {
            continue;
          }

          intersectingRange = pivotTable.PageRange.IntersectWith(range);
          if (intersectingRange == null || intersectingRange.CountLarge == 0)
          {
            continue;
          }

          intersectingRange = pivotTable.DataBodyRange.IntersectWith(range);
          if (intersectingRange == null || intersectingRange.CountLarge == 0)
          {
            continue;
          }

          break;
        }

        if (intersectingRange != null)
        {
          return intersectingRange;
        }
      }

      if (checkCharts)
      {
        foreach (var chartObject in range.Worksheet.GetChartObjects())
        {
          intersectingRange = range.Worksheet.Range[chartObject.TopLeftCell, chartObject.BottomRightCell].IntersectWith(range);
          if (intersectingRange != null && intersectingRange.CountLarge != 0)
          {
            break;
          }
        }
      }

      return intersectingRange;
    }

    /// <summary>
    /// Gets the maximum column number possible for the current configuration mode in the active workbook.
    /// </summary>
    /// <param name="activeWorkbook">The active workbook.</param>
    /// <returns>The number of the maximum row index in the current configuration mode for the active workbook.</returns>
    public static int GetWorkbookMaxColumnNumber(this ExcelInterop.Workbook activeWorkbook)
    {
      return activeWorkbook == null ? 0 : activeWorkbook.Excel8CompatibilityMode ? MAXIMUM_WORKSHEET_COLUMNS_IN_COMPATIBILITY_MODE : MAXIMUM_WORKSHEET_COLUMNS_IN_LATEST_VERSION;
    }

    /// <summary>
    /// Gets the maximum row number possible for the current configuration mode in the active workbook.
    /// </summary>
    /// <param name="activeWorkbook">The active workbook.</param>
    /// <returns>The number of the maximum row index in the current configuration mode for the active workbook.</returns>
    public static int GetWorkbookMaxRowNumber(this ExcelInterop.Workbook activeWorkbook)
    {
      return activeWorkbook == null ? 0 : activeWorkbook.Excel8CompatibilityMode ? MAXIMUM_WORKSHEET_ROWS_IN_COMPATIBILITY_MODE : MAXIMUM_WORKSHEET_ROWS_IN_LATEST_VERSION;
    }

    /// <summary>
    /// Gets a <see cref="ExcelInterop.Range"/> representing the top left cell where the next result set's data should be placed.
    /// </summary>
    /// <param name="currentResultSetRange">The <see cref="ExcelInterop.Range"/> of the current result set's data imported to Excel.</param>
    /// <param name="importType">The <see cref="DbProcedure.ProcedureResultSetsImportType"/> defining what result sets are imported and how they are laid out in the Excel worksheet. </param>
    /// <param name="withPivotTable">Flag indicating whether a PivotTable is to be created along with the imported data.</param>
    /// <param name="spacing">The number of columns or rows to skip before placing the next result set.</param>
    /// <returns>A <see cref="ExcelInterop.Range"/> representing the top left cell where the next result set's data should be placed.</returns>
    public static ExcelInterop.Range GetNextResultSetTopLeftCell(this ExcelInterop.Range currentResultSetRange, DbProcedure.ProcedureResultSetsImportType importType, bool withPivotTable, int spacing = 1)
    {
      if (currentResultSetRange == null)
      {
        return null;
      }

      ExcelInterop.Range currentTopLeftCell = currentResultSetRange.Cells[1, 1];
      int columnsOffset = 0;
      int rowsOffset = 0;
      switch (importType)
      {
        case DbProcedure.ProcedureResultSetsImportType.AllResultSetsHorizontally:
          var pivotTablePlaceHolderColumns = withPivotTable ? PIVOT_TABLES_PLACEHOLDER_DEFAULT_COLUMNS_SIZE + spacing : 0;
          columnsOffset = currentResultSetRange.Columns.Count + pivotTablePlaceHolderColumns + spacing;
          break;

        case DbProcedure.ProcedureResultSetsImportType.AllResultSetsVertically:
          var pivotTablePlaceHolderRows = withPivotTable ? PIVOT_TABLES_PLACEHOLDER_DEFAULT_ROWS_SIZE : 0;
          rowsOffset = Math.Max(currentResultSetRange.Rows.Count, pivotTablePlaceHolderRows) + spacing;
          if (Globals.ThisAddIn.ActiveWorkbook.Excel8CompatibilityMode && currentTopLeftCell.Row + rowsOffset > UInt16.MaxValue)
          {
            return null;
          }
          break;
      }

      return currentTopLeftCell.Offset[rowsOffset, columnsOffset];
    }

    /// <summary>
    /// Gets an <see cref="ExcelInterop.Range"/> object that represents all non-empty cells.
    /// </summary>
    /// <param name="range">A <see cref="ExcelInterop.Range"/> object.</param>
    /// <returns>An <see cref="ExcelInterop.Range"/> object that represents all non-empty cells.</returns>
    public static ExcelInterop.Range GetNonEmptyRange(this ExcelInterop.Range range)
    {
      if (range == null)
      {
        return null;
      }

      // Perform this validation since the SpecialCells method returns all cells in Worksheet if only 1 cell is in the range.
      if (range.Cells.Count == 1)
      {
        return range.Value != null ? range : null;
      }

      ExcelInterop.Range rangeWithFormulas = null;
      ExcelInterop.Range rangeWithConstants = null;
      ExcelInterop.Range finalRange = null;

      // SpecialCells method throws exception if no cells are found matching criteria (possible bug in VSTO).
      try
      {
        rangeWithFormulas = range.SpecialCells(ExcelInterop.XlCellType.xlCellTypeFormulas);
      }
      catch
      {
      }

      // SpecialCells method throws exception if no cells are found matching criteria (possible bug in VSTO).
      try
      {
        rangeWithConstants = range.SpecialCells(ExcelInterop.XlCellType.xlCellTypeConstants, (int)ExcelInterop.XlSpecialCellsValue.xlTextValues + (int)ExcelInterop.XlSpecialCellsValue.xlNumbers);
      }
      catch
      {
      }

      if (rangeWithFormulas != null && rangeWithConstants != null)
      {
        finalRange = Globals.ThisAddIn.Application.Union(rangeWithFormulas, rangeWithConstants);
      }
      else if (rangeWithFormulas != null)
      {
        finalRange = rangeWithFormulas;
      }
      else if (rangeWithConstants != null)
      {
        finalRange = rangeWithConstants;
      }

      return finalRange;
    }

    /// <summary>
    /// Gets an <see cref="ExcelInterop.Range"/> object representing an unique rectangular area where cells inside it contain values.
    /// There may be empty cells inside, the rectangular area is calculated by finding a topmost-leftmost cell with data and 
    /// a bottommost-rightmost cell with data to then compose the corners of the rectangular area.
    /// </summary>
    /// <param name="range">A <see cref="ExcelInterop.Range"/> object.</param>
    /// <returns>an <see cref="ExcelInterop.Range"/> object representing an unique rectangular area where cells inside it contain values.</returns>
    public static ExcelInterop.Range GetNonEmptyRectangularAreaRange(this ExcelInterop.Range range)
    {
      if (range == null)
      {
        return null;
      }

      // Inf only one cell in range no need to even perform the Find calls.
      if (range.Cells.CountLarge == 1)
      {
        return range.Value != null ? range : null;
      }

      ExcelInterop.Range firstOriginalCell = range.Cells[1, 1];
      ExcelInterop.Range lastRowCell = range.Cells.Find(
        "*",
        firstOriginalCell,
        ExcelInterop.XlFindLookIn.xlValues,
        Type.Missing,
        ExcelInterop.XlSearchOrder.xlByRows,
        ExcelInterop.XlSearchDirection.xlPrevious,
        Type.Missing,
        Type.Missing,
        Type.Missing);
      if (lastRowCell == null)
      {
        return null;
      }

      int lastCellRow = lastRowCell.Row;
      ExcelInterop.Range lastColumnCell = range.Cells.Find(
        "*",
        firstOriginalCell,
        ExcelInterop.XlFindLookIn.xlValues,
        Type.Missing,
        ExcelInterop.XlSearchOrder.xlByColumns,
        ExcelInterop.XlSearchDirection.xlPrevious,
        Type.Missing,
        Type.Missing,
        Type.Missing);
      if (lastColumnCell == null)
      {
        return null;
      }

      int lastCellColumn = lastColumnCell.Column;
      ExcelInterop.Range lastCell = range.Worksheet.Cells[lastCellRow, lastCellColumn];
      ExcelInterop.Range firstRowCell = range.Cells.Find(
        "*",
        lastCell,
        ExcelInterop.XlFindLookIn.xlValues,
        Type.Missing,
        ExcelInterop.XlSearchOrder.xlByRows,
        ExcelInterop.XlSearchDirection.xlNext,
        Type.Missing,
        Type.Missing,
        Type.Missing);
      if (firstRowCell == null)
      {
        return null;
      }

      int firstCellRow = firstRowCell.Row;
      ExcelInterop.Range firstColumnCell = range.Cells.Find(
        "*",
        lastCell,
        ExcelInterop.XlFindLookIn.xlValues,
        Type.Missing,
        ExcelInterop.XlSearchOrder.xlByColumns,
        ExcelInterop.XlSearchDirection.xlNext,
        Type.Missing,
        Type.Missing,
        Type.Missing);
      if (firstColumnCell == null)
      {
        return null;
      }

      int firstCellColumn = firstColumnCell.Column;
      ExcelInterop.Range firstCell = range.Worksheet.Cells[firstCellRow, firstCellColumn];
      return range.Worksheet.Range[firstCell, lastCell];
    }

    /// <summary>
    /// Gets the active workbook unique identifier if exists, if not, creates one and returns it.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> object.</param>
    /// <returns>The guid string for the current workbook.</returns>
    public static string GetOrCreateId(this ExcelInterop.Workbook workbook)
    {
      if (workbook == null || workbook.CustomDocumentProperties == null)
      {
        return null;
      }

      var guid = ((DocumentProperties)workbook.CustomDocumentProperties).Cast<DocumentProperty>().FirstOrDefault(property => property.Name.Equals("WorkbookGuid"));
      if (guid != null)
      {
        return guid.Value.ToString();
      }

      string newGuid = Guid.NewGuid().ToString();
      DocumentProperties properties = workbook.CustomDocumentProperties;
      properties.Add("WorkbookGuid", false, MsoDocProperties.msoPropertyTypeString, newGuid);
      return newGuid;
    }

    /// <summary>
    /// Gets a <see cref="ExcelInterop.Worksheet"/> with a given name existing in the given <see cref="ExcelInterop.Workbook"/> or creates a new one.
    /// </summary>
    /// <param name="workbook">The <see cref="ExcelInterop.Workbook"/> to look for a <see cref="ExcelInterop.Worksheet"/>.</param>
    /// <param name="workSheetName">The name of the new <see cref="ExcelInterop.Worksheet"/>.</param>
    /// <param name="selectTopLeftCell">Flag indicating whether the cell A1 receives focus.</param>
    /// <returns>The existing or new <see cref="ExcelInterop.Worksheet"/> object.</returns>
    public static ExcelInterop.Worksheet GetOrCreateWorksheet(this ExcelInterop.Workbook workbook, string workSheetName, bool selectTopLeftCell)
    {
      if (workbook == null)
      {
        return null;
      }

      var existingWorksheet = workbook.Worksheets.Cast<ExcelInterop.Worksheet>().FirstOrDefault(worksheet => string.Equals(worksheet.Name, workSheetName, StringComparison.InvariantCulture));
      if (existingWorksheet == null)
      {
        existingWorksheet = workbook.CreateWorksheet(workSheetName, selectTopLeftCell);
      }
      else if (selectTopLeftCell)
      {
        existingWorksheet.SelectTopLeftCell();
      }

      return existingWorksheet;
    }

    /// <summary>
    /// Gets the name of the parent <see cref="ExcelInterop.Workbook"/> of the given <see cref="ExcelInterop.Worksheet"/>.
    /// </summary>
    /// <param name="worksheet">A <see cref="ExcelInterop.Worksheet"/> object.</param>
    /// <returns>The name of the parent <see cref="ExcelInterop.Workbook"/>.</returns>
    public static string GetParentWorkbookName(this ExcelInterop.Worksheet worksheet)
    {
      if (worksheet == null)
      {
        return string.Empty;
      }

      var parentWorkbook = worksheet.Parent as ExcelInterop.Workbook;
      return parentWorkbook != null ? parentWorkbook.Name : string.Empty;
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="ExcelInterop.PivotTable"/> that avoids duplicates with existing ones in the current <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="pivotTableName">The proposed name for a <see cref="ExcelInterop.PivotTable"/>.</param>
    /// <returns>A <see cref="ExcelInterop.PivotTable"/> valid name.</returns>
    public static string GetPivotTableNameAvoidingDuplicates(this string pivotTableName)
    {
      return pivotTableName.GetPivotTableNameAvoidingDuplicates(1);
    }

    /// <summary>
    /// Gets the <see cref="ExcelInterop.Range"/> of the Excel cell where a <see cref="ExcelInterop.PivotTable"/> will be placed (its top left corner).
    /// </summary>
    /// <param name="fromSourceRange">The <see cref="ExcelInterop.Range"/> of the <see cref="ExcelInterop.PivotTable"/>'s source data.</param>
    /// <param name="pivotPosition">The <see cref="PivotTablePosition"/> relative to the source data.</param>
    /// <param name="skipCount">The number of rows or columns to skip before placing the <see cref="ExcelInterop.PivotTable"/>.</param>
    /// <returns>The <see cref="ExcelInterop.Range"/> of the Excel cell where a <see cref="ExcelInterop.PivotTable"/> will be placed (its top left corner).</returns>
    public static ExcelInterop.Range GetPivotTableTopLeftCell(this ExcelInterop.Range fromSourceRange, PivotTablePosition pivotPosition, int skipCount = 1)
    {
      if (fromSourceRange == null)
      {
        return null;
      }

      ExcelInterop.Range atCell = fromSourceRange.Cells[1, 1];
      switch (pivotPosition)
      {
        case PivotTablePosition.Below:
          atCell = atCell.Offset[fromSourceRange.Rows.Count + skipCount, 0];
          break;

        case PivotTablePosition.Right:
          atCell = atCell.Offset[0, fromSourceRange.Columns.Count + skipCount];
          break;
      }

      return atCell;
    }

    /// <summary>
    /// Gets a collection of <see cref="ExcelInterop.PivotTable"/> objects in the given <see cref="ExcelInterop.Worksheet"/>.
    /// This is used instead of the <see cref="ExcelInterop.Worksheet.PivotTables"/> method since it can return either a <see cref="ExcelInterop.PivotTables"/> or a <see cref="ExcelInterop.PivotTable"/> object.
    /// </summary>
    /// <param name="worksheet">A <see cref="ExcelInterop.Worksheet"/>.</param>
    /// <returns>a collection of <see cref="ExcelInterop.PivotTable"/> objects in the given <see cref="ExcelInterop.Worksheet"/>.</returns>
    public static IEnumerable<ExcelInterop.PivotTable> GetPivotTables(this ExcelInterop.Worksheet worksheet)
    {
      if (worksheet == null)
      {
        return null;
      }

      // Since the PivotTables method of an Excel Worksheet can return either a collection of PivotTable objects or
      // a single PivotTable instance, we need to test the type of the returned object first.
      object pivotTables = worksheet.PivotTables();
      if (pivotTables is ExcelInterop.PivotTables)
      {
        var pivotTablesCollection = pivotTables as ExcelInterop.PivotTables;
        return pivotTablesCollection.Cast<ExcelInterop.PivotTable>();
      }

      var pivotTable = pivotTables as ExcelInterop.PivotTable;
      return pivotTable != null ? new List<ExcelInterop.PivotTable>(1) { pivotTable } : null;
    }

    /// <summary>
    /// Gets the a protection key for the provided worksheet if exists.
    /// </summary>
    /// <param name="worksheet">A <see cref="ExcelInterop.Worksheet"/> object.</param>
    /// <returns>The worksheet's protection key if the property exist, otherwise returns null.</returns>
    public static string GetProtectionKey(this ExcelInterop.Worksheet worksheet)
    {
      if (worksheet == null)
      {
        return null;
      }

      ExcelInterop.CustomProperties properties = worksheet.CustomProperties;
      if (properties == null)
      {
        return null;
      }

      var guid = properties.Cast<ExcelInterop.CustomProperty>().FirstOrDefault(property => property.Name.Equals("WorksheetGuid"));
      return guid == null ? null : guid.Value.ToString();
    }

    /// <summary>
    /// Gets a linear array with the values of the cells of a single row within an <see cref="ExcelInterop.Range"/>.
    /// </summary>
    /// <param name="range">A <see cref="ExcelInterop.Range"/> object.</param>
    /// <param name="rowIndex">The index of the row within the <see cref="ExcelInterop.Range"/> to get values from.</param>
    /// <param name="useFormattedValues">Flag indicating whether the data is formatted (numbers, dates, text) or not (numbers and text).</param>
    /// <returns>A linear array with the values of the cells of a single row within an <see cref="ExcelInterop.Range"/>.</returns>
    public static object[] GetRowValuesAsLinearArray(this ExcelInterop.Range range, int rowIndex, bool useFormattedValues = true)
    {
      if (range == null || rowIndex < 1 || rowIndex > range.Rows.Count)
      {
        return null;
      }

      ExcelInterop.Range rowRange = range.Rows[rowIndex];
      int columnsCount = rowRange.Columns.Count;
      var linearArray = new object[columnsCount];
      for (int columnIndex = 1; columnIndex <= columnsCount; columnIndex++)
      {
        var excelCell = rowRange.Cells[1, columnIndex] as ExcelInterop.Range;
        linearArray[columnIndex - 1] = excelCell.GetCellPackedValue(useFormattedValues);
      }

      return linearArray;
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="ExcelInterop.WorkbookConnection"/> that avoids duplicates with existing ones in the current <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="workbookConnectionName">The proposed name for a <see cref="ExcelInterop.WorkbookConnection"/>.</param>
    /// <returns>A <see cref="ExcelInterop.WorkbookConnection"/> valid name.</returns>
    public static string GetWorkbookConnectionNameAvoidingDuplicates(this string workbookConnectionName)
    {
      return workbookConnectionName.GetWorkbookConnectionNameAvoidingDuplicates(1);
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="ExcelInterop.Worksheet"/> that avoids duplicates with existing ones in the given <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/>.</param>
    /// <param name="worksheetName">The proposed name for a <see cref="ExcelInterop.Worksheet"/>.</param>
    /// <returns>A <see cref="ExcelInterop.Worksheet"/> valid name.</returns>
    public static string GetWorksheetNameAvoidingDuplicates(this ExcelInterop.Workbook workbook, string worksheetName)
    {
      return workbook.GetWorksheetNameAvoidingDuplicates(worksheetName, 0);
    }

    /// <summary>
    /// Checks if a given <see cref="ExcelInterop.Range"/> intersects with any Excel object in its containing <see cref="ExcelInterop.Worksheet"/>.
    /// </summary>
    /// <param name="range">A <see cref="ExcelInterop.Range"/> object.</param>
    /// <param name="checkListObjects">Flag indicating whether intersection with <see cref="ExcelInterop.ListObject"/> objects is performed.</param>
    /// <param name="checkPivotTables">Flag indicating whether intersection with <see cref="ExcelInterop.PivotTable"/> objects is performed.</param>
    /// <param name="checkCharts">Flag indicating whether intersection with <see cref="ExcelInterop.ChartObject"/> objects is performed.</param>
    /// <param name="skipListObjectsWithGuid">A GUID of a <see cref="ExcelInterop.ListObject"/> to skip it from the intersection check.</param>
    /// <returns><c>true</c> if the given <see cref="ExcelInterop.Range"/> intersects with any Excel table in its containing <see cref="ExcelInterop.Worksheet"/>, <c>false</c> otherwise.</returns>
    public static bool IntersectsWithAnyExcelObject(this ExcelInterop.Range range, bool checkListObjects = true, bool checkPivotTables = true, bool checkCharts = true, string skipListObjectsWithGuid = null)
    {
      ExcelInterop.Range intersectingRange = range.GetIntersectingRangeWithAnyExcelObject(checkListObjects, checkPivotTables, checkCharts, skipListObjectsWithGuid);
      return intersectingRange != null && intersectingRange.CountLarge != 0;
    }

    /// <summary>
    /// Returns a Range object that represents the rectangular intersection of the given range with another range.
    /// </summary>
    /// <param name="range">A <see cref="ExcelInterop.Range"/> object.</param>
    /// <param name="otherRange">An intersecting <see cref="ExcelInterop.Range"/> object.</param>
    /// <returns>A <see cref="ExcelInterop.Range"/> object representing the rectangular intersection of the given range with another range.</returns>
    public static ExcelInterop.Range IntersectWith(this ExcelInterop.Range range, ExcelInterop.Range otherRange)
    {
      return Globals.ThisAddIn.Application.Intersect(range, otherRange);
    }

    /// <summary>
    /// Invokes a method present in the given target object receiving parameters in an English locale that are transformed to the native Excel locale.
    /// </summary>
    /// <param name="target">The Excel object containing the method.</param>
    /// <param name="name">The name of the method to be invoked.</param>
    /// <param name="args">The arguments passed to the method parameters.</param>
    /// <returns>Any return value from the invoked method.</returns>
    public static object InvokeMethodInternational(object target, string name, params object[] args)
    {
      return target.GetType().InvokeMember(
        name,
        System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance,
        null,
        target,
        args,
        new System.Globalization.CultureInfo(EN_US_LOCALE_CODE));
    }

    /// <summary>
    /// Checks if the <see cref="ExcelInterop.Workbook"/> has not been ever saved to disk.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> object.</param>
    /// <returns><c>true</c> if the <see cref="ExcelInterop.Workbook"/> has not been ever saved to disk, <c>false</c> otherwise.</returns>
    public static bool IsNew(this ExcelInterop.Workbook workbook)
    {
      // If the Path value is empty it means the Workbook has not been ever saved to disk.
      return workbook != null && string.IsNullOrEmpty(workbook.Path);
    }

    /// <summary>
    /// Checks if the PowerPivot add-in is installed in the computer.
    /// </summary>
    /// <returns><c>true</c> if PowerPivot is installed, <c>false</c> otherwise.</returns>
    public static bool IsPowerPivotEnabled()
    {
      return Globals.ThisAddIn.Application.AddIns.Cast<ExcelInterop.AddIn>().Any(addIn => addIn.Title.Contains("PowerPivot") && addIn.Name == "PowerPivotExcelClientAddIn.dll" && addIn.Installed && addIn.IsOpen);
    }

    /// <summary>
    /// Checks if the <see cref="ExcelInterop.Worksheet"/> is visible.
    /// </summary>
    /// <param name="worksheet">A <see cref="ExcelInterop.Worksheet"/> object.</param>
    /// <returns><c>true</c> if the <see cref="ExcelInterop.Worksheet"/> is visible, <c>false</c> otherwise.</returns>
    public static bool IsVisible(this ExcelInterop.Worksheet worksheet)
    {
      return worksheet != null && worksheet.Visible == ExcelInterop.XlSheetVisibility.xlSheetVisible;
    }

    /// <summary>
    /// Returns a list of <see cref="ExcelInterop.TableStyle"/> names available to be used within the given <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> object.</param>
    /// <returns>A list of style names available in the given <see cref="ExcelInterop.Workbook"/>.</returns>
    public static List<string> ListTableStyles(this ExcelInterop.Workbook workbook)
    {
      return workbook == null ? null : (from ExcelInterop.TableStyle tableStyle in workbook.TableStyles orderby tableStyle.Name select tableStyle.Name).ToList();
    }

    /// <summary>
    /// Locks the given Excel range and sets its fill color accordingly.
    /// </summary>
    /// <param name="range">The <see cref="ExcelInterop.Range"/> to lock or unlock.</param>
    /// <param name="lockRange">Flag indicating whether the Excel range is locked or unlocked.</param>
    public static void LockRange(this ExcelInterop.Range range, bool lockRange)
    {
      if (lockRange)
      {
        range.Interior.Color = LockedCellsOleColor;
      }
      else
      {
        range.Interior.ColorIndex = ExcelInterop.XlColorIndex.xlColorIndexNone;
      }

      range.Locked = lockRange;
    }

    /// <summary>
    /// Unprotects the given Excel worksheet and stops listening for its Change event.
    /// </summary>
    /// <param name="worksheet">The <see cref="ExcelInterop.Worksheet"/> to unprotect.</param>
    /// <param name="changeEventHandlerDelegate">The change event handler delegate of the Excel worksheet.</param>
    /// <param name="protectionKey">The key used to unprotect the worksheet.</param>
    /// <param name="mysqlDataRange">The Excel range containing the MySQL data being edited.</param>
    public static void ProtectEditingWorksheet(this ExcelInterop.Worksheet worksheet, ExcelInterop.DocEvents_ChangeEventHandler changeEventHandlerDelegate, string protectionKey, ExcelInterop.Range mysqlDataRange)
    {
      if (worksheet == null)
      {
        return;
      }

      if (changeEventHandlerDelegate != null)
      {
        worksheet.Change += changeEventHandlerDelegate;
      }

      if (mysqlDataRange != null)
      {
        ExcelInterop.Range extendedRange = mysqlDataRange.Range["A2"];
        extendedRange = extendedRange.SafeResize(mysqlDataRange.Rows.Count - 1, worksheet.Columns.Count);
        extendedRange.Locked = false;

        // Column names range code
        ExcelInterop.Range headersRange = mysqlDataRange.GetColumnNamesRange();
        headersRange.LockRange(true);
      }

      worksheet.Protect(protectionKey,
                        false,
                        true,
                        true,
                        true,
                        true,
                        true,
                        false,
                        false,
                        false,
                        false,
                        false,
                        true,
                        false,
                        false,
                        false);
    }

    /// <summary>
    /// Refreshes all <see cref="ExcelInterop.ListObject"/> objects in the given <see cref="ExcelInterop.Worksheet"/>.
    /// If a <see cref="ExcelInterop.ListObject"/> is related to a <see cref="ImportConnectionInfo"/> object then its custom refresh functionality is performed, otherwise the native one is.
    /// </summary>
    /// <param name="worksheet">A <see cref="ExcelInterop.Worksheet"/>.</param>
    /// <returns><c>true</c> if <see cref="ExcelInterop.ListObject"/>s in other <see cref="ExcelInterop.Worksheet"/>s should be refreshed as well, <c>false</c> to stop refreshing.</returns>
    public static bool RefreshAllListObjects(this ExcelInterop.Worksheet worksheet)
    {
      if (worksheet == null)
      {
        return true;
      }

      // If the ExcelInterop.ListObject is tied to MySQL data, then it will be refreshed and skip to the next one.
      // Otherwise the standard ExcelInterop.ListObject.Refresh is called.
      bool continueLoading = true;
      foreach (var listObject in worksheet.ListObjects.Cast<ExcelInterop.ListObject>().Where(listObject => !listObject.RefreshMySqlData()))
      {
        try
        {
          listObject.Refresh();
        }
        catch (Exception ex)
        {
          MySqlSourceTrace.WriteAppErrorToLog(ex);
          var infoProperties = InfoDialogProperties.GetYesNoDialogProperties(
            InfoDialog.InfoType.Error,
            Resources.OperationErrorTitle,
            string.Format(Resources.StandardListObjectRefreshError, listObject.DisplayName),
            Resources.ContinueRefreshingExcelTablesQuestionText,
            listObject.Comment.IsGuid() ? Resources.StandardListObjectRefreshMoreInfo : ex.GetFormattedMessage());
          infoProperties.WordWrapMoreInfo = true;
          if (InfoDialog.ShowDialog(infoProperties).DialogResult != DialogResult.Yes)
          {
            continueLoading = false;
            break;
          }
        }
      }

      return continueLoading;
    }

    /// <summary>
    /// Refreshes all <see cref="ExcelInterop.PivotTable"/> objects in the given <see cref="ExcelInterop.Worksheet"/>.
    /// </summary>
    /// <param name="worksheet">A <see cref="ExcelInterop.Worksheet"/>.</param>
    /// <returns><c>true</c> if <see cref="ExcelInterop.ListObject"/>s in other <see cref="ExcelInterop.Worksheet"/>s should be refreshed as well, <c>false</c> to stop refreshing.</returns>
    public static bool RefreshAllPivotTables(this ExcelInterop.Worksheet worksheet)
    {
      if (worksheet == null)
      {
        return true;
      }

      bool continueLoading = true;
      foreach (var pivotTable in worksheet.GetPivotTables())
      {
        try
        {
          if (pivotTable.RefreshTable())
          {
            pivotTable.Update();
          }
        }
        catch (Exception ex)
        {
          MySqlSourceTrace.WriteAppErrorToLog(ex);
          var infoProperties = InfoDialogProperties.GetYesNoDialogProperties(
            InfoDialog.InfoType.Error,
            Resources.OperationErrorTitle,
            string.Format(Resources.StandardPivotTableRefreshError, pivotTable.Name),
            Resources.ContinueRefreshingPivotTablesQuestionText,
            ex.GetFormattedMessage());
          infoProperties.WordWrapMoreInfo = true;
          if (InfoDialog.ShowDialog(infoProperties).DialogResult != DialogResult.Yes)
          {
            continueLoading = false;
            break;
          }
        }
      }

      return continueLoading;
    }

    /// <summary>
    /// Checks if the given <see cref="ExcelInterop.ListObject"/> object is related to a <see cref="ImportConnectionInfo"/> object in order to perform its custom refresh functionality.
    /// </summary>
    /// <param name="excelTable">A <see cref="ExcelInterop.ListObject"/>.</param>
    /// <returns><c>true</c> if the <see cref="ExcelInterop.ListObject"/> has a related <see cref="ImportConnectionInfo"/>, <c>false</c> otherwise.</returns>
    public static bool RefreshMySqlData(this ExcelInterop.ListObject excelTable)
    {
      var importConnectionInfo = excelTable.GetImportConnectionInfo();
      bool hasImportConnectionInfo = importConnectionInfo != null;
      if (hasImportConnectionInfo)
      {
        importConnectionInfo.Refresh();
      }
      else if (excelTable != null && excelTable.Comment.IsGuid())
      {
        // Display an error to users since the ListObject has a comment with a GUID in it that most probably
        // was added by MySQL for Excel, but its corresponding information was not found in the settings file
        hasImportConnectionInfo = true;
        InfoDialog.ShowDialog(InfoDialogProperties.GetErrorDialogProperties(
            Resources.OperationErrorTitle,
            string.Format(Resources.StandardListObjectRefreshError, excelTable.DisplayName),
            null,
            Resources.StandardListObjectRefreshMoreInfo));
      }

      return hasImportConnectionInfo;
    }

    /// <summary>
    /// Removes the protectionKey property (if exists) for the current worksheet.
    /// </summary>
    /// <param name="worksheet">A <see cref="ExcelInterop.Worksheet"/> object.</param>
    public static void RemoveProtectionKey(this ExcelInterop.Worksheet worksheet)
    {
      if (worksheet == null)
      {
        return;
      }

      var protectionKeyProperty = worksheet.CustomProperties.Cast<ExcelInterop.CustomProperty>().FirstOrDefault(property => property.Name.Equals("WorksheetGuid"));
      if (protectionKeyProperty != null)
      {
        protectionKeyProperty.Delete();
      }
    }

    /// <summary>
    /// Indicates whether a number of rows relative to the actual position exceeds the <see cref="ExcelInterop.Worksheet"/> rows limit.
    /// </summary>
    /// <param name="rows">The number of rows to the right of the actual position to evaluate.</param>
    /// <returns><c>true</c> if number of rows to the right of the actual position exceeds the <see cref="ExcelInterop.Worksheet"/> rows limit, <c>false</c> otherwise.</returns>
    public static bool CheckIfRowsExceedWorksheetLimit(long rows)
    {
      var atCell = Globals.ThisAddIn.Application.ActiveCell;
      if (atCell == null)
      {
        return false;
      }

      var currentRow = atCell.Row;
      var activeWorkbook = atCell.Worksheet.Parent as ExcelInterop.Workbook;
      if (activeWorkbook == null)
      {
        return false;
      }

      var maxRowNumber = activeWorkbook.GetWorkbookMaxRowNumber();
      var totalRows = Math.Min(rows, (maxRowNumber - currentRow) + 1);
      return rows > totalRows;
    }

    /// <summary>
    /// Returns a valid resized range to the given dimensions and sheet location.
    /// </summary>
    /// <param name="range">The range to be resized.</param>
    /// /// <param name="rows">The number of rows the range will be resized to in case it doesn't exceed the available <see cref="ExcelInterop.Worksheet"/> space.</param>
    /// /// <param name="columns">The number of columns the range will be resized to in case it doesn't exceed the available <see cref="ExcelInterop.Worksheet"/> space.</param>
    /// <returns>A <see cref="ExcelInterop.Range"/> within the available <see cref="ExcelInterop.Worksheet"/> space </returns>
    public static ExcelInterop.Range SafeResize(this ExcelInterop.Range range, long rows, long columns)
    {
      if (range == null)
      {
        return null;
      }

      var currentRow = range.Row;
      var currentColumn = range.Column;
      var activeWorkbook = range.Worksheet.Parent as ExcelInterop.Workbook;
      if (activeWorkbook == null)
      {
        return null;
      }

      var maxRowNumber = activeWorkbook.GetWorkbookMaxRowNumber();
      var maxColumnNumber = activeWorkbook.GetWorkbookMaxColumnNumber();
      var totalRows = Math.Min(rows, maxRowNumber - currentRow + 1);
      var totalColumns = Math.Min(columns, maxColumnNumber - currentColumn + 1);
      var result = range.Resize[totalRows, totalColumns];
      return result;
    }

    /// <summary>
    /// Places the A1 cell of the given <see cref="ExcelInterop.Worksheet"/> in focus.
    /// </summary>
    /// <param name="worksheet">A <see cref="ExcelInterop.Worksheet"/> object.</param>
    public static void SelectTopLeftCell(this ExcelInterop.Worksheet worksheet)
    {
      if (worksheet == null)
      {
        return;
      }

      Globals.ThisAddIn.Application.Goto(worksheet.Range["A1", Type.Missing], false);
    }

    /// <summary>
    /// Sets the font and color properties of a <see cref="ExcelInterop.TableStyleElement"/> as a MySQL minimalistic style.
    /// </summary>
    /// <param name="styleElement">The <see cref="ExcelInterop.TableStyleElement"/> to modify.</param>
    /// <param name="interiorOleColor">The OLE color to paint the Excel cells interior with.</param>
    /// <param name="makeBold">Flag indicating whether the font is set to bold.</param>
    public static void SetAsMySqlStyle(this ExcelInterop.TableStyleElement styleElement, int interiorOleColor = EMPTY_CELLS_OLE_COLOR, bool makeBold = false)
    {
      styleElement.Font.Color = ColorTranslator.ToOle(Color.Black);
      if (interiorOleColor == EMPTY_CELLS_OLE_COLOR)
      {
        styleElement.Interior.ColorIndex = ExcelInterop.XlColorIndex.xlColorIndexNone;
      }
      else
      {
        styleElement.Interior.Color = interiorOleColor;
      }

      styleElement.Font.Bold = makeBold;
    }

    /// <summary>
    /// Sets the style of the first row of a given range that represents its header with column names.
    /// </summary>
    /// <param name="range">A <see cref="ExcelInterop.Range"/> object.</param>
    public static void SetHeaderStyle(this ExcelInterop.Range range)
    {
      if (range == null)
      {
        return;
      }

      ExcelInterop.Range headerRange = range.GetColumnNamesRange();
      headerRange.SetInteriorColor(LockedCellsOleColor);
      headerRange.Font.Bold = true;
    }

    /// <summary>
    /// Sets the range cells interior color to the specified OLE color.
    /// </summary>
    /// <param name="range">Excel range to have their interior color changed.</param>
    /// <param name="oleColor">The new interior color for the Excel cells.</param>
    public static void SetInteriorColor(this ExcelInterop.Range range, int oleColor)
    {
      if (range == null)
      {
        return;
      }

      if (oleColor > 0)
      {
        range.Interior.Color = oleColor;
      }
      else
      {
        range.Interior.ColorIndex = ExcelInterop.XlColorIndex.xlColorIndexNone;
      }
    }

    /// <summary>
    /// Sets the interior color of all the Excel ranges within the given list to the specified color.
    /// </summary>
    /// <param name="rangesList">The list of Excel ranges to have their fill color changed.</param>
    /// <param name="oleColor">The new fill color for the Excel cells.</param>
    public static void SetInteriorColor(this IList<ExcelInterop.Range> rangesList, int oleColor)
    {
      if (rangesList == null)
      {
        return;
      }

      foreach (var range in rangesList)
      {
        range.SetInteriorColor(oleColor);
      }
    }

    /// <summary>
    /// Sets the protection key for the worksheet.
    /// </summary>
    /// <returns>The new protection key provided for the worksheet.</returns>
    public static bool StoreProtectionKey(this ExcelInterop.Worksheet worksheet, string protectionKey)
    {
      if (worksheet == null || string.IsNullOrEmpty(protectionKey))
      {
        return false;
      }

      var protectionKeyProperty = worksheet.CustomProperties.Cast<ExcelInterop.CustomProperty>().FirstOrDefault(property => property.Name.Equals("WorksheetGuid"));
      if (protectionKeyProperty == null)
      {
        ExcelInterop.CustomProperties properties = worksheet.CustomProperties;
        properties.Add("WorksheetGuid", protectionKey);
        return true;
      }
      protectionKeyProperty.Value = protectionKey;
      return true;
    }

    /// <summary>
    /// Unprotects the given Excel worksheet and stops listening for its Change event.
    /// </summary>
    /// <param name="worksheet">The Excel worksheet to unprotect.</param>
    /// <param name="changeEventHandlerDelegate">The change event handler delegate of the Excel worksheet.</param>
    /// <param name="protectionKey">The key used to unprotect the worksheet.</param>
    public static void UnprotectEditingWorksheet(this ExcelInterop.Worksheet worksheet, ExcelInterop.DocEvents_ChangeEventHandler changeEventHandlerDelegate, string protectionKey)
    {
      if (worksheet == null)
      {
        return;
      }

      if (changeEventHandlerDelegate != null)
      {
        worksheet.Change -= changeEventHandlerDelegate;
      }

      worksheet.Unprotect(worksheet.GetProtectionKey());
    }

    /// <summary>
    /// Checks if an Excel <see cref="ExcelInterop.Worksheet"/> with a given name exists in the given <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="workbook">The <see cref="ExcelInterop.Workbook"/>.</param>
    /// <param name="worksheetName">Name of the <see cref="ExcelInterop.Worksheet"/>.</param>
    /// <returns><c>true</c> if the <see cref="ExcelInterop.Worksheet"/> exists, <c>false</c> otherwise.</returns>
    public static bool WorksheetExists(this ExcelInterop.Workbook workbook, string worksheetName)
    {
      if (workbook == null || worksheetName.Length <= 0)
      {
        return false;
      }

      return workbook.Worksheets.Cast<ExcelInterop.Worksheet>().Any(ws => string.Equals(ws.Name, worksheetName, StringComparison.InvariantCulture));
    }

    /// <summary>
    /// Checks if an Excel <see cref="ExcelInterop.Worksheet"/> with a given name exists in a <see cref="ExcelInterop.Workbook"/> with the given name.
    /// </summary>
    /// <param name="workbookName">Name of the <see cref="ExcelInterop.Workbook"/>.</param>
    /// <param name="worksheetName">Name of the <see cref="ExcelInterop.Worksheet"/>.</param>
    /// <returns><c>true</c> if the <see cref="ExcelInterop.Worksheet"/> exists, <c>false</c> otherwise.</returns>
    public static bool WorksheetExists(string workbookName, string worksheetName)
    {
      if (workbookName.Length <= 0)
      {
        return false;
      }

      var wBook = Globals.ThisAddIn.Application.Workbooks.Cast<ExcelInterop.Workbook>().FirstOrDefault(wb => string.Equals(wb.Name, workbookName, StringComparison.InvariantCulture));
      return wBook != null && wBook.WorksheetExists(worksheetName);
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="ExcelInterop.ListObject"/> that avoids duplicates with existing ones in the current <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="excelTableName">The proposed name for a <see cref="ExcelInterop.ListObject"/>.</param>
    /// <param name="copyIndex">Consecutive number for a <see cref="ExcelInterop.ListObject"/> if duplicates are found.</param>
    /// <returns>A <see cref="ExcelInterop.ListObject"/> valid name.</returns>
    private static string GetExcelTableNameAvoidingDuplicates(this string excelTableName, ref int copyIndex)
    {
      var activeWorkbook = Globals.ThisAddIn.ActiveWorkbook;
      if (activeWorkbook == null)
      {
        return excelTableName;
      }

      string retName;
      do
      {
        retName = copyIndex > 1 ? string.Format("{0}.{1}", excelTableName, copyIndex) : excelTableName;
        copyIndex++;
      } while (activeWorkbook.Worksheets.Cast<ExcelInterop.Worksheet>().Any(ws => ws.ListObjects.Cast<ExcelInterop.ListObject>().Any(excelTable => excelTable.Name == retName)));

      return retName;
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="ExcelInterop.PivotTable"/> that avoids duplicates with existing ones in the current <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="pivotTableName">The proposed name for a <see cref="ExcelInterop.PivotTable"/>.</param>
    /// <param name="copyIndex">Consecutive number for a <see cref="ExcelInterop.PivotTable"/> if duplicates are found.</param>
    /// <returns>A <see cref="ExcelInterop.PivotTable"/> valid name.</returns>
    private static string GetPivotTableNameAvoidingDuplicates(this string pivotTableName, int copyIndex)
    {
      var activeWorkbook = Globals.ThisAddIn.ActiveWorkbook;
      if (activeWorkbook == null)
      {
        return pivotTableName;
      }

      string retName;
      bool foundSameName;
      do
      {
        foundSameName = true;
        retName = copyIndex > 1 ? string.Format("{0}.{1}", pivotTableName, copyIndex) : pivotTableName;
        copyIndex++;
        foreach (var worksheetPivotTables in activeWorkbook.Worksheets.Cast<ExcelInterop.Worksheet>().Select(worksheet => worksheet.GetPivotTables()).Where(worksheetPivotTables => worksheetPivotTables != null))
        {
          foundSameName = worksheetPivotTables.Any(pt => pt.Name == retName);
          if (foundSameName)
          {
            break;
          }
        }
      } while (foundSameName);

      return retName;
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="ExcelInterop.WorkbookConnection"/> that avoids duplicates with existing ones in the current <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="workbookConnectionName">The proposed name for a <see cref="ExcelInterop.WorkbookConnection"/>.</param>
    /// <param name="copyIndex">Consecutive number for a <see cref="ExcelInterop.WorkbookConnection"/> if duplicates are found.</param>
    /// <returns>A <see cref="ExcelInterop.WorkbookConnection"/> valid name.</returns>
    private static string GetWorkbookConnectionNameAvoidingDuplicates(this string workbookConnectionName, int copyIndex)
    {
      var activeWorkbook = Globals.ThisAddIn.ActiveWorkbook;
      if (activeWorkbook == null)
      {
        return workbookConnectionName;
      }

      string retName;
      do
      {
        retName = copyIndex > 1 ? string.Format("{0}.{1}", workbookConnectionName, copyIndex) : workbookConnectionName;
        copyIndex++;
      } while (activeWorkbook.Connections.Cast<ExcelInterop.WorkbookConnection>().Any(wBconn => wBconn.Name == retName));

      return retName;
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="ExcelInterop.Worksheet"/> that avoids duplicates with existing ones in the given <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/>.</param>
    /// <param name="worksheetName">The proposed name for a <see cref="ExcelInterop.Worksheet"/>.</param>
    /// <param name="copyIndex">Number of the copy of a <see cref="ExcelInterop.Worksheet"/> within its name.</param>
    /// <returns>A <see cref="ExcelInterop.Worksheet"/> valid name.</returns>
    private static string GetWorksheetNameAvoidingDuplicates(this ExcelInterop.Workbook workbook, string worksheetName, int copyIndex)
    {
      if (workbook == null)
      {
        return worksheetName;
      }

      string retName;
      do
      {
        retName = copyIndex > 0 ? string.Format("Copy {0} of {1}", copyIndex, worksheetName) : worksheetName;
        copyIndex++;
      } while (workbook.Worksheets.Cast<ExcelInterop.Worksheet>().Any(ws => ws.Name == retName));

      return retName;
    }
  }
}