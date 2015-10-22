using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models.Entities;
using WebApp.Properties;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private MongoClient mongoClient;
        IMongoDatabase KonradRequirementsDatabase;
        IMongoCollection<BsonDocument> usersCollection;
        IMongoCollection<BsonDocument> requirementsCollection;

        public HomeController()
        {
            mongoClient = new MongoClient(Settings.Default.MongoDBConnectionString);

            KonradRequirementsDatabase = mongoClient.GetDatabase("KonradRequirements");
            usersCollection = KonradRequirementsDatabase.GetCollection<BsonDocument>("Users");
            requirementsCollection = KonradRequirementsDatabase.GetCollection<BsonDocument>("Requirements");
                        
            //https://www.mongodb.com/blog/post/introducing-20-net-driver?jmp=docs&_ga=1.196294954.721581952.1441258055
        }

        private async void NewMethod(IMongoCollection<BsonDocument> collection)
        {
            var list = await collection.Find(_ => true).ToListAsync();
            foreach (var item in list)
            {

            }
        }

        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult _CreateRequirement()
        {
            return View();
        }

        public ActionResult _CheckStatusRequirement()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetStatusTicket(string ticket)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Ticket", ticket);
            var document = await requirementsCollection.Find(filter).FirstOrDefaultAsync();

            if (document != null)
            {
                Requirement requirement = BsonSerializer.Deserialize<Requirement>(document);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(requirement, JsonRequestBehavior.AllowGet);
            }
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return Json(new { Message = "No se encontró el ticket, asegurate de escribir bien el ticket" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRequirement()
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}