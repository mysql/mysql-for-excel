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
using System.Drawing;
using MySQL.ForExcel.Controls;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a header row that contains header columns used in a <see cref="MultiHeaderDataGridView"/> control.
  /// </summary>
  public class MultiHeaderRow : IList<MultiHeaderColumn>
  {
    #region Fields

    /// <summary>
    /// Gets a list of <see cref="MultiHeaderColumn"/> objects contained in the current header row.
    /// </summary>
    private readonly List<MultiHeaderColumn> _headerColumns;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiHeaderRow"/> class.
    /// </summary>
    /// <param name="defaultHeight">Default headers row height in pixels.</param>
    /// <param name="initialColumnsCapacity">The initial capacity of the colletion.</param>
    public MultiHeaderRow(int defaultHeight, int initialColumnsCapacity = 0)
    {
      _headerColumns = new List<MultiHeaderColumn>(initialColumnsCapacity);
      Height = defaultHeight;
    }

    #region Properties

    /// <summary>
    /// The height in pixels of this header row.
    /// </summary>
    public int Height { get; private set; }

    #region IList implementation

    /// <summary>
    /// Gets the number of columns contained in this row.
    /// </summary>
    public int Count
    {
      get
      {
        return _headerColumns.Count;
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
    public MultiHeaderColumn this[int index]
    {
      get
      {
        return _headerColumns[index];
      }

      set
      {
        _headerColumns[index] = value;
      }
    }

    #endregion IList implementation

    #endregion Properties

    #region IList implementation

    /// <summary>
    /// Adds an item to this header columns collection.
    /// </summary>
    /// <param name="headerColumn">A <see cref="MultiHeaderColumn"/> to add.</param>
    public void Add(MultiHeaderColumn headerColumn)
    {
      _headerColumns.Add(headerColumn);
    }

    /// <summary>
    /// Removes all items from this header columns colletction.
    /// </summary>
    public void Clear()
    {
      _headerColumns.Clear();
    }

    /// <summary>
    /// Determines whether this header columns collection contains a specific value.
    /// </summary>
    /// <param name="headerColumn">A <see cref="MultiHeaderColumn"/>.</param>
    /// <returns><c>true</c> if the header column exists in the collection, <c>false</c> otherwise.</returns>
    public bool Contains(MultiHeaderColumn headerColumn)
    {
      return _headerColumns.Contains(headerColumn);
    }

    /// <summary>
    /// Copies the elements of this header columns collection to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from this header columns collection. The <see cref="Array"/> must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in <seealso cref="array"/> at which copying begins.</param>
    public void CopyTo(MultiHeaderColumn[] array, int arrayIndex)
    {
      _headerColumns.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Returns an enumerator that iterates through this header columns collection.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through this header columns collection.</returns>
    public IEnumerator<MultiHeaderColumn> GetEnumerator()
    {
      return _headerColumns.GetEnumerator();
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
    /// <param name="headerColumn">A <see cref="MultiHeaderColumn"/>.</param>
    /// <returns></returns>
    public int IndexOf(MultiHeaderColumn headerColumn)
    {
      return _headerColumns.IndexOf(headerColumn);
    }

    /// <summary>
    /// Inserts an item to this header columns collection at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which <seealso cref="headerColumn"/> should be inserted.</param>
    /// <param name="headerColumn">A <see cref="MultiHeaderColumn"/> to insert to this header columns collection.</param>
    public void Insert(int index, MultiHeaderColumn headerColumn)
    {
      _headerColumns.Insert(index, headerColumn);
    }

    /// <summary>
    /// Removes the first occurrence of a specific object from this header columns collection.
    /// </summary>
    /// <param name="headerColumn">A <see cref="MultiHeaderColumn"/>.</param>
    /// <returns>
    /// <c>true</c> if <seealso cref="headerColumn"/> was successfully removed from this header columns collection, <c>false</c> otherwise.
    /// This method also returns <c>false</c> if <seealso cref="headerColumn"/> is not found in this header columns collection.
    /// </returns>
    public bool Remove(MultiHeaderColumn headerColumn)
    {
      return _headerColumns.Remove(headerColumn);
    }

    /// <summary>
    /// Removes a <see cref="MultiHeaderColumn"/> from this header columns collection at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="MultiHeaderColumn"/> to remove.</param>
    public void RemoveAt(int index)
    {
      _headerColumns.RemoveAt(index);
    }

    #endregion IList implementation

    /// <summary>
    /// Gets the maximum height, in pixels, of the text used on the <see cref="MultiHeaderColumn"/> objects in this row.
    /// </summary>
    /// <param name="graphics">The <see cref="Graphics"/> instance used to draw the text.</param>
    /// <param name="usePaddings">Flag indicating whether paddings are used to compute the height.</param>
    /// <param name="separatorsWidth">The width, in pixels, of the column header separators.</param>
    /// <returns>The maximum height of the text used on the <see cref="MultiHeaderColumn"/> objects in this row.</returns>
    public void ComputeHeight(Graphics graphics, bool usePaddings, int separatorsWidth)
    {
      if (graphics == null)
      {
        return;
      }

      int maxHeight = 0;
      foreach (var headerColumn in _headerColumns)
      {
        var text = string.IsNullOrEmpty(headerColumn.Text) ? "Text" : headerColumn.Text;
        var textHeight = Convert.ToInt32(graphics.MeasureString(text, headerColumn.Style.Font).Height);
        int paddingsHeight = usePaddings ? headerColumn.Style.Padding.Top + headerColumn.Style.Padding.Bottom : 0;
        maxHeight = Math.Max(maxHeight, textHeight + paddingsHeight + separatorsWidth);
      }

      Height = maxHeight;
    }
  }
}
