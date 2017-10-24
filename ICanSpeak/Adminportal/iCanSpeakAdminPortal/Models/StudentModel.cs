using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace iCanSpeakAdminPortal.Models
{
    public class StudentModel
    {
            public int UserId { get; set; }
            [Required]
            public string Email { get; set; }
            [Required]
            public string Country { get; set; }
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required]
            public string Gender { get; set; }
            [Required]
            public string DOB { get; set; }
            [Required]
            public string Phone { get; set; }
            public string Status { get; set; }
            public HttpPostedFileBase Image { get; set; }
            public bool IsActive { get; set; }
            public string Experience { get; set; }
            [Required]
            public string Specialisation { get; set; }
            public string Description { get; set; }
            public string CreatedDate { get; set; }
    }
}
