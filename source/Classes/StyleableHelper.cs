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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing;

namespace MySQL.ForExcel
{
  public static class StyleableHelper
  {
    public static bool IsWindowsVistaOrLater
    {
      get
      {
        return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= new Version(6, 0, 6000);
      }
    }

    public static bool IsWindowsXPOrLater
    {
      get
      {
        return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= new Version(5, 1, 2600);
      }
    }

    public static bool AreVistaDialogsThemeSupported
    {
      get
      {
        return IsWindowsVistaOrLater && VisualStyleRenderer.IsSupported && Application.RenderWithVisualStyles;
      }
    }

    public static Point DrawText(IDeviceContext deviceContext, string text, VisualStyleElement element, Font fallbackFont, Point location, bool measureOnly, int width)
    {
      Point newLocation = location;
      if (String.IsNullOrEmpty(text))
        return newLocation;
      Rectangle textRect = new Rectangle(location.X, location.Y, width, (IsWindowsXPOrLater ? Int32.MaxValue : 100000));
      TextFormatFlags flags = TextFormatFlags.WordBreak;
      if (AreVistaDialogsThemeSupported)
      {
        VisualStyleRenderer renderer = new VisualStyleRenderer(element);
        Rectangle textSize = renderer.GetTextExtent(deviceContext, textRect, text, flags);
        newLocation = location + new Size(0, textSize.Height);
        if (!measureOnly)
          renderer.DrawText(deviceContext, textSize, text, false, flags);
      }
      else
      {
        if (!measureOnly)
          TextRenderer.DrawText(deviceContext, text, fallbackFont, textRect, SystemColors.WindowText, flags);
        Size textSize = TextRenderer.MeasureText(deviceContext, text, fallbackFont, new Size(textRect.Width, textRect.Height), flags);
        newLocation = location + new Size(0, textSize.Height);
      }
      return newLocation;
    }
  }

  public static class CustomVisualStyleElements
  {
    public static class TaskDialog
    {
      private const string _className = "TASKDIALOG";

      private static VisualStyleElement _primaryPanel;
      private static VisualStyleElement _secondaryPanel;

      public static VisualStyleElement PrimaryPanel
      {
        get { return _primaryPanel ?? (_primaryPanel = VisualStyleElement.CreateElement(_className, 1, 0)); }
      }

      public static VisualStyleElement SecondaryPanel
      {
        get { return _secondaryPanel ?? (_secondaryPanel = VisualStyleElement.CreateElement(_className, 8, 0)); }
      }
    }

    public static class TextStyle
    {
      private const string _className = "TEXTSTYLE";

      private static VisualStyleElement _mainInstruction;
      private static VisualStyleElement _bodyText;

      public static VisualStyleElement MainInstruction
      {
        get { return _mainInstruction ?? (_mainInstruction = VisualStyleElement.CreateElement(_className, 1, 0)); }
      }

      public static VisualStyleElement BodyText
      {
        get { return _bodyText ?? (_bodyText = VisualStyleElement.CreateElement(_className, 4, 0)); }
      }

    }
  }
}
