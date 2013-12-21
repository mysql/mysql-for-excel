// Copyright (c) 2013, Oracle and/or its affiliates. All rights reserved.
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
using System.Drawing;
using System.Xml.Serialization;
using Microsoft.Office.Interop.Excel;
using MySQL.ForExcel.Forms;
using Point = System.Drawing.Point;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// This class stores all the information required by an Edit Session to be stored in disk, able to be reopened if excel is closed and restarted without closing the session.
  /// </summary>
  [Serializable]
  public class EditSessionInfo
  {
    #region Fields

    private EditDataDialog _editDialog;

    #endregion Fields

    /// <summary>
    /// DO NOT REMOVE. Default constructor required for serialization-deserialization.
    /// </summary>
    public EditSessionInfo()
    {
      _editDialog = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditSessionInfo"/> class.
    /// </summary>
    /// <param name="workbookGuid">Guid of the workbook used by the edit session.</param>
    /// <param name="wbConnectionId">Workbench Connection information to open the edit session.</param>
    /// <param name="schema">Name of the Schema used by the edit session.</param>
    /// <param name="table">Name of the table used by the edit session.</param>
    /// <param name="workbookFilePath">The workbook full path name.</param>
    public EditSessionInfo(string workbookGuid, string wbConnectionId, string schema, string table, string workbookFilePath)
    {
      _editDialog = null;
      ConnectionId = wbConnectionId;
      SchemaName = schema;
      TableName = table;
      WorkbookGuid = workbookGuid;
      WorkbookFilePath = workbookFilePath;
    }

    #region Properties

    /// <summary>
    /// Gets or sets the connection information the session works with, contains credentials with remote access permissions to the reffered MySQL instance in it.
    /// </summary>
    [XmlAttribute]
    public string ConnectionId { get; set; }

    /// <summary>
    /// Gets or sets the active <see cref="EditDataDialog"/> object of an editing session.
    /// </summary>
    [XmlIgnore]
    public EditDataDialog EditDialog
    {
      get
      {
        return _editDialog;
      }
      set
      {
        _editDialog = value;
        if (_editDialog != null)
        {
          EditDialog.Closed -= EditDialog_Closed;
          EditDialog.Closed += EditDialog_Closed;
        }
      }
    }

    /// <summary>
    /// Gets or sets the name of the schema the connection works with.
    /// </summary>
    [XmlAttribute]
    public string SchemaName { get; set; }

    /// <summary>
    /// Gets or sets the table name the connection works with.
    /// </summary>
    [XmlAttribute]
    public string TableName { get; set; }

    /// <summary>
    /// Gets or sets the workbook guid on excel the session is making the edit.
    /// </summary>
    [XmlAttribute]
    public string WorkbookGuid { get; set; }

    /// <summary>
    /// Gets or sets the workbook full path name.
    /// </summary>
    [XmlAttribute]
    public string WorkbookFilePath { get; set; }

    #endregion Properties

    /// <summary>
    /// Determines whether this session has same workbook and table as the specified comparing session.
    /// </summary>
    /// <param name="comparingSession">The comparing session.</param>
    /// <returns><c>true</c> when this session has same workbook and table as the specified comparing session, <c>false</c> otherwise.</returns>
    public bool HasSameWorkbookAndTable(EditSessionInfo comparingSession)
    {
      return comparingSession != null && (WorkbookGuid.Equals(comparingSession.WorkbookGuid) && string.Equals(TableName, comparingSession.TableName, StringComparison.InvariantCulture));
    }

    /// <summary>
    /// Handles the Closed event of the EditDialog control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void EditDialog_Closed(object sender, EventArgs e)
    {
      if (_editDialog != null)
      {
        _editDialog.Closed -= EditDialog_Closed;
        _editDialog = null;
      }
    }
  }
}