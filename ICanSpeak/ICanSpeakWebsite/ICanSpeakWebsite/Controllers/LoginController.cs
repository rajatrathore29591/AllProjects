using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICanSpeakWebsite.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using ICanSpeakWebsite.App_Start;
using System.Data;
using System.Xml;
using System.Web.Caching;

namespace ICanSpeakWebsite.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /StartUp/
        DataSet jsonDataSet = new DataSet();
        HelperClass objhelperclass = new HelperClass();
        string jsonstring = string.Empty;
        string resultjson = string.Empty;



        public ActionResult Login()
        {
            if (Session["UserId"] != null)
            {
                return RedirectToAction("MyCourse", "Home");
            }
            return View();
        }

        [HttpPost]

        public ActionResult Login(Login objlogin)
         {
            try
            {
                jsonstring = "{\"email\":\"" + objlogin.Email + "\",\"password\":\"" + objlogin.Password + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "Login");
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                  //  Cache obj = new Cache();
                    System.Web.HttpRuntime.Cache["Username"] = jsonDataSet.Tables["Profile"].Rows[0]["Username"].ToString();
                    System.Web.HttpRuntime.Cache["Country"] = jsonDataSet.Tables["Profile"].Rows[0]["Country"].ToString();
                    System.Web.HttpRuntime.Cache["ProfilePicture"] = jsonDataSet.Tables["Profile"].Rows[0]["ProfilePicture"].ToString();
                    Session["UserId"] = jsonDataSet.Tables["Profile"].Rows[0]["UserId"].ToString();
                    
                    System.Web.HttpRuntime.Cache["GrammerBookmark"] = jsonDataSet.Tables["Profile"].Rows[0]["GrammerBookmark"].ToString();
                    System.Web.HttpRuntime.Cache["VocabularyBookmark"] = jsonDataSet.Tables["Profile"].Rows[0]["VocabularyBookmark"].ToString();
                    System.Web.HttpRuntime.Cache["DialogBookmark"] = jsonDataSet.Tables["Profile"].Rows[0]["DialogBookmark"].ToString();
                    System.Web.HttpRuntime.Cache["BatchCount"] = jsonDataSet.Tables["Profile"].Rows[0]["BatchCount"].ToString();
                    System.Web.HttpRuntime.Cache["VocabId"] = jsonDataSet.Tables["Profile"].Rows[0]["VocabId"].ToString();
                    System.Web.HttpRuntime.Cache["VocabSubId"] = jsonDataSet.Tables["Profile"].Rows[0]["VocabSubId"].ToString();

                    return RedirectToAction("MyCourse", "Home");
                }
                else
                {
                    LoginError objloginerror = new LoginError();
                    objloginerror.Errormessage = jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString();
                    return RedirectToAction("LoginError", "Login", objloginerror);
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("LoginError", "Login");
            }
            //return View();
        }


        public ActionResult ForgotPassword()
        {
            //if (Session["UserId"] == null)
            //{
            //    return RedirectToAction("Login", "Login");
            //}
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPassword objModel)
        {
            //if (Session["UserId"] == null)
            //{
            //    return RedirectToAction("Login", "Login");
            //}
            try
            {
                jsonstring = "{\"email\":\"" + objModel.email+ "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "ForgotPassword");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));
                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "Password sent")
                {
                    ViewBag.Message = 1;
                }
                else
                {
                    ViewBag.Message = 0;
                }

                return View();

            }
            catch (Exception ex)
            {
                return RedirectToAction("LoginError", "Login");

            }
        }
      



        //------------------------------Old Codes Login Profile----------------------------------------------------------------------\\
        //[HttpPost]
        //public ActionResult Login(Login objlogin)
        //{

        //    try
        //    {
        //        jsonstring = "{\"email\":\""+objlogin.Email+"\",\"password\":\""+objlogin.Password+"\"}";
        //        resultjson = objhelperclass.callservice(jsonstring, "Login");
        //        string pp = resultjson[0].ToString();
        //        //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

        //        XmlDocument xd1 = new XmlDocument();
        //        xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson,"root");
        //        jsonDataSet.ReadXml(new XmlNodeReader(xd1));

        //        if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
        //        {
        //            Session["loginEmail"] = objlogin.Email;
        //            UserProfileDetails modeluserprofile = new UserProfileDetails();
        //            modeluserprofile.UserId = Convert.ToInt32(jsonDataSet.Tables["Profile"].Rows[0]["UserId"].ToString());
        //            modeluserprofile.Username = jsonDataSet.Tables["Profile"].Rows[0]["Username"].ToString();
        //            modeluserprofile.AboutMeUrl = jsonDataSet.Tables["Profile"].Rows[0]["AboutMe"].ToString();
        //            modeluserprofile.Country = jsonDataSet.Tables["Profile"].Rows[0]["Country"].ToString();
        //            modeluserprofile.CourseDaysLeft = jsonDataSet.Tables["Profile"].Rows[0]["CourseDaysLeft"].ToString();
        //            modeluserprofile.CourseDuration = jsonDataSet.Tables["Profile"].Rows[0]["CourseDuration"].ToString();
        //            modeluserprofile.CourseStartDate = jsonDataSet.Tables["Profile"].Rows[0]["CourseStartDate"].ToString();
        //            modeluserprofile.CourseType = jsonDataSet.Tables["Profile"].Rows[0]["CourseType"].ToString();
        //            modeluserprofile.DOB = jsonDataSet.Tables["Profile"].Rows[0]["DOB"].ToString();
        //            modeluserprofile.Email = jsonDataSet.Tables["Profile"].Rows[0]["Email"].ToString();
        //            modeluserprofile.Gender = jsonDataSet.Tables["Profile"].Rows[0]["Gender"].ToString();
        //            modeluserprofile.NotificationCount = jsonDataSet.Tables["Profile"].Rows[0]["NotificationCount"].ToString();
        //            modeluserprofile.ProfilePicture = jsonDataSet.Tables["Profile"].Rows[0]["ProfilePicture"].ToString();

                   
        //            return RedirectToAction("UserProfile", "UserProfile",modeluserprofile);
        //        }
        //        else
        //        {
        //            LoginError objloginerror = new LoginError();
        //            objloginerror.Errormessage = jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString();
        //            return RedirectToAction("LoginError", "Login",objloginerror);
        //        }
                
        //    }
        //    catch(Exception ex)
        //    {
        //        return RedirectToAction("LoginError", "Login");
        //    }
        //    //return View();
        //}
        //-----------------------------------------------------------------------------------------------------------------------------//
        public ActionResult LoginError(LoginError objloginerror)
        {
            ViewBag.ErrorMessage = objloginerror.Errormessage;
            return View();
        }

        [HttpPost]
        public ActionResult LoginError(Login objlogin)
        {

            try
            {
                jsonstring = "{\"email\":\"" + objlogin.Email + "\",\"password\":\"" + objlogin.Password + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "Login");
                string pp = resultjson[0].ToString();
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    LoginError objloginerror = new LoginError();
                    objloginerror.Errormessage = jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString();
                    return RedirectToAction("LoginError", "Login", objloginerror);
                }

            }
            catch (Exception ex)
            {

            }
            return View();
        }

       
        //public JsonResult ForgotPassword(string email)
        //{
        //    try
        //    {
        //        jsonstring = "{\"Email\":\"" + email + "\"}";
        //        resultjson = objhelperclass.callservice(jsonstring, "GetUserByEmail");
        //        string pp = resultjson[0].ToString();
        //        //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);
        //        if (resultjson != null)
        //        {
        //            return Json(resultjson, JsonRequestBehavior.AllowGet);
        //        }
        //        else {
        //            return Json("Email Id not Exist", JsonRequestBehavior.AllowGet);
        //        }
               
                

        //    }
        //    catch (Exception ex)
        //    {
        //      //  return RedirectToAction("LoginError", "Login");
        //        return Json("Please check your email for password.", JsonRequestBehavior.AllowGet);//GetUserByEmail
        //    }

         
        //}

        public ActionResult Test()
        {
            return View();
        }
    }
}
