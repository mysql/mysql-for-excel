// 
// Copyright (c) 2012, Oracle and/or its affiliates. All rights reserved.
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
