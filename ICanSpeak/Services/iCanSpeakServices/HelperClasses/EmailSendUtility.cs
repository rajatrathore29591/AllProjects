using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace iCanSpeakServices.HelperClasses
{
    public class EmailSendUtility
    {
        public Boolean SendMail(string subject, string HTMLBody, string SendTO)
        {
            try
            {
                string fromMsg = ConfigurationManager.AppSettings["EmailID"].ToString();
                string password = ConfigurationManager.AppSettings["MailPassword"].ToString();

                MailMessage Msg = new MailMessage();
                Msg.From = new MailAddress(fromMsg, "iCanSpeak");
                Msg.To.Add(SendTO) ;
                Msg.Subject = subject;
                // Set Mail Body Here
                Msg.Body = HTMLBody;
                Msg.IsBodyHtml = true;
                Msg.Priority = MailPriority.High;
                ///SmtpClient client = new SmtpClient("mail.techvalens.net", 25);
                //SmtpClient client = new SmtpClient("smtp.mail.yahoo.com", 25);
                SmtpClient client = new SmtpClient("smtp.gmx.com", 25);
                client.EnableSsl = false;
                //SmtpClient client = new SmtpClient("smtp.gmail.com", 25);
                //client.EnableSsl = true;
                client.Credentials = new NetworkCredential(fromMsg, password); //set mail account password ...
                client.Send(Msg);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}