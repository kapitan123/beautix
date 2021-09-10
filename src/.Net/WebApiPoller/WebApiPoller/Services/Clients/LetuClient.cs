using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiPoller.Entities;
using WebApiPoller.Entities.GoldenApple;
using WebApiPoller.Entities.Letu;

namespace WebApiPoller.Services.Clients
{
    public class LetuClient : IProductsSourceClient
    {
        private readonly Dictionary<Category, string> categoryMapping = new()
        {
            [Category.Parfume] = "parfyumeriya"
        };

        private readonly string _baseUrl = $"https://www.letu.ru/storeru/browse/";

        private readonly HttpClient _httpClient;

        public Source Source => Source.Letu;

        public LetuClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<IEnumerable<Product>> FetchFromCategoryPage(Category category, int pageNumber)
        {
            var url = GetCategoryUrl(categoryMapping[category], pageNumber);

            var letuProductsResponse = await _httpClient.GetAsync(url);

            var letuProductString = await letuProductsResponse.Content.ReadAsStringAsync();

            var _options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            var letuProducts = JsonSerializer.Deserialize<LetuResponse>(letuProductString, _options);

            var products = letuProducts.Contents.First().MainContent.First(mc => mc.Records != null)
    .               Records.Select(r => ToPropduct(r.Attributes));

            return products;
        }

        private string GetCategoryUrl(string category, int page)
        {
            var pagePlaceholder = page > 1 ? $"/page-{page}" : "";

            return $"{_baseUrl}{category}{pagePlaceholder}?format=json";
        }

        public Product ToPropduct(Attributes a)
        {
            var product = new Product
            {
                LocalId = a.Id,
                ImageUrl = a.ImageUrl,
                Price = a.Price,
                Category = a.Category,
                Name = a.Name,
                Source = Source.Letu,
                Url = a.Url,
                Brand = a.Brand
            };

            return product;
        }
    }
}
