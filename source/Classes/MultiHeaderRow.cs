// Copyright (c) 2015, Oracle and/or its affiliates. All rights reserved.
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MySQL.ForExcel.Classes.EventArguments;
using MySQL.ForExcel.Controls;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a header row that contains header columns used in a <see cref="MultiHeaderDataGridView"/> control.
  /// </summary>
  public class MultiHeaderRow : IList<MultiHeaderCell>
  {
    #region Fields

    /// <summary>
    /// List of <see cref="MultiHeaderCell"/> objects contained in the current header row.
    /// </summary>
    private readonly List<MultiHeaderCell> _headerCells;

    /// <summary>
    /// The height of this header row, in pixels, calculated by using the maximum height of the contained <see cref="MultiHeaderCell"/> objects in this row.
    /// </summary>
    private int _height;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiHeaderRow"/> class.
    /// </summary>
    /// <param name="initialColumnsCapacity">The initial capacity of the colletion.</param>
    public MultiHeaderRow(int initialColumnsCapacity = 0)
    {
      _height = 0;
      _headerCells = new List<MultiHeaderCell>(initialColumnsCapacity);
    }

    #region Properties

    /// <summary>
    /// Gets the height of this header row, in pixels, calculated by using the maximum height of the contained <see cref="MultiHeaderCell"/> objects in this row.
    /// </summary>
    public int Height
    {
      get
      {
        if (_height == 0)
        {
          _height = _headerCells.Select(headerCell => headerCell.CellSize.Height).Concat(new[] { 0 }).Max();
        }

        return _height;
      }
    }

    #region IList implementation

    /// <summary>
    /// Gets the number of columns contained in this row.
    /// </summary>
    public int Count
    {
      get
      {
        return _headerCells.Count;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this colletion is read-only.
    /// </summary>
    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <returns>The element at the specified index.</returns>
    public MultiHeaderCell this[int index]
    {
      get
      {
        return _headerCells[index];
      }

      set
      {
        _headerCells[index] = value;
      }
    }

    #endregion IList implementation

    #endregion Properties

    #region Events

    /// <summary>
    /// Occurs when the value of <see cref="MultiHeaderCell.ColumnSpan"/> changes.
    /// </summary>
    public event MultiHeaderCell.HeaderCellColumnSpanChangedHandler HeaderCellColumnSpanChanged;

    /// <summary>
    /// Occurs when the value of <see cref="MultiHeaderCell.Text"/> changes.
    /// </summary>
    public event MultiHeaderCell.HeaderCellTextChangedHandler HeaderCellTextChanged;

    #endregion Events

    #region IList implementation

    /// <summary>
    /// Adds an item to this header columns collection.
    /// </summary>
    /// <param name="headerCell">A <see cref="MultiHeaderCell"/> to add.</param>
    public void Add(MultiHeaderCell headerCell)
    {
      headerCell.HeaderCellColumnSpanChanged += HeaderCellColumnSpanChangedAction;
      headerCell.HeaderCellTextChanged += HeaderCellTextChangedAction;
      _headerCells.Add(headerCell);

      // Invalidate Height so it gets recalculated
      _height = 0;
    }

    /// <summary>
    /// Removes all items from this header columns colletction.
    /// </summary>
    public void Clear()
    {
      _headerCells.Clear();

      // Invalidate Height so it gets recalculated
      _height = 0;
    }

    /// <summary>
    /// Determines whether this header columns collection contains a specific value.
    /// </summary>
    /// <param name="headerCell">A <see cref="MultiHeaderCell"/>.</param>
    /// <returns><c>true</c> if the header column exists in the collection, <c>false</c> otherwise.</returns>
    public bool Contains(MultiHeaderCell headerCell)
    {
      return _headerCells.Contains(headerCell);
    }

    /// <summary>
    /// Copies the elements of this header columns collection to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from this header columns collection. The <see cref="Array"/> must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in <seealso cref="array"/> at which copying begins.</param>
    public void CopyTo(MultiHeaderCell[] array, int arrayIndex)
    {
      _headerCells.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Returns an enumerator that iterates through this header columns collection.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through this header columns collection.</returns>
    public IEnumerator<MultiHeaderCell> GetEnumerator()
    {
      return _headerCells.GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through this header columns collection.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through this header columns collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    /// <summary>
    /// Determines the index of a specific item in this header columns collection.
    /// </summary>
    /// <param name="headerCell">A <see cref="MultiHeaderCell"/>.</param>
    /// <returns></returns>
    public int IndexOf(MultiHeaderCell headerCell)
    {
      return _headerCells.IndexOf(headerCell);
    }

    /// <summary>
    /// Inserts an item to this header columns collection at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which <seealso cref="headerCell"/> should be inserted.</param>
    /// <param name="headerCell">A <see cref="MultiHeaderCell"/> to insert to this header columns collection.</param>
    public void Insert(int index, MultiHeaderCell headerCell)
    {
      _headerCells.Insert(index, headerCell);

      // Invalidate Height so it gets recalculated
      _height = 0;
    }

    /// <summary>
    /// Removes the first occurrence of a specific object from this header columns collection.
    /// </summary>
    /// <param name="headerCell">A <see cref="MultiHeaderCell"/>.</param>
    /// <returns>
    /// <c>true</c> if <seealso cref="headerCell"/> was successfully removed from this header columns collection, <c>false</c> otherwise.
    /// This method also returns <c>false</c> if <seealso cref="headerCell"/> is not found in this header columns collection.
    /// </returns>
    public bool Remove(MultiHeaderCell headerCell)
    {
      // Invalidate Height so it gets recalculated
      _height = 0;

      return _headerCells.Remove(headerCell);
    }

    /// <summary>
    /// Removes a <see cref="MultiHeaderCell"/> from this header columns collection at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="MultiHeaderCell"/> to remove.</param>
    public void RemoveAt(int index)
    {
      _headerCells.RemoveAt(index);

      // Invalidate Height so it gets recalculated
      _height = 0;
    }

    #endregion IList implementation

    /// <summary>
    /// Creates a new <see cref="MultiHeaderCell"/> object with the next consecutive column index from the last one in the collection.
    /// </summary>
    /// <param name="text">The text used in the top header of the grid column.</param>
    /// <param name="style">The <see cref="DataGridViewCellStyle"/> containing formatting and style of the header cell.</param>
    /// <returns>A new <see cref="MultiHeaderCell"/> object with the next consecutive column index from the last one in the collection.</returns>
    public MultiHeaderCell NewHeaderCell(string text, DataGridViewCellStyle style)
    {
      return new MultiHeaderCell(text, _headerCells.Count, style);
    }

    /// <summary>
    /// Recalculates the header cell size for each <see cref="MultiHeaderCell"/> objects in this row.
    /// </summary>
    public void RecalculateCellSizes()
    {
      foreach (var headerCell in _headerCells.Where(headerCell => !headerCell.InSpan))
      {
        headerCell.CalculateCellSize();
      }

      // Invalidate Height so it gets recalculated
      _height = 0;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="MultiHeaderCell.ColumnSpan"/> value changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    private void HeaderCellColumnSpanChangedAction(object sender, HeaderCellColumnSpanChangedArgs args)
    {
      var headerCell = args.HeaderCell;
      if (headerCell == null)
      {
        return;
      }

      if (headerCell.ColumnIndex + args.NewColumnSpan > _headerCells.Count)
      {
        // If column span was incorrectly set to span more columns than the ones in the collection, reset it to the largest possible span.
        headerCell.ColumnSpan = _headerCells.Count - headerCell.ColumnIndex;
        return;
      }

      ResetHeaderCellsInSpan(headerCell.ColumnIndex, args.OldColumnSpan, false);
      ResetHeaderCellsInSpan(headerCell.ColumnIndex, args.NewColumnSpan, true);

      // Invalidate Height so it gets recalculated
      _height = 0;

      // Fire corresponding event
      if (HeaderCellColumnSpanChanged != null)
      {
        HeaderCellColumnSpanChanged(this, args);
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="MultiHeaderCell.Text"/> value changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    private void HeaderCellTextChangedAction(object sender, HeaderCellTextChangedArgs args)
    {
      // Invalidate Height so it gets recalculated
      _height = 0;

      // Fire corresponding event
      if (HeaderCellTextChanged != null)
      {
        HeaderCellTextChanged(this, args);
      }
    }

    /// <summary>
    /// Resets the <see cref="MultiHeaderCell.InSpan"/> and <see cref="MultiHeaderCell.ColumnSpan"/> values of adjacent header cells in this row.
    /// </summary>
    /// <param name="columnIndex">The index of the column header with a column span being set.</param>
    /// <param name="columnSpan">The column span of the column header.</param>
    /// <param name="inSpan">Flag indicating if the adjacent columns should be in the span or not.</param>
    private void ResetHeaderCellsInSpan(int columnIndex, int columnSpan, bool inSpan)
    {
      for (int cellIndex = columnIndex + 1; cellIndex < columnIndex + columnSpan; cellIndex++)
      {
        var headerCell = _headerCells.FirstOrDefault(hc => hc.ColumnIndex == cellIndex);
        if (headerCell == null)
        {
          continue;
        }

        if (inSpan && headerCell.ColumnSpan > 1)
        {
          headerCell.ColumnSpan = 1;
        }

        headerCell.InSpan = inSpan;
      }
    }
  }
}
