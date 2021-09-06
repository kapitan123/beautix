using System.Linq;
using System.Text.Json.Serialization;

namespace WebApiPoller.Entities.Letu
{
    public class Attributes
    {
        [JsonPropertyName("rawPrice")]
        public int Price { get; set; }

        [JsonPropertyName("product.largeImage.url")]
        public string[] ImageUrls { get; set; }

        [JsonPropertyName("product.displayName")]
        public string[] Names { get; set; }

        [JsonPropertyName("product.brand.name")]
        public string[] Brands { get; set; }

        [JsonPropertyName("analyticsCategory")]
        public string[] Categories { get; set; }

        [JsonPropertyName("product.repositoryId")]
        public string[] Ids { get; set; }

        [JsonPropertyName("product.sefPath")]
        public string[] Urls { get; set; }

        public string Brand => Brands.First();

        public string Name => Names.First();

        public string ImageUrl => ImageUrls.First();

        public string Category => Categories.First();

        public string Id => Ids.First();

        public string Url => $"{Urls.First()}/{Ids.First()}";

        public Product ToPropduct()
        {
            var product = new Product
            {
                LocalId = Id,
                ImageUrl = ImageUrl,
                Price = Price,
                Category = Category,
                Name = Name,
                Source = Source.Letu,
                Url = Url,
                Brand = Brand
            };

            return product;
        }
    }
}
