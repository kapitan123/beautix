namespace WebApiPoller.Entities.Letu
{
    public class LetuProduct
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string CategoryType { get; set; }

        public int Price { get; set; }

        public int OldPrice { get; set; }

        public float Discount { get; set; }

        public string Url { get; set; }

        public string ImageUrl { get; set; }

        public LetuProduct()
        {

        }

        public Product MapToProduct()
        {
            var product = new Product
            {
                LocalId = Id,
                ImageUrl = ImageUrl,
                Price = Price,
                Name = Name
            };

            return product;
        }
    }
}
