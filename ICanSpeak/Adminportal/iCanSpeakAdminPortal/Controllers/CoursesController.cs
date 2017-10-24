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
    public class CoursesController : Controller
    {
        //
        // GET: /Courses/
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;

        [Authorize]
        public ActionResult CourseList()
        {
            try
            {
                string json = "{\"LoginUserId\":\"2\",\"AccessToken\":\"ugox09\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetAllCourse");
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
                        List<CourseModel> users = jsonArray.ToObject<List<CourseModel>>();
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

        [Authorize]
        public ActionResult AddCourse(int CourseId = 0)
        {
            try
            {
                if (CourseId != 0)
                {
                    string json = "{\"courseId\":\"" + CourseId + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetCourseByCourseId");
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
                            //int id = Convert.ToInt32(result);

                            var table = JsonConvert.DeserializeObject<DataTable>(result);
                            CourseModel objCourse = new CourseModel();
                            objCourse.CourseId = Convert.ToInt32(table.Rows[0]["CourseId"]);
                            objCourse.CourseName = table.Rows[0]["CourseName"].ToString();
                            objCourse.CourseDescription = table.Rows[0]["CourseDescription"].ToString();
                            objCourse.CourseType = table.Rows[0]["CourseType"].ToString();
                            objCourse.Duration = table.Rows[0]["Duration"].ToString();
                            objCourse.RewardPoints = table.Rows[0]["RewardPoints"].ToString();
                            objCourse.Price = Convert.ToInt32(table.Rows[0]["Price"]);

                            if (objCourse.ImageUrl != null)
                            {
                                objCourse.ImageUrl = table.Rows[0]["ImageUrl"].ToString();
                            }
                            if (objCourse.AudioUrl != null)
                            {
                                objCourse.AudioUrl = table.Rows[0]["AudioUrl"].ToString();
                            }
                            objCourse.MaxScore = table.Rows[0]["MaxScore"].ToString();
                            objCourse.IsFree = Convert.ToBoolean(table.Rows[0]["IsFree"]);
                            objCourse.Unit = Convert.ToInt32(table.Rows[0]["Unit"]);
                            return View(objCourse);
                            //return RedirectToAction("AddCourse", "Courses", new { UnitId = UnitId });
                        }
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult ViewCourseInfo(int CourseId = 0)
        {
            try
            {
                if (CourseId != 0)
                {
                    string json = "{\"courseId\":\"" + CourseId + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetCourseByCourseId");
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
                            //int id = Convert.ToInt32(result);

                            var table = JsonConvert.DeserializeObject<DataTable>(result);
                            CourseModel objCourse = new CourseModel();
                            objCourse.CourseId = Convert.ToInt32(table.Rows[0]["CourseId"]);
                            objCourse.CourseName = table.Rows[0]["CourseName"].ToString();
                            objCourse.CourseDescription = table.Rows[0]["CourseDescription"].ToString();
                            objCourse.CourseType = table.Rows[0]["CourseType"].ToString();
                            objCourse.Duration = table.Rows[0]["Duration"].ToString();
                            objCourse.RewardPoints = table.Rows[0]["RewardPoints"].ToString();
                            objCourse.ImageUrl = table.Rows[0]["ImageUrl"].ToString();
                            objCourse.AudioUrl = table.Rows[0]["AudioUrl"].ToString();
                            objCourse.Price = Convert.ToInt32(table.Rows[0]["Price"]);
                            if (objCourse.ImageUrl != null)
                            {
                                objCourse.ImageUrl = table.Rows[0]["ImageUrl"].ToString();
                            }
                            if (objCourse.AudioUrl != null)
                            {
                                objCourse.AudioUrl = table.Rows[0]["AudioUrl"].ToString();
                            }
                            objCourse.MaxScore = table.Rows[0]["MaxScore"].ToString();
                            objCourse.IsFree = Convert.ToBoolean(table.Rows[0]["IsFree"]);
                            objCourse.Unit = Convert.ToInt32(table.Rows[0]["Unit"]);
                            return View(objCourse);
                            //return RedirectToAction("AddCourse", "Courses", new { UnitId = UnitId });
                        }
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult AddCourse(CourseModel objModel)
        {
            string json = string.Empty;
            int id = 0;
            try
            {
                if (objModel.CourseId != 0)
                {
                    
                    if (objModel.CourseType == "grammer")
                    {
                        json = "{\"courseId\":\"" + objModel.CourseId + "\",\"courseName\":\"" + objModel.CourseName + "\",\"courseDescription\":\"" + objModel.CourseDescription + "\",\"courseType\":\"" + objModel.CourseType + "\",\"duration\":\"" + objModel.Duration + "\",\"rewardPoints\":\"" + objModel.RewardPoints + "\",\"price\":\"" + objModel.Price + "\",\"imageUrl\":\"\",\"audioUrl\":\"" + objModel.Video.FileName + "\",\"maxScore\":\"" + objModel.MaxScore + "\",\"isFree\":\"" + objModel.IsFree + "\",\"unit\":\"" + objModel.Unit + "\"}";
                    }
                    if (objModel.CourseType == "vocabulary")
                    {
                        json = "{\"courseId\":\"" + objModel.CourseId + "\",\"courseName\":\"" + objModel.CourseName + "\",\"courseDescription\":\"" + objModel.CourseDescription + "\",\"courseType\":\"" + objModel.CourseType + "\",\"duration\":\"" + objModel.Duration + "\",\"rewardPoints\":\"" + objModel.RewardPoints + "\",\"price\":\"" + objModel.Price + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"maxScore\":\"" + objModel.MaxScore + "\",\"isFree\":\"" + objModel.IsFree + "\",\"unit\":\"" + objModel.Unit + "\"}";
                    }
                    if (objModel.CourseType == "dialog")
                    {
                        json = "{\"courseId\":\"" + objModel.CourseId + "\",\"courseName\":\"" + objModel.CourseName + "\",\"courseDescription\":\"" + objModel.CourseDescription + "\",\"courseType\":\"" + objModel.CourseType + "\",\"duration\":\"" + objModel.Duration + "\",\"rewardPoints\":\"" + objModel.RewardPoints + "\",\"price\":\"" + objModel.Price + "\",\"imageUrl\":\"\",\"audioUrl\":\"" + objModel.Video.FileName + "\",\"maxScore\":\"" + objModel.MaxScore + "\",\"isFree\":\"" + objModel.IsFree + "\",\"unit\":\"" + objModel.Unit + "\"}";
                    }

                    //if (objModel.Image != null && objModel.Audio != null)
                    //{
                    //    json = "{\"courseId\":\"" + objModel.CourseId + "\",\"courseName\":\"" + objModel.CourseName + "\",\"courseDescription\":\"" + objModel.CourseDescription + "\",\"courseType\":\"" + objModel.CourseType + "\",\"duration\":\"" + objModel.Duration + "\",\"rewardPoints\":\"" + objModel.RewardPoints + "\",\"price\":\"" + objModel.Price + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"maxScore\":\"" + objModel.MaxScore + "\",\"isFree\":\"" + objModel.IsFree + "\",\"unit\":\"" + objModel.Unit + "\"}";
                    //}
                    //if (objModel.Image == null && objModel.Audio != null)
                    //{
                    //    json = "{\"courseId\":\"" + objModel.CourseId + "\",\"courseName\":\"" + objModel.CourseName + "\",\"courseDescription\":\"" + objModel.CourseDescription + "\",\"courseType\":\"" + objModel.CourseType + "\",\"duration\":\"" + objModel.Duration + "\",\"rewardPoints\":\"" + objModel.RewardPoints + "\",\"price\":\"" + objModel.Price + "\",\"imageUrl\":\"\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"maxScore\":\"" + objModel.MaxScore + "\",\"isFree\":\"" + objModel.IsFree + "\",\"unit\":\"" + objModel.Unit + "\"}";
                    //}
                    //if (objModel.Image != null && objModel.Audio == null)
                    //{
                    //    json = "{\"courseId\":\"" + objModel.CourseId + "\",\"courseName\":\"" + objModel.CourseName + "\",\"courseDescription\":\"" + objModel.CourseDescription + "\",\"courseType\":\"" + objModel.CourseType + "\",\"duration\":\"" + objModel.Duration + "\",\"rewardPoints\":\"" + objModel.RewardPoints + "\",\"price\":\"" + objModel.Price + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"\",\"maxScore\":\"" + objModel.MaxScore + "\",\"isFree\":\"" + objModel.IsFree + "\",\"unit\":\"" + objModel.Unit + "\"}";
                    //}
                    //else
                    //{
                    //    json = "{\"courseId\":\"" + objModel.CourseId + "\",\"courseName\":\"" + objModel.CourseName + "\",\"courseDescription\":\"" + objModel.CourseDescription + "\",\"courseType\":\"" + objModel.CourseType + "\",\"duration\":\"" + objModel.Duration + "\",\"rewardPoints\":\"" + objModel.RewardPoints + "\",\"price\":\"" + objModel.Price + "\",\"imageUrl\":\"\",\"audioUrl\":\"\",\"maxScore\":\"" + objModel.MaxScore + "\",\"isFree\":\"" + objModel.IsFree + "\",\"unit\":\"" + objModel.Unit + "\"}";
                    //}
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/UpdateCourse");
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
                                //int id = Convert.ToInt32(result);
                                if (objModel.Image != null)
                                {
                                    string filepath = Server.MapPath("../../CourseImages/");
                                    objModel.Image.SaveAs(filepath + objModel.CourseId.ToString() + "_image_" + objModel.Image.FileName);
                                }
                                if (objModel.Audio != null)
                                {
                                    string filepath = Server.MapPath("../../CourseImages/");
                                    objModel.Audio.SaveAs(filepath + objModel.CourseId.ToString() + "_audio_" + objModel.Audio.FileName);
                                }
                                if (objModel.Video != null)
                                {
                                    string filepath = Server.MapPath("../../CourseImages/");
                                    objModel.Video.SaveAs(filepath + objModel.CourseId.ToString() + "_video_" + objModel.Video.FileName);
                                }
                            }
                            return RedirectToAction("CourseList", "Courses");
                        }
                    }
                }
                else
                {
                    if(objModel.CourseType=="grammer")
                    {
                    json = "{\"courseName\":\"" + objModel.CourseName + "\",\"courseDescription\":\"" + objModel.CourseDescription + "\",\"courseType\":\"" + objModel.CourseType + "\",\"duration\":\"" + objModel.Duration + "\",\"rewardPoints\":\"" + objModel.RewardPoints + "\",\"price\":\"" + objModel.Price + "\",\"imageUrl\":\"\",\"audioUrl\":\"" + objModel.Video.FileName + "\",\"maxScore\":\"" + objModel.MaxScore + "\",\"isFree\":\"" + objModel.IsFree + "\",\"unit\":\"" + objModel.Unit + "\"}";
                    }
                    if(objModel.CourseType=="vocabulary")
                    {
                    json = "{\"courseName\":\"" + objModel.CourseName + "\",\"courseDescription\":\"" + objModel.CourseDescription + "\",\"courseType\":\"" + objModel.CourseType + "\",\"duration\":\"" + objModel.Duration + "\",\"rewardPoints\":\"" + objModel.RewardPoints + "\",\"price\":\"" + objModel.Price + "\",\"imageUrl\":\""+objModel.Image.FileName+"\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"maxScore\":\"" + objModel.MaxScore + "\",\"isFree\":\"" + objModel.IsFree + "\",\"unit\":\"" + objModel.Unit + "\"}";
                    }
                    if (objModel.CourseType == "dialog")
                    {
                    json = "{\"courseName\":\"" + objModel.CourseName + "\",\"courseDescription\":\"" + objModel.CourseDescription + "\",\"courseType\":\"" + objModel.CourseType + "\",\"duration\":\"" + objModel.Duration + "\",\"rewardPoints\":\"" + objModel.RewardPoints + "\",\"price\":\"" + objModel.Price + "\",\"imageUrl\":\"\",\"audioUrl\":\"" + objModel.Video.FileName + "\",\"maxScore\":\"" + objModel.MaxScore + "\",\"isFree\":\"" + objModel.IsFree + "\",\"unit\":\"" + objModel.Unit + "\"}";
                    }
                    
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AddCourse");
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
                            CourseModel cmodel = new CourseModel();

                            // string tabledata = table.Rows[0]["Message"].ToString();
                            if (table.Rows[0][0].ToString() == "Please choose a different course name ! This one is already in use")
                            {
                                ViewBag.tabledata = table.Rows[0][0].ToString();
                                return View();
                            }
                            id = Convert.ToInt32(table.Rows[0][0].ToString());
                            if (objModel.Image != null)
                            {
                                string filepath = Server.MapPath("../../CourseImages/");
                                objModel.Image.SaveAs(filepath + id.ToString() + "_image_" + objModel.Image.FileName);
                            }
                            if (objModel.Audio != null)
                            {
                                string filepath = Server.MapPath("../../CourseImages/");
                                objModel.Audio.SaveAs(filepath + id.ToString() + "_audio_" + objModel.Audio.FileName);
                            }
                            if (objModel.Video != null)
                            {
                                string filepath = Server.MapPath("../../CourseImages/");
                                objModel.Video.SaveAs(filepath + id.ToString() + "_video_" + objModel.Video.FileName);
                            }
                        }
                        return RedirectToAction("CourseList", "Courses", new { UnitId = objModel.CourseId });

                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public JsonResult ManageCourseStatus(int CourseId, string softDelete)
        {
            try
            {
                string json = "{\"courseId\":\"" + CourseId + "\",\"softDelete\":\"" + softDelete + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/DeleteCourse");
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
                        return Json(table.Rows[0][0].ToString(), JsonRequestBehavior.AllowGet);
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
