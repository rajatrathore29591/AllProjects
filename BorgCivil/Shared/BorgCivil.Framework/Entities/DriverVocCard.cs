using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("DriverVocCard")]
    public partial class DriverVocCard
    {
        [Key]
        public Guid DriverVocCardId { get; set; }

        public Guid? DriverId { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string CardNumber { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string RTONumber { get; set; }

        public DateTime IssueDate { get; set; }

        public string Notes { get; set; }

        public bool? IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual Driver Drivers { get; set; }

    }
}
