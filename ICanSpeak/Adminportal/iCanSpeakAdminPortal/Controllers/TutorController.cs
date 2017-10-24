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
using Newtonsoft.Json;

namespace iCanSpeakAdminPortal.Controllers
{
    public class TutorController : Controller
    {
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;
        string finalresult = string.Empty;
        DataSet jsonDataSet = new DataSet();
        //
        // GET: /Tutor/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TutorSubTutorList()
        {
            return View();
        }

        public ActionResult AddTutor(string userid)
        {
            try
            {
                string json = "{}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetTutorSubTutorList");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetTutorSubTutorList");
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
                        finalresult = sr.ReadToEnd();
                        if (finalresult != "")
                        {
                            XmlDocument xd1 = new XmlDocument();
                            xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(finalresult, "root");
                            jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                            if (jsonDataSet.Tables["Tutor"] != null)
                            {
                                SelectListItem[] allItems = jsonDataSet.Tables["Tutor"].AsEnumerable()
                               .Select(r => new SelectListItem { Text = r.Field<string>(1), Value = r.Field<string>(0) })
                               .ToArray();
                                ViewBag.TutorNames = allItems;
                            }
                            if (jsonDataSet.Tables["SubTutor"] != null)
                            {
                                SelectListItem[] allItems = jsonDataSet.Tables["SubTutor"].AsEnumerable()
                               .Select(r => new SelectListItem { Text = r.Field<string>(1), Value = r.Field<string>(0) })
                               .ToArray();

                                ViewBag.SubTutorNames = allItems;

                            }
                            ViewBag.StudentID = userid;
                        }

                        return View();
                    }
                }


            }
            catch (Exception ex)
            {
                ViewBag.Result = ex.Message;
                return View("Error");
            }

        }


        [HttpPost]
        public ActionResult AddTutor(TutorModel obj, IEnumerable<string> SelectItems)
        {

            try
            {
                string SubTutorID = string.Join("~", SelectItems);
                string json = "{\"TutorId\":\"" + obj.TutorID + "\",\"SubTutorID\":\"" + SubTutorID + "\",\"StudentUserId\":\"" + obj.studentid + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/SaveStudentTutor");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/SaveStudentTutor");
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
                        finalresult = sr.ReadToEnd();
                        if (finalresult.Contains("success"))
                        {
                            TempData["Alert"] = "Student is successfully associate with Tutor !!";
                            return RedirectToAction("UsersList", "UserAccounts");
                        }
                        else
                        {
                            TempData["Alert"] = "This student already associate with this Tutor!";
                        }

                        return View();
                    }
                }


            }
            catch (Exception ex)
            {
                ViewBag.Result = ex.Message;
                return View("Error");
            }

        }




    }
}
