using System;

namespace BorgCivil.Utils.Models
{
    public class GateDataModel
    {
        public Guid GateId { get; set; }

        public Guid SiteId { get; set; }
   
        public string GateNumber { get; set; }

        public decimal TipOffRate { get; set; }

        public string TippingSite { get; set; }
        
        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }
     
    }
}