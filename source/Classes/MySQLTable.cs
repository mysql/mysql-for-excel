using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using MySQL.Utility;
using MySql.Data.MySqlClient;

namespace MySQL.ForExcel
{
  public class MySQLTable
  {
    public MySqlWorkbenchConnection WBConnection { get; private set; }

    public MySQLTable(MySqlWorkbenchConnection wbConnection, DataRow tableRow, DataTable columnsTable)
    {
      WBConnection = wbConnection;
      Columns = new List<MySQLColumn>();
      Indexes = new List<MySQLIndex>();

      if (tableRow != null)
        parseTableData(tableRow);
      else
        _isNew = true;
      if (columnsTable != null)
        parseColumns(columnsTable);
    }

    private bool _isNew;
    [Browsable(false)]
    public bool IsNew
    {
      get { return _isNew; }
      private set { _isNew = value; }
    }

    private List<MySQLColumn> _columns;
    [Browsable(false)]
    public List<MySQLColumn> Columns
    {
      get { return _columns; }
      private set { _columns = value; }
    }

    private List<MySQLIndex> _indexes;
    [Browsable(false)]
    public List<MySQLIndex> Indexes
    {
      get { return _indexes; }
      private set { _indexes = value; }
    }

    [Browsable(false)]
    public MySQLIndex PrimaryKey
    {
      get { return _indexes.Find(idx => idx.IsPrimary); }
    }

    [Browsable(false)]
    public List<MySQLIndex> UniqueIndexes
    {
      get { return _indexes.FindAll(idx => idx.IsUnique); }
    }

    #region Table options

    private string _name;
    [Category("(Identity)")]
    [DescriptionAttribute("TableNameDesc")]
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    private string _schema;
    [Category("(Identity)")]
    [DescriptionAttribute("TableSchemaDesc")]
    public string Schema
    {
      get { return _schema; }
      private set { _schema = value; }
    }

    private string _comment;
    [DescriptionAttribute("TableCommentDesc")]
    public string Comment
    {
      get { return _comment; }
      set { _comment = value; }
    }

    private string _characterSet;
    [Category("Table Options")]
    [DisplayName("Character Set")]
    [TypeConverter(typeof(CharacterSetTypeConverter))]
    [RefreshProperties(RefreshProperties.All)]
    [DescriptionAttribute("TableCharSetDesc")]
    public string CharacterSet
    {
      get { return _characterSet; }
      set
      {
        if (value != _characterSet)
          Collation = String.Empty;
        _characterSet = value;
      }
    }

    private string _collation;
    [Category("Table Options")]
    [DisplayName("Collation")]
    [TypeConverter(typeof(CollationTypeConverter))]
    [DescriptionAttribute("TableCollationDesc")]
    public string Collation
    {
      get { return _collation; }
      set { _collation = value; }
    }

    private ulong _autoInc;
    [Category("Table")]
    [DisplayName("Auto Increment")]
    [DescriptionAttribute("TableAutoIncStartDesc")]
    public ulong AutoInc
    {
      get { return _autoInc; }
      set { _autoInc = value; }
    }

    #endregion
    #region Storage options

    private string _engine;
    [Category("Storage")]
    [DisplayName("Storage Engine")]
    [DescriptionAttribute("TableEngineDescription")]
    [TypeConverter(typeof(TableEngineTypeConverter))]
    [RefreshProperties(RefreshProperties.All)]
    public string Engine
    {
      get { return _engine; }
      set { _engine = value; }
    }

    #endregion
    #region ShouldSerializeMethods

    bool ShouldSerializeName() { return false; }
    bool ShouldSerializeSchema() { return false; }
    bool ShouldSerializeComment() { return false; }
    bool ShouldSerializeCharacterSet() { return false; }
    bool ShouldSerializeCollation() { return false; }
    bool ShouldSerializeAutoInc() { return false; }
    bool ShouldSerializeEngine() { return false; }
    bool ShouldSerializeDataDirectory() { return false; }
    bool ShouldSerializeIndexDirectory() { return false; }
    bool ShouldSerializeRowFormat() { return false; }
    bool ShouldSerializeCheckSum() { return false; }
    bool ShouldSerializeAvgRowLength() { return false; }
    bool ShouldSerializeMinRows() { return false; }
    bool ShouldSerializeMaxRows() { return false; }
    bool ShouldSerializePackKeys() { return false; }
    bool ShouldSerializeInsertMethod() { return false; }

    #endregion

