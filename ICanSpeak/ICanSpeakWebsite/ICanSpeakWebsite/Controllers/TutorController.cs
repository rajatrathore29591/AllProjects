using ICanSpeakWebsite.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ICanSpeakWebsite.App_Start;
using System.Xml;
using System.Text.RegularExpressions;

namespace ICanSpeakWebsite.Controllers
{
    public class TutorController : Controller
    {
        //
        // GET: /Tutor/
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;
        DataSet jsonDataSet = new DataSet();
        HelperClass objhelperclass = new HelperClass();
        string jsonstring = string.Empty;
        string resultjson = string.Empty;


        public ActionResult MyTutor()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            try
            {
                jsonstring = "{\"userid\":\"" + Session["userid"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetTutorsByUserId");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    var finalarray = Regex.Split(resultjson, "Tutors\":");
                    string finalresultjson = finalarray[1] + "\\|";
                    finalresultjson = finalresultjson.Replace("}\\|", "");
                    JArray jsonArray = JArray.Parse(finalresultjson);
                    List<TutorDetails> TutorDetails = jsonArray.ToObject<List<TutorDetails>>();
                    return View(TutorDetails);
                }
                else
                {

                    return View();
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
           
        }

        public ActionResult Tutors()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            try
            {
                jsonstring = "";
                resultjson = objhelperclass.callservice(jsonstring, "GetAllTutors");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    var finalarray = Regex.Split(resultjson, "Tutors\":");
                    string finalresultjson = finalarray[1]+"\\|";
                    finalresultjson = finalresultjson.Replace("}\\|", "");
                    JArray jsonArray = JArray.Parse(finalresultjson);
                    List<Users> TutorsList = jsonArray.ToObject<List<Users>>();
                    return View(TutorsList);
                }
                else
                {
                   
                    return View();
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("LoginError", "Login");
            }
            //return View();
        }

        public ActionResult TutorDetails(string userId)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            try
            {
                jsonstring = "{\"userid\":\"" + userId + "\"}";
                resultjson = objhelperclass.callservice(jsonstring,"GetTutorsByUserId");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    var finalarray = Regex.Split(resultjson, "Tutors\":");
                    string finalresultjson = finalarray[1] + "\\|";
                    finalresultjson = finalresultjson.Replace("}\\|", "");
                    JArray jsonArray = JArray.Parse(finalresultjson);
                    List<TutorDetails> TutorDetails = jsonArray.ToObject<List<TutorDetails>>();
                    return View(TutorDetails);
                }
                else
                {

                    return View();
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
