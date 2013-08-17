// 
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
//

namespace MySQL.ForExcel
{
  using System;
  using System.Collections;
  using System.ComponentModel;
  using System.Data;
  using System.Reflection;
  using System.Windows.Forms;
  using MySql.Data.MySqlClient;
  using MySQL.Utility;
  using MySQL.Utility.Forms;

  /// <summary>
  ///
  /// </summary>
  public partial class ImportProcedureForm : AutoStyleableBaseDialog
  {
    #region Fields

    /// <summary>
    /// Array of parameters for the selected MySQL procedure.
    /// </summary>
    private MySqlParameter[] _mysqlParameters;

    /// <summary>
    /// Collection of properties of the MySQL procedure's parameters.
    /// </summary>
    private PropertiesCollection _procedureParamsProperties;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportProcedureForm"/> class.
    /// </summary>
    /// <param name="wbConnection">The connection to a MySQL server instance selected by users.</param>
    /// <param name="importDBObject">The Procedure DB object selected by the users to import data from.</param>
    /// <param name="importToWorksheetName">The name of the Excel worksheet where data will be imported.</param>
    /// <param name="workSheetInCompatibilityMode">Flag indicating whether the Excel worksheet where data will be imported is open in compatibility mode.</param>
    public ImportProcedureForm(MySqlWorkbenchConnection wbConnection, DBObject importDBObject, string importToWorksheetName, bool workSheetInCompatibilityMode)
    {
      ImportDBObject = importDBObject;
      PreviewDataSet = null;
      SumOfResultSetsExceedsMaxCompatibilityRows = false;
      WBConnection = wbConnection;
      WorkSheetInCompatibilityMode = workSheetInCompatibilityMode;

      InitializeComponent();

      SelectedResultSetIndex = -1;
      Text = string.Format("Import Data - {0}", importToWorksheetName);
      _procedureParamsProperties = new PropertiesCollection();
      ProcedureNameLabel.Text = importDBObject.Name;
      OptionsWarningLabel.Text = Properties.Resources.WorkSheetInCompatibilityModeWarning;
      ParametersPropertyGrid.SelectedObject = _procedureParamsProperties;

      InitializeMultipleResultSetsCombo();
      PrepareParameters();
      IncludeHeadersCheckBox.Checked = true;
    }

    /// <summary>
    /// Specifies identifiers to indicate the type of import for multiple result sets returned by a MySQL procedure.
    /// </summary>
    public enum ImportMultipleType
    {
      /// <summary>
      /// Only the result seet selected by users is imported.
      /// </summary>
      SelectedResultSet,

      /// <summary>
      /// All result sets returned by the procedure are imported and arranged horizontally in the Excel worksheet.
      /// </summary>
      AllResultSetsHorizontally,

      /// <summary>
      /// All result sets returned by the procedure are imported and arranged vertically in the Excel worksheet.
      /// </summary>
      AllResultSetsVertically
    }

    #region Properties

    /// <summary>
    /// Gets the <see cref="DataSet"/> containing the tables with all the result sets returned by the MySQL procedure.
    /// </summary>
    public DataSet ImportDataSet { get; private set; }

    /// <summary>
    /// Get a value indicating whether column names are imported as the first data row in the Excel worksheet.
    /// </summary>
    public bool ImportHeaders
    {
      get
      {
        return IncludeHeadersCheckBox.Checked;
      }
    }

    /// <summary>
    /// Gets the Procedure DB object selected by the users to import data from.
    /// </summary>
    public DBObject ImportDBObject { get; private set; }

    /// <summary>
    /// Gets the import type selected by users.
    /// </summary>
    public ImportMultipleType ImportType
    {
      get
      {
        ImportMultipleType retType = ImportMultipleType.SelectedResultSet;
        int multTypeValue = ImportResultsetsComboBox != null && ImportResultsetsComboBox.Items.Count > 0 ? (int)ImportResultsetsComboBox.SelectedValue : 0;
        switch (multTypeValue)
        {
          case 0:
            retType = ImportMultipleType.SelectedResultSet;
            break;

          case 1:
            retType = ImportMultipleType.AllResultSetsHorizontally;
            break;

          case 2:
            retType = ImportMultipleType.AllResultSetsVertically;
            break;
        }
        return retType;
      }
    }

    /// <summary>
    /// Gets the <see cref="DataSet"/> with a subset of data to import from the procedure's result set to show in the preview grid.
    /// </summary>
    public DataSet PreviewDataSet { get; private set; }

