using Newtonsoft.Json;
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
using System.Xml;

namespace iCanSpeakAdminPortal.Controllers
{
    public class SubscriptionController : Controller
    {
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;

        public ActionResult GetSubscriptionDetail()
        {
            try
            {
                string json = "{\"email\":\"" + Session["email"] + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetSubscriptionDetail");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetSubscriptionDetail");
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
                        DataSet jsonDataSet = new DataSet();
                        string result = sr.ReadToEnd();

                        XmlDocument xd1 = new XmlDocument();
                        xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(result, "root");
                        jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                        ViewBag.SubscriptionList = jsonDataSet.Tables["Subscription"].DefaultView;
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        public ActionResult GrammerCount(string premiumSubscriptionId)
        {
            try
            {
                string json = "{\"PremiumSubscriptionId\":\"" + premiumSubscriptionId + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetGrammerBySubscriptionId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetGrammerBySubscriptionId");
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
                        DataSet jsonDataSet = new DataSet();
                        string result = sr.ReadToEnd();

                        XmlDocument xd1 = new XmlDocument();
                        xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(result, "root");
                        jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                        ViewBag.PremiumSubscriptionId = jsonDataSet.Tables["GrammerDetail"].Rows[0]["PremiumSubscriptionId"].ToString();
                        ViewBag.PlanName = jsonDataSet.Tables["GrammerDetail"].Rows[0]["SubscriptionName"].ToString();
                        ViewBag.GrammerCount = jsonDataSet.Tables["GrammerDetail"].DefaultView;
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }
        public ActionResult VocabCount(string premiumSubscriptionId)
        {
            try
            {
                string json = "{\"PremiumSubscriptionId\":\"" + premiumSubscriptionId + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetVocabBySubscriptionId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetVocabBySubscriptionId");
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
                        DataSet jsonDataSet = new DataSet();
                        string result = sr.ReadToEnd();

                        XmlDocument xd1 = new XmlDocument();
                        xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(result, "root");
                        jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                        ViewBag.PremiumSubscriptionId = jsonDataSet.Tables["VocabDetail"].Rows[0]["PremiumSubscriptionId"].ToString();
                        ViewBag.PlanName = jsonDataSet.Tables["VocabDetail"].Rows[0]["SubscriptionName"].ToString();
                        ViewBag.VocabCount = jsonDataSet.Tables["VocabDetail"].DefaultView;
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }
        public ActionResult DialogCount(string premiumSubscriptionId)
        {
            try
            {
                string json = "{\"PremiumSubscriptionId\":\"" + premiumSubscriptionId + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetDialogBySubscriptionId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetDialogBySubscriptionId");
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
                        DataSet jsonDataSet = new DataSet();
                        string result = sr.ReadToEnd();

                        XmlDocument xd1 = new XmlDocument();
                        xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(result, "root");
                        jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                        ViewBag.PremiumSubscriptionId = jsonDataSet.Tables["DialogDetail"].Rows[0]["PremiumSubscriptionId"].ToString();
                        ViewBag.PlanName = jsonDataSet.Tables["DialogDetail"].Rows[0]["SubscriptionName"].ToString();
                        ViewBag.DialogCount = jsonDataSet.Tables["DialogDetail"].DefaultView;
                        return View();
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
        public JsonResult DeleteSubscriptionByMappingId(string mappingId)
        {
            try
            {
                string json = "{\"MappingId\":\"" + mappingId + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/DeleteBySubscriptionId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/DeleteBySubscriptionId");
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
        public ActionResult GrammerSubscription(string PlanName, string SubscriptionId)
        {
            ViewBag.PlanName = PlanName;
            ViewBag.SubscriptionId = SubscriptionId;
            
            try
            {
                string json = "";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetGrammerUnitDetail");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetGrammerUnitDetail");
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
                        DataSet jsonDataSet = new DataSet();
                        string result = sr.ReadToEnd();

                        XmlDocument xd1 = new XmlDocument();
                        xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(result, "root");
                        jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                        ViewBag.GrammerDetail = jsonDataSet.Tables["GrammerDetail"].DefaultView;
                        return View();
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
        public ActionResult AddGrammerSubscription(string SubscriptionId, string CourseId)
        {
            try
            {
                string json = "{\"SubscriptionId\":\"" + SubscriptionId + "\",\"CourseId\":\"" + CourseId + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddGrammerBySubscriptionId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/AddGrammerBySubscriptionId");
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
                    }
                }
                return RedirectToAction("GrammerCount", "Subscription", new { premiumSubscriptionId = SubscriptionId });
            }
            catch (Exception ex)
            {
                TempData["Message"] = "An error occured, please try again";
                //return View("AddKeyPhrase");
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult VocabSubscription(string PlanName, string SubscriptionId)
        {
            ViewBag.PlanName = PlanName;
            ViewBag.SubscriptionId = SubscriptionId;

            try
            {
                string json = "";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetVocabUnitDetail");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetVocabUnitDetail");
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
                        DataSet jsonDataSet = new DataSet();
                        string result = sr.ReadToEnd();

                        XmlDocument xd1 = new XmlDocument();
                        xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(result, "root");
                        jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                        ViewBag.VocabDetail = jsonDataSet.Tables["VocabDetail"].DefaultView;
                        return View();
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
        public ActionResult AddVocabSubscription(string SubscriptionId, string CourseId)
        {
            try
            {
                string json = "{\"SubscriptionId\":\"" + SubscriptionId + "\",\"CourseId\":\"" + CourseId + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddVocabBySubscriptionId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/AddVocabBySubscriptionId");
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
                    }
                }
                return RedirectToAction("VocabCount", "Subscription", new { premiumSubscriptionId = SubscriptionId });
            }
            catch (Exception ex)
            {
                TempData["Message"] = "An error occured, please try again";
                //return View("AddKeyPhrase");
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult DialogSubscription(string PlanName, string SubscriptionId)
        {
            ViewBag.PlanName = PlanName;
            ViewBag.SubscriptionId = SubscriptionId;

            try
            {
                string json = "";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetDialogUnitDetail");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetDialogUnitDetail");
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
                        DataSet jsonDataSet = new DataSet();
                        string result = sr.ReadToEnd();

                        XmlDocument xd1 = new XmlDocument();
                        xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(result, "root");
                        jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                        ViewBag.DialogDetail = jsonDataSet.Tables["DialogDetail"].DefaultView;
                        return View();
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
        public ActionResult AddDialogSubscription(string SubscriptionId, string CourseId)
        {
            try
            {
                string json = "{\"SubscriptionId\":\"" + SubscriptionId + "\",\"CourseId\":\"" + CourseId + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddDialogBySubscriptionId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/AddDialogBySubscriptionId");
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
                    }
                }
                return RedirectToAction("DialogCount", "Subscription", new { premiumSubscriptionId = SubscriptionId });
            }
            catch (Exception ex)
            {
                TempData["Message"] = "An error occured, please try again";
                //return View("AddKeyPhrase");
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }
    }
}
