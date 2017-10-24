using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICanSpeakWebsite.Models
{
        public class DialogDetail
        {
            public string DialogId { get; set; }
            public string VideoUrl { get; set; }
            public string EnglishSubtitleUrl { get; set; }
            public string ArabicSubtitleUrl { get; set; }
            public string BothSubtitleUrl { get; set; }
            public string DialogName { get; set; }
        }
}