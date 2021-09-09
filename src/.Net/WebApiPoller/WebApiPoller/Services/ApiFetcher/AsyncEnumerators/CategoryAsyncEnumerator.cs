using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiPoller.Entities;
using WebApiPoller.Services.Clients;

namespace WebApiPoller.Services.ApiFetcher.AsyncEnumerators
{
    public class CategoryAsyncEnumerator : IAsyncEnumerator<IEnumerable<Product>>, IAsyncEnumerable<IEnumerable<Product>>
    {
        public int CurrentPage { get; private set; }

        public IEnumerable<Product> Current { get; private set; }

        private readonly IProductsSourceClient _gaClient;

        private readonly Category _category;

        public CategoryAsyncEnumerator(IProductsSourceClient gaClient, Category category)
        {
            CurrentPage = 0;
            _gaClient = gaClient;
            _category = category;
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            var nextPage = CurrentPage + 1;
            var result = await _gaClient.FetchFromCategoryPage(_category, nextPage);

            if (result.Any())
            {
                return false;
            }

            Current = result;
            CurrentPage = nextPage;
            return true;
        }

        public ValueTask DisposeAsync()
        {
            // nothing to dispose, connection is managed by parent class
            return default;       
        }

        public IAsyncEnumerator<IEnumerable<Product>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            // Enumerator does not support cancellation so far
            return this;
        }
    }
}
