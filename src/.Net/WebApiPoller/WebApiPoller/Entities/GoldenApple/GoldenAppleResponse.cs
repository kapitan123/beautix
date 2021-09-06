using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPoller.Entities.GoldenApple
{
    public class GoldenAppleResponse
    {
        public IEnumerable<GoldenAppleProduct> Products { get; set; }

        public GoldenAppleResponse()
        {
            Products = new List<GoldenAppleProduct>();
        }
    }
}
