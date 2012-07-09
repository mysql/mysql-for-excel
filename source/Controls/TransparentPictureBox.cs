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
