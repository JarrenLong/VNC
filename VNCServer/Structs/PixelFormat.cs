using System;
using System.Runtime.InteropServices;

namespace VNC.Structs
{
  [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 16)]
  public struct PixelFormat : IByteStruct
  {
    /// <summary>
    /// 8, 16, or 32; must be >= Depth
    /// </summary>
    public byte BitsPerPixel;
    /// <summary>
    /// 8, 16, or 32
    /// </summary>
    public byte Depth;
    /// <summary>
    /// Non-zero if multi-byte pixels are Big-Endian (MSB)
    /// </summary>
    public byte BigEndianFlag;
    /// <summary>
    /// If true, use RGB fields below
    /// </summary>
    public byte TrueColorFlag;
    /// <summary>
    /// Max red value, must be 2^N - 1 where N is # bits used for red.
    /// </summary>
    public ushort RedMax;
    /// <summary>
    /// Max green value, must be 2^N - 1 where N is # bits used for green.
    /// </summary>
    public ushort GreenMax;
    /// <summary>
    /// Max blue value, must be 2^N - 1 where N is # bits used for blue.
    /// </summary>
    public ushort BlueMax;
    /// <summary>
    /// # shifts needed to get the red value in a pixel to the least significant bit
    /// </summary>
    public byte RedShift;
    /// <summary>
    /// # shifts needed to get the green value in a pixel to the least significant bit
    /// </summary>
    public byte GreenShift;
    /// <summary>
    /// # shifts needed to get the blue value in a pixel to the least significant bit
    /// </summary>
    public byte BlueShift;
    // 3 bytes of padding
    public byte PaddingA;
    public byte PaddingB;
    public byte PaddingC;

    public void FromBytes(byte[] ret)
    {
      if (ret == null || ret.Length < 4)
        throw new ArgumentException("array size");

      BitsPerPixel = ret[0];
      Depth = ret[1];
      BigEndianFlag = ret[2];
      TrueColorFlag = ret[3];

      if (ret.Length >= 12)
      {
        RedMax = StructUtils.UshortFromArray(ref ret, 4);
        GreenMax = StructUtils.UshortFromArray(ref ret, 6);
        BlueMax = StructUtils.UshortFromArray(ref ret, 7);

        RedShift = ret[10];
        GreenShift = ret[11];
        BlueShift = ret[12];
      }

      PaddingA = PaddingB = PaddingC = 0;
    }

    public byte[] ToBytes()
    {
      byte[] ret = new byte[16];
      ret[0] = BitsPerPixel;
      ret[1] = Depth;
      ret[2] = BigEndianFlag;
      ret[3] = TrueColorFlag;
      StructUtils.ToArray(ref ret, 4, RedMax);
      StructUtils.ToArray(ref ret, 6, GreenMax);
      StructUtils.ToArray(ref ret, 8, BlueMax);
      ret[10] = RedShift;
      ret[11] = GreenShift;
      ret[12] = BlueShift;
      ret[13] = 0;
      ret[14] = 0;
      ret[15] = 0;

      return ret;
    }
  }
}
