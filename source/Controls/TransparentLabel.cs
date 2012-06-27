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
      if (ApplyAntiAlias)
      {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
      }
      SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(ShadowOpacity * 255), ShadowColor));
      SolidBrush textBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(ColorOpacity * 255), ForeColor));
      string textToDraw = String.Empty;
      string remainingText = Text;
      SizeF stringSize = SizeF.Empty;
      float delta = 0;
      int lengthToCut = 0;
      double deltaPercentage = 0;
      Point p = e.ClipRectangle.Location;
      int width = e.ClipRectangle.Width;
      int spacePos = -1;
      do
      {
        stringSize = e.Graphics.MeasureString(remainingText, Font);
        delta = stringSize.Width - width;
        deltaPercentage = (delta > 0 ? 1 - delta / stringSize.Width : 0);
        lengthToCut = Convert.ToInt32(remainingText.Length * deltaPercentage);
        spacePos = (lengthToCut > 0 ? remainingText.LastIndexOf(" ", lengthToCut - 1) : -1);
        lengthToCut = (spacePos > -1 ? spacePos  : lengthToCut);
        textToDraw = (lengthToCut > 0 ? remainingText.Substring(0, lengthToCut) : remainingText);
        if (DrawShadow)
          e.Graphics.DrawString(textToDraw, Font, shadowBrush, p.X + ShadowPixelsXOffset, p.Y + ShadowPixelsYOffset);
        e.Graphics.DrawString(textToDraw, Font, textBrush, p.X, p.Y);
        remainingText = (lengthToCut > 0 ? remainingText.Substring(lengthToCut) : String.Empty);
        p.Y += Convert.ToInt32(stringSize.Height + PixelsSpacingAdjustment);
      }
      while (remainingText.Length > 0 && stringSize.Width > width);
      textBrush.Dispose();
      shadowBrush.Dispose();
    }
  }
}
