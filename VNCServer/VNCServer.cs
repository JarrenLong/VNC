using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VNC.Enums;
using VNC.Structs;

namespace VNC
{
  public class VNCServer : SocketBase, IDisposable
  {
    public VNCServer(LoggerCb logger) : base(logger)
    {
    }

    private async Task<bool> ClientHandshake(Socket handler)
    {
      // Send the ProtocolVersion Handshake (v3.8)
      await SendAsync(handler, Encoding.UTF8.GetBytes("RFB 003.008\n"));

      // Make sure client sends the same version
      byte[] rxBuf = await ReceiveAsync(handler);
      string ver = Encoding.UTF8.GetString(rxBuf, 0, rxBuf.Length);

      if (ver == "RFB 003.008\n")
      {
        // Send the supported security versions
        await SendAsync(handler, new byte[2] { 1, (byte)SecurityType.VNCAuthentication });

        // Let the client pick the supported security type
        rxBuf = await ReceiveAsync(handler);
        SecurityType t = (SecurityType)rxBuf[0];

        if (t == SecurityType.VNCAuthentication)
        {
          // TODO: Send random challenge...
          await SendAsync(handler, new byte[16] {
            1, 2, 3, 4,
            5, 6, 7, 8,
            8, 7, 6, 5,
            4, 3, 2, 1
          });

          // read challenge response
          await ReceiveAsync(handler);
        }

        // Success, handshake complete
        await SendAsync(handler, new byte[4] { 0, 0, 0, 0 });

        return true;
      }

      return false;
    }

    public override async Task HandleConnection(Socket s, CancellationToken cancelToken)
    {
      ToLog("Client connected");

      if (!await ClientHandshake(s))
      {
        ToLog("Handshake failed, aborting connection");
        return;
      }
      ToLog("Client connected!");

      FramebufferParams fb = new FramebufferParams();
      fb.WidthPixels = 1024;
      fb.HeightPixels = 768;
      fb.FormatPixels.BitsPerPixel = 24;
      fb.FormatPixels.Depth = 24;
      fb.FormatPixels.BigEndianFlag = 1;
      fb.FormatPixels.TrueColorFlag = 0;

      await SendAsync(s, fb.ToBytes());

      //// Read ClientInit
      //byte[] rxBuf = await ReceiveAsync(s);
      //bool shareDesktop = rxBuf[0] != 0;

      // Send ServerInit

      //byte[] buf = ServerCommand.FrameBufferUpdate();
      byte[] rxBuf = new byte[0];

      while (!cancelToken.IsCancellationRequested)
      {
        rxBuf = await ReceiveAsync(s);
        ToLog("Received data from client, processing ...");

        if (rxBuf.Length == 0)
        {
          Thread.Sleep(10);
          continue;
        }

        // Read data, process the message
        byte[] rsp = await ClientCommand.Process(rxBuf);
        if (rsp != null)
          await SendAsync(s, rsp);
      }
    }
  }
}
