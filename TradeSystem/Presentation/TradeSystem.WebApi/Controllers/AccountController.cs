using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using TradeSystem.MVCWeb.Models;
using TradeSystem.Utils;
using TradeSystem.Utils.Models;
using TradeSystem.WebApi.Controllers;

namespace TradeSystem.MVCWeb.Controllers
{
    public class AccountController : BaseController
    {
        //The URL of the WEB API Service
        string url = WebConfigurationManager.AppSettings["url"];

        /// <summary>
        /// Constructor of Account Controller
        /// </summary>
        public AccountController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Get method for index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return RedirectToAction("CustomerManagement/Login");
        }

        /// <summary>
        /// Get method for page not found 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NotFoundError()
        {
            //Response.StatusCode = 404;  //you may want to set this to 200
            return View();
        }

        /// <summary>
        /// Get method for Login(only for CompnayUser).
        /// </summary>
        /// <returns></returns>
        public ActionResult Admin()
        {
            //logger.Info("Test log");
            AccountService.testmylog();
            ViewBag.Message = "";
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }
            //Session["AccessToken"] = "";
            LoginModel objLogin = new LoginModel();
            return View(objLogin);
        }

        /// <summary>
        /// Post method for Login(only for CompnayUser).
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Admin(LoginModel objLogin)
        {
            ViewBag.Message = "";
            try
            {
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/Account/CompanyLogin", objLogin);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    var response = responseMessage.Content.ReadAsAsync<AccessTokenModel>().Result;
                    Session["UserId"] = response.loginUserId;
                    Session["LoginUserName"] = response.loginUserName;
                    Session["Access_Token"] = response.access_token;

                    // initializing session for checking dashboard menus
                    Session["CustomerManagement"] = response.CustomerManagement;
                    Session["InvestmentConfiguration"] = response.InvestmentConfiguration;
                    Session["Inventory"] = response.Inventory;
                    Session["FinanceManagement"] = response.FinanceManagement;
                    Session["TicketManagement"] = response.TicketManagement;
                    Session["AccountManagement"] = response.AccountManagement;
                    Session["Reports"] = response.Reports;
                    Session["Promotions"] = response.Promotions;


                    //api call for get data of notification
                    PromotionDataModel objPromotionDataModel = new PromotionDataModel();
                    objPromotionDataModel.Lang = SiteLanguages.GetDefaultLanguage();
                    HttpResponseMessage responsePromotion = await client.GetAsync(url + "/promotion/GetAllPromotionByCustomerId/" + "0" + "/" + "en");
                    if (responsePromotion.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responsePromoptionData = responsePromotion.Content.ReadAsStringAsync().Result;

                        var jsonobjData = JObject.Parse(responsePromoptionData);
                        var responsejsonobjData = jsonobjData["DataList"].ToString();
                        var objPromotionDetail = JsonConvert.DeserializeObject<List<PromotionDataModel>>(responsejsonobjData);

                        //List for promnotion details store on session
                        List<PromotionDataModel> listPromotion = (List<PromotionDataModel>)Session["AdminPromotionListDetails"];

                        List<PromotionDataModel> listPromotionItems = new List<PromotionDataModel>();
                        foreach (var item in objPromotionDetail)
                        {
                            PromotionDataModel promotionDataModel = new PromotionDataModel();
                            promotionDataModel.Id = item.Id;
                            promotionDataModel.Description = Regex.Replace(item.Description, @"\t|\n|\r", "");
                            promotionDataModel.DescriptionSpanish = item.DescriptionSpanish != null ? Regex.Replace(item.DescriptionSpanish, @"\t|\n|\r", "") : "";
                            promotionDataModel.Url = item.Url;
                            promotionDataModel.Viewed = item.Viewed;
                            promotionDataModel.CreatedDate = item.CreatedDate;
                            listPromotionItems.Add(promotionDataModel);
                        }

                        Session["AdminPromotionListDetails"] = listPromotionItems;
                    }

                    return RedirectToAction("Dashboard", "Dashboard");
                }

                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
                {

                    ViewBag.Message = "The email or password you entered is incorrect.";
                }
                else
                {

                    ViewBag.Message = "Something went wrong";
                }
                return View(objLogin);

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.InnerException + " / / " + ex.TargetSite;
                return View(objLogin);
            }
        }

        /// <summary>
        /// Get method for Forgot Password(Both CompnayUser and Customer).
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgotPassword()
        {
            ViewBag.Message = "";
            ViewBag.Success = "";
            LoginDataModel objLoginDataModel = new LoginDataModel();
            return View(objLoginDataModel);
        }

        /// <summary>
        /// Post method for Forgot Password(Both CompnayUser and Customer).
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ForgotPassword(LoginDataModel objLoginDataModel)
        {
            ViewBag.Success = "";
            ViewBag.Message = "";

            try
            {

                var IsAdmin = true;
                var responseMessage = await client.GetAsync(string.Format("Account/ForgotPassword/" + objLoginDataModel.Email + "/" + IsAdmin + ""));
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ViewBag.Success = "true";
                    ViewBag.Message = "Password has been successfully sended.";
                    LoginDataModel obj = new LoginDataModel();
                    return View(obj);
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    ViewBag.Success = "false";
                    ViewBag.Message = "The email address you provided does not match our records. Please try again.";

                }
                else
                {
                    ViewBag.Message = "Something went wrong.";
                }
                return View(objLoginDataModel);

            }
            catch (Exception ex)
            {
                return View(objLoginDataModel);
            }
        }
        /// <summary>
        /// Get method for Change Password(Both CompnayUser and Customer).
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            ViewBag.Message = "";
            ViewBag.Success = "";
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            LoginDataModel objLoginDataModel = new LoginDataModel();
            return View(objLoginDataModel);
        }
        /// <summary>
        /// Post method for Change Password(Both CompnayUser and Customer).
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ChangePassword(LoginDataModel objLoginDataModel)
        {
            ViewBag.Success = "";
            ViewBag.Message = "";
            objLoginDataModel.Id = Session["UserId"].ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Request.Cookies["Access_Token"].Value);
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/CompanyUser/ChangePassword", objLoginDataModel);
                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.Success = "true";
                        TempData["TempChangedPasswordSuccessMessage"] = "Your password has been successfully changed";
                        Session["UserId"] = null;
                        Session["CustomerManagement"] = null;
                        Session["InvestmentConfiguration"] = null;
                        Session["Inventory"] = null;
                        Session["FinanceManagement"] = null;
                        Session["TicketManagement"] = null;
                        Session["AccountManagement"] = null;
                        Session["Reports"] = null;
                        Session["Promotions"] = null;
                        return RedirectToAction("Admin", "Account");
                    }
                    else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        ViewBag.Success = "false";
                        ViewBag.Message = "Your old password is not correct.";

                    }
                    else
                    {
                        ViewBag.Message = "Something went wrong.";
                    }
                    return View(objLoginDataModel);


                }

                return View(objLoginDataModel);

            }
            catch (Exception ex)
            {
                return View(objLoginDataModel);
            }
        }

        /// <summary>
        /// This method for logout 
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session["UserId"] = null;
            Session["CustomerManagement"] = null;
            Session["InvestmentConfiguration"] = null;
            Session["Inventory"] = null;
            Session["FinanceManagement"] = null;
            Session["TicketManagement"] = null;
            Session["AccountManagement"] = null;
            Session["Reports"] = null;
            Session["Promotions"] = null;
            Session["Access_Token"] = null;

            return RedirectToAction("Admin", "Account");
        }
    }
}