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
using System.Configuration;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using MySQL.ForExcel.Interfaces;

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
    /// Gets the active Edit session related to a given <see cref="Excel.Workbook"/>.
    /// </summary>
    /// <param name="sessionsList">The Edit sessions list.</param>
    /// <param name="workbook">The <see cref="Excel.Workbook"/> related to the active Edit session.</param>
    /// <param name="tableName">Name of the table being edited in the Edit session.</param>
    /// <returns>An <see cref="EditSessionInfo"/> containing the active edit session.</returns>
    public static EditSessionInfo GetActiveEditSession(this List<EditSessionInfo> sessionsList, Excel.Workbook workbook, string tableName)
    {
      var workBookId = workbook.GetOrCreateId();
      return sessionsList == null ? null : sessionsList.FirstOrDefault(session => session.EditDialog != null &&
      string.Equals(session.WorkbookGuid, workBookId, StringComparison.InvariantCulture) &&
      session.TableName == tableName);
    }

    /// <summary>
    /// Gets the active Edit session related to a given <see cref="Excel.Worksheet"/>.
    /// </summary>
    /// <param name="sessionsList">The Edit sessions list.</param>
    /// <param name="worksheet">The <see cref="Excel.Worksheet"/> related to the active Edit session.</param>
    /// <returns>An <see cref="EditSessionInfo"/> containing the active Edit session.</returns>
    public static EditSessionInfo GetActiveEditSession(this List<EditSessionInfo> sessionsList, Excel.Worksheet worksheet)
    {
      return sessionsList == null ? null : sessionsList.FirstOrDefault(session => session.EditDialog != null && session.EditDialog.EditingWorksheet.Name == worksheet.Name);
    }

    /// <summary>
    /// Gets a linear array from a bidimensional one extracting the elements of the given dimension.
    /// </summary>
    /// <param name="biDimensionalArray">The bidimensional array.</param>
    /// <param name="firstDimensionIndex">The index to extract elements from.</param>
    /// <param name="shiftIndexes">Flag indicating whether the indexes are shifted +1 (for Excel arrays) or not.</param>
    /// <returns>A linear array.</returns>
    public static IEnumerable<object> GetLinearArray(this object[,] biDimensionalArray, int firstDimensionIndex, bool shiftIndexes = false)
    {
      int startingIndex = shiftIndexes ? 1 : 0;
      int linearArrayLength = biDimensionalArray.GetLength(firstDimensionIndex) + startingIndex;
      for (int i = startingIndex; i < linearArrayLength; i++)
      {
        yield return biDimensionalArray[firstDimensionIndex, i];
      }
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
    /// Determines whether this session has same workbook and table as the specified comparing session.
    /// </summary>
    /// <param name="session">The current session.</param>
    /// <param name="comparingSession">The session we want to compare values with.</param>
    /// <returns><c>true</c> when this session has same workbook and table as the specified comparing session, <c>false</c> otherwise.</returns>
    public static bool HasSameWorkbookAndTable(this ISessionInfo session, ISessionInfo comparingSession)
    {
      return session != null && !string.IsNullOrEmpty(session.WorkbookGuid) && !string.IsNullOrEmpty(comparingSession.WorkbookGuid)
      && string.Equals(session.WorkbookGuid, comparingSession.WorkbookGuid, StringComparison.InvariantCulture)
      && !string.IsNullOrEmpty(session.TableName) && !string.IsNullOrEmpty(comparingSession.TableName)
      && string.Equals(session.TableName, comparingSession.TableName, StringComparison.InvariantCulture);
    }

    /// <summary>
    /// Determines whether this session has same workbook and table as the specified comparing session.
    /// </summary>
    /// <param name="session">The current session.</param>
    /// <param name="comparingSession">The session we want to compare values with.</param>
    /// <returns><c>true</c> when this session has same workbook and table as the specified comparing session, <c>false</c> otherwise.</returns>
    public static bool HasSameWorkbookWorkSheetAndExcelTable(this ImportSessionInfo session, ImportSessionInfo comparingSession)
    {
      return session != null && !string.IsNullOrEmpty(session.WorkbookGuid) && !string.IsNullOrEmpty(comparingSession.WorkbookGuid)
      && string.Equals(session.WorkbookGuid, comparingSession.WorkbookGuid, StringComparison.InvariantCulture)
      && !string.IsNullOrEmpty(session.WorksheetName) && !string.IsNullOrEmpty(comparingSession.WorksheetName)
      && string.Equals(session.WorksheetName, comparingSession.WorksheetName, StringComparison.InvariantCulture)
      && !string.IsNullOrEmpty(session.ExcelTableName) && !string.IsNullOrEmpty(comparingSession.ExcelTableName)
      && string.Equals(session.ExcelTableName, comparingSession.ExcelTableName, StringComparison.InvariantCulture);
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
      InfoDialog.DialogType dialogType = InfoDialog.DialogType.OKOnly;
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
          dialogType = InfoDialog.DialogType.BackOnly;
          break;

        case InfoDialog.InfoType.Info:
          title = Resources.OperationInformationTitle;
          break;
      }

      string subDetailText = string.Format(Resources.OperationSubDetailText, infoType == InfoDialog.InfoType.Error ? "Back" : "OK");
      return InfoDialog.ShowDialog(dialogType, infoType, title, detail, subDetailText, moreInformation, wordWrapMoreInfo);
    }

    /// <summary>
    /// Shows a warning dialog customized for MySQL for Excel showing Yes/No buttons.
    /// </summary>
    /// <param name="title">The main short title of the warning.</param>
    /// <param name="detail">The detail text describing further the warning.</param>
    /// <returns>A dialog result with the user's selection.</returns>
    public static DialogResult ShowCustomizedWarningDialog(string title, string detail)
    {
      return InfoDialog.ShowYesNoDialog(InfoDialog.InfoType.Warning, title, detail);
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