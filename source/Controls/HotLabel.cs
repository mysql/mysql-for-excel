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
  public partial class HotLabel : UserControl
  {
    private bool tracking;
    private MouseButtons downButton;

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
      ImagePixelsYOffset = 0;
      TitlePixelsXOffset = 0;
      TitlePixelsYOffset = 3;

      FontFamily family = Parent != null && Parent.Font != null ? Parent.Font.FontFamily : FontFamily.GenericSansSerif;
      float size = Parent != null && Parent.Font != null ? Parent.Font.Size : 8.25f;
      Font = new Font(family, size * 1.25f, FontStyle.Bold);
      DescriptionFont = new Font(Font.FontFamily, Font.Size * 0.5f, FontStyle.Regular);
    }

    public Image Image { get; set; }
    public Image DisabledImage { get; set; }
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
    public int ImagePixelsYOffset { get; set; }
    public int TitlePixelsXOffset { get; set; }
    public int TitlePixelsYOffset { get; set; }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);

      Image i = (Enabled ? Image : (DisabledImage == null && Image != null ? MiscUtilities.MakeGrayscale(new Bitmap(Image)) : DisabledImage));
      Size imageSize = Size.Empty;
      if (i != null)
      {
        imageSize = i.Size;
        int y = (Height - imageSize.Height) / 2;
        e.Graphics.DrawImage(i, ImagePixelsXOffset, y + ImagePixelsYOffset, imageSize.Width, imageSize.Height);
      }
      Point pt = new Point(imageSize.Width + TitlePixelsXOffset, TitlePixelsYOffset);
      if (!String.IsNullOrEmpty(Title))
      {
        SolidBrush titleBrush = null;
        Color currentTitleColor = (tracking ? SystemColors.HotTrack : TitleColor);
        if (DrawShadow && Enabled)
        {
          SolidBrush titleShadowBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(TitleShadowOpacity * 255), TitleColor));
          e.Graphics.DrawString(Title, Font, titleShadowBrush, pt.X + TitleShadowPixelsXOffset, pt.Y + TitleShadowPixelsYOffset);
          titleShadowBrush.Dispose();
        }
        
        if(Enabled)
          titleBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(TitleColorOpacity * 255), currentTitleColor));
        else
          titleBrush = new SolidBrush(Color.FromArgb(80, 0, 0, 0));
        
        e.Graphics.DrawString(Title, Font, titleBrush, pt.X, pt.Y);
        titleBrush.Dispose();
        
        SizeF stringSize = e.Graphics.MeasureString(Title, Font);
        pt.Y += (int)(stringSize.Height + TitleDescriptionPixelsSpacing);
      }
      if (!String.IsNullOrEmpty(Description))
      {
        SolidBrush descriptionBrush = null;
        if (DrawShadow && Enabled)
        {
          SolidBrush descriptionShadowBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(DescriptionShadowOpacity * 255), Color.White));
          e.Graphics.DrawString(Description, DescriptionFont, descriptionShadowBrush, pt.X + DescriptionShadowPixelsXOffset, pt.Y + DescriptionShadowPixelsYOffset);
          descriptionShadowBrush.Dispose();

          descriptionBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(DescriptionColorOpacity * 255), DescriptionColor));
        }
        if (!Enabled)
        {
          descriptionBrush = new SolidBrush(Color.FromArgb(80, 0, 0, 0));     
        }

        e.Graphics.DrawString(Description, DescriptionFont, descriptionBrush, pt.X, pt.Y);
        descriptionBrush.Dispose();
      }     
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
      base.OnMouseDown(e);
      downButton = e.Button;
    }

    protected override void OnClick(EventArgs e)
    {
      if (downButton == null || downButton != MouseButtons.Left)
        return;
      base.OnClick(e);
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
