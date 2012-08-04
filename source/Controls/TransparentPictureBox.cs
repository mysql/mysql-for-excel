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
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace MySQL.ForExcel
{
  public class TransparentPictureBox : UserControl
  {
    private float _opacity = 0;
    private ImageAttributes ia = null;

    public Image Image { set; get; }

    public float Opacity
    {
      get
      { return _opacity; }
      set
      {
        if (!(value <= 1 && value >= 0))
          throw new ArgumentOutOfRangeException("Value is out of range");
        _opacity = value;
        ColorMatrix cm = new ColorMatrix();
        cm.Matrix00 = cm.Matrix11 = cm.Matrix22 = cm.Matrix44 = 1;
        cm.Matrix33 = _opacity;
        ia = new ImageAttributes();
        ia.SetColorMatrix(cm);
      }    
    }

    public TransparentPictureBox()
    {
      SetStyle(ControlStyles.SupportsTransparentBackColor, true);
      BackColor = Color.Transparent;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (ia != null)
          ia.Dispose();
      }
      base.Dispose(disposing);
    }

    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams cp = base.CreateParams;
        cp.ExStyle |= 0x20;  // WS_EX_TRANSPARENT
        return cp;
      }
    }
  
    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      if (Image != null)
        e.Graphics.DrawImage(Image, new Rectangle(0, 0, Image.Width, Image.Height), 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, ia);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      //Don't paint background so we can keep transparency
    }
   
  }
}
