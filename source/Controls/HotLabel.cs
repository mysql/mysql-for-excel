// Copyright (c) 2012, 2016, Oracle and/or its affiliates. All rights reserved.
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MySql.Utility.Classes;

namespace MySQL.ForExcel.Controls
{
  /// <summary>
  /// Provides a text label that can be clicked to perform an action.
  /// </summary>
  public sealed partial class HotLabel : UserControl
  {
    #region Fields

    /// <summary>
    /// The mouse button pressed by the user wheb clicking on the label.
    /// </summary>
    private MouseButtons _downButton;

    /// <summary>
    /// Flag indicating whether the mouse is hovering over (tracking) the label.
    /// </summary>
    private bool _tracking;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="HotLabel"/> class.
    /// </summary>
    public HotLabel()
    {
      InitializeComponent();

      Behavior = BehaviorType.Label;
      CheckedState = CheckState.Indeterminate;
      DoubleBuffered = true;
      DrawShadow = false;
      TitleColor = SystemColors.WindowText;
      DescriptionColor = SystemColors.GrayText;
      TitleColorOpacity = 0.7;
      DescriptionColorOpacity = 0.7;
      TitleShadowOpacity = 0.3;
      DescriptionShadowOpacity = 0.3;
      TitleShadowXOffset = 0;
      TitleShadowYOffset = 1;
      DescriptionShadowXOffset = 0;
      DescriptionShadowYOffset = 1;
      TitleDescriptionPixelsSpacing = 4;
      ImagePixelsXOffset = 0;
      ImagePixelsYOffset = 0;
      TitleXOffset = 0;
      TitleYOffset = 3;

      FontFamily family = Parent != null ? Parent.Font.FontFamily : FontFamily.GenericSansSerif;
      float size = Parent != null ? Parent.Font.Size : 8.25f;
      Font = new Font(family, size * 1.25f, FontStyle.Bold);
      DescriptionFont = new Font(Font.FontFamily, Font.Size * 0.5f, FontStyle.Regular);
    }

    #region Enums

    /// <summary>
    /// Specifies identifiers to indicate the type of behavior of the <see cref="HotLabel"/> control.
    /// </summary>
    public enum BehaviorType
    {
      /// <summary>
      /// The control behaves as a button, when clicked an action is triggered.
      /// </summary>
      Button,

      /// <summary>
      /// The control behaves as a checkbox, when clicked the status changes to checked or unchecked.
      /// </summary>
      CheckBox,

      /// <summary>
      /// The control behaves as a label, clicking it has no effect.
      /// </summary>
      Label
    }

    #endregion Enums

    #region Properties

    /// <summary>
    /// Gets or sets the type of behavior of the <see cref="HotLabel"/> control.
    /// </summary>
    [Category("MySQL Custom"), Description("The type of behavior of the HotLabel control.")]
    public BehaviorType Behavior { get; set; }

    /// <summary>
    /// Gets or sets the image used when the state of the control is checked.
    /// </summary>
    [Category("MySQL Custom"), Description("The image used when the state of the control is checked.")]
    public Image CheckedImage { get; set; }

    /// <summary>
    /// Gets the state of the control, that can be checked, unchecked or set to an indeterminate state.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public CheckState CheckedState { get; private set; }

