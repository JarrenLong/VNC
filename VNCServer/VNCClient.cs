using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VNC.Enums;

namespace VNC
{
  public class VNCClient : SocketBase, IDisposable
  {
    public VNCClient(LoggerCb logger) : base(logger)
    {
    }

    private Socket conn;

    public override async Task HandleConnection(Socket s, CancellationToken cancelToken)
    {
      conn = s;

      if (!await InitClient())
      {
        ToLog("Handshake failed, aborting connection");
        return;
      }
      ToLog("Connected to server!");

      byte[] rxBuf = new byte[0];
      while (!cancelToken.IsCancellationRequested)
      {
        rxBuf = await ReceiveAsync(s);
        ToLog("Received data from server, processing ...");

        if (rxBuf.Length == 0)
        {
          Thread.Sleep(10);
          continue;
        }

        // Read data, process the message
        byte[] rsp = await ServerCommand.Process(rxBuf);
        if (rsp != null)
          await SendAsync(s, rsp);
      }
    }

    private async Task<bool> InitClient()
    {
      // Read the version, echo it back and shutdown
      byte[] rxBuf = await ReceiveAsync(conn);
      string ver = Encoding.UTF8.GetString(rxBuf, 0, rxBuf.Length);

      await SendAsync(conn, Encoding.UTF8.GetBytes(ver));

      // Read in the supported security types and pick the None option
      await ReceiveAsync(conn);
      await SendAsync(conn, new byte[1] { (byte)SecurityType.VNCAuthentication });

      byte[] ok = await ReceiveAsync(conn);
      if (ok != null && ok.Length == 4)
      {
        // Send ClientInit (0 = disconnect other clients, 1 - share desktop)
        await SendAsync(conn, new byte[1] { 0 });

        
        return true;
      }

      return false;
    }

    public async Task MouseEvent(MouseButton button, bool isDown, int x, int y)
    {
      ToLog(string.Format("Mouse event: {0} {1} ({2}, {3})", button, isDown, x, y));

      byte[] cmd = ClientCommand.PointerEvent(button, isDown, (ushort)x, (ushort)y);
      await SendAsync(conn, cmd);

      await Redraw(0, 0, 1024, 768);
    }

    public async Task KeyboardEvent(Keys button, bool isDown)
    {
      ToLog(string.Format("Keyboard event: {0} {1}", button, isDown));

      byte[] cmd = ClientCommand.KeyEvent(isDown, button);
      await SendAsync(conn, cmd);

      await Redraw(0, 0, 1024, 768);
    }

    public async Task Redraw(int x, int y, int w, int h)
    {
      ToLog(string.Format("Redraw: [{0},{1},{2},{3}]", x, y, w, h));

      byte[] cmd = ClientCommand.FrameBufferUpdateRequest(false, new Structs.Rectangle() { X = (ushort)x, Y = (ushort)y, Width = (ushort)w, Height = (ushort)h });
      await SendAsync(conn, cmd);

      byte[] ret = await ReceiveAsync(conn);

      int brk = 0;
    }
  }
}
