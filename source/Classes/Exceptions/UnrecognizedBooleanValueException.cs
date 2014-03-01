// Copyright (c) 2014, Oracle and/or its affiliates. All rights reserved.
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
  class UnrecognizedBooleanValueException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="UnrecognizedBooleanValueException"/>.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public UnrecognizedBooleanValueException(string message, Exception innerException)
      : base(string.IsNullOrEmpty(message) ? Resources.UnrecognizedBooleanValueError : message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnrecognizedBooleanValueException"/>.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public UnrecognizedBooleanValueException(string message)
      : this(message, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnrecognizedBooleanValueException"/>.
    /// </summary>
    public UnrecognizedBooleanValueException()
      : this(null)
    {
    }
  }
}
