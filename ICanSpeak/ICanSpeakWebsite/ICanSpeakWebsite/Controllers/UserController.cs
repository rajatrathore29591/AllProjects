using System;
using System.Collections.Generic;
using System.Data;
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
using ICanSpeakWebsite.App_Start;

namespace ICanSpeakWebsite.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataSet jsonDataSet = new DataSet();
        HelperClass objhelperclass = new HelperClass();
        string jsonstring = string.Empty;
        string resultjson = string.Empty;
        HttpWebRequest request;
        HttpWebResponse response;
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult UserProfile()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            try
            {
                //string strServicePath=
                UserProfileDetails objuserprofile = new UserProfileDetails();
                string Email = Session["loginEmail"].ToString();
                string json = "";
                json = "{\"Email\":\"" + Email + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetUserByEmail");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    //if (response.StatusCode != HttpStatusCode.OK)
                    //    throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode,response.StatusDescription));

                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        var table = JsonConvert.DeserializeObject<DataTable>(result);

                        if (table.Rows[0][0].ToString() == "There is no tutor")
                        {
                            List<Users> objUser = new List<Users>();
                            return View(objUser);
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(result);
                            List<Users> UsersList = jsonArray.ToObject<List<Users>>();
                            objuserprofile.Username = UsersList[0].Username;
                            objuserprofile.UserId = UsersList[0].UserId;
                            objuserprofile.DOB = UsersList[0].DOB.ToShortDateString();
                            objuserprofile.Email = UsersList[0].Email;
                            objuserprofile.Country = UsersList[0].Country;
                            objuserprofile.Gender = UsersList[0].Gender;
                            objuserprofile.AboutMeUrl = UsersList[0].AboutMe;
                            //return View(objuserprofile);
                            return View(objuserprofile);
                        }
                    }
                }

              
            
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }

        }
        public ActionResult ViewProfile()
        {
            return View();
        }
        public ActionResult EditProfile()
        {
            return View();
        }
        public ActionResult Account()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult Tutors()
        {
            Users objTutor = new Users();
            try
            {
                string json = "";//"{\"MailID\":\"" + id + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetTutorsByUserId");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    //if (response.StatusCode != HttpStatusCode.OK)
                    //    throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode,response.StatusDescription));

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

    }
}
