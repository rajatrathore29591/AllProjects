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
using TradeSystem.Framework.Entities;
using TradeSystem.Utils.Models;

namespace TradeSystem.WebApi.Controllers
{
    public class ReportController : BaseController
    {

        //The URL of the WEB API Service
        string url = WebConfigurationManager.AppSettings["url"];

        /// <summary>
        /// Constructor of Report
        /// </summary>
        public ReportController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// get customer report and bining country as well
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> CustomerReport()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            ViewBag.customerList = "";
            CustomerReportDataModel objCustomer = new CustomerReportDataModel();
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/customer/GetAllCountry");
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {

                new List<SelectListModel>();
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                ViewBag.Country = JsonConvert.DeserializeObject<List<SelectListModel>>(responseData);
                System.Web.HttpRuntime.Cache["Country"] = ViewBag.Country;
            }
            return View(objCustomer);
        }

        /// <summary>
        /// post method of customer report(managing all five reports of customer by this method)
        /// </summary>
        /// <param name="customerReportObj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CustomerReport(CustomerReportDataModel customerReportObj)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/report/GetAllCustomerReport", customerReportObj);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<CustomerReportDataModelList> objCustomerDetail = new List<CustomerReportDataModelList>();

                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                var jsonData = JObject.Parse(responseData);
                var message = jsonData["Message"].ToString();
                var errorInfo = jsonData["ErrorInfo"].ToString();

