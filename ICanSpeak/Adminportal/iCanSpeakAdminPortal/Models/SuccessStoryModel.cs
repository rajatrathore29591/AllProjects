using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iCanSpeakAdminPortal.Models
{
    public class SuccessStoryModel
    {
        public int StoryId { get; set; }
        public string ClientName { get; set; }
        public string ClientStory { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public string ClientImageUrl { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string DeleteDate { get; set; }
    }
}