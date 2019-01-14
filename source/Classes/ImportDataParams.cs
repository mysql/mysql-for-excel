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
using ExcelInterop = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Specifies parameters used on Import Data operations.
  /// </summary>
  public class ImportDataParams
  {
    #region Fields

    /// <summary>
    /// The number of rows to include in the select query.
    /// </summary>
    private int _rowsCount;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportDataParams"/> struct.
    /// </summary>
    /// <param name="dbObjectName">The name of the MySQL table, view or procedure to import data from.</param>
    public ImportDataParams(string dbObjectName)
    {
      _rowsCount = -1;
      AddSummaryRow = false;
      ColumnsNamesList = null;
      CreatePivotTable = false;
      DbObjectName = dbObjectName;
      FirstRowIndex = -1;
      ForEditDataOperation = false;
      IncludeColumnNames = true;
      IntoNewWorksheet = true;
      PivotTablePosition = ExcelUtilities.PivotTablePosition.Right;
    }

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether to include a row with summary fields at the end of the data rows.
    /// </summary>
    public bool AddSummaryRow { get; set; }

    /// <summary>
    /// Gets or sets the selected columns list. All columns are to be returned if <c>null</c>.
    /// </summary>
    public List<string> ColumnsNamesList { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a <see cref="ExcelInterop.PivotTable"/> is created for the imported data.
    /// </summary>
    public bool CreatePivotTable { get; set; }

    /// <summary>
    /// Gets the name of the MySQL table, view or procedure to import data from.
    /// </summary>
    public string DbObjectName { get; }

    /// <summary>
    /// Gets or sets the index of the row where the select query will start pulling data from.
    /// </summary>
    public int FirstRowIndex { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if column names will be imported as the first row of imported data.
    /// </summary>
    public bool IncludeColumnNames { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the data will be imported to a new <see cref="ExcelInterop.Worksheet"/> or to the active one.
    /// </summary>
    public bool IntoNewWorksheet { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the import is part of an Edit Data operation.
    /// </summary>
    public bool ForEditDataOperation { get; set; }

    /// <summary>
    /// Gets or sets the position where new <see cref="ExcelInterop.PivotTable"/> objects are placed relative to imported table's data.
    /// </summary>
    public ExcelUtilities.PivotTablePosition PivotTablePosition { get; set; }

    /// <summary>
    /// Gets or sets the number of rows to include in the select query.
    /// </summary>
    public int RowsCount
    {
      get => _rowsCount;
      set => _rowsCount = Globals.ThisAddIn.ActiveWorkbook.Excel8CompatibilityMode ? Math.Min(ushort.MaxValue, value) : value;
    }

    #endregion Properties
  }
}
