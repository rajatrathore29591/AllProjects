using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICanSpeakWebsite.Models
{
    public class FlashModel
    {
        public int FlashCardId { get; set; }
        public int UserId { get; set; }
        public int WordId { get; set; }
        public string CreativeDate { get; set; }
    }
}
