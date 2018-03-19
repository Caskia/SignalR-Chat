using Microsoft.AspNetCore.SignalR;
using SignalR.Chat.Services;
using System;
using System.Threading.Tasks;

namespace SignalR.Chat.Hubs
{
    public class Chat : Hub
    {
        private readonly PersistentHubConnectionManager _persistentConnectionManager;

        public Chat(PersistentHubConnectionManager persistentConnectionManager)
        {
            _persistentConnectionManager = persistentConnectionManager;
        }

        public async Task JoinRoom(string roomName)
        {
            await Groups.AddAsync(this.Context.ConnectionId, roomName);
        }

        public override Task OnConnectedAsync()
        {
            _persistentConnectionManager.OnConnectedAsync(this.Context.Connection);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _persistentConnectionManager.OnDisconnectedAsync(this.Context.Connection);

            return base.OnDisconnectedAsync(exception);
        }

        public async Task Send(string roomName, string message)
        {
            await Clients.Group(roomName).SendAsync("Send", roomName, message);
        }
    }
}