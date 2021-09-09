using System.Collections.Generic;
using System.Linq;
using WebApiPoller.Entities;
using WebApiPoller.Services.ApiFetcher.AsyncEnumerators;
using WebApiPoller.Services.Clients;

namespace WebApiPoller.Services.ApiFetcher
{
    // AK TODO looks like this can be converted to a generic
    public partial class ProductApiFetcher : IProductApiFetcher
    {
        private readonly Dictionary<Source, IProductsSourceClient> _clientsMap;

        public ProductApiFetcher(IEnumerable<IProductsSourceClient> clients)
        {
            _clientsMap = clients.ToDictionary(c => c.Source, c => c);
        }

        public IAsyncEnumerable<IEnumerable<Product>> ByCategory(Source source, Category category)
        {
            return new CategoryAsyncEnumerator(_clientsMap[source], category);
        }
    }
}
