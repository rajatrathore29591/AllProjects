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
using TradeSystem.Framework.Entities;
using TradeSystem.Utils.Models;
using TradeSystem.WebApi.Controllers;

namespace TradeSystem.MVCWeb.Controllers
{

    public class PromotionController : BaseController
    {
       
        //The URL of the WEB API Service
        string url = WebConfigurationManager.AppSettings["url"];

        /// <summary>
        /// Default Constructor of Promotion
        /// </summary>
        public PromotionController()
        {

            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Get method for add promotion details in DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Promotion()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            PromotionDataModel promotionDataModel = new PromotionDataModel();
            return View(promotionDataModel);
        }

        /// <summary>
        /// Post method for save add promption details
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Promotion(PromotionDataModel promotionDataModel)
        {
            if (promotionDataModel.Description != null || promotionDataModel.Alert != null)
            {
                //call api for add promotion
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/promotion/AddPromotion", promotionDataModel);

                //check respose
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (promotionDataModel.PromotionType == "Alert on Web & App")
                    {
                        ViewBag.Success = "true";
                        ViewBag.Message = "Alert has been sent successfully.";
                    }
                    else
                    {
                        ViewBag.Success = "true";
                        ViewBag.Message = "Email has been sent successfully.";
                    }

                    PromotionDataModel objPromotion = new PromotionDataModel();
                    return View(objPromotion);
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
                {

                    ViewBag.Message = "Something went wrong.";
                    PromotionDataModel objPromotion = new PromotionDataModel();
                    return View(objPromotion);
                }
            }
            else
            {
                ViewBag.DescriptionMessage = "Description value is required.";
                PromotionDataModel objPromotion = new PromotionDataModel();
                return View(objPromotion);
            }
            return View();
        }

        /// <summary>
        /// Get method for show role list using partial view .
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> _CustomerList()
        {
            //List<CustomerDataModel> promotionDataModelList;
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/promotion/GetCustomerSelectList");
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                new List<Customer>();
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                ViewBag.Customer = JsonConvert.DeserializeObject<List<Customer>>(responseData);
                System.Web.HttpRuntime.Cache["Customer"] = ViewBag.Customer;
                //Convert json to object type 
                // promotionDataModelList = JsonConvert.DeserializeObject<List<CustomerDataModel>>(responseData);

                if (ViewBag.Customer != null)
                {
                    return View();
                }
                else
                {
                    ViewBag.Message = "No customer available";
                    return View("Error");
                }
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                List<CustomerDataModel> obj = new List<CustomerDataModel>();
                return View(obj);
            }
            return View("Error");
        }

        /// <summary>
        /// Get mehtod for Check Promotion
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckPromotion()
        {
            return View();
        }

        #region Promotion for Admin
        /// <summary>
        /// Update the view status in DB
        /// </summary>
        /// <param name="promotionId"></param>
        /// <returns></returns>
        public async Task<ActionResult> _EditPromotionAdmin(string promotionId)
        {
            var Lang = SiteLanguages.GetDefaultLanguage();
            var Viewed = true;
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/promotion/EditPromotionViewed/" + promotionId + "/" + Lang + "/" + Viewed);
            if (responseMessage.IsSuccessStatusCode)
            {
                List<TradeSystem.Utils.Models.PromotionDataModel> objPromotionDetail = (List<TradeSystem.Utils.Models.PromotionDataModel>)Session["AdminPromotionListDetails"];
                List<PromotionDataModel> listPromotionItems = new List<PromotionDataModel>();
                foreach (var item in objPromotionDetail)
                {
                    PromotionDataModel promotionDataModel = new PromotionDataModel();
                    promotionDataModel.Id = item.Id;
                    promotionDataModel.Description = item.Description;
                    promotionDataModel.DescriptionSpanish = item.DescriptionSpanish;
                    promotionDataModel.Url = item.Url;

                    if (item.Id == promotionId)
                    {
                        promotionDataModel.Viewed = true;
                    }
                    else
                    {
                        promotionDataModel.Viewed = item.Viewed;
                    }
                    promotionDataModel.CreatedDate = item.CreatedDate;
                    listPromotionItems.Add(promotionDataModel);
                }

                Session["AdminPromotionListDetails"] = listPromotionItems;
            }
            //Code for get description of notification   
            HttpResponseMessage responsePromotion = await client.GetAsync(url + "/promotion/GetPromotionByPromotionId/" + promotionId + "/" + "en");
            PromotionDataModel objPromotion = new PromotionDataModel();
            if (responsePromotion.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseData = responsePromotion.Content.ReadAsStringAsync().Result;
                objPromotion = JsonConvert.DeserializeObject<PromotionDataModel>(responseData);

            }
            return Json(objPromotion.Description, JsonRequestBehavior.AllowGet);
            //return PartialView();
        }

        /// <summary>
        /// Show notification grid of admin
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminNotifications()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            return View();
        }

        #endregion

        #region Promotion for customer
        /// <summary>
        /// Update the view status in DB
        /// </summary>
        /// <param name="promotionId"></param>
        /// <returns></returns>
        public async Task<ActionResult> _EditPromotion(string promotionId)
        {
            var Lang = SiteLanguages.GetDefaultLanguage();
            var Viewed = true;
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/promotion/EditPromotionViewed/" + promotionId + "/" + Lang + "/" + Viewed);
            if (responseMessage.IsSuccessStatusCode)
            {
                List<TradeSystem.Utils.Models.PromotionDataModel> objPromotionDetail = (List<TradeSystem.Utils.Models.PromotionDataModel>)Session["PromotionListDetails"];
                List<PromotionDataModel> listPromotionItems = new List<PromotionDataModel>();
                foreach (var item in objPromotionDetail)
                {
                    PromotionDataModel promotionDataModel = new PromotionDataModel();
                    promotionDataModel.Id = item.Id;
                    promotionDataModel.Description = item.Description;
                    promotionDataModel.DescriptionSpanish = item.DescriptionSpanish;
                    promotionDataModel.Url = item.Url;

                    if (item.Id == promotionId)
                    {
                        promotionDataModel.Viewed = true;
                    }
                    else
                    {
                        promotionDataModel.Viewed = item.Viewed;
                    }
                    promotionDataModel.CreatedDate = item.CreatedDate;
                    listPromotionItems.Add(promotionDataModel);
                }

                Session["PromotionListDetails"] = listPromotionItems;
            }
            //Code for get description of notification   
            HttpResponseMessage responsePromotion = await client.GetAsync(url + "/promotion/GetPromotionByPromotionId/" + promotionId + "/" + Lang);
            PromotionDataModel objPromotion = new PromotionDataModel();
            if (responsePromotion.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseData = responsePromotion.Content.ReadAsStringAsync().Result;
                objPromotion = JsonConvert.DeserializeObject<PromotionDataModel>(responseData);

            }
            return Json(objPromotion.Description, JsonRequestBehavior.AllowGet);
            //return PartialView();
        }

        /// <summary>
        /// Show notification grid of customer
        /// </summary>
        /// <returns></returns>
        public ActionResult Notifications()
        {
            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
            }
            return View();
        }
        #endregion
    }
}