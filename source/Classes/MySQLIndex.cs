using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySQL.Utility;
using System.ComponentModel;
using System.Globalization;

namespace MySQL.ForExcel
{
  public class MySQLIndex
  {
    private List<IndexColumn> indexColumns = new List<IndexColumn>();
    private bool _isNew;
    private enum PropertyDescriptorStyles { Add, AddReadOnly, Skip };
    public MySqlWorkbenchConnection WBConnection { get; private set; }

    public MySQLIndex(MySqlWorkbenchConnection wbConnection, DataRow indexData, MySQLTable table)
    {
      _isNew = indexData == null;
      WBConnection = wbConnection;
      OwningTable = table;
      if (!_isNew)
        ParseIndexInfo(indexData);
    }

    private void ParseIndexInfo(DataRow indexData)
    {
      Name = indexData["INDEX_NAME"].ToString();
      IsPrimary = (bool)indexData["PRIMARY"];
      IsUnique = (bool)indexData["UNIQUE"] || IsPrimary;
      Comment = indexData["COMMENT"].ToString();
      string type = indexData["TYPE"].ToString();
      switch (type)
      {
        case "BTREE": IndexUsing = IndexUsingType.BTREE; break;
        case "RTREE": IndexUsing = IndexUsingType.RTREE; break;
        case "HASH": IndexUsing = IndexUsingType.HASH; break;
      }
      FullText = type == "FULLTEXT";
      Spatial = type == "SPATIAL";

      string[] restrictions = new string[5] { null, WBConnection.Schema, OwningTable.Name, this.Name, null };
      DataTable indexColumnsInfoTable = Utilities.GetSchemaCollection(WBConnection, "IndexColumns", restrictions);
      foreach (DataRow indexColumnRow in indexColumnsInfoTable.Rows)
      {
        IndexColumn indexCol = new IndexColumn();
        indexCol.OwningIndex = this;
        indexCol.ColumnName = indexColumnRow["COLUMN_NAME"].ToString();
        string sortOrder = indexColumnRow["SORT_ORDER"].ToString();
        indexCol.SortOrder = IndexSortOrder.Ascending;
        Columns.Add(indexCol);
      }

      if (IsPrimary)
        Type = IndexType.Primary;
    }

    #region Properties

    [Browsable(false)]
    public MySQLTable OwningTable;

    private string _name;
    [Category("Identity")]
    [DisplayName("(Name)")]
    [Description("The name of this index/key")]
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    private string _comment;
    [Category("Identity")]
    [Description("A description or comment about this index/key")]
    public string Comment
    {
      get { return _comment; }
      set { _comment = value; }
    }

    [Category("(General)")]
    [Description("The columns of this index/key and their associated sort order")]
    [TypeConverter(typeof(IndexColumnTypeConverter))]
    public List<IndexColumn> Columns
    {
      get { return indexColumns; }
    }

    private IndexType _indexType;
    [Category("(General)")]
    [Description("Specifies if this object is an index or primary key")]
    public IndexType Type
    {
      get { return _indexType; }
      set
      { 
        _indexType = value;
        _isPrimary = (_indexType == IndexType.Primary);
      }
    }

    private bool _isUnique;
    [Category("(General)")]
    [DisplayName("Is Unique")]
    [Description("Specifies if this index/key uniquely identifies every row")]
    [TypeConverter(typeof(YesNoTypeConverter))]
    public bool IsUnique
    {
      get { return _isUnique; }
      set { _isUnique = value; }
    }

    private bool _isPrimary;
    [Browsable(false)]
    public bool IsPrimary
    {
      get { return _isPrimary; }
      set { _isPrimary = value; }
    }

    [Browsable(false)]
    public bool IsNew
    {
      get { return _isNew; }
      private set { _isNew = value; }
    }

    private IndexUsingType _indexUsing;
    [Category("Storage")]
    [DisplayName("Index Algorithm")]
    [Description("Specifies the algorithm that should be used for storing the index/key")]
    public IndexUsingType IndexUsing
    {
      get { return _indexUsing; }
      set { _indexUsing = value; }
    }

