using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
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
using TradeSystem.Service;
using TradeSystem.Utils.Models;
using TradeSystem.WebApi.Controllers;
using System.Globalization;


namespace TradeSystem.MVCWeb.Controllers
{
    public class InvestmentController : BaseController
    {
        //HttpClient client;
        //The URL of the WEB API Service
        string url = WebConfigurationManager.AppSettings["url"];
        ICustomerService customerService;

        /// <summary>
        /// Constructor of Investment Controller
        /// </summary>
        public InvestmentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        /// <summary>
        /// Get list of new investement
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> NewInvestment()
        {
            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
            }

            var lang = SiteLanguages.GetDefaultLanguage();
            HttpResponseMessage responseMessage = await client.GetAsync(url + string.Format("/product/GetInvestments/" + Session["ClientUserId"] + "/" + lang + "/" + "0" + "/" + "0"));

            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var jsonData = JObject.Parse(responseData);
            var message = jsonData["Message"].ToString();
            var succeeded = jsonData["Succeeded"].ToString();
            var errorInfo = jsonData["ErrorInfo"].ToString();

            if (errorInfo == "false")
            {
                ViewBag.Message = message;
                //return View(objLogin);
            }
            if (Convert.ToBoolean(succeeded) == true)
            {
                var response = jsonData["DataList"].ToString();
                var objProductList = JsonConvert.DeserializeObject<List<ProductDataModel>>(response);
                return View(objProductList);
            }
            else
            {
                List<ProductDataModel> obj = new List<ProductDataModel>();
                return View(obj);
            }
            // return RedirectToAction("Error");

            //return View();
        }

