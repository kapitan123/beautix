using System.Collections.Generic;
using WebApiPoller.Entities;

namespace WebApiPoller.Services.ApiFetcher
{
    public interface IProductApiFetcher
    {
        IAsyncEnumerable<IEnumerable<Product>> ByCategory(Source source, Category category);
    }
}