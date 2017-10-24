using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Xml;
using ICanSpeakWebsite.App_Start;
using ICanSpeakWebsite.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ICanSpeakWebsite.Controllers
{
    public class GrammerController : Controller
    {
        HelperClass objhelperclass = new HelperClass();
        string jsonstring = string.Empty;
        string resultjson = string.Empty;
        DataSet jsonDataSet = new DataSet();

        public ActionResult GrammerDetail(string id, string index,string value)
        {
            int val = Convert.ToInt32(value);
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            try
            {
                jsonstring = "{\"unitId\":\"" + id + "\",\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GrammerUnitById");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                 {
                    GrammerModel.GrammarDetail objModel = new GrammerModel.GrammarDetail();
                    objModel.Username = System.Web.HttpRuntime.Cache["Username"].ToString() ;
                    objModel.Country = System.Web.HttpRuntime.Cache["Country"].ToString();
                    objModel.ProfilePicture = System.Web.HttpRuntime.Cache["ProfilePicture"].ToString();
                    objModel.Unitid = jsonDataSet.Tables["Grammer"].Rows[0]["Unitid"].ToString();
                    objModel.UnitNameEnglish = jsonDataSet.Tables["Grammer"].Rows[0]["UnitNameEnglish"].ToString();
                    objModel.UnitNameArabic = jsonDataSet.Tables["Grammer"].Rows[0]["UnitNameArabic"].ToString();
                    objModel.VideoUrl = jsonDataSet.Tables["Grammer"].Rows[0]["VideoUrl"].ToString();
                    objModel.DescriptionEnglish = jsonDataSet.Tables["Grammer"].Rows[0]["DescriptionEnglish"].ToString();
                    objModel.DescriptionArabic = jsonDataSet.Tables["Grammer"].Rows[0]["DescriptionArabic"].ToString();
                    objModel.index = index;
                    objModel.BookMarkStatus = jsonDataSet.Tables["Grammer"].Rows[0]["BookMarkStatus"].ToString();
                    ViewBag.isLastData = "NxtBack";
                    if (val == 1)
                    {
                        //return View("_GrammerDetail", objModel);
                        return View(objModel);
                    }
                    else
                    {
                        return View(objModel);
                    }
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

        public ActionResult NextGrammerDetail(string id)
        {

            try
            {
                jsonstring = "{\"unitId\":\"" + id + "\",\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "NextGrammerUnitById");
                //string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    GrammerModel.GrammarDetail objModel = new GrammerModel.GrammarDetail();
                    objModel.Unitid = jsonDataSet.Tables["Grammer"].Rows[0]["Unitid"].ToString();
                    objModel.UnitNameEnglish = jsonDataSet.Tables["Grammer"].Rows[0]["UnitNameEnglish"].ToString();
                    objModel.UnitNameArabic = jsonDataSet.Tables["Grammer"].Rows[0]["UnitNameArabic"].ToString();
                    objModel.VideoUrl = jsonDataSet.Tables["Grammer"].Rows[0]["VideoUrl"].ToString();
                    objModel.DescriptionEnglish = jsonDataSet.Tables["Grammer"].Rows[0]["DescriptionEnglish"].ToString();
                    objModel.DescriptionArabic = jsonDataSet.Tables["Grammer"].Rows[0]["DescriptionArabic"].ToString();
                    objModel.BookMarkStatus = jsonDataSet.Tables["Grammer"].Rows[0]["BookMarkStatus"].ToString();
                    if (jsonDataSet.Tables["Grammer"].Rows[0]["isLastData"].ToString() == "0")
                    {
                        ViewBag.isLastData = "NxtBack";
                    }
                    else
                    {
                        ViewBag.isLastData = "BackAssmt";
                    }
                    return View("_GrammerDetail",objModel);
                    //string datas = JsonConvert.SerializeObject(jsonDataSet.Tables[1]);
                    //JArray jsonArray = JArray.Parse(datas);
                    //List<GrammerModel.GrammarDetail> ObjList = jsonArray.ToObject<List<GrammerModel.GrammarDetail>>();
                    //return Json(ObjList, JsonRequestBehavior.AllowGet);
                }
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "No Data")
                {
                    GrammerModel.GrammarDetail objModel = new GrammerModel.GrammarDetail();
                    ViewBag.isLastData = "BackAssmt";
                    objModel.DescriptionEnglish = "No Description";
                    objModel.DescriptionArabic = "No Description";
                    objModel.Unitid = id;
                    objModel.UnitNameEnglish = "No Next Record";
                    objModel.UnitNameArabic = "No Next Record";
                    return View("_GrammerDetail", objModel);
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

        public ActionResult BackGrammerDetail(string id)
        {

            try
            {
                jsonstring = "{\"unitId\":\"" + id + "\",\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "BackGrammerUnitById");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    GrammerModel.GrammarDetail objModel = new GrammerModel.GrammarDetail();
                    objModel.Unitid = jsonDataSet.Tables["Grammer"].Rows[0]["Unitid"].ToString();
                    objModel.UnitNameEnglish = jsonDataSet.Tables["Grammer"].Rows[0]["UnitNameEnglish"].ToString();
                    objModel.UnitNameArabic = jsonDataSet.Tables["Grammer"].Rows[0]["UnitNameArabic"].ToString();
                    objModel.VideoUrl = jsonDataSet.Tables["Grammer"].Rows[0]["VideoUrl"].ToString();
                    objModel.DescriptionEnglish = jsonDataSet.Tables["Grammer"].Rows[0]["DescriptionEnglish"].ToString();
                    objModel.DescriptionArabic = jsonDataSet.Tables["Grammer"].Rows[0]["DescriptionArabic"].ToString();
                    objModel.BookMarkStatus = jsonDataSet.Tables["Grammer"].Rows[0]["BookMarkStatus"].ToString();
                    if (jsonDataSet.Tables["Grammer"].Rows[0]["isLastData"].ToString() == "0")
                    {
                        ViewBag.isLastData = "NxtBack";
                    }
                    else
                    {
                        ViewBag.isLastData = "Nxt";
                    }
                    return View("_GrammerDetail", objModel);
                }
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "No Data")
                {
                    GrammerModel.GrammarDetail objModel = new GrammerModel.GrammarDetail();
                    ViewBag.isLastData = "Nxt";
                    objModel.DescriptionEnglish = "No Description";
                    objModel.Unitid = id;
                    objModel.UnitNameEnglish = "No Previous Record";
                    return View("_GrammerDetail", objModel);
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

        //-----------------------------------------------------Start Book-Mark Code ---------------------------------//

        public JsonResult AddBookMarkGrammerWords(string bookMarkUrl, string GrammerCourseId, string courseName)
        {
            try
            {
                jsonstring = "{\"userid\":\"" + Session["UserId"] + "\",\"bookmarkUrl\":\"" + bookMarkUrl + "\",\"courseid\":\"" + GrammerCourseId + "\",\"courseType\":\"Grammer\",\"courseName\":\"" + courseName + "\",\"vocabId\":\"\",\"vocabSubId\":\"\"}";
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
                jsonstring = "{\"userid\":\"" + Session["UserId"] + "\", \"courseid\":\"" + courseid + "\",\"courseType\":\"Grammer\"}";
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

        public ActionResult Test()
        {
            return View();
        }

    }
}
