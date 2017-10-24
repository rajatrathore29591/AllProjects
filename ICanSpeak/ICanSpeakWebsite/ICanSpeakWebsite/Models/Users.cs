using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICanSpeakWebsite.Models
{
    public class Users
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string ProfilePicture { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.DateTime DOB { get; set; }
        public System.Nullable<int> Age { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string AboutMe { get; set; }
        public System.Nullable<System.DateTime> CreatedDate { get; set; }
        public System.Nullable<System.DateTime> ModifiedDate { get; set; }
        public System.Nullable<bool> IsActive { get; set; }
        public System.Nullable<System.DateTime> LastLogin { get; set; }
        public System.Nullable<int> RoleId { get; set; }
        public string Password { get; set; }
        public string AccessToken { get; set; }
        public string Experience { get; set; }
        public string Specialisation { get; set; }
        public string Education { get; set; }
    }
}