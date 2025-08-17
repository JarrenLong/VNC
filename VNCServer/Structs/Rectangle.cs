using System.Runtime.InteropServices;

namespace VNC.Structs
{
  [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8)]
  public struct Rectangle : IByteStruct
  {
    public ushort X;
    public ushort Y;
    public ushort Width;
    public ushort Height;

    public void FromBytes(byte[] data)
    {
      throw new System.NotImplementedException();
    }

    public byte[] ToBytes()
    {
      byte[] ret = new byte[8];

      StructUtils.ToArray(ref ret, 0, X);
      StructUtils.ToArray(ref ret, 2, Y);
      StructUtils.ToArray(ref ret, 4, Width);
      StructUtils.ToArray(ref ret, 6, Height);

      return ret;
    }
  }
}
