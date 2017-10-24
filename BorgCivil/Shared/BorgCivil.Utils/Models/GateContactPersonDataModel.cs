using System;

namespace BorgCivil.Utils.Models
{
    public class GateContactPersonDataModel
    {
        public Guid GateContactPersonId { get; set; }

        public Guid GateId { get; set; }
   
        public string ContactPerson { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }
     
    }
}