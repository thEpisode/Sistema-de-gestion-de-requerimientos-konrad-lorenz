using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models.Enums;

namespace WebApp.Models.Entities
{
    [BsonIgnoreExtraElements]
    class Requirement
    {
        [BsonId]
        ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Rate { get; set; }
        public Status Status { get; set; }
    }
}
