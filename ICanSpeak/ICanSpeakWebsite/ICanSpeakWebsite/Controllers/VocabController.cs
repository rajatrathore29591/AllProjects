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
    public class VocabController : Controller
    {
        DataSet jsonDataSet = new DataSet();
        HelperClass objhelperclass = new HelperClass();
        string jsonstring = string.Empty;
        string resultjson = string.Empty;

        public ActionResult VocabList()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            try
            {
                jsonstring = "{\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetVocabByUserId");
                
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
                    ViewBag.Vocab = jsonDataSet.Tables["Vocab"].DefaultView;
                    ViewBag.VocabularyBookmark = System.Web.HttpRuntime.Cache["VocabularyBookmark"].ToString();
                    ViewBag.VocabId = System.Web.HttpRuntime.Cache["VocabId"].ToString();
                    ViewBag.VocabSubId = System.Web.HttpRuntime.Cache["VocabSubId"].ToString();

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

        public ActionResult VocabSubCategory(string id)
        {
            try
            {
                jsonstring = "{\"vocabid\":\"" + id + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetVocabSubCategoryByVocabId");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    ViewBag.VocabSubCategory = jsonDataSet.Tables["VocabSubCategory"].DefaultView;
                    ViewBag.CategoryName = jsonDataSet.Tables["VocabSubCategory"].Rows[0]["CategoryName"].ToString();
                    ViewBag.WordCount = jsonDataSet.Tables["VocabSubCategory"].Rows[0]["WordCount"].ToString();
                    return View();
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
        }

        public ActionResult VocabSubCategoryWords(string id)
        {
            try
            {
                jsonstring = "{\"subcategoryId\":\"" + id + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetVocabSubCategoryWordsById");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                        ViewBag.VocabWords = jsonDataSet.Tables["Words"].DefaultView;
                        //ViewBag.CategoryName = jsonDataSet.Tables["Words"].Rows[0]["CategoryName"].ToString();
                        //ViewBag.SubCategoryName = jsonDataSet.Tables["Words"].Rows[0]["SubCategoryName"].ToString();
                        //ViewBag.VocabularyId = jsonDataSet.Tables["Words"].Rows[0]["VocabularyId"].ToString();
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

        public ActionResult VocabCategoryWords(string id)
        {
            try
            {
                jsonstring = "{\"categoryId\":\"" + id + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetVocabCategoryWordsById");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    
                        ViewBag.VocabWords = jsonDataSet.Tables["Words"].DefaultView;
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

        public ActionResult WordDetail(string id, string VocabularyId)
        {
            try
            {
                jsonstring = "{\"wordId\":\"" + id + "\",\"vocabid\":\"" + VocabularyId + "\",\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetWordsDetailByWordId");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    WordDetail objModel = new WordDetail();
                    ViewBag.Username = System.Web.HttpRuntime.Cache["Username"].ToString();
                    ViewBag.Country = System.Web.HttpRuntime.Cache["Country"].ToString();
                    objModel.UserId = Session["UserId"].ToString();
                    objModel.WordId = jsonDataSet.Tables["WordDetail"].Rows[0]["WordId"].ToString();
                    objModel.SubCategory = jsonDataSet.Tables["WordDetail"].Rows[0]["SubCategory"].ToString();
                    objModel.VocabularyId = jsonDataSet.Tables["WordDetail"].Rows[0]["VocabularyId"].ToString();
                    objModel.VocabularySubId = jsonDataSet.Tables["WordDetail"].Rows[0]["VocabularySubId"].ToString();
                    objModel.EnglishText = jsonDataSet.Tables["WordDetail"].Rows[0]["EnglishText"].ToString();
                    objModel.ArabicText = jsonDataSet.Tables["WordDetail"].Rows[0]["ArabicText"].ToString();
                    objModel.AudioUrl = jsonDataSet.Tables["WordDetail"].Rows[0]["AudioUrl"].ToString();
                    objModel.PictureUrl = jsonDataSet.Tables["WordDetail"].Rows[0]["PictureUrl"].ToString();
                    objModel.Vocab = jsonDataSet.Tables["WordDetail"].Rows[0]["Text"].ToString();
                    objModel.FlashCardStatus = jsonDataSet.Tables["WordDetail"].Rows[0]["FlashCardStatus"].ToString();
                    objModel.BookMarkStatus = jsonDataSet.Tables["WordDetail"].Rows[0]["BookMarkStatus"].ToString();
                    ViewBag.isLastData = "NxtBack";
                    return View(objModel);
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


        public ActionResult NextWordDetail(string id, string vocab)
        {
            try
            {
                jsonstring = "{\"wordId\":\"" + id + "\",\"vocabid\":\"" + vocab + "\",\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "NextWordDetailById");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    WordDetail objModel = new WordDetail();
                   
                    objModel.WordId = jsonDataSet.Tables["WordDetail"].Rows[0]["WordId"].ToString();
                    objModel.SubCategory = jsonDataSet.Tables["WordDetail"].Rows[0]["SubCategory"].ToString();
                    objModel.VocabularyId = jsonDataSet.Tables["WordDetail"].Rows[0]["VocabularyId"].ToString();
                    objModel.EnglishText = jsonDataSet.Tables["WordDetail"].Rows[0]["EnglishText"].ToString();
                    objModel.ArabicText = jsonDataSet.Tables["WordDetail"].Rows[0]["ArabicText"].ToString();
                    objModel.AudioUrl = jsonDataSet.Tables["WordDetail"].Rows[0]["AudioUrl"].ToString();
                    objModel.PictureUrl = jsonDataSet.Tables["WordDetail"].Rows[0]["PictureUrl"].ToString();
                    objModel.Vocab = jsonDataSet.Tables["WordDetail"].Rows[0]["Text"].ToString();
                    objModel.FlashCardStatus = jsonDataSet.Tables["WordDetail"].Rows[0]["FlashCardStatus"].ToString();
                    objModel.BookMarkStatus = jsonDataSet.Tables["WordDetail"].Rows[0]["BookMarkStatus"].ToString();

                    if (jsonDataSet.Tables["WordDetail"].Rows[0]["isLastData"].ToString() == "0")
                    {
                        ViewBag.isLastData = "NxtBack";
                    }
                    else
                    {
                        ViewBag.isLastData = "BackAssmt";
                    }

                    return View("_WordDetail", objModel);
                }
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "No Data")
                {
                    WordDetail objModel = new WordDetail();
                    ViewBag.isLastData = "BackAssmt";
                    objModel.Vocab = "No Data";
                    
                    objModel.WordId = id;
                    objModel.VocabularyId = vocab;
                    return View("_WordDetail", objModel);
                }
                else
                {
                    //ViewBag.VocabWords = "No Data";
                    return View();
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BackWordDetail(string id, string vocab)
        {

            try
            {
                jsonstring = "{\"wordId\":\"" + id + "\" ,\"vocabid\":\"" + vocab + "\",\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "BackWordDetailById");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    WordDetail objModel = new WordDetail();

                    objModel.WordId = jsonDataSet.Tables["WordDetail"].Rows[0]["WordId"].ToString();
                    objModel.SubCategory = jsonDataSet.Tables["WordDetail"].Rows[0]["SubCategory"].ToString();
                    objModel.VocabularyId = jsonDataSet.Tables["WordDetail"].Rows[0]["VocabularyId"].ToString();
                    objModel.EnglishText = jsonDataSet.Tables["WordDetail"].Rows[0]["EnglishText"].ToString();
                    objModel.ArabicText = jsonDataSet.Tables["WordDetail"].Rows[0]["ArabicText"].ToString();
                    objModel.AudioUrl = jsonDataSet.Tables["WordDetail"].Rows[0]["AudioUrl"].ToString();
                    objModel.PictureUrl = jsonDataSet.Tables["WordDetail"].Rows[0]["PictureUrl"].ToString();
                    objModel.Vocab = jsonDataSet.Tables["WordDetail"].Rows[0]["Text"].ToString();
                    objModel.FlashCardStatus = jsonDataSet.Tables["WordDetail"].Rows[0]["FlashCardStatus"].ToString();
                    objModel.BookMarkStatus = jsonDataSet.Tables["WordDetail"].Rows[0]["BookMarkStatus"].ToString();

                    if (jsonDataSet.Tables["WordDetail"].Rows[0]["isLastData"].ToString() == "0")
                    {
                        ViewBag.isLastData = "NxtBack";
                    }
                    else
                    {
                        ViewBag.isLastData = "Nxt";
                    }
                    return View("_WordDetail", objModel);
                }
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "No Data")
                {
                    WordDetail objModel = new WordDetail();
                    ViewBag.isLastData = "Nxt";
                    objModel.Vocab = "No Data";
                    
                    objModel.WordId = id;
                    objModel.VocabularyId = vocab;
                    return View("_WordDetail", objModel);
                }
                else
                {
                    //ViewBag.VocabWords = "No Data";
                    return View();
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult VocabDetail(string id)
        {
            return View();
        }

        //-----------------------------------------------------Start Book-Mark Code ---------------------------------//

        public JsonResult AddBookMarkVocabWords(string bookMarkUrl, string wordid, string courseName, string VocabularyId, string VocabularySubId)
        {
            try
            {
                jsonstring = "{\"userid\":\"" + Session["UserId"] + "\",\"bookmarkUrl\":\"" + bookMarkUrl + "\",\"courseid\":\"" + wordid + "\",\"courseType\":\"Vocabulary\",\"courseName\":\"" + courseName + "\",\"vocabId\":\"" + VocabularyId + "\",\"vocabSubId\":\"" + VocabularySubId + "\"}";
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

        public JsonResult RemoveBookMarkVocabWords(string courseid)
        {
            try
            {
                jsonstring = "{\"userid\":\"" + Session["UserId"] + "\", \"courseid\":\"" + courseid + "\",\"courseType\":\"Vocabulary\"}";
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

        //-----------------------------------------------------Start Flash Word Code ---------------------------------//

        public JsonResult AddFlashCardWords(string wordid)
        {
            try
            {
                jsonstring = "{\"userid\":\"" + Session["UserId"] + "\",\"wordid\":\"" + wordid + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "AddFlashCardWord");

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

        public JsonResult RemoveFlashCardWords(string wordid)
        {
            try
            {
                jsonstring = "{\"wordid\":\"" + wordid + "\", \"userid\":\"" + Session["UserId"] + "\"}";
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

        public ActionResult FlashCardWordList()
        {
            try
            {
                jsonstring = "{\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "FlashCardByUserId");

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                //if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    ViewBag.FlashWords = jsonDataSet.Tables["FlashCard"].DefaultView;
                    return View();
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

        public ActionResult FlashCardWordDetail(string wordid, string VocabularyId, string FlashCardId)
        {
            try
            {
                jsonstring = "{\"wordId\":\"" + wordid + "\",\"vocabid\":\"" + VocabularyId + "\",\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetWordsDetailByWordId");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    FlashCardWordDetails objModel = new FlashCardWordDetails();
                    ViewBag.Username = System.Web.HttpRuntime.Cache["Username"].ToString();
                    ViewBag.Country = System.Web.HttpRuntime.Cache["Country"].ToString();
                    objModel.UserId = Session["UserId"].ToString();
                    objModel.WordId = jsonDataSet.Tables["WordDetail"].Rows[0]["WordId"].ToString();
                    objModel.FlashCardId = FlashCardId;
                    objModel.SubCategory = jsonDataSet.Tables["WordDetail"].Rows[0]["SubCategory"].ToString();
                    objModel.VocabularyId = jsonDataSet.Tables["WordDetail"].Rows[0]["VocabularyId"].ToString();
                    objModel.EnglishText = jsonDataSet.Tables["WordDetail"].Rows[0]["EnglishText"].ToString();
                    objModel.ArabicText = jsonDataSet.Tables["WordDetail"].Rows[0]["ArabicText"].ToString();
                    objModel.AudioUrl = jsonDataSet.Tables["WordDetail"].Rows[0]["AudioUrl"].ToString();
                    objModel.PictureUrl = jsonDataSet.Tables["WordDetail"].Rows[0]["PictureUrl"].ToString();
                    objModel.Vocab = jsonDataSet.Tables["WordDetail"].Rows[0]["Text"].ToString();
                    objModel.FlashCardStatus = jsonDataSet.Tables["WordDetail"].Rows[0]["FlashCardStatus"].ToString();
                    ViewBag.isLastData = "NxtBack";

                    return View(objModel);
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

        public ActionResult NextFlahWordDetail(string id, string vocab, string flash)
        {

            try
            {
                jsonstring = "{\"wordId\":\"" + id + "\",\"vocabid\":\"" + vocab + "\",\"flashcardid\":\"" + flash + "\",\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "NextFlashCardDetail");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    FlashCardWordDetails objModel = new FlashCardWordDetails();
                    objModel.WordId = jsonDataSet.Tables["WordDetail"].Rows[0]["WordId"].ToString();
                    objModel.FlashCardId = jsonDataSet.Tables["WordDetail"].Rows[0]["FlashCardId"].ToString();
                    objModel.SubCategory = jsonDataSet.Tables["WordDetail"].Rows[0]["SubCategory"].ToString();
                    objModel.VocabularyId = jsonDataSet.Tables["WordDetail"].Rows[0]["VocabularyId"].ToString();
                    objModel.EnglishText = jsonDataSet.Tables["WordDetail"].Rows[0]["EnglishText"].ToString();
                    objModel.ArabicText = jsonDataSet.Tables["WordDetail"].Rows[0]["ArabicText"].ToString();
                    objModel.AudioUrl = jsonDataSet.Tables["WordDetail"].Rows[0]["AudioUrl"].ToString();
                    objModel.PictureUrl = jsonDataSet.Tables["WordDetail"].Rows[0]["PictureUrl"].ToString();
                    objModel.Vocab = jsonDataSet.Tables["WordDetail"].Rows[0]["Text"].ToString();
                    //objModel.FlashCardStatus = jsonDataSet.Tables["WordDetail"].Rows[0]["FlashCardStatus"].ToString();
                    
                    if (jsonDataSet.Tables["WordDetail"].Rows[0]["isLastData"].ToString() == "0")
                    {
                        ViewBag.isLastData = "NxtBack";
                    }
                    else
                    {
                        ViewBag.isLastData = "Back";
                    }

                    return View("_FlashCardWordDetail", objModel);
                }
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "No Data")
                {
                    FlashCardWordDetails objModel = new FlashCardWordDetails();
                    ViewBag.isLastData = "Back";
                    objModel.Vocab = "No Data";
                    objModel.FlashCardId = flash;
                    objModel.WordId = id;
                    objModel.VocabularyId = vocab;
                    return View("_FlashCardWordDetail", objModel);
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

        public ActionResult BackFlashWordDetail(string id, string vocab, string flash)
        {

            try
            {
                jsonstring = "{\"wordId\":\"" + id + "\" ,\"vocabid\":\"" + vocab + "\",\"flashcardid\":\"" + flash + "\",\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "BackFlashCardDetail");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    FlashCardWordDetails objModel = new FlashCardWordDetails();
                    objModel.WordId = jsonDataSet.Tables["WordDetail"].Rows[0]["WordId"].ToString();
                    objModel.FlashCardId = jsonDataSet.Tables["WordDetail"].Rows[0]["FlashCardId"].ToString();
                    objModel.SubCategory = jsonDataSet.Tables["WordDetail"].Rows[0]["SubCategory"].ToString();
                    objModel.VocabularyId = jsonDataSet.Tables["WordDetail"].Rows[0]["VocabularyId"].ToString();
                    objModel.EnglishText = jsonDataSet.Tables["WordDetail"].Rows[0]["EnglishText"].ToString();
                    objModel.ArabicText = jsonDataSet.Tables["WordDetail"].Rows[0]["ArabicText"].ToString();
                    objModel.AudioUrl = jsonDataSet.Tables["WordDetail"].Rows[0]["AudioUrl"].ToString();
                    objModel.PictureUrl = jsonDataSet.Tables["WordDetail"].Rows[0]["PictureUrl"].ToString();
                    objModel.Vocab = jsonDataSet.Tables["WordDetail"].Rows[0]["Text"].ToString();
                    //objModel.FlashCardStatus = jsonDataSet.Tables["WordDetail"].Rows[0]["FlashCardStatus"].ToString();
                    if (jsonDataSet.Tables["WordDetail"].Rows[0]["isLastData"].ToString() == "0")
                    {
                        ViewBag.isLastData = "NxtBack";
                    }
                    else
                    {
                        ViewBag.isLastData = "Nxt";
                    }
                    return View("_FlashCardWordDetail", objModel);
                }
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "No Data")
                {
                    FlashCardWordDetails objModel = new FlashCardWordDetails();
                    ViewBag.isLastData = "Nxt";
                    objModel.Vocab = "No Data";
                    objModel.FlashCardId = flash;
                    objModel.WordId = id;
                    objModel.VocabularyId = vocab;
                    return View("_FlashCardWordDetail", objModel);
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
