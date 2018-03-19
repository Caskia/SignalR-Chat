using Microsoft.AspNetCore.Mvc;
using SignalR.Chat.Services;
using System.Threading.Tasks;

namespace SignalR.Chat.Controllers
{
    [Route("1/status")]
    public class StatusController : Controller
    {
        private readonly PersistentConnectionManager _persistentConnectionManager;
        private readonly PersistentHubConnectionManager _persistentHubConnectionManager;

        public StatusController(
            PersistentConnectionManager persistentConnectionManager,
            PersistentHubConnectionManager persistentHubConnectionManager
            )
        {
            _persistentConnectionManager = persistentConnectionManager;
            _persistentHubConnectionManager = persistentHubConnectionManager;
        }

        public Task<dynamic> Index()
        {
            var hubCount = _persistentHubConnectionManager.All.Count;
            var websocketCount = _persistentConnectionManager.All.Count;

            return Task.FromResult<dynamic>(new
            {
                websocketCount,
                hubCount
            });
        }
    }
}