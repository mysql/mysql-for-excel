using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Data;

namespace MySQL.ExcelAddIn
{
  public static class Utilities
  {
    public static void SetDoubleBuffered(System.Windows.Forms.Control c)
    {
      if (System.Windows.Forms.SystemInformation.TerminalServerSession)
        return;

      System.Reflection.PropertyInfo aProp =
            typeof(System.Windows.Forms.Control).GetProperty(
                  "DoubleBuffered",
                  System.Reflection.BindingFlags.NonPublic |
                  System.Reflection.BindingFlags.Instance);

      aProp.SetValue(c, true, null);
    }

    public static Bitmap MakeGrayscale(Bitmap original)
    {
      //create a blank bitmap the same size as original
      Bitmap newBitmap = new Bitmap(original.Width, original.Height);

      //get a graphics object from the new image
      Graphics g = Graphics.FromImage(newBitmap);

      //create the grayscale ColorMatrix
      ColorMatrix colorMatrix = new ColorMatrix(
         new float[][] 
      {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}
      });

      //create some image attributes
      ImageAttributes attributes = new ImageAttributes();

      //set the color matrix attribute
      attributes.SetColorMatrix(colorMatrix);

      //draw the original image on the new image
      //using the grayscale color matrix
      g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
         0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

      //dispose the Graphics object
      g.Dispose();
      return newBitmap;
    }

    public static void ShowErrorBox(string errorMessage)
    {
      MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static void ShowExceptionBox(Exception ex)
    {
      ShowErrorBox(ex.Message);
    }

    public static List<string> DataRowsToList(DataRowCollection rowCollection, string colName)
    {
      List<string> retList = null;

      if (rowCollection != null && rowCollection.Count > 0)
      {
        foreach (DataRow dr in rowCollection)
        {
          retList.Add(dr[colName].ToString());
        }
      }

      return retList;
    }

    public static List<string> DataRowsToList(DataRow[] dataRows, string colName)
    {
      List<string> retList = null;

      if (dataRows != null && dataRows.Length > 0)
      {
        foreach (DataRow dr in dataRows)
        {
          retList.Add(dr[colName].ToString());
        }
      }

      return retList;
    }
  }
}
