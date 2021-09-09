using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebApiPoller.Entities;
using WebApiPoller.Repositories.Interfaces;
using WebApiPoller.Services.ApiFetcher;

namespace WebApiPoller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StartPollerController : ControllerBase
    {
        private readonly ILogger<StartPollerController> _logger;

        private readonly IProductApiFetcher _fetcher;

        private readonly IProductRepository _repo;

        public StartPollerController(ILogger<StartPollerController> logger, IProductApiFetcher fetcher, IProductRepository repo)
        {
            _logger = logger;
            _fetcher = fetcher;
            _repo = repo;
        }

        // AK TODO add a response, to show how many and which products where imported
        // As it can take some time I can add here a websocket to see results in realtime
        [HttpGet]
        [Route("products/{category:categoryEnum}")]
        public async Task GetByCategory(Category cat)
        {
            try
            {
                // AK TODO add source
                var source = Source.GoldenApple;

                // AK TODO pass an enumerable what service to poll
                await foreach (var products in _fetcher.ByCategory(source, cat))
                {
                    await _repo.CreateManyProducts(products);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
