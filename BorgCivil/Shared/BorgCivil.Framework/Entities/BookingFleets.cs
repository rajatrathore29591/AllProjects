using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("BookingFleets")]
    public partial class BookingFleets
    {
        [Key]
        public Guid BookingFleetId { get; set; }

        public Guid BookingId { get; set; }
       
        public Guid FleetTypeId { get; set; }
       
        public Guid? FleetRegistrationId { get; set; }
       
        public Guid? DriverId { get; set; }

        public Guid? StatusLookupId { get; set; }

        [Required]
        public bool IsDayShift { get; set; }

        [Required]
        public bool Iswethire { get; set; }

        [Column(TypeName = "NVARCHAR")]
        public string AttachmentIds { get; set; }

        [Column(TypeName = "NVARCHAR")]
        public string NotesForDrive { get; set; }

        [Required]
        public bool IsfleetCustomerSite { get; set; }

        public string Reason { get; set; }

        public DateTime FleetBookingDateTime { get; set; }

        public DateTime FleetBookingEndDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual Booking Booking { get; set; }

        public virtual FleetTypes FleetType { get; set; }

        public virtual FleetsRegistration FleetsRegistration { get; set; }

        public virtual Drivers Driver { get; set; }

        public virtual StatusLookup StatusLookup { get; set; }

        public virtual ICollection<FleetHistory> FleetHistorys { get; set; }

        public virtual ICollection<Docket> Dockets { get; set; }

    }
}
