using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySQL.ForExcel.Properties;
using System.Windows.Forms;

namespace MySQL.ForExcel
{
  public class MySQLColumnMappingList
  {
    public List<MySQLColumnMapping> UserColumnMappingsList
    {
      get { return Settings.Default.StoredDataMappings; }
      set { Settings.Default.StoredDataMappings = value; }
    }

    public MySQLColumnMappingList()
    {
      if (UserColumnMappingsList == null)
        UserColumnMappingsList = new List<MySQLColumnMapping>();
    }

    public bool Add(MySQLColumnMapping mapping)
    {
      // any other initialization for mapping can be here
      UserColumnMappingsList.Add(mapping);
      return MiscUtilities.SaveSettings();      
    }

    public bool Remove(MySQLColumnMapping mapping)
    {
      try
      {
        // check if it really exists
        if (UserColumnMappingsList.Contains(mapping))
        {
          UserColumnMappingsList.Remove(mapping);
          return MiscUtilities.SaveSettings();          
        }
      }
      catch (Exception ex)
      {
        InfoDialog infoDialog = new InfoDialog(false, "Error when deleting Column Mapping", String.Format(@"Description Error: \""{0}\""", ex.Message));
        infoDialog.ShowDialog();
        return false;              
      }
      return false;
    }

    public bool Rename(MySQLColumnMapping mapping, string newName)
    {
      try
      {
        // check if it really exists
        if (UserColumnMappingsList.Contains(mapping))
        {
          UserColumnMappingsList.Single(t => t.Equals(mapping)).Name = newName;
          return MiscUtilities.SaveSettings();
        }
      }
      catch (Exception ex)
      {
        InfoDialog infoDialog = new InfoDialog(false, "Error when attempting to Rename a Column Mapping", String.Format(@"Description Error: \""{0}\""", ex.Message));
        infoDialog.ShowDialog();
        return false;     
      }
      return false;
    }

    public List<MySQLColumnMapping> GetMappingsByConnection(string connectionName, int port)
    {
      if (UserColumnMappingsList != null && !String.IsNullOrEmpty(connectionName))
        return UserColumnMappingsList.Where(t => t.ConnectionName.Equals(connectionName) && t.Port == port).ToList();
      return null;
    }

    public List<MySQLColumnMapping> GetMappingsByConnectionSchemaAndTable(string connectionName, int port, string schema, string tableName)
    {
      if (UserColumnMappingsList != null && !String.IsNullOrEmpty(connectionName))
        return UserColumnMappingsList.Where(t => t.ConnectionName.Equals(connectionName)
                                            && t.Port == port
                                            && t.SchemaName.Equals(schema)
                                            && t.TableName.Equals(tableName)).ToList();
      return null;
    }

  }
}
