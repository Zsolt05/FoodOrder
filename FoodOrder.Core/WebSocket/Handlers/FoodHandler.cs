using System.Net.WebSockets;
using System.Text;

namespace FoodOrder.Core.WebSocket.Handlers
{
    public class FoodHandler : WebSocketHandler
    {
        public FoodHandler(ConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
        }

        public override async Task ReceiveAsync(System.Net.WebSockets.WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var message = $"Új étel lett rögzítve: {Encoding.UTF8.GetString(buffer, 0, result.Count)}";

            await SendMessageToAllAsync(message);
        }
    }
}
