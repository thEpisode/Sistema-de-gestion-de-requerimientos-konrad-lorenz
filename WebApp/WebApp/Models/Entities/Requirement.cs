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
    class Requirement
    {
        [BsonId]
        ObjectId _id { get; set; }
        public int Rate { get; set; }
        public Status Status { get; set; }
        public string Ticket { get; set; }
    }
}
