using BorgCivil.Utils.Models;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web.Hosting;
using System;
using BorgCivil.Service;
using System.Collections.Specialized;
using BorgCivil.Utils.Email;
using System.Dynamic;
using System.Web.Configuration;
using System.Web.Http.Cors;

namespace BorgCivil.MVCWeb.Controllers
{
    [RoutePrefix("EmailAttachment")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmailAttachmentController : Controller
    {

        //Initialized interface object. 
        #region Dependencies Injection with initialization

        IEmailService EmailService;

        // Constructor of EmailAttachment Controller
        public EmailAttachmentController(IEmailService _EmailService)
        {
            EmailService = _EmailService;
        }

        #endregion

        /// <summary>
        /// Export selected data into excel sheet.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SendAttachment")]
        public BaseResponseDataModel EmailAttachmentBooking(List<BookingDataModel> Booking)
        {
            BaseResponseDataModel response = new BaseResponseDataModel();
            try
            {
                var grid = new GridView();
                var modal = Booking;

                if (modal == null)
                {
                    grid.EmptyDataText = "No data available";
                }
                else
                {

                    grid.DataSource = from p in modal
                                      select new
                                      {
                                          BookingNumber = p.BookingNumber,
                                          CustomerName = p.CustomerName,
                                          SiteDetail = p.SiteDetail,
                                          WorkType = p.WorkType,
                                          StatusTitle = p.StatusTitle,
                                          FleetCount = p.FleetCount,
                                          FleetBookingDateTime = p.FleetBookingDateTime,
                                          EndDate = p.EndDate,
                                      };

                }
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Booking_List.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                string renderedGridView = sw.ToString();
                var filePath = HostingEnvironment.MapPath("~/Uploads/Booking_List.xlsx");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete((filePath));
                }
                System.IO.File.WriteAllText(HostingEnvironment.MapPath("~/Uploads/Booking_List.xlsx"), renderedGridView);

                ListDictionary Replacements = new ListDictionary();
                Replacements = new ListDictionary { { "<%FirstName%>", "BCA User" } };

                // reading key from web config of toEmailAddress
                var ToEmail = WebConfigurationManager.AppSettings["Email"];

                ////Code for Sending Email
                EmailService.SendEmailAttachmentAsync(new Guid(Booking[0].BookingId), ToEmail, string.Empty, string.Empty, ToEmail, EmailTemplatesHelper.SendAttachment, "", Replacements, "Booking_List.xlsx");

                response.Message = "Successfully send";
                response.Succeeded = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.ToString();
                response.Succeeded = false;
                return response;
            }
        }

    }
}