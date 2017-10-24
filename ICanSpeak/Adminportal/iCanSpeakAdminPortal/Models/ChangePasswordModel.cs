using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iCanSpeakAdminPortal.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Email Id field is required")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Current Password field is required")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New Password field is required")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm New Password field is required")]
        [Compare("NewPassword", ErrorMessage = "Confirm Password and Confirm New Password do not match")]
        public string ConfirmPassword { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }   
    }
}