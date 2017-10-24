using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("BookingSiteSupervisor")]
    public partial class BookingSiteSupervisor
    {
        [Key]
        public Guid BookingSiteSupervisorId { get; set; }

        public Guid SupervisorId { get; set; }

        public Guid BookingId { get; set; }
              
        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual Supervisor Supervisor { get; set; }

        public virtual Booking Booking { get; set; }

    }
}
