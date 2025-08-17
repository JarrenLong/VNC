using System;

namespace VNC
{
  public interface IByteStruct
  {
    public byte[] ToBytes();
    public void FromBytes(byte[] data);
  }

  public class StructUtils
  {
    public static int IntFromArray(ref byte[] arr, int offset)
    {
      if (arr == null || offset < 0)
        throw new ArgumentException("Param error");

      return (int)(arr[offset] << 24 | arr[offset + 1] << 16 | arr[offset + 2] << 8 | arr[offset + 3]);
    }
    public static uint UintFromArray(ref byte[] arr, int offset)
    {
      if (arr == null || offset < 0)
        throw new ArgumentException("Param error");

      return (uint)(arr[offset] << 24 | arr[offset + 1] << 16 | arr[offset + 2] << 8 | arr[offset + 3]);
    }
    public static ushort UshortFromArray(ref byte[] arr, int offset)
    {
      if (arr == null || offset < 0)
        throw new ArgumentException("Param error");

      return (ushort)(arr[offset] << 8 | arr[offset + 1]);
    }

    public static void ToArray(ref byte[] arr, int offset, int data)
    {
      if (arr == null || offset < 0)
        throw new ArgumentException("Param error");

      arr[offset] = (byte)((data >> 24) & 0xFF);
      arr[offset + 1] = (byte)((data >> 16) & 0xFF);
      arr[offset + 2] = (byte)((data >> 8) & 0xFF);
      arr[offset + 3] = (byte)(data & 0xFF);
    }
    public static void ToArray(ref byte[] arr, int offset, uint data)
    {
      if (arr == null || offset < 0)
        throw new ArgumentException("Param error");

      arr[offset] = (byte)((data >> 24) & 0xFF);
      arr[offset + 1] = (byte)((data >> 16) & 0xFF);
      arr[offset + 2] = (byte)((data >> 8) & 0xFF);
      arr[offset + 3] = (byte)(data & 0xFF);
    }
    public static void ToArray(ref byte[] arr, int offset, ushort data)
    {
      if (arr == null || offset < 0)
        throw new ArgumentException("Param error");

      arr[offset] = (byte)((data >> 8) & 0xFF);
      arr[offset + 1] = (byte)(data & 0xFF);
    }
  }
}
