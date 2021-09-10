using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiPoller.Entities;

namespace WebApiPoller.Data
{
    public class ConfigureMongoDbIndexesService : IHostedService
    {
        private readonly IMongoClient _client;
        private readonly ILogger<ConfigureMongoDbIndexesService> _logger;
        private readonly IConfiguration _configuration;
        public ConfigureMongoDbIndexesService(IMongoClient client, 
            ILogger<ConfigureMongoDbIndexesService> logger, IConfiguration configuration)
            => (_client, _logger, _configuration) = (client, logger, configuration);

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var database = _client.GetDatabase(_configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
            var collection = database.GetCollection<Product>(_configuration.GetValue<string>("DatabaseSettings:CollectionName"));

            _logger.LogInformation("Creating Source + LocalId Index on Products");

            await collection.Indexes.CreateOneAsync(
                ComposeUniqueIndexxOnSourceAndLocalId(),
                cancellationToken: cancellationToken);
        }

        private static CreateIndexModel<Product> ComposeUniqueIndexxOnSourceAndLocalId()
        {
            var indexKeysDefinition = Builders<Product>
                .IndexKeys
                .Ascending(x => x.Source)
                .Ascending(x => x.LocalId);

            var options = new CreateIndexOptions() { Unique = true };

            return new CreateIndexModel<Product>(indexKeysDefinition, options);
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
