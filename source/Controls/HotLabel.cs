using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySQL.ForExcel.Controls
{
  public partial class HotLabel : UserControl
  {
    private Image image;
    private Image disabledImage;
    private bool tracking;
    private SolidBrush titleBrush;
    private SolidBrush trackingTitleBrush;
    private SolidBrush descriptionBrush;

    public HotLabel()
    {
      InitializeComponent();
      DoubleBuffered = true;
      HotTracking = true;

      FontFamily family = Parent != null && Parent.Font != null ? Parent.Font.FontFamily : FontFamily.GenericSansSerif;
      float size = Parent != null && Parent.Font != null ? Parent.Font.Size : 8.25f;
      Font = new Font(family, size * 1.25f, FontStyle.Bold);
      DescriptionFont = new Font(Font.FontFamily, Font.Size * 0.5f, FontStyle.Regular);
      titleBrush = new SolidBrush(SystemColors.WindowText);
      trackingTitleBrush = new SolidBrush(SystemColors.HotTrack);
      descriptionBrush = new SolidBrush(SystemColors.GrayText);
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

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);

      Image i = Enabled ? Image : disabledImage;
      if (i != null)
      {
        int y = (Height - ImageSize.Height) / 2;
        e.Graphics.DrawImage(i, 5, y, ImageSize.Width, ImageSize.Height);
      }
      Point pt = new Point(10 + ImageSize.Width, 5);
      if (!String.IsNullOrEmpty(Title))
      {
        e.Graphics.DrawString(Title, Font, tracking ? trackingTitleBrush : titleBrush, pt.X, pt.Y);
        SizeF stringSize = e.Graphics.MeasureString(Title, Font);
        pt.Y += (int)(stringSize.Height + 3);
      }
      if (!String.IsNullOrEmpty(Description))
        e.Graphics.DrawString(Description, DescriptionFont, descriptionBrush, pt.X, pt.Y);
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
