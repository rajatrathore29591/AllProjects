using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("LicenseClass")]
    public partial class LicenseClass
    {
        [Key]
        public Guid LicenseClassId { get; set; }

        public Guid? CompanyId { get; set; }

        [Required]
        [StringLength(30)]
        [Column(TypeName = "NVARCHAR")]
        public string Class { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual Gate Gate { get; set; }

        public virtual Company Companies { get; set; }

        public virtual ICollection<Driver> Drivers { get; set; }

    }
}
