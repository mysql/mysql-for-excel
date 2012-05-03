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
  public partial class InfolLabel : UserControl
  {
    #region Properties

    private PictureSize imageSize = PictureSize.W32H32;
    private bool pictureAsButton = false;
    private Image colorImage;
    private Image disabledImage;
    private bool eventsEnabled = false;
    
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
      get { return picImage.Image; }
      set 
      { 
        picImage.Image = colorImage = value;
        disabledImage = Utilities.MakeGrayscale(new Bitmap(colorImage)) as Image;
      }
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
    public bool PictureAsButton
    {
      get { return pictureAsButton; }
      set
      {
        pictureAsButton = value;
        setInternalEvents(pictureAsButton);
      }
    }

    public bool PictureEnabled
    {
      get { return picImage.Enabled; }
      set 
      { 
        picImage.Enabled = value;
        if (pictureAsButton)
          setInternalEvents(value);
        picImage.Image = (value ? colorImage : disabledImage);
      }
    }

    public event EventHandler PictureClick;

    #endregion Properties

    public InfolLabel()
    {      
      InitializeComponent();
    }

    private void setInternalEvents(bool enabled)
    {
      if (enabled && !eventsEnabled)
      {
        picImage.Click += picImage_Click;
        //picImage.MouseHover += picImage_MouseHover;
        picImage.MouseEnter += picImage_MouseEnter;
        picImage.MouseLeave += picImage_MouseLeave;
        picImage.MouseDown += picImage_MouseDown;
        picImage.MouseUp += picImage_MouseUp;
        eventsEnabled = true;
      }
      if (!enabled && eventsEnabled)
      {
        picImage.Click -= picImage_Click;
        //picImage.MouseHover -= picImage_MouseHover;
        picImage.MouseEnter -= picImage_MouseEnter;
        picImage.MouseLeave -= picImage_MouseLeave;
        picImage.MouseDown -= picImage_MouseDown;
        picImage.MouseUp -= picImage_MouseUp;
        eventsEnabled = false;
      }
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
          picImage.Size = new Size(32, 32);
          lblText.Location = new Point(41, 3);
          lblInfo1.Location = new Point(41, 21);
          lblInfo2.Visible = false;
          currentControlSize = this.Size;
          this.Size = new Size(currentControlSize.Width, 38);
          break;
        case PictureSize.W48H48:
          picImage.Size = new Size(48, 48);
          lblText.Location = new Point(57, 3);
          lblInfo1.Location = new Point(57, 24);
          lblInfo2.Visible = true;
          currentControlSize = this.Size;
          this.Size = new Size(currentControlSize.Width, 55);
          break;
      }
    }

    private void InfolLabel_Load(object sender, EventArgs e)
    {
      alignControlsBasedOnPicSize(imageSize);
    }

    protected virtual void OnPictureClick(EventArgs e)
    {
      if (PictureClick != null)
        PictureClick(this, EventArgs.Empty);
    }

    private void picImage_Click(object sender, EventArgs e)
    {
      OnPictureClick(e);
      picImage_MouseLeave(this, EventArgs.Empty);
    }

    private void picImage_MouseEnter(object sender, EventArgs e)
    {
      Cursor = Cursors.Hand;
      picImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      picImage.BackColor = SystemColors.ControlLightLight;
    }

    private void picImage_MouseHover(object sender, EventArgs e)
    {
      Cursor = Cursors.Hand;
      picImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      picImage.BackColor = SystemColors.ControlLightLight;
    }

    private void picImage_MouseLeave(object sender, EventArgs e)
    {
      Cursor = Cursors.Default;
      picImage.BorderStyle = System.Windows.Forms.BorderStyle.None;
      picImage.BackColor = SystemColors.Control;
    }

    private void picImage_MouseUp(object sender, MouseEventArgs e)
    {
      picImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      picImage.BackColor = SystemColors.ControlLight;
    }

    private void picImage_MouseDown(object sender, MouseEventArgs e)
    {
      picImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      picImage.BackColor = SystemColors.ControlLightLight;
    }
  }
}