    private void parseTableData(DataRow tableRow)
    {
      Schema = tableRow["TABLE_SCHEMA"].ToString();
      Name = tableRow["TABLE_NAME"].ToString();
      Engine = tableRow["ENGINE"].ToString();
      AutoInc = (tableRow["AUTO_INCREMENT"] == DBNull.Value ? 0 : Convert.ToUInt64(tableRow["AUTO_INCREMENT"]));
      Comment = tableRow["TABLE_COMMENT"].ToString();
      Collation = tableRow["TABLE_COLLATION"].ToString();
      if (Collation != null)
      {
        int index = Collation.IndexOf("_");
        if (index != -1)
          CharacterSet = Collation.Substring(0, index);
      }
      loadIndexes();
    }

    private void parseColumns(DataTable columnData)
    {
      foreach (DataRow row in columnData.Rows)
      {
        MySQLColumn c = new MySQLColumn(row, this);
        Columns.Add(c);
      }
    }

    private void loadIndexes()
    {
      string[] restrictions = new string[4] { null, WBConnection.Schema, Name, null };
      DataTable indexesTable = Utilities.GetSchemaCollection(WBConnection, "Indexes", restrictions);
      foreach (DataRow indexRow in indexesTable.Rows)
      {
        MySQLIndex index = new MySQLIndex(WBConnection, indexRow, this);
        Indexes.Add(index);
      }
    }

    public string GetSQL()
    {
      StringBuilder sql = new StringBuilder();
      sql.AppendFormat("CREATE TABLE `{0}` (", Name);

      string delimiter = "";
      foreach (MySQLColumn column in Columns)
      {
        sql.AppendFormat("{0}{1}", delimiter, column.GetSQL());
        delimiter = ", ";
      }

      sql.Append(")");
      sql.Append(GetTableOptions());
      return sql.ToString();
    }

    private string GetTableOptions()
    {
      List<string> options = new List<string>();
      StringBuilder sql = new StringBuilder(" ");

      if (_autoInc > 0)
        options.Add(String.Format("AUTO_INCREMENT={0}", _autoInc));
      if (!String.IsNullOrEmpty(_engine))
        options.Add(String.Format("ENGINE={0}", _engine));
      if (!String.IsNullOrEmpty(_comment))
        options.Add(String.Format("COMMENT='{0}'", _comment));
      options.Add(String.IsNullOrEmpty(_characterSet) ? "DEFAULT CHARACTER SET" : String.Format("CHARACTER SET='{0}'", _characterSet));
      options.Add(String.IsNullOrEmpty(_collation) ? "DEFAULT COLLATE" : String.Format("COLLATE='{0}'", _collation));

      string delimiter = "";
      foreach (string option in options)
      {
        sql.AppendFormat("{0}{1}", delimiter, option);
        delimiter = ",\r\n";
      }
      return sql.ToString();
    }

    public void createUpdateCommand(ref MySqlDataAdapter dataAdapter, ref MySqlConnection connection)
    {
      if (dataAdapter == null || connection == null)
        return;
      dataAdapter.UpdateCommand = new MySqlCommand(String.Empty, connection);
      StringBuilder queryString = new StringBuilder();
      StringBuilder wClauseString = new StringBuilder(" WHERE ");
      StringBuilder setClauseString = new StringBuilder();
      string wClause = String.Empty;
      MySqlParameter updateParam = null;

      string wClauseSeparator = String.Empty;
      string sClauseSeparator = String.Empty;
      queryString.AppendFormat("USE {0}; UPDATE", _schema);
      queryString.AppendFormat(" {0} SET ", _name);

      foreach (MySQLColumn mysqlCol in _columns)
      {
        bool isPrimaryKeyColumn = PrimaryKey != null && PrimaryKey.Columns.Any(idx => idx.ColumnName == mysqlCol.ColumnName);
        MySqlDbType mysqlColType = Utilities.NameToType(mysqlCol.DataType, mysqlCol.IsUnsigned, false);

        updateParam = new MySqlParameter(String.Format("@W_{0}", mysqlCol.ColumnName), mysqlColType);
        updateParam.SourceColumn = mysqlCol.ColumnName;
        updateParam.SourceVersion = DataRowVersion.Original;
        dataAdapter.UpdateCommand.Parameters.Add(updateParam);
        wClauseString.AppendFormat("{0}{1}=@W_{1}", wClauseSeparator, mysqlCol.ColumnName);

        if (!isPrimaryKeyColumn)
        {
          updateParam = new MySqlParameter(String.Format("@S_{0}", mysqlCol.ColumnName), mysqlColType);
          updateParam.SourceColumn = mysqlCol.ColumnName;
          dataAdapter.UpdateCommand.Parameters.Add(updateParam);
          setClauseString.AppendFormat("{0}{1}=@S_{1}", sClauseSeparator, mysqlCol.ColumnName);
        }
        wClauseSeparator = " AND ";
        sClauseSeparator = ",";
      }
      queryString.Append(setClauseString.ToString());
      queryString.Append(wClauseString.ToString());
      dataAdapter.UpdateCommand.CommandText = queryString.ToString();
    }

