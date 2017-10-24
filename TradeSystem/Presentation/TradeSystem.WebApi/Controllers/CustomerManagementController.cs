using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradeSystem.Framework.Entities;
using TradeSystem.MVCWeb.Models;
using TradeSystem.Service;
using TradeSystem.Utils.Models;
using TradeSystem.WebApi.Controllers;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TradeSystem.MVCWeb.Controllers
{
    public class CustomerManagementController : BaseController
    {
        //The URL of the WEB API Service
        string url = WebConfigurationManager.AppSettings["url"];
        ICustomerService customerService;

        /// <summary>
        /// Constructor of Customer Management Controller 
        /// </summary>
        public CustomerManagementController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Code for language selector (multilingual selection)
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public ActionResult ChangeLanguage(string lang, string url)
        {
            //var currentUrl = Request.Url.AbsoluteUri;
            new SiteLanguages().SetLanguage(lang);
            //return RedirectToAction("Login", "CustomerManagement");
            return Redirect(url);
        }

        /// <summary>
        /// Get all customer details
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ExistingCustomers()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Request.Cookies["Access_Token"].Value);
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/Customer/GetAllCustomers");
            if (responseMessage.IsSuccessStatusCode)
            {
                //List<CompanyUserDataModel> objDashbordDetails = new List<CompanyUserDataModel>();
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var objCustomerDetails = JsonConvert.DeserializeObject<List<CustomerListDataModel>>(responseData);
                    // temp data initialization for export excel sheet
                    TempData["TempCustomerList"] = objCustomerDetails;
                    if (objCustomerDetails != null)
                    {
                        return View(objCustomerDetails);
                    }

                }
                else if (responseMessage.StatusCode == HttpStatusCode.NoContent)
                {
                    List<CustomerListDataModel> objProductDetails = new List<CustomerListDataModel>();
                    return View(objProductDetails);
                }
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        ///  GET method for Add Customer
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> NewCustomerRegistration()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            Session["CountryId"] = "";
            Session["BankId"] = "";
            ViewBag.Country = "";
            ViewBag.Bank = "";

            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Request.Cookies["Access_Token"].Value);
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/customer/GetAllCountry");
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {

                new List<SelectListModel>();
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                ViewBag.Country = JsonConvert.DeserializeObject<List<SelectListModel>>(responseData);
                System.Web.HttpRuntime.Cache["Country"] = ViewBag.Country;
            }

            //HttpResponseMessage response = await client.GetAsync(url + "/customer/GetAllBank");
            //if (responseMessage.IsSuccessStatusCode)
            //{
            //    new List<SelectListModel>();
            //    var responseBank = response.Content.ReadAsStringAsync().Result;

            //    ViewBag.Bank = JsonConvert.DeserializeObject<List<SelectListModel>>(responseBank);
            //    System.Web.HttpRuntime.Cache["Bank"] = ViewBag.Bank;
            //}



            CustomerDataModel customerData = new CustomerDataModel();
            return View(customerData);
        }

        /// <summary>
        /// Post method for add customer
        /// </summary>
        /// <param name="customerData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> NewCustomerRegistration(CustomerDataModel customerData)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            string a = Session["UserId"].ToString();
            if (customerData.CountryId != null)
            {
                Session["CountryId"] = customerData.CountryId;
            }
            if (customerData.BankId != null)
            {
                Session["BankId"] = customerData.BankId;
            }
            if (customerData.RFC == null)
            {
                customerData.RFC = customerData.DNI;
            }

            if (customerData.Password == customerData.ConfirmPassword)
            {
                if (customerData.Email.Trim() == customerData.ConfirmEmail.Trim())
                {
                    // intializing login id from session
                    customerData.companyUserId = Session["UserId"].ToString();
                    customerData.IsActive = true;
                    //string jsonData = JsonConvert.SerializeObject(customerData);
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/customer/Addcustomer", customerData);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        Session["CountryId"] = "";
                        Session["BankId"] = "";
                        CustomerDataModel obj = new CustomerDataModel();
                        return RedirectToAction("ExistingCustomers", "CustomerManagement");
                    }
                    else if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        ViewBag.Country = System.Web.HttpRuntime.Cache["Country"];
                        ViewBag.Bank = System.Web.HttpRuntime.Cache["Bank"];
                        ViewBag.Message = "This email already exist";
                        return View(customerData);
                    }
                    else
                    {
                        ViewBag.Message = "Something went wrong.";
                    }
                }
                else
                {
                    ViewBag.Country = System.Web.HttpRuntime.Cache["Country"];
                    ViewBag.Bank = System.Web.HttpRuntime.Cache["Bank"];
                    ViewBag.PasswordMessage = "";
                    ViewBag.EmailMessage = "Email and Confirmed Email not matched";
                    return View(customerData);
                }
            }
            else
            {
                if (customerData.Email.Trim() != customerData.ConfirmEmail.Trim())
                {
                    ViewBag.EmailMessage = "Email and Confirmed Email not matched";
                }
                ViewBag.Country = System.Web.HttpRuntime.Cache["Country"];
                ViewBag.Bank = System.Web.HttpRuntime.Cache["Bank"];
                ViewBag.PasswordMessage = "Password and Confirmed Password not matched";
                return View(customerData);
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Get details of customer using customer id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GeneralDetails(string id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            HttpResponseMessage responseMessage = await client.GetAsync(url + string.Format("/customer/GetCustomerByCustomerId/" + id));
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                //ViewBag.StateController = JsonConvert.DeserializeObject<List<SelectListModel>>(responseData);
                //return new JsonResult { Data = responseData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                var objCompanyUserDetail = JsonConvert.DeserializeObject<CustomerDataModel>(responseData);
                if (objCompanyUserDetail != null)
                {
                    return View(objCompanyUserDetail);
                }
                else
                {
                    return View("Error");
                }
            }
            return View("Error");
        }

        /// <summary>
        /// Post method for add customer
        /// </summary>
        /// <param name="customerData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GeneralDetails(CustomerDataModel customerData)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            if (customerData != null)
            {
                customerData.IsActive = customerData.CheckStatus == "Active" ? true : false;
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/customer/EditCustomer", customerData);
                if (responseMessage.IsSuccessStatusCode)
                {
                    CustomerDataModel obj = new CustomerDataModel();
                    return RedirectToAction("ExistingCustomers", "CustomerManagement");
                }
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Get all state using category id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetAllState(string id)
        {

            if (!String.IsNullOrEmpty(id))
            {
                HttpResponseMessage responseMessage = await client.GetAsync(url + "/customer/GetAllState/" + id);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                    return new JsonResult { Data = responseData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            else
            {
                var responseData = new List<SelectListItem>
           {
               new SelectListItem { Value = "", Text = "Select" }
           };
                var data = JsonConvert.SerializeObject(responseData);
                return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Get details of Investments List using customer id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> InvestmentsList(string id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }

            HttpResponseMessage responseMessage = await client.GetAsync(url + string.Format("/customer/GetAllCustomerInvestments/" + id));
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var objProductDetails = JsonConvert.DeserializeObject<List<CustomerProductDataModel>>(responseData);
                if (objProductDetails != null)
                {
                    return View(objProductDetails);
                }

            }
            else if (responseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                List<CustomerProductDataModel> objCustomer = new List<CustomerProductDataModel>();
                return View(objCustomer);
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Get method for Approve product by admin
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="ProductId"></param>
        /// <param name="Status"></param>
        /// <param name="Investment"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Approve(string CustomerId, string ProductId, string Status, string Investment)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            // adding params into withdraw data model
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/customer/FinanceApprove/" + CustomerId + "/" + ProductId + "/" + Status + "/?Investment=" + Investment);
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var jsonData = JObject.Parse(responseData);
                var message = jsonData["Message"].ToString();
                if (message == "False")
                {
                    TempData["IsApprove"] = false;
                }
                else
                {
                    TempData["IsApprove"] = null;
                }
                return RedirectToAction("InvestmentsList", new { id = CustomerId });
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return View("Error");
            }
            return View("Error");
        }

        /// <summary>
        /// Get details of Sale List using customer id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> SalesList(string id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            HttpResponseMessage responseMessage = await client.GetAsync(url + string.Format("/customer/GetAllCustomerSales/" + id));
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var objProductDetails = JsonConvert.DeserializeObject<List<CustomerProductDataModel>>(responseData);
                if (objProductDetails != null)
                {
                    return View(objProductDetails);
                }

            }
            else if (responseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                List<CustomerProductDataModel> objCustomer = new List<CustomerProductDataModel>();
                return View(objCustomer);
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Export selected data into excel sheet.
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportToExcel()
        {

            var grid = new GridView();
            var modal = (List<CustomerListDataModel>)TempData.Peek("TempCustomerList");
            if (modal == null)
            {
                grid.EmptyDataText = "No data available";
            }
            else
            {

                grid.DataSource = from p in modal
                                  select new
                                  {
                                      Date = p.CreatedDate,
                                      UserName = p.UserName,
                                      CustomerName = p.CustomerName,
                                      Status = p.Status,
                                      TotalInvestment = p.TotalInvestment,
                                      TotalInvestmentCount = p.TotalInvestmentCount,
                                      TotalSaleCount = p.TotalSaleCount
                                  };
            }
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Customer_List.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View("Index");
        }

        #region Login, ForgotPassword, Change Password For Customer

        /// <summary>
        /// Get method for Login(only for CompnayUser).
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Message = "";
            //TempData["Message"] = "";
            if (Session["ClientUserId"] != null)
            {
                return RedirectToAction("CustomerDashboard", "Dashboard");
            }

            LoginModel objLogin = new LoginModel();
            return View(objLogin);
        }

        /// <summary>
        /// Post method for Login(only for CompnayUser).
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CustomerLogin(LoginModel objLogin)
        {
            ViewBag.Message = "";
            objLogin.Lang = SiteLanguages.GetDefaultLanguage();
            try
            {
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/customer/CustomerLogin", objLogin);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                    var jsonData = JObject.Parse(responseData);
                    var message = jsonData["Message"].ToString();
                    var errorInfo = jsonData["ErrorInfo"].ToString();
                    var succeeded = jsonData["Succeeded"].ToString();

                    if (errorInfo == "false")
                    {
                        TempData["Message"] = message;
                        return RedirectToAction("Login");
                    }
                    if (succeeded == "True")
                    {
                        var response = jsonData["DataObject"].ToString();
                        var objCompanyUserDetail = JsonConvert.DeserializeObject<AccessTokenModel>(response);
                        Session["ClientUserId"] = objCompanyUserDetail.UserId;
                        Session["ClientLoginUserName"] = objCompanyUserDetail.UserName;
                        Session["Access_Token"] = objCompanyUserDetail.access_token;
                        //api call for wallet amount
                        client.DefaultRequestHeaders.Remove("Authorization");
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Access_Token"]);
                        HttpResponseMessage responseResult = await client.GetAsync(url + "/finance/GetWalletAmountByCustomerId/" + Session["ClientUserId"].ToString() + "/" + objLogin.Lang);

                        if (responseResult.StatusCode == HttpStatusCode.OK)
                        {
                            var responseDataResult = responseResult.Content.ReadAsStringAsync().Result;
                            var jsonDataResult = JObject.Parse(responseDataResult);
                            var DataResult = jsonDataResult["DataObject"].ToString();
                            var objCustomerDetails = JsonConvert.DeserializeObject<WithdrawDataModel>(DataResult);

                            Session["VirtualWalletAmount"] = objCustomerDetails.AvailableBalance;

                        }

                        //api call for get data of notification
                        PromotionDataModel objPromotionDataModel = new PromotionDataModel();
                        objPromotionDataModel.Lang = SiteLanguages.GetDefaultLanguage();
                        HttpResponseMessage responsePromotion = await client.GetAsync(url + "/promotion/GetAllPromotionByCustomerId/" + Session["ClientUserId"] + "/" + objPromotionDataModel.Lang);
                        if (responsePromotion.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var responsePromoptionData = responsePromotion.Content.ReadAsStringAsync().Result;

                            var jsonobjData = JObject.Parse(responsePromoptionData);
                            var responsejsonobjData = jsonobjData["DataList"].ToString();
                            var objPromotionDetail = JsonConvert.DeserializeObject<List<PromotionDataModel>>(responsejsonobjData);

                            //List for promnotion details store on session
                            List<PromotionDataModel> listPromotion = (List<PromotionDataModel>)Session["PromotionListDetails"];

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

                            Session["PromotionListDetails"] = listPromotionItems;
                        }
                        return RedirectToAction("CustomerDashboard", "Dashboard");
                    }
                }
                else
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    var jsonData = JObject.Parse(responseData);
                    var message = jsonData["Message"].ToString();
                    var errorInfo = jsonData["ErrorInfo"].ToString();
                    TempData["Message"] = message;
                }
                return RedirectToAction("Login");

            }
            catch (Exception ex)
            {
                // ViewBag.Message = ex.InnerException + " / / " + ex.TargetSite
                string stSomethingWentWorng = resmanager.GetString("Api_SomethingWentWorng", CultureInfo.GetCultureInfo(objLogin.Lang));
                TempData["Message"] = stSomethingWentWorng;
                return View(objLogin);
            }
        }

        /// <summary>
        /// Get method for logout
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session["ClientUserId"] = null;
            Session["ClientLoginUserName"] = null;
            Session["Access_Token"] = null;

            return RedirectToAction("Login", "CustomerManagement");
        }

        /// <summary>
        /// Get method for Forgot Password(Both CompnayUser and Customer).
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerForgotPassword()
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
        public async Task<ActionResult> CustomerForgotPassword(LoginDataModel objLoginDataModel)
        {
            ViewBag.Success = "";
            ViewBag.Message = "";

            try
            {
                var IsAdmin = "false";
                objLoginDataModel.Lang = SiteLanguages.GetDefaultLanguage();
                var responseMessage = await client.GetAsync(string.Format("Account/CustomerForgotPassword/" + objLoginDataModel.Email + "/" + IsAdmin + "/" + objLoginDataModel.Lang));
                // HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/Account/ForgotPassword", objLoginDataModel);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    var jsonData = JObject.Parse(responseData);
                    var message = jsonData["Message"].ToString();
                    var errorInfo = jsonData["ErrorInfo"].ToString();

                    if (errorInfo == "false")
                    {
                        ViewBag.Success = "false";
                        ViewBag.Message = message;
                        return View(objLoginDataModel);
                    }
                    ViewBag.Success = "true";
                    //ViewBag.Message = "Password has been successfully sended.";
                    ViewBag.Message = message;
                    LoginDataModel obj = new LoginDataModel();
                    return View("CustomerForgotPassword", obj);
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    ViewBag.Success = "false";
                    string stEmailDoesNotMatch = resmanager.GetString("EmailDoesNotMatch", CultureInfo.GetCultureInfo(objLoginDataModel.Lang));
                    ViewBag.Message = stEmailDoesNotMatch;
                    //ViewBag.Message = "The email address you provided does not match our records. Please try again.";

                }
                else
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    var jsonData = JObject.Parse(responseData);
                    var message = jsonData["Message"].ToString();
                    ViewBag.Success = "false";
                    ViewBag.Message = message;
                    //ViewBag.Message = "Something went wrong.";
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
            TempData["TempChangePasswordSuccess"] = null;
            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
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
            objLoginDataModel.Id = Session["ClientUserId"].ToString();
            objLoginDataModel.OldPassword = objLoginDataModel.Password;
            objLoginDataModel.Lang = SiteLanguages.GetDefaultLanguage();
            try
            {
                if (ModelState.IsValid)
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/Customer/ChangePassword", objLoginDataModel);
                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //ViewBag.Success = "true";
                        string stPasswordhasbeenSuccessfulyChanged = resmanager.GetString("Api_PasswordhasbeenSuccessfulyChanged", CultureInfo.GetCultureInfo(objLoginDataModel.Lang));
                        TempData["TempChangePasswordSuccess"] = stPasswordhasbeenSuccessfulyChanged;
                        Session["ClientUserId"] = null;
                        Session["ClientLoginUserName"] = null;

                        return RedirectToAction("Login", "CustomerManagement");

                    }
                    else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        ViewBag.Success = "false";
                        string stOldPasswordIsNotCorrect = resmanager.GetString("OldPasswordIsNotCorrect", CultureInfo.GetCultureInfo(objLoginDataModel.Lang));
                        ViewBag.Message = stOldPasswordIsNotCorrect;

                    }
                    else
                    {
                        string stSomethingWentWorng = resmanager.GetString("Api_SomethingWentWorng", CultureInfo.GetCultureInfo(objLoginDataModel.Lang));
                        ViewBag.Message = stSomethingWentWorng;
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
        /// Show details for edit customer user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CustomerDataModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> EditMyProfile(string id)
        {
            ViewBag.Message = "";
            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
            }

            HttpResponseMessage responseMessage = await client.GetAsync(url + string.Format("/customer/GetCustomerByCustomerId/" + id));
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var objCustomerDetail = JsonConvert.DeserializeObject<CustomerDataModel>(responseData);
                if (objCustomerDetail != null)
                {
                    return View(objCustomerDetail);
                }
                else
                {
                    return View("Error");
                }
            }
            return View("Error");
        }

        /// <summary>
        /// Post method for edit my profile
        /// </summary>
        /// <param name="objCustomerDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EditMyProfile(CustomerDataModel objCustomerDataModel)
        {
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/customer/EditCustomerProfile", objCustomerDataModel);

            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                Session["ClientLoginUserName"] = objCustomerDataModel.FirstName + " " + objCustomerDataModel.LastName;
                return RedirectToAction("CustomerDashboard", "Dashboard");
            }
            return View("Error");
        }

        #endregion
    }
}