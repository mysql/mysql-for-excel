// Copyright (c) 2013, Oracle and/or its affiliates. All rights reserved.
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

namespace MySQL.ForExcel
{
  using System.Collections.Generic;
  using System.Data;
  using System.Drawing;
  using Excel = Microsoft.Office.Interop.Excel;

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
    public const string DEFAULT_COMMITED_CELLS_HTML_COLOR = "#B8E5F7";

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
    /// The default interior color for Excel cells accepting data from users to create a new row in the table during an Edit Data operation.
    /// </summary>
    /// <remarks>Yellow-ish.</remarks>
    public const string DEFAULT_NEW_ROW_CELLS_HTML_COLOR = "#FFFCC7";

    /// <summary>
    /// The default interior color for Excel cells containing values that have been changed by the user but not yet committed during an Edit Data operation.
    /// </summary>
    /// <remarks>Green-ish.</remarks>
    public const string DEFAULT_UNCOMMITTED_CELLS_HTML_COLOR = "#7CC576";

    #endregion Constants

    static ExcelUtilities()
    {
      CommitedCellsHtmlColor = DEFAULT_COMMITED_CELLS_HTML_COLOR;
      ErroredCellsHtmlColor = DEFAULT_ERRORED_CELLS_HTML_COLOR;
      LockedCellsHtmlColor = DEFAULT_LOCKED_CELLS_HTML_COLOR;
      NewRowCellsHtmlColor = DEFAULT_NEW_ROW_CELLS_HTML_COLOR;
      UncommittedCellsHtmlColor = DEFAULT_UNCOMMITTED_CELLS_HTML_COLOR;
    }

    #region Properties

    /// <summary>
    /// Gets the interior color for Excel cells committed to the MySQL server during an Edit Data operation.
    /// </summary>
    public static int CommitedCellsOLEColor { get; private set; }

