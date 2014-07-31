// Copyright (c) 2012-2014, Oracle and/or its affiliates. All rights reserved.
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

using System;
using MySQL.Utility.Classes.MySQLWorkbench;
using ExcelInterop = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a MySQL database object that MySQL for Excel can interact with.
  /// </summary>
  public abstract class DbObject : IDisposable
  {
    #region Fields

    /// <summary>
    /// Flag indicating whether the <seealso cref="Dispose"/> method has already been called.
    /// </summary>
    protected bool Disposed;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="DbObject"/> class.
    /// </summary>
    /// <param name="connection">The MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="name">The name of the MySQL database object.</param>
    protected DbObject(MySqlWorkbenchConnection connection, string name)
    {
      Connection = connection;
      Disposed = false;
      Excluded = false;
      Name = name;
    }

    #region Properties

    /// <summary>
    /// Gets the MySQL Workbench connection to a MySQL server instance selected by users.
    /// </summary>
    public MySqlWorkbenchConnection Connection { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the object is excluded for further processing.
    /// </summary>
    public bool Excluded { get; set; }

    /// <summary>
    /// Gets the name of the MySQL database object.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the object is selected by the user or not.
    /// </summary>
    public bool Selected { get; set; }

    #endregion Properties

    /// <summary>
    /// Releases all resources used by the <see cref="TempRange"/> class
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases all resources used by the <see cref="DbObject"/> class
    /// </summary>
    /// <param name="disposing">If true this is called by Dispose(), otherwise it is called by the finalizer</param>
    protected virtual void Dispose(bool disposing)
    {
      if (Disposed)
      {
        return;
      }

      // Free managed resources
      if (disposing)
      {
        Connection = null;
      }

      // Add class finalizer if unmanaged resources are added to the class
      // Free unmanaged resources if there are any
      Disposed = true;
    }
  }
}