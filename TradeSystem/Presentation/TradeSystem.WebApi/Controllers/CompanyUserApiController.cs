using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TradeSystem.Service;
using TradeSystem.Utils;
using TradeSystem.Utils.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TradeSystem.Framework.Identity;
using System.Collections.Specialized;
using TradeSystem.Utilities.Email;
using TradeSystem.Utils.Enum;

namespace TradeSystem.MVCWeb.Controllers
{
    [Authorize]
    [RoutePrefix("api/companyuser")]
    public class CompanyUserApiController : ApiController
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        private Framework.Identity.ApplicationUserManager userManager;
        ICompanyUserService companyUserService;
        IEmailService emailService;
        IActivityService activityService;

        public CompanyUserApiController(ICompanyUserService _companyUserService, IEmailService _emailService, IActivityService _activityService)
        {
            companyUserService = _companyUserService;
            emailService = _emailService;
            activityService = _activityService;
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

        /// <summary>
        /// This method for get all record from "companyUsers" entity.
        /// </summary>
        /// <returns>companyUsers entity object value.</returns>
        [HttpGet]
        [Route("GetAllCompanyUser")]
        public HttpResponseMessage GetAllCompanyUsers()
        {
            try
            {
                ////get companyUsers list 
                var companyUserCollection = companyUserService.GetAllCompanyUser();

                ////check object
                if (companyUserCollection.Count > 0 && companyUserCollection != null)
                {
                    ////dynamic list.
                    dynamic companyUsers = new List<ExpandoObject>();

                    ////return response from companyUsers service.
                    foreach (var companyUserDetail in companyUserCollection)
                    {
                        ////bind dynamic property.
                        dynamic companyUser = new ExpandoObject();
                        ////map ids
                        companyUser.Id = companyUserDetail.Id;
                        companyUser.RoleId = companyUserDetail.RoleId;
                        companyUser.Role = companyUserDetail.Role.Name;
                        companyUser.CompanyName = companyUserDetail.CompanyName;
                        companyUser.FirstName = companyUserDetail.FirstName;
                        companyUser.LastName = companyUserDetail.LastName;
                        companyUser.Email = companyUserDetail.Email;
                        companyUser.Phone = companyUserDetail.Phone;
                        companyUser.UserName = companyUserDetail.UserName;
                        companyUser.Address = companyUserDetail.Address;
                        companyUser.IsSuperAdmin = companyUserDetail.IsSuperAdmin;
                        companyUser.Status = companyUserDetail.IsActive == true ? "Active" : "InActive";
                        companyUser.CreatedDate = companyUserDetail.CreatedDate.ToLocalTime();
                        companyUser.ModifiedDate = companyUserDetail.ModifiedDate;

                        ////set companyUsers values in list.
                        companyUsers.Add(companyUser);
                    }
                    //var companyUsers_clone = companyUsers.OrderDescindingBy(x=>x.Data);

                    ////return companyUsers service 
                    return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK,  (List<ExpandoObject>)companyUsers);
                    //return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "some thing went wrong", Succeeded = false,Data1 = (List<ExpandoObject>)companyUsers });
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
        /// This post method for insert record into "Add CompanyUser" from database entity.
        /// </summary>
        /// <param name="companyUserData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddCompanyUser")]
        public HttpResponseMessage AddCompanyUser(CompanyUserDataModel companyUserData)
        {
            try
            {
                ListDictionary replacements = new ListDictionary();
                var encryptedPassword = string.Empty;
                var user = new ApplicationUser();
                var checkEmail = UserManager.FindByEmail(companyUserData.Email);
                if (checkEmail == null)
                {
                    ////step:1
                    ////add user in AspNetUser.
                    user.UserName = companyUserData.Email;
                    user.Email = companyUserData.Email;
                    var result = UserManager.Create(user, companyUserData.Password);

                    if (result != null)
                    {
                        ////step:2
                        ////add companyuser userId in companyUserData.
                        companyUserData.UserId = user.Id;
                        companyUserData.Password = SecurityHelper.Encrypt(companyUserData.Password, true); ;
                        ////get service response.
                        var response = companyUserService.AddCompanyUser(companyUserData.RoleId, companyUserData.UserId, companyUserData.CompanyName, companyUserData.FirstName, companyUserData.MiddleName, companyUserData.LastName, companyUserData.Email, companyUserData.Phone, companyUserData.IsActive, companyUserData.UserName, companyUserData.Password, companyUserData.Address, companyUserData.IsSuperAdmin);

                        if (response != null)
                        {

                            var companyUser = companyUserService.GetCompanyUserByCompanyUserId(new Guid(response));
                            if (companyUser != null)
                            {
                                string coupon = SecurityHelper.Decrypt(companyUser.Password, true);
                                replacements = new ListDictionary { { "<%FirstName%>", companyUser.FirstName } };
                                replacements.Add("<%Subject%>", "Your company successfully registered with Split Deals");
                                replacements.Add("<%email%>", companyUser.Email);
                                replacements.Add("<%LastName%>", companyUser.LastName);
                                replacements.Add("<%Password%>", coupon);

                                ////step:7
                                ////add email data
                                var sendEmail = emailService.AddEmailData(companyUser.Id.ToString(), companyUser.Email, string.Empty, string.Empty, string.Empty, EmailTemplatesHelper.CompanyCreateEmail, replacements);

                                ////step:8
                                ////send email.
                                if (sendEmail != null)
                                {
                                    ////Code for Sending Email
                                    emailService.SendEmailAsync(companyUser.Id, companyUser.Email, string.Empty, string.Empty, companyUser.Email, EmailTemplatesHelper.CompanyCreateEmail, encryptedPassword, replacements);
                                }
                                //return this.Request.CreateResponse(HttpStatusCode.OK, new { message = "Email Sended Successfully." });
                            }

                            else
                            {
                                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "some thing went wrong", Succeeded = false });
                            }
                        }

                        ////return result from service response.
                        return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Customer successfully inserted", Succeeded = true });
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "some thing went wrong", Succeeded = false });
                    }
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new BaseResponseDataModel { Message = "Email already exist", Succeeded = false });
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
        /// This method for get record of CompanyUser from "CompanyUser" by customer id entity.
        /// </summary>
        /// <returns>CompanyUser, customer and customerCompanyUser entity object value.</returns>
        [HttpGet]
        [Route("GetCompanyUserDetail/{id}")]
        public HttpResponseMessage GetCompanyUserDetail(string id)
        {
            try
            {

                ////get CompanyUser list 
                var CompanyUserDetail = companyUserService.GetCompanyUserByCompanyUserId(new Guid(id));

                ////check object
                if (CompanyUserDetail != null)
                {
                    dynamic CompanyUser = new ExpandoObject();

                    CompanyUser.Id = CompanyUserDetail.Id;
                    CompanyUser.FirstName = CompanyUserDetail.FirstName;
                    CompanyUser.MiddleName = CompanyUserDetail.MiddleName;
                    CompanyUser.LastName = CompanyUserDetail.LastName;
                    CompanyUser.UserName = CompanyUserDetail.UserName;
                    CompanyUser.Phone = CompanyUserDetail.Phone;
                    CompanyUser.Address = CompanyUserDetail.Address;
                    CompanyUser.Email = CompanyUserDetail.Email;
                    CompanyUser.RoleId = CompanyUserDetail.RoleId;
                    CompanyUser.Role = CompanyUserDetail.Role.Name;
                    if (CompanyUserDetail.IsActive == true)
                    {
                        CompanyUser.Active = true;
                        CompanyUser.DeActive = false;

                    }
                    else
                    {
                        CompanyUser.Active = false;
                        CompanyUser.DeActive = true;
                    }
                    ////return all service 
                    return this.Request.CreateResponse<ExpandoObject>(HttpStatusCode.OK, (ExpandoObject)CompanyUser);
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
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
            }
        }

        /// <summary>
        /// This method for update data by id.
        /// </summary>
        /// <param name="companyUserData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EditCompanyUser")]
        public HttpResponseMessage EditCompanyUser(CompanyUserDataModel companyUserData)
        {
            try
            {
                ////check is valid role id.
                if (!ServiceHelper.IsGuid((string)companyUserData.Id))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid company user id." });
                }
                var companyUser = companyUserService.GetCompanyUserByCompanyUserId(new Guid(companyUserData.Id));
                if (companyUserData.RoleId == null)
                {
                    companyUserData.RoleId = companyUser.RoleId;
                }
                ////get service response.
                var response = companyUserService.EditCompanyUser(new Guid(companyUserData.Id), companyUserData.RoleId, companyUserData.FirstName, companyUserData.MiddleName, companyUserData.LastName, companyUserData.Email, companyUserData.Phone, companyUserData.Address, companyUserData.IsActive);                ////return result from service response.

                ////return result from service response.
                if (response == true)
                {
                    var user = UserManager.FindById(companyUser.UserId);
                    user.Email = companyUserData.Email;
                    UserManager.Update(user);
                }
                return this.Request.CreateResponse(HttpStatusCode.OK, new { result = response });
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
        ///  This method for delete(hard detete) from database entity by id.
        ///  Delete from "Company User" entity. 
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Api response result</returns>
        [HttpGet]
        [Route("DeleteCompanyUser/{Id}")]
        [AllowAnonymous]
        public HttpResponseMessage DeleteCompanyUser(string Id)
        {
            try
            {
                ////check is valid company user id.
                if (!ServiceHelper.IsGuid((string)Id))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Invalid company user id." });
                }
                var isExist = activityService.GetActivityByCompanyUserId(new Guid(Id));
                if (isExist.Count < 1)
                {
                    var userId = companyUserService.GetCompanyUserByCompanyUserId(new Guid(Id)).UserId;
                    if (userId != null)
                    {
                        var user = UserManager.FindById(userId);
                        if (user != null)
                        {
                            var result = UserManager.Delete(user);
                        }
                    }
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { result = "successfully deleted" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { result = "company user is synced" });
                }
                ////get service response.          
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception" });
            }
        }

        /// <summary>
        ///  This method for change password in both AspNetUser and CompanyUser Table by id.
        ///  Update into AspNetUser and CompanyUser from database entity.
        /// </summary>
        /// <returns>Api response result</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("ChangePassword")]
        public HttpResponseMessage ChangePassword(CustomerPasswordDataModel customerPasswordData)
        {
            try
            {
                ////step: 1
                ////check is valid email.
                if (ServiceHelper.IsGuid(customerPasswordData.Id))
                {

                    var companyUser = companyUserService.GetCompanyUserByCompanyUserId(new Guid(customerPasswordData.Id));
                    if (companyUser == null)
                    {
                        // Don't reveal that the user does not exist
                        return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "ID Not Exist or Password not matched" });
                    }
                    else
                    {
                        var user = UserManager.FindById(companyUser.UserId);
                        if (user != null)
                        {
                            // encrypting password for company user table
                            var password = SecurityHelper.Encrypt(customerPasswordData.NewPassword, true);

                            //user.PasswordHash = customerPasswordData.NewPassword;
                            var response = UserManager.ChangePassword(companyUser.UserId, customerPasswordData.Password, customerPasswordData.NewPassword);
                            if (response.Succeeded)
                            {
                                var result = companyUserService.EditCompanyUserPassword(new Guid(customerPasswordData.Id), password);
                                ////check result
                                if (result == true)
                                {
                                    return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Password has been Successfuly Changed." });
                                }
                                else
                                {
                                    ////return response
                                    return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Current password is incorrect" });
                                }
                            }
                            else
                            {
                                return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Current password is incorrect" });
                            }

                        }
                        else
                        {
                            return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "company user not exist", Succeeded = false });
                        }
                    }
                }
                else
                {
                    ////case of invalid id request.
                    return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = "Id not exist", Succeeded = false });
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
        /// This method for get all record from "activity" entity.
        /// </summary>
        /// <returns>activity entity object value.</returns>
        [HttpGet]
        [Route("GetAllActivityByCompanyUserId/{CompanyUserId}")]
        public HttpResponseMessage GetAllActivityByCompanyUserId(string CompanyUserId)
        {
            try
            {
                ////get activtyCollection list 
                var activtyCollection = activityService.GetActivityByCompanyUserId(new Guid(CompanyUserId));

                ////check object
                if (activtyCollection.Count > 0 && activtyCollection != null)
                {
                    ////dynamic list.
                    dynamic activitys = new List<ExpandoObject>();

                    ////return response from activity service.
                    foreach (var activityDetail in activtyCollection)
                    {
                        ////bind dynamic property.
                        dynamic activity = new ExpandoObject();
                        ////map ids
                        activity.Id = activityDetail.Id;
                        activity.Activity = activityDetail.Activity;
                        activity.IsCompanyUser = activityDetail.IsCompanyUser;
                        activity.Description = activityDetail.Description;
                        activity.CreatedDate = activityDetail.CreatedDate.ToString("MM-dd-yyyy HH:mm:ss tt");

                        ////set activitys values in list.
                        activitys.Add(activity);
                    }
                    ////return companyUsers service 
                    return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK, (List<ExpandoObject>)activitys);
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

    }
}