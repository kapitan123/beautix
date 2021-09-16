using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPoller.Infrastructure.ConfigObjects
{
    public class DatabaseConfig
    {
        public string ConnectionString { get; }
        public string DatabaseName { get; }
        public string CollectionName { get; }
    }
}
