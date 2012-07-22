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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySQL.ForExcel
{
  public partial class AutoStyleableBasePanel : UserControl
  {
    [Category("Appearance"), DefaultValue(true), Description("Indicates whether or not the form automatically uses the system default font.")]
    public bool UseSystemFont { get; set; }

    [Category("Appearance"), DefaultValue(true), Description("Applies the System Font to all controls in the InheritFontToControlsList.")]
    public bool InheritSystemFontToControls { get; set; }

    [Category("Appearance"), Description("List of control names that should NOT inherit the System Font.")]
    [Editor(@"System.Windows.Forms.Design.StringCollectionEditor," +
        "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
       typeof(System.Drawing.Design.UITypeEditor))]
    public List<string> InheritFontToControlsExceptionList { get; set; }

    public AutoStyleableBasePanel()
    {
      InitializeComponent();

      UseSystemFont = true;
      InheritSystemFontToControls = true;
      InheritFontToControlsExceptionList = new List<string>();
    }

    protected virtual void InheritFontToControls(Control.ControlCollection controls, Font inheritingFont)
    {
      if (controls == null || controls.Count == 0)
        return;

      foreach (Control c in controls)
      {
        InheritFontToControls(c.Controls, inheritingFont);
        if (InheritFontToControlsExceptionList != null && InheritFontToControlsExceptionList.Contains(c.Name))
          continue;
        if (c.Font.Name != inheritingFont.Name)
          c.Font = new Font(inheritingFont.FontFamily, c.Font.Size, c.Font.Style);
      }
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      if (!DesignMode && UseSystemFont)
      {
        Font inheritingFont = Font;
        if (Font.Name != System.Drawing.SystemFonts.IconTitleFont.Name)
          inheritingFont = new Font(System.Drawing.SystemFonts.IconTitleFont.FontFamily, Font.Size, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
        Microsoft.Win32.SystemEvents.UserPreferenceChanged += new Microsoft.Win32.UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
        if (InheritSystemFontToControls)
          InheritFontToControls(Controls, inheritingFont);
      }
    }

    private void SystemEvents_UserPreferenceChanged(object sender, Microsoft.Win32.UserPreferenceChangedEventArgs e)
    {
      if (e.Category == Microsoft.Win32.UserPreferenceCategory.Window && UseSystemFont)
        Font = new Font(System.Drawing.SystemFonts.IconTitleFont.FontFamily, Font.Size, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
    }

  }
}
