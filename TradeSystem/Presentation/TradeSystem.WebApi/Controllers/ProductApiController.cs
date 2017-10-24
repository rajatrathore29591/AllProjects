using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using TradeSystem.Framework.Identity;
using TradeSystem.Service;
using TradeSystem.Utils;
using TradeSystem.Utils.Enum;
using TradeSystem.Utils.Models;
using TradeSystem.WebApi.Controllers;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Collections.Specialized;
using TradeSystem.Utilities.Email;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;
using System.Configuration;

namespace TradeSystem.MVCWeb.Controllers
{
    //[Authorize]
    [RoutePrefix("api/product")]
    public class ProductApiController : BaseApiController
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        private Framework.Identity.ApplicationUserManager userManager;
        IProductService productService;
        IPenaltyService penaltyService;
        ICustomerProductService customerProductService;
        IDocumentService documentService;
        IActivityService activityService;
        IEmailService emailService;
        ICustomerService customerService;
        IWithdrawService withdrawService;
        IWalletService walletService;

        //Initialized Product Api Controller Constructor.
        public ProductApiController(IProductService _productService, ICustomerProductService _customerProductService, IPenaltyService _penaltyService, IDocumentService _documentService, IActivityService _activityService, IEmailService _emailService, ICustomerService _customerService, IWithdrawService _withdrawService, IWalletService _walletService)
        {
            productService = _productService;
            penaltyService = _penaltyService;
            customerProductService = _customerProductService;
            documentService = _documentService;
            activityService = _activityService;
            emailService = _emailService;
            customerService = _customerService;
            withdrawService = _withdrawService;
            walletService = _walletService;
        }
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
        string pictures = WebConfigurationManager.AppSettings["Pictures"].ToString();

