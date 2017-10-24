using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessServices.HelperClass
{
    public class EmailUtility
    {
        public Boolean SendMail(string subject, string HTMLBody, string SendTO)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("info@eddee.it");
                mailMessage.To.Add("rajat.rathore@techvalens.com");
                mailMessage.Subject = subject;

                mailMessage.Body = HTMLBody;
                mailMessage.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient("104.215.253.235", 25);
                smtpClient.Send(mailMessage);

                //MailMessage Msg = new MailMessage();
                //// Sender e-mail address.
                //Msg.From = new MailAddress(" info@eddee.it");
                //// Recipient e-mail address.
                //Msg.To.Add(SendTO);
                //Msg.Subject = subject;
                //Msg.Body = HTMLBody;
                ////Msg.Body = htmlBody;
                //Msg.IsBodyHtml = true;
                //// your remote SMTP server IP.
                //SmtpClient smtp = new SmtpClient();
                //smtp.Host = "localhost";
                ////smtp.Port = 8889;
                ////smtp.Credentials = new System.Net.NetworkCredential(" info@eddee.it", "Eddee.2015");
                //smtp.UseDefaultCredentials = true;
                //smtp.Send(Msg);
                //Msg = null;
               return true;




           //      SmtpClient serv = new SmtpClient();
           // MailMessage msg = new MailMessage();
           // msg.To.Add("toadrr@domain.com);
           // msg.Body = "body";
           // msg.Subject = "subj";
           // msg.BodyEncoding = System.Text.Encoding.ASCII;
           // msg.IsBodyHtml = isHTML;
           // serv.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
           //serv.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SmtpServerUserName"], ConfigurationManager.AppSettings["SmtpServerPassword"]);
           //  serv.Send(msg);



            }
            catch (Exception ex)
            {
                return false;
            }
        }

       
        //public Boolean SendMailToVendor(MailMessage message)
        //{
        //    try
        //    {
        //        MailMessage Msg = new MailMessage();
        //        // Sender e-mail address.
        //        Msg.From = new MailAddress("rajat.rathore@techvalens.com");
        //        // Recipient e-mail address.
        //        Msg.To.Add(message.To.Add);
        //        Msg.Subject = "Eddee | Vendor Subscription ";
        //        Msg.Body = message.Body;
        //        //Msg.Body = htmlBody;
        //        Msg.IsBodyHtml = true;
        //        // your remote SMTP server IP.
        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Host = "smtp.gmail.com";
        //        smtp.Port = 587;
        //        smtp.Credentials = new System.Net.NetworkCredential("rajat.rathore@techvalens.com", "07312343125");
        //        smtp.EnableSsl = true;
        //        smtp.Send(Msg);
        //        Msg = null;
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}


        public void SendMailToVendor(Object mailMsg)
        {
            MailMessage mailMessage = (MailMessage)mailMsg;
            try
            {
                /* Setting should be kept somewhere so no need to 
                   pass as a parameter (might be in web.config)       */
                SmtpClient smtpClient = new SmtpClient();
                NetworkCredential networkCredential = new NetworkCredential();

                networkCredential.UserName = "support@eddee.com";
                networkCredential.Password = "";
                smtpClient.Host = "localhost";
                smtpClient.Credentials = networkCredential;
                smtpClient.Port = 25;

                //If you are using gmail account then
                smtpClient.EnableSsl = false;

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtpClient.Send(mailMessage);
            }
            catch (SmtpException ex)
            {
                // Code to Log error
            }
        }

        public static string RandomString(int Size)
        {
            Random random = new Random();
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, Size)
                                   .Select(x => input[random.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }
    }
}