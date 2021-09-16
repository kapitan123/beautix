using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiPoller.Entities;

namespace WebApiPoller.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();        
        Task<Product> Get(string id);
        Task<IEnumerable<Product>> GetByName(string name);
        Task<IEnumerable<Product>> GetByCategory(string categoryName);

        Task Create(Product product);
        Task<IEnumerable<string>> CreateMany(IEnumerable<Product> products);
        Task<bool> Update(Product product);
        Task<bool> Delete(string id);
    }
}
