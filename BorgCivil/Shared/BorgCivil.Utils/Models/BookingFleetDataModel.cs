using System;

namespace BorgCivil.Utils.Models
{

    public partial class BookingFleetDataModel
    {

        public string BookingFleetId { get; set; }

        public string BookingId { get; set; }

        public string FleetTypeId { get; set; }

        public string FleetRegistrationId { get; set; }

        public string DriverId { get; set; }

        public string DriverName { get; set; }

        public string StatusLookupId { get; set; }

        public bool IsDayShift { get; set; }

        public bool Iswethire { get; set; }

        public string AttachmentIds { get; set; }

        public string NotesForDrive { get; set; }

        public bool IsfleetCustomerSite { get; set; }

        public string Reason { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public string EditedBy { get; set; }

        public string FleetBookingDateTime { get; set; }

        public string FleetNumber { get; set; }

        public string FleetDescription { get; set; }

        public string SiteDetail { get; set; }
        
        public DateTime FleetBookingEndDate { get; set; }

        public DateTime CallingDateTime { get; set; }

        public string DocketDetail { get; set; }
        
        public DateTime CreatedDate { get; set; }

        public DateTime EditedDate { get; set; }

    }
}
