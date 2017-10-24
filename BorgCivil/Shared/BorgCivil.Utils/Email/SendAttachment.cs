using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace BorgCivil.Utils.Email
{
    public class SendAttachment
    {
        public static Task SendEmailAttachment(string toEmail, string ccEmail, string bccEmail, string fromEmail, string subject, string message, string smtpUserName, string smtpEmail, string smtpPassword, string smtpServer, int smtpPort,string fileName)
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

            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(HostingEnvironment.MapPath("~/Uploads/" + fileName));
            msg.Attachments.Add(attachment);

            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(smtpUserName, smtpPassword);
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {
                ////Update Date Failed with Reason
                string failedReason = ex.Message.ToString();
            }
            return Task.FromResult(0);
        }

    }
}
