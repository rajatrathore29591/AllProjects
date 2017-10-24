using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICanSpeakWebsite.Models
{
    public class Login
    {
       
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginError
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Errormessage { get; set; }
    }

    public class ForgotPassword
    {
        public string email { get; set; }
    }
}