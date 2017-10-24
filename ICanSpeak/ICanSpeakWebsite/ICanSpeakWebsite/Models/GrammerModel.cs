using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICanSpeakWebsite.Models
{
    public class GrammerModel
    {

        public class GrammarDetail
        {
            public string Unitid { get; set; }
            public string Username { get; set; }
            public string Country { get; set; }
            public string ProfilePicture { get; set; }
            public string UnitNameEnglish { get; set; }
            public string UnitNameArabic { get; set; }
            public string DescriptionEnglish { get; set; }
            public string DescriptionArabic { get; set; }
            public string VideoUrl { get; set; }
            public string index { get; set; }
            public string BookMarkStatus { get; set; }

        }
    }
}