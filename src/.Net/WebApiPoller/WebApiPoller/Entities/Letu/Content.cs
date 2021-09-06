using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPoller.Entities.Letu
{
    public class Content
    {
        public IEnumerable<MainContent> MainContent { get; set; }

        public string Name { get; set; }
    }
}
