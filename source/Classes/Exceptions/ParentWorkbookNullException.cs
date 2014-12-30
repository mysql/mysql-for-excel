// Copyright (c) 2013-2015, Oracle and/or its affiliates. All rights reserved.
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
using MySQL.ForExcel.Properties;

namespace MySQL.ForExcel.Classes.Exceptions
{
  /// <summary>
  /// Represents an error of an attempt to get or set a parent <see cref="Microsoft.Office.Interop.Excel.Workbook"/> that is <c>null</c>.
  /// </summary>
  class ParentWorkbookNullException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ParentWorkbookNullException"/>.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public ParentWorkbookNullException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParentWorkbookNullException"/>.
    /// </summary>
    /// <param name="worksheetName">The name of the Excel worksheet which parent workbook can't be found.</param>
    public ParentWorkbookNullException(string worksheetName)
      : this(string.Format(Resources.ExcelTableCreationError, worksheetName), null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParentWorkbookNullException"/>.
    /// </summary>
    public ParentWorkbookNullException()
      : this(null)
    {
    }
  }
}
