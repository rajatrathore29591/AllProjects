using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICanSpeakWebsite.Models
{
    public class SuccessStoryModel
    {

        public int StoryId { get; set; }
        public string ClientName { get; set; }
        public string ClientStory { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public string ClientImageUrl { get; set; }

    }
}