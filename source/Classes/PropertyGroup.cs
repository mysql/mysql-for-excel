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
using MySQL.ForExcel.Forms;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Custom attribute class used to sectionize the application settings' properties by groups.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public class PropertyGroup : Attribute
  {
    /// <summary>
    /// Groups in which the Application Settings can be sub-divided into.
    /// </summary>
    public enum SettingsGroup
    {
      /// <summary>
      // Application Settings used in the <see cref="ImportAdvancedOptionsDialog"/> class.
      /// </summary>
      Import,
      /// <summary>
      ///Application Settings used in the <see cref="ExportAdvancedOptionsDialog"/> class.
      /// </summary>
      Export,
      /// <summary>
      /// Application Settings used in the <see cref="AppendAdvancedOptionsDialog"/> class.
      /// </summary>
      Append,
      /// <summary>
      /// Application Settings used in the <see cref="GlobalOptionsDialog"/> class.
      /// </summary>
      Global
    }

    /// <summary>
    /// The section the option/setting is related to.
    /// </summary>
    public SettingsGroup Value { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyGroup"/> class.
    /// </summary>
    /// <param name="section">The section group the option/setting is related to.</param>
    public PropertyGroup(SettingsGroup section)
    {
      Value = section;
    }
  }
}
