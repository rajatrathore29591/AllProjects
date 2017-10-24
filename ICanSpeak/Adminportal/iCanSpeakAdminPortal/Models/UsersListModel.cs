using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iCanSpeakAdminPortal.Models
{
    public class UsersListModel
    {
        public int UserId { get; set; }
        public int IsSuggested { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string CreatedDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public string ProfilePicture { get; set; }
        public string DOB { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string TutFirstName { get; set; }
        public string TutLastName { get; set; }
        public string AboutMe { get; set; }
        public string CourseType { get; set; }
        public string CourseDaysLeft { get; set; }
        public string CourseDuration { get; set; }
        public string CourseStartDate { get; set; }
        public string EndDate { get; set; }
        public bool IsActive { get; set; }
        
    }
}