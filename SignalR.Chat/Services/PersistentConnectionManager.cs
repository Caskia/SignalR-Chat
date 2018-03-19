using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.Chat.Services
{
    public class PersistentConnectionManager
    {
        private static Object objLock = new Object();
        private readonly HubConnectionList connectionList = new HubConnectionList();
        private readonly ConcurrentDictionary<string, List<dynamic>> roomContents = new ConcurrentDictionary<string, List<dynamic>>();

        public HubConnectionList All
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

        public Task OnConnectedAsync(HubConnectionContext connection)
        {
            connection.Metadata["subscribe"] = new HashSet<string>();
            connectionList.Add(connection);

            return Task.CompletedTask;
        }

        public Task OnDisconnectedAsync(HubConnectionContext connection)
        {
            connectionList.Remove(connection);

            return Task.CompletedTask;
        }

        #endregion Connection
    }
}