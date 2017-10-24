using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCanSpeakAdminPortal.Models
{
    public class DemoVideoModel
    {
        public int VideoId { get; set; }
        [Required]
        public string VideoName { get; set; }
        [Required]
        public HttpPostedFileBase Video { get; set; }
        public string VideoUrl { get; set; }        
    }
}