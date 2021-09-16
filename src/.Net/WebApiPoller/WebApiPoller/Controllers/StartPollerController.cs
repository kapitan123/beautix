using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebApiPoller.Entities;
using WebApiPoller.Repositories.Interfaces;
using WebApiPoller.Services.ApiFetcher;
using WebApiPoller.Services.Messaging;

namespace WebApiPoller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StartPollerController : ControllerBase
    {
        private readonly ILogger<StartPollerController> _logger;

        private readonly IProductApiFetcher _fetcher;

        private readonly IProductRepository _repo;

        private readonly IMessageProducer _producer;

        public StartPollerController(
            ILogger<StartPollerController> logger, 
            IProductApiFetcher fetcher, 
            IProductRepository repo,
            IMessageProducer producer)
            => (_fetcher, _logger, _repo, _producer) = (fetcher, logger, repo, producer);
        
        // AK TODO add a response, to show how many and which products where imported
        // As it can take some time I can add here a websocket to see results in realtime
        // AK TODO swagger documentation on responses
        // AK RODO ResponseDTO
        // AK TODO one endpoint for websocket real-time updates
        // Other is for background worker start
        // AK TODO move polling to the background task.
        [HttpGet]
        [Route("products/{category:categoryEnum}/{source}")]
        public async Task GetByCategory(Source source, Category category)
        {
            try
            {
                await foreach (var products in _fetcher.ByCategory(source, category))
                {
                    await _repo.CreateManyProducts(products);
                    _producer
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
