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
using System.IO;
using System.Linq;
using Microsoft.Office.Core;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using ExcelInterop = Microsoft.Office.Interop.Excel;
using MySQL.ForExcel.Interfaces;

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
    /// The connection string used on the creation of new <see cref="ExcelInterop.ListObject"/> instances holding imported MySQL data.
    /// </summary>
    public const string DUMMY_WORKBOOK_CONNECTION_STRING = @"OLEDB;Provider=MySqlDummy;Data Source=MySqlDummy;";

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
    /// Adds names to the whole application related to localized date format strings.
    /// </summary>
    /// <param name="workbook">The workbook where the new <see cref="ExcelInterop.Style"/> is added to.</param>
    /// <remarks>This method relies on the value of the setting HideLocalizedDateFormatNames.</remarks>
    public static void AddLocalizedDateFormatStringsAsNames(this ExcelInterop.Workbook workbook)
    {
      bool hideNames = Settings.Default.HideLocalizedDateFormatNames;
      workbook.AddNameWithInternationalFormula("LOCAL_DATE_SEPARATOR", "=INDEX(GET.WORKSPACE(37),17)", hideNames);
      workbook.AddNameWithInternationalFormula("LOCAL_TIME_SEPARATOR", "=INDEX(GET.WORKSPACE(37),18)", hideNames);
      workbook.AddNameWithInternationalFormula("LOCAL_YEAR_FORMAT", "=INDEX(GET.WORKSPACE(37),19)", hideNames);
      workbook.AddNameWithInternationalFormula("LOCAL_MONTH_FORMAT", "=INDEX(GET.WORKSPACE(37),20)", hideNames);
      workbook.AddNameWithInternationalFormula("LOCAL_DAY_FORMAT", "=INDEX(GET.WORKSPACE(37),21)", hideNames);
      workbook.AddNameWithInternationalFormula("LOCAL_HOUR_FORMAT", "=INDEX(GET.WORKSPACE(37),22)", hideNames);
      workbook.AddNameWithInternationalFormula("LOCAL_MINUTE_FORMAT", "=INDEX(GET.WORKSPACE(37),23)", hideNames);
      workbook.AddNameWithInternationalFormula("LOCAL_SECOND_FORMAT", "=INDEX(GET.WORKSPACE(37),24)", hideNames);
      workbook.AddNameWithInternationalFormula("LOCAL_MYSQL_DATE_FORMAT", "=REPT(LOCAL_YEAR_FORMAT,4)&LOCAL_DATE_SEPARATOR&REPT(LOCAL_MONTH_FORMAT,2)&LOCAL_DATE_SEPARATOR&REPT(LOCAL_DAY_FORMAT,2)&\" \"&REPT(LOCAL_HOUR_FORMAT,2)&LOCAL_TIME_SEPARATOR&REPT(LOCAL_MINUTE_FORMAT,2)&LOCAL_TIME_SEPARATOR&REPT(LOCAL_SECOND_FORMAT,2)", hideNames);
    }

    /// <summary>
    /// Adds a <see cref="ExcelInterop.Name"/> object to the collection of names (if it does not exist already) of the given <see cref="ExcelInterop.Workbook"/> with a formula in English that is translated to the current locale.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> object.</param>
    /// <param name="name">The name of the <see cref="ExcelInterop.Name"/> object.</param>
    /// <param name="internationalFormula">The tied formula expressed in English.</param>
    /// <param name="hidden">Flag indicating whether the name is hidden from the user.</param>
    public static void AddNameWithInternationalFormula(this ExcelInterop.Workbook workbook, string name, string internationalFormula, bool hidden)
    {
      if (workbook == null || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(internationalFormula))
      {
        return;
      }

      if (workbook.Names.Cast<ExcelInterop.Name>().Any(n => n.Name == name))
      {
        return;
      }

      var excelName = workbook.Names.Add(name, " ", !hidden);
      // The property is not set in the Add method above but below with the US locale.
      SetPropertyInternational(excelName, "RefersTo", internationalFormula);
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

      range = range.Resize[range.Rows.Count + 1, range.Columns.Count];
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
    /// Checks if the given <see cref="ExcelInterop.Range"/> contains data in any of its cells.
    /// </summary>
    /// <param name="range">An excel range.</param>
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
    /// Creates an Excel table from a given <see cref="ExcelInterop.Range"/> object.
    /// </summary>
    /// <param name="range">A <see cref="ExcelInterop.Range"/> object.</param>
    /// <param name="excelTableName">The proposed name for the new Excel table.</param>
    /// <param name="containsColumnNames">Flag indicating whether column names appear in the first row of the Excel range.</param>
    public static void CreateExcelTable(this ExcelInterop.Range range, string excelTableName, bool containsColumnNames)
    {
      if (range == null)
      {
        return;
      }

      var hasHeaders = containsColumnNames ? ExcelInterop.XlYesNoGuess.xlYes : ExcelInterop.XlYesNoGuess.xlNo;
      var namedTable = range.Worksheet.ListObjects.Add(ExcelInterop.XlListObjectSourceType.xlSrcRange, range, Type.Missing, hasHeaders, Type.Missing);
      namedTable.Name = excelTableName.GetExcelTableNameAvoidingDuplicates();
      namedTable.DisplayName = namedTable.Name;
      namedTable.TableStyle = Settings.Default.ImportExcelTableStyleName;
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
    /// Returns an Excel range with the first row cells corresponding to the column names.
    /// </summary>
    /// <param name="mysqlDataRange">If <c>null</c> the whole first row is returned, otherwise only the column cells within the editing range.</param>
    /// <returns>The Excel range with the first row cells corresponding to the column names</returns>
    public static ExcelInterop.Range GetColumnNamesRange(this ExcelInterop.Range mysqlDataRange)
    {
      return mysqlDataRange == null ? null : mysqlDataRange.Resize[1, mysqlDataRange.Columns.Count];
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="ExcelInterop.ListObject"/> that avoids duplicates with existing ones in the current <see cref="ExcelInterop.Worksheet"/>.
    /// </summary>
    /// <param name="excelTableName">The proposed name for a <see cref="ExcelInterop.ListObject"/>.</param>
    /// <returns>A <see cref="ExcelInterop.ListObject"/> valid name.</returns>
    public static string GetExcelTableNameAvoidingDuplicates(this string excelTableName)
    {
      return excelTableName.GetExcelTableNameAvoidingDuplicates(1);
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
    /// <param name="formattedValues">Falg indicating whether the data is formatted (numbers, dates, text) or not (numbers and text).</param>
    /// <returns>A linear array with the values of the cells of a single row within an <see cref="ExcelInterop.Range"/>.</returns>
    public static object[] GetRowValuesAsLinearArray(this ExcelInterop.Range range, int rowIndex, bool formattedValues = true)
    {
      if (range == null || rowIndex < 1 || rowIndex > range.Rows.Count)
      {
        return null;
      }

      ExcelInterop.Range rowRange = range.Rows[rowIndex];
      var rangeValues = formattedValues ? rowRange.Value : rowRange.Value2;
      var valuesBidimensionalArray = rowRange.Columns.Count > 1
        ? rangeValues as object[,]
        : new object[,] { { rangeValues } };
      return valuesBidimensionalArray.GetLinearArray(1, true).ToArray();
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
    /// Checks if a given <see cref="ExcelInterop.Range"/> intersects with any Excel table in its containing <see cref="ExcelInterop.Worksheet"/>. 
    /// </summary>
    /// <param name="range">A <see cref="ExcelInterop.Range"/> object.</param>
    /// <returns><c>true</c> if the given <see cref="ExcelInterop.Range"/> intersects with any Excel table in its containing <see cref="ExcelInterop.Worksheet"/>, <c>false</c> otherwise.</returns>
    public static bool IntersectsWithAnyExcelObject(this ExcelInterop.Range range)
    {
      bool intersects = (from ExcelInterop.ListObject excelTable in range.Worksheet.ListObjects select excelTable.Range.IntersectWith(range)).Any(intersectingRange => intersectingRange != null && intersectingRange.CountLarge != 0);
      if (intersects)
      {
        return true;
      }

      foreach (var pivotTable in range.Worksheet.GetPivotTables())
      {
        var intersectingRange = pivotTable.TableRange1.IntersectWith(range);
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

        intersects = true;
        break;
      }

      return intersects;
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
        extendedRange = extendedRange.Resize[mysqlDataRange.Rows.Count - 1, worksheet.Columns.Count];
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

      rangesList.Clear();
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
      var activeWorkbook = Globals.ThisAddIn.Application.ActiveWorkbook;
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
    /// Gets a property from the given target object returned in an English locale after transformed from the native Excel locale.
    /// </summary>
    /// <param name="target">The Excel object from which a property value is to be extracted.</param>
    /// <param name="name">The name of the property.</param>
    /// <returns>The value of the property returned in an English locale.</returns>
    static object GetPropertyInternational(object target, string name)
    {
      return target.GetType().InvokeMember(
        name,
        System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance,
        null,
        target,
        null,
        new System.Globalization.CultureInfo(EN_US_LOCALE_CODE));
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

    /// <summary>
    /// Invokes a method present in the given target object receiving parameters in an English locale that are transformed to the native Excel locale.
    /// </summary>
    /// <param name="target">The Excel object containing the method.</param>
    /// <param name="name">The name of the method to be invoked.</param>
    /// <param name="args">The arguments passed to the method parameters.</param>
    /// <returns>Any return value from the invoked method.</returns>
    static object InvokeMethodInternational(object target, string name, params object[] args)
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
    /// Sets a property value for the given target object given in an English locale to be transformed to the native Excel locale.
    /// </summary>
    /// <param name="target">The Excel object for which a property value is to be set.</param>
    /// <param name="name">The name of the property.</param>
    /// <param name="args">The property value in the English locale.</param>
    static void SetPropertyInternational(object target, string name, params object[] args)
    {
      target.GetType().InvokeMember(
        name,
        System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance,
        null,
        target,
        args,
        new System.Globalization.CultureInfo(EN_US_LOCALE_CODE));
    }
  }
}