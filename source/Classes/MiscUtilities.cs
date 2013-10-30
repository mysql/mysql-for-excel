// Copyright (c) 2012-2013, Oracle and/or its affiliates. All rights reserved.
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
using System.Windows.Forms;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Forms;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Provides extension methods and other static methods to leverage miscelaneous tasks.
  /// </summary>
  public static class MiscUtilities
  {
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
  }
}