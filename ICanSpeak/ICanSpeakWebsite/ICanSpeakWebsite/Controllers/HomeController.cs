using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICanSpeakWebsite.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Data;
using ICanSpeakWebsite.HelperClasses;
using System.Web.Caching;
using ICanSpeakWebsite.App_Start;
using System.Xml;
using System.Text.RegularExpressions;

namespace ICanSpeakWebsite.Controllers
{
    public class HomeController : Controller
    {
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;
        DataSet jsonDataSet = new DataSet();
        HelperClass objhelperclass = new HelperClass();
        string jsonstring = string.Empty;
        string resultjson = string.Empty;

        public ActionResult MyCourse()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            try
            {
                jsonstring = "{\"userid\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetUserCourseById");
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
                    ViewBag.Grammer = jsonDataSet.Tables[1].DefaultView;
                    ViewBag.GrammerBookmark = System.Web.HttpRuntime.Cache["GrammerBookmark"].ToString();
                    
                   
                    if (jsonDataSet.Tables["MyscoreDataStatus"].Rows[0]["resultmsg"].ToString() == "success")
                    {
                        ViewBag.MyScore = jsonDataSet.Tables["Myscores"].Rows[0]["MyScore"].ToString();
                        ViewBag.TotalScore = jsonDataSet.Tables["Myscores"].Rows[0]["TotalScore"].ToString();
                        ViewBag.FlashCard = jsonDataSet.Tables["Myscores"].Rows[0]["FlashCard"].ToString();
                    }
                    return View() ;
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

        public ActionResult Test()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            return View();
        }

