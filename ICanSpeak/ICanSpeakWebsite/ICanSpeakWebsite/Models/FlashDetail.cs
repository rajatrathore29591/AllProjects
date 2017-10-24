using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICanSpeakWebsite.Models
{
    public class FlashDetail
    {
        public int FlashCardId { get; set; }
        public int UserId { get; set; }
        public int WordId { get; set; }
        public int CreatedDate { get; set; }

    }
}