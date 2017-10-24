using ICanSpeakWebsite.Models;
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
using ICanSpeakWebsite.Models;
using ICanSpeakWebsite.App_Start;
using System.Xml;

namespace ICanSpeakWebsite.Controllers
{
    public class UserProfileController : Controller
    {
        //
        // GET: /UserProfile/
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        DataSet jsonDataSet = new DataSet();
        HelperClass objhelperclass = new HelperClass();
        string jsonstring = string.Empty;
        string resultjson = string.Empty;



        public ActionResult Profile()
        {
            return View();
        }

        
        
        public ActionResult UserProfile()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LoginError", "Login");
            }
            try
            {
                jsonstring = "{\"UserId\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetUserProfile");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    
                    UserProfileDetails modeluserprofile = new UserProfileDetails();
                    modeluserprofile.UserId = Convert.ToInt32(jsonDataSet.Tables["Profile"].Rows[0]["UserId"].ToString());
                    modeluserprofile.Username = jsonDataSet.Tables["Profile"].Rows[0]["Username"].ToString();
                    modeluserprofile.AboutMeUrl = jsonDataSet.Tables["Profile"].Rows[0]["AboutMe"].ToString();
                    modeluserprofile.Country = jsonDataSet.Tables["Profile"].Rows[0]["Country"].ToString();
                    modeluserprofile.CourseDaysLeft = jsonDataSet.Tables["Profile"].Rows[0]["CourseDaysLeft"].ToString();
                    modeluserprofile.CourseDuration = jsonDataSet.Tables["Profile"].Rows[0]["CourseDuration"].ToString();
                    modeluserprofile.CourseStartDate = jsonDataSet.Tables["Profile"].Rows[0]["CourseStartDate"].ToString();
                    modeluserprofile.CourseType = jsonDataSet.Tables["Profile"].Rows[0]["CourseType"].ToString();
                    modeluserprofile.DOB = jsonDataSet.Tables["Profile"].Rows[0]["DOB"].ToString();
                    modeluserprofile.Email = jsonDataSet.Tables["Profile"].Rows[0]["Email"].ToString();
                    modeluserprofile.Gender = jsonDataSet.Tables["Profile"].Rows[0]["Gender"].ToString();
                    modeluserprofile.NotificationCount = jsonDataSet.Tables["Profile"].Rows[0]["NotificationCount"].ToString();
                    modeluserprofile.ProfilePicture = jsonDataSet.Tables["Profile"].Rows[0]["ProfilePicture"].ToString();
                    modeluserprofile.Messages = jsonDataSet.Tables["Profile"].Rows[0]["Messages"].ToString();

                    return View(modeluserprofile);
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
                ViewBag.ErrorName = ex.Message;
                return View("Error");
            }

        }

        public ActionResult UpdatesProfile()
        {
            try
            {
                jsonstring = "{\"UserId\":\"" + Session["UserId"] + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "GetUserProfileEdit");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    EditProfile objeditprofile = new EditProfile();
                    objeditprofile.FirstName = jsonDataSet.Tables["Profile"].Rows[0]["FirstName"].ToString();
                    objeditprofile.LastName = jsonDataSet.Tables["Profile"].Rows[0]["LastName"].ToString();
                    objeditprofile.Country = jsonDataSet.Tables["Profile"].Rows[0]["Country"].ToString();
                    objeditprofile.AboutMe = jsonDataSet.Tables["Profile"].Rows[0]["AboutMe"].ToString();
                    objeditprofile.ProfilePicture = jsonDataSet.Tables["Profile"].Rows[0]["ProfilePicture"].ToString();
                    objeditprofile.DOB = jsonDataSet.Tables["Profile"].Rows[0]["DOB"].ToString();
                    objeditprofile.ZipCode = jsonDataSet.Tables["Profile"].Rows[0]["ZipCode"].ToString();
                    return View(objeditprofile);
                }
                else
                {

                    return ViewBag.Error = "An error";
                }

            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [HttpPost]
        public ActionResult UpdateProfile(EditProfile objeditprofile)
        {
           // string userid = "2";//Session["UserId"].ToString();
            try
            {
                string audiobase64 = string.Empty;
                string imagebase64 = string.Empty;
                if(objeditprofile.Audio!=null)
                {
                //----------------------------------Section to convert Audio file to base64-------------------------------------------------------//
                byte[] binaryData;
                binaryData = new Byte[objeditprofile.Audio.InputStream.Length];
                long bytesRead = objeditprofile.Audio.InputStream.Read(binaryData, 0, (int)objeditprofile.Audio.InputStream.Length);
                objeditprofile.Audio.InputStream.Close();
                audiobase64 = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
                //----------------------------------------Audio convert Ends----------------------------------------------------------------------//
                }

                if (objeditprofile.profilepic == null)
                {
                    //----------------------------------Section to convert Image  to base64-------------------------------------------------------//
                    if (objeditprofile.Image != null)
                    {
                        byte[] binaryDatas;
                        binaryDatas = new Byte[objeditprofile.Image.InputStream.Length];
                        long bytesReads = objeditprofile.Image.InputStream.Read(binaryDatas, 0, (int)objeditprofile.Image.InputStream.Length);
                        objeditprofile.Image.InputStream.Close();
                        imagebase64 = System.Convert.ToBase64String(binaryDatas, 0, binaryDatas.Length);
                        //----------------------------------------Image convert Ends----------------------------------------------------------------------//
                    }
                }


                jsonstring = "{\"UserId\":\"" + Session["UserId"] + "\",\"Firstname\":\"" + objeditprofile.FirstName + "\",\"Lastname\":\"" + objeditprofile.LastName + "\",\"DOB\":\"" + objeditprofile.DOB + "\",\"ZipCode\":\"" + objeditprofile.ZipCode + "\",\"Country\":\"" + objeditprofile.Country + "\",\"ProfilePicture\":\"" + objeditprofile.profilepic + "\",\"audiobase64\":\"" + audiobase64 + "\",\"imagebase64\":\"" + imagebase64 + "\"}";
                resultjson = objhelperclass.callservice(jsonstring, "EditUserProfile");
                string pp = resultjson[0].ToString();
                //DataSet ds  = JsonConvert.DeserializeObject<DataSet>(resultjson);

                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                    if (jsonDataSet.Tables["ResultMessage"].Rows[0]["resultmsg"].ToString() == "Profile update successfully !")
                    {
                        return RedirectToAction("UserProfile", "UserProfile");
                    }
                    
                }
                else
                {

                    TempData["Error"] = "An error occured, please try again";
                     return RedirectToAction("EditProfile");
                }

            }
            catch (Exception ex)
            {

            }
            return View();
        }
      

    }
}
