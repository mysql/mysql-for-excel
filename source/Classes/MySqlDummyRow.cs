// Copyright (c) 2013, 2014, Oracle and/or its affiliates. All rights reserved.
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
using MySQL.ForExcel.Interfaces;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a dummy MySQL row only to hold an arbitrary SQL statement.
  /// </summary>
  public class MySqlDummyRow : IMySqlDataRow
  {
    #region Fields

    /// <summary>
    /// The SQL query needed to commit changes contained in this row to the SQL server.
    /// </summary>
    private string _sqlQuery;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDummyRow"/> class.
    /// </summary>
    /// <param name="sqlQuery">The SQL query needed to commit changes contained in this row to the SQL server.</param>
    public MySqlDummyRow(string sqlQuery)
    {
      _sqlQuery = sqlQuery;
      RowError = string.Empty;
      Statement = new MySqlStatement(this);
    }

    #region Properties

    /// <summary>
    /// Gets the related Excel row number if any.
    /// A valule of 0 indicates there is no related Excel row.
    /// </summary>
    public int ExcelRow
    {
      get
      {
        return 0;
      }
    }

    /// <summary>
    /// Gets a value indicating whether there are concurrency warnings in a row.
    /// </summary>
    public bool HasConcurrencyWarnings
    {
      get
      {
        return !string.IsNullOrEmpty(RowError) && string.Equals(RowError, MySqlStatement.NO_MATCH, StringComparison.InvariantCulture);
      }
    }

    /// <summary>
    /// Gets a value indicating whether there are errors in a row.
    /// </summary>
    public bool HasErrors
    {
      get
      {
        return !string.IsNullOrEmpty(RowError) && !string.Equals(RowError, MySqlStatement.NO_MATCH, StringComparison.InvariantCulture);
      }
    }

    /// <summary>
    /// Gets or sets the custom error description for a row.
    /// </summary>
    public string RowError { get; set; }

    /// <summary>
    /// Gets the <see cref="MySqlStatement"/> object containing a SQL query to push changes to the database.
    /// </summary>
    public MySqlStatement Statement { get; private set; }

    #endregion Properties

    /// <summary>
    /// Commits all the changes made to this row since the last time AcceptChanges was called.
    /// </summary>
    public void AcceptChanges()
    {
    }

    /// <summary>
    /// Clears the errors for the row set in <see cref="RowError"/>.
    /// </summary>
    public void ClearErrors()
    {
      RowError = string.Empty;
    }

    /// <summary>
    /// Returns a SQL query meant to push changes in this row to the database server.
    /// </summary>
    /// <param name="setVariablesSql">An optional SET statement to initialize variables used in the returned SQL query.</param>
    /// <returns>A SQL query containing the data changes.</returns>
    public string GetSql(out string setVariablesSql)
    {
      setVariablesSql = null;
      return _sqlQuery;
    }

    /// <summary>
    /// Reflects the error set to the row on its corresponding Excel range cells.
    /// </summary>
    public void ReflectError()
    {
    }

    /// <summary>
    /// Refreshes the row's data and reflects the changes on the <see cref="ExcelRow"/>.
    /// </summary>
    /// <param name="acceptChanges">Flag indicating whether the refreshed data is committed immediately to the row.</param>
    public void RefreshData(bool acceptChanges)
    {
    }
  }
}
