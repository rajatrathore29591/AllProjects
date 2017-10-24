using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TradeSystem.Service;
using TradeSystem.Utils.Models;
using TradeSystem.WebApi.Controllers;

namespace TradeSystem.MVCWeb.Controllers
{
    [Authorize]
    [RoutePrefix("api/dashboardadmin")]

    public class DashboardAdminApiController : BaseApiController
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IProductService productService;
        ICustomerService customerService;
        ICustomerProductService customerProductService;
        IWalletService walletService;
        IWithdrawService withdrawService;
        // Constructor of Dashboard Admin Api Controller
        public DashboardAdminApiController(IProductService _productService, ICustomerService _customerService, ICustomerProductService _customerProductService, IWalletService _walletService, IWithdrawService _withdrawService)
        {
            productService = _productService;
            customerService = _customerService;
            customerProductService = _customerProductService;
            walletService = _walletService;
            withdrawService = _withdrawService;
        }
        #endregion

        /// <summary>
        /// This method for get all record from "product, customer and customerproduct" entity.
        /// </summary>
        /// <returns>product, customer and customerproduct entity object value.</returns>
        [HttpGet]
        [Route("GetAllDashboardAdmin")]
        public HttpResponseMessage GetAllDashboardAdmin()
        {
            try
            {
                ////get product, customer and customerProduct list 
                var productCollection = productService.GetAllProduct();
                var customerCollection = customerService.GetAllCustomer();
                var customerProductCollection = customerProductService.GetAllCustomerProduct().Where(x => x.Status == "Active");

                //get total number of product and customer 
                var productCount = productCollection.Count;
                var customerCount = customerCollection.Count;

                ////check object
                if (productCollection.Count > 0 && productCollection != null)
                {
                    ////dynamic list.
                    dynamic dashboardAdminDetails = new List<ExpandoObject>();
                    ////bind dynamic property.
                    dynamic dashboardAdmin = new ExpandoObject();
                    float total = 0;
                    ////return response from product service.
                    foreach (var dashboardAdminDetail in productCollection)
                    {

                        dashboardAdmin.TotalValueOfInvestment = dashboardAdminDetail.TotalValueOfInvestment;
                        total = total + dashboardAdminDetail.TotalValueOfInvestment;
                    }
                    dashboardAdmin.TotalValueOfInvestment = total;

                    dashboardAdmin.ProductCount = productCount;

                    var li = (from d in customerCollection join c in customerProductCollection on d.Id equals c.CustomerId select c).ToList();

                    if (li.Count > 0)
                    {
                        total = 0;
                        ////return response from product service.
                        foreach (var customerDetail in li)
                        {
                            dashboardAdmin.ProductAmount = customerDetail.Investment;
                            total = total + customerDetail.Investment;
                        }
                        dashboardAdmin.ProductAmount = total;
                    }
                    else
                    {
                        dashboardAdmin.ProductAmount = 0;
                    }

                    dashboardAdmin.CustomerCount = customerCount;
                    ////set all values in list.
                    dashboardAdminDetails.Add(dashboardAdmin);
                    ////return all service 
                    return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK, (List<ExpandoObject>)dashboardAdminDetails);
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

        #region Customer Dashboard 

        /// <summary>
        /// This method for get all record from "product, customer and customerproduct" entity.
        /// </summary>
        /// <returns>product, customer and customerproduct entity object value.</returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetAllDashboard/{customerId}/{lang}")]
        public HttpResponseMessage GetAllDashboard(string customerId, string lang)
        {
            try
            {
                var customerProductCollection = customerProductService.GetAllCustomerProductByCustomerId(new Guid(customerId));
                // getting wallet amount corresponding to the customer
                var walletAmount = walletService.GetWalletByCustomerId(new Guid(customerId));

                //get all withdraw amount by customer id
                var customerAllWithdrawCollection = withdrawService.GetAllWithdrawByCustomerId(new Guid(customerId));

                var salesCount = "";
                var customersaleCollection = customerService.GetAllReferalCustomerByCustomerId(new Guid(customerId));
                var saleEarning = (from custSale in customersaleCollection join custProduct in customerProductService.GetAllCustomerProduct() on custSale.Id equals custProduct.CustomerId select custProduct).ToList();

                // getting sum of weekly amount earned from investment
                var IsEarning = customerAllWithdrawCollection.ToList().Where(y => y.IsEarning == true && y.Status == "Fund Released" && y.CustomerProductId != null).Sum(x => x.WithdrawAmount);
                //var Earning = customerAllWithdrawCollection.ToList().Where(y => y.IsEarning == false && y.IsSale == false && y.Status == "Fund Released").Sum(x => x.WithdrawAmount);
                //var SumEarning = Convert.ToDouble(IsEarning) + Convert.ToDouble(Earning);
                var SumEarning = Convert.ToDouble(IsEarning);

                //    ////dynamic list.
                dynamic dashboardCustomerDetails = new List<ExpandoObject>();
                //    ////bind dynamic property.
                dynamic dashboardAdmin = new ExpandoObject();
                dashboardAdmin.EarningByInvestment = customerAllWithdrawCollection.Count > 0 ? SumEarning : 0;
                dashboardAdmin.EarningBySales = customerAllWithdrawCollection.Count > 0 ? customerAllWithdrawCollection.ToList().Where(y => y.IsSale == true).Sum(x => x.WithdrawAmount) : 0;
                // dashboardAdmin.EarningByInvestment = customerProductCollection.Count > 0 ? customerProductCollection.Sum(x => x.WeeklyEarning) : 0;
                //dashboardAdmin.EarningBySales = saleEarning.Count > 0 ? saleEarning.Sum(x => x.SaleEarning) : 0;
                dashboardAdmin.TotalValueOfInvestment = dashboardAdmin.EarningByInvestment + dashboardAdmin.EarningBySales;
                dashboardAdmin.VirtualWalletValue = walletAmount == null ? 0 : walletAmount.AvailableBalance;
                dashboardAdmin.ProductCount = customerProductCollection.Count;
                dashboardAdmin.SaleCount = customersaleCollection.Count;

                ////set all values in list.
                dashboardCustomerDetails.Add(dashboardAdmin);
                ////return all service 

                string st1 = resmanager.GetString("Api_DashbaordDetails", CultureInfo.GetCultureInfo(lang));
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st1, Succeeded = true, DataObject = new ExpandoObject(), DataList = dashboardCustomerDetails, ErrorInfo = "" });
                //return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK, (List<ExpandoObject>)dashboardCustomerDetails);
                //}
                //else
                //{
                //    return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Data Not Found." });
                //}
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
            }
        }

        #endregion
    }
}
