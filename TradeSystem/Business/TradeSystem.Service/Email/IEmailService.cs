using TradeSystem.Framework.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Service
{
    public interface IEmailService : IService
    {
        void SendEmailAsync(Guid id, string toEmail, string ccEmail, string bccEmail, string fromEmail, string templateName, string password, ListDictionary replacements);

        SentEmail AddEmailData(string userId, string toEmail, string ccEmail, string bccEmail, string fromEmail, string templateName, ListDictionary replacements);

    }
}
