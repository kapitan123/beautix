using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApiPoller.Entities;
using WebApiPoller.Repositories.Interfaces;

namespace WebApiPoller.Services.Poller
{
    public class PodrygkaPoller : IPoller
    {
        private readonly string _url = $"https://www.podrygka.ru/catalog/parfyumeriya/";

        private IProductRepository _productsRepos;
        private HttpClient _httpClient;

        public PodrygkaPoller(IProductRepository productsRepos)
        {
            _httpClient = new HttpClient();
            _productsRepos = productsRepos;
        }

        public async Task Run()
        {
            try
            {
                var web = new HtmlWeb();
                var document = web.Load(_url);

                var productCardNodes = document.DocumentNode.SelectNodes("//*[@class='item-product-card']").ToList();

                var products = productCardNodes.Select(pcn => HtmlToProduct(pcn));
                
                await _productsRepos.CreateManyProducts(products);
            }
            catch (Exception e)
            {
                var test = e;
            }
        }

        private static Product HtmlToProduct(HtmlNode node)
        {
            var id = node.GetAttributeValue("id", "");

            var price = node.SelectSingleNode("//*[@class='price']").InnerText;

            var brand = node.SelectSingleNode("//*[@class='brand']").InnerText;
            var categoryType = node.SelectSingleNode("//*[@class='category']").InnerText;
            var nameNode = node.SelectSingleNode("//*[@class='products-list-item__title']");

            var name = nameNode.InnerText;
            var url = nameNode.GetAttributeValue("href","");

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
