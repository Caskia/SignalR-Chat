using System.Threading.Tasks;
using Microsoft.AspNetCore.Sockets;
using SignalR.Chat.Services;

namespace SignalR.Chat.EndPoints
{
    public class StockEndPoint : EndPoint
    {
        private readonly PersistentConnectionManager _persistentConnectionManager;

        public StockEndPoint(PersistentConnectionManager persistentConnectionManager)
        {
            _persistentConnectionManager = persistentConnectionManager;
        }

        public override async Task OnConnectedAsync(ConnectionContext connection)
        {
            await ConnectAsync(connection);

            try
            {
                while (await connection.Transport.Reader.WaitToReadAsync())
                {
                }
            }
            finally
            {
                await DisconnectAsync(connection);
            }
        }

        private async Task ConnectAsync(ConnectionContext connection)
        {
            await _persistentConnectionManager.OnConnectedAsync(connection);
        }

        private async Task DisconnectAsync(ConnectionContext connection)
        {
            await _persistentConnectionManager.OnDisconnectedAsync(connection);
        }
    }
}