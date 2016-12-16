// Copyright (c) 2016, Oracle and/or its affiliates. All rights reserved.
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

namespace MySQL.ForExcel.Classes
{
  public class MySqlDummyErroredRow : MySqlDummyRow
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDummyRow"/> class.
    /// </summary>
    /// <param name="dummyStatementText">The text that corresponds to a dummy statement.</param>
    /// <param name="errorMessage">The error message related to the dummy statement.</param>
    public MySqlDummyErroredRow(string dummyStatementText, string errorMessage)
      : base(dummyStatementText)
    {
      RowError = errorMessage;
      Statement = new MySqlStatement(this, errorMessage);
    }
  }
}
