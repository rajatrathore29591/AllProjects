using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradeSystem.Framework.Entities;
using TradeSystem.Utils.Models;
using TradeSystem.WebApi.Controllers;

namespace TradeSystem.MVCWeb.Controllers
{
    public class InventoryManagementController : BaseController
    {      
        
        //The URL of the WEB API Service
        string url = WebConfigurationManager.AppSettings["url"];

        /// <summary>
        /// Constructor of Inventory Management Controller
        /// </summary>
        public InventoryManagementController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Get method for display ProductList
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Inventory()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/product/GetAllProduct");

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var objProductDetails = JsonConvert.DeserializeObject<List<ProductDataModel>>(responseData);
                    TempData["TempInventoryList"] = objProductDetails;
                    if (objProductDetails != null)
                    {
                        return View(objProductDetails);
                    }
                }
                else if (responseMessage.StatusCode == HttpStatusCode.NoContent)
                {
                    List<ProductDataModel> objProductDetails = new List<ProductDataModel>();
                    //ViewBag.Message = "No data found";
                    return View(objProductDetails);
                }

            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Method for get product details using product id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> InvestmentDetails(string id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            string customerId = "0";
            HttpResponseMessage responseMessage = await client.GetAsync(url + string.Format("/product/GetProductByProductId/" + id + "/" + customerId));
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var objCompanyUserDetail = JsonConvert.DeserializeObject<ProductDataModel>(responseData);
                if (objCompanyUserDetail != null)
                {
                    DateTime InvestmentWithdrawDate = Convert.ToDateTime(objCompanyUserDetail.InvestmentWithdrawDate);
                    var FormatDate = InvestmentWithdrawDate.ToString("MM/dd/yyyy");
                    DateTime CreatedDate = objCompanyUserDetail.CreatedDate;
                    var FormatDateOfCreatedDate = CreatedDate.ToString("MM/dd/yyyy");
                    var diffr = (Convert.ToDateTime(FormatDate).Date - Convert.ToDateTime(FormatDateOfCreatedDate).Date).TotalDays;
                    objCompanyUserDetail.TotalDaysOfInvestment = Convert.ToInt32(diffr);
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
        /// Get method for bind data in model using partial view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> _PenaltyChartPartial(string id, int totalDaysofInvestment)
        {

            List<PenaltyDataModel> objPenaltyChart = new List<PenaltyDataModel>();
            HttpResponseMessage responseMessage = await client.GetAsync(url + string.Format("/product/GetAllPenaltyByProductId/" + id));
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                objPenaltyChart = JsonConvert.DeserializeObject<List<PenaltyDataModel>>(responseData);
                var lastvalue = objPenaltyChart.LastOrDefault();
                if (Convert.ToInt32(lastvalue.PenaltyTo) < totalDaysofInvestment|| Convert.ToInt32(lastvalue.PenaltyTo) != totalDaysofInvestment)
                {
                    int lastPenaltyFrom = Convert.ToInt32(lastvalue.PenaltyTo) + 1;
                 
                //Add new last row for show 0 percent 
                objPenaltyChart.Add(new PenaltyDataModel() { PenaltyFrom = Convert.ToString(lastPenaltyFrom), PenaltyTo = Convert.ToString(totalDaysofInvestment), PenaltyPercent = "0" });
                }
                if (objPenaltyChart != null)
                {
                    return PartialView(objPenaltyChart);
                }
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
            var modal = (List<ProductDataModel>)TempData.Peek("TempInventoryList");
            if (modal == null)
            {
                grid.EmptyDataText = "No data available";
            }
            else
            {
                modal = modal.OrderBy(x => x.Name).ToList();
                grid.DataSource = from p in modal
                                  select new
                                  {
                                      InvestmentName = p.Name,
                                      TotalValue = p.TotalValueOfInvestment,
                                      RemainingValue = p.RemainingValueOfInvestment,
                                      PublishDate = p.CreatedDate,
                                      NoOfCustomers = p.CustomerProductsCount,
                                      // Status = p.IsActive
                                  };
            }
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Inventory_List.xls");
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
    }
}