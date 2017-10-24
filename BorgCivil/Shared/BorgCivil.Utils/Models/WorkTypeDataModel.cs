using System;

namespace BorgCivil.Utils.Models
{

    public partial class WorkTypeDataModel
    {

        public Guid WorkTypeId { get; set; }

        public string Type { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

    }
}
