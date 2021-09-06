using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiPoller.Entities.GoldenApple;
using WebApiPoller.Extensions;
using WebApiPoller.Repositories.Interfaces;

namespace WebApiPoller.Services.Poller
{
    public class GoldenApplePoller: IPoller
    {
        private readonly string _url = $"https://goldapple.ru/web_scripts/discover/category/products?cat=7&page=1";

        private IProductRepository _productsRepos;
        private HttpClient _httpClient;

        public GoldenApplePoller(IProductRepository productsRepos)
        {
            _httpClient = new HttpClient();
            _productsRepos = productsRepos;
        }

        public async Task Run()
        {
            var gaProductsResponse = await _httpClient.GetAsync(_url);

            var gaProductString = await gaProductsResponse.Content.ReadAsStringAsync();

            JsonSerializerOptions options = new();
            options.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
            options.PropertyNameCaseInsensitive = true;

            var gaProducts = JsonSerializer.Deserialize<GoldenAppleResponse>(gaProductString, options);

            var products = gaProducts.Products.Select(p => p.ToProduct());

            await _productsRepos.CreateManyProducts(products);
        }
    }
}
