using System;
using System.Collections.Generic;

namespace BorgCivil.Utils.Models
{

    public partial class BookingSiteDetailDataModel
    {
        public string BookingSiteGateId { get; set; }

        public string BookingSiteSupervisorId { get; set; }

        public string GateId { get; set; }

        public string GateContactPersonId { get; set; }

        public string BookingId { get; set; }

        public string SupervisorId { get; set; }

        public string FleetRegistrationId { get; set; }

        public string Note { get; set; }

        public string SiteNote { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public string EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime EditedDate { get; set; }

        public List<BookingSiteDetailDataModel> BookingSiteList { get; set; }

    }
}
