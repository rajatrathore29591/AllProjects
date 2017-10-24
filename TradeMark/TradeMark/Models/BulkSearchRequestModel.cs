using System;

namespace TradeMark.Models
{
    public class BulkSearchRequestModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string ExcelName { get; set; }
    }
}