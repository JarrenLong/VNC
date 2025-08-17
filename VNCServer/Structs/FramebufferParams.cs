using System;
using System.Runtime.InteropServices;

namespace VNC.Structs
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct FramebufferParams : IByteStruct
  {
    public ushort WidthPixels;
    public ushort HeightPixels;
    public PixelFormat FormatPixels;
    public uint NameLength;
    public string Name;

    public void FromBytes(byte[] data)
    {
      throw new System.NotImplementedException();
    }

    public byte[] ToBytes()
    {
      int size = 24;
      if (!string.IsNullOrEmpty(Name))
        size += Name.Length;
      byte[] ret = new byte[size];

      StructUtils.ToArray(ref ret, 0, WidthPixels);
      StructUtils.ToArray(ref ret, 2, HeightPixels);
      Array.Copy(FormatPixels.ToBytes(), 0, ret, 4, 16);
      StructUtils.ToArray(ref ret, 20, NameLength);
      if (!string.IsNullOrEmpty(Name))
      {
        for (int i = 0; i < Name.Length; i++)
          ret[21 + i] = (byte)Name[i];
      }
      return ret;
    }
  }
}
