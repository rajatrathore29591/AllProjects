using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("GateContactPerson")]
    public partial class GateContactPerson
    {
        [Key]
        public Guid GateContactPersonId { get; set; }

        public Guid GateId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string ContactPerson { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Email { get; set; }

        [Required]
        [StringLength(12)]
        [Column(TypeName = "NVARCHAR")]
        public string MobileNumber { get; set; }

        public bool IsDefault { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual Gate Gate { get; set; }

        public virtual ICollection<BookingSiteGate> BookingSiteGates { get; set; }

    }
}
