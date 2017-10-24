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
using ICanSpeakWebsite.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ICanSpeakWebsite.Controllers
{
    public class MailController : Controller
    {
        //
        // GET: /Mail/
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;

        public ActionResult Index()
        {
            MailBoxModel objMailBox = new MailBoxModel();
            objMailBox.GetInboxModel = GetInboxMails();
            objMailBox.GetOutBoxModel = GetOutBoxMails();
            return View(objMailBox);
        }
        public IEnumerable<MailModel> GetInboxMails()
        {
            try
            {
                string json = "{\"SenderID\":\"" + 2 + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetInboxMail");
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

                        if (table.Rows[0][0].ToString() == "No Question Available")
                        {
                            List<MailModel> objList = new List<MailModel>();
                            return objList;
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(result);
                            List<MailModel> grammer = jsonArray.ToObject<List<MailModel>>();
                            return grammer;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                List<MailModel> objList = new List<MailModel>();
                return objList;

            }
        }

        public IEnumerable<MailModel> GetOutBoxMails()
        {
            try
            {
                string json = "{\"SenderID\":\"" + 2 + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetOutboxMail");
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

                        if (table.Rows[0][0].ToString() == "No Question Available")
                        {
                            List<MailModel> objList = new List<MailModel>();
                            return objList;
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(result);
                            List<MailModel> grammer = jsonArray.ToObject<List<MailModel>>();
                            return grammer;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                List<MailModel> objList = new List<MailModel>();
                return objList;

            }
        }
        public ActionResult TagSearch(string term)
        {
            // Get Tags from database
            string[] tags = { "ASP.NET", "WebForms", 
                    "MVC", "jQuery", "ActionResult", 
                    "MangoDB", "Java", "Windows", "ActionScript",
"AppleScript",
"Asp",
"BASIC",
"C",
"C++",
"Clojure",
"COBOL",
"ColdFusion",
"Erlang",
"Fortran",
"Groovy",
"Haskell",
"Java",
"JavaScript",
"Lisp",
"Perl",
"PHP",
"Python",
"Ruby",
"Scala",
"Scheme" };
            return this.Json(tags.Where(t => t.Contains(term)),
                            JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMailByID(int? id)
        {
            MailModel objMailModel = new MailModel();
            try
            {
                string json = "{\"MailID\":\"" + id + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetMailByID");
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

                        if (table.Rows[0][0].ToString() == "No Question Available")
                        {
                            List<MailModel> objMail = new List<MailModel>();
                            return PartialView("_Mail", objMail);
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(result);
                            List<MailModel> grammer = jsonArray.ToObject<List<MailModel>>();
                            return PartialView("_Mail", grammer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                List<MailModel> objList = new List<MailModel>();
                return PartialView("_Mail", objList);
            }
        }
        public JsonResult DeleteMail(int? id)
        {
            try
            {
                string json = "{\"MailID\":\"" + id + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/DeleteMail");
                string sb = JsonConvert.SerializeObject(data);
                request.Method = "POST";
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string result = sr.ReadToEnd();
                        return Json(result.ToString(), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult ComposeMail(MailModel objMail)
        {
            string json = "{\"SenderID\":\"" + objMail.SenderID + "\",\"ReceiverID\":\"" + objMail.ReceiverID + "\",\"Subject\":\"" + objMail.Subject + "\",\"Body\":\"" + objMail.Body + "\",\"Date\":\"" + objMail.Date + "\",\"Status\":\"" + objMail.Status + "\",\"IsDeleted\":\"" + objMail.IsDeleted + "\"}";
            var data = javaScriptSerializer.DeserializeObject(json);
            request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/ComposeMail");
            string sb = JsonConvert.SerializeObject(data);
            request.Method = "POST";
            Byte[] bt = Encoding.UTF8.GetBytes(sb);
            Stream st = request.GetRequestStream();
            st.Write(bt, 0, bt.Length);
            st.Close();
            using (response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format(
                    "Server error (HTTP {0}: {1}).", response.StatusCode,
                    response.StatusDescription));
                Stream responseStream = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(responseStream))
                {
                    string result = sr.ReadToEnd();
                    var table = JsonConvert.DeserializeObject<DataTable>(result);
                    string tablemsg = table.Rows[0][0].ToString();
                    if (tablemsg == "Success")
                    {
                        return Json("Mail Send Successfully.", JsonRequestBehavior.AllowGet);
                        //int id = Convert.ToInt32(result);
                        //string filepath = Server.MapPath("/Uploads/CourseImages/");
                        //if (objModel.Image != null)
                        //{
                        //    filepath = Server.MapPath("../../CourseImages/");
                        //    objModel.Image.SaveAs(filepath + objModel.CourseId.ToString() + "_image_" + objModel.Image.FileName);
                        //}
                        //if (objModel.Audio != null)
                        //{
                        //    objModel.Image.SaveAs(filepath + objModel.CourseId.ToString() + "_audio_" + objModel.Audio.FileName);
                        //    filepath = Server.MapPath("../../CourseImages/");
                        //    objModel.Audio.SaveAs(filepath + objModel.CourseId.ToString() + "_audio_" + objModel.Audio.FileName);
                        //}
                    }
                    // return RedirectToAction("CourseList", "Courses");
                }
            }
            return Json("Mail Send Sucessfully.", JsonRequestBehavior.AllowGet);
        }

    }
}
