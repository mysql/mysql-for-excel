// Copyright (c) 2014, 2018, Oracle and/or its affiliates. All rights reserved.
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

using System.Collections;
using System.Windows.Forms;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Implements <see cref="IComparer"/> to sort a <see cref="ListView"/> by a specific column.
  /// </summary>
  public class ListViewColumnSorter : IComparer
  {
    #region Fields

    /// <summary>
    /// Case insensitive comparer object
    /// </summary>
    private readonly CaseInsensitiveComparer _objectCompare;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ListViewColumnSorter"/> class.
    /// </summary>
    public ListViewColumnSorter()
      : this(0, SortOrder.None)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListViewColumnSorter"/> class.
    /// </summary>
    /// <param name="sortColumnIndex">The index of the column to which to apply the sorting operation.</param>
    /// <param name="order">The <see cref="SortOrder"/> to apply.</param>
    public ListViewColumnSorter(int sortColumnIndex, SortOrder order)
    {
      SortColumnIndex = sortColumnIndex;
      Order = order;
      _objectCompare = new CaseInsensitiveComparer();
    }

    #region Properties

    /// <summary>
    /// Gets or sets the index of the column to which to apply the sorting operation.
    /// </summary>
    public int SortColumnIndex { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="SortOrder"/> to apply.
    /// </summary>
    public SortOrder Order { get; set; }

    #endregion Properties

    /// <summary>
    /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
    /// </summary>
    /// <param name="x">First object to be compared</param>
    /// <param name="y">Second object to be compared</param>
    /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
    public int Compare(object x, object y)
    {
      // Cast the objects to be compared to ListViewItem objects
      if (!(x is ListViewItem listViewX) || !(y is ListViewItem listViewY))
      {
        return 0;
      }

      // Compare the two items
      var compareResult = _objectCompare.Compare(listViewX.SubItems[SortColumnIndex].Text, listViewY.SubItems[SortColumnIndex].Text);

      // Calculate correct return value based on object comparison
      switch (Order)
      {
        case SortOrder.Ascending:
          return compareResult;

        case SortOrder.Descending:
          return -compareResult;

        default:
          return 0;
      }
    }
  }
}
