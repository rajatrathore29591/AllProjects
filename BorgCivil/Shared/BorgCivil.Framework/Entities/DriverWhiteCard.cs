using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("DriverWhiteCard")]
    public partial class DriverWhiteCard
    {
        [Key]
        public Guid DriverWhiteCardId { get; set; }

        public Guid? DriverId { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string CardNumber { get; set; }

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
