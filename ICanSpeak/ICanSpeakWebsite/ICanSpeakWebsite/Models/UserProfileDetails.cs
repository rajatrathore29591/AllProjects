using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICanSpeakWebsite.Models
{
    public class UserProfileDetails
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Country { get; set; }
        public string NotificationCount { get; set; }
        public string CourseStartDate { get; set; }
        public string CourseType { get; set; }
        public string CourseDuration { get; set; }
        public string CourseDaysLeft { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string AboutMeUrl { get; set; }
        public string ProfilePicture { get; set; }
        public string Messages { get; set; }
    }

    public class EditProfile
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string AboutMe { get; set; }
        public string ProfilePicture { get; set; }
        public string DOB { get; set; }
        public string ZipCode { get; set; }
        public string profilepic { get; set; }
        public HttpPostedFileBase Audio { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }
   


}