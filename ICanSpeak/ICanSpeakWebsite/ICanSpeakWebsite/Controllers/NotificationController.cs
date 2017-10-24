using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using ICanSpeakWebsite.App_Start;
using Newtonsoft.Json;

namespace ICanSpeakWebsite.Controllers
{
    public class NotificationController : Controller
    {
        DataSet jsonDataSet = new DataSet();
        HelperClass objhelperclass = new HelperClass();
        string jsonstring = string.Empty;
        string resultjson = string.Empty;

        public ActionResult Notifications()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            try
            {
                jsonstring = "{\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetAllNotification");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    ViewBag.Username = System.Web.HttpRuntime.Cache["Username"].ToString();
                    ViewBag.Country = System.Web.HttpRuntime.Cache["Country"].ToString();
                    ViewBag.Notification = jsonDataSet.Tables["Notification"].DefaultView;
                    ViewBag.ProfilePicture = System.Web.HttpRuntime.Cache["ProfilePicture"].ToString();
                    if (jsonDataSet.Tables["MyscoreDataStatus"].Rows[0]["resultmsg"].ToString() == "success")
                    {
                        ViewBag.MyScore = jsonDataSet.Tables["Myscores"].Rows[0]["MyScore"].ToString();
                        ViewBag.TotalScore = jsonDataSet.Tables["Myscores"].Rows[0]["TotalScore"].ToString();
                        ViewBag.FlashCard = jsonDataSet.Tables["Myscores"].Rows[0]["FlashCard"].ToString();
                    }
                    return View();
                }
                else
                {
                    //ViewBag.VocabWords = "No Data";
                    return View();
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("LoginError", "Login");
            }
            
        }

    }
}