    /// <summary>
    /// Gets the index of the table representing the result set selected by users.
    /// </summary>
    public int SelectedResultSetIndex { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the sum of rows in all result sets returned by the procedure exceeds
    /// the maximum number of rows in an Excel worksheet open in compatibility mode.
    /// </summary>
    public bool SumOfResultSetsExceedsMaxCompatibilityRows { get; private set; }

    /// <summary>
    /// Gets the connection to a MySQL server instance selected by users.
    /// </summary>
    public MySqlWorkbenchConnection WBConnection { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the Excel worksheet where data will be imported is open in compatibility mode.
    /// </summary>
    public bool WorkSheetInCompatibilityMode { get; private set; }

    #endregion Properties

    /// <summary>
    /// Event delegate method fired when the <see cref="CallButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void CallButton_Click(object sender, EventArgs e)
    {
      Cursor = Cursors.WaitCursor;
      try
      {
        //// Prepare parameters and execute the procedure and create OutAndReturnValues table
        DataTable outParamsTable = new DataTable("OutAndReturnValues");
        for (int paramIdx = 0; paramIdx < _procedureParamsProperties.Count; paramIdx++)
        {
          _mysqlParameters[paramIdx].Value = _procedureParamsProperties[paramIdx].Value;
          if (_mysqlParameters[paramIdx].Direction == ParameterDirection.Output || _mysqlParameters[paramIdx].Direction == ParameterDirection.ReturnValue)
          {
            outParamsTable.Columns.Add(_procedureParamsProperties[paramIdx].Name, _procedureParamsProperties[paramIdx].Value.GetType());
          }
        }

        ImportDataSet = WBConnection.GetDataSetFromProcedure(ImportDBObject, _mysqlParameters);
        if (ImportDataSet == null || ImportDataSet.Tables.Count == 0)
        {
          ImportButton.Enabled = false;
          return;
        }

        ImportButton.Enabled = true;

        //// Refresh output/return parameter values in PropertyGrid and add them to OutAndReturnValues table
        if (outParamsTable != null && outParamsTable.Columns.Count > 0)
        {
          DataRow valuesRow = outParamsTable.NewRow();
          for (int paramIdx = 0; paramIdx < _procedureParamsProperties.Count; paramIdx++)
          {
            if (_mysqlParameters[paramIdx].Direction == ParameterDirection.Output || _mysqlParameters[paramIdx].Direction == ParameterDirection.ReturnValue)
            {
              _procedureParamsProperties[paramIdx].Value = _mysqlParameters[paramIdx].Value;
              valuesRow[_mysqlParameters[paramIdx].ParameterName] = _mysqlParameters[paramIdx].Value;
            }
          }

          outParamsTable.Rows.Add(valuesRow);
          ImportDataSet.Tables.Add(outParamsTable);
          ParametersPropertyGrid.Refresh();
        }

        //// Prepare Preview DataSet to show it on Grids
        PreviewDataSet = ImportDataSet.Clone();
        int resultSetsRowSum = 0;
        for (int tableIdx = 0; tableIdx < ImportDataSet.Tables.Count; tableIdx++)
        {
          resultSetsRowSum += ImportDataSet.Tables[tableIdx].Rows.Count;
          if (WorkSheetInCompatibilityMode)
          {
            SumOfResultSetsExceedsMaxCompatibilityRows = SumOfResultSetsExceedsMaxCompatibilityRows || resultSetsRowSum > UInt16.MaxValue;
          }

          int limitRows = Math.Min(ImportDataSet.Tables[tableIdx].Rows.Count, 10);
          for (int rowIdx = 0; rowIdx < limitRows; rowIdx++)
          {
            PreviewDataSet.Tables[tableIdx].ImportRow(ImportDataSet.Tables[tableIdx].Rows[rowIdx]);
          }
        }

        //// Refresh ResultSets in Tab Control
        ResultSetsDataGridView.DataSource = null;
        ResultSetsTabControl.TabPages.Clear();
        for (int dtIdx = 0; dtIdx < ImportDataSet.Tables.Count; dtIdx++)
        {
          ResultSetsTabControl.TabPages.Add(ImportDataSet.Tables[dtIdx].TableName);
        }

        if (ResultSetsTabControl.TabPages.Count > 0)
        {
          SelectedResultSetIndex = ResultSetsTabControl.SelectedIndex = 0;
          ResultSetsTabControl_SelectedIndexChanged(ResultSetsTabControl, EventArgs.Empty);
        }

        Cursor = Cursors.Default;
      }
      catch (Exception ex)
      {
        Cursor = Cursors.Default;
        MiscUtilities.ShowCustomizedErrorDialog(Properties.Resources.ImportProcedureErrorTitle, ex.Message, true);
        MySQLSourceTrace.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ImportButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportButton_Click(object sender, EventArgs e)
    {
      if (SumOfResultSetsExceedsMaxCompatibilityRows && ImportType == ImportMultipleType.AllResultSetsVertically && ImportDataSet.Tables.Count > 1)
      {
        InfoDialog.ShowWarningDialog(Properties.Resources.ImportVerticallyExceedsMaxRowsTitleWarning, Properties.Resources.ImportVerticallyExceedsMaxRowsDetailWarning);
      }
    }

    /// <summary>
    /// Initializes the result sets combo box with the different import options.
    /// </summary>
    private void InitializeMultipleResultSetsCombo()
    {
      DataTable dt = new DataTable();
      dt.Columns.Add("value", Type.GetType("System.Int32"));
      dt.Columns.Add("description", Type.GetType("System.String"));
      DataRow dr = dt.NewRow();
      dr["value"] = ImportMultipleType.SelectedResultSet;
      dr["description"] = Properties.Resources.ImportProcedureSelectedResultSet;
      dt.Rows.Add(dr);
      dr = dt.NewRow();
      dr["value"] = ImportMultipleType.AllResultSetsHorizontally;
      dr["description"] = Properties.Resources.ImportProcedureAllResultSetsHorizontally;
      dt.Rows.Add(dr);
      dr = dt.NewRow();
      dr["value"] = ImportMultipleType.AllResultSetsVertically;
      dr["description"] = Properties.Resources.ImportProcedureAllResultSetsVertically;
      dt.Rows.Add(dr);
      ImportResultsetsComboBox.DataSource = dt;
      ImportResultsetsComboBox.DisplayMember = "description";
      ImportResultsetsComboBox.ValueMember = "value";
    }

    /// <summary>
    /// Prepares the procedure parameters needed to call the MySQL procedure.
    /// </summary>
    private void PrepareParameters()
    {
      CustomProperty parameter = null;
      DataTable parametersTable = WBConnection.GetSchemaCollection("Procedure Parameters", null, WBConnection.Schema, ImportDBObject.Name);
      _mysqlParameters = new MySqlParameter[parametersTable.Rows.Count];
      int paramIdx = 0;
      MySqlDbType dbType = MySqlDbType.Guid;
      object objValue = null;

      foreach (DataRow dr in parametersTable.Rows)
      {
        string dataType = dr["DATA_TYPE"].ToString().ToLowerInvariant();
        string paramName = dr["PARAMETER_NAME"].ToString();
        ParameterDirection paramDirection = ParameterDirection.Input;
        int paramSize = dr["CHARACTER_MAXIMUM_LENGTH"] != null && dr["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value ? Convert.ToInt32(dr["CHARACTER_MAXIMUM_LENGTH"]) : 0;
        byte paramPrecision = dr["NUMERIC_PRECISION"] != null && dr["NUMERIC_PRECISION"] != DBNull.Value ? Convert.ToByte(dr["NUMERIC_PRECISION"]) : (byte)0;
        byte paramScale = dr["NUMERIC_SCALE"] != null && dr["NUMERIC_SCALE"] != DBNull.Value ? Convert.ToByte(dr["NUMERIC_SCALE"]) : (byte)0;
        bool paramUnsigned = dr["DTD_IDENTIFIER"].ToString().Contains("unsigned");
        string paramDirectionStr = paramName != "RETURN_VALUE" ? dr["PARAMETER_MODE"].ToString().ToLowerInvariant() : "return";
        bool paramIsReadOnly = false;

        switch (paramDirectionStr)
        {
          case "in":
            paramDirection = ParameterDirection.Input;
            paramIsReadOnly = false;
            break;

          case "out":
            paramDirection = ParameterDirection.Output;
            paramIsReadOnly = true;
            break;

          case "inout":
            paramDirection = ParameterDirection.InputOutput;
            paramIsReadOnly = false;
            break;

          case "return":
            paramDirection = ParameterDirection.ReturnValue;
            paramIsReadOnly = true;
            break;
        }

        switch (dataType)
        {
          case "bit":
            dbType = MySqlDbType.Bit;
            if (paramPrecision > 1)
            {
              objValue = 0;
            }
            else
            {
              objValue = false;
            }
            break;

          case "int":
          case "integer":
            dbType = MySqlDbType.Int32;
            objValue = (Int32)0;
            break;

          case "tinyint":
            dbType = paramUnsigned ? MySqlDbType.UByte : MySqlDbType.Byte;
            objValue = (Byte)0;
            break;

          case "smallint":
            dbType = paramUnsigned ? MySqlDbType.UInt16 : MySqlDbType.Int16;
            objValue = (Int16)0;
            break;

          case "mediumint":
            dbType = paramUnsigned ? MySqlDbType.UInt24 : MySqlDbType.Int24;
            objValue = (Int32)0;
            break;

          case "bigint":
            dbType = paramUnsigned ? MySqlDbType.UInt64 : MySqlDbType.Int64;
            objValue = (Int64)0;
            break;

          case "numeric":
          case "decimal":
          case "float":
          case "double":
          case "real":
            dbType = dataType == "float" ? MySqlDbType.Float : (dataType == "double" || dataType == "real" ? MySqlDbType.Double : MySqlDbType.Decimal);
            objValue = (Double)0;
            break;

          case "char":
          case "varchar":
            dbType = MySqlDbType.VarChar;
            objValue = string.Empty;
            break;

          case "binary":
          case "varbinary":
            dbType = dataType.StartsWith("var") ? MySqlDbType.VarBinary : MySqlDbType.Binary;
            objValue = string.Empty;
            break;

          case "text":
          case "tinytext":
          case "mediumtext":
          case "longtext":
            dbType = dataType.StartsWith("var") ? MySqlDbType.VarBinary : MySqlDbType.Binary;
            objValue = string.Empty;
            break;

          case "date":
          case "datetime":
          case "timestamp":
            dbType = dataType == "date" ? MySqlDbType.Date : MySqlDbType.DateTime;
            objValue = DateTime.Today;
            break;

          case "time":
            dbType = MySqlDbType.Time;
            objValue = TimeSpan.Zero;
            break;

          case "blob":
            dbType = MySqlDbType.Blob;
            objValue = null;
            break;
        }

        parameter = new CustomProperty(paramName, objValue, paramIsReadOnly, true);
        parameter.Description = string.Format("Direction: {0}, Data Type: {1}", paramDirection.ToString(), dataType);
        _mysqlParameters[paramIdx] = new MySqlParameter(paramName, dbType, paramSize, paramDirection, false, paramPrecision, paramScale, null, DataRowVersion.Current, objValue);
        _procedureParamsProperties.Add(parameter);
        paramIdx++;
      }

      FieldInfo fi = ParametersPropertyGrid.GetType().GetField("gridView", BindingFlags.NonPublic | BindingFlags.Instance);
      object gridViewRef = fi.GetValue(ParametersPropertyGrid);
      Type gridViewType = gridViewRef.GetType();
      MethodInfo mi = gridViewType.GetMethod("MoveSplitterTo", BindingFlags.NonPublic | BindingFlags.Instance);
      int gridColWidth = (int)Math.Truncate(ParametersPropertyGrid.Width * 0.4);
      mi.Invoke(gridViewRef, new object[] { gridColWidth });
      ParametersPropertyGrid.Refresh();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ResultSetsTabControl"/> selected tab index changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ResultSetsTabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (ResultSetsTabControl.SelectedIndex < 0)
      {
        return;
      }

      SelectedResultSetIndex = ResultSetsTabControl.SelectedIndex;
      ResultSetsTabControl.TabPages[SelectedResultSetIndex].Controls.Add(ResultSetsDataGridView);
      ResultSetsDataGridView.Dock = DockStyle.Fill;
      ResultSetsDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
      if (ResultSetsDataGridView.DataSource == null)
      {
        ResultSetsDataGridView.DataSource = PreviewDataSet;
      }

      ResultSetsDataGridView.DataMember = PreviewDataSet.Tables[SelectedResultSetIndex].TableName;
      bool cappingAtMaxCompatRows = WorkSheetInCompatibilityMode && ImportDataSet.Tables[SelectedResultSetIndex].Rows.Count > UInt16.MaxValue;
      SetCompatibilityWarning(cappingAtMaxCompatRows);
      foreach (DataGridViewColumn gridCol in ResultSetsDataGridView.Columns)
      {
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
      }

      ResultSetsDataGridView.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
    }

    /// <summary>
    /// Shows or hides the Excel worksheet in compatibility mode warning controls.
    /// </summary>
    /// <param name="show"></param>
    private void SetCompatibilityWarning(bool show)
    {
      OptionsWarningLabel.Visible = show;
      OptionsWarningPictureBox.Visible = show;
    }
  }

  /// <summary>
  /// Represents a collection of properties of the MySQL procedure's parameters.
  /// </summary>
  public class PropertiesCollection : CollectionBase, ICustomTypeDescriptor
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
        return (CustomProperty)base.List[index];
      }

      set
      {
        base.List[index] = (CustomProperty)value;
      }
    }

    /// <summary>
    /// Adds a custom property to the collection.
    /// </summary>
    /// <param name="value">The custom property object to add.</param>
    public void Add(CustomProperty value)
    {
      base.List.Add(value);
    }

    /// <summary>
    /// Removes a custom property object from the collection.
    /// </summary>
    /// <param name="name">The name of the custom property to remove.</param>
    public void Remove(string name)
    {
      foreach (CustomProperty prop in base.List)
      {
        if (prop.Name == name)
        {
          base.List.Remove(prop);
          return;
        }
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
      PropertyDescriptor[] newProps = new PropertyDescriptor[this.Count];
      for (int i = 0; i < this.Count; i++)
      {
        CustomProperty prop = (CustomProperty)this[i];
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

  /// <summary>
  /// Represents a single property that can be displayed in a property editor.
  /// </summary>
  public class CustomProperty
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

  /// <summary>
  /// Provides an abstraction of a custom property on a class.
  /// </summary>
  public class CustomPropertyDescriptor : PropertyDescriptor
  {
    /// <summary>
    /// A single property that can be displayed in a property editor.
    /// </summary>
    private CustomProperty _property;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomPropertyDescriptor"/> class.
    /// </summary>
    /// <param name="myProperty">A single property that can be displayed in a property editor.</param>
    /// <param name="attrs">Property attributes.</param>
    public CustomPropertyDescriptor(ref CustomProperty myProperty, Attribute[] attrs)
      : base(myProperty.Name, attrs)
    {
      _property = myProperty;
    }

    #region PropertyDescriptor specific

    /// <summary>
    /// Gets the category name the property belongs to.
    /// </summary>
    public override string Category
    {
      get
      {
        return string.Empty;
      }
    }

    public override Type ComponentType
    {
      get
      {
        return null;
      }
    }

    /// <summary>
    /// Gets the property description.
    /// </summary>
    public override string Description
    {
      get
      {
        return _property.Description;
      }
    }

    /// <summary>
    /// Gets the name of the property.
    /// </summary>
    public override string DisplayName
    {
      get
      {
        return _property.Name;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the property is read only.
    /// </summary>
    public override bool IsReadOnly
    {
      get
      {
        return _property.ReadOnly;
      }
    }

    /// <summary>
    /// Gets the data type of the property.
    /// </summary>
    public override Type PropertyType
    {
      get
      {
        return _property.Value.GetType();
      }
    }

    /// <summary>
    /// Gets a value indicating whether the property value can be reset.
    /// </summary>
    /// <param name="component"></param>
    /// <returns><c>true</c> if the value can be reset, <c>false</c> otherwise.</returns>
    public override bool CanResetValue(object component)
    {
      return false;
    }

    /// <summary>
    /// Gets the property value.
    /// </summary>
    /// <param name="component"></param>
    /// <returns>The property value.</returns>
    public override object GetValue(object component)
    {
      return _property.Value;
    }

    /// <summary>
    /// Resets the property value.
    /// </summary>
    /// <param name="component"></param>
    public override void ResetValue(object component)
    {
      //// Have to implement
    }

    /// <summary>
    /// Sets the property value to the given one.
    /// </summary>
    /// <param name="component"></param>
    /// <param name="value">The new property value.</param>
    public override void SetValue(object component, object value)
    {
      _property.Value = value;
    }

    /// <summary>
    /// Indicates whether the property value is serializable.
    /// </summary>
    /// <param name="component"></param>
    /// <returns><c>true</c> if the property value is serializable, <c>false</c> otherwise.</returns>
    public override bool ShouldSerializeValue(object component)
    {
      return false;
    }

    #endregion PropertyDescriptor specific
  }
}