using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace MySQL.ForExcel
{
  class TransparentLabel : UserControl
  {
    private string transparentText = String.Empty;
    private List<string> wordWrapLines;
    private StringFormat customFormatter;
    private ContentAlignment textAlign = ContentAlignment.TopLeft;
    private bool wordWrapRecalculationNeeded = true;
    private bool textSizeRecalculationNeeded = true;
    private SizeF textSize = SizeF.Empty;

    [Category("Appearance"), DefaultValue(""), Description("The text to display in the control.")]
    public string TransparentText
    {
      get { return transparentText; }
      set
      {
        bool valueChanged = transparentText != value;
        transparentText = value;
        if (valueChanged)
        {
          wordWrapRecalculationNeeded = true;
          textSizeRecalculationNeeded = true;
        }
      }
    }

    [Category("Appearance"), DefaultValue(ContentAlignment.TopLeft), Description("Determines the position of the text within the label.")]
    public ContentAlignment TextAlign
    {
      get { return textAlign; }
      set
      {
        bool valueChanged = textAlign != value;
        textAlign = value;
        if (valueChanged)
          UpdateTextFormatter();
      }
    }

    [Category("Behavior"), DefaultValue(false), Description("Enables the automatic handling of text that extends beyond the width of the label control.")]
    public bool AutoEllipsis { get; set; }

    [Category("Appearance"), DefaultValue(1.0), Description("Opacity factor of the text ranging from 0 - 1.")]
    public double TextOpacity { get; set; }

    [Category("Appearance"), DefaultValue(false), Description("Applies anti-aliasing when rendering the text.")]
    public bool ApplyAntiAlias { get; set; }

    [Category("Appearance"), DefaultValue(false), Description("Draws a copy of the text with an offset to emulate a shadow.")]
    public bool DrawShadow { get; set; }

    [Category("Appearance"), Description("Color of the shadow text.")]
    public Color ShadowColor { get; set; }

    [Category("Appearance"), DefaultValue(0.5), Description("Opacity factor of the shadow ranging from 0 - 1.")]
    public double ShadowOpacity { get; set; }

    [Category("Appearance"), DefaultValue(1), Description("Horizontal offset for the shadow text given in pixels.")]
    public int ShadowPixelsXOffset { get; set; }

    [Category("Appearance"), DefaultValue(1), Description("Vertical offset for the shadow text given in pixels.")]
    public int ShadowPixelsYOffset { get; set; }

    [Category("Appearance"), DefaultValue(0), Description("Pixels to add as a vertical spacing between lines to adjust spacing.")]
    public int PixelsSpacingAdjustment { get; set; }

    public TransparentLabel()
    {
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

    private void UpdateTextFormatter()
    {
      if (customFormatter == null)
        customFormatter = new StringFormat();
      if (AutoEllipsis)
        customFormatter.Trimming = StringTrimming.EllipsisWord;
      switch (TextAlign)
      {
        case ContentAlignment.BottomCenter:
          customFormatter.Alignment = StringAlignment.Center;
          customFormatter.LineAlignment = StringAlignment.Far;
          break;
        case ContentAlignment.BottomLeft:
          customFormatter.Alignment = StringAlignment.Near;
          customFormatter.LineAlignment = StringAlignment.Far;
          break;
        case ContentAlignment.BottomRight:
          customFormatter.Alignment = StringAlignment.Far;
          customFormatter.LineAlignment = StringAlignment.Far;
          break;
        case ContentAlignment.MiddleCenter:
          customFormatter.Alignment = StringAlignment.Center;
          customFormatter.LineAlignment = StringAlignment.Center;
          break;
        case ContentAlignment.MiddleLeft:
          customFormatter.Alignment = StringAlignment.Near;
          customFormatter.LineAlignment = StringAlignment.Center;
          break;
        case ContentAlignment.MiddleRight:
          customFormatter.Alignment = StringAlignment.Far;
          customFormatter.LineAlignment = StringAlignment.Center;
          break;
        case ContentAlignment.TopCenter:
          customFormatter.Alignment = StringAlignment.Center;
          customFormatter.LineAlignment = StringAlignment.Near;
          break;
        case ContentAlignment.TopLeft:
          customFormatter.Alignment = StringAlignment.Near;
          customFormatter.LineAlignment = StringAlignment.Near;
          break;
        case ContentAlignment.TopRight:
          customFormatter.Alignment = StringAlignment.Far;
          customFormatter.LineAlignment = StringAlignment.Near;
          break;
      }
    }

    private void WordWrapText(Graphics graphics)
    {
      if (wordWrapLines == null)
        wordWrapLines = new List<string>();
      wordWrapLines.Clear();

      if (AutoSize)
      {
        wordWrapLines.Add(transparentText);
        return;
      }

      string remainingText = transparentText.Trim();
      string textToDraw = String.Empty;
      SizeF stringSize = SizeF.Empty;
      int lengthToCut = 0;
      double trimPercentage = 0;
      int spaceAfterPos = -1;
      int spaceBeforePos = -1;

      do
      {
        stringSize = graphics.MeasureString(remainingText, Font);
        trimPercentage = Width / stringSize.Width;
        if (trimPercentage < 1)
        {
          lengthToCut = Convert.ToInt32(remainingText.Length * trimPercentage);
          spaceBeforePos = lengthToCut = (lengthToCut > 0 ? lengthToCut - 1 : 0);
          spaceAfterPos = remainingText.IndexOf(" ", lengthToCut);
          textToDraw = (spaceAfterPos >= 0 ? remainingText.Substring(0, spaceAfterPos) : remainingText);
          while (spaceBeforePos > -1 && graphics.MeasureString(textToDraw, Font).Width > Width)
          {
            spaceBeforePos = remainingText.LastIndexOf(" ", spaceBeforePos);
            textToDraw = (spaceBeforePos >= 0 ? remainingText.Substring(0, spaceBeforePos) : textToDraw);
            spaceBeforePos--;
          }
        }
        else
          textToDraw = remainingText;
        textToDraw = textToDraw.Trim();
        if (textToDraw.Length > 0)
          wordWrapLines.Add(textToDraw);
        remainingText = (textToDraw.Length < remainingText.Length ? remainingText.Substring(textToDraw.Length).Trim() : String.Empty);
      }
      while (remainingText.Length > 0);
      wordWrapRecalculationNeeded = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (customFormatter != null)
          customFormatter.Dispose();
      }
      base.Dispose(disposing);
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      textSizeRecalculationNeeded = true;
      wordWrapRecalculationNeeded = true;
    }

    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);
      wordWrapRecalculationNeeded = true;
    }

    protected override void OnAutoSizeChanged(EventArgs e)
    {
      base.OnAutoSizeChanged(e);
      wordWrapRecalculationNeeded = true;
    }

    protected override void OnFontChanged(EventArgs e)
    {
      base.OnFontChanged(e);
      textSizeRecalculationNeeded = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);

      if (ApplyAntiAlias)
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
      else
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

      if (textSizeRecalculationNeeded)
      {
        textSize = e.Graphics.MeasureString(transparentText, Font);
        textSizeRecalculationNeeded = false;
      }
      if (wordWrapRecalculationNeeded)
        WordWrapText(e.Graphics);

      if (wordWrapLines == null || wordWrapLines.Count == 0)
        return;
      
      SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(ShadowOpacity * 255), ShadowColor));
      SolidBrush textBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(TextOpacity * 255), ForeColor));
      Point p = e.ClipRectangle.Location;

      switch (customFormatter.Alignment)
      {
        case StringAlignment.Center:
          p.X += e.ClipRectangle.Width / 2;
          break;
        case StringAlignment.Far:
          p.X += e.ClipRectangle.Width;
          break;
      }

      foreach (string lineText in wordWrapLines)
      {
        if (DrawShadow)
          e.Graphics.DrawString(lineText, Font, shadowBrush, p.X + ShadowPixelsXOffset, p.Y + ShadowPixelsYOffset, customFormatter);
        e.Graphics.DrawString(lineText, Font, textBrush, p.X, p.Y, customFormatter);
        p.Y += Convert.ToInt32(textSize.Height + PixelsSpacingAdjustment);
      }

      textBrush.Dispose();
      shadowBrush.Dispose();
    }
  }
}
