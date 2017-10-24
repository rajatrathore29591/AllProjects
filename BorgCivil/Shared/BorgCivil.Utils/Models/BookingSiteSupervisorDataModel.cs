using System;

namespace BorgCivil.Utils.Models
{

    public partial class BookingSiteSupervisorDataModel
    {
        public string BookingSiteSupervisorId { get; set; }

        public string SupervisorId { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public string EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime EditedDate { get; set; }

    }
}
