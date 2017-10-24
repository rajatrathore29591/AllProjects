using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iCanSpeakAdminPortal.Models
{
    public class TutorModel
    {
        public string TutorID { get; set; }
        public string SubTutorID { get; set; }
        public string studentid { get; set; }
        public IEnumerable<string> SelectItems { set; get; }
    }
}