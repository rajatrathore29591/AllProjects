using System.Web.Http;
using BorgCivil.Service;
using System.Net.Http;
using BorgCivil.Utils.Models;
using System.Net;
using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;
using BorgCivil.Utils.Enum;
using BorgCivil.Utils;
using BorgCivil.Framework.Entities;
using System.Web;
using System.Web.Http.Cors;
using System.Collections.Specialized;
using System.Web.UI.WebControls;


namespace BorgCivil.MVCWeb.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Booking")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BookingApiController : ApiController
    {
        //Initialized interface object. 
        #region Dependencies Injection with initialization
        IBookingService BookingService;
        IBookingFleetService BookingFleetService;
        IBookingSiteGateService BookingSiteGateService;
        IBookingSiteSupervisorService BookingSiteSupervisorService;
        IStatusLookupService StatusLookupService;
        ISupervisorService SupervisorService;
        IGatesService GatesService;
        IGateContactPersonService GateContactPersonService;
        IFleetHistoryService FleetHistoryService;
        IAttachmentsService AttachmentsService;
        IDocketService DocketService;
        IFleetsRegistrationService FleetsRegistrationService;
        ILoadDocketService LoadDocketService;
        IDocumentService DocumentService;
        IDocketCheckboxListService DocketCheckboxListService;

        // Constructor of Booking Api Controller 
        public BookingApiController(IBookingService _BookingService, IBookingFleetService _BookingFleetService, IBookingSiteGateService _BookingSiteGateService, IBookingSiteSupervisorService _BookingSiteSupervisorService, IStatusLookupService _StatusLookupService, ISupervisorService _SupervisorService, IGatesService _GatesService, IGateContactPersonService _GateContactPersonService, IFleetHistoryService _FleetHistoryService, IAttachmentsService _AttachmentsService, IDocketService _DocketService, IFleetsRegistrationService _FleetsRegistrationService, ILoadDocketService _LoadDocketService, IDocumentService _DocumentService, IDocketCheckboxListService _DocketCheckboxListService)
        {
            BookingService = _BookingService;
            BookingFleetService = _BookingFleetService;
            BookingSiteGateService = _BookingSiteGateService;
            BookingSiteSupervisorService = _BookingSiteSupervisorService;
            StatusLookupService = _StatusLookupService;
            SupervisorService = _SupervisorService;
            GatesService = _GatesService;
            GateContactPersonService = _GateContactPersonService;
            FleetHistoryService = _FleetHistoryService;
            AttachmentsService = _AttachmentsService;
            DocketService = _DocketService;
            FleetsRegistrationService = _FleetsRegistrationService;
            LoadDocketService = _LoadDocketService;
            DocumentService = _DocumentService;
            DocketCheckboxListService = _DocketCheckboxListService;
        }

        #endregion

        #region Booking Api's

        /// <summary>
        /// this method is for getting all booking by customerId
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllBookingByCustomerId/{CustomerId}")]
        public HttpResponseMessage GetAllBookingByCustomerId(Guid CustomerId)
        {
            try
            {
                //get booking list  by customerId
                var BookingCollection = BookingService.GetBookingByCustomerId(CustomerId);

                //check object
                if (BookingCollection.Count > 0 && BookingCollection != null)
                {
                    //dynamic list.
                    dynamic Bookings = new List<ExpandoObject>();

                    //return response from booking service.
                    foreach (var BookingDetail in BookingCollection)
                    {
                        //bind dynamic property.
                        dynamic Booking = new ExpandoObject();

                        //map ids
                        Booking.BookingId = BookingDetail.BookingId;
                        Booking.CustomerName = BookingDetail.Customer.CustomerName;
                        Booking.SiteName = BookingDetail.Site.SiteName;
                        Booking.WorkType = BookingDetail.WorkType.Type;
                        Booking.StatusTiltle = BookingDetail.StatusLookup.Title;
                        Booking.CallingDateTime = BookingDetail.CallingDateTime;
                        Booking.FleetBookingDateTime = BookingDetail.FleetBookingDateTime;
                        Booking.EndDate = BookingDetail.EndDate;

                        //set booking values in list.
                        Bookings.Add(Booking);
                    }

                    //return user service for get organasation.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Booking List", Succeeded = true, DataObject = new ExpandoObject(), DataList = Bookings, ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// this method is for getting all booking 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllBooking")]
        public HttpResponseMessage GetAllBooking()
        {
            try
            {
                //get booking list  by customerId
                var BookingCollection = BookingService.GetAllBooking().OrderByDescending(x => x.BookingNumber).ToList();

                //check object
                if (BookingCollection.Count > 0 && BookingCollection != null)
                {
                    //dynamic list.
                    dynamic Bookings = new List<ExpandoObject>();

                    //return response from booking service.
                    foreach (var BookingDetail in BookingCollection)
                    {
                        //bind dynamic property.
                        dynamic Booking = new ExpandoObject();

                        //map ids
                        Booking.BookingId = BookingDetail.BookingId;
                        Booking.CustomerName = BookingDetail.Customer.CustomerName;
                        Booking.SiteName = BookingDetail.Site.SiteName;
                        Booking.WorkType = BookingDetail.WorkType.Type;
                        Booking.StatusTiltle = BookingDetail.StatusLookup.Title;
                        Booking.CallingDateTime = BookingDetail.CallingDateTime;
                        Booking.FleetBookingDateTime = BookingDetail.FleetBookingDateTime;
                        Booking.EndDate = BookingDetail.EndDate;

                        //set booking values in list.
                        Bookings.Add(Booking);
                    }

                    //return user service for get organasation.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Booking List", Succeeded = true, DataObject = new ExpandoObject(), DataList = Bookings, ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// this method is for getting all booking by with date range filter 
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllBookingByDateRange/{FromDate}/{ToDate}")]
        public HttpResponseMessage GetAllBookingByDateRange(string FromDate, string ToDate)

        {
            try
            {
                // declare global variable for method
                DateTime fromDate;
                DateTime toDate;
                bool IsFleetAllocated = false;
                if (FromDate == "0" && ToDate == "0")
                {

                    // getting date of current month
                    DateTime now = DateTime.UtcNow;
                    fromDate = now.Date.AddMonths(-1);
                    toDate = now.Date;

                }
                else
                {
                    fromDate = Convert.ToDateTime(FromDate);
                    toDate = Convert.ToDateTime(ToDate);
                }

                //get booking list 
                var BookingCollection = BookingService.GetAllBookingDateRange(fromDate, toDate).OrderByDescending(x => x.BookingNumber).ToList();

                //check object
                if (BookingCollection.Count > 0 && BookingCollection != null)
                {
                    //dynamic list.
                    dynamic Bookings = new List<ExpandoObject>();

                    //return response from booking service.
                    foreach (var BookingDetail in BookingCollection)
                    {
                        //bind dynamic property.
                        dynamic Booking = new ExpandoObject();

                        //map ids
                        Booking.BookingId = BookingDetail.BookingId;
                        Booking.BookingNumber = BookingDetail.BookingNumber;
                        Booking.CustomerName = BookingDetail.Customer.CustomerName;
                        Booking.SiteName = BookingDetail.Site.SiteName;
                        Booking.SiteDetail = BookingDetail.Site.SiteDetail;
                        Booking.WorkType = BookingDetail.WorkType.Type;
                        Booking.StatusTitle = BookingDetail.StatusLookup.Title;
                        Booking.CallingDateTime = BookingDetail.CallingDateTime;
                        Booking.FleetBookingDateTime = BookingDetail.FleetBookingDateTime.ToString("dd-MM-yyyy hh:mm tt");
                        Booking.EndDate = BookingDetail.EndDate;
                        Booking.FleetCount = BookingDetail.BookingFleets.Count;
                        if (BookingDetail.StatusLookup.Title == ELookUpGroup.Pending.ToString())
                        {
                            // Is booking pending
                            IsFleetAllocated = true;
                        }

                        // booked customer detail
                        Booking.CustomerABN = BookingDetail.Customer.ABN == null ? "" : BookingDetail.Customer.ABN;
                        Booking.EmailForInvoices = BookingDetail.Customer.EmailForInvoices == null ? "" : BookingDetail.Customer.EmailForInvoices;
                        Booking.ContactNumber = BookingDetail.Customer.ContactNumber == null ? "" : BookingDetail.Customer.ContactNumber;

                        //bind dynamic list property.
                        dynamic BookingFleetList = new List<ExpandoObject>();

                        //adding booking fleet detail 
                        if (BookingDetail.BookingFleets.Count > 0)
                        {

                            foreach (var BookingFleetDetail in BookingDetail.BookingFleets)
                            {
                                //bind dynamic property.
                                dynamic BookingFleetObject = new ExpandoObject();
                                //bind child object.
                                BookingFleetObject.FleetBookingDateTime = BookingFleetDetail.FleetBookingDateTime;
                                BookingFleetObject.FleetNumber = BookingFleetDetail.FleetsRegistration != null ? BookingFleetDetail.FleetsRegistration.Registration : "";
                                BookingFleetObject.FleetDescription = BookingFleetDetail.FleetType.Description;
                                BookingFleetObject.SiteDetail = BookingFleetDetail.Booking.Site.SiteDetail;
                                BookingFleetObject.CallingDateTime = BookingFleetDetail.Booking.CallingDateTime;
                                BookingFleetObject.FleetBookingEndDate = BookingFleetDetail.FleetBookingEndDate;
                                BookingFleetObject.DriverId = BookingFleetDetail.DriverId;
                                //if (BookingFleetObject.DriverId == null || BookingFleetDetail.FleetsRegistration == null)
                                //{
                                //    IsFleetAllocated = true;
                                //}

                                //set BookingFleet detail in list.
                                BookingFleetList.Add(BookingFleetObject);
                            }
                        }
                        Booking.Fleet = BookingFleetList;
                        Booking.IsAllocated = IsFleetAllocated;
                        //set booking values in list.
                        Bookings.Add(Booking);
                    }

                    //return user service for get organasation.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Booking List", Succeeded = true, DataObject = new ExpandoObject(), DataList = Bookings, ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// this method is used for getting booking detail
        /// </summary>
        /// <param name="BookingId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBookingDetail/{BookingId}")]
        public HttpResponseMessage GetBookingDetail(Guid BookingId)
        {
            try
            {
                //check is valid booking id.
                if (!ServiceHelper.IsGuid((string)BookingId.ToString()))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid booking id." });
                }

                //getting booking detail by booking Id
                var BookingDetail = BookingService.GetBookingByBookingId(BookingId);

                // getting booking fleet records
                var BookingFleets = BookingFleetService.GetBookingFleetsByBookingId(BookingId);

                //check service response.
                if (BookingDetail != null)
                {
                    //bind dynamic property.
                    dynamic Booking = new ExpandoObject();
                    Booking.BookingId = BookingDetail.BookingId;
                    Booking.CustomerId = BookingDetail.CustomerId;
                    Booking.SiteId = BookingDetail.SiteId;
                    Booking.WorktypeId = BookingDetail.WorktypeId;
                    Booking.StatusLookupId = BookingDetail.StatusLookupId;
                    Booking.BookingNumber = BookingDetail.BookingNumber;
                    Booking.CallingDateTime = BookingDetail.CallingDateTime.ToString("dd-MM-yyyy hh:mm tt");
                    Booking.FleetBookingDateTime = BookingDetail.FleetBookingDateTime.Date;
                    Booking.EndDate = BookingDetail.EndDate;
                    Booking.BookingNumber = BookingDetail.BookingNumber;
                    Booking.AllocationNotes = BookingDetail.AllocationNotes;

                    //bind dynamic list property.
                    dynamic BookingFleetList = new List<ExpandoObject>();

                    //adding booking fleet detail 
                    if (BookingFleets != null)
                    {
                        foreach (var BookingFleetDetail in BookingFleets)
                        {
                            //bind dynamic property.
                            dynamic BookingFleetObject = new ExpandoObject();
                            //bind child object.
                            BookingFleetObject.BookingFleetId = BookingFleetDetail.BookingFleetId.ToString();
                            BookingFleetObject.BookingId = BookingFleetDetail.BookingId.ToString();
                            BookingFleetObject.FleetTypeId = BookingFleetDetail.FleetTypeId.ToString();
                            BookingFleetObject.FleetRegistrationId = BookingFleetDetail.FleetRegistrationId.ToString();
                            BookingFleetObject.DriverId = BookingFleetDetail.DriverId.ToString();
                            BookingFleetObject.StatusLookupId = BookingFleetDetail.StatusLookupId.ToString();
                            BookingFleetObject.IsDayShift = BookingFleetDetail.IsDayShift;
                            BookingFleetObject.Iswethire = BookingFleetDetail.Iswethire;
                            BookingFleetObject.AttachmentIds = BookingFleetDetail.AttachmentIds;
                            BookingFleetObject.NotesForDrive = BookingFleetDetail.NotesForDrive;
                            BookingFleetObject.IsfleetCustomerSite = BookingFleetDetail.IsfleetCustomerSite;
                            BookingFleetObject.FleetNumber = BookingFleetDetail.FleetsRegistration != null ? BookingFleetDetail.FleetsRegistration.Registration : "";
                            BookingFleetObject.FleetDescription = BookingFleetDetail.FleetType.Description;
                            BookingFleetObject.DriverName = BookingFleetDetail.Driver != null ? BookingFleetDetail.Driver.FirstName + " " + BookingFleetDetail.Driver.LastName : "";
                            BookingFleetObject.FleetBookingDateTime = BookingFleetDetail.FleetBookingDateTime.ToString("dd-MM-yyyy hh:mm tt");
                            BookingFleetObject.FleetBookingEndDate = BookingFleetDetail.FleetBookingEndDate;
                            BookingFleetObject.FleetName = BookingFleetDetail.FleetType.Fleet;

                            //set BookingFleet detail in list.
                            BookingFleetList.Add(BookingFleetObject);
                        }
                    }

                    Booking.BookedFleetList = BookingFleetList;

                    //call skill service for get skill.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Booking fleet details", Succeeded = true, DataObject = Booking, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    //return result from service response.
                    return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

            }
        }

        /// <summary>
        /// This method use for add record into booking entity.
        /// </summary>
        /// <param name="BookingDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddBooking")]
        public HttpResponseMessage AddBooking(BookingDataModel BookingDataModel)
        {
            try
            {
                //add booking detail into database
                var BookingId = BookingService.AddBooking(BookingDataModel);

                /// checking booking id not equal to null
                if (BookingId != null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Booking successfully added", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }

                //return service for get user.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

            }
            catch (Exception ex)
            {
                // handel exception log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// method to soft delete booking by bookingId
        /// </summary>
        /// <param name="BookingId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteBooking/{BookingId}")]
        public HttpResponseMessage DeleteBooking(Guid BookingId)
        {
            try
            {
                var DeleteBooking = BookingService.DeleteBooking(BookingId);
                if (DeleteBooking)
                {
                    //return booking delete service for get task type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Deleted successfully", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// this method is used for updating booking status
        /// </summary>
        /// <param name="BookingId"></param>
        /// <param name="StatusValue"></param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("UpdateBookingStatus/{BookingId}/{StatusValue}/{CancelNote}/{Rate}")]
        //public HttpResponseMessage UpdateBookingStatus(Guid BookingId, int StatusValue,string CancelNote, string Rate)
        //{
        //    try
        //    {
        //        // status text correponding to value setted in enum
        //        var EnumText = ETaskStatusHelper.GetEnumValue<ELookUpGroup>(StatusValue).ToString();

        //        //get statuslookup Id  by title
        //        var StatusLookupId = StatusLookupService.GetStatusByTitle(EnumText).StatusLookupId;

        //        // calling update status booking method
        //        var UpdateBooking = BookingService.UpdateBookingStatus(BookingId, StatusLookupId, CancelNote, Rate);
        //        if (UpdateBooking)
        //        {
        //            //return booking update service for get task type.
        //            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Delete data", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
        //        }
        //        else
        //        {
        //            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handel Exception Log.
        //        Console.Write(ex.Message);

        //        //return case of exception.
        //        return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
        //    }
        //}

        /// <summary>
        /// this method is used for updating booking status
        /// </summary>
        /// <param name="StatusDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateBookingStatus")]
        public HttpResponseMessage UpdateBookingStatus(StatusDataModel StatusDataModel)
        {
            try
            {
                // status text correponding to value setted in enum
                var EnumText = ETaskStatusHelper.GetEnumValue<ELookUpGroup>(StatusDataModel.StatusValue).ToString();

                //get statuslookup Id  by title
                var StatusLookupId = StatusLookupService.GetStatusByTitle(EnumText).StatusLookupId;

                // calling update status booking method
                var UpdateBooking = BookingService.UpdateBookingStatus(StatusDataModel.BookingId, StatusLookupId, StatusDataModel.CancelNote, StatusDataModel.Rate);
                if (UpdateBooking)
                {
                    //return booking update service for get task type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Status Updated", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        #endregion

        #region Booking Fleets Api's

        /// <summary>
        /// this method is used for getting booking detail
        /// </summary>
        /// <param name="BookingId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBookingFleetDetail/{BookingFleetId}")]
        public HttpResponseMessage GetBookingFleetDetail(Guid BookingFleetId)
        {
            try
            {
                //check is valid BookingFleetId.
                if (!ServiceHelper.IsGuid((string)BookingFleetId.ToString()))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid booking fleet id." });
                }

                //getting booking fleet detail by booking fleet Id
                var BookingFleetDetail = BookingFleetService.GetBookingFleetDetailByBookingFleetId(BookingFleetId);

                //check service response.
                if (BookingFleetDetail != null)
                {
                    //bind dynamic property.
                    dynamic BookingFleet = new ExpandoObject();
                    BookingFleet.BookingFleetId = BookingFleetDetail.BookingFleetId;
                    BookingFleet.BookingId = BookingFleetDetail.BookingId;
                    BookingFleet.FleetTypeId = BookingFleetDetail.FleetTypeId;
                    BookingFleet.FleetRegistrationId = BookingFleetDetail.FleetRegistrationId;
                    BookingFleet.DriverId = BookingFleetDetail.DriverId;
                    BookingFleet.StatusLookupId = BookingFleetDetail.StatusLookupId;
                    BookingFleet.IsDayShift = BookingFleetDetail.IsDayShift;
                    BookingFleet.Iswethire = BookingFleetDetail.Iswethire;
                    BookingFleet.AttachmentIds = BookingFleetDetail.AttachmentIds;
                    BookingFleet.NotesForDrive = BookingFleetDetail.NotesForDrive;
                    BookingFleet.IsfleetCustomerSite = BookingFleetDetail.IsfleetCustomerSite;
                    BookingFleet.FleetBookingDateTime = BookingFleetDetail.FleetBookingDateTime.ToString("dd-MM-yyyy hh:mm tt");
                    BookingFleet.FleetBookingEndDate = BookingFleetDetail.FleetBookingEndDate;
                    BookingFleet.Reason = "";
                    BookingFleet.SiteName = BookingFleetDetail.Booking.Site.SiteDetail;
                    BookingFleet.Fleet = BookingFleetDetail.FleetType.Fleet;
                    BookingFleet.CustomerName = BookingFleetDetail.Booking.Customer.CustomerName;
                    //BookingFleet.CustomerName = BookingFleetDetail.FleetsRegistration.;

                    //bind dynamic list property.
                    dynamic BookingSiteSupervisors = new List<ExpandoObject>();

                    // checking count not equal to zero
                    if (BookingFleetDetail.Booking.BookingSiteSupervisors.Count > 0)
                    {
                        foreach (var BookingSupervisor in BookingFleetDetail.Booking.BookingSiteSupervisors)
                        {
                            //bind dynamic property.
                            dynamic BookingSiteSupervisor = new ExpandoObject();
                            //bind child object.
                            BookingSiteSupervisor.BookingSiteSupervisorId = BookingSupervisor.BookingSiteSupervisorId;
                            BookingSiteSupervisor.SupervisorId = BookingSupervisor.Supervisor.SupervisorId;
                            BookingSiteSupervisor.SupervisorName = BookingSupervisor.Supervisor.SupervisorName;
                            BookingSiteSupervisor.MobileNumber = BookingSupervisor.Supervisor.MobileNumber;

                            //set BookingSupervisor detail in list.
                            BookingSiteSupervisors.Add(BookingSiteSupervisor);
                        }
                    }

                    //bind dynamic list property.
                    dynamic BookedFleetList = new List<ExpandoObject>();

                    if (BookingFleetDetail.FleetRegistrationId != null)
                    {
                        //getting detail of selected fleet
                        var FleetDetail = FleetsRegistrationService.GetRegisterFleetsByFleetRegistrationId(BookingFleetDetail.FleetRegistrationId.Value);

                        //bind dynamic property.
                        dynamic BookedFleetObject = new ExpandoObject();
                        //bind child object.
                        BookedFleetObject.FleetRegistrationId = FleetDetail.FleetRegistrationId;
                        BookedFleetObject.RegistrationNumber = FleetDetail.Registration;

                        //set BookedFleet detail in list.
                        BookedFleetList.Add(BookedFleetObject);
                    }
                    BookingFleet.BookedFleetDetail = BookedFleetList;
                    BookingFleet.BookingSiteSupervisors = BookingSiteSupervisors;
                    BookingFleet.DriverName = BookingFleetDetail.Driver.FirstName + " " + BookingFleetDetail.Driver.LastName;

                    //call skill service for get skill.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Booking fleet details", Succeeded = true, DataObject = BookingFleet, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    //return result from service response.
                    return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

            }
        }

        /// <summary>
        /// This method use for add record into booking fleet entity.
        /// </summary>
        /// <param name="BookingDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddBookingFleet")]
        public HttpResponseMessage AddBookingFleet(BookingDataModel BookingDataModel)
        {
            try
            {
                /// variable for holding booking fleet id
                var BookingFleetId = string.Empty;
                var StatusLookupId = (Guid?)null;
                var PendingId = (Guid?)null;
                var AllocatedId = (Guid?)null;
                var BookingStatus = false;
                // status text correponding to value setted in enum
                //var EnumText = ETaskStatusHelper.GetEnumValue<ELookUpGroup>(StatusValue).ToString();

                if (BookingDataModel.BookingFleet.DriverId == "" || BookingDataModel.BookingFleet.DriverId == null)
                {
                    //get statuslookup Id  by title
                    StatusLookupId = StatusLookupService.GetStatusByTitle(ELookUpGroup.Pending.ToString()).StatusLookupId;
                    PendingId = StatusLookupId;
                }
                else
                {
                    //get statuslookup Id  by title
                    StatusLookupId = StatusLookupService.GetStatusByTitle(ELookUpGroup.Allocated.ToString()).StatusLookupId;
                    AllocatedId = StatusLookupId;
                }
                // setting statusLookupId into model
                BookingDataModel.BookingFleet.StatusLookupId = StatusLookupId.ToString();

                /// add record in booking fleet table
                BookingFleetId = BookingFleetService.AddBookingFleet(BookingDataModel.BookingFleet);
                /// return if not succeded
                if (BookingFleetId != null)
                {
                    var Booking = new List<BookingFleet>();

                    // checking condition of null
                    if (PendingId != null)
                    {
                        // getting bookingfleet detail by bookingFleetId
                        Booking = BookingFleetService.GetBookingFleetsByBookingId(new Guid(BookingDataModel.BookingFleet.BookingId)).Where(x => x.StatusLookupId == StatusLookupId).ToList();
                        if (Booking.Count > 0)
                        {
                            BookingStatus = BookingService.UpdateBookingStatus(new Guid(BookingDataModel.BookingFleet.BookingId), StatusLookupId.Value, "", "");
                        }
                    }
                    if (AllocatedId != null)
                    {
                        // getting bookingfleet detail by bookingFleetId
                        Booking = BookingFleetService.GetBookingFleetsByBookingId(new Guid(BookingDataModel.BookingFleet.BookingId)).ToList();

                        // getting bookingfleet detail by bookingFleetId and status filter
                        var BookingAllocated = BookingFleetService.GetBookingFleetsByBookingId(new Guid(BookingDataModel.BookingFleet.BookingId)).Where(x => x.StatusLookupId == StatusLookupId).ToList();
                        if (Booking.Count == BookingAllocated.Count)
                        {
                            BookingStatus = BookingService.UpdateBookingStatus(new Guid(BookingDataModel.BookingFleet.BookingId), StatusLookupId.Value, "", "");
                        }
                    }
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Booking successfully added", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }

            }
            catch (Exception ex)
            {
                // handel exception log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// This method use for update record into booking fleet entity.
        /// </summary>
        /// <param name="BookingDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateBookingFleet")]
        public HttpResponseMessage UpdateBookingFleet(BookingDataModel BookingDataModel)
        {
            try
            {
                BookingFleetDataModel BookingFleetDataModel = new BookingFleetDataModel();
                var StatusLookupId = (Guid?)null;
                var PendingId = (Guid?)null;
                var AllocatedId = (Guid?)null;
                var BookingStatus = false;

                // status text correponding to value setted in enum
                if (BookingDataModel.BookingFleet.DriverId == null || BookingDataModel.BookingFleet.DriverId == "")
                {
                    //get statuslookup Id  by title
                    StatusLookupId = StatusLookupService.GetStatusByTitle(ELookUpGroup.Pending.ToString()).StatusLookupId;
                    PendingId = StatusLookupId;
                }
                else
                {
                    //get statuslookup Id  by title
                    StatusLookupId = StatusLookupService.GetStatusByTitle(ELookUpGroup.Allocated.ToString()).StatusLookupId;
                    AllocatedId = StatusLookupId;
                }
                // getting detail of bookingFleet by bookingfleetId
                var BookedFleet = BookingFleetService.GetBookingFleetDetailByBookingFleetId(new Guid(BookingDataModel.BookingFleet.BookingFleetId));

                // assigning value to BookingFleetDataModel
                BookingFleetDataModel.BookingFleetId = BookedFleet.BookingFleetId.ToString();
                BookingFleetDataModel.BookingId = BookedFleet.BookingId.ToString();
                BookingFleetDataModel.FleetTypeId = BookedFleet.FleetTypeId.ToString();
                BookingFleetDataModel.FleetRegistrationId = BookedFleet.FleetRegistrationId.ToString();
                BookingFleetDataModel.DriverId = BookedFleet.DriverId.ToString();
                BookingFleetDataModel.StatusLookupId = BookedFleet.StatusLookupId.ToString();
                BookingFleetDataModel.IsDayShift = BookedFleet.IsDayShift;
                BookingFleetDataModel.Iswethire = BookedFleet.Iswethire;
                BookingFleetDataModel.AttachmentIds = BookedFleet.AttachmentIds;
                BookingFleetDataModel.NotesForDrive = BookedFleet.NotesForDrive;
                BookingFleetDataModel.Reason = BookingDataModel.BookingFleet.Reason;
                BookingFleetDataModel.IsfleetCustomerSite = BookedFleet.IsfleetCustomerSite;
                BookingFleetDataModel.IsActive = true;

                // updating FleetHistory table
                var FleetHistory = FleetHistoryService.AddFleetHistory(BookingFleetDataModel);

                // setting statusLookupId into model
                BookingDataModel.BookingFleet.StatusLookupId = StatusLookupId.ToString();

                /// add record in booking fleet table
                var response = BookingFleetService.UpdateBookingFleet(BookingDataModel.BookingFleet);
                /// return if not succeded
                if (response)
                {
                    var Booking = new List<BookingFleet>();

                    // checking condition of null
                    if (PendingId != null)
                    {
                        // getting bookingfleet detail by bookingFleetId
                        Booking = BookingFleetService.GetBookingFleetsByBookingId(new Guid(BookingDataModel.BookingFleet.BookingId)).Where(x => x.StatusLookupId == StatusLookupId).ToList();
                        if (Booking.Count > 0)
                        {
                            BookingStatus = BookingService.UpdateBookingStatus(new Guid(BookingDataModel.BookingFleet.BookingId), StatusLookupId.Value, "", "");
                        }
                    }
                    if (AllocatedId != null)
                    {
                        // getting bookingfleet detail by bookingFleetId
                        Booking = BookingFleetService.GetBookingFleetsByBookingId(new Guid(BookingDataModel.BookingFleet.BookingId)).ToList();

                        // getting bookingfleet detail by bookingFleetId and status filter
                        var BookingAllocated = BookingFleetService.GetBookingFleetsByBookingId(new Guid(BookingDataModel.BookingFleet.BookingId)).Where(x => x.StatusLookupId == StatusLookupId).ToList();
                        if (Booking.Count == BookingAllocated.Count)
                        {
                            BookingStatus = BookingService.UpdateBookingStatus(new Guid(BookingDataModel.BookingFleet.BookingId), StatusLookupId.Value, "", "");
                        }
                    }
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Booking successfully updated", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }

            }
            catch (Exception ex)
            {
                // handel exception log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// this method is used for deleting booking fleet
        /// </summary>
        /// <param name="BookingFleetId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteBookingFleet/{BookingFleetId}")]
        public HttpResponseMessage DeleteBookingFleet(Guid BookingFleetId)
        {
            try
            {
                //check is valid BookingFleetId.
                if (!ServiceHelper.IsGuid((string)BookingFleetId.ToString()))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid booking fleet id." });
                }

                //getting booking fleet detail by booking fleet Id
                var BookingFleetDetail = BookingFleetService.DeleteBookingFleet(BookingFleetId);

                //check service response.
                if (BookingFleetDetail)
                {

                    //call skill service for get skill.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Deleted Successfully", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    //return result from service response.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Not deleted successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

            }
        }

        /// <summary>
        /// this method is used for updating booking status
        /// </summary>
        /// <param name="StatusDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateBookingFleetStatus")]
        public HttpResponseMessage UpdateBookingFleetStatus(StatusDataModel StatusDataModel)
        {
            try
            {
                // status text correponding to value setted in enum
                var EnumText = ETaskStatusHelper.GetEnumValue<ELookUpGroup>(StatusDataModel.StatusValue).ToString();

                //get statuslookup Id  by title
                var StatusLookupId = StatusLookupService.GetStatusByTitle(EnumText).StatusLookupId;

                // calling update status booking method
                var UpdateBooking = BookingFleetService.UpdateBookingFleetStatus(StatusDataModel.BookingFleetId, StatusLookupId);
                if (UpdateBooking)
                {
                    //return booking update service for get task type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Status Updated", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        #endregion

        #region BookingSiteDetail Api's

        /// <summary>
        /// this method is used for getting booking site detail
        /// </summary>
        /// <param name="BookingId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBookingSiteDetail/{BookingId}")]
        public HttpResponseMessage GetBookingSiteDetail(Guid BookingId)
        {
            try
            {
                // variable for allocated supervisor list
                var AllocatedSuper = new List<Supervisor>();

                //check is valid booking id.
                if (!ServiceHelper.IsGuid((string)BookingId.ToString()))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid booking id." });
                }

                //getting site Id from booking table corresponding to bookingId
                var Booking = BookingService.GetBookingByBookingId(BookingId);

                //check service response.
                if (Booking != null)
                {
                    // getting supervisor list by siteId
                    var SupervisorList = Booking.Site.Supervisors.Where(x => x.SiteId == Booking.SiteId).ToList();

                    // getting gate list by siteId
                    var Gatelist = Booking.Site.Gates.Where(x => x.SiteId == Booking.SiteId).ToList();

                    dynamic listSupervisors = new List<ExpandoObject>();

                    // Loop through supervisors
                    if (SupervisorList != null)
                    {
                        foreach (var Supervisor in SupervisorList)
                        {
                            dynamic oSupervisors = new ExpandoObject();
                            oSupervisors.SupervisorId = Supervisor.SupervisorId;
                            oSupervisors.SupervisorName = Supervisor.SupervisorName;
                            oSupervisors.Options = Supervisor.SupervisorName;
                            oSupervisors.SupervisorEmail = Supervisor.Email;
                            oSupervisors.SupervisorMobileNumber = Supervisor.MobileNumber;
                            listSupervisors.Add(oSupervisors);
                        }
                    }

                    dynamic listGates = new List<ExpandoObject>();

                    // Loop through gates
                    if (Gatelist != null)
                    {
                        foreach (var Gate in Gatelist)
                        {
                            dynamic oGates = new ExpandoObject();
                            oGates.GateId = Gate.GateId;
                            oGates.GateNumber = Gate.GateNumber;
                            oGates.Option = Gate.GateNumber;
                            //oGates.RegistrationDescription = Booking.BookingFleets.Where(x => x.FleetTypeId == Booking.)

                            listGates.Add(oGates);
                        }
                    }

                    dynamic listBookedFleet = new List<ExpandoObject>();

                    // Loop through gates
                    if (Booking.BookingFleets != null)
                    {
                        foreach (var BookedFleet in Booking.BookingFleets)
                        {
                            dynamic oBookedFleet = new ExpandoObject();
                            if (BookedFleet.FleetRegistrationId != null)
                            {
                                oBookedFleet.FleetRegistrationId = BookedFleet.FleetRegistrationId;
                                oBookedFleet.RegistrationDescription = BookedFleet.FleetsRegistration.Registration + '/' + BookedFleet.FleetType.Description;

                                listBookedFleet.Add(oBookedFleet);
                            }
                        }
                    }

                    dynamic listAllocatedSuper = new List<ExpandoObject>();

                    var allocatedList = Booking.BookingSiteSupervisors.Where(x => x.BookingId == Booking.BookingId);
                    //Loop through supervisors
                    if (SupervisorList != null)
                    {
                        foreach (var allocated in allocatedList)
                        {
                            var superVisorDetail = SupervisorService.GetSupervisorBySupervisorId(allocated.SupervisorId);
                            dynamic oAllocatedSuper = new ExpandoObject();
                            oAllocatedSuper.SupervisorId = superVisorDetail.SupervisorId;
                            oAllocatedSuper.SupervisorName = superVisorDetail.SupervisorName;
                            oAllocatedSuper.Options = superVisorDetail.SupervisorName;
                            oAllocatedSuper.SupervisorEmail = superVisorDetail.Email;
                            oAllocatedSuper.SupervisorMobileNumber = superVisorDetail.MobileNumber;
                            listAllocatedSuper.Add(oAllocatedSuper);
                        }
                    }

                    dynamic listAllocatedGate = new List<ExpandoObject>();

                    //Loop through bookingSiteGate
                    if (Booking.BookingSiteGates != null)
                    {
                        // apply join on gate, GateContactPerson and BookingSiteGate
                        //var AllocatedGate = GateContactPersonService.GetAllGateContactPerson().Join(BookingSiteGateService.GetAllBookingSiteGate(), a => a.GateId, g => g.GateId, (a, g) => new { a, g }).Join(allocatedGateList, c => c.g.GateId, ac => ac.GateId, (c, ac) => new { c, ac }).ToList();
                        foreach (var allocatedGate in Booking.BookingSiteGates)
                        {
                            // GateContactPerson List
                            dynamic oAllocatedGate = new ExpandoObject();

                            // getting detail of gateContactperson 
                            oAllocatedGate.GateId = allocatedGate.GateId;
                            oAllocatedGate.GateNumber = allocatedGate.Gate.GateNumber;
                            oAllocatedGate.FleetRegistrationId = allocatedGate.FleetsRegistration.FleetRegistrationId;
                            oAllocatedGate.Registration = allocatedGate.FleetsRegistration.Registration;
                            oAllocatedGate.GateContactPersonId = allocatedGate.GateContactPersonId;
                            oAllocatedGate.ContactPerson = allocatedGate.GateContactPerson.ContactPerson;
                            oAllocatedGate.Email = allocatedGate.GateContactPerson.Email;
                            oAllocatedGate.MobileNumber = allocatedGate.GateContactPerson.MobileNumber;

                            // adding object into list
                            listAllocatedGate.Add(oAllocatedGate);

                        }
                    }

                    // getting allocated supervisor list
                    if (Booking.SiteId != null)
                    {
                        AllocatedSuper = SupervisorService.GetSupervisorListBySiteId(Booking.SiteId.Value);
                    }
                    //bind dynamic property.
                    dynamic BookingSite = new ExpandoObject();
                    BookingSite.BookingId = Booking.BookingId;
                    BookingSite.CustomerName = Booking.Customer.CustomerName;
                    BookingSite.SiteName = Booking.Site.SiteName;
                    BookingSite.SiteDetail = Booking.Site.SiteDetail;
                    BookingSite.SupervisorList = listSupervisors;
                    BookingSite.GateList = listGates;
                    BookingSite.FleetList = listBookedFleet;
                    BookingSite.SiteNote = Booking.SiteNote;
                    BookingSite.AllocatedSupervisor = listAllocatedSuper;
                    BookingSite.AllocatedGateContactList = listAllocatedGate;

                    //call skill service for get skill.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Booking fleet details", Succeeded = true, DataObject = BookingSite, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    //return result from service response.
                    return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

            }
        }

        /// <summary>
        /// getting all contactPerson select list for dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetContactPersons/{GateId}")]
        public HttpResponseMessage GetContactPersons(Guid GateId)
        {
            try
            {
                //get contact person type list
                var GateContactPerson = GateContactPersonService.GetGateContactPersonList(GateId);

                //check object
                if (GateContactPerson.Count > 0 && GateContactPerson != null)
                {
                    List<ExpandoObject> GateContactPersonlist = new List<ExpandoObject>();
                    foreach (var ContactPersons in GateContactPerson)
                    {
                        dynamic GateContactPersonDetail = new ExpandoObject();
                        GateContactPersonDetail.GateContactPersonId = ContactPersons.GateContactPersonId;
                        GateContactPersonDetail.ContactPerson = ContactPersons.ContactPerson;
                        GateContactPersonDetail.MobileNumber = ContactPersons.MobileNumber;
                        GateContactPersonDetail.Email = ContactPersons.Email;
                        GateContactPersonDetail.IsDefault = ContactPersons.IsDefault;
                        GateContactPersonlist.Add(GateContactPersonDetail);
                    }
                    //return customer service for get customer type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Contact Person List", Succeeded = true, DataObject = new ExpandoObject(), DataList = GateContactPersonlist, ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// this method is used for adding booking site detail
        /// </summary>
        /// <param name="BookingSiteDetailDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddBookingSiteDetail")]
        public HttpResponseMessage AddBookingSiteDetail(BookingSiteDetailDataModel BookingSiteDetailDataModel)
        {
            try
            {
                /// variable for holding booking fleet id
                var BookingGate = false;
                var BookingSupervisor = false;
                var SiteNote = false;

                if (BookingSiteDetailDataModel.SupervisorId != "")
                {
                    // pushing supervisorId into array
                    string[] SupervisorId = BookingSiteDetailDataModel.SupervisorId.Split(',');

                    // deleteing existing supervisor
                    var BookingSupervisorDelete = BookingSiteSupervisorService.DeleteBookingSiteSupervisorByBookingId(new Guid(BookingSiteDetailDataModel.BookingId));

                    foreach (var item in SupervisorId)
                    {
                        BookingSiteDetailDataModel.SupervisorId = item;

                        //add booking site supervisor detail into database
                        BookingSupervisor = BookingSiteSupervisorService.AddBookingSiteSupervisor(BookingSiteDetailDataModel);
                    }
                }
                else
                {
                    var BookingSupervisorDelete = BookingSiteSupervisorService.DeleteBookingSiteSupervisorByBookingId(new Guid(BookingSiteDetailDataModel.BookingId));
                }

                /// checking response of success
                if (BookingSiteDetailDataModel.GateId != "")
                {
                    // pushing gateId into array
                    string[] GateId = BookingSiteDetailDataModel.GateId.Split(',');

                    // pushing gateContactPersonId into array
                    string[] GateContactPersonId = BookingSiteDetailDataModel.GateContactPersonId.Split(',');

                    // pushing FleetRegistrationId into array
                    string[] FleetRegistrationId = BookingSiteDetailDataModel.FleetRegistrationId.Split(',');

                    // deleteing existing supervisor
                    var BookingGateDelete = BookingSiteGateService.DeleteBookingSiteGateByBookingId(new Guid(BookingSiteDetailDataModel.BookingId));

                    var index = 0;
                    foreach (var item in GateId)
                    {
                        BookingSiteDetailDataModel.FleetRegistrationId = FleetRegistrationId[index];
                        BookingSiteDetailDataModel.GateContactPersonId = GateContactPersonId[index];
                        BookingSiteDetailDataModel.GateId = item;

                        /// add record in booking fleet table
                        BookingGate = BookingSiteGateService.AddBookingSiteGate(BookingSiteDetailDataModel);
                        ++index;
                    }
                    /// return if not succeded
                    if (BookingGate == true)
                    {
                        SiteNote = BookingService.UpdateSiteNote(new Guid(BookingSiteDetailDataModel.BookingId), BookingSiteDetailDataModel.SiteNote);
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Booking supervisor and gate successfully added", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    }
                    //return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Booking supervisor and gate successfully added", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }

                else
                {
                    var BookingGateDelete = BookingSiteGateService.DeleteBookingSiteGateByBookingId(new Guid(BookingSiteDetailDataModel.BookingId));
                }

                // update site note column
                SiteNote = BookingService.UpdateSiteNote(new Guid(BookingSiteDetailDataModel.BookingId), BookingSiteDetailDataModel.SiteNote);
                //return service for get user.
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "already added", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
            }
            catch (Exception ex)
            {
                // handel exception log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// this method is used for updating booking site status
        /// </summary>
        /// <param name="BookingSiteDetailDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateBookingSiteDetail")]
        public HttpResponseMessage UpdateBookingSiteDetail(BookingSiteDetailDataModel BookingSiteDetailDataModel)
        {
            try
            {
                /// variable for holding booking fleet id
                var BookingGate = false;
                var BookingSupervisor = false;

                // adding requested supervisor
                foreach (var item in BookingSiteDetailDataModel.BookingSiteList)
                {
                    //add booking site supervisor detail into database
                    BookingSupervisor = BookingSiteSupervisorService.UpdateBookingSiteSupervisor(item);
                }

                /// checking response of success
                if (BookingSupervisor == true)
                {
                    foreach (var item in BookingSiteDetailDataModel.BookingSiteList)
                    {
                        /// add record in booking fleet table
                        BookingGate = BookingSiteGateService.UpdateBookingSiteGate(item);
                    }
                    /// return if not succeded
                    if (BookingGate == true)
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Booking supervisor and gate successfully added", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    }
                }

                //return service for get user.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

            }
            catch (Exception ex)
            {
                // handel exception log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        #endregion

        #region Gate & GateContactPerson Api's

        /// <summary>
        /// this method is for getting all gate 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllGate/{SiteId}")]
        public HttpResponseMessage GetAllGate(string SiteId)
        {
            try
            {
                //get gate list 
                var GateCollection = GatesService.GetAllGates().ToList();

                if (ServiceHelper.IsGuid(SiteId))
                {
                    GateCollection = GatesService.GetAllGates().Where(x => x.SiteId == new Guid(SiteId)).OrderByDescending(x => x.CreatedDate).ToList();
                }

                //check object
                if (GateCollection.Count > 0 && GateCollection != null)
                {
                    //dynamic list.
                    dynamic Gates = new List<ExpandoObject>();

                    //return response from gate service.
                    foreach (var GateDetail in GateCollection)
                    {
                        //bind dynamic property.
                        dynamic Gate = new ExpandoObject();

                        //map ids
                        Gate.GateId = GateDetail.GateId;
                        Gate.SiteId = GateDetail.SiteId;
                        Gate.SiteName = GateDetail.Site.SiteName;
                        Gate.GateNumber = GateDetail.GateNumber;
                        Gate.TipOffRate = GateDetail.TipOffRate;
                        Gate.TippingSite = GateDetail.TippingSite;

                        //set gate values in list.
                        Gates.Add(Gate);
                    }

                    //return user service for get organasation.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Gate List", Succeeded = true, DataObject = new ExpandoObject(), DataList = Gates, ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// this method is used for getting gate detail
        /// </summary>
        /// <param name="GateId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetGateDetail/{GateId}")]
        public HttpResponseMessage GetGateDetail(Guid GateId)
        {
            try
            {
                //check is valid GateId.
                if (!ServiceHelper.IsGuid((string)GateId.ToString()))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid booking id." });
                }

                //getting gate detail by GateId
                var GateDetail = GatesService.GetGateByGateId(GateId);

                //check service response.
                if (GateDetail != null)
                {
                    //bind dynamic property.
                    dynamic Gate = new ExpandoObject();
                    Gate.GateId = GateDetail.GateId;
                    Gate.SiteId = GateDetail.SiteId;
                    Gate.SiteName = GateDetail.Site.SiteName;
                    Gate.GateNumber = GateDetail.GateNumber;
                    Gate.TipOffRate = GateDetail.TipOffRate;
                    Gate.TippingSite = GateDetail.TippingSite;

                    //bind dynamic list property.
                    dynamic GateContactList = new List<ExpandoObject>();

                    //checking gatecontact is not null
                    if (GateDetail.GateContactPersons != null)
                    {
                        foreach (var GateContactDetail in GateDetail.GateContactPersons)
                        {
                            //bind dynamic property.
                            dynamic GateContactObject = new ExpandoObject();
                            //bind child object.
                            GateContactObject.GateContactPersonId = GateContactDetail.GateContactPersonId;
                            GateContactObject.GateId = GateContactDetail.GateId;
                            GateContactObject.GateNumber = GateContactDetail.Gate.GateNumber;
                            GateContactObject.ContactPerson = GateContactDetail.ContactPerson;
                            GateContactObject.Email = GateContactDetail.Email;
                            GateContactObject.MobileNumber = GateContactDetail.MobileNumber;
                            GateContactObject.IsDefault = GateContactDetail.IsDefault;

                            //set GateContact detail in list.
                            GateContactList.Add(GateContactObject);
                        }
                    }
                    Gate.GateContactPerson = GateContactList;

                    //response of gate service
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Gate details", Succeeded = true, DataObject = Gate, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    //return result from service response.
                    return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

            }
        }

        /// <summary>
        /// this method is used for adding gate
        /// </summary>
        /// <param name="GateDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddGate")]
        public HttpResponseMessage AddGate(GateDataModel GateDataModel)
        {
            try
            {

                //add booking site supervisor detail into database
                var response = GatesService.AddGate(GateDataModel);

                /// checking response of success
                if (response == true)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Gate successfully added", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }

                //return service for get user.
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

            }
            catch (Exception ex)
            {
                // handel exception log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// this method is used for updating Gate
        /// </summary>
        /// <param name="GateDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateGate")]
        public HttpResponseMessage UpdateGate(GateDataModel GateDataModel)
        {
            try
            {
                //update gate detail into database
                var response = GatesService.UpdateGate(GateDataModel);

                /// checking response of success
                if (response == true)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Gate successfully updated", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }

                //return service for get user.
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

            }
            catch (Exception ex)
            {
                // handel exception log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// this method is used for getting gateContact detail
        /// </summary>
        /// <param name="GateContactId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetGateContactDetail/{GateContactId}")]
        public HttpResponseMessage GetGateContactDetail(Guid GateContactId)
        {
            try
            {
                //check is valid GateContactId.
                if (!ServiceHelper.IsGuid((string)GateContactId.ToString()))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid gateContactId." });
                }

                //getting gateContact detail by GateContactId
                var GateContactDetail = GateContactPersonService.GetGateContactByGateContactId(GateContactId);

                //check service response.
                if (GateContactDetail != null)
                {
                    //bind dynamic property.
                    dynamic GateContact = new ExpandoObject();
                    GateContact.GateContactPersonId = GateContactDetail.GateContactPersonId;
                    GateContact.GateId = GateContactDetail.GateId;
                    GateContact.ContactPerson = GateContactDetail.ContactPerson;
                    GateContact.Email = GateContactDetail.Email;
                    GateContact.MobileNumber = GateContactDetail.MobileNumber;
                    GateContact.IsDefault = GateContactDetail.IsDefault;

                    //response for GateContact Person
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "GateContactPerson details", Succeeded = true, DataObject = GateContact, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    //return result from service response.
                    return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

            }
        }

        /// <summary>
        /// this method is used for adding GateContactPerson
        /// </summary>
        /// <param name="GateContactPersonDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddGateContactPerson")]
        public HttpResponseMessage AddGateContactPerson(GateContactPersonDataModel GateContactPersonDataModel)
        {
            try
            {
                // get gatecontactperson IsDefault checked by gateId
                var IsDefault = GateContactPersonService.GetGateContactIsDefaultByGateId(GateContactPersonDataModel.GateId);
                if (IsDefault != null)
                {
                    // updating IsDefault status
                    var UpdateStatus = GateContactPersonService.UpdateIsDefaultStatus(IsDefault.GateContactPersonId, false);
                }

                //add gate contactperson detail into database
                var response = GateContactPersonService.AddGateContactPerson(GateContactPersonDataModel);

                /// checking response of success
                if (response == true)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Gate Contact successfully added", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }

                //return service for get user.
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

            }
            catch (Exception ex)
            {
                // handel exception log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// this method is used for updating GateContactPerson
        /// </summary>
        /// <param name="GateContactPersonDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateGateContactPerson")]
        public HttpResponseMessage UpdateGateContactPerson(GateContactPersonDataModel GateContactPersonDataModel)
        {
            try
            {
                // get gatecontactperson IsDefault checked by gateId
                var IsDefault = GateContactPersonService.GetGateContactIsDefaultByGateId(GateContactPersonDataModel.GateId);
                if (IsDefault != null)
                {
                    // updating IsDefault status
                    var UpdateStatus = GateContactPersonService.UpdateIsDefaultStatus(IsDefault.GateContactPersonId, false);
                }

                //update gate contactperson detail into database
                var response = GateContactPersonService.UpdateGateContactPerson(GateContactPersonDataModel);

                /// checking response of success
                if (response == true)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Gate Contact successfully updated", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }

                //return service for get user.
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

            }
            catch (Exception ex)
            {
                // handel exception log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }


        #endregion

        #region WorkAllocation Api's

        /// <summary>
        /// this method for all allocation list bt title
        /// </summary>
        /// <param name="Title"></param>(pending,allocated,completed)
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllWorkAllocationByStatus/{StatusValue}")]
        public HttpResponseMessage GetAllWorkAllocationByStatus(int StatusValue)
        {
            try
            {
                // status text correponding to value setted in enum
                var EnumText = ETaskStatusHelper.GetEnumValue<ELookUpGroup>(StatusValue).ToString();

                //get statuslookup Id  by title
                var StatusLookupId = StatusLookupService.GetStatusByTitle(EnumText).StatusLookupId;
                if (StatusLookupId != null)
                {

                    // getting all booking list by status
                    var WorkAllocationCollection = BookingFleetService.GetAllBookingFleetByStatusLookupId(StatusLookupId).OrderByDescending(x => x.CreatedDate).ToList();

                    //check object
                    if (WorkAllocationCollection.Count > 0 && WorkAllocationCollection != null)
                    {
                        //dynamic list.
                        dynamic WorkAllocations = new List<ExpandoObject>();

                        //return response from booking service.
                        foreach (var WorkAllocationDetail in WorkAllocationCollection)
                        {
                            // getting details from mapping tables
                            //var FleetDetail = WorkAllocationDetail.Booking.BookingFleets.Where(x => x.BookingId == WorkAllocationDetail.BookingId).FirstOrDefault();

                            //bind dynamic property.
                            dynamic WorkAllocation = new ExpandoObject();

                            //map ids
                            WorkAllocation.BookingId = WorkAllocationDetail.BookingId;
                            WorkAllocation.BookingFleetId = WorkAllocationDetail.BookingFleetId;
                            WorkAllocation.BookingNumber = WorkAllocationDetail.Booking.BookingNumber;
                            WorkAllocation.FleetBookingDateTime = WorkAllocationDetail.FleetBookingDateTime;
                            WorkAllocation.SiteDetail = WorkAllocationDetail.Booking.Site.SiteDetail;
                            WorkAllocation.CallingDateTime = WorkAllocationDetail.Booking.CallingDateTime;
                            WorkAllocation.EndDate = WorkAllocationDetail.Booking.EndDate;

                            // booked customer detail
                            WorkAllocation.CustomerName = WorkAllocationDetail.Booking.Customer.CustomerName == null ? "" : WorkAllocationDetail.Booking.Customer.CustomerName;
                            WorkAllocation.CustomerABN = WorkAllocationDetail.Booking.Customer.ABN == null ? "" : WorkAllocationDetail.Booking.Customer.ABN;
                            WorkAllocation.EmailForInvoices = WorkAllocationDetail.Booking.Customer.EmailForInvoices == null ? "" : WorkAllocationDetail.Booking.Customer.EmailForInvoices;
                            WorkAllocation.ContactNumber = WorkAllocationDetail.Booking.Customer.ContactNumber == null ? "" : WorkAllocationDetail.Booking.Customer.ContactNumber;

                            if (StatusValue != Convert.ToInt32(ELookUpGroup.Pending))
                            {
                                WorkAllocation.FleetDetail = "Fleet No: " + WorkAllocationDetail.FleetsRegistration.Registration + " Description: " + WorkAllocationDetail.FleetType.Description;
                                WorkAllocation.CustomerName = WorkAllocationDetail.Booking.Customer.CustomerName;
                                WorkAllocation.DriverName = WorkAllocationDetail.Driver.FirstName + " " + WorkAllocationDetail.Driver.LastName;
                                WorkAllocation.Dockets = WorkAllocationDetail.Dockets.Count;
                            }

                            //set booking values in list.
                            WorkAllocations.Add(WorkAllocation);
                        }

                        //return user service for get organasation.
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Work Allocation List", Succeeded = true, DataObject = new ExpandoObject(), DataList = WorkAllocations, ErrorInfo = "" });
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    }
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }

            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// this method for all allocation list bt title
        /// </summary>
        /// <param name="Title"></param>(pending,allocated,completed)
        /// <returns></returns>
        [HttpGet]
        [Route("GetWorkAllocationByBookingId/{BookingId}/{StatusValue}")]
        public HttpResponseMessage GetWorkAllocationByBookingId(Guid BookingId, int StatusValue)
        {
            try
            {
                // status text correponding to value setted in enum
                var EnumText = ETaskStatusHelper.GetEnumValue<ELookUpGroup>(StatusValue).ToString();

                //get statuslookup Id  by title
                var StatusLookupId = StatusLookupService.GetStatusByTitle(EnumText).StatusLookupId;
                if (StatusLookupId != null)
                {

                    // getting all booking list by status
                    var WorkAllocationCollection = BookingFleetService.GetBookingFleetsByBookingId(BookingId).Where(x => x.StatusLookupId == StatusLookupId).OrderByDescending(x => x.CreatedDate).ToList();

                    if (WorkAllocationCollection.Count > 0 && WorkAllocationCollection != null)
                    {
                        //bind dynamic property.
                        dynamic WorkAllocationObj = new ExpandoObject();
                        WorkAllocationObj.CustomerName = WorkAllocationCollection.FirstOrDefault().Booking.Customer.CustomerName;
                        WorkAllocationObj.RegistrationDescription = WorkAllocationCollection.FirstOrDefault().FleetsRegistration != null ? WorkAllocationCollection.FirstOrDefault().FleetsRegistration.Registration + " " + WorkAllocationCollection.FirstOrDefault().FleetType.Description : "";
                        WorkAllocationObj.BookingFleetStartDate = WorkAllocationCollection.FirstOrDefault().FleetBookingDateTime;
                        WorkAllocationObj.BookingFleetEndDate = WorkAllocationCollection.FirstOrDefault().FleetBookingEndDate;
                        WorkAllocationObj.FleetName = WorkAllocationCollection.FirstOrDefault().FleetType.Fleet;
                        WorkAllocationObj.BookingNumber = WorkAllocationCollection.FirstOrDefault().Booking.BookingNumber;
                        WorkAllocationObj.BookingId = WorkAllocationCollection.FirstOrDefault().Booking.BookingId;

                        //check object

                        //dynamic list.
                        dynamic WorkAllocations = new List<ExpandoObject>();

                        //return response from booking service.
                        foreach (var WorkAllocationDetail in WorkAllocationCollection)
                        {
                            // getting details from mapping tables
                            var FleetDetail = WorkAllocationDetail.Booking.BookingFleets.Where(x => x.BookingId == WorkAllocationDetail.BookingId && x.StatusLookupId == StatusLookupId).FirstOrDefault();

                            //bind dynamic property.
                            dynamic WorkAllocation = new ExpandoObject();

                            //map ids
                            WorkAllocation.BookingId = WorkAllocationDetail.BookingId;
                            WorkAllocation.BookingFleetId = WorkAllocationDetail.BookingFleetId;
                            WorkAllocation.BookingNumber = WorkAllocationDetail.Booking.BookingNumber;
                            WorkAllocation.FleetBookingDateTime = WorkAllocationDetail.FleetBookingDateTime;
                            WorkAllocation.SiteDetail = WorkAllocationDetail.Booking.Site.SiteDetail;
                            WorkAllocation.CallingDateTime = WorkAllocationDetail.Booking.CallingDateTime;
                            WorkAllocation.EndDate = WorkAllocationDetail.Booking.EndDate;

                            // booked customer detail
                            WorkAllocation.CustomerName = WorkAllocationDetail.Booking.Customer.CustomerName == null ? "" : WorkAllocationDetail.Booking.Customer.CustomerName;
                            WorkAllocation.CustomerABN = WorkAllocationDetail.Booking.Customer.ABN == null ? "" : WorkAllocationDetail.Booking.Customer.ABN;
                            WorkAllocation.EmailForInvoices = WorkAllocationDetail.Booking.Customer.EmailForInvoices == null ? "" : WorkAllocationDetail.Booking.Customer.EmailForInvoices;
                            WorkAllocation.ContactNumber = WorkAllocationDetail.Booking.Customer.ContactNumber == null ? "" : WorkAllocationDetail.Booking.Customer.ContactNumber;

                            if (StatusValue != Convert.ToInt32(ELookUpGroup.Pending))
                            {
                                var Driver = WorkAllocationDetail.Booking.BookingFleets.Where(x => x.BookingFleetId == WorkAllocationDetail.BookingFleetId).SingleOrDefault().Driver;
                                var Fleet = WorkAllocationDetail.Booking.BookingFleets.Where(x => x.BookingFleetId == WorkAllocationDetail.BookingFleetId).SingleOrDefault();
                                WorkAllocation.FleetDetail = Fleet.FleetsRegistration != null ? "Fleet No: " + Fleet.FleetsRegistration.Registration + " Description: " + Fleet.FleetType.Description : "";
                                WorkAllocation.CustomerName = WorkAllocationDetail.Booking.Customer.CustomerName;

                                WorkAllocation.DriverName = Driver != null ? Driver.FirstName + " " + Driver.LastName : "";
                                WorkAllocation.Dockets = WorkAllocationDetail.Dockets.Count;
                            }

                            //set booking values in list.
                            WorkAllocations.Add(WorkAllocation);
                        }

                        WorkAllocationObj.WorkAllocationList = WorkAllocations;


                        //return user service for get organasation.
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Work Allocation List", Succeeded = true, DataObject = WorkAllocationObj, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    }
                    else if (WorkAllocationCollection.Count == 0 || WorkAllocationCollection == null)
                    {
                        var BookingDetail = BookingService.GetBookingByBookingId(BookingId);

                        dynamic WorkAllocationObj = new ExpandoObject();
                        WorkAllocationObj.CustomerName = BookingDetail.Customer.CustomerName;
                        WorkAllocationObj.BookingFleetStartDate = BookingDetail.FleetBookingDateTime;
                        WorkAllocationObj.BookingFleetEndDate = BookingDetail.EndDate;
                        WorkAllocationObj.BookingNumber = BookingDetail.BookingNumber;
                        WorkAllocationObj.BookingId = BookingDetail.BookingId;

                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Work Allocation List", Succeeded = true, DataObject = WorkAllocationObj, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    }

                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    }
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }

            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        #endregion

        #region FleetHistory Api's

        /// <summary>
        /// this method is for getting all fleetHistory 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllFleetHistory/{BookingFleetId}/{BookingId}")]
        public HttpResponseMessage GetAllFleetHistory(Guid BookingFleetId,Guid BookingId)
        {
            try
            {
                //get FleetHistory list  by BookingFleetId
                var FleetHistoryCollection = FleetHistoryService.GetAllFleetHistoryByBookingFleetId(BookingFleetId).ToList();

                //check object
                if (FleetHistoryCollection.Count > 0 && FleetHistoryCollection != null)
                {
                    //bind dynamic property.
                    dynamic CustomerDetail = new ExpandoObject();
                    CustomerDetail.CustomerName = FleetHistoryCollection.FirstOrDefault().Booking.Customer.CustomerName;
                    CustomerDetail.RegistrationDescription = FleetHistoryCollection.FirstOrDefault().FleetsRegistration != null ? FleetHistoryCollection.FirstOrDefault().FleetsRegistration.Registration + " " + FleetHistoryCollection.FirstOrDefault().FleetType.Description : "";
                    CustomerDetail.BookingFleetStartDate = FleetHistoryCollection.FirstOrDefault().BookingFleet.FleetBookingDateTime;
                    CustomerDetail.BookingFleetEndDate = FleetHistoryCollection.FirstOrDefault().BookingFleet.FleetBookingEndDate;
                    CustomerDetail.FleetName = FleetHistoryCollection.FirstOrDefault().BookingFleet.FleetType.Fleet;
                    CustomerDetail.BookingNumber = FleetHistoryCollection.FirstOrDefault().Booking.BookingNumber;

                    //dynamic list.
                    dynamic FleetHistorys = new List<ExpandoObject>();

                    //return response from FleetHistory service.
                    foreach (var FleetHistoryDetail in FleetHistoryCollection)
                    {
                        //bind dynamic property.
                        dynamic FleetHistory = new ExpandoObject();

                        //map ids
                        FleetHistory.BookingId = FleetHistoryDetail.BookingId;
                        FleetHistory.BookingFleetId = FleetHistoryDetail.BookingFleetId;
                        FleetHistory.FleetTypeId = FleetHistoryDetail.FleetTypeId;
                        FleetHistory.FleetRegistrationId = FleetHistoryDetail.FleetRegistrationId;
                        FleetHistory.DriverId = FleetHistoryDetail.DriverId;
                        FleetHistory.BookingNumber = FleetHistoryDetail.Booking.BookingNumber;
                        FleetHistory.FleetName = FleetHistoryDetail.FleetType.Fleet;
                        FleetHistory.Registration = FleetHistoryDetail.FleetRegistrationId != null ? FleetHistoryDetail.FleetsRegistration.Registration : "";
                        FleetHistory.DriverName = FleetHistoryDetail.DriverId != null ? FleetHistoryDetail.Driver.FirstName + " " + FleetHistoryDetail.Driver.LastName : "";
                        FleetHistory.IsDayShift = FleetHistoryDetail.IsDayShift;

                        // list to add attachment name
                        string Attachment = string.Empty;
                        // convert string to array
                        var AttachmentIds = FleetHistoryDetail.AttachmentIds.Split(',');

                        if (FleetHistoryDetail.AttachmentIds != "")
                        {
                            // foreach loop for getting attachment name by Id
                            foreach (var item in AttachmentIds)
                            {
                                // get attachment name
                                var Name = AttachmentsService.GetAttachmentById(new Guid(item)).AttachmentTitle;
                                // add in array lkist
                                if (Attachment == string.Empty)
                                    Attachment += Name;
                                else
                                    Attachment += "," + Name;
                            }
                        }

                        FleetHistory.Attachments = Attachment;
                        FleetHistory.FleetStatus = FleetHistoryDetail.FleetStatus;
                        FleetHistory.IsfleetCustomerSite = FleetHistoryDetail.IsfleetCustomerSite;
                        FleetHistory.Reason = FleetHistoryDetail.Reason;
                        FleetHistory.CreatedDate = FleetHistoryDetail.CreatedDate;
                        FleetHistory.NoteForDrive = FleetHistoryDetail.BookingFleet.NotesForDrive;
                        FleetHistory.Reason = FleetHistoryDetail.Reason;

                        //set fleetHistory values in list.
                        FleetHistorys.Add(FleetHistory);
                    }

                    CustomerDetail.FleetHistory = FleetHistorys;

                    //return user service for get organasation.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Fleet History List", Succeeded = true, DataObject = CustomerDetail, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }

                if (FleetHistoryCollection.Count == 0 || FleetHistoryCollection == null)
                {
                    var BookingDetail = BookingService.GetBookingByBookingId(BookingId);

                    dynamic WorkAllocationObj = new ExpandoObject();
                    WorkAllocationObj.CustomerName = BookingDetail.Customer.CustomerName;
                    WorkAllocationObj.RegistrationDescription = BookingDetail.BookingFleets.FirstOrDefault().FleetsRegistration != null ? BookingDetail.BookingFleets.FirstOrDefault().FleetsRegistration.Registration + " " + BookingDetail.BookingFleets.FirstOrDefault().FleetType.Description : "";
                    WorkAllocationObj.BookingFleetStartDate = BookingDetail.FleetBookingDateTime;
                    WorkAllocationObj.BookingFleetEndDate = BookingDetail.EndDate;
                    WorkAllocationObj.BookingNumber = BookingDetail.BookingNumber;
                    WorkAllocationObj.BookingId = BookingDetail.BookingId;

                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Fleet History List", Succeeded = true, DataObject = WorkAllocationObj, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        #endregion

        #region Docket Api's

        /// <summary>
        /// this method is for getting all booked fleet detail
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBookedFleetRegistration")]
        public HttpResponseMessage GetBookedFleetRegistration()
        {
            try
            {
                var StatusValue = 3;
                // status text correponding to value setted in enum
                var EnumText = ETaskStatusHelper.GetEnumValue<ELookUpGroup>(StatusValue).ToString();

                //get statuslookup Id  by title
                var StatusLookupId = StatusLookupService.GetStatusByTitle(EnumText).StatusLookupId;

                //get bookingFleet list  by BookingId
                var BookingFleetCollection = BookingFleetService.GetAllBookingFleetByStatusLookupId(StatusLookupId);

                //check object
                if (BookingFleetCollection.Count > 0 && BookingFleetCollection != null)
                {
                    //dynamic list.
                    dynamic BookingFleets = new List<ExpandoObject>();

                    //return response from bookingFleet service.
                    foreach (var BookingFleetDetail in BookingFleetCollection)
                    {
                        // get fleetRegistration details by FleetRegistrationId
                        //var FleetDetail = FleetsRegistrationService.GetRegisterFleetsByFleetRegistrationId(FleetRegistrationId.Value);

                        //bind dynamic property.
                        dynamic BookingFleet = new ExpandoObject();

                        //map ids
                        BookingFleet.FleetRegistrationId = BookingFleetDetail.FleetRegistrationId;
                        BookingFleet.Registration = BookingFleetDetail.FleetsRegistration.Registration;
                        BookingFleet.BookingFleetId = BookingFleetDetail.BookingFleetId;

                        //set BookingFleet values in list.
                        BookingFleets.Add(BookingFleet);
                    }

                    //return user service for get organasation.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Booked Fleet List", Succeeded = true, DataObject = new ExpandoObject(), DataList = BookingFleets, ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// this method is for getting all docket by BookingId
        /// </summary>
        /// <param name="BookingId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllDocketByBookingFleetId/{BookingFleetId}")]
        public HttpResponseMessage GetAllDocketByBookingFleetId(Guid BookingFleetId)
        {
            try
            {
                //get docket list  by BookingId
                var DocketCollection = DocketService.GetAllDocketByBookingId(BookingFleetId);

                //check object
                if (DocketCollection.Count > 0 && DocketCollection != null)
                {
                    //dynamic list.
                    dynamic Dockets = new List<ExpandoObject>();

                    //return response from docket service.
                    foreach (var DocketDetail in DocketCollection)
                    {
                        //bind dynamic property.
                        dynamic Docket = new ExpandoObject();

                        //map ids
                        Docket.DocketId = DocketDetail.DocketId;
                        Docket.BookingFleetId = DocketDetail.BookingFleetId;
                        Docket.SiteName = DocketDetail.FleetRegistrationId;
                        Docket.DocumentId = DocketDetail.DocumentId;
                        Docket.DocketNo = DocketDetail.DocketNo;
                        Docket.CallingDateTime = DocketDetail.StartTime;
                        Docket.EndTime = DocketDetail.EndTime;
                        Docket.StartKMs = DocketDetail.StartKMs;
                        Docket.FinishKMsA = DocketDetail.FinishKMsA;
                        Docket.LunchBreak1From = DocketDetail.LunchBreak1From;
                        Docket.LunchBreak1End = DocketDetail.LunchBreak1End;
                        Docket.LunchBreak2From = DocketDetail.LunchBreak2From;
                        Docket.LunchBreak2End = DocketDetail.LunchBreak2End;
                        Docket.LunchBreak1From = DocketDetail.LunchBreak1From;
                        Docket.AttachmentIds = DocketDetail.AttachmentIds;
                        Docket.DocketCheckListId = DocketDetail.DocketCheckListId;
                        Docket.IsActive = DocketDetail.IsActive;
                        Docket.CreatedDate = DocketDetail.CreatedDate;
                        //bind dynamic list property.
                        dynamic LoadDocketList = new List<ExpandoObject>();

                        //getting LoadDockets detail 
                        if (DocketDetail.LoadDockets != null)
                        {
                            foreach (var LoadDocketsDetail in DocketDetail.LoadDockets)
                            {
                                //bind dynamic property.
                                dynamic LoadDocketObject = new ExpandoObject();
                                //bind child object.
                                LoadDocketObject.DocketLoadtId = LoadDocketsDetail.DocketLoadtId;
                                LoadDocketObject.DocketId = LoadDocketsDetail.DocketId;
                                LoadDocketObject.LoadingSite = LoadDocketsDetail.LoadingSite;
                                LoadDocketObject.Weight = LoadDocketsDetail.Weight;
                                LoadDocketObject.LoadTime = LoadDocketsDetail.LoadTime;
                                LoadDocketObject.TipOffSite = LoadDocketsDetail.TipOffSite;
                                LoadDocketObject.TipOffTime = LoadDocketsDetail.TipOffTime;
                                LoadDocketObject.Material = LoadDocketsDetail.Material;

                                //set LoadDocketList detail in list.
                                LoadDocketList.Add(LoadDocketObject);
                            }
                        }

                        Docket.LoadDocket = LoadDocketList;
                        Docket.Image = DocketDetail.Document.Name;
                        Docket.FleetBookingDateTime = DocketDetail.BookingFleet.FleetBookingDateTime;
                        Docket.Fleet = DocketDetail.BookingFleet.FleetType.Fleet;
                        Docket.Driver = DocketDetail.BookingFleet.Driver != null ? DocketDetail.BookingFleet.Driver.FirstName + " " + DocketDetail.BookingFleet.Driver.LastName : "";
                        Docket.IsDayShift = DocketDetail.BookingFleet.IsDayShift;

                        //set docket values in list.
                        Dockets.Add(Docket);
                    }

                    //return user service for get organasation.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Docket List", Succeeded = true, DataObject = new ExpandoObject(), DataList = Dockets, ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// this method is used for getting docket detail
        /// </summary>
        /// <param name="DocketId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDocketDetail/{DocketId}")]
        public HttpResponseMessage GetDocketDetail(Guid DocketId)
        {
            try
            {
                //check is valid BookingFleetId.
                if (!ServiceHelper.IsGuid((string)DocketId.ToString()))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid booking fleet id." });
                }

                //getting Docket detail by docketId
                var DocketDetail = DocketService.GetDocketByDocketId(DocketId);

                //check service response.
                if (DocketDetail != null)
                {
                    //bind dynamic property.
                    dynamic Docket = new ExpandoObject();
                    Docket.DocketId = DocketDetail.DocketId;
                    Docket.BookingFleetId = DocketDetail.BookingFleetId;
                    Docket.FleetRegistrationId = DocketDetail.FleetRegistrationId;
                    Docket.Registration = DocketDetail.FleetsRegistration.Registration;
                    Docket.DocumentId = DocketDetail.DocumentId;
                    Docket.DocketNo = DocketDetail.DocketNo;
                    Docket.StartTime = DocketDetail.StartTime;
                    Docket.EndTime = DocketDetail.EndTime;
                    Docket.StartKMs = DocketDetail.StartKMs;
                    Docket.FinishKMsA = DocketDetail.FinishKMsA;
                    Docket.LunchBreak1From = DocketDetail.LunchBreak1From;
                    Docket.LunchBreak1End = DocketDetail.LunchBreak1End;
                    Docket.LunchBreak2From = DocketDetail.LunchBreak2From;
                    Docket.LunchBreak2End = DocketDetail.LunchBreak2End;
                    Docket.AttachmentIds = DocketDetail.AttachmentIds;
                    Docket.DocketCheckListId = DocketDetail.DocketCheckListId;
                    Docket.IsActive = DocketDetail.IsActive;
                    Docket.CreatedDate = DocketDetail.CreatedDate;

                    //bind dynamic list property.
                    dynamic LoadDocketList = new List<ExpandoObject>();

                    //getting LoadDockets detail 
                    if (DocketDetail.LoadDockets != null)
                    {
                        foreach (var LoadDocketsDetail in DocketDetail.LoadDockets)
                        {
                            //bind dynamic property.
                            dynamic LoadDocketObject = new ExpandoObject();
                            //bind child object.
                            LoadDocketObject.DocketLoadtId = LoadDocketsDetail.DocketLoadtId;
                            LoadDocketObject.DocketId = LoadDocketsDetail.DocketId;
                            LoadDocketObject.LoadingSite = LoadDocketsDetail.LoadingSite;
                            LoadDocketObject.Weight = LoadDocketsDetail.Weight;
                            LoadDocketObject.LoadTime = LoadDocketsDetail.LoadTime;
                            LoadDocketObject.TipOffSite = LoadDocketsDetail.TipOffSite;
                            LoadDocketObject.TipOffTime = LoadDocketsDetail.TipOffTime;
                            LoadDocketObject.Material = LoadDocketsDetail.Material;

                            //set LoadDocketList detail in list.
                            LoadDocketList.Add(LoadDocketObject);
                        }
                    }
                    Docket.LoadDocket = LoadDocketList;
                    Docket.Image = DocketDetail.Document.Name;
                    Docket.CustomerName = DocketDetail.BookingFleet.Booking.Customer.CustomerName;
                    Docket.SiteName = DocketDetail.BookingFleet.Booking.Site.SiteName;
                    Docket.SupervisorName = DocketDetail.Supervisor.SupervisorName;
                    Docket.MobileNumber = DocketDetail.Supervisor.MobileNumber;
                    Docket.BookingNumber = DocketDetail.BookingFleet.Booking.BookingNumber;
                    Docket.DriverName = DocketDetail.BookingFleet.Driver.FirstName + " " + DocketDetail.BookingFleet.Driver.LastName;
                    Docket.IsDayShift = DocketDetail.BookingFleet.IsDayShift;
                    Docket.Iswethire = DocketDetail.BookingFleet.Iswethire;
                    Docket.SupervisorId = DocketDetail.SupervisorId;
                    Docket.DocketDate = DocketDetail.DocketDate.ToString("dd-MM-yyyy hh:mm tt");
                    Docket.BorgCivilPlantNumber = DocketDetail.BookingFleet.FleetsRegistration.BorgCivilPlantNumber;


                    //call skill service for get skill.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Docket details", Succeeded = true, DataObject = Docket, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    //return result from service response.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data not found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

            }
        }

        /// <summary>
        /// This method use for add docket into docket entity.
        /// </summary>
        /// <param name="DocketDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddDocket")]
        public HttpResponseMessage AddDocket(DocketDataModel DocketDataModel)
        {
            try
            {
                // defining variables globally
                var LoadDocketResponse = false;
                DocumentDataModel DocumentData = new DocumentDataModel();
                var DocumentId = (Guid?)null;

                //add docket detail into database
                var DocketId = DocketService.AddDocket(DocketDataModel);

                /// checking docket id not equal to null
                if (ServiceHelper.IsGuid(DocketId))
                {
                    #region Save data in document entity.

                    if (DocketDataModel.ImageBase64 != null)
                    {
                        String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                        String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

                        //bind file data in a document model.
                        DocumentData.Id = Guid.NewGuid().ToString();
                        DocumentData.OriginalName = "Image";
                        DocumentData.Name = DocketId + "_DocketImage_" + ".png";
                        DocumentData.Title = "Images";
                        DocumentData.Description = string.Empty;
                        DocumentData.Tags = string.Empty;
                        DocumentData.URL = strUrl + "/Uploads/" + DocketId + "_DocketImage_" + ".png";//provider.FileData.FirstOrDefault().LocalFileName;//
                        DocumentData.Extension = ".png";
                        DocumentData.ThumbnailFileName = "";
                        DocumentData.FileSize = 0;
                        DocumentData.Private = true;

                        //save document data.
                        DocumentId = DocumentService.AddDocument(
                            new Guid(DocumentData.Id),
                            DocumentData.OriginalName,
                            DocumentData.Name,
                            DocumentData.URL,
                            DocumentData.Title,
                            DocumentData.Description,
                            DocumentData.Extension,
                            DocumentData.FileSize,
                            DocumentData.Private,
                            DocumentData.Tags,
                            DocumentData.ThumbnailFileName
                            );
                        if (ServiceHelper.IsGuid(DocumentId.ToString()))
                        {
                            // updating documentId in docket table
                            var UpdateDocument = DocketService.UpdateDocumentId(new Guid(DocketId), DocumentId.Value);

                            // uploading image on Uploads folder
                            var ImageUploadStatus = UploadImage.base64ToImage(DocketDataModel.ImageBase64, DocketId + "_DocketImage_" + ".png");

                            // foreach for adding multiple loadDocket in table
                            foreach (var Item in DocketDataModel.LoadDocketDataModel)
                            {
                                Item.DocketId = new Guid(DocketId);

                                // adding items in loadDocket table
                                LoadDocketResponse = LoadDocketService.AddLoadDocket(Item);
                            }
                            if (LoadDocketResponse)
                            {
                                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Docket successfully added", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                            }
                        }
                        else
                        {
                            //return result from service response.
                            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Document not added successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                        }
                    }
                    #endregion

                }
                //return service for get user.
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Record not inserted successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
            }
            catch (Exception ex)
            {
                // handel exception log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// This method use for update record into docket entity.
        /// </summary>
        /// <param name="DocketDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateDocket")]
        public HttpResponseMessage UpdateDocket(DocketDataModel DocketDataModel)
        {
            try
            {
                // defining variables globally
                DocumentDataModel DocumentData = new DocumentDataModel();
                var DocumentId = (Guid?)null;
                var LoadDocketResponse = false;

                // calling update status docket method
                var DocketId = DocketService.UpdateDocket(DocketDataModel);

                /// checking docket id not equal to null
                if (ServiceHelper.IsGuid(DocketId))
                {
                    #region Update data in document entity.

                    if (DocketDataModel.ImageBase64 != null)
                    {
                        // uploading image on Uploads folder
                        var ImageUploadStatus = UploadImage.base64ToImage(DocketDataModel.ImageBase64, DocketId + "_DocketImage_" + ".png");
                    }
                    #endregion

                    if (ServiceHelper.IsGuid(DocketId.ToString()))
                    {
                        // deleting existing docket and adding new loadDocket
                        var DeletDocket = LoadDocketService.DeleteLoadDocket(new Guid(DocketId));

                        // foreach for adding multiple loadDocket in table
                        foreach (var Item in DocketDataModel.LoadDocketDataModel)
                        {
                            Item.DocketId = new Guid(DocketId);

                            // adding items in loadDocket table
                            LoadDocketResponse = LoadDocketService.AddLoadDocket(Item);
                        }
                        if (LoadDocketResponse)
                        {
                            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Docket successfully added", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                        }
                    }
                    else
                    {
                        //return result from service response.
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Docket not added successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    }

                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// this method is for getting all docketCheckList
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllDocketCheckboxList")]
        public HttpResponseMessage GetAllDocketCheckboxList()
        {
            try
            {
                //get docketCheckbox list
                var DocketCheckboxCollection = DocketCheckboxListService.GetAllDocketCheckList();

                //check object
                if (DocketCheckboxCollection.Count > 0 && DocketCheckboxCollection != null)
                {
                    //dynamic list.
                    dynamic DocketCheckboxs = new List<ExpandoObject>();

                    //return response from docket service.
                    foreach (var DocketCheckboxDetail in DocketCheckboxCollection)
                    {
                        //bind dynamic property.
                        dynamic DocketCheckbox = new ExpandoObject();

                        //map ids
                        DocketCheckbox.DocketCheckListId = DocketCheckboxDetail.DocketCheckListId;
                        DocketCheckbox.Title = DocketCheckboxDetail.Title;

                        //set DocketCheckbox values in list.
                        DocketCheckboxs.Add(DocketCheckbox);
                    }

                    //return DocketCheckbox service for get DocketCheckbox list
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "DocketCheckbox List", Succeeded = true, DataObject = new ExpandoObject(), DataList = DocketCheckboxs, ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        #endregion

        #region SentEmail Api

        /// <summary>
        /// This method use for sent email
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("EmailSent")]
        public HttpResponseMessage EmailSent()
        {
            try
            {

                //step:7
                //add email data
                //var sendEmail = emailService.AddEmailData(user.Id, user.Email, string.Empty, string.Empty, string.Empty, EmailTemplatesHelper.UserCreateEmail, replacements);

                ////step:8
                ////send email.
                //if (sendEmail != null)
                //{
                //    emailService.SendEmailAsync(sendEmail.Id, user.Email, string.Empty, string.Empty, string.Empty, EmailTemplatesHelper.UserCreateEmail, replacements);
                //}


                //return service for get user.
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Record not inserted successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
            }
            catch (Exception ex)
            {
                // handel exception log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        #endregion

        //#region WriteExcel

        ///// <summary>
        ///// Export selected data into excel sheet.
        ///// </summary>
        ///// <returns></returns>
        //public HttpResponseMessage ExportToExcel()
        //{
        //    //var lang = SiteLanguages.GetDefaultLanguage();
        //    var grid = new GridView();
        //    var modal = (List<CustomerProductDataModel>)TempData.Peek("TempMyProductList");

        //    if (modal == null)
        //    {
        //        grid.EmptyDataText = "No data available";
        //    }
        //    else
        //    {

        //        grid.DataSource = from p in modal
        //                          select new
        //                          {
        //                              InvestmentName = p.Name,
        //                              Investment = p.Investment,
        //                              TotalWeeklyEarning = p.CurrentWeekTotalEarning,
        //                              Status = p.Status,
        //                              Date = p.CreatedDate,
        //                              InvestmentWithdrawDate = p.InvestmentWithdrawDate
        //                          };


        //    }
        //    grid.DataBind();


        //    Response.ClearContent();
        //    Response.Buffer = true;
        //    Response.AddHeader("content-disposition", "attachment; filename=My_Investment.xls");
        //    Response.ContentType = "application/ms-excel";

        //    Response.Charset = "";
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter htw = new HtmlTextWriter(sw);

        //    grid.RenderControl(htw);

        //    Response.Output.Write(sw.ToString());
        //    Response.Flush();
        //    Response.End();
        //    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Fleet History List", Succeeded = true, DataObject = CustomerDetail, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
        //}

        //#endregion


    }
}
