using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TradeSystem.Framework.Entities;
using TradeSystem.Service;
using TradeSystem.Utils.Models;
using TradeSystem.WebApi.Controllers;

namespace TradeSystem.MVCWeb.Controllers
{
    [Authorize]
    [RoutePrefix("api/sale")]
    public class SaleApiController : BaseApiController
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        //private Framework.Identity.ApplicationUserManager userManager;
        ICustomerService customerService;
        ICustomerProductService customerProductService;

        private const string LocalLoginProvider = "Local";

        ////Initialized Sale Api Controller Constructor.
        public SaleApiController(ICustomerService _customerService, ICustomerProductService _customerProductService)
        {
            customerService = _customerService;
            customerProductService = _customerProductService;
        }
        #endregion

        /// <summary>
        /// This method for get all customer sale record from "customerProduct" entity.
        /// </summary>
        /// <returns>CustomerProduct entity object value.</returns>
        [HttpGet]
        [Route("GetAllSalesByCustomerReferalId/{customerId}/{lang}/{filterKey}/{filterValue}")]
        public HttpResponseMessage GetAllSalesByCustomerReferalId(string customerId, string lang, string filterKey, string filterValue)
        {
            try
            {
                ////get product, customer and customerProduct list 
                var customerCollection = customerService.GetAllReferalCustomerByCustomerId(new Guid(customerId)).OrderByDescending(x => x.CreatedDate).ToList();
                if (filterKey == "date")
                {
                    customerCollection = customerCollection.Where(x => x.CreatedDate.ToString("MM-dd-yyyy") == filterValue).ToList();
                }
                else if (filterKey == "status")
                {
                    customerCollection = customerCollection.Where(x => x.IsActive.ToString() == filterValue).ToList();
                }
                ////check object
                if (customerCollection.Count > 0 && customerCollection != null)
                {
                    ////dynamic list.
                    dynamic customers = new List<ExpandoObject>();

                    ////return response from product service.
                    foreach (var customerSaleDetail in customerCollection)
                    {
                        ////bind dynamic property.
                        dynamic customer = new ExpandoObject();
                        customer.Id = customerSaleDetail.Id;
                        customer.CreatedDate = customerSaleDetail.CreatedDate.ToString("MM-dd-yyyy");
                        customer.CustomerName = customerSaleDetail.FirstName + " " + customerSaleDetail.MiddleName + " " + customerSaleDetail.LastName;
                        customer.Status = customerSaleDetail.IsActive;
                        ////get product, customer and customerProduct list 
                        // var customerProductCollection = customerProductService.GetAllCustomerProductByCustomerId(customer.Id);


                        ////get product, customer and customerProduct list 
                        List<CustomerProduct> customerProductCollection = customerProductService.GetAllCustomerProductByCustomerId(customer.Id);
                        ////bind dynamic property.
                        dynamic customerProduct = new ExpandoObject();
                        double tempInvestment = 0;
                        double tempSaleEarning = 0;
                        ////check object
                        if (customerProductCollection.Count > 0 && customerProductCollection != null)
                        {
                            ////return response from product service.
                            foreach (var customerProductSaleDetail in customerProductCollection)
                            {
                                var customerSale = customerProductSaleDetail.Withdraws.Where(x => x.IsSale == true).ToList();
                                tempInvestment += customerProductSaleDetail.Investment;
                                //tempSaleEarning += customerProductSaleDetail.SaleEarning;
                                if (customerSale.Count > 0)
                                {
                                    tempSaleEarning += customerSale[0].WithdrawAmount;
                                }
                                else
                                {
                                    //customerSale[0].WithdrawAmount = 0;
                                    tempSaleEarning += 0;
                                }
                            }

                        }
                        customer.Investment = tempInvestment.ToString("0.00");
                        customer.InvestmentCustomer = tempInvestment.ToString("0.00");
                        customer.SaleEarning = tempSaleEarning.ToString("0.00");
                        customer.SaleEarningCustomer = tempSaleEarning.ToString("0.00");
                        ////set customers values in list.
                        customers.Add(customer);
                    }
                    ////return all service 
                    string st = resmanager.GetString("Api_MySales", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = new ExpandoObject(), DataList = customers, ErrorInfo = "" });
                    //return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK, (List<ExpandoObject>)customers);
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
                string stFailed = resmanager.GetString("Api_Failed", CultureInfo.GetCultureInfo(lang));
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stFailed, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
                ////return case of exception.
                // return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
            }
        }

