using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeMark.Models
{
    public class PaymentModel
    {
        public string ssl_merchant_id { get; set; } //xxxxxx
        public string ssl_user_id { get; set; } //xxxxxx
        public string ssl_pin { get; set; } //xxxxxx
        public bool ssl_show_form { get; set; } //false
        public string ssl_result_format { get; set; } //ascii
        public int ssl_card_number { get; set; }  //0000000000000000
        public DateTime ssl_exp_date { get; set; }  //1208
        public float ssl_amount { get; set; }  //1.02
        public string ssl_transaction_type { get; set; }  //ccsale
        public int ssl_cvv2cvc2_indicator { get; set; }  //1
        public int ssl_cvv2cvc2 { get; set; } //123

    }
}