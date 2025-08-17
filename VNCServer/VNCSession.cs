using VNC.Structs;

namespace VNC
{
  public class VNCSession
  {
    public int SessionId { get; private set; }
    public FramebufferParams Specs { get; private set; }
    public DesktopScreen[] Screens { get; set; }

    public VNCSession() : this(0, 0, new FramebufferParams()) { }

    public VNCSession(int desktopUserSessionId, int numScreens, FramebufferParams specs)
    {
      SessionId = desktopUserSessionId;
      Specs = specs;

      Screens = new DesktopScreen[numScreens];
      for (int i = 0; i < numScreens; i++)
        Screens[i] = new DesktopScreen(i, Specs);
    }
  }
}
