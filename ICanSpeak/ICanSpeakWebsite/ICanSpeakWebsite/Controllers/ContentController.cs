using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using ICanSpeakWebsite.App_Start;
using ICanSpeakWebsite.Models;
using Newtonsoft.Json;
using ICanSpeakWebsite.HelperClasses;
using System.IO;

namespace ICanSpeakWebsite.Controllers
{
    public class ContentController : Controller
    {
        //
        // GET: /Content/

        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ContactUs(ContactUsModel objContactUs)
        {
            try
            {
                string json = "{\"FullName\":\"" + objContactUs.FullName + "\",\"EmailId\":\"" + objContactUs.EmailId + "\",\"ContactNo\":\"" + objContactUs.ContactNo + "\",\"Message\":\"" + objContactUs.Message + "\"}";
                XmlDocument xd1 = new XmlDocument();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
            return View();
        }




        public ActionResult Mission()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }
    }
}
