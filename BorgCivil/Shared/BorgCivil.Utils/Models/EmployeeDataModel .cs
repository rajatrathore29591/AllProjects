using System;
using System.ComponentModel.DataAnnotations;

namespace BorgCivil.Utils.Models
{

    public partial class EmployeeDataModel
    {

        public Guid EmployeeId { get; set; }

        public string UserId { get; set; }

        public string EmploymentCategoryId { get; set; }

        public string EmploymentStatusId { get; set; }

        public string RoleId { get; set; }

        public string FirstName { get; set; }

        public string SurName { get; set; }

        public string Email { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public Guid CountryId { get; set; }

        public Guid StateId { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string DeviceToken { get; set; }

        public string DeviceType { get; set; }

        public string ContactNumber { get; set; }

        public string ImageBase64 { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public Guid? CompanyId { get; set; }

    }

    public class EmployeePasswordDataModel
    {
        [Required]
        public string EmployeeId { get; set; }

        //[DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        //[DataType(DataType.Password)]
        [Display(Name = "password")]
        public string Password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        public string Lang { get; set; }

    }
}
