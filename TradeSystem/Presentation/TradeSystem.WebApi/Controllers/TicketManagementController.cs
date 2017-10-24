using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
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
using TradeSystem.Utils.Models;
using TradeSystem.WebApi.Controllers;

namespace TradeSystem.MVCWeb.Controllers
{
    public class TicketManagementController : BaseController
    {        
        //The URL of the WEB API Service
        string url = WebConfigurationManager.AppSettings["url"];

        /// <summary>
        /// Initialized Ticket Management Controller Constructor.
        /// </summary>
        public TicketManagementController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Get method for show all tcket list  
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> CustomerSupport()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/ticket/GetAllTicket");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var objTicketDetails = JsonConvert.DeserializeObject<List<TicketDataModel>>(responseData);
                    // temp data initialization for export excel sheet
                    TempData["TempTicketList"] = objTicketDetails;
                    if (objTicketDetails != null)
                    {
                        return View(objTicketDetails);
                    }

                }
                else if (responseMessage.StatusCode == HttpStatusCode.NoContent)
                {
                    ViewBag.Message = "No data found";
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            return View();
        }

        /// <summary>
        /// Get method for ticket details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> TicketDetails(string id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            //ticketDataModel.Id = id;
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/ticket/GetTicketByTicketId/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var objTicketDetails = JsonConvert.DeserializeObject<TicketDataModel>(responseData);
                if (objTicketDetails != null)
                {
                    return View(objTicketDetails);
                }
                else
                {
                    return View("Error");
                }
            }
            return View("Error");
        }

        /// <summary>
        /// This method for post Ticket details
        /// </summary>
        /// <param name="objTicketDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> TicketDetails(TicketDataModel objTicketDataModel)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            objTicketDataModel.CompanyUserId = Session["UserId"].ToString();
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/ticket/EditTicket", objTicketDataModel);
            if (responseMessage.IsSuccessStatusCode)
            {
                //var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                //var objTicketDetails = JsonConvert.DeserializeObject<List<TicketDataModel>>(responseData);
                //if (objTicketDetails != null)
                //{
                //    return View(objTicketDetails);
                //}
                //else
                //{
                //    return View("Error");
                //}
                return RedirectToAction("CustomerSupport", "TicketManagement");
            }
            else
            {
                return RedirectToAction("CustomerSupport", "TicketManagement");
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
            var modal = (List<TicketDataModel>)TempData.Peek("TempTicketList");
            if (modal == null)
            {
                grid.EmptyDataText = "No data available";
            }
            else
            {
                grid.DataSource = from p in modal
                                  select new
                                  {
                                      TicketID = "TID:" + p.AutoIncrementedNo,
                                      Title = p.Title,
                                      CustomerName = p.CustomerName,
                                      Date = p.CreatedDate,
                                      Status = p.Status
                                  };
            }
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Ticket_List.xls");
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