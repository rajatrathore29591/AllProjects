using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Configuration;
using TradeSystem.Utils.Models;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Web.UI;
using TradeSystem.WebApi.Controllers;

namespace TradeSystem.MVCWeb.Controllers
{
    //[RoutePrefix("AccountManagement")]
    public class AccountManagementController : BaseController
    {
       
        //The URL of the WEB API Service
        string url = WebConfigurationManager.AppSettings["url"];
        static List<CompanyUserDataModel> objCompanyUserDetails;

        /// <summary>
        /// Constructor for Account Management
        /// </summary>
        public AccountManagementController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Get method for Existing Account
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ExistingAccount()
        {
            // sending bearer token while calling api
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["AccessToken"]);

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/CompanyUser/GetAllCompanyUser");
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //List<CompanyUserDataModel> objDashbordDetails = new List<CompanyUserDataModel>();
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                objCompanyUserDetails = JsonConvert.DeserializeObject<List<CompanyUserDataModel>>(responseData);
                TempData["TempCompanyUserList"] = objCompanyUserDetails;
                if (objCompanyUserDetails != null)
                {

                    return View(objCompanyUserDetails);
                }
                else
                {
                    return View("Error");
                }
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                List<CompanyUserDataModel> obj = new List<CompanyUserDataModel>();
                return View(obj);
            }
            return View("Error");
        }

        // GET: AddCompanyUser
        // [Route("CreateAccount")]
        public async Task<ActionResult> CreateAccount()
        {
            Session["RoleId"] = "";
            ViewBag.EmailMessage = "";
            ViewBag.PasswordMessage = "";
            ViewBag.Message = "";
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/role/GetAllRoles");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                ViewBag.Roles = JsonConvert.DeserializeObject<List<RoleDataModel>>(responseData);
                System.Web.HttpRuntime.Cache["Roles"] = ViewBag.Roles;
                CompanyUserDataModel companyUserData = new CompanyUserDataModel();
                return View(companyUserData);
            }
            return View("Error");

        }

        //The Post method
        //[Route("CreateAccount")]
        [HttpPost]
        public async Task<ActionResult> CreateAccount(CompanyUserDataModel companyUserData)
        {
            Session["RoleId"] = Request.Form["RoleId"];
            if (ModelState.IsValid)
            {
                if (companyUserData.Password == companyUserData.ConfirmPassword)
                {
                    if (companyUserData.Email.Trim() == companyUserData.ConfirmEmail.Trim())
                    {

                        //var roleId = Request.Form["RoleId"];
                        companyUserData.RoleId = Request.Form["RoleId"];
                        companyUserData.FirstName = Request.Form["FirstName"];
                        companyUserData.MiddleName = Request.Form["MiddleName"];
                        companyUserData.LastName = Request.Form["LastName"];
                        companyUserData.Phone = Request.Form["Phone"];
                        companyUserData.Address = Request.Form["Address"];
                        companyUserData.UserName = Request.Form["Email"];
                        companyUserData.Password = Request.Form["Password"];
                        companyUserData.Email = Request.Form["Email"];
                        companyUserData.IsActive = true;
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/companyuser/AddCompanyUser", companyUserData);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            CompanyUserDataModel obj = new CompanyUserDataModel();
                            Session["RoleId"] = null;
                            return RedirectToAction("ExistingAccount", "AccountManagement");
                        }
                        else if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            ViewBag.Roles = System.Web.HttpRuntime.Cache["Roles"];
                            ViewBag.Message = "This email already exist";
                            return View(companyUserData);

                        }
                        else
                        {

                            ViewBag.Message = "Something went wrong.";
                        }
                    }
                    else
                    {
                        ViewBag.Roles = System.Web.HttpRuntime.Cache["Roles"];
                        ViewBag.PasswordMessage = "";
                        ViewBag.EmailMessage = "Email and Confirmed Email not matched";
                        return View(companyUserData);
                    }
                }
                else
                {
                    if (companyUserData.Email.Trim() != companyUserData.ConfirmEmail.Trim())
                    {
                        ViewBag.EmailMessage = "Email and Confirmed Email not matched";
                    }
                    ViewBag.Roles = System.Web.HttpRuntime.Cache["Roles"];
                    ViewBag.PasswordMessage = "Password and Confirmed Password not matched";
                    return View(companyUserData);
                }
            }
            else
            {
                ViewBag.Roles = System.Web.HttpRuntime.Cache["Roles"];
                return View(companyUserData);
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Show details for edit company user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CompanyUserDataModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> EditAccount(string id)
        {
            ViewBag.Message = "";
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            //var id = Session["UserId"];

            HttpResponseMessage response = await client.GetAsync(url + "/role/GetAllRoles");
            if (response.IsSuccessStatusCode)
            {
                var responseData = response.Content.ReadAsStringAsync().Result;

                ViewBag.Roles = JsonConvert.DeserializeObject<List<RoleDataModel>>(responseData);
            }

            HttpResponseMessage responseMessage = await client.GetAsync(url + string.Format("/CompanyUser/GetCompanyUserDetail/" + id));
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var objCompanyUserDetail = JsonConvert.DeserializeObject<CompanyUserDataModel>(responseData);
                if (objCompanyUserDetail != null)
                {
                    return View(objCompanyUserDetail);
                }
                else
                {
                    return View("Error");
                }
            }
            return View("Error");
        }

        /// <summary>
        /// Post show details for edit company user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="objCompanyUserDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EditAccount(CompanyUserDataModel objCompanyUserDataModel)
        {
            if (objCompanyUserDataModel.Status == "Active")
            {
                objCompanyUserDataModel.IsActive = true;
            }
            if (objCompanyUserDataModel.Status == "DeActive")
            {
                objCompanyUserDataModel.IsActive = false;
            }
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/CompanyUser/EditCompanyUser", objCompanyUserDataModel);
            if (responseMessage.IsSuccessStatusCode)
            {

                //objCompanyUserDataModel = new CompanyUserDataModel();
                //ModelState.Clear();
                return RedirectToAction("ExistingAccount", "AccountManagement");
                //ViewBag.Message = "Company user successfully updated";
                //return View(objCompanyUserDataModel);
            }
            return View("Error");
        }

        /// <summary>
        /// delete company user
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> DeleteCompanyUser(string Id)
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url + "companyuser/DeleteCompanyUser/" + Id);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ExistingAccount", "AccountManagement");
            }
            else
            {
                ViewBag.Message = "Something went wrong.";
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Partial view for model _ViewLogPartial
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> _ViewLogPartial(string id)
        {
            // login company user id
            //var CompanyUserId = Session["UserId"].ToString();
            CompanyActivityListDataModel obj = new CompanyActivityListDataModel();
            CompanyUserDataModel objViewLog = new CompanyUserDataModel();
            List<ActivityLogDataModel> objActivity = new List<ActivityLogDataModel>();

            HttpResponseMessage responseMessage = await client.GetAsync(url + string.Format("/CompanyUser/GetCompanyUserDetail/" + id));
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                objViewLog = JsonConvert.DeserializeObject<CompanyUserDataModel>(responseData);
                if (objViewLog != null)
                {
                    obj.CompanyUser = objViewLog;
                    //return PartialView(objViewLog);
                }
            }

            HttpResponseMessage response = await client.GetAsync(url + "/CompanyUser/GetAllActivityByCompanyUserId/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseBank = response.Content.ReadAsStringAsync().Result;
                objActivity = JsonConvert.DeserializeObject<List<ActivityLogDataModel>>(responseBank);
                if (objActivity != null)
                {
                    obj.Activity = objActivity;
                    return PartialView(obj);
                }
                else
                {
                    return PartialView(obj);
                }
            }

            return View("Error");
        }

        /// <summary>
        /// Get method for Edit My Profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> EditMyProfile(string id)
        {
            ViewBag.Message = "";
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            HttpResponseMessage responseMessage = await client.GetAsync(url + string.Format("/CompanyUser/GetCompanyUserDetail/" + id));
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var objCompanyUserDetail = JsonConvert.DeserializeObject<CompanyUserDataModel>(responseData);
                if (objCompanyUserDetail != null)
                {
                    return View(objCompanyUserDetail);
                }
                else
                {
                    return View("Error");
                }
            }
            return View("Error");
        }

        /// <summary>
        /// Post method for Edit My Profile
        /// </summary>
        /// <param name="objCompanyUserDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EditMyProfile(CompanyUserDataModel objCompanyUserDataModel)
        {
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/CompanyUser/EditCompanyUser", objCompanyUserDataModel);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }
            return View("Error");
        }

        /// <summary>
        /// Export selected data into excel sheet.
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportToExcel()
        {

            var grid = new GridView();
            var modal = (List<CompanyUserDataModel>)TempData.Peek("TempCompanyUserList");
            if (modal == null)
            {
                grid.EmptyDataText = "No data available";
            }
            else
            {
                grid.DataSource = from p in modal
                                  select new
                                  {
                                      Date = p.CreatedDate,
                                      Email = p.Email,
                                      FullName = p.FirstName + " " + p.MiddleName + " " + p.LastName,
                                      RoleType = p.Role,
                                      Status = p.Status
                                  };
            }
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=CompanyUsers_List.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View("Index");
        }

        #region Role Method's

        /// <summary>
        /// Get method for show role list using partial view .
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> _RoleListPartial()
        {
            List<RoleDataModel> objRoleDataModelList;
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/role/GetAllRoleS");
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                //Convert json to object type 
                objRoleDataModelList = JsonConvert.DeserializeObject<List<RoleDataModel>>(responseData);
                if (objRoleDataModelList != null)
                {
                    return PartialView(objRoleDataModelList);
                }
                else
                {
                    return View("Error");
                }
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                List<RoleDataModel> obj = new List<RoleDataModel>();
                return View(obj);
            }
            return View("Error");
        }

        /// <summary>
        /// Get method for add role.
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageRolePermission()

        {
            ViewBag.Message = "";
            ViewBag.ErrorMessage = "";
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }

            RoleDataModel objRoleDataModel = new RoleDataModel();
            return View(objRoleDataModel);
        }

        /// <summary>
        /// Post Method for add role
        /// </summary>
        /// <param name="objRoleDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ManageRolePermission(RoleDataModel objRoleDataModel)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            ViewBag.ErrorMessage = "";
            TempData["ErrorMessage"] = "";
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/Role/AddRole", objRoleDataModel);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var objRoleDetails = JsonConvert.DeserializeObject<RoleDataModel>(responseData);
                if (objRoleDetails != null)
                {
                    objRoleDataModel = new RoleDataModel();
                    ModelState.Clear();
                    ViewBag.Message = "Role successfully created";
                    return View(objRoleDataModel);
                    //return RedirectToAction("AddRole", objRoleDataModel);
                }
                else
                {
                    return View("Error");
                }
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ViewBag.ErrorMessage = "Role already exist";
                return View();
            }
            return View("Error");
        }

        /// <summary>
        /// Get method for get data by role id for edit method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetRoleByRoleId(string id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            var responseMessage = await client.GetAsync(string.Format("role/GetRoleDetail/" + id));
            if (responseMessage.IsSuccessStatusCode)
            {
                //List<RoleDataModel> objDashbordDetails = new List<RoleDataModel>();
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var objRoleDetails = JsonConvert.DeserializeObject<RoleDataModel>(responseData);
                if (objRoleDetails != null)
                {
                    return View("EditRole", objRoleDetails);
                }
                else
                {
                    return View("Error");
                }
            }
            return View("Error");
        }

        /// <summary>
        /// Get method for edit role.
        /// </summary>
        /// <returns></returns>
        public ActionResult EditRole()
        {
            ViewBag.Message = "";
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            RoleDataModel objRoleDataModel = new RoleDataModel();
            return View(objRoleDataModel);
        }

        /// <summary>
        /// Post method for edit role by role id
        /// </summary>
        /// <param name="objRoleDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EditRoleByRoleId(RoleDataModel objRoleDataModel)
        {
            TempData["ErrorMessage"] = "";
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/Role/EditRole", objRoleDataModel);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var objRoleDetails = JsonConvert.DeserializeObject<RoleDataModel>(responseData);
                if (objRoleDetails != null)
                {
                    //objRoleDataModel = new RoleDataModel();
                    //ModelState.Clear();
                    return RedirectToAction("ManageRolePermission", "AccountManagement");
                    //return View(objRoleDataModel);
                }
                else
                {
                    return View("Error");
                }
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["ErrorMessage"] = "Role already exist";
                return RedirectToAction("ManageRolePermission", "AccountManagement");
            }
            return View("Error");
        }

        /// <summary>
        /// Get method for delete role
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> DeleteRole(string Id)
        {
            TempData["DeleteMessage"] = "";
            TempData["DeleteMessageExistCompanyUser"] = "";
            HttpResponseMessage responseMessage = await client.GetAsync(url + "role/DeleteRole/" + Id);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var objRoleDetails = JsonConvert.DeserializeObject<RoleDataModel>(responseData);
                if (objRoleDetails.RoleExistInCompanyUser == true)
                {
                    TempData["DeleteMessageExistCompanyUser"] = "This Role can't be deleted because it's already in used.";
                    return RedirectToAction("ManageRolePermission", "AccountManagement");
                }
                else
                {
                    return RedirectToAction("ManageRolePermission", "AccountManagement");
                }
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                TempData["DeleteMessage"] = "Can't delete the role as it is synced with SideMenu";
                return RedirectToAction("EditRole", "AccountManagement");
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        ///  Get method for show role title and get manage role title by id
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageRole(RoleDataModel objRoleDataModel)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            TempData["RoleTitle"] = objRoleDataModel.Name;
            return View(objRoleDataModel);
        }

        /// <summary>
        ///  Get method for show all role title list and show using partial view
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> _ManageRoleTitlePartial(string roleId)
        {

            // var roleId = Convert.ToString(TempData["RoleId"]);
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/role/GetAllSideMenuByRoleId/" + roleId);
            //  HttpResponseMessage responseMessage = await client.GetAsync(url + "/role/GetAllSideMenu");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var objRoleDataModelList = JsonConvert.DeserializeObject<List<RoleSideMenuDataModel>>(responseData);
                if (objRoleDataModelList != null)
                {
                    return PartialView(objRoleDataModelList);

                }
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Post method for manage role
        /// </summary>
        /// <param name="listModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ManageRole(List<RoleSideMenuDataModel> listModel)
        {
            List<RoleSideMenuDataModel> roleSideMenuData = new List<RoleSideMenuDataModel>();
            if (listModel != null && listModel.Count > 0)
            {

                foreach (var item in listModel)
                {

                    if (item.IsChecked)
                    {
                        RoleSideMenuDataModel objRoleDataModel = new RoleSideMenuDataModel();
                        objRoleDataModel.SideMenuId = item.SideMenuId;
                        objRoleDataModel.RoleId = Convert.ToString(TempData["RoleId"]);
                        roleSideMenuData.Add(objRoleDataModel);
                    }
                    else
                    {
                        // do not do anything. ( go to hell)
                    }


                }
            }
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/role/AddRoleSideMenu", roleSideMenuData);
            if (responseMessage.IsSuccessStatusCode)
            {
                return View("ManageRolePermission");
            }

            return RedirectToAction("Error");
        }

        #endregion

    }
}