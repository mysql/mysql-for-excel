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
  using System.Data;
  using Excel = Microsoft.Office.Interop.Excel;

  /// <summary>
  /// Records modifications done to an Excel row mapping it to its corresponding data table row.
  /// </summary>
  public class RangeAndAddress
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RangeAndAddress"/> class.
    /// </summary>
    /// <param name="modification">The type of modification done to the Excel range.</param>
    /// <param name="range">The Excel range being recorded.</param>
    /// <param name="address">The address of the Excel range.</param>
    /// <param name="rangeColor">The fill color assigned to the Excel range cells.</param>
    /// <param name="excelRow">The ordinal index of the Excel row corresponding to the Excel range.</param>
    /// <param name="tableRow">The <see cref="DataRow"/> mapped to the Excel range being recorded.</param>
    public RangeAndAddress(RangeModification modification, Excel.Range range, string address, int rangeColor, int excelRow, DataRow tableRow)
    {
      Modification = modification;
      Range = range;
      Address = address;
      RangeColor = rangeColor;
      ExcelRow = excelRow;
      TableRow = tableRow;
    }

    /// <summary>
    /// Specifies identifiers to indicate the type of modification done to Excel ranges.
    /// </summary>
    public enum RangeModification
    {
      /// <summary>
      /// A row was added to the editing Excel range.
      /// </summary>
      Added,

      /// <summary>
      /// A row was deleted from the editing Excel range.
      /// </summary>
      Deleted,

      /// <summary>
      /// Cell values were modified in the editing Excel range.
      /// </summary>
      Updated
    }

    #region Properties

    /// <summary>
    /// Gets or sets the address of the Excel range.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Gets or sets the ordinal index of the Excel row corresponding to the Excel range.
    /// </summary>
    public int ExcelRow { get; set; }

    /// <summary>
    /// Gets the type of modification done to the Excel range.
    /// </summary>
    public RangeModification Modification { get; private set; }

    /// <summary>
    /// Gets or sets the Excel range being recorded.
    /// </summary>
    public Excel.Range Range { get; set; }

    /// <summary>
    /// Gets the fill color assigned to the Excel range cells.
    /// </summary>
    public int RangeColor { get; private set; }

    /// <summary>
    /// Gets the <see cref="DataRow"/> mapped to the Excel range being recorded.
    /// </summary>
    public DataRow TableRow { get; private set; }

    #endregion Properties
  }
}