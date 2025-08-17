using System;
using System.Linq;
using System.Threading.Tasks;
using VNC.Enums;
using VNC.Structs;

namespace VNC
{
  public class ClientCommand
  {
    public enum CommandType : byte
    {
      SetPixelFormat = 0,
      SetEncodings = 2,
      FramebufferUpdateRequest = 3,
      KeyEvent = 4,
      PointerEvent = 5,
      ClientCutText = 6
    }

    public static async Task<byte[]> Process(byte[] data)
    {
      byte[] ret = null;

      // Read data, process the message
      switch ((CommandType)data[0])
      {
        case CommandType.SetPixelFormat:
          //ToLog(" - Set Pixel Format");
          break;
        case CommandType.SetEncodings:
          //ToLog(" - Set Encodings");
          break;
        case CommandType.FramebufferUpdateRequest:
          //ToLog(" - Update requested");
          break;
        case CommandType.KeyEvent:
          //ToLog(" - Key event");
          break;
        case CommandType.PointerEvent:
          //ToLog(" - Pointer event");
          break;
        case CommandType.ClientCutText:
          //ToLog(" - Cut Text");
          break;
        default:
          //ToLog(string.Format("Unknown command {0}, ignoring", data[0]));
          break;
      }

      return ret;
    }

    public static byte[] SetPixelFormat(PixelFormat fmt)
    {
      byte[] ret = new byte[20];
      ret[0] = (byte)CommandType.SetPixelFormat;
      Array.Copy(fmt.ToBytes(), 0, ret, 4, 16);
      return ret;
    }

    public static byte[] SetEncodings(Encodings[] vals)
    {
      byte[] ret = new byte[4 + (vals.Length * 4)];
      ret[0] = (byte)CommandType.SetEncodings;
      StructUtils.ToArray(ref ret, 2, vals.Length);

      int t = 0;
      for (int i = 0; i < vals.Length; i++)
      {
        t = (int)vals[i];
        StructUtils.ToArray(ref ret, 4 * i, t);
      }

      return ret;
    }

    public static byte[] FrameBufferUpdateRequest(bool incremental, Rectangle rect)
    {
      byte[] ret = new byte[10];
      ret[0] = (byte)CommandType.FramebufferUpdateRequest;
      ret[1] = (byte)(incremental ? 1 : 0);
      Array.Copy(rect.ToBytes(), 0, ret, 2, 8);
      return ret;
    }

    public static byte[] KeyEvent(bool keyIsDown, Keys k)
    {
      byte[] ret = new byte[8];
      uint kk = (uint)k;
      ret[0] = (byte)CommandType.KeyEvent;
      ret[1] = (byte)(keyIsDown ? 1 : 0);
      StructUtils.ToArray(ref ret, 4, kk);
      return ret;
    }

    public static byte[] PointerEvent(MouseButton button, bool buttonDown, ushort x, ushort y)
    {
      byte[] ret = new byte[6];
      byte buttonMask = (byte)button;
      if (buttonDown)
        buttonMask |= 0x80;
      ret[0] = (byte)CommandType.PointerEvent;
      ret[1] = buttonMask;

      StructUtils.ToArray(ref ret, 2, x);
      StructUtils.ToArray(ref ret, 4, y);
      return ret;
    }

    /// <summary>
    /// Supports ISO-8869-1 (Latin-1) text only. Use \n only, no \r supported
    /// </summary>
    /// <param name="length">Length of string to cut</param>
    /// <param name="text">The string cut</param>
    /// <returns></returns>
    public static byte[] ClientCutText(uint length, string text)
    {
      string buf = text;
      if (string.IsNullOrEmpty(buf))
        buf = "";
      if (buf.Length != length)
        throw new ArgumentException("text length doesn't match");

      byte[] ret = new byte[8 + buf.Length];
      ret[0] = (byte)CommandType.ClientCutText;
      StructUtils.ToArray(ref ret, 4, length);

      if (length > 0)
        Array.Copy(buf.ToArray(), 0, ret, 8, length);

      return ret;
    }
  }
}
