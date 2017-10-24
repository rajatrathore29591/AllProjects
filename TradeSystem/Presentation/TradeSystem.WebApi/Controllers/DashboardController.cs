using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using TradeSystem.Utils;
using TradeSystem.Utils.Models;
using TradeSystem.WebApi.Controllers;

namespace TradeSystem.MVCWeb.Controllers
{
    public class DashboardController : BaseController
    {
        //The URL of the WEB API Service
        string url = WebConfigurationManager.AppSettings["url"];

        /// <summary>
        /// Constructor of Dashboard Controller
        /// </summary>
        public DashboardController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Get method for index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///  GET method for Dashboard of admin 
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Dashboard()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/dashboardadmin/GetAllDashboardAdmin");
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var objDashbordDetails = JsonConvert.DeserializeObject<List<DashboardDataModel>>(responseData);
                if (objDashbordDetails != null)
                {
                    ViewBag.CountProduct = objDashbordDetails[0].ProductCount;
                    ViewBag.totalProductValue = objDashbordDetails[0].TotalValueOfInvestment.ToString();
                    ViewBag.CountCustomer = objDashbordDetails[0].CustomerCount;
                    ViewBag.totalCustomerValue = objDashbordDetails[0].ProductAmount.ToString();
                    return View();
                }
                else
                {
                    return View("Error");
                }
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                ViewBag.CountProduct = "0";
                ViewBag.totalProductValue = "0";
                ViewBag.CountCustomer = "0";
                ViewBag.totalCustomerValue = "0";
                return View();
            }
            return View("Error");
        }

        #region For Customer Dashboard

        /// <summary>
        ///  GET method for Dashboard of customer
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> CustomerDashboard()
        {

            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
            }
            var customerId = Session["ClientUserId"];
            var Lang = SiteLanguages.GetDefaultLanguage();
            HttpResponseMessage responsePromotion = await client.GetAsync(url + "/promotion/GetAllPromotionByCustomerId/" + customerId + "/" + Lang);

            HttpResponseMessage responseMessage = await client.GetAsync(url + "/dashboardadmin/GetAllDashboard/" + customerId + "/" + Lang);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var jsonData = JObject.Parse(responseData);
                var response = jsonData["DataList"].ToString();
                var objDashbordDetails = JsonConvert.DeserializeObject<List<DashboardDataModel>>(response);
                if (objDashbordDetails != null)
                {
                    //ViewBag.totalProductValue = string.Format("{0:0.00}", objDashbordDetails[0].TotalValueOfInvestment);
                    ViewBag.totalProductValue = objDashbordDetails[0].TotalValueOfInvestment.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
                    ViewBag.earningByInvestment = objDashbordDetails[0].EarningByInvestment.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
                    ViewBag.earningBySales = objDashbordDetails[0].EarningBySales.ToString("C", CultureInfo.CreateSpecificCulture("en-US")); ;
                    ViewBag.virtualWalletValue = objDashbordDetails[0].VirtualWalletValue.ToString("C", CultureInfo.CreateSpecificCulture("en-US")); ;
                    ViewBag.totalInvestmentsCount = objDashbordDetails[0].ProductCount;
                    ViewBag.totalSalesCount = objDashbordDetails[0].SaleCount;

                    return View();
                }
                else
                {
                    return View("Error");
                }
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                ViewBag.CountProduct = "0";
                ViewBag.totalProductValue = "0";
                ViewBag.CountCustomer = "0";
                ViewBag.totalCustomerValue = "0";
                return View();
            }
            return View("Error");
        }

        #endregion

        #region Notification update

        /// <summary>
        /// Get method for Notifications Update
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> NotificationsUpdate()
        {
            var Lang = SiteLanguages.GetDefaultLanguage();
            //api call for get data of notification
            PromotionDataModel objPromotionDataModel = new PromotionDataModel();
            //api call for wallet amount
            HttpResponseMessage responseResult = await client.GetAsync(url + "/finance/GetWalletAmountByCustomerId/" + Session["ClientUserId"].ToString() + "/" + Lang);

            if (responseResult.StatusCode == HttpStatusCode.OK)
            {
                var responseDataResult = responseResult.Content.ReadAsStringAsync().Result;
                var jsonDataResult = JObject.Parse(responseDataResult);
                var DataResult = jsonDataResult["DataObject"].ToString();
                var objCustomerDetails = JsonConvert.DeserializeObject<WithdrawDataModel>(DataResult);

                Session["VirtualWalletAmount"] = objCustomerDetails.AvailableBalance;
            }
            HttpResponseMessage responsePromotion = await client.GetAsync(url + "/promotion/GetAllPromotionByCustomerId/" + Session["ClientUserId"] + "/" + Lang);
            if (responsePromotion.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responsePromoptionData = responsePromotion.Content.ReadAsStringAsync().Result;

                var jsonobjData = JObject.Parse(responsePromoptionData);
                var responsejsonobjData = jsonobjData["DataList"].ToString();
                var objPromotionDetail = JsonConvert.DeserializeObject<List<PromotionDataModel>>(responsejsonobjData);

                //List for promnotion details store on session
                List<PromotionDataModel> listPromotion = (List<PromotionDataModel>)Session["PromotionListDetails"];

                List<PromotionDataModel> listPromotionItems = new List<PromotionDataModel>();
                foreach (var item in objPromotionDetail)
                {
                    PromotionDataModel promotionDataModel = new PromotionDataModel();
                    promotionDataModel.Id = item.Id;
                    promotionDataModel.Description = item.Description;
                    promotionDataModel.DescriptionSpanish = item.DescriptionSpanish;
                    promotionDataModel.Url = item.Url;
                    promotionDataModel.Viewed = item.Viewed;
                    promotionDataModel.CreatedDate = item.CreatedDate;
                    listPromotionItems.Add(promotionDataModel);
                }

                Session["PromotionListDetails"] = listPromotionItems;
            }
            PromotionDataModel objpromotionDataModel = new PromotionDataModel();
            //for code select amount 
            float virtualWalletAmount = float.Parse(Session["VirtualWalletAmount"].ToString());

            string virtualWallet = virtualWalletAmount.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));

            return Json(virtualWallet, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Admin Notification update

        /// <summary>
        /// Get method for Notifications Update
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AdminNotificationsUpdate()
        {
            var Lang = SiteLanguages.GetDefaultLanguage();
            //api call for get data of notification
            PromotionDataModel objPromotionDataModel = new PromotionDataModel();
          
            //api call for notification amount
            HttpResponseMessage responsePromotion = await client.GetAsync(url + "/promotion/GetAllPromotionByCustomerId/" + "0" + "/" + "en");
            if (responsePromotion.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responsePromoptionData = responsePromotion.Content.ReadAsStringAsync().Result;

                var jsonobjData = JObject.Parse(responsePromoptionData);
                var responsejsonobjData = jsonobjData["DataList"].ToString();
                var objPromotionDetail = JsonConvert.DeserializeObject<List<PromotionDataModel>>(responsejsonobjData);

                //List for promnotion details store on session
                List<PromotionDataModel> listPromotion = (List<PromotionDataModel>)Session["AdminPromotionListDetails"];

                List<PromotionDataModel> listPromotionItems = new List<PromotionDataModel>();
                foreach (var item in objPromotionDetail)
                {
                    PromotionDataModel promotionDataModel = new PromotionDataModel();
                    promotionDataModel.Id = item.Id;
                    promotionDataModel.Description = item.Description;
                    promotionDataModel.DescriptionSpanish = item.DescriptionSpanish;
                    promotionDataModel.Url = item.Url;
                    promotionDataModel.Viewed = item.Viewed;
                    promotionDataModel.CreatedDate = item.CreatedDate;
                    listPromotionItems.Add(promotionDataModel);
                }

                Session["AdminPromotionListDetails"] = listPromotionItems;
            }

            return null;
        }
        #endregion
    }
}
