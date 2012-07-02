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
  class TransparentLabel : Label
  {
    public double ColorOpacity { get; set; }
    public bool ApplyAntiAlias { get; set; }
    public bool DrawShadow { get; set; }
    public Color ShadowColor { get; set; }
    public double ShadowOpacity { get; set; }
    public int ShadowPixelsXOffset { get; set; }
    public int ShadowPixelsYOffset { get; set; }
    public int PixelsSpacingAdjustment { get; set; }

    public TransparentLabel()
    {
      ApplyAntiAlias = true;
      DoubleBuffered = true;
      ColorOpacity = 0.8;
      DrawShadow = true;
      ShadowColor = ForeColor;
      ShadowOpacity = 0.7;
      ShadowPixelsXOffset = 1;
      ShadowPixelsYOffset = 1;
      PixelsSpacingAdjustment = 0;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      string remainingText = Text.Trim();
      if (remainingText.Length == 0)
        return;
      if (ApplyAntiAlias)
      {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
      }
      SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(ShadowOpacity * 255), ShadowColor));
      SolidBrush textBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(ColorOpacity * 255), ForeColor));
      string textToDraw = String.Empty;
      SizeF stringSize = SizeF.Empty;
      int lengthToCut = 0;
      double trimPercentage = 0;
      Point p = e.ClipRectangle.Location;
      int width = e.ClipRectangle.Width;
      int spaceAfterPos = -1;
      int spaceBeforePos = -1;

      do
      {
        stringSize = e.Graphics.MeasureString(remainingText, Font);
        trimPercentage = width / stringSize.Width;
        if (trimPercentage < 1)
        {
          lengthToCut = Convert.ToInt32(remainingText.Length * trimPercentage);
          spaceBeforePos = lengthToCut = (lengthToCut > 0 ? lengthToCut - 1 : 0);
          spaceAfterPos = remainingText.IndexOf(" ", lengthToCut);
          textToDraw = (spaceAfterPos >= 0 ? remainingText.Substring(0, spaceAfterPos) : remainingText);
          while (spaceBeforePos > -1 && e.Graphics.MeasureString(textToDraw, Font).Width > width)
          {
            spaceBeforePos = remainingText.LastIndexOf(" ", spaceBeforePos);
            textToDraw = (spaceBeforePos >= 0 ? remainingText.Substring(0, spaceBeforePos) : textToDraw);
            spaceBeforePos--;
          }
        }
        else
          textToDraw = remainingText;
        textToDraw = textToDraw.Trim();
        if (DrawShadow)
          e.Graphics.DrawString(textToDraw, Font, shadowBrush, p.X + ShadowPixelsXOffset, p.Y + ShadowPixelsYOffset);
        e.Graphics.DrawString(textToDraw, Font, textBrush, p.X, p.Y);
        remainingText = (textToDraw.Length < remainingText.Length ? remainingText.Substring(textToDraw.Length).Trim() : String.Empty);
        p.Y += Convert.ToInt32(stringSize.Height + PixelsSpacingAdjustment);
      }
      while (remainingText.Length > 0);
      textBrush.Dispose();
      shadowBrush.Dispose();
    }
  }
}
