using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("WorkType")]
    public partial class WorkType
    {
        [Key]
        public Guid WorkTypeId { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string Type { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

    }
}
