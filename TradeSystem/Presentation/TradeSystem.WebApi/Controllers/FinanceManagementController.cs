using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Openpay;
using Openpay.Entities;
using Openpay.Entities.Request;
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
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradeSystem.MVCWeb;
using TradeSystem.Utils;
using TradeSystem.Utils.Models;

namespace TradeSystem.WebApi.Controllers
{
    public class FinanceManagementController : BaseController
    {
        
        //The URL of the WEB API Service
        string url = WebConfigurationManager.AppSettings["url"];

        /// <summary>
        /// Default FinanceManagement Controller
        /// </summary>
        public FinanceManagementController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// GET: Withdraw List of all records
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> WithdrawalRequest()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/Finance/GetAllFinance");
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //List<CompanyUserDataModel> objDashbordDetails = new List<CompanyUserDataModel>();
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                List<WithdrawDataModel> objWithdrawDetails = JsonConvert.DeserializeObject<List<WithdrawDataModel>>(responseData);
                TempData["TempFinanceList"] = objWithdrawDetails;
                if (objWithdrawDetails != null)
                {

                    return View(objWithdrawDetails);
                }
                else
                {
                    return View(objWithdrawDetails);
                }
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                List<WithdrawDataModel> obj = new List<WithdrawDataModel>();
                return View(obj);
            }
            return View("Error");
        }

        /// <summary>
        /// Post method for edit finance by finance id
        /// </summary>
        /// <param name="objRoleDataModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> EditFinanceByFinanceId(string id, string status)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            // adding params into withdraw data model
            WithdrawDataModel objWithdrawDataModel = new WithdrawDataModel();
            objWithdrawDataModel.Id = id;
            objWithdrawDataModel.Status = status;
            TempData["ErrorMessage"] = "";
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/Finance/EditFinance", objWithdrawDataModel);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var objRoleDetails = JsonConvert.DeserializeObject<WithdrawDataModel>(responseData);
                if (objRoleDetails != null)
                {
                    //objRoleDataModel = new RoleDataModel();
                    //ModelState.Clear();
                    return RedirectToAction("WithdrawalRequest", "FinanceManagement");
                    //return View(objRoleDataModel);
                }
                else
                {
                    return View("Error");
                }
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {

                return View("Error");
            }
            return View("Error");
        }

        /// <summary>
        /// Post method for edit wallet amount by finance id
        /// </summary>
        /// <param name="objRoleDataModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> EditWithdrawWalletByFinanceId(string id, string status)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            // adding params into withdraw data model
            WithdrawDataModel objWithdrawDataModel = new WithdrawDataModel();
            objWithdrawDataModel.Id = id;
            objWithdrawDataModel.Status = status;
            TempData["ErrorMessage"] = "";
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/Finance/EditWithdrawWalletAmount", objWithdrawDataModel);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var objRoleDetails = JsonConvert.DeserializeObject<WithdrawDataModel>(responseData);
                if (objRoleDetails != null)
                {
                    return RedirectToAction("WithdrawalRequest", "FinanceManagement");
                }
                else
                {
                    return View("Error");
                }
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {

                return View("Error");
            }
            return View("Error");
        }

        /// <summary>
        /// Export selected data into excel sheet.
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportToExcel()
        {

            var grid = new GridView();
            var modal = (List<WithdrawDataModel>)TempData.Peek("TempFinanceList");
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
                                      EstimatedDepositDate = p.EstimatedDepositDate,
                                      CustomerName = p.CustomerName,
                                      WithDrawalFor = p.WithDrawalFor,
                                      WithdrawalRequestAmount = p.WithdrawAmount,
                                      Status = p.Status,
                                      DaysRemaining = p.RemainingDaysForDepositing
                                  };
            }

            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Finance_List.xls");
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

        #region Payment Gateway
        /// <summary>
        /// Get method for payment page and binding wallet amount corresponding to customer
        /// </summary>
        /// <param name="productObj"></param>
        /// <returns></returns>
        public async Task<ActionResult> Payment(string productid)
        {
            // TempData["Message"] = "";
            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
            }
            //string customerId = "0";
            var Lang = SiteLanguages.GetDefaultLanguage();
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/finance/GetWalletAmountByCustomerId/" + Session["ClientUserId"].ToString() + "/" + Lang);

            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var jsonData = JObject.Parse(responseData);
                var response = jsonData["DataObject"].ToString();
                var objCustomerDetails = JsonConvert.DeserializeObject<WithdrawDataModel>(response);
                ViewBag.WalletAmount = objCustomerDetails.AvailableBalance;
            }
            
            HttpResponseMessage responseProduct = await client.GetAsync(url + "/product/GetProductByProductId/" + productid + "/" + Session["ClientUserId"].ToString());
            if (responseProduct.IsSuccessStatusCode)
            {
                var responseDataProduct = responseProduct.Content.ReadAsStringAsync().Result;
                if (responseProduct.StatusCode == HttpStatusCode.OK)
                {
                    var objProductDetails = JsonConvert.DeserializeObject<ProductDataModel>(responseDataProduct);
                    if (objProductDetails != null)
                    {
                        return View(objProductDetails);
                    }

                }
                else if (responseMessage.StatusCode == HttpStatusCode.NoContent)
                {
                    ProductDataModel productObj = new ProductDataModel();
                    //ViewBag.Message = "No data found";
                    return View(productObj);
                }
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Card Option
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateCharge(FormCollection collection)
        {

            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
            }
            try
            {
                PaymentDataModel paymentDataModel = new PaymentDataModel();
                paymentDataModel.customerId = Session["ClientUserId"].ToString();
                paymentDataModel.productId = Session["ProductId"].ToString();
                paymentDataModel.investment = collection["investment"];
                paymentDataModel.paymentType = collection["creditcard"];
                paymentDataModel.lang = SiteLanguages.GetDefaultLanguage();
                paymentDataModel.walletAmount = float.Parse(collection["walletamount"]);
                paymentDataModel.id = collection["token_id"];
                paymentDataModel.lastRemainingInvestmentAmountStatus = Convert.ToBoolean(collection["lastRemainingInvestmentAmountStatus"]);
                paymentDataModel.device_session_id = collection["deviceIdHiddenFieldName"];
                paymentDataModel.lastRemainingInvestmentAmountStatus= Convert.ToBoolean(collection["lastRemainingInvestmentAmountStatus"]);
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/finance/InvesmentPayment", paymentDataModel);

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var responseResult = responseMessage.Content.ReadAsStringAsync().Result;

                    var jsonData = JObject.Parse(responseResult);
                    var message = jsonData["Message"].ToString();
                    var succeeded = jsonData["Succeeded"].ToString();

                    if (succeeded == "True")
                    {
                        // TempData["Message"] = message;
                        var response = jsonData["DataObject"].ToString();
                        var objCardChargeDetail = JsonConvert.DeserializeObject<CardChargeDataModel>(response);
                        if(paymentDataModel.walletAmount>0)
                        {
                            Session["VirtualWalletAmount"] = objCardChargeDetail.walletAmount;
                        }                        
                        Session["ProductId"] = "";
                        return RedirectToAction("SuccessPage", objCardChargeDetail);
                    }
                    else
                    {
                        TempData["Message"] = message;
                        return RedirectToAction("Payment", new { productid = Session["ProductId"].ToString() });
                    }

                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Payment", new { productid = Session["ProductId"].ToString() });
            }
            return RedirectToAction("Payment", new { productid = Session["ProductId"].ToString() });
        }

        /// <summary>
        /// Success Page
        /// </summary>
        /// <param name="objCardChargeModel"></param>
        /// <returns></returns>
        public ActionResult SuccessPage(CardChargeDataModel objCardChargeModel)
        {
            objCardChargeModel.chargeid = objCardChargeModel.chargeid;
            return View(objCardChargeModel);
        }

        /// <summary>
        /// For wire transfer option
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> WireTransfer(ProductDataModel productDataModel, FormCollection collection)
        {
            try
            {
                if (Session["ClientUserId"] == null)
                {
                    return RedirectToAction("Login", "CustomerManagement");
                }

                PaymentDataModel paymentDataModel = new PaymentDataModel();
                paymentDataModel.customerId = Session["ClientUserId"].ToString();
                paymentDataModel.productId = Session["ProductId"].ToString();
                paymentDataModel.investment = collection["amountToInvest"];
                paymentDataModel.lastRemainingInvestmentAmountStatus = Convert.ToBoolean(collection["lastRemainingInvestmentAmountStatus"]);
                string paymentType = collection["wallet_payment_type"];

                if (paymentType == "Wallet")
                {
                    paymentDataModel.paymentType = collection["wallet_payment_type"];
                }
                else
                {
                    paymentDataModel.paymentType = collection["paymentRadio"];
                }
                paymentDataModel.lang = SiteLanguages.GetDefaultLanguage();
                string value = collection["walletAmount"];
                if (value != null)
                {
                    paymentDataModel.walletAmount = float.Parse(collection["walletAmount"]);
                }

                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/finance/InvesmentPayment", paymentDataModel);

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var responseResult = responseMessage.Content.ReadAsStringAsync().Result;

                    var jsonData = JObject.Parse(responseResult);
                    var message = jsonData["Message"].ToString();
                    var errorInfo = jsonData["ErrorInfo"].ToString();
                    var succeeded = jsonData["Succeeded"].ToString();

                    if (succeeded == "True")
                    {
                        //TempData["Message"] = message;
                        var response = jsonData["DataObject"].ToString();
                        var VirtualWalletAmount = Session["VirtualWalletAmount"];
                        //var response = jsonData["DataObject"].ToString();

                        //get desrialize value from th json  
                        var objCardChargeDetail = JsonConvert.DeserializeObject<CardChargeDataModel>(response);
                        if (value != null)
                        {
                            Session["VirtualWalletAmount"] = objCardChargeDetail.walletAmount;
                            //Session["VirtualWalletAmount"] = (float)VirtualWalletAmount - float.Parse(value);
                        }
                        objCardChargeDetail.productId = Session["ProductId"].ToString();
                        Session["ProductId"] = "";
                        return RedirectToAction("SuccessPage", objCardChargeDetail);
                    }
                    else
                    {
                        TempData["Message"] = message;
                        return RedirectToAction("Payment", new { productid = Session["ProductId"].ToString() });
                    }
                }
                return RedirectToAction("Payment", new { productid = Session["ProductId"].ToString() });
            }
            catch (Exception ex)
            {
                //HttpResponseMessage responseMessage = await client.GetAsync(url + "/customer/DeleteCustomerProductByCustomerIdProductId/" + Session["ClientUserId"].ToString() + "/" + Session["ProductId"].ToString());
                return RedirectToAction("Payment", new { productid = Session["ProductId"].ToString() });
            }
        }

        /// <summary>
        /// Generate the receipt when user select mini market option
        /// </summary>
        /// <param name="customerProductDataModel"></param>
        /// <returns></returns>
        public async Task<ActionResult> Receipt(string id)
        {
            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
            }
            try
            {
                Session["ProductId"] = id;
                var lang = SiteLanguages.GetDefaultLanguage();
                HttpResponseMessage responseMessage = await client.GetAsync(url + "/customer/GetCustomerProduct/" + Session["ClientUserId"].ToString() + "/" + Session["ProductId"] + "/" + lang);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    var jsonData = JObject.Parse(responseData);
                    var message = jsonData["Message"].ToString();
                    var succeeded = jsonData["Succeeded"].ToString();
                    var errorInfo = jsonData["ErrorInfo"].ToString();
                    var response = jsonData["DataObject"].ToString();
                    var objProductList = JsonConvert.DeserializeObject<CustomerProductDataModel>(response);
                    //Image convert into base64 and save into folder   
                    //ImageDownload(objProductList.BarCode, objProductList.BarCodeUrl);
                    using (WebClient client = new WebClient())
                    {
                        var path = HostingEnvironment.MapPath("~/Pictures/");
                        client.DownloadFile(new Uri(objProductList.BarCodeUrl), path + objProductList.BarCode + ".png");
                    }
                    return View(objProductList);
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        #endregion

        #region wallet withdraw

        /// <summary>
        /// Generate the receipt when user select mini market option
        /// </summary>
        /// <param name="customerProductDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> WithdrawAmountFromWallet(string withdrawAmount)
        {
            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
            }

            WithdrawDataModel withdrawDataModel = new WithdrawDataModel();
            withdrawDataModel.Lang = SiteLanguages.GetDefaultLanguage();
            withdrawDataModel.CustomerId = Session["ClientUserId"].ToString();
            withdrawDataModel.WithdrawAmount = float.Parse(withdrawAmount, CultureInfo.InvariantCulture);
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/finance/WithdrawAmountFromWalletByCustomerId", withdrawDataModel);

            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var jsonData = JObject.Parse(responseData);
                var message = jsonData["Message"].ToString();
                var succeeded = jsonData["Succeeded"].ToString();
                var errorInfo = jsonData["ErrorInfo"].ToString();
                if (errorInfo == "false")
                {
                    //TempData["Message"] = messsage;
                    // var Message = { message : "success",error: "true" };
                    BaseResponseDataModel obj = new BaseResponseDataModel();
                    obj.Message = message;
                    obj.Succeeded = Convert.ToBoolean(succeeded);
                    //var Succeeded = Convert.ToBoolean(succeeded);
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
                if (succeeded == "True")
                {
                    BaseResponseDataModel obj = new BaseResponseDataModel();
                    obj.Message = message;
                    obj.Succeeded = Convert.ToBoolean(succeeded);
                    var VirtualWalletAmount = Session["VirtualWalletAmount"].ToString();
                    Session["VirtualWalletAmount"] = float.Parse(VirtualWalletAmount, CultureInfo.InvariantCulture) - float.Parse(withdrawAmount, CultureInfo.InvariantCulture);
                    //var Succeeded = Convert.ToBoolean(succeeded);
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
            }
            return View();
        }

        #endregion

        #region payment receipt genrate from the admin panel 

        /// <summary>
        /// Generate the receipt when user select mini market option
        /// </summary>
        /// <param name="customerProductDataModel"></param>
        /// <returns></returns>
        public async Task<ActionResult> GenerateReceipt(string customerId, string productId)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            try
            {
                //Session["ProductId"] = id;
                var lang = SiteLanguages.GetDefaultLanguage();
                HttpResponseMessage responseMessage = await client.GetAsync(url + "/customer/GetCustomerProduct/" + customerId + "/" + productId + "/" + lang);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    var jsonData = JObject.Parse(responseData);
                    var message = jsonData["Message"].ToString();
                    var succeeded = jsonData["Succeeded"].ToString();
                    var errorInfo = jsonData["ErrorInfo"].ToString();
                    var response = jsonData["DataObject"].ToString();
                    var objProductList = JsonConvert.DeserializeObject<CustomerProductDataModel>(response);
                    //Image convert into base64 and save into folder   
                    //ImageDownload(objProductList.BarCode, objProductList.BarCodeUrl);
                    using (WebClient client = new WebClient())
                    {
                        var path = HostingEnvironment.MapPath("~/Pictures/");
                        client.DownloadFile(new Uri(objProductList.BarCodeUrl), path + objProductList.BarCode + ".png");
                    }
                    return View(objProductList);
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        #endregion
    }
}