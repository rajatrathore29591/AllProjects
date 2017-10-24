using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using TradeSystem.Utils.Models;

namespace TradeSystem.MVCWeb.Controllers
{
    public class RoleController : Controller
    {

        HttpClient client;
        //The URL of the WEB API Service
        string url = WebConfigurationManager.AppSettings["url"];

        public RoleController()
        {

            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


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
        public ActionResult AddRole()

        {
            ViewBag.Message = "";
            ViewBag.ErrorMessage = "";
            if (Session["UserId"] == null)
            {
                return RedirectToAction("AdminLogin", "Account");
            }

            RoleDataModel objRoleDataModel = new RoleDataModel();
            return View(objRoleDataModel);
        }

        // Post Method for add role
        [HttpPost]
        public async Task<ActionResult> AddRole(RoleDataModel objRoleDataModel)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("AdminLogin", "Account");
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
                return RedirectToAction("AdminLogin", "Account");
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
                return RedirectToAction("AdminLogin", "Account");
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
                return RedirectToAction("AdminLogin", "Account");
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
                    return RedirectToAction("AddRole", "Role");
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
                return RedirectToAction("AddRole", "Role");
            }
            return View("Error");
        }

        //The DELETE method
        [HttpGet]
        public async Task<ActionResult> DeleteRole(string Id)
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url + "role/DeleteRole/" + Id);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("AddRole", "Role");
            }
            else
            {
                ViewBag.Message = "Something went wrong.";
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
                return RedirectToAction("AdminLogin", "Account");
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

        //The Post method
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
                return View("AddRole");
            }

            return RedirectToAction("Error");
        }


    }
}
