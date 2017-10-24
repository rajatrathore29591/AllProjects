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
using System.Xml;
using iCanSpeakAdminPortal.Models;
using iCanSpeakAdminPortal.App_Start;


namespace iCanSpeakAdminPortal.Controllers
{
    public class UserAccountsController : Controller
    {
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;
        DataSet jsonDataSet = new DataSet();
        string jsonstring = string.Empty;
        string resultjson = string.Empty;
        HelperClass objhelperclass = new HelperClass();

        [Authorize]
        public ActionResult UsersList(string Gender, string Country)
        {
            try
            {
                string json = "{}";
                var data = javaScriptSerializer.DeserializeObject(json);

                if (Gender != null)
                {
                    json = "{\"Gender\":\"" + Gender + "\"}";
                    data = javaScriptSerializer.DeserializeObject(json);
                    //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/FilterByGender");
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/FilterByGender");
                }
                else if (Country != null)
                {
                    json = "{\"Country\":\"" + Country + "\"}";
                    data = javaScriptSerializer.DeserializeObject(json);
                    //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/FilterByCountry");
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/FilterByCountry");
                }
                else
                {
                    data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetAllUsersAdmin");                
                }
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
                        JArray jsonArray = JArray.Parse(result);
                        List<UsersListModel> users = jsonArray.ToObject<List<UsersListModel>>();
                        return PartialView("UsersList", users.ToList());
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
        public ActionResult UserProfile(int userId)
        {
            try
            {
                string json = "{\"userId\":\"" + userId + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetUserProfile");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetUserProfile");
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

                        XmlDocument xd1 = new XmlDocument();
                        xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(result, "root");
                        jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                        UsersListModel users = new UsersListModel();

                        users.AboutMe = jsonDataSet.Tables["Profile"].Rows[0]["AboutMe"].ToString();
                        users.FirstName = jsonDataSet.Tables["Profile"].Rows[0]["FirstName"].ToString();
                        users.LastName = jsonDataSet.Tables["Profile"].Rows[0]["LastName"].ToString();
                        users.Country = jsonDataSet.Tables["Profile"].Rows[0]["Country"].ToString();
                        users.ProfilePicture = jsonDataSet.Tables["Profile"].Rows[0]["ProfilePicture"].ToString();
                        users.DOB = jsonDataSet.Tables["Profile"].Rows[0]["DOB"].ToString();
                        return View(users);
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
        public JsonResult ManageSuggestedUsers(string userId, string status)
        {
            try
            {
                string json = "{\"userId\":\"" + Convert.ToInt32(userId) + "\",\"status\":\"" + Convert.ToInt32(status) + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/Services/Service.svc/MakeSuggestedByUserId");
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
        public JsonResult ManageUsersStatus(string userId, string status)
        {
            try
            {
                string json = "{\"userId\":\"" + Convert.ToInt32(userId) + "\",\"softDelete\":\"" + status + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/Services/Service.svc/DeleteUserById");
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
    }
}
