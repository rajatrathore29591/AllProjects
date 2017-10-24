using ICanSpeakWebsite.App_Start;
using ICanSpeakWebsite.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace ICanSpeakWebsite.Controllers
{
    public class CourseController : Controller
    {
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;
        DataSet jsonDataSet = new DataSet();
        HelperClass objhelperclass = new HelperClass();
        string jsonstring = string.Empty;
        string resultjson = string.Empty;
        // GET: /Course/

        public ActionResult Test()
        {
            return View();
        }


        public ActionResult MyCourse()
        {
            return View();
        }

        public ActionResult SubscriptionPlan()
        {
            ViewBag.Username = System.Web.HttpRuntime.Cache["Username"].ToString();
            ViewBag.Country = System.Web.HttpRuntime.Cache["Country"].ToString();
            ViewBag.ProfilePicture = System.Web.HttpRuntime.Cache["ProfilePicture"].ToString();

            try
            {
                jsonstring = "";
                resultjson = objhelperclass.callservice(jsonstring, "GetPremiumSubscriptionDetail");

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    SubscriptionModel objModel = new SubscriptionModel();


                    objModel.subscriptionId = jsonDataSet.Tables["Subscription"].Rows[0]["PremiumSubscriptionId"].ToString();
                    objModel.subscriptionName = jsonDataSet.Tables["Subscription"].Rows[0]["SubscriptionName"].ToString();
                    objModel.price = jsonDataSet.Tables["Subscription"].Rows[0]["Price"].ToString();
                    objModel.grammerCount = jsonDataSet.Tables["Subscription"].Rows[0]["GrammerCount"].ToString();
                    objModel.dialogCount = jsonDataSet.Tables["Subscription"].Rows[0]["DialogCount"].ToString();
                    objModel.vocabCount = jsonDataSet.Tables["Subscription"].Rows[0]["VocabCount"].ToString();
                    objModel.vocabwordCount = jsonDataSet.Tables["Subscription"].Rows[0]["VocabWordCount"].ToString();

                    objModel.subscriptionId1 = jsonDataSet.Tables["Subscription"].Rows[1]["PremiumSubscriptionId"].ToString();
                    objModel.subscriptionName1 = jsonDataSet.Tables["Subscription"].Rows[1]["SubscriptionName"].ToString();
                    objModel.price1 = jsonDataSet.Tables["Subscription"].Rows[1]["Price"].ToString();
                    objModel.grammerCount1 = jsonDataSet.Tables["Subscription"].Rows[1]["GrammerCount"].ToString();
                    objModel.dialogCount1 = jsonDataSet.Tables["Subscription"].Rows[1]["DialogCount"].ToString();
                    objModel.vocabCount1 = jsonDataSet.Tables["Subscription"].Rows[1]["VocabCount"].ToString();
                    objModel.vocabwordCount1 = jsonDataSet.Tables["Subscription"].Rows[1]["VocabWordCount"].ToString();


                    objModel.subscriptionId2 = jsonDataSet.Tables["Subscription"].Rows[2]["PremiumSubscriptionId"].ToString();
                    objModel.subscriptionName2 = jsonDataSet.Tables["Subscription"].Rows[2]["SubscriptionName"].ToString();
                    objModel.price2 = jsonDataSet.Tables["Subscription"].Rows[2]["Price"].ToString();
                    objModel.grammerCount2 = jsonDataSet.Tables["Subscription"].Rows[2]["GrammerCount"].ToString();
                    objModel.dialogCount2 = jsonDataSet.Tables["Subscription"].Rows[2]["DialogCount"].ToString();
                    objModel.vocabCount2 = jsonDataSet.Tables["Subscription"].Rows[2]["VocabCount"].ToString();
                    objModel.vocabwordCount2 = jsonDataSet.Tables["Subscription"].Rows[2]["VocabWordCount"].ToString();

                    return View(objModel);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult NormalPlan()
        {
            ViewBag.Username = System.Web.HttpRuntime.Cache["Username"].ToString();
            ViewBag.Country = System.Web.HttpRuntime.Cache["Country"].ToString();

            ViewBag.ProfilePicture = System.Web.HttpRuntime.Cache["ProfilePicture"].ToString();

            try
            {
                jsonstring = "";
                resultjson = objhelperclass.callservice(jsonstring, "GetNormalPlanSubscriptionDetail");

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    
                        ViewBag.ResultVocab = jsonDataSet.Tables["Result1"].DefaultView;
                        ViewBag.ResultDialog = jsonDataSet.Tables["Result2"].DefaultView;
                        ViewBag.ResultGrammer = jsonDataSet.Tables["Result3"].DefaultView;
                        return View();
                     

                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
