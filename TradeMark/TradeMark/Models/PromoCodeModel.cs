using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeMark.Models
{
    public class PromoCodeModel
    {
        public long PromoCodeId { get; set; }
        public decimal Amount { get; set; }
        public int Limit { get; set; }

        public string Title { get; set; }
        public string PromoCode { get; set; }
        public decimal Price { get; set; }
        public int Redeemlimit { get; set; }
        
    }
}