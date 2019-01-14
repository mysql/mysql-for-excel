// Copyright (c) 2012, 2019, Oracle and/or its affiliates. All rights reserved.
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
using System.Windows.Forms;

namespace MySQL.ForExcel.Controls
{
  /// <summary>
  /// Represents a search box that can show a search image at its left side and a text when no search text has been input.
  /// </summary>
  public sealed partial class SearchEdit : UserControl
  {
    #region Fields

    /// <summary>
    /// Flag indicating whether it's the first time the control will be painted.
    /// </summary>
    private bool _initialPaint;

    /// <summary>
    /// Flag indicating whether the search edit box is empty.
    /// </summary>
    private bool _isEmpty;

    /// <summary>
    /// The label displayed within the search control when no text has been input by users.
    /// </summary>
    private string _noTextLabel;

    /// <summary>
    /// The spacing, in pixels, used only when <see cref="ScaleImage"/> is <c>true</c>, to the left and above the image relative to the search box bounds rectangle.
    /// </summary>
    private int _imageXOffset;

    /// <summary>
    /// A value indicating whether the <see cref="SearchImage"/> is scaled to fit the height of the search box.
    /// </summary>
    private bool _scaleImage;

    /// <summary>
    /// The image displayed at the left side of the control.
    /// </summary>
    private Image _searchImage;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchEdit"/> class.
    /// </summary>
    public SearchEdit()
    {
      _isEmpty = true;
      _scaleImage = false;
      _imageXOffset = 3;
      _searchImage = null;
      InitializeComponent();
      DoubleBuffered = true;
      NoTextLabelColor = Color.Silver;
      SearchFiredOnLeave = false;
      Text = string.Empty;
      TextColor = SystemColors.WindowText;
      _initialPaint = true;
    }

    /// <summary>
    /// Event occurring when the ENTER key is pressed by the user.
    /// </summary>
    [Category("MySQL Custom"), Description("Event ocurring when the key code, specified in the SearchFiredTrigger property, is pressed by the user.")]
    public event EventHandler SearchFired;

    #region Properties

    /// <summary>
    /// Gets or sets the horizontal offset, in pixels, to draw the <see cref="SearchImage"/>.
    /// </summary>
    [Category("MySQL Custom"), Description("The horizontal offset, in pixels, to draw the SearchImage.")]
    public int ImageXOffset
    {
      get => _imageXOffset;

      set
      {
        _imageXOffset = value;
        SearchEdit_Resize(this, EventArgs.Empty);
        InnerTextBox.Invalidate();
      }
    }

    /// <summary>
    /// Gets or sets the label displayed within the search control when no text has been input by users.
    /// </summary>
    [Category("MySQL Custom"),
     Description("The label displayed within the search control when no text has been input by users.")]
    public string NoTextLabel
    {
      get => _noTextLabel;

      set
      {
        _noTextLabel = value;
        if (_isEmpty && !InnerTextBox.Focused)
        {
          InnerTextBox.Text = _noTextLabel;
        }
      }
    }

