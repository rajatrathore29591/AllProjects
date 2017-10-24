using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICanSpeakWebsite.Models
{
    public class MailModel
    {
        public int MailID { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }
        public string  EmailID { get; set; }

    
    }

    public class MailBoxModel
    {
        public IEnumerable<MailModel> GetInboxModel { get; set; }
        public IEnumerable<MailModel> GetOutBoxModel { get; set; }
        public MailModel GetComposedModel { get; set; }
    }
}