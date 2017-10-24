using System.Web.Http;
using BorgCivil.Service;
using System.Net.Http;
using System.Dynamic;
using System.Net;
using System;
using BorgCivil.Utils.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Cors;
using BorgCivil.Utils.Enum;
using System.Web;
using BorgCivil.Utils;
using System.IO;
using System.Globalization;
using BorgCivil.MVCWeb.Providers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BorgCivil.MVCWeb.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Fleet")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FleetApiController : BaseController
    {
        //Initialized interface object. 
        #region Dependencies Injection with initialization
        IAttachmentsService AttachmentsService;
        IFleetTypesService FleetTypesService;
        IFleetsRegistrationService FleetRegistrationService;
        IDriversService DriversService;
        IBookingFleetService BookingFleetService;
        IStatusLookupService StatusLookupService;
        IDocumentService DocumentService;
        IDriverWhiteCardService DriverWhiteCardService;
        IDriverInductionCardService DriverInductionCardService;
        IDriverVocCardService DriverVocCardService;
        IAnonymousFieldService AnonymousFieldService;
        IEmploymentCategoryService EmploymentCategoryService;
        ILicenseClassService LicenseClassService;
        ISubcontractorService SubcontractorService;

        // Constructor of Fleet Api Controller 
        public FleetApiController(IFleetTypesService _FleetTypesService, IFleetsRegistrationService _FleetRegistrationService, IDriversService _DriversService, IAttachmentsService _AttachmentsService, IBookingFleetService _BookingFleetService, IStatusLookupService _StatusLookupService, IDocumentService _DocumentService, IDriverWhiteCardService _DriverWhiteCardService, IDriverInductionCardService _DriverInductionCardService, IDriverVocCardService _DriverVocCardService, IAnonymousFieldService _AnonymousFieldService, IEmploymentCategoryService _EmploymentCategoryService, ILicenseClassService _LicenseClassService, ISubcontractorService _SubcontractorService)
        {
            AttachmentsService = _AttachmentsService;
            FleetTypesService = _FleetTypesService;
            FleetRegistrationService = _FleetRegistrationService;
            DriversService = _DriversService;
            BookingFleetService = _BookingFleetService;
            StatusLookupService = _StatusLookupService;
            DocumentService = _DocumentService;
            DriverWhiteCardService = _DriverWhiteCardService;
            DriverInductionCardService = _DriverInductionCardService;
            DriverVocCardService = _DriverVocCardService;
            AnonymousFieldService = _AnonymousFieldService;
            EmploymentCategoryService = _EmploymentCategoryService;
            LicenseClassService = _LicenseClassService;
            SubcontractorService = _SubcontractorService;
        }
        #endregion

        #region Attachments Api's
        /// <summary>
        /// method to get all attachments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllAttachment")]
        public HttpResponseMessage GetAllAttachment()
        {
            try
            {
                //get attachment list  by customerId
                var AttachmentCollection = AttachmentsService.GetAllAttachment().ToList();

                //check object
                if (AttachmentCollection.Count > 0 && AttachmentCollection != null)
                {
                    //dynamic list.
                    dynamic Attachment = new ExpandoObject();

                    var Attachemet = AttachmentCollection.Where(x => x.IsAttachment == true);

                    dynamic AttachmentList = new List<ExpandoObject>();

                    //return response from Attachment by filter
                    foreach (var AttachmentDetail in Attachemet)
                    {
                        //bind dynamic property.
                        dynamic AttachmentDetailList = new ExpandoObject();

                        //map ids
                        AttachmentDetailList.AttachmentId = AttachmentDetail.AttachmentId;
                        AttachmentDetailList.AttachmentTitle = AttachmentDetail.AttachmentTitle;

                        //set AttachmentDetailList values in list.
                        AttachmentList.Add(AttachmentDetailList);
                    }

                    var Combohire = AttachmentCollection.Where(x => x.IsAttachment == false);

                    dynamic ComboList = new List<ExpandoObject>();

                    //return response from Attachemnt by filter.
                    foreach (var CombohireDetail in Combohire)
                    {
                        //bind dynamic property.
                        dynamic CombohireDetailList = new ExpandoObject();

                        //map ids
                        CombohireDetailList.AttachmentId = CombohireDetail.AttachmentId;
                        CombohireDetailList.AttachmentTitle = CombohireDetail.AttachmentTitle;

                        //set CombohireDetailList values in list.
                        ComboList.Add(CombohireDetailList);
                    }

                    //set booking values in list.
                    Attachment.Attachments = AttachmentList;
                    Attachment.ComboHire = ComboList;

                    //return attachment response
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Attachment List", Succeeded = true, DataObject = Attachment, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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

        #region FleetTypes Api's

        /// <summary>
        /// getting all fleet types select list for dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFleetTypes")]
        public HttpResponseMessage GetFleetTypes()
        {
            try
            {
                //get fleet type list
                var FleetTypes = FleetTypesService.GetFleetTypesList();

                //check object
                if (FleetTypes.Count > 0 && FleetTypes != null)
                {
                    List<ExpandoObject> FleetTypeslist = new List<ExpandoObject>();
                    foreach (var FleetType in FleetTypes)
                    {
                        dynamic FleetTypeDetail = new ExpandoObject();
                        FleetTypeDetail.FleetTypeName = FleetType.Text;
                        FleetTypeDetail.FleetTypeId = FleetType.Value;
                        FleetTypeslist.Add(FleetTypeDetail);
                    }
                    //return customer service for get customer type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Fleet List", Succeeded = true, DataObject = new ExpandoObject(), DataList = FleetTypeslist, ErrorInfo = "" });
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
        /// this method is for getting all FleetType 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllFleetType")]
        public HttpResponseMessage GetAllFleetType()
        {
            try
            {
                //get fleet type list 
                var FleetTypeCollection = FleetTypesService.GetAllFleetTypes().OrderByDescending(x => x.CreatedDate).ToList();

                //check object
                if (FleetTypeCollection.Count > 0 && FleetTypeCollection != null)
                {
                    //dynamic list.
                    dynamic FleetTypes = new List<ExpandoObject>();

                    //return response from fleetType service.
                    foreach (var FleetTypeDetail in FleetTypeCollection)
                    {
                        //bind dynamic property.
                        dynamic FleetType = new ExpandoObject();

                        //map ids
                        FleetType.FleetTypeId = FleetTypeDetail.FleetTypeId;
                        FleetType.Fleet = FleetTypeDetail.Fleet;
                        FleetType.Description = FleetTypeDetail.Description;
                        FleetType.CreatedDate = FleetTypeDetail.CreatedDate;
                        FleetType.IsActive = FleetTypeDetail.IsActive;
                        FleetType.Image = FleetTypeDetail.Documents != null ? FleetTypeDetail.Documents.Name : "";

                        //set FleetType values in list.
                        FleetTypes.Add(FleetType);
                    }

                    //return user service for get organasation.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "FleetType List", Succeeded = true, DataObject = new ExpandoObject(), DataList = FleetTypes, ErrorInfo = "" });
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
        /// this method is used for getting FleetType detail
        /// </summary>
        /// <param name="FleetTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFleetTypeDetail/{FleetTypeId}")]
        public HttpResponseMessage GetFleetTypeDetail(Guid FleetTypeId)
        {
            try
            {
                //check is valid FleetTypeId.
                if (!ServiceHelper.IsGuid((string)FleetTypeId.ToString()))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid booking fleet id." });
                }

                //getting FleetType detail by FleetTypeId
                var FleetTypeDetail = FleetTypesService.GetFleetTypesByFleetTypeId(FleetTypeId);

                //check service response.
                if (FleetTypeDetail != null)
                {
                    //bind dynamic property.
                    dynamic FleetType = new ExpandoObject();
                    FleetType.FleetTypeId = FleetTypeDetail.FleetTypeId;
                    FleetType.Fleet = FleetTypeDetail.Fleet;
                    FleetType.Description = FleetTypeDetail.Description;
                    FleetType.CreatedDate = FleetTypeDetail.CreatedDate;
                    FleetType.IsActive = FleetTypeDetail.IsActive;
                    FleetType.Image = FleetTypeDetail.Documents != null ? FleetTypeDetail.Documents.Name : "";

                    //call skill service for get skill.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "FleetType details", Succeeded = true, DataObject = FleetType, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// This method used for add record into fleetType entity.
        /// </summary>
        /// <param name="FleetTypeDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddFleetType")]
        public HttpResponseMessage AddFleetType(FleetTypeDataModel FleetTypeDataModel)
        {
            try
            {
                // defining variables globally
                var DocumentId = (Guid?)null;
                DocumentDataModel DocumentData = new DocumentDataModel();

                // checking add Fleet response
                var FleetTypeId = FleetTypesService.AddFleetType(FleetTypeDataModel);
                if (ServiceHelper.IsGuid(FleetTypeId))
                {
                    if (FleetTypeDataModel.ImageBase64 != null)
                    {
                        String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                        String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

                        //bind file data in a document model.
                        DocumentData.Id = Guid.NewGuid().ToString();
                        DocumentData.OriginalName = "Image";
                        DocumentData.Name = FleetTypeId + "_FleetTypeImage_" + ".png";
                        DocumentData.Title = "Images";
                        DocumentData.Description = string.Empty;
                        DocumentData.Tags = string.Empty;
                        DocumentData.URL = strUrl + "/Uploads/" + FleetTypeId + "_FleetTypeImage_" + ".png";//provider.FileData.FirstOrDefault().LocalFileName;//
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

                        // updating documentId in docket table
                        var UpdateDocument = FleetTypesService.UpdateDocumentId(new Guid(FleetTypeId), DocumentId.Value);

                        // uploading image on Uploads folder
                        var ImageUploadStatus = UploadImage.base64ToImage(FleetTypeDataModel.ImageBase64, FleetTypeId + "_FleetTypeImage_" + ".png");
                    }
                }

                //return service for get user.
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Added successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

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
        /// This method use for update record into FleetType entity.
        /// </summary>
        /// <param name="FleetTypeDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateFleetType")]
        public HttpResponseMessage UpdateFleetType(FleetTypeDataModel FleetTypeDataModel)
        {
            try
            {
                // defining variables globally
                DocumentDataModel DocumentData = new DocumentDataModel();
                var DocumentId = (Guid?)null;

                // calling service of update fleetType
                var FleetTypeId = FleetTypesService.UpdateFleetType(FleetTypeDataModel);

                /// return if not succeded
                if (ServiceHelper.IsGuid(FleetTypeId))
                {
                    #region Update data in document entity.

                    var FleetTypeDetail = FleetTypesService.GetFleetTypesByFleetTypeId(new Guid(FleetTypeId));
                    if (FleetTypeDataModel.ImageBase64 != null && FleetTypeDetail.DocumentId != null)
                    {
                        // uploading image on Uploads folder
                        var ImageUploadStatus = UploadImage.base64ToImage(FleetTypeDataModel.ImageBase64, FleetTypeId + "_FleetTypeImage_" + ".png");
                    }
                    // if document is not loaded
                    if (FleetTypeDetail.DocumentId == null && FleetTypeDataModel.ImageBase64 != null)
                    {
                        String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                        String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

                        //bind file data in a document model.
                        DocumentData.Id = Guid.NewGuid().ToString();
                        DocumentData.OriginalName = "Image";
                        DocumentData.Name = FleetTypeId + "_FleetTypeImage_" + ".png";
                        DocumentData.Title = "Images";
                        DocumentData.Description = string.Empty;
                        DocumentData.Tags = string.Empty;
                        DocumentData.URL = strUrl + "/Uploads/" + FleetTypeId + "_FleetTypeImage_" + ".png";//provider.FileData.FirstOrDefault().LocalFileName;//
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

                        // updating documentId in docket table
                        var UpdateDocument = FleetTypesService.UpdateDocumentId(new Guid(FleetTypeId), DocumentId.Value);

                        // uploading image on Uploads folder
                        var ImageUploadStatus = UploadImage.base64ToImage(FleetTypeDataModel.ImageBase64, FleetTypeId + "_FleetTypeImage_" + ".png");
                    }

                    #endregion

                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "FleetType successfully updated", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// this method is used for deleting FleetType
        /// </summary>
        /// <param name="FleetTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteFleetType/{FleetTypeId}")]
        public HttpResponseMessage DeleteFleetType(Guid FleetTypeId)
        {
            try
            {
                //check is valid FleetTypeId.
                if (!ServiceHelper.IsGuid((string)FleetTypeId.ToString()))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid booking fleet id." });
                }

                //getting FleetType detail by FleetTypeId
                var FleetTypeDetail = FleetTypesService.DeleteFleetType(FleetTypeId);

                //check service response.
                if (FleetTypeDetail)
                {
                    //call employee service response
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

        #endregion

        #region FleetRegistration Api's

        /// <summary>
        /// getting all fleet registration select list for dropdown
        /// </summary>
        /// params FleetTypeId
        /// <returns></returns>
        [HttpGet]
        [Route("GetFleetRegistrationsByFleetTypeId/{FleetTypeId}")]
        public HttpResponseMessage GetFleetRegistrationsByFleetTypeId(Guid FleetTypeId)
        {
            try
            {
                // status value for allocated
                var StatusValue = 3;
                // status text correponding to value setted in enum
                var EnumText = ETaskStatusHelper.GetEnumValue<ELookUpGroup>(StatusValue).ToString();

                //get statuslookup Id  by title
                var StatusLookupId = StatusLookupService.GetStatusByTitle(EnumText).StatusLookupId;

                //get fleet registration list
                var RegisterFleet = (from p in FleetRegistrationService.GetFleetRegistrationList(FleetTypeId).ToList()
                                     where !BookingFleetService.GetAllBookingFleet().Where(st => st.StatusLookupId == StatusLookupId).Select(st => st.FleetRegistrationId.ToString()).ToList().Contains(p.Value)
                                     select p).ToList();
                //var RegisterFleet = FleetRegistrationService.GetFleetRegistrationList(FleetTypeId);

                //check object
                if (RegisterFleet.Count > 0 && RegisterFleet != null)
                {
                    List<ExpandoObject> FleetRegistrationlist = new List<ExpandoObject>();
                    foreach (var FleetRegistration in RegisterFleet)
                    {
                        dynamic FleetRegistrationDetail = new ExpandoObject();
                        FleetRegistrationDetail.RegistrationNumber = FleetRegistration.Text;
                        FleetRegistrationDetail.FleetRegistrationId = FleetRegistration.Value;
                        FleetRegistrationlist.Add(FleetRegistrationDetail);
                    }
                    //return customer service for get customer type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Fleet Registration List", Succeeded = true, DataObject = new ExpandoObject(), DataList = FleetRegistrationlist, ErrorInfo = "" });
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
        /// this method is for getting all FleetRegistration 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllFleetRegistration")]
        public HttpResponseMessage GetAllFleetRegistration()
        {
            try
            {
                var ImageName = string.Empty;
                //get FleetRegistration list 
                var FleetRegistrationCollection = FleetRegistrationService.GetAllFleetRegistration().OrderByDescending(x => x.CreatedDate).ToList();

                //check object
                if (FleetRegistrationCollection.Count > 0 && FleetRegistrationCollection != null)
                {
                    //dynamic list.
                    dynamic FleetRegistrations = new List<ExpandoObject>();

                    //return response from FleetRegistration service.
                    foreach (var FleetRegistrationDetail in FleetRegistrationCollection)
                    {
                        //bind dynamic property.
                        dynamic FleetRegistration = new ExpandoObject();

                        //map ids
                        FleetRegistration.FleetRegistrationId = FleetRegistrationDetail.FleetRegistrationId;
                        FleetRegistration.FleetTypeId = FleetRegistrationDetail.FleetTypeId;
                        FleetRegistration.Fleet = FleetRegistrationDetail.FleetType.Fleet;
                        FleetRegistration.MakeModelYear = FleetRegistrationDetail.Make + "/" + FleetRegistrationDetail.Model + "/" + FleetRegistrationDetail.Year;
                        FleetRegistration.VinEngineNumber = FleetRegistrationDetail.VINNumber + "/" + FleetRegistrationDetail.EngineNumber;
                        FleetRegistration.Make = FleetRegistrationDetail.Make;
                        FleetRegistration.Model = FleetRegistrationDetail.Model;
                        FleetRegistration.Capacity = FleetRegistrationDetail.Capacity;
                        FleetRegistration.Year = FleetRegistrationDetail.Year;
                        FleetRegistration.Registration = FleetRegistrationDetail.Registration;
                        FleetRegistration.BorgCivilPlantNumber = FleetRegistrationDetail.BorgCivilPlantNumber;
                        FleetRegistration.VINNumber = FleetRegistrationDetail.VINNumber;
                        FleetRegistration.EngineNumber = FleetRegistrationDetail.EngineNumber;
                        FleetRegistration.InsuranceDate = FleetRegistrationDetail.InsuranceDate;
                        FleetRegistration.CurrentMeterReading = FleetRegistrationDetail.CurrentMeterReading;
                        FleetRegistration.LastServiceMeterReading = FleetRegistrationDetail.LastServiceMeterReading;
                        FleetRegistration.ServiceInterval = FleetRegistrationDetail.ServiceInterval;
                        FleetRegistration.HVISType = FleetRegistrationDetail.HVISType;
                        FleetRegistration.IsActive = FleetRegistrationDetail.IsActive;
                        FleetRegistration.CreatedDate = FleetRegistrationDetail.CreatedDate;
                        FleetRegistration.EditedDate = FleetRegistrationDetail.EditedDate;
                        // split document Id to send image name
                        if (FleetRegistrationDetail.DocumentId != null)
                        {
                            string[] SplitDocument = FleetRegistrationDetail.DocumentId.Split(',');
                            foreach (var item in SplitDocument)
                            {
                                var Document = DocumentService.GetDocument(new Guid(item));

                                // creating comma seprated string of image name 
                                if (ImageName == "")
                                    ImageName = Document.Name;
                                else
                                    ImageName = ImageName + "," + Document.Name;
                            }
                        }
                        FleetRegistration.Image = ImageName;

                        ////set Driver values in list.
                        FleetRegistrations.Add(FleetRegistration);
                    }

                    //return user service for get organasation.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "FleetRegistration List", Succeeded = true, DataObject = new ExpandoObject(), DataList = FleetRegistrations, ErrorInfo = "" });
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
        /// this method is used for getting FleetRegistration detail
        /// </summary>
        /// <param name="FleetRegistrationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFleetRegistrationDetail/{FleetRegistrationId}")]
        public HttpResponseMessage GetFleetRegistrationDetail(Guid FleetRegistrationId)
        {
            try
            {
                var ImageName = string.Empty;

                //check is valid FleetRegistrationId.
                if (!ServiceHelper.IsGuid((string)FleetRegistrationId.ToString()))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid FleetRegistrationId" });
                }

                //getting FleetRegistration detail by FleetRegistrationId
                var FleetRegistrationDetail = FleetRegistrationService.GetRegisterFleetsByFleetRegistrationId(FleetRegistrationId);

                //check service response.
                if (FleetRegistrationDetail != null)
                {
                    //bind dynamic property.
                    dynamic FleetRegistration = new ExpandoObject();
                    FleetRegistration.FleetRegistrationId = FleetRegistrationDetail.FleetRegistrationId;
                    FleetRegistration.FleetTypeId = FleetRegistrationDetail.FleetTypeId;
                    FleetRegistration.Make = FleetRegistrationDetail.Make;
                    FleetRegistration.Model = FleetRegistrationDetail.Model;
                    FleetRegistration.Capacity = FleetRegistrationDetail.Capacity;
                    FleetRegistration.Year = FleetRegistrationDetail.Year;
                    FleetRegistration.Registration = FleetRegistrationDetail.Registration;
                    FleetRegistration.BorgCivilPlantNumber = FleetRegistrationDetail.BorgCivilPlantNumber;
                    FleetRegistration.VINNumber = FleetRegistrationDetail.VINNumber;
                    FleetRegistration.EngineNumber = FleetRegistrationDetail.EngineNumber;
                    FleetRegistration.InsuranceDate = FleetRegistrationDetail.InsuranceDate;
                    FleetRegistration.CurrentMeterReading = FleetRegistrationDetail.CurrentMeterReading;
                    FleetRegistration.LastServiceMeterReading = FleetRegistrationDetail.LastServiceMeterReading;
                    FleetRegistration.ServiceInterval = FleetRegistrationDetail.ServiceInterval;
                    FleetRegistration.HVISType = FleetRegistrationDetail.HVISType;
                    FleetRegistration.AttachmentId = FleetRegistrationDetail.AttachmentId;
                    FleetRegistration.IsActive = FleetRegistrationDetail.IsActive;
                    FleetRegistration.CreatedDate = FleetRegistrationDetail.CreatedDate;
                    FleetRegistration.UnavailableFromDate = FleetRegistrationDetail.UnavailableFromDate;
                    FleetRegistration.UnavailableToDate = FleetRegistrationDetail.UnavailableToDate;
                    FleetRegistration.UnavailableNote = FleetRegistrationDetail.UnavailableNote;

                    // split document Id to send image name
                    string[] SplitDocument = FleetRegistrationDetail.DocumentId.Split(',');
                    foreach (var item in SplitDocument)
                    {
                        var Document = DocumentService.GetDocument(new Guid(item));

                        // creating comma seprated string of image name 
                        if (ImageName == "")
                            ImageName = Document.Name;
                        else
                            ImageName = ImageName + "," + Document.Name;
                    }
                    FleetRegistration.Image = ImageName;

                    //fleetRegistration service response
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Get FleetRegistration Details", Succeeded = true, DataObject = FleetRegistration, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// This method use for add FleetRegistration into FleetRegistration entity.
        /// </summary>
        /// <param name="FleetRegistrationDataModel"></param>
        /// <returns></returns>s
        [HttpPost]
        [Route("AddFleetRegistration")]
        public HttpResponseMessage AddFleetRegistration()
        {
            try
            {
                // defining variables globally
                FleetRegistrationDataModel FleetRegistrationDataModel = new FleetRegistrationDataModel();
                DocumentDataModel DocumentData = new DocumentDataModel();
                var DocumentId = (Guid?)null;
                var SuccessResponse = false;

                // Define the path where we want to save the files.
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                var Form = System.Web.HttpContext.Current.Request.Form;

                // setting form data into entity
                FleetRegistrationDataModel.FleetTypeId = new Guid(Form["model[FleetTypeId]"]);
                FleetRegistrationDataModel.SubcontractorId = Form["model[SubcontractorId]"];
                FleetRegistrationDataModel.Make = Form["model[Make]"];
                FleetRegistrationDataModel.Model = Form["model[Model]"];
                FleetRegistrationDataModel.Capacity = Form["model[Capacity]"];
                FleetRegistrationDataModel.Year = Form["model[Year]"];
                FleetRegistrationDataModel.Registration = Form["model[Registration]"];
                FleetRegistrationDataModel.BorgCivilPlantNumber = Form["model[BorgCivilPlantNumber]"];
                FleetRegistrationDataModel.VINNumber = Form["model[VINNumber]"];
                FleetRegistrationDataModel.EngineNumber = Form["model[EngineNumber]"];
                FleetRegistrationDataModel.InsuranceDate = Form["model[InsuranceDate]"];
                FleetRegistrationDataModel.CurrentMeterReading = Form["model[CurrentMeterReading]"];
                FleetRegistrationDataModel.LastServiceMeterReading = Form["model[LastServiceMeterReading]"];
                FleetRegistrationDataModel.HVISType = Form["model[HVISType]"];
                FleetRegistrationDataModel.ServiceInterval = Form["model[ServiceInterval]"];
                FleetRegistrationDataModel.AttachmentId = Form["model[AttachmentId]"];
                FleetRegistrationDataModel.IsBooked = Convert.ToBoolean(Form["model[IsBooked]"]);
                FleetRegistrationDataModel.IsActive = Convert.ToBoolean(Form["model[IsActive]"]);
                FleetRegistrationDataModel.CreatedBy = Form["model[CreatedBy]"] != "" ? new Guid(Form["model[CreatedBy]"]) : (Guid?)null;
                FleetRegistrationDataModel.EditedBy = Form["model[EditedBy]"] != "" ? new Guid(Form["model[EditedBy]"]) : (Guid?)null;
                FleetRegistrationDataModel.UnavailableFromDate = Form["model[UnavailableFromDate]"];
                FleetRegistrationDataModel.UnavailableToDate = Form["model[UnavailableToDate]"];
                FleetRegistrationDataModel.UnavailableNote = Form["model[UnavailableNote]"];

                //add FleetRegistration detail into database
                var FleetRegistrationId = FleetRegistrationService.AddFleetRegistration(FleetRegistrationDataModel);

                /// checking FleetRegistrationId not equal to null
                if (ServiceHelper.IsGuid(FleetRegistrationId))
                {
                    #region Save data in document entity.

                    // multi-part upload code
                    int iUploadedCnt = 0;

                    // check the file count.
                    for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                    {
                        System.Web.HttpPostedFile hpf = hfc[iCnt];

                        if (hpf.ContentLength > 0)
                        {
                            // check if the seleted file(S) already exixts in folder. (AVOID DUPLICATE)
                            if (!File.Exists(sPath + Path.GetFileName(FleetRegistrationId + "_FleetRegistration_" + iCnt + 1 + "_" + hpf.FileName)))
                            {
                                // SAVE THE FILES IN THE FOLDER.
                                hpf.SaveAs(sPath + Path.GetFileName(FleetRegistrationId + "_FleetRegistration_" + iCnt + 1 + "_" + hpf.FileName));
                                iUploadedCnt = iUploadedCnt + 1;

                                // getting base URL of app
                                String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

                                //bind file data in a document model.
                                DocumentData.Id = Guid.NewGuid().ToString();
                                DocumentData.OriginalName = "Image";
                                DocumentData.Name = FleetRegistrationId + "_FleetRegistration_" + iCnt + 1 + "_" + hpf.FileName;
                                DocumentData.Title = "Images";
                                DocumentData.Description = string.Empty;
                                DocumentData.Tags = string.Empty;
                                DocumentData.URL = strUrl + "/Uploads/" + FleetRegistrationId + "_FleetRegistration_" + iCnt + 1 + "_" + hpf.FileName;//provider.FileData.FirstOrDefault().LocalFileName;//
                                DocumentData.Extension = new System.IO.FileInfo(hpf.FileName).Extension;
                                DocumentData.ThumbnailFileName = "";
                                DocumentData.FileSize = 0;
                                DocumentData.Private = true;

                                //save document data.
                                DocumentId = DocumentService.AddDocument(new Guid(DocumentData.Id), DocumentData.OriginalName, DocumentData.Name, DocumentData.URL, DocumentData.Title, DocumentData.Description, DocumentData.Extension, DocumentData.FileSize, DocumentData.Private, DocumentData.Tags, DocumentData.ThumbnailFileName);

                                if (ServiceHelper.IsGuid(DocumentId.ToString()))
                                {
                                    // updating documentId in docket table
                                    var UpdateDocument = FleetRegistrationService.UpdateDocumentId(new Guid(FleetRegistrationId), DocumentId.ToString());
                                    // checking document status
                                    if (UpdateDocument)
                                    {
                                        SuccessResponse = true;
                                    }
                                    else
                                    {
                                        SuccessResponse = false;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }

                // success response
                if (SuccessResponse)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Record inserted successfully", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    //return service for get user.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Record not inserted successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// This method use for update FleetRegistration into FleetRegistration entity.
        /// </summary>
        /// <param name="FleetRegistrationDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateFleetRegistration")]
        public HttpResponseMessage UpdateFleetRegistration()
        {
            try
            {
                // defining variables globally
                FleetRegistrationDataModel FleetRegistrationDataModel = new FleetRegistrationDataModel();
                DocumentDataModel DocumentData = new DocumentDataModel();
                var DocumentId = (Guid?)null;
                var SuccessResponse = false;
                var DocName = string.Empty;

                // Define the path where we want to save the files.
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                var Form = System.Web.HttpContext.Current.Request.Form;

                // setting form data into entity
                FleetRegistrationDataModel.FleetRegistrationId = new Guid(Form["model[FleetRegistrationId]"]);
                FleetRegistrationDataModel.FleetTypeId = new Guid(Form["model[FleetTypeId]"]);
                FleetRegistrationDataModel.Make = Form["model[Make]"];
                FleetRegistrationDataModel.Model = Form["model[Model]"];
                FleetRegistrationDataModel.Capacity = Form["model[Capacity]"];
                FleetRegistrationDataModel.Year = Form["model[Year]"];
                FleetRegistrationDataModel.Registration = Form["model[Registration]"];
                FleetRegistrationDataModel.BorgCivilPlantNumber = Form["model[BorgCivilPlantNumber]"];
                FleetRegistrationDataModel.VINNumber = Form["model[VINNumber]"];
                FleetRegistrationDataModel.EngineNumber = Form["model[EngineNumber]"];
                FleetRegistrationDataModel.InsuranceDate = Form["model[InsuranceDate]"];
                FleetRegistrationDataModel.CurrentMeterReading = Form["model[CurrentMeterReading]"];
                FleetRegistrationDataModel.LastServiceMeterReading = Form["model[LastServiceMeterReading]"];
                FleetRegistrationDataModel.HVISType = Form["model[HVISType]"];
                FleetRegistrationDataModel.ServiceInterval = Form["model[ServiceInterval]"];
                FleetRegistrationDataModel.AttachmentId = Form["model[AttachmentId]"] != "null" ? Form["model[AttachmentId]"] : "";
                FleetRegistrationDataModel.IsBooked = Convert.ToBoolean(Form["model[IsBooked]"]);
                FleetRegistrationDataModel.IsActive = Convert.ToBoolean(Form["model[IsActive]"]);
                FleetRegistrationDataModel.UnavailableFromDate = Form["model[UnavailableFromDate]"];
                FleetRegistrationDataModel.UnavailableToDate = Form["model[UnavailableToDate]"];
                FleetRegistrationDataModel.UnavailableNote = Form["model[UnavailableNote]"];

                //add FleetRegistration detail into database
                var FleetRegistrationId = FleetRegistrationService.UpdateFleetRegistration(FleetRegistrationDataModel);

                /// checking FleetRegistrationId not equal to null
                if (ServiceHelper.IsGuid(FleetRegistrationId))
                {
                    #region Save data in document entity.

                    // multi-part upload code
                    int iUploadedCnt = 0;
                    int Count = 0;

                    // check dcument exist or not
                    var DocumentIds = FleetRegistrationService.GetRegisterFleetsByFleetRegistrationId(new Guid(FleetRegistrationId)).DocumentId;

                    // holding document Ids
                    List<string> Ids = new List<string>();

                    if (DocumentId != null)
                    {
                        Ids = DocumentIds.Split(',').ToList<string>();
                        Count = Ids.Count;
                        Count++;
                    }
                    else
                    {
                        Count++;
                    }

                    //System.Web.HttpPostedFile hpf12 = hfc.AllKeys[0];
                    // check the file count.
                    for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                    {
                        System.Web.HttpPostedFile hpf = hfc[iCnt];

                        if (hpf.ContentLength > 0)
                        {
                            // check if the seleted file(S) already exixts in folder. (AVOID DUPLICATE)
                            if (!File.Exists(sPath + Path.GetFileName(FleetRegistrationId + "_FleetRegistration_" + Count + "_" + hpf.FileName)))
                            {
                                // SAVE THE FILES IN THE FOLDER.
                                hpf.SaveAs(sPath + Path.GetFileName(FleetRegistrationId + "_FleetRegistration_" + Count + "_" + hpf.FileName));
                                iUploadedCnt = iUploadedCnt + 1;

                                // getting base URL of app
                                String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

                                //bind file data in a document model.
                                DocumentData.Id = Guid.NewGuid().ToString();
                                DocumentData.OriginalName = "Image";
                                DocumentData.Name = FleetRegistrationId + "_FleetRegistration_" + Count + "_" + hpf.FileName;
                                DocumentData.Title = "Images";
                                DocumentData.Description = string.Empty;
                                DocumentData.Tags = string.Empty;
                                DocumentData.URL = strUrl + "/Uploads/" + FleetRegistrationId + "_FleetRegistration_" + Count + "_" + hpf.FileName;//provider.FileData.FirstOrDefault().LocalFileName;//
                                DocumentData.Extension = new System.IO.FileInfo(hpf.FileName).Extension;
                                DocumentData.ThumbnailFileName = "";
                                DocumentData.FileSize = 0;
                                DocumentData.Private = true;

                                //save document data.
                                DocumentId = DocumentService.AddDocument(new Guid(DocumentData.Id), DocumentData.OriginalName, DocumentData.Name, DocumentData.URL, DocumentData.Title, DocumentData.Description, DocumentData.Extension, DocumentData.FileSize, DocumentData.Private, DocumentData.Tags, DocumentData.ThumbnailFileName);

                                if (ServiceHelper.IsGuid(DocumentId.ToString()))
                                {
                                    // updating documentId in docket table
                                    var UpdateDocument = FleetRegistrationService.UpdateDocumentId(new Guid(FleetRegistrationId), DocumentId.ToString());
                                    // checking document status
                                    if (UpdateDocument)
                                    {
                                        SuccessResponse = true;
                                    }
                                    else
                                    {
                                        SuccessResponse = false;
                                    }
                                }
                            }
                        }
                        Count++;
                    }
                    #endregion
                }

                // success response
                if (SuccessResponse)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Record inserted successfully", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    //return service for get user.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Record not inserted successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// method to delete FleetRegistration by FleetRegistrationId
        /// </summary>
        /// <param name="FleetRegistrationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteFleetRegistration/{FleetRegistrationId}")]
        public HttpResponseMessage DeleteFleetRegistration(Guid FleetRegistrationId)
        {
            try
            {
                var FleetRegistration = FleetRegistrationService.DeleteFleetRegistration(FleetRegistrationId);
                if (FleetRegistration)
                {
                    //return FleetRegistration delete service 
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Deleted Successfully", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// This method use for add FleetRegistration into FleetRegistration entity.
        /// </summary>
        /// <param name="FleetRegistrationDataModel"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("AddFleetRegistration")]
        //public HttpResponseMessage AddFleetRegistration(FleetRegistrationDataModel FleetRegistrationDataModel)
        //{
        //    try
        //    {
        //        // defining variables globally
        //        DocumentDataModel DocumentData = new DocumentDataModel();
        //        var DocumentId = (Guid?)null;
        //        var SuccessResponse = false;

        //        //add FleetRegistration detail into database
        //        var FleetRegistrationId = FleetRegistrationService.AddFleetRegistration(FleetRegistrationDataModel);

        //        /// checking FleetRegistrationId not equal to null
        //        if (ServiceHelper.IsGuid(FleetRegistrationId))
        //        {
        //            #region Save data in document entity.

        //            // getting base URL of app
        //            String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
        //            String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

        //            // condition to check RegistrationImageBase64 not null
        //            if (FleetRegistrationDataModel.RegistrationImageBase64 != null)
        //            {
        //                //bind file data in a document model.
        //                DocumentData.Id = Guid.NewGuid().ToString();
        //                DocumentData.OriginalName = "Image";
        //                DocumentData.Name = FleetRegistrationId + "_RegistrationImage_" + ".png";
        //                DocumentData.Title = "Images";
        //                DocumentData.Description = string.Empty;
        //                DocumentData.Tags = string.Empty;
        //                DocumentData.URL = strUrl + "/Uploads/" + FleetRegistrationId + "_RegistrationImage_" + ".png";//provider.FileData.FirstOrDefault().LocalFileName;//
        //                DocumentData.Extension = ".png";
        //                DocumentData.ThumbnailFileName = "";
        //                DocumentData.FileSize = 0;
        //                DocumentData.Private = true;

        //                //save document data.
        //                DocumentId = DocumentService.AddDocument(new Guid(DocumentData.Id), DocumentData.OriginalName, DocumentData.Name, DocumentData.URL, DocumentData.Title, DocumentData.Description, DocumentData.Extension, DocumentData.FileSize, DocumentData.Private, DocumentData.Tags, DocumentData.ThumbnailFileName);

        //                if (ServiceHelper.IsGuid(DocumentId.ToString()))
        //                {
        //                    // updating documentId in docket table
        //                    var UpdateDocument = FleetRegistrationService.UpdateDocumentId(new Guid(FleetRegistrationId), DocumentId.ToString());

        //                    // uploading image on Uploads folder
        //                    var ImageUploadStatus = UploadImage.base64ToImage(FleetRegistrationDataModel.RegistrationImageBase64, FleetRegistrationId + "_RegistrationImage_" + ".png");

        //                    // checking document status
        //                    if (UpdateDocument)
        //                    {
        //                        SuccessResponse = true;
        //                    }
        //                    else
        //                    {
        //                        SuccessResponse = false;
        //                    }
        //                }
        //                else
        //                {
        //                    //return result from service response.
        //                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Document not added successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
        //                }
        //            }

        //            // condition to check ServiceImageBase64 not null
        //            if (FleetRegistrationDataModel.ServiceImageBase64 != null)
        //            {
        //                //bind file data in a document model.
        //                DocumentData.Id = Guid.NewGuid().ToString();
        //                DocumentData.OriginalName = "Image";
        //                DocumentData.Name = FleetRegistrationId + "_ServiceImage_" + ".png";
        //                DocumentData.Title = "Images";
        //                DocumentData.Description = string.Empty;
        //                DocumentData.Tags = string.Empty;
        //                DocumentData.URL = strUrl + "/Uploads/" + FleetRegistrationId + "_ServiceImage_" + ".png";//provider.FileData.FirstOrDefault().LocalFileName;//
        //                DocumentData.Extension = ".png";
        //                DocumentData.ThumbnailFileName = "";
        //                DocumentData.FileSize = 0;
        //                DocumentData.Private = true;

        //                //save document data.
        //                DocumentId = DocumentService.AddDocument(new Guid(DocumentData.Id), DocumentData.OriginalName, DocumentData.Name, DocumentData.URL, DocumentData.Title, DocumentData.Description, DocumentData.Extension, DocumentData.FileSize, DocumentData.Private, DocumentData.Tags, DocumentData.ThumbnailFileName);

        //                if (ServiceHelper.IsGuid(DocumentId.ToString()))
        //                {
        //                    // updating documentId in docket table
        //                    var UpdateDocument = FleetRegistrationService.UpdateDocumentId(new Guid(FleetRegistrationId), DocumentId.ToString());

        //                    // uploading image on Uploads folder
        //                    var ImageUploadStatus = UploadImage.base64ToImage(FleetRegistrationDataModel.ServiceImageBase64, FleetRegistrationId + "_ServiceImage_" + ".png");

        //                    // checking document status
        //                    if (UpdateDocument)
        //                    {
        //                        SuccessResponse = true;
        //                    }
        //                    else
        //                    {
        //                        SuccessResponse = false;
        //                    }
        //                }
        //                else
        //                {
        //                    //return result from service response.
        //                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Document not added successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
        //                }
        //            }

        //            // condition to check PlantRiskImageBase64 not null
        //            if (FleetRegistrationDataModel.PlantRiskImageBase64 != null)
        //            {
        //                //bind file data in a document model.
        //                DocumentData.Id = Guid.NewGuid().ToString();
        //                DocumentData.OriginalName = "Image";
        //                DocumentData.Name = FleetRegistrationId + "_PlantRiskImage_" + ".png";
        //                DocumentData.Title = "Images";
        //                DocumentData.Description = string.Empty;
        //                DocumentData.Tags = string.Empty;
        //                DocumentData.URL = strUrl + "/Uploads/" + FleetRegistrationId + "_PlantRiskImage_" + ".png";//provider.FileData.FirstOrDefault().LocalFileName;//
        //                DocumentData.Extension = ".png";
        //                DocumentData.ThumbnailFileName = "";
        //                DocumentData.FileSize = 0;
        //                DocumentData.Private = true;

        //                //save document data.
        //                DocumentId = DocumentService.AddDocument(new Guid(DocumentData.Id), DocumentData.OriginalName, DocumentData.Name, DocumentData.URL, DocumentData.Title, DocumentData.Description, DocumentData.Extension, DocumentData.FileSize, DocumentData.Private, DocumentData.Tags, DocumentData.ThumbnailFileName);

        //                if (ServiceHelper.IsGuid(DocumentId.ToString()))
        //                {
        //                    // updating documentId in docket table
        //                    var UpdateDocument = FleetRegistrationService.UpdateDocumentId(new Guid(FleetRegistrationId), DocumentId.ToString());

        //                    // uploading image on Uploads folder
        //                    var ImageUploadStatus = UploadImage.base64ToImage(FleetRegistrationDataModel.PlantRiskImageBase64, FleetRegistrationId + "_PlantRiskImage_" + ".png");

        //                    // checking document status
        //                    if (UpdateDocument)
        //                    {
        //                        SuccessResponse = true;
        //                    }
        //                    else
        //                    {
        //                        SuccessResponse = false;
        //                    }
        //                }
        //                else
        //                {
        //                    //return result from service response.
        //                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Document not added successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
        //                }
        //            }

        //            // condition to check NHVR Image base64 not null
        //            if (FleetRegistrationDataModel.NHVRImageBase64 != null)
        //            {
        //                //bind file data in a document model.
        //                DocumentData.Id = Guid.NewGuid().ToString();
        //                DocumentData.OriginalName = "Image";
        //                DocumentData.Name = FleetRegistrationId + "_NHVRImage_" + ".png";
        //                DocumentData.Title = "Images";
        //                DocumentData.Description = string.Empty;
        //                DocumentData.Tags = string.Empty;
        //                DocumentData.URL = strUrl + "/Uploads/" + FleetRegistrationId + "_NHVRImage_" + ".png";//provider.FileData.FirstOrDefault().LocalFileName;//
        //                DocumentData.Extension = ".png";
        //                DocumentData.ThumbnailFileName = "";
        //                DocumentData.FileSize = 0;
        //                DocumentData.Private = true;

        //                //save document data.
        //                DocumentId = DocumentService.AddDocument(new Guid(DocumentData.Id), DocumentData.OriginalName, DocumentData.Name, DocumentData.URL, DocumentData.Title, DocumentData.Description, DocumentData.Extension, DocumentData.FileSize, DocumentData.Private, DocumentData.Tags, DocumentData.ThumbnailFileName);

        //                if (ServiceHelper.IsGuid(DocumentId.ToString()))
        //                {
        //                    // updating documentId in docket table
        //                    var UpdateDocument = FleetRegistrationService.UpdateDocumentId(new Guid(FleetRegistrationId), DocumentId.ToString());

        //                    // uploading image on Uploads folder
        //                    var ImageUploadStatus = UploadImage.base64ToImage(FleetRegistrationDataModel.NHVRImageBase64, FleetRegistrationId + "_NHVRImage_" + ".png");

        //                    // checking document status
        //                    if (UpdateDocument)
        //                    {
        //                        SuccessResponse = true;
        //                    }
        //                    else
        //                    {
        //                        SuccessResponse = false;
        //                    }
        //                }
        //                else
        //                {
        //                    //return result from service response.
        //                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Document not added successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
        //                }
        //            }

        //            #endregion
        //        }

        //        // success response
        //        if (SuccessResponse)
        //        {
        //            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Record inserted successfully", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
        //        }
        //        else
        //        {
        //            //return service for get user.
        //            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Record not inserted successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // handel exception log.
        //        Console.Write(ex.Message);

        //        //return case of exception.
        //        return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
        //    }
        //}

        /// <summary>
        /// This method use for update FleetRegistration into FleetRegistration entity.
        /// </summary>
        /// <param name="FleetRegistrationDataModel"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("UpdateFleetRegistration")]
        //public HttpResponseMessage UpdateFleetRegistration(FleetRegistrationDataModel FleetRegistrationDataModel)
        //{
        //    try
        //    {
        //        // defining variables globally
        //        DocumentDataModel DocumentData = new DocumentDataModel();
        //        var DocumentId = (Guid?)null;
        //        var SuccessResponse = false;
        //        var DocName = string.Empty;

        //        //add FleetRegistration detail into database
        //        var FleetRegistrationId = FleetRegistrationService.UpdateFleetRegistration(FleetRegistrationDataModel);

        //        /// checking FleetRegistrationId not equal to null
        //        if (ServiceHelper.IsGuid(FleetRegistrationId))
        //        {
        //            #region Save data in document entity.

        //            // getting base URL of app
        //            String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
        //            String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

        //            // condition to check RegistrationImageBase64 not null
        //            if (FleetRegistrationDataModel.RegistrationImageBase64 != null)
        //            {
        //                DocName = FleetRegistrationId + "_RegistrationImage_" + ".png";
        //                var DocExist = DocumentService.GetDocumentByName(DocName);
        //                if (DocExist == null)
        //                {
        //                    //bind file data in a document model.
        //                    DocumentData.Id = Guid.NewGuid().ToString();
        //                    DocumentData.OriginalName = "Image";
        //                    DocumentData.Name = FleetRegistrationId + "_RegistrationImage_" + ".png";
        //                    DocumentData.Title = "Images";
        //                    DocumentData.Description = string.Empty;
        //                    DocumentData.Tags = string.Empty;
        //                    DocumentData.URL = strUrl + "/Uploads/" + FleetRegistrationId + "_RegistrationImage_" + ".png";//provider.FileData.FirstOrDefault().LocalFileName;//
        //                    DocumentData.Extension = ".png";
        //                    DocumentData.ThumbnailFileName = "";
        //                    DocumentData.FileSize = 0;
        //                    DocumentData.Private = true;

        //                    //save document data.
        //                    DocumentId = DocumentService.AddDocument(new Guid(DocumentData.Id), DocumentData.OriginalName, DocumentData.Name, DocumentData.URL, DocumentData.Title, DocumentData.Description, DocumentData.Extension, DocumentData.FileSize, DocumentData.Private, DocumentData.Tags, DocumentData.ThumbnailFileName);
        //                    if (ServiceHelper.IsGuid(DocumentId.ToString()))
        //                    {
        //                        // updating documentId in docket table
        //                        var UpdateDocument = FleetRegistrationService.UpdateDocumentId(new Guid(FleetRegistrationId), DocumentId.ToString());

        //                        // uploading image on Uploads folder
        //                        var ImageUploadStatus = UploadImage.base64ToImage(FleetRegistrationDataModel.RegistrationImageBase64, FleetRegistrationId + "_RegistrationImage_" + ".png");

        //                        // checking document status
        //                        if (UpdateDocument)
        //                        {
        //                            SuccessResponse = true;
        //                        }
        //                        else
        //                        {
        //                            SuccessResponse = false;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //return result from service response.
        //                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Document not added successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
        //                    }
        //                }
        //                else
        //                {
        //                    // uploading image on Uploads folder
        //                    var ImageUploadStatus = UploadImage.base64ToImage(FleetRegistrationDataModel.RegistrationImageBase64, FleetRegistrationId + "_RegistrationImage_" + ".png");
        //                }

        //            }

        //            // condition to check ServiceImageBase64 not null
        //            if (FleetRegistrationDataModel.ServiceImageBase64 != null)
        //            {
        //                DocName = FleetRegistrationId + "_RegistrationImage_" + ".png";
        //                var DocExist = DocumentService.GetDocumentByName(DocName);
        //                if (DocExist == null)
        //                {
        //                    //bind file data in a document model.
        //                    DocumentData.Id = Guid.NewGuid().ToString();
        //                    DocumentData.OriginalName = "Image";
        //                    DocumentData.Name = FleetRegistrationId + "_ServiceImage_" + ".png";
        //                    DocumentData.Title = "Images";
        //                    DocumentData.Description = string.Empty;
        //                    DocumentData.Tags = string.Empty;
        //                    DocumentData.URL = strUrl + "/Uploads/" + FleetRegistrationId + "_ServiceImage_" + ".png";//provider.FileData.FirstOrDefault().LocalFileName;//
        //                    DocumentData.Extension = ".png";
        //                    DocumentData.ThumbnailFileName = "";
        //                    DocumentData.FileSize = 0;
        //                    DocumentData.Private = true;

        //                    //save document data.
        //                    DocumentId = DocumentService.AddDocument(new Guid(DocumentData.Id), DocumentData.OriginalName, DocumentData.Name, DocumentData.URL, DocumentData.Title, DocumentData.Description, DocumentData.Extension, DocumentData.FileSize, DocumentData.Private, DocumentData.Tags, DocumentData.ThumbnailFileName);

        //                    if (ServiceHelper.IsGuid(DocumentId.ToString()))
        //                    {
        //                        // updating documentId in docket table
        //                        var UpdateDocument = FleetRegistrationService.UpdateDocumentId(new Guid(FleetRegistrationId), DocumentId.ToString());

        //                        // uploading image on Uploads folder
        //                        var ImageUploadStatus = UploadImage.base64ToImage(FleetRegistrationDataModel.ServiceImageBase64, FleetRegistrationId + "_ServiceImage_" + ".png");

        //                        // checking document status
        //                        if (UpdateDocument)
        //                        {
        //                            SuccessResponse = true;
        //                        }
        //                        else
        //                        {
        //                            SuccessResponse = false;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //return result from service response.
        //                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Document not added successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
        //                    }
        //                }
        //                else
        //                {
        //                    // uploading image on Uploads folder
        //                    var ImageUploadStatus = UploadImage.base64ToImage(FleetRegistrationDataModel.ServiceImageBase64, FleetRegistrationId + "_ServiceImage_" + ".png");
        //                }
        //            }

        //            // condition to check PlantRiskImageBase64 not null
        //            if (FleetRegistrationDataModel.PlantRiskImageBase64 != null)
        //            {
        //                DocName = FleetRegistrationId + "_RegistrationImage_" + ".png";
        //                var DocExist = DocumentService.GetDocumentByName(DocName);
        //                if (DocExist == null)
        //                {
        //                    //bind file data in a document model.
        //                    DocumentData.Id = Guid.NewGuid().ToString();
        //                    DocumentData.OriginalName = "Image";
        //                    DocumentData.Name = FleetRegistrationId + "_PlantRiskImage_" + ".png";
        //                    DocumentData.Title = "Images";
        //                    DocumentData.Description = string.Empty;
        //                    DocumentData.Tags = string.Empty;
        //                    DocumentData.URL = strUrl + "/Uploads/" + FleetRegistrationId + "_PlantRiskImage_" + ".png";//provider.FileData.FirstOrDefault().LocalFileName;//
        //                    DocumentData.Extension = ".png";
        //                    DocumentData.ThumbnailFileName = "";
        //                    DocumentData.FileSize = 0;
        //                    DocumentData.Private = true;

        //                    //save document data.
        //                    DocumentId = DocumentService.AddDocument(new Guid(DocumentData.Id), DocumentData.OriginalName, DocumentData.Name, DocumentData.URL, DocumentData.Title, DocumentData.Description, DocumentData.Extension, DocumentData.FileSize, DocumentData.Private, DocumentData.Tags, DocumentData.ThumbnailFileName);

        //                    if (ServiceHelper.IsGuid(DocumentId.ToString()))
        //                    {
        //                        // updating documentId in docket table
        //                        var UpdateDocument = FleetRegistrationService.UpdateDocumentId(new Guid(FleetRegistrationId), DocumentId.ToString());

        //                        // uploading image on Uploads folder
        //                        var ImageUploadStatus = UploadImage.base64ToImage(FleetRegistrationDataModel.PlantRiskImageBase64, FleetRegistrationId + "_PlantRiskImage_" + ".png");

        //                        // checking document status
        //                        if (UpdateDocument)
        //                        {
        //                            SuccessResponse = true;
        //                        }
        //                        else
        //                        {
        //                            SuccessResponse = false;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //return result from service response.
        //                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Document not added successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
        //                    }
        //                }
        //                else
        //                {
        //                    // uploading image on Uploads folder
        //                    var ImageUploadStatus = UploadImage.base64ToImage(FleetRegistrationDataModel.PlantRiskImageBase64, FleetRegistrationId + "_PlantRiskImage_" + ".png");
        //                }
        //            }

        //            // condition to check NHVR Image base64 not null
        //            if (FleetRegistrationDataModel.NHVRImageBase64 != null)
        //            {
        //                DocName = FleetRegistrationId + "_RegistrationImage_" + ".png";
        //                var DocExist = DocumentService.GetDocumentByName(DocName);
        //                if (DocExist == null)
        //                {
        //                    //bind file data in a document model.
        //                    DocumentData.Id = Guid.NewGuid().ToString();
        //                    DocumentData.OriginalName = "Image";
        //                    DocumentData.Name = FleetRegistrationId + "_NHVRImage_" + ".png";
        //                    DocumentData.Title = "Images";
        //                    DocumentData.Description = string.Empty;
        //                    DocumentData.Tags = string.Empty;
        //                    DocumentData.URL = strUrl + "/Uploads/" + FleetRegistrationId + "_NHVRImage_" + ".png";//provider.FileData.FirstOrDefault().LocalFileName;//
        //                    DocumentData.Extension = ".png";
        //                    DocumentData.ThumbnailFileName = "";
        //                    DocumentData.FileSize = 0;
        //                    DocumentData.Private = true;

        //                    //save document data.
        //                    DocumentId = DocumentService.AddDocument(new Guid(DocumentData.Id), DocumentData.OriginalName, DocumentData.Name, DocumentData.URL, DocumentData.Title, DocumentData.Description, DocumentData.Extension, DocumentData.FileSize, DocumentData.Private, DocumentData.Tags, DocumentData.ThumbnailFileName);

        //                    if (ServiceHelper.IsGuid(DocumentId.ToString()))
        //                    {
        //                        // updating documentId in docket table
        //                        var UpdateDocument = FleetRegistrationService.UpdateDocumentId(new Guid(FleetRegistrationId), DocumentId.ToString());

        //                        // uploading image on Uploads folder
        //                        var ImageUploadStatus = UploadImage.base64ToImage(FleetRegistrationDataModel.NHVRImageBase64, FleetRegistrationId + "_NHVRImage_" + ".png");

        //                        // checking document status
        //                        if (UpdateDocument)
        //                        {
        //                            SuccessResponse = true;
        //                        }
        //                        else
        //                        {
        //                            SuccessResponse = false;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //return result from service response.
        //                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Document not added successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
        //                    }
        //                }
        //                else
        //                {
        //                    // uploading image on Uploads folder
        //                    var ImageUploadStatus = UploadImage.base64ToImage(FleetRegistrationDataModel.NHVRImageBase64, FleetRegistrationId + "_NHVRImage_" + ".png");
        //                }
        //            }

        //            #endregion
        //        }

        //        // success response
        //        if (SuccessResponse)
        //        {
        //            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Record inserted successfully", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
        //        }
        //        else
        //        {
        //            //return service for get user.
        //            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Record not inserted successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // handel exception log.
        //        Console.Write(ex.Message);

        //        //return case of exception.
        //        return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
        //    }
        //}

        #endregion

        #region Drivers Api's

        /// <summary>
        /// getting all driver select list for dropdown
        /// </summary>
        /// params FleetRegistrationId
        /// <returns></returns>
        [HttpGet]
        [Route("GetDriversByFleetRegistrationId/{FleetRegistrationId}")]
        public HttpResponseMessage GetDriversByFleetRegistrationId(Guid FleetRegistrationId)
        {
            try
            {
                //get drivers list
                var Drivers = DriversService.GetDriversList(FleetRegistrationId);

                //check object
                if (Drivers.Count > 0 && Drivers != null)
                {
                    List<ExpandoObject> Driverlist = new List<ExpandoObject>();
                    foreach (var Driver in Drivers)
                    {
                        dynamic DriverDetail = new ExpandoObject();
                        DriverDetail.DriverName = Driver.Text;
                        DriverDetail.DriverId = Driver.Value;
                        Driverlist.Add(DriverDetail);
                    }
                    //return customer service for get customer type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Driver List", Succeeded = true, DataObject = new ExpandoObject(), DataList = Driverlist, ErrorInfo = "" });
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
        /// this method is for getting all driver 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllDriver")]
        public HttpResponseMessage GetAllDriver()
        {
            try
            {
                //get driver list 
                var DriverCollection = DriversService.GetAllDriver().OrderByDescending(x => x.CreatedDate).ToList();

                //check object
                if (DriverCollection.Count > 0 && DriverCollection != null)
                {
                    //dynamic list.
                    dynamic Drivers = new List<ExpandoObject>();

                    //return response from Driver service.
                    foreach (var DriverDetail in DriverCollection)
                    {
                        //bind dynamic property.
                        dynamic Driver = new ExpandoObject();

                        //map ids
                        Driver.DriverId = DriverDetail.DriverId;
                        Driver.FleetRegistrationId = DriverDetail.FleetsRegistration != null ? DriverDetail.FleetsRegistration.Registration : "";
                        Driver.CountryName = DriverDetail.Country != null ? DriverDetail.Country.Name : "";
                        Driver.EmploymentCategoryId = DriverDetail.EmploymentCategoryId;
                        Driver.Image = DriverDetail.Document != null ? DriverDetail.Document.Name : "";
                        Driver.LicenseClass = DriverDetail.LicenseClass != null ? DriverDetail.LicenseClass.Class : "";
                        Driver.FirstName = DriverDetail.FirstName;
                        Driver.LastName = DriverDetail.LastName;
                        Driver.Email = DriverDetail.Email;
                        Driver.MobileNumber = DriverDetail.MobileNumber;
                        Driver.AddressLine1 = DriverDetail.AddressLine1;
                        Driver.AddressLine2 = DriverDetail.AddressLine2;
                        Driver.City = DriverDetail.Suburb;
                        Driver.PostCode = DriverDetail.PostCode;
                        Driver.CardNumber = DriverDetail.CardNumber;
                        Driver.LicenseNumber = DriverDetail.LicenseNumber;
                        Driver.StateName = DriverDetail.State != null ? DriverDetail.State.Name : "";
                        Driver.ExpiryDate = DriverDetail.ExpiryDate;
                        Driver.BaseRate = DriverDetail.BaseRate;
                        Driver.Shift = DriverDetail.Shift;
                        Driver.Awards = DriverDetail.Awards;
                        Driver.IsActive = DriverDetail.IsActive;
                        Driver.CreatedDate = DriverDetail.CreatedDate;
                        Driver.EditedDate = DriverDetail.EditedDate;
                        Driver.EmploymentInfo = DriverDetail.EmploymentCategory != null && DriverDetail.StatusLookupId != null ? DriverDetail.EmploymentCategory.Category + ":" + "/" + DriverDetail.StatusLookup.Title : "";
                        Driver.LicenseInfo = DriverDetail.LicenseClass != null ? DriverDetail.LicenseNumber + "/" + DriverDetail.LicenseClass.Class : "";
                        Driver.CardInfo = DriverDetail.CardNumber + "/" + DriverDetail.ExpiryDate;
                        //Driver.ProficPic = DriverDetail.Document.Name;

                        ////set Driver values in list.
                        Drivers.Add(Driver);
                    }

                    //return user service for get organasation.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Driver List", Succeeded = true, DataObject = new ExpandoObject(), DataList = Drivers, ErrorInfo = "" });
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
        /// this method is used for getting driver detail
        /// </summary>
        /// <param name="DriverId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDriverDetail/{DriverId}")]
        public HttpResponseMessage GetDriverDetail(Guid DriverId)
        {
            try
            {
                var ImageName = string.Empty;
                //check is valid DriverId.
                if (!ServiceHelper.IsGuid((string)DriverId.ToString()))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid driver id." });
                }

                //getting driver detail by driver Id
                var DriverDetail = DriversService.GetDriverByDriverId(DriverId);

                //check service response.
                if (DriverDetail != null)
                {
                    //bind dynamic property.
                    dynamic Driver = new ExpandoObject();
                    Driver.DriverId = DriverDetail.DriverId;
                    Driver.FleetRegistrationId = DriverDetail.FleetRegistrationId;
                    Driver.CountryId = DriverDetail.CountryId;
                    Driver.StateId = DriverDetail.StateId;
                    Driver.EmploymentCategoryId = DriverDetail.EmploymentCategoryId;
                    Driver.LicenseClassId = DriverDetail.LicenseClassId;
                    Driver.FirstName = DriverDetail.FirstName;
                    Driver.LastName = DriverDetail.LastName;
                    Driver.Email = DriverDetail.Email;
                    Driver.MobileNumber = DriverDetail.MobileNumber;
                    Driver.AddressLine1 = DriverDetail.AddressLine1;
                    Driver.AddressLine2 = DriverDetail.AddressLine2;
                    Driver.Street = DriverDetail.Street;
                    Driver.Suburb = DriverDetail.Suburb;
                    Driver.ZipCode = DriverDetail.PostCode;
                    Driver.CardNumber = DriverDetail.CardNumber;
                    Driver.LicenseNumber = DriverDetail.LicenseNumber;
                    Driver.BaseRate = DriverDetail.BaseRate;
                    Driver.Shift = DriverDetail.Shift;
                    Driver.Awards = DriverDetail.Awards;
                    Driver.IsActive = DriverDetail.IsActive;
                    Driver.CreatedDate = DriverDetail.CreatedDate;
                    Driver.ProfilePic = DriverDetail.Document != null ? DriverDetail.Document.Name : "";
                    Driver.StatusLookupId = DriverDetail.StatusLookupId;
                    Driver.CreatedDate = DriverDetail.CreatedDate;
                    Driver.AnnualLeaveBalance = DriverDetail.AnnualLeaveBalance;
                    Driver.SickLeaveBalance = DriverDetail.SickLeaveBalance;
                    Driver.City = DriverDetail.City;
                    Driver.TypeFromDate = DriverDetail.TypeFromDate;
                    Driver.TypeToDate = DriverDetail.TypeToDate;
                    Driver.TypeNote = DriverDetail.TypeNote;
                    Driver.LeaveFromDate = DriverDetail.LeaveFromDate;
                    Driver.LeaveToDate = DriverDetail.LeaveToDate;
                    Driver.LeaveNote = DriverDetail.LeaveNote;
                    Driver.EmergencyContactName = DriverDetail.EmergencyContactName;
                    Driver.EmergencyContactNumber = DriverDetail.EmergencyContactNumber;
                    Driver.ExpiryDate = DriverDetail.ExpiryDate;

                    if (DriverDetail.DocumentId != null)
                    {
                        // split document Id to send image name
                        string[] SplitDocument = DriverDetail.DocumentId.Split(',');
                        foreach (var item in SplitDocument)
                        {
                            var Document = DocumentService.GetDocument(new Guid(item));

                            // creating comma seprated string of image name 
                            if (ImageName == "")
                                ImageName = Document.Name;
                            else
                                ImageName = ImageName + "," + Document.Name;
                        }
                    }
                    Driver.LicenseImage = ImageName;

                    //bind dynamic list property.
                    dynamic WhiteCardList = new List<ExpandoObject>();

                    // checking count not equal to zero
                    if (DriverDetail.DriverWhiteCards.Count > 0)
                    {
                        foreach (var WhiteCard in DriverDetail.DriverWhiteCards)
                        {
                            //bind dynamic property.
                            dynamic WhiteCardObject = new ExpandoObject();
                            //bind child object.
                            WhiteCardObject.DriverWhiteCardId = WhiteCard.DriverWhiteCardId;
                            WhiteCardObject.DriverId = WhiteCard.DriverId;
                            WhiteCardObject.CardNumber = WhiteCard.CardNumber;
                            WhiteCardObject.IssueDate = WhiteCard.IssueDate;
                            WhiteCardObject.Notes = WhiteCard.Notes;

                            //set WhiteCard detail in list.
                            WhiteCardList.Add(WhiteCardObject);
                        }
                    }

                    //bind dynamic list property.
                    dynamic VocCardList = new List<ExpandoObject>();

                    // checking count not equal to zero
                    if (DriverDetail.DriverVocCards.Count > 0)
                    {
                        foreach (var VocCard in DriverDetail.DriverVocCards)
                        {
                            //bind dynamic property.
                            dynamic VocCardObject = new ExpandoObject();
                            //bind child object.
                            VocCardObject.DriverVocCardId = VocCard.DriverVocCardId;
                            VocCardObject.DriverId = VocCard.DriverId;
                            VocCardObject.CardNumber = VocCard.CardNumber;
                            VocCardObject.RTONumber = VocCard.RTONumber;
                            VocCardObject.IssueDate = VocCard.IssueDate;
                            VocCardObject.Notes = VocCard.Notes;

                            //set VocCard detail in list.
                            VocCardList.Add(VocCardObject);
                        }
                    }

                    //bind dynamic list property.
                    dynamic InductionCardList = new List<ExpandoObject>();

                    // checking count not equal to zero
                    if (DriverDetail.DriverInductionCards.Count > 0)
                    {
                        foreach (var InductionCard in DriverDetail.DriverInductionCards)
                        {
                            //bind dynamic property.
                            dynamic InductionCardObject = new ExpandoObject();
                            //bind child object.
                            InductionCardObject.DriverInductionCardId = InductionCard.DriverInductionCardId;
                            InductionCardObject.DriverId = InductionCard.DriverId;
                            InductionCardObject.CardNumber = InductionCard.CardNumber;
                            InductionCardObject.SiteCost = InductionCard.SiteCost;
                            InductionCardObject.IssueDate = InductionCard.IssueDate;
                            InductionCardObject.ExpiryDate = InductionCard.ExpiryDate;
                            InductionCardObject.Notes = InductionCard.Notes;

                            //set InductionCard detail in list.
                            InductionCardList.Add(InductionCardObject);
                        }
                    }

                    //bind dynamic list property.
                    dynamic AnonymousList = new List<ExpandoObject>();

                    // checking count not equal to zero
                    if (DriverDetail.AnonymousFields.Count > 0)
                    {
                        foreach (var AnonymousCard in DriverDetail.AnonymousFields)
                        {
                            //bind dynamic property.
                            dynamic AnonymousCardObject = new ExpandoObject();
                            //bind child object.
                            AnonymousCardObject.AnonymousFieldId = AnonymousCard.AnonymousFieldId;
                            AnonymousCardObject.DriverId = AnonymousCard.DriverId;
                            AnonymousCardObject.Title = AnonymousCard.Title;
                            AnonymousCardObject.OtherOne = AnonymousCard.Other1;
                            AnonymousCardObject.OtherTwo = AnonymousCard.Other2;
                            AnonymousCardObject.IssueDate = AnonymousCard.IssueDate;
                            AnonymousCardObject.ExpiryDate = AnonymousCard.ExpiryDate;
                            AnonymousCardObject.Notes = AnonymousCard.Notes;

                            //set Anonymous detail in list.
                            AnonymousList.Add(AnonymousCardObject);
                        }
                    }

                    Driver.DriverWhiteCard = WhiteCardList;
                    Driver.DriverVocCard = VocCardList;
                    Driver.DriverInductionCard = InductionCardList;
                    Driver.AnonymousField = AnonymousList;

                    //employee service response
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Get driver Details", Succeeded = true, DataObject = Driver, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// This method use for add driver into driver entity.
        /// </summary>
        /// <param name="DriverDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddDriver")]
        public HttpResponseMessage AddDriver()
        {
            try
            {
                // defining variables globally
                var LoadDocketResponse = false;
                DocumentDataModel DocumentData = new DocumentDataModel();
                var DocumentId = (Guid?)null;
                DriverDataModel DriverDataModel = new DriverDataModel();
                var SuccessResponse = false;
                List<DriverWhiteCardDataModel> ListDriverWhiteCard = new List<DriverWhiteCardDataModel>();
                List<DriverVocCardDataModel> ListDriverVocCard = new List<DriverVocCardDataModel>();
                List<AnonymousFieldDataModel> ListAnonymousField = new List<AnonymousFieldDataModel>();
                List<DriverInductionCardDataModel> ListDriverInductionCard = new List<DriverInductionCardDataModel>();

                // Define the path where we want to save the files.
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                var Form = System.Web.HttpContext.Current.Request.Form;

                // setting form data into entity
                DriverDataModel.FleetRegistrationId = Form["model[FleetRegistrationId]"];
                DriverDataModel.CountryId = Form["model[CountryId]"];
                DriverDataModel.StateId = Form["model[StateId]"];
                DriverDataModel.EmploymentCategoryId = Form["model[EmploymentCategoryId]"];
                DriverDataModel.LicenseClassId = Form["model[LicenseClassId]"];
                DriverDataModel.FirstName = Form["model[FirstName]"];
                DriverDataModel.LastName = Form["model[LastName]"];
                DriverDataModel.Email = Form["model[Email]"];
                DriverDataModel.MobileNumber = Form["model[MobileNumber]"];
                DriverDataModel.AddressLine1 = Form["model[AddressLine1]"];
                DriverDataModel.AddressLine2 = Form["model[AddressLine2]"];
                DriverDataModel.Suburb = Form["model[Suburb]"];
                DriverDataModel.PostCode = Form["model[ZipCode]"];
                DriverDataModel.CardNumber = Form["model[CardNumber]"];
                DriverDataModel.LicenseNumber = Form["model[LicenseNumber]"];
                DriverDataModel.ExpiryDate = Form["model[ExpiryDate]"];
                DriverDataModel.BaseRate = Convert.ToDecimal(Form["model[BaseRate]"]);
                DriverDataModel.Shift = Form["model[Shift]"];
                DriverDataModel.IsActive = Convert.ToBoolean(Form["model[IsActive]"]);
                DriverDataModel.CreatedBy = Form["model[CreatedBy]"] != null ? new Guid(Form["model[CreatedBy]"]) : (Guid?)null;
                DriverDataModel.EditedBy = Form["model[EditedBy]"] != null ? new Guid(Form["model[EditedBy]"]) : (Guid?)null;
                DriverDataModel.ImageBase64 = Form["model[ImageBase64]"];
                DriverDataModel.AnnualLeaveBalance = Convert.ToInt32(Form["model[AnnualLeaveBalance]"]);
                DriverDataModel.SickLeaveBalance = Convert.ToInt32(Form["model[SickLeaveBalance]"]);
                DriverDataModel.StatusLookupId = Form["model[StatusLookupId]"];
                DriverDataModel.City = Form["model[City]"];
                DriverDataModel.TypeFromDate = Form["model[TypeFromDate]"];
                DriverDataModel.TypeToDate = Form["model[TypeToDate]"];
                DriverDataModel.TypeNote = Form["model[TypeNote]"];
                DriverDataModel.LeaveFromDate = Form["model[LeaveFromDate]"];
                DriverDataModel.LeaveToDate = Form["model[LeaveToDate]"];
                DriverDataModel.LeaveNote = Form["model[LeaveNote]"];


                int DriverWhiteCardCount = Convert.ToInt32(Form["model[DriverWhiteCardCount]"]);
                int DriverVocCardCount = Convert.ToInt32(Form["model[DriverVocCardCount]"]);
                int AnonymousFieldCount = Convert.ToInt32(Form["model[AnonymousFieldCount]"]);
                int DriverInductionCardCount = Convert.ToInt32(Form["model[DriverInductionCardCount]"]);

                // check if DriverWhiteCardCount is greater than 0
                if (DriverWhiteCardCount > 0)
                {
                    int i = 0;

                    for (i = 0; i < DriverWhiteCardCount; i++)
                    {
                        DriverWhiteCardDataModel ObjectDriverWhiteCard = new DriverWhiteCardDataModel();

                        //ObjectDriverWhiteCard.DriverId = new Guid(Form["model[DriverWhiteCard][" + i + "][DriverId]"]);
                        ObjectDriverWhiteCard.CardNumber = Form["model[DriverWhiteCard][" + i + "][CardNumber]"];
                        ObjectDriverWhiteCard.IssueDate = Convert.ToDateTime(Form["model[DriverWhiteCard][" + i + "][IssueDate]"]);
                        ObjectDriverWhiteCard.Notes = Form["model[DriverWhiteCard][" + i + "][Notes]"];
                        ObjectDriverWhiteCard.IsActive = Convert.ToBoolean(Form["model[DriverWhiteCard][" + i + "][IsActive]"]);
                        ListDriverWhiteCard.Add(ObjectDriverWhiteCard);
                    }
                }

                // check if DriverVocCardCount is greater than 0
                if (DriverVocCardCount > 0)
                {
                    int i = 0;

                    for (i = 0; i < DriverVocCardCount; i++)
                    {
                        DriverVocCardDataModel ObjectDriverVocCard = new DriverVocCardDataModel();

                        //ObjectDriverVocCard.DriverId = new Guid(Form["model[DriverVocCard][" + i + "][DriverId]"]);
                        ObjectDriverVocCard.CardNumber = Form["model[DriverVocCard][" + i + "][CardNumber]"];
                        ObjectDriverVocCard.RTONumber = Form["model[DriverVocCard][" + i + "][RTONumber]"];
                        ObjectDriverVocCard.IssueDate = Convert.ToDateTime(Form["model[DriverVocCard][" + i + "][IssueDate]"]);
                        ObjectDriverVocCard.Notes = Form["model[DriverVocCard][" + i + "][Notes]"];
                        ObjectDriverVocCard.IsActive = Convert.ToBoolean(Form["model[DriverVocCard][" + i + "][IsActive]"]);
                        ListDriverVocCard.Add(ObjectDriverVocCard);
                    }
                }

                // check if AnonymousFieldCount is greater than 0
                if (AnonymousFieldCount > 0)
                {
                    int i = 0;

                    for (i = 0; i < DriverWhiteCardCount; i++)
                    {
                        AnonymousFieldDataModel ObjectAnonymousField = new AnonymousFieldDataModel();

                        //ObjectAnonymousField.DriverId = new Guid(Form["model[AnonymousField][" + i + "][DriverId]"]);
                        ObjectAnonymousField.Title = Form["model[AnonymousField][" + i + "][Title]"];
                        ObjectAnonymousField.Other1 = Form["model[AnonymousField][" + i + "][OtherOne]"];
                        ObjectAnonymousField.Other2 = Form["model[AnonymousField][" + i + "][OtherTwo]"];
                        ObjectAnonymousField.IssueDate = Form["model[AnonymousField][" + i + "][IssueDate]"];
                        ObjectAnonymousField.ExpiryDate = Form["model[AnonymousField][" + i + "][ExpiryDate]"];
                        ObjectAnonymousField.Notes = Form["model[AnonymousField][" + i + "][Notes]"];
                        ObjectAnonymousField.IsActive = Convert.ToBoolean(Form["model[AnonymousField][" + i + "][IsActive]"]);
                        ListAnonymousField.Add(ObjectAnonymousField);
                    }
                }

                // check if DriverInductionCardCount is greater than 0
                if (DriverInductionCardCount > 0)
                {
                    int i = 0;

                    for (i = 0; i < DriverWhiteCardCount; i++)
                    {
                        DriverInductionCardDataModel ObjectDriverInductionCard = new DriverInductionCardDataModel();

                        //ObjectDriverInductionCard.DriverId = new Guid(Form["model[DriverInductionCard][" + i + "][DriverId]"]);
                        ObjectDriverInductionCard.CardNumber = Form["model[DriverInductionCard][" + i + "][CardNumber]"];
                        ObjectDriverInductionCard.SiteCost = Form["model[DriverInductionCard][" + i + "][SiteCost]"];
                        ObjectDriverInductionCard.IssueDate = Convert.ToDateTime(Form["model[DriverInductionCard][" + i + "][IssueDate]"]);
                        ObjectDriverInductionCard.ExpiryDate = Convert.ToDateTime(Form["model[DriverInductionCard][" + i + "][ExpiryDate]"]);
                        ObjectDriverInductionCard.Notes = Form["model[DriverInductionCard][" + i + "][Notes]"];
                        ObjectDriverInductionCard.IsActive = Convert.ToBoolean(Form["model[DriverInductionCard][" + i + "][IsActive]"]);
                        ListDriverInductionCard.Add(ObjectDriverInductionCard);

                    }
                }

                //add driver detail into database
                var DriverId = DriversService.AddDriver(DriverDataModel);

                /// checking Driver id not equal to null
                if (ServiceHelper.IsGuid(DriverId))
                {
                    #region Save data in document entity from Base64.

                    if (DriverDataModel.ImageBase64 != "")
                    {
                        String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                        String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

                        //bind file data in a document model.
                        DocumentData.Id = Guid.NewGuid().ToString();
                        DocumentData.OriginalName = "Image";
                        DocumentData.Name = DriverId + "_DriverProfileImage_" + ".png";
                        DocumentData.Title = "Images";
                        DocumentData.Description = string.Empty;
                        DocumentData.Tags = string.Empty;
                        DocumentData.URL = strUrl + "/Uploads/" + DriverId + "_DriverProfileImage_" + ".png";//provider.FileData.FirstOrDefault().LocalFileName;//
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
                            // updating proficPicId in docket table
                            var UpdateDocument = DriversService.UpdateDocumentId(new Guid(DriverId), DocumentId.Value);

                            // uploading image on Uploads folder
                            var ImageUploadStatus = UploadImage.base64ToImage(DriverDataModel.ImageBase64, DriverId + "_DriverProfileImage_" + ".png");
                        }
                    }

                    #endregion

                    #region Save Image data in document entity using multipart.

                    // multi-part upload code
                    int iUploadedCnt = 0;

                    // check the file count.
                    for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                    {
                        System.Web.HttpPostedFile hpf = hfc[iCnt];

                        if (hpf.ContentLength > 0)
                        {
                            // check if the seleted file(S) already exixts in folder. (AVOID DUPLICATE)
                            if (!File.Exists(sPath + Path.GetFileName(DriverId + "_DriverImage_" + iCnt + 1 + "_" + hpf.FileName)))
                            {
                                // SAVE THE FILES IN THE FOLDER.
                                hpf.SaveAs(sPath + Path.GetFileName(DriverId + "_DriverImage_" + iCnt + 1 + "_" + hpf.FileName));
                                iUploadedCnt = iUploadedCnt + 1;

                                // getting base URL of app
                                String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

                                //bind file data in a document model.
                                DocumentData.Id = Guid.NewGuid().ToString();
                                DocumentData.OriginalName = "Image";
                                DocumentData.Name = DriverId + "_DriverImage_" + iCnt + 1 + "_" + hpf.FileName;
                                DocumentData.Title = "Images";
                                DocumentData.Description = string.Empty;
                                DocumentData.Tags = string.Empty;
                                DocumentData.URL = strUrl + "/Uploads/" + DriverId + "_DriverImage_" + iCnt + 1 + "_" + hpf.FileName;//provider.FileData.FirstOrDefault().LocalFileName;//
                                DocumentData.Extension = new System.IO.FileInfo(hpf.FileName).Extension;
                                DocumentData.ThumbnailFileName = "";
                                DocumentData.FileSize = 0;
                                DocumentData.Private = true;

                                //save document data.
                                DocumentId = DocumentService.AddDocument(new Guid(DocumentData.Id), DocumentData.OriginalName, DocumentData.Name, DocumentData.URL, DocumentData.Title, DocumentData.Description, DocumentData.Extension, DocumentData.FileSize, DocumentData.Private, DocumentData.Tags, DocumentData.ThumbnailFileName);

                                if (ServiceHelper.IsGuid(DocumentId.ToString()))
                                {
                                    // updating documentId in driver table
                                    var UpdateDocument = DriversService.UpdateDocumentLicenseId(new Guid(DriverId), DocumentId.ToString());
                                    // checking document status
                                    if (UpdateDocument)
                                    {
                                        SuccessResponse = true;
                                    }
                                    else
                                    {
                                        SuccessResponse = false;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    // deleting all DriverWhitCard corresponding to driverId
                    var WhiteCardExist = DriverWhiteCardService.DeleteDriverWhiteCardByDriverId(new Guid(DriverId));

                    if (WhiteCardExist)
                    {
                        foreach (var item in ListDriverWhiteCard)
                        {
                            // initializing driverId into DriverWhitCard Model
                            item.DriverId = new Guid(DriverId);

                            // service for adding DriverWhitCard
                            var WhiteCard = DriverWhiteCardService.AddDriverWhiteCard(item);
                        }
                    }

                    // deleting all DriverInductionCard corresponding to driverId
                    var InductionCardExist = DriverInductionCardService.DeleteDriverInductionCardByDriverId(new Guid(DriverId));

                    if (InductionCardExist)
                    {
                        foreach (var item in ListDriverInductionCard)
                        {
                            // initializing driverId into DriverInductionCard Model
                            item.DriverId = new Guid(DriverId);

                            // service for adding DriverInductionCard
                            var WhiteCard = DriverInductionCardService.AddDriverInductionCard(item);
                        }
                    }

                    // deleting all DriverVocCard corresponding to driverId
                    var VocCardExist = DriverVocCardService.DeleteDriverVocCardByDriverId(new Guid(DriverId));

                    if (VocCardExist)
                    {
                        foreach (var item in ListDriverVocCard)
                        {
                            // initializing driverId into DriverVocCard Model
                            item.DriverId = new Guid(DriverId);

                            // service for adding DriverInductionCard
                            var WhiteCard = DriverVocCardService.AddDriverVocCard(item);
                        }
                    }

                    // deleting all Anonymous corresponding to driverId
                    var AnonymousExist = AnonymousFieldService.DeleteAnonymousFieldByDriverId(new Guid(DriverId));

                    if (AnonymousExist)
                    {
                        foreach (var item in ListAnonymousField)
                        {
                            // initializing driverId into AnonymousField Model
                            item.DriverId = new Guid(DriverId);

                            // service for adding AnonymousField
                            var WhiteCard = AnonymousFieldService.AddAnonymousField(item);
                        }
                    }

                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Driver successfully added", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

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
        /// This method use for update record into driver entity.
        /// </summary>
        /// <param name="DriverDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateDriver")]
        public HttpResponseMessage UpdateDriver()
        {
            try
            {
                // defining variables globally
                DocumentDataModel DocumentData = new DocumentDataModel();
                var DocumentId = (Guid?)null;
                var LoadDocketResponse = false;
                DriverDataModel DriverDataModel = new DriverDataModel();
                var SuccessResponse = false;

                List<DriverWhiteCardDataModel> ListDriverWhiteCard = new List<DriverWhiteCardDataModel>();
                List<DriverVocCardDataModel> ListDriverVocCard = new List<DriverVocCardDataModel>();
                List<AnonymousFieldDataModel> ListAnonymousField = new List<AnonymousFieldDataModel>();
                List<DriverInductionCardDataModel> ListDriverInductionCard = new List<DriverInductionCardDataModel>();

                // Define the path where we want to save the files.
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                var Form = System.Web.HttpContext.Current.Request.Form;

                // setting form data into entity
                DriverDataModel.DriverId = new Guid(Form["model[DriverId]"]);
                DriverDataModel.FleetRegistrationId = Form["model[FleetRegistrationId]"] != "null" ? Form["model[FleetRegistrationId]"] : "";
                DriverDataModel.CountryId = Form["model[CountryId]"];
                DriverDataModel.StateId = Form["model[StateId]"];
                DriverDataModel.EmploymentCategoryId = Form["model[EmploymentCategoryId]"];
                DriverDataModel.LicenseClassId = Form["model[LicenseClassId]"];
                DriverDataModel.FirstName = Form["model[FirstName]"];
                DriverDataModel.LastName = Form["model[LastName]"];
                DriverDataModel.Email = Form["model[Email]"];
                DriverDataModel.MobileNumber = Form["model[MobileNumber]"];
                DriverDataModel.AddressLine1 = Form["model[AddressLine1]"];
                DriverDataModel.AddressLine2 = Form["model[AddressLine2]"];
                DriverDataModel.Suburb = Form["model[Suburb]"];
                DriverDataModel.PostCode = Form["model[ZipCode]"];
                DriverDataModel.CardNumber = Form["model[CardNumber]"];
                DriverDataModel.LicenseNumber = Form["model[LicenseNumber]"];
                DriverDataModel.ExpiryDate = Form["model[ExpiryDate]"];
                DriverDataModel.BaseRate = Convert.ToDecimal(Form["model[BaseRate]"]);
                DriverDataModel.Shift = Form["model[Shift]"];
                DriverDataModel.IsActive = Convert.ToBoolean(Form["model[IsActive]"]);
                DriverDataModel.CreatedBy = Form["model[CreatedBy]"] != null ? new Guid(Form["model[CreatedBy]"]) : (Guid?)null;
                DriverDataModel.EditedBy = Form["model[EditedBy]"] != null ? new Guid(Form["model[EditedBy]"]) : (Guid?)null;
                DriverDataModel.ImageBase64 = Form["model[ImageBase64]"];
                DriverDataModel.AnnualLeaveBalance = Convert.ToInt32(Form["model[AnnualLeaveBalance]"]);
                DriverDataModel.SickLeaveBalance = Convert.ToInt32(Form["model[SickLeaveBalance]"]);
                DriverDataModel.StatusLookupId = Form["model[StatusLookupId]"];
                DriverDataModel.City = Form["model[City]"];
                DriverDataModel.TypeFromDate = Form["model[TypeFromDate]"];
                DriverDataModel.TypeToDate = Form["model[TypeToDate]"];
                DriverDataModel.TypeNote = Form["model[TypeNote]"];
                DriverDataModel.LeaveFromDate = Form["model[LeaveFromDate]"];
                DriverDataModel.LeaveToDate = Form["model[LeaveToDate]"];
                DriverDataModel.LeaveNote = Form["model[LeaveNote]"];

                int DriverWhiteCardCount = Convert.ToInt32(Form["model[DriverWhiteCardCount]"]);
                int DriverVocCardCount = Convert.ToInt32(Form["model[DriverVocCardCount]"]);
                int AnonymousFieldCount = Convert.ToInt32(Form["model[AnonymousFieldCount]"]);
                int DriverInductionCardCount = Convert.ToInt32(Form["model[DriverInductionCardCount]"]);

                // check if DriverWhiteCardCount is greater than 0
                if (DriverWhiteCardCount > 0)
                {
                    int i = 0;

                    for (i = 0; i < DriverWhiteCardCount; i++)
                    {
                        DriverWhiteCardDataModel ObjectDriverWhiteCard = new DriverWhiteCardDataModel();

                        //ObjectDriverWhiteCard.DriverId = new Guid(Form["model[DriverWhiteCard][" + i + "][DriverId]"]);
                        ObjectDriverWhiteCard.CardNumber = Form["model[DriverWhiteCard][" + i + "][CardNumber]"];
                        ObjectDriverWhiteCard.IssueDate = Convert.ToDateTime(Form["model[DriverWhiteCard][" + i + "][IssueDate]"]);
                        ObjectDriverWhiteCard.Notes = Form["model[DriverWhiteCard][" + i + "][Notes]"];
                        ObjectDriverWhiteCard.IsActive = Convert.ToBoolean(Form["model[DriverWhiteCard][" + i + "][IsActive]"]);
                        ListDriverWhiteCard.Add(ObjectDriverWhiteCard);
                    }
                }

                // check if DriverVocCardCount is greater than 0
                if (DriverVocCardCount > 0)
                {
                    int i = 0;

                    for (i = 0; i < DriverVocCardCount; i++)
                    {
                        DriverVocCardDataModel ObjectDriverVocCard = new DriverVocCardDataModel();

                        //ObjectDriverVocCard.DriverId = new Guid(Form["model[DriverVocCard][" + i + "][DriverId]"]);
                        ObjectDriverVocCard.CardNumber = Form["model[DriverVocCard][" + i + "][CardNumber]"];
                        ObjectDriverVocCard.RTONumber = Form["model[DriverVocCard][" + i + "][RTONumber]"];
                        ObjectDriverVocCard.IssueDate = Convert.ToDateTime(Form["model[DriverVocCard][" + i + "][IssueDate]"]);
                        ObjectDriverVocCard.Notes = Form["model[DriverVocCard][" + i + "][Notes]"];
                        ObjectDriverVocCard.IsActive = Convert.ToBoolean(Form["model[DriverVocCard][" + i + "][IsActive]"]);
                        ListDriverVocCard.Add(ObjectDriverVocCard);
                    }
                }

                // check if AnonymousFieldCount is greater than 0
                if (AnonymousFieldCount > 0)
                {
                    int i = 0;

                    for (i = 0; i < AnonymousFieldCount; i++)
                    {
                        AnonymousFieldDataModel ObjectAnonymousField = new AnonymousFieldDataModel();

                        //ObjectAnonymousField.DriverId = new Guid(Form["model[AnonymousField][" + i + "][DriverId]"]);
                        ObjectAnonymousField.Title = Form["model[AnonymousField][" + i + "][Title]"];
                        ObjectAnonymousField.Other1 = Form["model[AnonymousField][" + i + "][OtherOne]"];
                        ObjectAnonymousField.Other2 = Form["model[AnonymousField][" + i + "][OtherTwo]"];
                        ObjectAnonymousField.IssueDate = Form["model[AnonymousField][" + i + "][IssueDate]"];
                        ObjectAnonymousField.ExpiryDate = Form["model[AnonymousField][" + i + "][ExpiryDate]"];
                        ObjectAnonymousField.Notes = Form["model[AnonymousField][" + i + "][Notes]"];
                        ObjectAnonymousField.IsActive = Convert.ToBoolean(Form["model[AnonymousField][" + i + "][IsActive]"]);
                        ListAnonymousField.Add(ObjectAnonymousField);
                    }
                }

                // check if DriverInductionCardCount is greater than 0
                if (DriverInductionCardCount > 0)
                {
                    int i = 0;

                    for (i = 0; i < DriverInductionCardCount; i++)
                    {
                        DriverInductionCardDataModel ObjectDriverInductionCard = new DriverInductionCardDataModel();

                        //ObjectDriverInductionCard.DriverId = new Guid(Form["model[DriverInductionCard][" + i + "][DriverId]"]);
                        ObjectDriverInductionCard.CardNumber = Form["model[DriverInductionCard][" + i + "][CardNumber]"];
                        ObjectDriverInductionCard.SiteCost = Form["model[DriverInductionCard][" + i + "][SiteCost]"];
                        ObjectDriverInductionCard.IssueDate = Convert.ToDateTime(Form["model[DriverInductionCard][" + i + "][IssueDate]"]);
                        ObjectDriverInductionCard.ExpiryDate = Convert.ToDateTime(Form["model[DriverInductionCard][" + i + "][ExpiryDate]"]);
                        ObjectDriverInductionCard.Notes = Form["model[DriverInductionCard][" + i + "][Notes]"];
                        ObjectDriverInductionCard.IsActive = Convert.ToBoolean(Form["model[DriverInductionCard][" + i + "][IsActive]"]);
                        ListDriverInductionCard.Add(ObjectDriverInductionCard);
                    }
                }

                // calling update status driver method
                var DriverId = DriversService.UpdateDriver(DriverDataModel);

                /// checking Driver id not equal to null
                if (ServiceHelper.IsGuid(DriverId))
                {
                    #region Save data in document entity from Base64.

                    if (DriverDataModel.ImageBase64 != "")
                    {
                        String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                        String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

                        //bind file data in a document model.
                        DocumentData.Id = Guid.NewGuid().ToString();
                        DocumentData.OriginalName = "Image";
                        DocumentData.Name = DriverId + "_DriverProfileImage_" + ".png";
                        DocumentData.Title = "Images";
                        DocumentData.Description = string.Empty;
                        DocumentData.Tags = string.Empty;
                        DocumentData.URL = strUrl + "/Uploads/" + DriverId + "_DriverProfileImage_" + ".png";//provider.FileData.FirstOrDefault().LocalFileName;//
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
                            // updating proficPicId in docket table
                            var UpdateDocument = DriversService.UpdateDocumentId(new Guid(DriverId), DocumentId.Value);

                            // uploading image on Uploads folder
                            var ImageUploadStatus = UploadImage.base64ToImage(DriverDataModel.ImageBase64, DriverId + "_DriverProfileImage_" + ".png");
                        }
                    }

                    #endregion

                    #region Save Image data in document entity using multipart.

                    // multi-part upload code
                    int iUploadedCnt = 0;

                    // check the file count.
                    for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                    {
                        System.Web.HttpPostedFile hpf = hfc[iCnt];

                        if (hpf.ContentLength > 0)
                        {
                            // check if the seleted file(S) already exixts in folder. (AVOID DUPLICATE)
                            if (!File.Exists(sPath + Path.GetFileName(DriverId + "_DriverImage_" + iCnt + 1 + "_" + hpf.FileName)))
                            {
                                // SAVE THE FILES IN THE FOLDER.
                                hpf.SaveAs(sPath + Path.GetFileName(DriverId + "_DriverImage_" + iCnt + 1 + "_" + hpf.FileName));
                                iUploadedCnt = iUploadedCnt + 1;

                                // getting base URL of app
                                String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

                                //bind file data in a document model.
                                DocumentData.Id = Guid.NewGuid().ToString();
                                DocumentData.OriginalName = "Image";
                                DocumentData.Name = DriverId + "_DriverImage_" + iCnt + 1 + "_" + hpf.FileName;
                                DocumentData.Title = "Images";
                                DocumentData.Description = string.Empty;
                                DocumentData.Tags = string.Empty;
                                DocumentData.URL = strUrl + "/Uploads/" + DriverId + "_DriverImage_" + iCnt + 1 + "_" + hpf.FileName;//provider.FileData.FirstOrDefault().LocalFileName;//
                                DocumentData.Extension = new System.IO.FileInfo(hpf.FileName).Extension;
                                DocumentData.ThumbnailFileName = "";
                                DocumentData.FileSize = 0;
                                DocumentData.Private = true;

                                //save document data.
                                DocumentId = DocumentService.AddDocument(new Guid(DocumentData.Id), DocumentData.OriginalName, DocumentData.Name, DocumentData.URL, DocumentData.Title, DocumentData.Description, DocumentData.Extension, DocumentData.FileSize, DocumentData.Private, DocumentData.Tags, DocumentData.ThumbnailFileName);

                                if (ServiceHelper.IsGuid(DocumentId.ToString()))
                                {
                                    // updating documentId in docket table
                                    var UpdateDocument = DriversService.UpdateDocumentLicenseId(new Guid(DriverId), DocumentId.ToString());
                                    // checking document status
                                    if (UpdateDocument)
                                    {
                                        SuccessResponse = true;
                                    }
                                    else
                                    {
                                        SuccessResponse = false;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    // deleting all DriverWhitCard corresponding to driverId
                    var WhiteCardExist = DriverWhiteCardService.DeleteDriverWhiteCardByDriverId(new Guid(DriverId));

                    if (WhiteCardExist)
                    {
                        foreach (var item in ListDriverWhiteCard)
                        {
                            // initializing driverId into DriverWhitCard Model
                            item.DriverId = new Guid(DriverId);

                            // service for adding DriverWhitCard
                            var WhiteCard = DriverWhiteCardService.AddDriverWhiteCard(item);
                        }
                    }

                    // deleting all DriverInductionCard corresponding to driverId
                    var InductionCardExist = DriverInductionCardService.DeleteDriverInductionCardByDriverId(new Guid(DriverId));

                    if (InductionCardExist)
                    {
                        foreach (var item in ListDriverInductionCard)
                        {
                            // initializing driverId into DriverInductionCard Model
                            item.DriverId = new Guid(DriverId);

                            // service for adding DriverInductionCard
                            var WhiteCard = DriverInductionCardService.AddDriverInductionCard(item);
                        }
                    }

                    // deleting all DriverVocCard corresponding to driverId
                    var VocCardExist = DriverVocCardService.DeleteDriverVocCardByDriverId(new Guid(DriverId));

                    if (VocCardExist)
                    {
                        foreach (var item in ListDriverVocCard)
                        {
                            // initializing driverId into DriverVocCard Model
                            item.DriverId = new Guid(DriverId);

                            // service for adding DriverInductionCard
                            var WhiteCard = DriverVocCardService.AddDriverVocCard(item);
                        }
                    }

                    // deleting all Anonymous corresponding to driverId
                    var AnonymousExist = AnonymousFieldService.DeleteAnonymousFieldByDriverId(new Guid(DriverId));

                    if (AnonymousExist)
                    {
                        foreach (var item in ListAnonymousField)
                        {
                            // initializing driverId into AnonymousField Model
                            item.DriverId = new Guid(DriverId);

                            // service for adding AnonymousField
                            var WhiteCard = AnonymousFieldService.AddAnonymousField(item);
                        }
                    }

                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Driver successfully added", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

                }
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Inserted", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// method to delete Driver by DriverId
        /// </summary>
        /// <param name="DriverId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteDriver/{DriverId}")]
        public HttpResponseMessage DeleteDriver(Guid DriverId)
        {
            try
            {
                var Driver = DriversService.DeleteDriver(DriverId);
                if (Driver)
                {
                    //return Driver delete service 
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Deleted Successfully", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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

        #region EmploymentCategory Api's

        /// <summary>
        /// getting all GetEmployementCategory select list for dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEmploymentCategory")]
        public HttpResponseMessage GetEmploymentCategory()
        {
            try
            {
                //get EmployeeCategory type list
                var EmployeeCategoryTypes = EmploymentCategoryService.GetEmploymentCategoryList();

                //check object
                if (EmployeeCategoryTypes.Count > 0 && EmployeeCategoryTypes != null)
                {
                    List<ExpandoObject> EmployeeCategoryTypeslist = new List<ExpandoObject>();
                    foreach (var EmployeeCategory in EmployeeCategoryTypes)
                    {
                        dynamic EmployeeCategoryDetail = new ExpandoObject();
                        EmployeeCategoryDetail.Category = EmployeeCategory.Text;
                        EmployeeCategoryDetail.EmploymentCategoryId = EmployeeCategory.Value;
                        EmployeeCategoryTypeslist.Add(EmployeeCategoryDetail);
                    }
                    //return EmployeeCategory.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "EmployeeCategory List", Succeeded = true, DataObject = new ExpandoObject(), DataList = EmployeeCategoryTypeslist, ErrorInfo = "" });
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

        #region StatusLookUp Api's

        /// <summary>
        /// getting all StatusLookUp select list for dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetStatusLookup")]
        public HttpResponseMessage GetStatusLookup()
        {
            try
            {
                //get StatusLookup type list
                var StatusLookupTypes = StatusLookupService.GetStatusLookupList();

                //check object
                if (StatusLookupTypes.Count > 0 && StatusLookupTypes != null)
                {
                    List<ExpandoObject> StatusLookupTypeslist = new List<ExpandoObject>();
                    foreach (var StatusLookup in StatusLookupTypes)
                    {
                        dynamic StatusLookupDetail = new ExpandoObject();
                        StatusLookupDetail.Status = StatusLookup.Text;
                        StatusLookupDetail.StatusId = StatusLookup.Value;
                        StatusLookupTypeslist.Add(StatusLookupDetail);
                    }
                    //return StatusLookup.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "StatusLookup List", Succeeded = true, DataObject = new ExpandoObject(), DataList = StatusLookupTypeslist, ErrorInfo = "" });
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

        #region LicenseClass Api's

        /// <summary>
        /// getting all LicenseClass select list for dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetLicenseClass")]
        public HttpResponseMessage GetLicenseClass()
        {
            try
            {
                //get LicenseClass type list
                var LicenseClassTypes = LicenseClassService.GetLicenseClassList();

                //check object
                if (LicenseClassTypes.Count > 0 && LicenseClassTypes != null)
                {
                    List<ExpandoObject> LicenseClassTypeslist = new List<ExpandoObject>();
                    foreach (var LicenseClass in LicenseClassTypes)
                    {
                        dynamic LicenseClassDetail = new ExpandoObject();
                        LicenseClassDetail.LicenseClass = LicenseClass.Text;
                        LicenseClassDetail.LicenseClassId = LicenseClass.Value;
                        LicenseClassTypeslist.Add(LicenseClassDetail);
                    }
                    //return LicenseClass.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "LicenseClass List", Succeeded = true, DataObject = new ExpandoObject(), DataList = LicenseClassTypeslist, ErrorInfo = "" });
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

        #region Subcontractor Api's

        /// <summary>
        /// getting all contractor select list for dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSubcontractor")]
        public HttpResponseMessage GetSubcontractor()
        {
            try
            {
                //get Subcontractor type list
                var Subcontractors = SubcontractorService.GetSubcontractor();

                //check object
                if (Subcontractors.Count > 0 && Subcontractors != null)
                {
                    //return of country service
                    return this.Request.CreateResponse(HttpStatusCode.OK, Subcontractors);
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

    }
}
