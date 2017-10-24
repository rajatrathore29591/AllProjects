using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iCanSpeakAdminPortal.Models
{
    public class UserQueriesModel
    {
        public int QueryId { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string ContactNo { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsRead { get; set; }
        
    }
}