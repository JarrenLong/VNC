using System.Runtime.InteropServices;

namespace VNC.Structs
{
  [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 32)]
  public struct Pixel : IByteStruct
  {
    public uint Data;

    public ushort GetRed(PixelFormat pf)
    {
      return (ushort)(Data << pf.RedShift & pf.RedMax);
    }
    public ushort GetGreen(PixelFormat pf)
    {
      return (ushort)(Data << pf.GreenShift & pf.GreenMax);
    }
    public ushort GetBlue(PixelFormat pf)
    {
      return (ushort)(Data << pf.BlueShift & pf.BlueMax);
    }

    public byte[] ToBytes()
    {
      throw new System.NotImplementedException();
    }

    public void FromBytes(byte[] data)
    {
      throw new System.NotImplementedException();
    }
  }
}
