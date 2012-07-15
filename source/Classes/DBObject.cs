using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace MySQL.ForExcel
{
  public enum DBObjectType { Table = 0, View = 1, Routine = 2 };
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

}