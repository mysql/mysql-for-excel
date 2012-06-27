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
  public partial class HotLabel : UserControl
  {
    private Image image;
    private Image disabledImage;
    private bool tracking;

    public HotLabel()
    {
      InitializeComponent();
      DoubleBuffered = true;
      HotTracking = true;
      DrawShadow = false;
      TitleColor = SystemColors.WindowText;
      DescriptionColor = SystemColors.GrayText;
      TitleColorOpacity = 0.7;
      DescriptionColorOpacity = 0.7;
      TitleShadowOpacity = 0.3;
      DescriptionShadowOpacity = 0.3;
      TitleShadowPixelsXOffset = 0;
      TitleShadowPixelsYOffset = 1;
      DescriptionShadowPixelsXOffset = 0;
      DescriptionShadowPixelsYOffset = 1;
      TitleDescriptionPixelsSpacing = 4;
      ImagePixelsXOffset = 0;
      TitlePixelsXOffset = 0;
      TitlePixelsYOffset = 3;

      FontFamily family = Parent != null && Parent.Font != null ? Parent.Font.FontFamily : FontFamily.GenericSansSerif;
      float size = Parent != null && Parent.Font != null ? Parent.Font.Size : 8.25f;
      Font = new Font(family, size * 1.25f, FontStyle.Bold);
      DescriptionFont = new Font(Font.FontFamily, Font.Size * 0.5f, FontStyle.Regular);
    }

    public Image Image
    {
      get { return image; }
      set 
      { 
        image = value; 
        disabledImage = Utilities.MakeGrayscale(new Bitmap(Image));
        if (ImageSize.IsEmpty)
          ImageSize = image.Size;
      }
    }

    public Size ImageSize { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Font DescriptionFont { get; set; }
    public bool HotTracking { get; set; }
    public bool DrawShadow { get; set; }
    public Color TitleColor { get; set; }
    public double TitleColorOpacity { get; set; }
    public int TitleDescriptionPixelsSpacing { get; set; }
    public Color DescriptionColor { get; set; }
    public double DescriptionColorOpacity { get; set; }
    public double TitleShadowOpacity { get; set; }
    public int TitleShadowPixelsXOffset { get; set; }
    public int TitleShadowPixelsYOffset { get; set; }
    public double DescriptionShadowOpacity { get; set; }
    public int DescriptionShadowPixelsXOffset { get; set; }
    public int DescriptionShadowPixelsYOffset { get; set; }
    public int ImagePixelsXOffset { get; set; }
    public int TitlePixelsXOffset { get; set; }
    public int TitlePixelsYOffset { get; set; }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);

      Image i = Enabled ? Image : disabledImage;
      if (i != null)
      {
        int y = (Height - ImageSize.Height) / 2;
        e.Graphics.DrawImage(i, ImagePixelsXOffset, y, ImageSize.Width, ImageSize.Height);
      }
      Point pt = new Point(ImageSize.Width + TitlePixelsXOffset, TitlePixelsYOffset);
      if (!String.IsNullOrEmpty(Title))
      {
        if (DrawShadow)
        {
          SolidBrush titleShadowBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(TitleShadowOpacity * 255), TitleColor));
          e.Graphics.DrawString(Title, Font, titleShadowBrush, pt.X + TitleShadowPixelsXOffset, pt.Y + TitleShadowPixelsYOffset);
          titleShadowBrush.Dispose();
        }
        Color currentTitleColor = (tracking ? SystemColors.HotTrack : TitleColor);
        SolidBrush titleBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(TitleColorOpacity * 255), currentTitleColor));
        e.Graphics.DrawString(Title, Font, titleBrush, pt.X, pt.Y);
        titleBrush.Dispose();
        
        SizeF stringSize = e.Graphics.MeasureString(Title, Font);
        pt.Y += (int)(stringSize.Height + TitleDescriptionPixelsSpacing);
      }
      if (!String.IsNullOrEmpty(Description))
      {
        if (DrawShadow)
        {
          SolidBrush descriptionShadowBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(DescriptionShadowOpacity * 255), Color.White));
          e.Graphics.DrawString(Description, DescriptionFont, descriptionShadowBrush, pt.X + DescriptionShadowPixelsXOffset, pt.Y + DescriptionShadowPixelsYOffset);
          descriptionShadowBrush.Dispose();
        }
        SolidBrush descriptionBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(DescriptionColorOpacity * 255), DescriptionColor));
        e.Graphics.DrawString(Description, DescriptionFont, descriptionBrush, pt.X, pt.Y);
        descriptionBrush.Dispose();
      }
    }

    protected override void OnMouseEnter(EventArgs e)
    {
      base.OnMouseEnter(e);
      if (!HotTracking) return;
      tracking = true;
      Refresh();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
      base.OnMouseLeave(e);
      if (!HotTracking) return;
      tracking = false;
      Refresh();
    }
  }
}
