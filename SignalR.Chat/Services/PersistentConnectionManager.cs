using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Sockets;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.Chat.Services
{
    public class PersistentConnectionManager
    {
        private readonly ConnectionList connectionList = new ConnectionList();
        private readonly ConcurrentDictionary<string, List<dynamic>> roomContents = new ConcurrentDictionary<string, List<dynamic>>();

        public ConnectionList All
        {
            get
            {
                return connectionList;
            }
        }

        #region Ctor

        public PersistentConnectionManager(
            )
        {
        }

        #endregion Ctor

        #region Connection

        public Task OnConnectedAsync(ConnectionContext connection)
        {
            connectionList.Add(connection);

            return Task.CompletedTask;
        }

        public Task OnDisconnectedAsync(ConnectionContext connection)
        {
            connectionList.Remove(connection);

            return Task.CompletedTask;
        }

        #endregion Connection
    }
}