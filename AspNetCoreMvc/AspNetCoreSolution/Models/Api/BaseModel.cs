using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSolution.Models.Api
{
    public class BaseModel : IDocument<int>
    {
        [BsonId]
        public int Id { get; set; }
        public int Version { get; set; }
    }
}
