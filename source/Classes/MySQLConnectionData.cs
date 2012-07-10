using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace MySQL.ForExcel
{
  public enum DBObjectType { Routine = 0, Table = 1, View = 2 };
  public enum RoutineType { None, Procedure, Function };

  public class DBObject
  {
    public string Name;
    public DBObjectType Type;
    public RoutineType RoutineType;

    public DBObject()
    {
    }

    public DBObject(string name, DBObjectType type, RoutineType routineType)
    {
      Name = name;
      Type = type;
      RoutineType = routineType;
    }

    public DBObject(string name, DBObjectType type) : this(name, type, RoutineType.None)
    { }
  }

  public class MySQLConnectionData
  {
    private Guid id;
    private string stringId;

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
        MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
        sb.Server = HostName;
        sb.Port = Convert.ToUInt32(Port);
        sb.UserID = UserName;
        sb.Password = Password;
        return sb.ConnectionString;
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