// Copyright (c) 2012-2015, Oracle and/or its affiliates. All rights reserved.
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
using System.Configuration;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using MySQL.ForExcel.Interfaces;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;
using ExcelInterop = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Provides extension methods and other static methods to leverage miscelaneous tasks.
  /// </summary>
  public static class MiscUtilities
  {
    /// <summary>
    /// The default capacity to initialize a <see cref="StringBuilder"/> instance.
    /// </summary>
    public const int STRING_BUILDER_DEFAULT_CAPACITY = 255;

    /// <summary>
    /// Checks if the given <see cref="MySqlStatement.SqlStatementType"/> affects rows in the database.
    /// </summary>
    /// <param name="statementType">A <see cref="MySqlStatement.SqlStatementType"/> value.</param>
    /// <returns><c>true</c> if the <see cref="MySqlStatement.SqlStatementType"/> affects rows in the database, <c>false</c> otherwise.</returns>
    public static bool AffectsRowsOnServer(this MySqlStatement.SqlStatementType statementType)
    {
      return statementType == MySqlStatement.SqlStatementType.Delete ||
             statementType == MySqlStatement.SqlStatementType.Insert ||
             statementType == MySqlStatement.SqlStatementType.Update;
    }

    /// <summary>
    /// Adds new lines to the <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder"/> to add new lines to.</param>
    /// <param name="quantity">The quantity of new lines to add, adds only 1 if the parameter is not specified.</param>
    /// <param name="onlyIfNotEmpty">Flag indicating if the new lines are only added if the string builder is not empty.</param>
    public static void AddNewLine(this StringBuilder stringBuilder, int quantity = 1, bool onlyIfNotEmpty = false)
    {
      if (stringBuilder == null || (onlyIfNotEmpty && stringBuilder.Length == 0))
      {
        return;
      }

      for (int index = 1; index <= quantity; index++)
      {
        stringBuilder.Append(Environment.NewLine);
      }
    }

    /// <summary>
    /// Adds new lines to the <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder"/> to add new lines to.</param>
    /// <param name="separator">The separator text.</param>
    /// <param name="onlyIfNotEmpty">Flag indicating if the separator is only added if the string builder is not empty.</param>
    public static void AddSeparator(this StringBuilder stringBuilder, string separator, bool onlyIfNotEmpty = false)
    {
      if (stringBuilder == null || (onlyIfNotEmpty && stringBuilder.Length == 0))
      {
        return;
      }

      stringBuilder.Append(separator);
    }

    /// <summary>
    /// Checks that the given string is correctly wrapped in single quotes and that other single quotes inside are properly escaped in MySQL notation.
    /// </summary>
    /// <param name="element">An array of strings.</param>
    /// <returns><c>false</c> if the element has an incorrect escaping of single quotes or not wrapped in single quotes correctly, <c>true</c> if correct.</returns>
    public static bool CheckForCorrectSingleQuoting(this string element)
    {
      if (string.IsNullOrEmpty(element))
      {
        return false;
      }

      // Check for wrapping single quotes.
      if (!element.StartsWith("'") || !element.EndsWith("'"))
      {
        return false;
      }

      // Strip the element from its wrapping quotes.
      element = element.Trim(new[] { '\'' });

      // Check that each found single quote is properly wrapped in MySQL notation, i.e. that 2 consecutive single quotes appear where a single quote is expected to be in the text.
      int currentQuotePos;
      int previousQuotePos = 0;
      bool nonEscapedQuoteFound = false;
      while ((currentQuotePos = element.IndexOf('\'', previousQuotePos + 1)) >= 0)
      {
        // If a single quote was previously found, check if this new one is just next to it (meaning the previous one is escaping the current one)
        // and if not break since a new single quote was found but not next to the previous one, meaning none of them are escaped.
        if (nonEscapedQuoteFound && currentQuotePos > previousQuotePos + 1)
        {
          break;
        }

        // Update the flag and position. We can just flip the flag, if the code did not break above it means the current quote is just next to the previous one, meaning a correct escape.
        nonEscapedQuoteFound = !nonEscapedQuoteFound;
        previousQuotePos = currentQuotePos;
      }

      return !nonEscapedQuoteFound;
    }

    /// <summary>
    /// Checks that each element of a list of strings is correctly wrapped in single quotes and that other single quotes inside are properly escaped in MySQL notation.
    /// </summary>
    /// <param name="elementsList">A list of strings.</param>
    /// <returns>The indexes of the elements of the list with an incorrect escaping of single quotes, or not wrapped in single quotes correctly.</returns>
    public static List<int> CheckForCorrectSingleQuoting(this List<string> elementsList)
    {
      if (elementsList == null || elementsList.Count == 0)
      {
        return null;
      }

      var indexesList = new List<int>(elementsList.Count);
      for (int elementIndex = 0; elementIndex < elementsList.Count; elementIndex++)
      {
        if (elementsList[elementIndex].CheckForCorrectSingleQuoting())
        {
          continue;
        }

        indexesList.Add(elementIndex);
      }

      return indexesList;
    }

    /// <summary>
    /// Escapes a text that starts with an equals sign so Excel does not treat it as a formula.
    /// </summary>
    /// <param name="text">A string.</param>
    /// <returns>The text escaped with an apostrophe in case it starts with an equals sign.</returns>
    public static string EscapeStartingEqualSign(this string text)
    {
      if (string.IsNullOrEmpty(text))
      {
        return text;
      }

      // If the text starts with an equals sign Excel will treat it as a formula so it needs to be escaped prepending an apostrophe to it for Excel to treat it as standard text.
      return text.StartsWith("=") ? "'" + text : text;
    }

    /// <summary>
    /// Escapes a boxed text that starts with an equals sign so Excel does not treat it as a formula.
    /// </summary>
    /// <param name="possibleBoxedText">An object, that if is a <see cref="string"/> then it is escaped, otherwise just returned.</param>
    /// <returns>An object with text escaped with an apostrophe in case it starts with an equals sign.</returns>
    public static object EscapeStartingEqualSign(this object possibleBoxedText)
    {
      if (!(possibleBoxedText is string))
      {
        return possibleBoxedText;
      }

      return (possibleBoxedText as string).EscapeStartingEqualSign();
    }

    /// <summary>
    /// Gets the active <see cref="EditConnectionInfo"/> related to a given <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="connectionInfosList">The <see cref="EditConnectionInfo"/> objects list.</param>
    /// <param name="workbook">The <see cref="ExcelInterop.Workbook"/> related to the active <see cref="EditConnectionInfo"/>.</param>
    /// <param name="tableName">Name of the table being edited in the <see cref="EditConnectionInfo"/>.</param>
    /// <returns>An <see cref="EditConnectionInfo"/> object.</returns>
    public static EditConnectionInfo GetActiveEditConnectionInfo(this List<EditConnectionInfo> connectionInfosList, ExcelInterop.Workbook workbook, string tableName)
    {
      var workBookId = workbook.GetOrCreateId();
      return connectionInfosList == null ? null : connectionInfosList.FirstOrDefault(connectionInfo => connectionInfo.EditDialog != null &&
      string.Equals(connectionInfo.WorkbookGuid, workBookId, StringComparison.InvariantCulture) &&
      connectionInfo.TableName == tableName);
    }

    /// <summary>
    /// Gets the active <see cref="EditConnectionInfo"/> object related to a given <see cref="ExcelInterop.Worksheet"/>.
    /// </summary>
    /// <param name="connectionInfosList">The <see cref="EditConnectionInfo"/> objects list.</param>
    /// <param name="worksheet">The <see cref="ExcelInterop.Worksheet"/> related to the active <see cref="EditConnectionInfo"/> object.</param>
    /// <returns>An <see cref="EditConnectionInfo"/> object.</returns>
    public static EditConnectionInfo GetActiveEditConnectionInfo(this List<EditConnectionInfo> connectionInfosList, ExcelInterop.Worksheet worksheet)
    {
      return connectionInfosList == null ? null : connectionInfosList.FirstOrDefault(connectionInfo => connectionInfo.EditDialog != null && connectionInfo.EditDialog.EditingWorksheet.Name == worksheet.Name);
    }

    /// <summary>
    /// Gets a formatted string with the messages in the given <see cref="Exception"/> and its <see cref="Exception.InnerException"/> if it exists.
    /// </summary>
    /// <param name="exception">An <see cref="Exception"/> object.</param>
    /// <returns>A formatted string with the messages in the given <see cref="Exception"/> and its <see cref="Exception.InnerException"/> if it exists.</returns>
    public static string GetFormattedMessage(this Exception exception)
    {
      if (exception == null)
      {
        return string.Empty;
      }

      var formattedMessagesBuilder = new StringBuilder(exception.Message);
      if (exception.InnerException != null)
      {
        formattedMessagesBuilder.Append(Environment.NewLine);
        formattedMessagesBuilder.Append(Environment.NewLine);
        formattedMessagesBuilder.Append(exception.InnerException.Message);
      }

      return formattedMessagesBuilder.ToString();
    }

    /// <summary>
    /// Gets the owner <see cref="ListView"/> of a <see cref="ContextMenuStrip"/> control.
    /// </summary>
    /// <param name="toolStripMenuControl">An boxed object containing a <see cref="ContextMenuStrip"/> or <see cref="ToolStripMenuItem"/> control.</param>
    /// <returns>The owner <see cref="ListView"/> of a <see cref="ContextMenuStrip"/> control.</returns>
    public static ListView GetOwnerListViewControl(object toolStripMenuControl)
    {
      ContextMenuStrip ownerMenuStrip = null;
      if (toolStripMenuControl is ToolStripMenuItem)
      {
        var menuItem = toolStripMenuControl as ToolStripMenuItem;
        ownerMenuStrip = menuItem.Owner as ContextMenuStrip;
        if (ownerMenuStrip == null)
        {
          return null;
        }
      }
      else if (toolStripMenuControl is ContextMenuStrip)
      {
        ownerMenuStrip = toolStripMenuControl as ContextMenuStrip;
      }

      if (ownerMenuStrip == null)
      {
        return null;
      }

      var listView = ownerMenuStrip.SourceControl as ListView;
      return listView;
    }

    /// <summary>
    /// Gets the default property value by property name.
    /// </summary>
    /// <typeparam name="T">Type to which the property must be cast to in the end.</typeparam>
    /// <param name="settings">The application settings.</param>
    /// <param name="propertyName">Name of the property we want to get the default value from.</param>
    /// <returns></returns>
    public static T GetPropertyDefaultValueByName<T>(this ApplicationSettingsBase settings, string propertyName)
    {
      var settingsProperty = settings.Properties[propertyName];
      var propertyInfo = settings.GetType().GetProperties().FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.InvariantCulture));
      if (propertyInfo == null || settingsProperty == null)
      {
        return default(T);
      }

      return (T)Convert.ChangeType(settingsProperty.DefaultValue, propertyInfo.PropertyType);
    }

    /// <summary>
    /// Gets the <see cref="DbView"/> object representing a selected table or view in the <see cref="ListView"/> control from which the given <see cref="ContextMenuStrip"/> or <see cref="ToolStripMenuItem"/> control is opened.
    /// </summary>
    /// <param name="toolStripMenuControl">A boxed object with a <see cref="ContextMenuStrip"/> or <see cref="ToolStripMenuItem"/> control.</param>
    /// <param name="listView">The <see cref="ListView"/> control from which the given <see cref="ContextMenuStrip"/> or <see cref="ToolStripMenuItem"/> control is opened.</param>
    /// <returns>A <see cref="DbView"/> object representing a selected table or view.</returns>
    public static DbView GetSelectedDbTableOrView(object toolStripMenuControl, out ListView listView)
    {
      listView = GetOwnerListViewControl(toolStripMenuControl);
      if (listView == null)
      {
        return null;
      }

      if (listView.SelectedItems.Count <= 0)
      {
        return null;
      }

      return listView.SelectedItems[0].Tag as DbView;
    }

    /// <summary>
    /// Gets the <see cref="DbView"/> object representing a selected table or view in the <see cref="ListView"/> control from which the given <see cref="ContextMenuStrip"/> or <see cref="ToolStripMenuItem"/> control is opened.
    /// </summary>
    /// <param name="toolStripMenuControl">A boxed object with a <see cref="ContextMenuStrip"/> or <see cref="ToolStripMenuItem"/> control.</param>
    /// <returns>A <see cref="DbView"/> object representing a selected table or view.</returns>
    public static DbView GetSelectedDbTableOrView(object toolStripMenuControl)
    {
      ListView listView;
      return GetSelectedDbTableOrView(toolStripMenuControl, out listView);
    }

    /// <summary>
    /// Determines whether this IConnectionInfo has same workbook and table as the specified comparing IConnectionInfo object.
    /// </summary>
    /// <param name="connectionInfo">The current IConnectionInfo.</param>
    /// <param name="comparingConnectionInfo">The IConnectionInfo we want to compare values with.</param>
    /// <returns><c>true</c> when this IConnectionInfo has same workbook and table as the specified comparing IConnectionInfo object, <c>false</c> otherwise.</returns>
    public static bool HasSameWorkbookAndTable(this IConnectionInfo connectionInfo, IConnectionInfo comparingConnectionInfo)
    {
      return connectionInfo != null && !string.IsNullOrEmpty(connectionInfo.WorkbookGuid) && !string.IsNullOrEmpty(comparingConnectionInfo.WorkbookGuid)
      && string.Equals(connectionInfo.WorkbookGuid, comparingConnectionInfo.WorkbookGuid, StringComparison.InvariantCulture)
      && !string.IsNullOrEmpty(connectionInfo.TableName) && !string.IsNullOrEmpty(comparingConnectionInfo.TableName)
      && string.Equals(connectionInfo.TableName, comparingConnectionInfo.TableName, StringComparison.InvariantCulture);
    }

    /// <summary>
    /// Returns the position of a given integer number within an array of integers.
    /// </summary>
    /// <param name="intArray">The array of integers to look for the given number.</param>
    /// <param name="intElement">The integer to look for in the list.</param>
    /// <returns>The ordinal position of the given number within the list, or <c>-1</c> if not found.</returns>
    public static int IndexOfIntInArray(int[] intArray, int intElement)
    {
      if (intArray == null)
      {
        return -1;
      }

      int index = -1;
      for (int i = 0; i < intArray.Length; i++)
      {
        if (intArray[i] != intElement)
        {
          continue;
        }

        index = i;
        break;
      }

      return index;
    }

    /// <summary>
    /// Returns the position of a given string number within an array of strings.
    /// </summary>
    /// <param name="stringArray">The array of strings to look for the given string.</param>
    /// <param name="stringElement">The string to look for in the list.</param>
    /// <param name="caseSensitive">Flag indicating whether the search is performed in a case sensitive way.</param>
    /// <returns>The ordinal position of the given string within the list, or <c>-1</c> if not found.</returns>
    public static int IndexOfStringInArray(string[] stringArray, string stringElement, bool caseSensitive)
    {
      if (stringArray == null)
      {
        return -1;
      }

      if (!caseSensitive)
      {
        stringElement = stringElement.ToLowerInvariant();
      }

      int index = -1;
      for (int i = 0; i < stringArray.Length; i++)
      {
        if (stringElement != (caseSensitive ? stringArray[i] : stringArray[i].ToLowerInvariant()))
        {
          continue;
        }

        index = i;
        break;
      }

      return index;
    }

    /// <summary>
    /// Resets the settings that correspond to the defined section to its default values.
    /// </summary>
    /// <param name="settings">The application defualt settings (extension method)</param>
    /// <param name="section">The section type</param>
    public static void ResetSectionToDefaultValues(this ApplicationSettingsBase settings, PropertyGroup.SettingsGroup section)
    {
      foreach (var propertyInfo in settings.GetType().GetProperties())
      {
        var att = propertyInfo.GetCustomAttributes(typeof(PropertyGroup), true).FirstOrDefault();
        if (att == null || ((PropertyGroup)att).Value != section)
        {
          continue;
        }

        var settingsProperty = settings.Properties[propertyInfo.Name];
        if (settingsProperty == null)
        {
          continue;
        }

        propertyInfo.SetValue(settings, Convert.ChangeType(settingsProperty.DefaultValue, propertyInfo.PropertyType), null);
      }
    }

    /// <summary>
    /// Attempts to save settings values into the settings file.
    /// </summary>
    /// <returns><c>true</c> if the settings file was saved successfully, <c>false</c> otherwise.</returns>
    public static bool SaveSettings()
    {
      string errorMessage = null;

      // Attempt to save the settings file up to 3 times, if not successful show an error message to users.
      for (int i = 0; i < 3; i++)
      {
        try
        {
          Settings.Default.Save();
          errorMessage = null;
        }
        catch (Exception ex)
        {
          MySqlSourceTrace.WriteAppErrorToLog(ex);
          errorMessage = ex.Message;
        }
      }

      if (!string.IsNullOrEmpty(errorMessage))
      {
        ShowCustomizedErrorDialog(Resources.SettingsFileSaveErrorTitle, errorMessage);
      }

      return errorMessage == null;
    }

    /// <summary>
    /// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for
    /// the specified window and does not return until the window procedure has processed the message.
    /// </summary>
    /// <param name="hWnd">A handle to the window whose window procedure will receive the message.
    /// If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system,
    /// including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
    /// <param name="msg">The message to be sent.</param>
    /// <param name="wParam">Additional message-specific information.</param>
    /// <param name="lParam">Additional message-specific information.</param>
    /// <returns>Specifies the result of the message processing; it depends on the message sent.</returns>
    [DllImport("user32.dll")]
    public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// Initializes the contents of a <see cref="ComboBox"/> with character sets and their corresponding collations.
    /// </summary>
    /// <param name="comboBox">The <see cref="ComboBox"/> to initialize.</param>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="firstElement">A custom string for the first element of the dictioary.</param>
    public static void SetupCollations(this ComboBox comboBox, MySqlWorkbenchConnection connection, string firstElement)
    {
      if (comboBox == null)
      {
        return;
      }

      var collationsDictionary = connection.GetCollationsDictionary(firstElement);
      if (collationsDictionary == null)
      {
        return;
      }

      comboBox.DataSource = new BindingSource(collationsDictionary, null);
      comboBox.DisplayMember = "Value";
      comboBox.ValueMember = "Key";
    }

    /// <summary>
    /// Shows an error dialog customized for MySQL for Excel.
    /// </summary>
    /// <param name="detail">The text describing information details to the users.</param>
    /// <param name="moreInformation">The extended text users can see in the More Information text box.</param>
    /// <param name="wordWrapMoreInfo">Indicates if the More Information text box word wraps the text.</param>
    /// <returns>A dialog result with the user's selection.</returns>
    public static void ShowCustomizedErrorDialog(string detail, string moreInformation = null, bool wordWrapMoreInfo = false)
    {
      ShowCustomizedInfoDialog(InfoDialog.InfoType.Error, detail, moreInformation, wordWrapMoreInfo);
    }

    /// <summary>
    /// Shows a <see cref="InfoDialog"/> dialog customized for MySQL for Excel, only an OK/Back button is displayed to users.
    /// </summary>
    /// <param name="infoType">The type of information the dialog will display to users.</param>
    /// <param name="detail">The text describing information details to the users.</param>
    /// <param name="moreInformation">The extended text users can see in the More Information text box.</param>
    /// <param name="wordWrapMoreInfo">Indicates if the More Information text box word wraps the text.</param>
    /// <returns>A dialog result with the user's selection.</returns>
    public static DialogResult ShowCustomizedInfoDialog(InfoDialog.InfoType infoType, string detail, string moreInformation = null, bool wordWrapMoreInfo = true)
    {
      string title = string.Empty;
      var layoutType = InfoButtonsProperties.ButtonsLayoutType.OkOnly;
      switch (infoType)
      {
        case InfoDialog.InfoType.Success:
          title = Resources.OperationSuccessTitle;
          break;

        case InfoDialog.InfoType.Warning:
          title = Resources.OperationWarningTitle;
          break;

        case InfoDialog.InfoType.Error:
          title = Resources.OperationErrorTitle;
          layoutType = InfoButtonsProperties.ButtonsLayoutType.BackOnly;
          break;

        case InfoDialog.InfoType.Info:
          title = Resources.OperationInformationTitle;
          break;
      }

      string subDetailText = string.Format(Resources.OperationSubDetailText, infoType == InfoDialog.InfoType.Error ? "Back" : "OK");
      var infoProperties = new InfoDialogProperties
      {
        ButtonsProperties = new InfoButtonsProperties(layoutType),
        InfoType = infoType,
        TitleText =  title,
        DetailText = detail,
        DetailSubText = subDetailText,
        MoreInfoText = moreInformation,
        WordWrapMoreInfo = wordWrapMoreInfo
      };
      return InfoDialog.ShowDialog(infoProperties).DialogResult;
    }

    /// <summary>
    /// Shows a warning dialog customized for MySQL for Excel showing Yes/No buttons.
    /// </summary>
    /// <param name="title">The main short title of the warning.</param>
    /// <param name="detail">The detail text describing further the warning.</param>
    /// <returns>A dialog result with the user's selection.</returns>
    public static DialogResult ShowCustomizedWarningDialog(string title, string detail)
    {
      return InfoDialog.ShowDialog(InfoDialogProperties.GetYesNoDialogProperties(InfoDialog.InfoType.Warning, title, detail)).DialogResult;
    }

    /// <summary>
    /// Splits the given text containing a SQL script into individual SQL statements.
    /// </summary>
    /// <param name="sqlScript">A string containing a SQL script.</param>
    /// <returns>A list of individual SQL statements.</returns>
    public static List<string> SplitInSqlStatements(this string sqlScript)
    {
      if (string.IsNullOrEmpty(sqlScript))
      {
        return null;
      }

      var tokenizer = new MySqlTokenizer(sqlScript.Trim());
      return tokenizer.BreakIntoStatements();
    }

    /// <summary>
    /// Returns the string size of a given number.
    /// </summary>
    /// <param name="number">An integer number.</param>
    /// <param name="ignoreSign">Flag indicating whether the sign of the number is ignored.</param>
    /// <returns>The length of the string representation of this number.</returns>
    public static int StringSize(this int number, bool ignoreSign = true)
    {
      if (ignoreSign)
      {
        number = Math.Abs(number);
      }

      return number == 0 ? 0 : number.ToString(CultureInfo.InvariantCulture).Length;
    }
  }
}