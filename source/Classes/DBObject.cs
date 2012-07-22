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