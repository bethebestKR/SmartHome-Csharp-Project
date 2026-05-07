using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using SmartHome.Models;

namespace SmartHome.Services
{
    /// <summary>
    /// WPF WebSocket 서버 — Unity 클라이언트와 통신
    /// </summary>
    public class WebSocketServer
    {
        private HttpListener? _listener;
        private WebSocket? _unitySocket;
        private bool _running = false;

        public event Action<bool>? ConnectionChanged;
        public event Action<string>? PacketReceived;
        public event Action<LogEntry>? LogCreated;

        public bool IsConnected => _unitySocket?.State == WebSocketState.Open;

        public async Task StartAsync(int port = 8080)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://localhost:{port}/");
            _listener.Start();
            _running = true;

            Log("시스템", $"WPF WebSocket 서버 시작 — 포트 {port}", LogType.System);

            while (_running)
            {
                try
                {
                    var ctx = await _listener.GetContextAsync();
                    if (ctx.Request.IsWebSocketRequest)
                    {
                        var wsCtx = await ctx.AcceptWebSocketAsync(null);
                        _unitySocket = wsCtx.WebSocket;
                        ConnectionChanged?.Invoke(true);
                        Log("시스템", "Unity 클라이언트 연결됨", LogType.System);
                        await ReceiveLoopAsync(_unitySocket);
                    }
                }
                catch { break; }
            }
        }

        private async Task ReceiveLoopAsync(WebSocket ws)
        {
            var buf = new byte[4096];
            while (ws.State == WebSocketState.Open)
            {
                try
                {
                    var result = await ws.ReceiveAsync(buf, CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                        break;
                    }
                    var msg = Encoding.UTF8.GetString(buf, 0, result.Count);
                    PacketReceived?.Invoke(msg);
                }
                catch { break; }
            }
            ConnectionChanged?.Invoke(false);
            Log("시스템", "Unity 연결 끊김", LogType.System);
        }

        /// <summary>Unity로 패킷 송신</summary>
        public async Task SendAsync(Packet packet)
        {
            if (_unitySocket?.State != WebSocketState.Open) return;
            var json = JsonSerializer.Serialize(packet);
            var bytes = Encoding.UTF8.GetBytes(json);
            await _unitySocket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public void Stop()
        {
            _running = false;
            _listener?.Stop();
        }

        private void Log(string room, string msg, LogType type)
            => LogCreated?.Invoke(new LogEntry { Room = room, Message = msg, Type = type });
    }
}
