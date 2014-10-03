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

using MySQL.ForExcel.Classes;

namespace MySQL.ForExcel.Interfaces
{
  /// <summary>
  /// Represents a row of a MySQL table that contains a SQL statement to push its changes to the server.
  /// </summary>
  public interface IMySqlDataRow
  {
    /// <summary>
    /// Gets the related Excel row number if any.
    /// A valule of 0 indicates there is no related Excel row.
    /// </summary>
    int ExcelRow { get; }

    /// <summary>
    /// Gets a value indicating whether there are concurrency warnings in a row.
    /// </summary>
    bool HasConcurrencyWarnings { get; }

    /// <summary>
    /// Gets a value indicating whether there are errors in a row.
    /// </summary>
    bool HasErrors { get; }

    /// <summary>
    /// Gets or sets the custom error description for a row.
    /// </summary>
    string RowError { get; set; }

    /// <summary>
    /// Gets the <see cref="MySqlStatement"/> object containing a SQL query to push changes to the database.
    /// </summary>
    MySqlStatement Statement { get; }

    /// <summary>
    /// Commits all the changes made to this row since the last time AcceptChanges was called.
    /// </summary>
    void AcceptChanges();

    /// <summary>
    /// Clears the errors for the row. This includes the <see cref="RowError"/> and errors set with SetColumnError.
    /// </summary>
    void ClearErrors();

    /// <summary>
    /// Returns a SQL query meant to push changes in this row to the database server.
    /// </summary>
    /// <param name="setVariablesSql">An optional SET statement to initialize variables used in the returned SQL query.</param>
    /// <returns>A SQL query containing the data changes.</returns>
    string GetSql(out string setVariablesSql);

    /// <summary>
    /// Reflects the error set to the row on its corresponding Excel range cells.
    /// </summary>
    void ReflectError();

    /// <summary>
    /// Refreshes the row's data and reflects the changes on the <see cref="ExcelRow"/>.
    /// </summary>
    /// <param name="acceptChanges">Flag indicating whether the refreshed data is committed immediately to the row.</param>
    void RefreshData(bool acceptChanges);
  }
}
