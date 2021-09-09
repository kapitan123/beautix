namespace WebApiPoller.Entities.GoldenApple
{
    public class GoldenAppleProduct
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

        public GoldenAppleProduct()
        {

        }
    }
}
