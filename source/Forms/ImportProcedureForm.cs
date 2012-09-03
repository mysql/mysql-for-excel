// 
// Copyright (c) 2012, Oracle and/or its affiliates. All rights reserved.
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.Utility;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel
{
  public partial class ImportProcedureForm : AutoStyleableBaseDialog
  {
    private MySqlWorkbenchConnection wbConnection;
    private DBObject importDBObject;
    private PropertiesCollection procedureParamsProperties;
    private MySqlParameter[] mysqlParameters;
    private bool workSheetInCompatibilityMode = false;
    private bool sumOfResultSetsExceedsMaxCompatibilityRows = false;
    private DataSet previewDataSet = null;

    public DataSet ImportDataSet = null;
    public bool ImportHeaders { get { return chkIncludeHeaders.Checked; } }
    public int SelectedResultSet { get; private set; }
    public ImportMultipleType ImportType
    {
      get
      {
        ImportMultipleType retType = ImportMultipleType.SelectedResultSet;
        int multTypeValue = (cmbImportResultsets != null && cmbImportResultsets.Items.Count > 0 ? (int)cmbImportResultsets.SelectedValue : 0);
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

    public ImportProcedureForm(MySqlWorkbenchConnection wbConnection, DBObject importDBObject, string importToWorksheetName, bool workSheetInCompatibilityMode)
    {
      this.wbConnection = wbConnection;
      this.importDBObject = importDBObject;
      this.workSheetInCompatibilityMode = workSheetInCompatibilityMode;

      InitializeComponent();

      SelectedResultSet = -1;
      Text = String.Format("Import Data - {0}", importToWorksheetName);
      procedureParamsProperties = new PropertiesCollection();
      lblFromProcedureName.Text = importDBObject.Name;
      lblOptionsWarning.Text = Properties.Resources.WorkSheetInCompatibilityModeWarning;
      parametersGrid.SelectedObject = procedureParamsProperties;

      initializeMultipleResultSetsCombo();
      fillParameters();
      chkIncludeHeaders.Checked = true;
    }

    private void initCompatibilityWarning(bool show)
    {
      lblOptionsWarning.Visible = show;
      picOptionsWarning.Visible = show;
    }

    private void initializeMultipleResultSetsCombo()
    {
      DataTable dt = new DataTable();
      dt.Columns.Add("value", Type.GetType("System.Int32"));
      dt.Columns.Add("description", Type.GetType("System.String"));
      DataRow dr = dt.NewRow();
      dr["value"] = ImportMultipleType.SelectedResultSet;
      dr["description"] = "Selected Result Set";
      dt.Rows.Add(dr);
      dr = dt.NewRow();
      dr["value"] = ImportMultipleType.AllResultSetsHorizontally;
      dr["description"] = "All Result Sets - Arranged Horizontally";
      dt.Rows.Add(dr);
      dr = dt.NewRow();
      dr["value"] = ImportMultipleType.AllResultSetsVertically;
      dr["description"] = "All Result Sets - Arranged Vertically";
      dt.Rows.Add(dr);
      cmbImportResultsets.DataSource = dt;
      cmbImportResultsets.DisplayMember = "description";
      cmbImportResultsets.ValueMember = "value";
    }

    private void fillParameters()
    {
      CustomProperty parameter = null;
      DataTable parametersTable = MySQLDataUtilities.GetSchemaCollection(wbConnection, "Procedure Parameters", null, wbConnection.Schema, importDBObject.Name);
      mysqlParameters = new MySqlParameter[parametersTable.Rows.Count];
      int paramIdx = 0;
      MySqlDbType dbType = MySqlDbType.Guid;
      object objValue = null;

      foreach (DataRow dr in parametersTable.Rows)
      {
        string dataType = dr["DATA_TYPE"].ToString().ToLowerInvariant();
        string paramName = dr["PARAMETER_NAME"].ToString();
        ParameterDirection paramDirection = ParameterDirection.Input;
        int paramSize = (dr["CHARACTER_MAXIMUM_LENGTH"] != null && dr["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value ? Convert.ToInt32(dr["CHARACTER_MAXIMUM_LENGTH"]) : 0);
        byte paramPrecision = (dr["NUMERIC_PRECISION"] != null && dr["NUMERIC_PRECISION"] != DBNull.Value ? Convert.ToByte(dr["NUMERIC_PRECISION"]) : (byte)0);
        byte paramScale = (dr["NUMERIC_SCALE"] != null && dr["NUMERIC_SCALE"] != DBNull.Value ? Convert.ToByte(dr["NUMERIC_SCALE"]) : (byte)0);
        bool paramUnsigned = dr["DTD_IDENTIFIER"].ToString().Contains("unsigned");
        //string paramDirectionStr = (dr["PARAMETER_MODE"] != null && dr["PARAMETER_MODE"] != DBNull.Value ? dr["PARAMETER_MODE"].ToString().ToLowerInvariant() : "return");
        string paramDirectionStr = (paramName != "RETURN_VALUE" ? dr["PARAMETER_MODE"].ToString().ToLowerInvariant() : "return");
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
              objValue = 0;
            else
              objValue = false;
            break;
          case "int":
          case "integer":
            dbType = MySqlDbType.Int32;
            objValue = (Int32)0;
            break;
          case "tinyint":
            dbType = (paramUnsigned ? MySqlDbType.UByte : MySqlDbType.Byte);
            objValue = (Byte)0;
            break;
          case "smallint":
            dbType = (paramUnsigned ? MySqlDbType.UInt16 : MySqlDbType.Int16);
            objValue = (Int16)0;
            break;
          case "mediumint":
            dbType = (paramUnsigned ? MySqlDbType.UInt24 : MySqlDbType.Int24);
            objValue = (Int32)0;
            break;
          case "bigint":
            dbType = (paramUnsigned ? MySqlDbType.UInt64 : MySqlDbType.Int64);
            objValue = (Int64)0;
            break;
          case "numeric":
          case "decimal":
          case "float":
          case "double":
          case "real":
            dbType = (dataType == "float" ? MySqlDbType.Float : (dataType == "double" || dataType == "real" ? MySqlDbType.Double : MySqlDbType.Decimal));
            objValue = (Double)0;
            break;
          case "char":
          case "varchar":
            dbType = MySqlDbType.VarChar;
            objValue = String.Empty;
            break;
          case "binary":
          case "varbinary":
            dbType = (dataType.StartsWith("var") ? MySqlDbType.VarBinary : MySqlDbType.Binary);
            objValue = String.Empty;
            break;
          case "text":
          case "tinytext":
          case "mediumtext":
          case "longtext":
            dbType = (dataType.StartsWith("var") ? MySqlDbType.VarBinary : MySqlDbType.Binary);
            objValue = String.Empty;
            break;
          case "date":
          case "datetime":
          case "timestamp":
            dbType = (dataType == "date" ? MySqlDbType.Date : MySqlDbType.DateTime);
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
        parameter.Description = String.Format("Direction: {0}, Data Type: {1}", paramDirection.ToString(), dataType);
        mysqlParameters[paramIdx] = new MySqlParameter(paramName, dbType, paramSize, paramDirection, false, paramPrecision, paramScale, null, DataRowVersion.Current, objValue);
        procedureParamsProperties.Add(parameter);
        paramIdx++;
      }
      FieldInfo fi = parametersGrid.GetType().GetField("gridView", BindingFlags.NonPublic | BindingFlags.Instance);
      object gridViewRef = fi.GetValue(parametersGrid);
      Type gridViewType = gridViewRef.GetType();
      MethodInfo mi = gridViewType.GetMethod("MoveSplitterTo", BindingFlags.NonPublic | BindingFlags.Instance);
      int gridColWidth = (int)Math.Truncate(parametersGrid.Width * 0.4);
      mi.Invoke(gridViewRef, new object[] { gridColWidth });
      parametersGrid.Refresh();
    }

    private void btnCall_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      try
      {
        // Prepare parameters and execute routine and create OutAndReturnValues table
        DataTable outParamsTable = new DataTable("OutAndReturnValues");
        for (int paramIdx = 0; paramIdx < procedureParamsProperties.Count; paramIdx++)
        {
          mysqlParameters[paramIdx].Value = procedureParamsProperties[paramIdx].Value;
          if (mysqlParameters[paramIdx].Direction == ParameterDirection.Output || mysqlParameters[paramIdx].Direction == ParameterDirection.ReturnValue)
            outParamsTable.Columns.Add(procedureParamsProperties[paramIdx].Name, procedureParamsProperties[paramIdx].Value.GetType());
        }
        ImportDataSet = MySQLDataUtilities.GetDataSetFromRoutine(wbConnection, importDBObject, mysqlParameters);
        if (ImportDataSet == null || ImportDataSet.Tables.Count == 0)
        {
          btnImport.Enabled = false;
          return;
        }
        btnImport.Enabled = true;

        // Refresh output/return parameter values in PropertyGrid and add them to OutAndReturnValues table
        if (outParamsTable != null && outParamsTable.Columns.Count > 0)
        {
          DataRow valuesRow = outParamsTable.NewRow();
          for (int paramIdx = 0; paramIdx < procedureParamsProperties.Count; paramIdx++)
          {
            if (mysqlParameters[paramIdx].Direction == ParameterDirection.Output || mysqlParameters[paramIdx].Direction == ParameterDirection.ReturnValue)
            {
              procedureParamsProperties[paramIdx].Value = mysqlParameters[paramIdx].Value;
              valuesRow[mysqlParameters[paramIdx].ParameterName] = mysqlParameters[paramIdx].Value;
            }
          }
          outParamsTable.Rows.Add(valuesRow);
          ImportDataSet.Tables.Add(outParamsTable);
          parametersGrid.Refresh();
        }

        // Prepare Preview DataSet to show it on Grids
        previewDataSet = ImportDataSet.Clone();
        int resultSetsRowSum = 0;
        for (int tableIdx = 0; tableIdx < ImportDataSet.Tables.Count; tableIdx++)
        {
          resultSetsRowSum += ImportDataSet.Tables[tableIdx].Rows.Count;
          if (workSheetInCompatibilityMode)
            sumOfResultSetsExceedsMaxCompatibilityRows = sumOfResultSetsExceedsMaxCompatibilityRows || resultSetsRowSum > UInt16.MaxValue;
          int limitRows = Math.Min(ImportDataSet.Tables[tableIdx].Rows.Count, 10);
          for (int rowIdx = 0; rowIdx < limitRows; rowIdx++)
            previewDataSet.Tables[tableIdx].ImportRow(ImportDataSet.Tables[tableIdx].Rows[rowIdx]);
        }

        // Refresh ResultSets in Tab Control
        tabResultSets.TabPages.Clear();
        for (int dtIdx = 0; dtIdx < ImportDataSet.Tables.Count; dtIdx++)
        {
          tabResultSets.TabPages.Add(ImportDataSet.Tables[dtIdx].TableName);
        }
        if (tabResultSets.TabPages.Count > 0)
        {
          SelectedResultSet = tabResultSets.SelectedIndex = 0;
          tabResultSets_SelectedIndexChanged(tabResultSets, EventArgs.Empty);
        }

        this.Cursor = Cursors.Default;
      }
      catch (Exception ex)
      {
        this.Cursor = Cursors.Default;
        InfoDialog errorDialog = new InfoDialog(false, ex.Message, null);
        errorDialog.WordWrapDetails = true;
        errorDialog.ShowDialog();
        MiscUtilities.GetSourceTrace().WriteError("Application Exception on ImportProcedureForm.btnCall_Click - " + (ex.Message + " " + ex.InnerException), 1);
      }
    }

    private void tabResultSets_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (tabResultSets.SelectedIndex < 0)
        return;
      SelectedResultSet = tabResultSets.SelectedIndex;
      tabResultSets.TabPages[SelectedResultSet].Controls.Add(grdResultSet);
      grdResultSet.Dock = DockStyle.Fill;
      grdResultSet.SelectionMode = DataGridViewSelectionMode.CellSelect;
      if (grdResultSet.DataSource == null)
        grdResultSet.DataSource = previewDataSet;
      grdResultSet.DataMember = previewDataSet.Tables[SelectedResultSet].TableName;
      bool cappingAtMaxCompatRows = workSheetInCompatibilityMode && ImportDataSet.Tables[SelectedResultSet].Rows.Count > UInt16.MaxValue;
      initCompatibilityWarning(cappingAtMaxCompatRows);
      foreach (DataGridViewColumn gridCol in grdResultSet.Columns)
      {
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
      }
      grdResultSet.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      if (sumOfResultSetsExceedsMaxCompatibilityRows && ImportType == ImportMultipleType.AllResultSetsVertically && ImportDataSet.Tables.Count > 1)
      {
        WarningDialog warningDlg = new WarningDialog(WarningDialog.WarningButtons.OK,
                                                     Properties.Resources.ImportVerticallyExceedsMaxRowsTitleWarning,
                                                     Properties.Resources.ImportVerticallyExceedsMaxRowsDetailWarning);
        warningDlg.ShowDialog();
      }
    }
  }

  public enum ImportMultipleType
  {
    SelectedResultSet, AllResultSetsHorizontally, AllResultSetsVertically
  };

  public class CustomProperty
  {
    public string Name { get; private set; }
    public bool ReadOnly { get; private set; }
    public bool Visible { get; private set; }
    public object Value { get; set; }
    public string Description { get; set; }

    public CustomProperty(string sName, object value, bool bReadOnly, bool bVisible)
    {
      Name = sName;
      Value = value;
      ReadOnly = bReadOnly;
      Visible = bVisible;
    }
  }

  public class CustomPropertyDescriptor : PropertyDescriptor
  {
    CustomProperty m_Property;
    public CustomPropertyDescriptor(ref CustomProperty myProperty, Attribute[] attrs)
      : base(myProperty.Name, attrs)
    {
      m_Property = myProperty;
    }

    #region PropertyDescriptor specific

    public override bool CanResetValue(object component)
    {
      return false;
    }

    public override Type ComponentType
    {
      get { return null; }
    }

    public override object GetValue(object component)
    {
      return m_Property.Value;
    }

    public override string Description
    {
      get { return m_Property.Description; }
    }

    public override string Category
    {
      get { return String.Empty; }
    }

    public override string DisplayName
    {
      get { return m_Property.Name; }
    }

    public override bool IsReadOnly
    {
      get { return m_Property.ReadOnly; }
    }

    public override void ResetValue(object component)
    {
      //Have to implement
    }

    public override bool ShouldSerializeValue(object component)
    {
      return false;
    }

    public override void SetValue(object component, object value)
    {
      m_Property.Value = value;
    }

    public override Type PropertyType
    {
      get { return m_Property.Value.GetType(); }
    }

    #endregion
  }

  public class PropertiesCollection : CollectionBase, ICustomTypeDescriptor
  {
    public CustomProperty this[int index]
    {
      get { return (CustomProperty)base.List[index]; }
      set { base.List[index] = (CustomProperty)value; }
    }

    public void Add(CustomProperty value)
    {
      base.List.Add(value);
    }

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

    public String GetClassName()
    {
      return TypeDescriptor.GetClassName(this, true);
    }

    public AttributeCollection GetAttributes()
    {
      return TypeDescriptor.GetAttributes(this, true);
    }

    public String GetComponentName()
    {
      return TypeDescriptor.GetComponentName(this, true);
    }

    public TypeConverter GetConverter()
    {
      return TypeDescriptor.GetConverter(this, true);
    }

    public EventDescriptor GetDefaultEvent()
    {
      return TypeDescriptor.GetDefaultEvent(this, true);
    }

    public PropertyDescriptor GetDefaultProperty()
    {
      return TypeDescriptor.GetDefaultProperty(this, true);
    }

    public object GetEditor(Type editorBaseType)
    {
      return TypeDescriptor.GetEditor(this, editorBaseType, true);
    }

    public EventDescriptorCollection GetEvents(Attribute[] attributes)
    {
      return TypeDescriptor.GetEvents(this, attributes, true);
    }

    public EventDescriptorCollection GetEvents()
    {
      return TypeDescriptor.GetEvents(this, true);
    }

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

    public PropertyDescriptorCollection GetProperties()
    {
      return TypeDescriptor.GetProperties(this, true);
    }

    public object GetPropertyOwner(PropertyDescriptor pd)
    {
      return this;
    }

    #endregion TypeDescriptor Implementation
  }

}