    private int _keyBlockSize;
    [Category("Storage")]
    [DisplayName("Key Block Size")]
    [Description("Suggested size in bytes to use for index key blocks.  A zero value means to use the storage engine default.")]
    public int KeyBlockSize
    {
      get { return _keyBlockSize; }
      set { _keyBlockSize = value; }
    }

    private string _parser;
    [Description("Specifies a parser plugin to be used for this index/key.  This is only valid for full-text indexes or keys.")]
    public string Parser
    {
      get { return _parser; }
      set { _parser = value; }
    }

    private bool _fullText;
    [DisplayName("Is Full-text Index/Key")]
    [Description("Specifies if this is a full-text index or key.  This is only supported on MyISAM tables.")]
    [TypeConverter(typeof(YesNoTypeConverter))]
    [RefreshProperties(RefreshProperties.All)]
    public bool FullText
    {
      get { return _fullText; }
      set { _fullText = value; }
    }

    private bool _spatial;
    [DisplayName("Is Spatial Index/Key")]
    [Description("Specifies if this is a spatial index or key.  This is only supported on MyISAM tables.")]
    [TypeConverter(typeof(YesNoTypeConverter))]
    [RefreshProperties(RefreshProperties.All)]
    public bool Spatial
    {
      get { return _spatial; }
      set { _spatial = value; }
    }

    #endregion
    #region ShouldSerialize

    bool ShouldSerializeName() { return false; }
    bool ShouldSerializeComment() { return false; }
    bool ShouldSerializeColumns() { return false; }
    bool ShouldSerializeType() { return false; }
    bool ShouldSerializeIsUnique() { return false; }
    bool ShouldSerializeIndexUsing() { return false; }
    bool ShouldSerializeKeyBlockSize() { return false; }
    bool ShouldSerializeParser() { return false; }
    bool ShouldSerializeFullText() { return false; }
    bool ShouldSerializeSpatial() { return false; }

    #endregion
    #region ICustomTypeDescriptor Members

    public TypeConverter GetConverter()
    {
      return TypeDescriptor.GetConverter(this, true);
    }

    public EventDescriptorCollection GetEvents(Attribute[] attributes)
    {
      return TypeDescriptor.GetEvents(this, attributes, true);
    }

    public string GetComponentName()
    {
      return TypeDescriptor.GetComponentName(this, true);
    }

    public object GetPropertyOwner(PropertyDescriptor pd)
    {
      return this;
    }

    public AttributeCollection GetAttributes()
    {
      return TypeDescriptor.GetAttributes(this, true);
    }

    public object GetEditor(Type editorBaseType)
    {
      return TypeDescriptor.GetEditor(this, editorBaseType, true);
    }

    public PropertyDescriptor GetDefaultProperty()
    {
      return TypeDescriptor.GetDefaultProperty(this, true);
    }

    public EventDescriptor GetDefaultEvent()
    {
      return TypeDescriptor.GetDefaultEvent(this, true);
    }

    public string GetClassName()
    {
      return TypeDescriptor.GetClassName(this, true);
    }

    #endregion
  }

  public enum IndexType
  {
    Index, Primary
  }

  public enum IndexUsingType
  {
    BTREE, HASH, RTREE
  }

  public enum IndexSortOrder
  {
    Ascending, Descending
  }

  public class IndexColumn
  {
    public MySQLIndex OwningIndex;
    public string ColumnName;
    public IndexSortOrder SortOrder;
  }

  public class IndexColumnTypeConverter : TypeConverter
  {
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      if (destinationType == typeof(String))
      {
        StringBuilder str = new StringBuilder();
        List<IndexColumn> cols = (value as List<IndexColumn>);
        string separator = String.Empty;
        foreach (IndexColumn ic in cols)
        {
          str.AppendFormat("{2}{0} ({1})", ic.ColumnName, ic.SortOrder == IndexSortOrder.Ascending ? "ASC" : "DESC", separator);
          separator = ",";
        }
        return str.ToString();
      }
      return base.ConvertTo(context, culture, value, destinationType);
    }
  }
}
