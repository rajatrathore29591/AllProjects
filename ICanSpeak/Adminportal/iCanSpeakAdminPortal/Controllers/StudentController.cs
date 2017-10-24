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
using System.Drawing;
using System.Xml;

namespace iCanSpeakAdminPortal.Controllers
{
    public class StudentController : Controller
    {
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;

        [Authorize]
        public ActionResult StudentList()
        {
            try
            {
                string json = "{\"email\":\"" + Session["email"] + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetStudentByUserId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetStudentByUserId");
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

                        ViewBag.StudentsList = jsonDataSet.Tables["Student"].DefaultView;
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

        public ActionResult StudentMarks(string Studentid, string TutorId, string FirstName, string LastName)
        {
            ViewBag.FirstName = FirstName;
            ViewBag.LastName = LastName;
            ViewBag.StudentId = Studentid;
            ViewBag.TutorId = TutorId;

            return View();
        }

        [HttpPost]
        public ActionResult AddStudentMark(string Studentid, string TutorId, string Marks, string TotalMarks)
        {
            try
            {
                string json = "{\"StudentId\":\"" + Studentid + "\",\"TutorId\":\"" + TutorId + "\",\"Marks\":\"" + Marks + "\",\"TotalScore\":\"" + TotalMarks + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddStudentByTutorId");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/AddStudentByTutorId");
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
                return RedirectToAction("StudentList", "Student", new { TutorId = TutorId });
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
