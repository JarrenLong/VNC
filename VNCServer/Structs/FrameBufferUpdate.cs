using System;
using System.Runtime.InteropServices;

namespace VNC.Structs
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct FrameBufferUpdate : IByteStruct
  {
    public byte MessageType;
    public byte Padding;
    public ushort NumRects;
    public RectangleHeader[] Rectangles;

    public void FromBytes(byte[] data)
    {
      if (data == null || data.Length < 4)
        throw new ArgumentException("Invalid byte array");

      MessageType = data[0];
      Padding = data[1];
      NumRects = (ushort)(data[2] << 8 | data[3]);
      for (int i = 0; i < NumRects; i++)
      {

      }
    }

    public byte[] ToBytes()
    {
      byte[] ret = new byte[4];

      ret[0] = MessageType;
      ret[1] = 0;
      ret[2] = (byte)((NumRects >> 8) & 0xFF);
      ret[3] = (byte)(NumRects & 0xFF);

      if (Rectangles != null)
      {
        foreach (RectangleHeader h in Rectangles)
        {
          byte[] buf = h.ToBytes();
          int offset = ret.Length;
          Array.Resize(ref ret, ret.Length + buf.Length);
          Array.Copy(buf, 0, ret, offset, buf.Length);
        }
      }

      return ret;
    }
  }
}
