using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Utils.Models
{
    public partial class PenaltyDataModel
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string PenaltyPercent { get; set; }
        public string CreatedDate { get; set; }
        public string PenaltyFrom { get; set; }
        public string PenaltyTo { get; set; }

    }
}