        /// <summary>
        /// This method for get all record from "product, customer and customerproduct" entity.
        /// </summary>
        /// <returns>product, customer and customerproduct entity object value.</returns>
        [HttpGet]
        [Route("GetAllProduct")]
        public HttpResponseMessage GetAllProduct()
        {
            try
            {
                ////get product list 
                var productCollection = productService.GetAllProduct().OrderByDescending(x => x.CreatedDate).ToList();

                ////check object
                if (productCollection.Count > 0 && productCollection != null)
                {
                    ////dynamic list.
                    dynamic productDetails = new List<ExpandoObject>();
                    ////bind dynamic property.

                    ////return response from product service.
                    foreach (var productDetail in productCollection)
                    {
                        dynamic product = new ExpandoObject();
                        
                        ////get customer list using product id 
                        var customerProductCollection = customerProductService.GetAllCustomerProductByProductId(productDetail.Id);
                       
                        ////get product list 
                        string DateString = productDetail.CreatedDate.ToString("MM-dd-yyyy HH:mm:ss tt");
                        product.CreatedDate = DateString;
                        product.Id = productDetail.Id;
                        product.Name = productDetail.Name;
                        product.IsActive = productDetail.IsActive == true ? "Active" : "Inactive";
                        product.TotalValueOfInvestment = productDetail.TotalValueOfInvestment;
                        product.RemainingValueOfInvestment = productDetail.RemainingValueOfInvestment;
                        product.CustomerProductsCount = customerProductCollection.Count;

                        ////set all values in list.
                        productDetails.Add(product);
                    }

                    ////return all service 
                    return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK, (List<ExpandoObject>)productDetails);
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
        /// This method use for get 'Product' list for select list type controls.
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns>key value pair</returns>
        [HttpGet]
        [Route("GetProductSelectList")]
        public HttpResponseMessage GetProductSelectList()
        {
            try
            {
                ////get task type list
                var products = productService.GetProductSelectList().OrderBy(x => x.Text).ToList();

                ////check object
                if (products.Count > 0 && products != null)
                {
                    ////return task type service for get task type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, products);
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
        /// This method for get all customer sale record from "customerProduct" entity.
        /// productId
        /// customerId
        /// </summary>
        /// <returns>CustomerProduct entity object value.</returns>
        [HttpGet]
        [Route("GetProductByProductId/{id}/{customerId}")]
        public HttpResponseMessage GetProductByProductId(string id, string customerId)
        {
            try
            {
                //get product name by ids
                bool IsInvest = false;
                string[] productIds;
                CommaDelimitedStringCollection list = new CommaDelimitedStringCollection();
                string ProductName = "0";

                ////get product, customer and customerProduct list 
                var productDetails = productService.GetProductByProductId(new Guid(id));

                //for check is already invest or not
                if (customerId != "0")
                {
                    var getCustomerProduct = customerProductService.GetAllCustomerProductByProductIdCustomerId(new Guid(customerId), new Guid(id));
                    IsInvest = getCustomerProduct != null ? IsInvest = true : IsInvest = false;
                }

                // getting product name from product ids
                if (productDetails.Condition3 != "0")
                {
                    ProductName = "";
                    productIds = productDetails.Condition3.Split(',');
                    var productNameWithSpace = string.Empty;
                    foreach (var Id in productIds)
                    {
                        var product = productService.GetProductByProductId(new Guid(Id)).Name;
                        if (productNameWithSpace == "")
                        {
                            productNameWithSpace = product;
                        }
                        else
                        {
                            productNameWithSpace = productNameWithSpace + ", " + product;
                        }
                    }
                    ProductName = productNameWithSpace;
                }

                //We have comment for customerId is pass "0"
                // code to for the condition
                bool satisfy = true;// variable for checking conditions satisfied or not
                if (customerId != "0")
                {
                    // customer product list
                    var customerProduct = customerProductService.GetAllCustomerProductByCustomerId(new Guid(customerId)).Where(x => x.Status != "Reject");

                    // calculate sum of investment by user
                    var sum = customerProduct.Sum(x => x.Investment);

                    // sale count 
                    var customerSaleCount = customerService.GetAllReferalCustomerByCustomerId(new Guid(customerId)).Count;
                    if (float.Parse(productDetails.Condition1) <= sum && Convert.ToInt32(productDetails.Condition2) <= customerSaleCount)
                    {
                        if (productDetails.Condition3 == "0")
                        {
                            // (Show) this product.
                            satisfy = true;
                        }
                        else
                        {
                            bool canAdd = true;
                            string[] refProductList = productDetails.Condition3.Split(',');
                            foreach (var refPro in refProductList)
                            {
                                var matched = customerProduct.Where(st => st.ProductId == new Guid(refPro)).FirstOrDefault();
                                if (matched == null) canAdd = false;
                                if (!canAdd) break;
                            }
                            if (canAdd)
                            {
                                // (Show) this product.
                                satisfy = true;
                            }
                            else
                            {
                                satisfy = false;
                            }
                        }
                    }
                    else
                    {
                        satisfy = false;
                    }
                }

                ////check object
                if (productDetails != null)
                {

                    ////bind dynamic property.
                    dynamic product = new ExpandoObject();

                    product.Id = productDetails.Id;
                    product.Name = productDetails.Name;
                    product.TotalValueOfInvestment = productDetails.TotalValueOfInvestment;
                    product.RemainingValueOfInvestment = productDetails.RemainingValueOfInvestment;
                    product.MinPrice = productDetails.MinPrice;
                    product.MaxPrice = productDetails.MaxPrice;
                    product.Description = productDetails.Description;
                    product.PercentWeeklyEarning = productDetails.PercentWeeklyEarning;
                    product.PercentSaleEarning = productDetails.PercentSaleEarning;
                    product.WeeklyToWithdrawDay = productDetails.WeeklyToWithdrawDay;
                    product.WeeklyFromWithdrawDay = productDetails.WeeklyFromWithdrawDay;
                    product.SaleWithdrawDay = productDetails.SaleWithdrawDay;
                    product.InvestmentWithdrawDate = productDetails.InvestmentWithdrawDate.ToString("MM-dd-yyyy");
                    product.CreatedDate = productDetails.CreatedDate.ToString("MM-dd-yyyy");
                    product.Condition1 = productDetails.Condition1;
                    product.Condition2 = productDetails.Condition2;
                    product.Condition3 = ProductName;
                    product.TotalDaysOfInvestment = Convert.ToInt32((productDetails.InvestmentWithdrawDate - productDetails.CreatedDate).TotalDays);
                    product.ImageUrl = productDetails.Document != null ? projectPathUrl + projectName + pictures + productDetails.Document.Name : projectPathUrl + projectName + "/Pictures/48cb4537-3c0b-4de4-bb7b-31e1df9e71d5_ProductImage_.png";
                    product.IsInvest = IsInvest;
                    product.AllowUserToInvest = satisfy;

                    if (productDetails.RemainingValueOfInvestment <= productDetails.MinPrice)
                    {
                        product.LastRemainingInvestmentAmount = true;
                    }
                    else
                    {
                        product.LastRemainingInvestmentAmount = false;
                    }
                    ////return all service 
                    return this.Request.CreateResponse<ExpandoObject>(HttpStatusCode.OK, (ExpandoObject)product);
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
        /// This method for insert record into "Skill" from database entity.
        /// </summary>
        /// <param name="name">SkillName</param>
        /// <param name="description">Description</param>
        /// <param name="categoryId">CategoryId</param>
        /// <param name="subcategoryId">SubCategoryId</param>
        /// <returns>Api response result</returns>
        [HttpPost]
        [Route("AddProduct")]
        public HttpResponseMessage AddProduct(ProductDataModel productDataModel)
        {
            try
            {
                var response = string.Empty;
                DocumentDataModel documentData = new DocumentDataModel();
                Guid? avtarId = Guid.Empty;
                List<string> ids = new List<string>();
                List<CustomerProduct> customerProductObj = new List<CustomerProduct>();
                productDataModel.WeeklyFromWithdrawDay = productDataModel.DurationWithdraw == "Monthly" ? productDataModel.MonthlyToWithdrawDay : productDataModel.WeeklyFromWithdrawDay;

                ////inserting data into product table.
                response = productService.AddProduct(productDataModel);
                if (response != null)
                {
                    if (productDataModel.ByUserName != null)
                    {
                        var customerId = customerService.GetCustomerById(new Guid(productDataModel.ByUserName)).Id;
                        ids.Add(customerId.ToString());
                    }
                    if (productDataModel.ByCountry != null)
                    {
                        var countryWiseCustomer = customerService.GetCustomerListByCountryId(new Guid(productDataModel.ByCountry));
                        foreach (var item in countryWiseCustomer)
                        {
                            ids.Add(item.Id.ToString());
                        }
                    }
                    if (productDataModel.ByState != null)
                    {
                        var stateWiseCustomer = customerService.GetCustomerListByStateId(new Guid(productDataModel.ByState));
                        foreach (var item in stateWiseCustomer)
                        {
                            ids.Add(item.Id.ToString());
                        }
                    }
                    if (productDataModel.ByFromAmount != null && productDataModel.ByToAmount != null)
                    {
                        customerProductObj = customerProductService.GetAllCustomerProduct();
                        var groupedData = customerProductObj.GroupBy(x => x.CustomerId).Select(y => new
                        {
                            Id = y.Key,
                            Investment = y.Sum(x => x.Investment)

                        }).ToList();

                        foreach (var item in groupedData)
                        {
                            if (float.Parse(productDataModel.ByFromAmount) <= item.Investment && float.Parse(productDataModel.ByToAmount) >= item.Investment)
                            {
                                ids.Add(item.Id.ToString());
                            }
                        }
                    }

                    if (productDataModel.ByFromSale != null && productDataModel.ByToSale != null)
                    {
                        var groupedData = customerProductObj.GroupBy(x => x.CustomerId).Select(y => new
                        {
                            Id = y.Key,
                            Count = y.Count()
                        }).ToList();

                        foreach (var item in groupedData)
                        {
                            if (float.Parse(productDataModel.ByFromSale) <= item.Count && float.Parse(productDataModel.ByToSale) >= item.Count)
                            {
                                ids.Add(item.Id.ToString());
                            }
                        }
                    }

                    Task task = Task.Run(async () =>
                    {
                        await SendMail(ids, productDataModel.Name);
                    });
                }

                // inserting data into activity table
                ActivityLogDataModel activityObj = new ActivityLogDataModel();
                activityObj.Activity = ETaskStatus.AddProduct.ToString();
                activityObj.Description = "Added a Product named : " + productDataModel.Name;
                activityObj.IsCompanyUser = true;
                activityObj.CompanyUserId = productDataModel.CompanyUserId;
                var activityResult = activityService.AddActivity(activityObj);
                #region Save data in document entity.

                if (!string.IsNullOrEmpty(productDataModel.TableJson))
                {
                    List<PenaltyDataModel> penaltyDetails = new List<PenaltyDataModel>();
                    productDataModel.TableJson = "[" + productDataModel.TableJson + "]";
                    var resultJson = JsonConvert.DeserializeObject<List<PenaltyDataModel>>(productDataModel.TableJson);
                    if (resultJson != null)
                    {
                        foreach (var penalty in resultJson)
                        {
                            penalty.ProductId = response;
                            var responseData = penaltyService.AddPenalty(penalty);
                        }
                    }
                }

                if (response != null)
                {
                    if (productDataModel.ImageBase64 != null)
                    {
                        String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                        String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

                        ////bind file data in a document model.
                        documentData.Id = Guid.NewGuid().ToString();
                        documentData.OriginalName = "Image";
                        documentData.Name = response + "_ProductImage_" + ".png";
                        documentData.Title = "Images";
                        documentData.Description = string.Empty;
                        documentData.Tags = string.Empty;
                        documentData.URL = strUrl + projectUrl + "/Pictures/" + response + "_ProductImage_" + ".png";//provider.FileData.FirstOrDefault().LocalFileName;//
                        documentData.Extension = ".png";
                        documentData.ThumbnailFileName = "";
                        documentData.FileSize = 0;
                        documentData.Private = true;

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
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Customer successfully inserted", Succeeded = true });
                    }
                }

                #endregion
                ////check is insert on database.
                if (avtarId != Guid.Empty && avtarId != null)
                {
                    #region Update Logo in product entity.
                    ////get update response
                    var isUpdate = productService.UpdateLogo(new Guid(response), avtarId);
                    var imageUploadStatus = UploadImage.base64ToImage(productDataModel.ImageBase64, response + "_ProductImage_" + ".png");
                    #endregion
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Customer successfully inserted", Succeeded = true });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false });
                }

            }

            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);
                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Get method for send mail
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private async Task<bool> SendMail(List<string> ids, string name)
        {
            foreach (var id in ids)
            {
                var customer = customerService.GetCustomerById(new Guid(id));
                ListDictionary replacements = new ListDictionary();
                replacements = new ListDictionary { { "<%FirstName%>", customer.FirstName } };
                replacements.Add("<%LastName%>", customer.LastName);
                replacements.Add("<%InvestemtName%>", name);
                ////step:7
                ////add email data
                var sendEmail = emailService.AddEmailData(id, customer.Email, string.Empty, string.Empty, string.Empty, EmailTemplatesHelper.NewProductEmail, replacements);

                ////step:8
                ////send email.
                if (sendEmail != null)
                {
                    ////Code for Sending Email
                    emailService.SendEmailAsync(new Guid(id), customer.Email, string.Empty, string.Empty, customer.Email, EmailTemplatesHelper.NewProductEmail, "", replacements);
                }
            }
            return true;
        }

        /// <summary>
        /// This method for get all record from "product, customer and customerproduct" entity.
        /// </summary>
        /// <returns>product, customer and customerproduct entity object value.</returns>
        [HttpGet]
        [Route("GetAllPenaltyByProductId/{productid}")]
        public HttpResponseMessage GetAllPenaltyByProductId(string productid)
        {
            try
            {
                ////get penalty list 
                var penaltyCollection = penaltyService.GetAllPenaltyByProductId(productid).OrderBy(x => int.Parse(x.From)).ToList();

                ////check object
                if (penaltyCollection.Count > 0 && penaltyCollection != null)
                {
                    ////dynamic list.
                    dynamic penaltyDetails = new List<ExpandoObject>();

                    ////bind dynamic property.
                    ////return response from penalty service.
                    foreach (var penaltyDetail in penaltyCollection)
                    {
                        dynamic penalty = new ExpandoObject();

                        ////get product list 
                        penalty.PenaltyFrom = penaltyDetail.From;
                        penalty.PenaltyTo = penaltyDetail.To;
                        penalty.PenaltyPercent = penaltyDetail.PenaltyPercent;

                        ////set all values in list.
                        penaltyDetails.Add(penalty);
                    }


                    ////return all service 
                    return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK, (List<ExpandoObject>)penaltyDetails);
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

        #region Product Mobile Api

        /// <summary>
        /// This method for get all record from "product, customer and customerproduct" entity.
        /// </summary>
        /// <returns>product, customer and customerproduct entity object value.</returns>
        [HttpGet]
        [Route("GetInvestments/{customerId}/{lang}/{filterKey}/{filterValue}")]
        public HttpResponseMessage GetInvestments(string customerId, string lang, string filterKey, string filterValue)
        {
            try
            {
                // customer product list
                var customerProduct = customerProductService.GetAllCustomerProductByCustomerId(new Guid(customerId)).Where(x => x.Status != "Reject");
                
                // calculate sum of investment by user
                var sum = customerProduct.Sum(x => x.Investment);
                
                // sale count 
                var customerSaleCount = customerService.GetAllReferalCustomerByCustomerId(new Guid(customerId)).Count;
               
                // Get Product List
                var productCollection = productService.GetAllProduct().Where(a => !customerProduct.Select(z => z.ProductId).Contains(a.Id)).OrderByDescending(x => x.CreatedDate).ToList();
                productCollection = productCollection.Where(x => x.RemainingValueOfInvestment != 0).ToList();
                List<Product> productList = new List<Product>();
                
                // change the logic earlier we are not showing product if condition not staisfied, so remove the code and assign productCollection to productList
                productList = productCollection.Where(y => y.InvestmentWithdrawDate > DateTime.UtcNow).ToList();

                if (filterKey == "date")
                {
                    productList = productList.Where(x => x.CreatedDate.ToString("MM-dd-yyyy") == filterValue).ToList();
                }
                else if (filterKey == "status")
                {
                    productList = productList.Where(x => x.IsActive.ToString() == filterValue).ToList();
                }
                ////check object
                if (productList.Count > 0 && productList != null)
                {
                    ////dynamic list.
                    dynamic productDetails = new List<ExpandoObject>();
                    ////bind dynamic property.

                    ////return response from product service.
                    foreach (var productDetail in productList)
                    {
                        dynamic product = new ExpandoObject();
                        ////get customer list using product id 
                        var customerProductCollection = customerProductService.GetAllCustomerProductByProductId(productDetail.Id);
                        ////get product list 
                        product.CreatedDate = productDetail.CreatedDate.Date;
                        product.Id = productDetail.Id;
                        product.Name = productDetail.Name;
                        product.IsActive = productDetail.IsActive == true ? "Active" : "InActive";
                        product.TotalValueOfInvestment = productDetail.TotalValueOfInvestment;
                        product.RemainingValueOfInvestment = productDetail.RemainingValueOfInvestment;
                        product.CustomerProductsCount = customerProductCollection.Count;
                        product.MinPrice = productDetail.MinPrice;
                        product.MaxPrice = productDetail.MaxPrice;
                        product.ImageUrl = productDetail.Document == null ? projectPathUrl + projectName + "/Pictures/48cb4537-3c0b-4de4-bb7b-31e1df9e71d5_ProductImage_.png" : projectPathUrl + projectName + pictures + productDetail.Document.Name;
                        product.PercentWeeklyEarning = productDetail.PercentWeeklyEarning;
                        product.PercentSaleEarning = productDetail.PercentSaleEarning;
                        ////set all values in list.
                        productDetails.Add(product);
                    }

                    ////return all service 
                    string st = resmanager.GetString("Api_ProductList", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = new ExpandoObject(), DataList = productDetails, ErrorInfo = "" });
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
        /// This method for get all customer sale record from "customerProduct" entity.
        /// </summary>
        /// <returns>CustomerProduct entity object value.</returns>
        [HttpGet]
        [Route("GetInvestmentsDetailById/{id}/{customerId}/{lang}")]
        public HttpResponseMessage GetInvestmentsDetailById(string id, string customerId, string lang)
        {
            try
            {
                //get product name by ids
                bool IsInvest = false;
                string[] productIds;
                CommaDelimitedStringCollection list = new CommaDelimitedStringCollection();
                string ProductName = "0";
                ////get product, customer and customerProduct list 
                var productDetails = productService.GetProductByProductId(new Guid(id));

                //for check is already invest or not
                if (customerId != "0")
                {
                    var getCustomerProduct = customerProductService.GetAllCustomerProductByProductIdCustomerId(new Guid(customerId), new Guid(id));
                    IsInvest = getCustomerProduct != null ? IsInvest = true : IsInvest = false;
                }

                // getting product name from product ids
                if (productDetails.Condition3 != "0")
                {
                    ProductName = "";
                    productIds = productDetails.Condition3.Split(',');
                    foreach (var Id in productIds)
                    {
                        var product = productService.GetProductByProductId(new Guid(Id)).Name;
                        list.Add(product);
                    }
                    ProductName = list.ToString();
                }

                // code to for the condition
                bool satisfy = true;// variable for checking conditions satisfied or not
                if (customerId != "0")
                {
                    // customer product list
                    var customerProduct = customerProductService.GetAllCustomerProductByCustomerId(new Guid(customerId)).Where(x => x.Status != "Reject");
                    // calculate sum of investment by user
                    var sum = customerProduct.Sum(x => x.Investment);
                    // sale count 
                    var customerSaleCount = customerService.GetAllReferalCustomerByCustomerId(new Guid(customerId)).Count;
                    if (float.Parse(productDetails.Condition1) <= sum && Convert.ToInt32(productDetails.Condition2) <= customerSaleCount)
                    {
                        if (productDetails.Condition3 == "0")
                        {
                            // (Show) this product.
                            satisfy = true;
                        }
                        else
                        {
                            bool canAdd = true;
                            string[] refProductList = productDetails.Condition3.Split(',');
                            foreach (var refPro in refProductList)
                            {
                                var matched = customerProduct.Where(st => st.ProductId == new Guid(refPro)).FirstOrDefault();
                                if (matched == null) canAdd = false;
                                if (!canAdd) break;
                            }
                            if (canAdd)
                            {
                                // (Show) this product.
                                satisfy = true;
                            }
                            else
                            {
                                satisfy = false;
                            }
                        }
                    }
                    else
                    {
                        satisfy = false;
                    }
                }

                ////check object
                if (productDetails != null)
                {
                    ////bind dynamic property.
                    dynamic product = new ExpandoObject();
                    product.Id = productDetails.Id;
                    product.Name = productDetails.Name;
                    product.TotalValueOfInvestment = productDetails.TotalValueOfInvestment;
                    product.RemainingValueOfInvestment = productDetails.RemainingValueOfInvestment;
                    product.MinPrice = productDetails.MinPrice;
                    product.MaxPrice = productDetails.MaxPrice;
                    product.Description = productDetails.Description == null ? "" : productDetails.Description;
                    product.PercentWeeklyEarning = productDetails.PercentWeeklyEarning;
                    product.PercentSaleEarning = productDetails.PercentSaleEarning;
                    product.WeeklyToWithdrawDay = productDetails.WeeklyToWithdrawDay;
                    product.WeeklyFromWithdrawDay = productDetails.WeeklyFromWithdrawDay;
                    product.InvestmentWithdrawDate = productDetails.InvestmentWithdrawDate;
                    product.SaleWithdrawDay = productDetails.SaleWithdrawDay;
                    product.Condition1 = productDetails.Condition1;
                    product.Condition2 = productDetails.Condition2;
                    product.Condition3 = ProductName;
                    product.ImageUrl = productDetails.Document == null ? projectPathUrl + projectName + "/Pictures/48cb4537-3c0b-4de4-bb7b-31e1df9e71d5_ProductImage_.png" : projectPathUrl + projectName + pictures + productDetails.Document.Name;
                    product.AllowUserToInvest = satisfy;
                    product.IsInvest = IsInvest;


                    ////return all service 
                    string st = resmanager.GetString("Api_ProductDetails", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = product, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    // return this.Request.CreateResponse<ExpandoObject>(HttpStatusCode.OK, (ExpandoObject)product);
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

                ////return case of exception.
                string stFailed = resmanager.GetString("Api_Failed", CultureInfo.GetCultureInfo(lang));
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stFailed, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
                //return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
            }
        }

        /// <summary>
        /// This method for get all record from "product, customer and customerproduct" entity.
        /// </summary>
        /// <returns>product, customer and customerproduct entity object value.</returns>
        [HttpGet]
        [Route("GetPenaltyChartInfoByProductId/{productid}/{lang}")]
        public HttpResponseMessage GetPenaltyChartInfoByProductId(string productid, string lang)
        {
            try
            {
                ////get penalty list 
                var penaltyCollection = penaltyService.GetAllPenaltyByProductId(productid).OrderBy(x => x.From).ToList();

                ////check object
                if (penaltyCollection.Count > 0 && penaltyCollection != null)
                {
                    ////dynamic list.
                    dynamic penaltyDetails = new List<ExpandoObject>();
                    ////bind dynamic property.

                    ////return response from penalty service.
                    foreach (var penaltyDetail in penaltyCollection)
                    {
                        dynamic penalty = new ExpandoObject();

                        ////get product list 

                        penalty.PenaltyFrom = penaltyDetail.From;
                        penalty.PenaltyTo = penaltyDetail.To;
                        penalty.PenaltyPercent = penaltyDetail.PenaltyPercent;

                        ////set all values in list.
                        penaltyDetails.Add(penalty);
                    }

                    ////return all service 
                    string st = resmanager.GetString("Api_ProductDetails", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = new ExpandoObject(), DataList = penaltyDetails, ErrorInfo = "" });
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
        /// This method for insert record into "CustomerProduct" from database entity.
        /// </summary>
        /// <param name="CustomerId">CustomerId</param>
        /// <param name="ProductId">ProductId</param>
        /// <param name="Investment">Investment</param>
        /// <param name="PaymentStatus">PaymentStatus</param>
        /// <returns>result</returns>
        [HttpPost]
        [Route("InvestInProduct")]
        public HttpResponseMessage InvestInProduct(CustomerProductDataModel customerProductData)
        {
            try
            {
                ListDictionary replacements = new ListDictionary();
                var product = productService.GetProductByProductId(new Guid(customerProductData.ProductId));
                if ((float)(Convert.ToDecimal(customerProductData.Investment)) >= product.MinPrice)
                {
                    if ((float)(Convert.ToDecimal(customerProductData.Investment)) <= product.MaxPrice)
                    {
                        if (product.RemainingValueOfInvestment >= float.Parse(customerProductData.Investment))
                        {
                            // inserting data into activity table
                            var response = customerProductService.InvestInProduct(customerProductData);
                            if (response)
                            {
                                //if (WeeklyResult != false)
                                //{
                                if (customerProductData.PaymentStatus == true)
                                {
                                    // updating remaining investment
                                    float remainingValue = product.RemainingValueOfInvestment - float.Parse(customerProductData.Investment);
                                    var RemainingInvestmentResult = productService.UpdateInvestmentAmount(new Guid(customerProductData.ProductId), remainingValue);
                                    
                                    // current week total earning
                                    double dailyPercent = (double)(Convert.ToDecimal(product.PercentWeeklyEarning) / 7);
                                    double dailyAmount = dailyPercent * Convert.ToDouble(customerProductData.Investment);
                                    
                                    // update week percent
                                    var WeeklyResult = customerProductService.UpdateWeeklyEarning(new Guid(customerProductData.CustomerId), new Guid(customerProductData.ProductId), (float)dailyAmount);

                                    // send email notification to customer for lost amount 
                                    var customer = customerService.GetCustomerById(new Guid(customerProductData.CustomerId));
                                    if (customer.CustomerReferalId != null)
                                    {
                                        var referalCustomerDetail = customerService.GetCustomerById((customer.CustomerReferalId.Value));
                                        if (referalCustomerDetail.IsActive == true)
                                        {
                                            // referal customer total investment
                                            var referalCustomerProduct = customerProductService.GetAllCustomerProductByCustomerId(referalCustomerDetail.CustomerReferalId.Value);
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
                                            double TotalSaleEarning = (Convert.ToDouble(customerProductData.Investment) * Convert.ToDouble(product.PercentSaleEarning)) / 100;
                                            if (referalCustomerDetail.IsActive == true && totalReferalInvestedAmount.Sum(x => x.TotalSaleEarning) <= maxPercentAmount)
                                            {
                                                // update sale percent
                                                var result = customerProductService.UpdateSaleEarning(new Guid(customerProductData.CustomerId), new Guid(customerProductData.ProductId), (float)TotalSaleEarning);
                                                return this.Request.CreateResponse(HttpStatusCode.OK, new { result = "Successfully invested" });
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

                                    }
                                }
                                string st = resmanager.GetString("Api_InvestInProduct", CultureInfo.GetCultureInfo(customerProductData.lang));
                                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

                            }
                            else
                            {
                                string stDataNot = resmanager.GetString("Api_DataNotFound", CultureInfo.GetCultureInfo(customerProductData.lang));
                                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataNot, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = stDataNot });
                            }
                        }
                        else
                        {
                            string stDataNot = resmanager.GetString("Api_InvestmentAmountNotGreaterThanRemaining", CultureInfo.GetCultureInfo(customerProductData.lang));
                            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataNot, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = stDataNot });
                        }
                    }
                    else
                    {
                        string stDataNot = resmanager.GetString("Api_MaxPrice", CultureInfo.GetCultureInfo(customerProductData.lang));
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataNot, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = stDataNot });
                    }
                }
                else
                {
                    string stDataNot = resmanager.GetString("Api_MinPrice", CultureInfo.GetCultureInfo(customerProductData.lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataNot, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = stDataNot });
                }

            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                ////return case of exception.
                string stFailed = resmanager.GetString("Api_Failed", CultureInfo.GetCultureInfo(customerProductData.lang));
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stFailed, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// This method for get all record from "product and customerproduct" entity.
        /// </summary>
        /// <returns>product and customerproduct entity object value.</returns>
        [HttpGet]
        [Route("GetMyInvestments/{customerId}/{lang}/{filterKey}/{filterValue}")]
        public HttpResponseMessage GetMyInvestments(string customerId, string lang, string filterKey, string filterValue)
        {
            try
            {
                DateTime date = DateTime.UtcNow.Date;

                ////get customer product list 
                var customerProductCollection = customerProductService.GetAllCustomerProductByCustomerId(new Guid(customerId)).OrderByDescending(x => x.CreatedDate).ToList();
                if (filterKey == "date")
                {
                    customerProductCollection = customerProductCollection.Where(x => x.CreatedDate.Date.ToString("MM-dd-yyyy") == filterValue).ToList();
                }
                else if (filterKey == "status")
                {
                    customerProductCollection = customerProductCollection.Where(x => x.Status.ToString() == filterValue).ToList();
                }

                ////check object
                if (customerProductCollection.Count > 0 && customerProductCollection != null)
                {
                    ////dynamic list.
                    dynamic customerProductDetails = new List<ExpandoObject>();
                    ////bind dynamic property.  

                    ////return response from customer product service.
                    foreach (var customerProductDetail in customerProductCollection)
                    {
                        // current week total earning
                        double dailyPercent = (double)(Convert.ToDecimal(customerProductDetail.Product.PercentWeeklyEarning) / 7);
                        double dailyAmount = (dailyPercent * customerProductDetail.Investment) / 100;

                        dynamic customerProduct = new ExpandoObject();
                        customerProduct.Id = customerProductDetail.Id;
                        customerProduct.InvestmentWithdrawDate = customerProductDetail.Product.InvestmentWithdrawDate.Date;
                        customerProduct.Name = customerProductDetail.Product.Name;
                        customerProduct.Status = customerProductDetail.Status;
                        customerProduct.ImageUrl = customerProductDetail.Product.Document == null ? projectPathUrl + projectName + "//Pictures/48cb4537-3c0b-4de4-bb7b-31e1df9e71d5_ProductImage_.png" : customerProductDetail.Product.Document.URL;
                        customerProduct.Investment = customerProductDetail.Investment;
                        customerProduct.InvestmentCustomer = customerProductDetail.Investment;
                        customerProduct.CreatedDate = customerProductDetail.CreatedDate.Date.ToString("MM-dd-yyyy");
                        customerProduct.ProductId = customerProductDetail.ProductId;
                        customerProduct.PaymentType = customerProductDetail.PaymentType;

                        // calculating previous month withdraws of weekly earning
                        var customerWithdraws = withdrawService.GetTotalWithdrawByCustomerProductId(customerProductDetail.Id);
                        customerProduct.CurrentWeekTotalEarning = customerWithdraws.Where(x => x.Status == "Fund Released" && x.IsEarning == true).Sum(s => s.WithdrawAmount);

                        ////set all values in list.
                        customerProductDetails.Add(customerProduct);

                        if (customerProductDetail.StopCalculationDate != null)
                        {
                            int daysDiffrence = Convert.ToInt32((DateTime.UtcNow.Date - customerProductDetail.StopCalculationDate.Value.Date).TotalDays);
                            date = DateTime.UtcNow.AddDays(-daysDiffrence).Date;
                        }

                        if (customerProductDetail.Customer.IsActive == true && customerProductDetail.StartCalculationDate != null)
                        {
                            int answer = 0;
                            if (customerProductDetail.StopCalculationDate != null)
                            {
                                // to get the total days in between
                                answer = Convert.ToInt32((date - customerProductDetail.StartCalculationDate.Value.Date).TotalDays) - 1;
                            }
                            else if (DateTime.UtcNow.Date == customerProductDetail.StartCalculationDate.Value.Date)
                            {
                                // to get the total days in between
                                answer = Convert.ToInt32((DateTime.UtcNow.Date - customerProductDetail.StartCalculationDate.Value.Date).TotalDays) + 1;
                            }
                            else
                            {
                                if (DateTime.UtcNow.Date > customerProductDetail.StartCalculationDate.Value.Date)
                                {
                                    answer = Convert.ToInt32((DateTime.UtcNow.Date - customerProductDetail.StartCalculationDate.Value.Date.AddDays(-1)).TotalDays);
                                }
                            }
                            if (customerProductDetail.StopCalculationDate == null)
                            {
                                double TotalWeeklyEarning;
                                //if(customerProductDetail.LastWeeklyWithdrawEnableDate.Value.AddMonths(-2)
                                if (customerProductDetail.LastWithdrawMonthDate != null)
                                {

                                    var totalDays = (customerProductDetail.LastWithdrawMonthDate.Value.Date - customerProductDetail.StartCalculationDate.Value.Date).TotalDays;
                                    var amountToDeduct = totalDays * dailyAmount;
                                    var totalAmount = answer * dailyAmount;
                                    TotalWeeklyEarning = totalAmount - amountToDeduct;

                                }
                                else
                                {
                                    TotalWeeklyEarning = answer * dailyAmount;
                                }

                                // update weekly percent
                                var result = customerProductService.UpdateWeeklyEarning(customerProductDetail.CustomerId, customerProductDetail.ProductId, (float)TotalWeeklyEarning);
                            }
                        }
                        if (DateTime.UtcNow.Date > customerProductDetail.Product.InvestmentWithdrawDate.Date)
                        {
                            customerProductService.StopCalculation((new Guid(customerId)), customerProductDetail.ProductId, true, true);
                        }

                    }
                    ////return all service 
                    string st = resmanager.GetString("Api_MyInvestmentList", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = new ExpandoObject(), DataList = customerProductDetails, ErrorInfo = "" });
                    //return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK, (List<ExpandoObject>)productDetails);
                }
                else
                {
                    string stDataNot = resmanager.GetString("Api_DataNotFound", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataNot, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = stDataNot });
                    // return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Data Not Found." });
                }
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);
                ////return case of exception.
                string stFailed = resmanager.GetString("Api_Failed", CultureInfo.GetCultureInfo(lang));
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stFailed, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
                //  return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
            }
        }

        /// <summary>
        /// This method for get all customer sale record from "customerProduct" entity.
        /// </summary>
        /// <returns>CustomerProduct entity object value.</returns>
        [HttpGet]
        [Route("GetMyInvestmentDetailById/{customerId}/{productId}/{lang}")]
        public HttpResponseMessage GetMyInvestmentDetailById(string customerId, string productId, string lang)
        {
            try
            {
                var lastWithdrawDate = string.Empty;
                var weeklyFromWithdrawDay = string.Empty;
                DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                dtinfo.ShortDatePattern = "MM/dd/yyyy";
                dtinfo.DateSeparator = "/";

                ////get product, customer and customerProduct list 
                var productDetails = customerProductService.GetAllCustomerProductByProductIdCustomerId(new Guid(customerId), new Guid(productId));
                double dailyPercent = (double)(Convert.ToDecimal(productDetails.Product.PercentWeeklyEarning) / 7);
                double dailyAmount = (dailyPercent * productDetails.Investment) / 100;

                ////check object
                if (productDetails != null)
                {
                    ////bind dynamic property.
                    dynamic product = new ExpandoObject();

                    product.Id = productDetails.Id;
                    product.Name = productDetails.Product.Name;
                    product.Investment = productDetails.Investment;
                    product.InvestmentFloat = productDetails.Investment;
                    product.InvestmentWithdrawDate = productDetails.Product.InvestmentWithdrawDate;
                    // calculating previous month income
                    bool isJan = false;
                    if (DateTime.UtcNow.Month == 1) isJan = true;
                    if (productDetails.StartCalculationDate != null)
                    {
                        List<WeeklyPercentDataModel> percentResult = WeeklyPercentCalculation(productDetails.StartCalculationDate.Value, (float)(dailyAmount), productDetails.StopCalculation);
                        if (productDetails.StartCalculationDate.Value.Month == DateTime.UtcNow.Month && productDetails.StartCalculationDate.Value.Year == DateTime.UtcNow.Year)
                        {
                            product.PreviousMonthEarning = 0;
                            product.PreviousMonthEarningFloat = 0;
                        }
                        else if (isJan)
                        {
                            if (productDetails.StartCalculationDate.Value.Month == 12 && productDetails.StartCalculationDate.Value.Year == DateTime.UtcNow.Year - 1)
                            {
                                // this is previous month.
                                DateTime sdate = Convert.ToDateTime("12/1/" + (DateTime.UtcNow.Year - 1).ToString(), dtinfo);
                                DateTime edate = Convert.ToDateTime(DateTime.UtcNow.Month + "/1/" + DateTime.UtcNow.Year, dtinfo);
                                var daydiff = edate.Subtract(sdate).TotalDays;

                                var createdaydiff = productDetails.StartCalculationDate.Value.Date.Subtract(sdate).TotalDays + 1;
                                int totappaidday = Convert.ToInt32(daydiff - createdaydiff);
                                product.PreviousMonthEarning = totappaidday * dailyAmount;
                                product.PreviousMonthEarningFloat = totappaidday * dailyAmount;
                            }
                        }
                        else if (productDetails.StartCalculationDate.Value.Month == DateTime.UtcNow.Month - 1 && productDetails.StartCalculationDate.Value.Year == DateTime.UtcNow.Year)
                        {
                            // this is previous month.
                            DateTime sdate = Convert.ToDateTime(DateTime.UtcNow.Month - 1 + "/1/" + DateTime.UtcNow.Year, dtinfo);
                            DateTime edate = Convert.ToDateTime(DateTime.UtcNow.Month + "/1/" + DateTime.UtcNow.Year, dtinfo);
                            var daydiff = edate.Subtract(sdate).TotalDays;

                            var createdaydiff = productDetails.StartCalculationDate.Value.Date.Subtract(sdate).TotalDays + 1;
                            int totappaidday = Convert.ToInt32(daydiff - createdaydiff);
                            product.PreviousMonthEarning = totappaidday * dailyAmount;
                            product.PreviousMonthEarningFloat = totappaidday * dailyAmount;
                        }

                        if (productDetails.StartCalculationDate.Value.Month == DateTime.UtcNow.Month - 1)
                            product.PreviousWeekEarning = percentResult[0].Earning;
                        //else
                        //{
                        //    product.PreviousMonthEarning = 0;
                        //}
                        // removing previous amount value
                        percentResult.RemoveAt(0);
                        product.CurrentWeekTotalEarning = percentResult.Sum(s => s.Earning);
                        product.CurrentWeekEarning = percentResult;
                    }
                    else
                    {
                        product.PreviousMonthEarning = 0;
                        product.PreviousMonthEarningFloat = 0;
                        product.CurrentWeekTotalEarning = "0";
                        product.CurrentWeekEarning = "0";
                        //product.CurrentWeekEarning = null;
                    }
                    // calculating days if weeklyFromDay is in weeks
                    if (productDetails.Product.LastWeeklyWithdrawDate == null)
                    {
                        if (productDetails.Product.WeeklyFromWithdrawDay.Length == 1)
                        {
                            int days = Convert.ToInt32(productDetails.Product.WeeklyFromWithdrawDay) * 7;
                            lastWithdrawDate = productDetails.Product.CreatedDate.AddDays(days).ToString("yyyy-MM-dd 00:00:00");
                        }
                        else
                        {
                            dtinfo.ShortDatePattern = "dd/MM/yyyy";
                            DateTime date = Convert.ToDateTime(productDetails.Product.WeeklyFromWithdrawDay, dtinfo);
                            weeklyFromWithdrawDay = date.ToString("yyyy-MM-dd 00:00:00");
                        }
                    }
                    // Code for withdraw amount from is Earning
                    int currentDateWithAddWithdrawDay = Convert.ToInt32(productDetails.Product.WeeklyToWithdrawDay);
                    dtinfo.ShortDatePattern = "dd/MM/yyyy";
                    DateTime previousDate;
                    var result = IsValidDateTimeTest(productDetails.Product.WeeklyFromWithdrawDay);
                    if (result)
                    {
                        previousDate = Convert.ToDateTime(productDetails.Product.WeeklyFromWithdrawDay, dtinfo).AddDays(currentDateWithAddWithdrawDay);
                    }
                    else
                    {
                        int withdrawDay = Convert.ToInt32(productDetails.Product.WeeklyFromWithdrawDay) * 7;

                        previousDate = Convert.ToDateTime(productDetails.LastWeeklyWithdrawDate, dtinfo).AddDays(withdrawDay);
                        DateTime checkpreviousDate = previousDate;
                        previousDate = previousDate.AddDays(currentDateWithAddWithdrawDay);

                        // comment on 18-may for weekly withdraw enable button
                        //if (productDetails.LastWeeklyWithdrawDate <= System.DateTime.UtcNow && productDetails.LastWeeklyWithdrawDate.Value.AddDays(currentDateWithAddWithdrawDay) >= System.DateTime.UtcNow)
                        if (productDetails.LastWeeklyWithdrawEnableDate <= System.DateTime.UtcNow && productDetails.LastWeeklyWithdrawEnableDate.Value.AddDays(currentDateWithAddWithdrawDay) >= System.DateTime.UtcNow)
                        {
                            product.PreviousMonthEarning = dailyAmount * withdrawDay;
                            product.PreviousMonthEarningFloat = dailyAmount * withdrawDay;
                        }
                        else
                        {
                            product.PreviousMonthEarning = 0;
                            product.PreviousMonthEarningFloat = 0;
                        }

                    }
                    if (previousDate.Date.Day < DateTime.UtcNow.Date.Day && previousDate.Month <= DateTime.UtcNow.Month)
                    {
                        if (product.PreviousMonthEarning != 0)
                        {
                            var withdrawcollection = withdrawService.GetTotalWithdrawByCustomerProductId(productDetails.Id).ToList();
                            var newWithdrawCollection = withdrawcollection.Where(x => x.IsEarning = true).OrderByDescending(y => y.CreatedDate).FirstOrDefault();

                            if (newWithdrawCollection == null)
                            {
                                var responseCustomerProduct = customerProductService.UpdateWeeklyEarning(new Guid(customerId), new Guid(productId), (float)product.PreviousMonthEarning);
                                if (responseCustomerProduct)
                                {
                                    var wallet = walletService.GetWalletByCustomerId(new Guid(customerId));
                                    if (wallet != null)
                                    {
                                        float newWalletAmount = (float)wallet.AvailableBalance + (float)product.PreviousMonthEarning;
                                        //var response = walletService.UpdateWalletAmount(new Guid(customerId), newWalletAmount);

                                        //if (response)
                                        //{
                                        WithdrawDataModel withdrawDataModel = new WithdrawDataModel();
                                        withdrawDataModel.IsEarning = true;
                                        withdrawDataModel.CustomerId = customerId;
                                        withdrawDataModel.CustomerProductId = productDetails.Id.ToString();
                                        withdrawDataModel.WithdrawAmount = (float)product.PreviousMonthEarning;
                                        var responseData = withdrawService.AddWithdraw(withdrawDataModel);
                                        product.PreviousMonthEarning = 0;
                                        product.PreviousMonthEarningFloat = 0;
                                        //}
                                    }
                                }
                            }
                            else
                            {
                                var mon = newWithdrawCollection.CreatedDate.Month;
                                var currentMonth = DateTime.UtcNow.Month;
                                if (newWithdrawCollection.CreatedDate.Month == DateTime.UtcNow.Month && newWithdrawCollection.CreatedDate.Year == DateTime.UtcNow.Year)
                                {
                                    product.PreviousMonthEarning = 0;
                                    product.PreviousMonthEarningFloat = 0;
                                }
                                else
                                {
                                    // product.PreviousMonthEarning = dailyAmount * 7;
                                    float weeklyEarningUpdate = productDetails.WeeklyEarning - (float)product.PreviousMonthEarning;
                                    var responseCustomerProduct = customerProductService.UpdateWeeklyEarning(new Guid(customerId), new Guid(productId), weeklyEarningUpdate);
                                    if (responseCustomerProduct)
                                    {
                                        var wallet = walletService.GetWalletByCustomerId(new Guid(customerId));
                                        if (wallet != null)
                                        {
                                            float newWalletAmount = (float)wallet.AvailableBalance + (float)product.PreviousMonthEarning;
                                            var response = walletService.UpdateWalletAmount(new Guid(customerId), newWalletAmount);

                                            if (response)
                                            {
                                                WithdrawDataModel withdrawDataModel = new WithdrawDataModel();
                                                withdrawDataModel.IsEarning = true;
                                                withdrawDataModel.CustomerId = customerId;
                                                withdrawDataModel.CustomerProductId = productDetails.Id.ToString();
                                                withdrawDataModel.WithdrawAmount = (float)product.PreviousMonthEarning;
                                                var responseData = withdrawService.AddWithdraw(withdrawDataModel);
                                                product.PreviousMonthEarning = 0;
                                                product.PreviousMonthEarningFloat = 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (productDetails.LastWeeklyWithdrawEnableDate.Value.Date > System.DateTime.UtcNow.Date)
                    {
                        if (result)
                        {
                            product.PreviousMonthEarning = 0;
                            product.PreviousMonthEarningFloat = 0;
                        }
                    }


                    product.DatetoWithdrawEarning = productDetails.Product.WeeklyFromWithdrawDay.Length != 1 ? weeklyFromWithdrawDay : lastWithdrawDate;
                    product.IsActive = productDetails.Product.IsActive;
                    product.ImageUrl = productDetails.Product.Document == null ? projectPathUrl + projectName + "//Pictures/48cb4537-3c0b-4de4-bb7b-31e1df9e71d5_ProductImage_.png" : projectPathUrl + projectName + pictures + productDetails.Product.Document.Name;
                    product.CustomerId = customerId;
                    product.ProductId = productId;
                    product.CreatedDate = productDetails.CreatedDate;
                    product.StopCalculation = productDetails.StopCalculation;
                    product.Status = productDetails.Status;
                    var customerWithdraws = withdrawService.GetTotalWithdrawByCustomerProductId(productDetails.Id).Where(x => x.IsEarning == true).ToList();
                    product.TotalWeeklyEarning = customerWithdraws.Sum(s => s.WithdrawAmount);
                    product.IsMiniMarket = productDetails.PaymentType == "MiniMarket" ? true : false;


                    ////return all service 
                    string st = resmanager.GetString("Api_ProductDetails", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = product, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    // return this.Request.CreateResponse<ExpandoObject>(HttpStatusCode.OK, (ExpandoObject)product);
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
                logger.Error("Method: myinvestment()", ex);
                logger.Info("Method: myinvestment()" + ex.StackTrace);
                ////return case of exception.
                string stFailed = resmanager.GetString("Api_Failed", CultureInfo.GetCultureInfo(lang));
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stFailed, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
                //return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
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

        /// <summary>
        /// Method for Weekly Percent Calculation
        /// </summary>
        /// <param name="investDate"></param>
        /// <param name="dailyAmount"></param>
        /// <param name="stopCalcualtion"></param>
        /// <returns></returns>
        private List<WeeklyPercentDataModel> WeeklyPercentCalculation(DateTime investDate, float dailyAmount, bool stopCalcualtion)
        {
            bool thisWeek = false;
            List<WeeklyPercentDataModel> weekList = new List<WeeklyPercentDataModel>();
            EWeekDays enmDOW = (EWeekDays)Enum.Parse(typeof(EWeekDays), DateTime.UtcNow.ToString("dddd"));
            int dayNum = (int)enmDOW;
            if (DateTime.UtcNow.AddDays(-dayNum).Date <= investDate.Date)
            {
                WeeklyPercentDataModel mdl = new WeeklyPercentDataModel();
                mdl.date = DateTime.UtcNow; mdl.day = "Previous"; mdl.Earning = 0f;
                weekList.Add(mdl);
                thisWeek = true;
            }
            else if (DateTime.UtcNow.AddDays(-dayNum).AddDays(-7).Date <= investDate.Date)
            {
                bool matched2 = false; float total = 0f;
                for (int i = 1; i <= 7; i++)
                {
                    if (DateTime.UtcNow.AddDays(-dayNum).AddDays(-7).AddDays(i).Date == investDate.Date)
                    {
                        matched2 = true;
                        total = total + dailyAmount;
                    }
                    else
                    {
                        if (matched2)
                            total = total + dailyAmount;
                    }
                }
                WeeklyPercentDataModel mdl = new WeeklyPercentDataModel();
                mdl.date = DateTime.UtcNow; mdl.day = "Previous"; mdl.Earning = total;
                weekList.Add(mdl);
            }
            else
            {
                WeeklyPercentDataModel mdl = new WeeklyPercentDataModel();
                mdl.date = DateTime.UtcNow; mdl.day = "Previous"; mdl.Earning = dailyAmount * 7;
                weekList.Add(mdl);
            }
            if (thisWeek)
            {
                bool matched = false;
                for (int i = 0; i < dayNum; i++)
                {
                    if (DateTime.UtcNow.AddDays(-dayNum).AddDays(i + 1).Date == investDate.Date)
                    {
                        matched = true;
                        var day = (EWeekDays)(i + 1);
                        WeeklyPercentDataModel mdl = new WeeklyPercentDataModel();
                        mdl.date = DateTime.UtcNow.AddDays(-dayNum).AddDays(i + 1).Date; mdl.day = day.ToString(); mdl.Earning = dailyAmount;
                        weekList.Add(mdl);
                        // weekList.Add(day.ToString(), dailyPercent);
                    }
                    else
                    {
                        if (matched)
                        {
                            var day = (EWeekDays)(i + 1);
                            WeeklyPercentDataModel mdl = new WeeklyPercentDataModel();
                            mdl.date = DateTime.UtcNow.AddDays(-dayNum).AddDays(i + 1).Date; mdl.day = day.ToString(); mdl.Earning = dailyAmount;
                            weekList.Add(mdl);

                        }
                        else
                        {
                            var day = (EWeekDays)(i + 1);
                            WeeklyPercentDataModel mdl = new WeeklyPercentDataModel();
                            mdl.date = DateTime.UtcNow.AddDays(-dayNum).AddDays(i + 1).Date; mdl.day = day.ToString(); mdl.Earning = 0f;
                            weekList.Add(mdl);
                            //weekList.Add(day.ToString(), 0f);
                        }
                    }

                }
            }
            else
            {
                for (int i = 0; i <= dayNum - 1; i++)
                {
                    var day = (EWeekDays)(i + 1);
                    WeeklyPercentDataModel mdl = new WeeklyPercentDataModel();
                    mdl.date = DateTime.UtcNow.AddDays(-dayNum).AddDays(i).Date; mdl.day = day.ToString(); mdl.Earning = dailyAmount;
                    weekList.Add(mdl);

                }
            }
            for (int i = dayNum; i <= 6; i++)
            {
                var day = (EWeekDays)(i + 1);
                WeeklyPercentDataModel mdl = new WeeklyPercentDataModel();
                mdl.date = DateTime.UtcNow.AddDays(-dayNum).AddDays(i + 1).Date; mdl.day = day.ToString(); mdl.Earning = 0f;
                weekList.Add(mdl);

            }
            return weekList;
        }

        #endregion
    }
}
