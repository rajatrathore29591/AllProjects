using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TradeSystem.Service;
using TradeSystem.Utils;
using TradeSystem.Utils.Models;
using Microsoft.AspNet.Identity.Owin;
using TradeSystem.Framework.Identity;
using Microsoft.AspNet.Identity;
using TradeSystem.Framework.Entities;
using System.Web;
using TradeSystem.Utils.Enum;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Globalization;
using TradeSystem.WebApi.Controllers;
using TradeSystem.Utilities.Email;
using System.Linq;
using System.Web.Configuration;

namespace TradeSystem.MVCWeb.Controllers
{
    [Authorize]
    [RoutePrefix("api/customer")]
    public class CustomerApiController : BaseApiController
    {

        #region Dependencies Injection with initialization

        //Initialized interface object. 
        private Framework.Identity.ApplicationUserManager userManager;
        ICustomerService customerService;
        ICustomerProductService customerProductService;
        IDocumentService documentService;
        ICountryService countryService;
        IStateService stateService;
        IBankService bankService;
        IProductService productService;
        IActivityService activityService;
        IPenaltyService penaltyService;
        IEmailService emailService;
        IWithdrawService withdrawService;
        IWalletService walletService;
        IPromotionService promotionService;
        private const string LocalLoginProvider = "Local";

        // Constructor of Customer Api Controller 
        public CustomerApiController(ICustomerService _customerService, ICustomerProductService _customerProductService, IDocumentService _documentService, ICountryService _countryService, IStateService _stateService, IBankService _bankService, IProductService _productService, IActivityService _activityService, IPenaltyService _penaltyService, IEmailService _emailService, IWithdrawService _withdrawService, IWalletService _walletService, IPromotionService _promotionService)
        {
            customerService = _customerService;
            customerProductService = _customerProductService;
            documentService = _documentService;
            countryService = _countryService;
            stateService = _stateService;
            bankService = _bankService;
            productService = _productService;
            activityService = _activityService;
            penaltyService = _penaltyService;
            emailService = _emailService;
            withdrawService = _withdrawService;
            walletService = _walletService;
            promotionService = _promotionService;
        }

