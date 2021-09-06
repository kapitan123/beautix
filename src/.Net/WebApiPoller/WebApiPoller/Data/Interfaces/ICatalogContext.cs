 
using MongoDB.Driver;
using WebApiPoller.Entities;

namespace WebApiPoller.Data.Interfaces
{
    public interface ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }
    }
}
