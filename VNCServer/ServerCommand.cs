using System;
using System.Linq;
using System.Threading.Tasks;
using VNC.Structs;

namespace VNC
{
  public class ServerCommand
  {
    public enum CommandType : byte
    {
      FrameBufferUpdate = 0,
      SetColorMapEntries = 1,
      Bell = 2,
      ServerCutText = 3
    }

    public static async Task<byte[]> Process(byte[] data)
    {
      byte[] ret = null;

      // Read data, process the message
      switch ((CommandType)data[0])
      {
        case CommandType.FrameBufferUpdate:
          //ToLog(" - Update requested");
          break;
        case CommandType.SetColorMapEntries:
          //ToLog(" - Setting color map");
          break;
        case CommandType.Bell:
          //ToLog(" - Ding");
          break;
        case CommandType.ServerCutText:
          //ToLog(" - Cut Text");
          break;
        default:
          //ToLog(string.Format("Unknown command {0}, ignoring", data[0]));
          break;
      }

      return ret;
    }

    public static byte[] FrameBufferUpdate()
    {
      FrameBufferUpdate u = new FrameBufferUpdate();
      u.MessageType = (byte)CommandType.FrameBufferUpdate;
      u.NumRects = 2;
      u.Rectangles = new RectangleHeader[2];
      u.Rectangles[0].Header = new Rectangle();
      u.Rectangles[0].Header.Width = 4;
      u.Rectangles[0].Header.Height = 4;
      u.Rectangles[0].EncodingType = Enums.Encodings.Raw;
      u.Rectangles[0].PixelData = new byte[48];

      u.Rectangles[1].Header = new Rectangle();
      u.Rectangles[1].Header.Width = 4;
      u.Rectangles[1].Header.Height = 4;
      u.Rectangles[1].EncodingType = Enums.Encodings.Raw;
      u.Rectangles[1].PixelData = new byte[48];

      return u.ToBytes();
    }

    public static byte[] SetColorMapEntries()
    {
      byte[] ret = new byte[0];
      return ret;
    }
    public static byte[] Bell()
    {
      return new byte[1] { (byte)CommandType.Bell };
    }

    /// <summary>
    /// Supports ISO-8869-1 (Latin-1) text only. Use \n only, no \r supported
    /// </summary>
    /// <param name="length">Length of string to cut</param>
    /// <param name="text">The string cut</param>
    /// <returns></returns>
    public static byte[] ServerCutText(uint length, string text)
    {
      string buf = text;
      if (string.IsNullOrEmpty(buf))
        buf = "";
      if (buf.Length != length)
        throw new ArgumentException("text length doesn't match");

      byte[] ret = new byte[8 + buf.Length];
      ret[0] = (byte)CommandType.ServerCutText;
      ret[4] = (byte)((length >> 24) & 0xFF);
      ret[5] = (byte)((length >> 16) & 0xFF);
      ret[6] = (byte)((length >> 8) & 0xFF);
      ret[7] = (byte)(length & 0xFF);

      if (length > 0)
      {
        Array.Copy(buf.ToArray(), 0, ret, 8, length);
      }
      return ret;
    }
  }
}
