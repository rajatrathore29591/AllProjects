using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public partial class GetResponseEntity
    {
        public IEnumerable<object> data { get; set; }
        public object data1 { get; set; }
        public string statusMessage = string.Empty;
        public string statusCode = string.Empty;
        public string oAuthkey = string.Empty;
        public string Image { get; set; }
        public string serverTime { get; set; }
        public string exist { get; set; }
        public bool promoUsed { get; set; }
               
    }
}