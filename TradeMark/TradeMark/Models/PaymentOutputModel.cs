using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeMark.Models
{
    public class PaymentOutputModel
    {
        public string PaymentStatus { get; set; }
        public int Credits { get; set; }
        public string TransactionId { get; set; }

        public string UserEmailid { get; set; }
    }
}