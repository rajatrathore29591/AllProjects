using TradeSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeSystem.Framework.Entities;
using System.Collections;

using System.Collections.Specialized;
using Ipm.Hub.Utilities.Email;

namespace TradeSystem.Service
{
    /// <summary>
    /// This Service For All CRUD Operation FROM "Document" Entity. 
    /// </summary>
    public class EmailService : BaseService, IEmailService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;
        ICustomerService customerService;
        //Initialized Parameterized Constructor.
        public EmailService(IUnitOfWork _unitOfWork, ICustomerService _customerService) { unitOfWork = _unitOfWork;customerService = _customerService; }

        #endregion

        /// <summary>
        /// this method for send email.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="orgnasationName"></param>
        /// <param name="toEmail"></param>
        /// <param name="ccEmail"></param>
        /// <param name="bccEmail"></param>
        /// <param name="fromEmail"></param>
        /// <param name="templateName"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="smtpUserName"></param>
        /// <param name="smtpEmail"></param>
        /// <param name="smtpPassword"></param>
        /// <param name="smtpServer"></param>
        /// <param name="smtpPort"></param>
        /// <returns>result</returns>
        public void SendEmailAsync(Guid id, string toEmail, string ccEmail, string bccEmail, string fromEmail, string templateName,string password, ListDictionary replacements)
        {
            try
            {
                ////get email template.
                var template = GetEmailTemplate(templateName);

                ////check templet is exist.
                if (template != null)
                {
                    ////prapere email body
                    string message = PrepareMessageBody(template.Body, replacements);
                    string messageSubject = PrepareMessageBody(template.Subject, replacements);
                    ////get email setting
                    var emailSetting = GetEmailSettings();

                    ////send email 
                    SendEmailHelper.SendAsync(toEmail, ccEmail, bccEmail, fromEmail, messageSubject, message, emailSetting.SmtpUserName, emailSetting.SmtpEmail, emailSetting.SmtpPassword, emailSetting.SmtpServer, emailSetting.SmtpPort);

                    ////update email data case of success.
                    UpdateCustomerPasswordData(toEmail, password);
                }
                else
                {
                    ////update email data case of templet not found
                    //UpdateEmailData(id, "Template not found.", null,  DateTime.UtcNow, null);
                }
            }
            catch (Exception ex)
            {
                ////update email data case of faluir
                //UpdateEmailData(id, ex.Message, null, DateTime.UtcNow, null);
            }
        }

        /// <summary>
        /// Insert Record from Send Mail Entity.
        /// </summary>
        /// <returns></returns>
        /// <param name="toEmail"></param>
        /// <param name="ccEmail"></param>
        /// <param name="bccEmail"></param>
        /// <param name="fromEmail"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="failedReason"></param>
        /// <param name="archived"></param>
        /// <param name="templateName"></param>
        /// <param name="dateFailed"></param>
        /// <param name="dateSent"></param>
        public SentEmail AddEmailData(string userId, string toEmail, string ccEmail, string bccEmail, string fromEmail, string templateName, ListDictionary replacements)
        {
            try
            {
                ////get email template.
                var template = GetEmailTemplate(templateName);

                ////check templet is exist.
                if (template != null)
                {
                    //get email setting 
                    var emailSetting = GetEmailSettings();

                    ////prapere email body
                    string message = PrepareMessageBody(template.Body, replacements);
                    string messageSubject = PrepareMessageBody(template.Subject, replacements);
                    ////map entity
                    SentEmail entity = new SentEmail();
                    entity.Id = Guid.NewGuid();
                    entity.ToEmail = toEmail;
                    entity.BccEmail = bccEmail;
                    entity.CcEmail = ccEmail;
                    entity.Subject = messageSubject;
                    entity.Message = message;
                    entity.FromEmail = !string.IsNullOrEmpty(fromEmail) ? fromEmail : emailSetting.SmtpEmail;
                    entity.UserId = userId;

                    ////insert record in email table.
                    unitOfWork.EmailRepository.Insert<SentEmail>(entity);
                    unitOfWork.EmailRepository.Commit();
                    return entity;
                }
                return null;
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// update email data
        /// </summary>
        private void UpdateCustomerPasswordData(string email,string password)
        {
            try
            {
                ////map entity
                Customer entity = customerService.GetCustomerByEmail(email);
                entity.Password = password;
                ////insert record in email table.
                unitOfWork.CustomerRepository.Update<Customer>(entity);
                unitOfWork.CustomerRepository.Commit();
            }
            catch (Exception ex)
            {
                string log = ex.Message;
            }
        }

        /// <summary>
        /// Get Eamil Settings
        /// </summary>
        /// <returns></returns>
        private EmailSetting GetEmailSettings()
        {
            try
            {
                ////get emailsetting.
                return unitOfWork.EmailRepository.GetAll<EmailSetting>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  get email template by name.
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        private EmailTemplate GetEmailTemplate(string templateName)
        {
            try
            {
                ////get email template
                return unitOfWork.EmailRepository.FindBy<EmailTemplate>(t => t.Name == templateName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method for prepare message body.
        /// </summary>
        /// <param name="emailBody"></param>
        /// <param name="replacements"></param>
        /// <returns>email html body</returns>
        private string PrepareMessageBody(string emailBody, IDictionary replacements)
        {
            if (emailBody == null) throw new ArgumentNullException("emailBody");
            if (replacements == null) throw new ArgumentNullException("replacements");

            IDictionaryEnumerator itr = replacements.GetEnumerator();
            while (itr.MoveNext())
            {
                string oldValue = itr.Key.ToString();
                object newValue = itr.Value;
                if (newValue == null)
                {
                    throw new ArgumentException("replacements");
                }
                emailBody = emailBody.Replace(oldValue, newValue.ToString());
            }
            return emailBody;
        }
    
    }
}
