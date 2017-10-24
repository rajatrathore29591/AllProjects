using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICanSpeakWebsite.Models
{
    public class TutorDetails
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string Gender { get; set; }
        public string CreatedDate { get; set; }
        public string DOB { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string ProfilePicture { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string Expertise { get; set; }
        public string Description { get; set; }


        public bool IsActive { get; set; }
        public int RoleId { get; set; }
    }
}