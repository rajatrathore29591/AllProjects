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
    public class AssesmentController : Controller
    {
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;
        DataSet jsonDataSet = new DataSet();
        HelperClass objhelperclass = new HelperClass();
        string jsonstring = string.Empty;
        string resultjson = string.Empty;

        public ActionResult GetDialogAssesmentlist(string dialogId)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            try
            {
                
                //var id = "rv9glQ0xZx2n7tkHmC+ycg==";
                //rv9glQ0xZx2n7tkHmC+ycg==
                //bmY83UzQeQIsxDShURdGyw==
                jsonstring = "{\"dialogId\":\"" + dialogId + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetAssementByDialogId");
                string pp = resultjson[0].ToString();

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    //DialogAssesment objModel = new DialogAssesment();
                    ViewBag.QuestionID = jsonDataSet.Tables["DialogQuestion"].Rows[0]["QuestionId"].ToString();
                    ViewBag.DialogId = jsonDataSet.Tables["DialogQuestion"].Rows[0]["DialogId"].ToString();
                    ViewBag.Question = jsonDataSet.Tables["DialogQuestion"].Rows[0]["Question"].ToString();
                    ViewBag.QuestionType = jsonDataSet.Tables["DialogQuestion"].Rows[0]["QuestionType"].ToString();

                    ViewBag.OptionText1 = jsonDataSet.Tables["DialogQuestion"].Rows[0]["OptionText1"].ToString();
                    ViewBag.OptionText2 = jsonDataSet.Tables["DialogQuestion"].Rows[0]["OptionText2"].ToString();
                    ViewBag.OptionText3 = jsonDataSet.Tables["DialogQuestion"].Rows[0]["OptionText3"].ToString();

                    ViewBag.OptionAudio1 = jsonDataSet.Tables["DialogQuestion"].Rows[0]["OptionAudio1"].ToString();
                    ViewBag.OptionAudio2 = jsonDataSet.Tables["DialogQuestion"].Rows[0]["OptionAudio2"].ToString();
                    ViewBag.OptionAudio3 = jsonDataSet.Tables["DialogQuestion"].Rows[0]["OptionAudio3"].ToString();

                    ViewBag.FillAnswerText = jsonDataSet.Tables["DialogQuestion"].Rows[0]["FillAnswerText"].ToString();
                    ViewBag.TrueFalseAnswer = jsonDataSet.Tables["DialogQuestion"].Rows[0]["TrueFalseAnswer"].ToString();
                    ViewBag.OptionCorrectAnswer = jsonDataSet.Tables["DialogQuestion"].Rows[0]["OptionCorrectAnswer"].ToString();
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

        public ActionResult NextDialogAssesmentlist(string dialogId, string questionId, string status)
        {
            try
            {
                jsonstring = "{\"userId\":\"" + Session["UserId"] + "\",\"questionId\":\"" + questionId + "\",\"dialogId\":\"" + dialogId + "\",\"courseId\":\"" + dialogId + "\",\"coursetype\":\"dialog\",\"status\":\"" + status + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetNextAssementByDialogId");

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    //NextDialogAssesment objModel = new NextDialogAssesment();
                    ViewBag.QuestionID = jsonDataSet.Tables["DialogQuestion"].Rows[0]["QuestionId"].ToString();
                    ViewBag.DialogId = jsonDataSet.Tables["DialogQuestion"].Rows[0]["DialogId"].ToString();
                    ViewBag.Question = jsonDataSet.Tables["DialogQuestion"].Rows[0]["Question"].ToString();
                    ViewBag.QuestionType = jsonDataSet.Tables["DialogQuestion"].Rows[0]["QuestionType"].ToString();

                    ViewBag.OptionText1 = jsonDataSet.Tables["DialogQuestion"].Rows[0]["OptionText1"].ToString();
                    ViewBag.OptionText2 = jsonDataSet.Tables["DialogQuestion"].Rows[0]["OptionText2"].ToString();
                    ViewBag.OptionText3 = jsonDataSet.Tables["DialogQuestion"].Rows[0]["OptionText3"].ToString();

                    ViewBag.OptionAudio1 = jsonDataSet.Tables["DialogQuestion"].Rows[0]["OptionAudio1"].ToString();
                    ViewBag.OptionAudio2 = jsonDataSet.Tables["DialogQuestion"].Rows[0]["OptionAudio2"].ToString();
                    ViewBag.OptionAudio3 = jsonDataSet.Tables["DialogQuestion"].Rows[0]["OptionAudio3"].ToString();

                    ViewBag.FillAnswerText = jsonDataSet.Tables["DialogQuestion"].Rows[0]["FillAnswerText"].ToString();
                    ViewBag.TrueFalseAnswer = jsonDataSet.Tables["DialogQuestion"].Rows[0]["TrueFalseAnswer"].ToString();
                    ViewBag.OptionCorrectAnswer = jsonDataSet.Tables["DialogQuestion"].Rows[0]["OptionCorrectAnswer"].ToString();
                    return View("_GetDialogAssesmentlist");
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

        public ActionResult GetVocabAssesmentlist(string vocabId)
        {

            try
            {
                //var id = "zKiRYk9XL5YeFQK+uAot2w==";
                jsonstring = "{\"vocabId\":\"" + vocabId + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetAssementByVocabId");
                string pp = resultjson[0].ToString();

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    AssesmentModel objasses = new AssesmentModel();
                    objasses.QuestionID = jsonDataSet.Tables["VocabQuestion"].Rows[0]["QuestionId"].ToString();
                    objasses.VocabID = jsonDataSet.Tables["VocabQuestion"].Rows[0]["VocabularyId"].ToString();
                    objasses.Questions = jsonDataSet.Tables["VocabQuestion"].Rows[0]["Question"].ToString();
                    objasses.optionA = jsonDataSet.Tables["VocabQuestion"].Rows[0]["OptionsA"].ToString();
                    objasses.optionB = jsonDataSet.Tables["VocabQuestion"].Rows[0]["OptionsB"].ToString();
                    objasses.optionC = jsonDataSet.Tables["VocabQuestion"].Rows[0]["OptionsC"].ToString();
                    objasses.optionD = jsonDataSet.Tables["VocabQuestion"].Rows[0]["OptionsD"].ToString();
                    objasses.image = jsonDataSet.Tables["VocabQuestion"].Rows[0]["Picture"].ToString();
                    objasses.audio1 = jsonDataSet.Tables["VocabQuestion"].Rows[0]["OptionAAudio"].ToString();
                    objasses.audio2 = jsonDataSet.Tables["VocabQuestion"].Rows[0]["OptionBAudio"].ToString();
                    objasses.audio3 = jsonDataSet.Tables["VocabQuestion"].Rows[0]["OptionCAudio"].ToString();
                    objasses.audio4 = jsonDataSet.Tables["VocabQuestion"].Rows[0]["OptionDAudio"].ToString();

                    // ViewBag.FrndUserProfile = jsonDataSet.Tables["FrndUserProfile"].DefaultView;
                    return View(objasses);
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
            //return View();
        }


        public ActionResult GetVocabNextAssesment(string questionId, string vocabId, string Status, string Type)
        {
            try
            {
                jsonstring = "{\"vocabId\":\"" + vocabId + "\",\"questionId\":\"" + questionId + "\",\"userId\":\"" + Session["UserId"] + "\",\"courseId\":\"" + vocabId + "\",\"coursetype\":\"" + Type + "\",\"status\":\"" + Status + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetNextAssementByVocabId");
                string pp = resultjson[0].ToString();

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    AssesmentModel objasses = new AssesmentModel();
                    objasses.QuestionID = jsonDataSet.Tables["VocabQuestion"].Rows[0]["QuestionId"].ToString();
                    objasses.VocabID = jsonDataSet.Tables["VocabQuestion"].Rows[0]["VocabularyId"].ToString();
                    objasses.Questions = jsonDataSet.Tables["VocabQuestion"].Rows[0]["Question"].ToString();
                    objasses.optionA = jsonDataSet.Tables["VocabQuestion"].Rows[0]["OptionsA"].ToString();
                    objasses.optionB = jsonDataSet.Tables["VocabQuestion"].Rows[0]["OptionsB"].ToString();
                    objasses.optionC = jsonDataSet.Tables["VocabQuestion"].Rows[0]["OptionsC"].ToString();
                    objasses.optionD = jsonDataSet.Tables["VocabQuestion"].Rows[0]["OptionsD"].ToString();
                    objasses.image = jsonDataSet.Tables["VocabQuestion"].Rows[0]["Picture"].ToString();
                    objasses.audio1 = jsonDataSet.Tables["VocabQuestion"].Rows[0]["OptionAAudio"].ToString();
                    objasses.audio2 = jsonDataSet.Tables["VocabQuestion"].Rows[0]["OptionBAudio"].ToString();
                    objasses.audio3 = jsonDataSet.Tables["VocabQuestion"].Rows[0]["OptionCAudio"].ToString();
                    objasses.audio4 = jsonDataSet.Tables["VocabQuestion"].Rows[0]["OptionDAudio"].ToString();
                    objasses.CorrectAnswer = jsonDataSet.Tables["VocabQuestion"].Rows[0]["CorrectAnswer"].ToString();
                    // ViewBag.VocabQuestions = jsonDataSet.Tables["VocabQuestion"].DefaultView;
                    // ViewBag.FrndUserProfile = jsonDataSet.Tables["FrndUserProfile"].DefaultView;
                    return View("_GetVocabAssesmentlist", objasses);
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
            //return View();
        }
    }
}
