namespace VNC.Enums
{
  public enum Encodings : int
  {
    Raw = 0,
    CopyRect = 1,
    Obsolete_RRE = 2,
    Obsolete_Hextile = 5,
    TRLE = 15,
    ZRLE = 16,
    CursorPseudo = -239,
    DesktopSizePseudo = -223
  }
}
