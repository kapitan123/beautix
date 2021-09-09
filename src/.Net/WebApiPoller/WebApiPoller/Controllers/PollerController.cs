using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApiPoller.Services;
using WebApiPoller.Services.ApiFetcher;
using WebApiPoller.Services.Poller;

namespace WebApiPoller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PollerController : ControllerBase
    {
        private readonly ILogger<PollerController> _logger;

        private readonly IProductApiFetcher _fetcher;

        public PollerController(ILogger<PollerController> logger, IProductApiFetcher fetcher)
        {
            _logger = logger;
            _fetcher = fetcher;
        }

        [HttpGet]
        [Route("products/{category:categoryEnum}")]
        public async Task GetByCategory()
        {
            // AK TODO pass an enumerable what service to poll
            await _poller.Run();
        }
    }
}
