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
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Drawing;
  using System.Drawing.Drawing2D;
  using System.Windows.Forms;
  using MySQL.Utility;

  /// <summary>
  /// Provides a label control with a variable text color opacity.
  /// </summary>
  internal class TransparentLabel : UserControl
  {
    #region Fields

    /// <summary>
    /// The formatter containing alignment and orientation information to render the text.
    /// </summary>
    private StringFormat _customFormatter;

    /// <summary>
    /// Specifies the algignment for the label text.
    /// </summary>
    private ContentAlignment _textAlign;

    /// <summary>
    /// Flag indicating whether the text size needs to be recalculated.
    /// </summary>
    private bool _textSizeRecalculationNeeded;

    /// <summary>
    /// The text to display in the control.
    /// </summary>
    private string _transparentText;

    /// <summary>
    /// List of the text lines in which the text is divided into after word wrapping.
    /// </summary>
    private List<string> _wordWrapLines;

    /// <summary>
    /// Flag indicating whether the text needs to be word wrapped and redrawn.
    /// </summary>
    private bool _wordWrapRecalculationNeeded = true;

    /// <summary>
    /// The size of the text depending on the font used.
    /// </summary>
    private SizeF _textSize;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="TransparentLabel"/> class.
    /// </summary>
    public TransparentLabel()
    {
      _customFormatter = null;
      _textAlign = ContentAlignment.TopLeft;
      _textSizeRecalculationNeeded = true;
      _textSizeRecalculationNeeded = true;
      _transparentText = string.Empty;
      _textSize = SizeF.Empty;

      AutoEllipsis = false;
      ApplyAntiAlias = false;
      DoubleBuffered = true;
      TextOpacity = 1.0;
      DrawShadow = false;
      ShadowColor = ForeColor;
      ShadowOpacity = 0.5;
      ShadowPixelsXOffset = 1;
      ShadowPixelsYOffset = 1;
      PixelsSpacingAdjustment = 0;

      UpdateTextFormatter();
    }

    #region Properties

    /// <summary>
    /// Gets or sets a valie indicating whether anti-aliasing is applied when rendering the text.
    /// </summary>
    [Category("Appearance"), DefaultValue(false), Description("Applies anti-aliasing when rendering the text.")]
    public bool ApplyAntiAlias { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the automatic handling of text that extends beyond the width of the label control is enabled.
    /// </summary>
    [Category("Behavior"), DefaultValue(false), Description("Enables the automatic handling of text that extends beyond the width of the label control.")]
    public bool AutoEllipsis { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a copy of the text with an offset to emulate a shadow is drawn.
    /// </summary>
    [Category("Appearance"), DefaultValue(false), Description("Draws a copy of the text with an offset to emulate a shadow.")]
    public bool DrawShadow { get; set; }

    /// <summary>
    /// Gets or sets the pixels to add as a vertical spacing between lines to adjust spacing.
    /// </summary>
    [Category("Appearance"), DefaultValue(0), Description("Pixels to add as a vertical spacing between lines to adjust spacing.")]
    public int PixelsSpacingAdjustment { get; set; }

    /// <summary>
    /// Gets or sets the color of the shadow text.
    /// </summary>
    [Category("Appearance"), Description("Color of the shadow text.")]
    public Color ShadowColor { get; set; }

    /// <summary>
    /// Gets or sets the opacity factor of the shadow ranging from 0 to 1.
    /// </summary>
    [Category("Appearance"), DefaultValue(0.5), Description("Opacity factor of the shadow ranging from 0 to 1.")]
    public double ShadowOpacity { get; set; }

    /// <summary>
    /// Gets or sets the horizontal offset for the shadow text given in pixels.
    /// </summary>
    [Category("Appearance"), DefaultValue(1), Description("Horizontal offset for the shadow text given in pixels.")]
    public int ShadowPixelsXOffset { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset for the shadow text given in pixels.
    /// </summary>
    [Category("Appearance"), DefaultValue(1), Description("Vertical offset for the shadow text given in pixels.")]
    public int ShadowPixelsYOffset { get; set; }

    /// <summary>
    /// Gets or sets a <see cref="ContentAlignment"/> determining the position of the text within the label.
    /// </summary>
    [Category("Appearance"), DefaultValue(ContentAlignment.TopLeft), Description("Determines the position of the text within the label.")]
    public ContentAlignment TextAlign
    {
      get
      {
        return _textAlign;
      }

      set
      {
        bool valueChanged = _textAlign != value;
        _textAlign = value;
        if (valueChanged)
        {
          UpdateTextFormatter();
        }
      }
    }

    /// <summary>
    /// Gets pr sets the opacity factor of the text ranging from 0 to 1.
    /// </summary>
    [Category("Appearance"), DefaultValue(1.0), Description("Opacity factor of the text ranging from 0 to 1.")]
    public double TextOpacity { get; set; }

    /// <summary>
    /// Gets or sets the text to display in the control.
    /// </summary>
    [Category("Appearance"), DefaultValue(""), Description("The text to display in the control.")]
    public string TransparentText
    {
      get
      {
        return _transparentText;
      }

      set
      {
        bool valueChanged = _transparentText != value;
        _transparentText = value;
        if (valueChanged)
        {
          _wordWrapRecalculationNeeded = true;
          _textSizeRecalculationNeeded = true;
        }
      }
    }

    #endregion Properties

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="Control"/> and its child controls and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (_customFormatter != null)
        {
          _customFormatter.Dispose();
        }
      }

      base.Dispose(disposing);
    }

    /// <summary>
    /// Raises the <see cref="AutoSizeChanged"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected override void OnAutoSizeChanged(EventArgs e)
    {
      base.OnAutoSizeChanged(e);
      _wordWrapRecalculationNeeded = true;
    }

    /// <summary>
    /// Raises the <see cref="FontChanged"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected override void OnFontChanged(EventArgs e)
    {
      base.OnFontChanged(e);
      _textSizeRecalculationNeeded = true;
    }

    /// <summary>
    /// Raises the <see cref="Load"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      _textSizeRecalculationNeeded = true;
      _wordWrapRecalculationNeeded = true;
    }

    /// <summary>
    /// Raises the <see cref="Paint"/> event.
    /// </summary>
    /// <param name="e">A <see cref="PaintEventArgs"/> that contains the event data.</param>
    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);

      e.Graphics.SmoothingMode = ApplyAntiAlias ? SmoothingMode.AntiAlias : SmoothingMode.HighQuality;
      if (_textSizeRecalculationNeeded)
      {
        _textSize = e.Graphics.MeasureString(_transparentText, Font);
        _textSizeRecalculationNeeded = false;
      }

      if (_wordWrapRecalculationNeeded)
      {
        _wordWrapLines = this.WordWrapText(_transparentText);
        _wordWrapRecalculationNeeded = false;
      }

      if (_wordWrapLines == null || _wordWrapLines.Count == 0)
      {
        return;
      }

      SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(ShadowOpacity * 255), ShadowColor));
      SolidBrush textBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(TextOpacity * 255), ForeColor));
      Point p = e.ClipRectangle.Location;

      switch (_customFormatter.Alignment)
      {
        case StringAlignment.Center:
          p.X += e.ClipRectangle.Width / 2;
          break;

        case StringAlignment.Far:
          p.X += e.ClipRectangle.Width;
          break;
      }

      foreach (string lineText in _wordWrapLines)
      {
        if (DrawShadow)
        {
          e.Graphics.DrawString(lineText, Font, shadowBrush, p.X + ShadowPixelsXOffset, p.Y + ShadowPixelsYOffset, _customFormatter);
        }

        e.Graphics.DrawString(lineText, Font, textBrush, p.X, p.Y, _customFormatter);
        p.Y += Convert.ToInt32(_textSize.Height + PixelsSpacingAdjustment);
      }

      textBrush.Dispose();
      shadowBrush.Dispose();
    }

    /// <summary>
    /// Raises the <see cref="Resize"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);
      _wordWrapRecalculationNeeded = true;
    }

    /// <summary>
    /// Updates the text formatter to reflect the value in the <see cref="TextAlign"/> property.
    /// </summary>
    private void UpdateTextFormatter()
    {
      if (_customFormatter == null)
      {
        _customFormatter = new StringFormat();
      }

      if (AutoEllipsis)
      {
        _customFormatter.Trimming = StringTrimming.EllipsisWord;
      }

      switch (TextAlign)
      {
        case ContentAlignment.BottomCenter:
          _customFormatter.Alignment = StringAlignment.Center;
          _customFormatter.LineAlignment = StringAlignment.Far;
          break;

        case ContentAlignment.BottomLeft:
          _customFormatter.Alignment = StringAlignment.Near;
          _customFormatter.LineAlignment = StringAlignment.Far;
          break;

        case ContentAlignment.BottomRight:
          _customFormatter.Alignment = StringAlignment.Far;
          _customFormatter.LineAlignment = StringAlignment.Far;
          break;

        case ContentAlignment.MiddleCenter:
          _customFormatter.Alignment = StringAlignment.Center;
          _customFormatter.LineAlignment = StringAlignment.Center;
          break;

        case ContentAlignment.MiddleLeft:
          _customFormatter.Alignment = StringAlignment.Near;
          _customFormatter.LineAlignment = StringAlignment.Center;
          break;

        case ContentAlignment.MiddleRight:
          _customFormatter.Alignment = StringAlignment.Far;
          _customFormatter.LineAlignment = StringAlignment.Center;
          break;

        case ContentAlignment.TopCenter:
          _customFormatter.Alignment = StringAlignment.Center;
          _customFormatter.LineAlignment = StringAlignment.Near;
          break;

        case ContentAlignment.TopLeft:
          _customFormatter.Alignment = StringAlignment.Near;
          _customFormatter.LineAlignment = StringAlignment.Near;
          break;

        case ContentAlignment.TopRight:
          _customFormatter.Alignment = StringAlignment.Far;
          _customFormatter.LineAlignment = StringAlignment.Near;
          break;
      }
    }
  }
}