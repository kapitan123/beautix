using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPoller.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public Source Source { get; set; }

        public string LocalId { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string Category { get; set; }

        public int Price { get; set; }

        public string Url { get; set; }

        public string ImageUrl { get; set; }

        public Product()
        {

        }
    }
}
