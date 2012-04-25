using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySQL.ExcelAddIn.Controls
{
  public partial class ClickableInfolLabel : UserControl
  {
    #region Properties

    private PictureSize imageSize = PictureSize.W32H32;
    private bool clickableImage = false;
    
    public enum PictureSize { W32H32, W48H48 };
    public string MainText
    {
      get { return lblText.Text; }
      set { lblText.Text = value; }
    }
    public string InfoText1
    {
      get { return lblInfo1.Text; }
      set { lblInfo1.Text = value; }
    }
    public string InfoText2
    {
      get { return lblInfo2.Text; }
      set { lblInfo2.Text = value; }
    }
    public Image Image
    {
      get { return btnImage.Image; }
      set { btnImage.Image = value; }
    }
    public PictureSize ImageSize
    {
      get { return imageSize; }
      set
      {
        if (imageSize != value)
          alignControlsBasedOnPicSize(value);
        imageSize = value;
      }
    }
    public bool ClickableImage
    {
      get { return clickableImage; }
      set
      {
        clickableImage = value;
        if (clickableImage)
        {
          btnImage.Enabled = true;
          btnImage.Click += btnImage_Click;
        }
        else
        {
          btnImage.Enabled = false;
          btnImage.Click -= btnImage_Click;
        }
      }
    }
    public event EventHandler PictureClick;

    #endregion Properties

    public ClickableInfolLabel()
    {      
      InitializeComponent();
    }

    private int widestLabelWidth()
    {
      int retWidth = 0;
      int lblWidth = 0;

      foreach (var cont in this.Controls)
      {
        if (cont is Label)
        {
          lblWidth = (cont as Label).Size.Width;
          if (lblWidth > retWidth)
            retWidth = lblWidth;
        }
      }

      return retWidth;
    }

    private void alignControlsBasedOnPicSize(PictureSize newImageSize)
    {
      Size currentControlSize;

      switch(newImageSize)
      {
        case PictureSize.W32H32:
          btnImage.Size = new Size(32, 32);
          lblText.Location = new Point(41, 3);
          lblInfo1.Location = new Point(41, 21);
          lblInfo2.Visible = false;
          currentControlSize = this.Size;
          this.Size = new Size(currentControlSize.Width, 38);
          break;
        case PictureSize.W48H48:
          btnImage.Size = new Size(48, 48);
          lblText.Location = new Point(57, 3);
          lblInfo1.Location = new Point(57, 24);
          lblInfo2.Visible = true;
          currentControlSize = this.Size;
          this.Size = new Size(currentControlSize.Width, 55);
          break;
      }
    }

    private void ClickableInfolLabel_Load(object sender, EventArgs e)
    {
      alignControlsBasedOnPicSize(imageSize);
    }

    protected virtual void OnPictureClick(EventArgs e)
    {
      if (PictureClick != null)
        PictureClick(this, EventArgs.Empty);
    }

    private void btnImage_Click(object sender, EventArgs e)
    {
      OnPictureClick(e);
    }
  }
}