    /// <summary>
    /// Gets or sets the color used to draw the <see cref="NoTextLabel"/> text.
    /// </summary>
    [Category("MySQL Custom"), Description("the color used to draw the NoTextLabel text.")]
    public Color NoTextLabelColor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="SearchImage"/> is scaled to fit the height of the search box.
    /// </summary>
    [Category("MySQL Custom"), Description("Value indicating whether the SearchImage is scaled to fit the height of the search box.")]
    public bool ScaleImage
    {
      get => _scaleImage;

      set
      {
        _scaleImage = value;
        SearchEdit_Resize(this, EventArgs.Empty);
        InnerTextBox.Invalidate();
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="SearchFired"/> event is fired when focus leaves the search box as well as on hitting ENTER.
    /// </summary>
    [Category("MySQL Custom"), Description("Value indicating whether the SearchFired event is fired when focus leaves the search box as well as on hitting ENTER.")]
    public bool SearchFiredOnLeave { get; set; }

    /// <summary>
    /// Gets or sets the image displayed at the left side of the control.
    /// </summary>
    [Category("MySQL Custom"), Description("Tthe image displayed at the left side of the control.")]
    public Image SearchImage
    {
      get => _searchImage;

      set
      {
        _searchImage = value;
        SearchEdit_Resize(this, EventArgs.Empty);
        InnerTextBox.Invalidate();
      }
    }

    /// <summary>
    /// Gets or sets the search text.
    /// </summary>
    [Category("MySQL Custom"), Description("The search text.")]
    public override string Text
    {
      get => _isEmpty
        ? string.Empty
        : InnerTextBox.Text.Trim();

      set
      {
        var trimmedValue = value.Trim();
        _isEmpty = trimmedValue.Length == 0;
        InnerTextBox.Text = _isEmpty
          ? NoTextLabel
          : trimmedValue;
      }
    }

    /// <summary>
    /// Gets or sets the color used to draw the <see cref="Text"/> value.
    /// </summary>
    [Category("MySQL Custom"), Description("The color used to draw the Text value.")]
    public Color TextColor { get; set; }

    /// <summary>
    /// Gets the scaled height, in pixels, of the <see cref="SearchImage"/>.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    private int ScaledImageHeight => _searchImage == null
      ? 0
      : _scaleImage
        ? Height - ((Height - InnerTextBox.Height) / 2)
        : _searchImage.Height;

    /// <summary>
    /// Gets the scaled width, in pixels, of the <see cref="SearchImage"/>.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    private int ScaledImageWidth => _searchImage == null
      ? 0
      : _scaleImage
        ? _searchImage.Width * ScaledImageHeight / _searchImage.Height
        : _searchImage.Width;

    #endregion Properties

    /// <summary>
    /// Raises the <see cref="Control.Paint"/> event.
    /// </summary>
    /// <param name="e">A <see cref="PaintEventArgs"/> that contains the event data.</param>
    protected override void OnPaint(PaintEventArgs e)
    {
      if (_initialPaint)
      {
        InnerTextBox_Leave(InnerTextBox, EventArgs.Empty);
        _initialPaint = false;
      }

      base.OnPaint(e);
      if (_searchImage == null)
      {
        return;
      }

      if (ScaleImage)
      {
        e.Graphics.DrawImage(_searchImage, _imageXOffset, _imageXOffset, ScaledImageWidth, ScaledImageHeight);
      }
      else
      {
        var yOffset = (Height - _searchImage.Height) / 2;
        e.Graphics.DrawImageUnscaled(_searchImage, _imageXOffset, yOffset);
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="InnerTextBox"/> becomes the active control.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void InnerTextBox_Enter(object sender, EventArgs e)
    {
      if (!_isEmpty)
      {
        return;
      }

      if (_initialPaint)
      {
        InnerTextBox_Leave(InnerTextBox, EventArgs.Empty);
        _initialPaint = false;
      }

      InnerTextBox.Text = string.Empty;
      _isEmpty = false;
      InnerTextBox.ForeColor = TextColor;
    }

    /// <summary>
    /// Event delegate method fired when a key is pressed inside the <see cref="InnerTextBox"/> control.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void InnerTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      OnKeyDown(e);
      if (SearchFired == null || e.KeyCode != Keys.Enter)
      {
        return;
      }

      SearchFired(this, EventArgs.Empty);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="InnerTextBox"/> is no longer the active control.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void InnerTextBox_Leave(object sender, EventArgs e)
    {
      if (!_initialPaint && SearchFiredOnLeave && SearchFired != null)
      {
        SearchFired(this, EventArgs.Empty);
      }

      if (Text.Length > 0)
      {
        return;
      }

      _isEmpty = true;
      InnerTextBox.Text = NoTextLabel;
      InnerTextBox.ForeColor = NoTextLabelColor;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SearchEdit"/> control is resized.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SearchEdit_Resize(object sender, EventArgs e)
    {
      var imageWidth = ScaledImageWidth;
      var xOffset = imageWidth + (_imageXOffset * 2);
      InnerTextBox.SetBounds(xOffset, (Height - InnerTextBox.Height) / 2, Size.Width - xOffset, InnerTextBox.Height);
    }
  }
}