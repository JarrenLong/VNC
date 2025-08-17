using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VNC;
using VNC.Enums;
using VNC.Structs;

namespace test
{
  public partial class Form1 : Form
  {
    private VNCServer server;
    private VNCClient client;
    private VNCSession ses;

    public Form1()
    {
      InitializeComponent();
      server = new VNCServer(LogServer);
      client = new VNCClient(LogClient);
      ses = new VNCSession();
    }

    public void LogServer(string msg)
    {
      if (InvokeRequired)
      {
        BeginInvoke(() => { LogServer(msg); });
        return;
      }

      textBoxServerLog.Text += msg + Environment.NewLine;
    }

    public void LogClient(string msg)
    {
      if (InvokeRequired)
      {
        BeginInvoke(() => { LogClient(msg); });
        return;
      }

      textBoxClientLog.Text += msg + Environment.NewLine;
    }

    private void buttonStartServer_Click(object sender, EventArgs e)
    {
      if (Screen.PrimaryScreen == null)
        return;

      LogServer("Starting VNC server ...");

      FramebufferParams s = new FramebufferParams();
      s.WidthPixels = (ushort)Screen.PrimaryScreen.Bounds.Width;
      s.HeightPixels = (ushort)Screen.PrimaryScreen.Bounds.Height;
      s.FormatPixels.BitsPerPixel = 24;
      s.FormatPixels.Depth = 24;
      s.FormatPixels.BigEndianFlag = 1;
      s.FormatPixels.TrueColorFlag = 0;

      int numScreens = Screen.AllScreens.Length;
      ses = new VNCSession(0, numScreens, s);
      server.StartServer(5900, LogServer);

      // Grab screenshots of all three screens
      for (int i = 0; i < numScreens; i++)
      {
        Screen scr = Screen.AllScreens[i];

        System.Drawing.Rectangle bounds = scr.Bounds;
        using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
        {
          using (Graphics graphics = Graphics.FromImage(bitmap))
          {
            graphics.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
          }

          BitmapData bmpData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
            ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

          // Get the address of the first line
          IntPtr ptr = bmpData.Scan0;

          // Declare an array to hold the bytes of the bitmap
          int bytes = Math.Abs(bmpData.Stride) * bitmap.Height;
          byte[] rgbValues = new byte[bytes];

          // Copy the RGB values into the array
          Marshal.Copy(ptr, rgbValues, 0, bytes);
          ses.Screens[i].UpdateScreen(ref rgbValues);

          // Unlock the bits
          bitmap.UnlockBits(bmpData);

          Array.Clear(rgbValues);
        }
      }


      LogServer("Server started");
    }

    private void buttonStartClient_Click(object sender, EventArgs e)
    {
      client.StartClient(5900);
    }

    private void buttonStopServer_Click(object sender, EventArgs e)
    {
      server.StopServer();
    }

    private void buttonStopClient_Click(object sender, EventArgs e)
    {
      client.DisconnectClient();
    }

    private void pictureBoxClientView_MouseMove(object sender, MouseEventArgs e)
    {
      client.MouseEvent(MouseButton.None, false, e.X, e.Y).ConfigureAwait(false);
    }

    private void pictureBoxClientView_MouseUp(object sender, MouseEventArgs e)
    {
      MouseButton b;

      switch (e.Button)
      {
        case MouseButtons.Left: b = MouseButton.Left; break;
        case MouseButtons.Right: b = MouseButton.Right; break;
        case MouseButtons.Middle: b = MouseButton.Middle; break;
        default: b = MouseButton.None; break;
      }
      client.MouseEvent(b, false, e.X, e.Y).ConfigureAwait(false);
    }

    private void pictureBoxClientView_MouseDown(object sender, MouseEventArgs e)
    {
      MouseButton b;

      switch (e.Button)
      {
        case MouseButtons.Left: b = MouseButton.Left; break;
        case MouseButtons.Right: b = MouseButton.Right; break;
        case MouseButtons.Middle: b = MouseButton.Middle; break;
        default: b = MouseButton.None; break;
      }
      client.MouseEvent(b, true, e.X, e.Y).ConfigureAwait(false);
    }

    private void pictureBoxClientView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
    {
      client.KeyboardEvent((VNC.Enums.Keys)e.KeyCode, true).ConfigureAwait(false);
      client.KeyboardEvent((VNC.Enums.Keys)e.KeyCode, false).ConfigureAwait(false);
    }

    private void pictureBoxClientView_MouseEnter(object sender, EventArgs e)
    {
      pictureBoxClientView.Focus();
    }

    private void pictureBoxClientView_MouseLeave(object sender, EventArgs e)
    {
      textBoxClientLog.Focus();
    }
  }
}
