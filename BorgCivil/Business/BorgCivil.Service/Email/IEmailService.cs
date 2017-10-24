using BorgCivil.Framework.Entities;
using System;
using System.Collections.Specialized;

namespace BorgCivil.Service
{
    public interface IEmailService : IService
    {
        void SendEmailAsync(Guid id, string toEmail, string ccEmail, string bccEmail, string fromEmail, string templateName, string password, ListDictionary replacements);

        void SendEmailAttachmentAsync(Guid id, string toEmail, string ccEmail, string bccEmail, string fromEmail, string templateName, string password, ListDictionary replacements, string fileName);

        SentEmail AddEmailData(string userId, string toEmail, string ccEmail, string bccEmail, string fromEmail, string templateName, ListDictionary replacements);

    }
}
