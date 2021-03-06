﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
        
        public ActionResult GetForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModifyForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Detail(string ticket)
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

        [HttpPost]
        public async Task<ActionResult> CreateRequirement(FormCollection dataCollection)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Name", "Default");
            var document = await formsCollection.Find(filter).FirstOrDefaultAsync();


            if (document != null)
            {
                Dictionary<string, string> requirementDictionary = new Dictionary<string, string>();

                // Getting template
                Form form = BsonSerializer.Deserialize<Form>(document);
                string[] items = form.Template.Split(';');

                // Setting data into template
                foreach (string item in items)
                {
                    string[] field = item.Split('|');

                    string value = dataCollection[field[1]];

                    requirementDictionary.Add(field[1], value);
                }

                // Generating ticket
                Random rnd = new Random();
                StringBuilder ticketBuilder = new StringBuilder();
                ticketBuilder.AppendFormat("{0}{1}{2}{3}", ((char)rnd.Next(65, 90)).ToString(), ((char)rnd.Next(97, 122)).ToString(), ((char)rnd.Next(97, 122)).ToString(), (rnd.Next(0, 999)).ToString());

                // Creating the requirement
                Requirement requirement = new Requirement()
                {
                    Rate = 0,
                    Status = Models.Enums.Status.Stack,
                    Ticket = ticketBuilder.ToString(),
                    Data = requirementDictionary
                };

                try
                {
                    await requirementsCollection.InsertOneAsync(requirement.ToBsonDocument());
                }
                catch (Exception)
                {
                    await requirementsCollection.InsertOneAsync(requirement.ToBsonDocument());
                }

                Response.StatusCode = (int)HttpStatusCode.Created;
                return Json(ticketBuilder.ToString(), JsonRequestBehavior.AllowGet);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "No se encuentra la plantilla."}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRequirements()
        {
            var filter = new BsonDocument();

            List<BsonDocument> documents = null;

            try
            {
                documents = await requirementsCollection.Find(filter).ToListAsync();
            }
            catch (Exception)
            {
                documents = await requirementsCollection.Find(filter).ToListAsync();
            }

            List<Requirement> requirements = new List<Requirement>();

            if (documents != null)
            {
                if (documents.Count > 0)
                {
                    foreach (var item in documents)
                    {
                        Requirement requirement = BsonSerializer.Deserialize<Requirement>(item);
                        requirements.Add(requirement);
                    }
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(requirements, JsonRequestBehavior.AllowGet);
                }
            }

            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return Json(new { Message = "No existe ningún requerimiento registrado." }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public async Task<ActionResult> GetRequirementsCount()
        {
            var filter = new BsonDocument();
            
            var documents = await requirementsCollection.Find(filter).ToListAsync();

            if (documents.Count > 0)
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(documents.Count, JsonRequestBehavior.AllowGet);
            }

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> AssignAgent(string ticket, string agentUsername)
        {
            if (!String.IsNullOrWhiteSpace(ticket))
            {
                var filter = Builders<BsonDocument>.Filter.Eq("Ticket", ticket);

                var update = Builders<BsonDocument>.Update
                        .Set("AgentUsername", agentUsername);

                var result = await requirementsCollection.UpdateOneAsync(filter, update);

                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { Message = "Ok" }, JsonRequestBehavior.AllowGet);

                
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Por favor indique qué usuario quiere modificar" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetById(string ticket)
        {
            if (!String.IsNullOrWhiteSpace(ticket))
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
                return Json(new { Message = "No se encuentra el requerimiento." }, JsonRequestBehavior.AllowGet);
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Por favor indique qué requerimiento necesita." }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetMyRequirements(string username)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("AgentUsername", username);

            var documents = await requirementsCollection.Find(filter).ToListAsync();

            List<Requirement> requirements = new List<Requirement>();

            if (documents.Count > 0)
            {
                foreach (var item in documents)
                {
                    Requirement requirement = BsonSerializer.Deserialize<Requirement>(item);
                    requirements.Add(requirement);
                }
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(requirements, JsonRequestBehavior.AllowGet);
            }

            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return Json(new { Message = "No existe ningún requerimiento registrado." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeStatus(string ticket, int status)
        {
            if (!String.IsNullOrWhiteSpace(ticket))
            {
                var filter = Builders<BsonDocument>.Filter.Eq("Ticket", ticket);

                var update = Builders<BsonDocument>.Update
                        .Set("Status", status);

                var result = await requirementsCollection.UpdateOneAsync(filter, update);

                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { Message = "Ok" }, JsonRequestBehavior.AllowGet);


            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Por favor indique qué usuario quiere modificar" }, JsonRequestBehavior.AllowGet);
        }
    }
}