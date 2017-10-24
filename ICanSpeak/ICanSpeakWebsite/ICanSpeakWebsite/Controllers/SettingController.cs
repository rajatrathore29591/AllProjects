using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using ICanSpeakWebsite.App_Start;
using ICanSpeakWebsite.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ICanSpeakWebsite.Controllers
{
    public class SettingController : Controller
    {
        DataSet jsonDataSet = new DataSet();
        HelperClass objhelperclass = new HelperClass();
        string jsonstring = string.Empty;
        string resultjson = string.Empty;
        public ActionResult Setting()
        {
            return View();
        }

        
        public ActionResult ChangePassword()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword objModel)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            try
            {
                jsonstring = "{\"userid\":\"" + Session["userid"] + "\",\"Password\":\"" + objModel.oldpassword + "\",\"NewPassword\":\"" + objModel.newpassword + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "ChangePassword");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    ViewBag.Message = "success";
                    return View();
                }
                else
                {
                    if (jsonDataSet.Tables["ResultMessage"].Rows[0]["resultmsg"].ToString() == "invalid")
                    {
                        ViewBag.Message = "invalid";
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "error";
                        return View();
                    }
                    
                    
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }
    }
}
