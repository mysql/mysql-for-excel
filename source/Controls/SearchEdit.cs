// 
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
//

using System;
using System.Drawing;
using System.Windows.Forms;
using MySQL.ForExcel.Properties;

namespace MySQL.ForExcel
{
  /// <summary>
  /// 
  /// </summary>
  public partial class SearchEdit : UserControl
  {
    /// <summary>
    /// Flag indicating whether it's the first time the control will be painted.
    /// </summary>
    private bool _initialPaint;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchEdit"/> class.
    /// </summary>
    /// <param name="searchImage">The image displayed inside the search edit box.</param>
    public SearchEdit()
    {
      InitializeComponent();
      DoubleBuffered = true;
      InnerTextBox.Text = string.Empty;
      IsEmpty = true;
      _initialPaint = true;
    }

    #region Properties

    /// <summary>
    /// Flag indicating whether the search edit box is empty.
    /// </summary>
    public bool IsEmpty { get; private set; }

    /// <summary>
    /// Gets or sets the label displayed within the search control when no text has been input by users.
    /// </summary>
    public string NoTextLabel { get; set; }

    public Image SearchImage { get; set; }

    /// <summary>
    /// Gets or sets the search text.
    /// </summary>
    public override string Text
    {
      get
      {
        return InnerTextBox.Text.Trim();
      }

      set
      {
        InnerTextBox.Text = value;
      }
    }

    #endregion Properties

    /// <summary>
    /// Raises the <see cref="Paint"/> event.
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
      if (SearchImage != null)
      {
        int space = SearchImage.Width * 3 / 2;
        e.Graphics.DrawImage(SearchImage, (space - SearchImage.Width) / 2, (Height - SearchImage.Height) / 2);
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="InnerTextBox"/> becomes the active control.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void InnerTextBox_Enter(object sender, EventArgs e)
    {
      if (IsEmpty)
      {
        InnerTextBox.Text = string.Empty;
        IsEmpty = false;
        InnerTextBox.ForeColor = SystemColors.WindowText;
      }
    }

    /// <summary>
    /// Event delegate method fired when a key is pressed inside the <see cref="InnerTextBox"/> control.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void InnerTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      OnKeyDown(e);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="InnerTextBox"/> is no longer the active control.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void InnerTextBox_Leave(object sender, EventArgs e)
    {
      if (InnerTextBox.Text.Trim().Length == 0)
      {
        InnerTextBox.Text = NoTextLabel;
        InnerTextBox.ForeColor = Color.Silver;
        IsEmpty = true;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SearchEdit"/> control is resized.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SearchEdit_Resize(object sender, EventArgs e)
    {
      int imageWidth = SearchImage != null ? SearchImage.Width : 0;
      InnerTextBox.SetBounds(imageWidth * 3 / 2, (Height - InnerTextBox.Height) / 2, Size.Width - imageWidth, InnerTextBox.Height);
    }
  }
}