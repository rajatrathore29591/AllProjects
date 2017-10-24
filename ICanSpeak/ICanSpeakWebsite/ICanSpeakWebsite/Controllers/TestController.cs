using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICanSpeakWebsite.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            ViewBag.Message = "Hi Ravish";
            return View();
        }

        public ActionResult _Test()
        {
            ViewBag.Message = "Hi Ravish Its Partial";
            return View("_Test");
        }

    }
}
