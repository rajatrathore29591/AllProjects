using System.Web.Http;
using BorgCivil.Service;
using System.Net.Http;
using System.Dynamic;
using System.Collections.Generic;
using BorgCivil.Utils.Models;
using System.Net;
using System;
using BorgCivil.Utils;
using System.Web.Http.Cors;
using System.Linq;
using BorgCivil.Framework.Entities;

namespace BorgCivil.MVCWeb.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    //[Authorize]
    [RoutePrefix("api/Demo")]
    public class DemoApiController : ApiController
    {
        //Initialized interface object. 
        #region Dependencies Injection with initialization
        IDemoService DemoService;

        private const string LocalLoginProvider = "Local";
        private Framework.Identity.ApplicationUserManager _userManager;

        // Constructor of Booking Api Controller 
        public DemoApiController(IDemoService _DemoService)
        {

            DemoService = _DemoService;
        }

        #endregion

        [HttpGet]
        [Route("GetAllDemo1")]
        public HttpResponseMessage GetAllDemo1()
        {
            try
            {
                ////get DemoCollection list 
                var DemoCollection = DemoService.GetAllDemo();

                ////check object
                if (DemoCollection.Count > 0 && DemoCollection != null)
                {
                    ////dynamic list.
                    dynamic DemoList = new List<ExpandoObject>();
                    ////bind dynamic property.

                    ////return response from customer service.
                    foreach (var DemoDetail in DemoCollection)
                    {
                        dynamic Demo = new ExpandoObject();

                        ////map ids
                        Demo.DemoId = DemoDetail.DemoId;
                        Demo.Name = DemoDetail.Name;
                        Demo.Address = DemoDetail.Address;
                        Demo.RadioGender = DemoDetail.RadioGender;
                        Demo.CurrentDate = DemoDetail.CurrentDate.Value.ToString("MM-dd-yyyy HH:mm:ss tt");
                        Demo.CheckBoxGender = DemoDetail.CheckBoxGender;
                        Demo.DropDownGender = DemoDetail.DropDownGender;
                        ////set customers values in list.
                        DemoList.Add(Demo);
                    }

                    ////return customers service 
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "WorkType List", Succeeded = true, DataObject = new ExpandoObject(), DataList = DemoList, ErrorInfo = "" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetAllDemo/{filter}/{limit}/{order}/{page}")]
        public HttpResponseMessage GetAllDemo(string filter, string limit, string order, string page)
        {
            try
            {
                // declaring demo list type variable
                List<Demo> DemoCollection = new List<Demo>();

                //  variable to get total count
                int Count = DemoService.GetAllDemo().Count;

                // variable to skip the data
                int Skip = 0;

                // variable to hold the previous page value
                var previousPage = Convert.ToInt32(page);
                previousPage = previousPage - 1;
                Skip = (Convert.ToInt32(limit) * Convert.ToInt32(previousPage));

                if (order.Contains("-"))
                {
                    if (order == "-Name")
                    {
                        ////get DemoCollection list 
                        DemoCollection = DemoService.GetAllDemo().OrderByDescending(x => x.Name).Skip(Skip).Take(Convert.ToInt32(limit)).ToList();
                    }
                    if (order == "-Address")
                    {
                        ////get DemoCollection list 
                        DemoCollection = DemoService.GetAllDemo().OrderByDescending(x => x.Address).Skip(Skip).Take(Convert.ToInt32(limit)).ToList();
                    }
                }
                else
                {
                    if (order == "Name")
                    {
                        ////get DemoCollection list 
                        DemoCollection = DemoService.GetAllDemo().OrderBy(x => x.Name).Skip(Skip).Take(Convert.ToInt32(limit)).ToList();
                    }
                    if (order == "Address")
                    {
                        ////get DemoCollection list 
                        DemoCollection = DemoService.GetAllDemo().OrderBy(x => x.Address).Skip(Skip).Take(Convert.ToInt32(limit)).ToList();
                    }
                }
                if (filter != "null")
                {
                    DemoCollection = DemoCollection.Where(x => x.Name == filter || x.Address == filter).ToList();
                }
                ////check object
                if (DemoCollection.Count > 0 && DemoCollection != null)
                {
                    ////dynamic list.
                    dynamic DemoList = new List<ExpandoObject>();
                    ////bind dynamic property.

                    ////return response from customer service.
                    foreach (var DemoDetail in DemoCollection)
                    {
                        dynamic Demo = new ExpandoObject();

                        ////map ids
                        Demo.DemoId = DemoDetail.DemoId;
                        Demo.Name = DemoDetail.Name;
                        Demo.Address = DemoDetail.Address;
                        Demo.RadioGender = DemoDetail.RadioGender;
                        Demo.CurrentDate = DemoDetail.CurrentDate.Value.ToString("MM-dd-yyyy HH:mm:ss tt");
                        Demo.CheckBoxGender = DemoDetail.CheckBoxGender;
                        Demo.DropDownGender = DemoDetail.DropDownGender;
                        ////set customers values in list.
                        DemoList.Add(Demo);
                    }

                    ////return customers service 
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "WorkType List", Succeeded = true, DataObject = new ExpandoObject(), DataList = DemoList, ErrorInfo = "", Count = Count });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Data Not Found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Error", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
            }
        }


        [HttpGet]
        [Route("GetDemoByDemoId/{DemoId}")]
        public HttpResponseMessage GetDemoByDemoId(string DemoId)
        {
            try
            {

                ////check is valid user id.
                if (ServiceHelper.IsGuid(DemoId))
                {
                    ////get DemoDetail.
                    var DemoDetail = DemoService.GetDemoByDemoId(new Guid(DemoId));

                    if (DemoDetail != null)
                    {
                        ////bind dynamic property.
                        dynamic Demo = new ExpandoObject();

                        ////map ids
                        Demo.Id = DemoDetail.DemoId;
                        Demo.Name = DemoDetail.Name;
                        Demo.Address = DemoDetail.Address;
                        Demo.RadioGender = DemoDetail.RadioGender;
                        Demo.CheckBoxGender = DemoDetail.CheckBoxGender;
                        Demo.CurrentDate = DemoDetail.CurrentDate.Value.ToString("MM-dd-yyyy");
                        Demo.DropDownGender = DemoDetail.DropDownGender;
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "WorkType List", Succeeded = true, DataObject = Demo, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    }
                    else
                    {
                        ////case of record not found.
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "No Data", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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

        [HttpPost]
        [Route("AddDemo")]
        public HttpResponseMessage AddDemo(DemoModel demoData)
        {
            try
            {
                ////get service response.
                var response = DemoService.AddDemo(demoData);

                ////return result from service response.
                if (response == true)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Updated", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Not Updated", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Exception" });
            }
        }

        [HttpPost]
        [Route("EditDemo")]
        public HttpResponseMessage EditDemo(DemoModel demoData)
        {
            try
            {
                ////check is valid role id.
                if (!ServiceHelper.IsGuid(demoData.DemoId.ToString()))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid company user id." });
                }
                ////get service response.
                var response = DemoService.EditDemo(demoData);

                ////return result from service response.
                if (response == true)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Updated", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Not Updated", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Exception" });
            }
        }


        [HttpGet]
        [Route("DeleteDemo/{DemoId}")]
        public HttpResponseMessage DeleteDemo(string DemoId)
        {
            try
            {
                var deleteDemo = DemoService.DeleteDemo(new Guid(DemoId));
                if (deleteDemo != null)
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
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception" });
            }
        }

    }
}
