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
    public class RequirementController : Controller
    {
        private MongoClient mongoClient;
        private IMongoDatabase KonradRequirementsDatabase;
        private IMongoCollection<BsonDocument> usersCollection;
        private IMongoCollection<BsonDocument> requirementsCollection;
        private IMongoCollection<BsonDocument> formsCollection;

        public RequirementController()
        {
            mongoClient = new MongoClient(Settings.Default.MongoDBConnectionString);

            KonradRequirementsDatabase = mongoClient.GetDatabase("KonradRequirements");
            usersCollection = KonradRequirementsDatabase.GetCollection<BsonDocument>("Users");
            requirementsCollection = KonradRequirementsDatabase.GetCollection<BsonDocument>("Requirements");
            formsCollection = KonradRequirementsDatabase.GetCollection<BsonDocument>("Forms");
        }

        /// <summary>
        /// Get all requirements
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyRequirements()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetMyRequirements(string username)
        {
            return View();
        }

        public ActionResult GetForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModifyForm()
        {
            return View();
        }

        public async Task<ActionResult> GetTemplateByName(string name = "Default")
        {
            if (!String.IsNullOrWhiteSpace(name))
            {
                var filter = Builders<BsonDocument>.Filter.Eq("Name", name);
                var document = await formsCollection.Find(filter).FirstOrDefaultAsync();

                if (document != null)
                {
                    Form form = BsonSerializer.Deserialize<Form>(document);
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(form, JsonRequestBehavior.AllowGet);

                }
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Message = "La plantilla que está buscando no se encuentra." }, JsonRequestBehavior.AllowGet);
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Por favor indique qué plantilla desea modificar" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> EditTemplate(Form form)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Name", form.Name);

            var update = Builders<BsonDocument>.Update
            .Set("Template", form.Template);

            var result = await formsCollection.UpdateOneAsync(filter, update);

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}