                var succeeded = jsonData["Succeeded"].ToString();
                if (succeeded == "True")
                {
                    var responseObjData = jsonData["DataList"].ToString();
                    if (customerReportObj.ReportType == "Referals")
                    {
                        objCustomerDetail = JsonConvert.DeserializeObject<List<CustomerReportDataModelList>>(responseObjData);
                        ViewBag.customerList = objCustomerDetail;
                        TempData["TempReportList"] = objCustomerDetail;
                        return View("_CustomerReferals");
                    }
                    if (customerReportObj.ReportType == "MoneyWithdrawal")
                    {
                        objCustomerDetail = JsonConvert.DeserializeObject<List<CustomerReportDataModelList>>(responseObjData);
                        ViewBag.customerList = objCustomerDetail;
                        TempData["TempReportList"] = objCustomerDetail;
                        return View("_CustomerMoneyWithdrawal");
                    }
                    if (customerReportObj.ReportType == "PrematureWithdrawal")
                    {
                        objCustomerDetail = JsonConvert.DeserializeObject<List<CustomerReportDataModelList>>(responseObjData);
                        ViewBag.customerList = objCustomerDetail;
                        TempData["TempReportList"] = objCustomerDetail;
                        return View("_CustomerPrematureWithdrawal");
                    }
                    else
                    {
                        objCustomerDetail = JsonConvert.DeserializeObject<List<CustomerReportDataModelList>>(responseObjData);
                        ViewBag.customerList = objCustomerDetail;
                        TempData["TempReportList"] = objCustomerDetail;
                        return View("_CustomerReport");
                    }
                }
                else
                {
                    if (customerReportObj.ReportType == "MoneyWithdrawal")
                    {
                        ViewBag.Message = jsonData["Message"].ToString();
                        ViewBag.customerList = objCustomerDetail;
                        return View("_CustomerMoneyWithdrawal");
                    }
                    if (customerReportObj.ReportType == "PrematureWithdrawal")
                    {
                        ViewBag.Message = jsonData["Message"].ToString();
                        ViewBag.customerList = objCustomerDetail;
                        return View("_CustomerPrematureWithdrawal");
                    }
                    else
                    {
                        ViewBag.Message = jsonData["Message"].ToString();
                        ViewBag.customerList = objCustomerDetail;
                        return View("_CustomerReport");
                    }
                }
            }
            else
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var jsonData = JObject.Parse(responseData);
                ViewBag.Message = jsonData["Message"].ToString();
            }
            return View();
        }

        /// <summary>
        /// Export Customer Report data into excel sheet.
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerReportExcel()
        {

            var grid = new GridView();
            var modal = (List<CustomerReportDataModelList>)TempData.Peek("TempReportList");
            if (modal == null)
            {
                grid.EmptyDataText = "No data available";
            }
            else
            {
                grid.DataSource = from p in modal
                                  select new
                                  {
                                      CustomerName = p.Name,
                                      TotalValueOfInvestment = p.TotalValueOfInvestment,
                                      CountryName = p.CountryName,
                                      StateName = p.StateName,
                                      CustomerCreatedDate = p.CreatedDate
                                  };
            }
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=CustomerReport_List.xls");
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

        /// <summary>
        /// Export Customer Money Withdrawal data into excel sheet.
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerMoneyWithdrawalExcel()
        {

            var grid = new GridView();
            var modal = (List<CustomerReportDataModelList>)TempData.Peek("TempReportList");
            if (modal == null)
            {
                grid.EmptyDataText = "No data available";
            }
            else
            {
                grid.DataSource = from p in modal
                                  select new
                                  {
                                      CustomerName = p.Name,
                                      TotalWithdrawalAmount = p.TotalValueOfInvestment,
                                      CountryName = p.CountryName,
                                      StateName = p.StateName,
                                      CustomerCreatedDate = p.CreatedDate
                                  };
            }
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=CustomerMoneyWithdrawalReport_List.xls");
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

        /// <summary>
        /// Export Customer Premature Withdrawal data into excel sheet.
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerPrematureWithdrawalExcel()
        {

            var grid = new GridView();
            var modal = (List<CustomerReportDataModelList>)TempData.Peek("TempReportList");
            if (modal == null)
            {
                grid.EmptyDataText = "No data available";
            }
            else
            {
                grid.DataSource = from p in modal
                                  select new
                                  {
                                      CustomerName = p.Name,
                                      ProductName = p.ProductName,
                                      TotalValueOfInvestment = p.TotalValueOfInvestment,
                                      ActualInvestmentDate = p.CreatedDate,
                                      CustomerInvestmentDate = p.CustomerWithdrawDate
                                  };
            }
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=CustomerPrematureReport_List.xls");
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

        /// <summary>
        /// Export Customer Referals data into excel sheet.
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerReferalsExcel()
        {

            var grid = new GridView();
            var modal = (List<CustomerReportDataModelList>)TempData.Peek("TempReportList");
            if (modal == null)
            {
                grid.EmptyDataText = "No data available";
            }
            else
            {
                grid.DataSource = from p in modal
                                  select new
                                  {
                                      CustomerName = p.Name,
                                      ReferalsCount = p.ReferalsCount,
                                      CountryName = p.CountryName,
                                      StateName = p.StateName,
                                      CustomerCreatedDate = p.CreatedDate
                                  };
            }
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=CustomerReport_List.xls");
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

        /// <summary>
        /// Get product report... calling getallproduct api
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ProductReport()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            ViewBag.customerList = "";
            //CustomerReportDataModel objCustomer = new CustomerReportDataModel();
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/product/GetProductSelectList");
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                new List<SelectListModel>();
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                ViewBag.Product = JsonConvert.DeserializeObject<List<SelectListModel>>(responseData);
            }
            return View();
        }

        /// <summary>
        /// post method of product report
        /// </summary>
        /// <param name="productReportObj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ProductReport(CustomerReportDataModel productReportObj)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/report/GetAllProductReport", productReportObj);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<CustomerReportDataModelList> objCustomerDetail = new List<CustomerReportDataModelList>();
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                var jsonData = JObject.Parse(responseData);
                var message = jsonData["Message"].ToString();
                var errorInfo = jsonData["ErrorInfo"].ToString();

                var succeeded = jsonData["Succeeded"].ToString();
                if (succeeded == "True")
                {
                    var responseObjData = jsonData["DataList"].ToString();
                    objCustomerDetail = JsonConvert.DeserializeObject<List<CustomerReportDataModelList>>(responseObjData);
                    ViewBag.customerList = objCustomerDetail;
                    TempData["TempReportList"] = objCustomerDetail;
                    return View("_ProductReport");
                }
                else
                {
                    ViewBag.Message = jsonData["Message"].ToString();
                    ViewBag.customerList = objCustomerDetail;
                    return View("_ProductReport");
                }
            }
            return View("Error");
        }

        /// <summary>
        /// Export Product Report data into excel sheet.
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductReportExcel()
        {

            var grid = new GridView();
            var modal = (List<CustomerReportDataModelList>)TempData.Peek("TempReportList");
            if (modal == null)
            {
                grid.EmptyDataText = "No data available";
            }
            else
            {
                grid.DataSource = from p in modal
                                  select new
                                  {
                                      CustomerName = p.Name,
                                      TotalMoneyInvested = p.TotalValueOfInvestment,
                                      CountryName = p.CountryName,
                                      StateName = p.StateName,
                                      CustomerCreatedDate = p.CreatedDate
                                  };
            }
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ProductReport_List.xls");
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

        /// <summary>
        /// Get withdraw report...
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> WithdrawalReport()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            ViewBag.customerList = "";
            CustomerReportDataModel obj = new CustomerReportDataModel();

            return View(obj);
        }

        /// <summary>
        /// post method of withdrawal report
        /// </summary>
        /// <param name="withdrawReportObj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> WithdrawalReport(CustomerReportDataModel withdrawReportObj)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/report/GetAllWithdrawReport", withdrawReportObj);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<CustomerReportDataModelList> objCustomerDetail = new List<CustomerReportDataModelList>();
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                var jsonData = JObject.Parse(responseData);
                var message = jsonData["Message"].ToString();
                var errorInfo = jsonData["ErrorInfo"].ToString();

                var succeeded = jsonData["Succeeded"].ToString();
                if (succeeded == "True")
                {
                    var responseObjData = jsonData["DataList"].ToString();
                    objCustomerDetail = JsonConvert.DeserializeObject<List<CustomerReportDataModelList>>(responseObjData);
                    ViewBag.customerList = objCustomerDetail;
                    TempData["TempReportList"] = objCustomerDetail;
                    return View("_WithdrawalReport");
                }
                else
                {
                    ViewBag.Message = jsonData["Message"].ToString();
                    ViewBag.customerList = objCustomerDetail;
                    return View("_WithdrawalReport");
                }
            }
            return View("Error");
        }

        /// <summary>
        /// Export Withdrawal Report data into excel sheet.
        /// </summary>
        /// <returns></returns>
        public ActionResult WithdrawalReportExcel()
        {

            var grid = new GridView();
            var modal = (List<CustomerReportDataModelList>)TempData.Peek("TempReportList");
            if (modal == null)
            {
                grid.EmptyDataText = "No data available";
            }
            else
            {
                grid.DataSource = from p in modal
                                  select new
                                  {
                                      CustomerName = p.Name,
                                      MoneyWithdrawal = p.MoneyWithdrawal,
                                      ProductName = p.ProductName,
                                  };
            }
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=WithdrawalReport_List.xls");
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

        /// <summary>
        /// get ticket report and bining status as well
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> TicketReport()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            ViewBag.customerList = "";
            CustomerReportDataModel objCustomer = new CustomerReportDataModel();
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/ticket/GetAllTicketStatus");
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                new List<SelectListModel>();
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                ViewBag.TicketStatus = JsonConvert.DeserializeObject<List<SelectListModel>>(responseData);
            }
            return View(objCustomer);
        }

        /// <summary>
        /// post method of ticket report
        /// </summary>
        /// <param name="ticketReportObj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> TicketReport(CustomerReportDataModel ticketReportObj)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Admin", "Account");
            }
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/report/GetAllTicketReport", ticketReportObj);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<CustomerReportDataModelList> objCustomerDetail = new List<CustomerReportDataModelList>();
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                var jsonData = JObject.Parse(responseData);
                var message = jsonData["Message"].ToString();
                var errorInfo = jsonData["ErrorInfo"].ToString();

                var succeeded = jsonData["Succeeded"].ToString();
                if (succeeded == "True")
                {
                    var responseObjData = jsonData["DataList"].ToString();
                    objCustomerDetail = JsonConvert.DeserializeObject<List<CustomerReportDataModelList>>(responseObjData);
                    ViewBag.ticketList = objCustomerDetail;
                    TempData["TempReportList"] = objCustomerDetail;
                    return View("_TicketReport");
                }
                else
                {
                    ViewBag.Message = jsonData["Message"].ToString();
                    ViewBag.ticketList = objCustomerDetail;
                    return View("_TicketReport");
                }
            }
            return View("Error");
        }

        /// <summary>
        /// Export Ticket Report data into excel sheet.
        /// </summary>
        /// <returns></returns>
        public ActionResult TicketReportExcel()
        {

            var grid = new GridView();
            var modal = (List<CustomerReportDataModelList>)TempData.Peek("TempReportList");
            if (modal == null)
            {
                grid.EmptyDataText = "No data available";
            }
            else
            {
                grid.DataSource = from p in modal
                                  select new
                                  {
                                      TicketId = p.TicketId,
                                      CustomerName = p.Name,
                                      Title = p.Title,
                                      Status = p.Status,
                                      CreatedDate = p.CreatedDate
                                  };
            }
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=TicketReport_List.xls");
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