using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TradeSystem.Service;
using TradeSystem.Utilities.Email;
using TradeSystem.Utils;
using TradeSystem.Utils.Models;
using TradeSystem.WebApi.Controllers;

namespace TradeSystem.MVCWeb.Controllers
{

    //[Authorize]
    [RoutePrefix("api/promotion")]
    public class PromotionApiController : BaseApiController
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 

        IPromotionService promotionService;
        ICustomerService customerService;
        IEmailService emailService;

        // Constructor of Promotion Api Controller
        public PromotionApiController(IPromotionService _promotionService, ICustomerService _customerService, IEmailService _emailService)
        {
            promotionService = _promotionService;
            customerService = _customerService;
            emailService = _emailService;
        }
        #endregion

        /// <summary>
        /// This method use for get 'Customer' select list type controls.
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns>key value pair</returns>
        [HttpGet]
        [Route("GetCustomerSelectList")]
        public HttpResponseMessage GetCustomerSelectList()
        {
            try
            {
                ////get task type list
                var promotions = promotionService.GetAllCustomerByActive();
                //var promotions = customerService.GetAllCustomer();//GetAllCustomerByActive


                ////dynamic list.
                dynamic promotionsList = new List<ExpandoObject>();
                ////bind dynamic property.
                if (promotions.Count > 0 && promotions != null)
                {

                    ////return response from Promotion service.
                    foreach (var promotionData in promotions)
                    {
                        dynamic promotionItem = new ExpandoObject();

                        ////get promotion list 
                        promotionItem.Id = promotionData.Id;
                        promotionItem.UserName = promotionData.FirstName + " " + promotionData.LastName;
                        promotionItem.Email = promotionData.Email;

                        ////set all values in list.
                        promotionsList.Add(promotionItem);
                    }

                    ////return task type service for get task type.
                    return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK, (List<ExpandoObject>)promotionsList);
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
        /// post api method for add promotion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AddPromotion")]
        public async Task<HttpResponseMessage> AddPromotion(PromotionDataModel promotionDataModel)
        {
            try
            {
                ListDictionary replacements = new ListDictionary();

                if (promotionDataModel.PromotionType == "Alert on Web & App")
                {

                    ////inserting data into promotion table.
                    promotionDataModel.Description = promotionDataModel.Alert;
                    promotionDataModel.DescriptionSpanish = promotionDataModel.Alert;
                    promotionDataModel.SubjectSpanish = promotionDataModel.Subject;
                    promotionDataModel.To = promotionDataModel.To.Replace("on;", " ");
                    ////inserting data into promotion table.
                    //var response = promotionService.AddPromotion(promotionDataModel);
                    string[] values = promotionDataModel.To.Split(';');
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = values[i].Trim();
                        promotionDataModel.To = values[i];
                        var response = promotionService.AddPromotion(promotionDataModel);
                    }
                }
                else
                {
                    promotionDataModel.To = promotionDataModel.To.Replace("on;", " ");
                    ////inserting data into promotion table.
                    var response = promotionService.AddPromotion(promotionDataModel);
                    string[] values = promotionDataModel.To.Split(';');

                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = values[i].Trim();
                        var customer = customerService.GetCustomerById(new Guid(values[i]));
                        if (customer != null)
                        {
                            //string coupon = SecurityHelper.Decrypt(customer.Password, true);
                            replacements = new ListDictionary { { "<%FirstName%>", customer.FirstName } };
                            replacements.Add("<%LastName%>", customer.LastName);
                            replacements.Add("<%Subject%>", promotionDataModel.Subject);
                            replacements.Add("<%email%>", customer.Email);
                            replacements.Add("<%Description%>", promotionDataModel.Description);


                            ////add email data
                            var sendEmail = emailService.AddEmailData(customer.Id.ToString(), customer.Email, string.Empty, string.Empty, string.Empty, EmailTemplatesHelper.PromotionCreateEmail, replacements);


                            ////send email.
                            if (sendEmail != null)
                            {
                                ////Code for Sending Email
                                emailService.SendEmailAsync(customer.Id, customer.Email, string.Empty, string.Empty, customer.Email, EmailTemplatesHelper.PromotionCreateEmail, string.Empty, replacements);
                            }

                        }
                    }

                }
                return this.Request.CreateResponse(HttpStatusCode.OK, new { message = "Email Sended Successfully." });

            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);
                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, new BaseResponseDataModel { Message = "Exception", Succeeded = false });
            }
        }

        /// <summary>
        /// This method for get all customer sale record from "customerProduct" entity.
        /// </summary>
        /// <returns>CustomerProduct entity object value.</returns>
        [HttpGet]
        [Route("GetAllPromotionByCustomerId/{id}/{lang}")]
        public HttpResponseMessage GetAllPromotionByCustomerId(string id, string lang)
        {
            try
            {
                if (id == "0" )
                {
                    ////get product, customer and customerProduct list 
                    var promotionDetails = promotionService.GetAllPromotionByType().OrderByDescending(x => x.CreatedDate).ToList();
                   
                    ////bind dynamic property.
                    if (promotionDetails.Count > 0 && promotionDetails != null)
                    {
                        ////dynamic list.
                        dynamic promotionsList = new List<ExpandoObject>();
                        ////return response from customer product service.
                        foreach (var promotionDetail in promotionDetails)
                        {
                            ////bind dynamic property.
                            dynamic promotions = new ExpandoObject();

                            promotions.Id = promotionDetail.Id;
                            promotions.Description = promotionDetail.Description;
                            promotions.DescriptionSpanish = promotionDetail.DescriptionSpanish;
                            promotions.Url = promotionDetail.Url;
                            promotions.Viewed = promotionDetail.Viewed;
                            promotions.CreatedDate = promotionDetail.CreatedDate.ToString("MM-dd-yyyy");
                            promotionsList.Add(promotions);
                        }
                        ////return all service 
                        string st = resmanager.GetString("Api_GetData", CultureInfo.GetCultureInfo(lang));
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = new ExpandoObject(), DataList = promotionsList, ErrorInfo = "" });
                        //return this.Request.CreateResponse<ExpandoObject>(HttpStatusCode.OK, (ExpandoObject)promotionsList);
                    }
                    else
                    {
                        string stDataNot = resmanager.GetString("Api_DataNotFound", CultureInfo.GetCultureInfo(lang));
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataNot, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = stDataNot });
                        // return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Data Not Found." });
                    }
                }
                else
                {
                    ////get product, customer and customerProduct list 
                    var promotionDetails = promotionService.GetAllPromotionByCustomerId(id).OrderByDescending(x => x.CreatedDate).ToList();
                    promotionDetails = promotionDetails.Where(x => x.PromotionType == "Alert on Web & App").ToList();
                    ////bind dynamic property.
                    if (promotionDetails.Count > 0 && promotionDetails != null)
                    {
                        ////dynamic list.
                        dynamic promotionsList = new List<ExpandoObject>();
                        ////return response from customer product service.
                        foreach (var promotionDetail in promotionDetails)
                        {
                            ////bind dynamic property.
                            dynamic promotions = new ExpandoObject();

                            promotions.Id = promotionDetail.Id;
                            promotions.Description = promotionDetail.Description;
                            promotions.DescriptionSpanish = promotionDetail.DescriptionSpanish;
                            promotions.Url = promotionDetail.Url;
                            promotions.Viewed = promotionDetail.Viewed;
                            promotions.CreatedDate = promotionDetail.CreatedDate.ToString("MM-dd-yyyy");
                            promotionsList.Add(promotions);
                        }
                        ////return all service 
                        string st = resmanager.GetString("Api_GetData", CultureInfo.GetCultureInfo(lang));
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = new ExpandoObject(), DataList = promotionsList, ErrorInfo = "" });
                        //return this.Request.CreateResponse<ExpandoObject>(HttpStatusCode.OK, (ExpandoObject)promotionsList);
                    }
                    else
                    {
                        string stDataNot = resmanager.GetString("Api_DataNotFound", CultureInfo.GetCultureInfo(lang));
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stDataNot, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = stDataNot });
                        // return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Data Not Found." });
                    }
                }
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                string stFailed = resmanager.GetString("Api_Failed", CultureInfo.GetCultureInfo(lang));
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stFailed, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
                // return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
            }
        }

        /// <summary>
        /// This method for get all customer sale record from "customerProduct" entity.
        /// </summary>CustomerId
        /// </summary>Language
        /// <returns>CustomerProduct entity object value.</returns>
        [HttpGet]
        [Route("GetAllPromotionByCustomerIdAndUnview/{id}/{lang}")]
        public HttpResponseMessage GetAllPromotionByCustomerIdAndUnview(string id, string lang)
        {
            try
            {
                ////get product, customer and customerProduct list 
                var promotionDetails = promotionService.GetAllPromotionByCustomerIdAndUnView(id);
                promotionDetails = promotionDetails.Where(x => x.PromotionType == "Alert on Web & App").ToList();

                ////bind dynamic property.
                if (promotionDetails.Count > 0 && promotionDetails != null)
                {
                    ////dynamic list.
                    dynamic promotionsList = new List<ExpandoObject>();
                    ////return response from customer product service.
                    foreach (var promotionDetail in promotionDetails)
                    {
                        ////bind dynamic property.
                        dynamic promotions = new ExpandoObject();

                        promotions.Id = promotionDetail.Id;
                        promotions.Description = promotionDetail.Description;
                        promotions.Viewed = promotionDetail.Viewed;
                        promotions.CreatedDate = promotionDetail.CreatedDate;
                        promotionsList.Add(promotions);
                    }
                    ////return all service 
                    string st = resmanager.GetString("Api_GetDataUnviewed", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = new ExpandoObject(), DataList = promotionsList, ErrorInfo = "" });
                    //return this.Request.CreateResponse<ExpandoObject>(HttpStatusCode.OK, (ExpandoObject)promotionsList);
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
                // return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
            }
        }

        /// <summary>
        /// Edit Promotion view option by promotion id
        /// </summary>
        /// <param name="promotionData"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("EditPromotionViewed/{promotionId}/{lang}/{viewed}")]
        public HttpResponseMessage EditPromotionViewed(string promotionId, string lang, bool viewed)
        {
            try
            {
                ////get service response.
                var response = promotionService.UpdatePromotionViewed(new Guid(promotionId), viewed);                ////return result from service response.
                                                                                                                     ////return all service 
                string st = resmanager.GetString("Api_UpdatedSuccessfully", CultureInfo.GetCultureInfo(lang));
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                //return this.Request.CreateResponse(HttpStatusCode.OK, new { result = response });
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                string stFailed = resmanager.GetString("Api_Failed", CultureInfo.GetCultureInfo(lang));
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stFailed, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
                //return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Exception" });

            }
        }

        /// <summary>
        /// Get Promotion option by promotion id
        /// </summary>
        /// <param name="promotionData"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPromotionByPromotionId/{promotionId}/{lang}")]
        public HttpResponseMessage GetPromotionByPromotionId(string promotionId, string lang)
        {
            try
            {
                ////get service response.
                var response = promotionService.GetPromotionByPromotionId(new Guid(promotionId));                ////return result from service response.
                dynamic promotionDataModal = new ExpandoObject();
                if (response != null)
                {

                    promotionDataModal.Id = response.Id;
                    promotionDataModal.Description = response.Description;
                }
                return this.Request.CreateResponse<ExpandoObject>(HttpStatusCode.OK, (ExpandoObject)promotionDataModal);
                //return this.Request.CreateResponse(HttpStatusCode.OK, new { result = response });
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.               
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Something went worng" });

            }
        }
    }
}
