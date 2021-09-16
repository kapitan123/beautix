using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiPoller.Entities;
using WebApiPoller.Entities.GoldenApple;
using WebApiPoller.Infrastructure.JsonSerialization;

namespace WebApiPoller.Services.Clients
{
    public class GoldenAppleClient: IProductsSourceClient
    {
        private readonly Dictionary<Category, int> categoryMapping = new()
        {
            [Category.Parfume] = 7
        };

        private readonly string _baseUrl = $"https://goldapple.ru/web_scripts/discover/";

        private readonly HttpClient _httpClient;


        private readonly JsonSerializerOptions _options;

        public Source Source => Source.GoldenApple;

        public GoldenAppleClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
                    PropertyNameCaseInsensitive = true
                };
        }

        public async Task<IEnumerable<Product>> FetchFromCategoryPage(Category category, int pageNumber)
        {
            var url = GetCategoryUrl(categoryMapping[category], pageNumber);

            var gaProductsResponse = await _httpClient.GetAsync(url);

            var gaProductString = await gaProductsResponse.Content.ReadAsStringAsync();

            var gaProducts = JsonSerializer.Deserialize<GoldenAppleResponse>(gaProductString, _options);

            // AK TODO check what happens if there is no products
            var products = gaProducts.Products.Select(p => ToProduct(p));

            return products;
        }

        private static string GetCategoryUrl(int category, int page)
        {
            return $"category/products?cat={category}&page={page}";
        }

        private static Product ToProduct(GoldenAppleProduct p)
        {
            var product = new Product
            {
                LocalId = p.Id,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                Category = p.CategoryType,
                Name = p.Name,
                Source = Source.GoldenApple,
                Url = p.Url,
                Brand = p.Brand
            };

            return product;
        }
    }
}
