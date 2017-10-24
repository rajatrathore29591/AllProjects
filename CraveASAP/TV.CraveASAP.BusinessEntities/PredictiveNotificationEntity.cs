using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.CraveASAP.BusinessEntities
{
    public partial class PredictiveNotificationEntity
    {
        public int preId { get; set; }
        public int userId { get; set; }
        public int promotionCodeId { get; set; }
        public bool status { get; set; }
        public string sendTime { get; set; }
        public string reSendTime { get; set; }
        public string success  { get; set; }
        public string Failure { get; set; }
    }
}