        public JsonResult Facebook(string firstname, string lastname,string fbid,string  email, string birthday,string country, string imagebase64, string gender)
        {
            try
            {
                imagebase64 = imagebase64.Replace("data:image/png;base64,", "");
                jsonstring = "{\"usertype\":\"1\",\"FacebookId\":\"" + fbid + "\",\"email\":\"" + email + "\",\"Username\":\"" + firstname + "\",\"Country\":\"" + country + "\",\"DOB\":\"" + birthday + "\",\"firstName\":\"" + firstname + "\",\"lastName\":\"" + lastname + "\",\"DeviceId\":\"\",\"gender\":\"" + gender + "\",\"imagebase64\":\""+imagebase64+"\"}";
                resultjson = objhelperclass.callservice(jsonstring, "SaveUserThirdParty");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    System.Web.HttpRuntime.Cache["Username"] = jsonDataSet.Tables["Profile"].Rows[0]["Username"].ToString();
                    System.Web.HttpRuntime.Cache["Country"] = jsonDataSet.Tables["Profile"].Rows[0]["Country"].ToString();
                    System.Web.HttpRuntime.Cache["ProfilePicture"] = jsonDataSet.Tables["Profile"].Rows[0]["ProfilePicture"].ToString();
                    Session["UserId"] = jsonDataSet.Tables["Profile"].Rows[0]["UserId"].ToString();
                    System.Web.HttpRuntime.Cache["VocabularyBookmark"] = jsonDataSet.Tables["Profile"].Rows[0]["VocabularyBookmark"].ToString();
                    System.Web.HttpRuntime.Cache["DialogBookmark"] = jsonDataSet.Tables["Profile"].Rows[0]["DialogBookmark"].ToString();
                    System.Web.HttpRuntime.Cache["VocabId"] = jsonDataSet.Tables["Profile"].Rows[0]["VocabId"].ToString();
                    System.Web.HttpRuntime.Cache["VocabSubId"] = jsonDataSet.Tables["Profile"].Rows[0]["VocabSubId"].ToString();
                    //ViewBag.Grammer = jsonDataSet.Tables[1].DefaultView;
                    System.Web.HttpRuntime.Cache["GrammerBookmark"] = jsonDataSet.Tables["Profile"].Rows[0]["GrammerBookmark"].ToString();
                    return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
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

        public JsonResult Google(string firstname, string lastname, string fbid, string email, string birthday, string country, string imageurl, string gender)
        {
            try
            {
                imageurl = imageurl.Replace("sz=50", "sz=700");
                StringBuilder _sb = new StringBuilder();
                Byte[] _byte = GetImage(imageurl);
                _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
                string imagebase64 = _sb.ToString(); 
                jsonstring = "{\"usertype\":\"3\",\"GoogleId\":\"" + fbid + "\",\"email\":\"" + email + "\",\"Username\":\"" + firstname + "\",\"Country\":\"" + country + "\",\"DOB\":\"" + birthday + "\",\"firstName\":\"" + firstname + "\",\"lastName\":\"" + lastname + "\",\"DeviceId\":\"\",\"gender\":\"" + gender + "\",\"imagebase64\":\"" + imagebase64 + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "SaveUserThirdParty");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    System.Web.HttpRuntime.Cache["Username"] = jsonDataSet.Tables["Profile"].Rows[0]["Username"].ToString();
                    System.Web.HttpRuntime.Cache["Country"] = jsonDataSet.Tables["Profile"].Rows[0]["Country"].ToString();
                    System.Web.HttpRuntime.Cache["ProfilePicture"] = jsonDataSet.Tables["Profile"].Rows[0]["ProfilePicture"].ToString();
                    Session["UserId"] = jsonDataSet.Tables["Profile"].Rows[0]["UserId"].ToString();
                    //ViewBag.Grammer = jsonDataSet.Tables[1].DefaultView;
                    System.Web.HttpRuntime.Cache["GrammerBookmark"] = jsonDataSet.Tables["Profile"].Rows[0]["GrammerBookmark"].ToString();
                    System.Web.HttpRuntime.Cache["VocabularyBookmark"] = jsonDataSet.Tables["Profile"].Rows[0]["VocabularyBookmark"].ToString();
                    System.Web.HttpRuntime.Cache["DialogBookmark"] = jsonDataSet.Tables["Profile"].Rows[0]["DialogBookmark"].ToString();
                    System.Web.HttpRuntime.Cache["VocabId"] = jsonDataSet.Tables["Profile"].Rows[0]["VocabId"].ToString();
                    System.Web.HttpRuntime.Cache["VocabSubId"] = jsonDataSet.Tables["Profile"].Rows[0]["VocabSubId"].ToString();
                    return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
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


        private byte[] GetImage(string url)
        {
            Stream stream = null;
            byte[] buf;

            try
            {
                WebProxy myProxy = new WebProxy();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();

                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }

                stream.Close();
                response.Close();
            }
            catch (Exception exp)
            {
                buf = null;
            }

            return (buf);
        }

        public ActionResult StartUp()
        {
            return View();
        }


        public ActionResult Index()
        {
            try
            {
                jsonstring = "{}";
                resultjson = objhelperclass.callservice(jsonstring, "GetAllStory");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    var finalarray = Regex.Split(resultjson, "StoryList\":");
                    string finalresultjson = finalarray[1] + "\\|";
                    finalresultjson = finalresultjson.Replace("}\\|", "");
                    JArray jsonArray = JArray.Parse(finalresultjson);
                    List<SuccessStoryModel> StoryDetails = jsonArray.ToObject<List<SuccessStoryModel>>();
                    return View(StoryDetails);
                }
                else
                {
                    return View("Error");
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
            //if (Session["UserId"] != null)
            //{
            //    return RedirectToAction("MyCourse", "Home");
            //}
            //return View();
        }
        public ActionResult About()
        {         

            return View();
        }
        public ActionResult Aboutus()
        {

            return PartialView("_about");
        }
        public ActionResult Contact()
        {

            return PartialView("_contact");
        }
        public ActionResult Mission()
        {
            return PartialView("_mission");
        }
        public ActionResult Help()
        {
            return View();
        }
        public ActionResult Tutors()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }

            Users objTutor = new Users();
            try
            {
                string json = "";//"{\"MailID\":\"" + id + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetAllTutors");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    

                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);

                        if (table.Rows[0][0].ToString() == "There is no tutor")
                        {
                            List<Users> objMail = new List<Users>();
                            return View(objMail);
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(result);
                            List<Users> TutorsList = jsonArray.ToObject<List<Users>>();
                            return View(TutorsList);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                List<MailModel> objList = new List<MailModel>();
                return View(objList);
            }            
        }
        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Contents.RemoveAll();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult AddSubscribe(string emailId,string type)
        {
            try
            {
                jsonstring = "{\"email\":\"" + emailId + "\",\"isSubscribe\":\"" + type + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "AddSubscribeType");

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
