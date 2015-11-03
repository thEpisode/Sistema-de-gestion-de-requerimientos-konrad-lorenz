using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models.Entities
{
    public class Form
    {
        [BsonId]
        ObjectId _id { get; set; }
        public string Name { get; set; }
        public string Template { get; set; }
        public string HeaderColor { get; set; }
        public string SidebarColor { get; set; }
    }
}
