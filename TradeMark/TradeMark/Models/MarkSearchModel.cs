using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeMark.Models
{
    public class MarkSearchModel
    {
        public int SearchId { get; set; }
        public string SearchText { get; set; }
        public DateTime SearchDate { get; set; }
        public int FilterOption { get; set; }
        public string UserId { get; set; }
        public string ComponentsFullForm { get; set; }
        public string UsClassDescriptionId { get; set; }
        public string Title { get; set; }
        public string SearchGuid { get; set; }
    }
}