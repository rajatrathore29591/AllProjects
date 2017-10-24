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

namespace iCanSpeakAdminPortal.Controllers
{
    public class UserQueriesController : Controller
    {
        //
        // GET: /UserQueries/
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;

        [Authorize]
        public ActionResult QueryList()
        {
            try
            {
                //string json = "{\"LoginUserId\":\"1\",\"AccessToken\":\"xdoykz\"}";
                string json = "";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetAllUserQuery");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetAllUserQuery");
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
                        if (table.ToString() == "No data")
                        {
                            List<UserQueriesModel> objList = new List<UserQueriesModel>();
                            return View(objList);
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(result);
                            List<UserQueriesModel> queires = jsonArray.ToObject<List<UserQueriesModel>>();
                            return View(queires);
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
        public ActionResult GetQueryDetails(int QueryId = 0)
        {
            try
            {
                string json = "{\"queryId\":\"" + QueryId + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/ReadQuery");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/ReadQuery");
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
                        result = "[" + result + "]";
                        var table = JsonConvert.DeserializeObject<DataTable>(result);
                        UserQueriesModel objModel = new UserQueriesModel();
                        objModel.ContactNo = table.Rows[0]["ContactNo"].ToString();
                        objModel.CreatedDate = table.Rows[0]["CreatedDate"].ToString();
                        objModel.EmailId = table.Rows[0]["EmailId"].ToString();
                        objModel.Message = table.Rows[0]["Message"].ToString();
                        objModel.Name = table.Rows[0]["Name"].ToString();
                        objModel.Subject = table.Rows[0]["Subject"].ToString();
                        objModel.IsRead = Convert.ToBoolean(table.Rows[0]["IsRead"]);

                        return View(objModel);
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
        public JsonResult DeleteQueryByQueryId(int QueryId = 0)
        {
            try
            {
                string json = "{\"queryId\":\"" + QueryId + "\",\"softDelete\":\"false\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/DeleteQueryById");
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
                        if (table.Rows[0][0].ToString() == "Query deleted successfully")
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
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
