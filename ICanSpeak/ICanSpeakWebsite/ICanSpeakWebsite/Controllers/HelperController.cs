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

namespace ICanSpeakWebsite.Controllers
{
    public class HelperController : Controller
    {
        DataSet jsonDataSet = new DataSet();
        HelperClass objhelperclass = new HelperClass();
        string jsonstring = string.Empty;
        string resultjson = string.Empty;
        //
        // GET: /Helper/

        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Help()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            try
            {

                jsonstring = "{}";
                resultjson = objhelperclass.callservice(jsonstring, "GetHelpContent");

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {

                    HelperModel objhelp = new HelperModel();
                    objhelp.Signup = jsonDataSet.Tables["Help"].Rows[0]["Signup"].ToString();
                    objhelp.Practice = jsonDataSet.Tables["Help"].Rows[0]["Practice"].ToString();
                    objhelp.Friends = jsonDataSet.Tables["Help"].Rows[0]["Friends"].ToString();
                    objhelp.Payment = jsonDataSet.Tables["Help"].Rows[0]["Payment"].ToString();
                    objhelp.Tutors = jsonDataSet.Tables["Help"].Rows[0]["Tutors"].ToString();
                    return View(objhelp);
                }
                else
                {
                    ViewBag.Error = "An error occured , Please try again.";
                }

            }
            catch (Exception ex)
            {

            }

            return View();
        }

    }
}
