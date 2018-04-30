using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSolution.Models.Api.Library
{
    public class Counter : IDocument<String>
    {
        [BsonId]
        public string Id { get; set; }
        public int Version { get; set; }
        public int Count { get; set; }
    }
}