        //Initialzing User Manager 
        public Framework.Identity.ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? Request.GetOwinContext().GetUserManager<Framework.Identity.ApplicationUserManager>();
            }
            private set
            {
                userManager = value;
            }
        }

        #endregion

        //Get value of projectPathUrl and projectName from the web config 
        string projectPathUrl = WebConfigurationManager.AppSettings["Domain"].ToString();
        string projectName = WebConfigurationManager.AppSettings["ProjectName"].ToString();

        /// <summary>
        /// This method for get all record from "customer" entity.
        /// </summary>
        /// <returns>Customer entity object value.</returns>
        [HttpGet]
        [Route("GetAllCustomers")]
        public HttpResponseMessage GetAllCustomers()
        {
            try
            {
                int count = 0;
                List<Customer> customerSaleCount = new List<Customer>();
                ////get customers list 
                var customerCollection = customerService.GetAllCustomer();

                ////check object
                if (customerCollection.Count > 0 && customerCollection != null)
                {
                    ////dynamic list.
                    dynamic customers = new List<ExpandoObject>();
                    ////bind dynamic property.

                    ////return response from customer service.
                    foreach (var customerDetail in customerCollection)
                    {
                        dynamic customer = new ExpandoObject();
                        var customerProductCollection = customerProductService.GetAllCustomerProductByCustomerId(customerDetail.Id).Where(x => x.Status != "Reject").ToList();
                        customerSaleCount = customerService.GetAllReferalCustomerByCustomerId(customerDetail.Id).ToList();
                        if (customerProductCollection.Count > 0)
                        {
                            //if (customerDetail.CustomerReferalId != null)
                            //{
                            // customerSaleCount = customerService.GetAllReferalCustomerByCustomerId(customerDetail.Id).ToList();
                            //}
                            ////check object

                            //foreach (var customerProductDetail in customerProductCollection)
                            //{
                            //    totalInvestment += customerProductDetail.Investment;
                            //    //customer.TotalInvestmentCount = count++;
                            //}
                            customer.TotalInvestment = customerProductCollection.Sum(x => x.Investment);
                            // replica of TotalInvestment for add on issue
                            customer.TotalInvestmentFloat = customerProductCollection.Sum(x => x.Investment);
                            customer.TotalInvestmentCount = customerProductCollection.Count;
                            //customer.TotalSaleCount = customerSaleCount.Count;
                        }
                        else
                        {
                            customer.TotalInvestment = 0;
                            customer.TotalInvestmentCount = 0;
                            //customer.TotalSaleCount = 0;
                        }
                        ////map ids
                        customer.Id = customerDetail.Id;
                        customer.UserName = customerDetail.UserName;
                        customer.CustomerName = customerDetail.FirstName + " " + customerDetail.MiddleName + " " + customerDetail.LastName;
                        customer.Status = customerDetail.IsActive == true ? "Active" : "Inactive";
                        customer.CreatedDate = customerDetail.CreatedDate.ToString("MM-dd-yyyy HH:mm:ss tt");
                        customer.TotalSaleCount = customerSaleCount.Count;
                        ////set customers values in list.
                        customers.Add(customer);
                    }

                    ////return customers service 
                    return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK, (List<ExpandoObject>)customers);
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
        /// This method for Get All customer by customerId.
        /// </summary>
        /// <param name="id">customer Id</param>
        /// <returns>customer</returns>
        [HttpGet]
        [Route("GetCustomerByCustomerId/{Id}")]
        public HttpResponseMessage GetCustomerByCustomerId(string Id)
        {
            try
            {
                var Country = string.Empty;
                var State = string.Empty;
                var Bank = string.Empty;
                ////check is valid user id.
                if (ServiceHelper.IsGuid(Id))
                {
                    ////get customer.
                    var customerDetail = customerService.GetCustomerById(new Guid(Id));

                    if (customerDetail != null)
                    {
                        ////bind dynamic property.
                        dynamic customer = new ExpandoObject();

                        ////map ids
                        customer.Id = customerDetail.Id;
                        customer.FirstName = customerDetail.FirstName;
                        customer.MiddleName = customerDetail.MiddleName;
                        customer.LastName = customerDetail.LastName;
                        customer.MotherLastName = customerDetail.MotherLastName;
                        customer.BirthDate = customerDetail.BirthDate.ToString("MM-dd-yyyy");
                        customer.CountryName = customerDetail.Country == null ? "" : customerDetail.Country.CountryName;
                        customer.StateName = customerDetail.State == null ? "" : customerDetail.State.StateName;
                        customer.RFC = customerDetail.RFC;
                        customer.UserName = customerDetail.UserName;
                        customer.Email = customerDetail.Email;
                        customer.BankAccount = customerDetail.BankAccount;
                        customer.Clabe = customerDetail.Clabe;
                        customer.BenificiaryName = customerDetail.BenificiaryName;
                        customer.IsActive = customerDetail.IsActive;
                        customer.Commission = customerDetail.MaxSaleEarningPercent;
                        customer.Phone = customerDetail.Phone;
                        customer.OpenpayPaymentCustomerId = customerDetail.OpenPayCustomerId;
                        customer.BankName = customerDetail.BankName != null ? customerDetail.BankName : "";
                        return this.Request.CreateResponse<ExpandoObject>(HttpStatusCode.OK, (ExpandoObject)customer);
                    }
                    else
                    {
                        ////case of record not found.
                        return this.Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    ////case of invalid id request.
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest);
                }

            }
            catch (Exception ex)
            {
                //// handel exception log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
            }
        }

        /// <summary>
        /// This method for get all customer sale record from "customerProduct" entity.
        /// </summary>
        /// <returns>CustomerProduct entity object value.</returns>
        [HttpGet]
        [Route("GetAllCustomerSales/{customerId}")]
        public HttpResponseMessage GetAllCustomerSales(string customerId)
        {
            try
            {
                ////get product, customer and customerProduct list 
                // var customerProductCollection = customerProductService.GetAllCustomerProductByCustomerId(new Guid(customerId));
                var customerCollection = customerService.GetAllReferalCustomerByCustomerId(new Guid(customerId)).OrderBy(x => x.CreatedDate).ToList();
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
                        List<CustomerProduct> customerProductCollection = customerProductService.GetAllCustomerProductByCustomerId(customer.Id);

                        ////bind dynamic property.
                        dynamic customerProduct = new ExpandoObject();
                        //double tempInvestment = 0;
                        double tempSaleEarning = 0;
                        ////check object
                        if (customerProductCollection.Count > 0 && customerProductCollection != null)
                        {
                            ////return response from product service.
                            foreach (var customerProductSaleDetail in customerProductCollection)
                            {
                                var customerSale = customerProductSaleDetail.Withdraws.Where(x => x.IsSale == true).ToList();
                                if (customerSale.Count > 0)
                                {
                                    tempSaleEarning += customerSale[0].WithdrawAmount;
                                }
                                else
                                {
                                    tempSaleEarning += 0;
                                }

                            }
                            customer.SaleEarning = tempSaleEarning;
                        }
                        customer.SaleEarning = tempSaleEarning.ToString("0.00");
                        customer.SaleEarningFloat = tempSaleEarning.ToString("0.00");
                        ////set customers values in list.
                        customers.Add(customer);
                    }

                    ////return all service 
                    return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK, (List<ExpandoObject>)customers);
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
        /// This method for get all customer investment record from "customerProduct" entity.
        /// </summary>
        /// <returns>CustomerProduct entity object value.</returns>
        [HttpGet]
        [Route("GetAllCustomerInvestments/{customerId}")]
        public HttpResponseMessage GetAllCustomerInvestments(string customerId)
        {
            try
            {
                ////get customerProduct list 
                var customerProductCollection = customerProductService.GetAllCustomerProductByCustomerId(new Guid(customerId)).OrderByDescending(x => x.CreatedDate).ToList();

                ////check object
                if (customerProductCollection.Count > 0 && customerProductCollection != null)
                {
                    ////dynamic list.
                    dynamic customerProducts = new List<ExpandoObject>();


                    ////return response from product service.
                    foreach (var customerProductDetail in customerProductCollection)
                    {
                        ////bind dynamic property.
                        dynamic customerProduct = new ExpandoObject();
                        customerProduct.ProductId = customerProductDetail.ProductId;
                        customerProduct.Id = customerProductDetail.Id;
                        customerProduct.CustomerId = customerProductDetail.CustomerId;
                        customerProduct.CreatedDate = customerProductDetail.CreatedDate.ToString("MM-dd-yyyy HH:mm:ss tt");
                        customerProduct.InvestmentWithdrawDate = customerProductDetail.Product.InvestmentWithdrawDate;
                        customerProduct.InvestmentName = customerProductDetail.Product.Name;
                        customerProduct.Status = customerProductDetail.Status;
                        customerProduct.WeeklyEarning = customerProductDetail.WeeklyEarning.ToString("0.00");
                        customerProduct.Investment = customerProductDetail.Investment;
                        customerProduct.WeeklyEarningFloat = customerProductDetail.WeeklyEarning.ToString("0.00");
                        customerProduct.InvestmentFloat = customerProductDetail.Investment;
                        customerProduct.PaymentType = customerProductDetail.PaymentType;
                        customerProduct.WalletAmount = customerProductDetail.WalletAmount;
                        ////set customers values in list.
                        customerProducts.Add(customerProduct);
                    }
                    ////return all service 
                    return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK, (List<ExpandoObject>)customerProducts);
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
        /// method is used to approve and reject from finance management
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="ProductId"></param>
        /// <param name="Status"></param>
        /// <param name="Investment"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("FinanceApprove/{CustomerId}/{ProductId}/{Status}")]
        public HttpResponseMessage FinanceApprove(string CustomerId, string ProductId, string Status, string Investment)
        {
            try
            {
                ListDictionary replacements = new ListDictionary();
                PromotionDataModel promotionDataModel = new PromotionDataModel();
                if (Status != "Reject")
                {
                    // get product by product id
                    var product = productService.GetProductByProductId(new Guid(ProductId));
                    if (Convert.ToDouble(Investment) <= product.RemainingValueOfInvestment)
                    {
                        //// updating status service response.
                        var response = customerProductService.UpdateCustomerProductStatus(new Guid(CustomerId), new Guid(ProductId), Status);

                        //var customerProduct = customerProductService.GetAllCustomerProductByProductIdCustomerId(new Guid(CustomerId), new Guid(ProductId));
                        // updating remaining investment
                        float remainingValue = product.RemainingValueOfInvestment - float.Parse(Investment);
                        var RemainingInvestmentResult = productService.UpdateInvestmentAmount(new Guid(ProductId), remainingValue);
                        // current week total earning
                        double dailyPercent = (double)(Convert.ToDecimal(product.PercentWeeklyEarning) / 7);
                        double dailyAmount = (dailyPercent * Convert.ToDouble(Investment)) / 100;

                        //update weekly earning enable date [change code 03/02/2017]
                        DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                        dtinfo.ShortDatePattern = "dd/MM/yyyy";

                        int currentDateWithAddWithdrawDay = Convert.ToInt32(product.WeeklyToWithdrawDay);

                        DateTime weeklyEarningEnableDate;
                        var dateresult = IsValidDateTimeTest(product.WeeklyFromWithdrawDay);
                        if (dateresult)
                        {
                            weeklyEarningEnableDate = Convert.ToDateTime(product.WeeklyFromWithdrawDay, dtinfo);
                        }
                        else
                        {
                            int withdrawDay = Convert.ToInt32(product.WeeklyFromWithdrawDay) * 7;
                            weeklyEarningEnableDate = Convert.ToDateTime(DateTime.UtcNow.Date, dtinfo).AddDays(withdrawDay);
                        }

                        // new investment notification alert(Customer)
                        ////inserting data into promotion datamodel.
                        string SuccessPurchaseE = resmanager.GetString("SuccessPurchaseE", CultureInfo.GetCultureInfo("en"));
                        string SuccessPurchaseS = resmanager.GetString("SuccessPurchaseS", CultureInfo.GetCultureInfo("es"));
                        string InvestmentPlanE = resmanager.GetString("InvestmentPlanE", CultureInfo.GetCultureInfo("en"));
                        string InvestmentPlanS = resmanager.GetString("InvestmentPlanS", CultureInfo.GetCultureInfo("es"));
                        promotionDataModel.Description = SuccessPurchaseE + " " + product.Name + " " + InvestmentPlanE;
                        promotionDataModel.DescriptionSpanish = SuccessPurchaseS + " " + product.Name + " " + InvestmentPlanS;
                        promotionDataModel.To = CustomerId;
                        promotionDataModel.PromotionType = "Alert on Web & App";
                        promotionDataModel.Subject = "Successfully Invested";
                        promotionDataModel.SubjectSpanish = "Successfully Invested";
                        promotionDataModel.Url = projectPathUrl + projectName + "/Investment/MyInvestmentsList";

                        ////inserting data into promotion table.
                        var promotionCustomerResponse = promotionService.AddPromotion(promotionDataModel);

                        var WeeklyEarningResult = customerProductService.UpdateWeeklyEarningEnableDate(new Guid(CustomerId), new Guid(ProductId), weeklyEarningEnableDate);

                        // update week percent
                        var WeeklyResult = customerProductService.UpdateWeeklyEarning(new Guid(CustomerId), new Guid(ProductId), (float)0);
                        var customer = customerService.GetCustomerById(new Guid(CustomerId));
                        if (customer.CustomerReferalId != null)
                        {
                            var referalCustomerDetail = customerService.GetCustomerById((customer.CustomerReferalId.Value));
                            if (referalCustomerDetail.IsActive == true)
                            {
                                // referal customer total investment
                                var referalCustomerProduct = customerProductService.GetAllCustomerProductByCustomerId(customer.CustomerReferalId.Value);
                                var totalInvestment = referalCustomerProduct.Sum(x => x.Investment);

                                // total max percent calculated amount
                                var maxPercentAmount = (totalInvestment * referalCustomerDetail.MaxSaleEarningPercent) / 100;
                                var allReferalCustomerIds = customerService.GetAllReferalCustomerByCustomerId(customer.CustomerReferalId.Value);

                                var totalReferalInvestedAmount = (from allReferalCustomer in allReferalCustomerIds
                                                                  join allCustomerProduct in customerProductService.GetAllCustomerProduct() on allReferalCustomer.Id equals allCustomerProduct.CustomerId
                                                                  group allCustomerProduct by allCustomerProduct.CustomerId into g
                                                                  select new
                                                                  {
                                                                      CustomerId = g.Key,
                                                                      TotalSaleEarning = g.Sum(i => i.SaleEarning)
                                                                  }).ToList();

                                double TotalSaleEarning = (Convert.ToDouble(Investment) * Convert.ToDouble(product.PercentSaleEarning)) / 100;

                                if (referalCustomerDetail.IsActive == true && totalReferalInvestedAmount.Sum(x => x.TotalSaleEarning) <= maxPercentAmount)
                                {
                                    // update sale percent
                                    var result = customerProductService.UpdateSaleEarning(new Guid(CustomerId), new Guid(ProductId), (float)TotalSaleEarning);
                                    return this.Request.CreateResponse(HttpStatusCode.OK, new { result = "Successfully invested", Message = true });
                                }
                                else
                                {
                                    var result = customerProductService.UpdateSaleEarningStatus(new Guid(CustomerId), new Guid(ProductId), "Reached Commission Limit", true);
                                    return this.Request.CreateResponse(HttpStatusCode.OK, new { result = "Status has been updated Successfully", Message = true });
                                }

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
                            return this.Request.CreateResponse(HttpStatusCode.OK, new { result = "Data not found", Message = true });

                        }
                        return this.Request.CreateResponse(HttpStatusCode.OK, new { result = "Referal id is null", Message = true });
                    }
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { result = "Remaining value is less than invesment amount", Message = false });
                }
                else
                {
                    var customerProductDetails = customerProductService.GetAllCustomerProductByProductIdCustomerId(new Guid(CustomerId), new Guid(ProductId));
                    if (customerProductDetails.PaymentType == "Wallet" || customerProductDetails.WalletAmount != 0.0)
                    {
                        var WalletAmount = walletService.GetWalletByCustomerId(new Guid(CustomerId)).AvailableBalance;
                        if (customerProductDetails.WalletAmount != 0.0 && customerProductDetails.PaymentType != "Wallet")
                        {
                            Investment = customerProductDetails.WalletAmount.ToString();
                        }
                        WalletAmount = WalletAmount + Convert.ToDecimal(Investment);
                        var UpdateWallet = walletService.UpdateWalletAmount(new Guid(CustomerId), (float)WalletAmount);
                    }
                    //// updating status service response.
                    var response = customerProductService.UpdateCustomerProductStatus(new Guid(CustomerId), new Guid(ProductId), Status);
                    ////return result from service response.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { result = response, Message = true });
                }

            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Exception" });

            }
        }

        /// <summary>
        /// Check given value is date or days
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

        /// <summary>
        /// This method for insert record into "Customer" from database entity.
        /// </summary>
        /// <param name="firstName">firstName</param>
        /// <param name="lastName">lastName</param>
        /// <param name="middleName">middleName</param>
        /// <param name="motherLastName">motherLastName</param>
        /// <param name="birthDate">birthDate</param>
        /// <param name="lastName">lastName</param>
        /// <param name="categoryId">CategoryId</param>
        /// <param name="subcategoryId">SubCategoryId</param>
        /// <param name="firstName">firstName</param>
        /// <param name="lastName">lastName</param>
        /// <param name="categoryId">CategoryId</param>
        /// <param name="subcategoryId">SubCategoryId</param>
        /// <returns>Api response result</returns>
        [HttpPost]
        [Route("AddCustomer")]
        public HttpResponseMessage AddCustomer(CustomerDataModel customerData)
        {

            var user = new ApplicationUser();
            try
            {
                PromotionDataModel promotionDataModel = new PromotionDataModel();
                var response = string.Empty;
                DocumentDataModel documentData = new DocumentDataModel();
                Guid? avtarId = Guid.Empty;
                ListDictionary replacements = new ListDictionary(); //For email send to customer
                var encryptedPassword = string.Empty;

                var checkEmail = UserManager.FindByEmail(customerData.Email);
                if (checkEmail == null)
                {
                    ////step:1
                    ////add user in AspNetUser.
                    user.UserName = customerData.Email;
                    user.Email = customerData.Email;
                    //user.PasswordHash = SecurityHelper.Encrypt(customerData.Password, true);
                    var result = UserManager.Create(user, customerData.Password);

                    if (result.Succeeded)
                    {
                        ////step:2
                        ////add customer userId in customerData.
                        customerData.UserId = user.Id;

                        customerData.Password = SecurityHelper.Encrypt(customerData.Password, true); ;
                        ////get service response.
                        response = customerService.AddCustomer(customerData);
                        if (response != null)
                        {
                            var customerDetail = new Customer();
                            if (customerData.CustomerReferalId != null)
                            {
                                // getting customer detail by referal Id
                                customerDetail = customerService.GetCustomerById(new Guid(customerData.CustomerReferalId));

                                // new referal notification alert (Admin)
                                ////inserting data into promotion datamodel.
                                promotionDataModel.Description = "New Referal added by " + customerDetail.FirstName + " " + customerDetail.LastName;
                                promotionDataModel.DescriptionSpanish = "New Referal added by " + customerDetail.FirstName + " " + customerDetail.LastName;
                                promotionDataModel.To = null;
                                promotionDataModel.PromotionType = "Alert on Web & App";
                                promotionDataModel.Subject = "New Referal added";
                                promotionDataModel.SubjectSpanish = "New Referal added";
                                promotionDataModel.Url = projectPathUrl + projectName + "/CustomerManagement/SalesList/" + customerDetail.Id;

                                ////inserting data into promotion table.
                                var promotionResponse = promotionService.AddPromotion(promotionDataModel);

                                // new referal notification alert to same user who is adding (Customer)
                                ////inserting data into promotion datamodel.
                                string NewReferalE = resmanager.GetString("NewReferalE", CultureInfo.GetCultureInfo("en"));
                                string NewReferalS = resmanager.GetString("NewReferalS", CultureInfo.GetCultureInfo("es"));
                                string NewReferalSubE = resmanager.GetString("NewReferalSubE", CultureInfo.GetCultureInfo("en"));
                                string NewReferalSubS = resmanager.GetString("NewReferalSubS", CultureInfo.GetCultureInfo("es"));

                                promotionDataModel.Description = NewReferalE + " " + customerDetail.FirstName + " " + customerDetail.LastName;
                                promotionDataModel.DescriptionSpanish = NewReferalS + " " + customerDetail.FirstName + " " + customerDetail.LastName;
                                promotionDataModel.To = customerDetail.Id.ToString();
                                promotionDataModel.PromotionType = "Alert on Web & App";
                                promotionDataModel.Subject = NewReferalSubE;
                                promotionDataModel.SubjectSpanish = NewReferalSubS;
                                promotionDataModel.Url = projectPathUrl + projectName + "/Sale/MySales";

                                ////inserting data into promotion table.
                                var promotionCustomerResponse = promotionService.AddPromotion(promotionDataModel);

                                // new referal notification alert
                                ////inserting data into promotion datamodel.
                                promotionDataModel.Description = customerDetail.FirstName + " " + customerDetail.LastName + " account created successfully";
                                promotionDataModel.DescriptionSpanish = customerDetail.FirstName + " " + customerDetail.LastName + " account created successfully";
                                promotionDataModel.To = null;
                                promotionDataModel.PromotionType = "Alert on Web & App";
                                promotionDataModel.Subject = "New account created";
                                promotionDataModel.SubjectSpanish = "New account created";
                                promotionDataModel.Url = projectPathUrl + projectName + "/CustomerManagement/GeneralDetails/" + customerDetail.Id;

                                ////inserting data into promotion table.
                                var adminPromotionResponse = promotionService.AddPromotion(promotionDataModel);
                            }
                            else
                            {
                                // getting customer detail by newly added CustomerId
                                customerDetail = customerService.GetCustomerById(new Guid(response));
                                // new referal notification alert
                                ////inserting data into promotion datamodel.
                                promotionDataModel.Description = customerDetail.FirstName + " " + customerDetail.LastName + " account created successfully";
                                promotionDataModel.DescriptionSpanish = customerDetail.FirstName + " " + customerDetail.LastName + " account created successfully";
                                promotionDataModel.To = null;
                                promotionDataModel.PromotionType = "Alert on Web & App";
                                promotionDataModel.Subject = "New account created";
                                promotionDataModel.SubjectSpanish = "New account created";
                                promotionDataModel.Url = projectPathUrl + projectName + "/CustomerManagement/GeneralDetails/" + customerDetail.Id;

                                ////inserting data into promotion table.
                                var adminPromotionResponse = promotionService.AddPromotion(promotionDataModel);
                            }

                            if (customerData.companyUserId != null)
                            {
                                // inserting data into activity table
                                ActivityLogDataModel activityObj = new ActivityLogDataModel();
                                activityObj.Activity = ETaskStatus.Registration.ToString();
                                activityObj.Description = "Adding New Customer " + customerData.FirstName;
                                activityObj.IsCompanyUser = true;
                                activityObj.CompanyUserId = customerData.companyUserId;
                                var activityResult = activityService.AddActivity(activityObj);
                            }
                            if (customerData.CustomerReferalId != null)
                            {
                                dynamic customerModel = new ExpandoObject();
                                customerModel.FirstName = customerData.FirstName;
                                customerModel.Email = customerData.Email;
                                customerModel.Password = customerData.Password;

                                string coupon = SecurityHelper.Decrypt(customerData.Password, true);
                                replacements = new ListDictionary { { "<%FirstName%>", customerData.FirstName } };
                                replacements.Add("<%Subject%>", "Register");
                                replacements.Add("<%LastName%>", customerData.LastName);
                                replacements.Add("<%email%>", customerData.Email);
                                replacements.Add("<%Password%>", coupon);

                                ////add email data
                                var sendEmail = emailService.AddEmailData(response, customerData.Email, string.Empty, string.Empty, string.Empty, EmailTemplatesHelper.CustomerCreateEmail, replacements);

                                ////send email.
                                if (sendEmail != null)
                                {
                                    ////Code for Sending Email
                                    emailService.SendEmailAsync(new Guid(response), customerData.Email, string.Empty, string.Empty, customerData.Email, EmailTemplatesHelper.CustomerCreateEmail, encryptedPassword, replacements);
                                }

                            }
                            else
                            {
                                dynamic customerModel = new ExpandoObject();
                                customerModel.FirstName = customerData.FirstName;
                                customerModel.Email = customerData.Email;
                                customerModel.Password = customerData.Password;

                                string coupon = SecurityHelper.Decrypt(customerData.Password, true);
                                replacements = new ListDictionary { { "<%FirstName%>", customerData.FirstName } };
                                replacements.Add("<%Subject%>", "Register");
                                replacements.Add("<%LastName%>", customerData.LastName);
                                replacements.Add("<%email%>", customerData.Email);
                                replacements.Add("<%Password%>", coupon);

                                ////add email data
                                var sendEmail = emailService.AddEmailData(response, customerData.Email, string.Empty, string.Empty, string.Empty, EmailTemplatesHelper.CompanyCreateEmail, replacements);

                                ////send email.
                                if (sendEmail != null)
                                {
                                    ////Code for Sending Email
                                    emailService.SendEmailAsync(new Guid(response), customerData.Email, string.Empty, string.Empty, customerData.Email, EmailTemplatesHelper.CompanyCreateEmail, encryptedPassword, replacements);
                                }
                            }
                            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Customer successfully inserted", Succeeded = true });
                        }
                        else
                        {
                            return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false });
                        }

                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Not inserted in aspnet user table", Succeeded = false });
                    }
                }
                else
                {
                    //////return response
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new BaseResponseDataModel { Message = "Email already exist", Succeeded = false });
                }
            }
            catch (Exception ex)
            {
                var result = UserManager.Delete(user);

                //// Handel Exception Log.
                Console.Write(ex.Message);
                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, new BaseResponseDataModel { Message = "Exception", Succeeded = false });
            }
        }

        /// <summary>
        /// Edit Customer by customer id
        /// </summary>
        /// <param name="customerData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EditCustomer")]
        public HttpResponseMessage EditCustomer(CustomerDataModel customerData)
        {
            try
            {
                PromotionDataModel promotionDataModel = new PromotionDataModel();

                // getting customer detail
                var IsRefral = customerService.GetCustomerById(new Guid(customerData.Id));

                ////return all service 
                string ChangeSalePercentE = resmanager.GetString("ChangeSalePercent", CultureInfo.GetCultureInfo("en"));
                string SalePercentE = resmanager.GetString("SalePercent", CultureInfo.GetCultureInfo("en"));
                string ChangeSalePercentS = resmanager.GetString("ChangeSalePercent", CultureInfo.GetCultureInfo("es"));
                string SalePercentS = resmanager.GetString("SalePercent", CultureInfo.GetCultureInfo("es"));

                if (IsRefral.MaxSaleEarningPercent != customerData.Commission)
                {
                    // for customer notification
                    ////inserting data into promotion datamodel.
                    promotionDataModel.Description = ChangeSalePercentE + customerData.Commission;
                    promotionDataModel.DescriptionSpanish = ChangeSalePercentS + customerData.Commission;
                    promotionDataModel.To = customerData.Id;
                    promotionDataModel.PromotionType = "Alert on Web & App";
                    promotionDataModel.Subject = SalePercentE;
                    promotionDataModel.SubjectSpanish = SalePercentS;

                    //inserting data into promotion table.
                    var promotionResponse = promotionService.AddPromotion(promotionDataModel);

                }

                // checking if customer is deactive
                if (customerData.IsActive == false && IsRefral.IsActive == true)
                {
                    if (IsRefral.CustomerReferalId != null)
                    {
                        string DeActiveCustomerE = resmanager.GetString("DeActiveCustomer", CultureInfo.GetCultureInfo("en"));
                        string YourReferalE = resmanager.GetString("YourReferal", CultureInfo.GetCultureInfo("en"));
                        string IsDeactiveE = resmanager.GetString("DeActiveCustomer", CultureInfo.GetCultureInfo("en"));
                        string DeActiveCustomerS = resmanager.GetString("DeActiveCustomer", CultureInfo.GetCultureInfo("es"));
                        string YourReferalS = resmanager.GetString("YourReferal", CultureInfo.GetCultureInfo("es"));
                        string IsDeactiveS = resmanager.GetString("DeActiveCustomer", CultureInfo.GetCultureInfo("es"));

                        ////inserting data into promotion datamodel.
                        promotionDataModel.Description = YourReferalE + IsRefral.FirstName + " " + IsRefral.LastName + " " + IsDeactiveE;
                        promotionDataModel.DescriptionSpanish = YourReferalS + IsRefral.FirstName + " " + IsRefral.LastName + " " + IsDeactiveS;
                        promotionDataModel.To = IsRefral.CustomerReferalId.ToString();
                        promotionDataModel.PromotionType = "Alert on Web & App";
                        promotionDataModel.Subject = DeActiveCustomerE;
                        promotionDataModel.SubjectSpanish = DeActiveCustomerS;
                        promotionDataModel.Url = projectPathUrl + projectName + "/Sale/MySales";

                        ////inserting data into promotion table.
                        var deactiveResponse = promotionService.AddPromotion(promotionDataModel);

                    }

                    //for admin notification
                    //new investment notification alert
                    //inserting data into promotion datamodel.
                    promotionDataModel.Description = IsRefral.FirstName + " " + IsRefral.LastName + " customer account deactivated";
                    promotionDataModel.DescriptionSpanish = IsRefral.FirstName + " " + IsRefral.LastName + " customer account deactivated";
                    promotionDataModel.To = null;
                    promotionDataModel.PromotionType = "Alert on Web & App";
                    promotionDataModel.Subject = "Account Deactivated";
                    promotionDataModel.SubjectSpanish = "Account Deactivated";
                    promotionDataModel.Url = projectPathUrl + projectName + "/CustomerManagement/GeneralDetails/" + IsRefral.Id;

                    //inserting data into promotion table.
                    var adminPromotionResponse = promotionService.AddPromotion(promotionDataModel);
                }

                ////calling add promotion method service response.
                var response = customerService.EditCustomer(customerData);

                ////return result from service response.
                return this.Request.CreateResponse(HttpStatusCode.OK, new { result = response });
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Exception" });

            }
        }

        /// <summary>
        /// This method for customer change password.
        /// </summary>
        /// <returns>status with message</returns>
        [HttpPost]
        [Route("ChangePassword")]
        [AllowAnonymous]
        public HttpResponseMessage ChangePassword(CustomerPasswordDataModel customerPasswordData)
        {
            try
            {
                ////step: 1
                ////check is valid email.
                if (ServiceHelper.IsGuid(customerPasswordData.Id))
                {
                    if (ModelState.IsValid)
                    {
                        var customer = customerService.GetCustomerById(new Guid(customerPasswordData.Id));
                        if (customer == null)
                        {
                            // Don't reveal that the user does not exist
                            return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "ID Not Exist or Password not matched" });
                        }
                        else
                        {
                            var user = UserManager.FindById(customer.UserId);
                            if (user != null)
                            {
                                // encrypting password for company user table
                                var password = SecurityHelper.Encrypt(customerPasswordData.NewPassword, true);

                                //user.PasswordHash = customerPasswordData.NewPassword;
                                var response = UserManager.ChangePassword(customer.UserId, customerPasswordData.OldPassword, customerPasswordData.NewPassword);
                                if (response.Succeeded)
                                {
                                    var result = customerService.EditCustomerPassword(new Guid(customerPasswordData.Id), password);
                                    ////check result
                                    if (result == true)
                                    {
                                        return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Password has been Successfuly Changed." });
                                    }
                                    else
                                    {
                                        ////return response
                                        return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Current password is incorrect" });
                                    }
                                }
                                else
                                {
                                    return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Current password is incorrect" });
                                }

                            }
                            else
                            {
                                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "company user not exist", Succeeded = false });
                            }
                            //    ////calling change password method
                            //    var result = customerService.EditCustomerPassword(new Guid(customerPasswordData.Id), customerPasswordData.NewPassword);
                            //    ////check result
                            //    if (result == true)
                            //    {
                            //        return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Password has been Successfuly Changed." });
                            //    }
                            //    else
                            //    {
                            //        ////return response
                            //        return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Password not matched" });
                            //    }
                        }
                    }
                    else
                    {
                        ////case of invalid id request.
                        return this.Request.CreateResponse(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    ////case of invalid id request.
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid data" });
                }

            }
            catch (Exception ex)
            {
                //// handel exception log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
            }
        }

        /// <summary>
        /// This method use for get 'Country' list for select list type controls.
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns>key value pair</returns>
        [HttpGet]
        [Route("GetAllCustomerSelectList")]
        public HttpResponseMessage GetAllCustomerSelectList()
        {
            try
            {
                ////get customer list
                var customer = customerService.GetAllCustomerSelectList().OrderBy(x => x.Text).ToList();

                ////check object
                if (customer.Count > 0 && customer != null)
                {
                    ////return task type service for get task type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, customer);
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Data Not Found." });
                }
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// This method use for get 'Country' list for select list type controls.
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns>key value pair</returns>
        [HttpGet]
        [Route("GetAllCountry")]
        public HttpResponseMessage GetAllCountry()
        {
            try
            {
                ////get task type list
                var countries = countryService.GetCountryList();

                ////check object
                if (countries.Count > 0 && countryService != null)
                {
                    ////return task type service for get task type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, countries);
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Data Not Found." });
                }
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// This method use for get 'State' list for select list type controls.
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns>key value pair</returns>
        [HttpGet]
        [Route("GetAllState/{id}")]
        public HttpResponseMessage GetAllState(string id)
        {
            try
            {
                ////get task type list
                var states = stateService.GetAllStateByCountryId(new Guid(id));

                ////check object
                if (states.Count > 0 && countryService != null)
                {
                    ////return task type service for get task type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, states);
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
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// This method use for get 'Bank' list for select list type controls.
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns>key value pair</returns>
        [HttpGet]
        [Route("GetAllBank")]
        public HttpResponseMessage GetAllBank()
        {
            try
            {
                ////get task type list
                var banks = bankService.GetBankList();

                ////check object
                if (banks.Count > 0 && countryService != null)
                {
                    ////return task type service for get task type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, banks);
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
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// This method for get record of penalty from "penalty" by customer id entity.
        /// </summary>
        /// <returns>penalt entity object value.</returns>
        [HttpGet]
        [Route("GetCustomerProduct/{customerId}/{productId}/{lang}")]
        public HttpResponseMessage GetCustomerProduct(string customerId, string productId, string lang)
        {
            try
            {
                ////get ticket list 
                var customerProductDetails = customerProductService.GetAllCustomerProductByProductIdCustomerId(new Guid(customerId), new Guid(productId));


                ////check object
                if (customerProductDetails != null)
                {
                    dynamic customerProduct = new ExpandoObject();
                    ////get ticket list 
                    customerProduct.Id = customerProductDetails.Id;
                    customerProduct.Investment = customerProductDetails.Investment;
                    customerProduct.MiniMarketAmount = customerProductDetails.Investment - customerProductDetails.WalletAmount;
                    customerProduct.BarCode = customerProductDetails.BarCode != null ? customerProductDetails.BarCode : "";
                    customerProduct.BarCodeUrl = customerProductDetails.BarCodeUrl != null ? customerProductDetails.BarCodeUrl : "";
                    customerProduct.CustomerId = customerProductDetails.CustomerId;
                    customerProduct.ProductId = customerProductDetails.ProductId;
                    //customerProduct.BarCode = customerProductDetails.BarCode;
                    // customerProduct.BarCodeUrl = customerProductDetails.BarCodeUrl;
                    customerProduct.CreatedDate = customerProductDetails.CreatedDate;
                    customerProduct.Email = customerProductDetails.Customer.Email;
                    customerProduct.Description = customerProductDetails.Product.Description;
                    //customerProduct.BarCodeUrl = customerProductDetails.BarCodeUrl;
                    //penalty.AutoIncrementedNo = customerProductDetails.PenaltyPercent;

                    string st = resmanager.GetString("Api_CustomerProductDetails", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = customerProduct, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    //return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Data Not Found." });
                    string stDataNot = resmanager.GetString("Api_DataNotFound", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataNot, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = stDataNot });
                }
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);
                //logger.Error("Method: UpdateBookingStatus()", ex);
                // logger.Info("Method: UpdateBookingStatus()" + ex.StackTrace);

                ////return case of exception.
                // return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
                string stFailed = resmanager.GetString("Api_Failed", CultureInfo.GetCultureInfo(lang));
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stFailed, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        #region Customer Mobile API

        /// <summary>
        /// This method for CustomerLogin from "Customer" entity.
        /// </summary>
        /// <returns>customer object value.</returns>
        [Route("CustomerLogin")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> CustomerLogin(LoginDataModel loginModel)
        {
            PromotionDataModel promotionDataModel = new PromotionDataModel();
            // logger.Info("test customer login");
            try
            {
                if (loginModel.Email.Contains("@") == true)
                {
                    Framework.Identity.ApplicationUser user = await UserManager.FindByEmailAsync(loginModel.Email);
                    if (user != null)
                    {
                        loginModel.Email = user.UserName;
                    }
                    else
                    {
                        string stPassInCorrect = resmanager.GetString("Api_Invalidlogin", CultureInfo.GetCultureInfo(loginModel.Lang));
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stPassInCorrect, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "false" });
                    }

                }

                Framework.Identity.ApplicationUser customer = await UserManager.FindAsync(loginModel.Email, loginModel.Password);

                if (customer != null)
                {
                    var objcustomer = customerService.GetCustomerByAspNetUserId(customer.Id.ToString());
                    if (!objcustomer.IsActive)
                    {
                        string stInactiveUser = resmanager.GetString("Api_InactiveUser", CultureInfo.GetCultureInfo(loginModel.Lang));
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stInactiveUser, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "false" });
                    }

                    // get Access Token info
                    var tkn = App_Helper.AuthToken.GetLoingInfo(loginModel);

                    if (tkn != null)
                    {

                        // implement notification logic once weekly earning available to withdraw
                        if (objcustomer.ModifiedDate.Date != DateTime.UtcNow.Date)
                        {
                            var customerProductData = customerProductService.GetAllCustomerProductByCustomerId(objcustomer.Id);
                            foreach (var item in customerProductData)
                            {
                                if (item.LastWeeklyWithdrawEnableDate != null)
                                {
                                    if (item.LastWeeklyWithdrawEnableDate.Value.Date <= DateTime.UtcNow.Date)
                                    {
                                        // for customer notification
                                        ////inserting data into promotion datamodel.
                                        string WeeklyEarningAvailableE = resmanager.GetString("WeeklyEarningAvailableE", CultureInfo.GetCultureInfo("en"));
                                        string WeeklyEarningAvailableS = resmanager.GetString("WeeklyEarningAvailableS", CultureInfo.GetCultureInfo("es"));
                                        string InvestementE = resmanager.GetString("InvestementE", CultureInfo.GetCultureInfo("en"));
                                        string InvestementS = resmanager.GetString("InvestementS", CultureInfo.GetCultureInfo("es"));
                                        promotionDataModel.Description = WeeklyEarningAvailableE + " " + item.Product.Name + " " + InvestementE;
                                        promotionDataModel.DescriptionSpanish = WeeklyEarningAvailableS + " " + item.Product.Name + " " + InvestementS;
                                        promotionDataModel.To = item.CustomerId.ToString();
                                        promotionDataModel.PromotionType = "Alert on Web & App";
                                        promotionDataModel.Subject = "Weekly Earning available to withdraw";
                                        promotionDataModel.SubjectSpanish = "Weekly Earning available to withdraw";

                                        //inserting data into promotion table.
                                        var promotionResponse = promotionService.AddPromotion(promotionDataModel);
                                    }
                                }
                            }
                        }

                        //if (loginModel.DeviceToken != null && loginModel.DeviceType != null)
                        //{
                        //loginModel.Id
                        var response = customerService.EditCustomerByLogin(loginModel);
                        //}
                        // var objcustomer = customerService.GetCustomerByAspNetUserId(customer.Id.ToString());

                        var TokenModel = new AccessTokenModel();
                        TokenModel = tkn;
                        TokenModel.UserId = objcustomer.Id.ToString();
                        TokenModel.UserName = objcustomer.FirstName + " " + objcustomer.LastName;
                        TokenModel.Email = objcustomer.Email;
                        TokenModel.FirstName = objcustomer.FirstName;
                        TokenModel.MiddleName = objcustomer.MiddleName;
                        TokenModel.LastName = objcustomer.LastName;
                        TokenModel.Phone = objcustomer.Phone != null ? objcustomer.Phone : "";
                        // Get message from the resoures file using key
                        string st1 = resmanager.GetString("Api_Loginsuccessfully", CultureInfo.GetCultureInfo(loginModel.Lang));
                        return this.Request.CreateResponse(HttpStatusCode.OK, new LoginResponseDataModel { Message = st1, Succeeded = true, DataObject = TokenModel, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                        //return this.Request.CreateResponse<ExpandoObject>(HttpStatusCode.OK, (ExpandoObject)TokenModel);
                    }
                    string st = resmanager.GetString("Api_Invalidlogin", CultureInfo.GetCultureInfo(loginModel.Lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new LoginResponseDataModel { Message = st, Succeeded = true, DataObject = new object(), DataList = new List<ExpandoObject>(), ErrorInfo = "false" });
                }
                else
                {
                    string stPassInCorrect = resmanager.GetString("Api_Invalidlogin", CultureInfo.GetCultureInfo(loginModel.Lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new LoginResponseDataModel { Message = stPassInCorrect, Succeeded = false, DataObject = new object(), DataList = new List<ExpandoObject>(), ErrorInfo = "false" });
                    //return this.Request.CreateResponse(HttpStatusCode.NoContent, new BaseResponseDataModel { Message = stPassInCorrect, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "", });
                    //return Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);
                ////return case of exception.
                string stSomethinngWentWorng = resmanager.GetString("Api_Invalidlogin", CultureInfo.GetCultureInfo(loginModel.Lang));
                return this.Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, new LoginResponseDataModel { Message = stSomethinngWentWorng, Succeeded = false, DataObject = new object(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// Edit Customer by customer id
        /// </summary>
        /// <param name="customerData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EditCustomerProfile")]
        public HttpResponseMessage EditCustomerProfile(CustomerDataModel customerData)
        {
            try
            {
                ////get service response.
                var response = customerService.EditCustomerProfile(customerData);                ////return result from service response.

                return this.Request.CreateResponse(HttpStatusCode.OK, new { result = response });
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Exception" });

            }
        }

        /// <summary>
        /// This method for customer change password for mobile formate.
        /// </summary>
        /// <returns>status with message</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("CustomerChangePassword")]
        public HttpResponseMessage CustomerChangePassword(CustomerPasswordDataModel customerPasswordData)
        {
            try
            {
                ////step: 1
                ////check is valid email.
                if (ServiceHelper.IsGuid(customerPasswordData.Id))
                {
                    if (ModelState.IsValid)
                    {
                        var customer = customerService.GetCustomerById(new Guid(customerPasswordData.Id));
                        if (customer == null)
                        {
                            // Don't reveal that the user does not exist
                            string st = resmanager.GetString("Api_IDNotExistorPasswordnotmatched", CultureInfo.GetCultureInfo(customerPasswordData.Lang));
                            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                            //return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "ID Not Exist or Password not matched" });
                        }
                        else
                        {
                            var user = UserManager.FindById(customer.UserId);
                            if (user != null)
                            {
                                // encrypting password for company user table
                                var password = SecurityHelper.Encrypt(customerPasswordData.NewPassword, true);

                                //user.PasswordHash = customerPasswordData.NewPassword;
                                var response = UserManager.ChangePassword(customer.UserId, customerPasswordData.OldPassword, customerPasswordData.NewPassword);
                                if (response.Succeeded)
                                {
                                    var result = customerService.EditCustomerPassword(new Guid(customerPasswordData.Id), password);
                                    ////check result
                                    if (result == true)
                                    {
                                        string st1 = resmanager.GetString("Api_PasswordhasbeenSuccessfulyChanged", CultureInfo.GetCultureInfo(customerPasswordData.Lang));
                                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st1, Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                                        //return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Password has been Successfuly Changed" });
                                    }
                                    else
                                    {
                                        ////return response
                                        string st = resmanager.GetString("Api_Currentpasswordisincorrect", CultureInfo.GetCultureInfo(customerPasswordData.Lang));
                                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                                        //return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Current password is incorrect" });
                                    }
                                }
                                else
                                {
                                    string st = resmanager.GetString("Api_Currentpasswordisincorrect", CultureInfo.GetCultureInfo(customerPasswordData.Lang));
                                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                                    //return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Current password is incorrect" });
                                }

                            }
                            else
                            {
                                string stSomethinngWentWorng = resmanager.GetString("Api_SomethingWentWorng", CultureInfo.GetCultureInfo(customerPasswordData.Lang));
                                return this.Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = stSomethinngWentWorng, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                                //return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "company user not exist", Succeeded = false });
                            }
                        }
                    }
                    else
                    {
                        ////case of invalid id request.
                        //return this.Request.CreateResponse(HttpStatusCode.BadRequest);
                        string stSomethinngWentWorng = resmanager.GetString("Api_SomethingWentWorng", CultureInfo.GetCultureInfo(customerPasswordData.Lang));
                        return this.Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, new BaseResponseDataModel { Message = stSomethinngWentWorng, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    }
                }
                else
                {
                    ////case of invalid id request.
                    //return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid data" });
                    string stSomethinngWentWorng = resmanager.GetString("Api_SomethingWentWorng", CultureInfo.GetCultureInfo(customerPasswordData.Lang));
                    return this.Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, new BaseResponseDataModel { Message = stSomethinngWentWorng, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }

            }
            catch (Exception ex)
            {
                //// handel exception log.
                Console.Write(ex.Message);

                ////return case of exception.
                // return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
                return this.Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        //Not use in this app
        /// <summary>
        /// This method for get record of penalty from "penalty" by customer id entity.
        /// </summary>
        /// <returns>penalt entity object value.</returns>
        [HttpGet]
        [Route("GetPenaltyChartInfo/{id}/{lang}")]
        public HttpResponseMessage GetPenaltyChartInfo(string id, string lang)
        {
            try
            {
                ////get ticket list 
                var penaltyDetail = penaltyService.GetPenaltyByCustomerId(new Guid(id));


                ////check object
                if (penaltyDetail != null)
                {
                    dynamic penalty = new ExpandoObject();
                    ////get ticket list 
                    penalty.To = penaltyDetail.To;
                    penalty.From = penaltyDetail.From;
                    penalty.AutoIncrementedNo = penaltyDetail.PenaltyPercent;


                    string st = resmanager.GetString("Api_Penalty", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = penalty, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    //return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Data Not Found." });
                    string stDataNot = resmanager.GetString("Api_DataNotFound", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataNot, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = stDataNot });
                }
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);
                //logger.Error("Method: UpdateBookingStatus()", ex);
                // logger.Info("Method: UpdateBookingStatus()" + ex.StackTrace);

                ////return case of exception.
                // return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
                string stFailed = resmanager.GetString("Api_Failed", CultureInfo.GetCultureInfo(lang));
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stFailed, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// This method for insert record into "Customer" from database entity.
        /// </summary>
        /// <param name="firstName">firstName</param>
        /// <param name="lastName">lastName</param>
        /// <param name="middleName">middleName</param>
        /// <param name="motherLastName">motherLastName</param>
        /// <param name="birthDate">birthDate</param>
        /// <param name="lastName">lastName</param>
        /// <param name="categoryId">CategoryId</param>
        /// <param name="subcategoryId">SubCategoryId</param>
        /// <param name="firstName">firstName</param>
        /// <param name="lastName">lastName</param>
        /// <param name="categoryId">CategoryId</param>
        /// <param name="subcategoryId">SubCategoryId</param>
        /// <returns>Api response result</returns>
        [HttpPost]
        [Route("CreateNewReferalUser")]
        public HttpResponseMessage CreateNewReferalUser(CustomerDataModel customerData)
        {
            var user = new ApplicationUser();
            try
            {
                var response = string.Empty;
                DocumentDataModel documentData = new DocumentDataModel();
                PromotionDataModel promotionDataModel = new PromotionDataModel();
                Guid? avtarId = Guid.Empty;
                ListDictionary replacements = new ListDictionary(); //For email send to customer
                var encryptedPassword = string.Empty;

                var checkEmail = UserManager.FindByEmail(customerData.Email);
                if (checkEmail == null)
                {
                    ////step:1
                    ////add user in AspNetUser.
                    user.UserName = customerData.Email;
                    user.Email = customerData.Email;
                    //user.PasswordHash = SecurityHelper.Encrypt(customerData.Password, true);
                    var result = UserManager.Create(user, customerData.Password);

                    if (result.Succeeded)
                    {
                        ////step:2
                        ////add customer userId in customerData.
                        customerData.UserId = user.Id;

                        customerData.Password = SecurityHelper.Encrypt(customerData.Password, true);
                        ////get service response.
                        response = customerService.AddCustomer(customerData);

                        if (ServiceHelper.IsGuid(response))
                        {
                            // getting customer detail
                            var customerDetail = customerService.GetCustomerById(new Guid(customerData.CustomerReferalId));

                            // new referal notification alert
                            ////inserting data into promotion datamodel.
                            promotionDataModel.Description = "New Referal added by " + customerDetail.FirstName + " " + customerDetail.LastName;
                            promotionDataModel.DescriptionSpanish = "New Referal added by " + customerDetail.FirstName + " " + customerDetail.LastName;
                            promotionDataModel.To = null;
                            promotionDataModel.PromotionType = "Alert on Web & App";
                            promotionDataModel.Subject = "New Referal added";
                            promotionDataModel.SubjectSpanish = "New Referal added";
                            promotionDataModel.Url = projectPathUrl + projectName + "/CustomerManagement/SalesList/" + customerDetail.Id;

                            ////inserting data into promotion table.
                            var promotionResponse = promotionService.AddPromotion(promotionDataModel);

                            // new referal notification alert to same user who is adding (Customer)
                            ////inserting data into promotion datamodel.
                            string NewReferalE = resmanager.GetString("NewReferalE", CultureInfo.GetCultureInfo("en"));
                            string NewReferalS = resmanager.GetString("NewReferalS", CultureInfo.GetCultureInfo("es"));
                            string NewReferalSubE = resmanager.GetString("NewReferalSubE", CultureInfo.GetCultureInfo("en"));
                            string NewReferalSubS = resmanager.GetString("NewReferalSubS", CultureInfo.GetCultureInfo("es"));

                            promotionDataModel.Description = NewReferalE + " " + customerDetail.FirstName + " " + customerDetail.LastName;
                            promotionDataModel.DescriptionSpanish = NewReferalS + " " + customerDetail.FirstName + " " + customerDetail.LastName;
                            promotionDataModel.To = customerDetail.Id.ToString();
                            promotionDataModel.PromotionType = "Alert on Web & App";
                            promotionDataModel.Subject = NewReferalSubE;
                            promotionDataModel.SubjectSpanish = NewReferalSubS;
                            promotionDataModel.Url = projectPathUrl + projectName + "/Sale/MySales";

                            ////inserting data into promotion table.
                            var promotionCustomerResponse = promotionService.AddPromotion(promotionDataModel);

                            // new referal notification alert
                            ////inserting data into promotion datamodel.
                            promotionDataModel.Description = customerDetail.FirstName + " " + customerDetail.LastName + " account created successfully";
                            promotionDataModel.DescriptionSpanish = customerDetail.FirstName + " " + customerDetail.LastName + " account created successfully";
                            promotionDataModel.To = null;
                            promotionDataModel.PromotionType = "Alert on Web & App";
                            promotionDataModel.Subject = "New account created";
                            promotionDataModel.SubjectSpanish = "New account created";
                            promotionDataModel.Url = projectPathUrl + projectName + "/CustomerManagement/GeneralDetails/" + customerDetail.Id;

                            ////inserting data into promotion table.
                            var adminPromotionResponse = promotionService.AddPromotion(promotionDataModel);

                        }
                        #region Save data in document entity.

                        if (response != null)
                        {
                            if (customerData.CustomerReferalId != null)
                            {
                                dynamic customerModel = new ExpandoObject();
                                customerModel.FirstName = customerData.FirstName;
                                customerModel.Email = customerData.Email;
                                customerModel.Password = customerData.Password;

                                string coupon = SecurityHelper.Decrypt(customerData.Password, true);
                                replacements = new ListDictionary { { "<%FirstName%>", customerData.FirstName } };
                                replacements.Add("<%Subject%>", "Register");
                                replacements.Add("<%LastName%>", customerData.LastName);
                                replacements.Add("<%email%>", customerData.Email);
                                replacements.Add("<%Password%>", coupon);

                                ////add email data
                                var sendEmail = emailService.AddEmailData(response, customerData.Email, string.Empty, string.Empty, string.Empty, EmailTemplatesHelper.CustomerCreateEmail, replacements);

                                ////send email.
                                if (sendEmail != null)
                                {
                                    ////Code for Sending Email
                                    emailService.SendEmailAsync(new Guid(response), customerData.Email, string.Empty, string.Empty, customerData.Email, EmailTemplatesHelper.CustomerCreateEmail, encryptedPassword, replacements);
                                }

                            }
                            if (customerData.ImageBase64 != null)
                            {
                                String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                //step-3//
                                ////bind file data in a document model.
                                documentData.Id = Guid.NewGuid().ToString();
                                documentData.OriginalName = "Image";
                                documentData.Name = response + "_CustomerImage_" + ".png";
                                documentData.Title = "Images";
                                documentData.Description = string.Empty;
                                documentData.Tags = string.Empty;
                                documentData.URL = strUrl + "/Pictures/" + response + "_CustomerImage_" + ".png";//provider.FileData.FirstOrDefault().LocalFileName;//
                                documentData.Extension = ".png";
                                documentData.ThumbnailFileName = "";
                                documentData.FileSize = 0;
                                documentData.Private = true;

                                //step-4//
                                ////save document data.
                                avtarId = documentService.AddDocument(
                                    new Guid(documentData.Id),
                                    documentData.OriginalName,
                                    documentData.Name,
                                    documentData.URL,
                                    documentData.Title,
                                    documentData.Description,
                                    documentData.Extension,
                                    documentData.FileSize,
                                    documentData.Private,
                                    documentData.Tags,
                                    documentData.ThumbnailFileName
                                    );
                            }
                            else
                            {
                                ////return result from service response.
                                string st = resmanager.GetString("Api_CustomerInsertedSuccessfully", CultureInfo.GetCultureInfo(customerData.Lang));
                                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                                // return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Customer successfully inserted", Succeeded = true });

                            }
                        }
                        #endregion
                        ////check is insert on database.
                        if (avtarId != Guid.Empty && avtarId != null)
                        {
                            #region Update Logo in customer entity.
                            //step-12//
                            ////get update response
                            var isUpdate = customerService.UpdateLogo(new Guid(response), avtarId);
                            var imageUploadStatus = UploadImage.base64ToImage(customerData.ImageBase64, response);
                            #endregion
                            string st = resmanager.GetString("Api_CustomerInsertedSuccessfully", CultureInfo.GetCultureInfo(customerData.Lang));
                            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                            //return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Customer has been inserted successfully", Succeeded = true });
                        }
                        else
                        {
                            string stFailed = resmanager.GetString("Api_SomethingWentWorng", CultureInfo.GetCultureInfo(customerData.Lang));
                            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stFailed, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                            // return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false });
                        }
                        ////return result from service response.
                    }
                    else
                    {
                        string stSomethingWentWorng = resmanager.GetString("Api_SomethingWentWorng", CultureInfo.GetCultureInfo(customerData.Lang));
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stSomethingWentWorng, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                        //return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Not inserted in aspnet user table", Succeeded = false });
                    }
                }
                else
                {
                    //////return response
                    string sEmailAddressNotExist = resmanager.GetString("EmailAlreadyExist", CultureInfo.GetCultureInfo(customerData.Lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = sEmailAddressNotExist, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    //return this.Request.CreateResponse(HttpStatusCode.BadRequest, new BaseResponseDataModel { Message = "Email already exist", Succeeded = false });
                }
            }
            catch (Exception ex)
            {
                var result = UserManager.Delete(user);

                //// Handel Exception Log.
                Console.Write(ex.Message);
                ////return case of exception.
                string stFailed = resmanager.GetString("Api_SomethingWentWorng", CultureInfo.GetCultureInfo(customerData.Lang));
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, new BaseResponseDataModel { Message = stFailed, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.InnerException.ToString() });

                //return this.Request.CreateResponse(HttpStatusCode.BadRequest, new BaseResponseDataModel { Message = "Exception", Succeeded = false });
            }
        }

        /// <summary>
        /// This method use for get 'Country' list for select list type controls for mobile.
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns>key value pair</returns>
        [HttpGet]
        [Route("GetAllCountries")]
        public HttpResponseMessage GetAllCountries()
        {
            try
            {
                ////get task type list
                var countries = countryService.GetCountryList();

                ////check object
                if (countries.Count > 0 && countryService != null)
                {
                    List<ExpandoObject> countrylist = new List<ExpandoObject>();
                    foreach (var country in countries)
                    {
                        dynamic countryDetail = new ExpandoObject();
                        countryDetail.CountryName = country.Text;
                        countryDetail.CountryId = country.Value;
                        countrylist.Add(countryDetail);
                    }
                    ////return task type service for get task type.
                    //return this.Request.CreateResponse(HttpStatusCode.OK, countries);
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Country List", Succeeded = true, DataObject = new ExpandoObject(), DataList = countrylist, ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    // return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Data Not Found." });
                }
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                // return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// This method use for get 'State' list for select list type controls for mobile.
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns>key value pair</returns>
        [HttpGet]
        [Route("GetAllStates/{countryid}")]
        public HttpResponseMessage GetAllStates(string countryid)
        {
            try
            {
                ////get task type list
                var states = stateService.GetAllStateByCountryId(new Guid(countryid));

                ////check object
                if (states.Count > 0 && countryService != null)
                {
                    List<ExpandoObject> statelist = new List<ExpandoObject>();
                    foreach (var state in states)
                    {
                        dynamic stateDetail = new ExpandoObject();
                        stateDetail.stateName = state.Text;
                        stateDetail.stateId = state.Value;
                        statelist.Add(stateDetail);
                    }
                    ////return task type service for get task type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "State List", Succeeded = true, DataObject = new ExpandoObject(), DataList = statelist, ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    // return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Data Not Found." });
                }
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
                //return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// This method use for get 'Bank' list for select list type controls for mobile.
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns>key value pair</returns>
        [HttpGet]
        [Route("GetAllBanks")]
        public HttpResponseMessage GetAllBanks()
        {
            try
            {
                ////get task type list
                var banks = bankService.GetBankList();

                ////check object
                if (banks.Count > 0 && bankService != null)
                {
                    ////return task type service for get task type.
                    //return this.Request.CreateResponse(HttpStatusCode.OK, banks);
                    List<ExpandoObject> banklist = new List<ExpandoObject>();
                    foreach (var bank in banks)
                    {
                        dynamic bankDetail = new ExpandoObject();
                        bankDetail.bankName = bank.Text;
                        bankDetail.bankId = bank.Value;
                        banklist.Add(bankDetail);
                    }
                    ////return task type service for get task type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Bank List", Succeeded = true, DataObject = new ExpandoObject(), DataList = banklist, ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    // return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Data Not Found." });
                }
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                //return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// Edit Customer by customer id
        /// </summary>
        /// <param name="customerData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EditCustomerMyProfile")]
        public HttpResponseMessage EditCustomerMyProfile(CustomerDataModel customerData)
        {
            try
            {
                ////get service response.
                var response = customerService.EditCustomerProfile(customerData);                ////return result from service response.
                string st1 = resmanager.GetString("Api_ProfilehasbeenSuccessfulyUpdated", CultureInfo.GetCultureInfo(customerData.Lang));
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st1, Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                //return this.Request.CreateResponse(HttpStatusCode.OK, new { result = response });
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                string stSomethinngWentWorng = resmanager.GetString("Api_SomethingWentWorng", CultureInfo.GetCultureInfo(customerData.Lang));
                return this.Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, new BaseResponseDataModel { Message = stSomethinngWentWorng, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                //return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Exception" });

            }
        }

        /// <summary>
        /// Get method for Delete CustomerProduct By CustomerId and ProductId
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteCustomerProductByCustomerIdProductId/{customerId}/{productId}")]
        public HttpResponseMessage DeleteCustomerProductByCustomerIdProductId(string customerId, string productId)
        {
            var deleteCustomerProductData = customerProductService.DeleteCustomerProductByCustomerIdProductId(new Guid(customerId), new Guid(productId));
            if (deleteCustomerProductData)
            {
                ////return task type service for get task type.
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Delete data", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                // return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Data Not Found." });
            }
        }

        #endregion
    }
}
