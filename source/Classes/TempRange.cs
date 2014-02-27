// Copyright (c) 2014, Oracle and/or its affiliates. All rights reserved.
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
using MySQL.Utility.Classes;
using Excel = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a temporary Excel range stored in a <see cref="Excel.Worksheet"/> that will be deleted when the object is disposed of.
  /// </summary>
  public class TempRange : IDisposable
  {
    /// <summary>
    /// Flag indicating whether screen updating will be disabled to speed up processing.
    /// </summary>
    private readonly bool _disableScreenupdating;

    /// <summary>
    /// Initializes a new instance of the <see cref="TempRange"/> class copying its data from the source range.
    /// </summary>
    /// <param name="sourceRange">The original source <see cref="Excel.Range"/> whose data is copied to the temporary one.</param>
    /// <param name="cropToNonEmptyRange">Flag indicating whether the range is cropped to a subrange with only non-empty cells.</param>
    /// <param name="skipEmptyColumns">Flag indicating whether empty columns are not copied to the target range.</param>
    /// <param name="hideWorksheet">Flag indicating whether the new temporary <see cref="Excel.Worksheet"/> will be hidden.</param>
    /// <param name="dateColumnIndexes">Array of indexes of columns that will populate a date MySQL column.</param>
    /// <param name="limitRowsQuantity">Gets a limit on the number of rows copied from the source range to the temporary range. If less than 1 it means there is no limit.</param>
    /// <param name="disableScreenUpdating">Flag indicating whether screen updating will be disabled to speed up processing.</param>
    public TempRange(Excel.Range sourceRange, bool cropToNonEmptyRange, bool skipEmptyColumns, bool hideWorksheet, int[] dateColumnIndexes = null, int limitRowsQuantity = 0, bool disableScreenUpdating = true)
      : this(sourceRange, cropToNonEmptyRange, skipEmptyColumns, hideWorksheet, limitRowsQuantity, disableScreenUpdating)
    {
      RangeType = TempRangeType.CopiedRange;
      CreateCopiedTempRange(dateColumnIndexes);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TempRange"/> class that prepends an AutoPK column to the range.
    /// </summary>
    /// <param name="sourceRange">The original source <see cref="Excel.Range"/> whose data is copied to the temporary one.</param>
    /// <param name="cropToNonEmptyRange">Flag indicating whether the range is cropped to a subrange with only non-empty cells.</param>
    /// <param name="skipEmptyColumns">Flag indicating whether empty columns are not copied to the target range.</param>
    /// <param name="hideWorksheet">Flag indicating whether the new temporary <see cref="Excel.Worksheet"/> will be hidden.</param>
    /// <param name="startWithNumber">The first number in the sequence for the new first column.</param>
    /// <param name="dateColumnIndexes">Array of indexes of columns that will populate a date MySQL column.</param>
    /// <param name="limitRowsQuantity">Gets a limit on the number of rows copied from the source range to the temporary range. If less than 1 it means there is no limit.</param>
    /// <param name="disableScreenUpdating">Flag indicating whether screen updating will be disabled to speed up processing.</param>
    public TempRange(Excel.Range sourceRange, bool cropToNonEmptyRange, bool skipEmptyColumns, bool hideWorksheet, int startWithNumber = 1, int[] dateColumnIndexes = null, int limitRowsQuantity = 0, bool disableScreenUpdating = true)
      : this(sourceRange, cropToNonEmptyRange, skipEmptyColumns, hideWorksheet, limitRowsQuantity, disableScreenUpdating)
    {
      RangeType = TempRangeType.AutoPkRange;
      CreateAutoPkTempRange(startWithNumber, dateColumnIndexes);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TempRange"/> class with columns mapped from the source range.
    /// </summary>
    /// <param name="sourceRange">The original source <see cref="Excel.Range"/> whose data is copied to the temporary one.</param>
    /// <param name="cropToNonEmptyRange">Flag indicating whether the range is cropped to a subrange with only non-empty cells.</param>
    /// <param name="skipEmptyColumns">Flag indicating whether empty columns are not copied to the target range.</param>
    /// <param name="hideWorksheet">Flag indicating whether the new temporary <see cref="Excel.Worksheet"/> will be hidden.</param>
    /// <param name="mappedIndexes">An array of indexes containing the source column from the <see cref="sourceRange"/> whose contents will be copied to the returned range.</param>
    /// <param name="disableScreenUpdating">Flag indicating whether screen updating will be disabled to speed up processing.</param>
    public TempRange(Excel.Range sourceRange, bool cropToNonEmptyRange, bool skipEmptyColumns, bool hideWorksheet, IList<int> mappedIndexes, bool disableScreenUpdating = true)
      : this(sourceRange, cropToNonEmptyRange, skipEmptyColumns, hideWorksheet, 0, disableScreenUpdating)
    {
      RangeType = TempRangeType.MappedRange;
      CreateMappedTempRange(mappedIndexes);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TempRange"/> class.
    /// </summary>
    /// <param name="sourceRange">The original source <see cref="Excel.Range"/> whose data is copied to the temporary one.</param>
    /// <param name="cropToNonEmptyRange">Flag indicating whether the range is cropped to a subrange with only non-empty cells.</param>
    /// <param name="skipEmptyColumns">Flag indicating whether empty columns are not copied to the target range.</param>
    /// <param name="hideWorksheet">Flag indicating whether the new temporary <see cref="Excel.Worksheet"/> will be hidden.</param>
    /// <param name="limitRowsQuantity">Gets a limit on the number of rows copied from the source range to the temporary range. If less than 1 it means there is no limit.</param>
    /// <param name="disableScreenUpdating">Flag indicating whether screen updating will be disabled to speed up processing.</param>
    private TempRange(Excel.Range sourceRange, bool cropToNonEmptyRange, bool skipEmptyColumns, bool hideWorksheet, int limitRowsQuantity = 0, bool disableScreenUpdating = true)
    {
      _disableScreenupdating = disableScreenUpdating;
      CropToNonEmptyRange = cropToNonEmptyRange;
      LimitRowsQuantity = limitRowsQuantity;
      SkipEmptyColumns = skipEmptyColumns;
      SourceRange = sourceRange;
      SourceCroppedRange = null;
      CreateTempWorksheet(hideWorksheet);
      if (_disableScreenupdating)
      {
        Globals.ThisAddIn.Application.ScreenUpdating = false;
      }

      Globals.ThisAddIn.UsingTempWorksheet = true;
    }

    /// <summary>
    /// Describes the type of temporary range created.
    /// </summary>
    public enum TempRangeType
    {
      /// <summary>
      /// A temporary range with a prepended column holding sequential numbers represenging an automatic primary key column.
      /// </summary>
      AutoPkRange,

      /// <summary>
      /// A temporary range holding a copy of a source range.
      /// </summary>
      CopiedRange,

      /// <summary>
      /// A temporary range holding contents with columns mapped from a source range.
      /// </summary>
      MappedRange,
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the range was cropped to a subrange with only non-empty cells.
    /// </summary>
    public bool CropToNonEmptyRange { get; private set; }

    /// <summary>
    /// Gets a limit on the number of rows copied from the source range to the temporary range.
    /// If less than 1 it means there is no limit.
    /// </summary>
    public int LimitRowsQuantity { get; private set; }

    /// <summary>
    /// Gets the temporary <see cref="Excel.Range"/>.
    /// </summary>
    public Excel.Range Range { get; private set; }

    /// <summary>
    /// Gets the type of temporary range created.
    /// </summary>
    public TempRangeType RangeType { get; private set; }

    /// <summary>
    /// Gets a value indicating whether empty columns are not copied to the target range.
    /// </summary>
    public bool SkipEmptyColumns;

    /// <summary>
    /// Gets the original source <see cref="Excel.Range"/> cropped to a subrange with only non-empty cells.
    /// </summary>
    public Excel.Range SourceCroppedRange { get; private set; }

    /// <summary>
    /// Gets the original source <see cref="Excel.Range"/> whose data is copied to the temporary one.
    /// </summary>
    public Excel.Range SourceRange { get; private set; }

    /// <summary>
    /// Gets the temporary <see cref="Excel.Worksheet"/> that will contain the temporary <see cref="Excel.Range"/>.
    /// </summary>
    public Excel.Worksheet TempWorksheet { get; private set; }

    #endregion Properties

    /// <summary>
    /// Releases all resources used by the <see cref="TempRange"/> class
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases all resources used by the <see cref="TempRange"/> class
    /// </summary>
    /// <param name="disposing">If true this is called by Dispose(), otherwise it is called by the finalizer</param>
    protected virtual void Dispose(bool disposing)
    {
      // Free managed resources
      if (disposing)
      {
        Globals.ThisAddIn.Application.DisplayAlerts = false;

        // If the TempWorksheeet has been hidden, lower the hidden strength from VeryHidden to Hidden to avoid an error while attempting to delete it.
        if (TempWorksheet.Visible != Excel.XlSheetVisibility.xlSheetVisible)
        {
          TempWorksheet.Visible = Excel.XlSheetVisibility.xlSheetHidden;
        }

        TempWorksheet.Delete();
        Globals.ThisAddIn.Application.DisplayAlerts = true;
        if (_disableScreenupdating)
        {
          Globals.ThisAddIn.Application.ScreenUpdating = true;
        }

        Globals.ThisAddIn.UsingTempWorksheet = false;
      }

      // Add class finalizer if unmanaged resources are added to the class
      // Free unmanaged resources if there are any
    }

    /// <summary>
    /// Creates a temporary <see cref="Excel.Range"/> containing a copy of the data in <see cref="SourceRange"/> with a new sequential numeric column prepended to it.
    /// </summary>
    /// <param name="startWithNumber">The first number in the sequence for the new first column.</param>
    /// <param name="dateColumnIndexes">Array of indexes of columns that will populate a date MySQL column.</param>
    private void CreateAutoPkTempRange(int startWithNumber = 1, int[] dateColumnIndexes = null)
    {
      if (TempWorksheet == null)
      {
        return;
      }

      CreateCopiedTempRange(dateColumnIndexes);
      if (startWithNumber < 0)
      {
        return;
      }

      --startWithNumber;
      int rowsCount = Range.Rows.Count;
      Excel.Range firstColumn = TempWorksheet.Columns[1];
      firstColumn.Insert();
      firstColumn = TempWorksheet.Cells[1, 1];
      firstColumn = firstColumn.Resize[rowsCount, 1];
      firstColumn.FormulaArray = "=ROW()" + (startWithNumber > 0 ? string.Format(" + {0}", startWithNumber) : string.Empty);
      firstColumn = TempWorksheet.Cells[1, 1];
      Range = firstColumn.Resize[rowsCount, Range.Columns.Count + 1];
    }

    /// <summary>
    /// Creates a temporary <see cref="Excel.Range"/> containing a copy of the data in <see cref="SourceRange"/>.
    /// </summary>
    /// <param name="dateColumnIndexes">Array of indexes of columns that will populate a date MySQL column.</param>
    private void CreateCopiedTempRange(int[] dateColumnIndexes = null)
    {
      if (TempWorksheet == null)
      {
        return;
      }

      if (CropToNonEmptyRange)
      {
        SourceCroppedRange = SourceRange.GetNonEmptyRectangularAreaRange();
      }

      int firstTargetColumnIndex = 1;
      var sourceCopyRange = CropToNonEmptyRange ? SourceCroppedRange : SourceRange;
      int copiedRows = LimitRowsQuantity > 0 ? Math.Min(LimitRowsQuantity, sourceCopyRange.Rows.Count) : sourceCopyRange.Rows.Count;
      if (copiedRows < sourceCopyRange.Rows.Count)
      {
        sourceCopyRange = sourceCopyRange.Resize[copiedRows, sourceCopyRange.Columns.Count];
      }

      string sourceWorksheetName = sourceCopyRange.Worksheet.Name;
      foreach (Excel.Range sourceColumnRange in sourceCopyRange.Columns)
      {
        if (SkipEmptyColumns && !sourceColumnRange.ContainsAnyData())
        {
          continue;
        }

        Excel.Range targetColumnRange = TempWorksheet.Cells[1, firstTargetColumnIndex];
        targetColumnRange = targetColumnRange.Resize[copiedRows, 1];
        if (dateColumnIndexes != null && dateColumnIndexes.Contains(firstTargetColumnIndex))
        {
          string formula = string.Format("=IF({0}!{1}<>0,TEXT({0}!{1},LOCAL_MYSQL_DATE_FORMAT),\"{2}\")",
            sourceWorksheetName,
            sourceColumnRange.Address[false, false],
            DataTypeUtilities.MYSQL_EMPTY_DATE);
          targetColumnRange.FormulaArray = formula;
        }
        else
        {
          sourceColumnRange.Copy(targetColumnRange);
        }

        firstTargetColumnIndex++;
      }

      Range = TempWorksheet.UsedRange;
    }

    /// <summary>
    /// Creates a temporary <see cref="Excel.Range"/> containing a copy of the data in <see cref="SourceRange"/> according to the supplied column mapping indexes.
    /// </summary>
    /// <param name="mappedIndexes">An array of indexes containing the source column from the <see cref="SourceRange"/> whose contents will be copied to the returned range.</param>
    private void CreateMappedTempRange(IList<int> mappedIndexes)
    {
      if (TempWorksheet == null)
      {
        return;
      }

      if (CropToNonEmptyRange)
      {
        SourceCroppedRange = SourceRange.GetNonEmptyRectangularAreaRange();
      }

      var sourceCopyRange = CropToNonEmptyRange ? SourceCroppedRange : SourceRange;
      for (int arrayIndex = 0; arrayIndex < mappedIndexes.Count; arrayIndex++)
      {
        int excelColumnIndex = arrayIndex + 1;
        if (excelColumnIndex > sourceCopyRange.Columns.Count)
        {
          break;
        }

        int mappedIndex = mappedIndexes[arrayIndex];
        if (mappedIndex < 1)
        {
          continue;
        }

        Excel.Range sourceColumnRange = sourceCopyRange.Columns[mappedIndex];
        Excel.Range targetColumnTopCell = TempWorksheet.Cells[1, excelColumnIndex];
        Excel.Range targetColumnRange = targetColumnTopCell.Resize[sourceCopyRange.Rows.Count, 1];
        sourceColumnRange.Copy(targetColumnRange);
      }

      Range = TempWorksheet.Cells[1, 1];
      Range = Range.Resize[sourceCopyRange.Rows.Count, sourceCopyRange.Columns.Count];
    }

    /// <summary>
    /// Creates the temporary <see cref="Excel.Worksheet"/> that will contain the temporary <see cref="Excel.Range"/>.
    /// </summary>
    /// <param name="hideWorksheet">Flag indicating whether the new temporary <see cref="Excel.Worksheet"/> will be hidden.</param>
    private void CreateTempWorksheet(bool hideWorksheet)
    {
      if (SourceRange == null)
      {
        return;
      }

      try
      {
        var parentWorkbook = SourceRange.Worksheet.Parent as Excel.Workbook;
        if (parentWorkbook == null)
        {
          return;
        }

        TempWorksheet = parentWorkbook.Worksheets.Add();
        TempWorksheet.Visible = hideWorksheet
          ? Excel.XlSheetVisibility.xlSheetVeryHidden
          : Excel.XlSheetVisibility.xlSheetVisible;
        TempWorksheet.Name = parentWorkbook.GetWorksheetNameAvoidingDuplicates("TEMP_SHEET");
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex);
        MiscUtilities.ShowCustomizedErrorDialog(ex.Message, ex.StackTrace);
      }
    }
  }
}
