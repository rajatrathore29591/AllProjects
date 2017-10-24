using iCanSpeakAdminPortal.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Web.Security;

namespace iCanSpeakAdminPortal.Controllers
{
    public class PageManagementController : Controller
    {
        //
        // GET: /PageManagement/
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;

        [Authorize]
        public ActionResult About()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult About(AboutTextModel objModel)
        //{
        //    return View();
        //}

        [Authorize]
        public ActionResult SaveAbout(string abouttext, string title, string operation)
        {
            try
            {
                abouttext = abouttext.Replace("~#", "\'");
                abouttext = abouttext.Replace("~!", "<");
                abouttext = abouttext.Replace("!~", ">");

                string json = "{\"aboutId\":\"1\",\"title\":\"" + title + "\",\"screenName\":\"\",\"description\":\"" + abouttext + "\",\"operation\":\"" + operation + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddUpdateAboutContent");
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
                        int count = result.Length;
                        if (count == 23)
                        {
                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            // JArray jsonArray = JArray.Parse(result);
                            var table = JsonConvert.DeserializeObject<DataTable>(result);
                            List<AboutTextModel> obj = new List<AboutTextModel>();
                            AboutTextModel objModel = new AboutTextModel();
                            objModel.Title = table.Rows[0]["Title"].ToString();
                            objModel.Description = table.Rows[0]["Description"].ToString();
                            obj.Add(objModel);
                            return Json(obj, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                List<UsersListModel> users = new List<UsersListModel>();
                return View(users);
            }
        }


        [Authorize]
        public ActionResult FAQ(int? FaqId)
        {
            try
            {
                string json = "{\"FaqId\":\"" + FaqId + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetFaqByFaqId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetFaqByFaqId");
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

                        FaqTextModel objFaq = new FaqTextModel();
                        objFaq.FaqId = table.Rows[0]["FaqId"].ToString();
                        objFaq.Title = table.Rows[0]["Title"].ToString();
                        objFaq.Description = table.Rows[0]["Description"].ToString();
                        return View(objFaq);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }
        //public ActionResult FAQ(string abouttext, string title, string operation)
        //{
        //    try
        //    {
        //        abouttext = abouttext.Replace("~#", "\'");
        //        abouttext = abouttext.Replace("~!", "<");
        //        abouttext = abouttext.Replace("!~", ">");

        //        string json = "{\"aboutId\":\"1\",\"title\":\"" + title + "\",\"screenName\":\"\",\"description\":\"" + abouttext + "\",\"operation\":\"" + operation + "\"}";
        //        var data = javaScriptSerializer.DeserializeObject(json);
        //        request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddUpdateFaqContent");
        //        request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/AddUpdateFaqContent");

        //        string sb = JsonConvert.SerializeObject(data);
        //        request.Method = "POST";
        //        Byte[] bt = Encoding.UTF8.GetBytes(sb);
        //        Stream st = request.GetRequestStream();
        //        st.Write(bt, 0, bt.Length);
        //        st.Close();
        //        using (response = request.GetResponse() as HttpWebResponse)
        //        {
        //            if (response.StatusCode != HttpStatusCode.OK)
        //                throw new Exception(String.Format(
        //                "Server error (HTTP {0}: {1}).", response.StatusCode,
        //                response.StatusDescription));
        //            Stream responseStream = response.GetResponseStream();
        //            using (StreamReader sr = new StreamReader(responseStream))
        //            {
        //                string result = sr.ReadToEnd();
        //                int count = result.Length;
        //                if (count == 23)
        //                {
        //                    return Json("Success", JsonRequestBehavior.AllowGet);
        //                }
        //                else
        //                {
        //                    // JArray jsonArray = JArray.Parse(result);
        //                    var table = JsonConvert.DeserializeObject<DataTable>(result);
        //                    List<AboutTextModel> obj = new List<AboutTextModel>();
        //                    AboutTextModel objModel = new AboutTextModel();
        //                    objModel.Title = table.Rows[0]["Title"].ToString();
        //                    objModel.Description = table.Rows[0]["Description"].ToString();
        //                    obj.Add(objModel);
        //                    return Json(obj, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        List<UsersListModel> users = new List<UsersListModel>();
        //        return View(users);
        //    }
        //}
        [Authorize]
        public ActionResult FAQList()
        {
            try
            {
                string json = "";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetAllFAQ1");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetAllFAQ1");
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
                        if (table.Rows[0][0].ToString() == "No Data")
                        {
                            List<FaqTextModel> faq = new List<FaqTextModel>();
                            return View(faq);
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(result);
                            List<FaqTextModel> faq = jsonArray.ToObject<List<FaqTextModel>>();
                            return View(faq);
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

        [Authorize]
        public ActionResult AddFaq(int FaqId = 0)
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFaq(FaqTextModel objModel)
        {
            try
            {

                string json = "{\"Title\":\"" + objModel.Title + "\",\"Description\":\"" + objModel.Description + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddFaq1");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/AddFaq1");
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
                        return RedirectToAction("FAQList", "PageManagement", new { FaqId = objModel.FaqId });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult UpdateFAQ(int FaqId)
        {
            try
            {
                FaqTextModel objFaqModel = new FaqTextModel();
                string json = "{\"FaqId\":\"" + FaqId + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetFaqByFaqId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetFaqByFaqId");
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
                        objFaqModel.FaqId = table.Rows[0]["FaqId"].ToString();
                        objFaqModel.Title = table.Rows[0]["Title"].ToString();
                        objFaqModel.Description = table.Rows[0]["Description"].ToString();

                        return View(objFaqModel);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateFAQ(FaqTextModel objModel)
        {
            try
            {
                string json = "{\"FaqId\":\"" + objModel.FaqId + "\",\"Title\":\"" + objModel.Title + "\",\"Description\":\"" + objModel.Description + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/UpdateFAQ");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/UpdateFAQ");
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
                        return RedirectToAction("FAQList", "PageManagement");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult Welcome()
        {
            return View();
        }

        [Authorize]
        public JsonResult DeleteFAQByFaqId(string FaqId, string status)
        {
            try
            {
                string json = "{\"FaqId\":\"" + FaqId + "\",\"softDelete\":\"" + status + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/DeleteFAQByFaqId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/DeleteFAQByFaqId");
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
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        [Authorize]
        public ActionResult SaveWelcome(string header, string message, string operation)
        {
            try
            {
                message = message.Replace("~#", "\'");
                message = message.Replace("~!", "<");
                message = message.Replace("!~", ">");

                string json = "{\"id\":\"1\",\"header\":\"" + header + "\",\"screenName\":\"\",\"message\":\"" + message + "\",\"operation\":\"" + operation + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddUpdateWelcomeContent");

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
                        int count = result.Length;
                        if (count == 23)
                        {
                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(result);
                            List<WelcomeTextModel> users = jsonArray.ToObject<List<WelcomeTextModel>>();
                            return Json(users, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                List<UsersListModel> users = new List<UsersListModel>();
                return View(users);
            }
        }

        [Authorize]
        public ActionResult Help()
        {
            return View();
        }

        [Authorize]
        public ActionResult SaveHelp(string header, string message, string operation)
        {
            try
            {
                message = message.Replace("~#", "\'");
                message = message.Replace("~!", "<");
                message = message.Replace("!~", ">");

                string json = "{\"ContentType\":\"" + header + "\",\"Content\":\"" + message + "\",\"operation\":\"" + operation + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddUpdateHelpContent");
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
                        int count = result.Length;
                        if (count == 23)
                        {
                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(result);
                            List<HelpContentModel> users = jsonArray.ToObject<List<HelpContentModel>>();
                            return Json(users, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                List<UsersListModel> users = new List<UsersListModel>();
                return View(users);
            }
        }

        [Authorize]
        public ActionResult NewsLetter()
        {
            return View();
        }

        [Authorize]
        public ActionResult SendNewsLetter(string subject, string message)
        {
            try
            {
                string json = "{\"subject\":\"" + subject + "\",\"messageBody\":\"" + message + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/NewsLetter");
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
                        if (result.ToLower().Contains("success"))
                        {
                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("Failed", JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                List<UsersListModel> users = new List<UsersListModel>();
                return View(users);
            }
        }

        [Authorize]
        public ActionResult TermsConditions()
        {
            return View();
        }

        [Authorize]
        public ActionResult SaveTermsConditions(string message, string operation)
        {
            try
            {
                message = message.Replace("~#", "\'");
                message = message.Replace("~!", "<");
                message = message.Replace("!~", ">");

                string json = "{\"text\":\"" + message + "\",\"operation\":\"" + operation + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/TermsAndCondition");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/TermsAndCondition");
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

                        if (operation == "select")
                        {
                            var table = JsonConvert.DeserializeObject<DataTable>("[" + sr.ReadToEnd() + "]");
                            return Json(table.Rows[0]["Text"].ToString(), JsonRequestBehavior.AllowGet);
                        }
                        if (operation == "update")
                        {
                            var table = JsonConvert.DeserializeObject<DataTable>(sr.ReadToEnd());
                            return Json(table.Rows[0]["Message"].ToString(), JsonRequestBehavior.AllowGet);
                        }
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult SuccessStoryList()
        {
            try
            {
                string json = "";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/SuccessStoryList");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/SuccessStoryList");
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
                        if (table.Rows[0][0].ToString() == "No Data")
                        {
                            List<SuccessStoryModel> story = new List<SuccessStoryModel>();
                            return View(story);
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(result);
                            List<SuccessStoryModel> story = jsonArray.ToObject<List<SuccessStoryModel>>();
                            return View(story);
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

        [Authorize]
        public ActionResult AddSuccessStory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSuccessStory(SuccessStoryModel obj)
        {
            string filepath = string.Empty;
            try
            {
                string json = "{\"clientName\":\"" + obj.ClientName + "\",\"clientStory\":\"" + obj.ClientStory + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddSuccessStory");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/AddSuccessStory");
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
                        string StoryId = sr.ReadToEnd();                       
                        if (obj.Image != null)
                        {
                            filepath = Server.MapPath("../../ClientImage/");
                            obj.Image.SaveAs(filepath + StoryId.ToString() + "_ClientImage.jpg");
                        }
                        return RedirectToAction("SuccessStoryList", "PageManagement");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult DeleteStoryByStoryId(string StoryId)
        {
            try
            {
                string json = "{\"StoryId\":\"" + Convert.ToInt32(StoryId) + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/Services/Service.svc/DeleteStoryByStoryId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/DeleteStoryByStoryId");
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
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult UpdateStory(int StoryId)
        {
            try
            {
                SuccessStoryModel objStoryModel = new SuccessStoryModel();
                string json = "{\"StoryId\":\"" + StoryId.ToString() + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetStoryByStoryId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetStoryByStoryId");
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
                        //SuccessStoryModel objModel = new SuccessStoryModel();
                        objStoryModel.StoryId = Convert.ToInt32(table.Rows[0]["StoryId"].ToString());
                        objStoryModel.ClientName = table.Rows[0]["ClientName"].ToString();
                        objStoryModel.ClientStory = table.Rows[0]["ClientStory"].ToString();
                        //objStoryModel.ClientImageUrl = table.Rows[0]["ClientImageUrl"].ToString();
                        return View(objStoryModel);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult UpdateStory1(string storyId, string clientName, string clientStory)
        {
            try
            {
                string json = "{\"storyId\":\"" + storyId + "\",\"clientName\":\"" + clientName + "\",\"clientStory\":\"" + clientStory + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/UpdateStoryByStoryId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/UpdateStoryByStoryId");
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
                        //if (objTrans.Image != null)
                        //{
                        //    string filepath = Server.MapPath("../../ClientImage/");
                        //    objTrans.Image.SaveAs(filepath + objTrans.StoryId.ToString() + "_ClientImage.jpg");
                        //}
                        return RedirectToAction("SuccessStoryList", "PageManagement");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult ViewStory(int? StoryId)
        {
            try
            {
                string json = "{\"StoryId\":\"" + StoryId + "\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetStoryByStoryId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetStoryByStoryId");
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

                        SuccessStoryModel objStory = new SuccessStoryModel();
                        objStory.StoryId = Convert.ToInt32(table.Rows[0]["StoryId"].ToString());
                        objStory.ClientName = table.Rows[0]["ClientName"].ToString();
                        objStory.ClientStory = table.Rows[0]["ClientStory"].ToString();
                        objStory.ClientImageUrl = table.Rows[0]["ClientImageUrl"].ToString();
                        return View(objStory);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult DemoVideo()
        {
            return View();
        }
        

        //[Authorize]
        //public ActionResult DemoVideo(string VideoId)
        //{
        //    try
        //    {
        //        string json = "{\"VideoId\":\"1\"}";
        //        var data = javaScriptSerializer.DeserializeObject(json);
        //        request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetVideoDetail");
        //        request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetVideoDetail");
        //        string sb = JsonConvert.SerializeObject(data);
        //        request.Method = "POST";
        //        Byte[] bt = Encoding.UTF8.GetBytes(sb);
        //        Stream st = request.GetRequestStream();
        //        st.Write(bt, 0, bt.Length);
        //        st.Close();
        //        using (response = request.GetResponse() as HttpWebResponse)
        //        {
        //            if (response.StatusCode != HttpStatusCode.OK)
        //                throw new Exception(String.Format(
        //                "Server error (HTTP {0}: {1}).", response.StatusCode,
        //                response.StatusDescription));
        //            Stream responseStream = response.GetResponseStream();
        //            using (StreamReader sr = new StreamReader(responseStream))
        //            {
        //                string result = sr.ReadToEnd();
        //                var table = JsonConvert.DeserializeObject<DataTable>(result);
        //                DemoVideoModel objVideo = new DemoVideoModel();
        //                objVideo.VideoId = Convert.ToInt32(table.Rows[0]["VideoId"].ToString());
        //                objVideo.VideoName = table.Rows[0]["VideoName"].ToString();
        //                objVideo.VideoUrl = table.Rows[0]["VideoUrl"].ToString();
        //                return View(objVideo);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrorName = ex.Message;
        //        return View("Error");
        //    }
        //}

        [Authorize]
        [HttpPost]
        public ActionResult DemoVideo(DemoVideoModel objModel)
        {
            string json = "{\"VideoName\":\"" + objModel.VideoName + "\"}";
            var data = javaScriptSerializer.DeserializeObject(json);
            request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/UpdateVideo");
            //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/UpdateVideo");
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
                    //var table = JsonConvert.DeserializeObject<DataTable>(result);
                    //if (table.Rows[0][0].ToString() == "success")
                    {
                        int VideoId = objModel.VideoId;
                        VideoId = VideoId + 1;
                        if (objModel.Video != null)
                        {
                            string filepath = Server.MapPath("../../DemoVideo/");
                            objModel.Video.SaveAs(filepath + VideoId.ToString() + "_demovideo.mp4");
                        }
                    }
                    return RedirectToAction("DemoVideo", "PageManagement");
                }
            }
        }
    }
}


