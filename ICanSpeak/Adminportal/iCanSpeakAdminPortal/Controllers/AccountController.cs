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
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        HttpWebRequest request;
        HttpWebResponse response;


        public ActionResult Tests()
        {
            return View();
        }

        public ActionResult Login()
        {
            string url = Request.Url.ToString();
            if (url.Contains("ReturnUrl"))
            {
                Session["url"] = url;
                return View("_login");
            }
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                if (Convert.ToString(FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name) != "")
                {
                    return RedirectToAction("Dashboard", "Account");
                }
            }
            LoginModel obj = new LoginModel();
            if (Request.Cookies["iCanSpeakCookies"] != null)
            {
                HttpCookie getCookie = Request.Cookies["iCanSpeakCookies"];
                obj.Email = Convert.ToString(getCookie.Values["Email"]);
                obj.Password = Convert.ToString(getCookie.Values["Password"]);
                obj.RememberMe = true;
                return View(obj);
            }
            return View(obj);
        }

        [HttpPost]
        public ActionResult Login(LoginModel objModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (objModel.RememberMe == true)
                    {
                        Response.Cookies["iCanSpeakCookies"]["Email"] = objModel.Email;
                        Response.Cookies["iCanSpeakCookies"]["Password"] = objModel.Password;
                        Response.Cookies["iCanSpeakCookies"].Expires = DateTime.Now.AddDays(28);
                    }
                    else
                    {
                        Response.Cookies["iCanSpeakCookies"].Expires = DateTime.Now.AddDays(-1);
                    }

                    string json = "{\"email\":\"" + objModel.Email + "\",\"password\":\"" + objModel.Password + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/AdminLogin");
                    //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/AdminLogin");
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
                            string charr = result[0].ToString();
                            if (charr != "[")
                            {
                                result = "[" + result + "]";
                            }
                            var table = JsonConvert.DeserializeObject<DataTable>(result);

                            if (table.Rows[0][0].ToString() == "Invalid email" || table.Rows[0][0].ToString() == "Invalid password" || table.Rows[0][0].ToString() == "Account inactive contact to admin")
                            {
                                ViewBag.Result = table.Rows[0][0].ToString();
                                LoginModel objLoginModel = new LoginModel();
                                return View(objLoginModel);
                            }
                            else
                            {
                                FormsAuthentication.SetAuthCookie(table.Rows[0][0].ToString(), true);
                                //Session["userid"] = table.Rows[0][0].ToString();
                                Session["email"] = table.Rows[0]["Email"].ToString();

                                if (objModel.ReturnUrl != null)
                                {
                                    // string[] path = objModel.ReturnUrl.Split('/');
                                    //return RedirectToAction(path[2], path[1]);
                                    return Redirect(objModel.ReturnUrl);
                                }
                                else
                                {
                                    return RedirectToAction("Dashboard", "Account");
                                }
                            }
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
                ViewBag.Result = ex.Message;
                return View();
            }
        }

        public ActionResult AuthenticateUser(string email, string password)
        {
            try
            {
                string json = "{\"email\":\"rahul@techvalens.com\",\"password\":\"h\"}";// "{\"vocabularyId\":\"" + objModel.VocabularyId + "\",\"text\":\"" + objModel.Text + "\",\"imageUrl\":\"" + objModel.Image.FileName + "\",\"audioUrl\":\"" + objModel.Audio.FileName + "\",\"sampleSentance\":\"" + objModel.SampleSentance + "\",\"arabicText\":\"" + objModel.ArabicText + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/Login");
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
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                // List<UsersListModel> users = new List<UsersListModel>();
                return View();
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordModel objModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string json = "{\"email\":\"" + objModel.Email + "\"}";
                    var data = javaScriptSerializer.DeserializeObject(json);
                    request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/ForgotPasswordAdmin");
                    //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/ForgotPasswordAdmin");
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
                            var table = JsonConvert.DeserializeObject<DataTable>(sr.ReadToEnd());
                            if (table.Rows[0][0].ToString().ToLower().Contains("password sent"))
                            {
                                ViewBag.Result = "success";// "New password successfully sent on given email id, please check your mail box for new password";
                            }
                            else
                            {
                                ViewBag.Result = table.Rows[0][0].ToString();
                            }
                            return View();
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
                ViewBag.Result = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult UserMenus()
        {
            try
            {
                string userid = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
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
                        return View(users);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Result = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            try
            {
                string json = "";// "{\"userId\":\"" + Convert.ToString(Session["userid"]) + "\",\"currentPassword\":\"" + objModel.CurrentPassword + "\",\"newPassword\":\"" + objModel.NewPassword + "\",\"confirmPassword\":\"" + objModel.ConfirmPassword + "\"}";
                var data = javaScriptSerializer.DeserializeObject(json);
                request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/DashboardData");
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc/DashboardData");
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
                        DashBoardModel objModel = new DashBoardModel();
                        //For User
                        objModel.userCount = Convert.ToInt32(table.Rows[0]["userCount"]);
                        objModel.activeUserCount = Convert.ToInt32(table.Rows[0]["activeUserCount"]);
                        objModel.inactiveUserCount = Convert.ToInt32(table.Rows[0]["inactiveUserCount"]);
                        //For Course
                        objModel.totalCourseCount = Convert.ToInt32(table.Rows[1]["totalCourseCount"]);
                        objModel.activeCourseCount = Convert.ToInt32(table.Rows[1]["activeCourseCount"]);
                        objModel.inactiveCourseCount = Convert.ToInt32(table.Rows[1]["inactiveCourseCount"]);
                        objModel.totalCoursePaidCount = Convert.ToInt32(table.Rows[1]["totalCoursePaidCount"]);
                        objModel.totalCourseFreeCount = Convert.ToInt32(table.Rows[1]["totalCourseFreeCount"]);
                        //For Payment
                        objModel.totalPaymentCount = Convert.ToInt32(table.Rows[2]["totalPaymentCount"]);
                        objModel.totalPaidAmount = Convert.ToInt32(table.Rows[2]["totalPaidAmount"]);
                        //For Tutor
                        objModel.totaltutorcount = Convert.ToInt32(table.Rows[3]["totaltutorcount"]);
                        objModel.activetutorCount = Convert.ToInt32(table.Rows[3]["activetutorCount"]);
                        objModel.inactivetutorCount = Convert.ToInt32(table.Rows[3]["inactivetutorCount"]);
                        //For Vocabulary 
                        objModel.totalVocabCount = Convert.ToInt32(table.Rows[4]["totalVocabCount"]);
                        objModel.activeVocabCount = Convert.ToInt32(table.Rows[4]["activeVocabCount"]);
                        objModel.inactiveVocabCount = Convert.ToInt32(table.Rows[4]["inactiveVocabCount"]);
                        objModel.totalVocabPaidCount = Convert.ToInt32(table.Rows[4]["totalVocabPaidCount"]);
                        objModel.totalVocabFreeCount = Convert.ToInt32(table.Rows[4]["totalVocabFreeCount"]);
                        //For Vocabulary Sub Category
                        objModel.totalVocabSubCount = Convert.ToInt32(table.Rows[5]["totalVocabSubCount"]);
                        objModel.activeVocabSubCount = Convert.ToInt32(table.Rows[5]["activeVocabSubCount"]);
                        objModel.inactiveVocabSubCount = Convert.ToInt32(table.Rows[5]["inactiveVocabSubCount"]);
                        //For Vocabulary WordList
                        objModel.totalWordCount = Convert.ToInt32(table.Rows[6]["totalWordCount"]);
                        objModel.activeWordCount = Convert.ToInt32(table.Rows[6]["activeWordCount"]);
                        objModel.inactiveWordCount = Convert.ToInt32(table.Rows[6]["inactiveWordCount"]);
                        //For Vocabulary Question
                        objModel.totalVocabQueCount = Convert.ToInt32(table.Rows[7]["totalVocabQueCount"]);
                        objModel.activeVocabQueCount = Convert.ToInt32(table.Rows[7]["activeVocabQueCount"]);
                        objModel.inactiveVocabQueCount = Convert.ToInt32(table.Rows[7]["inactiveVocabQueCount"]);
                        //For Grammer Unit 
                        objModel.totalGrammerCount = Convert.ToInt32(table.Rows[8]["totalGrammerCount"]);
                        objModel.activeGrammerCount = Convert.ToInt32(table.Rows[8]["activeGrammerCount"]);
                        objModel.inactiveGrammerCount = Convert.ToInt32(table.Rows[8]["inactiveGrammerCount"]);
                        objModel.totalGrammerPaidCount = Convert.ToInt32(table.Rows[8]["totalGrammerPaidCount"]);
                        objModel.totalGrammerFreeCount = Convert.ToInt32(table.Rows[8]["totalGrammerFreeCount"]);
                        //For Grammer Assisment Question
                        objModel.totalGrammerQueCount = Convert.ToInt32(table.Rows[9]["totalGrammerQueCount"]);
                        objModel.activeGrammerQueCount = Convert.ToInt32(table.Rows[9]["activeGrammerQueCount"]);
                        objModel.inactiveGrammerQueCount = Convert.ToInt32(table.Rows[9]["inactiveGrammerQueCount"]);
                        //For Dialog 
                        objModel.totalDialogCount = Convert.ToInt32(table.Rows[10]["totalDialogCount"]);
                        objModel.activeDialogCount = Convert.ToInt32(table.Rows[10]["activeDialogCount"]);
                        objModel.inactiveDialogCount = Convert.ToInt32(table.Rows[10]["inactiveDialogCount"]);
                        objModel.totalDialogPaidCount = Convert.ToInt32(table.Rows[10]["totalDialogPaidCount"]);
                        objModel.totalDialogFreeCount = Convert.ToInt32(table.Rows[10]["totalDialogFreeCount"]);
                        //For Dialog Key Phrases
                        objModel.totalKeyPhrasesCount = Convert.ToInt32(table.Rows[11]["totalKeyPhrasesCount"]);
                        objModel.activeKeyPhrasesCount = Convert.ToInt32(table.Rows[11]["activeKeyPhrasesCount"]);
                        objModel.inactiveKeyPhrasesCount = Convert.ToInt32(table.Rows[11]["inactiveKeyPhrasesCount"]);
                        //For Dialog Conversation
                        objModel.totalConversationCount = Convert.ToInt32(table.Rows[12]["totalConversationCount"]);
                        objModel.activeConversationCount = Convert.ToInt32(table.Rows[12]["activeConversationCount"]);
                        objModel.inactiveConversationCount = Convert.ToInt32(table.Rows[12]["inactiveConversationCount"]);
                        //For Dialog Assessment Question 
                        objModel.totalDialogAssiQueCount = Convert.ToInt32(table.Rows[13]["totalDialogAssiQueCount"]);
                        objModel.activeDialogAssiQueCount = Convert.ToInt32(table.Rows[13]["activeDialogAssiQueCount"]);
                        objModel.inactiveDialogAssiQueCount = Convert.ToInt32(table.Rows[13]["inactiveDialogAssiQueCount"]);
                        return View(objModel);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Result = ex.Message;
                return View("Error");
            }
        }


        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel objModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (objModel.ConfirmPassword == objModel.NewPassword)
                    {
                        string json = "{\"Email\":\"" + objModel.Email + "\",\"Password\":\"" + objModel.CurrentPassword + "\",\"NewPassword\":\"" + objModel.NewPassword + "\"}";
                        var data = javaScriptSerializer.DeserializeObject(json);
                        request = (HttpWebRequest)WebRequest.Create("http://lla.techvalens.net/services/Service.svc/ChangePasswordAdmin");
                        //request = (HttpWebRequest)WebRequest.Create("http://localhost:31017/Service.svc//ChangePasswordAdmin");
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

                                ViewBag.Result = table.Rows[0][0].ToString();

                                return View();
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Result = "New Password and Confirm Password field must be the same.";
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Result = ex.Message;
                return View("Error");
            }
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            //Session["email"] = null;
            return RedirectToAction("Login", "Account");
        }

    }
}
