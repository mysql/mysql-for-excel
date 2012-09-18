using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace MySQL.ForExcel
{
  public class PushResultsDataTable : DataTable
  {
    public enum OperationType { Prepare, Insert, Delete, Update };
    public enum OperationResult { Success, Error, Warning };

    public int InsertedOperations { get { return GetInsertOperations("AffectedRows > 0").Length; } }
    public int DeletedOperations { get { return GetDeleteOperations("AffectedRows > 0").Length; } }
    public int UpdatedOperations { get { return GetUpdateOperations("AffectedRows > 0").Length; } }

    public PushResultsDataTable()
    {
      Columns.Add("OperationIndex", Type.GetType("System.Int32"));
      Columns.Add("OperationType", Type.GetType("System.String"));
      Columns.Add("OperationResult", Type.GetType("System.String"));
      Columns.Add("QueryText", Type.GetType("System.String"));
      Columns.Add("ResultText", Type.GetType("System.String"));
      Columns.Add("AffectedRows", Type.GetType("System.Int32"));
    }

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

    public DataRow[] GetInsertOperations(string extendedWClause)
    {
      string filter = String.Format("OperationType = 'Insert'{0}{1}",
                                    (!String.IsNullOrEmpty(extendedWClause) ? " AND " : String.Empty),
                                    extendedWClause);
      return Select(filter);
    }

    public DataRow[] GetInsertOperations()
    {
      return GetInsertOperations(null);
    }

    public DataRow[] GetDeleteOperations(string extendedWClause)
    {
      string filter = String.Format("OperationType = 'Delete'{0}{1}",
                                    (!String.IsNullOrEmpty(extendedWClause) ? " AND " : String.Empty),
                                    extendedWClause);
      return Select(filter);
    }

    public DataRow[] GetDeleteOperations()
    {
      return GetDeleteOperations(null);
    }

    public DataRow[] GetUpdateOperations(string extendedWClause)
    {
      string filter = String.Format("OperationType = 'Update'{0}{1}",
                                    (!String.IsNullOrEmpty(extendedWClause) ? " AND " : String.Empty),
                                    extendedWClause);
      return Select(filter);
    }

    public DataRow[] GetUpdateOperations()
    {
      return GetUpdateOperations(null);
    }

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
  }
}
