using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace FoodOrder.Core.WebSocket
{
    public class ConnectionManager
    {
        private ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> _sockets = new ConcurrentDictionary<string, System.Net.WebSockets.WebSocket>();

        public System.Net.WebSockets.WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> GetAll()
        {
            return _sockets;
        }

        public string GetId(System.Net.WebSockets.WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key;
        }
        public void AddSocket(System.Net.WebSockets.WebSocket socket)
        {
            _sockets.TryAdd(CreateConnectionId(), socket);
        }

        public async Task RemoveSocket(string id)
        {
            _sockets.TryRemove(id, out System.Net.WebSockets.WebSocket socket);

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the ConnectionManager",
                                    cancellationToken: CancellationToken.None);
        }

        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
