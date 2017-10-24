using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("Companies")]
    public partial class Companies
    {
        [Key]
        public Guid CompanyId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Name { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string OfficeStreet { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string OfficeSuburb { get; set; }

        [StringLength(12)]
        [Column(TypeName = "NVARCHAR")]
        public string MobileNumber1 { get; set; }

        [StringLength(10)]
        [Column(TypeName = "NVARCHAR")]
        public string OfficePostalCode { get; set; }

        [StringLength(12)]
        [Column(TypeName = "NVARCHAR")]
        public string Fax { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Email { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }

        public virtual ICollection<StatusLookup> StatusLookups { get; set; }

        public virtual ICollection<Attachments> Attachments { get; set; }

        public virtual ICollection<FleetTypes> FleetTypes { get; set; }

        public virtual ICollection<LicenseClass> LicenseClass { get; set; }

        public virtual ICollection<EmploymentCategory> EmploymentCategorys { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }



    }
}
