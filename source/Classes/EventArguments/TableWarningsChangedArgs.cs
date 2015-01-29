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

namespace MySQL.ForExcel.Classes.EventArguments
{
  /// <summary>
  /// Event arguments for the TableWarningsChanged event.
  /// </summary>
  public class TableWarningsChangedArgs : EventArgs
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="TableWarningsChangedArgs"/> class.
    /// </summary>
    /// <param name="column">The <see cref="MySqlDataColumn"/> object that contains changes in its warning texts.</param>
    public TableWarningsChangedArgs(MySqlDataColumn column)
    {
      CurrentWarning = column.CurrentWarningText;
      WarningsType = TableWarningsType.ColumnWarnings;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TableWarningsChangedArgs"/> class.
    /// </summary>
    /// <param name="table">The <see cref="MySqlDataTable"/> object that contains changes in its warning texts.</param>
    /// <param name="autoPkWarning">Flag indicating if the warning is related to the auto-generated primary key or to the table.</param>
    public TableWarningsChangedArgs(MySqlDataTable table, bool autoPkWarning)
    {
      CurrentWarning = autoPkWarning ? table.CurrentAutoPkWarningText : table.CurrentTableWarningText;
      WarningsType = autoPkWarning ? TableWarningsType.AutoPrimaryKeyWarnings : TableWarningsType.TableNameWarnings;
    }

    /// <summary>
    /// Specifies identifiers to indicate the type of warnings that were updated.
    /// </summary>
    public enum TableWarningsType
    {
      /// <summary>
      /// Warnings belong to a table column.
      /// </summary>
      ColumnWarnings,

      /// <summary>
      /// Warnings belong to the table's auto-generated primary key.
      /// </summary>
      AutoPrimaryKeyWarnings,

      /// <summary>
      /// Warnings belong to the table name.
      /// </summary>
      TableNameWarnings
    }

    /// <summary>
    /// Gets the last warning text in the warnings collection.
    /// </summary>
    public string CurrentWarning { get; private set; }

    /// <summary>
    /// Gets the type of warnings that were updated.
    /// </summary>
    public TableWarningsType WarningsType { get; set; }
  }
}
