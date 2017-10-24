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
using System.Linq;

namespace BorgCivil.MVCWeb.Controllers
{
    //[Authorize]
    [RoutePrefix("api/WorkType")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class WorkTypeApiController : ApiController
    {

        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IWorkTypesService WorkTypeService;

        // Constructor of Work Type Api Controller 
        public WorkTypeApiController(IWorkTypesService _WorkTypeService)
        {
            WorkTypeService = _WorkTypeService;
        }

        #endregion

        #region WorkType Api's

        /// <summary>
        /// getting all worktype select list for dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetWorkTypes")]
        public HttpResponseMessage GetWorkTypes()
        {
            try
            {
                //get work type list
                var WorkTypes = WorkTypeService.GetWorkTypesList();

                //check object
                if (WorkTypes.Count > 0 && WorkTypes != null)
                {
                    List<ExpandoObject> WorkTypelist = new List<ExpandoObject>();
                    foreach (var WorkType in WorkTypes)
                    {
                        dynamic WorkTypeDetail = new ExpandoObject();
                        WorkTypeDetail.Text = WorkType.Text;
                        WorkTypeDetail.Value = WorkType.Value;
                        WorkTypelist.Add(WorkTypeDetail);
                    }
                    //return work type service for get work type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "WorkType List", Succeeded = true, DataObject = new ExpandoObject(), DataList = WorkTypelist, ErrorInfo = "" });
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
        /// this method is for getting all WorkType 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllWorkType")]
        public HttpResponseMessage GetAllWorkType()
        {
            try
            {
                //get work type list 
                var WorkTypeCollection = WorkTypeService.GetAllWorkTypes().OrderByDescending(x => x.CreatedDate).ToList();

                //check object
                if (WorkTypeCollection.Count > 0 && WorkTypeCollection != null)
                {
                    //dynamic list.
                    dynamic WorkTypes = new List<ExpandoObject>();

                    //return response from workType service.
                    foreach (var WorkTypeDetail in WorkTypeCollection)
                    {
                        //bind dynamic property.
                        dynamic WorkType = new ExpandoObject();

                        //map ids
                        WorkType.WorkTypeId = WorkTypeDetail.WorkTypeId;
                        WorkType.Type = WorkTypeDetail.Type;
                        WorkType.CreatedDate = WorkTypeDetail.CreatedDate;
                        WorkType.IsActive = WorkTypeDetail.IsActive;

                        //set WorkType values in list.
                        WorkTypes.Add(WorkType);
                    }

                    //return workType service response
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "WorkType List", Succeeded = true, DataObject = new ExpandoObject(), DataList = WorkTypes, ErrorInfo = "" });
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
        /// this method is used for getting WorkType detail
        /// </summary>
        /// <param name="WorkTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetWorkTypeDetail/{WorkTypeId}")]
        public HttpResponseMessage GetWorkTypeDetail(Guid WorkTypeId)
        {
            try
            {
                //check is valid FleetTypeId.
                if (!ServiceHelper.IsGuid((string)WorkTypeId.ToString()))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid booking fleet id." });
                }

                //getting WorkType detail by WorkTypeId
                var WorkTypeDetail = WorkTypeService.GetWorkTypeByWorkTypeId(WorkTypeId);

                //check service response.
                if (WorkTypeDetail != null)
                {
                    //bind dynamic property.
                    dynamic WorkType = new ExpandoObject();
                    WorkType.WorkTypeId = WorkTypeDetail.WorkTypeId;
                    WorkType.Type = WorkTypeDetail.Type;
                    WorkType.CreatedDate = WorkTypeDetail.CreatedDate;
                    WorkType.IsActive = WorkTypeDetail.IsActive;

                    //call skill service for get skill.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "WorkType details", Succeeded = true, DataObject = WorkType, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// update WorkType by WorkTypeId
        /// </summary>
        /// <param name="WorkTypeDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddWorkType")]
        public HttpResponseMessage AddWorkType(WorkTypeDataModel WorkTypeDataModel)
        {
            try
            {

                //get service response.
                var response = WorkTypeService.AddWorkType(WorkTypeDataModel);

                if (ServiceHelper.IsGuid(response))
                {
                    //return result from service response.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Added successfully", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }

            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception" });

            }
        }

        /// <summary>
        /// update WorkType by WorkTypeId
        /// </summary>
        /// <param name="WorkTypeDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateWorkType")]
        public HttpResponseMessage UpdateWorkType(WorkTypeDataModel WorkTypeDataModel)
        {
            try
            {
                if (ServiceHelper.IsGuid(WorkTypeDataModel.WorkTypeId.ToString()))
                {
                    //get service response.
                    var response = WorkTypeService.UpdateWorkType(WorkTypeDataModel);

                    //return result from service response.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Added successfully", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    //return result from service response.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Something went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception" });

            }
        }

        /// <summary>
        /// method to delete workType by workTypeId
        /// </summary>
        /// <param name="WorkTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteWorkType/{WorkTypeId}")]
        public HttpResponseMessage DeleteWorkType(Guid WorkTypeId)
        {
            try
            {
                var DeleteWorkType = WorkTypeService.DeleteWorkType(WorkTypeId);
                if (DeleteWorkType)
                {
                    //return workType delete service for get workType.
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

        #endregion

    }
}