        /// <summary>
        /// This method for get all customer sale record from "customerProduct" entity.
        /// </summary>
        /// <returns>CustomerProduct entity object value.</returns>
        [HttpGet]
        [Route("GetAllCommisionSalesByCustomerReferalId/{customerId}/{lang}/{filterKey}/{filterValue}")]
        public HttpResponseMessage GetAllCommisionSalesByCustomerReferalId(string customerId, string lang, string filterKey, string filterValue)
        {
            try
            {
                List<Withdraw> CommissionStatus = new List<Withdraw>(); //For status of commission table
                ////get product, customer and customerProduct list 
                var customerCollection = customerService.GetAllReferalCustomerByCustomerId(new Guid(customerId));

                ////check object
                if (customerCollection.Count > 0 && customerCollection != null)
                {
                    ////dynamic list.
                    dynamic customers = new List<ExpandoObject>();

                    ////return response from product service.
                    foreach (var customerSaleDetail in customerCollection)
                    {
                        ////get product, customer and customerProduct list 
                        var customerProductCollection = customerProductService.GetAllCustomerProductByCustomerId(customerSaleDetail.Id).Where(x => x.Status != "Reject").OrderByDescending(y => y.CreatedDate).ToList();
                        //filter for mobile
                        if (filterKey == "date")
                        {
                            customerProductCollection = customerProductCollection.Where(x => x.CreatedDate.ToString("MM-dd-yyyy") == filterValue).ToList();
                        }
                        else if (filterKey == "status")
                        {
                            customerProductCollection = customerProductCollection.Where(x => x.Status.ToString() == filterValue).ToList();
                        }
                        ////bind dynamic property.
                        dynamic customerProduct = new ExpandoObject();
                        ////check object
                        if (customerProductCollection.Count > 0 && customerProductCollection != null)
                        {
                            ////return response from product service.
                            foreach (var customerProductSaleDetail in customerProductCollection)
                            {
                                ////bind dynamic property.
                                dynamic customer = new ExpandoObject();
                                customer.ProductId = customerProductSaleDetail.ProductId;
                                customer.CustomerId = customerProductSaleDetail.CustomerId;
                                customer.CreatedDate = customerProductSaleDetail.CreatedDate.ToString("MM-dd-yyyy");
                                customer.CustomerName = customerSaleDetail.FirstName + " " + customerSaleDetail.MiddleName + " " + customerSaleDetail.LastName;
                                customer.WithdrawStatus = customerProductSaleDetail.WithdrawStatus;
                                // Convert.ToDateTime(product.WeeklyFromWithdrawDay, dtinfo).AddDays(withdrawDay);  
                                if (customerProductSaleDetail.StartCalculationDate != null)
                                {
                                    DateTime saleWithdrawdate = Convert.ToDateTime(customerProductSaleDetail.StartCalculationDate).AddDays(Convert.ToInt32(customerProductSaleDetail.Product.SaleWithdrawDay));
                                    customer.SaleWithdrawDate = Convert.ToDateTime(saleWithdrawdate.ToString("MM/dd/yyyy"), CultureInfo.InvariantCulture);
                                    customer.CurrentTodayDate = Convert.ToDateTime(DateTime.UtcNow.ToString("MM/dd/yyyy"), CultureInfo.InvariantCulture);
                                }
                                //dynamic message for "en" and "es"
                                string stPendingtoWithdraw = resmanager.GetString("PendingtoWithdraw", CultureInfo.GetCultureInfo(lang));
                                string stDepositeToBank = resmanager.GetString("DepositeToBank", CultureInfo.GetCultureInfo(lang));
                                string stDepositeToWallet = resmanager.GetString("DepositeToWallet", CultureInfo.GetCultureInfo(lang));
                                string stReachedCommissionLimit = resmanager.GetString("ReachedCommissionLimit", CultureInfo.GetCultureInfo(lang));


                                if (customerProductSaleDetail.SaleEarningStatus == "Reached Commission Limit")
                                {
                                    customer.Status = stReachedCommissionLimit;
                                }
                                else
                                {
                                    customer.Status = stPendingtoWithdraw;
                                }
                                CommissionStatus = customerProductSaleDetail.Withdraws.Where(x => x.CustomerProductId == customerProductSaleDetail.Id && x.IsSale == true && x.IsVirtualWallet == true).ToList();
                                {
                                    if (CommissionStatus.Count != 0)
                                        customer.Status = stDepositeToWallet;
                                }
                                CommissionStatus = customerProductSaleDetail.Withdraws.Where(x => x.CustomerProductId == customerProductSaleDetail.Id && x.IsSale == true && x.IsVirtualWallet == false).ToList();
                                {
                                    if (CommissionStatus.Count != 0)
                                        customer.Status = stDepositeToBank;
                                }
                                customer.InvestmentName = customerProductSaleDetail.Product.Name != null ? customerProductSaleDetail.Product.Name : "-";
                                if (customerProductSaleDetail.SaleEarning != 0)
                                {
                                    if (customerProductSaleDetail.SaleEarning == float.Parse("0.00"))
                                    {
                                        customer.SaleEarningCustomer = 0;
                                    }
                                    customer.SaleEarningCustomer = customerProductSaleDetail.SaleEarning.ToString("0.00");
                                }
                                else
                                {
                                    customer.SaleEarningCustomer = 0;
                                }
                                if (!customerProductSaleDetail.WithdrawStatus && customerProductSaleDetail.SaleEarning != 0 && customer.SaleWithdrawDate.Date < customer.CurrentTodayDate.Date)
                                {
                                    customer.showWithdraw = true;
                                }
                                else
                                {
                                    customer.showWithdraw = false;
                                }

                                ////set customers values in list.
                                customers.Add(customer);
                            }

                        }
                    }
                    ////return all service 
                    string st = resmanager.GetString("Api_MySaleCommission", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = new ExpandoObject(), DataList = customers, ErrorInfo = "" });
                    //return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK, (List<ExpandoObject>)customers);
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
                string stFailed = resmanager.GetString("Api_Failed", CultureInfo.GetCultureInfo(lang));
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stFailed, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
                ////return case of exception.
                // return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
            }
        }
    }
}
