using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using ICanSpeakWebsite.App_Start;
using ICanSpeakWebsite.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ICanSpeakWebsite.Controllers
{
    public class DialogController : Controller
    {
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;
        DataSet jsonDataSet = new DataSet();
        HelperClass objhelperclass = new HelperClass();
        string jsonstring = string.Empty;
        string resultjson = string.Empty;

        public ActionResult DialogDetail(string id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            try
            {
                jsonstring = "{\"DialogId\":\"" + id + "\",\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetDialogDetails");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    ViewBag.Username = System.Web.HttpRuntime.Cache["Username"].ToString();
                    ViewBag.Country = System.Web.HttpRuntime.Cache["Country"].ToString();
                    ViewBag.ProfilePicture = System.Web.HttpRuntime.Cache["ProfilePicture"].ToString();
                    ViewBag.Msg = jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString();
                    ViewBag.DialogId = jsonDataSet.Tables["Dialog"].Rows[0]["DialogId"].ToString();
                    ViewBag.VideoUrl = jsonDataSet.Tables["Dialog"].Rows[0]["VideoUrl"].ToString();
                    ViewBag.EnglishSubtitleUrl = jsonDataSet.Tables["Dialog"].Rows[0]["EnglishSubtitleUrl"].ToString();
                    ViewBag.ArabicSubtitleUrl = jsonDataSet.Tables["Dialog"].Rows[0]["ArabicSubtitleUrl"].ToString();
                    ViewBag.BothSubtitleUrl = jsonDataSet.Tables["Dialog"].Rows[0]["BothSubtitleUrl"].ToString();
                    ViewBag.DialogName = jsonDataSet.Tables["Dialog"].Rows[0]["DialogName"].ToString();
                    ViewBag.Text = jsonDataSet.Tables["KeyPhrase"].DefaultView;
                    ViewBag.Conversation = jsonDataSet.Tables["Conversation"].DefaultView;
                    ViewBag.BookMarkStatus = jsonDataSet.Tables["Dialog"].Rows[0]["BookMarkStatus"].ToString();
                   // ViewBag.isLastData = "NxtBack";
                   // ViewBag.Score = "10";
                    if (jsonDataSet.Tables["Dialog"].Rows[0]["isLastData"].ToString() == "0")
                    {
                        ViewBag.isLastData = "NxtBack";
                    }
                    else
                    {
                        ViewBag.isLastData = "BackAssmt";
                    }
                    if (jsonDataSet.Tables["Dialog"].Rows[0]["isScore"].ToString() == "0")
                    {
                        ViewBag.isScore = "0";
                    }
                    else
                    {
                        ViewBag.isScore = "1";
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
        
        public ActionResult NextDialogDetail(string id)
        {
            //id = "nBg+oK7HWWeZVo4G1oAzng==";
            try
            {
                jsonstring = "{\"DialogId\":\"" + id + "\",\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "NextDialogDetails");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                //------------Dialog detail------------//
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    ViewBag.DialogId = jsonDataSet.Tables["Dialog"].Rows[0]["DialogId"].ToString();
                    ViewBag.VideoUrl = jsonDataSet.Tables["Dialog"].Rows[0]["VideoUrl"].ToString();
                    ViewBag.EnglishSubtitleUrl = jsonDataSet.Tables["Dialog"].Rows[0]["EnglishSubtitleUrl"].ToString();
                    ViewBag.ArabicSubtitleUrl = jsonDataSet.Tables["Dialog"].Rows[0]["ArabicSubtitleUrl"].ToString();
                    ViewBag.BothSubtitleUrl = jsonDataSet.Tables["Dialog"].Rows[0]["BothSubtitleUrl"].ToString();
                    ViewBag.DialogName = jsonDataSet.Tables["Dialog"].Rows[0]["DialogName"].ToString();
                    ViewBag.Msg = jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString();
                    ViewBag.BookMarkStatus = jsonDataSet.Tables["Dialog"].Rows[0]["BookMarkStatus"].ToString();
                    if (jsonDataSet.Tables["Dialog"].Rows[0]["isLastData"].ToString() == "0")
                    {
                        ViewBag.isLastData = "NxtBack";
                    }
                    else
                    {
                        ViewBag.isLastData = "BackAssmt";
                    }
                    if (jsonDataSet.Tables["Dialog"].Rows[0]["isScore"].ToString() == "0")
                    {
                        ViewBag.isScore = "0";
                    }
                    else
                    {
                        ViewBag.isScore = "1";
                    }
                    //------------Conversation------------//
                    if (jsonDataSet.Tables["ConversationDataStatus"].Rows[0]["resultmsg"].ToString() == "success")
                    {
                        ViewBag.Conversation = jsonDataSet.Tables["Conversation"].DefaultView;
                    }
                    else
                    {
                        ViewBag.Conversation = jsonDataSet.Tables["ConversationDataStatus"].Rows[0]["resultmsg"].ToString();
                    }
                    //------------KeyPhrase------------//
                    if (jsonDataSet.Tables["KeyPhraseDataStatus"].Rows[0]["resultmsg"].ToString() == "success")
                    {
                        ViewBag.Text = jsonDataSet.Tables["KeyPhrase"].DefaultView;
                    }
                    else
                    {
                        ViewBag.Text = jsonDataSet.Tables["KeyPhraseDataStatus"].Rows[0]["resultmsg"].ToString();
                    }
                    return View("_DialogDetail");
                }
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "No Data")
                {
                    ViewBag.isLastData = "BackAssmt";
                    ViewBag.Conversation = "No Data";
                    ViewBag.Text = "No Data";
                    ViewBag.DialogId = id;
                    ViewBag.DialogName = "No Next Record";
                    return View("_DialogDetail");
                }
                else
                {
                    //ViewBag.Msg = jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString();
                    //ViewBag.DialogName = "No Data Available";
                    return View();
                }

            }
            catch (Exception ex)
            {
                return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BackDialogDetail(string id)
        {
            try
            {
                jsonstring = "{\"DialogId\":\"" + id + "\",\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "BackDialogDetails");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    ViewBag.Msg = jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString();
                    ViewBag.DialogId = jsonDataSet.Tables["Dialog"].Rows[0]["DialogId"].ToString();
                    ViewBag.VideoUrl = jsonDataSet.Tables["Dialog"].Rows[0]["VideoUrl"].ToString();
                    ViewBag.EnglishSubtitleUrl = jsonDataSet.Tables["Dialog"].Rows[0]["EnglishSubtitleUrl"].ToString();
                    ViewBag.ArabicSubtitleUrl = jsonDataSet.Tables["Dialog"].Rows[0]["ArabicSubtitleUrl"].ToString();
                    ViewBag.BothSubtitleUrl = jsonDataSet.Tables["Dialog"].Rows[0]["BothSubtitleUrl"].ToString();
                    ViewBag.DialogName = jsonDataSet.Tables["Dialog"].Rows[0]["DialogName"].ToString();
                    ViewBag.Text = jsonDataSet.Tables["KeyPhrase"].DefaultView;
                    ViewBag.Conversation = jsonDataSet.Tables["Conversation"].DefaultView;
                    ViewBag.BookMarkStatus = jsonDataSet.Tables["Dialog"].Rows[0]["BookMarkStatus"].ToString();
                    if (jsonDataSet.Tables["Dialog"].Rows[0]["isLastData"].ToString() == "0")
                    {
                        ViewBag.isLastData = "NxtBack";
                    }
                    else
                    {
                        ViewBag.isLastData = "Nxt";
                    }
                    if (jsonDataSet.Tables["Dialog"].Rows[0]["isScore"].ToString() == "0")
                    {
                        ViewBag.isScore = "0";
                    }
                    else
                    {
                        ViewBag.isScore = "1";
                    }
                    return View("_DialogDetail");
                }
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "No Data")
                {
                    ViewBag.isLastData = "Nxt";
                    ViewBag.Conversation = "No Data";
                    ViewBag.Text = "No Data";
                    ViewBag.DialogId = id;
                    ViewBag.DialogName = "No Previous Record";
                    return View("_DialogDetail");
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
        public ActionResult ViewConversation(string id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }

            try
            {
                jsonstring = "{\"DialogId\":\"" + id + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetDialogConversation");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    ViewBag.DialogId = jsonDataSet.Tables["Conversation"].Rows[0]["DialogId"].ToString();
                    ViewBag.VideoUrl = jsonDataSet.Tables["Video"].Rows[0]["VideoUrl"].ToString();
                    ViewBag.EnglishSubtitleUrl = jsonDataSet.Tables["Dialog"].Rows[0]["EnglishSubtitleUrl"].ToString();
                    ViewBag.ArabicSubtitleUrl = jsonDataSet.Tables["Dialog"].Rows[0]["ArabicSubtitleUrl"].ToString();
                    ViewBag.BothSubtitleUrl = jsonDataSet.Tables["Dialog"].Rows[0]["BothSubtitleUrl"].ToString();
                    ViewBag.Conversation = jsonDataSet.Tables["Conversation"].DefaultView;
                    ViewBag.DialogName = jsonDataSet.Tables["Video"].Rows[0]["DialogName"].ToString();
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

        public ActionResult DialogList()
        {
            //if (Session["UserId"] == null)
            //{
            //    return RedirectToAction("Login", "Login");
            //}
            try
            {
                jsonstring = "{\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetDialogByUserId");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    ViewBag.Username = System.Web.HttpRuntime.Cache["Username"].ToString();
                    ViewBag.Country = System.Web.HttpRuntime.Cache["Country"].ToString();
                    ViewBag.ProfilePicture = System.Web.HttpRuntime.Cache["ProfilePicture"].ToString();
                    ViewBag.Dialog = jsonDataSet.Tables["Dialog"].DefaultView;
                    ViewBag.DialogBookmark = System.Web.HttpRuntime.Cache["DialogBookmark"].ToString();
                    
                    if (jsonDataSet.Tables["ResultMessage"].Rows[0]["resultmsg"].ToString() == "success")
                    {
                        ViewBag.AssessmentScore = jsonDataSet.Tables["DialogAssessmentScore"].DefaultView;
                    }

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


        //-----------------------------------------------------Start Book-Mark Code ---------------------------------//

        public JsonResult AddBookMarkDialogWords(string bookMarkUrl, string dialogCourseId, string courseName)
        {
            try
            {
                jsonstring = "{\"userid\":\"" + Session["UserId"] + "\",\"bookmarkUrl\":\"" + bookMarkUrl + "\",\"courseid\":\"" + dialogCourseId + "\",\"courseType\":\"Dialog\",\"courseName\":\"" + courseName + "\",\"vocabId\":\"\",\"vocabSubId\":\"\"}";
                resultjson = objhelperclass.callservice(jsonstring, "AddBookMark");

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

        public JsonResult RemoveBookMarkDialogWords(string courseid)
        {
            try
            {
                jsonstring = "{\"userid\":\"" + Session["UserId"] + "\", \"courseid\":\"" + courseid + "\",\"courseType\":\"Dialog\"}";
                resultjson = objhelperclass.callservice(jsonstring, "RemoveFlashCardWord");

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
        //-----------------------------------------------------End Book-Mark Code ---------------------------------//
    }
}
