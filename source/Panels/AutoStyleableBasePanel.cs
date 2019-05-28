// Copyright (c) 2012, 2019, Oracle and/or its affiliates. All rights reserved.
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MySql.Utility.Classes;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Controls;

namespace MySQL.ForExcel.Panels
{
  /// <summary>
  /// The base class for all MySQL for Excel panels, it provides a template where its child controls Font can be easily changed.
  /// </summary>
  public partial class AutoStyleableBasePanel : UserControl
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoStyleableBasePanel"/> class.
    /// </summary>
    public AutoStyleableBasePanel()
    {
      InitializeComponent();
      UseSystemFont = true;
      InheritSystemFontToControls = true;
      InheritFontToControlsExceptionList = new List<string>();
    }

    #region Properties

    /// <summary>
    /// Gets or sets a list of control names that should NOT inherit the System Font.
    /// </summary>
    [Category("Appearance"), Description("List of control names that should NOT inherit the System Font.")]
    [Editor(@"System.Windows.Forms.Design.StringCollectionEditor," +
        "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
       typeof(System.Drawing.Design.UITypeEditor))]
    public List<string> InheritFontToControlsExceptionList { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the System Font is applied to all controls in the InheritFontToControlsList.
    /// </summary>
    [Category("Appearance"), DefaultValue(true), Description("Applies the System Font to all controls in the InheritFontToControlsList.")]
    public bool InheritSystemFontToControls { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the form automatically uses the system default font.
    /// </summary>
    [Category("Appearance"), DefaultValue(true), Description("Indicates whether or not the form automatically uses the system default font.")]
    public bool UseSystemFont { get; set; }

    #endregion Properties

    /// <summary>
    /// Adjusts the colors to match the current color theme.
    /// </summary>
    /// <param name="callBase">Calls the base functionality.</param>
    /// <param name="officeTheme">The current <see cref="OfficeTheme"/>.</param>
    public virtual void AdjustColorsForColorTheme(bool callBase, OfficeTheme officeTheme)
    {
      BackColor = officeTheme?.BodyBackgroundColor ?? SystemColors.Control;
      var hotLabels = this.GetChildControlsOfType<HotLabel>();
      foreach (var hotLabel in hotLabels)
      {
        hotLabel.AdjustColorsForColorTheme(officeTheme);
      }

      var listViews = this.GetChildControlsOfType<MySqlListView>();
      foreach (var listView in listViews)
      {
        listView.AdjustColorsForColorTheme(officeTheme);
      }

      var searchEdits = this.GetChildControlsOfType<SearchEdit>();
      foreach (var searchEdit in searchEdits)
      {
        searchEdit.AdjustColorsForColorTheme(officeTheme);
      }

      var panels = this.GetChildControlsOfType<Panel>();
      foreach (var panel in panels)
      {
        panel.BackColor = officeTheme?.BodyBackgroundColor ?? SystemColors.Control;
      }
    }

    /// <summary>
    /// Sets the font in all controls of the given controls collection to the given font.
    /// </summary>
    /// <param name="controls">A collection of controls to inherit a font to.</param>
    /// <param name="inheritingFont">A <see cref="Font"/> object to set it in each control of the given collection.</param>
    protected virtual void InheritFontToControls(ControlCollection controls, Font inheritingFont)
    {
      if (controls == null || controls.Count == 0)
      {
        return;
      }

      foreach (Control c in controls)
      {
        InheritFontToControls(c.Controls, inheritingFont);
        if (InheritFontToControlsExceptionList != null && InheritFontToControlsExceptionList.Contains(c.Name))
        {
          continue;
        }

        if (c.Font.Name != inheritingFont.Name)
        {
          c.Font = new Font(inheritingFont.FontFamily, c.Font.Size, c.Font.Style);
        }
      }
    }

    /// <summary>
    /// Raises the <see cref="System.Windows.Forms.UserControl.Load"/> event.
    /// </summary>
    /// <param name="e">Event arguments.</param>
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      if (DesignMode || !UseSystemFont)
      {
        return;
      }

      var inheritingFont = Font;
      if (Font.Name != SystemFonts.IconTitleFont.Name)
      {
        inheritingFont = new Font(SystemFonts.IconTitleFont.FontFamily, Font.Size, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
      }

      Microsoft.Win32.SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
      if (InheritSystemFontToControls)
      {
        InheritFontToControls(Controls, inheritingFont);
      }
    }

    /// <summary>
    /// Event delegate method fired when the user preferences are being overriden.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SystemEvents_UserPreferenceChanged(object sender, Microsoft.Win32.UserPreferenceChangedEventArgs e)
    {
      if (e.Category == Microsoft.Win32.UserPreferenceCategory.Window && UseSystemFont)
      {
        Font = new Font(SystemFonts.IconTitleFont.FontFamily, Font.Size, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
      }
    }
  }
}