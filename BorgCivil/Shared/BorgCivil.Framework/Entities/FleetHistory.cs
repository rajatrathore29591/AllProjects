using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("FleetHistory")]
    public partial class FleetHistory
    {
        [Key]
        public Guid FleetHistoryId { get; set; }

        public Guid? BookingFleetId { get; set; }

        public Guid? BookingId { get; set; }

        public Guid? FleetTypeId { get; set; }

        public Guid? FleetRegistrationId { get; set; }

        public Guid? DriverId { get; set; }

        public Guid? FleetStatus { get; set; }

        public bool IsDayShift { get; set; }
       
        public bool Iswethire { get; set; }
       
        public string AttachmentIds { get; set; }

        public string NotesForDrive { get; set; }
       
        public string Reason { get; set; }

        public bool IsfleetCustomerSite { get; set; }
       
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual BookingFleet BookingFleet { get; set; }

        public virtual Booking Booking { get; set; }

        public virtual FleetType FleetType { get; set; }

        public virtual FleetsRegistration FleetsRegistration { get; set; }

        public virtual Driver Driver { get; set; }

        public virtual StatusLookup StatusLookup { get; set; }

    }
}
