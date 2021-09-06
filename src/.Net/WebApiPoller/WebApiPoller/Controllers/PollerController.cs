using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApiPoller.Services;
using WebApiPoller.Services.Poller;

namespace WebApiPoller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PollerController : ControllerBase
    {
        private readonly ILogger<PollerController> _logger;

        private readonly IPoller _poller;

        public PollerController(ILogger<PollerController> logger, IPoller poller)
        {
            _logger = logger;
            _poller = poller;
        }

        [HttpGet]
        public async Task Run()
        {
            // AK TODO pass an enumerable what service to poll
            await _poller.Run();
        }
    }
}
