using System;

namespace BorgCivil.Utils.Models
{

    public partial class FleetTypeDataModel
    {

        public Guid FleetTypeId { get; set; }

        public Guid CompanyId { get; set; }

        public Guid DocumentId { get; set; }

        public string Fleet { get; set; }

        public string Description { get; set; }

        public string ImageBase64 { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

    }
}
