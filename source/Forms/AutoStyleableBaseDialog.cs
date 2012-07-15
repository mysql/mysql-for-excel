using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace MySQL.ForExcel
{
  public partial class AutoStyleableBaseDialog : AutoStyleableBaseForm
  {
    private bool displayFootNoteArea = false;
    private bool displayCommandArea = true;
    private int panelsSeparatorWidth = 1;
    private Point renderingStartingPoint = Point.Empty;

    [Category("Appearance"), DefaultValue(true), Description("Indicates if dialog should draw visual styles depending on the Windows version.")]
    public bool AutoStyleDialog { get; set; }

    [Category("Appearance"), DefaultValue(true), Description("Draws a thin line to separate panels.")]
    public bool DrawPanelsSeparator { get; set; }

    [Category("Appearance"), Description("Color of the panels separator.")]
    public Color PanelsSeparatorColor { get; set; }

    [Category("Appearance"), DefaultValue(""), Description("Main instruction text for a Windows compliant dialog box.")]
    public string MainInstruction { get; set; }

    [Category("Appearance"), DefaultValue(null), Description("Main instruction optional icon.")]
    public Image MainInstructionImage { get; set; }

    [Category("Layout"), Description("Main instruction image or text initial location.")]
    public Point MainInstructionLocation { get; set; }

    [Category("Layout"), Description("Offset applied to MainInstructionLocation property.")]
    public Size MainInstructionLocationOffset { get; set; }

    [Category("Layout"), DefaultValue(false), Description("Displays or hides footnote area at the bottom of the dialog.")]
    public bool DisplayFootNoteArea
    {
      get { return displayFootNoteArea; }
      set
      {
        if (!value)
          FootNoteAreaHeight = 0;
        else if (!displayFootNoteArea)
          FootNoteAreaHeight = 80;
      }
    }

    [Category("Layout"), DefaultValue(0), Description("Sets the footnote area height; when 0 hides the footnote area.")]
    public int FootNoteAreaHeight
    {
      get { return footNoteAreaPanel.Height; }
      set
      {
        if (value < 0)
          throw new ArgumentOutOfRangeException("DisplayFootNoteArea", "Must be at least 0.");
        int delta = value - footNoteAreaPanel.Height;
        contentAreaPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        commandAreaPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        footNoteAreaPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        displayFootNoteArea = footNoteAreaPanel.Visible = (value > 0);
        footNoteAreaPanel.Height = value;
        Height += delta;
        contentAreaPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
        commandAreaPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        footNoteAreaPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      }
    }

    [Category("Layout"), DefaultValue(true), Description("Displays or hides command area (normally for command buttons) at the middle of the dialog.")]
    public bool DisplayCommandArea
    {
      get { return displayCommandArea; }
      set
      {
        if (!value)
          CommandAreaHeight = 0;
        else if (!displayCommandArea)
          CommandAreaHeight = 40;
      }
    }

    [Category("Layout"), DefaultValue(40), Description("Sets the footnote area height; when 0 hides the footnote area.")]
    public int CommandAreaHeight
    {
      get { return commandAreaPanel.Height; }
      set
      {
        if (value < 0)
          throw new ArgumentOutOfRangeException("CommandNoteAreaHeight", "Must be at least 0.");
        int delta = value - commandAreaPanel.Height;
        contentAreaPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        commandAreaPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        footNoteAreaPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        displayCommandArea = commandAreaPanel.Visible = (value > 0);
        commandAreaPanel.Height = value;
        footNoteAreaPanel.Location = new Point(footNoteAreaPanel.Location.X, footNoteAreaPanel.Location.Y + delta);
        Height += delta;
        contentAreaPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
        commandAreaPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        footNoteAreaPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      }
    }

    [Category("Appearance"), DefaultValue(1), Description("Width in pixels of the separator line.")]
    public int PanelsSeparatorWidth 
    {
      get { return panelsSeparatorWidth; }
      set
      {
        if (value < 0)
          throw new ArgumentOutOfRangeException("PanelsSeparatorWidth", "Must be at least 0.");
        int delta = value - panelsSeparatorWidth;
        panelsSeparatorWidth = value;
        CommandAreaHeight += delta;
        FootNoteAreaHeight += delta;
      }
    }

    public AutoStyleableBaseDialog()
    {
      InitializeComponent();

      AutoStyleDialog = false;
      DrawPanelsSeparator = true;
      PanelsSeparatorColor = SystemColors.ControlDark;
      MainInstruction = String.Empty;
      MainInstructionLocation = new Point(12, 9);
      MainInstructionLocationOffset = new Size(0, 0);
      MainInstructionImage = null;
    }

    private Point DrawImage(Graphics graphics, Image img, Point location)
    {
      Point newLocation = location;
      if (img != null)
      {
        newLocation = new Point(location.X + img.Width, location.Y);
        graphics.DrawImage(img, location);
      }
      return newLocation;
    }

    private void DrawThemeBackground(IDeviceContext deviceContext, VisualStyleElement element, Rectangle bounds, Rectangle clipRectangle)
    {
      if (StyleableHelper.AreVistaDialogsThemeSupported)
      {
        VisualStyleRenderer renderer = new VisualStyleRenderer(element);
        renderer.DrawBackground(deviceContext, bounds, clipRectangle);
      }
    }

    private void contentAreaPanel_Paint(object sender, PaintEventArgs e)
    {
      if (AutoStyleDialog)
      {
        DrawThemeBackground(e.Graphics, CustomVisualStyleElements.TaskDialog.PrimaryPanel, contentAreaPanel.ClientRectangle, e.ClipRectangle);
        renderingStartingPoint = DrawImage(e.Graphics, MainInstructionImage, MainInstructionLocation);
        renderingStartingPoint.Offset(MainInstructionLocation.X, 0);
        if (MainInstructionLocationOffset != null)
          renderingStartingPoint.Offset(MainInstructionLocationOffset.Width, MainInstructionLocationOffset.Height);
        StyleableHelper.DrawText(e.Graphics, MainInstruction, CustomVisualStyleElements.TextStyle.MainInstruction, new Font(Font, FontStyle.Bold), renderingStartingPoint, false, ClientSize.Width - renderingStartingPoint.X - MainInstructionLocation.X);
      }
    }

    private void commandAreaPanel_Paint(object sender, PaintEventArgs e)
    {
      if (!displayCommandArea)
        return;
      if (AutoStyleDialog)
        DrawThemeBackground(e.Graphics, CustomVisualStyleElements.TaskDialog.SecondaryPanel, commandAreaPanel.ClientRectangle, e.ClipRectangle);
      else if (DrawPanelsSeparator && panelsSeparatorWidth > 0)
      {
        Pen separatorPen = new Pen(PanelsSeparatorColor, panelsSeparatorWidth);
        e.Graphics.DrawLine(separatorPen, 0, 0, commandAreaPanel.Width, 0);
        separatorPen.Dispose();
      }
    }

    private void footNoteAreaPanel_Paint(object sender, PaintEventArgs e)
    {
      if (!displayFootNoteArea)
        return;
      if (AutoStyleDialog)
        DrawThemeBackground(e.Graphics, CustomVisualStyleElements.TaskDialog.SecondaryPanel, footNoteAreaPanel.ClientRectangle, e.ClipRectangle);
      else if (DrawPanelsSeparator && panelsSeparatorWidth > 0)
      {
        Pen separatorPen = new Pen(PanelsSeparatorColor, panelsSeparatorWidth);
        e.Graphics.DrawLine(separatorPen, 0, 0, footNoteAreaPanel.Width, 0);
        separatorPen.Dispose();
      }
    }
  }
}