    /// <summary>
    /// Gets or sets the interior color for Excel cells committed to the MySQL server during an Edit Data operation.
    /// </summary>
    public static string CommitedCellsHtmlColor
    {
      get
      {
        return ColorTranslator.ToHtml(ColorTranslator.FromOle(CommitedCellsOLEColor));
      }

      set
      {
        CommitedCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml(value));
      }
    }

    /// <summary>
    /// Gets the interior color for Excel cells that caused errors during a commit of an Edit Data operation.
    /// </summary>
    public static int ErroredCellsOLEColor { get; private set; }

    /// <summary>
    /// Gets or sets the interior color for Excel cells that caused errors during a commit of an Edit Data operation.
    /// </summary>
    public static string ErroredCellsHtmlColor
    {
      get
      {
        return ColorTranslator.ToHtml(ColorTranslator.FromOle(ErroredCellsOLEColor));
      }

      set
      {
        ErroredCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml(value));
      }
    }

    /// <summary>
    /// Gets the default interior color for Excel cells locked during an Edit Data operation (like the headers containing column names).
    /// </summary>
    public static int LockedCellsOLEColor { get; private set; }

    /// <summary>
    /// Gets or sets the interior color for Excel cells locked during an Edit Data operation (like the headers containing column names).
    /// </summary>
    public static string LockedCellsHtmlColor
    {
      get
      {
        return ColorTranslator.ToHtml(ColorTranslator.FromOle(LockedCellsOLEColor));
      }

      set
      {
        LockedCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml(value));
      }
    }

    /// <summary>
    /// Gets the interior color for Excel cells accepting data from users to create a new row in the table during an Edit Data operation.
    /// </summary>
    public static int NewRowCellsOLEColor { get; private set; }

    /// <summary>
    /// Gets or sets the interior color for Excel cells accepting data from users to create a new row in the table during an Edit Data operation.
    /// </summary>
    public static string NewRowCellsHtmlColor
    {
      get
      {
        return ColorTranslator.ToHtml(ColorTranslator.FromOle(NewRowCellsOLEColor));
      }

      set
      {
        NewRowCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml(value));
      }
    }

    /// <summary>
    /// Gets the interior color for Excel cells containing values that have been changed by the user but not yet committed during an Edit Data operation.
    /// </summary>
    public static int UncommittedCellsOLEColor { get; private set; }

    /// <summary>
    /// Gets or sets the interior color for Excel cells containing values that have been changed by the user but not yet committed during an Edit Data operation.
    /// </summary>
    public static string UncommittedCellsHtmlColor
    {
      get
      {
        return ColorTranslator.ToHtml(ColorTranslator.FromOle(UncommittedCellsOLEColor));
      }

      set
      {
        UncommittedCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml(value));
      }
    }

    /// <summary>
    /// Gets the interior color used to revert Excel cells to their original background color.
    /// </summary>
    /// <remarks>White.</remarks>
    public static int EmptyCellsOLEColor
    {
      get
      {
        return ColorTranslator.ToOle(Color.White);
      }
    }

    #endregion Properties

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

      range = range.get_Resize(range.Rows.Count + 1, range.Columns.Count);
      newRowRange = range.Rows[range.Rows.Count] as Excel.Range;
      if (newRowRange != null)
      {
        newRowRange.Interior.Color = NewRowCellsOLEColor;
      }

      if (clearLastRowColoring && range.Rows.Count > 0)
      {
        newRowRange = range.Rows[range.Rows.Count - 1] as Excel.Range;
        if (newRowRange != null)
        {
          newRowRange.Interior.ColorIndex = Excel.XlColorIndex.xlColorIndexNone;
        }
      }

      return range;
    }

    /// <summary>
    /// Returns an Excel range with the first row cells corresponding to the column names.
    /// </summary>
    /// <param name="mysqlDataRange">If <c>null</c> the whole first row is returned, otherwise only the column cells within the editing range.</param>
    /// <returns>The Excel range with the first row cells corresponding to the column names</returns>
    public static Excel.Range GetColumnNamesRange(this Excel.Range mysqlDataRange)
    {
      if (mysqlDataRange == null)
      {
        return null;
      }

      return mysqlDataRange.get_Resize(1, mysqlDataRange.Columns.Count);
    }

    /// <summary>
    /// Returns a Range object that represents the rectangular intersection of the given range with another range.
    /// </summary>
    /// <param name="range">The given Excel range.</param>
    /// <param name="otherRange">The intersecting Excel range.</param>
    /// <returns>The rectangular intersection of the given range with another range.</returns>
    public static Excel.Range IntersectWith(this Excel.Range range, Excel.Range otherRange)
    {
      return Globals.ThisAddIn.Application.Intersect(range, otherRange);
    }

    /// <summary>
    /// Locks the given Excel range and sets its fill color accordingly.
    /// </summary>
    /// <param name="range">The Excel range to lock or unlock.</param>
    /// <param name="lockRange">Flag indicating whether the Excel range is locked or unlocked.</param>
    public static void LockRange(this Excel.Range range, bool lockRange)
    {
      if (lockRange)
      {
        range.Interior.Color = LockedCellsOLEColor;
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
    /// <param name="worksheet">The Excel worksheet to unprotect.</param>
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
        Excel.Range extendedRange = mysqlDataRange.get_Range("A2");
        extendedRange = extendedRange.get_Resize(mysqlDataRange.Rows.Count - 1, worksheet.Columns.Count);
        extendedRange.Locked = false;

        //// Column names range code
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
    /// Refreshes the Excel range addresses of recorded changes in case rows have been added or deleted.
    /// </summary>
    /// <param name="rangesAndAddressesList">The list of Excel ranges to have their data refreshed.</param>
    /// <returns>The number of Excel ranges with address changes.</returns>
    public static int RefreshAddressesOfStoredRanges(this IList<RangeAndAddress> rangesAndAddressesList)
    {
      int qtyUpdated = 0;

      if (rangesAndAddressesList != null && rangesAndAddressesList.Count > 0)
      {
        foreach (RangeAndAddress ra in rangesAndAddressesList)
        {
          if (ra.Modification != RangeAndAddress.RangeModification.Added && ra.Modification != RangeAndAddress.RangeModification.Updated)
          {
            continue;
          }

          try
          {
            if (ra.Address != ra.Range.Address)
            {
              ra.Address = ra.Range.Address;
              ra.ExcelRow = ra.Range.Row;
              qtyUpdated++;
            }
          }
          catch
          {
            ra.Range = ra.Range.Worksheet.get_Range(ra.Address);
            ra.ExcelRow = ra.Range.Row;
            qtyUpdated++;
          }
        }
      }

      return qtyUpdated;
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
    /// <param name="rangesAndAddressesList">The list of Excel ranges to have their fill color changed.</param>
    /// <param name="oleColor">The new fill color for the Excel cells.</param>
    public static void SetInteriorColor(this IList<RangeAndAddress> rangesAndAddressesList, int oleColor)
    {
      if (rangesAndAddressesList == null)
      {
        return;
      }

      foreach (RangeAndAddress ra in rangesAndAddressesList)
      {
        ra.Range.SetInteriorColor(oleColor);
      }

      rangesAndAddressesList.Clear();
    }

    /// <summary>
    /// Sets the interior color of all the Excel cells recorded in the editing session to the committed data color, if cells errored out they are set to the errored color.
    /// </summary>
    /// <param name="rangesAndAddressesList">The list of Excel ranges to have their fill color changed.</param>
    /// <param name="commitSuccessful">Flag indicating whether the commit of the Excel cells recorded in the editing session was successful.</param>
    public static void SetInteriorColorToCommmited(this IList<RangeAndAddress> rangesAndAddressesList, bool commitSuccessful)
    {
      if (rangesAndAddressesList == null)
      {
        return;
      }

      for (int idx = 0; idx < rangesAndAddressesList.Count; idx++)
      {
        RangeAndAddress ra = rangesAndAddressesList[idx];
        if (ra.TableRow.HasErrors)
        {
          ra.Range.SetInteriorColor(ErroredCellsOLEColor);
          continue;
        }

        if (!commitSuccessful)
        {
          continue;
        }

        if (ra.TableRow.RowState != DataRowState.Detached && ra.TableRow.RowState != DataRowState.Deleted)
        {
          ra.Range.SetInteriorColor(CommitedCellsOLEColor);
        }

        rangesAndAddressesList.Remove(ra);
        idx--;
      }
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

      worksheet.Unprotect(protectionKey);
    }
  }
}