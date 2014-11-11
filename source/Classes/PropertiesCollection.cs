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
using System.Collections;
using System.ComponentModel;
using System.Linq;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a collection of properties of the MySQL procedure's parameters.
  /// </summary>
  internal class PropertiesCollection : CollectionBase, ICustomTypeDescriptor
  {
    /// <summary>
    /// Gets or sets the custom property in the specified index position.
    /// </summary>
    /// <param name="index">Index position.</param>
    /// <returns>The custom property object.</returns>
    public CustomProperty this[int index]
    {
      get
      {
        return (CustomProperty)List[index];
      }

      set
      {
        List[index] = value;
      }
    }

    /// <summary>
    /// Adds a custom property to the collection.
    /// </summary>
    /// <param name="value">The custom property object to add.</param>
    public void Add(CustomProperty value)
    {
      List.Add(value);
    }

    /// <summary>
    /// Removes a custom property object from the collection.
    /// </summary>
    /// <param name="name">The name of the custom property to remove.</param>
    public void Remove(string name)
    {
      foreach (CustomProperty prop in List.Cast<CustomProperty>().Where(prop => prop.Name == name))
      {
        List.Remove(prop);
        return;
      }
    }

    #region TypeDescriptor Implementation

    /// <summary>
    /// Returns a collection of attributes for the specified component and a Boolean indicating that a custom type descriptor has been created.
    /// </summary>
    /// <returns>An <see cref="AttributeCollection"/> with the attributes for the component. If the component is <c>null</c>, this method returns an empty collection.</returns>
    public AttributeCollection GetAttributes()
    {
      return TypeDescriptor.GetAttributes(this, true);
    }

    /// <summary>
    /// Returns the name of the class for the specified component using a custom type descriptor.
    /// </summary>
    /// <returns>A <see cref="String"/> containing the name of the class for the specified component.</returns>
    public String GetClassName()
    {
      return TypeDescriptor.GetClassName(this, true);
    }

    /// <summary>
    /// Returns the name of the specified component using a custom type descriptor.
    /// </summary>
    /// <returns>The name of the class for the specified component, or <c>null</c> if there is no component name.</returns>
    public String GetComponentName()
    {
      return TypeDescriptor.GetComponentName(this, true);
    }

    /// <summary>
    /// Returns a type converter for the type of the specified component with a custom type descriptor.
    /// </summary>
    /// <returns>A <see cref="TypeConverter"/> for the specified component.</returns>
    public TypeConverter GetConverter()
    {
      return TypeDescriptor.GetConverter(this, true);
    }

    /// <summary>
    /// Returns the default event for a component with a custom type descriptor.
    /// </summary>
    /// <returns>An <see cref="EventDescriptor"/> with the default event, or <c>null</c> if there are no events.</returns>
    public EventDescriptor GetDefaultEvent()
    {
      return TypeDescriptor.GetDefaultEvent(this, true);
    }

    /// <summary>
    /// Returns the default property for the specified component with a custom type descriptor.
    /// </summary>
    /// <returns>A <see cref="PropertyDescriptor"/> with the default property, or <c>null</c> if there are no properties.</returns>
    public PropertyDescriptor GetDefaultProperty()
    {
      return TypeDescriptor.GetDefaultProperty(this, true);
    }

    /// <summary>
    /// Returns an editor with the specified base type and with a custom type descriptor for the specified component.
    /// </summary>
    /// <param name="editorBaseType">A <see cref="Type"/> that represents the base type of the editor you want to find.</param>
    /// <returns>An instance of the editor that can be cast to the specified editor type, or <c>null</c> if no editor of the requested type can be found.</returns>
    public object GetEditor(Type editorBaseType)
    {
      return TypeDescriptor.GetEditor(this, editorBaseType, true);
    }

    /// <summary>
    /// Returns the collection of events for a specified component using a specified array of attributes as a filter and using a custom type descriptor.
    /// </summary>
    /// <param name="attributes">An array of type <see cref="Attribute"/> to use as a filter.</param>
    /// <returns>An <see cref="EventDescriptorCollection"/> with the events that match the specified attributes for this component.</returns>
    public EventDescriptorCollection GetEvents(Attribute[] attributes)
    {
      return TypeDescriptor.GetEvents(this, attributes, true);
    }

    /// <summary>
    /// Returns the collection of events for a specified component with a custom type descriptor.
    /// </summary>
    /// <returns>An <see cref="EventDescriptorCollection"/> with the events for this component.</returns>
    public EventDescriptorCollection GetEvents()
    {
      return TypeDescriptor.GetEvents(this, true);
    }

    /// <summary>
    /// Returns the collection of properties based on their corresponding attributes.
    /// </summary>
    /// <param name="attributes">Array of attributes.</param>
    /// <returns>A <see cref="PropertyDescriptorCollection"/> with properties corresponding to thegiven attributes.</returns>
    public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
    {
      PropertyDescriptor[] newProps = new PropertyDescriptor[Count];
      for (int i = 0; i < Count; i++)
      {
        CustomProperty prop = this[i];
        newProps[i] = new CustomPropertyDescriptor(ref prop, attributes);
      }

      return new PropertyDescriptorCollection(newProps);
    }

    /// <summary>
    /// Returns the collection of properties for a specified component using the default type descriptor.
    /// </summary>
    /// <returns>A <see cref="PropertyDescriptorCollection"/> with the properties for a specified component.</returns>
    public PropertyDescriptorCollection GetProperties()
    {
      return TypeDescriptor.GetProperties(this, true);
    }

    /// <summary>
    /// Returns an object that contains the property described by the specified property descriptor.
    /// </summary>
    /// <param name="pd">A <see cref="PropertyDescriptor"/> that represents the property whose owner is to be found.</param>
    /// <returns>An <see cref="Object"/> that represents the owner of the specified property.</returns>
    public object GetPropertyOwner(PropertyDescriptor pd)
    {
      return this;
    }

    #endregion TypeDescriptor Implementation
  }
}
