
using System;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace BorgCivil.Utils.Email
{
     
    public class SendEmailHelper
    {
        /// <summary>
        /// This Method for Send Email
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Task SendAsync(string toEmail, string ccEmail, string bccEmail, string fromEmail, string subject, string message, string smtpUserName, string smtpEmail, string smtpPassword, string smtpServer, int smtpPort)
        {
            //// Plug in your email service here to send an email.
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(smtpEmail);
            msg.To.Add(new MailAddress(toEmail));
            if (!string.IsNullOrEmpty(bccEmail))
                msg.Bcc.Add(new MailAddress(bccEmail));
            if (!string.IsNullOrEmpty(ccEmail))
                msg.CC.Add(new MailAddress(ccEmail));
            msg.Subject = subject;
            // msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(message.Body, null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(message, null, MediaTypeNames.Text.Html));
            ////Sent Email
            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(smtpUserName, smtpPassword);
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            try
            {
                smtpClient.Send(msg);
            } 
            catch(Exception ex)
            {
                ////Update Date Failed with Reason
                string failedReason = ex.Message.ToString();
            }
            return Task.FromResult(0);

        }
    }
}
