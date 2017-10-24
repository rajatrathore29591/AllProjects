using System.Web.Http;
using BorgCivil.Service;
using System.Net.Http;
using System.Dynamic;
using System.Collections.Generic;
using System.Net;
using BorgCivil.Utils.Models;
using System;
using System.Web.Http.Cors;
using BorgCivil.Utils;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Linq;

namespace BorgCivil.MVCWeb.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Site")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SiteApiController : BaseController
    {

        #region Dependencies Injection with initialization

        //Initialized interface object. 
        ICustomerService CustomerService;
        ISitesService SiteService;
        ISupervisorService SupervisorService;
        IGatesService GateService;
        IDocumentService DocumentService;

        // Constructor of Site Api Controller 
        public SiteApiController(ICustomerService _CustomerService, ISitesService _SiteService, ISupervisorService _SupervisorService, IGatesService _GateService, IDocumentService _DocumentService)
        {
            CustomerService = _CustomerService;
            SiteService = _SiteService;
            SupervisorService = _SupervisorService;
            GateService = _GateService;
            DocumentService = _DocumentService;
        }

        #endregion

        #region Site Api's
        /// <summary>
        /// Get all sites by customerId
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSitesByCustomerId/{CustomerId}")]
        public HttpResponseMessage GetSitesByCustomerId(string CustomerId)
        {
            try
            {
                //get site list by customer id
                var Sites = SiteService.GetSitesListByCustomerId(new Guid(CustomerId));

                //check object
                if (Sites.Count > 0 && Sites != null)
                {
                    List<ExpandoObject> Sitelist = new List<ExpandoObject>();
                    foreach (var Site in Sites)
                    {
                        dynamic SiteDetail = new ExpandoObject();
                        SiteDetail.SiteName = Site.Text;
                        SiteDetail.SiteId = Site.Value;
                        Sitelist.Add(SiteDetail);
                    }
                    //return Site service for get site type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Site List", Succeeded = true, DataObject = new ExpandoObject(), DataList = Sitelist, ErrorInfo = "" });
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
        /// this method is for getting all Site 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllSite/{CustomerId}")]
        public HttpResponseMessage GetAllSite(string CustomerId)
        {
            try
            {
                //get Site list 
                var SiteCollection = SiteService.GetAllSites().OrderByDescending(x => x.CreatedDate).ToList();

                if (ServiceHelper.IsGuid(CustomerId))
                {
                    SiteCollection = SiteService.GetAllSites().Where(x => x.CustomerId == new Guid(CustomerId)).ToList();
                }
                //check object
                if (SiteCollection.Count > 0 && SiteCollection != null)
                {
                    //dynamic list.
                    dynamic SiteList = new List<ExpandoObject>();

                    //return response from Site service.
                    foreach (var SiteDetail in SiteCollection)
                    {
                        //bind dynamic property.
                        dynamic SiteDetailList = new ExpandoObject();

                        //map ids
                        SiteDetailList.SiteId = SiteDetail.SiteId;
                        SiteDetailList.CustomerId = SiteDetail.CustomerId;
                        SiteDetailList.CustomerName = SiteDetail.Customer.CustomerName;
                        SiteDetailList.SiteName = SiteDetail.SiteName;
                        SiteDetailList.SiteDetail = SiteDetail.SiteDetail;
                        SiteDetailList.PoNumber = SiteDetail.PoNumber;
                        SiteDetailList.FuelIncluded = SiteDetail.FuelIncluded;
                        SiteDetailList.TollTax = SiteDetail.TollTax;
                        SiteDetailList.CreditTermAgreed = SiteDetail.CreditTermAgreed;

                        ////set Site values in list.
                        SiteList.Add(SiteDetailList);
                    }

                    //return user service for get organasation.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Site List", Succeeded = true, DataObject = new ExpandoObject(), DataList = SiteList, ErrorInfo = "" });
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
        /// this method is used for getting GetSite detail
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSiteDetail/{SiteId}")]
        public HttpResponseMessage GetSiteDetail(Guid SiteId)
        {
            try
            {
                //check is valid SiteId.
                if (!ServiceHelper.IsGuid((string)SiteId.ToString()))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid FleetRegistrationId" });
                }

                //getting Site detail by SiteId
                var SiteDetail = SiteService.GetSitesBySiteId(SiteId);

                //check service response.
                if (SiteDetail != null)
                {
                    //bind dynamic property.
                    dynamic Site = new ExpandoObject();
                    Site.SiteId = SiteDetail.SiteId;
                    Site.CustomerId = SiteDetail.CustomerId;
                    Site.CustomerName = SiteDetail.Customer.CustomerName;
                    Site.PoNumber = SiteDetail.PoNumber;
                    Site.SiteName = SiteDetail.SiteName;
                    Site.SiteDetail = SiteDetail.SiteDetail;
                    Site.FuelIncluded = SiteDetail.FuelIncluded;
                    Site.TollTax = SiteDetail.TollTax;
                    Site.CreditTermAgreed = SiteDetail.CreditTermAgreed;
                    Site.IsActive = SiteDetail.IsActive;
                    Site.CreatedDate = SiteDetail.CreatedDate;
                    Site.Image = SiteDetail.Document != null ? SiteDetail.Document.Name : "";

                    //site service response
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Get Site Details", Succeeded = true, DataObject = Site, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// This method use for add Site into Site entity.
        /// </summary>
        /// <param name="SiteDataModel"></param>
        /// <returns></returns>s
        [HttpPost]
        [Route("AddSite")]
        public HttpResponseMessage AddSite()
        {
            try
            {
                // defining variables globally
                SiteDataModel SiteDataModel = new SiteDataModel();
                DocumentDataModel DocumentData = new DocumentDataModel();
                var DocumentId = (Guid?)null;
                var SuccessResponse = false;
                string filePath = string.Empty;

                // Define the path where we want to save the files.
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                var Form = System.Web.HttpContext.Current.Request.Form;

                // setting form data into entity
                SiteDataModel.CustomerId = Form["model[CustomerId]"];
                SiteDataModel.SiteName = Form["model[SiteName]"];
                SiteDataModel.SiteDetail = Form["model[SiteDetail]"];
                SiteDataModel.PoNumber = Form["model[PoNumber]"];
                SiteDataModel.FuelIncluded = Convert.ToBoolean(Form["model[FuelIncluded]"]);
                SiteDataModel.TollTax = Convert.ToBoolean(Form["model[TollTax]"]);
                SiteDataModel.CreditTermAgreed = Form["model[CreditTermAgreed]"];
                SiteDataModel.IsActive = Convert.ToBoolean(Form["model[IsActive]"]);
                SiteDataModel.CreatedBy = Form["model[CreatedBy]"] != "" ? new Guid(Form["model[CreatedBy]"]) : (Guid.Empty);
                SiteDataModel.EditedBy = Form["model[EditedBy]"] != "" ? new Guid(Form["model[EditedBy]"]) : (Guid.Empty);

                //add site detail into database
                var SiteId = SiteService.AddSite(SiteDataModel);

                /// checking SiteId not equal to null
                if (ServiceHelper.IsGuid(SiteId))
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
                            if (!File.Exists(sPath + Path.GetFileName(SiteId + "_Site_" + iCnt + 1 + "_" + new FileInfo(hpf.FileName).Extension)))
                            {
                                filePath = HostingEnvironment.MapPath("~/Uploads/");
                                if (File.Exists(filePath + SiteId + "_Site_" + iCnt + 1 + "_" + new FileInfo(hpf.FileName).Extension))
                                {
                                    System.IO.File.Delete((filePath + SiteId + "_Site_" + iCnt + 1 + "_" + new FileInfo(hpf.FileName).Extension));
                                }

                                // SAVE THE FILES IN THE FOLDER.
                                hpf.SaveAs(sPath + Path.GetFileName(SiteId + "_Site_" + iCnt + 1 + "_" + new FileInfo(hpf.FileName).Extension));
                                iUploadedCnt = iUploadedCnt + 1;

                                // getting base URL of app
                                String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

                                //bind file data in a document model.
                                DocumentData.Id = Guid.NewGuid().ToString();
                                DocumentData.OriginalName = "Image";
                                DocumentData.Name = SiteId + "_Site_" + iCnt + 1 + "_" + new FileInfo(hpf.FileName).Extension;
                                DocumentData.Title = "Images";
                                DocumentData.Description = string.Empty;
                                DocumentData.Tags = string.Empty;
                                DocumentData.URL = strUrl + "/Uploads/" + SiteId + "_Site_" + iCnt + 1 + "_" + new FileInfo(hpf.FileName).Extension;//provider.FileData.FirstOrDefault().LocalFileName;//
                                DocumentData.Extension = new FileInfo(hpf.FileName).Extension;
                                DocumentData.ThumbnailFileName = "";
                                DocumentData.FileSize = 0;
                                DocumentData.Private = true;

                                //save document data.
                                DocumentId = DocumentService.AddDocument(new Guid(DocumentData.Id), DocumentData.OriginalName, DocumentData.Name, DocumentData.URL, DocumentData.Title, DocumentData.Description, DocumentData.Extension, DocumentData.FileSize, DocumentData.Private, DocumentData.Tags, DocumentData.ThumbnailFileName);

                                if (ServiceHelper.IsGuid(DocumentId.ToString()))
                                {
                                    // updating documentId in docket table
                                    var UpdateDocument = SiteService.UpdateDocumentId(new Guid(SiteId), DocumentId.Value);
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
        /// method to add record in site,gate and supervisor
        /// </summary>
        /// <param name="SiteDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddCustomerSite")]
        public HttpResponseMessage AddCustomerSite()
        {
            try
            {
                // defining variables globally
                DocumentDataModel DocumentData = new DocumentDataModel();
                var DocumentId = (Guid?)null;
                SiteDataModel SiteDataModel = new SiteDataModel();
                var SuccessResponse = false;
                List<SupervisorDataModel> ListSupervisor = new List<SupervisorDataModel>();
                List<GateDataModel> ListGate = new List<GateDataModel>();

                // Define the path where we want to save the files.
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                var Form = System.Web.HttpContext.Current.Request.Form;

                // setting form data into entity
                SiteDataModel.CustomerId = Form["model[CustomerId]"];
                SiteDataModel.SiteName = Form["model[SiteName]"];
                SiteDataModel.SiteDetail = Form["model[SiteDetail]"];
                SiteDataModel.PoNumber = Form["model[PoNumber]"];
                SiteDataModel.CreditTermAgreed = Form["model[CreditTermAgreed]"];
                SiteDataModel.FuelIncluded = Convert.ToBoolean(Form["model[FuelIncluded]"]);
                SiteDataModel.TollTax = Convert.ToBoolean(Form["model[TollsIncluded]"]);
                SiteDataModel.IsActive = Convert.ToBoolean(Form["model[IsActive]"]);
                SiteDataModel.CreatedBy = Form["model[CreatedBy]"] != null ? new Guid(Form["model[CreatedBy]"]) : (Guid?)null;
                SiteDataModel.EditedBy = Form["model[EditedBy]"] != null ? new Guid(Form["model[EditedBy]"]) : (Guid?)null;

                int SupervisorCount = Convert.ToInt32(Form["model[SupervisorCount]"]);
                int GateCount = Convert.ToInt32(Form["model[GateCount]"]);

                ////add site detail into database
                var SiteId = SiteService.AddSite(SiteDataModel);

                if (ServiceHelper.IsGuid(SiteId))
                {
                    // check if SupervisorCount is greater than 0
                    if (SupervisorCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < SupervisorCount; i++)
                        {
                            SupervisorDataModel ObjectSupervisor = new SupervisorDataModel();

                            ObjectSupervisor.SiteId = new Guid(SiteId);
                            ObjectSupervisor.SupervisorName = Form["model[SupervisorList][" + i + "][SupervisorName]"];
                            ObjectSupervisor.Email = Form["model[SupervisorList][" + i + "][SupervisorEmail]"];
                            ObjectSupervisor.MobileNumber = Form["model[SupervisorList][" + i + "][SupervisorMobile]"];
                            ObjectSupervisor.IsActive = Convert.ToBoolean(Form["model[SupervisorList][" + i + "][IsActive]"]);
                            ListSupervisor.Add(ObjectSupervisor);
                        }
                    }

                    // check if GateCount is greater than 0
                    if (GateCount > 0)
                    {
                        int i = 0;
                        for (i = 0; i < GateCount; i++)
                        {
                            GateDataModel ObjectGate = new GateDataModel();

                            ObjectGate.SiteId = new Guid(SiteId);
                            ObjectGate.GateNumber = Form["model[GateList][" + i + "][GateNumber]"];
                            ObjectGate.TipOffRate = Convert.ToDecimal(Form["model[GateList][" + i + "][TipOffRate]"]);
                            ObjectGate.TippingSite = Form["model[GateList][" + i + "][TippingSite]"];
                            ObjectGate.IsActive = Convert.ToBoolean(Form["model[DriverVocCard][" + i + "][IsActive]"]);
                            ListGate.Add(ObjectGate);
                        }
                    }

                }

                /// checking SiteId not equal to null
                if (ServiceHelper.IsGuid(SiteId))
                {
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
                            if (!File.Exists(sPath + Path.GetFileName(SiteId + "_SiteImage_" + iCnt + 1 + "_" + hpf.FileName)))
                            {
                                // SAVE THE FILES IN THE FOLDER.
                                hpf.SaveAs(sPath + Path.GetFileName(SiteId + "_SiteImage_" + iCnt + 1 + "_" + hpf.FileName));
                                iUploadedCnt = iUploadedCnt + 1;

                                // getting base URL of app
                                String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

                                //bind file data in a document model.
                                DocumentData.Id = Guid.NewGuid().ToString();
                                DocumentData.OriginalName = "Image";
                                DocumentData.Name = SiteId + "_SiteImage_" + iCnt + 1 + "_" + hpf.FileName;
                                DocumentData.Title = "Images";
                                DocumentData.Description = string.Empty;
                                DocumentData.Tags = string.Empty;
                                DocumentData.URL = strUrl + "/Uploads/" + SiteId + "_SiteImage_" + iCnt + 1 + "_" + hpf.FileName;//provider.FileData.FirstOrDefault().LocalFileName;//
                                DocumentData.Extension = new System.IO.FileInfo(hpf.FileName).Extension;
                                DocumentData.ThumbnailFileName = "";
                                DocumentData.FileSize = 0;
                                DocumentData.Private = true;

                                //save document data.
                                DocumentId = DocumentService.AddDocument(new Guid(DocumentData.Id), DocumentData.OriginalName, DocumentData.Name, DocumentData.URL, DocumentData.Title, DocumentData.Description, DocumentData.Extension, DocumentData.FileSize, DocumentData.Private, DocumentData.Tags, DocumentData.ThumbnailFileName);

                                if (ServiceHelper.IsGuid(DocumentId.ToString()))
                                {
                                    // updating documentId in driver table
                                    var UpdateDocument = SiteService.UpdateDocumentId(new Guid(SiteId), DocumentId.Value);

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

                    if (ServiceHelper.IsGuid(SiteId))
                    {
                        foreach (var Item in ListSupervisor)
                        {
                            Item.SiteId = new Guid(SiteId);

                            // adding supervisor data
                            var Supervisor = SupervisorService.AddSupervisor(Item);
                        }

                        foreach (var Item in ListGate)
                        {
                            Item.SiteId = new Guid(SiteId);

                            // adding gate data
                            var Gate = GateService.AddGate(Item);
                        }
                    }

                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Record inserted successfully", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                ////return service for get user.
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Record not inserted successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
            }
            catch (Exception ex)
            {
                //// handel exception log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        /// <summary>
        /// This method use for update site into site entity.
        /// </summary>
        /// <param name="SiteDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateSite")]
        public HttpResponseMessage UpdateSite()
        {
            try
            {
                // defining variables globally
                SiteDataModel SiteDataModel = new SiteDataModel();
                DocumentDataModel DocumentData = new DocumentDataModel();
                var DocumentId = (Guid?)null;
                var SuccessResponse = false;
                var DocName = string.Empty;
                string filePath = string.Empty;

                // Define the path where we want to save the files.
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                var Form = System.Web.HttpContext.Current.Request.Form;

                // setting form data into entity
                SiteDataModel.SiteId = Form["model[SiteId]"];
                SiteDataModel.CustomerId = Form["model[CustomerId]"];
                SiteDataModel.SiteName = Form["model[SiteName]"];
                SiteDataModel.SiteDetail = Form["model[SiteDetail]"];
                SiteDataModel.PoNumber = Form["model[PoNumber]"];
                SiteDataModel.FuelIncluded = Convert.ToBoolean(Form["model[FuelIncluded]"]);
                SiteDataModel.TollTax = Convert.ToBoolean(Form["model[TollTax]"]);
                SiteDataModel.CreditTermAgreed = Form["model[CreditTermAgreed]"];
                SiteDataModel.IsActive = Convert.ToBoolean(Form["model[IsActive]"]);
                SiteDataModel.CreatedBy = Form["model[CreatedBy]"] != "" ? new Guid(Form["model[CreatedBy]"]) : (Guid.Empty);
                SiteDataModel.EditedBy = Form["model[EditedBy]"] != "" ? new Guid(Form["model[EditedBy]"]) : (Guid.Empty);

                //add Site detail into database
                var SiteId = SiteService.UpdateSite(SiteDataModel);

                /// checking SiteId not equal to null
                if (ServiceHelper.IsGuid(SiteId))
                {
                    #region Save data in document entity.

                    // multi-part upload code
                    int iUploadedCnt = 0;
                    int Count = 1;

                    // check dcument exist or not
                    var DocumentIds = SiteService.GetSitesBySiteId(new Guid(SiteId)).DocumentId;

                    // check the file count.
                    for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                    {
                        System.Web.HttpPostedFile hpf = hfc[iCnt];

                        if (hpf.ContentLength > 0)
                        {
                            filePath = HostingEnvironment.MapPath("~/Uploads/");
                            if (File.Exists(filePath + SiteId + "_Site_" + Count + "_" + new FileInfo(hpf.FileName).Extension))
                            {
                                //System.IO.File.Delete((filePath + SiteId + "_Site_" + Count + "_" + ".xlsx"));
                                hpf.SaveAs(sPath + Path.GetFileName(SiteId + "_Site_" + Count + "_" + new FileInfo(hpf.FileName).Extension));
                            }

                            // check if the seleted file(S) already exixts in folder. (AVOID DUPLICATE)
                            if (!File.Exists(sPath + Path.GetFileName(SiteId + "_Site_" + Count + "_" + new FileInfo(hpf.FileName).Extension)))
                            {
                                // SAVE THE FILES IN THE FOLDER.
                                hpf.SaveAs(sPath + Path.GetFileName(SiteId + "_Site_" + Count + "_" + new FileInfo(hpf.FileName).Extension));
                                iUploadedCnt = iUploadedCnt + 1;

                                // getting base URL of app
                                String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

                                //bind file data in a document model.
                                DocumentData.Id = Guid.NewGuid().ToString();
                                DocumentData.OriginalName = "Image";
                                DocumentData.Name = SiteId + "_Site_" + Count + "_" + new FileInfo(hpf.FileName).Extension;
                                DocumentData.Title = "Images";
                                DocumentData.Description = string.Empty;
                                DocumentData.Tags = string.Empty;
                                DocumentData.URL = strUrl + "/Uploads/" + SiteId + "_Site_" + Count + "_" + new FileInfo(hpf.FileName).Extension;//provider.FileData.FirstOrDefault().LocalFileName;//
                                DocumentData.Extension = new System.IO.FileInfo(hpf.FileName).Extension;
                                DocumentData.ThumbnailFileName = "";
                                DocumentData.FileSize = 0;
                                DocumentData.Private = true;

                                //save document data.
                                DocumentId = DocumentService.AddDocument(new Guid(DocumentData.Id), DocumentData.OriginalName, DocumentData.Name, DocumentData.URL, DocumentData.Title, DocumentData.Description, DocumentData.Extension, DocumentData.FileSize, DocumentData.Private, DocumentData.Tags, DocumentData.ThumbnailFileName);

                                if (ServiceHelper.IsGuid(DocumentId.ToString()))
                                {
                                    // updating documentId in docket table
                                    var UpdateDocument = SiteService.UpdateDocumentId(new Guid(SiteId), DocumentId.Value);
                                    
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
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Record updated successfully", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    //return service for get user.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Record not updated successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// method to delete Site by SiteId
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteSite/{SiteId}")]
        public HttpResponseMessage DeleteSite(Guid SiteId)
        {
            try
            {
                var Site = SiteService.DeleteSite(SiteId);
                if (Site)
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

        #endregion

        #region Supervisor Api's

        /// <summary>
        /// Get all supervisor by siteId
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllSupervisorSelectListBySiteId/{SiteId}")]
        public HttpResponseMessage GetAllSupervisorSelectListBySiteId(string SiteId)
        {
            try
            {
                //get supervisor list by site id
                var Supervisors = SupervisorService.GetSupervisorList(new Guid(SiteId));

                //check object
                if (Supervisors.Count > 0 && Supervisors != null)
                {
                    List<ExpandoObject> Supervisorlist = new List<ExpandoObject>();
                    foreach (var Supervisor in Supervisors)
                    {
                        dynamic SupervisorDetail = new ExpandoObject();
                        SupervisorDetail.SupervisorName = Supervisor.Text;
                        SupervisorDetail.SiteId = Supervisor.Value;
                        Supervisorlist.Add(SupervisorDetail);
                    }
                    //return Site service for get site type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Supervisor List", Succeeded = true, DataObject = new ExpandoObject(), DataList = Supervisorlist, ErrorInfo = "" });
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
        /// this method is for getting all supervisor 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllSupervisor/{SiteId}")]
        public HttpResponseMessage GetAllSupervisor(string SiteId)
        {
            try
            {
                //get supervisor list 
                var SupervisorCollection = SupervisorService.GetAllSupervisor().OrderByDescending(x => x.CreatedDate).ToList();
                if (ServiceHelper.IsGuid(SiteId))
                {
                    //get supervisor list by SiteId
                    SupervisorCollection = SupervisorService.GetAllSupervisor().Where(x => x.SiteId == new Guid(SiteId)).ToList();
                }

                //check object
                if (SupervisorCollection.Count > 0 && SupervisorCollection != null)
                {
                    //dynamic list.
                    dynamic Supervisors = new List<ExpandoObject>();

                    //return response from supervisor service.
                    foreach (var SupervisorDetail in SupervisorCollection)
                    {
                        //bind dynamic property.
                        dynamic Supervisor = new ExpandoObject();

                        //map ids
                        Supervisor.SupervisorId = SupervisorDetail.SupervisorId;
                        Supervisor.SupervisorName = SupervisorDetail.SupervisorName;
                        Supervisor.CustomerName = SupervisorDetail.Site.Customer.CustomerName;
                        Supervisor.SiteName = SupervisorDetail.Site.SiteName;
                        Supervisor.Email = SupervisorDetail.Email;
                        Supervisor.MobileNumber = SupervisorDetail.MobileNumber;

                        //set supervisor values in list.
                        Supervisors.Add(Supervisor);
                    }

                    //return success response of supervisor
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Supervisor List", Succeeded = true, DataObject = new ExpandoObject(), DataList = Supervisors, ErrorInfo = "" });
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
        /// this method is used for getting supervisor detail by supervisorId
        /// </summary>
        /// <param name="SupervisorId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSupervisorBySupervisorId/{SupervisorId}")]
        public HttpResponseMessage GetSupervisorBySupervisorId(Guid SupervisorId)
        {
            try
            {
                //get supervisor list by SupervisorId
                var Supervisors = SupervisorService.GetSupervisorBySupervisorId(SupervisorId);

                //check object
                if (Supervisors != null)
                {
                    //bind dynamic property.
                    dynamic Supervisor = new ExpandoObject();

                    //map ids
                    Supervisor.SupervisorId = Supervisors.SupervisorId;
                    Supervisor.SupervisorName = Supervisors.SupervisorName;
                    Supervisor.Email = Supervisors.Email;
                    Supervisor.MobileNumber = Supervisors.MobileNumber;

                    //return supervisor service for get supervisor.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Supervisor List by Id", Succeeded = true, DataObject = Supervisor, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// This method use for add record into supervisor entity.
        /// </summary>
        /// <param name="SupervisorDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddSupervisor")]
        public HttpResponseMessage AddSupervisor(SupervisorDataModel SupervisorDataModel)
        {
            try
            {
                //add supervisor detail into database
                var SupervisorId = SupervisorService.AddSupervisor(SupervisorDataModel);

                /// checking SupervisorId not equal to null
                if (SupervisorId != null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Supervisor successfully added", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// This method use for add record into supervisor entity.
        /// </summary>
        /// <param name="SupervisorDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateSupervisor")]
        public HttpResponseMessage UpdateSupervisor(SupervisorDataModel SupervisorDataModel)
        {
            try
            {
                //add supervisor detail into database
                var SupervisorId = SupervisorService.UpdateSupervisor(SupervisorDataModel);

                /// checking SupervisorId not equal to null
                if (SupervisorId != null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Supervisor successfully updated", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// method to delete Supervisor by SupervisorId
        /// </summary>
        /// <param name="SupervisorId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteSupervisor/{SupervisorId}")]
        public HttpResponseMessage DeleteSupervisor(Guid SupervisorId)
        {
            try
            {
                var Supervisor = SupervisorService.DeleteSupervisor(SupervisorId);
                if (Supervisor)
                {
                    //return DocketCheckboxList delete service 
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

        #region Gate Api's
        /// <summary>
        /// Get all gates by siteId
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllGateSelectListBySiteId/{SiteId}")]
        public HttpResponseMessage GetAllGateSelectListBySiteId(Guid SiteId)
        {
            try
            {
                //get gate list by site id
                var Gates = GateService.GetGatesList(SiteId);

                //check object
                if (Gates.Count > 0 && Gates != null)
                {
                    List<ExpandoObject> Gatelist = new List<ExpandoObject>();
                    foreach (var Gate in Gates)
                    {
                        dynamic GateDetail = new ExpandoObject();
                        GateDetail.GateName = Gate.Text;
                        GateDetail.GateId = Gate.Value;
                        Gatelist.Add(GateDetail);
                    }
                    //return Site service for get site type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Gate List", Succeeded = true, DataObject = new ExpandoObject(), DataList = Gatelist, ErrorInfo = "" });
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