    /// <summary>
    /// Gets or sets the description text appearing below the title.
    /// </summary>
    [Category("MySQL Custom"), Description("The description text appearing below the title.")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the color used to paint the description text.
    /// </summary>
    [Category("MySQL Custom"), Description("The color used to paint the description text.")]
    public Color DescriptionColor { get; set; }

    /// <summary>
    /// Gets or sets the opacity factor for the color used to paint the description text.
    /// </summary>
    [Category("MySQL Custom"), Description("The opacity factor for the color used to paint the description text.")]
    public double DescriptionColorOpacity { get; set; }

    /// <summary>
    /// Gets or sets the font used to paint the description text.
    /// </summary>
    [Category("MySQL Custom"), Description("The font used to paint the description text.")]
    public Font DescriptionFont { get; set; }

    /// <summary>
    /// Gets or sets the opacity factor for the shadow of the description text.
    /// </summary>
    [Category("MySQL Custom"), Description("The opacity factor for the shadow of the description text.")]
    public double DescriptionShadowOpacity { get; set; }

    /// <summary>
    /// Gets or sets the horizontal offset, in pixels, for the shadow of the description text.
    /// </summary>
    [Category("MySQL Custom"), Description("The horizontal offset, in pixels, for the shadow of the description text.")]
    public int DescriptionShadowXOffset { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset for the shadow of the description text.
    /// </summary>
    [Category("MySQL Custom"), Description("The vertical offset for the shadow of the description text.")]
    public int DescriptionShadowYOffset { get; set; }

    /// <summary>
    /// Gets or sets the image used when the control is not enabled.
    /// </summary>
    [Category("MySQL Custom"), Description("The image used when the control is not enabled.")]
    public Image DisabledImage { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a shadow is drawn beneath the text.
    /// </summary>
    [Category("MySQL Custom"), Description("A value indicating whether a shadow is drawn beneath the text.")]
    public bool DrawShadow { get; set; }

    /// <summary>
    /// Gets or sets the image displayed at the left side of the label.
    /// </summary>
    [Category("MySQL Custom"), Description("The image displayed at the left side of the label.")]
    public Image Image { get; set; }

    /// <summary>
    /// Gets or sets the horizontal offset, in pixels, from the upper-left corner of the label.
    /// </summary>
    [Category("MySQL Custom"), Description("The horizontal offset, in pixels, from the upper-left corner of the label.")]
    public int ImagePixelsXOffset { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset, in pixels, from the upper-left corner of the label.
    /// </summary>
    [Category("MySQL Custom"), Description("The vertical offset, in pixels, from the upper-left corner of the label.")]
    public int ImagePixelsYOffset { get; set; }

    /// <summary>
    /// Gets or sets the title text.
    /// </summary>
    [Category("MySQL Custom"), Description("The title text.")]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the color used to paint the title text.
    /// </summary>
    [Category("MySQL Custom"), Description("The color used to paint the title text.")]
    public Color TitleColor { get; set; }

    /// <summary>
    /// Gets or sets the opacity factor for the color used to paint the title text.
    /// </summary>
    [Category("MySQL Custom"), Description("The opacity factor for the color used to paint the title text.")]
    public double TitleColorOpacity { get; set; }

    /// <summary>
    /// Gets or sets a spacing, in pixels, between the title and its description.
    /// </summary>
    [Category("MySQL Custom"), Description("The spacing, in pixels, between the title and its description.")]
    public int TitleDescriptionPixelsSpacing { get; set; }

    /// <summary>
    /// Gets or sets the horizontal offset, in pixels, for the title text.
    /// </summary>
    [Category("MySQL Custom"), Description("The horizontal offset, in pixels, for the title text.")]
    public int TitleXOffset { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset, in pixels, for the title text.
    /// </summary>
    [Category("MySQL Custom"), Description("The vertical offset, in pixels, for the title text.")]
    public int TitleYOffset { get; set; }

    /// <summary>
    /// Gets or sets the opacity factor for the shadow of the title text.
    /// </summary>
    [Category("MySQL Custom"), Description("The opacity factor for the shadow of the title text.")]
    public double TitleShadowOpacity { get; set; }

    /// <summary>
    /// Gets or sets the horizontal offset, in pixels, for the shadow of the title text.
    /// </summary>
    [Category("MySQL Custom"), Description("The horizontal offset, in pixels, for the shadow of the title text.")]
    public int TitleShadowXOffset { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset, in pixels, for the shadow of the title text.
    /// </summary>
    [Category("MySQL Custom"), Description("The vertical offset, in pixels, for the shadow of the title text.")]
    public int TitleShadowYOffset { get; set; }

    /// <summary>
    /// Gets a value indicating whether this control should redraw its surface using a secondary buffer to reduce or prevent flicker.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    protected override bool DoubleBuffered
    {
      get
      {
        return base.DoubleBuffered;
      }

      set
      {
        base.DoubleBuffered = value;
      }
    }

    #endregion Properties

    /// <summary>
    /// Raises the <see cref="Control.Click"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected override void OnClick(EventArgs e)
    {
      if (_downButton != MouseButtons.Left || Behavior == BehaviorType.Label)
      {
        return;
      }

      if (Behavior == BehaviorType.CheckBox)
      {
        CheckedState = CheckedState != CheckState.Checked ? CheckState.Checked : CheckState.Unchecked;
        Refresh();
      }

      base.OnClick(e);
    }

    /// <summary>
    /// Raises the <see cref="Control.MouseDown"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected override void OnMouseDown(MouseEventArgs e)
    {
      base.OnMouseDown(e);
      _downButton = e.Button;
    }

    /// <summary>
    /// Raises the <see cref="Control.MouseEnter"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected override void OnMouseEnter(EventArgs e)
    {
      base.OnMouseEnter(e);
      if (Behavior == BehaviorType.Label)
      {
        return;
      }

      _tracking = true;
      Refresh();
    }

    /// <summary>
    /// Raises the <see cref="Control.MouseLeave"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected override void OnMouseLeave(EventArgs e)
    {
      base.OnMouseLeave(e);
      if (Behavior == BehaviorType.Label)
      {
        return;
      }

      _tracking = false;
      Refresh();
    }

    /// <summary>
    /// Raises the <see cref="Control.Paint"/> event.
    /// </summary>
    /// <param name="e">A <see cref="PaintEventArgs"/> that contains the event data.</param>
    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);

      Image i = Enabled
        ? (Behavior != BehaviorType.CheckBox
          ? Image
          : (CheckedState == CheckState.Checked ? CheckedImage : Image))
        : (DisabledImage == null && Image != null ? new Bitmap(Image).ToGrayscale() : DisabledImage);
      Size imageSize = Size.Empty;
      if (i != null)
      {
        imageSize = i.Size;
        int y = (Height - imageSize.Height) / 2;
        e.Graphics.DrawImage(i, ImagePixelsXOffset, y + ImagePixelsYOffset, imageSize.Width, imageSize.Height);
      }

      Point pt = new Point(imageSize.Width + TitleXOffset, TitleYOffset);
      if (!string.IsNullOrEmpty(Title))
      {
        Color currentTitleColor = _tracking ? SystemColors.HotTrack : TitleColor;
        if (DrawShadow && Enabled)
        {
          using (var titleShadowBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(TitleShadowOpacity * 255), TitleColor)))
          {
            e.Graphics.DrawString(Title, Font, titleShadowBrush, pt.X + TitleShadowXOffset, pt.Y + TitleShadowYOffset);
          }
        }

        using (var titleBrush = Enabled
            ? new SolidBrush(Color.FromArgb(Convert.ToInt32(TitleColorOpacity*255), currentTitleColor))
            : new SolidBrush(Color.FromArgb(80, 0, 0, 0)))
        {
          e.Graphics.DrawString(Title, Font, titleBrush, pt.X, pt.Y);
        }

        SizeF stringSize = e.Graphics.MeasureString(Title, Font);
        pt.Y += (int)(stringSize.Height + TitleDescriptionPixelsSpacing);
      }

      if (string.IsNullOrEmpty(Description))
      {
        return;
      }

      if (DrawShadow && Enabled)
      {
        using (var descriptionShadowBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(DescriptionShadowOpacity*255), Color.White)))
        {
          e.Graphics.DrawString(Description, DescriptionFont, descriptionShadowBrush,
            pt.X + DescriptionShadowXOffset, pt.Y + DescriptionShadowYOffset);
        }
      }

      using (var descriptionBrush = Enabled
          ? new SolidBrush(Color.FromArgb(Convert.ToInt32(DescriptionColorOpacity*255), DescriptionColor))
          : new SolidBrush(Color.FromArgb(80, 0, 0, 0)))
      {
        e.Graphics.DrawString(Description, DescriptionFont, descriptionBrush, pt.X, pt.Y);
      }
    }
  }
}