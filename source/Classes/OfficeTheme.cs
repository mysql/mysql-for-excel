// Copyright (c) 2019, Oracle and/or its affiliates. All rights reserved.
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

using System.Drawing;
using MySql.Utility.Classes.Attributes;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents properties for a specific Office theme.
  /// </summary>
  public class OfficeTheme
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="OfficeTheme"/> class.
    /// </summary>
    /// <param name="bodyBackgroundColor">The background color for the add-in body.</param>
    /// <param name="bodyForegroundColor">The foreground color for the add-in body.</param>
    /// <param name="controlBackgroundColor">The background color for the add-in controls.</param>
    /// <param name="controlForegroundColor">The foreground color for the add-in controls.</param>
    public OfficeTheme(Color bodyBackgroundColor, Color bodyForegroundColor, Color controlBackgroundColor, Color controlForegroundColor)
    {
      ThemeColor = ColorType.Custom;
      BodyBackgroundColor = bodyBackgroundColor;
      BodyForegroundColor = bodyForegroundColor;
      ControlBackgroundColor = controlBackgroundColor;
      ControlForegroundColor = controlForegroundColor;
    }

    /// <summary>
    /// Gets an <see cref="OfficeTheme"/> from a given theme color.
    /// </summary>
    /// <param name="themeColor">A theme color value.</param>
    /// <returns>An <see cref="OfficeTheme"/> instance, or <c>null</c> if the given theme color is unknown.</returns>
    public static OfficeTheme FromThemeColor(ColorType themeColor)
    {
      OfficeTheme officeTheme = null;
      switch (themeColor)
      {
        case ColorType.Colorful16:
          officeTheme = new OfficeTheme(ColorTranslator.FromHtml("#e6e6e6"), ColorTranslator.FromHtml("#444444"), ColorTranslator.FromHtml("#fdfdfd"), ColorTranslator.FromHtml("#444444"));
          break;

        case ColorType.DarkGray16:
          officeTheme = new OfficeTheme(ColorTranslator.FromHtml("#6a6a6a"), ColorTranslator.FromHtml("#f0f0f0"), ColorTranslator.FromHtml("#d4d4d4"), ColorTranslator.FromHtml("#262626"));
          break;

        case ColorType.Black16:
          officeTheme = new OfficeTheme(ColorTranslator.FromHtml("#262626"), ColorTranslator.FromHtml("#f0f0f0"), ColorTranslator.FromHtml("#363636"), ColorTranslator.FromHtml("#f0f0f0"));
          break;

        case ColorType.White16:
          officeTheme = new OfficeTheme(ColorTranslator.FromHtml("#ffffff"), ColorTranslator.FromHtml("#444444"), ColorTranslator.FromHtml("#fdfdfd"), ColorTranslator.FromHtml("#444444"));
          break;

        case ColorType.LightGray15:
          officeTheme = new OfficeTheme(ColorTranslator.FromHtml("#f1f1f1"), ColorTranslator.FromHtml("#5e5e5e"), ColorTranslator.FromHtml("#fdfdfd"), ColorTranslator.FromHtml("#5e5e5e"));
          break;

        case ColorType.Black14:
          officeTheme = new OfficeTheme(ColorTranslator.FromHtml("#a1a1a1"), ColorTranslator.FromHtml("#ffffff"), ColorTranslator.FromHtml("#ffffff"), ColorTranslator.FromHtml("#000000"));
          break;

        case ColorType.Blue14:
          officeTheme = new OfficeTheme(ColorTranslator.FromHtml("#deecfc"), ColorTranslator.FromHtml("#1e395b"), ColorTranslator.FromHtml("#ffffff"), ColorTranslator.FromHtml("#000000"));
          break;

        case ColorType.Silver14:
          officeTheme = new OfficeTheme(ColorTranslator.FromHtml("#e9edf1"), ColorTranslator.FromHtml("#3b3b3b"), ColorTranslator.FromHtml("#ffffff"), ColorTranslator.FromHtml("#000000"));
          break;

        case ColorType.Blue12:
          officeTheme = new OfficeTheme(ColorTranslator.FromHtml("#d5e4f2"), ColorTranslator.FromHtml("#3c67c4"), ColorTranslator.FromHtml("#ffffff"), ColorTranslator.FromHtml("#000000"));
          break;

        case ColorType.Silver12:
          officeTheme = new OfficeTheme(ColorTranslator.FromHtml("#eeeef4"), ColorTranslator.FromHtml("#000000"), ColorTranslator.FromHtml("#ffffff"), ColorTranslator.FromHtml("#000000"));
          break;

        case ColorType.Black12:
          officeTheme = new OfficeTheme(ColorTranslator.FromHtml("#eeeef4"), ColorTranslator.FromHtml("#000000"), ColorTranslator.FromHtml("#ffffff"), ColorTranslator.FromHtml("#000000"));
          break;

        case ColorType.White15:
          officeTheme = new OfficeTheme(ColorTranslator.FromHtml("#ffffff"), ColorTranslator.FromHtml("#217346"), ColorTranslator.FromHtml("#ffffff"), ColorTranslator.FromHtml("#000000"));
          break;

        case ColorType.DarkGray15:
          officeTheme = new OfficeTheme(ColorTranslator.FromHtml("#dedede"), ColorTranslator.FromHtml("#0a6332"), ColorTranslator.FromHtml("#ffffff"), ColorTranslator.FromHtml("#000000"));
          break;
      }

      if (officeTheme != null)
      {
        officeTheme.ThemeColor = themeColor;
      }

      return officeTheme;
    }

    #region Properties

    /// <summary>
    /// Gets the background color for the add-in body.
    /// </summary>
    public Color BodyBackgroundColor { get; private set; }

    /// <summary>
    /// Gets the foreground color for the add-in body.
    /// </summary>
    public Color BodyForegroundColor { get; private set; }

    /// <summary>
    /// Gets the background color for the add-in controls.
    /// </summary>
    public Color ControlBackgroundColor { get; private set; }

    /// <summary>
    /// Gets the foreground color for the add-in controls.
    /// </summary>
    public Color ControlForegroundColor { get; private set; }

    /// <summary>
    /// Gets the color theme type.
    /// </summary>
    public ColorType ThemeColor { get; private set; }

    #endregion Properties

    #region Enums

    /// <summary>
    /// Specifies identifiers to indicate the type of color theme valid for an Office version.
    /// </summary>
    public enum ColorType
    {
      /// <summary>
      /// Blue for Office 2007 (12.0).
      /// </summary>
      [NumericCode(1)]
      Blue12,

      /// <summary>
      /// Blue for Office 2010 (14.0).
      /// </summary>
      [NumericCode(1)]
      Blue14,

      /// <summary>
      /// Silver for Office 2007 (12.0).
      /// </summary>
      [NumericCode(2)]
      Silver12,

      /// <summary>
      /// Silver for Office 2010 (14.0).
      /// </summary>
      [NumericCode(2)]
      Silver14,

      /// <summary>
      /// Black for Office 2007 (12.0).
      /// </summary>
      [NumericCode(3)]
      Black12,

      /// <summary>
      /// Black for Office 2010 (14.0).
      /// </summary>
      [NumericCode(3)]
      Black14,

      /// <summary>
      /// Black for Office 2019 and 365 (16.0).
      /// </summary>
      [NumericCode(4)]
      Black16,

      /// <summary>
      /// Colorful for Office 2016, 2019 and 365 (16.0).
      /// </summary>
      [NumericCode(0)]
      Colorful16,

      /// <summary>
      /// White for Office 2013 (15.0).
      /// </summary>
      [NumericCode(0)]
      White15,

      /// <summary>
      /// White for Office 2016, 2019 and 365 (16.0).
      /// </summary>
      [NumericCode(5)]
      White16,

      /// <summary>
      /// Light gray for Office 2013 (15.0).
      /// </summary>
      [NumericCode(1)]
      LightGray15,

      /// <summary>
      /// Dark gray for Office 2013 (15.0).
      /// </summary>
      [NumericCode(2)]
      DarkGray15,

      /// <summary>
      /// Dark gray for Office 2016, 2019 and 365 (16.0).
      /// </summary>
      [NumericCode(3)]
      DarkGray16,

      /// <summary>
      /// Custom or unknown.
      /// </summary>
      Custom
    }

    #endregion Enums
  }
}
