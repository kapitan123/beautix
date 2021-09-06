using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiPoller.Entities.GoldenApple;
using WebApiPoller.Extensions;
using WebApiPoller.Repositories.Interfaces;

namespace WebApiPoller.Services.Poller
{
    public class LetuPoller: IPoller
    {
        private readonly string _url = $"https://www.letu.ru/storeru/browse/parfyumeriya?pushSite=storeMobileRU&format=json&pushSite=storeMobileRU&_dynSessConf=6023308454904025890";

        private IProductRepository _productsRepos;
        private HttpClient _httpClient;

        public LetuPoller(IProductRepository productsRepos)
        {
            _httpClient = new HttpClient();
            _productsRepos = productsRepos;
        }

        public async Task Run()
        {
            var letuProductsResponse = await _httpClient.GetAsync(_url);


            var letuProductString = await letuProductsResponse.Content.ReadAsStringAsync();

            JsonSerializerOptions options = new();
            options.PropertyNameCaseInsensitive = true;

            try
            {
                var letuProducts = JsonSerializer.Deserialize<LetuResponse>(letuProductString, options);

                var test = letuProducts.Contents;
            }
            catch (Exception e)
            {
                var test = e;
            }

            //var products = gaProducts.Products.Select(p => p.MapToProduct());

            //await _productsRepos.CreateManyProducts(products);
        }
    }
}
