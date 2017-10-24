using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessServices
{
    public class PushNotificationsResponse
    {
        public string alert = string.Empty;
        public string message { get; set; }
        public string messageThai { get; set; }
        public string badge = string.Empty;
        public string sound = string.Empty;
        public string type = string.Empty;
        public string vendorId { get; set; }
        public string subCategory { get; set; }
        public string optCategory { get; set; }
       
    }
}
