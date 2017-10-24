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

namespace iCanSpeakAdminPortal.Controllers
{
    public class SubAdminController : Controller
    {
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(SubAdminModel obj)
        {
            string filepath = string.Empty;
            try
            {
                if (obj.Tutortype == "subadmin")
                {
                    string json = "{\"email\":\"" + obj.Email + "\",\"DOB\":\"" + obj.DOB + "\",\"firstName\":\"" + obj.FirstName + "\",\"lastName\":\"" + obj.LastName + "\",\"city\":\"" + obj.City + "\",\"zipcode\":\"" + obj.ZipCode + "\",\"password\":\"" + obj.Password + "\",\"gender\":\"" + obj.Gender + "\",\"roleId\":\"2\",\"contactNo\":\"" + obj.ContactNo + "\",\"education\":\"" + obj.Education + "\",\"experience\":\"" + obj.Experience + "\",\"expertise\":\"" + obj.Expertise + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/CreateSubAdmin");
                    //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/CreateSubAdmin");
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
                            int userid = Convert.ToInt32(result);
                            if (obj.Image != null)
                            {
                                filepath = Server.MapPath("../../ProfilePictures/");
                                obj.Image.SaveAs(filepath + userid.ToString() + "_ProfilePic.jpg");
                            }
                            return RedirectToAction("UserList", "SubAdmin");
                        }
                    }
                }
                else
                {
                    string roleid = "";
                    if (obj.Tutortype == "tutor")
                    {
                        roleid = "3";
                    }
                    else
                    {
                        roleid = "6";
                    }
                    string json = "{\"email\":\"" + obj.Email + "\",\"DOB\":\"" + obj.DOB + "\",\"firstName\":\"" + obj.FirstName + "\",\"lastName\":\"" + obj.LastName + "\",\"city\":\"" + obj.City + "\",\"zipcode\":\"" + obj.ZipCode + "\", \"password\":\"" + obj.Password + "\",\"gender\":\"" + obj.Gender + "\",\"roleId\":\"" + roleid + "\",\"contactNo\":\"" + obj.ContactNo + "\",\"education\":\"" + obj.Education + "\",\"experience\":\"" + obj.Experience + "\",\"expertise\":\"" + obj.Expertise + "\",\"Description\":\"" + obj.Description + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/CreateTutorSubTutor");
                    //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/CreateTutorSubTutor");
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
                            int userid = Convert.ToInt32(result);
                            if (obj.Image != null)
                            {
                                filepath = Server.MapPath("../../ProfilePictures/");
                                obj.Image.SaveAs(filepath + userid.ToString() + "_ProfilePic.jpg");
                            }

                            return RedirectToAction("UserList", "SubAdmin");
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
        public ActionResult UpdateProfile(int userid = 0)
        {
            try
            {
                if (userid == 0)
                {
                    return RedirectToAction("UserList", "SubAdmin");
                }
                else
                {
                    string json = "{\"userid\":\"" + userid.ToString() + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetSubAdminByUserId");
                    //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetSubAdminByUserId");

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
                            //JArray jsonArray = JArray.Parse(result);
                            //List<VocabularyModel> vocab = jsonArray.ToObject<List<VocabularyModel>>();
                            SubAdminModel objModel = new SubAdminModel();

                            objModel.FirstName = table.Rows[0]["FirstName"].ToString();
                            objModel.LastName = table.Rows[0]["LastName"].ToString();

                            objModel.Email = table.Rows[0]["Email"].ToString();
                            objModel.City = table.Rows[0]["City"].ToString();

                            objModel.ContactNo = table.Rows[0]["ContactNo"].ToString();
                            DateTime DateCNG = Convert.ToDateTime(table.Rows[0]["DOB"].ToString());
                            objModel.DOB = DateCNG.ToString("yyyy-MM-dd");
                            objModel.Gender = table.Rows[0]["Gender"].ToString();
                            objModel.Password = table.Rows[0]["Password"].ToString();
                            objModel.State = table.Rows[0]["State"].ToString();
                            objModel.ZipCode = table.Rows[0]["zipCode"].ToString();

                            objModel.RoleId = Convert.ToInt32(table.Rows[0]["RoleId"].ToString());
                            objModel.UserId = userid;
                            objModel.Education = table.Rows[0]["Education"].ToString();
                            objModel.Experience = table.Rows[0]["Experience"].ToString();
                            objModel.Expertise = table.Rows[0]["Expertise"].ToString();

                            return View(objModel);
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
        public JsonResult SaveUpdateProfile(string userid, string usertype, string firstName, string lastName, string emailId, string password, string dob, string education, string experience, string expertise, string contactNo, string city, string state, string zipCode, string gender)
        {
            try
            {
                string roleid = string.Empty;
                if (usertype == "subadmin")
                    roleid = "2";
                if (usertype == "tutor")
                    roleid = "3";
                if (usertype == "subtutor")
                    roleid = "6";
                string json = "{\"userid\":\"" + userid + "\",\"DOB\":\"" + dob + "\",\"firstName\":\"" + firstName + "\",\"lastName\":\"" + lastName + "\",\"password\":\"" + password + "\",\"gender\":\"" + gender + "\",\"profilePicture\":\"\",\"roleId\":\"" + roleid + "\",\"contactNo\":\"" + contactNo + "\",\"education\":\"" + education + "\",\"experience\":\"" + experience + "\",\"expertise\":\"" + expertise + "\",\"zipcode\":\"" + zipCode + "\",\"email\":\"" + emailId + "\",\"city\":\"" + city + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/UpdateSubAdmin");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/UpdateSubAdmin");
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
                List<UsersListModel> users = new List<UsersListModel>();
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
        }



        [Authorize]
        public JsonResult CreateSubAdmin(string firstName, string lastName, string emailId, string password, string dob, string contactNo, string city, string state, string zipCode, string gender, string profilepicture)
        {
            try
            {
                profilepicture = profilepicture.Replace("data:image/jpeg;base64,", "").Replace("data:image/png;base64,", "").Replace("data:image/jpg;base64,", "");
                string json = "{\"email\":\"" + emailId + "\",\"DOB\":\"" + dob + "\",\"firstName\":\"" + firstName + "\",\"lastName\":\"" + lastName + "\",\"city\":\"" + city + "\",\"state\":\"" + state + "\",\"zipCode\":\"" + zipCode + "\",\"password\":\"" + password + "\",\"gender\":\"" + gender + "\",\"roleId\":\"2\",\"contactNo\":\"" + contactNo + "\",\"education\":\"\",\"experience\":\"\",\"expertise\":\"\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/CreateSubAdmin");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/CreateSubAdmin");
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
                        int userid = Convert.ToInt32(result);
                        if (!string.IsNullOrEmpty(profilepicture))
                        {
                            string filepath = Server.MapPath("../../ProfilePictures/" + userid.ToString() + "_ProfilePic.jpg");
                            var bytes = Convert.FromBase64String(profilepicture);
                            MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
                            ms.Write(bytes, 0, bytes.Length);
                            Image image = Image.FromStream(ms);
                            // filePath = "http://lla.techvalens.net/Services/ProfilePictures/" + userId + "_ProfilePic.jpg";
                            image.Save(filepath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }

                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult CreateTutor(string firstName, string lastName, string emailId, string password, string dob, string education, string experience, string expertise, string contactNo, string city, string state, string zipCode, string gender, string profilepicture)
        {
            try
            {
                profilepicture = profilepicture.Replace("data:image/jpeg;base64,", "").Replace("data:image/png;base64,", "").Replace("data:image/jpg;base64,", "");
                string json = "{\"email\":\"" + emailId + "\",\"DOB\":\"" + dob + "\",\"firstName\":\"" + firstName + "\",\"lastName\":\"" + lastName + "\",\"password\":\"" + password + "\",\"city\":\"" + city + "\",\"state\":\"" + state + "\",\"zipCode\":\"" + zipCode + "\",\"gender\":\"" + gender + "\",\"profilePicture\":\"" + profilepicture + "\",\"roleId\":\"3\",\"contactNo\":\"" + contactNo + "\",\"education\":\"" + education + "\",\"experience\":\"" + experience + "\",\"expertise\":\"" + expertise + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/CreateTutorSubTutor");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/CreateTutorSubTutor");
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
                        int userid = Convert.ToInt32(result);
                        if (!string.IsNullOrEmpty(profilepicture))
                        {
                            string filepath = Server.MapPath("../../ProfilePictures/" + userid.ToString() + "_ProfilePic.jpg");
                            var bytes = Convert.FromBase64String(profilepicture);
                            MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
                            ms.Write(bytes, 0, bytes.Length);
                            Image image = Image.FromStream(ms);
                            // filePath = "http://lla.techvalens.net//ProfilePictures/" + userId + "_ProfilePic.jpg";
                            image.Save(filepath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }

                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                List<UsersListModel> users = new List<UsersListModel>();
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult UserList()
        {
            try
            {
                string json = "{\"LoginUserId\":\"7\",\"AccessToken\":\"i4vxvc\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetAllSubAdminUsersList");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/GetAllSubAdminUsersList");
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
                        List<SubAdminModel> users = jsonArray.ToObject<List<SubAdminModel>>();
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
        public ActionResult ManageSubAdminStatus(string userId, string status)
        {
            try
            {
                string json = "{\"userId\":\"" + Convert.ToInt32(userId) + "\",\"softDelete\":\"" + status + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/Services/Service.svc/DeleteSubAdmin");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/DeleteSubAdmin");
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
        public JsonResult DeleteUserById(string userid)
        {
            try
            {
                string json = "{\"userid\":\"" + userid + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/DeleteSubUserById");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/DeleteSubUserById");
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
        public ActionResult SetAccess(int userid = 0)
        {
            try
            {
                if (userid == 0)
                {
                    return RedirectToAction("UserList", "SubAdmin");
                }

                string jsonrequest = "";//"{\"LoginUserId\":\"7\",\"AccessToken\":\"i4vxvc\"}";
                var dataRequest = javaScriptSerializer.DeserializeObject(jsonrequest);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetAllMenu");
                string sb1 = JsonConvert.SerializeObject(dataRequest);
                request.Method = "POST";
                Byte[] bt1 = Encoding.UTF8.GetBytes(sb1);
                Stream st1 = request.GetRequestStream();
                st1.Write(bt1, 0, bt1.Length);
                st1.Close();
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
                        List<SubAdminAccessPermissionDisplayNamesModel> users = jsonArray.ToObject<List<SubAdminAccessPermissionDisplayNamesModel>>();

                        ViewBag.Menus = users;
                    }
                }

                string json = "{\"LoginUserId\":\"7\",\"AccessToken\":\"i4vxvc\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetAllSubAdminUsersList");
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
                        List<SubAdminAccessPermissionModel> users = jsonArray.ToObject<List<SubAdminAccessPermissionModel>>();
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
        public JsonResult SaveAccessRights(string userid, string menuids)
        {
            try
            {
                if (menuids.Length > 0)
                {
                    if (menuids.Contains(",,"))
                    {
                        menuids = menuids.Replace(",,", "");
                    }
                    else
                    {
                        menuids = menuids.Remove(0, 1);
                    }
                }

                string json = "{\"userId\":\"" + userid + "\",\"menuId\":\"" + menuids + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/UserRoleMapping");
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
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult GetUserAccessRights(int userid)
        {
            try
            {
                string json = "{\"userId\":\"" + userid + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/GetMenuByUserId");
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
                        List<SubAdminAccessPermissionDisplayNamesModel> users = jsonArray.ToObject<List<SubAdminAccessPermissionDisplayNamesModel>>();
                        return Json(users, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
