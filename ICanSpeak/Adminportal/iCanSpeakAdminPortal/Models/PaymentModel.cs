using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iCanSpeakAdminPortal.Models
{
    public class PaymentModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public int CoursePrice { get; set; }
        public string Discription { get; set; }
        public int PaidAmount { get; set; }
        public string PaidBy { get; set; }
        public string PaidDate { get; set; }
        public int PaymentDetailId { get; set; }
        public string TranctionId { get; set; }
    }
}