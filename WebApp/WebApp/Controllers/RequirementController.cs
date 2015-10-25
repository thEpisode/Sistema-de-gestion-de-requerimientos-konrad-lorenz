using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class RequirementController : Controller
    {
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
    }
}