using System;

namespace TradeMark.Models
{
    public class UserModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string ContactNo { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string SocialMediaUserId { get; set; }
        public string MediaProvider { get; set; }
        public string ProfilePic { get; set; }
        public bool IsAdmin { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public string StreetAddress { get; set; }
        public string IsActive { get; set; }
        public DateTime RegisteredDate { get; set; }
        public string StripeCustomerId { get; set; }
    }
}