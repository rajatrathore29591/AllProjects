using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TradeSystem.Framework.Entities;
using TradeSystem.Framework.Identity;
using TradeSystem.Service;
using TradeSystem.Utils;
using TradeSystem.Utils.Models;

namespace TradeSystem.MVCWeb.Controllers
{
    [Authorize]
    [RoutePrefix("api/report")]
    public class ReportApiController : ApiController
    {
        #region Dependencies Injection with initialization
        ICustomerService customerService;
        IProductService productService;
        ICustomerProductService customerProductService;
        IWithdrawService withDrawService;
        ITicketService ticketService;

        //Constructor of Report Api
        public ReportApiController(ICustomerService _customerService, IProductService _productService, ICustomerProductService _customerProductService, IWithdrawService _withDrawService,ITicketService _ticketService)
        {
            customerService = _customerService;
            productService = _productService;
            customerProductService = _customerProductService;
            withDrawService = _withDrawService;
            ticketService = _ticketService;
        }
        #endregion

        /// <summary>
        /// all five reports of customer managed by this api
        /// </summary>
        /// <param name="customerReportObj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAllCustomerReport")]
        public HttpResponseMessage GetAllCustomerReport(CustomerReportDataModel customerReportObj)
        {
            try
            {
                List<Customer> customerCollection = new List<Customer>();

                ////dynamic list.
                dynamic customerDetails = new List<ExpandoObject>();

                ////get customer list with country and state filter
                if (customerReportObj.ReportType == "CountryState")
                {
                    customerCollection = customerService.GetAllCustomer().Where(x => x.CountryId == new Guid(customerReportObj.CountryId) && x.StateId == new Guid(customerReportObj.StateId)).ToList();
                    ////check object
                    if (customerCollection.Count > 0 && customerCollection != null)
                    {
                        ////return response from customer service.
                        foreach (var customerDetail in customerCollection)
                        {
                            dynamic customer = new ExpandoObject();
                            customer.CreatedDate = customerDetail.CreatedDate;
                            customer.Id = customerDetail.Id;
                            customer.Name = customerDetail.FirstName + " " + customerDetail.LastName;
                            customer.TotalValueOfInvestment = customerDetail.CustomerProducts.Sum(x => x.Investment);
                            customer.TotalValueOfInvestmentFloat = customerDetail.CustomerProducts.Sum(x => x.Investment);
                            customer.CountryName = customerDetail.Country.CountryName;
                            customer.StateName = customerDetail.State.StateName;
                            customer.ReportType = "CountryState";
                            ////set all values in list.
                            customerDetails.Add(customer);
                        }
                        ////return all service 
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "successfully record fetched", Succeeded = true, DataObject = new ExpandoObject(), DataList = customerDetails, ErrorInfo = "" });
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "No Data Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "some thing went wrong" });
                    }
                }
                // get customer investment detail by money invested filter
                if (customerReportObj.ReportType == "MoneyInvested")
                {
                    var customerProductCollection = customerProductService.GetAllCustomerProduct();
                    var customerProductCollectionData = (from customerData in customerService.GetAllCustomer()
                                                         join customerProductData in customerProductCollection on customerData.Id equals customerProductData.CustomerId into g
                                                         where g.Sum(s => s.Investment) >= float.Parse(customerReportObj.From) && g.Sum(s => s.Investment) <= float.Parse(customerReportObj.To)
                                                         select new
                                                         {
                                                             TotalInvestment = g.Sum(x => x.Investment),
                                                             customerObj = customerData
                                                         }).ToList();
                    ////check object
                    if (customerProductCollectionData.Count > 0 && customerProductCollectionData != null)
                    {

                        ////return response from customer service.
                        foreach (var customerDetail in customerProductCollectionData)
                        {
                            dynamic customer = new ExpandoObject();
                            customer.CreatedDate = customerDetail.customerObj.CreatedDate;
                            customer.Id = customerDetail.customerObj.Id;
                            customer.Name = customerDetail.customerObj.FirstName + " " + customerDetail.customerObj.LastName;
                            customer.TotalValueOfInvestment = customerDetail.TotalInvestment;
                            customer.CountryName = customerDetail.customerObj.Country.CountryName;
                            customer.StateName = customerDetail.customerObj.State.StateName;
                            customer.ReportType = "MoneyInvested";
                            ////set all values in list.
                            customerDetails.Add(customer);
                        }
                        ////return all service 
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "successfully record fetched", Succeeded = true, DataObject = new ExpandoObject(), DataList = customerDetails, ErrorInfo = "" });
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "No Data Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "some thing went wrong" });
                    }
                }
                // get customer withdrawal detail by money withdrawal filter
                if (customerReportObj.ReportType == "MoneyWithdrawal")
                {
                    var withDrawCollection = withDrawService.GetAllFinanceList();
                    var customerCollectionData = (from customerData in customerService.GetAllCustomer()
                                                  join withDrawData in withDrawCollection on customerData.Id equals withDrawData.CustomerId into g
                                                  where g.Sum(s => s.WithdrawAmount) >= float.Parse(customerReportObj.From) && g.Sum(s => s.WithdrawAmount) <= float.Parse(customerReportObj.To)
                                                  select new
                                                  {
                                                      WithDrawAmount = g.Sum(x => x.WithdrawAmount),
                                                      customerObj = customerData
                                                  }).ToList();
                    ////check object
                    if (customerCollectionData.Count > 0 && customerCollectionData != null)
                    {

                        ////return response from customer service.
                        foreach (var customerDetail in customerCollectionData)
                        {
                            dynamic customer = new ExpandoObject();
                            customer.CreatedDate = customerDetail.customerObj.CreatedDate;
                            customer.Id = customerDetail.customerObj.Id;
                            customer.Name = customerDetail.customerObj.FirstName + " " + customerDetail.customerObj.LastName;
                            customer.TotalValueOfInvestment = Decimal.Round(Convert.ToDecimal(customerDetail.WithDrawAmount), 2); 
                            customer.CountryName = customerDetail.customerObj.Country.CountryName;
                            customer.StateName = customerDetail.customerObj.State.StateName;
                            customer.ReportType = "MoneyWithdrawal";
                            ////set all values in list.
                            customerDetails.Add(customer);
                        }
                        ////return all service 
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "successfully record fetched", Succeeded = true, DataObject = new ExpandoObject(), DataList = customerDetails, ErrorInfo = "" });
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "No Data Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "some thing went wrong" });
                    }
                }
                // get customer withdrawal detail by premature withdrawal filter
                if (customerReportObj.ReportType == "PrematureWithdrawal")
                {
                    var productCollection = productService.GetAllProduct();
                    var customerCollectionData = (from customerProductData in customerProductService.GetAllCustomerProduct().Where(z => z.Status == "Completed")
                                                  join productData in productCollection on customerProductData.ProductId equals productData.Id
                                                  where customerProductData.InvestmentWithdrawDate <= productData.InvestmentWithdrawDate
                                                  select new
                                                  {
                                                      productObj = productData,
                                                      customerObj = customerProductData
                                                  }).ToList();
                    ////check object
                    if (customerCollectionData.Count > 0 && customerCollectionData != null)
                    {

                        ////return response from customer service.
                        foreach (var customerDetail in customerCollectionData)
                        {
                            dynamic customer = new ExpandoObject();
                            customer.CreatedDate = customerDetail.productObj.InvestmentWithdrawDate;
                            customer.CustomerWithdrawDate = customerDetail.customerObj.InvestmentWithdrawDate;
                            customer.Id = customerDetail.customerObj.Id;
                            customer.Name = customerDetail.customerObj.Customer.FirstName + " " + customerDetail.customerObj.Customer.LastName;
                            customer.ProductName = customerDetail.productObj.Name;
                            customer.TotalValueOfInvestment = customerDetail.customerObj.Investment;
                            customer.ReportType = "PrematureWithdrawal";
                            ////set all values in list.
                            customerDetails.Add(customer);
                        }
                        ////return all service 
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "successfully record fetched", Succeeded = true, DataObject = new ExpandoObject(), DataList = customerDetails, ErrorInfo = "" });
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "No Data Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "some thing went wrong" });
                    }
                }
                // get all referal customers list
                if (customerReportObj.ReportType == "Referals")
                {
                    // getting referal customers list and putting group by on customer referal id
                    var referalCustomer = customerService.GetAllCustomer().Where(x => x.CustomerReferalId != null);
                    var customerCollectionData = (from customerObj in referalCustomer
                                                  group customerObj by customerObj.CustomerReferalId into g
                                                  let count = g.Count()
                                                  select new
                                                  {
                                                      Count = count,
                                                      Id = g.Key,
                                                  }).ToList();

                    ////check object
                    if (customerCollectionData.Count > 0 && customerCollectionData != null)
                    {
                        ////return response from customer service.
                        foreach (var customerDetail in customerCollectionData)
                        {
                            var customerInfo = customerService.GetCustomerById(customerDetail.Id.Value);
                            dynamic customer = new ExpandoObject();
                            customer.CreatedDate = customerInfo.CreatedDate;
                            customer.Id = customerInfo.Id;
                            customer.Name = customerInfo.FirstName + " " + customerInfo.MiddleName + " " + customerInfo.LastName;
                            customer.CountryName = customerInfo.Country.CountryName;
                            customer.StateName = customerInfo.State.StateName;
                            customer.ReferalsCount = customerDetail.Count;
                            customer.ReportType = "Referals";
                            ////set all values in list.
                            customerDetails.Add(customer);
                        }
                        ////return all service 
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "successfully record fetched", Succeeded = true, DataObject = new ExpandoObject(), DataList = customerDetails, ErrorInfo = "" });
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "No Data Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "some thing went wrong" });
                    }
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "some thing went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "some thing went wrong" });

            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);
                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = ex.Message, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// get customer investment detail in particular product 
        /// </summary>
        /// <param name="productReportObj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAllProductReport")]
        public HttpResponseMessage GetAllProductReport(CustomerReportDataModel productReportObj)
        {
            try
            {
                List<Customer> productCollection = new List<Customer>();
                ////dynamic list.
                dynamic customerDetails = new List<ExpandoObject>();
                ////get customer list with on the basis of product
                var customerProductCollection = customerProductService.GetAllCustomerProduct().Where(x => x.ProductId == new Guid(productReportObj.ProductId) && x.Status != "Reject").ToList();
                ////check object
                if (customerProductCollection.Count > 0 && customerProductCollection != null)
                {
                    ////return response from customer service.
                    foreach (var customerDetail in customerProductCollection)
                    {
                        dynamic customer = new ExpandoObject();
                        customer.CreatedDate = customerDetail.CreatedDate;
                        customer.Id = customerDetail.Id;
                        var a = customerDetail.Customer.LastName;
                        customer.Name = customerDetail.Customer.FirstName + " " + customerDetail.Customer.LastName;
                        customer.TotalValueOfInvestment = customerDetail.Investment;
                        customer.CountryName = customerDetail.Customer.Country.CountryName;
                        customer.StateName = customerDetail.Customer.State.StateName;
                        customer.ReportType = "Product";
                        ////set all values in list.
                        customerDetails.Add(customer);
                    }
                    ////return all service 
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "successfully record fetched", Succeeded = true, DataObject = new ExpandoObject(), DataList = customerDetails, ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "No Data Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "some thing went wrong" });
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "some thing went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "some thing went wrong" });

            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);
                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = ex.Message, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// all withdraw data basis of date filters
        /// </summary>
        /// <param name="withdrawReportObj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAllWithdrawReport")]
        public HttpResponseMessage GetAllWithdrawReport(CustomerReportDataModel withdrawReportObj)
        {
            try
            {
                List<Withdraw> withdrawCollection = new List<Withdraw>();
                DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                dtinfo.ShortDatePattern = "MM/dd/yyyy";
                dtinfo.DateSeparator = "/";

                ////dynamic list.
                dynamic withdrawDetails = new List<ExpandoObject>();
                ////get customer list with on he basis of product
                if(withdrawReportObj.ReportType == "WeeklyWithdraw")
                {
                    withdrawCollection = withDrawService.GetAllFinanceList().Where(x => x.CreatedDate >= Convert.ToDateTime(withdrawReportObj.From) && x.CreatedDate <= Convert.ToDateTime(withdrawReportObj.To) && x.IsEarning == true && x.CustomerProductId != null).ToList();
                    
                }
                if (withdrawReportObj.ReportType == "SaleWithdraw")
                {
                    withdrawCollection = withDrawService.GetAllFinanceList().Where(x => x.CreatedDate >= Convert.ToDateTime(withdrawReportObj.From) && x.CreatedDate <= Convert.ToDateTime(withdrawReportObj.To) && x.IsSale == true && x.CustomerProductId != null).ToList();

                }
                if (withdrawReportObj.ReportType == "InvestmentWithdraw")
                {
                    withdrawCollection = withDrawService.GetAllFinanceList().Where(x => x.CreatedDate.Date >= Convert.ToDateTime(withdrawReportObj.From,dtinfo).Date && x.CreatedDate.Date <= Convert.ToDateTime(withdrawReportObj.To,dtinfo).Date && x.IsSale == false && x.IsEarning == false && x.CustomerProductId != null).ToList();

                }
                //var customerProductCollection = customerProductService.GetAllCustomerProduct().Where(x => x.ProductId == new Guid(withdrawReportObj.ProductId) && x.Status != "Reject").ToList();
                ////check object
                if (withdrawCollection.Count > 0 && withdrawCollection != null)
                {
                    ////return response from customer service.
                    foreach (var withdrawDetail in withdrawCollection)
                    {
                        dynamic withdraw = new ExpandoObject();
                        withdraw.Name = withdrawDetail.CustomerProduct.Customer.FirstName + " " + withdrawDetail.CustomerProduct.Customer.MiddleName + " " + withdrawDetail.CustomerProduct.Customer.LastName;
                        withdraw.ProductName = withdrawDetail.CustomerProduct.Product.Name;
                        withdraw.MoneyWithdrawal = withdrawDetail.WithdrawAmount;
                        withdraw.MoneyWithdrawalFloat = withdrawDetail.WithdrawAmount;
                        withdraw.ReportType = "WithdrawalReport";
                        ////set all values in list.
                        withdrawDetails.Add(withdraw);
                    }
                    ////return all service 
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "successfully record fetched", Succeeded = true, DataObject = new ExpandoObject(), DataList = withdrawDetails, ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "No Data Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "some thing went wrong" });
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "some thing went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "some thing went wrong" });

            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);
                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = ex.Message, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// ticket report data basis of status and date filter
        /// </summary>
        /// <param name="ticketReportObj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAllTicketReport")]
        public HttpResponseMessage GetAllTicketReport(CustomerReportDataModel ticketReportObj)
        {
            try
            {
                DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                dtinfo.ShortDatePattern = "MM/dd/yyyy";
                dtinfo.DateSeparator = "/";

                ////dynamic list.
                dynamic ticketDetails = new List<ExpandoObject>();
                ////get ticket list with on he basis of date and status filters
                 var ticketCollection = ticketService.GetAllTicket().Where(x => x.CreatedDate.Date >= Convert.ToDateTime(ticketReportObj.From, dtinfo).Date && x.CreatedDate.Date <= Convert.ToDateTime(ticketReportObj.To, dtinfo).Date && x.TicketStatusId == new Guid(ticketReportObj.ReportType)).OrderBy(x => x.AutoIncrementedNo).ToList();
                
                ////check object
                if (ticketCollection.Count > 0 && ticketCollection != null)
                {
                    ////return response from ticket service.
                    foreach (var ticketDetail in ticketCollection)
                    {
                        dynamic ticket = new ExpandoObject();
                        ticket.Name = ticketDetail.Customer.FirstName + " " + ticketDetail.Customer.MiddleName + " " + ticketDetail.Customer.LastName;
                        ticket.Title = ticketDetail.Title;
                        ticket.TicketId = ticketDetail.AutoIncrementedNo;
                        ticket.Status = ticketDetail.TicketStatus.Name;
                        ticket.CreatedDate = ticketDetail.CreatedDate;
                        ticket.ReportType = "TicketReport";
                        ////set all values in list.
                        ticketDetails.Add(ticket);
                    }
                    ////return all service 
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "successfully record fetched", Succeeded = true, DataObject = new ExpandoObject(), DataList = ticketDetails, ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "No Data Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "some thing went wrong" });
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "some thing went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "some thing went wrong" });

            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);
                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = ex.Message, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }
    }
}
