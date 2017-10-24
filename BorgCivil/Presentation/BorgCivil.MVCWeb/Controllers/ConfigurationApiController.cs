using System.Web.Http;
using BorgCivil.Service;
using System.Net.Http;
using BorgCivil.Utils.Models;
using System.Net;
using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;
using BorgCivil.Utils;
using System.Web;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using BorgCivil.Framework.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Specialized;
using BorgCivil.Utils.Email;

namespace BorgCivil.MVCWeb.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Configuration")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConfigurationApiController : ApiController
    {
        // defining hardcode for now as not cleared later we will make it dynamic
        string Password = "Qwerty@123";

        //Initialized interface object. 
        #region Dependencies Injection with initialization
        Framework.Identity.ApplicationUserManager userManager;
        RoleStore<ApplicationRole> roleStore;
        RoleManager<ApplicationRole> roleManager;
        IDocumentService DocumentService;
        ICountryService CountryService;
        IStateService StateService;
        IEmployeeService EmployeeService;
        IFleetTypesService FleetTypesService;
        IDriversService DriversService;
        IDocketCheckboxListService DocketCheckboxListService;
        IEmailService EmailService;

        // Constructor of Booking Api Controller 
        public ConfigurationApiController(IDocumentService _DocumentService, ICountryService _CountryService, IStateService _StateService, IEmployeeService _EmployeeService, IFleetTypesService _FleetTypesService, IDriversService _DriversService, IDocketCheckboxListService _DocketCheckboxListService, IEmailService _EmailService)
        {
            AppIdentityDbContext context = new AppIdentityDbContext();
            roleStore = new RoleStore<ApplicationRole>(context);
            roleManager = new RoleManager<ApplicationRole>(roleStore);
            DocumentService = _DocumentService;
            CountryService = _CountryService;
            StateService = _StateService;
            EmployeeService = _EmployeeService;
            FleetTypesService = _FleetTypesService;
            DriversService = _DriversService;
            DocketCheckboxListService = _DocketCheckboxListService;
            EmailService = _EmailService;
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

        #region Employee Api's

        /// <summary>
        /// This method for EmployeeLogin from "Employee" entity.
        /// </summary>
        /// <returns>Employee object value.</returns>
        [Route("EmployeeLogin")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> EmployeeLogin(EmployeeDataModel LoginModel)
        {
            try
            {
                // checking requested email contains @ or not
                if (LoginModel.Email.Contains("@") == true)
                {
                    // get user by email
                    Framework.Identity.ApplicationUser User = await UserManager.FindByEmailAsync(LoginModel.Email);
                    if (User != null)
                    {
                        LoginModel.Email = User.UserName;
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Invalid Creadentials", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    }
                }
                // checking employee requested email and password exist or not
                Framework.Identity.ApplicationUser Employee = await UserManager.FindAsync(LoginModel.Email, LoginModel.Password);

                // checking employee exist or not
                if (Employee != null)
                {
                    // getting employee detail by employeeId
                    var ObjEmployee = EmployeeService.GetEmployeeByEmail(LoginModel.Email);

                    // get Access Token info
                    var Tkn = App_Helper.AuthToken.GetLoingInfo(LoginModel);

                    if (Tkn != null)
                    {
                        var TokenModel = new AccessTokenModel();
                        TokenModel = Tkn;
                        TokenModel.LoginUserId = ObjEmployee.EmployeeId.ToString();
                        TokenModel.LoginUserName = ObjEmployee.FirstName + " " + ObjEmployee.SurName;
                        TokenModel.ImageUrl = ObjEmployee.Document != null ? ObjEmployee.Document.Name : "";

                        //employee service response
                        return this.Request.CreateResponse(HttpStatusCode.OK, new LoginResponseDataModel { Message = "Login successfully", Succeeded = true, DataObject = TokenModel, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    }
                    //employee service response
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Invalid Creadentials", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    //return result from service response.
                    return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Invalid Creadentials", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// This method for Forgot Password
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public HttpResponseMessage ForgotPassword(EmployeeDataModel LoginModel)
        {
            try
            {
                ListDictionary Replacements = new ListDictionary();
                var EncryptedPassword = string.Empty;

                //check is valid email.
                if (!string.IsNullOrEmpty(LoginModel.Email))
                {
                    var Employee = EmployeeService.GetEmployeeByEmail(LoginModel.Email);
                    if (Employee != null)
                    {
                        dynamic EmployeeModel = new ExpandoObject();
                        EmployeeModel.FirstName = Employee.FirstName;
                        EmployeeModel.Email = Employee.Email;
                        EmployeeModel.Password = Employee.Password;

                        string Password = SecurityHelper.Decrypt(Employee.Password, true);
                        Replacements = new ListDictionary { { "<%FirstName%>", Employee.FirstName } };
                        Replacements.Add("<%Subject%>", "Your password has changed");
                        Replacements.Add("<%LastName%>", Employee.SurName);
                        Replacements.Add("<%username%>", Employee.FirstName + " " + Employee.SurName);
                        Replacements.Add("<%Password%>", Password);

                        ////step:7
                        ////add email data
                        var sendEmail = EmailService.AddEmailData(Employee.EmployeeId.ToString(), Employee.Email, string.Empty, string.Empty, string.Empty, EmailTemplatesHelper.ForgotEmail, Replacements);

                        ////step:8
                        ////send email.
                        if (sendEmail != null)
                        {
                            ////Code for Sending Email
                            EmailService.SendEmailAsync(Employee.EmployeeId, Employee.Email, string.Empty, string.Empty, Employee.Email, EmailTemplatesHelper.ForgotEmail, Password, Replacements);

                            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Email Sended Successfully.", Succeeded = true, DataObject = EmployeeModel, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                        }
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Email Address Not Exist.", Succeeded = true, DataObject = EmployeeModel, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.NoContent, new { message = "Email Address Not Exist" });
                    }

                    ////case of valid id request.
                    return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Email is Empty" });
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
        /// This method for customer change password.
        /// </summary>
        /// <returns>status with message</returns>
        [HttpPost]
        [Route("ChangePassword")]
        public HttpResponseMessage ChangePassword(EmployeePasswordDataModel EmployeePasswordDataModel)
        {
            try
            {
                ////step: 1
                ////check is valid email.
                if (ServiceHelper.IsGuid(EmployeePasswordDataModel.EmployeeId))
                {
                    if (ModelState.IsValid)
                    {
                        var Employee = EmployeeService.GetEmployeeByEmployeeId(new Guid(EmployeePasswordDataModel.EmployeeId));
                        if (Employee == null)
                        {
                            // Don't reveal that the user does not exist
                            return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "ID Not Exist or Password not matched" });
                        }
                        else
                        {
                            var user = UserManager.FindById(Employee.UserId);
                            if (user != null)
                            {
                                // encrypting password for company user table
                                var Password = SecurityHelper.Encrypt(EmployeePasswordDataModel.NewPassword, true);

                                //user.PasswordHash = EmployeePasswordDataModel.NewPassword;
                                var response = UserManager.ChangePassword(Employee.UserId, EmployeePasswordDataModel.OldPassword, EmployeePasswordDataModel.NewPassword);
                                if (response.Succeeded)
                                {
                                    var result = EmployeeService.EditEmployeePassword(new Guid(EmployeePasswordDataModel.EmployeeId), Password);
                                    ////check result
                                    if (result == true)
                                    {
                                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Password has been Successfuly Changed.", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                                    }
                                    else
                                    {
                                        ////return response
                                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Password is incorrect", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                                    }
                                }
                                else
                                {
                                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "data not found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                                }

                            }
                            else
                            {
                                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "data not found", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                            }
                        }
                    }
                    else
                    {
                        ////case of invalid id request.
                        return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "model is not validate", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    }
                }
                else
                {
                    ////case of invalid id request.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Invalid Id" });
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
        /// this method is for getting all employee 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllEmployee")]
        public HttpResponseMessage GetAllEmployee()
        {
            try
            {
                //get employee list 
                var EmployeeCollection = EmployeeService.GetAllEmployee().OrderByDescending(x => x.CreatedDate).ToList();

                //check object
                if (EmployeeCollection.Count > 0 && EmployeeCollection != null)
                {
                    //dynamic list.
                    dynamic Employees = new List<ExpandoObject>();

                    //return response from employee service.
                    foreach (var EmployeeDetail in EmployeeCollection)
                    {
                        //bind dynamic property.
                        dynamic Employee = new ExpandoObject();

                        //map ids
                        Employee.EmployeeId = EmployeeDetail.EmployeeId;
                        Employee.EmploymentCategoryId = EmployeeDetail.EmploymentCategoryId;
                        Employee.EmploymentStatusId = EmployeeDetail.EmploymentStatusId;
                        Employee.FirstName = EmployeeDetail.FirstName;
                        Employee.SurName = EmployeeDetail.SurName;
                        Employee.Email = EmployeeDetail.Email;
                        Employee.CountryName = EmployeeDetail.Country.Name;
                        Employee.StateName = EmployeeDetail.State.Name;
                        Employee.Address1 = EmployeeDetail.Address1;
                        Employee.Address2 = EmployeeDetail.Address2;
                        Employee.ContactNumber = EmployeeDetail.ContactNumber;
                        Employee.City = EmployeeDetail.City;
                        Employee.ZipCode = EmployeeDetail.ZipCode;
                        Employee.IsActive = EmployeeDetail.IsActive;
                        Employee.RoleName = EmployeeDetail.Role != null ? EmployeeDetail.Role.Name : "";
                        Employee.CreatedDate = EmployeeDetail.CreatedDate;
                        Employee.EditedDate = EmployeeDetail.EditedDate;

                        //set Employee values in list.
                        Employees.Add(Employee);
                    }

                    //return user service for get organasation.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Employee List", Succeeded = true, DataObject = new ExpandoObject(), DataList = Employees, ErrorInfo = "" });
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
        /// Get method for Get All Roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllRoles")]
        public HttpResponseMessage GetAllRoles()
        {
            try
            {
                AppIdentityDbContext context = new AppIdentityDbContext();
                var roleStore = new RoleStore<ApplicationRole>(context);
                var roleManager = new RoleManager<ApplicationRole>(roleStore);

                ////get roles list 
                var RoleCollection = roleManager.Roles.OrderBy(x => x.Name).ToList();

                ////check object
                if (RoleCollection != null)
                {
                    ////dynamic list.
                    dynamic RoleList = new List<ExpandoObject>();

                    ////return response form role service.
                    foreach (var RoleDetail in RoleCollection)
                    {
                        ////declare var
                        dynamic Role = new ExpandoObject();

                        ////map ids
                        Role.Id = RoleDetail.Id;

                        ////map prop value
                        Role.Name = RoleDetail.Name;
                        Role.Description = RoleDetail.Description;

                        ////set role values in list.
                        RoleList.Add(Role);
                    }
                    ////return role list
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Role List", Succeeded = true, DataObject = new ExpandoObject(), DataList = RoleList, ErrorInfo = "" });
                }
                else
                {
                    //return case of exception.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Some thing went wrong", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
            }
            catch (Exception ex)
            {
                //// Handle Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
            }
        }

        /// <summary>
        /// this method is used for getting employee detail
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEmployeeDetail/{EmployeeId}")]
        public HttpResponseMessage GetEmployeeDetail(Guid EmployeeId)
        {
            try
            {
                //check is valid EmployeeId.
                if (!ServiceHelper.IsGuid((string)EmployeeId.ToString()))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid booking fleet id." });
                }

                //getting employee detail by employee Id
                var EmployeeDetail = EmployeeService.GetEmployeeByEmployeeId(EmployeeId);

                //check service response.
                if (EmployeeDetail != null)
                {
                    //bind dynamic property.
                    dynamic Employee = new ExpandoObject();
                    Employee.EmployeeId = EmployeeDetail.EmployeeId;
                    Employee.UserId = EmployeeDetail.UserId;
                    Employee.RoleId = EmployeeDetail.RoleId;
                    Employee.EmploymentCategoryId = EmployeeDetail.EmploymentCategoryId;
                    Employee.FirstName = EmployeeDetail.FirstName;
                    Employee.SurName = EmployeeDetail.SurName;
                    Employee.ContactNumber = EmployeeDetail.ContactNumber;
                    Employee.Email = EmployeeDetail.Email;
                    Employee.Address1 = EmployeeDetail.Address1;
                    Employee.Address2 = EmployeeDetail.Address2;
                    Employee.City = EmployeeDetail.City;
                    Employee.ZipCode = EmployeeDetail.ZipCode;
                    Employee.IsActive = EmployeeDetail.IsActive;
                    Employee.Image = EmployeeDetail.Document.Name;
                    Employee.CountryId = EmployeeDetail.CountryId;
                    Employee.StateId = EmployeeDetail.StateId;

                    //employee service response
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Get Employee Details", Succeeded = true, DataObject = Employee, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// This method use for add record into employee entity.
        /// </summary>
        /// <param name="EmployeeDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddEmployee")]
        public HttpResponseMessage AddEmployee(EmployeeDataModel EmployeeDataModel)
        {
            try
            {
                // defining variables globally
                DocumentDataModel DocumentData = new DocumentDataModel();
                var DocumentId = (Guid?)null;
                var user = new ApplicationUser();

                // checking email already exist or not
                var EmailExist = UserManager.FindByEmail(EmployeeDataModel.Email);
                if (EmailExist == null)
                {
                    //step:1
                    //add user in AspNetUser.
                    user.UserName = EmployeeDataModel.Email;
                    user.Email = EmployeeDataModel.Email;

                    // adding user in usermanager table
                    var result = UserManager.Create(user, Password);
                    if (result != null)
                    {
                        // setting userId
                        EmployeeDataModel.UserId = user.Id;
                        EmployeeDataModel.Password = SecurityHelper.Encrypt(Password, true);
                        //add employee detail into database
                        var EmployeeId = EmployeeService.AddEmployee(EmployeeDataModel);

                        #region Save data in document entity.

                        if (EmployeeDataModel.ImageBase64 != null)
                        {
                            String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                            String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

                            //bind file data in a document model.
                            DocumentData.Id = Guid.NewGuid().ToString();
                            DocumentData.OriginalName = "Image";
                            DocumentData.Name = EmployeeId + "_EmployeeImage_" + ".png";
                            DocumentData.Title = "Images";
                            DocumentData.Description = string.Empty;
                            DocumentData.Tags = string.Empty;
                            DocumentData.URL = strUrl + "/Uploads/" + EmployeeId + "_EmployeeImage_" + ".png";//provider.FileData.FirstOrDefault().LocalFileName;//
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
                                var UpdateDocument = EmployeeService.UpdateDocumentId(new Guid(EmployeeId), DocumentId.Value);

                                // uploading image on Uploads folder
                                var ImageUploadStatus = UploadImage.base64ToImage(EmployeeDataModel.ImageBase64, EmployeeId + "_EmployeeImage_" + ".png");

                                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Employee successfully added", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                            }
                            else
                            {
                                //return result from service response.
                                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Document not added successfully", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                            }
                        }
                        #endregion

                        /// checking employee id not equal to null
                        if (EmployeeId != null)
                        {
                            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Employee successfully added", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                        }
                    }
                }

                //return service for get user.
                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Email already exist", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });

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
        /// This method use for update record into employee entity.
        /// </summary>
        /// <param name="EmployeeDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateEmployee")]
        public HttpResponseMessage UpdateEmployee(EmployeeDataModel EmployeeDataModel)
        {
            try
            {
                // defining variables globally
                DocumentDataModel DocumentData = new DocumentDataModel();
                var DocumentId = (Guid?)null;

                // calling service of update employee
                var EmployeeId = EmployeeService.UpdateEmployee(EmployeeDataModel);

                /// return if not succeded
                if (ServiceHelper.IsGuid(EmployeeId))
                {
                    #region Update data in document entity.

                    var EmployeeDetail = EmployeeService.GetEmployeeByEmployeeId(new Guid(EmployeeId));
                    if (EmployeeDataModel.ImageBase64 != null && EmployeeDetail.DocumentId != null)
                    {
                        // uploading image on Uploads folder
                        var ImageUploadStatus = UploadImage.base64ToImage(EmployeeDataModel.ImageBase64, EmployeeId + "_EmployeeImage_" + ".png");
                    }
                    // if document is not loaded
                    if (EmployeeDetail.DocumentId == null && EmployeeDataModel.ImageBase64 != null)
                    {
                        String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                        String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

                        //bind file data in a document model.
                        DocumentData.Id = Guid.NewGuid().ToString();
                        DocumentData.OriginalName = "Image";
                        DocumentData.Name = EmployeeId + "_EmployeeImage_" + ".png";
                        DocumentData.Title = "Images";
                        DocumentData.Description = string.Empty;
                        DocumentData.Tags = string.Empty;
                        DocumentData.URL = strUrl + "/Uploads/" + EmployeeId + "_EmployeeImage_" + ".png";//provider.FileData.FirstOrDefault().LocalFileName;//
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
                        var UpdateDocument = EmployeeService.UpdateDocumentId(new Guid(EmployeeId), DocumentId.Value);

                        // uploading image on Uploads folder
                        var ImageUploadStatus = UploadImage.base64ToImage(EmployeeDataModel.ImageBase64, EmployeeId + "_EmployeeImage_" + ".png");
                    }

                    #endregion

                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Employee successfully updated", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// this method is used for deleting Employee
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteEmployee/{EmployeeId}")]
        public HttpResponseMessage DeleteEmployee(Guid EmployeeId)
        {
            try
            {
                //check is valid EmployeeId.
                if (!ServiceHelper.IsGuid((string)EmployeeId.ToString()))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid booking fleet id." });
                }

                //getting Employee detail by EmployeeId
                var EmployeeDetail = EmployeeService.DeleteEmployee(EmployeeId);

                //check service response.
                if (EmployeeDetail)
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

        #region DocketCheckList Api's

        /// <summary>
        /// this method is for getting all DocketCheckList 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllDocketCheckList")]
        public HttpResponseMessage GetAllDocketCheckList()
        {
            try
            {
                //get DocketCheckList list 
                var DocketCheckListCollection = DocketCheckboxListService.GetAllDocketCheckList().OrderByDescending(x => x.CreatedDate).ToList();

                //check object
                if (DocketCheckListCollection.Count > 0 && DocketCheckListCollection != null)
                {
                    //dynamic list.
                    dynamic DocketCheckLists = new List<ExpandoObject>();

                    //return response from employee service.
                    foreach (var DocketCheckListDetail in DocketCheckListCollection)
                    {
                        //bind dynamic property.
                        dynamic DocketCheckList = new ExpandoObject();

                        //map ids
                        DocketCheckList.DocketCheckListId = DocketCheckListDetail.DocketCheckListId;
                        DocketCheckList.Title = DocketCheckListDetail.Title;
                        DocketCheckList.IsActive = DocketCheckListDetail.IsActive;
                        DocketCheckList.CreatedDate = DocketCheckListDetail.CreatedDate;
                        DocketCheckList.EditedDate = DocketCheckListDetail.EditedDate;

                        //set DocketCheckList values in list.
                        DocketCheckLists.Add(DocketCheckList);
                    }

                    //return DocketCheckList response
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "DocketCheckList List", Succeeded = true, DataObject = new ExpandoObject(), DataList = DocketCheckLists, ErrorInfo = "" });
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
        /// this method is used for getting DocketCheckList detail
        /// </summary>
        /// <param name="DocketCheckId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDocketCheckListDetail/{DocketCheckId}")]
        public HttpResponseMessage GetDocketCheckListDetail(Guid DocketCheckId)
        {
            try
            {
                //check is valid DocketCheckId.
                if (!ServiceHelper.IsGuid((string)DocketCheckId.ToString()))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid booking fleet id." });
                }

                //getting DocketCheck detail by DocketCheckId
                var DocketCheckDetail = DocketCheckboxListService.GetDocketCheckListDetail(DocketCheckId);

                //check service response.
                if (DocketCheckDetail != null)
                {
                    //bind dynamic property.
                    dynamic DocketCheckList = new ExpandoObject();
                    DocketCheckList.DocketCheckListId = DocketCheckDetail.DocketCheckListId;
                    DocketCheckList.Title = DocketCheckDetail.Title;
                    DocketCheckList.IsActive = DocketCheckDetail.IsActive;

                    //employee service response
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Get DocketCheckList Details", Succeeded = true, DataObject = DocketCheckList, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// method to add record in DocketCheckList
        /// </summary>
        /// <param name="DocketCheckboxListDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddDocketCheckboxList")]
        public HttpResponseMessage AddDocketCheckboxList(DocketCheckboxListDataModel DocketCheckboxListDataModel)
        {
            try
            {
                ////add docketCheckList detail into database
                var DocketCheckListId = DocketCheckboxListService.AddDocketCheckboxList(DocketCheckboxListDataModel);

                /// checking DocketCheckListId not equal to null
                if (ServiceHelper.IsGuid(DocketCheckListId))
                {
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
        /// method to update record in DocketCheckList
        /// </summary>
        /// <param name="DocketCheckboxListDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateDocketCheckboxList")]
        public HttpResponseMessage UpdateDocketCheckboxList(DocketCheckboxListDataModel DocketCheckboxListDataModel)
        {
            try
            {
                ////update docketCheckList detail into database
                var DocketCheckListId = DocketCheckboxListService.UpdateDocketCheckboxList(DocketCheckboxListDataModel);

                /// checking DocketCheckListId not equal to null
                if (ServiceHelper.IsGuid(DocketCheckListId))
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Record updated successfully", Succeeded = true, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
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
        /// method to delete DocketCheckboxList by DocketCheckboxListId
        /// </summary>
        /// <param name="DocketCheckboxListId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteDocketCheckboxList/{DocketCheckboxListId}")]
        public HttpResponseMessage DeleteDocketCheckboxList(Guid DocketCheckboxListId)
        {
            try
            {
                var DeleteDocketCheckboxList = DocketCheckboxListService.DeleteDocketCheckboxList(DocketCheckboxListId);
                if (DeleteDocketCheckboxList)
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

        #region Country & State Api's

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
                //get country list
                var countries = CountryService.GetCountryList();

                //check object
                if (countries.Count > 0 && countries != null)
                {
                    //return of country service
                    return this.Request.CreateResponse(HttpStatusCode.OK, countries);
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Data Not Found." });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// This method use for get 'State' list for select list type controls.
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns>key value pair</returns>
        [HttpGet]
        [Route("GetAllState/{CountryId}")]
        public HttpResponseMessage GetAllState(string CountryId)
        {
            try
            {
                //get state list
                var states = StateService.GetAllStateByCountryId(new Guid(CountryId));

                //check object
                if (states.Count > 0 && states != null)
                {
                    //return state service.
                    return this.Request.CreateResponse(HttpStatusCode.OK, states);
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Data Not Found." });
                }
            }
            catch (Exception ex)
            {
                // Handel Exception Log.
                Console.Write(ex.Message);

                //return case of exception.
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion

        #region ImageUpload

        [HttpPost()]
        public string UploadFiles()
        {
            int iUploadedCnt = 0;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "";
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/");

            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                System.Web.HttpPostedFile hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {
                    // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                    if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                    {
                        // SAVE THE FILES IN THE FOLDER.
                        hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                        iUploadedCnt = iUploadedCnt + 1;
                    }
                }
            }

            // RETURN A MESSAGE.
            if (iUploadedCnt > 0)
            {
                return iUploadedCnt + " Files Uploaded Successfully";
            }
            else
            {
                return "Upload Failed";
            }
        }

        #endregion

    }
}
