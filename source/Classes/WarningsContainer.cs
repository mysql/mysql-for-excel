// Copyright (c) 2015, 2019, Oracle and/or its affiliates. All rights reserved.
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
using System.Collections.Generic;
using System.Linq;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a self-managed container of warning texts.
  /// </summary>
  public class WarningsContainer
  {
    #region Fields

    /// <summary>
    /// Dictionary containing all warnings.
    /// </summary>
    private readonly Dictionary<string, string> _allWarnings;

    /// <summary>
    /// List of warning keys set to be shown. The last one in the list represents the currently shown warning.
    /// </summary>
    private readonly List<string> _shownWarningKeys;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="WarningsContainer"/> class.
    /// </summary>
    /// <param name="initialCapacity">The initial capacity of the warnings collection.</param>
    public WarningsContainer(int initialCapacity = 10)
      : this(CurrentWarningChangedMethodType.OnEveryShow, initialCapacity)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WarningsContainer"/> class.
    /// </summary>
    /// <param name="currentWarningChangedMethod">The method used to signal a change on the current warning when the <see cref="Show"/> method is called.</param>
    /// <param name="initialCapacity">The initial capacity of the warnings collection.</param>
    public WarningsContainer(CurrentWarningChangedMethodType currentWarningChangedMethod, int initialCapacity = 10)
    {
      _allWarnings = new Dictionary<string, string>(initialCapacity);
      _shownWarningKeys = new List<string>(initialCapacity);
      CurrentWarningChangedMethod = currentWarningChangedMethod;
    }

    #region Enums

    /// <summary>
    /// Describes the type of method used to signal a change on the current warning.
    /// </summary>
    public enum CurrentWarningChangedMethodType
    {
      /// <summary>
      /// Signals a change of current warning every time the <see cref="Show"/> method is called for a warning already being shown.
      /// </summary>
      OnEveryShow,

      /// <summary>
      /// Signals a change of current warning when the <see cref="Show"/> method is called, but only if the warning was not already being shown.
      /// </summary>
      OnShowIfWarningNotPresent
    }

    #endregion Enums

    #region Properties

    /// <summary>
    /// Gets or sets the method used to signal a change on the current warning when the <see cref="Show"/> method is called.
    /// </summary>
    public CurrentWarningChangedMethodType CurrentWarningChangedMethod { get; set; }

    /// <summary>
    /// Gets the currently shown warning key, i.e. the warning that was shown last using the <see cref="Show"/> method.
    /// </summary>
    public string CurrentWarningKey => _shownWarningKeys != null && _shownWarningKeys.Count > 0
      ? _shownWarningKeys.Last()
      : null;

    /// <summary>
    /// Gets the currently shown warning, i.e. the warning that was shown last using the <see cref="Show"/> method.
    /// </summary>
    public string CurrentWarningText
    {
      get
      {
        var currentKey = CurrentWarningKey;
        return !string.IsNullOrEmpty(currentKey) && _allWarnings != null && _allWarnings.ContainsKey(currentKey)
          ? _allWarnings[currentKey]
          : null;
      }
    }

    /// <summary>
    /// Gets the quantity of warnings defined in the container.
    /// </summary>
    public int DefinedQuantity => _allWarnings?.Count ?? 0;

    /// <summary>
    /// Gets the quantity of warnings set to be shown.
    /// </summary>
    public int ShownQuantity => _shownWarningKeys?.Count ?? 0;

    #endregion Properties

    /// <summary>
    /// Adds a new warning to the container.
    /// </summary>
    /// <param name="warningKey">A key representing the warning.</param>
    /// <param name="warningText">The warning text.</param>
    /// <returns><c>true</c> if the warning does not already exist in the collection so it was added, <c>false</c> if it already exists.</returns>
    public bool Add(string warningKey, string warningText)
    {
      if (string.IsNullOrEmpty(warningKey) || _allWarnings.ContainsKey(warningKey))
      {
        return false;
      }

      _allWarnings.Add(warningKey, warningText);
      return true;
    }

    /// <summary>
    /// Clears the list of warnings set to be shown.
    /// </summary>
    /// <returns><c>true</c> if this operation caused the currently shown warning to change, <c>false</c> otherwise.</returns>
    public bool Clear()
    {
      // Nothing to do.
      if (_shownWarningKeys.Count == 0)
      {
        return false;
      }

      _shownWarningKeys.Clear();
      return true;
    }

    /// <summary>
    /// Removes the warning with the given key from the list of warnings to be shown.
    /// If the currently shown warning is hidden, the next warning in the list is set as the current one.
    /// </summary>
    /// <param name="warningKey">A key representing the warning.</param>
    /// <returns><c>true</c> if this operation caused the currently shown warning to change, <c>false</c> otherwise.</returns>
    public bool Hide(string warningKey)
    {
      // The warning key is not found, so nothing to hide.
      if (!_shownWarningKeys.Contains(warningKey))
      {
        return false;
      }

      // Remove the warning
      var currentKeyChanged = string.Equals(_shownWarningKeys.Last(), warningKey, StringComparison.InvariantCulture);
      _shownWarningKeys.Remove(warningKey);
      return currentKeyChanged;
    }

    /// <summary>
    /// Removes a warning with the given key from the container.
    /// If the currently shown warning is removed, the next warning in the list is set as the current one.
    /// </summary>
    /// <param name="warningKey">A key representing the warning.</param>
    /// <returns><c>true</c> if this operation caused the currently shown warning to change, <c>false</c> otherwise.</returns>
    public bool Remove(string warningKey)
    {
      if (string.IsNullOrEmpty(warningKey) || !_allWarnings.ContainsKey(warningKey))
      {
        return false;
      }

      var currentKeyChanged = string.Equals(_shownWarningKeys.Last(), warningKey, StringComparison.InvariantCulture);
      _shownWarningKeys.Remove(warningKey);
      _allWarnings.Remove(warningKey);
      return currentKeyChanged;
    }

    /// <summary>
    /// Sets the visibility of a warning within the collection.
    /// </summary>
    /// <param name="warningKey">A key representing the warning.</param>
    /// <param name="show">Flag indicating whether the warning is to be shown or hidden.</param>
    /// <returns><c>true</c> if this operation caused the currently shown warning to change, <c>false</c> otherwise.</returns>
    public bool SetVisibility(string warningKey, bool show)
    {
      return show ? Show(warningKey) : Hide(warningKey);
    }

    /// <summary>
    /// Places the warning with the given key as the currently shown warning.
    /// </summary>
    /// <param name="warningKey">A key representing the warning.</param>
    /// <returns><c>true</c> if this operation caused the currently shown warning to change, <c>false</c> otherwise.</returns>
    public bool Show(string warningKey)
    {
      // First time the warning is shown, so we just add it.
      if (!_shownWarningKeys.Contains(warningKey))
      {
        _shownWarningKeys.Add(warningKey);
        return true;
      }

      // If the warning is already in the list and it is the last one, then nothing to do.
      if (string.Equals(_shownWarningKeys.Last(), warningKey, StringComparison.InvariantCulture))
      {
        return false;
      }

      // The warning is already in the list, so we need to move it to be the last one.
      _shownWarningKeys.Remove(warningKey);
      _shownWarningKeys.Add(warningKey);
      return true;
    }
  }
}
