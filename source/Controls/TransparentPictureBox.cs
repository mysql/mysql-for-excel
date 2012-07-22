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
    
    private Image _image;
    private float _opacity = 0;

    public Image Image
    {
      get
      {
        return _image;
      }
      set
      {
        _image = value;
        RecreateHandle();
      }
    }

    public float Opacity
    {
      get
      {
        return _opacity;            
      }
      set
      {
        if (!(value <= 1 && value >= 0))
          throw new ArgumentOutOfRangeException("Value is out of range");
        else
          _opacity = value;      
      }    
    }


    public TransparentPictureBox()
    {
      SetStyle(ControlStyles.SupportsTransparentBackColor, true);
      BackColor = Color.Transparent;     
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
      if (_image != null)
      {
        ColorMatrix cm = new ColorMatrix();
        cm.Matrix00 = cm.Matrix11 = cm.Matrix22 = cm.Matrix44 = 1;
        cm.Matrix33 = _opacity;
        
        ImageAttributes ia = new ImageAttributes();
        ia.SetColorMatrix(cm);  
        e.Graphics.DrawImage(_image, new Rectangle(0, 0, _image.Width, _image.Height), 0, 0, _image.Width, _image.Height, GraphicsUnit.Pixel, ia);                
      }
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      //Don't paint background so we can keep transparency
    }

    protected override void OnMove(EventArgs e)
    {
      RecreateHandle();
    }

    public void Redraw()
    {
      RecreateHandle();
    }
   
  }
}
