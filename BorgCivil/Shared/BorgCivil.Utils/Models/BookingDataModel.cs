using System;
using System.Collections.Generic;

namespace BorgCivil.Utils.Models
{

    public partial class BookingDataModel
    {

        public string BookingId { get; set; }

        public string CustomerId { get; set; }

        public string SiteId { get; set; }

        public string WorktypeId { get; set; }

        public string StatusLookupId { get; set; }

        public string CallingDateTime { get; set; }

        public DateTime FleetBookingDateTime { get; set; }

        public DateTime EndDate { get; set; }

        public string AllocationNotes { get; set; }

        public string Reason { get; set; }

        public string BookingNumber { get; set; }

        public string CustomerName { get; set; }

        public string SiteName { get; set; }

        public string SiteDetail { get; set; }

        public string WorkType { get; set; }

        public bool IsfleetCustomerSite { get; set; }

        public string StatusTitle { get; set; }

        public string FleetCount { get; set; }

        public string CustomerABN { get; set; }

        public string EmailForInvoices { get; set; }

        public string ContactNumber { get; set; }

        public bool IsAllocated { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public string EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime EditedDate { get; set; }

        public List<BookingFleetDataModel> Fleet { get; set; }

        public BookingFleetDataModel BookingFleet { get; set; }

    }
}
