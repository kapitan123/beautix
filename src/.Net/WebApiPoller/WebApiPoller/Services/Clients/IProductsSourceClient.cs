using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiPoller.Entities;

namespace WebApiPoller.Services.Clients
{
    // AK TODO can split the interface to ICategoryFetch, IProductFetch, So It can be more atomic
    public interface IProductsSourceClient
    {
        /// <summary>
        /// Source
        /// </summary>
        Source Source { get; }

        /// <summary>
        /// Does not throw on nonexistant category
        /// </summary>
        /// <param name="category"></param>
        /// <param name="pageNumber"></param>
        /// <returns>List of products of a page, empty list of there is no products</returns>
        Task<IEnumerable<Product>> FetchFromCategoryPage(Category category, int pageNumber);

        // GetProductById
        // GetProductBy URL
    }
}
