using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class AgentController : Controller
    {
        public AgentController()
        {

        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Authenticate()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}