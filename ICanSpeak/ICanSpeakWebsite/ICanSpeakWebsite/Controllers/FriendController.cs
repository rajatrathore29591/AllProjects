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
using Newtonsoft.Json.Linq;

namespace ICanSpeakWebsite.Controllers
{
    public class FriendController : Controller
    {
        DataSet jsonDataSet = new DataSet();
        HelperClass objhelperclass = new HelperClass();
        string jsonstring = string.Empty;
        string resultjson = string.Empty;

        public ActionResult FriendUnfriendListByUserId()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            try
            {
                jsonstring = "{\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetUnfriendsByUserId");
                string pp = resultjson[0].ToString();

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");              
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    ViewBag.UnFrndUserProfile = jsonDataSet.Tables["UnFrndUserProfile"].DefaultView;
                  
                    ViewBag.Username = System.Web.HttpRuntime.Cache["Username"].ToString();
                    ViewBag.Country = System.Web.HttpRuntime.Cache["Country"].ToString();
                    ViewBag.ProfilePicture = System.Web.HttpRuntime.Cache["ProfilePicture"].ToString();
                    if (jsonDataSet.Tables["MyscoreDataStatus"].Rows[0]["resultmsg"].ToString() == "success")
                    {
                        ViewBag.MyScore = jsonDataSet.Tables["Myscores"].Rows[0]["MyScore"].ToString();
                        ViewBag.TotalScore = jsonDataSet.Tables["Myscores"].Rows[0]["TotalScore"].ToString();
                        ViewBag.FlashCard = jsonDataSet.Tables["Myscores"].Rows[0]["FlashCard"].ToString();
                    }
                    if (jsonDataSet.Tables["FriendListDataStatus"].Rows[0]["resultmsg"].ToString() == "No Data")
                    {
                        ViewBag.FriendStatus = "No Data";
                    }
                    else
                    {
                        ViewBag.FrndUserProfile = jsonDataSet.Tables["FrndUserProfile"].DefaultView;
                    }
                    return View();
                }
                else
                {
                    LoginError objloginerror = new LoginError();
                    objloginerror.Errormessage = jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString();
                    return RedirectToAction("LoginError", "Login", objloginerror);
                }
            }
                catch (Exception ex)
            {
                return RedirectToAction("LoginError", "Login");
            }
        }

        public ActionResult SendRequest(string inviteid)
        {

            try
            {
                jsonstring = "{\"invitedid\":\"" + inviteid + "\",\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "SendFreindRequest");

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CancelSendRequest(string inviteid)
        {
            try
            {
                jsonstring = "{\"invitedid\":\"" + inviteid + "\",\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "CancelSendfriendsRequestByUserId");
                string pp = resultjson[0].ToString();

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AcceptGetRequest(string inviteid)
        {
            try
            {
                jsonstring = "{\"invitedid\":\"" + inviteid + "\",\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "AcceptGetfriendsRequestByUserId");
                string pp = resultjson[0].ToString();

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CancelGetRequest(string inviteid)
        {
            try
            {
                jsonstring = "{\"invitedid\":\"" + inviteid + "\",\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "CancelGetfriendsRequestByUserId");
                string pp = resultjson[0].ToString();

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UnFriendGetRequest(string inviteid)
        {
            try
            {
                jsonstring = "{\"invitedid\":\"" + inviteid + "\",\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "UnFriendRequestByUserId");
                string pp = resultjson[0].ToString();

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
