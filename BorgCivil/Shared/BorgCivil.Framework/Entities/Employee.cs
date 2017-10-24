using BorgCivil.Framework.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("Employee")]
    public partial class Employee
    {
        [Key]
        public Guid EmployeeId { get; set; }
        
        public string UserId { get; set; }

        public Guid? DocumentId { get; set; }

        public string RoleId { get; set; }

        public Guid? EmploymentCategoryId { get; set; }

        public Guid? EmploymentStatusId { get; set; }

        public Guid? CountryId { get; set; }

        public Guid? StateId { get; set; }

        [StringLength(30)]
        [Column(TypeName = "NVARCHAR")]
        public string FirstName { get; set; }

        [StringLength(30)]
        [Column(TypeName = "NVARCHAR")]
        public string SurName { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Email { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        [StringLength(12)]
        [Column(TypeName = "NVARCHAR")]
        public string ContactNumber { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public Guid? CompanyId { get; set; }

        public virtual Company Companies { get; set; }

        public virtual EmploymentCategory EmploymentCategory { get; set; }

        public virtual StatusLookup StatusLookup { get; set; }

        public virtual Country Country { get; set; }

        public virtual Document Document { get; set; }

        public virtual State State { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ApplicationRole Role { get; set; }

    }
}
