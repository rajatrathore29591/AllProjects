using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public partial class SubscribeEntity
    {
        public int subscribeId { get;set;}
        public string email { get; set; }
        public bool isSubscribe { get; set; }
    }
}
