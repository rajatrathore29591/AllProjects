using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSystem.Utils.Models
{
    public partial class CompanyUserDataModel
    {
        public string Id { get; set; }

        public string DocumentId { get; set; }
        public string RoleId { get; set; }
        public string Role { get; set; }
        public DateTime CreatedDate { get; set;}
        public string UserId { get; set; }

        public string CompanyName { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string Phone { get; set; }

        public bool IsActive { get; set; }


        public string UserName { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "ConfirmPassword")]
       
        public string ConfirmPassword { get; set; }
        [Display(Name = "ConfirmEmail")]
        public string ConfirmEmail { get; set; }

        public string Address { get; set; }

        public bool IsSuperAdmin { get; set; }
        public string Status { get; set; }

        public bool Active { get; set; }

        public bool DeActive { get; set; }

    }
}
