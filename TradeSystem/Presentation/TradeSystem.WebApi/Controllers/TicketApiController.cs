using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TradeSystem.Service;
using TradeSystem.Utils;
using TradeSystem.Utils.Enum;
using TradeSystem.Utils.Models;
using TradeSystem.WebApi.Controllers;

namespace TradeSystem.MVCWeb.Controllers
{
    [Authorize]
    [RoutePrefix("api/ticket")]
    public class TicketApiController : BaseApiController
    {

        #region Dependencies Injection with initialization

        //Initialized interface object. 

        ITicketService ticketService;
        ITicketStatusService ticketStatusService;
        IActivityService activityService;

        //Initialized Ticket Api Controller Constructor.
        public TicketApiController(ITicketService _ticketService, ITicketStatusService _ticketStatusService, IActivityService _activityService)
        {
            ticketService = _ticketService;
            ticketStatusService = _ticketStatusService;
            activityService = _activityService;
        }
        #endregion

        /// <summary>
        /// This method for get all record from "ticket, customer and customerticket" entity.
        /// </summary>
        /// <returns>ticket, customer and customerticket entity object value.</returns>
        [HttpGet]
        [Route("GetAllTicket")]
        public HttpResponseMessage GetAllTicket()
        {
            try
            {
                ////get ticket list 
                var ticketCollection = ticketService.GetAllTicket().OrderByDescending(x => x.AutoIncrementedNo).ToList();

                ////check object
                if (ticketCollection.Count > 0 && ticketCollection != null)
                {
                    ////dynamic list.
                    dynamic ticketDetails = new List<ExpandoObject>();
                    ////bind dynamic property.

                    ////return response from ticket service.
                    foreach (var ticketDetail in ticketCollection)
                    {
                        dynamic ticket = new ExpandoObject();
                        ////get ticket list 
                        ticket.CreatedDate = ticketDetail.CreatedDate.ToString("MM-dd-yyyy HH:mm:ss tt");
                        ticket.Id = ticketDetail.Id;
                        ticket.AutoIncrementedNo = ticketDetail.AutoIncrementedNo;
                        //ticket.TicketStatusId = ticketDetail.TicketStatusId;
                        ticket.Title = ticketDetail.Title;
                        ticket.CustomerId = ticketDetail.Customer.Id;
                        ticket.CustomerName = ticketDetail.Customer.FirstName + " " + ticketDetail.Customer.LastName;
                        ticket.Status = ticketDetail.TicketStatus.Name;

                        ////set all values in list.
                        ticketDetails.Add(ticket);
                    }

                    ////return all service 
                    return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK, (List<ExpandoObject>)ticketDetails);
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
        /// This method for get record of ticket from "ticket" by customer id entity.
        /// </summary>
        /// <returns>ticket, customer and customerticket entity object value.</returns>
        [HttpGet]
        [Route("GetTicketByTicketId/{id}")]
        public HttpResponseMessage GetTicketByTicketId(string id)
        {
            try
            {
                ////get ticket list 
                var ticketDetail = ticketService.GetTicketDetailByTicketId(new Guid(id));

                ////check object
                if (ticketDetail != null)
                {
                    dynamic ticket = new ExpandoObject();

                    ticket.Id = ticketDetail.Id;
                    ticket.Title = ticketDetail.Title;
                    ticket.CustomerName = ticketDetail.Customer.FirstName + " " + ticketDetail.Customer.LastName;
                    ticket.CreatedDate = ticketDetail.CreatedDate.ToString("MM-dd-yyyy HH:mm:ss tt"); ;
                    ticket.Description = ticketDetail.Description;
                    ticket.Status = ticketDetail.TicketStatus.Name;
                    ticket.AutoIncrementedNo = ticketDetail.AutoIncrementedNo;
                    // checking
                    if (ticketDetail.TicketStatus.Name == "In-Process") { ticket.Process = true; }
                    else { ticket.Process = false; }
                    if (ticketDetail.TicketStatus.Name == "Completed") { ticket.Completed = true; }
                    else { ticket.Completed = false; }


                    ////return all service 
                    return this.Request.CreateResponse<ExpandoObject>(HttpStatusCode.OK, (ExpandoObject)ticket);
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Data Not Found." });
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
        /// This method for get record of ticket from "ticket" by customer id entity.
        /// </summary>
        /// <returns>ticket, customer and customerticket entity object value.</returns>
        [HttpPost]
        [Route("EditTicket")]
        public HttpResponseMessage EditTicket(TicketDataModel ticketData)
        {
            try
            {
                if (ticketData.Process)
                {
                    ticketData.Status = "In-Process";
                }
                if (ticketData.Completed)
                {
                    ticketData.Status = "Completed";
                }
                var ticketDetail = ticketStatusService.GetTicketStatusByStatus(ticketData.Status);
                if (ticketDetail != null)
                {
                    ticketData.TicketStatusId = ticketDetail.Id.ToString();
                    var ticket = ticketService.GetTicketDetailByTicketId(new Guid(ticketData.Id));
                    var response = ticketService.EditTicket(new Guid(ticketData.Id), new Guid(ticketData.TicketStatusId));

                    // inserting data into activity table
                    ActivityLogDataModel activityObj = new ActivityLogDataModel();
                    activityObj.Activity = ETaskStatus.TicketUpdate.ToString();
                    activityObj.Description = "Update ticket status of TID : " + ticket.AutoIncrementedNo;
                    activityObj.IsCompanyUser = true;
                    activityObj.CompanyUserId = ticketData.CompanyUserId;
                    var activityResult = activityService.AddActivity(activityObj);
                    ////return result from service response.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { result = response });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "No data" });
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
        /// This method use for get 'Ticket Status' list for select list type controls.
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns>key value pair</returns>
        [HttpGet]
        [Route("GetAllTicketStatus")]
        public HttpResponseMessage GetAllTicketStatus()
        {
            try
            {
                ////get task type list
                var ticketStatus = ticketStatusService.GetAllTicketStatusList();
                ////check object
                if (ticketStatus.Count > 0 && ticketStatus != null)
                {
                    ////return task type service for get task type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, ticketStatus);
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

        #region Ticket Mobile 

        /// <summary>
        /// This method for get record of ticket from "ticket" by customer id entity.
        /// </summary>
        /// <returns>ticket, customer and customerticket entity object value.</returns>
        [HttpGet]
        [Route("GetTicketListByCustomerId/{id}/{lang}/{filterKey}/{filterValue}")]
        public HttpResponseMessage GetTicketListByCustomerId(string id, string lang, string filterKey, string filterValue)
        {
            try
            {
                ////get ticket list 
                var ticketCollection = ticketService.GetTicketByCustomerId(new Guid(id)).OrderByDescending(x => x.AutoIncrementedNo).ToList();

                if (filterKey == "date")
                {
                    ticketCollection = ticketCollection.Where(x => x.CreatedDate.ToString("MM-dd-yyyy") == filterValue).ToList();
                }
                else if (filterKey == "status")
                {
                    ticketCollection = ticketCollection.Where(x => x.TicketStatus.Name == filterValue).ToList();
                }
                ////check object
                if (ticketCollection.Count > 0 && ticketCollection != null)
                {
                    ////dynamic list.
                    dynamic ticketDetails = new List<ExpandoObject>();
                    ////bind dynamic property.

                    ////return response from ticket service.
                    foreach (var ticketDetail in ticketCollection)
                    {
                        dynamic ticket = new ExpandoObject();
                        ////get ticket list 
                        ticket.CreatedDate = ticketDetail.CreatedDate.ToString("MM-dd-yyyy HH:mm:ss tt");
                        ticket.Id = ticketDetail.Id;
                        ticket.AutoIncrementedNo = ticketDetail.AutoIncrementedNo;
                        //ticket.TicketStatusId = ticketDetail.TicketStatusId;
                        ticket.Title = ticketDetail.Title;
                        ticket.CustomerId = ticketDetail.Customer.Id;
                        ticket.CustomerName = ticketDetail.Customer.FirstName + " " + ticketDetail.Customer.LastName;
                        ticket.Status = ticketDetail.TicketStatus.Name;
                        ticket.Description = ticketDetail.Description;
                        ////set all values in list.
                        ticketDetails.Add(ticket);
                    }

                    ////return all service 
                    // return this.Request.CreateResponse<ExpandoObject>(HttpStatusCode.OK, (ExpandoObject)ticket);
                    string st = resmanager.GetString("Api_TicketList", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = new ExpandoObject(), DataList = ticketDetails, ErrorInfo = "" });
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

                ////return case of exception.
                // return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
                string stFailed = resmanager.GetString("Api_Failed", CultureInfo.GetCultureInfo(lang));
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = stFailed, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// This method for post the raise ticket 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("RaiseTicket")]
        public HttpResponseMessage RaiseTicket(TicketDataModel ticketData)
        {
            try
            {
                ticketData.Status = "Pending";
                var ticketDetail = ticketStatusService.GetTicketStatusByStatus(ticketData.Status);
                if (ticketDetail != null)
                {
                    ticketData.TicketStatusId = ticketDetail.Id.ToString();
                    //var ticket = ticketService.GetTicketDetailByTicketId(new Guid(ticketData.Id));
                    var response = ticketService.AddTicket(ticketData);

                    //// inserting data into activity table
                    //ActivityLogDataModel activityObj = new ActivityLogDataModel();
                    //activityObj.Activity = ETaskStatus.TicketUpdate.ToString();
                    //activityObj.Description = "Update ticket status of TID : " + ticket.AutoIncrementedNo;
                    //activityObj.IsCompanyUser = true;
                    //activityObj.CompanyUserId = ticketData.CompanyUserId;
                    //var activityResult = activityService.AddActivity(activityObj);

                    ////return result from service response.
                    string st = resmanager.GetString("Api_TicketInsertedSuccessfully", CultureInfo.GetCultureInfo(ticketData.Lang));
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

                    //return this.Request.CreateResponse(HttpStatusCode.OK, new { result = response });
                }
                else
                {
                    string stFailed = resmanager.GetString("Api_SomethingWentWorng", CultureInfo.GetCultureInfo(ticketData.Lang));
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new BaseResponseDataModel { Message = stFailed, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "false" });
                    //return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "No data" });
                }

            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                string stFailed = resmanager.GetString("Api_SomethingWentWorng", CultureInfo.GetCultureInfo(ticketData.Lang));
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, new BaseResponseDataModel { Message = stFailed, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "false" });
                // return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Exception" });

            }
        }
        #endregion

    }
}
