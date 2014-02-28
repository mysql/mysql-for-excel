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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.Office.Core;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using Excel = Microsoft.Office.Interop.Excel;

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
    /// <remarks>Blue-ish.</remarks>
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
    /// <remarks>Green-ish.</remarks>
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
    /// Gets the interior color for Excel cells committed to the MySQL server during an Edit Data operation.
    /// </summary>
    public static int CommitedCellsOleColor { get; private set; }

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
    /// Gets the interior color for Excel cells that caused errors during a commit of an Edit Data operation.
    /// </summary>
    public static int ErroredCellsOleColor { get; private set; }

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
    /// Gets the default interior color for Excel cells locked during an Edit Data operation (like the headers containing column names).
    /// </summary>
    public static int LockedCellsOleColor { get; private set; }

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
    /// Gets the interior color for Excel cells accepting data from users to create a new row in the table during an Edit Data operation.
    /// </summary>
    public static int NewRowCellsOleColor { get; private set; }

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
    /// Gets the interior color for Excel cells containing values that have been changed by the user but not yet committed during an Edit Data operation.
    /// </summary>
    public static int UncommittedCellsOleColor { get; private set; }

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
    /// Gets the interior color for Excel cells containing values that caused concurrency warnings during an Edit Data operation using optimistic updates.
    /// </summary>
    public static int WarningCellsOleColor { get; private set; }

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

    #endregion Properties

    /// <summary>
    /// Adds names to the whole application related to localized date format strings.
    /// </summary>
    /// <param name="workbook">The workbook where the new <see cref="Excel.Style"/> is added to.</param>
    public static void AddLocalizedDateFormatStringsAsNames(this Excel.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      IList<Excel.Name> namesCollection = workbook.Names.Cast<Excel.Name>().ToList();
      if (namesCollection.All(name => name.Name != "LOCAL_DATE_SEPARATOR"))
      {
        workbook.Names.Add("LOCAL_DATE_SEPARATOR", "=INDEX(GET.WORKSPACE(37),17)");
      }

      if (namesCollection.All(name => name.Name != "LOCAL_TIME_SEPARATOR"))
      {
        workbook.Names.Add("LOCAL_TIME_SEPARATOR", "=INDEX(GET.WORKSPACE(37),18)");
      }

      if (namesCollection.All(name => name.Name != "LOCAL_YEAR_FORMAT"))
      {
        workbook.Names.Add("LOCAL_YEAR_FORMAT", "=INDEX(GET.WORKSPACE(37),19)");
      }

      if (namesCollection.All(name => name.Name != "LOCAL_MONTH_FORMAT"))
      {
        workbook.Names.Add("LOCAL_MONTH_FORMAT", "=INDEX(GET.WORKSPACE(37),20)");
      }

      if (namesCollection.All(name => name.Name != "LOCAL_DAY_FORMAT"))
      {
        workbook.Names.Add("LOCAL_DAY_FORMAT", "=INDEX(GET.WORKSPACE(37),21)");
      }

      if (namesCollection.All(name => name.Name != "LOCAL_HOUR_FORMAT"))
      {
        workbook.Names.Add("LOCAL_HOUR_FORMAT", "=INDEX(GET.WORKSPACE(37),22)");
      }

      if (namesCollection.All(name => name.Name != "LOCAL_MINUTE_FORMAT"))
      {
        workbook.Names.Add("LOCAL_MINUTE_FORMAT", "=INDEX(GET.WORKSPACE(37),23)");
      }

      if (namesCollection.All(name => name.Name != "LOCAL_MINUTE_FORMAT"))
      {
        workbook.Names.Add("LOCAL_SECOND_FORMAT", "=INDEX(GET.WORKSPACE(37),24)");
      }

      if (namesCollection.All(name => name.Name != "LOCAL_MYSQL_DATE_FORMAT"))
      {
        workbook.Names.Add("LOCAL_MYSQL_DATE_FORMAT", "=REPT(LOCAL_YEAR_FORMAT,4)&LOCAL_DATE_SEPARATOR&REPT(LOCAL_MONTH_FORMAT,2)&LOCAL_DATE_SEPARATOR&REPT(LOCAL_DAY_FORMAT,2)&\" \"&REPT(LOCAL_HOUR_FORMAT,2)&LOCAL_TIME_SEPARATOR&REPT(LOCAL_MINUTE_FORMAT,2)&LOCAL_TIME_SEPARATOR&REPT(LOCAL_SECOND_FORMAT,2)");
      }
    }

    /// <summary>
    /// Adds a new row at the bottom of the given Excel range.
    /// </summary>
    /// <param name="range">The Excel range to add a new row to the end of it.</param>
    /// <param name="clearLastRowColoring">Flag indicating whether the previous row that was placeholder for new rows is cleared of its formatting.</param>
    /// <param name="newRowRange">An Excel range containing just the newly added row if <see cref="clearLastRowColoring"/> is <c>true</c>, or containing the row above the newly added one otherwise.</param>
    /// <returns>The original Excel range with the newly added row at the end of it.</returns>
    public static Excel.Range AddNewRow(this Excel.Range range, bool clearLastRowColoring, out Excel.Range newRowRange)
    {
      newRowRange = null;
      if (range == null)
      {
        return null;
      }

      range = range.Resize[range.Rows.Count + 1, range.Columns.Count];
      newRowRange = range.Rows[range.Rows.Count] as Excel.Range;
      if (newRowRange != null)
      {
        newRowRange.Interior.Color = NewRowCellsOleColor;
      }

      if (!clearLastRowColoring || range.Rows.Count <= 0)
      {
        return range;
      }

      newRowRange = range.Rows[range.Rows.Count - 1] as Excel.Range;
      if (newRowRange != null)
      {
        newRowRange.Interior.ColorIndex = Excel.XlColorIndex.xlColorIndexNone;
      }

      return range;
    }

    /// <summary>
    /// Checks if the given <see cref="Excel.Range"/> contains data in any of its cells.
    /// </summary>
    /// <param name="range">An excel range.</param>
    /// <returns><c>true</c> if the given range is not empty, <c>false</c> otherwise.</returns>
    public static bool ContainsAnyData(this Excel.Range range)
    {
      if (range == null || range.CountLarge < 1)
      {
        return false;
      }

      return Globals.ThisAddIn.Application.WorksheetFunction.CountA(range).CompareTo(0) != 0;
    }

    /// <summary>
    /// Creates an Excel table from a given <see cref="Excel.Range"/> object.
    /// </summary>
    /// <param name="range">A <see cref="Excel.Range"/> object.</param>
    /// <param name="excelTableName">The proposed name for the new Excel table.</param>
    /// <param name="containsColumnNames">Flag indicating whether column names appear in the first row of the Excel range.</param>
    public static void CreateExcelTable(this Excel.Range range, string excelTableName, bool containsColumnNames)
    {
      if (range == null)
      {
        return;
      }

      Excel.XlYesNoGuess hasHeaders = containsColumnNames ? Excel.XlYesNoGuess.xlYes : Excel.XlYesNoGuess.xlNo;
      var namedTable = range.Worksheet.ListObjects.Add(Excel.XlListObjectSourceType.xlSrcRange, range, hasHeaders);
      namedTable.Name = excelTableName.GetExcelTableNameAvoidingDuplicates();
      namedTable.DisplayName = namedTable.Name;
      namedTable.TableStyle = Settings.Default.ImportExcelTableStyleName;
    }

    /// <summary>
    /// Creates a default <see cref="Excel.TableStyle"/> for MySQL imported data.
    /// </summary>
    /// <param name="workbook">The workbook where the new <see cref="Excel.Style"/> is added to.</param>
    /// <returns>A new <see cref="Excel.TableStyle"/> for MySQL imported data.</returns>
    public static Excel.TableStyle CreateMySqlTableStyle(this Excel.Workbook workbook)
    {
      if (workbook == null || workbook.TableStyles.Cast<Excel.TableStyle>().Any(style => style.Name == DEFAULT_MYSQL_STYLE_NAME))
      {
        return null;
      }

      Excel.TableStyle mySqlTableStyle = workbook.TableStyles.Add(DEFAULT_MYSQL_STYLE_NAME);
      mySqlTableStyle.ShowAsAvailableTableStyle = false;
      mySqlTableStyle.TableStyleElements[Excel.XlTableStyleElementType.xlWholeTable].SetAsMySqlStyle();
      mySqlTableStyle.TableStyleElements[Excel.XlTableStyleElementType.xlHeaderRow].SetAsMySqlStyle(LockedCellsOleColor, true);
      return mySqlTableStyle;
    }

    /// <summary>
    /// Gets a <see cref="Excel.Worksheet"/> with a given name existing in the given <see cref="Excel.Workbook"/> or creates a new one.
    /// </summary>
    /// <param name="workBook">The <see cref="Excel.Workbook"/> to look for a <see cref="Excel.Worksheet"/>.</param>
    /// <param name="workSheetName">The name of the new <see cref="Excel.Worksheet"/>.</param>
    /// <param name="selectTopLeftCell">Flag indicating whether the cell A1 receives focus.</param>
    /// <returns>The existing or new <see cref="Excel.Worksheet"/> object.</returns>
    public static Excel.Worksheet CreateWorksheet(this Excel.Workbook workBook, string workSheetName, bool selectTopLeftCell)
    {
      if (workBook == null)
      {
        return null;
      }

      Excel.Worksheet newWorksheet = null;
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
    /// Returns an Excel range with the first row cells corresponding to the column names.
    /// </summary>
    /// <param name="mysqlDataRange">If <c>null</c> the whole first row is returned, otherwise only the column cells within the editing range.</param>
    /// <returns>The Excel range with the first row cells corresponding to the column names</returns>
    public static Excel.Range GetColumnNamesRange(this Excel.Range mysqlDataRange)
    {
      return mysqlDataRange == null ? null : mysqlDataRange.Resize[1, mysqlDataRange.Columns.Count];
    }

    /// <summary>
    /// Gets a <see cref="Excel.Worksheet"/> with a given name existing in the given <see cref="Excel.Workbook"/> or creates a new one.
    /// </summary>
    /// <param name="workbook">The <see cref="Excel.Workbook"/> to look for a <see cref="Excel.Worksheet"/>.</param>
    /// <param name="workSheetName">The name of the new <see cref="Excel.Worksheet"/>.</param>
    /// <param name="selectTopLeftCell">Flag indicating whether the cell A1 receives focus.</param>
    /// <returns>The existing or new <see cref="Excel.Worksheet"/> object.</returns>
    public static Excel.Worksheet GetOrCreateWorksheet(this Excel.Workbook workbook, string workSheetName, bool selectTopLeftCell)
    {
      if (workbook == null)
      {
        return null;
      }

      Excel.Worksheet existingWorksheet = workbook.Worksheets.Cast<Excel.Worksheet>().FirstOrDefault(worksheet => string.Equals(worksheet.Name, workSheetName, StringComparison.InvariantCulture));
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
    /// Gets the name of the parent <see cref="Excel.Workbook"/> of the given <see cref="Excel.Worksheet"/>.
    /// </summary>
    /// <param name="worksheet">A <see cref="Excel.Worksheet"/> object.</param>
    /// <returns>The name of the parent <see cref="Excel.Workbook"/>.</returns>
    public static string GetParentWorkbookName(this Excel.Worksheet worksheet)
    {
      if (worksheet == null)
      {
        return string.Empty;
      }

      Excel.Workbook parentWorkbook = worksheet.Parent as Excel.Workbook;
      return parentWorkbook != null ? parentWorkbook.Name : string.Empty;
    }

    /// <summary>
    /// Gets a linear array with the values of the cells of a single row within an <see cref="Excel.Range"/>.
    /// </summary>
    /// <param name="range">A <see cref="Excel.Range"/> object.</param>
    /// <param name="rowIndex">The index of the row within the <see cref="Excel.Range"/> to get values from.</param>
    /// <param name="formattedValues">Falg indicating whether the data is formatted (numbers, dates, text) or not (numbers and text).</param>
    /// <returns>A linear array with the values of the cells of a single row within an <see cref="Excel.Range"/>.</returns>
    public static object[] GetRowValuesAsLinearArray(this Excel.Range range, int rowIndex, bool formattedValues = true)
    {
      if (range == null || rowIndex < 1 || rowIndex > range.Rows.Count)
      {
        return null;
      }

      Excel.Range rowRange = range.Rows[rowIndex];
      var rangeValues = formattedValues ? rowRange.Value : rowRange.Value2;
      var valuesBidimensionalArray = rowRange.Columns.Count > 1
        ? rangeValues as object[,]
        : new object[,] { { rangeValues } };
      return valuesBidimensionalArray.GetLinearArray(1, true).ToArray();
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="Excel.ListObject"/> that avoids duplicates with existing ones in the current <see cref="Excel.Worksheet"/>.
    /// </summary>
    /// <param name="excelTableName">The proposed name for a <see cref="Excel.ListObject"/>.</param>
    /// <returns>A <see cref="Excel.ListObject"/> valid name.</returns>
    public static string GetExcelTableNameAvoidingDuplicates(this string excelTableName)
    {
      return excelTableName.GetExcelTableNameAvoidingDuplicates(1);
    }

    /// <summary>
    /// Gets the active workbook unique identifier if exists, if not, creates one and returns it.
    /// </summary>
    /// <param name="workbook">A <see cref="Excel.Workbook"/> object.</param>
    /// <returns>The guid string for the current workbook.</returns>
    public static string GetOrCreateId(this Excel.Workbook workbook)
    {
      if (workbook == null || workbook.CustomDocumentProperties == null)
      {
        return null;
      }

      DocumentProperty guid = ((DocumentProperties)workbook.CustomDocumentProperties).Cast<DocumentProperty>().FirstOrDefault(property => property.Name.Equals("WorkbookGuid"));
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
    /// Gets the a protection key for the provided worksheet if exists.
    /// </summary>
    /// <param name="worksheet">A <see cref="Excel.Worksheet"/> object.</param>
    /// <returns>The worksheet's protection key if the property exist, otherwise returns null.</returns>
    public static string GetProtectionKey(this Excel.Worksheet worksheet)
    {
      if (worksheet == null)
      {
        return null;
      }

      Excel.CustomProperties properties = worksheet.CustomProperties;
      if (properties == null)
      {
        return null;
      }

      Excel.CustomProperty guid = properties.Cast<Excel.CustomProperty>().FirstOrDefault(property => property.Name.Equals("WorksheetGuid"));
      return guid == null ? null : guid.Value.ToString();
    }

    /// <summary>
    /// Gets an <see cref="Excel.Range"/> object that represents all non-empty cells.
    /// </summary>
    /// <param name="range">A <see cref="Excel.Range"/> object.</param>
    /// <returns>An <see cref="Excel.Range"/> object that represents all non-empty cells.</returns>
    public static Excel.Range GetNonEmptyRange(this Excel.Range range)
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

      Excel.Range rangeWithFormulas = null;
      Excel.Range rangeWithConstants = null;
      Excel.Range finalRange = null;

      // SpecialCells method throws exception if no cells are found matching criteria (possible bug in VSTO).
      try
      {
        rangeWithFormulas = range.SpecialCells(Excel.XlCellType.xlCellTypeFormulas);
      }
      catch
      {
      }

      // SpecialCells method throws exception if no cells are found matching criteria (possible bug in VSTO).
      try
      {
        rangeWithConstants = range.SpecialCells(Excel.XlCellType.xlCellTypeConstants, (int)Excel.XlSpecialCellsValue.xlTextValues + (int)Excel.XlSpecialCellsValue.xlNumbers);
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
    /// Gets an <see cref="Excel.Range"/> object representing an unique rectangular area where cells inside it contain values.
    /// There may be empty cells inside, the rectangular area is calculated by finding a topmost-leftmost cell with data and 
    /// a bottommost-rightmost cell with data to then compose the corners of the rectangular area.
    /// </summary>
    /// <param name="range">A <see cref="Excel.Range"/> object.</param>
    /// <returns>an <see cref="Excel.Range"/> object representing an unique rectangular area where cells inside it contain values.</returns>
    public static Excel.Range GetNonEmptyRectangularAreaRange(this Excel.Range range)
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

      Excel.Range firstOriginalCell = range.Cells[1, 1];
      Excel.Range lastRowCell = range.Cells.Find(
        "*",
        firstOriginalCell,
        Excel.XlFindLookIn.xlValues,
        Type.Missing,
        Excel.XlSearchOrder.xlByRows,
        Excel.XlSearchDirection.xlPrevious,
        Type.Missing,
        Type.Missing,
        Type.Missing);
      if (lastRowCell == null)
      {
        return null;
      }

      int lastCellRow = lastRowCell.Row;
      Excel.Range lastColumnCell = range.Cells.Find(
        "*",
        firstOriginalCell,
        Excel.XlFindLookIn.xlValues,
        Type.Missing,
        Excel.XlSearchOrder.xlByColumns,
        Excel.XlSearchDirection.xlPrevious,
        Type.Missing,
        Type.Missing,
        Type.Missing);
      if (lastColumnCell == null)
      {
        return null;
      }

      int lastCellColumn = lastColumnCell.Column;
      Excel.Range lastCell = range.Worksheet.Cells[lastCellRow, lastCellColumn];
      Excel.Range firstRowCell = range.Cells.Find(
        "*",
        lastCell,
        Excel.XlFindLookIn.xlValues,
        Type.Missing,
        Excel.XlSearchOrder.xlByRows,
        Excel.XlSearchDirection.xlNext,
        Type.Missing,
        Type.Missing,
        Type.Missing);
      if (firstRowCell == null)
      {
        return null;
      }

      int firstCellRow = firstRowCell.Row;
      Excel.Range firstColumnCell = range.Cells.Find(
        "*",
        lastCell,
        Excel.XlFindLookIn.xlValues,
        Type.Missing,
        Excel.XlSearchOrder.xlByColumns,
        Excel.XlSearchDirection.xlNext,
        Type.Missing,
        Type.Missing,
        Type.Missing);
      if (firstColumnCell == null)
      {
        return null;
      }

      int firstCellColumn = firstColumnCell.Column;
      Excel.Range firstCell = range.Worksheet.Cells[firstCellRow, firstCellColumn];
      return range.Worksheet.Range[firstCell, lastCell];
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="Excel.Worksheet"/> that avoids duplicates with existing ones in the given <see cref="Excel.Workbook"/>.
    /// </summary>
    /// <param name="workbook">A <see cref="Excel.Workbook"/>.</param>
    /// <param name="worksheetName">The proposed name for a <see cref="Excel.Worksheet"/>.</param>
    /// <returns>A <see cref="Excel.Worksheet"/> valid name.</returns>
    public static string GetWorksheetNameAvoidingDuplicates(this Excel.Workbook workbook, string worksheetName)
    {
      return workbook.GetWorksheetNameAvoidingDuplicates(worksheetName, 0);
    }

    /// <summary>
    /// Returns a Range object that represents the rectangular intersection of the given range with another range.
    /// </summary>
    /// <param name="range">A <see cref="Excel.Range"/> object.</param>
    /// <param name="otherRange">An intersecting <see cref="Excel.Range"/> object.</param>
    /// <returns>A <see cref="Excel.Range"/> object representing the rectangular intersection of the given range with another range.</returns>
    public static Excel.Range IntersectWith(this Excel.Range range, Excel.Range otherRange)
    {
      return Globals.ThisAddIn.Application.Intersect(range, otherRange);
    }

    /// <summary>
    /// Checks if a given <see cref="Excel.Range"/> intersects with any Excel table in its containing <see cref="Excel.Worksheet"/>. 
    /// </summary>
    /// <param name="range">A <see cref="Excel.Range"/> object.</param>
    /// <returns><c>true</c> if the given <see cref="Excel.Range"/> intersects with any Excel table in its containing <see cref="Excel.Worksheet"/>, <c>false</c> otherwise.</returns>
    public static bool IntersectsWithAnyExcelTable(this Excel.Range range)
    {
      return range != null && (from Excel.ListObject excelTable in range.Worksheet.ListObjects select excelTable.Range.IntersectWith(range)).Any(intersectRange => intersectRange != null && intersectRange.CountLarge != 0);
    }

    /// <summary>
    /// Checks if the <see cref="Excel.Worksheet"/> is visible.
    /// </summary>
    /// <param name="worksheet">A <see cref="Excel.Worksheet"/> object.</param>
    /// <returns><c>true</c> if the <see cref="Excel.Worksheet"/> is visible, <c>false</c> otherwise.</returns>
    public static bool IsVisible(this Excel.Worksheet worksheet)
    {
      return worksheet != null && worksheet.Visible == Excel.XlSheetVisibility.xlSheetVisible;
    }

    /// <summary>
    /// Returns a list of <see cref="Excel.TableStyle"/> names available to be used within the given <see cref="Excel.Workbook"/>.
    /// </summary>
    /// <param name="workbook">A <see cref="Excel.Workbook"/> object.</param>
    /// <returns>A list of style names available in the given <see cref="Excel.Workbook"/>.</returns>
    public static List<string> ListTableStyles(this Excel.Workbook workbook)
    {
      return workbook == null ? null : (from Excel.TableStyle tableStyle in workbook.TableStyles orderby tableStyle.Name select tableStyle.Name).ToList();
    }

    /// <summary>
    /// Locks the given Excel range and sets its fill color accordingly.
    /// </summary>
    /// <param name="range">The <see cref="Excel.Range"/> to lock or unlock.</param>
    /// <param name="lockRange">Flag indicating whether the Excel range is locked or unlocked.</param>
    public static void LockRange(this Excel.Range range, bool lockRange)
    {
      if (lockRange)
      {
        range.Interior.Color = LockedCellsOleColor;
      }
      else
      {
        range.Interior.ColorIndex = Excel.XlColorIndex.xlColorIndexNone;
      }

      range.Locked = lockRange;
    }

    /// <summary>
    /// Unprotects the given Excel worksheet and stops listening for its Change event.
    /// </summary>
    /// <param name="worksheet">The <see cref="Excel.Worksheet"/> to unprotect.</param>
    /// <param name="changeEventHandlerDelegate">The change event handler delegate of the Excel worksheet.</param>
    /// <param name="protectionKey">The key used to unprotect the worksheet.</param>
    /// <param name="mysqlDataRange">The Excel range containing the MySQL data being edited.</param>
    public static void ProtectEditingWorksheet(this Excel.Worksheet worksheet, Excel.DocEvents_ChangeEventHandler changeEventHandlerDelegate, string protectionKey, Excel.Range mysqlDataRange)
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
        Excel.Range extendedRange = mysqlDataRange.Range["A2"];
        extendedRange = extendedRange.Resize[mysqlDataRange.Rows.Count - 1, worksheet.Columns.Count];
        extendedRange.Locked = false;

        // Column names range code
        Excel.Range headersRange = mysqlDataRange.GetColumnNamesRange();
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
    /// Removes the protectionKey property (if exists) for the current worksheet.
    /// </summary>
    /// <param name="worksheet">A <see cref="Excel.Worksheet"/> object.</param>
    public static void RemoveProtectionKey(this Excel.Worksheet worksheet)
    {
      if (worksheet == null)
      {
        return;
      }

      var protectionKeyProperty = worksheet.CustomProperties.Cast<Excel.CustomProperty>().FirstOrDefault(property => property.Name.Equals("WorksheetGuid"));
      if (protectionKeyProperty != null)
      {
        protectionKeyProperty.Delete();
      }
    }

    /// <summary>
    /// Places the A1 cell of the given <see cref="Excel.Worksheet"/> in focus.
    /// </summary>
    /// <param name="worksheet">A <see cref="Excel.Worksheet"/> object.</param>
    public static void SelectTopLeftCell(this Excel.Worksheet worksheet)
    {
      if (worksheet == null)
      {
        return;
      }

      Globals.ThisAddIn.Application.Goto(worksheet.Range["A1", Type.Missing], false);
    }

    /// <summary>
    /// Sets the font and color properties of a <see cref="Excel.TableStyleElement"/> as a MySQL minimalistic style.
    /// </summary>
    /// <param name="styleElement">The <see cref="Excel.TableStyleElement"/> to modify.</param>
    /// <param name="interiorOleColor">The OLE color to paint the Excel cells interior with.</param>
    /// <param name="makeBold">Flag indicating whether the font is set to bold.</param>
    public static void SetAsMySqlStyle(this Excel.TableStyleElement styleElement, int interiorOleColor = EMPTY_CELLS_OLE_COLOR, bool makeBold = false)
    {
      styleElement.Font.Color = ColorTranslator.ToOle(Color.Black);
      if (interiorOleColor == EMPTY_CELLS_OLE_COLOR)
      {
        styleElement.Interior.ColorIndex = Excel.XlColorIndex.xlColorIndexNone;
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
    /// <param name="range">A <see cref="Excel.Range"/> object.</param>
    public static void SetHeaderStyle(this Excel.Range range)
    {
      if (range == null)
      {
        return;
      }

      Excel.Range headerRange = range.GetColumnNamesRange();
      headerRange.SetInteriorColor(LockedCellsOleColor);
      headerRange.Font.Bold = true;
    }

    /// <summary>
    /// Sets the range cells interior color to the specified OLE color.
    /// </summary>
    /// <param name="range">Excel range to have their interior color changed.</param>
    /// <param name="oleColor">The new interior color for the Excel cells.</param>
    public static void SetInteriorColor(this Excel.Range range, int oleColor)
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
        range.Interior.ColorIndex = Excel.XlColorIndex.xlColorIndexNone;
      }
    }

    /// <summary>
    /// Sets the interior color of all the Excel ranges within the given list to the specified color.
    /// </summary>
    /// <param name="rangesList">The list of Excel ranges to have their fill color changed.</param>
    /// <param name="oleColor">The new fill color for the Excel cells.</param>
    public static void SetInteriorColor(this IList<Excel.Range> rangesList, int oleColor)
    {
      if (rangesList == null)
      {
        return;
      }

      foreach (var range in rangesList)
      {
        range.SetInteriorColor(oleColor);
      }

      rangesList.Clear();
    }

    /// <summary>
    /// Sets the protection key for the worksheet.
    /// </summary>
    /// <returns>The new protection key provided for the worksheet.</returns>
    public static bool StoreProtectionKey(this Excel.Worksheet worksheet, string protectionKey)
    {
      if (worksheet == null || string.IsNullOrEmpty(protectionKey))
      {
        return false;
      }

      var protectionKeyProperty = worksheet.CustomProperties.Cast<Excel.CustomProperty>().FirstOrDefault(property => property.Name.Equals("WorksheetGuid"));
      if (protectionKeyProperty == null)
      {
        Excel.CustomProperties properties = worksheet.CustomProperties;
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
    public static void UnprotectEditingWorksheet(this Excel.Worksheet worksheet, Excel.DocEvents_ChangeEventHandler changeEventHandlerDelegate, string protectionKey)
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
    /// Checks if an Excel <see cref="Excel.Worksheet"/> with a given name exists in the given <see cref="Excel.Workbook"/>.
    /// </summary>
    /// <param name="workbook">The <see cref="Excel.Workbook"/>.</param>
    /// <param name="worksheetName">Name of the <see cref="Excel.Worksheet"/>.</param>
    /// <returns><c>true</c> if the <see cref="Excel.Worksheet"/> exists, <c>false</c> otherwise.</returns>
    public static bool WorksheetExists(this Excel.Workbook workbook, string worksheetName)
    {
      if (workbook == null || worksheetName.Length <= 0)
      {
        return false;
      }

      return workbook.Worksheets.Cast<Excel.Worksheet>().Any(ws => string.Equals(ws.Name, worksheetName, StringComparison.InvariantCulture));
    }

    /// <summary>
    /// Checks if an Excel <see cref="Excel.Worksheet"/> with a given name exists in a <see cref="Excel.Workbook"/> with the given name.
    /// </summary>
    /// <param name="workbookName">Name of the <see cref="Excel.Workbook"/>.</param>
    /// <param name="worksheetName">Name of the <see cref="Excel.Worksheet"/>.</param>
    /// <returns><c>true</c> if the <see cref="Excel.Worksheet"/> exists, <c>false</c> otherwise.</returns>
    public static bool WorksheetExists(string workbookName, string worksheetName)
    {
      if (workbookName.Length <= 0)
      {
        return false;
      }

      var wBook = Globals.ThisAddIn.Application.Workbooks.Cast<Excel.Workbook>().FirstOrDefault(wb => string.Equals(wb.Name, workbookName, StringComparison.InvariantCulture));
      return wBook != null && wBook.WorksheetExists(worksheetName);
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="Excel.ListObject"/> that avoids duplicates with existing ones in the current <see cref="Excel.Worksheet"/>.
    /// </summary>
    /// <param name="excelTableName">The proposed name for a <see cref="Excel.ListObject"/>.</param>
    /// <param name="copyIndex">Number of the copy of a <see cref="Excel.Worksheet"/> within its name.</param>
    /// <returns>A <see cref="Excel.ListObject"/> valid name.</returns>
    private static string GetExcelTableNameAvoidingDuplicates(this string excelTableName, int copyIndex)
    {
      var activeWorkbook = Globals.ThisAddIn.Application.ActiveWorkbook;
      if (activeWorkbook == null)
      {
        return excelTableName;
      }

      string retName;
      do
      {
        retName = copyIndex > 1 ? string.Format("{0}.{1}", excelTableName, copyIndex) : excelTableName;
        copyIndex++;
      } while (activeWorkbook.Worksheets.Cast<Excel.Worksheet>().Any(ws => ws.ListObjects.Cast<Excel.ListObject>().Any(excelTable => excelTable.Name == retName)));

      return retName;
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="Excel.Worksheet"/> that avoids duplicates with existing ones in the given <see cref="Excel.Workbook"/>.
    /// </summary>
    /// <param name="workbook">A <see cref="Excel.Workbook"/>.</param>
    /// <param name="worksheetName">The proposed name for a <see cref="Excel.Worksheet"/>.</param>
    /// <param name="copyIndex">Number of the copy of a <see cref="Excel.Worksheet"/> within its name.</param>
    /// <returns>A <see cref="Excel.Worksheet"/> valid name.</returns>
    private static string GetWorksheetNameAvoidingDuplicates(this Excel.Workbook workbook, string worksheetName, int copyIndex)
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
      } while (workbook.Worksheets.Cast<Excel.Worksheet>().Any(ws => ws.Name == retName));

      return retName;
    }
  }
}