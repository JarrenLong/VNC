using System;
using System.Runtime.InteropServices;
using VNC.Enums;

namespace VNC.Structs
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct RectangleHeader : IByteStruct
  {
    public Rectangle Header;
    public Encodings EncodingType;
    public byte[] PixelData;

    public void FromBytes(byte[] data)
    {
      Header.FromBytes(data);
      EncodingType = (Encodings)StructUtils.IntFromArray(ref data, 8);
      // TODO: encoding type should determine bytes/pixel;
      int sz = Header.Width * Header.Height * 3;
      PixelData = new byte[sz];
      Array.Copy(data, 12, PixelData, 0, sz);
    }

    public byte[] ToBytes()
    {
      int size = Marshal.SizeOf(typeof(Rectangle)) + sizeof(Encodings);
      if (PixelData != null)
        size += PixelData.Length;

      byte[] ret = new byte[size];

      byte[] buf = Header.ToBytes();
      int offset = buf.Length;

      Array.Copy(buf, 0, ret, 0, offset);
      buf = null;

      StructUtils.ToArray(ref ret, offset, (int)EncodingType);
      offset += 4;

      if (PixelData != null)
      {
        Array.Copy(PixelData, 0, ret, offset, PixelData.Length);
      }

      return ret;
    }
  }
}
