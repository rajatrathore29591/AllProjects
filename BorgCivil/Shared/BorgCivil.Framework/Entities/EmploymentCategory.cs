using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("EmploymentCategory")]
    public partial class EmploymentCategory
    {
        [Key]
        public Guid EmploymentCategoryId { get; set; }

        public Guid? CompanyId { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "NVARCHAR")]
        public string Category { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual Company Companies { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<Driver> Drivers { get; set; }

    }
}
