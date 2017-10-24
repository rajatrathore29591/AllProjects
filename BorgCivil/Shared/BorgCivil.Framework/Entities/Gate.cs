using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("Gate")]
    public partial class Gate
    {
        [Key]
        public Guid GateId { get; set; }

        public Guid SiteId { get; set; }

        [StringLength(20)]
        [Column(TypeName = "NVARCHAR")]
        public string GateNumber { get; set; }

        public decimal TipOffRate { get; set; }

        [StringLength(300)]
        [Column(TypeName = "NVARCHAR")]
        public string TippingSite { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual ICollection<BookingSiteGate> BookingSiteGates { get; set; }

        public virtual Site Site { get; set; }

        public virtual ICollection<GateContactPerson> GateContactPersons { get; set; }

    }
}
