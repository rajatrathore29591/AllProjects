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
    [RoutePrefix("api/Customer")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CustomerApiController : ApiController
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        ICustomerService CustomerService;

        // Constructor of Customer Api Controller 
        public CustomerApiController(ICustomerService _CustomerService)
        {
            CustomerService = _CustomerService;
        }

        #endregion

        #region Customer Api's

        /// <summary>
        /// getting all customer select list for dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCustomers")]
        public HttpResponseMessage GetCustomers()
        {
            try
            {
                //get customer type list
                var Customers = CustomerService.GetCustomerList();

                //check object
                if (Customers.Count > 0 && Customers != null)
                {
                    List<ExpandoObject> Customerlist = new List<ExpandoObject>();
                    foreach (var Customer in Customers)
                    {
                        dynamic CustomerDetail = new ExpandoObject();
                        CustomerDetail.CustomerName = Customer.Text;
                        CustomerDetail.CustomerId = Customer.Value;
                        Customerlist.Add(CustomerDetail);
                    }
                    //return customer service for get customer type.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Customer List", Succeeded = true, DataObject = new ExpandoObject(), DataList = Customerlist, ErrorInfo = "" });
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
        /// this method is for getting all Customer 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllCustomer")]
        public HttpResponseMessage GetAllCustomer()
        {
            try
            {
                //get Customer list 
                var CustomerCollection = CustomerService.GetAllCustomer().OrderByDescending(x => x.CreatedDate).ToList();

                //check object
                if (CustomerCollection.Count > 0 && CustomerCollection != null)
                {
                    //dynamic list.
                    dynamic CustomerLists = new List<ExpandoObject>();

                    //return response from customer service.
                    foreach (var CustomerDetail in CustomerCollection)
                    {
                        //bind dynamic property.
                        dynamic CustomerList = new ExpandoObject();

                        //map ids
                        CustomerList.CustomerId = CustomerDetail.CustomerId;
                        CustomerList.CustomerName = CustomerDetail.CustomerName;
                        CustomerList.ABN = CustomerDetail.ABN;
                        CustomerList.ContactName = CustomerDetail.ContactName;
                        CustomerList.ContactNumber = CustomerDetail.ContactNumber;
                        CustomerList.MobileNumber1 = CustomerDetail.MobileNumber1;
                        CustomerList.MobileNumber2 = CustomerDetail.MobileNumber2;
                        CustomerList.EmailForInvoices = CustomerDetail.EmailForInvoices;
                        CustomerList.IsActive = CustomerDetail.IsActive;
                        CustomerList.CreatedDate = CustomerDetail.CreatedDate;
                        CustomerList.EditedDate = CustomerDetail.EditedDate;

                        //set CustomerList values in list.
                        CustomerLists.Add(CustomerList);
                    }

                    //return CustomerList response
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Customer List", Succeeded = true, DataObject = new ExpandoObject(), DataList = CustomerLists, ErrorInfo = "" });
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
        /// this method is used for getting Customer detail
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCustomerByCustomerId/{CustomerId}")]
        public HttpResponseMessage GetCustomerByCustomerId(Guid CustomerId)
        {
            try
            {
                //check is valid CustomerId.
                if (!ServiceHelper.IsGuid((string)CustomerId.ToString()))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid booking fleet id." });
                }

                //getting Customer detail by CustomerId
                var CustomerDetail = CustomerService.GetCustomerByCustomerId(CustomerId);

                //check service response.
                if (CustomerDetail != null)
                {
                    //bind dynamic property.
                    dynamic CustomerList = new ExpandoObject();
                    CustomerList.CustomerId = CustomerDetail.CustomerId;
                    CustomerList.CustomerName = CustomerDetail.CustomerName;
                    CustomerList.ABN = CustomerDetail.ABN;
                    CustomerList.ContactName = CustomerDetail.ContactName;
                    CustomerList.ContactNumber = CustomerDetail.ContactNumber;
                    CustomerList.AccountsContact = CustomerDetail.AccountsContact;
                    CustomerList.AccountsNumber = CustomerDetail.AccountsNumber;
                    CustomerList.PhoneNumber1 = CustomerDetail.PhoneNumber1;
                    CustomerList.PhoneNumber2 = CustomerDetail.PhoneNumber2;
                    CustomerList.MobileNumber1 = CustomerDetail.MobileNumber1;
                    CustomerList.MobileNumber2 = CustomerDetail.MobileNumber2;
                    CustomerList.Fax = CustomerDetail.Fax;
                    CustomerList.EmailForInvoices = CustomerDetail.EmailForInvoices;
                    CustomerList.OfficeStreet = CustomerDetail.OfficeStreet;
                    CustomerList.OfficeSuburb = CustomerDetail.OfficeSuburb;
                    CustomerList.OfficeState = CustomerDetail.OfficeState;
                    CustomerList.OfficePostalCode = CustomerDetail.OfficePostalCode;
                    CustomerList.PostalStreetPoBox = CustomerDetail.PostalStreetPoBox;
                    CustomerList.PostalSuburb = CustomerDetail.PostalSuburb;
                    CustomerList.PostalStreetPoBox = CustomerDetail.PostalState;
                    CustomerList.PostalPostCode = CustomerDetail.PostalPostCode;
                    CustomerList.PostalState = CustomerDetail.PostalState;

                    //bind dynamic list property.
                    dynamic SiteList = new List<ExpandoObject>();

                    // checking count not equal to zero
                    if (CustomerDetail.Sites.Count > 0)
                    {
                        foreach (var Site in CustomerDetail.Sites)
                        {
                            //bind dynamic property.
                            dynamic SiteObject = new ExpandoObject();

                            //bind child object.
                            SiteObject.SiteName = Site.SiteName;
                            SiteObject.PoNumber = Site.PoNumber;
                            SiteObject.SiteDetail = Site.SiteDetail;
                            SiteObject.FuelIncluded = Site.FuelIncluded;
                            SiteObject.TollTax = Site.TollTax;
                            SiteObject.CreditTermAgreed = Site.CreditTermAgreed;

                            dynamic SupervisorList = new List<ExpandoObject>();

                            // checking count not equal to zero
                            if (Site.Supervisors.Count() > 0)
                            {
                                foreach (var Supervisor in Site.Supervisors)
                                {
                                    //bind dynamic property.
                                    dynamic SupervisorObject = new ExpandoObject();

                                    //bind child object.
                                    SupervisorObject.SupervisorId = Supervisor.SupervisorId;
                                    SupervisorObject.SupervisorName = Supervisor.SupervisorName;
                                    SupervisorObject.Email = Supervisor.Email;
                                    SupervisorObject.MobileNumber = Supervisor.MobileNumber;

                                    //set supervisor detail in list.
                                    SupervisorList.Add(SupervisorObject);
                                }
                            }

                            dynamic GateList = new List<ExpandoObject>();

                            // checking count not equal to zero
                            if (Site.Gates.Count() > 0)
                            {
                                foreach (var Gate in Site.Gates)
                                {
                                    //bind dynamic property.
                                    dynamic GateObject = new ExpandoObject();

                                    //bind child object.
                                    GateObject.GateId = Gate.GateId;
                                    GateObject.GateNumber = Gate.GateNumber;
                                    GateObject.TipOffRate = Gate.TipOffRate;
                                    GateObject.TippingSite = Gate.TippingSite;

                                    //set gate detail in list.
                                    GateList.Add(GateObject);
                                }
                            }

                            SiteObject.SupervisorList = SupervisorList;
                            SiteObject.GateList = GateList;

                            //set Site detail in list.
                            SiteList.Add(SiteObject);
                        }
                    }

                    CustomerList.SiteDetail = SiteList;

                    //customer service response
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Get Customer Details", Succeeded = true, DataObject = CustomerList, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// method to add record in Customer
        /// </summary>
        /// <param name="CustomerDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddCustomer")]
        public HttpResponseMessage AddCustomer(CustomerDataModel CustomerDataModel)
        {
            try
            {
                ////add Customer detail into database
                var CustomerId = CustomerService.AddCustomer(CustomerDataModel);

                /// checking Customer not equal to null
                if (ServiceHelper.IsGuid(CustomerId))
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Record inserted successfully", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "", Id = CustomerId });
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
        /// method to update record in Customer
        /// </summary>
        /// <param name="CustomerDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateCustomer")]
        public HttpResponseMessage UpdateCustomer(CustomerDataModel CustomerDataModel)
        {
            try
            {
                ////update customer detail into database
                var CustomerId = CustomerService.UpdateCustomer(CustomerDataModel);

                /// checking CustomerId not equal to null
                if (ServiceHelper.IsGuid(CustomerId))
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Record updated successfully", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "", Id = CustomerId });
                }
                ////return service for get user.
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Record not updated successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// method to delete Customer by CustomerId
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteCustomer/{CustomerId}")]
        public HttpResponseMessage DeleteCustomer(Guid CustomerId)
        {
            try
            {
                var Customer = CustomerService.DeleteCustomer(CustomerId);
                if (Customer)
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

    }
}