        /// <summary>
        /// Method for get product details using product id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> NewInvestmentDetails(string id)
        {
            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
            }
            //logger.Info("Test log on invertory");
            HttpResponseMessage responseMessage = await client.GetAsync(url + string.Format("/product/GetProductByProductId/" + id + "/" + Session["ClientUserId"].ToString()));
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var objProductDetail = JsonConvert.DeserializeObject<ProductDataModel>(responseData);
                if (objProductDetail != null)
                {
                    Session["ProductId"] = objProductDetail.Id;
                    return View(objProductDetail);
                }
                else
                {
                    return View("Error");
                }
            }
            return View("Error");
        }

        /// <summary>
        /// Get method Withdraw Earning 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="withdrawType"></param>
        /// <param name="productId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WithdrawEarning(string amount, string withdrawType, string productId, string customerId)
        {
            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
            }
            TempData["WithdrawAmount"] = amount;
            TempData["WithdrawType"] = withdrawType;
            TempData["ProductId"] = productId;
            TempData["CustomerId"] = customerId;
            return View("WithdrawEarning");
        }

        /// <summary>
        /// Post method for WithdrawEarning(only for CompnayUser).
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> WithdrawEarning(WithdrawDataModel withdrawObj)
        {
            try
            {
                //WithdrawDataModel withdrawObj = new WithdrawDataModel();
                withdrawObj.Lang = SiteLanguages.GetDefaultLanguage();
                withdrawObj.CustomerId = TempData["CustomerId"].ToString();
                withdrawObj.ProductId = TempData["ProductId"].ToString();
                withdrawObj.SessionCustomerId = Session["ClientUserId"].ToString();
                withdrawObj.WithdrawAmount = float.Parse(TempData["WithdrawAmount"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                withdrawObj.Status = "true";
                // checking the conditions
                if (TempData["WithdrawType"].ToString() == "Investment")
                {
                    withdrawObj.IsEarning = false;
                    withdrawObj.IsSale = false;

                }
                if (TempData["WithdrawType"].ToString() == "WeeklyEarning")
                {
                    withdrawObj.IsEarning = true;
                    withdrawObj.IsSale = false;

                }
                if (TempData["WithdrawType"].ToString() == "SaleEarning")
                {
                    withdrawObj.IsEarning = false;
                    withdrawObj.IsSale = true;

                }
                if (withdrawObj.IsWalletOrBank == "Deposit earning into virtual wallet")
                {
                    withdrawObj.IsVirtualWallet = true;
                }
                else
                { withdrawObj.IsVirtualWallet = false; }
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/finance/GetWithdrawInvestmentOrEarnings", withdrawObj);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                    var jsonData = JObject.Parse(responseData);
                    var message = jsonData["Message"].ToString();
                    var errorInfo = jsonData["ErrorInfo"].ToString();

                    if (errorInfo == "false")
                    {
                        ViewBag.Message = message;
                        return View(withdrawObj);
                    }
                    var succeeded = jsonData["Succeeded"].ToString();
                    if (succeeded == "True")
                    {
                        //Session["VirtualWalletAmount"] = errorInfo;
                        if (TempData["WithdrawType"].ToString() == "SaleEarning")
                        {
                            return RedirectToAction("CommissionFromSalesReleased", "Sale");
                        }
                        else
                        {
                            TempData["WithdrawInvestmentMessage"] = true;                       
                            return View(withdrawObj);
                        }
                    }
                    else
                    {
                        return View("Error");
                    }
                }
                else
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    var jsonData = JObject.Parse(responseData);
                    var message = jsonData["Message"].ToString();
                    ViewBag.Message = message;
                }
                return View("Error");

            }
            catch (Exception ex)
            {
                // ViewBag.Message = ex.InnerException + " / / " + ex.TargetSite
                ViewBag.Message = "Something went wrong.";
                return View(withdrawObj);
            }
        }

        /// <summary>
        /// Get list of my Investments using customer id
        /// </summary>
        /// <param name="customer id"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> MyInvestmentsList()
        {
            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
            }

            var Lang = SiteLanguages.GetDefaultLanguage();
            HttpResponseMessage responseMessage = await client.GetAsync(url + string.Format("/product/GetMyInvestments/" + Session["ClientUserId"] + "/" + Lang + "/" + "0" + "/" + "0"));

            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var jsonData = JObject.Parse(responseData);
            var message = jsonData["Message"].ToString();
            var succeeded = jsonData["Succeeded"].ToString();
            var errorInfo = jsonData["ErrorInfo"].ToString();
            logger.Info(message);


            if (errorInfo == "false")
            {
                ViewBag.Message = message;
                //return View(objLogin);
            }
            if (Convert.ToBoolean(succeeded) == true)
            {
                var response = jsonData["DataList"].ToString();
                var objProductList = JsonConvert.DeserializeObject<List<CustomerProductDataModel>>(response);
                TempData["TempMyProductList"] = objProductList;
                return View(objProductList);
            }
            else
            {
                List<CustomerProductDataModel> obj = new List<CustomerProductDataModel>();
                return View(obj);
            }
            // return RedirectToAction("Error");
        }

        /// <summary>
        /// Get list of my Investments detail using customerId and productId
        /// </summary>
        /// <param name="customer id"></param>
        /// <param name="product id"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> MyInvestmentsDetails(string id)
        {
            TempData["ProductId"] = id;
            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
            }
            var Lang = SiteLanguages.GetDefaultLanguage();
            // getting weekly earning history data
            HttpResponseMessage responseHistoryMessage = await client.GetAsync(url + string.Format("/finance/GetInvestmentHistory/" + Session["ClientUserId"] + "/" + id + "/" + Lang));

            var responseDataHistory = responseHistoryMessage.Content.ReadAsStringAsync().Result;
            var jsonDataHistory = JObject.Parse(responseDataHistory);
            var messageHistory = jsonDataHistory["Message"].ToString();
            var succeededHistory = jsonDataHistory["Succeeded"].ToString();
            var errorInfoHistory = jsonDataHistory["ErrorInfo"].ToString();

            if (Convert.ToBoolean(succeededHistory) == true)
            {
                var response = jsonDataHistory["DataList"].ToString();
                var objHistoryList = JsonConvert.DeserializeObject<List<WeeklyWithdrawHistoryDataModel>>(response);
                ViewBag.WeeklyWithdrawHistory = objHistoryList;
            }
            else
            {
                WithdrawDataModel objWithDrawModel = new WithdrawDataModel();
                ViewBag.WeeklyWithdrawHistory = "";
                //return View(obj);
            }

            // getting my investment detail info
            HttpResponseMessage responseMessage = await client.GetAsync(url + string.Format("/product/GetMyInvestmentDetailById/" + Session["ClientUserId"] + "/" + id + "/" + Lang));

            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var jsonData = JObject.Parse(responseData);
            var message = jsonData["Message"].ToString();
            var succeeded = jsonData["Succeeded"].ToString();
            var errorInfo = jsonData["ErrorInfo"].ToString();
            logger.Info(message);

            if (Convert.ToBoolean(succeeded) == true)
            {
                var response = jsonData["DataObject"].ToString();
                var objProductList = JsonConvert.DeserializeObject<CustomerProductDataModel>(response);
                return View(objProductList);
            }
            else
            {
                CustomerProductDataModel obj = new CustomerProductDataModel();
                //throw new Exception(errorInfo);
                return View(obj);

            }
            // return RedirectToAction("Error");
        }

        /// <summary>
        /// Export selected data into excel sheet.
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportToExcel()
        {
            var lang = SiteLanguages.GetDefaultLanguage();
            var grid = new GridView();
            var modal = (List<CustomerProductDataModel>)TempData.Peek("TempMyProductList");

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
                                          InvestmentName = p.Name,
                                          Investment = p.Investment,
                                          TotalWeeklyEarning = p.CurrentWeekTotalEarning,
                                          Status = p.Status,
                                          Date = p.CreatedDate,
                                          InvestmentWithdrawDate = p.InvestmentWithdrawDate
                                      };
                }
                else
                {
                    grid.DataSource = from p in modal
                                      select new
                                      {
                                          Nombredelacampaña = p.Name,
                                          Campaña = p.Investment,
                                          GananciasSemanalesTotales = p.CurrentWeekTotalEarning,
                                          Estado = p.Status,
                                          Fecha = p.CreatedDate,
                                          RetirodeInversiones = p.InvestmentWithdrawDate
                                      };
                }

            }
            grid.DataBind();

            
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=My_Investment.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View("MyInvestmentList");
        }
    }
}