using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICanSpeakWebsite.Controllers
{
    public class PaymentController : Controller
    {
        //
        // GET: /Payment/

        public ActionResult PaymentInfo()
        {
            ViewBag.Username = System.Web.HttpRuntime.Cache["Username"].ToString();
            ViewBag.Country = System.Web.HttpRuntime.Cache["Country"].ToString();
            ViewBag.ProfilePicture = System.Web.HttpRuntime.Cache["ProfilePicture"].ToString();
            return View();
        }

    }
}
