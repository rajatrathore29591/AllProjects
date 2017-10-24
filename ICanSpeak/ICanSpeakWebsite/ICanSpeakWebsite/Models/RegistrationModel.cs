using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ICanSpeakWebsite.Models
{
    public class RegistrationModel
    {
        
        public string Email { get; set; }
        public string ConfirmEmail { get; set; }
        public string Username { get; set; }
        public string DOB { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Country { get; set; }
        public bool Gender { get; set; }
        public string Zipcode { get; set; }
        public string profilepic { get; set; }
        public string audiobase64 { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public HttpPostedFileBase Audio { get; set; }
    }
}