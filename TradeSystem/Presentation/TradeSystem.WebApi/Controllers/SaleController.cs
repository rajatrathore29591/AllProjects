using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using TradeSystem.Service;
using TradeSystem.Utils.Models;
using TradeSystem.WebApi.Controllers;

namespace TradeSystem.MVCWeb.Controllers
{
    public class SaleController : BaseController
    {

        //The URL of the WEB API Service
        string url = WebConfigurationManager.AppSettings["url"];
        ICustomerService customerService;

        /// <summary>
        /// Default Constructor of Sale Controller
        /// </summary>
        public SaleController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Get method for register new user
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> RegisterNewUser()
        {
            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
            }
            Session["CountryId"] = "";
            Session["BankId"] = "";
            ViewBag.Country = "";
            ViewBag.Bank = "";

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
        public async Task<ActionResult> RegisterNewUser(CustomerDataModel customerData)
        {
            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
            }
            customerData.Lang = SiteLanguages.GetDefaultLanguage();
            string a = Session["ClientUserId"].ToString();
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
                    customerData.IsActive = false;
                    customerData.CustomerReferalId = Session["ClientUserId"].ToString();                   
                    //string jsonData = JsonConvert.SerializeObject(customerData);
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/customer/Addcustomer", customerData);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        Session["CountryId"] = "";
                        Session["BankId"] = "";
                        CustomerDataModel obj = new CustomerDataModel();
                        return RedirectToAction("MySales", "Sale");
                    }
                    else if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        ViewBag.Country = System.Web.HttpRuntime.Cache["Country"];
                        ViewBag.Bank = System.Web.HttpRuntime.Cache["Bank"]; 
                        string stEmailExist = resmanager.GetString("EmailAlreadyExist", CultureInfo.GetCultureInfo(customerData.Lang));
                        ViewBag.Message = stEmailExist;
                        return View(customerData);
                    }
                    else
                    {
                        string stSomethingWentWorng = resmanager.GetString("Api_SomethingWentWorng", CultureInfo.GetCultureInfo(customerData.Lang));
                        ViewBag.Message = stSomethingWentWorng;
                    }
                }
                else
                {
                    ViewBag.Country = System.Web.HttpRuntime.Cache["Country"];
                    ViewBag.Bank = System.Web.HttpRuntime.Cache["Bank"];
                    ViewBag.PasswordMessage = "";
                    string stEmailNConfirmedEmailNotMatched = resmanager.GetString("EmailNConfirmedEmailNotMatched", CultureInfo.GetCultureInfo(customerData.Lang));
                    ViewBag.EmailMessage = stEmailNConfirmedEmailNotMatched;
                    return View(customerData);
                }
            }
            else
            {
                if (customerData.Email.Trim() != customerData.ConfirmEmail.Trim())
                {
                    string stEmailNConfirmedEmailNotMatched = resmanager.GetString("EmailNConfirmedEmailNotMatched", CultureInfo.GetCultureInfo(customerData.Lang));
                    ViewBag.EmailMessage = stEmailNConfirmedEmailNotMatched;
                }
                ViewBag.Country = System.Web.HttpRuntime.Cache["Country"];
                ViewBag.Bank = System.Web.HttpRuntime.Cache["Bank"];
                string stPasswordNConfirmedPasswordNotMatched = resmanager.GetString("PasswordNConfirmedPasswordNotMatched", CultureInfo.GetCultureInfo(customerData.Lang));
                ViewBag.PasswordMessage = stPasswordNConfirmedPasswordNotMatched;
                return View(customerData);
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Get method for display ProductList
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> MySales()
        {

            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
            }
            string customerId = Session["ClientUserId"].ToString();
            string lang = SiteLanguages.GetDefaultLanguage();
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/sale/GetAllSalesByCustomerReferalId/" + customerId +"/"+ lang + "/" + "0" + "/" + "0");

            if (responseMessage.IsSuccessStatusCode)
            {
                //List<CompanyUserDataModel> objDashbordDetails = new List<CompanyUserDataModel>();
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var jsonData = JObject.Parse(responseData);
                    var response = jsonData["DataList"].ToString();
                    var objCustomerSalesDetails = JsonConvert.DeserializeObject<List<CustomerProductDataModel>>(response);
                    //var objCustomerSalesDetails = JsonConvert.DeserializeObject<List<BaseResponseDataModel>>(responseData);
                    //new List<CustomerProductDataModel> = objCustomerSalesDetails.;
                    
                    TempData["TempSalesCustomerList"] = objCustomerSalesDetails;
                    if (objCustomerSalesDetails != null)
                    {
                        return View(objCustomerSalesDetails);
                    }

                }
                else if (responseMessage.StatusCode == HttpStatusCode.NoContent)
                {
                    List<CustomerProductDataModel> objCustomerSalesDetails = new List<CustomerProductDataModel>();
                    //ViewBag.Message = "No data found";
                    return View(objCustomerSalesDetails);
                }
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Get method for display ProductList
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> CommissionFromSalesReleased()
        {

            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
            }
            string customerId = Session["ClientUserId"].ToString();
            string lang = SiteLanguages.GetDefaultLanguage();
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/sale/GetAllCommisionSalesByCustomerReferalId/" + customerId + "/" + lang + "/" + "0" + "/" + "0");

            if (responseMessage.IsSuccessStatusCode)
            {
                //List<CompanyUserDataModel> objDashbordDetails = new List<CompanyUserDataModel>();
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var jsonData = JObject.Parse(responseData);
                    var response = jsonData["DataList"].ToString();
                    var objCustomerSalesDetails = JsonConvert.DeserializeObject<List<CustomerProductDataModel>>(response);                    
                    TempData["TempCommissionSalesCustomerList"] = objCustomerSalesDetails;
                    if (objCustomerSalesDetails != null)
                    {
                        return View(objCustomerSalesDetails);
                    }

                }
                else if (responseMessage.StatusCode == HttpStatusCode.NoContent)
                {
                    List<CustomerProductDataModel> objCustomerSalesDetails = new List<CustomerProductDataModel>();
                    //ViewBag.Message = "No data found";
                    return View(objCustomerSalesDetails);
                }
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Export selected data into excel sheet.
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportToExcel()
        {
            var lang = SiteLanguages.GetDefaultLanguage();
            var grid = new GridView();
            var modal = (List<CustomerProductDataModel>)TempData.Peek("TempSalesCustomerList");
            if (modal == null)
            {
                grid.EmptyDataText = "No data available";
            }
            else
            {
                if (lang == "en")
                {
                    grid.DataSource = from p in modal
                                      select new
                                      {
                                          CustomerName = p.CustomerName,
                                          Date = p.CreatedDate,
                                          Investment = p.Investment,
                                          SaleEarning = p.SaleEarning,
                                          Status = p.Status == "true" ? "Active" : "InActive"
                                      };
                }
                else
                {
                    grid.DataSource = from p in modal
                                      select new
                                      {
                                          NombredelCliente = p.CustomerName,
                                          Fecha = p.CreatedDate,
                                          Campaña = p.Investment,
                                          VentaGanancia = p.SaleEarning,
                                          Estado = p.Status == "true" ? "Active" : "InActive"
                                      };
                }
            }
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=My_Sales.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View("MySales");
        }

        /// <summary>
        /// Export selected data into excel sheet.
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportToCommissionExcel()
        {
            var lang = SiteLanguages.GetDefaultLanguage();
            var grid = new GridView();
            var modal = (List<CustomerProductDataModel>)TempData.Peek("TempCommissionSalesCustomerList");
            if (modal == null)
            {
                grid.EmptyDataText = "No data available";
            }
            else
            {
                if (lang == "en")
                {
                    grid.DataSource = from p in modal
                                      select new
                                      {
                                          CustomerName = p.CustomerName,
                                          RegisterDate = p.CreatedDate,
                                          InvestmentName = p.InvestmentName,
                                          SaleEarning = p.SaleEarning,
                                          Status = p.Status
                                      };
                }
                else
                {
                    grid.DataSource = from p in modal
                                      select new
                                      {
                                          NombredelCliente = p.CustomerName,
                                          FechadeRegistro = p.CreatedDate,
                                          Nombredelacampaña = p.InvestmentName,
                                          VentaGanancia = p.SaleEarning,
                                          Estado = p.Status
                                      };
                }
            }
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Commission_Sales.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View("MySales");
        }
    }
}