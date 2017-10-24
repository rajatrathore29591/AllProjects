using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using TradeSystem.MVCWeb.Models;
using TradeSystem.MVCWeb.Providers;
using TradeSystem.MVCWeb.Results;
using TradeSystem.Framework.Entities;
using System.Net;
using System.Collections.Specialized;
using TradeSystem.Utils;
using TradeSystem.Utilities.Email;
using TradeSystem.Utils.Models;
using TradeSystem.Framework.Identity;
using TradeSystem.Utils.Enum;
using TradeSystem.Service;
using System.Dynamic;
using TradeSystem.WebApi.Controllers;
using System.Globalization;

namespace TradeSystem.MVCWeb.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountApiController : BaseApiController
    {
        //Initialized interface object. 
        ICompanyUserService companyUserService;
        ICustomerService customerService;
        IEmailService emailService;
        IRoleSideMenuService roleSideMenuService;
        ISideMenuService sideMenuService;
        IActivityService activityService;

        private const string LocalLoginProvider = "Local";
        private Framework.Identity.ApplicationUserManager _userManager;

        // Constructor of Account Api Controller 
        public AccountApiController(ICompanyUserService _companyUserService, ICustomerService _customerService, IEmailService _emailService, IRoleSideMenuService _roleSideMenuService, ISideMenuService _sideMenuService, IActivityService _activityService)
        {
            companyUserService = _companyUserService;
            customerService = _customerService;
            emailService = _emailService;
            roleSideMenuService = _roleSideMenuService;
            sideMenuService = _sideMenuService;
            activityService = _activityService;
        }
        
        //Initialzing User Manager 
        public Framework.Identity.ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<Framework.Identity.ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET method for UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        /// <summary>
        /// This method for companyLogin from "companyUsers" entity.
        /// </summary>
        /// <returns>AccessTokenModel entity object value.</returns>
        [AllowAnonymous]
        [Route("CompanyLogin")]
        public async Task<HttpResponseMessage> CompanyLogin(LoginDataModel loginModel)
        {
            try
            {
                if (loginModel.Email.Contains("@") == true)
                {
                    Framework.Identity.ApplicationUser user = await UserManager.FindByEmailAsync(loginModel.Email);
                    if (user != null)
                    {
                        loginModel.Email = user.UserName;
                    }
                    else { return Request.CreateResponse(System.Net.HttpStatusCode.NoContent); }

                }

                Framework.Identity.ApplicationUser companyUser = await UserManager.FindAsync(loginModel.Email, loginModel.Password);

                if (companyUser != null)
                {
                    // get Access Token info
                    var tkn = App_Helper.AuthToken.GetLoingInfo(loginModel);

                    if (tkn != null)
                    {
                        var TokenModel = new AccessTokenModel();
                        TokenModel = tkn;
                        CompanyUser companyUserName = companyUserService.GetCompanyUserByAspNetUserId(companyUser.Id.ToString());
                        TokenModel.organisationId = companyUserName.Id.ToString();
                        TokenModel.loginUserId = companyUserName.Id.ToString();
                        TokenModel.loginUserName = companyUserName.FirstName + " " + companyUserName.LastName;

                        // inserting data into activity table
                        ActivityLogDataModel activityObj = new ActivityLogDataModel();
                        activityObj.Activity = ETaskStatus.LogIn.ToString();
                        activityObj.Description = "LogIn By Company User named : " + companyUserName.FirstName + " " + companyUserName.LastName;
                        activityObj.IsCompanyUser = true;
                        activityObj.CompanyUserId = companyUserName.Id.ToString();
                        var activityResult = activityService.AddActivity(activityObj);

                        if (companyUserName.IsSuperAdmin == false)
                        {
                            var sideMenuId = roleSideMenuService.GetRoleSideMenuByRoleId(new Guid(companyUserName.RoleId));
                            List<string> sideMenus = new List<string>();
                            foreach (var item in sideMenuId)
                            {
                                var sideMenuName = sideMenuService.GetSideMenuById(item.SideMenuId).Name;
                                sideMenus.Add(sideMenuName);
                            }
                            if (sideMenus.Contains(Roles.CustomerManagement))
                            {
                                TokenModel.CustomerManagement = "true";
                            }
                            else { TokenModel.CustomerManagement = null; }
                            if (sideMenus.Contains(Roles.InvestmentConfiguration))
                            {
                                TokenModel.InvestmentConfiguration = "true";
                            }
                            else { TokenModel.InvestmentConfiguration = null; }
                            if (sideMenus.Contains(Roles.Inventory))
                            {
                                TokenModel.Inventory = "true";
                            }
                            else { TokenModel.Inventory = null; }
                            if (sideMenus.Contains(Roles.FinanceManagement))
                            {
                                TokenModel.FinanceManagement = "true";
                            }
                            else { TokenModel.FinanceManagement = null; }
                            if (sideMenus.Contains(Roles.TicketManagement))
                            {
                                TokenModel.TicketManagement = "true";
                            }
                            else { TokenModel.TicketManagement = null; }
                            if (sideMenus.Contains(Roles.AccountManagement))
                            {
                                TokenModel.AccountManagement = "true";
                            }
                            else { TokenModel.AccountManagement = null; }
                            if (sideMenus.Contains(Roles.Reports))
                            {
                                TokenModel.Reports = "true";
                            }
                            else { TokenModel.Reports = null; }
                            if (sideMenus.Contains(Roles.Promotions))
                            {
                                TokenModel.Promotions = "true";
                            }
                            else { TokenModel.Promotions = null; }
                        }
                        else
                        {
                            TokenModel.CustomerManagement = "true";
                            TokenModel.InvestmentConfiguration = "true";
                            TokenModel.Inventory = "true";
                            TokenModel.FinanceManagement = "true";
                            TokenModel.TicketManagement = "true";
                            TokenModel.AccountManagement = "true";
                            TokenModel.Reports = "true";
                            TokenModel.Promotions = "true";
                        }
                        AccountService.testmylog();
                        return Request.CreateResponse<AccessTokenModel>(System.Net.HttpStatusCode.OK, TokenModel);
                    }
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "token not found" });
                }
                else
                {
                    return Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                //// Handel Exception Log.
                Console.Write(ex.Message);
                ////return case of exception.
                return this.Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, new { Message = ex.Message });
            }

        }

        /// <summary>
        /// This method for Forgot Password(Both CompnayUser and Customer).
        /// </summary>
        /// <returns>Status with message.</returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("ForgotPassword/{email}/{isAdmin}")]
        public HttpResponseMessage ForgotPassword(string email, string isAdmin)
        {
            try
            {
                ListDictionary replacements = new ListDictionary();
                var encryptedPassword = string.Empty;
                ////step: 1
                ////check is valid email.
                if (!string.IsNullOrEmpty(email))
                {
                    if (isAdmin == "false")
                    {
                        var customer = customerService.GetCustomerByEmail(email);
                        if (customer != null)
                        {
                            dynamic customerModel = new ExpandoObject();
                            customerModel.FirstName = customer.FirstName;
                            customerModel.Email = customer.Email;
                            customerModel.Password = customer.Password;

                            string coupon = SecurityHelper.Decrypt(customer.Password, true);
                            replacements = new ListDictionary { { "<%FirstName%>", customer.FirstName } };
                            replacements.Add("<%Subject%>", "Your password has changed");
                            replacements.Add("<%LastName%>", customer.LastName);
                            replacements.Add("<%username%>", customer.UserName);
                            replacements.Add("<%Password%>", coupon);

                            ////step:7
                            ////add email data
                            var sendEmail = emailService.AddEmailData(customer.Id.ToString(), customer.Email, string.Empty, string.Empty, string.Empty, EmailTemplatesHelper.CompanyCreateEmail, replacements);

                            ////step:8
                            ////send email.
                            if (sendEmail != null)
                            {
                                ////Code for Sending Email                             
                                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Email Sended Successfully.", Succeeded = true, DataObject = customerModel, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                            }
                            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = "Email Address Not Exist.", Succeeded = true, DataObject = customerModel, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                        }
                        else
                        {
                            return this.Request.CreateResponse(HttpStatusCode.NoContent, new { message = "Email Address Not Exist" });
                        }
                    }
                    else
                    {
                        var companyUser = companyUserService.GetCompanyUserByEmail(email);
                        if (companyUser != null)
                        {
                            string coupon = SecurityHelper.Decrypt(companyUser.Password, true);
                            replacements = new ListDictionary { { "<%FirstName%>", companyUser.FirstName } };
                            replacements.Add("<%Subject%>", "Your password has changed");
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
                            return this.Request.CreateResponse(HttpStatusCode.OK, new { message = "Email Sended Successfully." });
                        }
                        // Don't reveal that the user does not exist or is not confirmed
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
        /// This method for logout 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            Framework.Identity.ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new Framework.Identity.ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new Framework.Identity.ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion

        #region Account Login Api Mobile

        /// <summary>
        /// This method for Forgot Password(Both CompnayUser and Customer).
        /// </summary>
        /// <returns>Status with message.</returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("CustomerForgotPassword/{email}/{isAdmin}/{lang}")]
        public HttpResponseMessage CustomerForgotPassword(string email, string isAdmin, string lang)
        {
            try
            {
                ListDictionary replacements = new ListDictionary();
                var encryptedPassword = string.Empty;
                ////step: 1
                ////check is valid email.
                if (!string.IsNullOrEmpty(email))
                {
                    if (isAdmin == "false")
                    {
                        var customer = customerService.GetCustomerByEmail(email);
                        if (customer != null)
                        {
                            dynamic customerModel = new ExpandoObject();
                            customerModel.FirstName = customer.FirstName;
                            customerModel.Email = customer.Email;
                            customerModel.Password = customer.Password;

                            string coupon = SecurityHelper.Decrypt(customer.Password, true);
                            replacements = new ListDictionary { { "<%FirstName%>", customer.FirstName } };
                            replacements.Add("<%Subject%>", "Your password has changed");
                            replacements.Add("<%LastName%>", customer.LastName);
                            replacements.Add("<%email%>", customer.Email);
                            replacements.Add("<%username%>", customer.UserName);
                            replacements.Add("<%Password%>", coupon);

                            ////step:7
                            ////add email data
                            var sendEmail = emailService.AddEmailData(customer.Id.ToString(), customer.Email, string.Empty, string.Empty, string.Empty, EmailTemplatesHelper.CompanyCreateEmail, replacements);

                            ////step:8
                            ////send email.
                            if (sendEmail != null)
                            {
                                ////Code for Sending Email
                                emailService.SendEmailAsync(customer.Id, customer.Email, string.Empty, string.Empty, customer.Email, EmailTemplatesHelper.CompanyCreateEmail, encryptedPassword, replacements);
                                
                                // Get message from the resoures file using key
                                string st = resmanager.GetString("Api_EmailSendedSuccessfully", CultureInfo.GetCultureInfo(lang));
                                return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st, Succeeded = true, DataObject = customerModel, DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                            }
                            string st1 = resmanager.GetString("Api_EmailAddressNotExist", CultureInfo.GetCultureInfo(lang));
                            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st1, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "false" });
                        }
                        else
                        {
                            string st1 = resmanager.GetString("Api_EmailAddressNotExist", CultureInfo.GetCultureInfo(lang));
                            return this.Request.CreateResponse(HttpStatusCode.OK, new BaseResponseDataModel { Message = st1, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "false" });
                        }
                    }
                    else
                    {
                        var companyUser = companyUserService.GetCompanyUserByEmail(email);
                        if (companyUser != null)
                        {
                            string coupon = SecurityHelper.Decrypt(companyUser.Password, true);
                            replacements = new ListDictionary { { "<%FirstName%>", companyUser.FirstName } };
                            replacements.Add("<%Subject%>", "Your password has changed");
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
                            return this.Request.CreateResponse(HttpStatusCode.OK, new { message = "Email Sended Successfully." });
                        }
                        // Don't reveal that the user does not exist or is not confirmed
                        return this.Request.CreateResponse(HttpStatusCode.NoContent, new { message = "Email Address Not Exist" });
                    }
                    ////case of valid id request.
                    string str = resmanager.GetString("Api_EmailisEmpty", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new BaseResponseDataModel { Message = str, Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                }
                else
                {
                    ////case of invalid id request.
                    // string str = resmanager.GetString("Api_EmailisEmpty", CultureInfo.GetCultureInfo(lang));
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new BaseResponseDataModel { Message = "", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = "" });
                    // return this.Request.CreateResponse(HttpStatusCode.BadRequest);
                }

            }
            catch (Exception ex)
            {
                //// handel exception log.
                Console.Write(ex.Message);

                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "", Succeeded = false, DataObject = new ExpandoObject(), DataList = new List<ExpandoObject>(), ErrorInfo = ex.Message });
                //return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = "Exception : " + ex.Message });
            }
        }
        #endregion

    }
}
