using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using TradeMark.BAL;
using TradeMark.Models;
using Newtonsoft.Json;
using System.Globalization;

namespace TradeMark
{
    public partial class AjaxHandler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string status = Request.QueryString["status"].ToString();

            if (!string.IsNullOrEmpty(status))
            {
                if (status == "CheckEmail")
                {
                    CheckEmail(Request.QueryString["emailId"].ToString());
                }
                else if (status == "LoginWithFacebook")
                {
                    LoginWithFacebook(Request.QueryString["id"].ToString(), Request.QueryString["name"].ToString(), Request.QueryString["email"].ToString());
                }
                else if (status == "SaveSearchresult")
                {
                    SaveSearchresult(Request.QueryString["markIndex"].ToString(), Request.QueryString["componentFullForm"].ToString(), Convert.ToInt32(Request.QueryString["pointofContact"]), Request.QueryString["Title"].ToString(), Request.QueryString["goodsServices"].ToString(), Request.QueryString["fullForm"].ToString());
                }
                else if (status == "GetUserSearchResult")
                {
                    GetUserSearchResult();
                }
                else if (status == "CheckPasswordAvailable")
                {
                    CheckPasswordAvailable(Request.QueryString["password"]);
                }
                else if (status == "GetSearchLogReport")
                {
                    GetSearchLogReport(Request.QueryString["Startdate"].ToString(), Request.QueryString["Enddate"].ToString());
                }
                else if (status == "GetAllUsers")
                {
                    GetAllUsers();
                }
                else if (status == "DeleteUser")
                {
                    DeleteUser(Request.QueryString["UserId"].ToString());
                }
                else if (status == "EditUserStatus")
                {
                    EditUserStatus(Request.QueryString["UserId"].ToString(), Convert.ToBoolean(Request.QueryString["UserStatus"]));
                }
                else if (status == "CheckUSClassDescription")
                {
                    CheckUSClassDescription(Request.QueryString["USClassDescription"].ToString());
                }
                else if (status == "MakePayment")
                {
                    MakePayment(Convert.ToString(Request.QueryString["Token"]), float.Parse(Request.QueryString["Amount"].ToString(), CultureInfo.InvariantCulture.NumberFormat), Convert.ToString(Request.QueryString["Credits"]), Convert.ToString(Request.QueryString["PromoCode"]));
                }
                else if (status == "CheckPromoCode")
                {
                    CheckPromoCode(Convert.ToString(Request.QueryString["PromoCode"]));
                }

            }
        }

        /// <summary>
        /// Check email id 
        /// </summary>
        /// <param name="emailId"></param>
        public void CheckEmail(string emailId)
        {
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
            //Context.Response.Write(js.Serialize(new { data = retval }));
            Response.Write(retval);
            Response.End();

        }

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
            //JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            string retval = "";

            //Session["SocialMediaUserId"] = id;
            int value = oUserService.UserLogin(objUserModel);
            if (value > 0)
                retval = "true";
            else
                retval = "false";

            //    HttpContext.Current.Response.Write("{property: value}")
            //Context.Response.Write(js.Serialize(new { data = retval }));

            Session["UserId"] = oUserService.GetUserId("", id).ToString();
            Response.Write(retval);
            Response.End();
        }

        /// <summary>
        /// Method to save search result
        /// </summary>
        /// <param name="markIndex"></param>
        /// <param name="componentFullForm"></param>
        /// <param name="pointofContact"></param>
        /// <param name="Title"></param>
        /// <param name="goodsServices"></param>
        public void SaveSearchresult(string markIndex, string componentFullForm, int pointofContact, string Title, string goodsServices, string fullForm)
        {
            var userId = Session["UserId"].ToString();
            UserService oUserService = new UserService();
            bool value = oUserService.SaveSearchresult(markIndex, componentFullForm, pointofContact, Title, goodsServices, userId, fullForm);
            if (value == true)
            {
                Response.Write("true");
                Response.End();
            }
            else
            {
                Response.Write("false");
                Response.End();
            }

        }

        /// <summary>
        /// method to get the save search result list by loggined in user
        /// </summary>
        public void GetUserSearchResult()
        {
            string JSONString = string.Empty;

            JavaScriptSerializer js = new JavaScriptSerializer();

            var userId = Session["UserId"].ToString();
            UserService oUserService = new UserService();

            JSONString = JsonConvert.SerializeObject(oUserService.GetUserSearchResult(userId));
            // searchMarkList = oUserService.GetUserSearchResult(userId);

            // Response.Write(js.Serialize(new { Response = searchMarkList.ToArray()}));
            Response.Write(JSONString);
            Response.End();
            // return JSONString;
        }

        /// <summary>
        /// method to get the save search result list by loggined in user
        /// </summary>
        public void GetSearchLogReport(string startDate, string endDate)
        {
            string JSONString = string.Empty;

            JavaScriptSerializer js = new JavaScriptSerializer();

            SearchService oService = new SearchService();
            var dtlog = oService.GetSearchLogReport(startDate, endDate);
            Session["dtlog"] = dtlog;
            JSONString = JsonConvert.SerializeObject(dtlog);
            //DataTable dtSearchlog = oService.GetSearchLogReport();
            //Need to create list 

            // searchMarkList = oUserService.GetUserSearchResult(userId);

            // Response.Write(js.Serialize(new { Response = searchMarkList.ToArray()}));
            Response.Write(JSONString);
            Response.End();
            // return JSONString;
        }

        public void CheckPasswordAvailable(string password)
        {
            var userId = Session["UserId"].ToString();
            UserService oUserService = new UserService();
            bool passwordCheck = oUserService.PasswordCheck(userId, password);
            if (passwordCheck == true)
            {
                Response.Write("true");
                Response.End();
            }
            else
            {
                Response.Write("false");
                Response.End();
            }
        }
        /// <summary>
        /// Method to get all users
        /// </summary>
        public void GetAllUsers()
        {
            string JSONString = string.Empty;

            JavaScriptSerializer js = new JavaScriptSerializer();

            UserService oUserService = new UserService();
            var dtUser = oUserService.GetAllUsers();
            JSONString = JsonConvert.SerializeObject(dtUser);
            Response.Write(JSONString);
            Response.End();
        }

        /// <summary>
        /// Method to get all users
        /// </summary>
        public void DeleteUser(string userId)
        {
            string JSONString = string.Empty;

            JavaScriptSerializer js = new JavaScriptSerializer();

            UserService oUserService = new UserService();
            var dtUser = oUserService.DeleteUser(userId);
            JSONString = JsonConvert.SerializeObject(dtUser);
            Response.Write(JSONString);
            Response.End();
        }

        /// <summary>
        /// Method to edit status of user by user id
        /// </summary>
        public void EditUserStatus(string userId, bool userStatus)
        {
            string JSONString = string.Empty;

            JavaScriptSerializer js = new JavaScriptSerializer();
            UserService oUserService = new UserService();
            var dtUser = oUserService.EditUserStatus(userId, userStatus);
            JSONString = JsonConvert.SerializeObject(dtUser);
            Response.Write(JSONString);
            Response.End();
        }

        /// <summary>
        /// Check USClassDescription is available or not
        /// </summary>
        /// <param name="usClassDescription"></param>
        public void CheckUSClassDescription(string usClassDescription)
        {
            Context.Response.Clear();
            //JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.ContentType = "application/json";
            string retval = "";
            USClassService oUSClassService = new USClassService();
            bool value = oUSClassService.CheckUSClassDescription(usClassDescription);
            if (value == true)
                retval = "true";
            else
                retval = "false";
            Response.Write(retval);
            Response.End();

        }
        /// <summary>
        /// user buy the search credit from here
        /// </summary>
        /// <param name="token">stripe token</param>
        /// <param name="amount">amount pay to buy search credits</param>
        public void MakePayment(string token, float amount, string credits, string promocode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            UserTransactionModel objUserTransactionModel = new UserTransactionModel();
            objUserTransactionModel.Amount = Convert.ToInt32(amount * 100);
            objUserTransactionModel.Credits = Convert.ToInt32(credits);
            objUserTransactionModel.Token = token;
            objUserTransactionModel.PromoCode = promocode;
            objUserTransactionModel.UserId = Session["UserId"].ToString();
            UserService oUserService = new UserService();
            string JSONString = string.Empty;
            var objPaymentOutputModel = oUserService.MakePayment(objUserTransactionModel);
            JSONString = JsonConvert.SerializeObject(objPaymentOutputModel);
            //Email: Payment receipt to customer
            string emailBody = "<p>Hello, <br/><br/>Thanks for purchase credits.<br/>Please find the payment details below.<br/><br/>";
            emailBody = emailBody + "<table><tr><td>Credits:</td><td><b>" + objPaymentOutputModel.Credits + "</b></td></tr><tr><td> Cost: </td><td><b>$" + amount + "</b></td></tr><tr><td> Transaction Id: </td><td><b>" + objPaymentOutputModel.TransactionId + "</b></td></tr></table><br/><br/>Thanks<br/><br/>Very truly yours,<br/><br/>BOB";

            var Issent =Utility.Utility.SendEmail("BOB - Payment receipt.", emailBody, objPaymentOutputModel.UserEmailid);
            
            Response.Write(JSONString);
            Response.End();
            // return objPaymentOutputModel;
        }
        /// <summary>
        /// Get PromoCode Detail
        /// </summary>
        /// <param name="promocode"></param>
        /// <returns></returns>
        public void CheckPromoCode(string promocode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var userId = Session["UserId"].ToString();
            UserService oUserService = new UserService();
            string JSONString = string.Empty;
            var promoCodeDetail = oUserService.CheckPromoCode(promocode, userId);
            JSONString = JsonConvert.SerializeObject(promoCodeDetail);
            Response.Write(JSONString);
            Response.End();
        }

    }
}