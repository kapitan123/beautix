using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using WebApiPoller.Data.Interfaces;
using WebApiPoller.Entities;
using WebApiPoller.Infrastructure.ConfigObjects;

namespace WebApiPoller.Data
{
    public class CatalogContext: ICatalogContext
    {
        public CatalogContext(DatabaseConfig configuration, IMongoClient client)
        {
            var database = client.GetDatabase(configuration.DatabaseName);
            Products = database.GetCollection<Product>(configuration.CollectionName);
        }

        public IMongoCollection<Product> Products { get; }
    }
}
