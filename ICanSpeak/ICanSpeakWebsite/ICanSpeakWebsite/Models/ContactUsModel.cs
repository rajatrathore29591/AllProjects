using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICanSpeakWebsite.Models
{
    public class ContactUsModel
    {
        public int QueryId { get; set; }
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string ContactNo { get; set; }
        public string Message { get; set; }
       
    }
}