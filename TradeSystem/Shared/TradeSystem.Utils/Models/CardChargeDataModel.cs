using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Utils.Models
{
    public class CardChargeDataModel
    {
        public string status { get; set; }
        public string chargeid { get; set; }
        public string barCodeUrl { get; set; }
        public string barCodePaybinUrl { get; set; }
        public string productId { get; set; }
        public float walletAmount { get; set; }
    }
}
