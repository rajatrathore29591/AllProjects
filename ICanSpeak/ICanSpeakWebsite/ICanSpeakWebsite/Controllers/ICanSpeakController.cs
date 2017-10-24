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
    public class ICanSpeakController : Controller
    {
        DataSet jsonDataSet = new DataSet();
        HelperClass objhelperclass = new HelperClass();
        string jsonstring = string.Empty;
        string resultjson = string.Empty;

        public ActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ContactUs(ContactUsModel objContact)
        {
            try
            {
                jsonstring = "{\"FullName\":\"" + objContact.FullName + "\",\"EmailId\":\"" + objContact.EmailId + "\",\"ContactNo\":\"" + objContact.ContactNo + "\",\"Message\":\"" + objContact.Message + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "AddQuery1");
                string pp = resultjson[0].ToString();
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                }
                else
                {
                    ViewBag.Error = "An error occured , Please try again.";
                }
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


        public ActionResult Idea()
        {
            return View();
        }


        public ActionResult Contents()
        {
            return View();
        }

        public ActionResult Benifits()
        {
            return View();
        }

        public ActionResult Help()
        {
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
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }

            return View();
        }

        public ActionResult AllTutor()
        {
            try
            {
                jsonstring = "{}";
                resultjson = objhelperclass.callservice(jsonstring, "GetAllTutors");
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

        public ActionResult Faq()
        {
            try
            {
                jsonstring = "{}";
                resultjson = objhelperclass.callservice(jsonstring, "GetAllFAQ1s");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    var finalarray = Regex.Split(resultjson, "FAQList\":");
                    string finalresultjson = finalarray[1] + "\\|";
                    finalresultjson = finalresultjson.Replace("}\\|", "");
                    JArray jsonArray = JArray.Parse(finalresultjson);
                    List<FAQModel> FAQDetails = jsonArray.ToObject<List<FAQModel>>();
                    return View(FAQDetails);

                    //ViewBag.Question = jsonDataSet.Tables["Faq"].Rows[0]["Title"].ToString();
                    //ViewBag.Description = jsonDataSet.Tables["Faq"].Rows[0]["Description"].ToString();                    
                    //return View();
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

        public ActionResult Story()
        {
            //try
            //{
            //    jsonstring = "{}";
            //    resultjson = objhelperclass.callservice(jsonstring, "GetAllStory");
            //    XmlDocument xd1 = new XmlDocument();
            //    xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
            //    jsonDataSet.ReadXml(new XmlNodeReader(xd1));

            //    if(jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
            //    {
            //        var finalarray = Regex.Split(resultjson, "StoryList\":");
            //        string finalresultjson = finalarray[1] + "\\|";
            //        finalresultjson = finalresultjson.Replace("}\\|", "");
            //        JArray jsonArray = JArray.Parse(finalresultjson);
            //        List<SuccessStoryModel> StoryDetails = jsonArray.ToObject<List<SuccessStoryModel>>();
            //        return View(StoryDetails);
            //    }
            //    else
            //    {
            //        return View("Error");
            //    }

            //}
            //catch (Exception ex)
            //{
            //    ViewBag.ErrorName = ex.Message;
            //    return View("Error");
            //}
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult UserAgreement()
        {
            return View();
        }

        public ActionResult WatchVideo()
        {
            try
            {
                jsonstring = "{}";
                resultjson = objhelperclass.callservice(jsonstring, "GetVideoDetail");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    var finalarray = Regex.Split(resultjson, "DemoVideo\":");
                    string finalresultjson = finalarray[1] + "\\|";
                    finalresultjson = finalresultjson.Replace("}\\|", "");
                    JArray jsonArray = JArray.Parse(finalresultjson);
                    List<VideoModel> DemoVideos = jsonArray.ToObject<List<VideoModel>>();
                    return View(DemoVideos);
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
