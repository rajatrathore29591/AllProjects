using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeMark.Models
{
    public class UserTransactionModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public int Credits { get; set; }
        public DateTime CreatDate { get; set; }

        public DateTime EditedDate { get; set; }

        public long PromocodeId { get; set; }
        public string PromoCode { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public int Limit { get; set; }
        public string StripeCustomerId { get; set; }
    }
}