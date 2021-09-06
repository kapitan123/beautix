using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiPoller.Entities.Letu;

namespace WebApiPoller.Entities.GoldenApple
{
    public class LetuResponse
    {
        public IEnumerable<Content> Contents { get; set; }
    }
}
