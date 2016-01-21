// Copyright (c) 2012, 2016, Oracle and/or its affiliates. All rights reserved.
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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace MySQL.ForExcel.Controls
{
  /// <summary>
  /// Provides a Windows picture box control for displaying an image controlling its opacity making it transparent.
  /// </summary>
  public class TransparentPictureBox : UserControl
  {
    #region Fields

    /// <summary>
    /// The image that is displayed by <see cref="TransparentPictureBox"/>.
    /// </summary>
    private Image _image;

    /// <summary>
    /// Flag indicating whether the scaled image size is calculated to maintain its original aspect ratio.
    /// </summary>
    private bool _maintainAspectRatio;

    /// <summary>
    /// The opacity factor of the picture ranging from 0 to 1.
    /// </summary>
    private float _opacity;

    /// <summary>
    /// The attributes used to manipulate the bitmap on rendering.
    /// </summary>
    private ImageAttributes _imageAttributes;

    /// <summary>
    /// A value indicating whether the <see cref="Image"/> is scaled to fit the size of this control.
    /// </summary>
    private bool _scaleImage;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="TransparentPictureBox"/> class.
    /// </summary>
    public TransparentPictureBox()
    {
      _image = null;
      _opacity = 0;
      _maintainAspectRatio = true;
      _scaleImage = false;
      SetStyle(ControlStyles.SupportsTransparentBackColor, true);
      BackColor = Color.Transparent;

      // Do not change, the Image is not drawn correctly if DoubleBuffered is true.
      DoubleBuffered = false;
    }

    #region Properties

    /// <summary>
    /// Gets or sets the background color of the control.
    /// </summary>
    [Category("MySQL Custom"), Description("The background color of the control.")]
    public new Color BackColor
    {
      get
      {
        return base.BackColor;
      }

      protected set
      {
        base.BackColor = value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this control should redraw its surface using a secondary buffer to reduce or prevent flicker.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    protected sealed override bool DoubleBuffered
    {
      get
      {
        return base.DoubleBuffered;
      }

      set
      {
        base.DoubleBuffered = value;
      }
    }

    /// <summary>
    /// Gets or sets the image that is displayed by <see cref="TransparentPictureBox"/>.
    /// </summary>
    [Category("MySQL Custom"), Description("The image that is displayed by this control.")]
    public Image Image
    {
      get
      {
        return _image;
      }

      set
      {
        _image = value;
        Invalidate();
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the image size is calculated to maintain its original aspect ratio.
    /// </summary>
    /// <remarks>This setting only applies when <see cref="ScaleImage"/> is <c>true</c>.</remarks>
    [Category("MySQL Custom"), Description("Indicates whether the image size is calculated to maintain its original aspect ratio when the image is scaled.")]
    public bool MaintainAspectRatio
    {
      get
      {
        return _maintainAspectRatio;
      }

      set
      {
        _maintainAspectRatio = value;
        Invalidate();
      }
    }

    /// <summary>
    /// Gets or sets the opacity factor of the picture ranging from 0 to 1.
    /// </summary>
    [Category("MySQL Custom"), Description("The opacity factor of the picture ranging from 0 to 1.")]
    public float Opacity
    {
      get
      {
        return _opacity;
      }

      set
      {
        if (value < 0 || value > 1)
        {
          throw new IndexOutOfRangeException("Value is out of range");
        }

        _opacity = value;
        ColorMatrix cm = new ColorMatrix();
        cm.Matrix00 = cm.Matrix11 = cm.Matrix22 = cm.Matrix44 = 1;
        cm.Matrix33 = _opacity;
        _imageAttributes = new ImageAttributes();
        _imageAttributes.SetColorMatrix(cm);
        Invalidate();
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Image"/> is scaled to fit the size of this control.
    /// </summary>
    [Category("MySQL Custom"), Description("Value indicating whether the Image is scaled to fit the size of this control.")]
    public bool ScaleImage
    {
      get
      {
        return _scaleImage;
      }

      set
      {
        _scaleImage = value;
        Invalidate();
        Update();
      }
    }

    /// <summary>
    /// Gets the required creation parameters when the control handle is created.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
      if (_image == null)
      {
        return;
      }

      Rectangle drawRectangle;
      Size sourceImageSize = Size.Empty;
      int clipWidth = e.ClipRectangle.Width;
      int clipHeight = e.ClipRectangle.Height;
      if (ScaleImage)
      {
        Size drawImageSize;
        if (_maintainAspectRatio)
        {
          int deltaHeight = clipHeight - _image.Height;
          int deltaWidth = clipWidth - _image.Width;
          drawImageSize = deltaHeight > deltaWidth
            ? new Size(clipWidth, _image.Height * clipWidth / _image.Width)
            : new Size(_image.Width * clipHeight / _image.Height, clipHeight);
        }
        else
        {
          drawImageSize = new Size(clipWidth, clipHeight);
        }

        drawRectangle = new Rectangle(0, 0, drawImageSize.Width, drawImageSize.Height);
        sourceImageSize.Width = _image.Width;
        sourceImageSize.Height = _image.Height;
      }
      else
      {
        drawRectangle = new Rectangle(0, 0, Math.Min(clipWidth, _image.Width), Math.Min(clipHeight, _image.Height));
        sourceImageSize.Width = Math.Min(clipWidth, _image.Width);
        sourceImageSize.Height = Math.Min(clipHeight, _image.Height);
      }

      e.Graphics.DrawImage(Image, drawRectangle, 0, 0, sourceImageSize.Width, sourceImageSize.Height, GraphicsUnit.Pixel, _imageAttributes);
    }
  }
}