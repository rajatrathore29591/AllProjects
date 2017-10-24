using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TradeSystem.Framework.Identity;
using TradeSystem.Service;
using TradeSystem.Utils;
using TradeSystem.Utils.Models;

namespace TradeSystem.MVCWeb.Controllers
{
    [Authorize]
    [RoutePrefix("api/role")]
    public class RoleApiController : ApiController
    {
        #region Dependencies Injection with initialization
        RoleStore<ApplicationRole> roleStore;
        RoleManager<ApplicationRole> roleManager;
        IRoleService roleService;
        IRoleSideMenuService roleSideMenuService;
        ISideMenuService sideMenuService;
        IProductService productService;

        //Initialized Role Api Controller Constructor.
        public RoleApiController(IRoleService _roleService, IRoleSideMenuService _roleSideMenuService, ISideMenuService _sideMenuService)
        {
            AppIdentityDbContext context = new AppIdentityDbContext();
            roleStore = new RoleStore<ApplicationRole>(context);
            roleManager = new RoleManager<ApplicationRole>(roleStore);
            roleService = _roleService;
            roleSideMenuService = _roleSideMenuService;
            sideMenuService = _sideMenuService;
        }
        #endregion

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
                //var superAdmin = "SuperAdmin";

                ////get roles list 
                var roleCollection = roleManager.Roles.ToList().OrderBy(x => x.Name);
                ////check object
                if (roleCollection != null)
                {
                    ////dynamic list.
                    dynamic roles = new List<ExpandoObject>();

                    ////return response form role service.
                    foreach (var roleDetail in roleCollection)
                    {
                        ////declare var
                        dynamic role = new ExpandoObject();

                        ////map ids
                        role.Id = roleDetail.Id;
                        ////map prop value
                        role.Name = roleDetail.Name;
                        role.Description = roleDetail.Description;

                        ////skip super admin in table
                        if (roleDetail.Name != "SuperAdmin")
                        {
                            ////set role values in list.
                            roles.Add(role);
                        }
                    }
                    ////return role list
                    return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK, (List<ExpandoObject>)roles);
                }
                else
                {
                    ////case of invalid id request.
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
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
        /// Post mentod for Add Role
        /// </summary>
        /// <param name="roleDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddRole")]
        public HttpResponseMessage AddRole(RoleDataModel roleDataModel)
        {
            try
            {
                //Create Role Super Admin if it does not exist
                var role = roleManager.FindByName(roleDataModel.Name);
                if (role == null)
                {
                    ApplicationRole roleData = new ApplicationRole(roleDataModel.Name);
                    roleData.Name = roleDataModel.Name;
                    roleData.Description = roleDataModel.Description;

                    var roleresult = roleManager.Create(roleData);
                    ////return result from service response.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Successfully Added" });
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Already Added" });
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

        /// <summary>
        /// This method use for get role info by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>role details</returns>
        [HttpGet]
        [Route("GetRoleDetail/{id}")]
        public HttpResponseMessage GetRoleDetail(string id)
        {
            try
            {
                ////get role.
                Guid RoleId = new Guid(id);
                var roleDetail = roleService.GetRoleById(RoleId);
                if (roleDetail != null)
                {
                    ////bind dynamic property.
                    dynamic role = new ExpandoObject();

                    ////map ids
                    role.Id = roleDetail.Id;
                    role.Name = roleDetail.Name;
                    ////map prop value
                    role.Description = roleDetail.Description;
                    ////return service for get role.
                    return this.Request.CreateResponse<ExpandoObject>(HttpStatusCode.OK, (ExpandoObject)role);
                }
                else
                {
                    ////case of record not found.
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
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
        /// Post method for edit the role
        /// </summary>
        /// <param name="roleDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EditRole")]
        public HttpResponseMessage EditRole(RoleDataModel roleDataModel)
        {
            try
            {
                var roleCheck = roleManager.FindByName(roleDataModel.Name);
                if (roleCheck == null)
                {

                    //Create Role Super Admin if it does not exist
                    //var role = roleManager.FindById(roleDataModel.Id);
                    //if (role != null)
                    //{
                    //var role1 = new ApplicationRole();
                    //ApplicationRole roleData = new ApplicationRole(roleDataModel.Name);
                    //roleData.Id = roleDataModel.Id;
                    //roleData.Description = roleDataModel.Description;
                    //role.Name = roleDataModel.Name;
                    //role.Description = roleDataModel.Description;
                    ApplicationRole roleObj = new ApplicationRole();
                    roleObj.Id = roleDataModel.Id;
                    roleObj.Name = roleDataModel.Name;
                    roleObj.Description = roleDataModel.Description;

                    var roleresult = roleManager.Update(roleObj);
                    ////return result from service response.
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Successfully Updated" });
                    //}
                    //else
                    //{
                    //    return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Data Not Found" });
                    //}
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Role already exist" });
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

        /// <summary>
        /// Get method for delete role 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteRole/{roleId}")]
        public HttpResponseMessage DeleteRole(Guid roleId)
        {
            try
            {
                var sideMenuRole = roleSideMenuService.GetRoleSideMenuByRoleId(roleId);
                if (sideMenuRole.Count < 1)
                {
                    //Create Role Super Admin if it does not exist
                    var role = roleManager.FindById(roleId.ToString());
                    if (role != null)
                    {
                        var roleUser = roleService.GetRoleCompanyUserByRoleId(roleId);
                        if (roleUser == true)
                        {
                            dynamic roleExit = new ExpandoObject();
                            roleExit.RoleExistInCompanyUser = true;
                            //return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Role is synced can't delete it" });
                            return this.Request.CreateResponse<ExpandoObject>(HttpStatusCode.OK, (ExpandoObject)roleExit);
                        }
                        else
                        {
                            dynamic roleExit = new ExpandoObject();
                            roleExit.RoleExitInCompanyUser = true;
                            var roleresult = roleManager.Delete(role);
                            ////return result from service response.
                            return this.Request.CreateResponse<ExpandoObject>(HttpStatusCode.OK, (ExpandoObject)roleExit);
                            //return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Successfully Deleted" });
                        }
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "Data Not Found" });
                    }
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.NoContent, new { Message = "Role is synced can't delete it" });
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

        /// <summary>
        /// Get method for Get All Side Menu By RoleId
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllSideMenuByRoleId/{roleId}")]
        public HttpResponseMessage GetAllSideMenuByRoleId(Guid roleId)
        {
            try
            {

                ////get sideMenu list 
                var sideMenuCollection = sideMenuService.GetAllSideMenu();
                var roleSideMenuCollection = roleSideMenuService.GetRoleSideMenuByRoleId(roleId);
                ////check object
                if (sideMenuCollection.Count > 0 && sideMenuCollection != null)
                {
                    ////dynamic list.
                    dynamic roleSideMenus = new List<ExpandoObject>();

                    ////return response form user service.
                    foreach (var sideMenuDetail in sideMenuCollection)
                    {
                        bool Checked = false;
                        dynamic roleSideMenu = new ExpandoObject();
                        if (roleSideMenuCollection.Count > 0 && roleSideMenuCollection != null)
                        {
                            foreach (var roleSideMenuDetail in roleSideMenuCollection)
                            {
                                ////declare var
                                if (sideMenuDetail.Id == roleSideMenuDetail.SideMenuId)
                                {
                                    Checked = true;

                                }
                            }
                        }

                        ////map ids
                        roleSideMenu.SideMenuId = sideMenuDetail.Id;
                        ////map prop value
                        roleSideMenu.SideMenuName = sideMenuDetail.Name;
                        roleSideMenu.SideMenuDescription = sideMenuDetail.Description;
                        roleSideMenu.IsChecked = Checked;
                        //roleSideMenus.RoleSideMenuId = roleSideMenuDetail.
                        roleSideMenus.Add(roleSideMenu);
                    }
                    ////return role list
                    return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK, (List<ExpandoObject>)roleSideMenus);
                }
                else
                {
                    ////case of invalid id request.
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest);
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
        /// This method use for add record into entity.
        /// rolesidemenu entity.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="sideMenuId"></param>
        /// <returns>result</returns>
        [HttpPost]
        [Route("AddRoleSideMenu")]
        public HttpResponseMessage AddRoleSideMenu(List<RoleSideMenuDataModel> roleSideMenuData)
        {
            try
            {
                var result = false;
                if (roleSideMenuData.Count > 0 && roleSideMenuData != null)
                {
                    // deleting existing roleId from rolesidemenu table
                    var response = roleSideMenuService.DeleteRoleSideMenu(new Guid(roleSideMenuData[0].RoleId));
                    if (response)
                    {
                        foreach (var item in roleSideMenuData)
                        {
                            /// add record in roleSideMenu entity 
                            result = roleSideMenuService.AddRoleSideMenu(new Guid(item.SideMenuId), item.RoleId);
                            /// return if not succeded
                            if (result == false)
                            {
                                return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "not inserted successfully" });
                            }
                        }
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "not successfully deleted the rolesidemenu" });
                    }
                }
                ////return service for get user.
                return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "rolesidemenu successfully created!!" });
            }
            catch (Exception ex)
            {
                //// handel exception log.
                Console.Write(ex.Message);
                ////return case of exception.
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { result = "Exception : " + ex.Message });
            }
        }

        /// <summary>
        /// This method use for get all side menu role info.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>role details</returns>
        [HttpGet]
        [Route("GetAllSideMenu")]
        public HttpResponseMessage GetAllSideMenu()
        {
            try
            {
                ////get sideMenu list 
                var sideMenuCollection = sideMenuService.GetAllSideMenu();

                ////check object
                if (sideMenuCollection.Count > 0 && sideMenuCollection != null)
                {
                    ////dynamic list.
                    dynamic sideMenuDetails = new List<ExpandoObject>();
                    ////bind dynamic property.

                    ////return response from sideMenu service.
                    foreach (var sideMenuDetail in sideMenuCollection)
                    {
                        dynamic sideMenu = new ExpandoObject();
                        ////get sideMenu list 
                        sideMenu.Id = sideMenuDetail.Id;
                        sideMenu.RoleSideMenu = sideMenuDetail.Name;



                        ////set all values in list.
                        sideMenuDetails.Add(sideMenu);
                    }

                    ////return all service 
                    return this.Request.CreateResponse<List<ExpandoObject>>(HttpStatusCode.OK, (List<ExpandoObject>)sideMenuDetails);
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Data Not Found." });
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