    public void createInsertCommand(ref MySqlDataAdapter dataAdapter, ref MySqlConnection connection)
    {
      if (dataAdapter == null || connection == null)
        return;
      dataAdapter.InsertCommand = new MySqlCommand(String.Empty, connection);
      StringBuilder queryString = new StringBuilder();
      StringBuilder wClauseString = new StringBuilder(" WHERE ");
      StringBuilder setClauseString = new StringBuilder();
      string wClause = String.Empty;
      MySqlParameter updateParam = null;

      string wClauseSeparator = String.Empty;
      string sClauseSeparator = String.Empty;
      queryString.AppendFormat("USE {0}; INSERT INTO {1} (", _schema, _name);

      foreach (MySQLColumn mysqlCol in _columns)
      {
        bool isPrimaryKeyColumn = PrimaryKey != null && PrimaryKey.Columns.Any(idx => idx.ColumnName == mysqlCol.ColumnName);
        MySqlDbType mysqlColType = Utilities.NameToType(mysqlCol.DataType, mysqlCol.IsUnsigned, false);

        updateParam = new MySqlParameter(String.Format("@W_{0}", mysqlCol.ColumnName), mysqlColType);
        updateParam.SourceColumn = mysqlCol.ColumnName;
        updateParam.SourceVersion = DataRowVersion.Original;
        dataAdapter.UpdateCommand.Parameters.Add(updateParam);
        wClauseString.AppendFormat("{0}{1}=@W_{1}", wClauseSeparator, mysqlCol.ColumnName);

        if (!isPrimaryKeyColumn)
        {
          updateParam = new MySqlParameter(String.Format("@S_{0}", mysqlCol.ColumnName), mysqlColType);
          updateParam.SourceColumn = mysqlCol.ColumnName;
          dataAdapter.UpdateCommand.Parameters.Add(updateParam);
          setClauseString.AppendFormat("{0}{1}=@S_{1}", sClauseSeparator, mysqlCol.ColumnName);
        }
        wClauseSeparator = " AND ";
        sClauseSeparator = ",";
      }
      queryString.Append(setClauseString.ToString());
      queryString.Append(wClauseString.ToString());
      dataAdapter.UpdateCommand.CommandText = queryString.ToString();
    }
  }

  public class CollationTypeConverter : StringConverter
  {
    private DataTable collationData;

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
      return true;
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
      return true;
    }

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
      MySQLTable table = ((context.Instance is MySQLTable) ? (context.Instance as MySQLTable) : (context.Instance as MySQLColumn).OwningTable);
      if (collationData == null)
        PopulateList(table);
      StandardValuesCollection coll = new StandardValuesCollection(GetRelevantCollations(context.Instance));
      return coll;
    }

    private List<string> GetRelevantCollations(object instance)
    {
      List<string> collations = new List<string>();
      string charset = String.Empty;
      if (instance is MySQLTable)
        charset = (instance as MySQLTable).CharacterSet;
      else
        charset = (instance as MySQLColumn).CharacterSet;
      if (String.IsNullOrEmpty(charset)) return collations;

      foreach (DataRow row in collationData.Rows)
        if (row["charset"].Equals(charset))
          collations.Add(row["collation"].ToString());
      return collations;
    }

    private void PopulateList(MySQLTable table)
    {
      collationData = Utilities.GetSchemaCollection(table.WBConnection, "COLLATIONS");
    }
  }

  public class CharacterSetTypeConverter : StringConverter
  {
    private List<string> charSets;

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
      return true;
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
      return true;
    }

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
      if (charSets == null)
        PopulateList(context.Instance);
      StandardValuesCollection coll = new StandardValuesCollection(charSets);
      return coll;
    }

    private void PopulateList(object instance)
    {
      MySQLTable table = ((instance is MySQLTable) ? (instance as MySQLTable) : (instance as MySQLColumn).OwningTable);
      DataTable data = Utilities.GetSchemaCollection(table.WBConnection, "CHARSETS");
      charSets = new List<string>();
      charSets.Add(String.Empty);
      foreach (DataRow row in data.Rows)
        charSets.Add(row["charset"].ToString());
    }
  }

  internal class TableEngineTypeConverter : StringConverter
  {
    private List<string> engineList;

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
      return true;
    }

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
      MySQLTable table = context.Instance as MySQLTable;

      if (engineList == null)
        PopulateList(table);
      StandardValuesCollection coll = new StandardValuesCollection(engineList);
      return coll;
    }

    private void PopulateList(MySQLTable table)
    {
      engineList = new List<string>();
      DataTable data = Utilities.GetSchemaCollection(table.WBConnection, "ENGINES");
      foreach (DataRow row in data.Rows)
      {
        if (!row[1].Equals("NO"))
          engineList.Add(row[0].ToString());
      }
    }
  }
}
