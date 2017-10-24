using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("Fleets")]
    public partial class Fleets
    {
        [Key]
        public Guid FleetId { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string Fleet { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

    }
}
