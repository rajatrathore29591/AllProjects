using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Utils.Models
{
    public partial class PaymentDataModel
    {
        public string customerId { get; set; }
        public string productId { get; set; }
        public string id { get; set; }
        public string investment { get; set; }
        public string device_session_id { get; set; }
        public string paymentType { get; set; }
        public float walletAmount { get; set; }
        public string lang { get; set; }
        public bool lastRemainingInvestmentAmountStatus { get; set; }
        public card card { get; set; }
         
    }
    public class card
    {
        public string card_number { get; set; }
        public string expiration_month { get; set; }
        public string expiration_year { get; set; }
        public string holder_name { get; set; }
        public string cvv_number { get; set; }

    }

}
