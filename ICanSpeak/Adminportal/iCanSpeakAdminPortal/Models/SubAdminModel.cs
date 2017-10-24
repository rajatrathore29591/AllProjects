using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCanSpeakAdminPortal.Models
{
    public class SubAdminModel
    {
        public int UserId { get; set; }
        [Required]
        public string Tutortype { get; set; }

        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string ContactNo { get; set; }
        [Required]
        public string Gender { get; set; }

        public string CreatedDate { get; set; }
        [Required]
        public string DOB { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserType { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string Education { get; set; }
        [Required]
        public string Experience { get; set; }
        [Required]
        public string Expertise { get; set; }
        public string Description { get; set; }
        [Required]
        public HttpPostedFileBase Image { get; set; }
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
        

    }
}