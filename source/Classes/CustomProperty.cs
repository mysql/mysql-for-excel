// Copyright (c) 2012, 2014, Oracle and/or its affiliates. All rights reserved.
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
  /// <summary>
  /// Represents a single property that can be displayed in a property editor.
  /// </summary>
  internal class CustomProperty
  {
    /// <summary>
    /// Instantiates a new instance of the <see cref="CustomProperty"/> class.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// <param name="value">The property value.</param>
    /// <param name="readOnly">Flag indicating whether the property is read only.</param>
    /// <param name="visible">Flag indicating whether the property is visible in a property editor.</param>
    public CustomProperty(string name, object value, bool readOnly, bool visible)
    {
      Name = name;
      Value = value;
      ReadOnly = readOnly;
      Visible = visible;
    }

    /// <summary>
    /// Gets or sets the property description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets the property name.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the property is read only.
    /// </summary>
    public bool ReadOnly { get; private set; }

    /// <summary>
    /// Gets or sets the property value.
    /// </summary>
    public object Value { get; set; }

    /// <summary>
    /// Gets a value indicating whether the property is visible in a property editor.
    /// </summary>
    public bool Visible { get; private set; }
  }
}
