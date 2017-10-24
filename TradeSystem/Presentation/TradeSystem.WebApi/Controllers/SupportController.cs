using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
using TradeSystem.MVCWeb;
using TradeSystem.Utils.Models;

namespace TradeSystem.WebApi.Controllers
{
    public class SupportController : BaseController
    {
       
        //The URL of the WEB API Service
        string url = WebConfigurationManager.AppSettings["url"];

        //Default Constructorof Support Controller
        public SupportController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }       

        #region Ticket Management
        /// <summary>
        /// Get my ticket list by customer id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> MyTickets()
        {
            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
            }
            var Lang = SiteLanguages.GetDefaultLanguage();
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/ticket/GetTicketListByCustomerId/" + Session["ClientUserId"].ToString() + "/" + Lang + "/" + "0" + "/" + "0");

            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var jsonData = JObject.Parse(responseData);
                var message = jsonData["Message"].ToString();
                var succeeded = jsonData["Succeeded"].ToString();
                var errorInfo = jsonData["ErrorInfo"].ToString();
                if (errorInfo == "false")
                {
                    return View("Error");
                }
                if (succeeded == "True")
                {
                    var response = jsonData["DataList"].ToString(); 
                    var objTicketDetails = JsonConvert.DeserializeObject<List<TicketDataModel>>(response);
                    // temp data initialization for export excel sheet
                    TempData["TempTicketList"] = objTicketDetails;
                    return View(objTicketDetails);
                }
                else
                {
                    List<TicketDataModel> obj = new List<TicketDataModel>();
                    return View(obj);
                }
            }
            return View("Error");
        }

        /// <summary>
        /// Get method for raise ticket by customer
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RaiseTicket()
        {
            if (Session["ClientUserId"] == null)
            {
                return RedirectToAction("Login", "CustomerManagement");
            }
            TicketDataModel promotionDataModel = new TicketDataModel();
            return View(promotionDataModel);
        }

        /// <summary>
        ///  Post method for raise ticket by customer
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> RaiseTicket(TicketDataModel ticketData)
        {
            try
            {
                //call api for add promotion
                ticketData.Lang = SiteLanguages.GetDefaultLanguage();
                ticketData.CustomerId = Session["ClientUserId"].ToString();
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/ticket/RaiseTicket", ticketData);
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var jsonData = JObject.Parse(responseData);
                var message = jsonData["Message"].ToString();
                var succeeded = jsonData["Succeeded"].ToString();
                var errorInfo = jsonData["ErrorInfo"].ToString();
                //check respose
                if (succeeded == "True")
                {
                    ViewBag.Success = "true";
                    ViewBag.Message = message;
                    TicketDataModel objTicketDataModel = new TicketDataModel();
                    return View(objTicketDataModel);
                }
                else
                {
                    ViewBag.Success = "false";
                    ViewBag.Message = message;
                    return View(ticketData);
                }                
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        /// <summary>
        /// Export selected data into excel sheet.
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportToExcel()
        {
            var lang = SiteLanguages.GetDefaultLanguage();
            var grid = new GridView();
            var modal = (List<TicketDataModel>)TempData.Peek("TempTicketList");
            if (modal == null)
            {
                grid.EmptyDataText = "No data available";
            }
            else
            {
                if (lang == "en")
                {
                    grid.DataSource = from p in modal
                                      select new
                                      {
                                          Date = p.CreatedDate,
                                          TicketID = p.Id,
                                          Title = p.Title,
                                          CustomerName = p.CustomerName,
                                          Status = p.Status
                                      };
                }
                else
                {
                    grid.DataSource = from p in modal
                                      select new
                                      {
                                          Fecha = p.CreatedDate,
                                          Identificacióndeentradas = p.Id,
                                          Titulo = p.Title,
                                          NombredelCliente = p.CustomerName,
                                          Estado = p.Status
                                      };
                }
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

        #endregion

        public ActionResult ExportExcel()
        {
            return View();
        }

    }
}