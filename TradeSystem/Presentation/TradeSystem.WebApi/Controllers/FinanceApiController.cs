using Newtonsoft.Json.Linq;
using Openpay;
using Openpay.Entities;
using Openpay.Entities.Request;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using TradeSystem.Framework.Identity;
using TradeSystem.MVCWeb;
using TradeSystem.Service;
using TradeSystem.Utilities.Email;
using TradeSystem.Utils;
using TradeSystem.Utils.Enum;
using TradeSystem.Utils.Models;

namespace TradeSystem.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/finance")]
    public class FinanceApiController : BaseApiController
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 

        IWithdrawService withdrawService;
        ICustomerProductService customerProductService;
        ICustomerService customerService;
        IWalletService walletService;
        IProductService productService;
        IEmailService emailService;
        IPenaltyService penaltyService;
        IPromotionService promotionService;

        // Constructor of Finance Api Controller
        public FinanceApiController(IWithdrawService _withdrawService, ICustomerProductService _customerProductService, ICustomerService _customerService, IWalletService _walletService, IProductService _productService, IEmailService _emailService, IPenaltyService _penaltyService, IPromotionService _promotionService)
        {
            withdrawService = _withdrawService;
            customerProductService = _customerProductService;
            customerService = _customerService;
            walletService = _walletService;
            productService = _productService;
            emailService = _emailService;
            penaltyService = _penaltyService;
            promotionService = _promotionService;
        }
        #endregion

        //Get value of projectPathUrl and projectName from the web config 
        string projectPathUrl = WebConfigurationManager.AppSettings["Domain"].ToString();
        string projectName = WebConfigurationManager.AppSettings["ProjectName"].ToString();

        //Get value of openpay merchant account from the web config 
        string openPaySkId = WebConfigurationManager.AppSettings["OpenPaySkId"].ToString();
        string openPayMrId = WebConfigurationManager.AppSettings["OpenPayMrId"].ToString();

        /// <summary>
        /// This method for get all record from "withdarw" entity.
        /// </summary>
        /// <returns>Customer entity object value.</returns>
        [HttpGet]
        [Route("GetAllFinance")]
        public HttpResponseMessage GetAllFinance()
        {
            try
            {
                ////get withdraw list 
                var financeCollection = withdrawService.GetAllFinanceList();

                ////check object
                if (financeCollection.Count > 0 && financeCollection != null)
                {
                    ////dynamic list.
                    dynamic finances = new List<ExpandoObject>();


                    ////return response from withdraw service.
                    foreach (var financeDetail in financeCollection)
                    {
                        ////bind dynamic property.
                        dynamic finance = new ExpandoObject();
                        var customer = customerService.GetCustomerById(financeDetail.CustomerId.Value);
                        ////map ids
                        finance.Id = financeDetail.Id;
                        finance.CreatedDate = financeDetail.CreatedDate;

                        //adding 30 days into created date for generating estimated deposit date
                        finance.EstimatedDepositDate = financeDetail.CreatedDate.AddDays(30);
                        finance.CustomerName = customer.FirstName + " " + customer.MiddleName + " " + customer.LastName;
                        if (financeDetail.IsEarning == true)
                        {
                            finance.WithDrawalFor = "Weekly Yield";
                        }
                        else if (financeDetail.IsSale == true)
                        {
                            finance.WithDrawalFor = "Commission By Sale";
                        }
                        //Check for virtual wallet 
                        else if (financeDetail.IsVirtualWallet == true && financeDetail.CustomerProductId == null)
                        {
                            finance.WithDrawalFor = "Virtual Wallet";
                        }
                        else
                        {
                            finance.WithDrawalFor = "Investment Withdrawal";
                        }

                        //Status for To "Wallet" or "Bank"
                        if (financeDetail.IsVirtualWallet == false)
                        {
                            finance.ToWithdrawStatus = "Bank";
                        }
                        else if (financeDetail.IsVirtualWallet == true && financeDetail.IsSale == false && financeDetail.IsEarning == false && financeDetail.CustomerProductId == null && financeDetail.Status == "Fund Released")
                        {
                            finance.ToWithdrawStatus = "Bank";
                        }
                        else if (financeDetail.CustomerProductId != null)
                        {
                            finance.ToWithdrawStatus = "Wallet";
                        }

                        finance.WithdrawAmount = financeDetail.WithdrawAmount;
                        finance.Status = financeDetail.Status;
                        var estimatedDays = Convert.ToInt32((finance.EstimatedDepositDate - DateTime.UtcNow).TotalDays);
                        if (financeDetail.IsVirtualWallet == true && financeDetail.IsSale == false && financeDetail.IsEarning == false && financeDetail.CustomerProductId == null && financeDetail.Status == "Fund Released")
                        {
                            finance.RemainingDaysForDepositing = 0;
                        }
                        else
                        {
                            finance.RemainingDaysForDepositing = estimatedDays > 0 ? estimatedDays : 0;
                        }


                        //set customers values in list.
                        finances.Add(finance);
                    }

                    ////return customers service 
                    return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK, (List<ExpandoObject>)finances);
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Data Not Found." });
                }
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
            }
        }

        /// <summary>
        /// Post method edit finance 
        /// </summary>
        /// <param name="withdrawDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EditFinance")]
        public HttpResponseMessage EditFinance(WithdrawDataModel withdrawDataModel)
        {
            try
            {
                //Call finance  entity for get data fro the withdraw table 
                var finance = withdrawService.GetFinanceByFinanceId(new Guid(withdrawDataModel.Id));
                if (finance != null)
                {
                    finance.Status = withdrawDataModel.Status;
                    if (finance.Status != "Rejected")
                    {
                        //status update of my investment
                        if (finance.CustomerProductId != null)
                        {
                            var customerProductDetails = customerProductService.GetAllCustomerProductByCustomerProductId(finance.CustomerProductId.Value);
                            if (!finance.IsEarning && !finance.IsSale)
                            {

                                var resultUpdate = customerProductService.UpdateCustomerProductStatus(customerProductDetails.CustomerId, customerProductDetails.ProductId, "Completed");
                            }
                            if (finance.IsSale)
                            {
                                //update data into customerproduct table.                             
                                var SaleStatus = string.Empty;
                                if (finance.IsVirtualWallet)
                                {
                                    SaleStatus = "Deposite to Wallet";
                                }
                                else
                                {
                                    SaleStatus = "Deposite to Bank";
                                }
                                var status = customerProductService.UpdateSaleEarningStatus(customerProductDetails.CustomerId, customerProductDetails.ProductId, SaleStatus, true);
                            }
                        }
                        if (finance.IsVirtualWallet)
                        {
                            var WalletDetail = walletService.GetWalletByCustomerId(finance.CustomerId.Value);
                            if (WalletDetail != null)
                            {
                                ////updating data into wallet table                            
                                float virtualAmount = (float)WalletDetail.AvailableBalance + finance.WithdrawAmount;
                                var responseDataResult = walletService.UpdateWalletAmount(finance.CustomerId.Value, virtualAmount);
                            }
                            else
                            {
                                withdrawDataModel.CustomerId = finance.CustomerId.ToString();
                                withdrawDataModel.WithdrawAmount = finance.WithdrawAmount;
                                ////inserting data into wallet table.                            
                                var responsedata = walletService.AddVirtualAmount(withdrawDataModel);
                            }
                        }
                    }
                    //Check response 
                    var response = withdrawService.EditFinance(finance.Id, finance.Status);
                    if (response)
                    {
                        ////return result from service response.
                        return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Successfully Updated" });
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "something went wrong" });
                    }
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Finance already exist" });
                }

            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception" });

            }
        }

        /// <summary>
        /// Post method edit finance 
        /// </summary>
        /// <param name="withdrawDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EditWithdrawWalletAmount")]
        public HttpResponseMessage EditWithdrawWalletAmount(WithdrawDataModel withdrawDataModel)
        {
            try
            {
                //Call finance  entity for get data fro the withdraw table 
                var finance = withdrawService.GetFinanceByFinanceId(new Guid(withdrawDataModel.Id));
                if (finance != null)
                {
                    finance.Status = withdrawDataModel.Status;

                    //Check response 
                    var response = withdrawService.EditFinance(finance.Id, finance.Status);
                    if (response)
                    {
                        ////return result from service response.
                        return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Successfully Updated" });
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "something went wrong" });
                    }
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Finance already exist" });
                }

            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception" });

            }
        }

        /// <summary>
        /// Post for update barcode in customer product table
        /// </summary>
        /// <param name="customerProductDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateCustomerProductWithBarCode")]
        public HttpResponseMessage UpdateCustomerProductWithBarCode(CustomerProductDataModel customerProductDataModel)
        {
            try
            {
                var responseUpdate = customerProductService.UpdateCustomerProductWithBarCode(customerProductDataModel);
                if (responseUpdate == true)
                {
                    ////return result from service response.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Successfully Updated" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "something went wrong" });
                }

            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception" });

            }
        }

        /// <summary>
        /// This method for get all record from "withdarw" entity.
        /// </summary>
        /// <returns>Customer entity object value.</returns>
        [HttpGet]
        [Route("GetWalletAmountByCustomerId/{customerId}/{lang}")]
        public HttpResponseMessage GetWalletAmountByCustomerId(string customerId, string lang)
        {
            try
            {
                ////get withdraw list 
                var walletDetail = walletService.GetWalletByCustomerId(new Guid(customerId));

                ////check object

                if (walletDetail != null)
                {
                    ////bind dynamic property.
                    dynamic wallet = new ExpandoObject();

                    ////map ids
                    wallet.Id = walletDetail.Id;
                    wallet.AvailableBalance = walletDetail.AvailableBalance;
                    wallet.CustomerId = walletDetail.CustomerId;
                    ////return wallet service 
                    string st = resmanager.GetString("Api_WalletDetails", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = wallet, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }

                else
                {
                    string stDataNot = resmanager.GetString("Api_DataNotFound", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataNot, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = stDataNot });

                }
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                string stFailed = resmanager.GetString("Api_Failed", CultureInfo.GetCultureInfo(lang));
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stFailed, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// Function for give string is valid date format
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public bool IsValidDateTimeTest(string dateTime)
        {
            string[] formats = { "dd/MM/yyyy" };
            DateTime parsedDateTime;
            return DateTime.TryParseExact(dateTime, formats, new CultureInfo("en-US"),
                                           DateTimeStyles.None, out parsedDateTime);
        }

        #region For withDrawing InvestmentOrEarnings

        /// <summary>
        /// This method for get all customer WithDraw Investment record .
        /// </summary>
        [HttpPost]
        [Route("GetWithdrawInvestmentOrEarnings")]
        public HttpResponseMessage GetWithdrawInvestmentOrEarnings(WithdrawDataModel withdrawDataModel)
        {
            try
            {
                var response = string.Empty;
                bool responsedata;
                float amount = 0;
                Guid customerId = new Guid(withdrawDataModel.CustomerId);
                Guid productId = new Guid(withdrawDataModel.ProductId);
                var withdrawAmountForAddInWallet = withdrawDataModel.WithdrawAmount;
                float finalAmountAfterPenalty = 0;
                bool isEnter = false;
                PromotionDataModel promotionDataModel = new PromotionDataModel();

                //getCustomerProductId By CustomerId and ProductId      
                var CustomerProductDetails = customerProductService.GetAllCustomerProductByProductIdCustomerId(customerId, productId);
                if (CustomerProductDetails != null)
                {
                    withdrawDataModel.CustomerProductId = CustomerProductDetails.Id.ToString();

                    if (withdrawDataModel.IsSale)
                    {
                        ////update data into customerproduct table.
                        //var withdrawAmount = CustomerProductDetails.SaleEarning - withdrawDataModel.WithdrawAmount;
                        //svar reduceSaleEarningAmount = customerProductService.UpdateSaleEarning(customerId, productId, withdrawAmount);

                        //Before time directly add into customer product table but now first admin approve then add 
                        var SaleStatus = "Requested";
                        var status = customerProductService.UpdateSaleEarningStatus(customerId, productId, SaleStatus, true);

                        // admin weekly yield notification alert
                        ////inserting data into promotion datamodel.
                        promotionDataModel.Description = "Sales withdraw request from " + CustomerProductDetails.Customer.FirstName + " " + CustomerProductDetails.Customer.LastName;
                        promotionDataModel.DescriptionSpanish = "Sales withdraw request from " + CustomerProductDetails.Customer.FirstName + " " + CustomerProductDetails.Customer.LastName;
                        promotionDataModel.To = null;
                        promotionDataModel.PromotionType = "Alert on Web & App";
                        promotionDataModel.Subject = "Sales withdraw";
                        promotionDataModel.SubjectSpanish = "Sales withdraw";
                        promotionDataModel.Url = projectPathUrl + projectName + "/FinanceManagement/WithdrawalRequest";

                        ////inserting data into promotion table.
                        var adminPromotionResponse = promotionService.AddPromotion(promotionDataModel);

                    }
                    if (withdrawDataModel.IsEarning)
                    {

                        var result = IsValidDateTimeTest(CustomerProductDetails.Product.WeeklyFromWithdrawDay);
                        if (!result)
                        {
                            int withdrawDay = Convert.ToInt32(CustomerProductDetails.Product.WeeklyFromWithdrawDay) * 7;
                            DateTime lastWeeklyEnableDate = System.DateTime.UtcNow;
                            if (CustomerProductDetails.LastWeeklyWithdrawEnableDate.Value.Year < System.DateTime.UtcNow.Year)
                            {
                                lastWeeklyEnableDate = CustomerProductDetails.LastWeeklyWithdrawEnableDate.Value.AddYears(1).AddDays(withdrawDay);
                            }
                            else
                            {
                                lastWeeklyEnableDate = CustomerProductDetails.LastWeeklyWithdrawEnableDate.Value.AddDays(withdrawDay);
                            }

                            ////update data into customerproduct table.
                            var withdrawAmount = CustomerProductDetails.WeeklyEarning - withdrawDataModel.WithdrawAmount;
                            var reduceWeeklyEarningAmount = customerProductService.UpdateWeeklyEarning(customerId, productId, withdrawAmount);
                            var lastWeeklyEnableResponse = customerProductService.UpdateLastWeeklyEnableDate(customerId, productId, lastWeeklyEnableDate);

                            // weekly case
                            DateTime lastWeeklyDate = CustomerProductDetails.LastWeeklyWithdrawDate.Value.AddDays(withdrawDay);
                            var lastWeeklyResponse = customerProductService.UpdateLastWeeklyDate(customerId, productId, lastWeeklyDate);
                        }
                        else
                        {
                            DateTime lastWeeklyEnableDate = System.DateTime.UtcNow;
                            if (CustomerProductDetails.LastWeeklyWithdrawEnableDate.Value.Year < System.DateTime.UtcNow.Year)
                            {
                                lastWeeklyEnableDate = CustomerProductDetails.LastWeeklyWithdrawEnableDate.Value.AddYears(1).AddMonths(1);
                            }
                            else
                            {
                                lastWeeklyEnableDate = CustomerProductDetails.LastWeeklyWithdrawEnableDate.Value.AddMonths(1);
                            }

                            ////update data into customerproduct table.
                            var withdrawAmount = CustomerProductDetails.WeeklyEarning - withdrawDataModel.WithdrawAmount;
                            var reduceWeeklyEarningAmount = customerProductService.UpdateWeeklyEarning(customerId, productId, withdrawAmount);
                            var lastWeeklyEnableResponse = customerProductService.UpdateLastWeeklyEnableDate(customerId, productId, lastWeeklyEnableDate);
                        }

                        // admin weekly yield notification alert
                        ////inserting data into promotion datamodel.
                        promotionDataModel.Description = "Weekly yields withdraw request from " + CustomerProductDetails.Customer.FirstName + " " + CustomerProductDetails.Customer.LastName;
                        promotionDataModel.DescriptionSpanish = "Weekly yields withdraw request from " + CustomerProductDetails.Customer.FirstName + " " + CustomerProductDetails.Customer.LastName;
                        promotionDataModel.To = null;
                        promotionDataModel.PromotionType = "Alert on Web & App";
                        promotionDataModel.Subject = "Weekly yield withdraw";
                        promotionDataModel.SubjectSpanish = "Weekly yield withdraw";
                        promotionDataModel.Url = projectPathUrl + projectName + "/FinanceManagement/WithdrawalRequest";

                        ////inserting data into promotion table.
                        var adminPromotionResponse = promotionService.AddPromotion(promotionDataModel);
                    }
                    if (withdrawDataModel.IsEarning == false && withdrawDataModel.IsSale == false)
                    {
                        ////update data into customerproduct table.
                        var withdrawAmount = CustomerProductDetails.Investment;
                        var reduceInvestmentAmount = customerProductService.UpdateInvestment(customerId, productId, withdrawAmount);
                        var stopCalculation = customerProductService.StopCalculation(customerId, productId, true, false);
                        var status = customerProductService.UpdateCustomerProductStatus(customerId, productId, "Requested");

                        // Penalty charge logic for premature withdrawal
                        // current day total earning
                        double dailyPercent = (double)(Convert.ToDecimal(CustomerProductDetails.Product.PercentWeeklyEarning) / 7);
                        double dailyAmount = (dailyPercent * withdrawAmount) / 100;
                        // penalty of days to deduct from weekly earning
                        int days = 0;
                        // calculating days
                        if (CustomerProductDetails.Product.WeeklyFromWithdrawDay.Contains('/'))  //Monthly
                        {
                            var regDay = CustomerProductDetails.StartCalculationDate.Value.Day;
                            var regDate = Convert.ToDateTime(DateTime.UtcNow.Month + "/" + regDay + "/" + DateTime.UtcNow.Year);
                            if (DateTime.UtcNow.Date >= regDate)
                            {
                                days = Convert.ToInt32(regDate.Subtract(DateTime.UtcNow.Date).TotalDays);
                            }
                            else
                            {
                                days = Convert.ToInt32(regDate.AddMonths(-1).Subtract(DateTime.UtcNow).TotalDays);
                            }
                        }
                        else  // Weekly
                        {
                            var lastWithdrawl = CustomerProductDetails.LastWeeklyWithdrawDate.Value;
                            if (lastWithdrawl.Date == DateTime.UtcNow.Date)
                            {
                                days = 1;
                            }
                            else
                            {
                                days = Convert.ToInt32(DateTime.UtcNow.Date.Subtract(lastWithdrawl).TotalDays);
                            }

                        }

                        double totalAmountOfMonthTillDate = days * dailyAmount;
                        // calulate days diffrence bet current date and created date 
                        var totalDays = (DateTime.UtcNow.Date.AddDays(1) - CustomerProductDetails.StartCalculationDate.Value.Date).TotalDays;
                        //get penalty list
                        var penalty = penaltyService.GetAllPenaltyByProductId(productId.ToString());
                        double penaltyAmount;
                        double calculatedAmount = 0;

                        foreach (var item in penalty)
                        {
                            if (Convert.ToDouble(item.From) <= totalDays && Convert.ToDouble(item.To) >= totalDays)
                            {
                                penaltyAmount = Convert.ToDouble(item.PenaltyPercent);
                                penaltyAmount = (penaltyAmount * totalAmountOfMonthTillDate) / 100;
                                calculatedAmount = CustomerProductDetails.WeeklyEarning - penaltyAmount;
                                finalAmountAfterPenalty = CustomerProductDetails.WeeklyEarning - float.Parse(calculatedAmount.ToString());
                                var updateWeekly = customerProductService.UpdateWeeklyEarning(customerId, productId, finalAmountAfterPenalty);
                                isEnter = true;
                                break;
                            }
                        }
                        //var updateWeeklyEarning = customerProductService.GetAllCustomerProductByProductIdCustomerId(customerId, productId);
                        //// adding weekly earning with requesting amount
                        //withdrawDataModel.WithdrawAmount += updateWeeklyEarning.WeeklyEarning;
                    }

                    ////inserting data into Withdraw table.
                    response = withdrawService.AddWithdraw(withdrawDataModel);
                    if (withdrawDataModel.IsVirtualWallet)
                    {
                        // condition to add wallet row corresponding to customer
                        var CustomerWallet = walletService.GetWalletByCustomerId(new Guid(withdrawDataModel.CustomerId));
                        if (CustomerWallet == null)
                        {
                            WithdrawTableDataModel WithdrawTableDataModel = new WithdrawTableDataModel();
                            WithdrawTableDataModel.CustomerId = withdrawDataModel.CustomerId;
                            WithdrawTableDataModel.AvailableBalance = 0;
                            var WalletResponse = walletService.AddCustomerWalletWithZero(WithdrawTableDataModel);
                        }

                        if (withdrawDataModel.IsSale == true)
                        {
                            withdrawDataModel.CustomerId = withdrawDataModel.SessionCustomerId;
                        }
                        var WalletDetail = walletService.GetWalletByCustomerId(new Guid(withdrawDataModel.CustomerId));
                        if (WalletDetail != null)
                        {
                            if (finalAmountAfterPenalty != 0 && isEnter == true)
                            {
                                withdrawDataModel.IsEarning = true;
                                withdrawDataModel.WithdrawAmount = finalAmountAfterPenalty;
                                response = withdrawService.AddWithdraw(withdrawDataModel);

                            }
                            if (withdrawDataModel.IsEarning == false && withdrawDataModel.IsSale == false && finalAmountAfterPenalty == 0 && isEnter == false)
                            {
                                withdrawDataModel.IsEarning = true;
                                withdrawDataModel.WithdrawAmount = CustomerProductDetails.WeeklyEarning;
                                finalAmountAfterPenalty = CustomerProductDetails.WeeklyEarning;
                                response = withdrawService.AddWithdraw(withdrawDataModel);
                            }
                            ////updating data into wallet table
                            //if (!withdrawDataModel.IsSale)
                            //{
                            //    float virtualAmount = (float)WalletDetail.AvailableBalance + withdrawAmountForAddInWallet + finalAmountAfterPenalty;
                            //    responsedata = walletService.UpdateWalletAmount(new Guid(withdrawDataModel.CustomerId), virtualAmount);

                            //    if (responsedata)
                            //    {
                            //        amount = virtualAmount;
                            //    }
                            //}
                            //else
                            //{
                            //    amount = (float)WalletDetail.AvailableBalance;
                            //}
                            if (!withdrawDataModel.IsSale)
                            {
                                amount = (float)WalletDetail.AvailableBalance;
                            }
                        }
                        //else
                        //{
                        //    ////inserting data into wallet table.
                        //    if (withdrawDataModel.IsSale == true)
                        //    {
                        //        withdrawDataModel.CustomerId = withdrawDataModel.SessionCustomerId;
                        //    }
                        //    responsedata = walletService.AddVirtualAmount(withdrawDataModel);
                        //    if (responsedata)
                        //    {
                        //        amount = withdrawDataModel.WithdrawAmount;
                        //    }
                        //}

                    }

                    if (response != null)
                    {
                        if (withdrawDataModel.IsVirtualWallet)
                        {
                            ////return result from service response.
                            string stWallet = resmanager.GetString("Api_DepositeInVirtualWallet", CultureInfo.GetCultureInfo(withdrawDataModel.Lang));
                            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stWallet, Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = amount.ToString() });
                        }
                        ////return result from service response.
                        string stDepsoiteAmount = resmanager.GetString("Api_DepsoiteAmount", CultureInfo.GetCultureInfo(withdrawDataModel.Lang));
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDepsoiteAmount, Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = amount.ToString() });
                        // return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "withdraw successfully inserted", Succeeded = true });

                    }
                    else
                    {
                        string stSomethingWentWrong = resmanager.GetString("Api_SomethingWentWorng", CultureInfo.GetCultureInfo(withdrawDataModel.Lang));
                        return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = stSomethingWentWrong, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    }
                }
                else
                {
                    string stSomethingWentWrong = resmanager.GetString("Api_SomethingWentWorng", CultureInfo.GetCultureInfo(withdrawDataModel.Lang));
                    return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = stSomethingWentWrong, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                string stSomethingWentWrong = resmanager.GetString("Api_SomethingWentWorng", CultureInfo.GetCultureInfo(withdrawDataModel.Lang));
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = stSomethingWentWrong, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
            }
        }

        #endregion

        #region GetInvestmentHistory

        /// <summary>
        /// This method for get all record from "withdarw" entity.
        /// </summary>
        /// <returns>Customer entity object value.</returns>
        [HttpGet]
        [Route("GetInvestmentHistory/{customerId}/{productId}/{lang}")]
        public HttpResponseMessage GetInvestmentHistory(string customerId, string productId, string lang)
        {
            try
            {
                var CustomerId = new Guid(customerId);
                var ProductId = new Guid(productId);
                var CustomerProductDetails = customerProductService.GetAllCustomerProductByProductIdCustomerId(CustomerId, ProductId);
                if (CustomerProductDetails != null)
                {
                    ////get withdraw list 
                    var financeCollection = withdrawService.GetAllFinanceInvestmentHistory(CustomerProductDetails.Id);

                    ////check object
                    if (financeCollection.Count > 0 && financeCollection != null)
                    {
                        ////dynamic list.
                        dynamic finances = new List<ExpandoObject>();


                        ////return response from withdraw service.
                        foreach (var financeDetail in financeCollection)
                        {
                            ////bind dynamic property.
                            dynamic finance = new ExpandoObject();
                            ////map ids
                            //finance.Id = financeDetail.Id;
                            finance.CreatedDate = ((EMonth)financeDetail.CreatedDate.Month).ToString();

                            //if (financeDetail.IsEarning == true)
                            //{
                            //    finance.WithDrawalFor = "Weekly Yield";
                            //}
                            //else if (financeDetail.IsSale == true)
                            //{
                            //    finance.WithDrawalFor = "Commission By Sale";
                            //}
                            //else 
                            if (financeDetail.IsVirtualWallet == true)
                            {
                                finance.WithDrawalFor = "Virtual Wallet";
                            }
                            else
                            {
                                finance.WithDrawalFor = "Back Account";
                            }
                            finance.WithdrawAmount = financeDetail.WithdrawAmount;
                            // finance.Status = financeDetail.Status;
                            //finance.RemainingDaysForDepositing = (finance.EstimatedDepositDate - DateTime.UtcNow).TotalDays;

                            //////set customers values in list.
                            finances.Add(finance);
                        }
                        ////return customers service 
                        //  return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK, (List<ExpandoObject>)finances);
                        string st = resmanager.GetString("Api_InvestInProduct", CultureInfo.GetCultureInfo(lang));
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = new ExpandoObject(), DataList = finances, ErrorInfo = "" });
                    }
                    else
                    {
                        string stDataNot = resmanager.GetString("Api_DataNotFound", CultureInfo.GetCultureInfo(lang));
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataNot, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = stDataNot });
                        //return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Data Not Found." });
                    }
                }
                else
                {
                    string stDataNot = resmanager.GetString("Api_DataNotFound", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataNot, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = stDataNot });
                    //return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Data Not Found." });
                }
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);
                logger.Error("Method: myinvsdasestment()", ex);
                logger.Info("Method: myinvessdsdatment()" + ex.StackTrace);
                ////return case of exception.
                //return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
                string stFailed = resmanager.GetString("Api_Failed", CultureInfo.GetCultureInfo(lang));
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stFailed, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        #endregion

        #region Payment using Credit,Wire,MiniMarket and Payment At Office
        /// <summary>
        /// Api for payment of investment by credit card/wire transfer/mini market/payment on office
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("InvesmentPayment")]
        public HttpResponseMessage InvesmentPayment(PaymentDataModel paymentDataModel)
        {
            CustomerProductDataModel customerProductData = new CustomerProductDataModel();
            try
            {
                string customerCreated_id = "";
                PromotionDataModel promotionDataModel = new PromotionDataModel();
                //Private key and merchant id for openpay 
                //OpenpayAPI openpayAPI = new OpenpayAPI("sk_71a7e47e1c3248b3a608e4feed96974c", "mqedj8cghx5dofntpqyg");
                OpenpayAPI openpayAPI = new OpenpayAPI(openPaySkId, openPayMrId);
                customerProductData.lang = paymentDataModel.lang;
                // condition to check remaining value exist or not(In case of multiple request)
                var product = productService.GetProductByProductId(new Guid(paymentDataModel.productId));
                //if (product.RemainingValueOfInvestment >= float.Parse(paymentDataModel.investment))
                //{
                var responseDetail = customerService.GetCustomerById(new Guid(paymentDataModel.customerId));
                //Code change for client gave to new marchent id 
                //Code comment for change by client
                var alreadyExist = customerProductService.GetAllCustomerProductByProductIdCustomerId(new Guid(paymentDataModel.customerId), new Guid(paymentDataModel.productId));
                if (alreadyExist == null)
                {
                    Customer customerOfOpenpay = new Customer();
                    customerOfOpenpay.Name = responseDetail.FirstName;
                    customerOfOpenpay.LastName = responseDetail.LastName;
                    customerOfOpenpay.Email = responseDetail.Email;
                    customerOfOpenpay.Address = new Address();
                    customerOfOpenpay.Address.Line1 = "line 1";
                    customerOfOpenpay.Address.PostalCode = "12355";
                    customerOfOpenpay.Address.City = "Indore";
                    customerOfOpenpay.Address.CountryCode = "MX";
                    customerOfOpenpay.Address.State = "MP";

                    Customer customerCreated = openpayAPI.CustomerService.Create(customerOfOpenpay);
                    customerCreated_id = customerCreated.Id;

                    //Assign value in customer product model object
                    customerProductData.Investment = paymentDataModel.investment;
                    customerProductData.ProductId = paymentDataModel.productId;
                    customerProductData.CustomerId = paymentDataModel.customerId.ToString();
                    customerProductData.PaymentStatus = paymentDataModel.paymentType == "CreditCard" ? true : false;
                    customerProductData.Status = "Pending";
                    customerProductData.lang = paymentDataModel.lang;
                    customerProductData.PaymentType = paymentDataModel.paymentType;
                    var paymentAmountValue = paymentDataModel.walletAmount;
                    customerProductData.WalletAmount = paymentAmountValue.ToString();

                    // for get value and bind into card model
                    float paymetWalletAmount = 0;
                    //Code for deduct the investment price
                    ListDictionary replacements = new ListDictionary();
                    var CheckInvestmentAmount = false;
                    if (paymentDataModel.lastRemainingInvestmentAmountStatus)
                    {
                        CheckInvestmentAmount = true;
                    }
                    else if ((float)(Convert.ToDecimal(customerProductData.Investment)) >= product.MinPrice)
                    {
                        CheckInvestmentAmount = true;
                    }
                    else
                    {
                        string stDataNot = resmanager.GetString("Api_MinPrice", CultureInfo.GetCultureInfo(customerProductData.lang));
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataNot + " $" + product.MinPrice, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = stDataNot });
                    }

                    if (CheckInvestmentAmount)
                    {
                        if ((float)(Convert.ToDecimal(customerProductData.Investment)) <= product.MaxPrice)
                        {
                            if (product.RemainingValueOfInvestment >= float.Parse(customerProductData.Investment))
                            {
                                // inserting data into activity table
                                var response = customerProductService.InvestInProduct(customerProductData);

                                if (response)
                                {
                                    // new investment notification alert(Admin)
                                    ////inserting data into promotion datamodel.
                                    promotionDataModel.Description = "Purchase of " + product.Name + " investment plan by " + responseDetail.FirstName + " " + responseDetail.LastName;
                                    promotionDataModel.DescriptionSpanish = "Purchase of " + product.Name + " investment plan by " + responseDetail.FirstName + " " + responseDetail.LastName;
                                    promotionDataModel.To = null;
                                    promotionDataModel.PromotionType = "Alert on Web & App";
                                    promotionDataModel.Subject = "Purchase of investment";
                                    promotionDataModel.SubjectSpanish = "Purchase of investment";
                                    promotionDataModel.Url = projectPathUrl + projectName + "/CustomerManagement/InvestmentsList/" + responseDetail.Id;

                                    ////inserting data into promotion table.
                                    var promotionResponse = promotionService.AddPromotion(promotionDataModel);

                                    var walletDetails = walletService.GetWalletByCustomerId(new Guid(paymentDataModel.customerId));
                                    if (paymentDataModel.paymentType == "Wallet")
                                    {
                                        if (paymentDataModel.walletAmount >= float.Parse(paymentDataModel.investment))
                                        {
                                            paymentDataModel.walletAmount = (float)walletDetails.AvailableBalance - float.Parse(paymentDataModel.investment);
                                            var dataresponse = walletService.UpdateWalletAmount(new Guid(paymentDataModel.customerId), paymentDataModel.walletAmount);

                                        }
                                        dynamic paymentWalletAmount = new ExpandoObject();
                                        paymentWalletAmount.walletAmount = paymentDataModel.walletAmount;
                                        string st = resmanager.GetString("Api_Successfully", CultureInfo.GetCultureInfo(paymentDataModel.lang));
                                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = paymentWalletAmount, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                                    }
                                    else
                                    {
                                        //Code for Wallet Amount is not null then update 
                                        if (paymentDataModel.walletAmount != 0)
                                        {

                                            if ((float)walletDetails.AvailableBalance >= paymentDataModel.walletAmount)
                                            {
                                                paymetWalletAmount = (float)walletDetails.AvailableBalance - paymentDataModel.walletAmount;
                                                var dataresponse = walletService.UpdateWalletAmount(new Guid(paymentDataModel.customerId), paymetWalletAmount);
                                            }
                                        }

                                        if (paymentDataModel.paymentType == "CreditCard")
                                        {
                                            //// updating remaining investment                                          
                                            if (customerProductData.PaymentStatus == true)
                                            {
                                                // send email notification to customer for lost amount 
                                                var customer = customerService.GetCustomerById(new Guid(customerProductData.CustomerId));
                                                if (customer.CustomerReferalId != null)
                                                {
                                                    var referalCustomerDetail = customerService.GetCustomerById((customer.CustomerReferalId.Value));
                                                    double TotalSaleEarning = Convert.ToDouble(customerProductData.Investment) * Convert.ToDouble(product.PercentSaleEarning);

                                                    // update sale percent
                                                    var result = customerProductService.UpdateSaleEarning(new Guid(customerProductData.CustomerId), new Guid(customerProductData.ProductId), (float)TotalSaleEarning);
                                                    if (result != true)
                                                    {
                                                        string stDataNot = resmanager.GetString("Api_SaleEarningNotUpdated", CultureInfo.GetCultureInfo(customerProductData.lang));
                                                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataNot, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = stDataNot });
                                                    }

                                                    if (customer.IsActive != true)
                                                    {
                                                        replacements = new ListDictionary { { "<%FirstName%>", referalCustomerDetail.FirstName } };
                                                        replacements.Add("<%Subject%>", "Referal Earning Lost Due To InActive");
                                                        replacements.Add("<%Amount%>", TotalSaleEarning);
                                                        replacements.Add("<%LastName%>", referalCustomerDetail.LastName);
                                                        replacements.Add("<%OrganisationName%>", "SplitDeals");
                                                        ////step:1
                                                        ////add email data
                                                        var sendEmail = emailService.AddEmailData(referalCustomerDetail.Id.ToString(), referalCustomerDetail.Email, string.Empty, string.Empty, string.Empty, EmailTemplatesHelper.AmountLostEmail, replacements);

                                                        ////step:2
                                                        ////send email.
                                                        if (sendEmail != null)
                                                        {
                                                            ////Code for Sending Email
                                                            emailService.SendEmailAsync(referalCustomerDetail.Id, referalCustomerDetail.Email, string.Empty, string.Empty, referalCustomerDetail.Email, EmailTemplatesHelper.AmountLostEmail, "", replacements);
                                                        }
                                                    }
                                                }
                                                openpayAPI.Production = false;
                                                ChargeRequest request = new ChargeRequest();
                                                request.Method = "card";
                                                request.SourceId = paymentDataModel.id;
                                                request.Description = "Payment from the credit card";
                                                request.Amount = Convert.ToDecimal(paymentDataModel.investment);
                                                request.DeviceSessionId = paymentDataModel.device_session_id;
                                                Charge charge = openpayAPI.ChargeService.Create(customerCreated_id, request);
                                                CardChargeDataModel obj = new CardChargeDataModel();
                                                obj.chargeid = charge.Id;
                                                obj.status = charge.Status;

                                                if (obj.status == "completed")
                                                {
                                                    ////bind dynamic property.
                                                    dynamic paymentChargeDetial = new ExpandoObject();
                                                    paymentChargeDetial.chargeid = obj.chargeid;
                                                    paymentChargeDetial.status = obj.status;
                                                    paymentChargeDetial.walletAmount = paymetWalletAmount;
                                                    string st = resmanager.GetString("Api_Successfully", CultureInfo.GetCultureInfo(paymentDataModel.lang));
                                                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = paymentChargeDetial, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                                                    // return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Successfully Updated" });
                                                }
                                            }
                                        }
                                        else if (paymentDataModel.paymentType == "MiniMarket")
                                        {
                                            //Code for mini market
                                            openpayAPI.Production = false;
                                            ChargeRequest request = new ChargeRequest();
                                            request.Method = "store";
                                            request.Description = "Payment from the minimarket";
                                            if (paymentDataModel.walletAmount != 0)
                                            {
                                                float deductedAmount = float.Parse(paymentDataModel.investment) - paymentDataModel.walletAmount;
                                                request.Amount = Convert.ToDecimal(deductedAmount);
                                            }
                                            else
                                            {
                                                request.Amount = Convert.ToDecimal(paymentDataModel.investment);
                                            }
                                            Charge charge = openpayAPI.ChargeService.Create(customerCreated_id, request);
                                            CardChargeDataModel obj = new CardChargeDataModel();
                                            obj.chargeid = charge.Id;
                                            obj.status = charge.Status;
                                            obj.barCodeUrl = charge.PaymentMethod.BarcodeURL;

                                            if (obj.status == "in_progress")
                                            {
                                                ////bind dynamic property.
                                                dynamic paymentChargeDetial = new ExpandoObject();
                                                paymentChargeDetial.chargeid = obj.chargeid;
                                                paymentChargeDetial.status = obj.status;
                                                paymentChargeDetial.barCodeUrl = obj.barCodeUrl;
                                                paymentChargeDetial.barCode = charge.PaymentMethod.Reference;
                                                paymentChargeDetial.walletAmount = paymetWalletAmount;
                                                CustomerProductDataModel objcustomerProduct = new CustomerProductDataModel();

                                                objcustomerProduct.CustomerId = paymentDataModel.customerId;
                                                objcustomerProduct.ProductId = paymentDataModel.productId;
                                                objcustomerProduct.BarCodeUrl = obj.barCodeUrl;
                                                objcustomerProduct.BarCode = charge.PaymentMethod.Reference;
                                                //objcustomerProduct.BarCodeImage = imageUploadStatus
                                                var responseUpdate = customerProductService.UpdateCustomerProductWithBarCode(objcustomerProduct);
                                                string st = resmanager.GetString("Api_Successfully", CultureInfo.GetCultureInfo(paymentDataModel.lang));
                                                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = paymentChargeDetial, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                                            }
                                        }
                                        else
                                        {
                                            ////bind dynamic property.
                                            dynamic paymentChargeDetial = new ExpandoObject();
                                            paymentChargeDetial.walletAmount = paymetWalletAmount;
                                            string st = resmanager.GetString("Api_Successfully", CultureInfo.GetCultureInfo(paymentDataModel.lang));
                                            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = paymentChargeDetial, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string stDataNot = resmanager.GetString("Api_InvestmentAmountNotGreaterThanRemaining", CultureInfo.GetCultureInfo(customerProductData.lang));
                                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataNot + " $" + product.RemainingValueOfInvestment, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = stDataNot });
                            }
                        }
                        else
                        {
                            string stDataNot = resmanager.GetString("Api_MaxPrice", CultureInfo.GetCultureInfo(customerProductData.lang));
                            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataNot + " $" + product.MaxPrice, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = stDataNot });
                        }
                    }
                }
                else
                {
                    ////return result from service response.
                    string alreadyExisted = resmanager.GetString("Api_AlreadyExisted", CultureInfo.GetCultureInfo(customerProductData.lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = alreadyExisted, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }

                ////return result from service response.
                string stSomethingWrong = resmanager.GetString("Api_SomethingWentWorng", CultureInfo.GetCultureInfo(customerProductData.lang));
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stSomethingWrong, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = stSomethingWrong });
                //}
                //else
                //{
                //    string stDataNot = resmanager.GetString("Api_InvestmentAmountNotGreaterThanRemaining", CultureInfo.GetCultureInfo(customerProductData.lang));
                //    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataNot + " $" + product.RemainingValueOfInvestment, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = stDataNot });
                //}

            }

            catch (Exception ex)
            {

                var deleteCustomerProductData = customerProductService.DeleteCustomerProductByCustomerIdProductId(new Guid(paymentDataModel.customerId), new Guid(paymentDataModel.productId));

                if (deleteCustomerProductData)
                {
                    var walletDetails = walletService.GetWalletByCustomerId(new Guid(paymentDataModel.customerId));
                    if (paymentDataModel.walletAmount >= float.Parse(paymentDataModel.investment))
                    {
                        var virtualWalletAmount = paymentDataModel.walletAmount + float.Parse(paymentDataModel.investment);
                        var dataresponse = walletService.UpdateWalletAmount(new Guid(paymentDataModel.customerId), paymentDataModel.walletAmount);
                    }
                    else
                    {
                        var virtualWalletAmount = (float)walletDetails.AvailableBalance + paymentDataModel.walletAmount;
                        var responsedata = walletService.UpdateWalletAmount(new Guid(paymentDataModel.customerId), virtualWalletAmount);
                    }
                }
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

            }
        }

        #endregion

        #region Withdraw Amount From Wallet By CustomerId 

        /// <summary>
        /// Api for payment Withdraw Amount From Wallet By CustomerId
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("WithdrawAmountFromWalletByCustomerId")]
        public HttpResponseMessage WithdrawAmountFromWalletByCustomerId(WithdrawDataModel withdrawDataModel)
        {
            try
            {
                withdrawDataModel.Status = withdrawDataModel.Status == "" ? null : withdrawDataModel.Status;
                var wallet = walletService.GetWalletByCustomerId(new Guid(withdrawDataModel.CustomerId));
                PromotionDataModel promotionDataModel = new PromotionDataModel();

                if (wallet != null && (float)wallet.AvailableBalance > 0)
                {
                    if ((float)wallet.AvailableBalance >= withdrawDataModel.WithdrawAmount)
                    {
                        float newWalletAmount = (float)wallet.AvailableBalance - withdrawDataModel.WithdrawAmount;
                        var response = walletService.UpdateWalletAmount(new Guid(withdrawDataModel.CustomerId), newWalletAmount);
                        if (response)
                        {
                            withdrawDataModel.IsVirtualWallet = true;
                            withdrawDataModel.CustomerId = withdrawDataModel.CustomerId;
                            var responseData = withdrawService.AddWithdraw(withdrawDataModel);

                            // admin weekly yield notification alert
                            ////inserting data into promotion datamodel.
                            promotionDataModel.Description = "Wallet withdraw request from " + wallet.Customer.FirstName + " " + wallet.Customer.LastName;
                            promotionDataModel.DescriptionSpanish = "Wallet withdraw request from " + wallet.Customer.FirstName + " " + wallet.Customer.LastName;
                            promotionDataModel.To = null;
                            promotionDataModel.PromotionType = "Alert on Web & App";
                            promotionDataModel.Subject = "Wallet withdraw";
                            promotionDataModel.SubjectSpanish = "Wallet withdraw";
                            promotionDataModel.Url = projectPathUrl + projectName + "/FinanceManagement/WithdrawalRequest";

                            ////inserting data into promotion table.
                            var adminPromotionResponse = promotionService.AddPromotion(promotionDataModel);

                            ////return result from service response.
                            string st = resmanager.GetString("Api_Successfully", CultureInfo.GetCultureInfo(withdrawDataModel.Lang));
                            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                        }
                        else
                        {
                            string stData = resmanager.GetString("Api_SomethingWentWorng", CultureInfo.GetCultureInfo(withdrawDataModel.Lang));
                            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stData, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "false" });
                        }
                    }
                    string stDataRecord = resmanager.GetString("Api_WithrawAmountForWallet", CultureInfo.GetCultureInfo(withdrawDataModel.Lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataRecord, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "false" });
                }
                string stDataNot = resmanager.GetString("Api_WalletAmountZero", CultureInfo.GetCultureInfo(withdrawDataModel.Lang));
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataNot, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "false" });
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                string stSomethingWentWorng = resmanager.GetString("Api_SomethingWentWorng", CultureInfo.GetCultureInfo(withdrawDataModel.Lang));
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = stSomethingWentWorng, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "false" });

            }
        }

        #endregion
    }
}

