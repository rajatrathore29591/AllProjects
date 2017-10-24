using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using TradeMark.BAL;
using TradeMark.Models;

namespace TradeMark
{


    /// <summary>
    /// Summary description for AjaxServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class AjaxServices : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void LoginWithFacebook(string id, string name, string email)
        {
            UserModel objUserModel = new UserModel();
            objUserModel.Email = email;
            objUserModel.Password = "";
            objUserModel.FirstName = name;
            objUserModel.MediaProvider = "Fb";
            objUserModel.SocialMediaUserId = id;
            objUserModel.ContactNo = "";
            objUserModel.UserName = name;
            UserService oUserService = new UserService();
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            string retval = "";
        //    HttpContext.Current.Session["SocialMediaUserId"] = id;
           // Session["SocialMediaUserId"] = id;
            int value = oUserService.UserLogin(objUserModel);
            if (value > 0)
                retval = "true";
            else
                retval = "false";

            //    HttpContext.Current.Response.Write("{property: value}")
            Context.Response.Write(js.Serialize(new { data = retval }));
        }
        // [WebMethod]
        //public void CheckEmail(string emailId)
        //{
        //    string retval = "";
        //    UserService oUserService = new UserService();
        //    bool value = oUserService.CheckEmail(emailId);
        //    //if (value == true)
        //    //    return true;
        //    //else
        //    //    return false;

        //    HttpContext.Current.Response.Write("{property: value}");

        //}


        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void CheckEmail(string emailId)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            string retval = "";
            UserService oUserService = new UserService();
            bool value = oUserService.CheckEmail(emailId);
            if (value == true)
                retval = "true";
            else
                retval = "false";

            //    HttpContext.Current.Response.Write("{property: value}")
            Context.Response.Write(js.Serialize(new { data = retval }));

       }

    }
}
