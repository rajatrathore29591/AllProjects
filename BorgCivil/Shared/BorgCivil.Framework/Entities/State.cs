using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("State")]
    public partial class State
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? CountryId { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Name { get; set; }

        public string Description { get; set; }
        
        public bool? IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime? ModifiedDate { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<Driver> Drivers { get; set; }
    }
}
