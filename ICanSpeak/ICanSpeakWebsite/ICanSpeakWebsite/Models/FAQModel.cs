using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICanSpeakWebsite.Models
{
    public class FAQModel
    {
        public string FaqId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreateDate { get; set; }
        public string ModifyDate { get; set; }
        public string DeleteDate { get; set; }
    }
}