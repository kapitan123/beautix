using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiPoller.Entities;

namespace WebApiPoller.Services.Clients
{
    public class PodrygkaClient : IProductsSourceClient
    {
        private readonly Dictionary<Category, string> categoryMapping = new()
        {
            [Category.Parfume] = "parfyumeriya"
        };

        private readonly string _baseUrl = $"https://www.podrygka.ru/catalog/";

        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options;

        public Source Source => Source.Podrygka;

        public PodrygkaClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IEnumerable<Product>> FetchFromCategoryPage(Category category, int pageNumber)
        {
            var url = GetCategoryUrl(categoryMapping[category], pageNumber);

            var letuProductsResponse = await _httpClient.GetAsync(url);

            var letuProductString = await letuProductsResponse.Content.ReadAsStringAsync();

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(letuProductString);

            var productCardNodes = GetProductNodes(htmlDocument);

            var products = productCardNodes.Select(pcn => HtmlToProduct(pcn));

            return products;
        }

        private static List<HtmlNode> GetProductNodes(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectNodes("//*[@class='item-product-card']").ToList();
        }

        private string GetCategoryUrl(string category, int page)
        {
            return $"{_baseUrl}{category}/?PAGEN_1={page}";
        }

        private static Product HtmlToProduct(HtmlNode node)
        {
            var id = node.GetAttributeValue("id", "");

            var price = node.SelectSingleNode("//*[@class='price']").InnerText;

            var brand = node.SelectSingleNode("//*[@class='brand']").InnerText;
            var categoryType = node.SelectSingleNode("//*[@class='category']").InnerText;
            var nameNode = node.SelectSingleNode("//*[@class='products-list-item__title']");

            var name = nameNode.InnerText;
            var url = nameNode.GetAttributeValue("href", "");

            var imageUrl = node.SelectSingleNode("//*[@class='products-list-item__image']/img")
                .GetAttributeValue("src", "");

            var product = new Product
            {
                LocalId = id,
                ImageUrl = imageUrl,
                Price = int.Parse(price),
                Category = categoryType,
                Name = name,
                Source = Source.Podrygka,
                Url = url,
                Brand = brand
            };

            return product;
        }
    }
}
