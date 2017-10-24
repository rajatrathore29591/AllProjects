using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using ICanSpeakWebsite.App_Start;
using ICanSpeakWebsite.Models;
using Newtonsoft.Json;
using ICanSpeakWebsite.HelperClasses;
using System.IO;

namespace ICanSpeakWebsite.Controllers
{
    public class RegistrationController : Controller
    {
        //
        // GET: /Registration/
        DataSet jsonDataSet = new DataSet();
        HelperClass objhelperclass = new HelperClass();
        string jsonstring = string.Empty;
        string resultjson = string.Empty;



        public ActionResult UserRegistration()
        {
           // return Redirect("https://www.facebook.com/");
            return View();
        }

        [HttpPost]
        public ActionResult UserRegistration(RegistrationModel objregistration)
        {
            try
            {
                string imagebase64 = string.Empty;
                string audiobase64 = string.Empty;
                byte[] binaryData;
             //----------------------------------Section to convert Audio file to base64-------------------------------------------------------//
                if (objregistration.Audio != null)
                {
                    
                    binaryData = new Byte[objregistration.Audio.InputStream.Length];
                    long bytesRead = objregistration.Audio.InputStream.Read(binaryData, 0, (int)objregistration.Audio.InputStream.Length);
                    objregistration.Audio.InputStream.Close();
                    audiobase64 = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
                }
               //----------------------------------------Audio convert Ends----------------------------------------------------------------------//
                if (objregistration.profilepic == null)
                {
                    //----------------------------------Section to convert Image  to base64-------------------------------------------------------//
                    if (objregistration.Image != null)
                    {
                        binaryData = new Byte[objregistration.Image.InputStream.Length];
                        long bytesReads = objregistration.Image.InputStream.Read(binaryData, 0, (int)objregistration.Image.InputStream.Length);
                        objregistration.Image.InputStream.Close();
                        imagebase64 = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
                    }
                    else
                    {
                        objregistration.profilepic = "default_pro_pic_large.png";
                    }
                  //----------------------------------------Image convert Ends----------------------------------------------------------------------//
                }
                string Gender = String.Empty;
                if (objregistration.Gender == true)
                {
                    Gender = "Male";
                }
                else
                {
                    Gender = "Female";
                }
               //jsonstring = "{\"email\":\"" + objregistration.Email + "\",\"DOB\":\"" + objregistration.DOB + "\",\"Zipcode\":\"" + objregistration.Zipcode + "\",\"firstName\":\"" + objregistration.Firstname + "\",\"lastName\":\"" + objregistration.Lastname + "\",\"gender\":\"" + Gender + "\",\"Username\":\"" + objregistration.Username + "\",\"Country\":\"" + objregistration.Country + "\",\"DeviceId\":\"\",\"profilePicture\":\"" + objregistration.profilepic + "\",\"aboutmeaudio\":\"" + audiobase64 + "\",\"imagebase64\":\"" + imagebase64 + "\"}";
               jsonstring = "{\"email\":\"" + objregistration.Email + "\",\"DOB\":\"" + objregistration.DOB + "\",\"firstName\":\"" + objregistration.Firstname + "\",\"lastName\":\"" + objregistration.Lastname + "\",\"gender\":\"" + Gender + "\",\"Username\":\"" + objregistration.Username + "\",\"Country\":\"" + objregistration.Country + "\",\"DeviceId\":\"\",\"profilePicture\":\"" + objregistration.profilepic + "\",\"aboutmeaudio\":\"" + audiobase64 + "\",\"imagebase64\":\"" + imagebase64 + "\"}";
               resultjson = objhelperclass.callservice(jsonstring, "UserRegistration");
                string pp = resultjson[0].ToString();
                XmlDocument xd1 = new XmlDocument();
                xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(resultjson, "root");
                jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                if (jsonDataSet.Tables["Message"].Rows[0]["msg"].ToString() == "success")
                {
                   // Cache obj = new Cache();                  
                    System.Web.HttpRuntime.Cache["Username"] = jsonDataSet.Tables["Profile"].Rows[0]["Username"].ToString();
                    System.Web.HttpRuntime.Cache["Country"] = jsonDataSet.Tables["Profile"].Rows[0]["Country"].ToString();
                    System.Web.HttpRuntime.Cache["ProfilePicture"] = jsonDataSet.Tables["Profile"].Rows[0]["ProfilePicture"].ToString();
                    Session["UserId"] = jsonDataSet.Tables["Profile"].Rows[0]["UserId"].ToString();

                    //System.Web.HttpRuntime.Cache["GrammerBookmark"] = jsonDataSet.Tables["Profile"].Rows[0]["GrammerBookmark"].ToString();
                    //System.Web.HttpRuntime.Cache["VocabularyBookmark"] = jsonDataSet.Tables["Profile"].Rows[0]["VocabularyBookmark"].ToString();
                    //System.Web.HttpRuntime.Cache["DialogBookmark"] = jsonDataSet.Tables["Profile"].Rows[0]["DialogBookmark"].ToString();
                    //System.Web.HttpRuntime.Cache["BatchCount"] = jsonDataSet.Tables["Profile"].Rows[0]["BatchCount"].ToString();
                    //System.Web.HttpRuntime.Cache["VocabId"] = jsonDataSet.Tables["Profile"].Rows[0]["VocabId"].ToString();
                    //System.Web.HttpRuntime.Cache["VocabSubId"] = jsonDataSet.Tables["Profile"].Rows[0]["VocabSubId"].ToString();
                    
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

            }
           
            return View();
        }   
    }

}
