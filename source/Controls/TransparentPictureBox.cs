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

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace MySQL.ForExcel.Controls
{
  /// <summary>
  /// Provides a Windows picture box control for displaying an image controlling its opacity making it transparent.
  /// </summary>
  public sealed class TransparentPictureBox : UserControl
  {
    #region Fields

    /// <summary>
    /// The opacity factor of the picture ranging from 0 to 1.
    /// </summary>
    private float _opacity;

    /// <summary>
    /// The attributes used to manipulate the bitmap on rendering. 
    /// </summary>
    private ImageAttributes _imageAttributes;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="TransparentPictureBox"/> class.
    /// </summary>
    public TransparentPictureBox()
    {
      _opacity = 0;
      SetStyle(ControlStyles.SupportsTransparentBackColor, true);
      BackColor = Color.Transparent;
    }

    #region Properties

    /// <summary>
    /// Gets or sets the image that is displayed by <see cref="TransparentPictureBox"/>.
    /// </summary>
    public Image Image { set; get; }

    /// <summary>
    /// Gets or sets the opacity factor of the picture ranging from 0 to 1.
    /// </summary>
    public float Opacity
    {
      get
      {
        return _opacity;
      }

      set
      {
        if (!(value <= 1 && value >= 0))
        {
          throw new IndexOutOfRangeException("Value is out of range");
        }

        _opacity = value;
        ColorMatrix cm = new ColorMatrix();
        cm.Matrix00 = cm.Matrix11 = cm.Matrix22 = cm.Matrix44 = 1;
        cm.Matrix33 = _opacity;
        _imageAttributes = new ImageAttributes();
        _imageAttributes.SetColorMatrix(cm);
      }
    }

    /// <summary>
    /// Gets the required creation parameters when the control handle is created.
    /// </summary>
    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams cp = base.CreateParams;
        cp.ExStyle |= 0x20;  // WS_EX_TRANSPARENT
        return cp;
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
        if (_imageAttributes != null)
        {
          _imageAttributes.Dispose();
        }
      }

      base.Dispose(disposing);
    }

    /// <summary>
    /// Raises the <see cref="Control.Paint"/> event.
    /// </summary>
    /// <param name="e">A <see cref="PaintEventArgs"/> that contains the event data.</param>
    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      if (Image != null)
      {
        e.Graphics.DrawImage(Image, new Rectangle(0, 0, Image.Width, Image.Height), 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, _imageAttributes);
      }
    }

    /// <summary>
    /// Paints the background of the control.
    /// </summary>
    /// <param name="e">A <see cref="PaintEventArgs"/> that contains the event data.</param>
    protected override void OnPaintBackground(PaintEventArgs e)
    {
      //// Don't paint background so we can keep transparency
    }
  }
}