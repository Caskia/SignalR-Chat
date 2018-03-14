using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace SignalR.Chat.Hubs
{
    public class Chat : Hub
    {
        public async Task JoinRoom(string roomName)
        {
            await Groups.AddAsync(this.Context.ConnectionId, roomName);
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Send(string roomName, string message)
        {
            await Clients.Group(roomName).SendAsync("Send", roomName, message);
        }
    }
}