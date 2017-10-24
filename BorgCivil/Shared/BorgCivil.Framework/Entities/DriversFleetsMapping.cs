using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("DriversFleetsMapping")]
    public partial class DriversFleetsMapping
    {
        [Key]
        public Guid DriverFleetMapping { get; set; }

        public Guid DriverId { get; set; }

        public Guid FleetRegistrationId { get; set; }

        [Required]
        public bool IsActive { get; set; }
        
        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }
        
        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual Driver Driver { get; set; }

        public virtual FleetsRegistration FleetRegistration { get; set; }

    }
}
