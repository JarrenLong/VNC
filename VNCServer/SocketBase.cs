using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace VNC
{
  public delegate void LoggerCb(string msg);

  public abstract class SocketBase : IDisposable
  {
    private LoggerCb Logger = null;
    private Socket listener = null;
    private bool isListening = false;
    private CancellationTokenSource cancelTokenSource;
    private CancellationToken cancelToken;

    public SocketBase(LoggerCb logger)
    {
      Logger = logger;
      cancelTokenSource = new CancellationTokenSource();
      cancelToken = cancelTokenSource.Token;
    }

    protected void ToLog(string msg)
    {
      if (Logger != null)
        Logger.Invoke(msg);
    }
    protected async Task<int> SendAsync(Socket handler, byte[] msg)
    {
      if (!isListening)
        return -1;

      if (msg != null && msg.Length > 0)
        ToLog(string.Format(">>> {0} bytes: {1}", msg.Length, BitConverter.ToString(msg)));
      return await handler.SendAsync(msg, SocketFlags.None);
    }

    protected async Task<byte[]> ReceiveAsync(Socket handler)
    {
      if (!isListening)
        return null;

      var buffer = new byte[1024];
      var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
      if (received != buffer.Length)
        Array.Resize(ref buffer, received);
      if (buffer.Length > 0)
        ToLog(string.Format("<<< {0} bytes: {1}", buffer.Length, BitConverter.ToString(buffer)));
      return buffer;
    }

    public virtual async Task HandleConnection(Socket s, CancellationToken cancellationToken)
    {
      // Do nothing
      await Task.Run(() => { return; });
    }

    public async void StartServer(int port, LoggerCb logger)
    {
      Logger = logger;

      ToLog("Starting server on port " + port + "...");

      // Listen on all interfaces
      IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, port);
      listener = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

      try
      {
        listener.Bind(ipEndPoint);
        listener.Listen(100);
        isListening = true;

        ToLog("Server running");

        while (isListening)
        {
          try
          {
            if (cancelToken.IsCancellationRequested)
            {
              ToLog("Server stop requested");
              break;
            }

            var handler = await listener.AcceptAsync();

            await Task.Run(async () => { await HandleConnection(handler, cancelToken); });
          }
          catch (SocketException s)
          {
            if (isListening)
              ToLog("Socket Exception: " + s);
          }
        }
      }
      catch (Exception e)
      {
        ToLog("Error: " + e);
      }
    }

    public void StopServer()
    {
      ToLog("Stopping server ...");

      isListening = false;
      if (listener != null)
      {
        listener.Close();
        listener = null;
      }

      ToLog("Server stopped");
    }

    public async void StartClient(int port)
    {
      ToLog("Starting VNC client test ...");

      IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Loopback, port);

      try
      {
        using (Socket client = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
        {
          await client.ConnectAsync(ipEndPoint);
          isListening = true;

          ToLog("Client started");

          await Task.Run(async () => { await HandleConnection(client, cancelToken); });

          isListening = false;
          client.Shutdown(SocketShutdown.Both);
          ToLog("Client shutdown");
        }
      }
      catch (Exception ex)
      {
        ToLog("Error: " + ex);
      }
    }

    public void DisconnectClient()
    {
      ToLog("Client disconnect requested");
      cancelTokenSource.Cancel();
      isListening = false;
    }

    public void Dispose()
    {
      DisconnectClient();
      StopServer();
    }
  }
}
