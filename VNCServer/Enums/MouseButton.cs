namespace VNC.Enums
{
  public enum MouseButton : byte
  {
    None = 0,
    Left = 1,
    Middle = 2,
    Right = 3,
    // Send Button down+up to trigger mouse wheel up
    WheelUp = 4,
    // Send Button down+up to trigger mouse wheel down
    WheelDown = 5
  }
}
