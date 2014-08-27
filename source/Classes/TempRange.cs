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
using ExcelInterop = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a temporary Excel range stored in a <see cref="ExcelInterop.Worksheet"/> that will be deleted when the object is disposed of.
  /// </summary>
  public class TempRange : IDisposable
  {
    #region Fields

    /// <summary>
    /// Flag indicating whether screen updating will be disabled to speed up processing.
    /// </summary>
    private readonly bool _disableScreenUpdating;

    /// <summary>
    /// Flag indicating whether the <seealso cref="Dispose"/> method has already been called.
    /// </summary>
    private bool _disposed;

    /// <summary>
    /// Flag holding the current value in <see cref="ExcelInterop.Application.ScreenUpdating"/>.
    /// </summary>
    private readonly bool _previousScreenUpdatingValue;

    /// <summary>
    /// The original source <see cref="ExcelInterop.Range"/> cropped to a subrange with only non-empty cells.
    /// </summary>
    private ExcelInterop.Range _sourceCroppedRange;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="TempRange"/> class that prepends an AutoPK column to the range.
    /// </summary>
    /// <param name="sourceRange">The original source <see cref="ExcelInterop.Range"/> whose data is copied to the temporary one.</param>
    /// <param name="cropToNonEmptyRange">Flag indicating whether the range is cropped to a subrange with only non-empty cells.</param>
    /// <param name="skipEmptyColumns">Flag indicating whether empty columns are not copied to the target range.</param>
    /// <param name="hideWorksheet">Flag indicating whether the new temporary <see cref="ExcelInterop.Worksheet"/> will be hidden.</param>
    /// <param name="createAutoPkRange">Flag indicating whether a sequential numbered column is prepended to the range to represent the values for an AutoPK column.</param>
    /// <param name="firstRowContainsColumnNames">Flag indicating whether the the first row of excelData contains the column names for a new table.</param>
    /// <param name="dateColumnIndexes">Array of indexes of columns that will populate a date MySQL column.</param>
    /// <param name="limitRowsQuantity">Gets a limit on the number of rows copied from the source range to the temporary range. If less than 1 it means there is no limit.</param>
    /// <param name="disableScreenUpdating">Flag indicating whether screen updating will be disabled to speed up processing.</param>
    public TempRange(ExcelInterop.Range sourceRange, bool cropToNonEmptyRange, bool skipEmptyColumns, bool hideWorksheet, bool createAutoPkRange, bool firstRowContainsColumnNames = false, int[] dateColumnIndexes = null, int limitRowsQuantity = 0, bool disableScreenUpdating = true)
      : this(sourceRange, cropToNonEmptyRange, skipEmptyColumns, hideWorksheet, limitRowsQuantity, disableScreenUpdating)
    {
      if (createAutoPkRange)
      {
        RangeType = TempRangeType.AutoPkRange;
        CreateAutoPkTempRange(dateColumnIndexes, firstRowContainsColumnNames);
      }
      else
      {
        RangeType = TempRangeType.CopiedRange;
        CreateCopiedTempRange(dateColumnIndexes);
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TempRange"/> class with columns mapped from the source range.
    /// </summary>
    /// <param name="sourceRange">The original source <see cref="ExcelInterop.Range"/> whose data is copied to the temporary one.</param>
    /// <param name="cropToNonEmptyRange">Flag indicating whether the range is cropped to a subrange with only non-empty cells.</param>
    /// <param name="skipEmptyColumns">Flag indicating whether empty columns are not copied to the target range.</param>
    /// <param name="hideWorksheet">Flag indicating whether the new temporary <see cref="ExcelInterop.Worksheet"/> will be hidden.</param>
    /// <param name="mappedIndexes">An array of indexes containing the source column from the <see cref="sourceRange"/> whose contents will be copied to the returned range.</param>
    /// <param name="disableScreenUpdating">Flag indicating whether screen updating will be disabled to speed up processing.</param>
    public TempRange(ExcelInterop.Range sourceRange, bool cropToNonEmptyRange, bool skipEmptyColumns, bool hideWorksheet, IList<int> mappedIndexes, bool disableScreenUpdating = true)
      : this(sourceRange, cropToNonEmptyRange, skipEmptyColumns, hideWorksheet, 0, disableScreenUpdating)
    {
      RangeType = TempRangeType.MappedRange;
      CreateMappedTempRange(mappedIndexes);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TempRange"/> class.
    /// </summary>
    /// <param name="sourceRange">The original source <see cref="ExcelInterop.Range"/> whose data is copied to the temporary one.</param>
    /// <param name="cropToNonEmptyRange">Flag indicating whether the range is cropped to a subrange with only non-empty cells.</param>
    /// <param name="skipEmptyColumns">Flag indicating whether empty columns are not copied to the target range.</param>
    /// <param name="hideWorksheet">Flag indicating whether the new temporary <see cref="ExcelInterop.Worksheet"/> will be hidden.</param>
    /// <param name="limitRowsQuantity">Gets a limit on the number of rows copied from the source range to the temporary range. If less than 1 it means there is no limit.</param>
    /// <param name="disableScreenUpdating">Flag indicating whether screen updating will be disabled to speed up processing.</param>
    private TempRange(ExcelInterop.Range sourceRange, bool cropToNonEmptyRange, bool skipEmptyColumns, bool hideWorksheet, int limitRowsQuantity = 0, bool disableScreenUpdating = true)
    {
      _disableScreenUpdating = disableScreenUpdating;
      _disposed = false;
      _previousScreenUpdatingValue = false;
      _sourceCroppedRange = null;
      if (_disableScreenUpdating)
      {
        _previousScreenUpdatingValue = Globals.ThisAddIn.Application.ScreenUpdating;
        Globals.ThisAddIn.Application.ScreenUpdating = false;
      }

      Globals.ThisAddIn.UsingTempWorksheet = true;
      CropToNonEmptyRange = cropToNonEmptyRange;
      LimitRowsQuantity = limitRowsQuantity;
      SkipEmptyColumns = skipEmptyColumns;
      SourceRange = sourceRange;
      CreateTempWorksheet(hideWorksheet);
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
    /// Gets the temporary <see cref="ExcelInterop.Range"/>.
    /// </summary>
    public ExcelInterop.Range Range { get; private set; }

    /// <summary>
    /// Gets the type of temporary range created.
    /// </summary>
    public TempRangeType RangeType { get; private set; }

    /// <summary>
    /// Gets a value indicating whether empty columns are not copied to the target range.
    /// </summary>
    public bool SkipEmptyColumns { get; private set; }

    /// <summary>
    /// Gets the original source <see cref="ExcelInterop.Range"/> whose data is copied to the temporary one.
    /// </summary>
    public ExcelInterop.Range SourceRange { get; private set; }

    /// <summary>
    /// Gets the temporary <see cref="ExcelInterop.Worksheet"/> that will contain the temporary <see cref="ExcelInterop.Range"/>.
    /// </summary>
    public ExcelInterop.Worksheet TempWorksheet { get; private set; }

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
      if (_disposed)
      {
        return;
      }

      // Free managed resources
      if (disposing)
      {
        var previousDisplayAlertsValue = Globals.ThisAddIn.Application.DisplayAlerts;
        Globals.ThisAddIn.Application.DisplayAlerts = false;

        // If the TempWorksheeet has been hidden, lower the hidden strength from VeryHidden to Hidden to avoid an error while attempting to delete it.
        if (TempWorksheet.Visible != ExcelInterop.XlSheetVisibility.xlSheetVisible)
        {
          TempWorksheet.Visible = ExcelInterop.XlSheetVisibility.xlSheetHidden;
        }

        TempWorksheet.Delete();
        TempWorksheet = null;
        Globals.ThisAddIn.Application.DisplayAlerts = previousDisplayAlertsValue;
        if (_disableScreenUpdating)
        {
          Globals.ThisAddIn.Application.ScreenUpdating = _previousScreenUpdatingValue;
        }

        Globals.ThisAddIn.UsingTempWorksheet = false;
        SourceRange.Select();
        SourceRange = null;
        Range = null;
      }

      // Add class finalizer if unmanaged resources are added to the class
      // Free unmanaged resources if there are any
      _disposed = true;
    }

    /// <summary>
    /// Creates a temporary <see cref="ExcelInterop.Range"/> containing a copy of the data in <see cref="SourceRange"/> with a new sequential numeric column prepended to it.
    /// </summary>
    /// <param name="dateColumnIndexes">Array of indexes of columns that will populate a date MySQL column.</param>
    /// <param name="firstRowContainsColumnNames">Flag indicating whether the the first row of excelData contains the column names for a new table.</param>
    private void CreateAutoPkTempRange(int[] dateColumnIndexes = null, bool firstRowContainsColumnNames = false)
    {
      if (TempWorksheet == null)
      {
        return;
      }

      CreateCopiedTempRange(dateColumnIndexes);
      int rowsCount = Range.Rows.Count;
      ExcelInterop.Range firstColumn = TempWorksheet.Columns[1];
      firstColumn.Insert();
      firstColumn = TempWorksheet.Cells[1, 1];
      firstColumn = firstColumn.Resize[rowsCount, 1];
      firstColumn.FormulaArray = string.Format("=ROW() - {0}", firstRowContainsColumnNames ? 1 : 0);
      firstColumn = TempWorksheet.Cells[1, 1];
      Range = firstColumn.Resize[rowsCount, Range.Columns.Count + 1];
    }

    /// <summary>
    /// Creates a temporary <see cref="ExcelInterop.Range"/> containing a copy of the data in <see cref="SourceRange"/>.
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
        _sourceCroppedRange = SourceRange.GetNonEmptyRectangularAreaRange();
      }

      int firstTargetColumnIndex = 1;
      var sourceCopyRange = CropToNonEmptyRange ? _sourceCroppedRange : SourceRange;
      int copiedRows = LimitRowsQuantity > 0 ? Math.Min(LimitRowsQuantity, sourceCopyRange.Rows.Count) : sourceCopyRange.Rows.Count;
      if (copiedRows < sourceCopyRange.Rows.Count)
      {
        sourceCopyRange = sourceCopyRange.Resize[copiedRows, sourceCopyRange.Columns.Count];
      }

      string sourceWorksheetName = sourceCopyRange.Worksheet.Name;
      foreach (ExcelInterop.Range sourceColumnRange in sourceCopyRange.Columns)
      {
        if (SkipEmptyColumns && !sourceColumnRange.ContainsAnyData())
        {
          continue;
        }

        ExcelInterop.Range targetColumnRange = TempWorksheet.Cells[1, firstTargetColumnIndex];
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
    /// Creates a temporary <see cref="ExcelInterop.Range"/> containing a copy of the data in <see cref="SourceRange"/> according to the supplied column mapping indexes.
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
        _sourceCroppedRange = SourceRange.GetNonEmptyRectangularAreaRange();
      }

      var sourceCopyRange = CropToNonEmptyRange ? _sourceCroppedRange : SourceRange;
      for (int arrayIndex = 0; arrayIndex < mappedIndexes.Count; arrayIndex++)
      {
        int excelColumnIndex = arrayIndex + 1;
        int mappedIndex = mappedIndexes[arrayIndex];
        if (mappedIndex < 1)
        {
          continue;
        }

        ExcelInterop.Range sourceColumnRange = sourceCopyRange.Columns[mappedIndex];
        ExcelInterop.Range targetColumnTopCell = TempWorksheet.Cells[1, excelColumnIndex];
        ExcelInterop.Range targetColumnRange = targetColumnTopCell.Resize[sourceCopyRange.Rows.Count, 1];
        sourceColumnRange.Copy(targetColumnRange);
      }

      Range = TempWorksheet.Cells[1, 1];
      Range = Range.Resize[sourceCopyRange.Rows.Count, mappedIndexes.Count];
    }

    /// <summary>
    /// Creates the temporary <see cref="ExcelInterop.Worksheet"/> that will contain the temporary <see cref="ExcelInterop.Range"/>.
    /// </summary>
    /// <param name="hideWorksheet">Flag indicating whether the new temporary <see cref="ExcelInterop.Worksheet"/> will be hidden.</param>
    private void CreateTempWorksheet(bool hideWorksheet)
    {
      if (SourceRange == null)
      {
        return;
      }

      try
      {
        var parentWorkbook = SourceRange.Worksheet.Parent as ExcelInterop.Workbook;
        if (parentWorkbook == null)
        {
          return;
        }

        TempWorksheet = parentWorkbook.Worksheets.Add();
        TempWorksheet.Visible = hideWorksheet
          ? ExcelInterop.XlSheetVisibility.xlSheetVeryHidden
          : ExcelInterop.XlSheetVisibility.xlSheetVisible;
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
