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
    public class AgentController : Controller
    {
        private MongoClient mongoClient;
        private IMongoDatabase KonradRequirementsDatabase;
        private IMongoCollection<BsonDocument> usersCollection;
        private IMongoCollection<BsonDocument> requirementsCollection;

        public AgentController()
        {
            mongoClient = new MongoClient(Settings.Default.MongoDBConnectionString);

            KonradRequirementsDatabase = mongoClient.GetDatabase("KonradRequirements");
            usersCollection = KonradRequirementsDatabase.GetCollection<BsonDocument>("Users");
            requirementsCollection = KonradRequirementsDatabase.GetCollection<BsonDocument>("Requirements");
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Me()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult All()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        /// <summary>
        /// Used for authenticate user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Authenticate(string username, string password)
        {
            if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password))
            {
                var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
                var document = await usersCollection.Find(filter).FirstOrDefaultAsync();

                if (document != null)
                {
                    User user = BsonSerializer.Deserialize<User>(document);
                    if (user.Password.Equals(password))
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(user, JsonRequestBehavior.AllowGet);
                    }
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json(new { Message = "Contraseña erronea." }, JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Message = "El usuario no existe." }, JsonRequestBehavior.AllowGet);
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Por favor indique usuario y contraseña" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetProfilePhoto(string fileName)
        {
            GridFSBucketOptions bucketOptions = new GridFSBucketOptions() { BucketName = "ProfileImages" };
            var fs = new GridFSBucket(KonradRequirementsDatabase, bucketOptions);

            var t = fs.DownloadAsBytesByNameAsync(fileName);
            Task.WaitAll(t);
            byte[] image = t.Result;
            return File(image, "image/jpg"); ;
        }

        public async Task<ActionResult> GetAll()
        {
            var agentRole = Builders<BsonDocument>.Filter.Eq("Role", 1);
            var managingRole = Builders<BsonDocument>.Filter.Eq("Role", 0);
            var joinFilter = Builders<BsonDocument>.Filter.Or(agentRole, managingRole);

            List<User> agents = new List<User>();

            var documents = await usersCollection.Find(joinFilter).ToListAsync();
            
            if (documents.Count > 0)
            {
                foreach (var item in documents)
                {
                    User user = BsonSerializer.Deserialize<User>(item);
                    agents.Add(user);
                }
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(agents, JsonRequestBehavior.AllowGet);
            }

            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return Json(new { Message = "No existe ningún agente registrado." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAgent(User user)
        {
            await usersCollection.InsertOneAsync(user.ToBsonDocument());
            Response.StatusCode = (int)HttpStatusCode.Created;
            return Json(new { Message = "Usuario registrado." }, JsonRequestBehavior.AllowGet);
        }
    }
}