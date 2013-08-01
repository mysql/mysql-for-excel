// 
// Copyright (c) 2012-2013, Oracle and/or its affiliates. All rights reserved.
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
//

namespace MySQL.ForExcel
{
  using System;
  using System.Data;

  /// <summary>
  /// Represents an in-memory table containing a log of the results of queries executed against the database server.
  /// </summary>
  public class PushResultsDataTable : DataTable
  {
    private const string AFFECTED_ROWS_GREATHER_THAN_ZERO = "AffectedRows > 0";

    /// <summary>
    /// Initializes a new instance of the <see cref="PushResultsDataTable"/> class.
    /// </summary>
    public PushResultsDataTable()
    {
      Columns.Add("OperationIndex", Type.GetType("System.Int32"));
      Columns.Add("OperationType", Type.GetType("System.String"));
      Columns.Add("OperationResult", Type.GetType("System.String"));
      Columns.Add("QueryText", Type.GetType("System.String"));
      Columns.Add("ResultText", Type.GetType("System.String"));
      Columns.Add("AffectedRows", Type.GetType("System.Int32"));
    }

    /// <summary>
    /// Describes the type of operation done against the database server.
    /// </summary>
    public enum OperationType
    {
      /// <summary>
      /// Operation to prepare a connection and queries before they are sent to the database server for processing.
      /// </summary>
      Prepare,

      /// <summary>
      /// Operation to insert new rows into the corresponding database table.
      /// </summary>
      Insert,

      /// <summary>
      /// Operation to delete rows from the corresponding database table.
      /// </summary>
      Delete,

      /// <summary>
      /// Operation to update rows from the corresponding database table.
      /// </summary>
      Update
    }

    /// <summary>
    /// Describes the result of an operation done against the database server.
    /// </summary>
    public enum OperationResult
    {
      /// <summary>
      /// The operation performed was successful.
      /// </summary>
      Success,

      /// <summary>
      /// The operation performed did not complete due to errors thrown by the database server.
      /// </summary>
      Error,

      /// <summary>
      /// The operation performed was successful but the database server returned warnings.
      /// </summary>
      Warning
    }

    #region Properties

    /// <summary>
    /// Gets the number of delete operations successfully performed against the database server.
    /// </summary>
    public int DeletedOperations
    {
      get
      {
        return GetDeleteOperations(AFFECTED_ROWS_GREATHER_THAN_ZERO).Length;
      }
    }

    /// <summary>
    /// Gets the number of insert operations successfully performed against the database server.
    /// </summary>
    public int InsertedOperations
    {
      get
      {
        return GetInsertOperations(AFFECTED_ROWS_GREATHER_THAN_ZERO).Length;
      }
    }

    /// <summary>
    /// Gets the number of update operations successfully performed against the database server.
    /// </summary>
    public int UpdatedOperations
    {
      get
      {
        return GetUpdateOperations(AFFECTED_ROWS_GREATHER_THAN_ZERO).Length;
      }
    }

    #endregion Properties

    /// <summary>
    /// Gets the corresponding <see cref="OperationType"/> for a <see cref="DataRowState"/> enumeration.
    /// </summary>
    /// <param name="rowState">The state of a <see cref="DataRow"/> object.</param>
    /// <returns>Type of operation done against the database server</returns>
    public static OperationType GetRelatedOperationType(DataRowState rowState)
    {
      OperationType operationType = OperationType.Prepare;
      switch (rowState)
      {
        case DataRowState.Deleted:
          operationType = PushResultsDataTable.OperationType.Delete;
          break;

        case DataRowState.Added:
          operationType = PushResultsDataTable.OperationType.Insert;
          break;

        case DataRowState.Modified:
          operationType = PushResultsDataTable.OperationType.Update;
          break;
      }

      return operationType;
    }

    /// <summary>
    /// Adds a new database operation along with its type and result to the log table.
    /// </summary>
    /// <param name="operationIndex">Ordinal number for the database operation.</param>
    /// <param name="operationType">Type of operation done against the database server.</param>
    /// <param name="operationResult">Result of the database operation.</param>
    /// <param name="queryText">Query text of the database operation.</param>
    /// <param name="resultText">Result text returned by the database server for the database operation.</param>
    /// <param name="affectedRows">Number of rows affected by the database operation.</param>
    public void AddResult(int operationIndex, OperationType operationType, OperationResult operationResult, string queryText, string resultText, int affectedRows)
    {
      DataRow newRow = NewRow();
      newRow["OperationIndex"] = operationIndex;
      newRow["OperationType"] = operationType.ToString();
      newRow["OperationResult"] = operationResult.ToString();
      newRow["QueryText"] = queryText;
      newRow["ResultText"] = resultText;
      newRow["AffectedRows"] = affectedRows;
      Rows.Add(newRow);
    }

    /// <summary>
    /// Gets a list of rows containing delete operations performed against the database server and their results.
    /// </summary>
    /// <param name="extendedWClause">Additional criteria to search for operation result rows.</param>
    /// <returns>Array of <see cref="DataRow"/> objects containing operations and results.</returns>
    public DataRow[] GetDeleteOperations(string extendedWClause)
    {
      string filter = "OperationType = 'Delete'";
      if (!string.IsNullOrEmpty(extendedWClause))
      {
        filter += " AND " + extendedWClause;
      }

      return Select(filter);
    }

    /// <summary>
    /// Gets a list of rows containing delete operations performed against the database server and their results.
    /// </summary>
    /// <returns>Array of <see cref="DataRow"/> objects containing operations and results.</returns>
    public DataRow[] GetDeleteOperations()
    {
      return GetDeleteOperations(null);
    }

    /// <summary>
    /// Gets a list of rows containing insert operations performed against the database server and their results.
    /// </summary>
    /// <param name="extendedWClause">Additional criteria to search for operation result rows.</param>
    /// <returns>Array of <see cref="DataRow"/> objects containing operations and results.</returns>
    public DataRow[] GetInsertOperations(string extendedWClause)
    {
      string filter = "OperationType = 'Insert'";
      if (!string.IsNullOrEmpty(extendedWClause))
      {
        filter += " AND " + extendedWClause;
      }

      return Select(filter);
    }

    /// <summary>
    /// Gets a list of rows containing insert operations performed against the database server and their results.
    /// </summary>
    /// <returns>Array of <see cref="DataRow"/> objects containing operations and results.</returns>
    public DataRow[] GetInsertOperations()
    {
      return GetInsertOperations(null);
    }

    /// <summary>
    /// Gets a list of rows containing update operations performed against the database server and their results.
    /// </summary>
    /// <param name="extendedWClause">Additional criteria to search for operation result rows.</param>
    /// <returns>Array of <see cref="DataRow"/> objects containing operations and results.</returns>
    public DataRow[] GetUpdateOperations(string extendedWClause)
    {
      string filter = "OperationType = 'Update'";
      if (!string.IsNullOrEmpty(extendedWClause))
      {
        filter += " AND " + extendedWClause;
      }

      return Select(filter);
    }

    /// <summary>
    /// Gets a list of rows containing update operations performed against the database server and their results.
    /// </summary>
    /// <returns>Array of <see cref="DataRow"/> objects containing operations and results.</returns>
    public DataRow[] GetUpdateOperations()
    {
      return GetUpdateOperations(null);
    }
  }
}