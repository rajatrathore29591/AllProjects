using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("BookingSiteGates")]
    public partial class BookingSiteGates
    {
        [Key]
        public Guid BookingSiteGateId { get; set; }

        public Guid? GateId { get; set; }

        public Guid? GateContactPersonId { get; set; }

        public Guid? BookingId { get; set; }

        public Guid? FleetRegistrationId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual Gate Gate { get; set; }

        public virtual FleetsRegistration FleetsRegistration { get; set; }

        public virtual Booking Booking { get; set; }

        public virtual GateContactPerson GateContactPerson { get; set; }

    }
}
