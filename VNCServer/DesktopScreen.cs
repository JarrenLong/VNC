using System;
using VNC.Structs;

namespace VNC
{
  public class DesktopScreen : IDisposable
  {
    public int ScreenId { get; private set; }
    private byte[] ScreenBuffer;
    private readonly int screenBufferSize;
    private readonly int screenWidth;
    private readonly int screenHeight;
    private readonly int bytesPerPixel;
    private readonly object lockObj;

    public DesktopScreen(int screenId, FramebufferParams specs)
    {
      lockObj = new object();

      ScreenId = screenId;

      screenWidth = specs.WidthPixels;
      screenHeight = specs.HeightPixels;
      bytesPerPixel = (specs.FormatPixels.BitsPerPixel / 8);
      screenBufferSize = screenWidth * screenHeight * bytesPerPixel;

      lock (lockObj)
      {
        // Build a buffer to hold the raw pixels for each screen for a 1024*768 screen @32 bits, this would be a 3Mb array
        // Pixels will be stored starting at top-left corner of image in R[G[B[A]]] bytes per pixel
        ScreenBuffer = new byte[screenBufferSize];
      }
    }

    public void UpdateScreen(ref byte[] data)
    {
      lock (lockObj)
      {
        if (data.Length == screenBufferSize)
          Array.Copy(data, ScreenBuffer, screenBufferSize);
      }
    }

    public static int Clamp(int val, int min, int max)
    {
      if (val < min)
        return min;
      if (val > max)
        return max;
      return val;
    }

    public byte[] GetScreen()
    {
      byte[] ret = new byte[screenBufferSize];
      lock (lockObj)
        Array.Copy(ScreenBuffer, ret, screenBufferSize);
      return ret;
    }

    public byte[] GetRect(Rectangle rect)
    {
      // Clamp our rectangle to the screen bounds if necessary
      int xMin = Clamp(rect.X, 0, screenWidth);
      int xMax = Clamp(rect.X + rect.Width, 0, screenWidth);
      int yMin = Clamp(rect.Y, 0, screenHeight);
      int yMax = Clamp(rect.Y + rect.Height, 0, screenHeight);

      int aw = xMax - xMin;
      int ah = yMax - yMax;

      // Allocate a buffer to hold the pixel data
      byte[] ret = new byte[aw * ah * bytesPerPixel];

      lock (lockObj)
      {
        for (int y = yMin; y < yMax; y++)
          Array.Copy(ScreenBuffer, (y * screenWidth) + xMin, ret, y * aw * bytesPerPixel, aw * bytesPerPixel);
      }

      return ret;
    }

    public void Dispose()
    {
      lock (lockObj)
      {
        if (ScreenBuffer != null)
        {
          Array.Clear(ScreenBuffer, 0, ScreenBuffer.Length);
          ScreenBuffer = null;
        }
      }
    }
  }
}
