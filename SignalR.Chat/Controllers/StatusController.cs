using Microsoft.AspNetCore.Mvc;
using SignalR.Chat.Services;
using System.Threading.Tasks;

namespace SignalR.Chat.Controllers
{
    [Route("1/status")]
    public class StatusController : Controller
    {
        private readonly PersistentHubConnectionManager _persistentConnectionManager;

        public StatusController(
            PersistentHubConnectionManager persistentConnectionManager
            )
        {
            _persistentConnectionManager = persistentConnectionManager;
        }

        public Task<dynamic> Index()
        {
            var count = _persistentConnectionManager.All.Count;

            return Task.FromResult<dynamic>(new
            {
                count
            });
        }
    }
}