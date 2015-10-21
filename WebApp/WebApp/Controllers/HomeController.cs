using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Properties;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        //public MongoDatabase MongoDatabase;
        public HomeController()
        {
            var mongoClient = new MongoClient(Settings.Default.MongoDBConnectionString);
            
            var database = mongoClient.GetDatabase("KonradRequirements");
            var collection = database.GetCollection<BsonDocument>("User");
            //https://www.mongodb.com/blog/post/introducing-20-net-driver?jmp=docs&_ga=1.196294954.721581952.1441258055
        }
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}