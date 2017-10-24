using iCanSpeakAdminPortal.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace iCanSpeakAdminPortal.Controllers
{
    public class SubmissionsController : Controller
    {
        //
        // GET: /Submissions/
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;

        [Authorize]
        public ActionResult AllSubmissions()
        {
            try
            {
                string json = "{\"LoginUserId\":\"2\",\"AccessToken\":\"ugox09\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetAllSubmissions");
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
                        List<SubmissionModel> users = jsonArray.ToObject<List<SubmissionModel>>();
                        return View(users.ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }
    }
}
