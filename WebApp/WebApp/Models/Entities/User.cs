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
    public class User
    {
        [BsonId]
        ObjectId Id { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public Role Role { get; set; }
    }
}
