using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorgCivil.Framework.Entities
{
    [Table("Booking")]
    public partial class Booking
    {
        [Key]
        public Guid BookingId { get; set; }

        public Guid CustomerId { get; set; }
      
        public Guid? SiteId { get; set; }
      
        public Guid WorktypeId { get; set; }

        public Guid StatusLookupId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingNumber { get; set; }

        public DateTime CallingDateTime { get; set; }

        public DateTime FleetBookingDateTime { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsDeleted { get; set; }

        public string AllocationNotes { get; set; }

        public string CancelNote { get; set; }

        public string SiteNote { get; set; }

        public decimal Rate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Site Site { get; set; }

        public virtual WorkType WorkType { get; set; }

        public virtual StatusLookup StatusLookup { get; set; }

        public virtual ICollection<BookingSiteSupervisor> BookingSiteSupervisors { get; set; }

        public virtual ICollection<BookingSiteGate> BookingSiteGates { get; set; }

        public virtual ICollection<BookingFleet> BookingFleets { get; set; }

        public virtual ICollection<FleetHistory> FleetHistorys { get; set; }

       

    }
}
