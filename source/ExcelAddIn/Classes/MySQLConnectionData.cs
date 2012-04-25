using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace MySQL.ExcelAddIn
{
  public enum DBObjectType { Table, View, Routine };

  public class DBObject
  {
    public string Name;
    public DBObjectType Type;

    public DBObject()
    {
    }

    public DBObject(string name, DBObjectType type)
    {
      Name = name;
      Type = type;
    }
  }

  public class MySQLConnectionData
  {
    private Guid id;
    private string stringId;
    private string connectionString = String.Empty;

    public string Name { get; set; }
    public Guid Id { get { return id; } }
    public string StringId { get { return stringId; } }
    public string HostName { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string HostIdentifier { get; set; }
    public string Password { get; set; }

    public string GluedConnection
    {
      get { return String.Format("User: {0}, IP: {1}", UserName, HostName); }
    }

    public string ConnectionString
    {
      get
      {
        if (connectionString == String.Empty)
        {
          MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
          sb.Server = HostName;
          sb.Port = Convert.ToUInt32(Port);
          sb.UserID = UserName;
          sb.Password = Password;
          connectionString = sb.ConnectionString;
        }
        return connectionString;
      }
    }

    public MySQLConnectionData(Guid id)
    {
      this.id = id;
      this.stringId = id.ToString();
      Password = null;
    }

  }

}