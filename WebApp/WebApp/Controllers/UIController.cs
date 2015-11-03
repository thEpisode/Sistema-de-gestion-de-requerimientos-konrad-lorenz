using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
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
    public class UIController : Controller
    {
        private MongoClient mongoClient;
        private IMongoDatabase KonradRequirementsDatabase;
        private IMongoCollection<BsonDocument> usersCollection;
        private IMongoCollection<BsonDocument> requirementsCollection;
        private IMongoCollection<BsonDocument> formsCollection;

        public UIController()
        {
            mongoClient = new MongoClient(Settings.Default.MongoDBConnectionString);

            KonradRequirementsDatabase = mongoClient.GetDatabase("KonradRequirements");
            usersCollection = KonradRequirementsDatabase.GetCollection<BsonDocument>("Users");
            requirementsCollection = KonradRequirementsDatabase.GetCollection<BsonDocument>("Requirements");
            formsCollection = KonradRequirementsDatabase.GetCollection<BsonDocument>("Forms");
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ModifyHeaderColor(string cssClass)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Name", "Default");

            var update = Builders<BsonDocument>.Update
            .Set("HeaderColor", cssClass);

            var result = await formsCollection.UpdateOneAsync(filter, update);

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(cssClass, JsonRequestBehavior.AllowGet);
            
        }

        [HttpPost]
        public async Task<ActionResult> ModifySidebarColor(string cssClass)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Name", "Default");

            var update = Builders<BsonDocument>.Update
            .Set("SidebarColor", cssClass);

            var result = await formsCollection.UpdateOneAsync(filter, update);

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(cssClass, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public async Task<ActionResult> GetTheme()
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Name", "Default");
            var document = await formsCollection.Find(filter).FirstOrDefaultAsync();

            if (document != null)
            {
                Form form = BsonSerializer.Deserialize<Form>(document);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { HeaderColor = form.HeaderColor, SidebarColor = form.SidebarColor }, JsonRequestBehavior.AllowGet);

            }
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return Json(new { Message = "No se encuentra el tema." }, JsonRequestBehavior.AllowGet);
        }
    }
}