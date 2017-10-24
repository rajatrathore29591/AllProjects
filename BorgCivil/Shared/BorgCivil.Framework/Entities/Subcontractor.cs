using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("Subcontractor")]
    public partial class Subcontractor
    {
        [Key]
        public Guid SubcontractorId { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR")]
        public string Name { get; set; }

        public bool? IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<FleetsRegistration> FleetsRegistrations { get; set; }

    }
}
