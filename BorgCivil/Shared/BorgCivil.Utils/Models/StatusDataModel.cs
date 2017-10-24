using System;

namespace BorgCivil.Utils.Models
{
    public class StatusDataModel
    {
        public Guid BookingId { get; set; }

        public Guid BookingFleetId { get; set; }

        public int StatusValue { get; set; }

        public string CancelNote { get; set; }

        public string Rate { get; set; }
    }
}