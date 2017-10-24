using System;
using System.Collections.Generic;

namespace BorgCivil.Utils.Models
{

    public partial class SiteDataModel
    {

        public string SiteId { get; set; }

        public string CustomerId { get; set; }

        public string SiteName { get; set; }

        public string PoNumber { get; set; }

        public string CreditTermAgreed { get; set; }

        public string SiteDetail { get; set; }

        public decimal DayShiftRate { get; set; }

        public decimal NightShiftRate { get; set; }

        public decimal FloatCharge { get; set; }

        public decimal PilotCharge { get; set; }

        public decimal TipOffRate { get; set; }

        public bool FuelIncluded { get; set; }

        public bool TollTax { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime EditedDate { get; set; }

        public List<GateDataModel> GateList { get; set; }

        public List<SupervisorDataModel> SupervisorList { get; set; }

    }
}
