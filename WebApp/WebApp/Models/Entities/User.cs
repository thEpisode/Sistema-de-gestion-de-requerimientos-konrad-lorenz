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
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string ProfileImageName { get; set; }
    }
}
